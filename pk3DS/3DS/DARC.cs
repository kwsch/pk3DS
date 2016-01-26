using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTR
{
    public class DARC
    {
        public byte[] Data;
        public DARCHeader Header;
        public FileTableEntry[] Entries;
        public NameTableEntry[] FileNameTable;

        public DARC(byte[] Data = null)
        {
            if (Data == null) return;
            using (BinaryReader br = new BinaryReader(new MemoryStream(Data)))
            try
            {
                Header = new DARCHeader(br);
                br.BaseStream.Position = Header.FileTableOffset;
                FileTableEntry root = new FileTableEntry(br);
                Entries = new FileTableEntry[root.DataLength];
                Entries[0] = root;
                for (int i = 1; i < root.DataLength; i++) Entries[i] = new FileTableEntry(br);
                FileNameTable = new NameTableEntry[root.DataLength];
                uint offs = 0;
                for (int i = 0; i < root.DataLength; i++)
                {
                    char c; string s = string.Empty;
                    while ((c = (char) br.ReadUInt16()) > 0) s += c;

                    FileNameTable[i] = new NameTableEntry(offs, s);
                    offs += (uint)s.Length * 2 + 2;
                }
                br.BaseStream.Position = Header.FileDataOffset;
                this.Data = br.ReadBytes((int)(Header.FileSize - Header.FileDataOffset));
            }
            catch (Exception)
            { br.Close(); }
        }

        public class DARCHeader
        {
            public DARCHeader(BinaryReader br = null)
            {
                if (br == null) return;
                Signature = new string(br.ReadChars(4));
                if (Signature != "darc") throw new Exception(Signature);
                Endianness = br.ReadUInt16();
                HeaderSize = br.ReadUInt16();
                Version = br.ReadUInt32();
                FileSize = br.ReadUInt32();
                FileTableOffset = br.ReadUInt32();
                FileTableLength = br.ReadUInt32();
                FileDataOffset = br.ReadUInt32();
            }
            public string Signature;
            public UInt16 Endianness;
            public UInt16 HeaderSize;
            public uint Version;
            public uint FileSize;
            public uint FileTableOffset;
            public uint FileTableLength;
            public uint FileDataOffset;
        }
        public class FileTableEntry
        {
            public FileTableEntry(BinaryReader br = null)
            {
                if (br == null) return;
                NameOffset = br.ReadUInt32();
                IsFolder = NameOffset >> 24 == 1;
                NameOffset &= 0xFFFFFF;
                DataOffset = br.ReadUInt32();
                DataLength = br.ReadUInt32();
            }
            public uint NameOffset;
            public Boolean IsFolder;
            public uint DataOffset; // FOLDER: Parent Entry Index
            public uint DataLength; // FOLDER: Next Folder Index
        }
        public class NameTableEntry
        {
            public uint NameOffset;
            public string FileName;
            public NameTableEntry(uint offset, string fileName)
            {
                NameOffset = offset;
                FileName = fileName;
            }
        }

        // DARC r/w
        internal static byte[] setDARC(DARC darc)
        {
            // Package DARC into a writable array.
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                // Write Header
                bw.Write(Encoding.ASCII.GetBytes(darc.Header.Signature));
                bw.Write(darc.Header.Endianness);
                bw.Write(darc.Header.HeaderSize);
                bw.Write(darc.Header.Version);
                bw.Write(darc.Header.FileSize);
                bw.Write(darc.Header.FileTableOffset);
                bw.Write(darc.Header.FileTableLength);
                bw.Write(darc.Header.FileDataOffset);
                // Write FileTableEntries
                foreach (FileTableEntry entry in darc.Entries)
                {
                    bw.Write(entry.NameOffset | (entry.IsFolder ? (uint)1 << 24 : 0));
                    bw.Write(entry.DataOffset);
                    bw.Write(entry.DataLength);
                }
                foreach (NameTableEntry entry in darc.FileNameTable)
                {
                    bw.Write(Encoding.Unicode.GetBytes(entry.FileName + "\0"));
                }
                while (bw.BaseStream.Position < darc.Header.FileDataOffset)
                    bw.Write((byte)0);

                // Write Data
                bw.Write(darc.Data);

                return ms.ToArray();
            }
        }
        internal static DARC getDARC(string folderName)
        {
            // Package Folder into a DARC.
            List<FileTableEntry> EntryList = new List<FileTableEntry>();
            List<NameTableEntry> NameList = new List<NameTableEntry>();
            byte[] Data = new byte[0];
            uint nameOffset = 6; // 00 00 + 00 2E 00 00
            #region Build FileTable/NameTables
            {
                // Null First File
                {
                    EntryList.Add(new FileTableEntry {DataOffset = 0, DataLength = 0, IsFolder = true, NameOffset = 0});
                    NameList.Add(new NameTableEntry(0, ""));
                }
                // "." Second File
                {
                    EntryList.Add(new FileTableEntry {DataOffset = 0, DataLength = 0, IsFolder = true, NameOffset = 2});
                    NameList.Add(new NameTableEntry(6, "."));
                }
                foreach (string folder in Directory.GetDirectories(folderName))
                {
                    string parentName = new DirectoryInfo(folder).Name;
                    string[] files = Directory.GetFiles(folder);
                    NameList.Add(new NameTableEntry(nameOffset, parentName));
                    EntryList.Add(new FileTableEntry
                    {
                        DataOffset = 1,
                        DataLength = (uint) (files.Count() + EntryList.Count),
                        IsFolder = true,
                        NameOffset = nameOffset
                    });
                    nameOffset += (uint) parentName.Length + 2; // Account for null terminator

                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        string fileName = fi.Name;
                        NameList.Add(new NameTableEntry(nameOffset, parentName));


                        EntryList.Add(new FileTableEntry
                        {
                            DataOffset = (uint) Data.Length,
                            DataLength = (uint) fi.Length,
                            IsFolder = false,
                            NameOffset = nameOffset
                        });
                        Data = Data.Concat(File.ReadAllBytes(file)).ToArray();
                        nameOffset += (uint) fileName.Length + 2; // Account for null terminator
                    }
                }
            }
            #endregion

            // Compute Necessary DARC information
            int darcFileCount = NameList.Count;
            int NameListOffset = darcFileCount * 0xC;
            int NameListLength = (int)(nameOffset + NameListOffset);
            int DataOffset = NameListLength % 4 == 0 ? NameListLength : NameListLength + (4 - NameListLength % 4);
            Array.Resize(ref Data, Data.Length % 4 == 0 ? Data.Length : Data.Length + 4 - Data.Length % 4);
            int FinalSize = DataOffset + Data.Length;

            // Create New DARC
            DARC darc = new DARC
            {
                Header =
                {
                    Signature = "darc", 
                    Endianness = 0xFEFF, 
                    HeaderSize = 0x1C, 
                    Version = 1,
                    FileSize = (uint)FinalSize,
                    FileTableOffset = 0x1C,
                    FileTableLength = (uint)NameListLength,
                    FileDataOffset = (uint)DataOffset,
                },
                Entries = EntryList.ToArray(),
                FileNameTable = NameList.ToArray(),
                Data = Data,
            };
            // Fix the First two folders to specify the number of files
            darc.Entries[0].DataLength = (uint)darcFileCount;
            darc.Entries[1].DataLength = (uint)darcFileCount;

            // Fix the Data Offset of the files to point to actual destination
            foreach (FileTableEntry f in darc.Entries.Where(x => !x.IsFolder))
                f.DataOffset += darc.Header.FileDataOffset;
            return darc;
        }

        internal static bool darc2files(string path, string folderName)
        {
            try { return darc2files(File.ReadAllBytes(path), folderName); }
            catch (Exception) { return false; }
        }
        internal static bool darc2files(byte[] darc, string folderName)
        {
            // Save all contents of a DARC to a folder, assuming there's only 1 layer of folders.
            try
            {
                // Clear existing contents
                string root = folderName;
                if (Directory.Exists(root))
                    Directory.Delete(root, true);

                // Create new DARC object from input data
                DARC DARC = new DARC(darc);

                // Output data
                for (int i = 2; i < DARC.FileNameTable.Length;)
                {
                    bool isFolder = DARC.Entries[i].IsFolder;
                    if (!isFolder) 
                        return false;
                    // uint level = DARC.Entries[i].DataOffset; Only assuming 1 layer of folders.
                    string parentName = DARC.FileNameTable[i].FileName;
                    Directory.CreateDirectory(Path.Combine(root, parentName));

                    int nextFolder = (int)DARC.Entries[i++].DataLength;

                    // Extract all Contents of said folder
                    while (i < nextFolder)
                    {
                        string fileName = DARC.FileNameTable[i].FileName;
                        int offset = (int)DARC.Entries[i].DataOffset;
                        int length = (int)DARC.Entries[i].DataLength;
                        byte[] data = DARC.Data.Skip((int)(offset - DARC.Header.FileDataOffset)).Take(length).ToArray();

                        string outPath = Path.Combine(root, parentName, fileName);
                        File.WriteAllBytes(outPath, data);
                        i++; // Advance to next Entry
                    }
                }
                return true;
            }
            catch (Exception) { return false; }
        }
        internal static bool files2darc(string folderName, bool delete = false, string originalDARC = null, string outFile = null)
        {
            // Save all contents of a folder to a darc.
            try
            {
                byte[] darcData;
                DARC orig;
                string root = folderName;
                if (originalDARC != null)
                {
                    // Fetch offset of DARC within file.
                    byte[] darc = File.ReadAllBytes(originalDARC);
                    int darcPos = getDARCposition(darc);
                    if (darcPos < 0) return false;
                    byte[] origData = darc.Skip(darcPos).ToArray();

                    orig = new DARC(origData);
                    orig = insertFiles(orig, folderName);
                    byte[] newDARC = setDARC(orig);
                    darcData = darc.Take(darcPos).Concat(newDARC).ToArray();
                }
                else // no existing darc to get
                {
                    orig = getDARC(folderName);
                    darcData = setDARC(orig);
                }

                // Fetch final name if not specified
                outFile = outFile ?? originalDARC ?? new DirectoryInfo(folderName).Name.Replace("_d", "") + ".darc";

                if (darcData == null) return false;
                File.WriteAllBytes(outFile, darcData);

                if (Directory.Exists(root) && delete)
                    Directory.Delete(root, true);
                return true;
            } catch (Exception) { return false; }
        }
        
        // DARC Utility
        internal static int getDARCposition(byte[] data)
        {
            int pos = 0;
            while (BitConverter.ToUInt32(data, pos) != 0x63726164)
            { pos += 4; if (pos >= data.Length) return -1; }
            return pos;
        }
        internal static bool insertFile(ref DARC orig, int index, string path)
        {
            try { return insertFile(ref orig, index, File.ReadAllBytes(path)); }
            catch (Exception) { return false; }
        }
        internal static bool insertFile(ref DARC orig, int index, byte[] data)
        {
            if (index < 0) return false;

            try
            {
                uint oldLength = orig.Entries[index].DataLength;
                uint offset = orig.Entries[index].DataOffset - orig.Header.FileDataOffset;
                int diff = (int) (data.Length - oldLength);

                // Insert into Data Block
                byte[] pre = orig.Data.Take((int) offset).ToArray();
                byte[] post = orig.Data.Skip((int) (offset + oldLength)).ToArray();

                // Reassemble data
                orig.Data = pre.Concat(data).Concat(post).ToArray();

                // Fix Offset references of other files
                foreach (var x in orig.Entries.Where(x => x.DataOffset >= offset + oldLength))
                    x.DataOffset += (uint) diff;
                orig.Entries[index].DataLength = (uint)data.Length; 
                orig.Header.FileSize += (uint)diff;
                return true;
            }
            catch (Exception) { return false; }
        }
        internal static DARC insertFiles(DARC orig, string folderName)
        {
            string[] fileNames = new string[orig.Entries.Count()];
            for (int i = 0; i < fileNames.Length; i++)
                fileNames[i] = orig.FileNameTable[i].FileName;

            string[] files = Directory.GetFiles(folderName, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                string FileName = fi.Name;

                // Get Index of file
                int index = Array.IndexOf(fileNames, FileName);
                if (orig.Entries[index].IsFolder)
                    throw new Exception(file + " is not a valid file to reinsert!");

                insertFile(ref orig, index, file);
            }
            // Fix Data layout
            Array.Resize(ref orig.Data, orig.Data.Length % 4 == 0 ? orig.Data.Length : orig.Data.Length + 4 - orig.Data.Length % 4);
            orig.Header.FileSize = (uint)(orig.Data.Length + orig.Header.FileDataOffset);
            return orig;
        }
    }
}
