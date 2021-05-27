using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using pk3DS.Core.Properties;

namespace pk3DS.Core.CTR
{
    public static class CTRUtil
    {
        internal const uint MEDIA_UNIT_SIZE = 0x200;

        // Main wrapper that assembles the ROM based on the following specifications:
        public static bool BuildROM(bool Card2, string LOGO_NAME,
            string EXEFS_PATH, string ROMFS_PATH, string EXHEADER_PATH,
            string SERIAL_TEXT, string SAVE_PATH,
            bool trimmed = false, ProgressBar PB_Show = null, RichTextBox TB_Progress = null)
        {
            PB_Show ??= new ProgressBar();
            TB_Progress ??= new RichTextBox();

            // Sanity check the input files.
            if (!
                ((File.Exists(EXEFS_PATH) || Directory.Exists(EXEFS_PATH))
                && (File.Exists(ROMFS_PATH) || Directory.Exists(ROMFS_PATH))
                && File.Exists(EXHEADER_PATH)))
            {
                return false;
            }

            // If ExeFS and RomFS are not built, build.
            if (!File.Exists(EXEFS_PATH) && Directory.Exists(EXEFS_PATH))
                ExeFS.PackExeFS(Directory.GetFiles(EXEFS_PATH), EXEFS_PATH = "exefs.bin");
            if (!File.Exists(ROMFS_PATH) && Directory.Exists(ROMFS_PATH))
                RomFS.BuildRomFS(ROMFS_PATH, ROMFS_PATH = "romfs.bin", TB_Progress, PB_Show);

            NCCH NCCH = SetNCCH(EXEFS_PATH, ROMFS_PATH, EXHEADER_PATH, SERIAL_TEXT, LOGO_NAME, TB_Progress);
            NCSD NCSD = SetNCSD(NCCH, Card2, TB_Progress);
            bool success = WriteROM(NCSD, SAVE_PATH, trimmed, PB_Show, TB_Progress);
            return success;
        }

