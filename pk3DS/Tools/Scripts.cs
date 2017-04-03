using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pk3DS
{
    // Big thanks to FireFly for figuring out the 7/6-bit compression routine for scripts.
    public static class Scripts
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
        internal static string[] parseScript(uint[] cmd, int sanity = -1)
        {
            // sub_148CBC Moon v1.0
            List<string> parse = new List<string>();
            int sanityMode = 0;

            int i = 0; // Current Offset of decompressed instructions
            while (i < cmd.Length) // read away
            {
                // Read a Command
                int line = i;
                uint c = cmd[i++];

                string op;
                switch (c & 0x7FFF)
                {
                    default:
                        throw new ArgumentException("Invalid Command ID");
                    case 0x01:
                    case 0x02:
                    case 0x05:
                    case 0x06:
                    case 0x0F:
                    case 0x10:
                    case 0x13:
                    case 0x14:
                    case 0x6D:
                    case 0x72:
                    {
                        // Peek at next value
                        var next = (int)cmd[i++];
                        // Check Value against negative and zero... ?
                        
                        op = eA(c, next);
                        break;
                    }
                    case 0x03:
                    case 0x04:
                    case 0x07:
                    case 0x08:
                    case 0x11:
                    case 0x12:
                    case 0x15:
                    case 0x16:
                    case 0x6E:
                    case 0x73:
                    {
                        // Peek at next value
                        var next = (int)cmd[i++];
                        // Check Value against negative... ?
                        
                        op = eA(c, next);
                        break;
                    }
                    case 0x09:
                    case 0x17:
                    case 0x19:
                    case 0x1B:
                    case 0x21:
                    case 0x22:
                    case 0x23:
                    case 0x24:
                    case 0x25:
                    case 0x2A:
                    case 0x2B:
                    case 0x2E: // Begin
                    case 0x2F:
                    case 0x30: // Return
                    case 0x41:
                    case 0x42:
                    case 0x43:
                    case 0x48:
                    case 0x49:
                    case 0x4A:
                    case 0x4B:
                    case 0x4C:
                    case 0x4D:
                    case 0x4E: // Add?
                    case 0x4F:
                    case 0x50:
                    case 0x51: // Cmp?
                    case 0x52:
                    case 0x53:
                    case 0x54:
                    case 0x55:
                    case 0x56:
                    case 0x59: // ClearAll
                    case 0x5A:
                    case 0x5D:
                    case 0x5E:
                    case 0x5F:
                    case 0x60:
                    case 0x61:
                    case 0x62:
                    case 0x63:
                    case 0x64:
                    case 0x65:
                    case 0x66:
                    case 0x67:
                    case 0x68:
                    case 0x6B:
                    case 0x6C:
                    case 0x6F:
                    case 0x70:
                    case 0x71:
                    case 0x74:
                    case 0x7A:
                    case 0x83:
                    case 0x84:
                    case 0x86:
                    case 0x89: // LineNo?
                    case 0xAA:
                    case 0xAB: // PushConst2
                    case 0xAC: // CmpConst2
                    case 0xAD:
                    case 0xAE:
                    case 0xB7:
                    case 0xB8:
                    case 0xB9:
                    case 0xBA:
                    case 0xBB:
                    case 0xBC: // PushConst
                    case 0xBD:
                    case 0xBE:
                    case 0xBF: // AdjustStack
                    case 0xC0:
                    case 0xC1:
                    case 0xC2:
                    case 0xC3:
                    case 0xC4:
                    case 0xC5:
                    case 0xC6:
                    case 0xC7:
                    case 0xC8: // CmpLocal
                    case 0xC9: // CmpConst
                    case 0xCA:
                    case 0xCF:
                    case 0xD0:
                    case 0xD1:
                    case 0xD2:
                    case 0xD3:
                    case 0xD4:
                    {
                        // no sanity checks
                        var arg = (short)(c >> 16);

                        op = eA(c & 0xFF, arg);

                        if ((c & 0xFF) == 0x30) // return
                            op += Environment.NewLine;
                        break;
                    }
                    case 0x0A:
                    case 0x0B:
                    case 0x0C:
                    case 0x0D:
                    case 0x0E:
                    case 0x18:
                    case 0x1A:
                    case 0x1C:
                    case 0x1D:
                    case 0x1E:
                    case 0x1F:
                    case 0x20:
                    case 0x26:
                    case 0x27: // PushConst
                    case 0x28:
                    case 0x29:
                    case 0x2C:
                    case 0x2D:
                    case 0x34:
                    case 0x44:
                    case 0x45:
                    case 0x46:
                    case 0x47:
                    case 0x57:
                    case 0x58:
                    case 0x5B:
                    case 0x5C:
                    case 0x69:
                    case 0x6A:
                    case 0x75:
                    case 0x76:
                    case 0x77:
                    case 0x78:
                    case 0x79:
                    case 0x85:
                    {
                        var next = (int)cmd[i++];
                        // No sanity check needed
                        
                        op = eA(c, next);
                        break;
                    }

                    case 0x31: // CallFunc
                    case 0x33:
                    case 0x35: // Jump!=
                    case 0x36: // Jump==
                    case 0x37:
                    case 0x38:
                    case 0x39:
                    case 0x3A:
                    case 0x3B:
                    case 0x3C:
                    case 0x3D:
                    case 0x3E:
                    case 0x3F:
                    case 0x40:
                    case 0x81: // Jump
                    {
                        var delta = (int)cmd[i++];
                        // sanity check range...
                        // negative.. weird
                        
                        int newOfs = line*4 + delta;
                        op = $"{Commands[c]} => 0x{newOfs:X4} ({delta})";
                        break;
                    }
                    case 0x7B:
                    {
                        var next = (int)cmd[i++];
                        sanityMode |= 1;                       // flag mode 1

                        op = eA(c, next);
                        break;
                    }
                    case 0x82: // JumpIfElse
                    {
                        var jOffset = i*4 -4; // todo: this may be the correct jump start point...
                        var count = cmd[i++]; // switch case table
                        // sanity check
                        
                        // Populate If-Case Tree
                        var tree = new List<string>();

                        // Cases
                        for (int j = 0; j < count; j++)
                        {
                            var jmp = (int)cmd[i++];
                            var toOffset = (i-2)*4 + jmp;
                            var ifValue = (int)cmd[i++];
                            tree.Add($"\t{ifValue} => 0x{toOffset:X4} ({jmp})");
                        }
                        // Default
                        {
                            int jmp = (int)cmd[i++];
                            var toOffset = (i-2)*4 + jmp;
                            tree.Add($"\t{"*"} => 0x{toOffset:X4} ({jmp})");
                        }

                        op = Commands[c] + Environment.NewLine + string.Join(Environment.NewLine, tree);
                        break;
                    }
                    case 0x87:
                    {
                        var next1 = (int)cmd[i++];
                        var next2 = (int)cmd[i++];
                        sanityMode |= 2;                       // flag mode 2

                        op = eA(c, next1, next2);
                        break;
                    }

                    case 0x8A:
                    case 0x8B:
                    case 0x8C:
                    case 0x8D:
                    case 0x9C:
                    case 0x9D:
                    {
                        var next1 = (int)cmd[i++];
                        var next2 = (int)cmd[i++];

                        op = eA(c, next1, next2);
                        break;
                    }

                    case 0x8E:
                    case 0x8F:
                    case 0x90:
                    case 0x91:
                    {
                        var next1 = cmd[i++];
                        var next2 = cmd[i++];
                        var next3 = cmd[i++];

                        op = eF(c, next1, next2, next3);
                        break;
                    }

                    case 0x92:
                    case 0x93:
                    case 0x94:
                    case 0x95:
                    {
                        var next1 = (int)cmd[i++];
                        var next2 = (int)cmd[i++];
                        var next3 = (int)cmd[i++];
                        var next4 = (int)cmd[i++];

                        op = eA(c, next1, next2, next3, next4);
                        break;
                    }

                    case 0x96: // float
                    case 0x97:
                    case 0x98:
                    case 0x99:
                    {
                        var next1 = cmd[i++];
                        var next2 = cmd[i++];
                        var next3 = cmd[i++];
                        var next4 = cmd[i++];
                        var next5 = cmd[i++];

                        op = eF(c, next1, next2, next3, next4, next5);
                        break;
                    }

                    case 0x9A:
                    {
                        var next1 = (int)cmd[i++];
                        var next2 = (int)cmd[i++];
                        // a bunch of sanity checking

                        op = eA(c, next1, next2);
                        break;
                    }

                    case 0x9B: // Copy
                    {
                        var next1 = (int)cmd[i++];
                        var next2 = (int)cmd[i++];
                        // a bunch of sanity checking

                        op = eA(c, next1, next2);
                        break;
                    }

                    case 0x9E:
                    {
                        var next1 = (int)cmd[i++];
                        // perm check a1 + 0x14
                        // can return error code 0x1C

                        op = eA(c, next1);
                        break;
                    }

                    case 0x9F:
                    {
                        // perm check a1 + 0x14
                        // same permission checking as 0x9E
                        // can return error code 0x1C

                        op = eA(c);
                        break;
                    }

                    case 0xA1: // Goto
                    {
                        // minimal sanity checks
                        // can return error code 0x1C
                        int newPos = i + (int)(1 + 2*(cmd[i]/4) + 1);

                        op = eA(c, newPos);
                        break;
                    }

                    case 0xA2: // GetGlobal2
                    case 0xA3: // GetGlobal
                    case 0xA6:
                    case 0xA7:
                    case 0xAF: // SetGlobal
                    case 0xB0:
                    case 0xB3:
                    case 0xB4:
                    case 0xCB:
                    case 0xCD:
                    {
                        // sanity check arg
                        var arg = (short)(c >> 16);

                        op = eA(c & 0xFF, arg);
                        break;
                    }
                    case 0xA4: // GetGlobal4
                    case 0xA5:
                    case 0xA8:
                    case 0xA9:
                    case 0xB1: // SetLocal
                    case 0xB2:
                    case 0xB5:
                    case 0xB6:
                    case 0xCC:
                    case 0xCE:
                    {
                        // sanity check arg, slightly different
                        var arg = (short)(c >> 16);

                        op = eA(c & 0xFF, arg);
                        break;
                    }
                }
                parse.Add($"0x{line*4:X4}: [{c & 0x7FF:X2}] {op}");
            }

            if (sanity >= 0 && sanity != sanityMode)
                throw new ArgumentException();

            return parse.ToArray();
        }

        internal static string[] parseMovement(uint[] cmd)
        {
            return getHexLines(cmd);
        }

        internal static string eA(uint c, params int[] arr)
        {
            string cmd = Commands[c];
            string parameters = arr.Length == 0 ? "" : string.Join(", ", arr.Select(z => $"{(Math.Abs(z) < 100 ? z.ToString() : "0x"+z.ToString("X4"))}"));
            return $"{cmd}({parameters})";
        }
        private static readonly Func<uint, float> getFloat = val => BitConverter.ToSingle(BitConverter.GetBytes(val), 0);
        internal static string eF(uint c, params uint[] arr)
        {
            string cmd = Commands[c];
            string parameters = arr.Length == 1 ? "" : string.Join(", ", arr.Select(z => getFloat(z)));
            return $"{cmd}({parameters})";
        }
        
        internal static readonly Dictionary<uint, string> Commands = new Dictionary<uint, string>
        {// { 0x00, "$00" }, // Invalid Code
            { 0x01, "$01" },
            { 0x02, "$02" },
            { 0x03, "$03" },
            { 0x04, "$04" },
            { 0x05, "$05" },
            { 0x06, "$06" },
            { 0x07, "$07" },
            { 0x08, "$08" },
            { 0x09, "$09" },
            { 0x0A, "$0A" },
            { 0x0B, "$0B" },
            { 0x0C, "$0C" },
            { 0x0D, "$0D" },
            { 0x0E, "$0E" },
            { 0x0F, "$0F" },
            { 0x10, "$10" },
            { 0x11, "$11" },
            { 0x12, "$12" },
            { 0x13, "$13" },
            { 0x14, "$14" },
            { 0x15, "$15" },
            { 0x16, "$16" },
            { 0x17, "$17" },
            { 0x18, "$18" },
            { 0x19, "$19" },
            { 0x1A, "$1A" },
            { 0x1B, "$1B" },
            { 0x1C, "$1C" },
            { 0x1D, "$1D" },
            { 0x1E, "$1E" },
            { 0x1F, "$1F" },
            { 0x20, "$20" },
            { 0x21, "$21" },
            { 0x22, "$22" },
            { 0x23, "$23" },
            { 0x24, "$24" },
            { 0x25, "$25" },
            { 0x26, "$26" },
            { 0x27, "PushConst" },
            { 0x28, "$28" },
            { 0x29, "$29" },
            { 0x2A, "$2A" },
            { 0x2B, "$2B" },
            { 0x2C, "$2C" },
            { 0x2D, "$2D" },
            { 0x2E, "Begin" },
            { 0x2F, "$2F" },
            { 0x30, "Return" },
            { 0x31, "CallFunc" },
            { 0x32, "$32" },
            { 0x33, "$33" },
            { 0x34, "$34" },
            { 0x35, "BNE" },
            { 0x36, "BEQ" },
            { 0x37, "$37" },
            { 0x38, "$38" },
            { 0x39, "$39" },
            { 0x3A, "$3A" },
            { 0x3B, "$3B" },
            { 0x3C, "$3C" },
            { 0x3D, "$3D" },
            { 0x3E, "$3E" },
            { 0x3F, "$3F" },
            { 0x40, "$40" },
            { 0x41, "$41" },
            { 0x42, "$42" },
            { 0x43, "$43" },
            { 0x44, "$44" },
            { 0x45, "$45" },
            { 0x46, "$46" },
            { 0x47, "$47" },
            { 0x48, "$48" },
            { 0x49, "$49" },
            { 0x4A, "$4A" },
            { 0x4B, "$4B" },
            { 0x4C, "$4C" },
            { 0x4D, "$4D" },
            { 0x4E, "Add?" },
            { 0x4F, "$4F" },
            { 0x50, "$50" },
            { 0x51, "Cmp?" },
            { 0x52, "$52" },
            { 0x53, "$53" },
            { 0x54, "$54" },
            { 0x55, "$55" },
            { 0x56, "$56" },
            { 0x57, "$57" },
            { 0x58, "$58" },
            { 0x59, "ClearAll" },
            { 0x5A, "$5A" },
            { 0x5B, "$5B" },
            { 0x5C, "$5C" },
            { 0x5D, "$5D" },
            { 0x5E, "$5E" },
            { 0x5F, "$5F" },
            { 0x60, "$60" },
            { 0x61, "$61" },
            { 0x62, "$62" },
            { 0x63, "$63" },
            { 0x64, "$64" },
            { 0x65, "$65" },
            { 0x66, "$66" },
            { 0x67, "$67" },
            { 0x68, "$68" },
            { 0x69, "$69" },
            { 0x6A, "$6A" },
            { 0x6B, "$6B" },
            { 0x6C, "$6C" },
            { 0x6D, "$6D" },
            { 0x6E, "$6E" },
            { 0x6F, "$6F" },
            { 0x70, "$70" },
            { 0x71, "$71" },
            { 0x72, "$72" },
            { 0x73, "$73" },
            { 0x74, "$74" },
            { 0x75, "$75" },
            { 0x76, "$76" },
            { 0x77, "$77" },
            { 0x78, "$78" },
            { 0x79, "$79" },
            { 0x7A, "$7A" },
            { 0x7B, "$7B" },
         // { 0x7C, "$7C" }, // Invalid Code
         // { 0x7D, "$7D" }, // Invalid Code
         // { 0x7E, "$7E" }, // Invalid Code
         // { 0x7F, "$7F" }, // Invalid Code
         // { 0x80, "$80" }, // Invalid Code
            { 0x81, "JMP" },
            { 0x82, "switch" },
            { 0x83, "$83" },
            { 0x84, "$84" },
            { 0x85, "$85" },
            { 0x86, "$86" },
            { 0x87, "DoCommand?" },
         // { 0x88, "$88" }, // Invalid Code
            { 0x89, "LineNo?" },
            { 0x8A, "$8A" },
            { 0x8B, "$8B" },
            { 0x8C, "$8C" },
            { 0x8D, "$8D" },
            { 0x8E, "$8E" },
            { 0x8F, "$8F" },
            { 0x90, "$90" },
            { 0x91, "$91" },
            { 0x92, "$92" },
            { 0x93, "$93" },
            { 0x94, "$94" },
            { 0x95, "$95" },
            { 0x96, "$96" },
            { 0x97, "$97" },
            { 0x98, "$98" },
            { 0x99, "$99" },
            { 0x9A, "$9A" },
            { 0x9B, "Copy" },
            { 0x9C, "$9C" },
            { 0x9D, "$9D" },
            { 0x9E, "$9E" },
            { 0x9F, "$9F" },
            { 0xA0, "$A0" },
            { 0xA1, "$A1" },
            { 0xA2, "GetGlobal2" },
            { 0xA3, "GetGlobal" },
            { 0xA4, "GetGlobal4" },
            { 0xA5, "$A5" },
            { 0xA6, "$A6" },
            { 0xA7, "$A7" },
            { 0xA8, "$A8" },
            { 0xA9, "$A9" },
            { 0xAA, "$AA" },
            { 0xAB, "PushConst2" },
            { 0xAC, "CmpConst2" },
            { 0xAD, "$AD" },
            { 0xAE, "$AE" },
            { 0xAF, "SetGlobal" },
            { 0xB0, "$B0" },
            { 0xB1, "SetLocal" },
            { 0xB2, "$B2" },
            { 0xB3, "$B3" },
            { 0xB4, "$B4" },
            { 0xB5, "$B5" },
            { 0xB6, "$B6" },
            { 0xB7, "$B7" },
            { 0xB8, "$B8" },
            { 0xB9, "$B9" },
            { 0xBA, "$BA" },
            { 0xBB, "$BB" },
            { 0xBC, "PushConst" },
            { 0xBD, "GetGlobal3" },
            { 0xBE, "GetArg" },
            { 0xBF, "AdjustStack" },
            { 0xC0, "$C0" },
            { 0xC1, "$C1" },
            { 0xC2, "$C2" },
            { 0xC3, "$C3" },
            { 0xC4, "$C4" },
            { 0xC5, "$C5" },
            { 0xC6, "$C6" },
            { 0xC7, "$C7" },
            { 0xC8, "CmpLocal" },
            { 0xC9, "CmpConst" },
            { 0xCA, "$CA" },
            { 0xCB, "$CB" },
            { 0xCC, "$CC" },
            { 0xCD, "$CD" },
            { 0xCE, "$CE" },
            { 0xCF, "$CF" },
            { 0xD0, "$D0" },

            { 0xD1, "$D1" },
            { 0xD2, "$D2" },
            { 0xD3, "$D3" },
            { 0xD4, "$D4" },
            { 0xD5, "Mode1" },
            { 0xD6, "Mode2" },
        };
    }
}
