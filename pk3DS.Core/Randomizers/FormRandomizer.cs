using System.Linq;
using pk3DS.Core.Structures.PersonalInfo;

namespace pk3DS.Core.Randomizers
{
    public class FormRandomizer
    {
        private readonly GameConfig Game;
        public FormRandomizer(GameConfig game)
        {
            Game = game;
        }

        public bool AllowMega = false;
        public bool AllowAlolanForm = true;

        public int GetRandomForme(int species, PersonalInfo[] stats = null)
        {
            if (stats == null)
                stats = Game.Personal.Table;
            if (stats[species].FormeCount <= 1)
                return 0;

            switch (species)
            {
                case 664: case 665: case 666: // Vivillon evo chain
                    return 30; // save file specific
                case 774: // Minior
                    return (int)(Util.rnd32() % 7);
            }

            if (AllowAlolanForm && Legal.EvolveToAlolanForms.Contains(species))
                return (int)(Util.rnd32() % 2);
            if (!Legal.Mega_ORAS.Contains((ushort)species) || AllowMega)
                return (int)(Util.rnd32() % stats[species].FormeCount); // Slot-Random
            return 0;
        }
    }
}
