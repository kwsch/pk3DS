using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class TrainerRand : Form
    {
        public TrainerRand()
        {
            InitializeComponent();
            CB_Moves.SelectedIndex = 1;
            trClassnorep = new List<string>();
            foreach (string tclass in trClass.Where(tclass => !trClassnorep.Contains(tclass) && !tclass.StartsWith("[~")))
                trClassnorep.Add(tclass);
            trClassnorep.Sort();
        }

        private string[] trName = Main.getText(Main.oras ? 22 : 21);
        private readonly string[] trClass = Main.getText(Main.oras ? 21 : 20);
        private readonly List<string> trClassnorep;

        private void B_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void B_Save_Click(object sender, EventArgs e)
        {
            RSTE.rPKM = CHK_RandomPKM.Checked;
            RSTE.sL = Randomizer.getSpeciesList(CHK_G1.Checked, CHK_G2.Checked, CHK_G3.Checked, CHK_G4.Checked, CHK_G5.Checked, CHK_G6.Checked, CHK_L.Checked, CHK_E.Checked, ModifierKeys == Keys.Control);
            RSTE.rSmart = CHK_BST.Checked;
            RSTE.rLevel = CHK_Level.Checked;
            RSTE.rLevelPercent = NUD_Level.Value;

            RSTE.rMove = CB_Moves.SelectedIndex == 1;
            RSTE.rNoMove = CB_Moves.SelectedIndex == 2;
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
                    ? Main.oras
                        ? Legal.SpecialClasses_ORAS
                        : Legal.SpecialClasses_XY
                    : new int[] {};
                RSTE.rOnlySingles = CHK_OnlySingles.Checked;
            }
            RSTE.rGift = CHK_RandomGift.Checked;
            RSTE.rGiftPercent = NUD_GiftPercent.Value;
            RSTE.rDiffAI = CHK_MaxDiffAI.Checked;
            RSTE.rTypeTheme = CHK_TypeTheme.Checked;
            RSTE.rTypeGymTrainers = CHK_GymTrainers.Checked;
            RSTE.rGymE4Only = CHK_GymE4Only.Checked;
            RSTE.r6PKM = CHK_6PKM.Checked;
            RSTE.rRandomMegas = CHK_RandomMegaForm.Checked;

            if (CHK_StoryMEvos.Checked)
            {
                RSTE.rEnsureMEvo = Main.oras 
                    ? new [] { 178, 235, 557, 583, 687, 698, 699, 700, 701, 713, 906, 907, 908, 909, 910, 911, 912, 913, 942, 944, 946 } 
                    : new [] { 188, 263, 276, 277, 519, 520, 521, 526, 599, 600, 601 };
            }
            else
            {
                RSTE.rEnsureMEvo = new int[] { };
            }
            
            RSTE.rThemedClasses = new bool[trClass.Length];

            RSTE.rDoRand = true;
            Close();
        }

        private void CHK_RandomPKM_CheckedChanged(object sender, EventArgs e)
        {
            GB_Tweak.Enabled = 
                CHK_G1.Checked = CHK_G2.Checked = CHK_G3.Checked = 
                CHK_G4.Checked = CHK_G5.Checked = CHK_G6.Checked = 
                CHK_L.Checked = CHK_E.Checked = CHK_StoryMEvos.Checked = 
                CHK_RandomPKM.Checked;

            CHK_TypeTheme.Checked = CHK_GymTrainers.Checked = CHK_GymE4Only.Checked = 
                CHK_BST.Checked = CHK_6PKM.Checked = CHK_RandomMegaForm.Checked = false; // Off by default
        }

        private void CHK_Level_CheckedChanged(object sender, EventArgs e)
        {
            NUD_Level.Enabled = CHK_Level.Checked;
            NUD_Level.Value = Convert.ToDecimal(CHK_Level.Checked) * 50;
        }
        private void changeLevelPercent(object sender, EventArgs e)
        {
            CHK_Level.Checked = NUD_Level.Value != 0;
        }
        private void CHK_RandomGift_CheckedChanged(object sender, EventArgs e)
        {
            NUD_GiftPercent.Enabled = CHK_RandomGift.Checked;
            NUD_GiftPercent.Value = Convert.ToDecimal(CHK_RandomGift.Checked) * 15;
        }
        private void changeGiftPercent(object sender, EventArgs e)
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

        private void changeMoveRandomization(object sender, EventArgs e)
        {
            CHK_Damage.Checked = CHK_STAB.Checked =
            CHK_Damage.Enabled = CHK_STAB.Enabled =
            NUD_Damage.Enabled = NUD_STAB.Enabled = CB_Moves.SelectedIndex == 1;

            if (CB_Moves.SelectedIndex == 0)
                CHK_6PKM.Checked = false;
        }
        private void CHK_6PKM_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_Moves.SelectedIndex == 0)
                CHK_6PKM.Checked = false;
        }
    }
}