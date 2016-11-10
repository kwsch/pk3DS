using System;
using System.Collections.Generic;
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
        private static readonly Random rand = new Random();
        internal static uint rnd32()
        {
            return (uint)rand.Next(1 << 30) << 2 | (uint)rand.Next(1 << 2);
        }
        private int index = -1;
        private PictureBox[] pba;

        private readonly string[] trdatapaths = Directory.GetFiles("trdata");
        private readonly string[] trpokepaths = Directory.GetFiles("trpoke");
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

        public SMTE()
        {
            Console.WriteLine("Starting SMTE...");
            InitializeComponent();

            foreach (var pb in pba)
                pb.Click += clickSlot;
            mnuView.Click += clickView;
            mnuSet.Click += clickSet;
            mnuDelete.Click += clickDelete;
            Trainers = new trdata7[trdatapaths.Length];
            Setup();

            CB_TrainerID.SelectedIndex = 0;
        }

        private int getSlot(object sender)
        {
            return Array.IndexOf(pba, sender as PictureBox);
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
            if (CB_Pokemon.SelectedIndex == 0)
            { Util.Alert("Can't set empty slot."); return; }

            var pk = prepareTP7();
            var tr = Trainers[index];
            if (tr.Pokemon.Count < slot)
                tr.Pokemon[slot] = pk;
            else
            {
                tr.Pokemon.Add(pk);
                slot = tr.Pokemon.Count - 1;
            }

            getQuickFiller(pba[slot], pk);
            getSlotColor(slot, Properties.Resources.slotSet);
        }
        private void clickDelete(object sender, EventArgs e)
        {
            int slot = getSlot(sender);

            var pk = new trpoke7();
            Trainers[index].Pokemon.RemoveAt(slot);

            getQuickFiller(pba[slot], pk);
            getSlotColor(slot, Properties.Resources.slotDel);
        }
        private void getSlotColor(int slot, Image color)
        {
            foreach (PictureBox t in pba)
                t.BackgroundImage = null;

            pba[slot].BackgroundImage = color;
        }
        private static void getQuickFiller(PictureBox pb, trpoke7 pk)
        {
            Bitmap rawImg = Util.getSprite(pk.Species, pk.Form, pk.Gender, pk.Item);
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
            pkm.Species = (ushort)CB_Pokemon.SelectedIndex;
            Personal.setForms(CB_Pokemon.SelectedIndex, CB_Forme, AltForms);
            refreshPKMSlotAbility();
        }
        private void refreshPKMSlotAbility()
        {
            int previousAbility = CB_Ability.SelectedIndex;

            int species = CB_Pokemon.SelectedIndex;
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

            Array.Resize(ref trName, trdatapaths.Length);
            CB_TrainerID.Items.Clear();
            for (int i = 0; i < trdatapaths.Length; i++)
                CB_TrainerID.Items.Add(string.Format("{1} - {0}", i.ToString("000"), trName[i] ?? "UNKNOWN"));

            CB_Trainer_Class.Items.Clear();
            for (int i = 0; i < trClass.Length; i++)
                CB_Trainer_Class.Items.Add(string.Format("{1} - {0}", i.ToString("000"), trClass[i]));

            Trainers[0] = new trdata7();

            for (int i = 1; i < trdatapaths.Length; i++)
            {
                Trainers[i] = new trdata7(File.ReadAllBytes(trdatapaths[i]), File.ReadAllBytes(trpokepaths[i]))
                {
                    Name = trName[i],
                    ID = i
                };
            }

            specieslist[0] = "---";
            abilitylist[0] = itemlist[0] = movelist[0] = "(None)";
            pba = new[] { PB_Team1, PB_Team2, PB_Team3, PB_Team4, PB_Team5, PB_Team6 };
            
            CB_Pokemon.Items.Clear();
            foreach (string s in specieslist)
                CB_Pokemon.Items.Add(s);

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

            CB_Pokemon.SelectedIndex = 0;
            CB_Item_1.Items.Clear();
            CB_Item_2.Items.Clear();
            CB_Item_3.Items.Clear();
            CB_Item_4.Items.Clear();
            CB_Prize.Items.Clear();
            foreach (string s in itemlist)
            {
                CB_Item_1.Items.Add(s);
                CB_Item_2.Items.Add(s);
                CB_Item_3.Items.Add(s);
                CB_Item_4.Items.Add(s);
                CB_Prize.Items.Add(s);
            }

            CB_Money.Items.Clear();
            for (int i = 0; i < 256; i++)
            { CB_Money.Items.Add(i.ToString()); }

            CB_Battle_Type.Items.Clear();
            CB_Battle_Type.Items.Add("Single");
            CB_Battle_Type.Items.Add("Double");
            CB_Battle_Type.Items.Add("Royal");

            CB_TrainerID.SelectedIndex = 1;
            index = 0;

            SystemSounds.Asterisk.Play();
        }

        private void changeTrainerIndex(object sender, EventArgs e)
        {
            saveEntry();
            loadEntry();
        }
        private void saveEntry()
        {
            var tr = Trainers[index];
            prepareTR7(tr);
            byte[] trdata;
            byte[] trpoke;
            tr.Write(out trdata, out trpoke);
            File.WriteAllBytes(trdatapaths[index], trdata);
            File.WriteAllBytes(trpokepaths[index], trpoke);
        }
        private void loadEntry()
        {
            index = CB_TrainerID.SelectedIndex;
            var tr = Trainers[index];
            populateFieldsTD7(tr);
        }

        private trpoke7 pkm;
        private void populateFieldsTP7(trpoke7 pk)
        {
            pkm = pk;

            CB_Pokemon.SelectedIndex = pkm.Species;
            CB_Forme.SelectedIndex = pkm.Form;
            NUD_Level.Value = Math.Min(NUD_Level.Maximum, pkm.Level);
            CB_Ability.SelectedIndex = pkm.Ability;
            CB_Item.SelectedIndex = pkm.Item;
            CHK_Shiny.Checked = pkm.Shiny;
            CB_Nature.SelectedIndex = pkm.Nature;
            CB_Gender.SelectedIndex = pkm.Gender;

            CB_Move1.SelectedIndex = pkm.Move1;
            CB_Move2.SelectedIndex = pkm.Move2;
            CB_Move3.SelectedIndex = pkm.Move3;
            CB_Move4.SelectedIndex = pkm.Move4;
        }
        private trpoke7 prepareTP7()
        {
            pkm.Species = (ushort)CB_Pokemon.SelectedIndex;
            pkm.Form = CB_Forme.SelectedIndex;
            pkm.Level = (byte)NUD_Level.Value;
            pkm.Ability = CB_Ability.SelectedIndex;
            pkm.Item = (ushort) CB_Item.SelectedIndex;
            pkm.Shiny = CHK_Shiny.Checked;
            pkm.Nature = CB_Nature.SelectedIndex;
            pkm.Gender = CB_Gender.SelectedIndex;

            pkm.Move1 = (ushort)CB_Move1.SelectedIndex;
            pkm.Move2 = (ushort)CB_Move2.SelectedIndex;
            pkm.Move3 = (ushort)CB_Move3.SelectedIndex;
            pkm.Move4 = (ushort)CB_Move4.SelectedIndex;

            return pkm;
        }
        private void populateFieldsTD7(trdata7 tr)
        {
            // Load Trainer Data
            CB_Trainer_Class.SelectedIndex = tr.TrainerClass;
            NUD_NumPoke.Value = tr.NumPokemon;
        }
        private void prepareTR7(trdata7 tr)
        {
            tr.TrainerClass = (byte)CB_Trainer_Class.SelectedIndex;
            tr.NumPokemon = (byte)NUD_NumPoke.Value;
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
    }
}
