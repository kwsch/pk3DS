using System;
using System.Runtime.InteropServices;

namespace CTR
{
    class ETC1
    {
        [DllImport("ETC1Lib.dll", EntryPoint = "ConvertETC1", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ConvertETC1(IntPtr dataOut, ref UInt32 dataOutSize, IntPtr dataIn, UInt16 width, UInt16 height, bool alpha);
    }
}