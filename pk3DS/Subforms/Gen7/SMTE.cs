﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

using pk3DS.Core;
using pk3DS.Core.Randomizers;
using pk3DS.Core.Structures;

namespace pk3DS
{
    public partial class SMTE : Form
    {
        private readonly LearnsetRandomizer learn = new LearnsetRandomizer(Main.Config, Main.Config.Learnsets);
        private readonly trdata7[] Trainers;
        private string[][] AltForms;
        private static int[] TrainerClasses;
        private static int[] ImportantTrainers;
        private static int[] FinalEvo;
        private static int[] ReplaceLegend;
        private static Dictionary<int, int[]> MegaDictionary;
        private int index = -1;
        private PictureBox[] pba;

        private readonly byte[][] trclass, trdata, trpoke;
        private readonly string[] abilitylist = Main.Config.getText(TextName.AbilityNames);
        private readonly string[] movelist = Main.Config.getText(TextName.MoveNames);
        private readonly string[] itemlist = Main.Config.getText(TextName.ItemNames);
        private readonly string[] specieslist = Main.Config.getText(TextName.SpeciesNames);
        private readonly string[] types = Main.Config.getText(TextName.Types);
        private readonly string[] natures = Main.Config.getText(TextName.Natures);
        private readonly string[] forms = Enumerable.Range(0, 1000).Select(i => i.ToString("000")).ToArray();
        private readonly string[] trName = Main.Config.getText(TextName.TrainerNames);
        private readonly string[] trClass = Main.Config.getText(TextName.TrainerClasses);
        private readonly TextData trText = Main.Config.getTextData(TextName.TrainerText);
        private readonly TextData TrainerNames;
        

        public SMTE(byte[][] trc, byte[][] trd, byte[][] trp)
        {
            trclass = trc;
            trdata = trd;
            trpoke = trp;
            TrainerNames = new TextData(trName);
            InitializeComponent();

            mnuView.Click += ClickView;
            mnuSet.Click += ClickSet;
            mnuDelete.Click += ClickDelete;
            Trainers = new trdata7[trdata.Length];
            Setup();
            foreach (var pb in pba)
                pb.Click += ClickSlot;

            CB_TrainerID.SelectedIndex = 0;
            CB_Moves.SelectedIndex = 0;
            CHK_ReplaceLegend.Visible = Main.Config.USUM; // Team Rainbow Rocket only in USUM
            MegaDictionary = GiftEditor6.GetMegaDictionary(Main.Config);

            TrainerClasses = Main.Config.USUM ? Legal.SpecialClasses_USUM : Legal.SpecialClasses_SM;
            ImportantTrainers = Main.Config.USUM ? Legal.ImportantTrainers_USUM : Legal.ImportantTrainers_SM;
            FinalEvo = Main.Config.USUM ? Legal.FinalEvolutions_USUM : Legal.FinalEvolutions_SM;
            ReplaceLegend = Legal.Legendary_Mythical_USUM;
            RandSettings.GetFormSettings(this, Tab_Misc.Controls);
        }

        private int GetSlot(object sender)
        {
            var send = ((sender as ToolStripItem)?.Owner as ContextMenuStrip)?.SourceControl ?? sender as PictureBox;
            return Array.IndexOf(pba, send);
        }
        private void ClickSlot(object sender, EventArgs e)
        {
            switch (ModifierKeys)
            {
                case Keys.Control: ClickView(sender, e); break;
                case Keys.Shift: ClickSet(sender, e); break;
                case Keys.Alt: ClickDelete(sender, e); break;
            }
        }
        private void ClickView(object sender, EventArgs e)
        {
            int slot = GetSlot(sender);
            if (pba[slot].Image == null)
            { SystemSounds.Exclamation.Play(); return; }
            
            // Load the PKM
            var pk = Trainers[index].Pokemon[slot];
            if (pk.Species != 0)
            {
                try { PopulateFieldsTP7(pk); }
                catch { }
                // Visual to display what slot is currently loaded.
                GetSlotColor(slot, Properties.Resources.slotView);
            }
            else
                SystemSounds.Exclamation.Play();
        }
        private void ClickSet(object sender, EventArgs e)
        {
            int slot = GetSlot(sender);
            if (CB_Species.SelectedIndex == 0)
            { WinFormsUtil.Alert("Can't set empty slot."); return; }

            var pk = PrepareTP7();
            var tr = Trainers[index];
            if (slot < tr.NumPokemon)
                tr.Pokemon[slot] = pk;
            else
            {
                tr.Pokemon.Add(pk);
                slot = tr.Pokemon.Count - 1;
                Trainers[index].NumPokemon = (int)++NUD_NumPoke.Value;
            }

            GetQuickFiller(pba[slot], pk);
            GetSlotColor(slot, Properties.Resources.slotSet);
        }
        private void ClickDelete(object sender, EventArgs e)
        {
            int slot = GetSlot(sender);

            if (slot < Trainers[index].NumPokemon)
            {
                Trainers[index].Pokemon.RemoveAt(slot);
                Trainers[index].NumPokemon = (int)--NUD_NumPoke.Value;
            }

            PopulateTeam(Trainers[index]);
            GetSlotColor(slot, Properties.Resources.slotDel);
        }

