using System;
using System.IO;
using System.Linq;

namespace pk3DS.Core.CTR
{
    #region GARC Class & Struct
    public static class GARC
    {
        public const ushort VER_6 = 0x0600;
        public const ushort VER_4 = 0x0400;

        public class FileCountDeterminedEventArgs : EventArgs
        {
            public int Total { get; set; }
        }

        public class PackProgressedEventArgs : EventArgs
        {
            public int Current { get; set; }
            public int Total { get; set; }
            public string CurrentFile { get; set; }
        }

        public class UnpackProgressedEventArgs : EventArgs
        {
            public int Current { get; set; }
            public int Total { get; set; }
        }


        public static event EventHandler<FileCountDeterminedEventArgs> FileCountDetermined;
        public static event EventHandler<PackProgressedEventArgs> PackProgressed;
        public static event EventHandler<UnpackProgressedEventArgs> UnpackProgressed;

        public static int garcPackMS(string folderPath, string garcPath, int version, int bytesPadding)
        {
            // Check to see if our input folder exists.
            if (!new DirectoryInfo(folderPath).Exists) throw new DirectoryNotFoundException("Folder does not exist");

            if (version != VER_4 && version != VER_6)
                throw new FormatException("Invalid GARC Version: 0x" + version.ToString("X4"));

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
            catch (Exception e) { throw new Exception("Invalid packing filenames", e); }

            FileCountDetermined?.Invoke(null, new FileCountDeterminedEventArgs { Total = filectr });
            #endregion            

            // Set Up the GARC template.
            GARCFile garc = new GARCFile
            {
                ContentPadToNearest = 4,
                fato =
                {
                    // Magic = new[] { 'O', 'T', 'A', 'F' },
                    Entries = new FATO_Entry[packOrder.Length],
                    EntryCount = (ushort) packOrder.Length,
                    HeaderSize = 0xC + packOrder.Length*4,
                    Padding = 0xFFFF
                },
                fatb =
                {
                    // Magic = new[] { 'B', 'T', 'A', 'F' },
                    Entries = new FATB_Entry[packOrder.Length],
                    FileCount = filectr
                }
            };
            if (version == VER_6)
            {
                // Some files have larger bytes-to-pad values (ex/ 0x80 for a109)
                // Hopefully there's no problems defining this with a constant number.
                garc.ContentPadToNearest = 4;
            }

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

                        // Step
                        PackProgressed?.Invoke(null, new PackProgressedEventArgs { Current = index++, Total = filectr, CurrentFile = packOrder[i] });
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

                            // Step
                            PackProgressed?.Invoke(null, new PackProgressedEventArgs { Current = index++, Total = filectr, CurrentFile = packOrder[i] });
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
            using (var newGARC = new FileStream(garcPath, FileMode.Create))
            using (BinaryWriter gw = new BinaryWriter(newGARC))
            {
                #region Write GARC Headers
                // Write GARC
                gw.Write((uint)0x47415243); // GARC
                gw.Write((uint)(version == VER_6 ? 0x24 : 0x1C)); // Header Length
                gw.Write((ushort)0xFEFF);   // Endianness BOM
                gw.Write((ushort)version);  // Version
                gw.Write((uint)0x00000004); // Section Count (4)
                gw.Write((uint)0x00000000); // Data Offset (temp)
                gw.Write((uint)0x00000000); // File Length (temp)
                gw.Write((uint)0x00000000); // Largest File Size (temp)

                if (version == VER_6)
                {
                    gw.Write((uint)0x0);
                    gw.Write((uint)0x0);
                }

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

                // Write FIMB
                gw.Write((uint)0x46494D42); // FIMB
                gw.Write((uint)0x0000000C); // Header Length
                var dataLen = gw.BaseStream.Position;
                gw.Write((uint)0); // Data Length - TEMP

                gw.Seek(0x10, SeekOrigin.Begin); // Goto the start of the un-set 0 data we set earlier and set it.
                var hdrLen = gw.BaseStream.Position;
                gw.Write((uint)0); // Write Data Offset - TEMP
                gw.Write((uint)0); // Write total GARC Length - TEMP

                // Write Handling information
                if (version == VER_4)
                {
                    gw.Write(garc.ContentLargestUnpadded); // Write Largest File stat
                }
                else if (version == VER_6)
                {
                    gw.Write(garc.ContentLargestPadded); // Write Largest With Padding
                    gw.Write(garc.ContentLargestUnpadded); // Write Largest Without Padding
                    gw.Write(garc.ContentPadToNearest);
                }

                newGARC.Seek(0, SeekOrigin.End); // Goto the end so we can copy the filedata after the GARC headers.

                #endregion

                #region Write Files
                var ghLength = gw.BaseStream.Length;

                long largestSize = 0; // Required memory to allocate to handle the largest file
                long largestPadded = 0; // Required memory to allocate to handle the largest PADDED file (Ver6 only)
                foreach (string e in packOrder)
                {
                    string[] fa = Directory.Exists(e) ? Directory.GetFiles(e) : new[] { e };
                    foreach (string f in fa)
                    {
                        // Update largest file length if necessary
                        long len = new FileInfo(f).Length;
                        int padding = (int)(len % bytesPadding);
                        if (padding != 0) padding = bytesPadding - padding;
                        bool largest = len > largestSize;
                        if (largest)
                        {
                            largestSize = len;
                            largestPadded = len + padding;
                        }

                        // Write to FIMB
                        using (var x = File.OpenRead(f))
                            x.CopyTo(newGARC);

                        // While length is not divisible by 4, pad with FF (unused byte)
                        while (padding-- > 0)
                            gw.Write((byte)0xFF);
                    }
                }
                garc.ContentLargestUnpadded = (uint)largestSize;
                garc.ContentLargestPadded = (uint)largestPadded;
                var gdLength = gw.BaseStream.Length - ghLength;
                #endregion

                gw.Seek((int)dataLen, SeekOrigin.Begin);
                gw.Write((uint)gdLength); // Data Length
                gw.Seek((int)hdrLen, SeekOrigin.Begin);
                gw.Write((uint)ghLength); // Write Data Offset
                gw.Write((uint)gw.BaseStream.Length); // Write total GARC Length

                // Write Handling information
                if (version == VER_4)
                {
                    gw.Write(garc.ContentLargestUnpadded);              // Write Largest File stat
                }
                else if (version == VER_6)
                {
                    gw.Write(garc.ContentLargestPadded);                // Write Largest With Padding
                    gw.Write(garc.ContentLargestUnpadded);              // Write Largest Without Padding
                    gw.Write(garc.ContentPadToNearest);
                }                             

                return filectr;
            }
        }
        public static int garcUnpack(string garcPath, string outPath, bool skipDecompression)
        {
            if (!File.Exists(garcPath)) throw new FileNotFoundException("File does not exist");

            // Unpack the GARC
            GARCFile garc = unpackGARC(garcPath);
            const string ext = "bin"; // Default Extension Name
            int fileCount = garc.fatb.FileCount;
            string format = "D" + Math.Ceiling(Math.Log10(fileCount));
            if (outPath == "gametext")
                format = "D3";

            FileCountDetermined?.Invoke(null, new FileCountDeterminedEventArgs { Total = fileCount });

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
                                catch (Exception e) { throw new Exception("A compressed file could not be deleted: " + fileOut, e); }
                            }
                            catch
                            {
                                // File is really not encrypted.
                                File.Delete(decout);
                            }
                        }
                        #endregion