        // Sub methods that drive the operation
        internal static NCCH SetNCCH(string EXEFS_PATH, string ROMFS_PATH, string EXHEADER_PATH, string TB_Serial, string LOGO_NAME, RichTextBox TB_Progress = null)
        {
            TB_Progress ??= new RichTextBox();

            UpdateTB(TB_Progress, "Creating NCCH...");
            UpdateTB(TB_Progress, "Adding Exheader...");
            NCCH NCCH = new NCCH
            {
                Exheader = new Exheader(EXHEADER_PATH),
                plainregion = Array.Empty<byte>()
            };
            if (NCCH.Exheader.IsSupported())
            {
                UpdateTB(TB_Progress, "Detected Pokemon Game. Adding Plain Region...");
                if (NCCH.Exheader.IsXY())
                    NCCH.plainregion = Resources.XY;
                else if (NCCH.Exheader.IsORAS())
                    NCCH.plainregion = Resources.ORAS;
                else if (NCCH.Exheader.IsSM())
                    NCCH.plainregion = Resources.SuMo;
                else if (NCCH.Exheader.IsUSUM())
                    NCCH.plainregion = Resources.USUM;
            }
            UpdateTB(TB_Progress, "Adding ExeFS...");
            NCCH.ExeFS = new ExeFS(EXEFS_PATH);
            UpdateTB(TB_Progress, "Adding RomFS...");
            NCCH.RomFS = new RomFS(ROMFS_PATH);

            UpdateTB(TB_Progress, "Adding Logo...");
            NCCH.logo = (byte[])Resources.ResourceManager.GetObject(LOGO_NAME);
            UpdateTB(TB_Progress, "Assembling NCCH Header...");
            ulong Len = 0x200; //NCCH Signature + NCCH Header
            NCCH.Header = new NCCH.NCCHHeader { Signature = new byte[0x100], Magic = 0x4843434E };
            NCCH.Header.TitleId = NCCH.Header.ProgramId = NCCH.Exheader.TitleID;
            NCCH.Header.MakerCode = 0x3130; //01
            NCCH.Header.FormatVersion = 0x2; //Default
            NCCH.Header.LogoHash = new SHA256Managed().ComputeHash(NCCH.logo);
            NCCH.Header.ProductCode = Encoding.ASCII.GetBytes(TB_Serial);
            Array.Resize(ref NCCH.Header.ProductCode, 0x10);
            NCCH.Header.ExheaderHash = NCCH.Exheader.GetSuperBlockHash();
            NCCH.Header.ExheaderSize = (uint)NCCH.Exheader.Data.Length;
            Len += NCCH.Header.ExheaderSize + (uint)NCCH.Exheader.AccessDescriptor.Length;
            NCCH.Header.Flags = new byte[0x8];
            //FLAGS
            NCCH.Header.Flags[3] = 0; // Crypto: 0 = <7.x, 1=7.x;
            NCCH.Header.Flags[4] = 1; // Content Platform: 1 = CTR;
            NCCH.Header.Flags[5] = 0x3; // Content Type Bitflags: 1=Data, 2=Executable, 4=SysUpdate, 8=Manual, 0x10=Trial;
            NCCH.Header.Flags[6] = 0; // MEDIA_UNIT_SIZE = 0x200*Math.Pow(2, Content.header.Flags[6]);
            NCCH.Header.Flags[7] = 1; // FixedCrypto = 1, NoMountRomfs = 2; NoCrypto=4;
            NCCH.Header.LogoOffset = (uint)(Len / MEDIA_UNIT_SIZE);
            NCCH.Header.LogoSize = (uint)(NCCH.logo.Length / MEDIA_UNIT_SIZE);
            Len += (uint)NCCH.logo.Length;
            NCCH.Header.PlainRegionOffset = (uint)(NCCH.plainregion.Length > 0 ? Len / MEDIA_UNIT_SIZE : 0);
            NCCH.Header.PlainRegionSize = (uint)NCCH.plainregion.Length / MEDIA_UNIT_SIZE;
            Len += (uint)NCCH.plainregion.Length;
            NCCH.Header.ExefsOffset = (uint)(Len / MEDIA_UNIT_SIZE);
            NCCH.Header.ExefsSize = (uint)(NCCH.ExeFS.Data.Length / MEDIA_UNIT_SIZE);
            NCCH.Header.ExefsSuperBlockSize = 0x200 / MEDIA_UNIT_SIZE; //Static 0x200 for exefs superblock
            Len += (uint)NCCH.ExeFS.Data.Length;
            Len = (uint)Align(Len, 0x1000); //Romfs Start is aligned to 0x1000
            NCCH.Header.RomfsOffset = (uint)(Len / MEDIA_UNIT_SIZE);
            NCCH.Header.RomfsSize = (uint)(new FileInfo(NCCH.RomFS.FileName).Length / MEDIA_UNIT_SIZE);
            NCCH.Header.RomfsSuperBlockSize = NCCH.RomFS.SuperBlockLen / MEDIA_UNIT_SIZE;
            Len += NCCH.Header.RomfsSize * MEDIA_UNIT_SIZE;
            NCCH.Header.ExefsHash = NCCH.ExeFS.SuperBlockHash;
            NCCH.Header.RomfsHash = NCCH.RomFS.SuperBlockHash;
            NCCH.Header.Size = (uint)(Len / MEDIA_UNIT_SIZE);
            //Build the Header byte[].
            UpdateTB(TB_Progress, "Building NCCH Header...");
            NCCH.Header.BuildHeader();

            return NCCH;
        }

