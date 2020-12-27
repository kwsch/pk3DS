using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using pk3DS.Core.CTR;

namespace pk3DS.ARCUtil
{
    public static class ARC
    {
        public static SARC AnalyzeSARC(string path)
        {
            try { return new SARC(path); }
            catch { return new SARC(); }
        }

        internal static ShuffleARC AnalyzeShuffle(string path)
        {
            ShuffleARC sharc = new ShuffleARC
            {
                FileName = Path.GetFileNameWithoutExtension(path),
                FilePath = Path.GetDirectoryName(path),
                Extension = Path.GetExtension(path)
            };
            using var br = new BinaryReader(File.OpenRead(path));
            if (br.ReadUInt32() != 0xB)
            {
                br.BaseStream.Seek(0x100, SeekOrigin.Begin);
                if (br.ReadUInt32() != 0xB)
                {
                    sharc.valid = false;
                    return sharc;
                }
                sharc.add100 = true;
            }
            uint magic = br.ReadUInt32();
            if (magic.ToString("X8") != sharc.FileName)
            {
                Console.WriteLine("Sharc mismatch - " + magic.ToString("X8") + "," + sharc.FileName);
                sharc.valid = false;
                return sharc;
            }
            sharc.valid = true;
            br.ReadUInt32();
            br.ReadUInt32();
            sharc.FileCount = br.ReadUInt32();
            br.ReadUInt32();
            sharc.Files = new List<ShuffleFile>();
            for (int i = 0; i < sharc.FileCount; i++)
            {
                br.BaseStream.Seek(0x8, SeekOrigin.Current);
                ShuffleFile sf = new ShuffleFile
                {
                    Length = br.ReadUInt32(),
                    Offset = br.ReadUInt32() + (uint)(sharc.add100 ? 0x100 : 0)
                };
                br.BaseStream.Seek(0x10, SeekOrigin.Current);
                sharc.Files.Add(sf);
            }
            return sharc;
        }

        internal static GAR AnalyzeGAR(string path)
        {
            GAR gar = new GAR
            {
                FileName = Path.GetFileNameWithoutExtension(path),
                FilePath = Path.GetDirectoryName(path),
                Extension = Path.GetExtension(path)
            };
            using var br = new BinaryReader(File.OpenRead(path));
            long len = br.BaseStream.Length;
            gar.Magic = br.ReadUInt32();
            gar.FileLength = br.ReadUInt32();
            if (gar.Magic != 0x02524147 || gar.FileLength != len)
            {
                gar.valid = false;
                return gar;
            }
            gar.valid = true;
            gar.Unknown = br.ReadUInt32();
            gar.HeaderLength = br.ReadUInt32();
            gar.FileMetaOffset = br.ReadUInt32();
            gar.FileOffsetsOffset = br.ReadUInt32();
            br.BaseStream.Seek(0x1C, SeekOrigin.Current);
            gar.FileCountOffset = br.ReadUInt32();
            gar.CTXBOffset = br.ReadUInt32();
            br.BaseStream.Seek(gar.FileOffsetsOffset, SeekOrigin.Begin);
            gar.DataOffset = br.ReadUInt32();
            gar.FileCount = (gar.DataOffset - gar.FileOffsetsOffset) / 4;
            br.BaseStream.Seek(gar.FileMetaOffset, SeekOrigin.Begin);
            gar.Files = new List<GARFile>();
            for (int i = 0; i < gar.FileCount; i++)
            {
                GARFile gf = new GARFile
                {
                    Length = br.ReadUInt32(),
                    NOffset = br.ReadUInt32(),
                    NWEOffset = br.ReadUInt32()
                };
                gar.Files.Add(gf);
            }
            for (int i = 0; i < gar.FileCount; i++)
            {
                br.BaseStream.Seek(gar.Files[i].NOffset, SeekOrigin.Begin);
                StringBuilder sb = new StringBuilder();
                for (char c = br.ReadChar(); c != (char)0; c = br.ReadChar())
                {
                    sb.Append(c);
                }
                gar.Files[i].Name = sb.ToString();
                br.BaseStream.Seek(gar.Files[i].NWEOffset, SeekOrigin.Begin);
                sb = new StringBuilder();
                for (char c = br.ReadChar(); c != (char)0; c = br.ReadChar())
                {
                    sb.Append(c);
                }
                gar.Files[i].NameWithExtension = sb.ToString();
            }
            br.BaseStream.Seek(gar.FileOffsetsOffset, SeekOrigin.Begin);
            if (gar.Files.Count > 0)
            {
                gar.Files[0].Offset = gar.DataOffset;
                br.ReadUInt32();
            }
            for (int i = 1; i < gar.FileCount; i++)
            {
                gar.Files[i].Offset = br.ReadUInt32();
            }
            return gar;
        }

