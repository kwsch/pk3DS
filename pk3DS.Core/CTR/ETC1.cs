using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using pk3DS.Core.Properties;

namespace pk3DS.Core.CTR;

internal static partial class ETC1
{
    [LibraryImport("ETC1Lib.dll", EntryPoint = "ConvertETC1")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial void ConvertETC1(IntPtr dataOut, ref uint dataOutSize, IntPtr dataIn, ushort width, ushort height, [MarshalAs(UnmanagedType.Bool)] bool alpha);

    public static void CheckETC1Lib()
    {
        var loc = Assembly.GetEntryAssembly()?.Location ?? typeof(ETC1).Assembly.Location;
        string dllpath = Path.GetDirectoryName(loc) + "\\ETC1Lib.dll";
        if (!File.Exists(dllpath))
            File.WriteAllBytes(dllpath, Resources.ETC1Lib);
    }
}