        internal static NCSD SetNCSD(NCCH NCCH, bool Card2, RichTextBox TB_Progress = null)
        {
            TB_Progress ??= new RichTextBox();
            UpdateTB(TB_Progress, "Building NCSD Header...");
            NCSD NCSD = new NCSD
            {
                NCCH_Array = new List<NCCH> {NCCH},
                Card2 = Card2,
                Header = new NCSD.NCSDHeader {Signature = new byte[0x100], Magic = 0x4453434E}
            };
            ulong Length = 0x80 * 0x100000; // 128 MB
            while (Length <= (NCCH.Header.Size * MEDIA_UNIT_SIZE) + 0x400000) //Extra 4 MB for potential save data
            {
                Length *= 2;
            }
            NCSD.Header.MediaSize = (uint)(Length / MEDIA_UNIT_SIZE);
            NCSD.Header.TitleId = NCCH.Exheader.TitleID;
            NCSD.Header.OffsetSizeTable = new NCSD.NCCH_Meta[8];
            ulong OSOfs = 0x4000;
            for (int i = 0; i < NCSD.Header.OffsetSizeTable.Length; i++)
            {
                NCSD.NCCH_Meta ncchm = new NCSD.NCCH_Meta();
                if (i < NCSD.NCCH_Array.Count)
                {
                    ncchm.Offset = (uint)(OSOfs / MEDIA_UNIT_SIZE);
                    ncchm.Size = NCSD.NCCH_Array[i].Header.Size;
                }
                else
                {
                    ncchm.Offset = 0;
                    ncchm.Size = 0;
                }
                NCSD.Header.OffsetSizeTable[i] = ncchm;
                OSOfs += ncchm.Size * MEDIA_UNIT_SIZE;
            }
            NCSD.Header.flags = new byte[0x8];
            NCSD.Header.flags[0] = 0; // 0-255 seconds of waiting for save writing.
            NCSD.Header.flags[3] = NCSD.Card2 ? (byte)2 : (byte)1; // Media Card Device: 1 = NOR Flash, 2 = None, 3 = BT
            NCSD.Header.flags[4] = 1; // Media Platform Index: 1 = CTR
            NCSD.Header.flags[5] = NCSD.Card2 ? (byte)2 : (byte)1; // Media Type Index: 0 = Inner Device, 1 = Card1, 2 = Card2, 3 = Extended Device
            NCSD.Header.flags[6] = 0; // Media Unit Size. Same as NCCH.
            NCSD.Header.flags[7] = 0; // Old Media Card Device.
            NCSD.Header.NCCHIdTable = new ulong[8];
            for (int i = 0; i < NCSD.NCCH_Array.Count; i++)
            {
                NCSD.Header.NCCHIdTable[i] = NCSD.NCCH_Array[i].Header.TitleId;
            }
            NCSD.cardinfoheader = new NCSD.CardInfoHeader
            {
                WritableAddress = (uint)NCSD.GetWritableAddress(),
                CardInfoBitmask = 0,
                CIN = new NCSD.CardInfoHeader.CardInfoNotes
                {
                    Reserved0 = new byte[0xF8],
                    MediaSizeUsed = OSOfs,
                    Reserved1 = 0,
                    Unknown = 0,
                    Reserved2 = new byte[0xC],
                    CVerTitleId = 0,
                    CVerTitleVersion = 0,
                    Reserved3 = new byte[0xCD6]
                },
                NCCH0TitleId = NCSD.NCCH_Array[0].Header.TitleId,
                Reserved0 = 0,
                InitialData = new byte[0x30]
            };
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randbuffer = new byte[0x2C];
            rng.GetBytes(randbuffer);
            Array.Copy(randbuffer, NCSD.cardinfoheader.InitialData, randbuffer.Length);
            NCSD.cardinfoheader.Reserved1 = new byte[0xC0];
            NCSD.cardinfoheader.NCCH0Header = new byte[0x100];
            Array.Copy(NCSD.NCCH_Array[0].Header.Data, 0x100, NCSD.cardinfoheader.NCCH0Header, 0, 0x100);

            NCSD.BuildHeader();

            //NCSD is Initialized
            return NCSD;
        }

