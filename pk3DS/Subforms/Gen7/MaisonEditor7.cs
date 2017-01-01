using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class MaisonEditor7 : Form
    {
        public MaisonEditor7(byte[][] trd, byte[][] trp, bool royal)
        {
            trFiles = trd;
            pkFiles = trp;
            Array.Resize(ref specieslist, Main.Config.MaxSpeciesID);
            movelist[0] = specieslist[0] = itemlist[0] = "";
            
            trNames = Main.getText(royal ? TextName.BattleRoyalNames : TextName.BattleTreeNames); Array.Resize(ref trNames, trFiles.Length);

            InitializeComponent();
            Setup();
        }

        private readonly byte[][] trFiles;
        private readonly string[] trNames;
        private readonly byte[][] pkFiles;
        private readonly string[] natures = Main.getText(TextName.Natures);
        private readonly string[] movelist = Main.getText(TextName.MoveNames);
        private readonly string[] specieslist = Main.getText(TextName.SpeciesNames);
        private readonly string[] trClass = Main.getText(TextName.TrainerClasses);
        private readonly string[] itemlist = Main.getText(TextName.ItemNames);
        private int trEntry = -1;
        private int pkEntry = -1;
        private bool dumping;
        private void Setup()
        {
            foreach (string s in trClass) CB_Class.Items.Add(s);
            foreach (string s in specieslist) CB_Species.Items.Add(s);
            foreach (string s in movelist) CB_Move1.Items.Add(s);
            foreach (string s in movelist) CB_Move2.Items.Add(s);
            foreach (string s in movelist) CB_Move3.Items.Add(s);
            foreach (string s in movelist) CB_Move4.Items.Add(s);
            foreach (string s in natures) CB_Nature.Items.Add(s);
            foreach (string s in itemlist) CB_Item.Items.Add(s);
            foreach (string s in trNames) CB_Trainer.Items.Add(s ?? "UNKNOWN");
            for (int i = 0; i < pkFiles.Length; i++) CB_Pokemon.Items.Add(i.ToString());

            CB_Trainer.SelectedIndex = 1;
        }
        private void changeTrainer(object sender, EventArgs e)
        {
            setTrainer();
            trEntry = CB_Trainer.SelectedIndex;
            getTrainer();
            if (GB_Trainer.Enabled)
                LB_Choices.SelectedIndex = 0;
        }
        private void changePokemon(object sender, EventArgs e)
        {
            setPokemon();
            pkEntry = CB_Pokemon.SelectedIndex;
            getPokemon();
        }
        private void getTrainer()
        {
            if (trEntry < 0) return;

            // Get
            LB_Choices.Items.Clear();
            Maison7.Trainer tr = new Maison7.Trainer(trFiles[trEntry]);

            CB_Class.SelectedIndex = tr.Class;
            GB_Trainer.Enabled = tr.Count > 0;

            foreach (ushort Entry in tr.Choices)
                LB_Choices.Items.Add(Entry.ToString());
        }
        private void setTrainer()
        {
            if (trEntry < 0 || !GB_Trainer.Enabled || dumping) return;
            // Gather
            Maison7.Trainer tr = new Maison7.Trainer
            {
                Class = (ushort) CB_Class.SelectedIndex,
                Count = (ushort) LB_Choices.Items.Count
            };
            tr.Choices = new ushort[tr.Count];
            for (int i = 0; i < tr.Count; i++)
                tr.Choices[i] = Convert.ToUInt16(LB_Choices.Items[i].ToString());
            Array.Sort(tr.Choices);
            trFiles[trEntry] = tr.Write();
        }
        private void getPokemon()
        {
            if (pkEntry < 0 || dumping) return;
            Maison7.Pokemon pkm = new Maison7.Pokemon(pkFiles[pkEntry]);

            // Get
            CB_Move1.SelectedIndex = pkm.Moves[0];
            CB_Move2.SelectedIndex = pkm.Moves[1];
            CB_Move3.SelectedIndex = pkm.Moves[2];
            CB_Move4.SelectedIndex = pkm.Moves[3];
            CHK_HP.Checked = pkm.HP;
            CHK_ATK.Checked = pkm.ATK;
            CHK_DEF.Checked = pkm.DEF;
            CHK_Spe.Checked = pkm.SPE;
            CHK_SpA.Checked = pkm.SPA;
            CHK_SpD.Checked = pkm.SPD;
            CB_Nature.SelectedIndex = pkm.Nature;
            CB_Item.SelectedIndex = pkm.Item;
            NUD_Form.Value = pkm.Form;

            CB_Species.SelectedIndex = pkm.Species; // Loaded last in order to refresh the sprite with all info.
            // Last 2 Bytes are unused.
        }
        private void setPokemon()
        {
            if (pkEntry < 0 || dumping) return;

            // Each File is 16 Bytes.
            Maison7.Pokemon pkm = new Maison7.Pokemon(pkFiles[pkEntry])
            {
                Species = (ushort) CB_Species.SelectedIndex,
                HP = CHK_HP.Checked,
                ATK = CHK_ATK.Checked,
                DEF = CHK_DEF.Checked,
                SPE = CHK_Spe.Checked,
                SPA = CHK_SpA.Checked,
                SPD = CHK_SpD.Checked,
                Nature = (byte) CB_Nature.SelectedIndex,
                Item = (ushort) CB_Item.SelectedIndex,
                Move1 = CB_Move1.SelectedIndex,
                Move2 = CB_Move2.SelectedIndex,
                Move3 = CB_Move3.SelectedIndex,
                Move4 = CB_Move4.SelectedIndex,
                Form = (ushort)NUD_Form.Value,
            };

            byte[] data = pkm.Write();
            pkFiles[pkEntry] = data;
        }

        private void changeSpecies(object sender, EventArgs e)
        {
            PB_PKM.Image = Util.getSprite(CB_Species.SelectedIndex, (int)NUD_Form.Value, 0, CB_Item.SelectedIndex);
        }

        private void B_Remove_Click(object sender, EventArgs e)
        {
            if (LB_Choices.SelectedIndex > -1 && GB_Trainer.Enabled)
                LB_Choices.Items.RemoveAt(LB_Choices.SelectedIndex);
        }
        private void B_Set_Click(object sender, EventArgs e)
        {
            if (LB_Choices.SelectedIndex <= -1 || !GB_Trainer.Enabled) return;

            int toAdd = CB_Pokemon.SelectedIndex;
            int count = LB_Choices.Items.Count;
            List<ushort> choices = new List<ushort>();
            for (int i = 0; i < count; i++)
                choices.Add(Convert.ToUInt16(LB_Choices.Items[i].ToString()));

            if (Array.IndexOf(choices.ToArray(), toAdd) > 0) return; // Abort if already in the list
            choices.Add((ushort)toAdd); // Add it to the list.

            // Get new list, and sort it.
            ushort[] choiceList = choices.ToArray(); Array.Sort(choiceList);

            // Set new list.
            LB_Choices.Items.Clear();
            foreach (ushort t in choiceList)
                LB_Choices.Items.Add(t.ToString());

            // Set current index to the one just added.
            LB_Choices.SelectedIndex = Array.IndexOf(choiceList, toAdd);
        }
        private void B_View_Click(object sender, EventArgs e)
        {
            if (LB_Choices.SelectedIndex > -1 && GB_Trainer.Enabled)
                CB_Pokemon.SelectedIndex = Convert.ToUInt16(LB_Choices.Items[LB_Choices.SelectedIndex].ToString());
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setTrainer();
            setPokemon();
        }

        private void DumpTRs_Click(object sender, EventArgs e)
        {
            dumping = true;
            string result = "";
            for (int i = 0; i < CB_Trainer.Items.Count; i++)
            {
                CB_Trainer.SelectedIndex = i;
                int count = LB_Choices.Items.Count - 1;
                if (count > 0)
                {
                    result += "======" + Environment.NewLine + i + " - (" + CB_Class.Text + ") " + CB_Trainer.Text + Environment.NewLine + "======" + Environment.NewLine;
                    result += "Choices: ";
                    for (int c = 0; c < count; c++)
                        result += LB_Choices.Items[c] + ", ";

                    result += Environment.NewLine; result += Environment.NewLine;
                }
            }
            SaveFileDialog sfd = new SaveFileDialog {FileName = "Maison Trainers.txt", Filter = "Text File|*.txt"};

            SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                File.WriteAllText(path, result, Encoding.Unicode);
            }
            dumping = false;
            CB_Trainer.SelectedIndex = 0;
        }
        private void B_DumpPKs_Click(object sender, EventArgs e)
        {
            //File.WriteAllBytes("maiz", pkFiles.SelectMany(t => t).ToArray());
            string[] stats = {"HP", "ATK", "DEF", "Spe", "SpA", "SpD"};
            string result = "";
            for (int i = 0; i < pkFiles.Length; i++)
            {
                var pk = new Maison7.Pokemon(pkFiles[i]);
                if (pk.Species == 0)
                    continue;

                result += "======" + Environment.NewLine;
                result += $"{i} - {specieslist[pk.Species]}" + Environment.NewLine;
                result += "======" + Environment.NewLine;
                result += $"Held Item: {itemlist[pk.Item]}" + Environment.NewLine;
                result += $"Nature: {natures[pk.Nature]}" + Environment.NewLine;
                result += $"Move 1: {movelist[pk.Move1]}" + Environment.NewLine;
                result += $"Move 2: {movelist[pk.Move2]}" + Environment.NewLine;
                result += $"Move 3: {movelist[pk.Move3]}" + Environment.NewLine;
                result += $"Move 4: {movelist[pk.Move4]}" + Environment.NewLine;

                var EVstr = string.Join(",", pk.EVs.Select((iv, x) => iv ? stats[x] : string.Empty).Where(x => !string.IsNullOrWhiteSpace(x)));
                result += $"EV'd in: {(pk.EVs.Any() ? EVstr : "None")}" + Environment.NewLine;

                if (pk.Form > 0)
                    result += $"Form: {pk.Form}" + Environment.NewLine;

                result += Environment.NewLine;
            }
            SaveFileDialog sfd = new SaveFileDialog {FileName = "Maison Pokemon.txt", Filter = "Text File|*.txt"};

            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            
            File.WriteAllText(sfd.FileName, result, Encoding.Unicode);
        }
    }
}