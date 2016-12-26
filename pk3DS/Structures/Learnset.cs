using System.IO;
using System.Linq;

namespace pk3DS
{
    public abstract class Learnset
    {
        public int Count;
        public int[] Moves;
        public int[] Levels;

        public int[] getMoves(int level)
        {
            return Moves.TakeWhile((move, i) => Levels[i] <= level).Distinct().ToArray();
        }
        public int[] getCurrentMoves(int level)
        {
            return getMoves(level).Reverse().Take(4).Reverse().ToArray();
        }
        public abstract byte[] Write();
    }
    public class Learnset6 : Learnset
    {
        public Learnset6(byte[] data)
        {
            if (data.Length < 4 || data.Length % 4 != 0)
            { Count = 0; Levels = new int[0]; Moves = new int[0]; return; }
            Count = data.Length / 4 - 1;
            Moves = new int[Count];
            Levels = new int[Count];
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                for (int i = 0; i < Count; i++)
                {
                    Moves[i] = br.ReadInt16();
                    Levels[i] = br.ReadInt16();
                }
        }
        public static Learnset[] getArray(byte[][] entries)
        {
            Learnset[] data = new Learnset[entries.Length];
            for (int i = 0; i < data.Length; i++)
                data[i] = new Learnset6(entries[i]);
            return data;
        }
        public override byte[] Write()
        {
            Count = (ushort)Moves.Length;
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                for (int i = 0; i < Count; i++)
                {
                    bw.Write((short)Moves[i]);
                    bw.Write((short)Levels[i]);
                }
                bw.Write(-1);
                return ms.ToArray();
            }
        }
    }
    public class Learnset7 : Learnset
    {
        public Learnset7(byte[] data)
        {
            if (data.Length < 4 || data.Length % 4 != 0)
            { Count = 0; Levels = new int[0]; Moves = new int[0]; return; }
            Count = data.Length / 4 - 1;
            Moves = new int[Count];
            Levels = new int[Count];
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                for (int i = 0; i < Count; i++)
                {
                    Moves[i] = br.ReadInt16();
                    Levels[i] = br.ReadInt16();
                }
        }
        public static Learnset[] getArray(byte[][] entries)
        {
            Learnset[] data = new Learnset[entries.Length];
            for (int i = 0; i < data.Length; i++)
                data[i] = new Learnset7(entries[i]);
            return data;
        }
        public override byte[] Write()
        {
            Count = (ushort)Moves.Length;
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                for (int i = 0; i < Count; i++)
                {
                    bw.Write((short)Moves[i]);
                    bw.Write((short)Levels[i]);
                }
                bw.Write(-1);
                return ms.ToArray();
            }
        }
    }
}
