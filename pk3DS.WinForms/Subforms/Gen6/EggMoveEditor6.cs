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

public partial class EggMoveEditor6 : Form
{
    public EggMoveEditor6(byte[][] infiles)
    {
        InitializeComponent();
        files = infiles;
        string[] specieslist = Main.Config.GetText(TextName.SpeciesNames);
        specieslist[0] = movelist[0] = "";

        string[] sortedspecies = (string[])specieslist.Clone();
        Array.Resize(ref sortedspecies, Main.Config.MaxSpeciesID); Array.Sort(sortedspecies);
        SetupDGV();

        var newlist = new List<ComboItem>();
        for (int i = 1; i < Main.Config.MaxSpeciesID; i++) // add all species
            newlist.Add(new ComboItem { Text = sortedspecies[i], Value = Array.IndexOf(specieslist, sortedspecies[i]) });

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
        var dgvMove = new DataGridViewComboBoxColumn();
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

    private EggMoves6 pkm = new([]);

    private void GetList()
    {
        entry = WinFormsUtil.GetIndex(CB_Species);

        int[] specForm = Main.Config.Personal.GetSpeciesForm(entry, Main.Config);
        string filename = "_" + specForm[0] + (entry > 721 ? "_" + (specForm[1] + 1) : "");
        PB_MonSprite.Image = (Bitmap)Resources.ResourceManager.GetObject(filename);

        dgv.Rows.Clear();
        byte[] input = files[entry];
        if (input.Length == 0) return;
        pkm = new EggMoves6(input);
        if (pkm.Count < 1) { files[entry] = []; return; }
        dgv.Rows.Add(pkm.Count);

        // Fill Entries
        for (int i = 0; i < pkm.Count; i++)
            dgv.Rows[i].Cells[0].Value = movelist[pkm.Moves[i]];

        dgv.CancelEdit();
    }

    private void SetList()
    {
        if (entry < 1 || dumping) return;
        List<int> moves = [];
        for (int i = 0; i < dgv.Rows.Count - 1; i++)
        {
            int move = Array.IndexOf(movelist, dgv.Rows[i].Cells[0].Value);
            if (move > 0 && !moves.Contains((ushort)move)) moves.Add(move);
        }
        pkm.Moves = [.. moves];

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

        SetList();
        var sets = files.Select(z => new EggMoves6(z)).ToArray();
        var rand = new EggMoveRandomizer(Main.Config, sets)
        {
            Expand = CHK_Expand.Checked,
            ExpandTo = (int)NUD_Moves.Value,
            STAB = CHK_STAB.Checked,
            STABPercent = NUD_STAB.Value,
            BannedMoves = [.. banned],
        };
        rand.Execute();
        sets.Select(z => z.Write()).ToArray().CopyTo(files, 0);
        GetList();
        WinFormsUtil.Alert("All Pokémon's Egg Moves have been randomized!", "Press the Dump button to see the new Egg Moves!");
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
        var sfd = new SaveFileDialog { FileName = "Egg Moves.txt", Filter = "Text File|*.txt" };

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

    public void CalcStats()
    {
        Move[] MoveData = Main.Config.Moves;

        int movectr = 0;
        int max = 0;
        int spec = 0;
        int stab = 0;
        int species = 0;
        for (int i = 0; i < Main.Config.MaxSpeciesID; i++)
        {
            byte[] movedata = files[i];
            if (movedata.Length <= 2) continue;
            int movecount = BitConverter.ToUInt16(movedata, 0);
            if (movecount == 65535)
                continue;
            species++;
            movectr += movecount; // Average Moves
            if (max < movecount) { max = movecount; spec = i; } // Max Moves (and species)
            for (int m = 1; m < movedata.Length / 2; m++)
            {
                int move = BitConverter.ToUInt16(movedata, m * 2);
                if (move == 65535)
                {
                    movectr--;
                    continue;
                }
                if (Main.Config.Personal[species].Types.Contains(MoveData[move].Type))
                    stab++;
            }
        }
        WinFormsUtil.Alert(
            $"Moves Learned: {movectr}\r\nMost Learned: {max} @ {spec}\r\nSTAB Count: {stab}\r\nSpecies with EggMoves: {species}");
    }
}