        private void PopulateTeam(trdata7 tr)
        {
            for (int i = 0; i < tr.NumPokemon; i++)
                GetQuickFiller(pba[i], tr.Pokemon[i]);
            for (int i = tr.NumPokemon; i < 6; i++)
                pba[i].Image = null;
        }

        private void GetSlotColor(int slot, Image color)
        {
            foreach (PictureBox t in pba)
                t.BackgroundImage = null;

            pba[slot].BackgroundImage = color;
        }
        private static void GetQuickFiller(PictureBox pb, trpoke7 pk)
        {
            Bitmap rawImg = WinFormsUtil.getSprite(pk.Species, pk.Form, pk.Gender, pk.Item, Main.Config, pk.Shiny);
            pb.Image = WinFormsUtil.scaleImage(rawImg, 2);
        }

        // Top Level Functions
        private void RefreshFormAbility(object sender, EventArgs e)
        {
            if (index < 0)
                return;
            pkm.Form = CB_Forme.SelectedIndex;
            RefreshPKMSlotAbility();
        }
        private void RefreshSpeciesAbility(object sender, EventArgs e)
        {
            if (index < 0)
                return;
            pkm.Species = (ushort)CB_Species.SelectedIndex;
            FormUtil.setForms(CB_Species.SelectedIndex, CB_Forme, AltForms);
            RefreshPKMSlotAbility();
        }
        private void RefreshPKMSlotAbility()
        {
            int previousAbility = CB_Ability.SelectedIndex;

            int species = CB_Species.SelectedIndex;
            int formnum = CB_Forme.SelectedIndex;
            species = Main.SpeciesStat[species].FormeIndex(species, formnum);

            CB_Ability.Items.Clear();
            CB_Ability.Items.Add("Any (1 or 2)");
            CB_Ability.Items.Add(abilitylist[Main.SpeciesStat[species].Abilities[0]] + " (1)");
            CB_Ability.Items.Add(abilitylist[Main.SpeciesStat[species].Abilities[1]] + " (2)");
            CB_Ability.Items.Add(abilitylist[Main.SpeciesStat[species].Abilities[2]] + " (H)");

            CB_Ability.SelectedIndex = previousAbility;
        }

        private static string GetEntryTitle(string str, int i) => $"{str} - {i:000}";
        private void Setup()
        {
            AltForms = forms.Select(f => Enumerable.Range(0, 100).Select(i => i.ToString()).ToArray()).ToArray();
            CB_TrainerID.Items.Clear();
            for (int i = 0; i < trdata.Length; i++)
                CB_TrainerID.Items.Add(GetEntryTitle(trName[i] ?? "UNKNOWN", i));

            CB_Trainer_Class.Items.Clear();
            for (int i = 0; i < trClass.Length; i++)
                CB_Trainer_Class.Items.Add(GetEntryTitle(trClass[i], i));

            Trainers[0] = new trdata7();

            for (int i = 1; i < trdata.Length; i++)
            {
                Trainers[i] = new trdata7(trdata[i], trpoke[i])
                {
                    Name = trName[i],
                    ID = i
                };
            }

            specieslist[0] = "---";
            abilitylist[0] = itemlist[0] = movelist[0] = "(None)";
            pba = new[] { PB_Team1, PB_Team2, PB_Team3, PB_Team4, PB_Team5, PB_Team6 };
            
            CB_Species.Items.Clear();
            foreach (string s in specieslist)
                CB_Species.Items.Add(s);

            CB_Move1.Items.Clear();
            CB_Move2.Items.Clear();
            CB_Move3.Items.Clear();
            CB_Move4.Items.Clear();
            foreach (string s in movelist)
            {
                CB_Move1.Items.Add(s);
                CB_Move2.Items.Add(s);
                CB_Move3.Items.Add(s);
                CB_Move4.Items.Add(s);
            }

            CB_HPType.DataSource = types.Skip(1).Take(16).ToArray();
            CB_HPType.SelectedIndex = 0;

            CB_Nature.Items.Clear();
            CB_Nature.Items.AddRange(natures.Take(25).ToArray());

            CB_Item.Items.Clear();
            foreach (string s in itemlist)
                CB_Item.Items.Add(s);
                
            CB_Gender.Items.Clear();
            CB_Gender.Items.Add("- / G/Random");
            CB_Gender.Items.Add("♂ / M");
            CB_Gender.Items.Add("♀ / F");

            CB_Forme.Items.Add("");

            CB_Species.SelectedIndex = 0;
            CB_Item_1.Items.Clear();
            CB_Item_2.Items.Clear();
            CB_Item_3.Items.Clear();
            CB_Item_4.Items.Clear();
            foreach (string s in itemlist)
            {
                CB_Item_1.Items.Add(s);
                CB_Item_2.Items.Add(s);
                CB_Item_3.Items.Add(s);
                CB_Item_4.Items.Add(s);
            }

            CB_Money.Items.Clear();
            for (int i = 0; i < 256; i++)
            { CB_Money.Items.Add(i.ToString()); }

            CB_TrainerID.SelectedIndex = 0;
            index = 0;
            pkm = new trpoke7();
            PopulateFieldsTP7(pkm);
        }

