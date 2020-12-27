using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS.Core.CTR
{
    public class NCCH
    {
        public NCCHHeader Header;
        public ExeFS ExeFS;
        public RomFS RomFS;
        public Exheader Exheader;
        public byte[] logo;
        public byte[] plainregion;
        public const uint MEDIA_UNIT_SIZE = 0x200;

        public class NCCHHeader
        {
            public byte[] Signature; //Size: 0x100
            public uint Magic;
            public uint Size;
            public ulong TitleId;
            public ushort MakerCode;
            public ushort FormatVersion;
            //public uint padding0;
            public ulong ProgramId;
            //public byte[0x10] padding1;
            public byte[] LogoHash; // Size: 0x20
            public byte[] ProductCode; // Size: 0x10
            public byte[] ExheaderHash; // Size: 0x20
            public uint ExheaderSize;
            //public uint padding2;
            public byte[] Flags; // Size: 8
            public uint PlainRegionOffset;
            public uint PlainRegionSize;
            public uint LogoOffset;
            public uint LogoSize;
            public uint ExefsOffset;
            public uint ExefsSize;
            public uint ExefsSuperBlockSize;
            //public uint padding4;
            public uint RomfsOffset;
            public uint RomfsSize;
            public uint RomfsSuperBlockSize;
            //public uint padding5;
            public byte[] ExefsHash; // Size: 0x20
            public byte[] RomfsHash; // Size: 0x20

            public byte[] Data;

            public void BuildHeader()
            {
                Data = new byte[0x200];
                Array.Copy(Signature, Data, 0x100);
                Array.Copy(BitConverter.GetBytes(Magic), 0, Data, 0x100, 4);
                Array.Copy(BitConverter.GetBytes(Size), 0, Data, 0x104, 4);
                Array.Copy(BitConverter.GetBytes(TitleId), 0, Data, 0x108, 8);
                Array.Copy(BitConverter.GetBytes(MakerCode), 0, Data, 0x110, 2);
                Array.Copy(BitConverter.GetBytes(FormatVersion), 0, Data, 0x112, 2);
                //4 Byte Padding
                Array.Copy(BitConverter.GetBytes(ProgramId), 0, Data, 0x118, 8);
                //0x10 Byte Padding
                Array.Copy(LogoHash, 0, Data, 0x130, 0x20);
                Array.Copy(ProductCode, 0, Data, 0x150, 0x10);
                Array.Copy(ExheaderHash, 0, Data, 0x160, 0x20);
                Array.Copy(BitConverter.GetBytes(ExheaderSize), 0, Data, 0x180, 4);
                //4 Byte Padding
                Array.Copy(Flags, 0, Data, 0x188, 0x8);
                uint ofs = 0x190;
                foreach (uint val in new uint[] { PlainRegionOffset, PlainRegionSize, LogoOffset, LogoSize, ExefsOffset, ExefsSize, ExefsSuperBlockSize, 0, RomfsOffset, RomfsSize, RomfsSuperBlockSize, 0 })
                {
                    Array.Copy(BitConverter.GetBytes(val), 0, Data, ofs, 4);
                    ofs += 4;
                }
                Array.Copy(ExefsHash, 0, Data, 0x1C0, 0x20);
                Array.Copy(RomfsHash, 0, Data, 0x1E0, 0x20);
            }

            public void BuildHeaderFromBytes(byte[] data)
            {
                Data = data;
                Signature = new byte[0x100];
                Array.Copy(data, Signature, 0x100);
                Magic = BitConverter.ToUInt32(data, 0x100);
                Size = BitConverter.ToUInt32(data, 0x104);
                TitleId = BitConverter.ToUInt64(data, 0x108);
                MakerCode = BitConverter.ToUInt16(data, 0x110);
                FormatVersion = BitConverter.ToUInt16(data, 0x112);
                //4 Byte Padding
                ProgramId = BitConverter.ToUInt64(data, 0x118);
                //0x10 Byte Padding
                LogoHash = new byte[0x20];
                Array.Copy(data, 0x130, LogoHash, 0x0, 0x20);
                ProductCode = new byte[0x10];
                Array.Copy(data, 0x150, ProductCode, 0x0, 0x10);
                ExheaderHash = new byte[0x20];
                Array.Copy(data, 0x160, ExheaderHash, 0x0, 0x20);
                ExheaderSize = BitConverter.ToUInt32(data, 0x180);
                //4 Byte Padding
                Flags = new byte[0x8];
                Array.Copy(data, 0x188, Flags, 0, 0x8);
                PlainRegionOffset = BitConverter.ToUInt32(data, 0x190);
                PlainRegionSize = BitConverter.ToUInt32(data, 0x194);
                LogoOffset = BitConverter.ToUInt32(data, 0x198);
                LogoSize = BitConverter.ToUInt32(data, 0x19C);
                ExefsOffset = BitConverter.ToUInt32(data, 0x1A0);
                ExefsSize = BitConverter.ToUInt32(data, 0x1A4);
                ExefsSuperBlockSize = BitConverter.ToUInt32(data, 0x1A8);
                //4 Byte Padding
                RomfsOffset = BitConverter.ToUInt32(data, 0x1B0);
                RomfsSize = BitConverter.ToUInt32(data, 0x1B4);
                RomfsSuperBlockSize = BitConverter.ToUInt32(data, 0x1B8);
                //4 Byte Padding
                ExefsHash = new byte[0x20];
                Array.Copy(data, 0x1C0, ExefsHash, 0x0, 0x20);
                RomfsHash = new byte[0x20];
                Array.Copy(data, 0x1E0, RomfsHash, 0x0, 0x20);
            }
        }

        public void ExtractNCCHFromFile(string NCCH_PATH, string outputDirectory, RichTextBox TB_Progress = null, ProgressBar PB_Show = null)
        {
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            byte[] headerBytes = new byte[0x200];
            using (FileStream fs = new FileStream(NCCH_PATH, FileMode.Open, FileAccess.Read))
            {
                fs.Read(headerBytes, 0, headerBytes.Length);
                Header = new NCCHHeader();
                Header.BuildHeaderFromBytes(headerBytes);

                logo = new byte[Header.LogoSize * MEDIA_UNIT_SIZE];
                fs.Seek(Convert.ToInt32(Header.LogoOffset * MEDIA_UNIT_SIZE), SeekOrigin.Begin);
                fs.Read(logo, 0, logo.Length);

                plainregion = new byte[Header.PlainRegionSize * MEDIA_UNIT_SIZE];
                fs.Seek(Convert.ToInt32(Header.PlainRegionOffset * MEDIA_UNIT_SIZE), SeekOrigin.Begin);
                fs.Read(plainregion, 0, plainregion.Length);
            }

            ExtractExheader(NCCH_PATH, outputDirectory, TB_Progress);
            ExtractExeFS(NCCH_PATH, outputDirectory, TB_Progress);
            ExtractRomFS(NCCH_PATH, outputDirectory, TB_Progress, PB_Show);
        }

        private void ExtractExheader(string NCCH_PATH, string outputDirectory, RichTextBox TB_Progress = null)
        {
            string exheaderpath = Path.Combine(outputDirectory, "exheader.bin");
            UpdateTB(TB_Progress, "Extracting exheader.bin from CXI...");
            byte[] exheaderbytes = new byte[Header.ExheaderSize * 2];

            using (FileStream fs = new FileStream(NCCH_PATH, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(Convert.ToInt32(0x200), SeekOrigin.Begin);
                fs.Read(exheaderbytes, 0, exheaderbytes.Length);
            }

            File.WriteAllBytes(exheaderpath, exheaderbytes);
            Exheader = new Exheader(exheaderpath);
        }

        private void ExtractExeFS(string NCCH_PATH, string outputDirectory, RichTextBox TB_Progress = null)
        {
            string exefsbinpath = Path.Combine(outputDirectory, "exefs.bin");
            string exefspath = Path.Combine(outputDirectory, "exefs");
            UpdateTB(TB_Progress, "Extracting exefs.bin from CXI...");
            byte[] exefsbytes = new byte[Header.ExefsSize * MEDIA_UNIT_SIZE];

            using (FileStream fs = new FileStream(NCCH_PATH, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(Convert.ToInt32(Header.ExefsOffset * MEDIA_UNIT_SIZE), SeekOrigin.Begin);
                fs.Read(exefsbytes, 0, exefsbytes.Length);
            }

            File.WriteAllBytes(exefsbinpath, exefsbytes);
            ExeFS.UnpackExeFS(exefsbinpath, exefspath);
            File.Delete(exefsbinpath);
        }

        private void ExtractRomFS(string NCCH_PATH, string outputDirectory, RichTextBox TB_Progress = null, ProgressBar PB_Show = null)
        {
            UpdateTB(TB_Progress, "Extracting romfs.bin from CXI...");
            string romfsbinpath = Path.Combine(outputDirectory, "romfs.bin");
            string romfspath = Path.Combine(outputDirectory, "romfs");
            byte[] romfsBytes = new byte[MEDIA_UNIT_SIZE];

            using (FileStream ncchstream = new(NCCH_PATH, FileMode.Open, FileAccess.Read),
                              romfsstream = new(romfsbinpath, FileMode.Append, FileAccess.Write))
            {
                ncchstream.Seek(Convert.ToInt32(Header.RomfsOffset * MEDIA_UNIT_SIZE), SeekOrigin.Begin);
                if (PB_Show.InvokeRequired)
                {
                    PB_Show.Invoke((MethodInvoker)delegate { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = Convert.ToInt32(Header.RomfsSize); });
                }
                else { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = Convert.ToInt32(Header.RomfsSize); }
                for (int i = 0; i < Header.RomfsSize; i++)
                {
                    ncchstream.Read(romfsBytes, 0, romfsBytes.Length);
                    romfsstream.Write(romfsBytes, 0, romfsBytes.Length);
                    if (PB_Show.InvokeRequired)
                    {
                        PB_Show.Invoke((MethodInvoker)PB_Show.PerformStep);
                    }
                    else { PB_Show.PerformStep(); }
                }
            }

            RomFS romfs = new RomFS(romfsbinpath);
            romfs.ExtractRomFS(romfspath, TB_Progress, PB_Show);
            File.Delete(romfsbinpath);
        }

        public void WriteHeaderToFile(string outputDirectory, RichTextBox TB_Progress)
        {
            UpdateTB(TB_Progress, "Extracting ncchheader.bin from CXI...");
            string headerParth = Path.Combine(outputDirectory, "ncchheader.bin");
            using FileStream headerStream = new FileStream(headerParth, FileMode.OpenOrCreate, FileAccess.Write);
            headerStream.Write(this.Header.Data, 0, this.Header.Data.Length);
        }

        public void WritePlainRegionAndLogo(string outputDirectory, RichTextBox TB_Progress)
        {
            string plainRegionPath = Path.Combine(outputDirectory, "plain.bin");
            string logoPath = Path.Combine(outputDirectory, "logo.bcma.lz");
            UpdateTB(TB_Progress, "Extracting plain.bin and logo.bcma.lz from CXI...");
            using FileStream plainStream = new(plainRegionPath, FileMode.OpenOrCreate, FileAccess.Write);
            using FileStream logoStream = new(logoPath, FileMode.OpenOrCreate, FileAccess.Write);
            plainStream.Write(this.plainregion, 0, this.plainregion.Length);
            logoStream.Write(this.logo, 0, this.logo.Length);
        }

        internal static void UpdateTB(RichTextBox RTB, string progress)
        {
            try
            {
                if (RTB.InvokeRequired)
                {
                    RTB.Invoke((MethodInvoker)delegate
                   {
                       RTB.AppendText(Environment.NewLine + progress);
                       RTB.SelectionStart = RTB.Text.Length;
                       RTB.ScrollToCaret();
                   });
                }
                else
                {
                    RTB.SelectionStart = RTB.Text.Length;
                    RTB.ScrollToCaret();
                    RTB.AppendText(progress + Environment.NewLine);
                }
            }
            catch { }
        }
    }
}
