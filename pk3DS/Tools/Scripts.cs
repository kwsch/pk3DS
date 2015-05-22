using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace pk3DS
{
    // Big thanks to FireFly for figuring out the 7-bit compression routine for scripts.
    class Scripts
    {
        internal static byte[] decompressScript(byte[] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            using (MemoryStream mn = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(mn))
            {
                // Read away!
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    // Read until the top bit is not set
                    List<byte>raw = new List<byte>();
                    byte val = br.ReadByte(); raw.Add(val);
                    while ((val >> 7) > 0)
                    {
                        if (br.BaseStream.Position == br.BaseStream.Length) return mn.ToArray();
                        val = br.ReadByte(); raw.Add(val);
                    }
                    byte[] compressedBytes = raw.ToArray();

                    // Interpret the bytecode
                    val = compressedBytes[0];

                    if ((val & 0x40) > 0) // Signed Parameter
                    {
                        // Check the next bytecode
                        if (compressedBytes.Length > 1 && (compressedBytes[1] >> 7) > 0) // Many-bits-required command
                        {
                            // 2 Byte Signed Parameter
                            int cmd = (compressedBytes[0] & 0x3 << 14) | (compressedBytes[1] & 0x7F << 7) | compressedBytes[2]; // 16 Bits total
                            bw.Write(BitConverter.GetBytes(cmd), 0, 2);

                            int deviation = ((val & 0x3F) - 0x40) >> 2; // Lowest 2 bits have already been used for the command
                            bw.Write(BitConverter.GetBytes(deviation), 0, 2); // 16 Bits
                        }
                        else if ((val >> 7) > 0)
                        {
                            // 3 Byte Signed Parameter
                            int cmd = ((compressedBytes[0] << 7) | compressedBytes[1]);
                            bw.Write(BitConverter.GetBytes(cmd), 0, 1);  // 8 Bits Total

                            int deviation = ((val & 0x3F) - 0x40) >> 1; // Lowest bit has already been used for the command
                            bw.Write(BitConverter.GetBytes(deviation), 0, 3); // 24 Bits
                        }
                        else
                        {
                            // 4 Byte Signed Parameter
                            int deviation = ((val & 0x3F) - 0x40) >> 0; // No bits have already been used; no command
                            bw.Write(BitConverter.GetBytes(deviation), 0, 4); // 32 Bits
                        }
                    }
                    else if ((val >> 7) > 0) // Manybit
                    {
                        Array.Reverse(compressedBytes);
                        int cmd = 0;
                        for (int i = 0; i < compressedBytes.Length; i++)
                        {
                            cmd |= ((compressedBytes[i] & 0x7F) << (7*i));
                        }

                        bw.Write(BitConverter.GetBytes((uint)cmd), 0, 4);
                    }
                    else // Literal
                    {
                        bw.Write(BitConverter.GetBytes((uint)val), 0, 4);
                    }

                    uint result = BitConverter.ToUInt32(mn.GetBuffer(), (int) bw.BaseStream.Position - 4);
                }
                return mn.ToArray();
            }
        }
        internal static byte[] compressScript(byte[] data)
        {
            // Need decompression functional first.
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            using (MemoryStream mn = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(mn))
            {
                // Compress away!
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    // Read the command to compress
                    uint val = br.ReadUInt32();

                    // Write byte if more bits are needed
                    while ((val >> 7) > 0) // 8th+ bit is set
                    {
                        bw.Write((byte)((val & 0x7F) | 0x80));
                        val >>= 7;
                    }
                    // Write the remaining bits
                    bw.Write((byte)(val & 0x7F));
                }
                return mn.ToArray();
            }
        }

        internal static string getu32line(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i+= 4)
                sb.Append(BitConverter.ToString(data, i, 4).Replace('-', ' ') + "\r\n");
            return sb.ToString();
        }
    }
}
