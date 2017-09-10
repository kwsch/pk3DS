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

        private void Randomize(EvolutionSet evo, int i)
        {
            var evos = evo.PossibleEvolutions;
            foreach (EvolutionMethod v in evos)
            {
                if (v.Method > 0)
                    v.Species = Randomizer.GetRandomSpecies(v.Species, i);
            }
        }
    }
}
