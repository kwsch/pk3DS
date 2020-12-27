using pk3DS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using pk3DS.Core.Randomizers;

namespace pk3DS
{
    public partial class TrainerRand : Form
    {
        public TrainerRand()
        {
            InitializeComponent();
            CB_Moves.SelectedIndex = 1;
            var trClassnorep = new List<string>();
            foreach (string tclass in trClass.Where(tclass => !trClassnorep.Contains(tclass) && !tclass.StartsWith("[~")))
                trClassnorep.Add(tclass);
            trClassnorep.Sort();
            RandSettings.GetFormSettings(this, Controls);
        }

        //private readonly string[] trName = Main.Config.GetText(TextName.TrainerNames);
        private readonly string[] trClass = Main.Config.GetText(TextName.TrainerClasses);
        //private readonly List<string> trClassnorep;
        private static readonly int[] Legendary = Legal.Legendary_6;
        private static readonly int[] Mythical = Legal.Mythical_6;

        private void B_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize all? Cannot undo.", "Double check Randomization settings before continuing.") != DialogResult.Yes)
                return;

            RSTE.rPKM = CHK_RandomPKM.Checked;
            RSTE.rSmart = CHK_BST.Checked;
            RSTE.rLevel = CHK_Level.Checked;
            RSTE.rLevelMultiplier = NUD_Level.Value;
            RSTE.rNoFixedDamage = CHK_NoFixedDamage.Checked;

            RSTE.rMove = CB_Moves.SelectedIndex == 1;
            RSTE.rNoMove = CB_Moves.SelectedIndex == 2;
            RSTE.rMetronome = CB_Moves.SelectedIndex == 3;
            if (RSTE.rMove)
            {
                RSTE.rDMG = CHK_Damage.Checked;
                if (RSTE.rDMG)
                    RSTE.rDMGCount = (int)NUD_Damage.Value;
                RSTE.rSTAB = CHK_STAB.Checked;
                if (RSTE.rSTAB)
                    RSTE.rSTABCount = (int)NUD_STAB.Value;
            }
            RSTE.rItem = CHK_RandomItems.Checked;
            RSTE.rAbility = CHK_RandomAbilities.Checked;
            RSTE.rDiffIV = CHK_MaxDiffPKM.Checked;

            RSTE.rClass = CHK_RandomClass.Checked;
            if (RSTE.rClass)
            {
                RSTE.rIgnoreClass = CHK_IgnoreSpecialClass.Checked
                    ? Main.Config.ORAS
                        ? Legal.SpecialClasses_ORAS
                        : Legal.SpecialClasses_XY
                    : Array.Empty<int>();
                RSTE.rOnlySingles = CHK_OnlySingles.Checked;
            }
            RSTE.rGift = CHK_RandomGift.Checked;
            RSTE.rGiftPercent = NUD_GiftPercent.Value;
            RSTE.rDiffAI = CHK_MaxDiffAI.Checked;
            RSTE.rTypeTheme = CHK_TypeTheme.Checked;
            RSTE.rTypeGymTrainers = CHK_GymTrainers.Checked;
            RSTE.rGymE4Only = CHK_GymE4Only.Checked;
            RSTE.rMinPKM = NUD_RMin.Value;
            RSTE.rMaxPKM = NUD_RMax.Value;
            RSTE.r6PKM = CHK_6PKM.Checked;
            RSTE.rRandomMegas = CHK_RandomMegaForm.Checked;
            RSTE.rForceFullyEvolved = CHK_ForceFullyEvolved.Checked;
            RSTE.rForceFullyEvolvedLevel = NUD_ForceFullyEvolved.Value;
            RSTE.rForceHighPower = CHK_ForceHighPower.Checked;
            RSTE.rForceHighPowerLevel = NUD_ForceHighPower.Value;

            if (CHK_StoryMEvos.Checked)
            {
                RSTE.rEnsureMEvo = Main.Config.ORAS
                    ? new [] { 178, 235, 557, 583, 687, 698, 699, 700, 701, 713, 906, 907, 908, 909, 910, 911, 912, 913, 942, 944, 946 }
                    : new [] { 188, 263, 276, 277, 519, 520, 521, 526, 599, 600, 601 };
            }
            else
            {
                RSTE.rEnsureMEvo = Array.Empty<int>();
            }