        internal static bool WriteROM(NCSD NCSD, string SAVE_PATH, bool trimmed = false,
            ProgressBar PB_Show = null, RichTextBox TB_Progress = null)
        {
            PB_Show ??= new ProgressBar();
            TB_Progress ??= new RichTextBox();
            using (FileStream OutFileStream = new FileStream(SAVE_PATH, FileMode.Create))
            {
                UpdateTB(TB_Progress, "Writing NCSD Header...");
                OutFileStream.Write(NCSD.Data, 0, NCSD.Data.Length);
                UpdateTB(TB_Progress, "Writing NCCH...");
                OutFileStream.Write(NCSD.NCCH_Array[0].Header.Data, 0, NCSD.NCCH_Array[0].Header.Data.Length); //Write NCCH header
                //AES time.
                byte[] key = new byte[0x10]; //Fixed-Crypto key is all zero.
                for (int i = 0; i < 3; i++)
                {
                    AesCtr aesctr = new AesCtr(key, NCSD.NCCH_Array[0].Header.ProgramId, (ulong)(i + 1) << 56); //CTR is ProgramID, section id<<88
                    switch (i)
                    {
                        case 0: //Exheader + AccessDesc
                            UpdateTB(TB_Progress, "Writing Exheader...");
                            byte[] inEncExheader = new byte[NCSD.NCCH_Array[0].Exheader.Data.Length + NCSD.NCCH_Array[0].Exheader.AccessDescriptor.Length];
                            byte[] outEncExheader = new byte[NCSD.NCCH_Array[0].Exheader.Data.Length + NCSD.NCCH_Array[0].Exheader.AccessDescriptor.Length];
                            Array.Copy(NCSD.NCCH_Array[0].Exheader.Data, inEncExheader, NCSD.NCCH_Array[0].Exheader.Data.Length);
                            Array.Copy(NCSD.NCCH_Array[0].Exheader.AccessDescriptor, 0, inEncExheader, NCSD.NCCH_Array[0].Exheader.Data.Length, NCSD.NCCH_Array[0].Exheader.AccessDescriptor.Length);
                            aesctr.TransformBlock(inEncExheader, 0, inEncExheader.Length, outEncExheader, 0);
                            OutFileStream.Write(outEncExheader, 0, outEncExheader.Length); // Write Exheader
                            break;
                        case 1: //Exefs
                            UpdateTB(TB_Progress, "Writing Exefs...");
                            OutFileStream.Seek(0x4000 + (NCSD.NCCH_Array[0].Header.ExefsOffset * MEDIA_UNIT_SIZE), SeekOrigin.Begin);
                            byte[] OutExefs = new byte[NCSD.NCCH_Array[0].ExeFS.Data.Length];
                            aesctr.TransformBlock(NCSD.NCCH_Array[0].ExeFS.Data, 0, NCSD.NCCH_Array[0].ExeFS.Data.Length, OutExefs, 0);
                            OutFileStream.Write(OutExefs, 0, OutExefs.Length);
                            break;
                        case 2: //Romfs
                            UpdateTB(TB_Progress, "Writing Romfs...");
                            OutFileStream.Seek(0x4000 + (NCSD.NCCH_Array[0].Header.RomfsOffset * MEDIA_UNIT_SIZE), SeekOrigin.Begin);
                            using (FileStream InFileStream = new FileStream(NCSD.NCCH_Array[0].RomFS.FileName, FileMode.Open, FileAccess.Read))
                            {
                                uint BUFFER_SIZE;
                                ulong RomfsLen = NCSD.NCCH_Array[0].Header.RomfsSize * MEDIA_UNIT_SIZE;
                                PB_Show.Invoke((Action)(() =>
                                {
                                    PB_Show.Minimum = 0;
                                    PB_Show.Maximum = (int)(RomfsLen / 0x400000);
                                    PB_Show.Value = 0;
                                    PB_Show.Step = 1;
                                }));
                                for (ulong j = 0; j < RomfsLen; j += BUFFER_SIZE)
                                {
                                    BUFFER_SIZE = RomfsLen - j > 0x400000 ? 0x400000 : (uint)(RomfsLen - j);
                                    byte[] buf = new byte[BUFFER_SIZE];
                                    byte[] outbuf = new byte[BUFFER_SIZE];
                                    InFileStream.Read(buf, 0, (int)BUFFER_SIZE);
                                    aesctr.TransformBlock(buf, 0, (int)BUFFER_SIZE, outbuf, 0);
                                    OutFileStream.Write(outbuf, 0, (int)BUFFER_SIZE);
                                    PB_Show.Invoke((Action)PB_Show.PerformStep);
                                }
                            }
                            break;
                    }
                }
                UpdateTB(TB_Progress, "Writing Logo...");
                OutFileStream.Seek(0x4000 + (NCSD.NCCH_Array[0].Header.LogoOffset * MEDIA_UNIT_SIZE), SeekOrigin.Begin);
                OutFileStream.Write(NCSD.NCCH_Array[0].logo, 0, NCSD.NCCH_Array[0].logo.Length);
                if (NCSD.NCCH_Array[0].plainregion.Length > 0)
                {
                    UpdateTB(TB_Progress, "Writing Plain Region...");
                    OutFileStream.Seek(0x4000 + (NCSD.NCCH_Array[0].Header.PlainRegionOffset * MEDIA_UNIT_SIZE), SeekOrigin.Begin);
                    OutFileStream.Write(NCSD.NCCH_Array[0].plainregion, 0, NCSD.NCCH_Array[0].plainregion.Length);
                }

                //NCSD Padding
                if (!trimmed)
                {
                    OutFileStream.Seek((NCSD.Header.OffsetSizeTable[NCSD.NCCH_Array.Count - 1].Offset * MEDIA_UNIT_SIZE) + (NCSD.Header.OffsetSizeTable[NCSD.NCCH_Array.Count - 1].Size * MEDIA_UNIT_SIZE), SeekOrigin.Begin);
                    ulong TotalLen = NCSD.Header.MediaSize * MEDIA_UNIT_SIZE;
                    byte[] Buffer = Enumerable.Repeat((byte)0xFF, 0x400000).ToArray();
                    UpdateTB(TB_Progress, "Writing NCSD Padding...");
                    while ((ulong)OutFileStream.Position < TotalLen)
                    {
                        int BUFFER_LEN = TotalLen - (ulong)OutFileStream.Position < 0x400000 ? (int)(TotalLen - (ulong)OutFileStream.Position) : 0x400000;
                        OutFileStream.Write(Buffer, 0, BUFFER_LEN);
                    }
                }
            }

            //Delete Temporary Romfs File
            if (NCSD.NCCH_Array[0].RomFS.isTempFile)
                File.Delete(NCSD.NCCH_Array[0].RomFS.FileName);

            UpdateTB(TB_Progress, "Done!");
            return true;
        }

