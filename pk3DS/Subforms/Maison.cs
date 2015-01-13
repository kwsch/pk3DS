using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace pk3DS
{
    public partial class Maison : Form
    {
        public Maison(bool rom_oras, bool super)
        {
            oras = rom_oras;

            movelist = Main.getText((oras) ? 14 : 13);
            itemlist = Main.getText((oras) ? 114 : 96);
            specieslist = Main.getText((oras) ? 98 : 80); Array.Resize(ref specieslist, 722);
            movelist[0] = specieslist[0] = itemlist[0] = "";

            trClass = Main.getText((oras) ? 21 : 20);
            natures = Main.getText((oras) ? 51 : 47);

            trFiles = Directory.GetFiles((super) ? "maisontrS" : "maisontrN");
            pkFiles = Directory.GetFiles((super) ? "maisonpkS" : "maisonpkN");

            int trTXTFile = (oras) ? 153 : 130;
            trNames = Main.getText((super) ? trTXTFile : trTXTFile + 1); Array.Resize(ref trNames, trFiles.Length);

            InitializeComponent();
            Setup();
        }
        string[] trFiles;
        string[] trNames;
        string[] pkFiles;
        bool oras = false;
        string[] natures;
        string[] movelist;
        string[] specieslist;
        string[] trClass;
        string[] itemlist;
        int trEntry = -1;
        int pkEntry = -1;
        bool dumping = false;
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
            byte[] data = File.ReadAllBytes(trFiles[trEntry]);
            // Get
            CB_Class.SelectedIndex = BitConverter.ToUInt16(data, 0);
            ushort count = BitConverter.ToUInt16(data, 2);
            LB_Choices.Items.Clear();
            GB_Trainer.Enabled = (count > 0);
            for (int i = 0; i < count; i++)
                LB_Choices.Items.Add(BitConverter.ToUInt16(data, 4 + 2 * i).ToString());
        }
        private void setTrainer()
        {
            if (trEntry < 0 || !GB_Trainer.Enabled || dumping) return;
            // Gather
            int trclass = CB_Class.SelectedIndex;
            int count = LB_Choices.Items.Count;
            byte[] data = new byte[4 + count * 2];
            // Set
            Array.Copy(BitConverter.GetBytes((ushort)trclass), 0, data, 0, 2);
            Array.Copy(BitConverter.GetBytes((ushort)count), 0, data, 2, 2);
            List<ushort> choices = new List<ushort>();
            for (int i = 0; i < count; i++)
                choices.Add(Convert.ToUInt16(LB_Choices.Items[i].ToString()));

            ushort[] choiceList = choices.ToArray(); Array.Sort(choiceList);

            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes((ushort)choiceList[i]), 0, data, 4 + 2 * i, 2);

            File.WriteAllBytes(trFiles[trEntry], data);
        }
        private void getPokemon()
        {
            if (pkEntry < 0 || dumping) return;
            byte[] data = File.ReadAllBytes(pkFiles[pkEntry]);
            
            // Get
            CB_Species.SelectedIndex = BitConverter.ToUInt16(data, 0);
            CB_Move1.SelectedIndex = BitConverter.ToUInt16(data, 2);
            CB_Move2.SelectedIndex = BitConverter.ToUInt16(data, 4);
            CB_Move3.SelectedIndex = BitConverter.ToUInt16(data, 6);
            CB_Move4.SelectedIndex = BitConverter.ToUInt16(data, 8);
            byte EVs = data[0xA];
            CHK_HP.Checked = ((EVs & 0x01) > 0);
            CHK_ATK.Checked = ((EVs & 0x02) > 0);
            CHK_DEF.Checked = ((EVs & 0x04) > 0);
            CHK_Spe.Checked = ((EVs & 0x08) > 0);
            CHK_SpA.Checked = ((EVs & 0x10) > 0);
            CHK_SpD.Checked = ((EVs & 0x20) > 0);
            CB_Nature.SelectedIndex = data[0xB];
            CB_Item.SelectedIndex = BitConverter.ToUInt16(data, 0xC);

            // Last 2 Bytes are unused.
        }
        private void setPokemon()
        {
            if (pkEntry < 0 || dumping) return;

            // Each File is 16 Bytes.
            byte[] data = new byte[0x10];

            // Set
            Array.Copy(BitConverter.GetBytes((ushort)((int)CB_Species.SelectedIndex)), 0, data, 0, 2);
            Array.Copy(BitConverter.GetBytes((ushort)((int)CB_Move1.SelectedIndex)), 0, data, 2, 2);
            Array.Copy(BitConverter.GetBytes((ushort)((int)CB_Move2.SelectedIndex)), 0, data, 4, 2);
            Array.Copy(BitConverter.GetBytes((ushort)((int)CB_Move3.SelectedIndex)), 0, data, 6, 2);
            Array.Copy(BitConverter.GetBytes((ushort)((int)CB_Move4.SelectedIndex)), 0, data, 8, 2);
            int EVs = 0;
            EVs |= (CHK_HP.Checked)  ? 1 << 0 : 0;
            EVs |= (CHK_ATK.Checked) ? 1 << 1 : 0;
            EVs |= (CHK_DEF.Checked) ? 1 << 2 : 0;
            EVs |= (CHK_Spe.Checked) ? 1 << 3 : 0;
            EVs |= (CHK_SpA.Checked) ? 1 << 4 : 0;
            EVs |= (CHK_SpD.Checked) ? 1 << 5 : 0;
            data[0xA] = (byte)EVs;
            data[0xB] = (byte)((int)CB_Nature.SelectedIndex);
            Array.Copy(BitConverter.GetBytes((ushort)((int)CB_Item.SelectedIndex)), 0, data, 0xC, 2);

            // Last 2 Bytes are unused.
            File.WriteAllBytes(pkFiles[pkEntry], data);
        }

        private void changeSpecies(object sender, EventArgs e)
        {
            string filename = "_" + CB_Species.SelectedIndex;
            PB_PKM.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(filename);
        }

        private void B_Remove_Click(object sender, EventArgs e)
        {
            if (LB_Choices.SelectedIndex > -1 && GB_Trainer.Enabled)
                LB_Choices.Items.RemoveAt(LB_Choices.SelectedIndex);
        }
        private void B_Set_Click(object sender, EventArgs e)
        {

            if (LB_Choices.SelectedIndex > -1 && GB_Trainer.Enabled)
            {
                int toAdd = CB_Pokemon.SelectedIndex;
                int count = LB_Choices.Items.Count;
                List<ushort> choices = new List<ushort>();
                for (int i = 0; i < count; i++)
                    choices.Add(Convert.ToUInt16(LB_Choices.Items[i].ToString()));

                if (Array.IndexOf(choices.ToArray(), toAdd) > 0) return; // Abort if already in the list
                else choices.Add((ushort)toAdd); // Add it to the list.

                // Get new list, and sort it.
                ushort[] choiceList = choices.ToArray(); Array.Sort(choiceList);

                // Set new list.
                LB_Choices.Items.Clear();
                for (int i = 0; i < choiceList.Length; i++)
                    LB_Choices.Items.Add(choiceList[i].ToString());

                // Set current index to the one just added.
                LB_Choices.SelectedIndex = Array.IndexOf(choiceList, toAdd);
            }
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
                        result += LB_Choices.Items[c].ToString() + ", ";

                    result += Environment.NewLine; result += Environment.NewLine;
                }
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Maison Trainers.txt";
            sfd.Filter = "Text File|*.txt";

            System.Media.SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                File.WriteAllText(path, result, System.Text.Encoding.Unicode);
            }
            dumping = false;
            CB_Trainer.SelectedIndex = 0;
        }

        private void B_DumpPKs_Click(object sender, EventArgs e)
        {
            dumping = true;
            string result = "";
            for (int i = 0; i < CB_Pokemon.Items.Count; i++)
            {
                CB_Pokemon.SelectedIndex = i;
                if (CB_Species.SelectedIndex > 0)
                {
                    result += "======" + Environment.NewLine + i + " - " + CB_Species.Text + Environment.NewLine + "======" + Environment.NewLine;
                    result += String.Format("Held Item: {0}" + Environment.NewLine, CB_Item.Text);
                    result += String.Format("Nature: {0}" + Environment.NewLine, CB_Nature.Text);
                    result += String.Format("Move 1: {0}" + Environment.NewLine, CB_Move1.Text);
                    result += String.Format("Move 2: {0}" + Environment.NewLine, CB_Move2.Text);
                    result += String.Format("Move 3: {0}" + Environment.NewLine, CB_Move3.Text);
                    result += String.Format("Move 4: {0}" + Environment.NewLine, CB_Move4.Text);

                    result += "EV'd in: ";
                    result += (CHK_HP.Checked) ? "HP, " : "";
                    result += (CHK_ATK.Checked) ? "ATK, " : "";
                    result += (CHK_DEF.Checked) ? "DEF, " : "";
                    result += (CHK_SpA.Checked) ? "SpA, " : "";
                    result += (CHK_SpD.Checked) ? "SpD, " : "";
                    result += (CHK_Spe.Checked) ? "Spe, " : "";
                    result += Environment.NewLine;

                    result += Environment.NewLine; 
                }
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Maison Pokemon.txt";
            sfd.Filter = "Text File|*.txt";

            System.Media.SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                File.WriteAllText(path, result, System.Text.Encoding.Unicode);
            }
            dumping = false;
            CB_Trainer.SelectedIndex = 0;
        }
    }
}
