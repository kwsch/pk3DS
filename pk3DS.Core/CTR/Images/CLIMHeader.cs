using System.Runtime.InteropServices;

namespace pk3DS.Core.CTR
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct CLIMHeader : IXLIMHeader
    {
        public const int SIZE = 40;
        public const string Identifier = "CLIM";
        public bool Valid => LittleEndian && Magic == 0x4D_49_4C_43; // CLIM
        public bool LittleEndian => BOM == 0xFEFF;
        public bool BigEndian => BOM == 0xFFFE;

        public uint Magic { get; set; }// CLIM = 0x4D494C43
        public ushort BOM;          // 0xFFFE
        public uint HeaderLength;   // always 0x14
        public byte TileWidth;       // 1<<[[n]]
        public byte TileHeight;      // 1<<[[n]]
        public uint totalLength;  // Total Length of file
        public uint Count;        // "1" , guessing it's just Count.

        public uint imag;         // imag = 0x67616D69
        public uint imagLength;   // HeaderLength - 10
        public ushort Width { get; set; }      // Final Dimensions
        public ushort Height { get; set; }     // Final Dimensions
        public XLIMEncoding Format { get; set; }
        public XLIMOrientation Orientation { get; set; } // unused
        public short Alignment; // unused
        public uint DataSize;   // Pixel Data Region Length
    }
}