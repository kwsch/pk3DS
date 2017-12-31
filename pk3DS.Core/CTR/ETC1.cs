using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using pk3DS.Core.Properties;

namespace pk3DS.Core.CTR
{
    internal static class ETC1
    {
        [DllImport("ETC1Lib.dll", EntryPoint = "ConvertETC1", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ConvertETC1(IntPtr dataOut, ref uint dataOutSize, IntPtr dataIn, ushort width, ushort height, bool alpha);

        public static void CheckETC1Lib()
        {
            var loc = Assembly.GetEntryAssembly()?.Location ?? typeof(ETC1).Assembly.Location;
            string dllpath = Path.GetDirectoryName(loc) + "\\ETC1Lib.dll";
            if (!File.Exists(dllpath))
                File.WriteAllBytes(dllpath, Resources.ETC1Lib);
        }
    }
}