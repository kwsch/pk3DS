using System;
using System.IO;
using System.Linq;

namespace pk3DS
{
    // Big thanks to FireFly for figuring out the 7/6-bit compression routine for scripts.
    class Scripts
    {
        // Decompression
        internal static byte[] decompressScript(byte[] data)
        {
            data = data ?? new byte[0]; // Bad Input
                
            using (MemoryStream mn = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(mn))
            {
                // Read away!
                int pos = 0;
                while (pos < data.Length)
                {
                    // Read until the top bit is not set
                    byte[] cb = readCompressed(data, pos);
                    pos += cb.Length;
                    // Interpret the bytecode
                    byte[] db = decompressBytes(cb);
                    // Write Bytes
                    bw.Write(db);
                }
                return mn.ToArray();
            }
        }
        internal static byte[] readCompressed(byte[] data, int pos)
        {
            byte[] c1 = data.Skip(pos).TakeWhile(b => b >> 7 > 0).ToArray(); // Take while >= 0x80
            return c1.Concat(data.Skip(pos + c1.Count()).Take(1)).ToArray(); // Take another
        }
        internal static byte[] decompressBytes(byte[] cb)
        {
            byte[] db = new byte[0];

            if ((cb[0] & 0x40) > 0) // Signed Parameter
            {
                // Check the next bytecode
                if (cb.Length > 1 && (cb[1] >> 7) > 0) // Many-bits-required command
                {
                    // 2 Byte Signed Parameter
                    int cmd = (cb[0] & 0x3 << 14) | (cb[1] & 0x7F << 7) | cb[2]; // 16 Bits total
                    db = db.Concat(BitConverter.GetBytes(cmd).Take(2)).ToArray(); // 16 Bits

                    int dev = ((cb[0] & 0x3F) - 0x40) >> 2; // Lowest 2 bits have already been used for the command
                    db = db.Concat(BitConverter.GetBytes(dev).Take(2)).ToArray(); // 16 Bits
                }
                else if ((cb[0] >> 7) > 0) // Signed Command
                {
                    // 3 Byte Signed Parameter
                    int cmd = ((cb[0] << 7) | cb[1]);
                    db = db.Concat(BitConverter.GetBytes(cmd).Take(1)).ToArray(); // 8 Bits Total

                    int dev = ((cb[0] & 0x3F) - 0x40) >> 1; // Lowest bit has already been used for the command
                    db = db.Concat(BitConverter.GetBytes(dev).Take(3)).ToArray(); // 24 Bits
                }
                else // Signed Value
                {
                    // 4 Byte Signed Parameter
                    int dev = ((cb[0] & 0x3F) - 0x40) >> 0; // No bits have already been used; no command
                    db = db.Concat(BitConverter.GetBytes(dev).Take(4)).ToArray(); // 32 Bits
                }
            }
            else if ((cb[0] >> 7) > 0) // Manybit
            {
                Array.Reverse(cb);
                int cmd = 0;
                for (int i = 0; i < cb.Length; i++)
                    cmd |= ((cb[i] & 0x7F) << (7 * i));

                db = db.Concat(BitConverter.GetBytes((uint)cmd).Take(4)).ToArray();
            }
            else // Literal
            {
                db = db.Concat(BitConverter.GetBytes((uint)cb[0]).Take(4)).ToArray();
            }
            return db;
        }

        // Compression
        internal static byte[] compressScript(byte[] data)
        {
            if (data == null || data.Length % 4 != 0) // Bad Input
                return null;
            using (MemoryStream mn = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(mn))
            {
                int pos = 0;
                while (pos < data.Length)
                {
                    byte[] db = data.Skip(pos+=4).Take(4).ToArray();
                    byte[] cb = compressBytes(db);
                    bw.Write(cb);
                }
                return mn.ToArray();
            }
        }
        internal static byte[] compressBytes(byte[] db)
        {
            short cmd = BitConverter.ToInt16(db, 0);
            short val = BitConverter.ToInt16(db, 2);

            byte[] cb = new byte[0];
            bool sign4 = val < 0 && cmd < 0 && db[0] >= 0xC0; // 4 byte signed
            bool sign3 = val < 0 && cmd < 0 && db[0] < 0xC0; // 3 byte signed
            bool sign2 = val < 0 && cmd > 0; // 2 byte signed
            bool liter = cmd >= 0 && cmd < 0x40; // Literal
            bool manyb = cmd >= 0x40; // manybit

            if (sign4)
            {
                int dev = 0x40 + BitConverter.ToInt32(db, 0);
                if (dev < 0) // BADLOGIC
                    return cb;
                cb = new[] {(byte)((dev & 0x3F) | 0x40)};
            }
            else if (sign3)
            {
                byte dev = (byte)(((db[1] << 1) + 0x40) | 0xC0 | db[0] >> 7);
                byte low = db[0];
                cb = new[] {dev, low};
            }
            else if (sign2)
            {
                if (manyb)
                {
                    byte dev = (byte)(((db[2] << 2) + 0x40) | 0xC0 | db[1] >> 7);
                    byte low1 = (byte)(0x80 | (db[0] >> 7) | (db[1] & 0x80));
                    byte low0 = (byte)(db[0] & 0x80);
                    cb = new[] {low0, low1, dev};
                }
                else // Dunno if this ever naturally happens; the command reader may ignore db[1] if db[0] < 0x80... needs verification.
                {
                    byte dev = (byte)(((db[1] << 2) + 0x40) | 0xC0 | db[0] >> 6);
                    byte low0 = (byte)(db[0] & 0x3F);
                    cb = new[] {low0, dev};
                }
            }
            else if (manyb)
            {
                ulong bitStorage = 0;

                uint dv = BitConverter.ToUInt32(db, 0);
                int ctr = 0;
                while (dv != 0) // bits remaining
                {
                    byte bits = (byte)(((byte)dv) & 0x7F); dv >>= 7; // Take off 7 bits at a time
                    bitStorage |= (byte)(bits << (ctr*8)); // Write the 7 bits into storage
                    bitStorage |= (byte)(1 << (7 + (ctr++*8))); // continue reading flag
                }
                byte[] compressedBits = BitConverter.GetBytes(bitStorage);

                Array.Reverse(compressedBits);
                // Trim off leading zero-bytes
                cb = compressedBits.SkipWhile(v => v == 0).ToArray();
            }
            else if (liter)
            {
                cb = new[] { (byte)cmd };
            }
            return cb;
        }

        // General Viewing Utility
        internal static string[] getHexLines(byte[] data, int count = 4)
        {
            data = data ?? new byte[0];
            // Generates an x-byte wide space separated string array; leftovers included at the end.
            string[] s = new string[data.Length/count + ((data.Length % count > 0) ? 1 : 0)];
            for (int i = 0; i < s.Length;i++)
                s[i] = BitConverter.ToString(data.Skip(i*4).Take(count).ToArray()).Replace('-', ' ');
            return s;
        }
    }
}
