using System.IO;
using pk3DS.Core.CTR.Images;

namespace pk3DS.Core.CTR
{
    public class BFLIM : BXLIM
    {
        public BFLIM(Stream data) => ReadBFLIM(data);

        public BFLIM(byte[] data)
        {
            using var ms = new MemoryStream(data);
            ReadBFLIM(ms);
        }

        public BFLIM(string path)
        {
            var data = File.ReadAllBytes(path);
            using var ms = new MemoryStream(data);
            ReadBFLIM(ms);
        }

        private void ReadBFLIM(Stream ms)
        {
            PixelData = new byte[ms.Length - FLIMHeader.SIZE];
            ms.Read(PixelData, 0, PixelData.Length);
            var footer = new byte[FLIMHeader.SIZE];
            ms.Read(footer, 0, footer.Length);
            Footer = footer.ToStructure<FLIMHeader>();
        }
    }
}
