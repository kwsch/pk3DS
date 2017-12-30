using System;
using System.Collections.Generic;
using System.IO;

namespace pk3DS.Core.CTR
{
    public class SARC
    {
        private const string Identifier = nameof(SARC);

        public string Magic;
        public ushort HeaderSize;
        public ushort Endianness;
        public uint FileSize;
        public uint DataOffset;
        public uint Unknown;

        public SFAT SFAT;
        public SFNT SFNT;

        // Assigned Properties
        public string FileName;
        public string FilePath;
        public string Extension;
        public readonly bool Valid;

        public bool SigMatches => Magic == Identifier;

        public SARC()
        {
            SFAT = new SFAT();
            SFNT = new SFNT();
        }
        public SARC(string path)
        {
            FileName = Path.GetFileNameWithoutExtension(path);
            FilePath = Path.GetDirectoryName(path);
            Extension = Path.GetExtension(path);

            using (var br = new BinaryReader(File.OpenRead(path)))
            {
                ReadHeader(br);
                SFAT = new SFAT(br);
                SFNT = new SFNT(br);
            }
            Valid = true;
        }
        private void ReadHeader(BinaryReader br)
        {
            Magic = new string(br.ReadChars(4));
            if (!SigMatches)
                return;

            HeaderSize = br.ReadUInt16();
            Endianness = br.ReadUInt16();
            FileSize = br.ReadUInt32();
            DataOffset = br.ReadUInt32();
            Unknown = br.ReadUInt32();
        }
    }
    public class SFAT
    {
        public const string Identifier = nameof(SFAT);
        public bool SigMatches => Magic == Identifier;

        public string Magic;
        public ushort HeaderSize;
        public ushort EntryCount;
        public uint HashMult;
        public List<SFATEntry> Entries;

        public SFAT() { }
        public SFAT(BinaryReader br)
        {
            Magic = new string(br.ReadChars(4));
            if (!SigMatches)
                throw new FormatException(nameof(SFAT));

            HeaderSize = br.ReadUInt16();
            EntryCount = br.ReadUInt16();
            HashMult = br.ReadUInt32();
            Entries = new List<SFATEntry>();

            for (int i = 0; i < EntryCount; i++)
                Entries.Add(new SFATEntry(br));
        }
    }
    public class SFNT
    {
        public const string Identifier = nameof(SFNT);
        public bool SigMatches => Magic == Identifier;

        public string Magic;
        public ushort HeaderSize;
        public ushort Unknown;
        public uint StringOffset;

        public SFNT() { }
        public SFNT(BinaryReader br)
        {
            Magic = new string(br.ReadChars(4));
            if (!SigMatches)
                throw new FormatException(nameof(SFNT));

            HeaderSize = br.ReadUInt16();
            Unknown = br.ReadUInt16();
            StringOffset = (uint)br.BaseStream.Position;
        }
    }
    public class SFATEntry
    {
        public uint FileNameHash;
        public uint FileNameOffset;
        public uint FileDataStart;
        public uint FileDataEnd;

        public SFATEntry(BinaryReader br)
        {
            FileNameHash = br.ReadUInt32();
            FileNameOffset = br.ReadUInt32();
            FileDataStart = br.ReadUInt32();
            FileDataEnd = br.ReadUInt32();
        }
    }
}