        private void ChangeTrainerIndex(object sender, EventArgs e)
        {
            SaveEntry();
            LoadEntry();
            if (TC_trdata.SelectedIndex == TC_trdata.TabCount - 1) // last
                TC_trdata.SelectedIndex = 0;
        }
        private void SaveEntry()
        {
            if (index < 0)
                return;
            var tr = Trainers[index];
            PrepareTR7(tr);
            SaveData(tr, index);
            TrainerNames[index] = TB_TrainerName.Text;
        }
        private void SaveData(trdata7 tr, int i)
        {
            tr.Write(out byte[] trd, out byte[] trp);
            trdata[i] = trd;
            trpoke[i] = trp;
        }
        private void LoadEntry()
        {
            index = CB_TrainerID.SelectedIndex;
            var tr = Trainers[index];

            loading = true;
            TB_TrainerName.Text = TrainerNames[index];

            PopulateFieldsTD7(tr);
            loading = false;
        }

        private bool loading;
        private trpoke7 pkm;
        private void PopulateFieldsTP7(trpoke7 pk)
        {
            pkm = pk.Clone();

            int spec = pkm.Species, form = pkm.Form;

            CB_Species.SelectedIndex = spec;
            CB_Forme.SelectedIndex = form;
            CB_Ability.SelectedIndex = pkm.Ability;
            CB_Item.SelectedIndex = pkm.Item;
            CHK_Shiny.Checked = pkm.Shiny;
            CB_Gender.SelectedIndex = pkm.Gender;

            CB_Move1.SelectedIndex = pkm.Move1;
            CB_Move2.SelectedIndex = pkm.Move2;
            CB_Move3.SelectedIndex = pkm.Move3;
            CB_Move4.SelectedIndex = pkm.Move4;

            updatingStats = true;
            CB_Nature.SelectedIndex = pkm.Nature;
            NUD_Level.Value = Math.Min(NUD_Level.Maximum, pkm.Level);

            TB_HPIV.Text = pkm.IV_HP.ToString();
            TB_ATKIV.Text = pkm.IV_ATK.ToString();
            TB_DEFIV.Text = pkm.IV_DEF.ToString();
            TB_SPAIV.Text = pkm.IV_SPA.ToString();
            TB_SPEIV.Text = pkm.IV_SPE.ToString();
            TB_SPDIV.Text = pkm.IV_SPD.ToString();

            TB_HPEV.Text = pkm.EV_HP.ToString();
            TB_ATKEV.Text = pkm.EV_ATK.ToString();
            TB_DEFEV.Text = pkm.EV_DEF.ToString();
            TB_SPAEV.Text = pkm.EV_SPA.ToString();
            TB_SPEEV.Text = pkm.EV_SPE.ToString();
            TB_SPDEV.Text = pkm.EV_SPD.ToString();
            updatingStats = false;
            UpdateStats(null, null);
        }
        private trpoke7 PrepareTP7()
        {
            var pk = pkm.Clone();
            pk.Species = CB_Species.SelectedIndex;
            pk.Form = CB_Forme.SelectedIndex;
            pk.Level = (byte)NUD_Level.Value;
            pk.Ability = CB_Ability.SelectedIndex;
            pk.Item = CB_Item.SelectedIndex;
            pk.Shiny = CHK_Shiny.Checked;
            pk.Nature = CB_Nature.SelectedIndex;
            pk.Gender = CB_Gender.SelectedIndex;

            pk.Move1 = CB_Move1.SelectedIndex;
            pk.Move2 = CB_Move2.SelectedIndex;
            pk.Move3 = CB_Move3.SelectedIndex;
            pk.Move4 = CB_Move4.SelectedIndex;

            pk.IV_HP = WinFormsUtil.ToInt32(TB_HPIV);
            pk.IV_ATK = WinFormsUtil.ToInt32(TB_ATKIV);
            pk.IV_DEF = WinFormsUtil.ToInt32(TB_DEFIV);
            pk.IV_SPA = WinFormsUtil.ToInt32(TB_SPAIV);
            pk.IV_SPE = WinFormsUtil.ToInt32(TB_SPEIV);
            pk.IV_SPD = WinFormsUtil.ToInt32(TB_SPDIV);

            pk.EV_HP = WinFormsUtil.ToInt32(TB_HPEV);
            pk.EV_ATK = WinFormsUtil.ToInt32(TB_ATKEV);
            pk.EV_DEF = WinFormsUtil.ToInt32(TB_DEFEV);
            pk.EV_SPA = WinFormsUtil.ToInt32(TB_SPAEV);
            pk.EV_SPE = WinFormsUtil.ToInt32(TB_SPEEV);
            pk.EV_SPD = WinFormsUtil.ToInt32(TB_SPDEV);

            return pk;
        }
        private void PopulateFieldsTD7(trdata7 tr)
        {
            // Load Trainer Data
            CB_Trainer_Class.SelectedIndex = tr.TrainerClass;
            NUD_NumPoke.Value = tr.NumPokemon;
            CB_Item_1.SelectedIndex = tr.Item1;
            CB_Item_2.SelectedIndex = tr.Item2;
            CB_Item_3.SelectedIndex = tr.Item3;
            CB_Item_4.SelectedIndex = tr.Item4;
            CB_Money.SelectedIndex = tr.Money;
            NUD_AI.Value = tr.AI;
            CHK_Flag.Checked = tr.Flag;
            PopulateTeam(tr);
        }
        private void PrepareTR7(trdata7 tr)
        {
            tr.TrainerClass = (byte)CB_Trainer_Class.SelectedIndex;
            tr.NumPokemon = (byte)NUD_NumPoke.Value;
            tr.Item1 = CB_Item_1.SelectedIndex;
            tr.Item2 = CB_Item_2.SelectedIndex;
            tr.Item3 = CB_Item_3.SelectedIndex;
            tr.Item4 = CB_Item_4.SelectedIndex;
            tr.Money = CB_Money.SelectedIndex;
            tr.AI = (int)NUD_AI.Value;
            tr.Flag = CHK_Flag.Checked;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveEntry();
            if (TrainerNames.Modified)
                Main.Config.setText(TextName.TrainerNames, TrainerNames.Lines);
            base.OnFormClosing(e);
            RandSettings.SetFormSettings(this, Tab_Misc.Controls);
        }

