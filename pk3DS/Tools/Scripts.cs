using System;
using System.IO;
using System.Linq;

namespace pk3DS
{
    // Big thanks to FireFly for figuring out the 7/6-bit compression routine for scripts.
    class Scripts
    {
        // Decompression - Deprecated: Use FireFly's method.
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
            return c1.Concat(data.Skip(pos + c1.Length).Take(1)).ToArray(); // Take another
        }
        internal static byte[] decompressBytes(byte[] cb)
        {
            byte[] db = new byte[0];

            if ((cb[0] & 0x40) > 0) // Signed Parameter
            {
                // Check the next bytecode
                if (cb.Length > 1 && cb[1] >> 7 > 0) // Many-bits-required command
                {
                    // 2 Byte Signed Parameter
                    int cmd = (cb[0] & 0x3 << 14) | (cb[1] & 0x7F << 7) | cb[2]; // 16 Bits total
                    db = db.Concat(BitConverter.GetBytes(cmd).Take(2)).ToArray(); // 16 Bits

                    int dev = ((cb[0] & 0x3F) - 0x40) >> 2; // Lowest 2 bits have already been used for the command
                    db = db.Concat(BitConverter.GetBytes(dev).Take(2)).ToArray(); // 16 Bits
                }
                else if (cb[0] >> 7 > 0) // Signed Command
                {
                    // 3 Byte Signed Parameter
                    int cmd = (cb[0] << 7) | cb[1];
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
            else if (cb[0] >> 7 > 0) // Manybit
            {
                Array.Reverse(cb);
                int cmd = 0;
                for (int i = 0; i < cb.Length; i++)
                    cmd |= (cb[i] & 0x7F) << (7 * i);

                db = db.Concat(BitConverter.GetBytes((uint)cmd).Take(4)).ToArray();
            }
            else // Literal
            {
                db = db.Concat(BitConverter.GetBytes((uint)cb[0]).Take(4)).ToArray();
            }
            return db;
        }
        // FireFly's (github.com/FireyFly) concise decompression (ported c->c#):
        // https://github.com/FireyFly/poketools/blob/e74538a5b5e5dab1e78c1cd313c55d158f37534d/src/formats/script.c#L61
        internal static uint[] quickDecompress(byte[] data, int count)
        {
            uint[] code = new uint[count];
            uint i = 0, j = 0, x = 0, f = 0;
            while (i < code.Length) {
                int b = data[f++], 
                    v = b & 0x7F;
                if (++j == 1) // sign extension possible
                    x = (uint)((((v >> 6 == 0 ? 1 : 0) - 1) << 6) | v); // only for bit6 being set
                else x = (x << 7) | (byte)v; // shift data into place

                if ((b & 0x80) != 0) continue; // more data to read
                code[i++] = x; j = 0; // write finalized instruction
            }
            return code;
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
                    byte bits = (byte)((byte)dv & 0x7F); dv >>= 7; // Take off 7 bits at a time
                    bitStorage |= (byte)(bits << (ctr*8)); // Write the 7 bits into storage
                    bitStorage |= (byte)(1 << (7 + ctr++*8)); // continue reading flag
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

        // General Utility
        internal static string[] getHexLines(byte[] data, int count = 4)
        {
            data = data ?? new byte[0];
            // Generates an x-byte wide space separated string array; leftovers included at the end.
            string[] s = new string[data.Length/count + (data.Length % count > 0 ? 1 : 0)];
            for (int i = 0; i < s.Length;i++)
                s[i] = BitConverter.ToString(data.Skip(i*count).Take(count).ToArray()).Replace('-', ' ');
            return s;
        }
        internal static string[] getHexLines(uint[] data)
        {
            data = data ?? new uint[0];
            // Generates an 4-byte wide space separated string array.
            string[] s = new string[data.Length];
            for (int i = 0; i < s.Length; i++)
                s[i] = BitConverter.ToString(BitConverter.GetBytes(data[i])).Replace('-', ' ');
            return s;
        }
        internal static byte[] getBytes(uint[] data)
        {
            return data.Aggregate(new byte[0], (current, t) => current.Concat(BitConverter.GetBytes(t)).ToArray());
        }

        // Interpreting
        internal static string[] parseScript(uint[] cmd)
        {
            string[] rv = new string[cmd.Length * 4];  // arbitrary length, gets resized to final dim at the end.
            int used = 0;
            for (int i = 0; i < cmd.Length; i++)
            {
                uint c = cmd[i];
                string op;
                int offset = i*4;
                switch (c & 0x7FF)
                {
                    case 0x09: op = "$09";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x0B: op = "$0B";
                        op += eA(new[] { cmd[++i] }); break;
                    case 0x0C: op = "$0C";
                        op += eA(new[] { cmd[++i] }); break;
                    case 0x0E: op = "$0E";
                        op += eA(new[] { cmd[++i] }); break;
                    case 0x17: op = "$17";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x20: op = "$20";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x21: op = "$21";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x22: op = "$22";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x23: op = "$23";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x24: op = "$24";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x25: op = "$25";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x27: op = "PushConst";
                        op += eA(new[] { cmd[++i] }); break;
                    case 0x2B: op = "$2B";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x2E: op = "Begin"; break;
                    case 0x30: op = "Return\n"; break;
                    case 0x31: op = "CallFunc";
                        op += $"[0x{(i*4 + (int)cmd[++i]).ToString("X4")}] ({(int)cmd[i]})";
                        break;
                    case 0x33: op = "$33";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                        break;
                    case 0x34: op = "$34";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                        break;
                    case 0x35: op = "Jump!=";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                        break;
                    case 0x36: op = "Jump==";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                        break;
                    case 0x37: op = "$37";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                        break;
                    case 0x38: op = "$38";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                        break;
                    case 0x3D: op = "$3D";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                        break;
                    case 0x3E: op = "$3E";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                        break;
                    case 0x3F: op = "$3F";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                        break;
                    case 0x40: op = "$40";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                        break;
                    case 0x41: op = "$41";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x43: op = "$43";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x48: op = "$48";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x4A: op = "$4A";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x4E: op = "Add?"; break;
                    case 0x4F: op = "$4F"; // followed by 0x24
                        op += eA(new[] { c >> 16 }); break;
                    case 0x50: op = "$50";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x51: op = "Cmp?";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x52: op = "$52";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x55: op = "$55";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x56: op = "$56";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x59: op = "ClearAll"; break;
                    case 0x5A: op = "$5A";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x5F: op = "$5F";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x60: op = "$60";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x65: op = "$65"; // followed by 0x24
                        op += eA(new[] { c >> 16 }); break;
                    case 0x66: op = "$66";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x67: op = "$67";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x68: op = "$68";
                        op += eA(new[] { c >> 16 }); break;
                    case 0x69: op = "$69";
                        op += eA(new[] { cmd[++i] }); break;
                    case 0x75: op = "$75";
                        op += eA(new[] { cmd[++i] }); break;
                    case 0x77: op = "$77";
                        op += eA(new[] { cmd[++i] }); break;
                    case 0x78: op = "$78";
                        op += eA(new[] { cmd[++i] }); break;
                    case 0x81: op = "Jump";
                        op += $" => 0x{(i*4 + (int)cmd[++i]).ToString("X4")} ({(int)cmd[i]})";
                                break;
                    case 0x82:
                    {
                        op = "JumpIfElse";
                        uint count = cmd[++i];
                        int[] jump = new int[count];
                        int[] val = new int[count];
                        // Populate If-Case Tree
                        for (int j = 0; j < count; j++)
                        {
                            jump[j] = (int)cmd[++i];
                            val[j] = (int)cmd[++i];
                            op += Environment.NewLine +
                                     string.Format("\t{2} => 0x{0} ({1})",
                                        ((i-1)*4 + jump[j]).ToString("X4"),
                                        jump[j],
                                        val[j]);
                        }
                        // Else-Default
                        int elsejump = (int)cmd[++i];
                        op += Environment.NewLine +
                              $"\t * => 0x{((i - 1)*4 + elsejump).ToString("X4")} ({elsejump})";
                        break;
                    }
                    case 0x87: op = "DoCommand?";
                        op += eA(new[] { cmd[++i], cmd[++i] }); break;
                    case 0x89: op = "LineNo?"; break;
                    case 0x8A: op = "$8A";
                        op += eF(new[] { cmd[++i], cmd[++i] }); break;
                    case 0x8E: op = "$8E";
                        op += eF(new[] { cmd[++i], cmd[++i], cmd[++i] }); break;
                    case 0x92: op = "$92";
                        op += eA(new[] { cmd[++i], cmd[++i], cmd[++i], cmd[++i] }); break;
                    case 0x96: op = "$96";
                        op += eF(new[] { cmd[++i], 
                            cmd[++i], cmd[++i], cmd[++i], cmd[++i] }); break;
                    case 0x9B: op = "Copy";
                        op += eA(new[] { cmd[++i], cmd[++i] }); break;
                    case 0x9C: op = "$9C";
                        op += eA(new[] { cmd[++i], cmd[++i] }); break;
                    case 0x9D: op = "$9D";
                        op += eA(new[] { cmd[++i], cmd[++i] }); break;
                    case 0xA2: op = "GetGlobal2";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xA3: op = "GetGlobal";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xA4: op = "GetGlobal";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xA5: op = "$A5";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xAB: op = "PushConst2";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xAC: op = "CmpConst2";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xAD: op = "$AD";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xAE: op = "$AE";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xAF: op = "SetGlobal";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xB1: op = "SetLocal";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xB8: op = "$B8";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xB9: op = "$B9";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xBC: op = "PushConst";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xBD: op = "GetGlobal3";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xBE: op = "GetArg";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xBF: op = "AdjustStack";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xC5: op = "$C5";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xC6: op = "$C6";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xC7: op = "$C7";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xC8: op = "CmpLocal";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xC9: op = "CmpConst";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xCB: op = "$CB";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xCC: op = "$CC";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xCE: op = "$CE";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xCF: op = "$CF";
                        op += eA(new[] { c >> 16 }); break;
                    case 0xD4: op = "$D4";
                        op += eA(new[] { c >> 16 }); break;

                    case 0xD2: op = "BeginScript"+Environment.NewLine; break;
                    case 0x0: op = "Nop"; break;
                    default: op = $"**${(c & 0xFFFF).ToString("X2")}**";
                        op += eA(new[] { c >> 16 }); break;
                }
                rv[used++] = string.Format("0x{2}: [{0}] {1}", (c & 0x7FF).ToString("X2"), op, offset.ToString("X4"));
            }
            Array.Resize(ref rv, used);  // End result will cap out at lines used.
            return rv;
        }

        internal static string[] parseMovement(uint[] cmd)
        {
            return getHexLines(cmd);
        }

        internal static string eA(uint[] arr)
        {
            string s = "";
            for (int i = 0; i < arr.Length; i++)
                s += $"0x{arr[i].ToString("X4")}{(i + 1 < arr.Length ? ", " : "")}";
            return "("+s+")";
        }
        internal static string eF(uint[] arr)
        {
            string s = "";
            for (int i = 0; i < arr.Length; i++)  // stupid hack, Convert.ToSingle((uint)) doesn't behave.
                s += $"{BitConverter.ToSingle(BitConverter.GetBytes(arr[i]), 0)}{(i + 1 < arr.Length ? ", " : "")}";
            return "(" + s + ")";
        }
    }
}
