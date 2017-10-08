using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

using pk3DS.Properties;

using pk3DS.Core.Structures;
using pk3DS.Core;
using pk3DS.Core.Randomizers;

namespace pk3DS
{
    public partial class LevelUpEditor7 : Form
    {
        public LevelUpEditor7(byte[][] infiles)
        {
            InitializeComponent();
            files = infiles;
            string[] species = Main.Config.getText(TextName.SpeciesNames);
            string[][] AltForms = Main.Config.Personal.getFormList(species, Main.Config.MaxSpeciesID);
            string[] specieslist = Main.Config.Personal.getPersonalEntryList(AltForms, species, Main.Config.MaxSpeciesID, out baseForms, out formVal);
            specieslist[0] = movelist[0] = "";

            string[] sortedspecies = (string[])specieslist.Clone();
            Array.Resize(ref sortedspecies, Main.Config.MaxSpeciesID + 1); Array.Sort(sortedspecies);
            setupDGV();

            var newlist = new List<WinFormsUtil.cbItem>();
            for (int i = 1; i <= Main.Config.MaxSpeciesID; i++) // add all species
                newlist.Add(new WinFormsUtil.cbItem { Text = sortedspecies[i], Value = Array.IndexOf(specieslist, sortedspecies[i]) });
            for (int i = Main.Config.MaxSpeciesID + 1; i < specieslist.Length; i++) // add all forms
                newlist.Add(new WinFormsUtil.cbItem { Text = specieslist[i], Value = i });

            CB_Species.DisplayMember = "Text";
            CB_Species.ValueMember = "Value";
            CB_Species.DataSource = newlist;
            CB_Species.SelectedIndex = 0;
        }

        private readonly byte[][] files;
        private int entry = -1;
        private readonly string[] movelist = Main.Config.getText(TextName.MoveNames);
        private bool dumping;
        private readonly int[] baseForms, formVal;
        private void setupDGV()
        {
            string[] sortedmoves = (string[])movelist.Clone();
            Array.Sort(sortedmoves);
            DataGridViewColumn dgvLevel = new DataGridViewTextBoxColumn();
            {
                dgvLevel.HeaderText = "Level";
                dgvLevel.DisplayIndex = 0;
                dgvLevel.Width = 45;
                dgvLevel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            DataGridViewComboBoxColumn dgvMove = new DataGridViewComboBoxColumn();
            {
                dgvMove.HeaderText = "Move";
                dgvMove.DisplayIndex = 1;
                for (int i = 0; i < movelist.Length; i++)
                    dgvMove.Items.Add(sortedmoves[i]); // add only the Names

                dgvMove.Width = 135;
                dgvMove.FlatStyle = FlatStyle.Flat;
            }
            dgv.Columns.Add(dgvLevel);
            dgv.Columns.Add(dgvMove);
        }

        private Learnset pkm;
        private void getList()
        {
            entry = WinFormsUtil.getIndex(CB_Species);
            int s = baseForms[entry];
            int f = formVal[entry];
            if (entry <= Main.Config.MaxSpeciesID)
                s = entry;
            int[] specForm = { s, f };
            string filename = "_" + specForm[0] + (entry > Main.Config.MaxSpeciesID ? "_" + (specForm[1] + 1) : "");
            PB_MonSprite.Image = (Bitmap)Resources.ResourceManager.GetObject(filename);

            dgv.Rows.Clear();
            byte[] input = files[entry];
            if (input.Length <= 4) { files[entry] = BitConverter.GetBytes(-1); return; }
            pkm = new Learnset7(input);

            dgv.Rows.Add(pkm.Count);

            // Fill Entries
            for (int i = 0; i < pkm.Count; i++)
            {
                dgv.Rows[i].Cells[0].Value = pkm.Levels[i];
                dgv.Rows[i].Cells[1].Value = movelist[pkm.Moves[i]];
            }

            dgv.CancelEdit();
        }
        private void setList()
        {
            if (entry < 1 || dumping) return;
            List<int> moves = new List<int>();
            List<int> levels = new List<int>();
            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                int move = Array.IndexOf(movelist, dgv.Rows[i].Cells[1].Value);
                if (move < 1) continue;

                moves.Add((short)move);
                string level = (dgv.Rows[i].Cells[0].Value ?? 0).ToString();
                short lv;
                short.TryParse(level, out lv);
                if (lv > 100) lv = 100;
                levels.Add(lv);
            }
            pkm.Moves = moves.ToArray();
            pkm.Levels = levels.ToArray();
            files[entry] = pkm.Write();
        }