        // Dumping
        private void DumpTxt(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = "Trainers.txt";
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                var sb = new StringBuilder();
                foreach (var Trainer in Trainers)
                    sb.Append(GetTrainerString(Trainer));
                File.WriteAllText(sfd.FileName, sb.ToString());
            }
        }
        private string GetTrainerString(trdata7 tr)
        {
            var sb = new StringBuilder();
            sb.AppendLine("======");
            sb.AppendLine($"{tr.ID} - {trClass[tr.TrainerClass]} {tr.Name}");
            sb.AppendLine("======");
            sb.AppendLine($"Pokemon: {tr.NumPokemon}");
            for (int i = 0; i < tr.NumPokemon; i++)
            {
                if (tr.Pokemon[i].Shiny)
                    sb.Append("Shiny ");
                sb.Append(specieslist[tr.Pokemon[i].Species]);
                sb.Append($" (Lv. {tr.Pokemon[i].Level}) ");
                if (tr.Pokemon[i].Item > 0)
                    sb.Append($"@{itemlist[tr.Pokemon[i].Item]}");

                if (tr.Pokemon[i].Nature != 0)
                    sb.Append($" (Nature: {natures[tr.Pokemon[i].Nature]})");

                sb.Append($" (Moves: {string.Join("/", tr.Pokemon[i].Moves.Select(m => m == 0 ? "(None)" : movelist[m]))})");
                sb.Append($" IVs: {string.Join("/", tr.Pokemon[i].IVs)}");
                sb.Append($" EVs: {string.Join("/", tr.Pokemon[i].EVs)}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void UpdateNumPokemon(object sender, EventArgs e)
        {
            if (index < 0)
                return;
            Trainers[index].NumPokemon = (int) NUD_NumPoke.Value;
        }
        private void UpdateTrainerName(object sender, EventArgs e)
        {
            if (loading)
                return;
            string str = TB_TrainerName.Text;
            CB_TrainerID.Items[index] = GetEntryTitle(str, index);
        }

        private static bool updatingStats;

        private void UpdateStats(object sender, EventArgs e)
        {
            if (updatingStats)
                return;
            var tb_iv = new[] { TB_HPIV, TB_ATKIV, TB_DEFIV, TB_SPEIV, TB_SPAIV, TB_SPDIV };
            var tb_ev = new[] { TB_HPEV, TB_ATKEV, TB_DEFEV, TB_SPEEV, TB_SPAEV, TB_SPDEV };
            for (int i = 0; i < 6; i++)
            {
                updatingStats = true;
                if (WinFormsUtil.ToInt32(tb_iv[i]) > 31)
                    tb_iv[i].Text = "31";
                if (WinFormsUtil.ToInt32(tb_ev[i]) > 255)
                    tb_ev[i].Text = "255";
                updatingStats = false;
            }

            int species = CB_Species.SelectedIndex;
            species = Main.SpeciesStat[species].FormeIndex(species, CB_Forme.SelectedIndex);
            var p = Main.SpeciesStat[species];
            int level = (int)NUD_Level.Value;
            int Nature = CB_Nature.SelectedIndex;

            ushort[] Stats = new ushort[6];
            Stats[0] = (ushort)(p.HP == 1 ? 1 : (Util.ToInt32(TB_HPIV.Text) + 2 * p.HP + Util.ToInt32(TB_HPEV.Text) / 4 + 100) * level / 100 + 10);
            Stats[1] = (ushort)((Util.ToInt32(TB_ATKIV.Text) + 2 * p.ATK + Util.ToInt32(TB_ATKEV.Text) / 4) * level / 100 + 5);
            Stats[2] = (ushort)((Util.ToInt32(TB_DEFIV.Text) + 2 * p.DEF + Util.ToInt32(TB_DEFEV.Text) / 4) * level / 100 + 5);
            Stats[4] = (ushort)((Util.ToInt32(TB_SPAIV.Text) + 2 * p.SPA + Util.ToInt32(TB_SPAEV.Text) / 4) * level / 100 + 5);
            Stats[5] = (ushort)((Util.ToInt32(TB_SPDIV.Text) + 2 * p.SPD + Util.ToInt32(TB_SPDEV.Text) / 4) * level / 100 + 5);
            Stats[3] = (ushort)((Util.ToInt32(TB_SPEIV.Text) + 2 * p.SPE + Util.ToInt32(TB_SPEEV.Text) / 4) * level / 100 + 5);

            // Account for nature
            int incr = Nature / 5 + 1;
            int decr = Nature % 5 + 1;
            if (incr != decr)
            {
                Stats[incr] *= 11;
                Stats[incr] /= 10;
                Stats[decr] *= 9;
                Stats[decr] /= 10;
            }

            Stat_HP.Text = Stats[0].ToString();
            Stat_ATK.Text = Stats[1].ToString();
            Stat_DEF.Text = Stats[2].ToString();
            Stat_SPA.Text = Stats[4].ToString();
            Stat_SPD.Text = Stats[5].ToString();
            Stat_SPE.Text = Stats[3].ToString();

            TB_IVTotal.Text = tb_iv.Select(WinFormsUtil.ToInt32).Sum().ToString();
            TB_EVTotal.Text = tb_ev.Select(WinFormsUtil.ToInt32).Sum().ToString();

            // Recolor the Stat Labels based on boosted stats.
            {
                incr--;
                decr--;
                Label[] labarray = { Label_ATK, Label_DEF, Label_SPE, Label_SPA, Label_SPD };
                // Reset Label Colors
                foreach (Label label in labarray)
                    label.ResetForeColor();

                // Set Colored StatLabels only if Nature isn't Neutral
                if (incr != decr)
                {
                    labarray[incr].ForeColor = Color.Red;
                    labarray[decr].ForeColor = Color.Blue;
                }
            }
            var ivs = tb_iv.Select(tb => WinFormsUtil.ToInt32(tb) & 1).ToArray();
            updatingStats = true;
            CB_HPType.SelectedIndex = 15 * (ivs[0] + 2 * ivs[1] + 4 * ivs[2] + 8 * ivs[3] + 16 * ivs[4] + 32 * ivs[5]) / 63;
            updatingStats = false;
        }

        private void UpdateHPType(object sender, EventArgs e)
        {
            if (updatingStats)
                return;
            var tb_iv = new[] { TB_HPIV, TB_ATKIV, TB_DEFIV, TB_SPAIV, TB_SPDIV, TB_SPEIV };
            int[] newIVs = SetHPIVs(CB_HPType.SelectedIndex, tb_iv.Select(WinFormsUtil.ToInt32).ToArray());
            updatingStats = true;
            TB_HPIV.Text = newIVs[0].ToString();
            TB_ATKIV.Text = newIVs[1].ToString();
            TB_DEFIV.Text = newIVs[2].ToString();
            TB_SPAIV.Text = newIVs[3].ToString();
            TB_SPDIV.Text = newIVs[4].ToString();
            TB_SPEIV.Text = newIVs[5].ToString();
            updatingStats = false;
        }
        public static int[] SetHPIVs(int type, int[] ivs)
        {
            for (int i = 0; i < 6; i++)
                ivs[i] = (ivs[i] & 0x1E) + hpivs[type, i];
            return ivs;
        }

        private static readonly int[,] hpivs = {
            { 1, 1, 0, 0, 0, 0 }, // Fighting
            { 0, 0, 0, 0, 0, 1 }, // Flying
            { 1, 1, 0, 0, 0, 1 }, // Poison
            { 1, 1, 1, 0, 0, 1 }, // Ground
            { 1, 1, 0, 1, 0, 0 }, // Rock
            { 1, 0, 0, 1, 0, 1 }, // Bug
            { 1, 0, 1, 1, 0, 1 }, // Ghost
            { 1, 1, 1, 1, 0, 1 }, // Steel
            { 1, 0, 1, 0, 1, 0 }, // Fire
            { 1, 0, 0, 0, 1, 1 }, // Water
            { 1, 0, 1, 0, 1, 1 }, // Grass
            { 1, 1, 1, 0, 1, 1 }, // Electric
            { 1, 0, 1, 1, 1, 0 }, // Psychic
            { 1, 0, 0, 1, 1, 1 }, // Ice
            { 1, 0, 1, 1, 1, 1 }, // Dragon
            { 1, 1, 1, 1, 1, 1 }, // Dark
        };

        private void B_Randomize_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize all? Cannot undo.", "Double check Randomization settings in the Misc/Rand tab.") != DialogResult.Yes) return;

            CB_TrainerID.SelectedIndex = 0;
            var rnd = new SpeciesRandomizer(Main.Config)
            {
                G1 = CHK_G1.Checked,
                G2 = CHK_G2.Checked,
                G3 = CHK_G3.Checked,
                G4 = CHK_G4.Checked,
                G5 = CHK_G5.Checked,
                G6 = CHK_G6.Checked,
                G7 = CHK_G7.Checked,

                E = CHK_E.Checked,
                L = CHK_L.Checked,
                rBST = CHK_BST.Checked,
            };
            rnd.Initialize();

            var banned = new List<int>(new[] { 165, 621, 464 }.Concat(Legal.Z_Moves)); // Struggle, Hyperspace Fury, Dark Void
            if (CHK_NoFixedDamage.Checked)
                banned.AddRange(MoveRandomizer.FixedDamageMoves);
            var move = new MoveRandomizer(Main.Config)
            {
                BannedMoves = banned,
                rSTABCount = (int) NUD_STAB.Value,
                rDMG = CHK_Damage.Checked,
                rDMGCount = (int) NUD_Damage.Value,
                rSTAB = CHK_STAB.Checked
            };

            var items = Randomizer.getRandomItemList();
            for (int i = 0; i < Trainers.Length; i++)
            {
                var tr = Trainers[i];
                if (tr.Pokemon.Count == 0)
                    continue;
                // Trainer Properties
                if (CHK_RandomClass.Checked)
                {
                    int rv;
                    do
                    {
                        rv = (int) (Util.rnd32()%CB_Trainer_Class.Items.Count);
                    } while (/*trClass[rv].StartsWith("[~") || */TrainerClasses.Contains(rv) && CHK_IgnoreSpecialClass.Checked); // don't allow disallowed classes

                    if (rv == 082) // Lusamine 2 (Aether President - 082) can crash Multi Battles, skip
                        continue;

                    tr.TrainerClass = (byte) rv;
                }

                var avgBST = (int)tr.Pokemon.Average(pk => Main.SpeciesStat[pk.Species].BST);
                int avgLevel = (int)tr.Pokemon.Average(pk => pk.Level);
                var pinfo = Main.SpeciesStat.OrderBy(pk => Math.Abs(avgBST - pk.BST)).First();
                int avgSpec = Array.IndexOf(Main.SpeciesStat, pinfo);

                if (tr.NumPokemon < NUD_RMin.Value)
                {
                    for (int p = tr.NumPokemon; p < NUD_RMin.Value; p++)
                        tr.Pokemon.Add(new trpoke7
                        {
                            Species = rnd.GetRandomSpecies(avgSpec),
                            Level = avgLevel,
                        });
                    tr.NumPokemon = (int)NUD_RMin.Value;
                }
                if (tr.NumPokemon > NUD_RMax.Value)
                {
                    tr.Pokemon.RemoveRange((int)NUD_RMax.Value, (int)(tr.NumPokemon - NUD_RMax.Value));
                    tr.NumPokemon = (int)NUD_RMax.Value;
                }
                if (CHK_6PKM.Checked && ImportantTrainers.Contains(tr.ID))
                {
                    for (int g = tr.NumPokemon; g < 6; g++)
                        tr.Pokemon.Add(new trpoke7
                        {
                            Species = rnd.GetRandomSpecies(avgSpec),
                            Level = avgLevel,
                        });
                    tr.NumPokemon = 6;
                }

                // PKM Properties
                foreach (var pk in tr.Pokemon)
                {
                    if (CHK_RandomPKM.Checked)
                    {
                        int Type = CHK_TypeTheme.Checked ? (int)Util.rnd32() % 17 : -1;

                        // replaces Megas with another Mega (Dexio and Lysandre in USUM)
                        if (MegaDictionary.Values.Any(z => z.Contains(pk.Item)))
                        {
                            int species = pk.Species;
                            int[] mega = GetRandomMega(out species);
                            pk.Species = species;
                            pk.Item = mega[Util.rand.Next(0, mega.Length)];
                            pk.Form = 0; // allow it to Mega Evolve naturally
                        }

                        // replaces Team Rainbow Rocket Legendaries with another Legendary
                        else if (CHK_ReplaceLegend.Checked && ReplaceLegend.Contains(pk.Species))
                        {
                            int randLegend() => (int)(Util.rnd32() % ReplaceLegend.Length);
                            pk.Species = ReplaceLegend[randLegend()];
                            pk.Item = items[Util.rnd32() % items.Length];
                            pk.Form = Randomizer.GetRandomForme(pk.Species, CHK_RandomMegaForm.Checked, true, Main.SpeciesStat);
                        }
                        
                        // every other pkm
                        else
                        {
                            pk.Species = rnd.GetRandomSpeciesType(pk.Species, Type);
                            pk.Item = items[Util.rnd32() % items.Length];
                            pk.Form = Randomizer.GetRandomForme(pk.Species, CHK_RandomMegaForm.Checked, true, Main.SpeciesStat);
                        }
                        pk.Gender = 0; // random
                        pk.Nature = (int)(Util.rnd32() % CB_Nature.Items.Count); // random
                    }
                    if (CHK_Level.Checked)
                        pk.Level = Randomizer.getModifiedLevel(pk.Level, NUD_LevelBoost.Value);
                    if (CHK_RandomShiny.Checked)
                        pk.Shiny = Util.rand.Next(0, 100 + 1) < NUD_Shiny.Value;
                    if (CHK_RandomAbilities.Checked)
                        pk.Ability = (int)Util.rnd32() % 4;
                    if (CHK_MaxDiffPKM.Checked)
                        pk.IVs = new[] {31, 31, 31, 31, 31, 31};
                    
                    if (CHK_ForceFullyEvolved.Checked && pk.Level >= NUD_ForceFullyEvolved.Value && !FinalEvo.Contains(pk.Species))
                    {
                        int randFinalEvo() => (int)(Util.rnd32() % FinalEvo.Length);
                        pk.Species = FinalEvo[randFinalEvo()];
                        pk.Form = Randomizer.GetRandomForme(pk.Species, CHK_RandomMegaForm.Checked, true, Main.SpeciesStat);
                    }

                    switch (CB_Moves.SelectedIndex)
                    {
                        case 1: // Random
                            pk.Moves = move.GetRandomMoveset(pk.Species, 4);
                            break;
                        case 2: // Current LevelUp
                            pk.Moves = learn.GetCurrentMoves(pk.Species, pk.Form, pk.Level, 4);
                            break;
                        case 3:
                            pk.Moves = new[] { 118, 0, 0, 0 };
                            break;
                    }

                    // high-power attacks
                    if (CHK_ForceHighPower.Checked && pk.Level >= NUD_ForceHighPower.Value)
                        pk.Moves = learn.GetHighPoweredMoves(pk.Species, pk.Form, 4);

                    // sanitize moves
                    if (CB_Moves.SelectedIndex > 1) // learn source
                    {
                        var moves = pk.Moves;
                        if (move.SanitizeMovesetForBannedMoves(moves, pk.Species))
                            pk.Moves = moves;
                    }
                }
                SaveData(tr, i);
            }
            WinFormsUtil.Alert("Randomized all Trainers according to specification!", "Press the Dump to .TXT button to view the new Trainer information!");
        }
        private void B_HighAttack_Click(object sender, EventArgs e)
        {
            pkm.Species = CB_Species.SelectedIndex;
            pkm.Level = (int)NUD_Level.Value;
            pkm.Form = CB_Forme.SelectedIndex;
            var moves = learn.GetHighPoweredMoves(pkm.Species, pkm.Form, 4);
            SetMoves(moves);
        }
        private void B_CurrentAttack_Click(object sender, EventArgs e)
        {
            pkm.Species = CB_Species.SelectedIndex;
            pkm.Level = (int)NUD_Level.Value;
            pkm.Form = CB_Forme.SelectedIndex;
            var moves = learn.GetCurrentMoves(pkm.Species, pkm.Form, pkm.Level, 4);
            SetMoves(moves);
        }
        private void B_Clear_Click(object sender, EventArgs e) => SetMoves(new int[4]);

        private void SetMoves(IList<int> moves)
        {
            var mcb = new[] { CB_Move1, CB_Move2, CB_Move3, CB_Move4 };
            for (int i = 0; i < mcb.Length; i++)
                mcb[i].SelectedIndex = moves[i];
        }

        // Randomization UI
        private void CB_Moves_SelectedIndexChanged(object sender, EventArgs e)
        {
            CHK_Damage.Checked = CHK_STAB.Checked =
            CHK_Damage.Enabled = CHK_STAB.Enabled =
            NUD_Damage.Enabled = NUD_STAB.Enabled = CB_Moves.SelectedIndex == 1;

            CHK_ForceHighPower.Enabled = CHK_ForceHighPower.Checked = NUD_ForceHighPower.Enabled =
            CHK_NoFixedDamage.Enabled = CHK_NoFixedDamage.Checked = (CB_Moves.SelectedIndex == 1 || CB_Moves.SelectedIndex == 2);
        }
        private void CHK_Damage_CheckedChanged(object sender, EventArgs e)
        {
            NUD_Damage.Enabled = CHK_Damage.Checked;
        }
        private void CHK_STAB_CheckedChanged(object sender, EventArgs e)
        {
            NUD_STAB.Enabled = CHK_STAB.Checked;
        }
        private void CHK_RandomPKM_CheckedChanged(object sender, EventArgs e)
        {
            if (!CHK_RandomPKM.Checked)
                foreach (CheckBox c in new[] { CHK_G1, CHK_G2, CHK_G3, CHK_G4, CHK_G5, CHK_G6, CHK_G7, CHK_L, CHK_E, CHK_BST })
                {
                    c.Enabled = false;
                    c.Checked = false;
                }
            else
                foreach (CheckBox c in new[] { CHK_G1, CHK_G2, CHK_G3, CHK_G4, CHK_G5, CHK_G6, CHK_G7, CHK_L, CHK_E, CHK_BST })
                {
                    c.Enabled = true;
                    c.Checked = true;
                }
        }
        private void CHK_RandomClass_CheckedChanged(object sender, EventArgs e)
        {
            CHK_IgnoreSpecialClass.Enabled = CHK_RandomClass.Checked;
            if (!CHK_RandomClass.Checked)
                CHK_IgnoreSpecialClass.Checked = false;
        }
        private void CHK_RandomShiny_CheckedChanged(object sender, EventArgs e)
        {
            NUD_Shiny.Enabled = CHK_RandomShiny.Checked;
        }
        private void CHK_Level_CheckedChanged(object sender, EventArgs e)
        {
            NUD_LevelBoost.Enabled = CHK_Level.Checked;
        }
        private int[] GetRandomMega(out int species)
        {
            int rnd = Util.rand.Next(0, MegaDictionary.Count - 1);
            species = MegaDictionary.Keys.ElementAt(rnd);
            return MegaDictionary.Values.ElementAt(rnd);
        }
    }
}
