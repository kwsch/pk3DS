﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using pk3DS.Properties;

namespace pk3DS
{
    public partial class LevelUpEditor7 : Form
    {
        public LevelUpEditor7(byte[][] infiles)
        {
            InitializeComponent();
            files = infiles;
            string[] species = Main.getText(TextName.SpeciesNames);
            string[][] AltForms = Main.Config.Personal.getFormList(species, Main.Config.MaxSpeciesID);
            string[] specieslist = Main.Config.Personal.getPersonalEntryList(AltForms, species, Main.Config.MaxSpeciesID, out baseForms, out formVal);
            specieslist[0] = movelist[0] = "";

            string[] sortedspecies = (string[])specieslist.Clone();
            Array.Resize(ref sortedspecies, Main.Config.MaxSpeciesID + 1); Array.Sort(sortedspecies);
            setupDGV();

            var newlist = new List<Util.cbItem>();
            for (int i = 1; i <= Main.Config.MaxSpeciesID; i++) // add all species
                newlist.Add(new Util.cbItem { Text = sortedspecies[i], Value = Array.IndexOf(specieslist, sortedspecies[i]) });
            for (int i = Main.Config.MaxSpeciesID + 1; i < specieslist.Length; i++) // add all forms
                newlist.Add(new Util.cbItem { Text = specieslist[i], Value = i });

            CB_Species.DisplayMember = "Text";
            CB_Species.ValueMember = "Value";
            CB_Species.DataSource = newlist;
            CB_Species.SelectedIndex = 0;
        }

        private readonly byte[][] files;
        private int entry = -1;
        private readonly string[] movelist = Main.getText(TextName.MoveNames);
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
            entry = Util.getIndex(CB_Species);
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
            // ORAS: 10682 moves learned on levelup/birth. 
            // 5593 are STAB. 52.3% are STAB. 
            // Steelix learns the most @ 25 (so many level 1)!
            // Move relearner ingame does not behave properly if the count exceeds 25:
            // https://twitter.com/Drayano60/status/630044237883666433
            Random rnd = new Random();

            int[] firstMoves = { 1, 40, 52, 55, 64, 71, 84, 98, 122, 141 };
            // Pound, Poison Sting, Ember, Water Gun, Peck, Absorb, Thunder Shock, Quick Attack, Lick, Leech Life
            
            int[] banned = new[] { 165, 621, 464 }.Concat(Legal.Z_Moves).ToArray(); // Struggle, Hyperspace Fury, Dark Void

            // Move Stats
            Move[] moveTypes = Main.Config.Moves;

            // Set up Randomized Moves
            int[] randomMoves = Enumerable.Range(1, movelist.Length - 1).Select(i => i).ToArray();
            Util.Shuffle(randomMoves);
            int ctr = 0;

            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                int count = dgv.Rows.Count - 1;
                int species = Util.getIndex(CB_Species);
                if (CHK_Expand.Checked && (int)NUD_Moves.Value > count)
                    dgv.Rows.AddCopies(count, (int)NUD_Moves.Value - count);

                // Default First Move
                dgv.Rows[0].Cells[0].Value = 1;
                dgv.Rows[0].Cells[1].Value = movelist[firstMoves[rnd.Next(0, firstMoves.Length)]];
                for (int j = 1; j < dgv.Rows.Count - 1; j++)
                {
                    // Assign New Moves
                    bool forceSTAB = CHK_STAB.Checked && rnd.Next(0, 99) < NUD_STAB.Value;
                    int move = Randomizer.getRandomSpecies(ref randomMoves, ref ctr);

                    while (banned.Contains(move) /* Invalid */
                        || (forceSTAB && !Main.SpeciesStat[species].Types.Contains(moveTypes[move].Type))) // STAB is required
                        move = Randomizer.getRandomSpecies(ref randomMoves, ref ctr);

                    // Assign Move
                    dgv.Rows[j].Cells[1].Value = movelist[move];
                    // Assign Level
                    if (j >= count)
                    {
                        string level = (dgv.Rows[count - 1].Cells[0].Value ?? 0).ToString();
                        ushort lv;
                        UInt16.TryParse(level, out lv);
                        if (lv > 100) lv = 100;
                        dgv.Rows[j].Cells[0].Value = lv + (j - count) + 1;
                    }
                    if (CHK_Spread.Checked)
                        dgv.Rows[j].Cells[0].Value = (j * (NUD_Level.Value / (dgv.Rows.Count - 1))).ToString();
                }
            }
            CB_Species.SelectedIndex = 0;
            Util.Alert("All Pokemon's Level Up Moves have been randomized!");
        }
        private void B_Dump_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Dump all Egg Moves to Text File?"))
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
            Util.Alert($"Moves Learned: {movectr}\r\nMost Learned: {max} @ {spec}\r\nSTAB Count: {stab}");
        }
    }
}
