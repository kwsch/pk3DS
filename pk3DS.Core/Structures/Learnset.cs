using System;
using System.Collections.Generic;
using System.IO;

namespace pk3DS.Core.Structures
{
    public abstract class Learnset
    {
        public int Count;
        public int[] Moves;
        public int[] Levels;

        public abstract byte[] Write();

        /// <summary>
        /// Returns the moves a Pokémon can learn between the specified level range.
        /// </summary>
        /// <param name="maxLevel">Maximum level</param>
        /// <param name="minLevel">Minimum level</param>
        /// <returns>Array of Move IDs</returns>
        public int[] GetMoves(int maxLevel, int minLevel = 0)
        {
            if (minLevel <= 1 && maxLevel >= 100)
                return Moves;
            if (minLevel > maxLevel)
                return Array.Empty<int>();
            int start = Array.FindIndex(Levels, z => z >= minLevel);
            if (start < 0)
                return Array.Empty<int>();
            int end = Array.FindLastIndex(Levels, z => z <= maxLevel);
            if (end < 0)
                return Array.Empty<int>();
            int[] result = new int[end - start + 1];
            Array.Copy(Moves, start, result, 0, result.Length);
            return result;
        }

        /// <summary>Returns the moves a Pokémon would have if it were encountered at the specified level.</summary>
        /// <remarks>In Generation 1, it is not possible to learn any moves lower than these encounter moves.</remarks>
        /// <param name="level">The level the Pokémon was encountered at.</param>
        /// <returns>Array of Move IDs</returns>
        public int[] GetEncounterMoves(int level)
        {
            const int count = 4;
            IList<int> moves = new int[count];
            int ctr = 0;
            for (int i = 0; i < Moves.Length; i++)
            {
                if (Levels[i] > level)
                    break;
                int move = Moves[i];
                if (moves.Contains(move))
                    continue;

                moves[ctr++] = move;
                ctr &= 3;
            }
            return (int[])moves;
        }

        /// <summary>Returns the index of the lowest level move if the Pokémon were encountered at the specified level.</summary>
        /// <remarks>Helps determine the minimum level an encounter can be at.</remarks>
        /// <param name="level">The level the Pokémon was encountered at.</param>
        /// <returns>Array of Move IDs</returns>
        public int GetMinMoveLevel(int level)
        {
            if (Levels.Length == 0)
                return 1;

            int end = Array.FindLastIndex(Levels, z => z <= level);
            return Math.Max(end - 4, 1);
        }

        /// <summary>Returns the level that a Pokémon can learn the specified move.</summary>
        /// <param name="move">Move ID</param>
        /// <returns>Level the move is learned at. If the result is below 0, it cannot be learned by levelup.</returns>
        public int GetLevelLearnMove(int move)
        {
            int index = Array.IndexOf(Moves, move);
            return index < 0 ? index : Levels[index];
        }
    }

    public class Learnset6 : Learnset
    {
        public Learnset6(byte[] data)
        {
            if (data.Length < 4 || data.Length % 4 != 0)
            { Count = 0; Levels = Array.Empty<int>(); Moves = Array.Empty<int>(); return; }
            Count = (data.Length / 4) - 1;
            Moves = new int[Count];
            Levels = new int[Count];
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            for (int i = 0; i < Count; i++)
            {
                Moves[i] = br.ReadInt16();
                Levels[i] = br.ReadInt16();
            }
        }

        public override byte[] Write()
        {
            Count = (ushort)Moves.Length;
            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);
            for (int i = 0; i < Count; i++)
            {
                bw.Write((short)Moves[i]);
                bw.Write((short)Levels[i]);
            }
            bw.Write(-1);
            return ms.ToArray();
        }

        public static Learnset[] GetArray(byte[][] entries)
        {
            Learnset[] data = new Learnset[entries.Length];
            for (int i = 0; i < data.Length; i++)
                data[i] = new Learnset6(entries[i]);
            return data;
        }
    }
}