                        UnpackProgressed?.Invoke(null, new UnpackProgressedEventArgs { Current = filectr, Total = fileCount });

                        if ((vector >>= 1) == 0) break;
                    }
                }
            }
            return fileCount;
        }

        public static GARCFile unpackGARC(string path)
        {
            return unpackGARC(File.OpenRead(path));
        }
        private static GARCFile unpackGARC(byte[] data)
        {
            GARCFile garc;
            using (var gd = new MemoryStream(data))
                garc = unpackGARC(gd);
            return garc;
        }
        private static GARCFile unpackGARC(Stream stream)
        {
            GARCFile garc = new GARCFile();
            using (BinaryReader br = new BinaryReader(stream))
            {
                // GARC Header
                garc.Magic = br.ReadChars(4);
                garc.HeaderSize = br.ReadUInt32();
                garc.Endianess = br.ReadUInt16();
                garc.Version = br.ReadUInt16();
                garc.ChunkCount = br.ReadUInt32();

                garc.DataOffset = br.ReadUInt32();
                garc.FileSize = br.ReadUInt32();
                if (garc.Version == VER_4)
                {
                    garc.ContentLargestUnpadded = br.ReadUInt32();
                    garc.ContentPadToNearest = 4;
                }
                else if (garc.Version == VER_6)
                {
                    garc.ContentLargestPadded = br.ReadUInt32();
                    garc.ContentLargestUnpadded = br.ReadUInt32();
                    garc.ContentPadToNearest = br.ReadUInt32();
                }
                else
                    throw new FormatException("Invalid GARC Version: 0x" + garc.Version.ToString("X4"));
                if (garc.ChunkCount != 4)
                    throw new FormatException("Invalid GARC Chunk Count: " + garc.ChunkCount);

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

        public static MemGARC packGARC(byte[][] data, int version, int contentpadnearest)
        {
            if (contentpadnearest < 0)
                contentpadnearest = 4;
            // Set Up the GARC template.
            GARCFile garc = new GARCFile
            {
                ContentPadToNearest = (uint)contentpadnearest,
                fato =
                {
                    // Magic = new[] { 'O', 'T', 'A', 'F' },
                    Entries = new FATO_Entry[data.Length],
                    EntryCount = (ushort) data.Length,
                    HeaderSize = 0xC + data.Length*4,
                    Padding = 0xFFFF
                },
                fatb =
                {
                    // Magic = new[] { 'B', 'T', 'A', 'F' },
                    Entries = new FATB_Entry[data.Length],
                    FileCount = data.Length
                }
            };

            if (version == VER_6)
                garc.ContentPadToNearest = 4;

            int op = 0;
            int od = 0;
            for (int i = 0; i < garc.fatb.Entries.Length; i++)
            {
                garc.fato.Entries[i].Offset = op; // FATO offset
                garc.fatb.Entries[i].SubEntries = new FATB_SubEntry[32];
                op += 4; // Vector
                garc.fatb.Entries[i].IsFolder = false;
                garc.fatb.Entries[i].SubEntries[0].Exists = true;

                // Assemble Entry
                var paddingRequired = data[i].Length%garc.ContentPadToNearest;
                if (paddingRequired != 0) paddingRequired = garc.ContentPadToNearest - paddingRequired;
                int actualLength = data[i].Length + (int)paddingRequired;
                garc.fatb.Entries[i].SubEntries[0].Start = od;
                garc.fatb.Entries[i].SubEntries[0].End = actualLength + garc.fatb.Entries[i].SubEntries[0].Start;
                garc.fatb.Entries[i].SubEntries[0].Length = data[i].Length;
                garc.fatb.Entries[i].SubEntries[0].Padding = (int)paddingRequired;
                od += actualLength;

                op += 12;
                garc.fatb.Entries[i].Vector = 1;
            }
            garc.fatb.HeaderSize = 0xC + op;

            // Set up the Header Info
            string tempFile = Path.GetTempFileName();
            using (var newGARC = new FileStream(tempFile, FileMode.Create))
            using (BinaryWriter gw = new BinaryWriter(newGARC))
            {
                #region Write GARC Headers

                // Write GARC
                gw.Write((uint) 0x47415243); // GARC
                gw.Write((uint) (version == VER_6 ? 0x24 : 0x1C)); // Header Length
                gw.Write((ushort) 0xFEFF); // Endianness BOM
                gw.Write((ushort) version); // Version
                gw.Write((uint) 0x00000004); // Section Count (4)
                gw.Write((uint) 0x00000000); // Data Offset (temp)
                gw.Write((uint) 0x00000000); // File Length (temp)
                gw.Write((uint) 0x00000000); // Largest File Size (temp)

                if (version == VER_6)
                {
                    gw.Write((uint) 0x0);
                    gw.Write((uint) 0x0);
                }

                // Write FATO
                gw.Write((uint) 0x4641544F); // FATO
                gw.Write(garc.fato.HeaderSize); // Header Size 
                gw.Write(garc.fato.EntryCount); // Entry Count
                gw.Write(garc.fato.Padding); // Padding
                for (int i = 0; i < garc.fato.Entries.Length; i++)
                    gw.Write((uint) garc.fato.Entries[i].Offset);

                // Write FATB
                gw.Write((uint) 0x46415442); // FATB
                gw.Write(garc.fatb.HeaderSize); // Header Size
                gw.Write(garc.fatb.FileCount); // File Count
                foreach (var e in garc.fatb.Entries)
                {
                    gw.Write(e.Vector);
                    foreach (var s in e.SubEntries.Where(s => s.Exists))
                    {
                        gw.Write((uint) s.Start);
                        gw.Write((uint) s.End);
                        gw.Write((uint) s.Length);
                    }
                }

                gw.Write((uint) 0x46494D42); // FIMB
                gw.Write((uint) 0x0000000C); // Header Length
                var dataLen = gw.BaseStream.Position;
                gw.Write((uint) 0); // Data Length - TEMP

                gw.Seek(0x10, SeekOrigin.Begin); // Goto the start of the un-set 0 data we set earlier and set it.
                var hdrLen = gw.BaseStream.Position;
                gw.Write((uint) 0); // Write Data Offset - TEMP
                gw.Write((uint) 0); // Write total GARC Length - TEMP

                // Write Handling information
                if (version == VER_4)
                {
                    gw.Write(garc.ContentLargestUnpadded); // Write Largest File stat
                }
                else if (version == VER_6)
                {
                    gw.Write(garc.ContentLargestPadded); // Write Largest With Padding
                    gw.Write(garc.ContentLargestUnpadded); // Write Largest Without Padding
                    gw.Write(garc.ContentPadToNearest);
                }

                newGARC.Seek(0, SeekOrigin.End); // Goto the end so we can copy the filedata after the GARC headers.

                #endregion

                #region Write Files

                var ghLength = gw.BaseStream.Length;

                long largestSize = 0; // Required memory to allocate to handle the largest file
                long largestPadded = 0; // Required memory to allocate to handle the largest PADDED file (Ver6 only)
                for (int i = 0; i < data.Length; i++)
                {
                    byte[] e = data[i];

                    // Update largest file length if necessary
                    int len = e.Length;
                    int padding = garc.fatb.Entries[i].SubEntries[0].Padding;
                    bool largest = len > largestSize;
                    if (largest)
                    {
                        largestSize = len;
                        largestPadded = len + padding;
                    }

                    // Write to FIMB
                    gw.Write(e);

                    // Pad with FF (unused byte)
                    while (padding-- > 0)
                        gw.Write((byte)0xFF);
                }
                garc.ContentLargestUnpadded = (uint)largestSize;
                garc.ContentLargestPadded = (uint)largestPadded;
                var gdLength = gw.BaseStream.Length - ghLength;

                #endregion

                gw.Seek((int)dataLen, SeekOrigin.Begin);
                gw.Write((uint)gdLength); // Data Length
                gw.Seek((int)hdrLen, SeekOrigin.Begin);
                gw.Write((uint)ghLength); // Write Data Offset
                gw.Write((uint)gw.BaseStream.Length); // Write total GARC Length

                if (version == VER_4)
                {
                    gw.Write(garc.ContentLargestUnpadded); // Write Largest File stat
                }
                else if (version == VER_6)
                {
                    gw.Write(garc.ContentLargestPadded); // Write Largest With Padding
                    gw.Write(garc.ContentLargestUnpadded); // Write Largest Without Padding
                    gw.Write(garc.ContentPadToNearest);
                }
            }

            byte[] GARCdata = File.ReadAllBytes(tempFile);
            File.Delete(tempFile);
            return new MemGARC(GARCdata);
        }

        public class MemGARC
        {
            internal GARCFile garc;
            internal byte[] Data;
            public int FileCount => garc.fato.EntryCount;

            public MemGARC(byte[] data)
            {
                Data = data;
                garc = unpackGARC(data);
            }

            // Returns an individual file
            public byte[] getFile(int file, int subfile = 0)
            {
                var Entry = garc.fatb.Entries[file];
                var SubEntry = Entry.SubEntries[subfile];
                if (!SubEntry.Exists)
                    throw new ArgumentException("SubFile does not exist.");
                var offset = SubEntry.Start + garc.DataOffset;
                byte[] data = new byte[SubEntry.Length];
                Array.Copy(Data, offset, data, 0, data.Length);
                return data;
            }

            // Returns all files (excluding language vectorized)
            public byte[][] Files
            {
                get
                {
                    byte[][] data = new byte[FileCount][];
                    for (int i = 0; i < data.Length; i++)
                        data[i] = getFile(i);
                    return data;
                }
                set
                {
                    if (value == null || value.Length != FileCount)
                        throw new ArgumentException();

                    var ng = packGARC(value, garc.Version, (int)garc.ContentPadToNearest);
                    garc = ng.garc;
                    Data = ng.Data;
                }
            }
        }

        /// <summary>
        /// GARC Class that is heavier on OOP to allow for compression tracking for faster edits
        /// </summary>
        public class lzGARC
        {
            private GARCFile garc;
            private byte[] Data;
            public int FileCount => garc.fato.EntryCount;

            public lzGARC(byte[] data)
            {
                Data = data;
                garc = unpackGARC(data);
                Storage = new GARCEntry[FileCount];
            }

            private readonly GARCEntry[] Storage;
            private class GARCEntry
            {
                public bool Accessed;
                public bool Saved;
                public byte[] Data;
                public readonly bool WasCompressed;

                public GARCEntry() { }
                public GARCEntry(byte[] data)
                {
                    Data = data;
                    Accessed = true;

                    if (data.Length == 0)
                        return;

                    if (data[0] != 0x11)
                        return;

                    try
                    {
                        using (MemoryStream newMS = new MemoryStream())
                        {
                            LZSS.Decompress(new MemoryStream(data), data.Length, newMS);
                            Data = newMS.ToArray();
                        }
                        WasCompressed = true;
                    }
                    catch { }
                }

                public byte[] Save()
                {
                    if (!WasCompressed)
                        return Data;

                    byte[] data;
                    try
                    {
                        using (MemoryStream newMS = new MemoryStream())
                        {
                            LZSS.Compress(new MemoryStream(Data), Data.Length, newMS, original: true);
                            data = newMS.ToArray();
                        }
                    }
                    catch { data = new byte[0]; }
                    return data;
                }
            }

            private byte[] getFile(int file, int subfile = 0)
            {
                var Entry = garc.fatb.Entries[file];
                var SubEntry = Entry.SubEntries[subfile];
                if (!SubEntry.Exists)
                    throw new ArgumentException("SubFile does not exist.");
                var offset = SubEntry.Start + garc.DataOffset;
                byte[] data = new byte[SubEntry.Length];
                Array.Copy(Data, offset, data, 0, data.Length);
                return data;
            }
            public byte[] this[int file]
            {
                get
                {
                    if (file > FileCount)
                        throw new ArgumentException();

                    if (Storage[file] == null)
                        Storage[file] = new GARCEntry(getFile(file, 0));
                    return Storage[file].Data;
                }
                set
                {
                    if (Storage[file] == null)
                        Storage[file] = new GARCEntry(value);
                    Storage[file].Data = value;
                    Storage[file].Saved = true;
                }
            }
            public byte[] Save()
            {
                byte[][] data = new byte[FileCount][];
                for (int i = 0; i < data.Length; i++)
                {
                    if (Storage[i] == null || !Storage[i].Saved) // retrieve original
                        data[i] = getFile(i, 0);
                    else // use modified
                        data[i] = Storage[i].Save();
                }

                var ng = packGARC(data, garc.Version, (int)garc.ContentPadToNearest);
                garc = ng.garc;
                Data = ng.Data;
                return Data;
            }
        }

        public struct GARCFile
        {
            public char[] Magic; // Always GARC = 0x4E415243
            public uint HeaderSize; // Always 0x001C
            public ushort Endianess; // 0xFFFE
            public ushort Version; // 4: gen6, 6: gen7
            public uint ChunkCount; // Always 0x0400 chunk count

            public uint DataOffset;
            public uint FileSize;
            
            public uint ContentLargestPadded;   // Format 6 Only
            public uint ContentLargestUnpadded;
            public uint ContentPadToNearest;    // Format 6 Only (4 bytes is standard in VER_4, and is not stored)

            public FATO fato;
            public FATB fatb;
            public FIMG fimg;
        }

        public struct FATO
        {
            public char[] Magic;
            public int HeaderSize;
            public ushort EntryCount;
            public ushort Padding;

            public FATO_Entry[] Entries;
        }
        public struct FATO_Entry
        {
            public int Offset;
        }

        public struct FATB
        {
            public char[] Magic;
            public int HeaderSize;
            public int FileCount;

            public FATB_Entry[] Entries;
        }
        public struct FATB_Entry
        {
            public uint Vector;
            public bool IsFolder;
            public FATB_SubEntry[] SubEntries;
        }
        public struct FATB_SubEntry
        {
            public bool Exists;
            public int Start;
            public int End;
            public int Length;

            // Unsaved Properties
            public int Padding { get; set; }
        }

        public struct FIMG
        {
            public char[] Magic;
            public int HeaderSize;
            public int DataSize;
        }
    }
    #endregion
}