        internal static DARC Analyze(string path)
        {
            DARC darc = new DARC
            {
                FileName = Path.GetFileNameWithoutExtension(path),
                FilePath = Path.GetDirectoryName(path),
                Extension = Path.GetExtension(path)
            };
            using BinaryReader br = new BinaryReader(File.OpenRead(path));
            long len = br.BaseStream.Length;
            darc.Magic = br.ReadUInt32();
            uint m = darc.Magic;
            darc.HeaderOffset = 0;
            while (m != 0x63726164 && darc.HeaderOffset < len - 4)
            {
                m = br.ReadUInt32();
                darc.HeaderOffset += 4;
            }
            if (darc.HeaderOffset >= len - 4)
            {
                darc.valid = false;
                return darc;
            }
            darc.Magic = m;
            darc.valid = true;
            darc.BOM = br.ReadUInt16();
            darc.HeaderLength = br.ReadUInt16();
            darc.Unknown = br.ReadUInt32();
            darc.totalLength = br.ReadUInt32();
            darc.TableOffset = br.ReadUInt32(); //from start of file
            darc.TableOffset += darc.HeaderOffset;
            darc.TableLength = br.ReadUInt32();
            darc.DataOffset = br.ReadUInt32();
            FileTable ft = new FileTable();
            br.BaseStream.Seek(darc.TableOffset + 8, SeekOrigin.Begin);
            int count = br.ReadByte();
            ft.Files = new List<DarcFile>();
            ft.FileNames = new List<string>();
            br.BaseStream.Seek(darc.TableOffset, SeekOrigin.Begin);
            for (int i = 0; i < count; i++)
            {
                DarcFile file = new DarcFile
                {
                    NameOffset = br.ReadUInt16(),
                    Parent = br.ReadByte(),
                    Folder = br.ReadByte(),
                    Offset = br.ReadUInt32() + darc.HeaderOffset,
                    Length = br.ReadUInt32()
                };
                DarcFile f2 = file;
                ft.Files.Add(f2);
            }

            uint NameTableOffset = (uint)br.BaseStream.Position;
            foreach (var t in ft.Files)
            {
                br.BaseStream.Seek(NameTableOffset + t.NameOffset, SeekOrigin.Begin);
                var stream = new MemoryStream();
                for (byte fb = br.ReadByte(), sb = br.ReadByte();
                    fb != 0 || sb != 0;
                    fb = br.ReadByte(), sb = br.ReadByte())
                {
                    stream.WriteByte(fb);
                    stream.WriteByte(sb);
                }
                ft.FileNames.Add(Encoding.Unicode.GetString(stream.ToArray()));
                stream.Close();
            }
            darc.Files = ft;
            darc.FileName = Path.GetFileNameWithoutExtension(path);
            darc.FilePath = Path.GetDirectoryName(path);
            darc.Extension = Path.GetExtension(path);
            return darc;
        }

