using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CTR
{
    internal class FileFormat
    {
        internal static string defaultExtension = "bin";
        internal static string[] validEXT = {"BCH",};

        internal static string Guess(string path)
        {
            string ext;
            using (BinaryReader br = new BinaryReader(File.OpenRead(path)))
                ext = Guess(br);
            return ext;
        }
        internal static string Guess(byte[] data)
        {
            string ext;
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                ext = Guess(br);
            return ext;
        }
        internal static string Guess(MemoryStream ms, bool start = true)
        {
            string ext;
            using (BinaryReader br = new BinaryReader(ms))
                ext = Guess(br, start);
            return ext;
        }
        internal static string Guess(BinaryReader br, bool start = true)
        {
            long position = br.BaseStream.Position; // Store current position to reset after.

            if (start) // Seek to top of stream if requested
                br.BaseStream.Position = 0;

            // Guess Extension
            string ext;
            if (GuessMini(br, out ext))
                Console.WriteLine("Mini Packed File detected, extension type " + ext);
            else if (GuessHeaderedDARC(br, out ext))
                Console.WriteLine("Headered DARC File detected, extension type " + ext);
            else if (GuessBCLIM(br, out ext))
                Console.WriteLine("BCLIM File detected, extension type " + ext);
            else if (GuessLZ11(br, out ext))
                Console.WriteLine("LZ11 Compressed File detected, extension type " + ext);
            else if (Guess4CHAR(br, out ext))
                Console.WriteLine("4CHAR File detected, extension type " + ext);
            else if (Guess3CHAR(br, out ext))
                Console.WriteLine("3CHAR File detected, extension type " + ext);
            else ext = defaultExtension; // default

            // Return BaseStream position to the start.
            br.BaseStream.Position = position;
            return "." + ext;
        }

        internal static bool GuessMini(BinaryReader br, out string ext)
        {
            long position = br.BaseStream.Position; // Store current position to reset after.
            ext = ""; // Reset extension
            try
            {
                // check for 2char container extensions
                ushort magic = br.ReadUInt16();
                ushort count = br.ReadUInt16();
                br.BaseStream.Position = 4 + 4 * count;
                if (br.ReadUInt32() == br.BaseStream.Length)
                {
                    ext += (char)magic & 0xFF;
                    ext += (char)magic << 8;
                }
            }
            catch { }
            // Return BaseStream position to the start.
            br.BaseStream.Position = position;

            return ext.Length > 0;
        }
        internal static bool GuessHeaderedDARC(BinaryReader br, out string ext)
        {
            long position = br.BaseStream.Position; // Store current position to reset after.
            ext = ""; // Reset extension
            try
            {
                byte[] magic = Encoding.ASCII.GetBytes(br.ReadChars(4));
                int count = BitConverter.ToUInt16(magic, 0);
                br.BaseStream.Position = position + 4 + 0x40 * count;
                uint tableval = br.ReadUInt32();
                br.BaseStream.Position += 0x20 * tableval;
                while (br.PeekChar() == 0) // seek forward
                    br.ReadByte();
                if (br.ReadUInt32() == 0x63726164)
                    ext = "darc";
            }
            catch { }
            // Return BaseStream position to the start.
            br.BaseStream.Position = position;

            return ext.Length > 0;
        }
        internal static bool GuessBCLIM(BinaryReader br, out string ext)
        {
            long position = br.BaseStream.Position; // Store current position to reset after.
            ext = ""; // Reset extension
            try
            {
                br.BaseStream.Position = br.BaseStream.Length - 0x28;
                if (br.ReadUInt32() == 0x4D494C43)
                {
                    br.BaseStream.Position = br.BaseStream.Length - 0x4;
                    if (br.ReadUInt32() == br.BaseStream.Length - 0x28)
                        ext = "bclim";
                }
            }
            catch { }
            // Return BaseStream position to the start.
            br.BaseStream.Position = position;

            return ext.Length > 0;
        }
        internal static bool GuessLZ11(BinaryReader br, out string ext)
        {
            long position = br.BaseStream.Position; // Store current position to reset after.
            ext = ""; // Reset extension
            try
            {
                int type = br.PeekChar();
                if (type != 0x11)
                    return false;
                byte[] sizeBytes = new byte[3];
                br.Read(sizeBytes, 0, 3);

                int decompressedSize = sizeBytes[0] | sizeBytes[1] << 8 | sizeBytes[2];
                if (decompressedSize > br.BaseStream.Length && decompressedSize < br.BaseStream.Length * 10) // assuming 10x compression isn't feasible
                    ext = "lz"; // really weak LZ detection, at most 16MB
            }
            catch { }
            br.BaseStream.Position = position;
            return ext.Length > 0;
        }
        internal static bool Guess4CHAR(BinaryReader br, out string ext)
        {
            long position = br.BaseStream.Position; // Store current position to reset after.
            ext = ""; // Reset extension
            try
            {
                byte[] magic = Encoding.ASCII.GetBytes(br.ReadChars(4));

                Regex r = new Regex("^[a-zA-Z0-9]*$");
                ext = Encoding.ASCII.GetString(magic);
                // Return BaseStream position to the start.
                br.BaseStream.Position = position;

                return r.IsMatch(ext) && ext.Length == 4;
            }
            catch { }
            br.BaseStream.Position = position;
            return false;
        }
        internal static bool Guess3CHAR(BinaryReader br, out string ext)
        {
            long position = br.BaseStream.Position; // Store current position to reset after.
            ext = ""; // Reset extension
            try
            {
                byte[] magic = Encoding.ASCII.GetBytes(br.ReadChars(3));

                ext = Encoding.ASCII.GetString(magic);
                // Return BaseStream position to the start.
                br.BaseStream.Position = position;

                return validEXT.Contains(ext);
            }
            catch { }
            br.BaseStream.Position = position;
            return false;
        }
    }
}
