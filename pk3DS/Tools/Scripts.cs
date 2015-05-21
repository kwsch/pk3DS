using System;
using System.Collections.Generic;
using System.IO;

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
                        val = br.ReadByte(); raw.Add(val);
                    }

                    byte[] compressedBytes = raw.ToArray();
                    Array.Reverse(compressedBytes);
                    uint cmd = val;

                    // Assemble the bits into the final uint
                    for (int i = 0; i < compressedBytes.Length; i++)
                        cmd |= (uint)(compressedBytes[i] & 0x7F) << (7 * i);

                    bw.Write(cmd);
                }
                return mn.ToArray();
            }
        }
        internal static byte[] compressScript(byte[] data)
        {
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
    }
}
