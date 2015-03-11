using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace pk3DS
{
    public class RomFSTool
    {
        internal const int PADDING_ALIGN = 16;
        internal static string ROOT_DIR;
        internal const string TempFile = "tempRomFS.bin";
        internal static string OutFile;
        internal const uint ROMFS_UNUSED_ENTRY = 0xFFFFFFFF;

        internal static void updateTB(RichTextBox RTB, string progress)
        {
            try
            {
                if (RTB.InvokeRequired)
                    RTB.Invoke((MethodInvoker)delegate
                    {
                        RTB.AppendText(Environment.NewLine + progress);
                        RTB.SelectionStart = RTB.Text.Length;
                        RTB.ScrollToCaret();
                    });
                else
                {
                    RTB.SelectionStart = RTB.Text.Length;
                    RTB.ScrollToCaret();
                    RTB.AppendText(progress + Environment.NewLine);
                }
            }
            catch { }
        }
        internal static void BuildRomFS(string outfile, string infile, RichTextBox TB_Progress = null, ProgressBar PB_Show = null)
        {
            OutFile = outfile;
            ROOT_DIR = infile;
            if (File.Exists(TempFile)) File.Delete(TempFile);

            FileNameTable FNT = new FileNameTable(ROOT_DIR);
            RomfsFile[] RomFiles = new RomfsFile[FNT.NumFiles];
            LayoutManager.Input[] In = new LayoutManager.Input[FNT.NumFiles];
            updateTB(TB_Progress, "Creating Layout...");
            for (int i = 0; i < FNT.NumFiles; i++)
            {
                In[i] = new LayoutManager.Input {FilePath = FNT.NameEntryTable[i].FullName, AlignmentSize = 0x10};
            }
            LayoutManager.Output[] Out = LayoutManager.Create(In);
            for (int i = 0; i < Out.Length; i++)
            {
                RomFiles[i] = new RomfsFile
                {
                    Offset = Out[i].Offset,
                    PathName = Out[i].FilePath.Replace(Path.GetFullPath(ROOT_DIR), "").Replace("\\", "/"),
                    FullName = Out[i].FilePath,
                    Size = Out[i].Size
                };
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                updateTB(TB_Progress, "Creating RomFS MetaData...");
                BuildRomFSHeader(memoryStream, RomFiles, ROOT_DIR);
                MakeRomFSData(RomFiles, memoryStream, TB_Progress, PB_Show);
            }
        }

        internal static ulong Align(ulong input, ulong alignsize)
        {
            ulong output = input;
            if (output % alignsize != 0)
            {
                output += (alignsize - (output % alignsize));
            }
            return output;
        }
        internal static void MakeRomFSData(RomfsFile[] RomFiles, MemoryStream metadata, RichTextBox TB_Progress = null, ProgressBar PB_Show = null)
        {
            updateTB(TB_Progress, "Computing IVFC Header Data...");
            IVFCInfo ivfc = new IVFCInfo {Levels = new IVFCLevel[3]};
            for (int i = 0; i < ivfc.Levels.Length; i++)
            {
                ivfc.Levels[i] = new IVFCLevel {BlockSize = 0x1000};
            }
            ivfc.Levels[2].DataLength = RomfsFile.GetDataBlockLength(RomFiles, (ulong)metadata.Length);
            ivfc.Levels[1].DataLength = (Align(ivfc.Levels[2].DataLength, ivfc.Levels[2].BlockSize) / ivfc.Levels[2].BlockSize) * 0x20; //0x20 per SHA256 hash
            ivfc.Levels[0].DataLength = (Align(ivfc.Levels[1].DataLength, ivfc.Levels[1].BlockSize) / ivfc.Levels[1].BlockSize) * 0x20; //0x20 per SHA256 hash
            ulong MasterHashLen = (Align(ivfc.Levels[0].DataLength, ivfc.Levels[0].BlockSize) / ivfc.Levels[0].BlockSize) * 0x20;
            ulong lofs = 0;
            foreach (IVFCLevel t in ivfc.Levels)
            {
                t.HashOffset = lofs;
                lofs += Align(t.DataLength, t.BlockSize);
            }
            const uint IVFC_MAGIC = 0x43465649; //IVFC
            const uint RESERVED = 0x0;
            const uint HeaderLen = 0x5C;
            const uint MEDIA_UNIT_SIZE = 0x200;
            byte[] SuperBlockHash = new byte[0x20];
            FileStream OutFileStream = new FileStream(TempFile, FileMode.Create, FileAccess.ReadWrite);
            try
            {
                OutFileStream.Seek(0, SeekOrigin.Begin);
                OutFileStream.Write(BitConverter.GetBytes(IVFC_MAGIC), 0, 0x4);
                OutFileStream.Write(BitConverter.GetBytes(0x10000), 0, 0x4);
                OutFileStream.Write(BitConverter.GetBytes(MasterHashLen), 0, 0x4);
                foreach (IVFCLevel t in ivfc.Levels)
                {
                    OutFileStream.Write(BitConverter.GetBytes(t.HashOffset), 0, 0x8);
                    OutFileStream.Write(BitConverter.GetBytes(t.DataLength), 0, 0x8);
                    OutFileStream.Write(BitConverter.GetBytes((int)(Math.Log(t.BlockSize, 2))), 0, 0x4);
                    OutFileStream.Write(BitConverter.GetBytes(RESERVED), 0, 0x4);
                }
                OutFileStream.Write(BitConverter.GetBytes(HeaderLen), 0, 0x4);
                //IVFC Header is Written.
                OutFileStream.Seek((long)Align(MasterHashLen + 0x60, ivfc.Levels[0].BlockSize), SeekOrigin.Begin);
                byte[] metadataArray = metadata.ToArray();
                OutFileStream.Write(metadataArray, 0, metadataArray.Length);
                long baseOfs = OutFileStream.Position;
                updateTB(TB_Progress, "Writing Level 2 Data...");
                if (PB_Show.InvokeRequired)
                    PB_Show.Invoke((MethodInvoker)delegate { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = RomFiles.Length; });
                else { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = RomFiles.Length; }
                    
                foreach (RomfsFile t in RomFiles)
                {
                    OutFileStream.Seek(baseOfs + (long)t.Offset, SeekOrigin.Begin);
                    using (FileStream inStream = new FileStream(t.FullName, FileMode.Open, FileAccess.Read))
                    {
                        while (inStream.Position < inStream.Length)
                        {
                            byte[] buffer = new byte[inStream.Length - inStream.Position > 0x100000 ? 0x100000 : inStream.Length - inStream.Position];
                            inStream.Read(buffer, 0, buffer.Length);
                            OutFileStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    if (PB_Show.InvokeRequired)
                        PB_Show.Invoke((MethodInvoker)delegate { PB_Show.PerformStep(); });
                    else { PB_Show.PerformStep(); }
                }
                long hashBaseOfs = (long)Align((ulong)OutFileStream.Position, ivfc.Levels[2].BlockSize);
                long hOfs = (long)Align(MasterHashLen, ivfc.Levels[0].BlockSize);
                long cOfs = hashBaseOfs + (long)ivfc.Levels[1].HashOffset;
                SHA256Managed sha = new SHA256Managed();
                for (int i = ivfc.Levels.Length - 1; i >= 0; i--)
                {
                    updateTB(TB_Progress, "Computing Level " + i + " Hashes...");
                    byte[] buffer = new byte[(int)ivfc.Levels[i].BlockSize];

                    if (PB_Show.InvokeRequired)
                    PB_Show.Invoke((MethodInvoker)delegate { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = (int)(ivfc.Levels[i].DataLength / ivfc.Levels[i].BlockSize); });
                    else { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = (int)(ivfc.Levels[i].DataLength / ivfc.Levels[i].BlockSize); }
                    
                    for (long ofs = 0; ofs < (long)ivfc.Levels[i].DataLength; ofs += ivfc.Levels[i].BlockSize)
                    {
                        OutFileStream.Seek(hOfs, SeekOrigin.Begin);
                        OutFileStream.Read(buffer, 0, (int)ivfc.Levels[i].BlockSize);
                        hOfs = OutFileStream.Position;
                        byte[] hash = sha.ComputeHash(buffer);
                        OutFileStream.Seek(cOfs, SeekOrigin.Begin);
                        OutFileStream.Write(hash, 0, hash.Length);
                        cOfs = OutFileStream.Position;
                        if (PB_Show.InvokeRequired)
                            PB_Show.Invoke((MethodInvoker)delegate { PB_Show.PerformStep(); });
                        else { PB_Show.PerformStep(); }
                    }
                    if (i == 2)
                    {
                        long len = OutFileStream.Position;
                        if (len % 0x1000 != 0)
                        {
                            len = (long)Align((ulong)len, 0x1000);
                            byte[] buf = new byte[len - OutFileStream.Position];
                            OutFileStream.Write(buf, 0, buf.Length);
                        }
                    }
                    if (i <= 0) continue;
                    hOfs = hashBaseOfs + (long)ivfc.Levels[i - 1].HashOffset;
                    if (i > 1)
                        cOfs = hashBaseOfs + (long)ivfc.Levels[i - 2].HashOffset;
                    else
                        cOfs = (long)Align(HeaderLen, PADDING_ALIGN);
                }
                OutFileStream.Seek(0, SeekOrigin.Begin);
                uint SuperBlockLen = (uint)Align(MasterHashLen + 0x60, MEDIA_UNIT_SIZE);
                byte[] MasterHashes = new byte[SuperBlockLen];
                OutFileStream.Read(MasterHashes, 0, (int)SuperBlockLen);
                SuperBlockHash = sha.ComputeHash(MasterHashes);
            }
            finally
            {
                if (OutFileStream != null)
                    OutFileStream.Dispose();
            }
            updateTB(TB_Progress, "RomFS Super Block Hash: " + ByteArrayToString(SuperBlockHash));
            updateTB(TB_Progress, "Saving to destination...");
            
            if (File.Exists(OutFile)) File.Delete(OutFile);
            File.Move(TempFile, OutFile);
        }
        internal static void WriteBinary(string tempFile, string outFile, RichTextBox TB_Progress = null, ProgressBar PB_Show = null)
        {
            using (FileStream fs = new FileStream(outFile, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    using (FileStream fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                    {
                        const uint BUFFER_SIZE = 0x100000;

                        if (PB_Show.InvokeRequired)
                            PB_Show.Invoke((MethodInvoker)delegate { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = (int)(fileStream.Length / BUFFER_SIZE); });
                        else { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = (int)(fileStream.Length / BUFFER_SIZE); }

                        byte[] buffer = new byte[BUFFER_SIZE];
                        while (true)
                        {
                            int count = fileStream.Read(buffer, 0, buffer.Length);
                            if (count != 0)
                            {
                                writer.Write(buffer, 0, count);
                                if (PB_Show.InvokeRequired)
                                    PB_Show.Invoke((MethodInvoker)delegate { PB_Show.PerformStep(); });
                                else { PB_Show.PerformStep(); }
                            }
                            else
                                break;
                        }
                    }
                    writer.Flush();
                }
            }
            File.Delete(TempFile);
            Util.Alert("Wrote RomFS to path:", outFile);
        }
        internal static string ByteArrayToString(IEnumerable<byte> input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in input)
                sb.Append(b.ToString("X2") + " ");

            return sb.ToString();
        }

        internal static void UpdateTB_Progress(string text, RichTextBox TB_Progress = null)
        {
            TB_Progress.Text += text + Environment.NewLine;
        }
        internal static void BuildRomFSHeader(MemoryStream romfs_stream, RomfsFile[] Entries, string DIR)
        {
            ROOT_DIR = DIR;

            Romfs_MetaData MetaData = new Romfs_MetaData();

            InitializeMetaData(MetaData);

            CalcRomfsSize(MetaData);

            PopulateRomfs(MetaData, Entries);

            WriteMetaDataToStream(MetaData, romfs_stream);
        }
        internal static void InitializeMetaData(Romfs_MetaData MetaData)
        {
            MetaData.InfoHeader = new Romfs_InfoHeader();
            MetaData.DirTable = new Romfs_DirTable();
            MetaData.DirTableLen = 0;
            MetaData.M_DirTableLen = 0;
            MetaData.FileTable = new Romfs_FileTable();
            MetaData.FileTableLen = 0;
            MetaData.DirTable.DirectoryTable = new List<Romfs_DirEntry>();
            MetaData.FileTable.FileTable = new List<Romfs_FileEntry>();
            MetaData.InfoHeader.HeaderLength = 0x28;
            MetaData.InfoHeader.Sections = new Romfs_SectionHeader[4];
            MetaData.DirUTable = new List<uint>();
            MetaData.FileUTable = new List<uint>();
        }
        internal static void CalcRomfsSize(Romfs_MetaData MetaData)
        {
            MetaData.DirNum = 1;
            DirectoryInfo Root_DI = new DirectoryInfo(ROOT_DIR);
            CalcDirSize(MetaData, Root_DI);

            MetaData.M_DirUTableEntry = 3;
            if (MetaData.DirNum > 3)
                MetaData.M_DirUTableEntry += (uint)Align((ulong)MetaData.DirNum - 3, 2);

            MetaData.M_FileUTableEntry = 3;
            if (MetaData.FileNum > 3)
                MetaData.M_FileUTableEntry += (uint)Align((ulong)MetaData.FileNum - 3, 2);


            uint MetaDataSize = (uint)Align(0x28 + MetaData.M_DirUTableEntry * 4 + MetaData.M_DirTableLen + MetaData.M_FileUTableEntry * 4 + MetaData.M_FileTableLen, PADDING_ALIGN);
            for (int i = 0; i < MetaData.M_DirUTableEntry; i++)
                MetaData.DirUTable.Add(ROMFS_UNUSED_ENTRY);

            for (int i = 0; i < MetaData.M_FileUTableEntry; i++)
                MetaData.FileUTable.Add(ROMFS_UNUSED_ENTRY);

            uint Pos = MetaData.InfoHeader.HeaderLength;
            for (int i = 0; i < 4; i++)
            {
                MetaData.InfoHeader.Sections[i].Offset = Pos;
                uint size = 0;
                switch (i)
                {
                    case 0:
                        size = MetaData.M_DirUTableEntry * 4;
                        break;
                    case 1:
                        size = MetaData.M_DirTableLen;
                        break;
                    case 2:
                        size = MetaData.M_FileUTableEntry * 4;
                        break;
                    case 3:
                        size = MetaData.M_FileTableLen;
                        break;
                }
                MetaData.InfoHeader.Sections[i].Size = size;
                Pos += size;
            }
            MetaData.InfoHeader.DataOffset = MetaDataSize;
        }
        internal static void CalcDirSize(Romfs_MetaData MetaData, DirectoryInfo dir)
        {
            if (MetaData.M_DirTableLen == 0)
                MetaData.M_DirTableLen = 0x18;
            else
                MetaData.M_DirTableLen += 0x18 + (uint)Align((ulong)dir.Name.Length * 2, 4);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo t in files)
                MetaData.M_FileTableLen += 0x20 + (uint)Align((ulong)t.Name.Length * 2, 4);

            DirectoryInfo[] SubDirectories = dir.GetDirectories();
            foreach (DirectoryInfo t in SubDirectories)
                CalcDirSize(MetaData, t);

            MetaData.FileNum += (uint)files.Length;
            MetaData.DirNum += (uint)SubDirectories.Length;
        }
        internal static void PopulateRomfs(Romfs_MetaData MetaData, RomfsFile[] Entries)
        {
            //Recursively Add All Directories to DirectoryTable
            AddDir(MetaData, new DirectoryInfo(ROOT_DIR), 0, ROMFS_UNUSED_ENTRY);

            //Iteratively Add All Files to FileTable
            AddFiles(MetaData, Entries);

            //Set Weird Offsets, Buld DirUTable+FileUTable
            CalculateWeirdOffsets(MetaData);

            //Thats it.
        }
        internal static void CalculateWeirdOffsets(Romfs_MetaData MetaData)
        {
            for (int i = 0; i < MetaData.DirTable.DirectoryTable.Count; i++)
                AddDirHashKey(MetaData, i);
            for (int i = 0; i < MetaData.FileTable.FileTable.Count; i++)
                AddFileHashKey(MetaData, i);
        }

        internal static void AddDirHashKey(Romfs_MetaData MetaData, int index)
        {
            uint parent = MetaData.DirTable.DirectoryTable[index].ParentOffset;
            string Name = MetaData.DirTable.DirectoryTable[index].Name;
            byte[] NArr = (index == 0) ? Encoding.Unicode.GetBytes("") : Encoding.Unicode.GetBytes(Name);
            uint hash = CalcPathHash(parent, NArr, 0, NArr.Length);
            int ind2 = (int)(hash % MetaData.M_DirUTableEntry);
            if (MetaData.DirUTable[ind2] == ROMFS_UNUSED_ENTRY)
            {
                MetaData.DirUTable[ind2] = MetaData.DirTable.DirectoryTable[index].Offset;
            }
            else
            {
                int i = GetRomfsDirEntry(MetaData, MetaData.DirUTable[ind2]);
                while (true)
                {
                    if (MetaData.DirTable.DirectoryTable[i].WeirdOffset == ROMFS_UNUSED_ENTRY)
                    {
                        MetaData.DirTable.DirectoryTable[i].WeirdOffset = MetaData.DirTable.DirectoryTable[index].Offset;
                        break;
                    }
                    i = GetRomfsDirEntry(MetaData, MetaData.DirTable.DirectoryTable[i].WeirdOffset);
                }
            }
        }
        internal static void AddFileHashKey(Romfs_MetaData MetaData, int index)
        {
            uint parent = MetaData.FileTable.FileTable[index].ParentDirOffset;
            string Name = MetaData.FileTable.FileTable[index].Name;
            byte[] NArr = Encoding.Unicode.GetBytes(Name);
            uint hash = CalcPathHash(parent, NArr, 0, NArr.Length);
            int ind2 = (int)(hash % MetaData.M_FileUTableEntry);
            if (MetaData.FileUTable[ind2] == ROMFS_UNUSED_ENTRY)
            {
                MetaData.FileUTable[ind2] = MetaData.FileTable.FileTable[index].Offset;
            }
            else
            {
                int i = GetRomfsFileEntry(MetaData, MetaData.FileUTable[ind2]);
                while (true)
                {
                    if (MetaData.FileTable.FileTable[i].WeirdOffset == ROMFS_UNUSED_ENTRY)
                    {
                        MetaData.FileTable.FileTable[i].WeirdOffset = MetaData.FileTable.FileTable[index].Offset;
                        break;
                    }
                    i = GetRomfsFileEntry(MetaData, MetaData.FileTable.FileTable[i].WeirdOffset);
                }
            }
        }

        internal static uint CalcPathHash(uint ParentOffset, byte[] NameArray, int start, int len)
        {
            uint hash = ParentOffset ^ 123456789;
            for (int i = 0; i < NameArray.Length; i += 2)
            {
                hash = (hash >> 5) | (hash << 27);
                hash ^= (ushort)((NameArray[start + i]) | (NameArray[start + i + 1] << 8));
            }
            return hash;
        }

        internal static void AddDir(Romfs_MetaData MetaData, DirectoryInfo Dir, uint parent, uint sibling)
        {
            uint CurrentDir = MetaData.DirTableLen;
            Romfs_DirEntry Entry = new Romfs_DirEntry {ParentOffset = parent};
            Entry.ChildOffset = Entry.WeirdOffset = Entry.FileOffset = ROMFS_UNUSED_ENTRY;
            Entry.SiblingOffset = sibling;
            Entry.FullName = Dir.FullName;
            Entry.Name = (Entry.FullName == ROOT_DIR) ? "" : Dir.Name;
            Entry.Offset = CurrentDir;
            MetaData.DirTable.DirectoryTable.Add(Entry);
            MetaData.DirTableLen += (CurrentDir == 0) ? 0x18 : 0x18 + (uint)Align((ulong)Dir.Name.Length * 2, 4);
            DirectoryInfo[] SubDirectories = Dir.GetDirectories();
            int ParentIndex = GetRomfsDirEntry(MetaData, Dir.FullName);
            if (SubDirectories.Length > 0)
            {
                MetaData.DirTable.DirectoryTable[ParentIndex].ChildOffset = MetaData.DirTableLen;
            }
            for (int i = 0; i < SubDirectories.Length; i++)
            {
                AddDir(MetaData, SubDirectories[i], Entry.Offset, sibling);
                if (i <= 0) continue;
                string PrevFullName = SubDirectories[i - 1].FullName;
                string ThisName = SubDirectories[i].FullName;
                int PrevIndex = GetRomfsDirEntry(MetaData, PrevFullName);
                int ThisIndex = GetRomfsDirEntry(MetaData, ThisName);
                MetaData.DirTable.DirectoryTable[PrevIndex].SiblingOffset = MetaData.DirTable.DirectoryTable[ThisIndex].Offset;
            }
        }
        internal static void AddFiles(Romfs_MetaData MetaData, RomfsFile[] Entries)
        {
            string PrevDirPath = "";
            for (int i = 0; i < Entries.Length; i++)
            {
                FileInfo file = new FileInfo(Entries[i].FullName);
                Romfs_FileEntry Entry = new Romfs_FileEntry();
                string DirPath = Path.GetDirectoryName(Entries[i].FullName);
                int ParentIndex = GetRomfsDirEntry(MetaData, DirPath);
                Entry.FullName = Entries[i].FullName;
                Entry.Offset = MetaData.FileTableLen;
                Entry.ParentDirOffset = MetaData.DirTable.DirectoryTable[ParentIndex].Offset;
                Entry.SiblingOffset = ROMFS_UNUSED_ENTRY;
                if (DirPath == PrevDirPath)
                {
                    MetaData.FileTable.FileTable[i - 1].SiblingOffset = Entry.Offset;
                }
                if (MetaData.DirTable.DirectoryTable[ParentIndex].FileOffset == ROMFS_UNUSED_ENTRY)
                {
                    MetaData.DirTable.DirectoryTable[ParentIndex].FileOffset = Entry.Offset;
                }
                Entry.WeirdOffset = ROMFS_UNUSED_ENTRY;
                Entry.NameSize = (uint)file.Name.Length * 2;
                Entry.Name = file.Name;
                Entry.DataOffset = Entries[i].Offset;
                Entry.DataSize = Entries[i].Size;
                MetaData.FileTable.FileTable.Add(Entry);
                MetaData.FileTableLen += 0x20 + (uint)Align((ulong)file.Name.Length * 2, 4);
                PrevDirPath = DirPath;
            }
        }

        internal static void WriteMetaDataToStream(Romfs_MetaData MetaData, MemoryStream stream)
        {
            //First, InfoHeader.
            stream.Write(BitConverter.GetBytes(MetaData.InfoHeader.HeaderLength), 0, 4);
            foreach (Romfs_SectionHeader SH in MetaData.InfoHeader.Sections)
            {
                stream.Write(BitConverter.GetBytes(SH.Offset), 0, 4);
                stream.Write(BitConverter.GetBytes(SH.Size), 0, 4);
            }
            stream.Write(BitConverter.GetBytes(MetaData.InfoHeader.DataOffset), 0, 4);

            //DirUTable
            foreach (uint u in MetaData.DirUTable)
            {
                stream.Write(BitConverter.GetBytes(u), 0, 4);
            }

            //DirTable
            foreach (Romfs_DirEntry dir in MetaData.DirTable.DirectoryTable)
            {
                stream.Write(BitConverter.GetBytes(dir.ParentOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(dir.SiblingOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(dir.ChildOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(dir.FileOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(dir.WeirdOffset), 0, 4);
                uint nlen = (uint)dir.Name.Length * 2;
                stream.Write(BitConverter.GetBytes(nlen), 0, 4);
                byte[] NameArray = new byte[(int)Align(nlen, 4)];
                Array.Copy(Encoding.Unicode.GetBytes(dir.Name), 0, NameArray, 0, nlen);
                stream.Write(NameArray, 0, NameArray.Length);
            }

            //FileUTable
            foreach (uint u in MetaData.FileUTable)
            {
                stream.Write(BitConverter.GetBytes(u), 0, 4);
            }

            //FileTable
            foreach (Romfs_FileEntry file in MetaData.FileTable.FileTable)
            {
                stream.Write(BitConverter.GetBytes(file.ParentDirOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(file.SiblingOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(file.DataOffset), 0, 8);
                stream.Write(BitConverter.GetBytes(file.DataSize), 0, 8);
                stream.Write(BitConverter.GetBytes(file.WeirdOffset), 0, 4);
                uint nlen = (uint)file.Name.Length * 2;
                stream.Write(BitConverter.GetBytes(nlen), 0, 4);
                byte[] NameArray = new byte[(int)Align(nlen, 4)];
                Array.Copy(Encoding.Unicode.GetBytes(file.Name), 0, NameArray, 0, nlen);
                stream.Write(NameArray, 0, NameArray.Length);
            }
            //All Done.
        }

        //GetRomfs[...]Entry Functions are all O(n)

        internal static int GetRomfsDirEntry(Romfs_MetaData MetaData, string FullName)
        {
            for (int i = 0; i < MetaData.DirTable.DirectoryTable.Count; i++)
            {
                if (MetaData.DirTable.DirectoryTable[i].FullName == FullName)
                {
                    return i;
                }
            }
            return -1;
        }
        internal static int GetRomfsDirEntry(Romfs_MetaData MetaData, uint Offset)
        {
            for (int i = 0; i < MetaData.DirTable.DirectoryTable.Count; i++)
            {
                if (MetaData.DirTable.DirectoryTable[i].Offset == Offset)
                {
                    return i;
                }
            }
            return -1;
        }
        internal static int GetRomfsFileEntry(Romfs_MetaData MetaData, uint Offset)
        {
            for (int i = 0; i < MetaData.FileTable.FileTable.Count; i++)
            {
                if (MetaData.FileTable.FileTable[i].Offset == Offset)
                {
                    return i;
                }
            }
            return -1;
        }

        #region Support Class/Struct
        public class Romfs_MetaData
        {
            public Romfs_InfoHeader InfoHeader;
            public uint DirNum;
            public uint FileNum;
            public List<uint> DirUTable;
            public uint M_DirUTableEntry;
            public Romfs_DirTable DirTable;
            public uint DirTableLen;
            public uint M_DirTableLen;
            public List<uint> FileUTable;
            public uint M_FileUTableEntry;
            public Romfs_FileTable FileTable;
            public uint FileTableLen;
            public uint M_FileTableLen;
        }
        public struct Romfs_SectionHeader
        {
            public uint Offset;
            public uint Size;
        }
        public struct Romfs_InfoHeader
        {
            public uint HeaderLength;
            public Romfs_SectionHeader[] Sections;
            public uint DataOffset;
        }
        public class Romfs_DirTable
        {
            public List<Romfs_DirEntry> DirectoryTable;
        }
        public class Romfs_FileTable
        {
            public List<Romfs_FileEntry> FileTable;
        }
        public class Romfs_DirEntry
        {
            public uint ParentOffset;
            public uint SiblingOffset;
            public uint ChildOffset;
            public uint FileOffset;
            public uint WeirdOffset;
            public string Name;
            public string FullName;
            public uint Offset;
        }
        public class Romfs_FileEntry
        {
            public uint ParentDirOffset;
            public uint SiblingOffset;
            public ulong DataOffset;
            public ulong DataSize;
            public uint WeirdOffset;
            public uint NameSize;
            public string Name;
            public string FullName;
            public uint Offset;
        }

        public class RomfsFile
        {
            public string PathName;
            public ulong Offset;
            public ulong Size;
            public string FullName;

            public static ulong GetDataBlockLength(RomfsFile[] files, ulong PreData)
            {
                return (files.Length == 0) ? PreData : PreData + files[files.Length - 1].Offset + files[files.Length - 1].Size;
            }
        }
        public class IVFCInfo
        {
            public IVFCLevel[] Levels;
        }
        public class IVFCLevel
        {
            public ulong HashOffset;
            public ulong DataLength;
            public uint BlockSize;
        }
        public class FileNameTable
        {
            public List<FileInfo> NameEntryTable { get; private set; }

            public int NumFiles
            {
                get
                {
                    return NameEntryTable.Count;
                }
            }

            internal FileNameTable(string rootPath)
            {
                NameEntryTable = new List<FileInfo>();
                AddDirectory(new DirectoryInfo(rootPath));
            }

            internal void AddDirectory(DirectoryInfo dir)
            {
                foreach (FileInfo fileInfo in dir.GetFiles())
                {
                    NameEntryTable.Add(fileInfo);
                }
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    AddDirectory(subdir);
                }
            }
        }
        public class LayoutManager
        {
            public static Output[] Create(IEnumerable<Input> Input)
            {
                List<Output> list = new List<Output>();
                ulong Len = 0;
                foreach (Input input in Input)
                {
                    Output output = new Output();
                    FileInfo fileInfo = new FileInfo(input.FilePath);
                    ulong ofs = Align(Len, input.AlignmentSize);
                    output.FilePath = input.FilePath;
                    output.Offset = ofs;
                    output.Size = (ulong)fileInfo.Length;
                    list.Add(output);
                    Len = ofs + (ulong)fileInfo.Length;
                }
                return list.ToArray();
            }
            public static ulong Align(ulong input, ulong alignsize)
            {
                ulong output = input;
                if (output % alignsize != 0)
                    output += (alignsize - (output % alignsize));

                return output;
            }
            public class Input
            {
                public string FilePath;
                public uint AlignmentSize;
            }
            public class Output
            {
                public string FilePath;
                public ulong Offset;
                public ulong Size;
            }
        }
        #endregion
    }
}