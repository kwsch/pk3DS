using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pk3DS.Core.Structures.PersonalInfo;

namespace pk3DS.Core.Randomizers
{
    public class PersonalRandomizer : IRandomizer
    {
        private static readonly Random rnd = new Random();

        private const decimal LearnTMPercent = 35; // Average Learnable TMs is 35.260.
        private const decimal LearnTypeTutorPercent = 2; //136 special tutor moves learnable by species in Untouched ORAS.
        private const decimal LearnMoveTutorPercent = 30; //10001 tutor moves learnable by 826 species in Untouched ORAS.
        private const int tmcount = 100;
        private const int eggGroupCount = 16;

        private readonly GameConfig Game;
        private readonly PersonalInfo[] Table;

        // Randomization Settings
        public int TypeCount;
        public bool ModifyCatchRate = true;
        public bool ModifyLearnsetTM = true;
        public bool ModifyLearnsetHM = true;
        public bool ModifyLearnsetTypeTutors = true;
        public bool ModifyLearnsetMoveTutors = true;
        public bool ModifyHeldItems = true;

        public bool ModifyAbilities = true;
        public bool AllowWonderGuard = true;

        public bool ModifyStats = true;
        public decimal StatDeviation = 25;
        public bool[] StatsToRandomize = { true, true, true, true, true, true };

        public bool ModifyTypes = true;
        public decimal SameTypeChance = 50;
        public bool ModifyEggGroup = true;
        public decimal SameEggGroupChance = 50;

        public PersonalRandomizer(PersonalInfo[] table, GameConfig game)
        {
            Game = game;
            Table = table;
            if (File.Exists("bannedabilites.txt"))
            {
                var data = File.ReadAllLines("bannedabilities.txt");
                var list = new List<int>(BannedAbilities);
                list.AddRange(data.Select(z => Convert.ToInt32(z)));
                BannedAbilities = list;
            }
        }

        public void Execute()
        {
            for (var i = 0; i < Table.Length; i++)
                Randomize(Table[i], i);
        }

        public void Randomize(PersonalInfo z, int index)
        {
            // Fiddle with Learnsets
            if (ModifyLearnsetTM || ModifyLearnsetHM)
                RandomizeTMHM(z);
            if (ModifyLearnsetTypeTutors)
                RandomizeTypeTutors(z, index);
            if (ModifyLearnsetMoveTutors)
                RandomizeSpecialTutors(z);
            if (ModifyStats)
                RandomizeStats(z);
            if (ModifyAbilities)
                RandomizeAbilities(z);
            if (ModifyEggGroup)
                RandomizeEggGroups(z);
            if (ModifyHeldItems)
                RandomizeHeldItems(z);
            if (ModifyTypes)
                RandomizeTypes(z);
            if (ModifyCatchRate)
                z.CatchRate = rnd.Next(3, 251); // Random Catch Rate between 3 and 250.
        }

        private void RandomizeTMHM(PersonalInfo z)
        {
            var tms = z.TMHM;

            if (ModifyLearnsetTM)
            for (int j = 0; j < tmcount; j++)
                tms[j] = rnd.Next(0, 100) < LearnTMPercent;

            if (ModifyLearnsetHM)
            for (int j = tmcount; j < tms.Length; j++)
                tms[j] = rnd.Next(0, 100) < LearnTMPercent;

            z.TMHM = tms;
        }
        private void RandomizeTypeTutors(PersonalInfo z, int index)
        {
            var t = z.TypeTutors;
            for (int i = 0; i < t.Length; i++)
                t[i] = rnd.Next(0, 100) < LearnTypeTutorPercent;

            // Make sure Rayquaza can learn Dragon Ascent.
            if (!Game.XY && (index == 384 || index == 814))
                t[7] = true;

            z.TypeTutors = t;
        }
        private void RandomizeSpecialTutors(PersonalInfo z)
        {
            var tutors = z.SpecialTutors;
            foreach (bool[] tutor in tutors)
                for (int i = 0; i < tutor.Length; i++)
                    tutor[i] = rnd.Next(0, 100) < LearnMoveTutorPercent;
            z.SpecialTutors = tutors;
        }
        private void RandomizeAbilities(PersonalInfo z)
        {
            var abils = z.Abilities;
            for (int i = 0; i < abils.Length; i++)
                abils[i] = GetRandomAbility();
            z.Abilities = abils;
        }
        private void RandomizeEggGroups(PersonalInfo z)
        {
            var egg = z.EggGroups;
            egg[0] = GetRandomEggGroup();
            egg[1] = rnd.Next(0, 100) < SameEggGroupChance ? egg[0] : GetRandomEggGroup();
            z.EggGroups = egg;
        }
        private void RandomizeHeldItems(PersonalInfo z)
        {
            var item = z.Items;
            for (int j = 0; j < item.Length; j++)
                item[j] = GetRandomHeldItem();
            z.Items = item;
        }
        private void RandomizeTypes(PersonalInfo z)
        {
            var t = z.Types;
            t[0] = GetRandomType();
            t[1] = rnd.Next(0, 100) < SameTypeChance ? t[0] : GetRandomType();
            z.Types = t;
        }
        private void RandomizeStats(PersonalInfo z)
        {
            // Fiddle with Base Stats, don't muck with Shedinja.
            var stats = z.Stats;
            if (stats[0] == 1)
                return;
            for (int i = 0; i < stats.Length; i++)
            {
                if (!StatsToRandomize[i])
                    continue;
                var l = Math.Min(255, (int) (stats[i] * (1 - StatDeviation / 100)));
                var h = Math.Min(255, (int) (stats[i] * (1 + StatDeviation / 100)));
                stats[i] = Math.Max(5, rnd.Next(l, h));
            }
            z.Stats = stats;
        }

        private int GetRandomType() => rnd.Next(0, TypeCount);
        private int GetRandomEggGroup() => rnd.Next(1, eggGroupCount);
        private int GetRandomHeldItem() => Game.Info.HeldItems[rnd.Next(1, Game.Info.HeldItems.Length)];
        private readonly IList<int> BannedAbilities = new int[0];
        private int GetRandomAbility()
        {
            const int WonderGuard = 25;
            int newabil;
            do newabil = rnd.Next(1, Game.Info.MaxAbilityID + 1);
            while (newabil == WonderGuard && !AllowWonderGuard || BannedAbilities.Contains(newabil));
            return newabil;
        }
    }
}