        internal static FARC AnalyzeFARC(string path)
        {
            FARC farc = new FARC
            {
                FileName = Path.GetFileNameWithoutExtension(path),
                FilePath = Path.GetDirectoryName(path),
                Extension = Path.GetExtension(path)
            };
            BinaryReader br = new BinaryReader(File.OpenRead(path));
            long len = br.BaseStream.Length;
            farc.Magic = br.ReadUInt32();
            uint m = farc.Magic;
            farc.HeaderOffset = 0;
            while (m != 0x43524146 && farc.HeaderOffset < len - 4) //FARC
            {
                m = br.ReadUInt32();
                farc.HeaderOffset += 4;
            }
            if (farc.HeaderOffset >= len - 4)
            {
                farc.valid = false;
                return farc;
            }
            farc.Magic = m;
            farc.valid = true;
            br.BaseStream.Seek(farc.HeaderOffset + 0x24, SeekOrigin.Begin);
            farc.SirOffset = br.ReadUInt32() + farc.HeaderOffset;
            br.ReadUInt32(); //unk
            farc.DataOffset = br.ReadUInt32() + farc.HeaderOffset;
            br.BaseStream.Seek(farc.SirOffset, SeekOrigin.Begin);
            farc.SirMagic = br.ReadUInt32();
            if (farc.SirMagic != 0x30524953)
            {
                farc.valid = false;
                return farc;
            }
            farc.MetaPointer = farc.SirOffset + br.ReadUInt32();
            br.BaseStream.Seek(farc.MetaPointer, SeekOrigin.Begin);
            farc.TableOffset = farc.SirOffset + br.ReadUInt32();
            farc.FileCount = br.ReadUInt32();
            br.BaseStream.Seek(farc.TableOffset, SeekOrigin.Begin);
            FARCFileTable ft = new FARCFileTable
            {
                Files = new List<FARCFile>(),
                FileNames = new List<string>()
            };
            for (int i = 0; i < farc.FileCount; i++)
            {
                FARCFile file = new FARCFile
                {
                    NameOffset = br.ReadUInt32(),
                    Offset = br.ReadUInt32(),
                    Length = br.ReadUInt32()
                };
                br.ReadUInt32(); //align to 0x10
                FARCFile f2 = file;
                ft.Files.Add(f2);
            }
            for (int i = 0; i < farc.FileCount; i++)
            {
                br.BaseStream.Seek(ft.Files[i].NameOffset + farc.SirOffset, SeekOrigin.Begin);
                MemoryStream stream = new MemoryStream();
                int firstByte = 1, secondByte = 1;
                while (firstByte != 0 || secondByte != 0)
                {
                    firstByte = br.ReadByte();
                    secondByte = br.ReadByte();
                    stream.WriteByte((byte) firstByte);
                    stream.WriteByte((byte) secondByte);
                }
                ft.FileNames.Add(Encoding.Unicode.GetString(stream.ToArray()));
                stream.Close();
            }
            farc.Files = ft;
            farc.FileName = Path.GetFileNameWithoutExtension(path);
            farc.FilePath = Path.GetDirectoryName(path);
            farc.Extension = Path.GetExtension(path);
            return farc;
        }

