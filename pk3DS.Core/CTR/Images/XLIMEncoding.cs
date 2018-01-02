namespace pk3DS.Core.CTR
{
    public enum XLIMEncoding : byte
    {
        L8        = 0x00, // 8    Luminance
        A8        = 0x01, // 8    Alpha
        LA4       = 0x02, // 8    Luminance + Alpha
        LA8       = 0x03, // 16   Luminance + Alpha
        HILO8     = 0x04, // 16   ?
        RGB565    = 0x05, // 16   Color
        RGBX8     = 0x06, // 24   Color
        RGB5A1    = 0x07, // 16   Color + Alpha
        RGBA4     = 0x08, // 16   Color + Alpha
        RGBA8     = 0x09, // 32   Color + Alpha
        ETC1      = 0x0A, // 4    Color
        ETC1A4    = 0x0B, // 8    Color + Alpha
        L4        = 0x0C, // 4    Luminance
        A4        = 0x0D, // 4    Alpha
    }
}