        // Utility
        internal static bool IsValid(string exeFS, string romFS, string exeheader, string path, string serial, bool Card2)
        {
            bool isSerialValid = true;
            if (serial.Length == 10)
            {
                string[] subs = serial.Split('-');
                if (subs.Length != 3)
                {
                    isSerialValid = false;
                }
                else
                {
                    if (subs[0].Length != 3 || subs[1].Length != 1 || subs[2].Length != 4)
                    {
                        isSerialValid = false;
                    }
                    else if (subs[0] != "CTR" && subs[0] != "KTR")
                    {
                        isSerialValid = false;
                    }
                    else if (subs[1] != "P" && subs[1] != "N" && subs[2] != "U")
                    {
                        isSerialValid = false;
                    }
                    else
                    {
                        if (subs[2].Any(c => !char.IsLetterOrDigit(c)))
                            isSerialValid = false;
                    }
                }
            }
            else
            {
                isSerialValid = false;
            }
            if (string.IsNullOrEmpty(exeFS)
                || string.IsNullOrEmpty(romFS)
                || string.IsNullOrEmpty(exeheader)
                || string.IsNullOrEmpty(path)
                || !isSerialValid)
            {
                return false;
            }

            Exheader exh = new Exheader(exeheader);
            return !exh.IsSupported() || Card2;
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

        internal static ulong Align(ulong input, ulong alignsize)
        {
            ulong output = input;
            if (output % alignsize != 0)
            {
                output += alignsize - (output % alignsize);
            }
            return output;
        }
    }
}