        internal static string Interpret(string path)
        {
            string fn = Path.GetFileName(path);
            if (fn == "save0.bin" || fn == "save1.bin" || fn == "save2.bin")
            {
                return FixMajoraChecksum(path);
            }
            if (fn.StartsWith("message") && (fn.EndsWith("_US.bin") || fn.EndsWith("_UK.bin")))
            {
                return ParseShuffleText(path);
            }
            DARC darc = Analyze(path);
            FARC farc = new FARC();
            GAR gar = new GAR();
            SARC sarc = new SARC();
            ShuffleARC sharc = new ShuffleARC();
            if (!darc.valid) farc = AnalyzeFARC(path);
            if (!farc.valid) gar = AnalyzeGAR(path);
            if (!gar.valid) sharc = AnalyzeShuffle(path);
            if (!sharc.valid) sarc = AnalyzeSARC(path);

            string ret = "";
            if (darc.valid)
            {
                ret += "Header Offset: " + darc.HeaderOffset + Environment.NewLine + "File Count: " + darc.Files.Files.Count + Environment.NewLine;
                int extracted = 0;
                int folder = 0;
                for (int i = 0; i < darc.Files.Files.Count; i++)
                {
                    if (darc.Files.Files[i].Folder > 0) { folder++; }
                    else
                    {
                        extracted++;
                        string dir = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + darc.FileName + Path.DirectorySeparatorChar;
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        string outPath = dir + darc.Files.FileNames[i];
                        var fs = File.OpenRead(path);
                        fs.Seek(darc.Files.Files[i].Offset, SeekOrigin.Begin);
                        byte[] fileBuffer = new byte[darc.Files.Files[i].Length];
                        fs.Read(fileBuffer, 0, fileBuffer.Length);
                        fs.Close();
                        File.WriteAllBytes(outPath, fileBuffer);
                    }
                }
                ret += "Extracted " + extracted + " files";
                if (folder > 0)
                {
                    ret += ", did not extract " + folder + " folders";
                }
                ret += "." + Environment.NewLine + "Open a .DARC/.SARC/.FARC/.GAR/Shuffle Archive file (or drag/drop).";
            }
            else if (farc.valid)
            {
                ret += "Header Offset: " + farc.HeaderOffset + Environment.NewLine;
                int extracted = 0;
                for (int i = 0; i < farc.Files.Files.Count; i++)
                {
                    extracted++;
                    string dir = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + "FARC_" + farc.FileName + Path.DirectorySeparatorChar;
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string outPath = dir + farc.Files.FileNames[i];
                    var fs = File.OpenRead(farc.FilePath + "\\" + farc.FileName + farc.Extension);
                    fs.Seek(farc.Files.Files[i].Offset + farc.DataOffset, SeekOrigin.Begin);
                    byte[] fileBuffer = new byte[farc.Files.Files[i].Length];
                    fs.Read(fileBuffer, 0, fileBuffer.Length);
                    fs.Close();
                    File.WriteAllBytes(outPath, fileBuffer);
                }
                ret += "Extracted " + extracted + " files";
                ret += "." + Environment.NewLine + ".DARC/.FARC/.SARC/.GAR/Shuffle Archive file (or drag/drop).";
            }
            else if (gar.valid)
            {
                ret += "New GAR with " + gar.FileCount + " files." + Environment.NewLine;
                string dir = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + gar.FileName + Path.DirectorySeparatorChar;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                for (int i = 0; i < gar.FileCount; i++)
                {
                    var fs = File.OpenRead(path);
                    fs.Seek(gar.Files[i].Offset, SeekOrigin.Begin);
                    byte[] fileBuffer = new byte[gar.Files[i].Length];
                    fs.Read(fileBuffer, 0, fileBuffer.Length);
                    fs.Close();
                    File.WriteAllBytes(dir + gar.Files[i].NameWithExtension, fileBuffer);
                    ret += "Extracted " + gar.Files[i].NameWithExtension + " (Offset: " + gar.Files[i].Offset.ToString("X8") + ", Len: " + gar.Files[i].Length.ToString("X8") + ")." + Environment.NewLine;
                }
                ret += Environment.NewLine;
            }
            else if (sharc.valid)
            {
                UnpackShuffleARC(path, sharc);
            }
            else if (sarc.Valid)
            {
                UnpackSARC(sarc);
            }
            else
            {
                ret = "Not a valid .DARC/.FARC/.SARC/.GAR/Shuffle Archive file";
            }
            return ret;
        }

        private static bool UnpackShuffleARC(string path, ShuffleARC sharc)
        {
            Debug.WriteLine($"New Shuffle Archive with {sharc.FileCount} files.");
            string dir = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + sharc.FileName + "_" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Making dir: " + dir);
                Directory.CreateDirectory(dir);
            }

            string diglen = "".PadLeft((int)(Math.Log10(sharc.FileCount) + 1), '0');
            for (int i = 0; i < sharc.FileCount; i++)
            {
                var fs = File.OpenRead(path);
                fs.Seek(sharc.Files[i].Offset, SeekOrigin.Begin);
                byte[] fileBuffer = new byte[sharc.Files[i].Length];
                fs.Read(fileBuffer, 0, fileBuffer.Length);
                fs.Close();
                uint check = 0;
                for (int j = 0; j < fileBuffer.Length; j += 4)
                    check += BitConverter.ToUInt32(fileBuffer, j);
                Console.WriteLine(i.ToString(diglen) + ": " + check.ToString("X8"));
                File.WriteAllBytes(dir + i.ToString(diglen) + ".zip", fileBuffer);
                Debug.WriteLine($"Extracted {i.ToString(diglen)} (Offset: {sharc.Files[i].Offset:X8}, Len: {sharc.Files[i].Length:X8}).");
            }
            return true;
        }

        public static bool UnpackSARC(SARC sarc)
        {
            Debug.WriteLine($"New SARC with {sarc.SFAT.EntryCount} files.");

            string outfolder = $"{sarc.FileName}_sarc";
            string outpath = Path.Combine(sarc.FilePath, outfolder);
            Directory.CreateDirectory(outpath);

            foreach (SFATEntry t in sarc.SFAT.Entries)
                sarc.ExportFile(t, outpath);

            return true;
        }

