using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using pk3DS;

namespace CTR
{
    #region GARC Class & Struct
    public class GARC
    {
        internal static bool garcPackMS(string folderPath, string garcPath, ProgressBar pBar1 = null, Label label = null, bool supress = false)
        {
            // Check to see if our input folder exists.
            if (!new DirectoryInfo(folderPath).Exists) { Util.Error("Folder does not exist."); return false; }

            // Okay some basic proofing is done. Proceed.
            int filectr = 0;
            // Get the paths of the files to pack up. 
            string[] files = Directory.GetFiles(folderPath);
            string[] folders = Directory.GetDirectories(folderPath, "*.*", SearchOption.TopDirectoryOnly);

            string[] packOrder = new string[files.Length + folders.Length];
            #region Reassemble a list of filenames.
            try
            {
                foreach (string f in files)
                {
                    string fn = Path.GetFileNameWithoutExtension(f);
                    int compressed = fn.IndexOf("dec_", StringComparison.Ordinal);
                    int fileNumber = compressed < 0
                        ? int.Parse(fn)
                        : int.Parse(fn.Substring(compressed + 4));

                    packOrder[fileNumber] = f;
                    filectr++;
                }
                foreach (string f in folders)
                {
                    packOrder[int.Parse(new DirectoryInfo(f).Name)] = f;
                    filectr += Directory.GetFiles(f).Length;
                }
            }
            catch (Exception e) { Util.Error("Invalid packing filenames", e.ToString()); return false; }
            #endregion

            #region Initialize Progress Update Display
            if (pBar1 == null) pBar1 = new ProgressBar();
            if (pBar1.InvokeRequired)
                pBar1.Invoke((MethodInvoker)delegate { pBar1.Minimum = 0; pBar1.Step = 1; pBar1.Value = 0; pBar1.Maximum = filectr; });
            else { pBar1.Minimum = 0; pBar1.Step = 1; pBar1.Value = 0; pBar1.Maximum = filectr; }
            if (label == null) label = new Label();
            if (label.InvokeRequired)
                label.Invoke((MethodInvoker)delegate { label.Visible = true; });
            #endregion

            // Set Up the GARC template.
            GARCFile garc = new GARCFile
            {
                fato =
                {
                    // Magic = new[] { 'O', 'T', 'A', 'F' },
                    Entries = new FATO_Entry[packOrder.Length],
                    EntryCount = (ushort)packOrder.Length,
                    HeaderSize = 0xC + packOrder.Length * 4,
                    Padding = 0xFFFF
                },
                fatb =
                {
                    // Magic = new[] { 'B', 'T', 'A', 'F' },
                    Entries = new FATB_Entry[packOrder.Length],
                    FileCount = filectr
                }
            };

            #region Start Reassembling the FAT* tables.
            {
                int op = 0;
                int od = 0;
                int v = 0;
                int index = 0;
                for (int i = 0; i < garc.fatb.Entries.Length; i++)
                {
                    garc.fato.Entries[i].Offset = op; // FATO offset
                    garc.fatb.Entries[i].SubEntries = new FATB_SubEntry[32];
                    op += 4; // Vector
                    if (!Directory.Exists(packOrder[i])) // is not folder
                    {
                        garc.fatb.Entries[i].IsFolder = false;
                        garc.fatb.Entries[i].SubEntries[0].Exists = true;

                        string fn = Path.GetFileNameWithoutExtension(packOrder[i]);
                        int compressed = fn.IndexOf("dec_", StringComparison.Ordinal);
                        int fileNumber = compressed < 0
                            ? int.Parse(fn)
                            : int.Parse(fn.Substring(compressed + 4));

                        if (compressed >= 0)
                        {
                            string old = packOrder[i];
                            LZSS.Compress(packOrder[i], packOrder[i] = Path.Combine(Path.GetDirectoryName(packOrder[i]), fileNumber.ToString()));
                            File.Delete(old);
                        }

                        // Assemble Vector
                        v = 1;

                        // Assemble Entry
                        FileInfo fi = new FileInfo(packOrder[i]);
                        int actualLength = (int)(fi.Length % 4 == 0 ? fi.Length : fi.Length + 4 - fi.Length % 4);
                        garc.fatb.Entries[i].SubEntries[0].Start = od;
                        garc.fatb.Entries[i].SubEntries[0].End = actualLength + garc.fatb.Entries[i].SubEntries[0].Start;
                        garc.fatb.Entries[i].SubEntries[0].Length = (int)fi.Length;
                        od += actualLength;

                        op += 12;
                        #region Step
                        if (pBar1.InvokeRequired)
                            pBar1.Invoke((MethodInvoker)(() => pBar1.PerformStep()));
                        else { pBar1.PerformStep(); }
                        string update = $"{(float) index/(float) filectr:P2} - {index}/{filectr} - {packOrder[i]}";
                        index++;
                        if (label.InvokeRequired)
                            label.Invoke((MethodInvoker)delegate { label.Text = update; });
                        else { label.Text = update; }
                        #endregion
                    }
                    else
                    {
                        garc.fatb.Entries[i].IsFolder = true;
                        string[] subFiles = Directory.GetFiles(packOrder[i]);
                        foreach (string f in subFiles)
                        {
                            string s = f;
                            string fn = Path.GetFileNameWithoutExtension(f);
                            int compressed = fn.IndexOf("dec_", StringComparison.Ordinal);
                            int fileNumber = compressed < 0
                                ? int.Parse(fn)
                                : int.Parse(fn.Substring(compressed + 4));
                            garc.fatb.Entries[i].SubEntries[fileNumber].Exists = true;

                            if (compressed >= 0)
                            {
                                LZSS.Compress(f, s = Path.Combine(Path.GetDirectoryName(f), fileNumber.ToString()));
                                File.Delete(f);
                            }

                            // Assemble Vector
                            v |= 1 << fileNumber;

                            // Assemble Entry
                            FileInfo fi = new FileInfo(s);
                            int actualLength = (int)(fi.Length % 4 == 0 ? fi.Length : fi.Length + 4 - fi.Length % 4);
                            garc.fatb.Entries[i].SubEntries[fileNumber].Start = od;
                            garc.fatb.Entries[i].SubEntries[fileNumber].End = actualLength + garc.fatb.Entries[i].SubEntries[fileNumber].Start;
                            garc.fatb.Entries[i].SubEntries[fileNumber].Length = (int)fi.Length;
                            od += actualLength;

                            op += 12;
                            #region Step
                            if (pBar1.InvokeRequired)
                                pBar1.Invoke((MethodInvoker)(() => pBar1.PerformStep()));
                            else { pBar1.PerformStep(); }
                            string update = $"{(float) index/(float) filectr:P2} - {index}/{filectr} - {f}";
                            index++;
                            if (label.InvokeRequired)
                                label.Invoke((MethodInvoker)delegate { label.Text = update; });
                            else { label.Text = update; }
                            #endregion
                        }
                    }
                    garc.fatb.Entries[i].Vector = (uint)v;
                }
                garc.fatb.HeaderSize = 0xC + op;
            }
            #endregion

            // Delete the old garc if it exists, then begin writing our new one 
            try { File.Delete(garcPath); }
            catch { }

            // Set up the Header Info
            using (MemoryStream newGARC = new MemoryStream())
            using (MemoryStream GARCdata = new MemoryStream())
            using (BinaryWriter gw = new BinaryWriter(newGARC))
            {
                #region Write GARC Headers
                // Write GARC
                gw.Write((uint)0x47415243); // GARC
                gw.Write((uint)0x0000001C); // Header Length
                gw.Write((ushort)0xFEFF);   // Endianness BOM
                gw.Write((ushort)0x0400);   // Const (4)
                gw.Write((uint)0x00000004); // Section Count (4)
                gw.Write((uint)0x00000000); // Data Offset (temp)
                gw.Write((uint)0x00000000); // File Length (temp)
                gw.Write((uint)0x00000000); // Largest File Size (temp)

                // Write FATO
                gw.Write((uint)0x4641544F);     // FATO
                gw.Write(garc.fato.HeaderSize); // Header Size 
                gw.Write(garc.fato.EntryCount); // Entry Count
                gw.Write(garc.fato.Padding);    // Padding
                for (int i = 0; i < garc.fato.Entries.Length; i++)
                    gw.Write((uint)garc.fato.Entries[i].Offset);

                // Write FATB
                gw.Write((uint)0x46415442);     // FATB
                gw.Write(garc.fatb.HeaderSize); // Header Size
                gw.Write(garc.fatb.FileCount);  // File Count
                foreach (var e in garc.fatb.Entries)
                {
                    gw.Write(e.Vector);
                    foreach (var s in e.SubEntries.Where(s => s.Exists))
                    { gw.Write((uint)s.Start); gw.Write((uint)s.End); gw.Write((uint)s.Length); }
                }
                #endregion

                #region Write Files
                long largestSize = 0; // Required memory to allocate to handle the largest file
                foreach (string e in packOrder)
                {
                    string[] fa = Directory.Exists(e) ? Directory.GetFiles(e) : new[] { e };
                    foreach (string f in fa)
                    {
                        // Update largest file length if necessary
                        long len = new FileInfo(f).Length;
                        if (len > largestSize) largestSize = len;

                        // Write to FIMB
                        using (var input = File.OpenRead(f))
                            input.CopyTo(GARCdata);

                        // While length is not divisible by 4, pad with FF (unused byte)
                        while (GARCdata.Length % 4 > 0) GARCdata.WriteByte(0xFF);
                    }
                }
                #endregion

                gw.Write((uint)0x46494D42);      // FIMB
                gw.Write((uint)0x0000000C);      // Header Length
                gw.Write((uint)GARCdata.Length); // Data Length

                gw.Seek(0x10, SeekOrigin.Begin);                        // Goto the start of the un-set 0 data we set earlier and set it.
                gw.Write((uint)newGARC.Length);                         // Write Data Offset
                gw.Write((uint)(newGARC.Length + GARCdata.Length));     // Write total GARC Length
                gw.Write((uint)largestSize);                            // Write Largest File stat (?)

                newGARC.Seek(0, SeekOrigin.End);    // Goto the end so we can copy the filedata after the GARC headers.

                // Write in the data
                GARCdata.Position = 0;
                GARCdata.CopyTo(newGARC);       // Copy the data.
                // New File is ready to be saved (memstream newGARC)
                try
                {
                    File.WriteAllBytes(garcPath, newGARC.ToArray());
                    if (label.InvokeRequired)
                        label.Invoke((MethodInvoker)delegate { label.Visible = false; });
                    else { label.Visible = false; }

                    // We're done.
                    SystemSounds.Exclamation.Play();
                    if (!supress) Util.Alert("Pack Successful!", filectr + " files packed to the GARC!");

                    return true;
                }
                catch (Exception e) { Util.Error("Packing Failed!", e.ToString()); return false; }
            }
        }
        internal static bool garcUnpack(string garcPath, string outPath, bool skipDecompression, ProgressBar pBar1 = null, Label label = null, bool supress = false, bool bypassExt = false)
        {
            if (!File.Exists(garcPath) && !supress) { Util.Alert("File does not exist"); return false; }

            // Unpack the GARC
            GARCFile garc = unpackGARC(garcPath);
            const string ext = "bin"; // Default Extension Name
            int fileCount = garc.fatb.FileCount;
            string format = "D" + Math.Ceiling(Math.Log10(fileCount));

            #region Display
            // Initialize ProgressBar
            if (pBar1 == null) pBar1 = new ProgressBar();
            if (pBar1.InvokeRequired)
                pBar1.Invoke((MethodInvoker)delegate { pBar1.Minimum = 0; pBar1.Step = 1; pBar1.Value = 0; pBar1.Maximum = fileCount; });
            else { pBar1.Minimum = 0; pBar1.Step = 1; pBar1.Value = 0; pBar1.Maximum = garc.fatb.FileCount; }

            if (label == null) label = new Label();
            if (label.InvokeRequired)
                label.Invoke((MethodInvoker)delegate { label.Visible = true; });
            #endregion

            using (BinaryReader br = new BinaryReader(File.OpenRead(garcPath)))
            {
                // Create Extraction folder if it does not exist.
                if (!Directory.Exists(outPath))
                    Directory.CreateDirectory(outPath);

                int filectr = 0;
                // Pull out all the files
                for (int o = 0; o < garc.fato.EntryCount; o++)
                {

                    var Entry = garc.fatb.Entries[o];
                    // Set Entry File Name
                    string fileName = o.ToString(format);

                    #region OutDirectory Determination
                    string parentFolder = Entry.IsFolder ? Path.Combine(outPath, fileName) : outPath;
                    if (Entry.IsFolder) // Process Folder
                        Directory.CreateDirectory(parentFolder);
                    #endregion

                    uint vector = Entry.Vector;
                    for (int i = 0; i < 32; i++) // For each bit in vector
                    {
                        var SubEntry = Entry.SubEntries[i];
                        if (!SubEntry.Exists) continue;

                        // Seek to Offset
                        br.BaseStream.Position = SubEntry.Start + garc.DataOffset;

                        // Check if Compressed
                        bool compressed = false;
                        if (!skipDecompression)
                            try { compressed = (byte)br.PeekChar() == 0x11; }
                            catch { }

                        // Write File
                        string fileOut = Path.Combine(parentFolder, (Entry.IsFolder ? i.ToString("00") : fileName) + "." + ext);
                        using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileOut)))
                        {
                            // Write out the data for the file
                            br.BaseStream.Position = SubEntry.Start + garc.DataOffset;
                            bw.Write(br.ReadBytes(SubEntry.Length));
                            filectr++;
                        }
                        if (compressed)
                        #region Decompression
                        {
                            string decout = Path.Combine(Path.GetDirectoryName(fileOut), "dec_" + Path.GetFileName(fileOut));
                            try
                            {
                                LZSS.Decompress(fileOut, decout);
                                try { File.Delete(fileOut); }
                                catch (Exception e) { Util.Error("A compressed file could not be deleted.", fileOut, e.ToString()); }
                            }
                            catch
                            {
                                // File is really not encrypted.
                                try { File.Delete(decout); }
                                catch (Exception e) { Util.Error("This shouldn't happen", e.ToString()); }
                            }
                        }
                        #endregion

                        #region Step
                        if (pBar1.InvokeRequired) pBar1.Invoke((MethodInvoker)(() => pBar1.PerformStep()));
                        else pBar1.PerformStep();

                        string update = $"{filectr/fileCount:P2} - {filectr}/{fileCount}";
                        if (label.InvokeRequired)
                            label.Invoke((MethodInvoker)delegate { label.Text = update; });
                        else { label.Text = update; }
                        #endregion

                        if ((vector >>= 1) == 0) break;
                    }
                }
            }
            #region Updates
            if (label.InvokeRequired)
                label.Invoke((MethodInvoker)delegate { label.Visible = false; });
            else { label.Visible = false; }
            SystemSounds.Exclamation.Play();
            if (!supress) Util.Alert("Unpack Successful!", fileCount + " files unpacked from the GARC!");
            #endregion
            return true;
        }

        internal static GARCFile unpackGARC(string path)
        {
            GARCFile garc = new GARCFile();
            using (BinaryReader br = new BinaryReader(File.OpenRead(path)))
            {
                // GARC Header
                garc.Magic = br.ReadChars(4);
                garc.HeaderSize = br.ReadUInt32();
                garc.Endianess = br.ReadUInt16();
                garc.ChunkCount = br.ReadUInt16();
                garc.FileSize = br.ReadUInt32();

                garc.DataOffset = br.ReadUInt32();
                garc.FileSize = br.ReadUInt32();
                garc.LastSize = br.ReadUInt32();

                // FATO (File Allocation Table Offsets)
                garc.fato.Magic = br.ReadChars(4);
                garc.fato.HeaderSize = br.ReadInt32();
                garc.fato.EntryCount = br.ReadUInt16();
                garc.fato.Padding = br.ReadUInt16();

                garc.fato.Entries = new FATO_Entry[garc.fato.EntryCount];
                for (int i = 0; i < garc.fato.EntryCount; i++)
                    garc.fato.Entries[i].Offset = br.ReadInt32();

                // FATB (File Allocation Table Bits)
                garc.fatb.Magic = br.ReadChars(4);
                garc.fatb.HeaderSize = br.ReadInt32();
                garc.fatb.FileCount = br.ReadInt32();

                garc.fatb.Entries = new FATB_Entry[garc.fato.EntryCount];
                for (int i = 0; i < garc.fato.EntryCount; i++) // Loop through all FATO entries
                {
                    garc.fatb.Entries[i].Vector = br.ReadUInt32();
                    garc.fatb.Entries[i].SubEntries = new FATB_SubEntry[32];
                    uint bitvector = garc.fatb.Entries[i].Vector;
                    int ctr = 0;
                    for (int b = 0; b < 32; b++)
                    {
                        garc.fatb.Entries[i].SubEntries[b].Exists = (bitvector & 1) == 1;
                        bitvector >>= 1;
                        if (!garc.fatb.Entries[i].SubEntries[b].Exists) continue;
                        garc.fatb.Entries[i].SubEntries[b].Start = br.ReadInt32();
                        garc.fatb.Entries[i].SubEntries[b].End = br.ReadInt32();
                        garc.fatb.Entries[i].SubEntries[b].Length = br.ReadInt32();
                        ctr++;
                    }
                    garc.fatb.Entries[i].IsFolder = ctr > 1;
                }

                // FIMB (File IMage Bytes)
                garc.fimg.Magic = br.ReadChars(4);
                garc.fimg.HeaderSize = br.ReadInt32();
                garc.fimg.DataSize = br.ReadInt32();

                // Files data
                // Oftentimes too large to toss into a byte array. Fetch as needed with a BinaryReader.
            }
            return garc;
        }

        public struct GARCFile
        {
            public Char[] Magic; // Always GARC = 0x4E415243
            public uint HeaderSize; // Always 0x001C
            public UInt16 Endianess; // 0xFFFE
            public UInt16 ChunkCount; // Always 0x0400 chunk count

            public uint DataOffset;
            public uint FileSize;
            public uint LastSize;

            public FATO fato;
            public FATB fatb;
            public FIMG fimg;
        }

        public struct FATO
        {
            public Char[] Magic;
            public int HeaderSize;
            public UInt16 EntryCount;
            public UInt16 Padding;

            public FATO_Entry[] Entries;
        }
        public struct FATO_Entry
        {
            public int Offset;
        }

        public struct FATB
        {
            public Char[] Magic;
            public int HeaderSize;
            public int FileCount;

            public FATB_Entry[] Entries;
        }
        public struct FATB_Entry
        {
            public uint Vector;
            public Boolean IsFolder;
            public FATB_SubEntry[] SubEntries;
        }
        public struct FATB_SubEntry
        {
            public Boolean Exists;
            public int Start;
            public int End;
            public int Length;
        }

        public struct FIMG
        {
            public Char[] Magic;
            public int HeaderSize;
            public int DataSize;
        }
    }
    #endregion
}