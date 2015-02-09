using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class TrainerRand : Form
    {
        public TrainerRand()
        {
            InitializeComponent();
        }

        private void B_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void B_Save_Click(object sender, EventArgs e)
        {
            RSTE.rPKM = CHK_RandomPKM.Checked;
            RSTE.rSmart = CHK_Smart.Checked;
            RSTE.rLevel = CHK_Level.Checked;
            RSTE.rLevelPercent = NUD_Level.Value;

            RSTE.rMove = CHK_RandomMoves.Checked;
            RSTE.rItem = CHK_RandomItems.Checked;
            RSTE.rAbility = CHK_RandomAbilities.Checked;
            RSTE.rDiffIV = CHK_MaxDiffPKM.Checked;

            RSTE.rClass = CHK_RandomClass.Checked;
            RSTE.rGift = CHK_RandomGift.Checked;
            RSTE.rGiftPercent = NUD_GiftPercent.Value;
            RSTE.rDiffAI = CHK_MaxDiffAI.Checked;

            RSTE.rDoRand = true;
            this.Close();
        }

        private void CHK_RandomPKM_CheckedChanged(object sender, EventArgs e)
        {
            CHK_Smart.Enabled = CHK_Smart.Checked = CHK_RandomPKM.Checked;
        }

        private void CHK_Level_CheckedChanged(object sender, EventArgs e)
        {
            NUD_Level.Enabled = CHK_Level.Checked;
            NUD_Level.Value = Convert.ToDecimal(CHK_RandomGift.Checked) * 50;
        }
        private void changeLevelPercent(object sender, EventArgs e)
        {
            CHK_Level.Checked = (NUD_Level.Value != 0);
        }
        private void CHK_RandomGift_CheckedChanged(object sender, EventArgs e)
        {
            NUD_GiftPercent.Enabled = CHK_RandomGift.Checked;
            NUD_GiftPercent.Value = Convert.ToDecimal(CHK_RandomGift.Checked) * 15;
        }
        private void changeGiftPercent(object sender, EventArgs e)
        {
            CHK_RandomGift.Checked = (NUD_GiftPercent.Value != 0);
        }
    }
}