using System;
using System.Linq;
using pk3DS.Core.Structures;

namespace pk3DS.Core.Randomizers
{
    // https://twitter.com/Drayano60/status/807297858244411397
    // ORAS: 10682 moves learned on levelup/birth. 
    // 5593 are STAB. 52.3% are STAB. 
    // Steelix learns the most @ 25 (so many level 1)!
    // Move relearner ingame glitch fixed (52 tested), but keep below 75
    public class LearnsetRandomizer : IRandomizer
    {
        private readonly MoveRandomizer moverand;
        private readonly GameConfig Config;
        private readonly Learnset[] Learnsets;

        public LearnsetRandomizer(GameConfig config, Learnset[] sets)
        {
            Config = config;
            moverand = new MoveRandomizer(config);
            Learnsets = sets;
        }

        public bool Expand = true;
        public int ExpandTo = 25;
        public bool Spread = true;
        public int SpreadTo = 75;

        public bool STAB { set => moverand.rSTAB = value; }
        public int[] BannedMoves { set => moverand.BannedMoves = value; }
        public decimal rSTABPercent = 52.3m;
        
        public void Execute()
        {
            for (var i = 0; i < Learnsets.Length; i++)
                Randomize(Learnsets[i], i);
        }

        private void Randomize(Learnset set, int index)
        {
            int[] moves = GetRandomMoves(set.Count, index);
            int[] levels = GetRandomLevels(set, moves.Length);

            set.Moves = moves;
            set.Levels = levels;
        }

        private int[] GetRandomLevels(Learnset set, int count)
        {
            int[] levels = new int[count];
            if (Spread)
            {
                levels[0] = 1;
                decimal increment = SpreadTo / (decimal)count;
                for (int i = 1; i < count; i++)
                    levels[i] = (int)(i * increment);
                return set.Levels;
            }
            if (levels.Length == count)
                return set.Levels;

            var exist = set.Levels;
            int lastlevel = exist[exist.Length - 1];
            exist.CopyTo(levels, 0);
            for (int i = exist.Length; i < levels.Length; i++)
                levels[i] = Math.Max(100, lastlevel + (exist.Length - i + 1));

            return levels;
        }

        private int[] GetRandomMoves(int count, int index)
        {
            count = Expand ? ExpandTo : count;
            moverand.rSTABCount = (int)(count * rSTABPercent / 100);

            int[] moves = new int[count];
            if (count == 0)
                return moves;
            moves[0] = moverand.GetRandomFirstMove(index);
            moverand.GetRandomLearnset(index, count - 1).CopyTo(moves, 1);
            return moves;
        }

        public int[] GetHighPoweredMoves(int species, int form, int count = 4)
        {
            int index = Config.Personal.getFormeIndex(species, form);
            var moves = Learnsets[index].Moves.OrderByDescending(move => Config.Moves[move].Power).Distinct().Take(count).ToArray();
            Array.Resize(ref moves, count);
            return moves;
        }

        public int[] GetCurrentMoves(int species, int form, int level, int count = 4)
        {
            int i = Config.Personal.getFormeIndex(species, form);
            var moves = Learnsets[i].getCurrentMoves(level);
            Array.Resize(ref moves, count);
            return moves;
        }
    }
}