            RSTE.rThemedClasses = new bool[trClass.Length];
            RSTE.rSpeciesRand = new SpeciesRandomizer(Main.Config)
            {
                G1 = CHK_G1.Checked,
                G2 = CHK_G2.Checked,
                G3 = CHK_G3.Checked,
                G4 = CHK_G4.Checked,
                G5 = CHK_G5.Checked,
                G6 = CHK_G6.Checked,

                L = CHK_L.Checked,
                E = CHK_E.Checked,
                Shedinja = true,

                rBST = CHK_BST.Checked,
                rEXP = false,
            };
            RSTE.rSpeciesRand.Initialize();

            // add Legendary/Mythical to final evolutions if checked
            if (CHK_L.Checked) RSTE.rFinalEvo = RSTE.rFinalEvo.Concat(Legendary).ToArray();
            if (CHK_E.Checked) RSTE.rFinalEvo = RSTE.rFinalEvo.Concat(Mythical).ToArray();

            RSTE.rDoRand = true;
            RandSettings.SetFormSettings(this, Controls);
            Close();
        }

        private void CHK_RandomPKM_CheckedChanged(object sender, EventArgs e)
        {
            GB_Tweak.Enabled =
                CHK_G1.Checked = CHK_G2.Checked = CHK_G3.Checked =
                CHK_G4.Checked = CHK_G5.Checked = CHK_G6.Checked =
                CHK_L.Checked = CHK_E.Checked = CHK_StoryMEvos.Checked = CHK_ForceFullyEvolved.Checked =
                CHK_RandomPKM.Checked;

            CHK_TypeTheme.Checked = CHK_GymTrainers.Checked = CHK_GymE4Only.Checked =
                CHK_BST.Checked = CHK_6PKM.Checked = CHK_RandomMegaForm.Checked = false; // Off by default
        }

        private void CHK_Level_CheckedChanged(object sender, EventArgs e)
        {
            NUD_Level.Enabled = CHK_Level.Checked;
        }

        private void ChangeLevelPercent(object sender, EventArgs e)
        {
            CHK_Level.Checked = NUD_Level.Value != 0;
        }

        private void CHK_RandomGift_CheckedChanged(object sender, EventArgs e)
        {
            NUD_GiftPercent.Enabled = CHK_RandomGift.Checked;
            NUD_GiftPercent.Value = Convert.ToDecimal(CHK_RandomGift.Checked) * 15;
        }

        private void ChangeGiftPercent(object sender, EventArgs e)
        {
            CHK_RandomGift.Checked = NUD_GiftPercent.Value != 0;
        }

        private void CHK_TypeTheme_CheckedChanged(object sender, EventArgs e)
        {
            CHK_GymTrainers.Enabled = CHK_GymTrainers.Checked = CHK_GymE4Only.Enabled = CHK_TypeTheme.Checked;
            if (!CHK_TypeTheme.Checked)
                CHK_GymTrainers.Checked = CHK_GymE4Only.Checked = false;
        }

        private void CHK_RandomClass_CheckedChanged(object sender, EventArgs e)
        {
            CHK_IgnoreSpecialClass.Enabled = CHK_IgnoreSpecialClass.Checked =
            CHK_OnlySingles.Enabled = CHK_OnlySingles.Checked = CHK_RandomClass.Checked;
        }

        private void ChangeMoveRandomization(object sender, EventArgs e)
        {
            CHK_Damage.Checked = CHK_STAB.Checked =
            CHK_Damage.Enabled = CHK_STAB.Enabled =
            NUD_Damage.Enabled = NUD_STAB.Enabled = CB_Moves.SelectedIndex == 1;

            CHK_ForceHighPower.Enabled = CHK_ForceHighPower.Checked = NUD_ForceHighPower.Enabled =
            CHK_NoFixedDamage.Enabled = CHK_NoFixedDamage.Checked = (CB_Moves.SelectedIndex == 1 || CB_Moves.SelectedIndex == 2);
        }

        private void CHK_6PKM_CheckedChanged(object sender, EventArgs e)
        {
            //if (CB_Moves.SelectedIndex == 0)
            //    CHK_6PKM.Checked = false;
        }
    }
}