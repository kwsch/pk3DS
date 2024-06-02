using pk3DS.Core.Structures;

namespace pk3DS.Core.Randomizers;

public class EggMoveRandomizer(GameConfig config, EggMoves[] sets) : IRandomizer
{
    /*
     * 3111 Egg Moves Learned by 290 Species (10.73 avg)
     * 18 is the most
     * 1000 moves learned were STAB (32.1%)
     */
    private readonly MoveRandomizer moverand = new(config);

    public bool Expand = true;
    public int ExpandTo = 18;
    public bool STAB { set => moverand.rSTAB = value; }
    public int[] BannedMoves { set => moverand.BannedMoves = value; }
    public decimal STABPercent { set => moverand.rSTABPercent = value; }

    public void Execute()
    {
        if (sets[0] is EggMoves6)
        {
            for (int i = 0; i < sets.Length; i++)
                Randomize(sets[i], i);
        }
        else if (sets[0] is EggMoves7)
        {
            for (int i = 0; i <= config.MaxSpeciesID; i++)
            {
                Randomize(sets[i], i);
                int formoff = ((EggMoves7)sets[i]).FormTableIndex;
                int count = config.Personal[i].FormeCount;
                for (int j = 1; j < count; j++)
                    Randomize(sets[formoff + j - 1], config.Personal.GetFormIndex(i, j));
            }
        }
    }

    private void Randomize(EggMoves eggMoves, int index)
    {
        int count = Expand ? ExpandTo : eggMoves.Count;
        eggMoves.Moves = GetRandomMoves(count, index);
    }

    private int[] GetRandomMoves(int count, int index)
    {
        count = Expand ? ExpandTo : count;
        int[] moves = new int[count];
        moverand.GetRandomLearnset(index, count).CopyTo(moves, 0);
        return moves;
    }
}