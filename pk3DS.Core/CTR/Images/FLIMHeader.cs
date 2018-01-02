using System.Runtime.InteropServices;

namespace pk3DS.Core.CTR
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FLIMHeader : IXLIMHeader
    {
        public const int SIZE = 40;
        public const string Identifier = "FLIM";
        public bool Valid => LittleEndian && Magic == 0x4D_49_4C_46; // FLIM
        public bool LittleEndian => BOM == 0xFEFF;
        public bool BigEndian => BOM == 0xFFFE;

        public uint Magic { get; set; }          // FLIM
        public ushort BOM;          // 0xFFFE
        public ushort HeaderLength; // always 0x14
        public int Version;
        public uint TotalLength;
        public uint Count;

        public uint imag; // imag = 67616D69
        public uint imagLength; // always 0x10
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public short Alignment;
        public XLIMEncoding Format { get; set; }
        public XLIMOrientation Orientation { get; set; }
        public uint DataSize;
    }
}