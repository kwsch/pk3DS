using System.Collections.Generic;
using System.IO;
using System.Text;

namespace pk3DS.Core.CTR
{
    /// <summary>
    /// Archive LaYouT container
    /// </summary>
    /// <remarks> File length is padded to nearest 0x80 </remarks>
    public class ALYT
    {
        private const string Identifier = nameof(ALYT);
        public string Magic;   // 0x00
        public short unk4;     // 0x04
        public short unk6;     // 0x06
        public int LTBLOffset; // 0x08
        public int LTBLSize;   // 0x0C
        public int LMTLOffset; // 0x10
        public int LMTLSize;   // 0x14
        public int LFNLOffset; // 0x18
        public int LFNLSize;   // 0x1C

        public int DataOffset; // 0x20
        public int DataSize;   // 0x24

        public LTBL LTBL;
        public LMTL LMTL;
        public LFNL LFNL;
        public Contents Content;
        public byte[] Data;

        public string FileName { get; }
        public string FilePath { get; }
        public string Extension { get; }
        public bool SigMatches => Magic == Identifier;

        public ALYT(string path)
        {
            FileName = Path.GetFileNameWithoutExtension(path);
            FilePath = Path.GetDirectoryName(path);
            Extension = Path.GetExtension(path);

            using var br = new BinaryReader(File.OpenRead(path));
            ReadALYT(br);
        }

        public ALYT(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            ReadALYT(br);
        }

        public ALYT(Stream ms)
        {
            using var br = new BinaryReader(ms);
            ReadALYT(br);
        }

        private void ReadALYT(BinaryReader br)
        {
            ReadHeader(br);
            LTBL = new LTBL(br, (LTBLSize - 8) / 4);
            LMTL = new LMTL(br, (LMTLSize - 8) / 4);
            LFNL = new LFNL(br, (LFNLSize - 8) / 4);
            Content = new Contents(br, this);
        }

        private void ReadHeader(BinaryReader br)
        {
            Magic = new string(br.ReadChars(4));
            if (!SigMatches)
                return;
            unk4 = br.ReadInt16();
            unk6 = br.ReadInt16();
            LTBLOffset = br.ReadInt32();
            LTBLSize = br.ReadInt32();
            LMTLOffset = br.ReadInt32();
            LMTLSize = br.ReadInt32();
            LFNLOffset = br.ReadInt32();
            LFNLSize = br.ReadInt32();
            DataOffset = br.ReadInt32();
            DataSize = br.ReadInt32();
        }

        public class Contents
        {
            public readonly string[] Labels;
            public readonly string[] Symbols;

            public Contents(BinaryReader br, ALYT alyt)
            {
                br.BaseStream.Position = alyt.DataOffset;
                Labels = new string[br.ReadInt32()];
                for (int i = 0; i < Labels.Length; i++)
                    Labels[i] = ReadString(br, 0x40);

                Symbols = new string[br.ReadInt32()];
                for (int i = 0; i < Symbols.Length; i++)
                    Symbols[i] = ReadString(br, 0x20);

                // skip to end of section
                // this is bad
                while (br.PeekChar() == 0)
                    br.ReadByte();

                int len = (int)br.BaseStream.Position - alyt.DataOffset;
                len = alyt.DataSize - len;
                alyt.Data = br.ReadBytes(len);
            }

            public static string ReadString(BinaryReader br, int skip)
            {
                var sb = new StringBuilder();
                var ofs = br.BaseStream.Position;
                while (true)
                {
                    var c = (char)br.ReadByte();
                    if (c == 0)
                    {
                        br.BaseStream.Position = ofs + skip;
                        return sb.ToString();
                    }
                    sb.Append(c);
                }
            }
        }

        /// <summary>
        /// Rips out the data portion of the ALYT, assuming the ALYT is partially valid.
        /// </summary>
        public static byte[] GetData(string path)
        {
            using var br = new BinaryReader(File.OpenRead(path));
            return GetData(br);
        }

        /// <summary>
        /// Rips out the data portion of the ALYT, assuming the ALYT is partially valid.
        /// </summary>
        public static byte[] GetData(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            return GetData(br);
        }

        /// <summary>
        /// Rips out the data portion of the ALYT, assuming the ALYT is partially valid.
        /// </summary>
        public static byte[] GetData(Stream ms)
        {
            using var br = new BinaryReader(ms);
            return GetData(br);
        }

        private static byte[] GetData(BinaryReader br)
        {
            if (br.BaseStream.Length <= 0x80)
                return null;
            if (Identifier != new string(br.ReadChars(4)))
                return null; // not ALYT

            br.BaseStream.Position = 0x20; // DataOffset
            int start = br.ReadInt32();
            int length = br.ReadInt32();

            br.BaseStream.Position = start;
            int count40 = br.ReadInt32();
            br.BaseStream.Position += count40 * 0x40;

            int count20 = br.ReadInt32();
            br.BaseStream.Position += count20 * 0x20;

            // skip to end of section
            // this is bad
            while (br.PeekChar() == 0)
                br.ReadByte();

            int len = (int)br.BaseStream.Position - start;
            len = length - len;
            return br.ReadBytes(len);
        }
    }

    public class LTBL
    {
        private const string Identifier = nameof(LTBL);
        public string Magic;   // 0x00
        public short unk4;     // 0x04
        public short unk6;     // 0x06
        public List<int> Values;

        public LTBL(BinaryReader br, int count)
        {
            Magic = new string(br.ReadChars(Identifier.Length));
            unk4 = br.ReadInt16();
            unk6 = br.ReadInt16();
            Values = new List<int>(count);
            for (int i = 0; i < count; i++)
                Values.Add((short)br.ReadInt32());
        }
    }

    public class LMTL
    {
        private const string Identifier = nameof(LMTL);
        public string Magic;   // 0x00
        public short unk4;     // 0x04
        public short unk6;     // 0x06
        public List<int> Values;

        public LMTL(BinaryReader br, int count)
        {
            Magic = new string(br.ReadChars(Identifier.Length));
            unk4 = br.ReadInt16();
            unk6 = br.ReadInt16();
            Values = new List<int>(count);
            for (int i = 0; i < count; i++)
                Values.Add((short)br.ReadInt32());
        }
    }

    public class LFNL
    {
        private const string Identifier = nameof(LFNL);
        public string Magic;   // 0x00
        public short unk4;     // 0x04
        public short unk6;     // 0x06
        public List<int> Values;

        public LFNL(BinaryReader br, int count)
        {
            Magic = new string(br.ReadChars(Identifier.Length));
            unk4 = br.ReadInt16();
            unk6 = br.ReadInt16();
            Values = new List<int>(count);
            for (int i = 0; i < count; i++)
                Values.Add((short)br.ReadInt32());
        }
    }
}