        // Unpacking
        internal static string UnpackDARC(string path, string outFolder = null, bool delete = true)
        {
            int extracted = 0;
            int folder = 0;
            DARC darc = Analyze(path);
            if (!darc.valid) return "Not a DARC?";

            for (int i = 0; i < darc.Files.Files.Count; i++)
            {
                if (darc.Files.Files[i].Folder > 0)
                {
                    folder++;
                }
                else
                {
                    extracted++;
                    string dir = outFolder ?? Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + darc.FileName + Path.DirectorySeparatorChar;
                    if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

                    using var fs = File.OpenRead(path);
                    fs.Seek(darc.Files.Files[i].Offset, SeekOrigin.Begin);
                    byte[] fileBuffer = new byte[darc.Files.Files[i].Length];
                    fs.Read(fileBuffer, 0, fileBuffer.Length);
                    File.WriteAllBytes(Path.Combine(dir, darc.Files.FileNames[i]), fileBuffer);
                }
            }
            if (delete)
                File.Delete(path); // File is unpacked.

            // Debug info string:
            string s = "";
            s += "Header Offset: " + darc.HeaderOffset + Environment.NewLine;
            s += "File Count: " + darc.Files.Files.Count + Environment.NewLine;
            s += "Extracted " + extracted + " files";
            s += folder > 0 ? ", did not extract " + folder + " folders." : ".";
            return s;
        }

        internal static void RepackDARC(string path, string fileName, string outfolder = null, bool header = true)
        {
            var data = Array.Empty<byte>();
            string[] files = Directory.GetFiles(path);
            int count = files.Length;

            outfolder ??= Directory.GetParent(path).FullName;
            string donor = Path.Combine(outfolder, fileName);

            if (header && File.Exists(donor))
            {
                data = data.Concat(BitConverter.GetBytes(count)).ToArray();
                foreach (string file in files)
                {
                    // File Names are 0x40 characters
                    string fn = new FileInfo(file).Name;
                    byte[] bytes = Encoding.ASCII.GetBytes(fn);
                    Array.Resize(ref bytes, 0x40);
                    data = data.Concat(bytes).ToArray();
                }

                // Check the original file
                byte[] donorBytes = File.ReadAllBytes(donor);
                if (data.SequenceEqual(donorBytes.Take(data.Length)))
                {
                    int headerLen = data.Length + (BitConverter.ToInt32(donorBytes, data.Length)*0x20);
                    headerLen += 0x80 - (headerLen % 0x80);
                    data = donorBytes.Take(headerLen).ToArray();
                }
                else
                {
                    data = Array.Empty<byte>();
                }
            }
            if (data.Length == 0)
            {
                //var dr = Util.Prompt(MessageBoxButtons.YesNoCancel, "Donor DARC has no header. Export without header?");
                //if (dr != DialogResult.Yes) 
                return;
            }
            WinFormsUtil.Alert("Not finished.");
        }

        // Generic Utility
        internal static string FixMajoraChecksum(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            Array.Copy(BitConverter.GetBytes((ushort)0), 0, data, 0x1A88, 2);
            CRC16 crc = new CRC16();
            ushort val = crc.ComputeChecksum(data);
            val ^= 0x903B;
            Array.Copy(BitConverter.GetBytes(val), 0, data, 0x1A88, 2);
            File.WriteAllBytes(path, data);
            return "Corrected Majora Checksum to " + val.ToString("X4");
        }

