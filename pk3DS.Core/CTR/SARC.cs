using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace pk3DS.Core.CTR
{
    /// <summary>
    /// Simple (?) ARChive
    /// </summary>
    public sealed class SARC : IDisposable
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

        /// <summary>
        /// The required <see cref="Magic"/> matches the first 4 bytes of the file data.
        /// </summary>
        public bool SigMatches => Magic == Identifier;
        private readonly Stream stream;
        private readonly BinaryReader br;

        /// <summary>
        /// Initializes an empty <see cref="SARC"/>.
        /// </summary>
        public SARC()
        {
            SFAT = new SFAT();
            SFNT = new SFNT();
        }

        /// <summary>
        /// Initializes a <see cref="SARC"/> from a file location.
        /// </summary>
        /// <param name="path"></param>
        public SARC(string path)
        {
            SetFileInfo(path);

            stream = File.OpenRead(path);
            br = new BinaryReader(stream);
            ReadSARC();
            Valid = true;
        }

        /// <summary>
        /// Initializes a <see cref="SARC"/> from a provided stream.
        /// </summary>
        /// <param name="fs"></param>
        public SARC(Stream fs)
        {
            stream = fs;
            br = new BinaryReader(stream);
            ReadSARC();
            Valid = true;
        }

        /// <summary>
        /// Initializes a <see cref="SARC"/> from a provided array.
        /// </summary>
        /// <param name="data"></param>
        public SARC(byte[] data)
        {
            stream = new MemoryStream(data);
            br = new BinaryReader(stream);
            ReadSARC();
            Valid = true;
        }

        /// <summary>
        /// Reads the contents of the <see cref="SARC"/> header and file info tables.
        /// </summary>
        private void ReadSARC()
        {
            Magic = new string(br.ReadChars(4));
            if (!SigMatches)
                return;

            HeaderSize = br.ReadUInt16();
            Endianness = br.ReadUInt16();
            FileSize = br.ReadUInt32();
            DataOffset = br.ReadUInt32();
            Unknown = br.ReadUInt32();

            SFAT = new SFAT(br);
            SFNT = new SFNT(br);
        }

        /// <summary>
        /// Sets File information for the original file.
        /// </summary>
        /// <param name="path"></param>
        public void SetFileInfo(string path)
        {
            FileName = Path.GetFileNameWithoutExtension(path);
            FilePath = Path.GetDirectoryName(path);
            Extension = Path.GetExtension(path);
        }

        /// <summary>
        /// Gets the entry filename for a given <see cref="SFATEntry"/>.
        /// </summary>
        /// <param name="entry">Entry to fetch data for</param>
        /// <returns>File Name</returns>
        public string GetFileName(SFATEntry entry) => GetFileName(entry.FileNameOffset);

        /// <summary>
        /// Gets the entry data for a given <see cref="SFATEntry"/>,
        /// </summary>
        /// <param name="entry">Entry to fetch data for</param>
        /// <returns>Data array</returns>
        public byte[] GetData(SFATEntry entry) => GetData(entry.FileDataStart, entry.FileDataLength);

        /// <summary>
        /// Overwrites the entry data, assuming the size is the exact same.
        /// </summary>
        /// <param name="entry">File entry to overwrite</param>
        /// <param name="data">Data to write</param>
        public void SetData(SFATEntry entry, byte[] data)
        {
            if (data.Length != entry.FileDataLength)
                throw new ArgumentException(nameof(data.Length));
            SetData(entry.FileDataStart, data);
        }

        /// <summary>
        /// Exports the entry data for a given <see cref="SFATEntry"/> at a provided path with its assigned <see cref="SFATEntry"/> file name via the <see cref="SFNT"/> name table.
        /// </summary>
        /// <param name="t">Entry to export</param>
        /// <param name="outpath">Path to export to. If left null, will output to the <see cref="SARC"/> FilePath, if it is assigned.</param>
        public string ExportFile(SFATEntry t, string outpath = null)
        {
            outpath ??= FilePath;
            byte[] data = GetData(t);
            string name = GetFileName(t);

            string dir = Path.GetDirectoryName(name);
            if (dir == null)
                throw new ArgumentException(name);
            string location = Path.Combine(outpath, dir);
            Directory.CreateDirectory(location);

            var filepath = Path.Combine(outpath, name);
            File.WriteAllBytes(filepath, data);
            return filepath;
        }

        /// <summary>
        /// Dumps the contents of the <see cref="SARC"/> to a provided folder. If no location is provided, it will dump to the SARC's location.
        /// </summary>
        /// <param name="path">Path to create dump folder in</param>
        /// <param name="folder">Folder to dump contents to</param>
        public IEnumerable<string> Dump(string path = null, string folder = null)
        {
            path ??= FilePath;
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (File.Exists(path))
                path = Path.GetDirectoryName(path);
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            folder ??= FileName ?? "sarc";
            string dir = Path.Combine(path, folder);

            Directory.CreateDirectory(dir);

            foreach (SFATEntry t in SFAT.Entries)
                yield return ExportFile(t, dir);
        }

        private string GetFileName(int offset)
        {
            stream.Seek(SFNT.StringOffset, SeekOrigin.Begin);
            stream.Seek((offset & 0x00FFFFFF) * 4, SeekOrigin.Current);
            StringBuilder sb = new StringBuilder();
            for (char c = (char)stream.ReadByte(); c != 0; c = (char)stream.ReadByte())
                sb.Append(c);

            return sb.ToString().Replace('/', Path.DirectorySeparatorChar);
        }

        public void SetFileName(int offset, string value)
        {
            var str = value.Replace(Path.DirectorySeparatorChar, '/');
            stream.Seek(SFNT.StringOffset, SeekOrigin.Begin);
            stream.Seek((offset & 0x00FFFFFF) * 4, SeekOrigin.Current);
            foreach (var b in str)
                stream.WriteByte((byte)b);
            stream.WriteByte((byte)'\0');
        }

        private byte[] GetData(int offset, int length)
        {
            byte[] fileBuffer = new byte[length];
            stream.Seek(offset + DataOffset, SeekOrigin.Begin);
            stream.Read(fileBuffer, 0, length);
            return fileBuffer;
        }

        private void SetData(int offset, byte[] data)
        {
            stream.Seek(offset + DataOffset, SeekOrigin.Begin);
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Disposes of the <see cref="stream"/> and <see cref="br"/> objects and frees the <see cref="FileName"/> if originally loaded from that location.
        /// </summary>
        public void Dispose()
        {
            stream?.Dispose();
            br?.Dispose();
        }
    }

    /// <summary>
    /// <see cref="SARC"/> File Access Table
    /// </summary>
    public class SFAT
    {
        public const string Identifier = nameof(SFAT);

        /// <summary>
        /// The required <see cref="Magic"/> matches the first 4 bytes of the file data.
        /// </summary>
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

    /// <summary>
    /// <see cref="SARC"/> File Name Table
    /// </summary>
    public class SFNT
    {
        public const string Identifier = nameof(SFNT);

        /// <summary>
        /// The required <see cref="Magic"/> matches the first 4 bytes of the file data.
        /// </summary>
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

    /// <summary>
    /// <see cref="SARC"/> File Access Table (<see cref="SFAT"/>) Entry
    /// </summary>
    public class SFATEntry
    {
        public uint FileNameHash;
        public int FileNameOffset;
        public int FileDataStart;
        public int FileDataEnd;

        public int FileDataLength => FileDataEnd - FileDataStart;

        public SFATEntry(BinaryReader br)
        {
            FileNameHash = br.ReadUInt32();
            FileNameOffset = br.ReadInt32();
            FileDataStart = br.ReadInt32();
            FileDataEnd = br.ReadInt32();
        }
    }
}