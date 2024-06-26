﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using pk3DS.Core.Structures;
using pk3DS.Core;
using pk3DS.Core.Randomizers;
using pk3DS.WinForms.Properties;

namespace pk3DS.WinForms;

public partial class LevelUpEditor6 : Form
{
    public LevelUpEditor6(byte[][] infiles)
    {
        InitializeComponent();
        files = infiles;
        string[] species = Main.Config.GetText(TextName.SpeciesNames);
        string[][] AltForms = Main.Config.Personal.GetFormList(species, Main.Config.MaxSpeciesID);
        string[] specieslist = Main.Config.Personal.GetPersonalEntryList(AltForms, species, Main.Config.MaxSpeciesID, out _, out _);
        specieslist[0] = movelist[0] = "";

        string[] sortedspecies = (string[])specieslist.Clone();
        Array.Resize(ref sortedspecies, Main.Config.MaxSpeciesID); Array.Sort(sortedspecies);
        SetupDGV();

        var newlist = new List<ComboItem>();
        for (int i = 1; i < Main.Config.MaxSpeciesID; i++) // add all species
            newlist.Add(new ComboItem { Text = sortedspecies[i], Value = Array.IndexOf(specieslist, sortedspecies[i]) });
        for (int i = Main.Config.MaxSpeciesID; i < specieslist.Length; i++) // add all forms
            newlist.Add(new ComboItem { Text = specieslist[i], Value = i });

        CB_Species.DisplayMember = "Text";
        CB_Species.ValueMember = "Value";
        CB_Species.DataSource = newlist;
        CB_Species.SelectedIndex = 0;
        RandSettings.GetFormSettings(this, groupBox1.Controls);
    }

    private readonly byte[][] files;
    private int entry = -1;
    private readonly string[] movelist = Main.Config.GetText(TextName.MoveNames);
    private bool dumping;

    private void SetupDGV()
    {
        string[] sortedmoves = (string[])movelist.Clone();
        Array.Sort(sortedmoves);
        var dgvLevel = new DataGridViewTextBoxColumn();
        {
            dgvLevel.HeaderText = "Level";
            dgvLevel.DisplayIndex = 0;
            dgvLevel.Width = 45;
            dgvLevel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        var dgvMove = new DataGridViewComboBoxColumn();
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

    private Learnset6 pkm;

    private void GetList()
    {
        entry = WinFormsUtil.GetIndex(CB_Species);

        int[] specForm = Main.Config.Personal.GetSpeciesForm(entry, Main.Config);
        string filename = "_" + specForm[0] + (entry > 721 ? "_" + (specForm[1] + 1) : "");
        PB_MonSprite.Image = (Bitmap)Resources.ResourceManager.GetObject(filename);

        dgv.Rows.Clear();
        byte[] input = files[entry];
        if (input.Length <= 4) { files[entry] = BitConverter.GetBytes(-1); return; }
        pkm = new Learnset6(input);

        dgv.Rows.Add(pkm.Count);

        // Fill Entries
        for (int i = 0; i < pkm.Count; i++)
        {
            dgv.Rows[i].Cells[0].Value = pkm.Levels[i];
            dgv.Rows[i].Cells[1].Value = movelist[pkm.Moves[i]];
        }

        dgv.CancelEdit();
    }

    private void SetList()
    {
        if (entry < 1 || dumping) return;
        List<int> moves = [];
        List<int> levels = [];
        for (int i = 0; i < dgv.Rows.Count - 1; i++)
        {
            int move = Array.IndexOf(movelist, dgv.Rows[i].Cells[1].Value);
            if (move < 1) continue;

            moves.Add((short)move);
            string level = (dgv.Rows[i].Cells[0].Value ?? 0).ToString();
            _ = short.TryParse(level, out var lv);
            if (lv > 100) lv = 100;
            else if (lv == 0) lv = 1;
            levels.Add(lv);
        }
        pkm.Moves = [.. moves];
        pkm.Levels = [.. levels];
        files[entry] = pkm.Write();
    }

    private void ChangeEntry(object sender, EventArgs e)
    {
        SetList();
        GetList();
    }

    private void B_RandAll_Click(object sender, EventArgs e)
    {
        ushort[] HMs = [15, 19, 57, 70, 127, 249, 291];
        if (CHK_HMs.Checked && Main.ExeFSPath != null)
            TMHMEditor6.GetTMHMList(out _, out HMs);

        List<int> banned = [165, 621]; // Struggle, Hyperspace Fury
        if (!CHK_HMs.Checked)
            banned.AddRange(HMs.Select(z => (int)z));
        if (CHK_NoFixedDamage.Checked)
            banned.AddRange(MoveRandomizer.FixedDamageMoves);

        SetList();
        var sets = files.Select(z => new Learnset6(z)).ToArray();
        var rand = new LearnsetRandomizer(Main.Config, sets)
        {
            Expand = CHK_Expand.Checked,
            ExpandTo = (int)NUD_Moves.Value,
            Spread = CHK_Spread.Checked,
            SpreadTo = (int)NUD_Level.Value,
            STAB = CHK_STAB.Checked,
            STABPercent = NUD_STAB.Value,
            STABFirst = CHK_STAB.Checked,
            BannedMoves = [.. banned],
            Learn4Level1 = CHK_4MovesLvl1.Checked,
        };
        rand.Execute();
        sets.Select(z => z.Write()).ToArray().CopyTo(files, 0);
        GetList();
        WinFormsUtil.Alert("All Pokémon's Level Up Moves have been randomized!", "Press the Dump button to see the new Level Up Moves!");
    }

    private void B_Metronome_Click(object sender, EventArgs e)
    {
        if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Play using Metronome Mode?", "This will modify learnsets to only have Metronome.") != DialogResult.Yes) return;

        // clear all data, then only assign Metronome at Lv1
        for (int i = 0; i < CB_Species.Items.Count; i++)
        {
            CB_Species.SelectedIndex = i;
            dgv.Rows.Clear();
            dgv.Rows.Add();
            dgv.Rows[0].Cells[0].Value = 1;
            dgv.Rows[0].Cells[1].Value = movelist[118];
        }
        CB_Species.SelectedIndex = 0;
        WinFormsUtil.Alert("All Pokémon now only know the move Metronome!");
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
        var sfd = new SaveFileDialog { FileName = "Level Up Moves.txt", Filter = "Text File|*.txt" };

        SystemSounds.Asterisk.Play();
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            string path = sfd.FileName;
            File.WriteAllText(path, result, Encoding.Unicode);
        }
        dumping = false;
    }

    private void Form_Closing(object sender, FormClosingEventArgs e)
    {
        SetList();
        RandSettings.SetFormSettings(this, groupBox1.Controls);
    }

    private void CHK_TypeBias_CheckedChanged(object sender, EventArgs e)
    {
        NUD_STAB.Enabled = CHK_STAB.Checked;
        NUD_STAB.Value = CHK_STAB.Checked ? 52 : NUD_STAB.Minimum;
    }

    public void CalcStats() // Debug Function
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
                int move = BitConverter.ToUInt16(movedata, m * 4);
                if (move == 65535)
                {
                    movectr--;
                    continue;
                }
                if (Main.Config.Personal[i].Types.Contains(MoveData[move].Type))
                    stab++;
            }
        }
        WinFormsUtil.Alert($"Moves Learned: {movectr}\r\nMost Learned: {max} @ {spec}\r\nSTAB Count: {stab}");
    }
}