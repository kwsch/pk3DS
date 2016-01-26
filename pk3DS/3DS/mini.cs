using System;
using System.IO;
using System.Linq;

namespace CTR
{
    // Mini Packing Util
    class mini
    {
        internal static byte[] adjustMiniHeader(byte[] data, int headerLength)
        {
            // Adjust the header size of the mini file.
            int count = BitConverter.ToUInt16(data, 2);
            int[] start = new int[count];
            for (int i = 0; i < count; i++)
                start[i] = BitConverter.ToInt32(data, 4 + i*4);

            int dataStart = start.Min();
            if (headerLength < dataStart)
                throw new Exception("Specified Header length is too small!?");
            byte[] pack = data.Skip(dataStart).ToArray(); // pull out payload
            byte[] newData = new byte[headerLength].Concat(pack).ToArray(); // append payload onto new header
            Array.Copy(data, 0, newData, 0, dataStart); // copy in old header (then repoint)

            int diff = headerLength - dataStart; // shift pointer
            for (int i = 0; i < count + 1; i++)
                Array.Copy(BitConverter.GetBytes(BitConverter.ToInt32(data, 4 + i * 4) + diff), 0, newData, 4 + 4 * i, 4);

            return newData;
        }
        internal static void packMini(string path, string ident, string fileName, string outExt = null, string outFolder = null, bool delete = true)
        {
            if (outFolder == null)
            {
                delete = false;
                outFolder = path;
            }
            if (outExt == null) outExt = ".bin";
            // Create new Binary with the relevant header bytes
            byte[] data = new byte[4];
            data[0] = (byte)ident[0];
            data[1] = (byte)ident[1];
            string[] files = Directory.GetFiles(path);
            Array.Copy(BitConverter.GetBytes((ushort)files.Length), 0, data, 2, 2);

            int count = files.Length;
            int dataOffset = 4 + 4 + count * 4;

            // Start the data filling.
            using (MemoryStream dataout = new MemoryStream())
            using (MemoryStream offsetMap = new MemoryStream())
            using (BinaryWriter bd = new BinaryWriter(dataout))
            using (BinaryWriter bo = new BinaryWriter(offsetMap))
            {
                // For each file...
                for (int i = 0; i < count; i++)
                {
                    // Write File Offset
                    uint fileOffset = (uint)(dataout.Position + dataOffset);
                    bo.Write(fileOffset);

                    // Write File to Stream
                    bd.Write(File.ReadAllBytes(files[i]));

                    // Pad the Data MemoryStream with Zeroes until len%4=0;
                    while (dataout.Length % 4 != 0)
                        bd.Write((byte)0);
                    // File Offset will be updated as the offset is based off of the Data length.

                    // Delete the File
                    File.Delete(files[i]);
                }
                // Cap the File
                bo.Write((uint)(dataout.Position + dataOffset));

                using (var newPack = File.Create(Path.Combine(outFolder, fileName + outExt)))
                using (var header = new MemoryStream(data))
                {
                    header.WriteTo(newPack);
                    offsetMap.WriteTo(newPack);
                    dataout.WriteTo(newPack);
                }
            }
            if (delete)
                Directory.Delete(path, true);
        }
        internal static byte[] packMini(byte[][] fileData, string ident)
        {
            // Create new Binary with the relevant header bytes
            byte[] data = new byte[4];
            data[0] = (byte)ident[0];
            data[1] = (byte)ident[1];
            Array.Copy(BitConverter.GetBytes((ushort)fileData.Length), 0, data, 2, 2);

            int count = fileData.Length;
            int dataOffset = 4 + 4 + count * 4;

            // Start the data filling.
            using (MemoryStream dataout = new MemoryStream())
            using (MemoryStream offsetMap = new MemoryStream())
            using (BinaryWriter bd = new BinaryWriter(dataout))
            using (BinaryWriter bo = new BinaryWriter(offsetMap))
            {
                // For each file...
                for (int i = 0; i < count; i++)
                {
                    // Write File Offset
                    uint fileOffset = (uint)(dataout.Position + dataOffset);
                    bo.Write(fileOffset);

                    // Write File to Stream
                    bd.Write(fileData[i]);

                    // Pad the Data MemoryStream with Zeroes until len%4=0;
                    while (dataout.Length % 4 != 0)
                        bd.Write((byte)0);
                    // File Offset will be updated as the offset is based off of the Data length.
                }
                // Cap the File
                bo.Write((uint)(dataout.Position + dataOffset));

                using (var newPack = new MemoryStream())
                using (var header = new MemoryStream(data))
                {
                    header.WriteTo(newPack);
                    offsetMap.WriteTo(newPack);
                    dataout.WriteTo(newPack);
                    return newPack.ToArray();
                }
            }
        }
        internal static bool packMini2(string path, string ident, string fileName)
        {
            if (!Directory.Exists(path)) return false;
            try
            {
                string[] filesToPack = Directory.GetFiles(path);
                byte[][] fileData = new byte[filesToPack.Length][];
                for (int i = 0; i < filesToPack.Length; i++) fileData[i] = File.ReadAllBytes(filesToPack[i]);
                byte[] miniBytes = packMini(fileData, ident);
                File.WriteAllBytes(fileName, miniBytes);
                return true;
            }
            catch { return false; }
        }
        internal static void unpackMini(string path, string ident, string outFolder = null, bool delete = true)
        {
            if (outFolder == null) outFolder = Path.GetDirectoryName(path);
            if (!Directory.Exists(outFolder)) Directory.CreateDirectory(outFolder);
            using (var s = new MemoryStream(File.ReadAllBytes(path)))
            using (var br = new BinaryReader(s))
            {
                string fx = new string(br.ReadChars(2));

                if (fx != ident) return;

                ushort count = br.ReadUInt16();
                string namePad = "D" + Math.Ceiling(Math.Log10(count));
                uint[] offsets = new uint[count + 1];
                for (int i = 0; i < count; i++)
                    offsets[i] = br.ReadUInt32();

                uint length = br.ReadUInt32();
                offsets[offsets.Length - 1] = length;

                for (int i = 0; i < count; i++)
                {
                    br.BaseStream.Seek(offsets[i], SeekOrigin.Begin);
                    using (MemoryStream dataout = new MemoryStream())
                    {
                        byte[] data = new byte[0];
                        s.CopyTo(dataout, (int)offsets[i]);
                        int len = (int)offsets[i + 1] - (int)offsets[i];

                        if (len != 0)
                        {
                            data = dataout.ToArray();
                            Array.Resize(ref data, len);
                        }
                        string newFile = Path.Combine(outFolder, i.ToString(namePad) + ".bin");
                        File.WriteAllBytes(newFile, data);
                    }
                }
            }
            if (delete)
                File.Delete(path); // File is unpacked.
        }
        internal static byte[][] unpackMini(byte[] fileData, string ident)
        {
            using (var s = new MemoryStream(fileData))
            using (var br = new BinaryReader(s))
            {
                string fx = new string(br.ReadChars(2));

                if (fx != ident) return null;

                ushort count = br.ReadUInt16();
                byte[][] returnData = new byte[count][];

                uint[] offsets = new uint[count + 1];
                for (int i = 0; i < count; i++)
                    offsets[i] = br.ReadUInt32();

                uint length = br.ReadUInt32();
                offsets[offsets.Length - 1] = length;

                for (int i = 0; i < count; i++)
                {
                    br.BaseStream.Seek(offsets[i], SeekOrigin.Begin);
                    using (MemoryStream dataout = new MemoryStream())
                    {
                        byte[] data = new byte[0];
                        s.CopyTo(dataout, (int)offsets[i]);
                        int len = (int)offsets[i + 1] - (int)offsets[i];

                        if (len != 0)
                        {
                            data = dataout.ToArray();
                            Array.Resize(ref data, len);
                        }
                        returnData[i] = data;
                    }
                }
                return returnData;
            }
        }
        internal static string getIsMini(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            var fi = new FileInfo(path);
            try
            {
                string fx = new string(new[] { (char)data[0], (char)data[1] });
                ushort count = BitConverter.ToUInt16(data, 2);

                uint[] offsets = new uint[count + 1];
                uint length = 1338;
                for (int i = 0; i < count; i++)
                {
                    offsets[i] = BitConverter.ToUInt32(data, 4 + i * 4);
                    length = BitConverter.ToUInt32(data, 8 + i * 4);
                }

                offsets[offsets.Length - 1] = length;
                return fi.Length == length ? fx : null;
            }
            catch { return null; }
        }
    }
}
