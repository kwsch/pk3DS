using pk3DS.Core.Structures;

namespace pk3DS.Core.Randomizers
{
    public class EvolutionRandomizer : IRandomizer
    {
        private readonly EvolutionSet[] Evolutions;
        private readonly GameConfig Config;

        public readonly SpeciesRandomizer Randomizer;
        public readonly FormRandomizer FormRandomizer;

        public EvolutionRandomizer(GameConfig config, EvolutionSet[] evolutions)
        {
            Config = config;
            Evolutions = evolutions;
            Randomizer = new SpeciesRandomizer(Config);
            FormRandomizer = new FormRandomizer(config);
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

        public void ExecuteEvolveEveryLevel()
        {
            foreach (var evo in Evolutions)
                MakeEvolveEveryLevel(evo);
        }

        private void Randomize(EvolutionSet evo, int i)
        {
            var evos = evo.PossibleEvolutions;
            foreach (EvolutionMethod v in evos)
            {
                if (v.Method > 0)
                {
                    v.Species = Randomizer.GetRandomSpecies(v.Species, i);
                    v.Form = FormRandomizer.GetRandomForme(v.Species);
                }
            }
        }

        private void Trade(EvolutionSet evo, int i)
        {
            var evos = evo.PossibleEvolutions;
            foreach (EvolutionMethod v in evos)
            {
                if (Config.Generation == 6 && v.Method == 5) // Gen 6 uses Argument rather than Level
                {
                    v.Argument = 30;
                    v.Method = 4; // trade -> level up
                }
                else if (Config.Generation == 7 && v.Method == 5)
                {
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

        private void MakeEvolveEveryLevel(EvolutionSet evo)
        {
            var evos = evo.PossibleEvolutions;
            foreach (EvolutionMethod v in evos)
            {
                switch (Config.Generation)
                {
                    case 6:
                        v.Argument = 1;
                        v.Method = 4;
                        v.Species = 1;
                        break;
                    default:
                        v.Argument = 0;
                        v.Form = 0;
                        v.Level = 1;
                        v.Method = 4;
                        v.Species = 1; // will be randomized after
                        break;
                }
            }

            if (evos[1].Species != 0) // has other branched evolutions; remove them
            {
                for (int i = 1; i < evos.Length; i++)
                    evos[i] = new EvolutionMethod();
            }
        }
    }
}