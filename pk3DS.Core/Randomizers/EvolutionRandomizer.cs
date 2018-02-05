using pk3DS.Core.Structures;

namespace pk3DS.Core.Randomizers
{
    public class EvolutionRandomizer : IRandomizer
    {
        private readonly EvolutionSet[] Evolutions;
        private readonly GameConfig Config;

        public readonly SpeciesRandomizer Randomizer;

        public EvolutionRandomizer(GameConfig config, EvolutionSet[] evolutions)
        {
            Config = config;
            Evolutions = evolutions;
            Randomizer = new SpeciesRandomizer(Config);
        }


        public void Execute()
        {
            for (var i = 0; i < Evolutions.Length; i++)
            {
                var evo = Evolutions[i];
                Randomize(evo, i);
            }
        }

        public void ExecuteTrade()
        {
            for (var i = 0; i < Evolutions.Length; i++)
            {
                var evo = Evolutions[i];
                Trade(evo, i);
            }
        }

        private void Randomize(EvolutionSet evo, int i)
        {
            var evos = evo.PossibleEvolutions;
            foreach (EvolutionMethod v in evos)
            {
                if (v.Method > 0)
                    v.Species = Randomizer.GetRandomSpecies(v.Species, i);
            }
        }

        private void Trade(EvolutionSet evo, int i)
        {
            var evos = evo.PossibleEvolutions;
            foreach (EvolutionMethod v in evos)
            {
                if ((Config.XY || Config.ORAS) && v.Method == 5) // Gen 6 uses Argument rather than Level
                {
                    if (i == 708 || i == 710) // Phantump/Pumpkaboo
                        v.Argument = 20;
                    else
                        v.Argument = 30;
                    v.Method = 4; // trade -> level up
                }

                if ((Config.SM || Config.USUM) && v.Method == 5)
                {
                    if (i == 708 || i == 710 || i == 876 || i == 877 | i == 878) // Phantump/Pumpkaboo forms
                        v.Level = 20;
                    else
                        v.Level = 30;
                    v.Method = 4; // trade -> level up
                }
                
                if (v.Method == 6) // trade with held item -> level up with held item
                    v.Method = 19;

                if (v.Method == 7) // trade for opposite -> level up with party
                {
                    if (i == 588)
                        v.Argument = 616; // Karrablast with Shelmet
                    if (i == 616)
                        v.Argument = 588; // Shelmet with Karrablast
                    v.Method = 22;
                }
            }
        }
    }
}