        private void changeEntry(object sender, EventArgs e)
        {
            setList();
            getList();
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            setList();
            var sets = files.Select(z => new Learnset7(z)).ToArray();
            var banned = new List<int>(new[] {165, 621, 464}.Concat(Legal.Z_Moves)); // Struggle, Hyperspace Fury, Dark Void
            if (CHK_NoFixedDamage.Checked)
                banned.AddRange(MoveRandomizer.FixedDamageMoves);

            var rand = new LearnsetRandomizer(Main.Config, sets)
            {
                Expand = CHK_Expand.Checked,
                ExpandTo = (int) NUD_Moves.Value,
                Spread = CHK_Spread.Checked,
                SpreadTo = (int) NUD_Level.Value,
                STAB = CHK_STAB.Checked,
                rSTABPercent = NUD_STAB.Value,
                STABFirst = CHK_STAB.Checked,
                BannedMoves = banned,
                Learn4Level1 = CHK_4MovesLvl1.Checked,
            };
            rand.Execute();
            sets.Select(z => z.Write()).ToArray().CopyTo(files, 0);
            getList();
            WinFormsUtil.Alert("All Pokémon's Level Up Moves have been randomized!");
        }
        private void B_Dump_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Dump all Level Up Moves to Text File?"))
                return;

            dumping = true;
            string result = "";
            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                result += "======" + Environment.NewLine + entry + " " + CB_Species.Text + Environment.NewLine + "======" + Environment.NewLine;
                for (int j = 0; j < dgv.Rows.Count - 1; j++)
                    result += $"{dgv.Rows[j].Cells[0].Value} - {dgv.Rows[j].Cells[1].Value + Environment.NewLine}";

                result += Environment.NewLine;
            }
            SaveFileDialog sfd = new SaveFileDialog {FileName = "Level Up Moves.txt", Filter = "Text File|*.txt"};

            SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                File.WriteAllText(path, result, Encoding.Unicode);
            }
            dumping = false;
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setList();
        }

        private void CHK_TypeBias_CheckedChanged(object sender, EventArgs e)
        {
            NUD_STAB.Enabled = CHK_STAB.Checked;
            NUD_STAB.Value = CHK_STAB.Checked ? 52 : NUD_STAB.Minimum;
        }

        public void calcStats() // Debug Function
        {
            Move[] MoveData = Main.Config.Moves;
            int movectr = 0;
            int max = 0;
            int spec = 0;
            int stab = 0;
            for (int i = 0; i < Main.Config.MaxSpeciesID; i++)
            {
                byte[] movedata = files[i];
                int movecount = (movedata.Length - 4) / 4;
                if (movecount == 65535)
                    continue;
                movectr += movecount; // Average Moves
                if (max < movecount) { max = movecount; spec = i; } // Max Moves (and species)
                for (int m = 0; m < movedata.Length / 4; m++)
                {
                    int move = BitConverter.ToUInt16(movedata, m*4);
                    if (move == 65535)
                    {
                        movectr--;
                        continue;
                    }
                    if (Main.SpeciesStat[i].Types.Contains(MoveData[move].Type))
                        stab++;
                }
            }
            WinFormsUtil.Alert($"Moves Learned: {movectr}\r\nMost Learned: {max} @ {spec}\r\nSTAB Count: {stab}");
        }
    }
}
