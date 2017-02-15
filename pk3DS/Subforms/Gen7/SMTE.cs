using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class SMTE : Form
    {
        private readonly trdata7[] Trainers;
        private string[][] AltForms;
        private int index = -1;
        private PictureBox[] pba;

        private readonly byte[][] trclass, trdata, trpoke;
        private readonly string[] abilitylist = Main.getText(TextName.AbilityNames);
        private readonly string[] movelist = Main.getText(TextName.MoveNames);
        private readonly string[] itemlist = Main.getText(TextName.ItemNames);
        private readonly string[] specieslist = Main.getText(TextName.SpeciesNames);
        private readonly string[] types = Main.getText(TextName.Types);
        private readonly string[] natures = Main.getText(TextName.Natures);
        private readonly string[] forms = Enumerable.Range(0, 1000).Select(i => i.ToString("000")).ToArray();
        private string[] trName = Main.getText(TextName.TrainerNames);
        private readonly string[] trClass = Main.getText(TextName.TrainerClasses);
        private readonly string[] trText = Main.getText(TextName.TrainerText);

        public SMTE(byte[][] trc, byte[][] trd, byte[][] trp)
        {
            trclass = trc;
            trdata = trd;
            trpoke = trp;
            InitializeComponent();

            mnuView.Click += clickView;
            mnuSet.Click += clickSet;
            mnuDelete.Click += clickDelete;
            Trainers = new trdata7[trdata.Length];
            Setup();
            foreach (var pb in pba)
                pb.Click += clickSlot;

            CB_TrainerID.SelectedIndex = 0;
            CB_Moves.SelectedIndex = 0;
        }

        private int getSlot(object sender)
        {
            var send = ((sender as ToolStripItem)?.Owner as ContextMenuStrip)?.SourceControl ?? sender as PictureBox;
            return Array.IndexOf(pba, send);
        }
        private void clickSlot(object sender, EventArgs e)
        {
            switch (ModifierKeys)
            {
                case Keys.Control: clickView(sender, e); break;
                case Keys.Shift: clickSet(sender, e); break;
                case Keys.Alt: clickDelete(sender, e); break;
            }
        }
        private void clickView(object sender, EventArgs e)
        {
            int slot = getSlot(sender);
            if (pba[slot].Image == null)
            { SystemSounds.Exclamation.Play(); return; }
            
            // Load the PKM
            var pk = Trainers[index].Pokemon[slot];
            if (pk.Species != 0)
            {
                try { populateFieldsTP7(pk); }
                catch { }
                // Visual to display what slot is currently loaded.
                getSlotColor(slot, Properties.Resources.slotView);
            }
            else
                SystemSounds.Exclamation.Play();
        }
        private void clickSet(object sender, EventArgs e)
        {
            int slot = getSlot(sender);
            if (CB_Species.SelectedIndex == 0)
            { Util.Alert("Can't set empty slot."); return; }

            var pk = prepareTP7();
            var tr = Trainers[index];
            if (slot < tr.NumPokemon)
                tr.Pokemon[slot] = pk;
            else
            {
                tr.Pokemon.Add(pk);
                slot = tr.Pokemon.Count - 1;
                Trainers[index].NumPokemon = (int)(++NUD_NumPoke.Value);
            }

            getQuickFiller(pba[slot], pk);
            getSlotColor(slot, Properties.Resources.slotSet);
        }
        private void clickDelete(object sender, EventArgs e)
        {
            int slot = getSlot(sender);

            if (slot < Trainers[index].NumPokemon)
            {
                Trainers[index].Pokemon.RemoveAt(slot);
                Trainers[index].NumPokemon = (int)(--NUD_NumPoke.Value);
            }

            populateTeam(Trainers[index]);
            getSlotColor(slot, Properties.Resources.slotDel);
        }

        private void populateTeam(trdata7 tr)
        {
            for (int i = 0; i < tr.NumPokemon; i++)
                getQuickFiller(pba[i], tr.Pokemon[i]);
            for (int i = tr.NumPokemon; i < 6; i++)
                pba[i].Image = null;
        }

        private void getSlotColor(int slot, Image color)
        {
            foreach (PictureBox t in pba)
                t.BackgroundImage = null;

            pba[slot].BackgroundImage = color;
        }
        private static void getQuickFiller(PictureBox pb, trpoke7 pk)
        {
            Bitmap rawImg = Util.getSprite(pk.Species, pk.Form, pk.Gender, pk.Item, pk.Shiny);
            pb.Image = Util.scaleImage(rawImg, 2);
        }

        // Top Level Functions
        private void refreshFormAbility(object sender, EventArgs e)
        {
            if (index < 0)
                return;
            pkm.Form = CB_Forme.SelectedIndex;
            refreshPKMSlotAbility();
        }
        private void refreshSpeciesAbility(object sender, EventArgs e)
        {
            if (index < 0)
                return;
            pkm.Species = (ushort)CB_Species.SelectedIndex;
            FormUtil.setForms(CB_Species.SelectedIndex, CB_Forme, AltForms);
            refreshPKMSlotAbility();
        }
        private void refreshPKMSlotAbility()
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
        
        private void Setup()
        {
            AltForms = forms.Select(f => Enumerable.Range(0, 100).Select(i => i.ToString()).ToArray()).ToArray();

            Array.Resize(ref trName, trdata.Length);
            CB_TrainerID.Items.Clear();
            for (int i = 0; i < trdata.Length; i++)
                CB_TrainerID.Items.Add(string.Format("{1} - {0}", i.ToString("000"), trName[i] ?? "UNKNOWN"));

            CB_Trainer_Class.Items.Clear();
            for (int i = 0; i < trClass.Length; i++)
                CB_Trainer_Class.Items.Add(string.Format("{1} - {0}", i.ToString("000"), trClass[i]));

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
            foreach (string s in natures)
                CB_Nature.Items.Add(s);

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
            populateFieldsTP7(pkm);
        }

        private void changeTrainerIndex(object sender, EventArgs e)
        {
            saveEntry();
            loadEntry();
            if (TC_trdata.SelectedIndex == TC_trdata.TabCount - 1) // last
                TC_trdata.SelectedIndex = 0;
        }
        private void saveEntry()
        {
            if (index < 0)
                return;
            var tr = Trainers[index];
            prepareTR7(tr);
            saveData(tr, index);
            trName[index] = TB_TrainerName.Text;
        }
        private void saveData(trdata7 tr, int i)
        {
            byte[] trd;
            byte[] trp;
            tr.Write(out trd, out trp);
            trdata[i] = trd;
            trpoke[i] = trp;
        }
        private void loadEntry()
        {
            index = CB_TrainerID.SelectedIndex;
            var tr = Trainers[index];

            loading = true;
            TB_TrainerName.Text = trName[index];

            populateFieldsTD7(tr);
            loading = false;
        }

        private bool loading;
        private trpoke7 pkm;
        private void populateFieldsTP7(trpoke7 pk)
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
            updateStats(null, null);
        }
        private trpoke7 prepareTP7()
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

            pk.IV_HP = Util.ToInt32(TB_HPIV);
            pk.IV_ATK = Util.ToInt32(TB_ATKIV);
            pk.IV_DEF = Util.ToInt32(TB_DEFIV);
            pk.IV_SPA = Util.ToInt32(TB_SPAIV);
            pk.IV_SPE = Util.ToInt32(TB_SPEIV);
            pk.IV_SPD = Util.ToInt32(TB_SPDIV);

            pk.EV_HP = Util.ToInt32(TB_HPEV);
            pk.EV_ATK = Util.ToInt32(TB_ATKEV);
            pk.EV_DEF = Util.ToInt32(TB_DEFEV);
            pk.EV_SPA = Util.ToInt32(TB_SPAEV);
            pk.EV_SPE = Util.ToInt32(TB_SPEEV);
            pk.EV_SPD = Util.ToInt32(TB_SPDEV);

            return pk;
        }
        private void populateFieldsTD7(trdata7 tr)
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
            populateTeam(tr);
        }
        private void prepareTR7(trdata7 tr)
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
        private static int[] getHighAttacks(trpoke7 pk)
        {
            int i = Main.Config.Personal.getFormeIndex(pk.Species, pk.Form);
            var learnset = Main.Config.Learnsets[i];
            var moves = learnset.Moves.OrderByDescending(move => Main.Config.Moves[move].Power).Distinct().Take(4).ToArray();
            Array.Resize(ref moves, 4);
            return moves;
        }
        private static int[] getCurrentAttacks(trpoke7 pk)
        {
            int i = Main.Config.Personal.getFormeIndex(pk.Species, pk.Form);
            var learnset = Main.Config.Learnsets[i];
            var moves = learnset.getCurrentMoves(pk.Level);
            Array.Resize(ref moves, 4);
            return moves;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            saveEntry();
            base.OnFormClosing(e);
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
                    sb.Append(getTrainerString(Trainer));
                File.WriteAllText(sfd.FileName, sb.ToString());
            }
        }
        private string getTrainerString(trdata7 tr)
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

        private void updateNumPokemon(object sender, EventArgs e)
        {
            if (index < 0)
                return;
            Trainers[index].NumPokemon = (int) (NUD_NumPoke.Value);
        }
        private void updateTrainerName(object sender, EventArgs e)
        {
            if (loading)
                return;
            CB_TrainerID.Items[index] = $"{TB_TrainerName.Text} - {index:000}";
        }

        private static bool updatingStats;

        private void updateStats(object sender, EventArgs e)
        {
            if (updatingStats)
                return;
            var tb_iv = new[] { TB_HPIV, TB_ATKIV, TB_DEFIV, TB_SPEIV, TB_SPAIV, TB_SPDIV };
            var tb_ev = new[] { TB_HPEV, TB_ATKEV, TB_DEFEV, TB_SPEEV, TB_SPAEV, TB_SPDEV };
            for (int i = 0; i < 6; i++)
            {
                updatingStats = true;
                if (Util.ToInt32(tb_iv[i]) > 31)
                    tb_iv[i].Text = "31";
                if (Util.ToInt32(tb_ev[i]) > 255)
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

            TB_IVTotal.Text = tb_iv.Select(Util.ToInt32).Sum().ToString();
            TB_EVTotal.Text = tb_ev.Select(Util.ToInt32).Sum().ToString();

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
            var ivs = tb_iv.Select(tb => Util.ToInt32(tb) & 1).ToArray();
            updatingStats = true;
            CB_HPType.SelectedIndex = 15 * ((ivs[0]) + 2 * ivs[1] + 4 * ivs[2] + 8 * ivs[3] + 16 * ivs[4] + 32 * ivs[5]) / 63;
            updatingStats = false;
        }

        private void updateHPType(object sender, EventArgs e)
        {
            if (updatingStats)
                return;
            var tb_iv = new[] { TB_HPIV, TB_ATKIV, TB_DEFIV, TB_SPAIV, TB_SPDIV, TB_SPEIV };
            int[] newIVs = setHPIVs(CB_HPType.SelectedIndex, tb_iv.Select(Util.ToInt32).ToArray());
            updatingStats = true;
            TB_HPIV.Text = newIVs[0].ToString();
            TB_ATKIV.Text = newIVs[1].ToString();
            TB_DEFIV.Text = newIVs[2].ToString();
            TB_SPAIV.Text = newIVs[3].ToString();
            TB_SPDIV.Text = newIVs[4].ToString();
            TB_SPEIV.Text = newIVs[5].ToString();
            updatingStats = false;
        }
        public static int[] setHPIVs(int type, int[] ivs)
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
            CB_TrainerID.SelectedIndex = 0;
            Randomizer rnd = new Randomizer(CHK_G1.Checked, CHK_G2.Checked, CHK_G3.Checked, CHK_G4.Checked, CHK_G5.Checked, 
                CHK_G6.Checked, CHK_G7.Checked, CHK_L.Checked, CHK_E.Checked, Shedinja: true)
            {
                BST = CHK_BST.Checked,
                Stats = Main.SpeciesStat
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
                    } while (/*trClass[rv].StartsWith("[~") || */(Legal.SpecialClasses_SM.Contains(rv) && !CHK_IgnoreSpecialClass.Checked));
                    // don't allow disallowed classes
                    tr.TrainerClass = (byte) rv;
                }

                if (tr.NumPokemon < NUD_RMin.Value)
                {
                    var avgBST = (int)tr.Pokemon.Average(pk => Main.SpeciesStat[pk.Species].BST);
                    int avgLevel = (int)tr.Pokemon.Average(pk => pk.Level);
                    var pinfo = Main.SpeciesStat.OrderBy(pk => Math.Abs(avgBST - pk.BST)).First();
                    int avgSpec = Array.IndexOf(Main.SpeciesStat, pinfo);
                    for (int p = tr.NumPokemon; p < NUD_RMin.Value; p++)
                        tr.Pokemon.Add(new trpoke7
                        {
                            Species = rnd.getRandomSpecies(avgSpec),
                            Level = avgLevel,
                        });
                    tr.NumPokemon = (int)NUD_RMin.Value;
                }
                if (tr.NumPokemon > NUD_RMax.Value)
                {
                    tr.Pokemon.RemoveRange((int)NUD_RMax.Value, (int)(tr.NumPokemon - NUD_RMax.Value));
                    tr.NumPokemon = (int)NUD_RMax.Value;
                }

                // PKM Properties
                foreach (var pk in tr.Pokemon)
                {
                    if (CHK_RandomPKM.Checked)
                    {
                        int Type = CHK_TypeTheme.Checked ? (int)Util.rnd32()%17 : -1;
                        pk.Species = rnd.getRandomSpecies(pk.Species, Type);
                        pk.Form = Randomizer.GetRandomForme(pk.Species, CHK_RandomMegaForm.Checked, true, Main.SpeciesStat);
                        pk.Gender = 0; // Random Gender
                    }
                    if (CHK_Level.Checked)
                        pk.Level = Randomizer.getModifiedLevel(pk.Level, NUD_LevelBoost.Value);
                    if (CHK_RandomShiny.Checked)
                        pk.Shiny = Util.rand.Next(0, 100 + 1) < NUD_Shiny.Value;
                    if (CHK_RandomItems.Checked)
                        pk.Item = items[Util.rnd32()%items.Length];
                    if (CHK_RandomAbilities.Checked)
                        pk.Ability = (int)Util.rnd32()%4;
                    if (CHK_MaxDiffPKM.Checked)
                        pk.IVs = new[] {31, 31, 31, 31, 31, 31};

                    switch (CB_Moves.SelectedIndex)
                    {
                        case 1: // Random
                            pk.Moves = Randomizer.getRandomMoves(
                                Main.Config.Personal.getFormeEntry(pk.Species, pk.Form).Types,
                                Main.Config.Moves,
                                CHK_Damage.Checked, (int)NUD_Damage.Value,
                                CHK_STAB.Checked, (int)NUD_STAB.Value);
                            break;
                        case 2: // Current LevelUp
                            pk.Moves = getCurrentAttacks(pk);
                            break;
                        case 3: // High Attacks
                            pk.Moves = getHighAttacks(pk);
                            break;
                    }
                }
                saveData(tr, i);
            }
            Util.Alert("Randomized!");
        }
        private void B_HighAttack_Click(object sender, EventArgs e)
        {
            pkm.Species = CB_Species.SelectedIndex;
            pkm.Level = (int)NUD_Level.Value;
            pkm.Form = CB_Forme.SelectedIndex;
            var moves = getHighAttacks(pkm);
            setMoves(moves);
        }
        private void B_CurrentAttack_Click(object sender, EventArgs e)
        {
            pkm.Species = CB_Species.SelectedIndex;
            pkm.Level = (int)NUD_Level.Value;
            pkm.Form = CB_Forme.SelectedIndex;
            var moves = getCurrentAttacks(pkm);
            setMoves(moves);
        }
        private void B_Clear_Click(object sender, EventArgs e)
        {
            setMoves(new int[4]);
        }
        private void setMoves(int[] moves)
        {
            var mcb = new[] { CB_Move1, CB_Move2, CB_Move3, CB_Move4 };
            for (int i = 0; i < mcb.Length; i++)
                mcb[i].SelectedIndex = moves[i];
        }

        // Randomization UI
        private void CB_Moves_SelectedIndexChanged(object sender, EventArgs e)
        {
            CHK_Damage.Visible = CHK_STAB.Visible = NUD_Damage.Visible = NUD_STAB.Visible = CB_Moves.SelectedIndex == 1; // Randomized
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
            CHK_BST.Visible = CHK_RandomPKM.Checked;
            if (CHK_RandomPKM.Checked)
                return;
            foreach (CheckBox c in new[] { CHK_G1, CHK_G2, CHK_G3, CHK_G4, CHK_G5, CHK_G6, CHK_G7, CHK_L, CHK_E })
            {
                c.Visible = false;
                c.Checked = true;
            }
        }
        private void CHK_RandomClass_CheckedChanged(object sender, EventArgs e)
        {
            CHK_IgnoreSpecialClass.Visible = CHK_RandomClass.Checked;
        }
        private void CHK_RandomShiny_CheckedChanged(object sender, EventArgs e)
        {
            NUD_Shiny.Enabled = CHK_RandomShiny.Checked;
        }
        private void CHK_Level_CheckedChanged(object sender, EventArgs e)
        {
            NUD_LevelBoost.Enabled = CHK_Level.Checked;
        }
    }
}
