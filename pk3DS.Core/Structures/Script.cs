using System;
using System.Linq;

namespace pk3DS.Core
{
    public class Script
    {
        public int Length => BitConverter.ToInt32(Raw, 0x00);
        public uint Magic => BitConverter.ToUInt32(Raw, 0x04);
        // case 0x0A0AF1E0: code = read_code_block(f); break;
        // case 0x0A0AF1EF: debug = read_debug_block(f); break;
        public bool Debug => Magic  == 0x0A0AF1EF;

        public ushort PtrOffset => BitConverter.ToUInt16(Raw, 0x08);
        public ushort PtrCount => BitConverter.ToUInt16(Raw, 0x0A);

        public int ScriptInstructionStart => BitConverter.ToInt32(Raw, 0x0C);
        public int ScriptMovementStart => BitConverter.ToInt32(Raw, 0x10);
        public int FinalOffset => BitConverter.ToInt32(Raw, 0x14);
        public int AllocatedMemory => BitConverter.ToInt32(Raw, 0x18);

        // Generated Attributes
        public int CompressedLength => Length - ScriptInstructionStart;
        public byte[] CompressedBytes => Raw.Skip(ScriptInstructionStart).ToArray();
        public int DecompressedLength => FinalOffset - ScriptInstructionStart;
        public uint[] DecompressedInstructions => Scripts.QuickDecompress(CompressedBytes, DecompressedLength/4);

        public uint[] ScriptCommands => DecompressedInstructions.Take((ScriptMovementStart - ScriptInstructionStart) / 4).ToArray();
        public uint[] MoveCommands => DecompressedInstructions.Skip((ScriptMovementStart - ScriptInstructionStart) / 4).ToArray();
        public string[] ParseScript => Scripts.ParseScript(ScriptCommands);
        public string[] ParseMoves => Scripts.ParseMovement(MoveCommands);

        public string Info => "Data Start: 0x" + ScriptInstructionStart.ToString("X4")
                              + Environment.NewLine + "Movement Offset: 0x" + ScriptMovementStart.ToString("X4")
                              + Environment.NewLine + "Total Used Size: 0x" + FinalOffset.ToString("X4")
                              + Environment.NewLine + "Reserved Size: 0x" + AllocatedMemory.ToString("X4")
                              + Environment.NewLine + "Compressed Len: 0x" + CompressedLength.ToString("X4")
                              + Environment.NewLine + "Decompressed Len: 0x" + DecompressedLength.ToString("X4")
                              + Environment.NewLine + "Compression Ratio: " +
                              ((DecompressedLength - CompressedLength)/(decimal)DecompressedLength).ToString("p1");

        public byte[] Raw;

        public Script(byte[] data = null)
        {
            Raw = data ?? Array.Empty<byte>();

            // sub_51AAFC
            if ((Raw[8] & 1) != 0)
                throw new ArgumentException("Multi-environment script!?");
        }

        public byte[] Write()
        {
            return Raw;
        }
    }
}