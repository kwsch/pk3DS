using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using pk3DS.Properties;

namespace CTR
{
    public class CTR
    {
        public const uint MEDIA_UNIT_SIZE = 0x200;

        // Main wrapper that assembles the ROM based on the following specifications:
        internal static bool buildROM(bool Card2, string LOGO_NAME,
            string EXEFS_PATH, string ROMFS_PATH, string EXHEADER_PATH,
            string SERIAL_TEXT, string SAVE_PATH,
            ProgressBar PB_Show = null, RichTextBox TB_Progress = null)
        {
            PB_Show = PB_Show ?? new ProgressBar();
            TB_Progress = TB_Progress ?? new RichTextBox();

            // Sanity check the input files.
            if (!(File.Exists(EXEFS_PATH) && File.Exists(ROMFS_PATH) && File.Exists(EXHEADER_PATH))) return false;

            var NCCH = setNCCH(EXEFS_PATH, ROMFS_PATH, EXHEADER_PATH, SERIAL_TEXT, LOGO_NAME, PB_Show, TB_Progress);
            var NCSD = setNCSD(NCCH, Card2, PB_Show, TB_Progress);
            bool success = writeROM(NCSD, NCCH, SAVE_PATH, PB_Show, TB_Progress);
            return success;
        }

        // Sub methods that drive the operation
        internal static NCCH setNCCH(string EXEFS_PATH, string ROMFS_PATH, string EXHEADER_PATH, string TB_Serial, string LOGO_NAME,
            ProgressBar PB_Show = null, RichTextBox TB_Progress = null)
        {
            PB_Show = PB_Show ?? new ProgressBar();
            TB_Progress = TB_Progress ?? new RichTextBox();
            SHA256Managed sha = new SHA256Managed();
            NCCH Content = new NCCH();
            updateTB(TB_Progress, "Adding Exheader...");
            Content.exheader = new Exheader(EXHEADER_PATH);
            Content.plainregion = new byte[0]; //No plain region by default.
            if (Content.exheader.isPokemon())
            {
                updateTB(TB_Progress, "Detected Pokemon Game. Adding Plain Region...");
                if (Content.exheader.isXY())
                {
                    Content.plainregion = (byte[])Resources.ResourceManager.GetObject("XY");
                }
                else if (Content.exheader.isORAS())
                {
                    Content.plainregion = (byte[])Resources.ResourceManager.GetObject("ORAS");
                }
            }
            updateTB(TB_Progress, "Adding ExeFS...");
            Content.exefs = new ExeFS(EXEFS_PATH);
            updateTB(TB_Progress, "Adding RomFS...");
            Content.romfs = new RomFS(ROMFS_PATH);

            updateTB(TB_Progress, "Adding Logo...");
            Content.logo = (byte[])Resources.ResourceManager.GetObject(LOGO_NAME);
            updateTB(TB_Progress, "Building NCCH Header...");
            ulong Len = 0x200; //NCCH Signature + NCCH Header
            Content.header = new NCCH.Header { Signature = new byte[0x100], Magic = 0x4843434E };
            Content.header.TitleId = Content.header.ProgramId = Content.exheader.TitleID;
            Content.header.MakerCode = 0x3130; //01
            Content.header.FormatVersion = 0x2; //Default
            Content.header.LogoHash = sha.ComputeHash(Content.logo);
            Content.header.ProductCode = Encoding.ASCII.GetBytes(TB_Serial);
            Array.Resize(ref Content.header.ProductCode, 0x10);
            Content.header.ExheaderHash = Content.exheader.GetSuperBlockHash();
            Content.header.ExheaderSize = (uint)(Content.exheader.Data.Length);
            Len += Content.header.ExheaderSize + (uint)Content.exheader.AccessDescriptor.Length;
            Content.header.Flags = new byte[0x8];
            //FLAGS
            Content.header.Flags[3] = 0; // Crypto: 0 = <7.x, 1=7.x;
            Content.header.Flags[4] = 1; // Content Platform: 1 = CTR;
            Content.header.Flags[5] = 0x3; // Content Type Bitflags: 1=Data, 2=Executable, 4=SysUpdate, 8=Manual, 0x10=Trial;
            Content.header.Flags[6] = 0; // MEDIA_UNIT_SIZE = 0x200*Math.Pow(2, Content.header.Flags[6]);
            Content.header.Flags[7] = 1; // FixedCrypto = 1, NoMountRomfs = 2; NoCrypto=4;
            Content.header.LogoOffset = (uint)(Len / MEDIA_UNIT_SIZE);
            Content.header.LogoSize = (uint)(Content.logo.Length / MEDIA_UNIT_SIZE);
            Len += (uint)Content.logo.Length;
            Content.header.PlainRegionOffset = (uint)((Content.plainregion.Length > 0) ? Len / MEDIA_UNIT_SIZE : 0);
            Content.header.PlainRegionSize = (uint)Content.plainregion.Length / MEDIA_UNIT_SIZE;
            Len += (uint)Content.plainregion.Length;
            Content.header.ExefsOffset = (uint)(Len / MEDIA_UNIT_SIZE);
            Content.header.ExefsSize = (uint)(Content.exefs.Data.Length / MEDIA_UNIT_SIZE);
            Content.header.ExefsSuperBlockSize = 0x200 / MEDIA_UNIT_SIZE; //Static 0x200 for exefs superblock
            Len += (uint)Content.exefs.Data.Length;
            Len = (uint)Align(Len, 0x1000); //Romfs Start is aligned to 0x1000
            Content.header.RomfsOffset = (uint)(Len / MEDIA_UNIT_SIZE);
            Content.header.RomfsSize = (uint)((new FileInfo(Content.romfs.FileName)).Length / MEDIA_UNIT_SIZE);
            Content.header.RomfsSuperBlockSize = Content.romfs.SuperBlockLen / MEDIA_UNIT_SIZE;
            Len += Content.header.RomfsSize * MEDIA_UNIT_SIZE;
            Content.header.ExefsHash = Content.exefs.SuperBlockHash;
            Content.header.RomfsHash = Content.romfs.SuperBlockHash;
            Content.header.Size = (uint)(Len / MEDIA_UNIT_SIZE);
            //Build the Header byte[].
            Content.header.BuildHeader();

            return Content;
        }
        internal static NCSD setNCSD(NCCH Content, bool Card2,
            ProgressBar PB_Show = null, RichTextBox TB_Progress = null)
        {
            PB_Show = PB_Show ?? new ProgressBar();
            TB_Progress = TB_Progress ?? new RichTextBox();
            NCSD Rom = new NCSD { NCCH_Array = new List<NCCH>() };
            Rom.NCCH_Array.Add(Content);
            updateTB(TB_Progress, "Building NCSD Header...");
            Rom.Card2 = Card2;
            Rom.header = new NCSD.Header { Signature = new byte[0x100], Magic = 0x4453434E };
            ulong Length = 0x80 * 0x100000; // 128 MB
            while (Length <= Content.header.Size * MEDIA_UNIT_SIZE + 0x400000) //Extra 4 MB for potential save data
            {
                Length *= 2;
            }
            Rom.header.MediaSize = (uint)(Length / MEDIA_UNIT_SIZE);
            Rom.header.TitleId = Content.exheader.TitleID;
            Rom.header.OffsetSizeTable = new NCSD.NCCH_Meta[8];
            ulong OSOfs = 0x4000;
            for (int i = 0; i < Rom.header.OffsetSizeTable.Length; i++)
            {
                NCSD.NCCH_Meta ncchm = new NCSD.NCCH_Meta();
                if (i < Rom.NCCH_Array.Count)
                {
                    ncchm.Offset = (uint)(OSOfs / MEDIA_UNIT_SIZE);
                    ncchm.Size = Rom.NCCH_Array[i].header.Size;
                }
                else
                {
                    ncchm.Offset = 0;
                    ncchm.Size = 0;
                }
                Rom.header.OffsetSizeTable[i] = ncchm;
                OSOfs += ncchm.Size * MEDIA_UNIT_SIZE;
            }
            Rom.header.flags = new byte[0x8];
            Rom.header.flags[0] = 0; // 0-255 seconds of waiting for save writing.
            Rom.header.flags[3] = (byte)(Rom.Card2 ? 2 : 1); // Media Card Device: 1 = NOR Flash, 2 = None, 3 = BT
            Rom.header.flags[4] = 1; // Media Platform Index: 1 = CTR
            Rom.header.flags[5] = (byte)(Rom.Card2 ? 2 : 1); // Media Type Index: 0 = Inner Device, 1 = Card1, 2 = Card2, 3 = Extended Device
            Rom.header.flags[6] = 0; // Media Unit Size. Same as NCCH.
            Rom.header.flags[7] = 0; // Old Media Card Device.
            Rom.header.NCCHIdTable = new ulong[8];
            for (int i = 0; i < Rom.NCCH_Array.Count; i++)
            {
                Rom.header.NCCHIdTable[i] = Rom.NCCH_Array[i].header.TitleId;
            }
            Rom.cardinfoheader = new NCSD.CardInfoHeader
            {
                WritableAddress = (uint)(Rom.GetWritableAddress()),
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
                NCCH0TitleId = Rom.NCCH_Array[0].header.TitleId,
                Reserved0 = 0,
                InitialData = new byte[0x30]
            };
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randbuffer = new byte[0x2C];
            rng.GetBytes(randbuffer);
            Array.Copy(randbuffer, Rom.cardinfoheader.InitialData, randbuffer.Length);
            Rom.cardinfoheader.Reserved1 = new byte[0xC0];
            Rom.cardinfoheader.NCCH0Header = new byte[0x100];
            Array.Copy(Rom.NCCH_Array[0].header.Data, 0x100, Rom.cardinfoheader.NCCH0Header, 0, 0x100);

            Rom.BuildHeader();

            //NCSD is Initialized
            return Rom;
        }
        internal static bool writeROM(NCSD Rom, NCCH Content, string SAVE_PATH,
            ProgressBar PB_Show = null, RichTextBox TB_Progress = null)
        {
            PB_Show = PB_Show ?? new ProgressBar();
            TB_Progress = TB_Progress ?? new RichTextBox();
            using (FileStream OutFileStream = new FileStream(SAVE_PATH, FileMode.Create))
            {
                updateTB(TB_Progress, "Writing NCSD Header...");
                OutFileStream.Write(Rom.Data, 0, Rom.Data.Length);
                updateTB(TB_Progress, "Writing NCCH...");
                OutFileStream.Write(Rom.NCCH_Array[0].header.Data, 0, Rom.NCCH_Array[0].header.Data.Length); //Write NCCH header
                //AES time.
                byte[] key = new byte[0x10]; //Fixed-Crypto key is all zero.
                for (int i = 0; i < 3; i++)
                {
                    AesCtr aesctr = new AesCtr(key, Rom.NCCH_Array[0].header.ProgramId, ((ulong)(i + 1)) << 56); //CTR is ProgramID, section id<<88
                    switch (i)
                    {
                        case 0: //Exheader + AccessDesc
                            updateTB(TB_Progress, "Writing Exheader...");
                            byte[] inEncExheader = new byte[Rom.NCCH_Array[0].exheader.Data.Length + Rom.NCCH_Array[0].exheader.AccessDescriptor.Length];
                            byte[] outEncExheader = new byte[Rom.NCCH_Array[0].exheader.Data.Length + Rom.NCCH_Array[0].exheader.AccessDescriptor.Length];
                            Array.Copy(Rom.NCCH_Array[0].exheader.Data, inEncExheader, Rom.NCCH_Array[0].exheader.Data.Length);
                            Array.Copy(Rom.NCCH_Array[0].exheader.AccessDescriptor, 0, inEncExheader, Rom.NCCH_Array[0].exheader.Data.Length, Rom.NCCH_Array[0].exheader.AccessDescriptor.Length);
                            aesctr.TransformBlock(inEncExheader, 0, inEncExheader.Length, outEncExheader, 0);
                            OutFileStream.Write(outEncExheader, 0, outEncExheader.Length); // Write Exheader
                            break;
                        case 1: //Exefs
                            updateTB(TB_Progress, "Writing Exefs...");
                            OutFileStream.Seek(0x4000 + Rom.NCCH_Array[0].header.ExefsOffset * MEDIA_UNIT_SIZE, SeekOrigin.Begin);
                            byte[] OutExefs = new byte[Rom.NCCH_Array[0].exefs.Data.Length];
                            aesctr.TransformBlock(Rom.NCCH_Array[0].exefs.Data, 0, Rom.NCCH_Array[0].exefs.Data.Length, OutExefs, 0);
                            OutFileStream.Write(OutExefs, 0, OutExefs.Length);
                            break;
                        case 2: //Romfs
                            updateTB(TB_Progress, "Writing Romfs...");
                            OutFileStream.Seek(0x4000 + Rom.NCCH_Array[0].header.RomfsOffset * MEDIA_UNIT_SIZE, SeekOrigin.Begin);
                            using (FileStream InFileStream = new FileStream(Rom.NCCH_Array[0].romfs.FileName, FileMode.Open, FileAccess.Read))
                            {
                                uint BUFFER_SIZE = 0;
                                ulong RomfsLen = Rom.NCCH_Array[0].header.RomfsSize * MEDIA_UNIT_SIZE;
                                PB_Show.Invoke((Action)(() =>
                                {
                                    PB_Show.Minimum = 0;
                                    PB_Show.Maximum = (int)(RomfsLen / 0x400000);
                                    PB_Show.Value = 0;
                                    PB_Show.Step = 1;
                                }));
                                for (ulong j = 0; j < (RomfsLen); j += BUFFER_SIZE)
                                {
                                    BUFFER_SIZE = (RomfsLen - j) > 0x400000 ? 0x400000 : (uint)(RomfsLen - j);
                                    byte[] buf = new byte[BUFFER_SIZE];
                                    byte[] outbuf = new byte[BUFFER_SIZE];
                                    InFileStream.Read(buf, 0, (int)BUFFER_SIZE);
                                    aesctr.TransformBlock(buf, 0, (int)BUFFER_SIZE, outbuf, 0);
                                    OutFileStream.Write(outbuf, 0, (int)BUFFER_SIZE);
                                    PB_Show.Invoke((Action)(PB_Show.PerformStep));
                                }
                            }
                            break;
                    }
                }
                updateTB(TB_Progress, "Writing Logo...");
                OutFileStream.Seek(0x4000 + Rom.NCCH_Array[0].header.LogoOffset * MEDIA_UNIT_SIZE, SeekOrigin.Begin);
                OutFileStream.Write(Rom.NCCH_Array[0].logo, 0, Rom.NCCH_Array[0].logo.Length);
                if (Rom.NCCH_Array[0].plainregion.Length > 0)
                {
                    updateTB(TB_Progress, "Writing Plain Region...");
                    OutFileStream.Seek(0x4000 + Rom.NCCH_Array[0].header.PlainRegionOffset * MEDIA_UNIT_SIZE, SeekOrigin.Begin);
                    OutFileStream.Write(Rom.NCCH_Array[0].plainregion, 0, Rom.NCCH_Array[0].plainregion.Length);
                }

                //NCSD Padding
                OutFileStream.Seek(Rom.header.OffsetSizeTable[Rom.NCCH_Array.Count - 1].Offset * MEDIA_UNIT_SIZE + Rom.header.OffsetSizeTable[Rom.NCCH_Array.Count - 1].Size * MEDIA_UNIT_SIZE, SeekOrigin.Begin);
                ulong TotalLen = Rom.header.MediaSize * MEDIA_UNIT_SIZE;
                byte[] Buffer = Enumerable.Repeat((byte)0xFF, 0x400000).ToArray();
                updateTB(TB_Progress, "Writing NCSD Padding...");
                while ((ulong)OutFileStream.Position < TotalLen)
                {
                    int BUFFER_LEN = ((TotalLen - (ulong)OutFileStream.Position) < 0x400000) ? (int)(TotalLen - (ulong)OutFileStream.Position) : 0x400000;
                    OutFileStream.Write(Buffer, 0, BUFFER_LEN);
                }
            }

            //Delete Temporary Romfs File
            File.Delete(Content.romfs.FileName);

            updateTB(TB_Progress, "Done!");
            return true;
        }

