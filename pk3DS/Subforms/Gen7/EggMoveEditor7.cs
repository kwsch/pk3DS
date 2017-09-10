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
    public partial class EggMoveEditor7 : Form
    {
        public EggMoveEditor7(byte[][] infiles)
        {
            InitializeComponent();
            files = infiles;
            string[] species = Main.Config.getText(TextName.SpeciesNames);
            string[][] AltForms = Main.Config.Personal.getFormList(species, Main.Config.MaxSpeciesID);
            string[] specieslist = Main.Config.Personal.getPersonalEntryList(AltForms, species, Main.Config.MaxSpeciesID, out baseForms, out formVal);
            specieslist[0] = movelist[0] = "";
            
            setupDGV();
            entries = infiles.Select(z => new EggMoves7(z)).ToArray();
            string[] names = new string[entries.Length];

            for (int i = 0; i < species.Length; i++) // add all species & forms
            {
                names[i] = species[i];
                int formoff = entries[i].FormTableIndex;
                int count = Main.Config.Personal[i].FormeCount;
                for (int j = 1; j < count; j++)
                {
                    if (names[formoff + j - 1] == null)
                        names[formoff + j - 1] = $"{species[i]} [{AltForms[i][j].Replace(species[i] + " ", "")}]";
                }
            }

            var newlist = names.Select((z, i) => new WinFormsUtil.cbItem{Text = (names[i] ?? "Extra") + $" ({i})", Value = i});
            newlist = newlist.GroupBy(z => z.Text.StartsWith("Extra"))
                .Select(z => z.OrderBy(item => item.Text))
                .SelectMany(z => z).ToList();
            NUD_FormTable.Maximum = files.Length;

            CB_Species.DisplayMember = "Text";
            CB_Species.ValueMember = "Value";
            CB_Species.DataSource = newlist;

            CB_Species.SelectedIndex = 0;
        }

        private readonly EggMoves7[] entries;

        private readonly byte[][] files;
        private int entry = -1;
        private readonly string[] movelist = Main.Config.getText(TextName.MoveNames);
        private bool dumping;
        private readonly int[] baseForms, formVal;
        private void setupDGV()
        {
            string[] sortedmoves = (string[])movelist.Clone();
            Array.Sort(sortedmoves);
            DataGridViewComboBoxColumn dgvMove = new DataGridViewComboBoxColumn();
            {
                dgvMove.HeaderText = "Move";
                dgvMove.DisplayIndex = 0;
                for (int i = 0; i < movelist.Length; i++)
                    dgvMove.Items.Add(sortedmoves[i]); // add only the Names

                dgvMove.Width = 135;
                dgvMove.FlatStyle = FlatStyle.Flat;
            }
            dgv.Columns.Add(dgvMove);
        }

        private EggMoves pkm = new EggMoves7(new byte[0]);
        private void getList()
        {
            entry = WinFormsUtil.getIndex(CB_Species);
            int s = 0, f = 0;
            if (entry <= Main.Config.MaxSpeciesID)
            {
                s = entry;
            }
            int[] specForm = { s, f };
            string filename = "_" + specForm[0] + (entry > Main.Config.MaxSpeciesID ? "_" + (specForm[1] + 1) : "");
            PB_MonSprite.Image = (Bitmap)Resources.ResourceManager.GetObject(filename);

            dgv.Rows.Clear();
            pkm = entries[entry];
            NUD_FormTable.Value = pkm.FormTableIndex;
            if (pkm.Count < 1) { files[entry] = new byte[0]; return; }
            dgv.Rows.Add(pkm.Count);

            // Fill Entries
            for (int i = 0; i < pkm.Count; i++)
                dgv.Rows[i].Cells[0].Value = movelist[pkm.Moves[i]];

            dgv.CancelEdit();
        }
        private void setList()
        {
            if (entry < 1 || dumping) return;
            List<int> moves = new List<int>();
            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                int move = Array.IndexOf(movelist, dgv.Rows[i].Cells[0].Value);
                if (move > 0 && !moves.Contains((ushort)move)) moves.Add(move);
            }
            pkm.Moves = moves.ToArray();
            pkm.FormTableIndex = (int)NUD_FormTable.Value;

            entries[entry] = (EggMoves7)pkm;
        }

        private void changeEntry(object sender, EventArgs e)
        {
            setList();
            getList();
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            var sets = entries;
            var rand = new EggMoveRandomizer(Main.Config, sets)
            {
                Expand = CHK_Expand.Checked,
                ExpandTo = (int)NUD_Moves.Value,
                STAB = CHK_STAB.Checked,
                rSTABPercent = NUD_STAB.Value,
                BannedMoves = new[] { 165, 621, 464 }.Concat(Legal.Z_Moves).ToArray(), // Struggle, Hyperspace Fury, Dark Void
            };
            rand.Execute();
            // sets.Select(z => z.Write()).ToArray().CopyTo(files, 0);
            getList();
            WinFormsUtil.Alert("All Pokémon's Egg Moves have been randomized!");
        }
        private void B_Dump_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Dump all Egg Moves to Text File?"))
                return;

            dumping = true;
            string result = "";
            for (int i = 0; i < CB_Species.Items.Count; i++)
            {
                CB_Species.SelectedIndex = i; // Get new Species
                result += "======" + Environment.NewLine + entry + " " + CB_Species.Text + Environment.NewLine + "======" + Environment.NewLine;
                for (int j = 0; j < dgv.Rows.Count - 1; j++)
                    result += dgv.Rows[j].Cells[0].Value + Environment.NewLine;

                result += Environment.NewLine;
            }
            SaveFileDialog sfd = new SaveFileDialog {FileName = "Egg Moves.txt", Filter = "Text File|*.txt"};

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
            entries.Select(z => z.Write()).ToArray().CopyTo(files, 0);
        }

        private void B_Goto_Click(object sender, EventArgs e)
        {
            CB_Species.SelectedValue = (int)NUD_FormTable.Value;
        }

        private void calcStats()
        {
            Move[] MoveData = Main.Config.Moves;
            int movectr = 0;
            int max = 0;
            int spec = 0;
            int stab = 0;
            for (int i = 0; i < Main.Config.MaxSpeciesID; i++)
            {
                byte[] movedata = files[i];
                int movecount = BitConverter.ToUInt16(movedata, 2);
                if (movecount == 65535)
                    continue;
                movectr += movecount; // Average Moves
                if (max < movecount) { max = movecount; spec = i; } // Max Moves (and species)
                for (int m = 0; m < movecount; m++)
                {
                    int move = BitConverter.ToUInt16(movedata, m * 2 + 4);
                    if (Main.SpeciesStat[i].Types.Contains(MoveData[move].Type))
                        stab++;
                }
            }
            WinFormsUtil.Alert($"Egg Moves: {movectr}\r\nMost Moves: {max} @ {spec}\r\nSTAB Count: {stab}");
        }
    }
}