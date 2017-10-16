using System;
using System.Collections.Generic;
using System.Linq;
using pk3DS.Core.Structures;
using pk3DS.Core.Structures.PersonalInfo;

namespace pk3DS.Core.Randomizers
{
    public class MoveRandomizer : IRandomizer
    {
        private readonly GameConfig Config;
        private readonly int MaxMoveID;
        private readonly Move[] MoveData;
        private readonly PersonalInfo[] SpeciesStat;

        private readonly GenericRandomizer RandMove;

        public MoveRandomizer(GameConfig config)
        {
            Config = config;
            MaxMoveID = config.Info.MaxMoveID;
            MoveData = config.Moves;
            SpeciesStat = config.Personal.Table;
            RandMove = new GenericRandomizer(Enumerable.Range(1, MaxMoveID-1).ToArray());
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public bool rDMG = true;
        public int rDMGCount = 2;
        public bool rSTAB = true;
        public int rSTABCount = 2;
        public decimal rSTABPercent = 100;
        public IList<int> BannedMoves = new int[0];

        public static readonly int[] FixedDamageMoves = { 49, 82 };

        private int loopctr = 0;

        public int[] GetRandomLearnset(int index, int movecount) => GetRandomLearnset(SpeciesStat[index].Types, movecount);
        public int[] GetRandomLearnset(int[] Types, int movecount)
        {
            var oldSTABCount = rSTABCount;
            rSTABCount = (int)(rSTABPercent * movecount / 100);
            int[] moves = GetRandomMoveset(Types, movecount);
            rSTABCount = oldSTABCount;
            return moves;
        }
        public int[] GetRandomMoveset(int index, int movecount = 4) => GetRandomMoveset(SpeciesStat[index].Types, movecount);
        public int[] GetRandomMoveset(int[] Types, int movecount = 4)
        {
            loopctr = 0;
            const int maxLoop = 666;

            int[] moves;
            do { moves = GetRandomMoves(Types, movecount); }
            while (!IsMovesetMeetingRequirements(moves, movecount) && loopctr++ <= maxLoop);

            return moves;
        }

        private int[] GetRandomMoves(int[] Types, int movecount = 4)
        {
            int i = 0;
            int[] moves = new int[movecount];
            if (rSTAB)
            for (; i < rSTABCount; i++)
                moves[i] = GetRandomSTABMove(Types);
            for (; i < moves.Length; i++) // remainder of moves
                moves[i] = RandMove.Next();
            return moves;
        }
        private int GetRandomSTABMove(int[] types)
        {
            int move;
            do { move = RandMove.Next(); }
            while (!types.Contains(MoveData[move].Type));
            return move;
        }
        private bool IsMovesetMeetingRequirements(int[] moves, int count)
        {
            if (rDMG && rDMGCount > moves.Count(move => MoveData[move].Category != 0))
                return false;

            if (moves.Any(BannedMoves.Contains))
                return false;

            return moves.Distinct().Count() == count;
        }

        private static readonly int[] firstMoves =
        {
            1,   // Pound
            40,  // Poison Sting
            52,  // Ember
            55,  // Water Gun
            64,  // Peck
            71,  // Absorb
            84,  // Thunder Shock
            98,  // Quick Attack
            122, // Lick
            141, // Leech Life
            
        };
        private static readonly GenericRandomizer first = new GenericRandomizer(firstMoves);
        public int GetRandomFirstMoveAny()
        {
            first.Reset();
            return first.Next();
        }
        public int GetRandomFirstMove(int index) => GetRandomFirstMove(SpeciesStat[index].Types);
        public int GetRandomFirstMove(int[] types)
        {
            first.Reset();
            int ctr = 0;
            int move;
            do
            {
                move = first.Next();
                if (++ctr == firstMoves.Length)
                    return move;
            } while (!types.Contains(MoveData[move].Type));
            return move;
        }

        public bool SanitizeMovesetForBannedMoves(int[] moves, int index)
        {
            bool updated = false;
            for (int m = 0; m < moves.Length; m++)
            {
                if (!BannedMoves.Contains(moves[m]))
                    continue;
                updated = true;
                moves[m] = GetRandomFirstMove(index);
            }

            return updated;
        }
    }
}