        internal static string ParseShuffleText(string path)
        {
            ShuffleText st = new ShuffleText
            {
                FileName = Path.GetFileNameWithoutExtension(path),
                FilePath = Path.GetDirectoryName(path),
                Extension = Path.GetExtension(path)
            };
            Console.WriteLine(st.FilePath);
            BinaryReader br = new BinaryReader(File.OpenRead(path));
            br.BaseStream.Seek(0xC, SeekOrigin.Begin);
            uint StringDataLen = br.ReadUInt32();
            st.StringMetaOffset = br.ReadUInt32();
            st.StringMetaLen = br.ReadUInt32();
            st.StringCount = st.StringMetaLen / 4;
            Console.WriteLine(st.StringCount);
            br.BaseStream.Seek(st.StringMetaOffset, SeekOrigin.Begin);
            st.offsets = new List<uint>();
            for (int i = 0; i < st.StringCount; i++)
            {
                st.offsets.Add(br.ReadUInt32());
            }
            string ret = "Dumped Offsets.";
            st.strings = new List<string>();
            for (int i = 0; i < st.StringCount; i++)
            {
                br.BaseStream.Seek(st.offsets[i], SeekOrigin.Begin);
                uint len = i < st.StringCount - 1
                    ? st.offsets[i + 1] - st.offsets[i]
                    : StringDataLen + 0x40 - st.offsets[i];
                byte[] data = br.ReadBytes((int)len);
                st.strings.Add(Encoding.Unicode.GetString(data).Replace((char)0, ' ').Replace((char)0xa, ' '));
            }
            ret += Environment.NewLine + "Dumped Strings.";
            StringBuilder sb = new StringBuilder();
            foreach (string t in st.strings)
                sb.AppendLine(t);

            string newfilename = st.FilePath + Path.DirectorySeparatorChar + st.FileName + ".txt";
            File.WriteAllText(newfilename, sb.ToString());
            return ret;
        }
    }

    public class FARC
    {
        public uint Magic;
        public uint SirMagic;
        public uint SirOffset;
        public uint HeaderOffset;
        public uint MetaPointer; //from start of file
        public uint NamesOffset;
        public uint TableOffset; //from start of file
        public uint DataOffset; //from start of file
        public uint FileCount;
        public FARCFileTable Files;

        public string FileName;
        public string FilePath;
        public string Extension;
        public bool valid;
    }

    public class FARCFileTable
    {
        public List<FARCFile> Files;
        public List<string> FileNames;
    }

    public class FARCFile
    {
        public uint NameOffset;
        public uint Offset;
        public uint Length;

        public string Name;
    }

    public class ShuffleARC
    {
        public uint magic; //0xB
        public uint FileNameCheck;
        public uint unk;
        public uint unk2;
        public uint FileCount;
        public uint padding;
        public List<ShuffleFile> Files;

        public string FileName;
        public string FilePath;
        public string Extension;
        public bool add100;
        public bool valid;
    }

    public class ShuffleFile
    {
        public uint Offset;
        public uint Length;
    }

    public class ShuffleText
    {
        public uint StringMetaOffset;
        public uint StringMetaLen;
        public uint StringCount;
        public List<uint> offsets;
        public List<string> strings;

        public string FileName;
        public string FilePath;
        public string Extension;
        public bool valid;
    }

    public class GAR
    {
        public uint Magic; //0x02524146 "GAR"
        public uint FileLength;
        public uint Unknown;
        public uint HeaderLength;
        public uint FileMetaOffset;
        public uint FileOffsetsOffset;
        public uint FileCountOffset;
        public uint CTXBOffset; //Filecount = (CTXBOffset-FileCountOffset)/4;
        public uint DataOffset;

        public uint FileCount;
        public List<GARFile> Files;

        public string FileName;
        public string FilePath;
        public string Extension;
        public bool valid;
    }

    public class GARFile
    {
        public string NameWithExtension;
        public string Name;
        public uint NOffset;
        public uint NWEOffset;
        public uint Offset;
        public uint Length;
    }

    public class DARC
    {
        public uint HeaderOffset; // Where is header in file?

        public uint Magic; // 0x64617263 "darc"
        public ushort BOM; // 0xFFFE
        public ushort HeaderLength; // HeaderLength - 0x1C
        public uint Unknown; // 0x10000000
        public uint totalLength; // Total Length of file
        public uint TableOffset; // Offset from Start of File
        public uint TableLength; // Table Length
        public uint DataOffset; // Data Offset

        public FileTable Files;

        public string FileName;
        public string FilePath;
        public string Extension;
        public bool valid;
    }

    public class FileTable
    {
        public List<DarcFile> Files;
        public List<string> FileNames;
    }

    public class DarcFile
    {
        public ushort NameOffset; //
        public byte Parent;
        public byte Folder;
        public uint Offset;
        public uint Length;

        public string Name;
    }

    public class CRC16
    {
        private const ushort polynomial = 0xA001;
        private readonly ushort[] table = new ushort[256];

        public ushort ComputeChecksum(byte[] bytes)
        {
            return bytes.Aggregate<byte, ushort>(0, (current, t) => (ushort) ((current >> 8) ^ table[current ^ t]));
        }

        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            ushort crc = ComputeChecksum(bytes);
            return BitConverter.GetBytes(crc);
        }

        public CRC16()
        {
            for (ushort i = 0; i < table.Length; ++i)
            {
                ushort value = 0;
                ushort temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                        value = (ushort) ((value >> 1) ^ polynomial);
                    else
                        value >>= 1;

                    temp >>= 1;
                }
                table[i] = value;
            }
        }
    }
}