        // Utility
        internal static bool isValid(string exeFS, string romFS, string exeheader, string path, string serial, bool Card2)
        {
            bool isSerialValid = true;
            if (serial.Length == 10)
            {
                string[] subs = serial.Split('-');
                if (subs.Length != 3)
                    isSerialValid = false;
                else
                {
                    if (subs[0].Length != 3 || subs[1].Length != 1 || subs[2].Length != 4)
                        isSerialValid = false;
                    else if (subs[0] != "CTR" && subs[0] != "KTR")
                        isSerialValid = false;
                    else if (subs[1] != "P" && subs[1] != "N" && subs[2] != "U")
                        isSerialValid = false;
                    else
                    {
                        foreach (char c in subs[2].Where(c => !Char.IsLetterOrDigit(c)))
                            isSerialValid = false;
                    }
                }
            }
            else
            {
                isSerialValid = false;
            }
            if (exeFS == string.Empty
                || romFS == string.Empty
                || exeheader == string.Empty
                || path == string.Empty
                || !isSerialValid)
                return false;

            Exheader exh = new Exheader(exeheader);
            return !exh.isPokemon() || Card2;
        }
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
        internal static ulong Align(ulong input, ulong alignsize)
        {
            ulong output = input;
            if (output % alignsize != 0)
            {
                output += (alignsize - (output % alignsize));
            }
            return output;
        }
    }
}
