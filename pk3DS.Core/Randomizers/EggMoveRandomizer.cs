using pk3DS.Core.Structures;

namespace pk3DS.Core.Randomizers
{
    public class EggMoveRandomizer : IRandomizer
    {
        /*
         * 3111 Egg Moves Learned by 290 Species (10.73 avg)
         * 18 is the most
         * 1000 moves learned were STAB (32.1%)
         */
        private readonly MoveRandomizer moverand;
        private readonly GameConfig Config;
        private readonly EggMoves[] Sets;
        public EggMoveRandomizer(GameConfig config, EggMoves[] sets)
        {
            Config = config;
            Sets = sets;
            moverand = new MoveRandomizer(config);
        }

        public bool Expand = true;
        public int ExpandTo = 18;
        public bool STAB { set => moverand.rSTAB = value; }
        public int[] BannedMoves { set => moverand.BannedMoves = value; }
        public decimal rSTABPercent = 32.1m;

        public void Execute()
        {
            for (int i = 0; i < Sets.Length; i++)
                Randomize(Sets[i], i);
        }

        private void Randomize(EggMoves eggMoves, int index)
        {
            int count = Expand ? ExpandTo : eggMoves.Count;
            eggMoves.Moves = GetRandomMoves(count, index);
        }

        private int[] GetRandomMoves(int count, int index)
        {
            count = Expand ? ExpandTo : count;
            moverand.rSTABCount = (int)(count * rSTABPercent / 100);

            int[] moves = new int[count];
            moverand.GetRandomLearnset(index, count).CopyTo(moves, 0);
            return moves;
        }
    }
}
