using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace pk3DS
{
    public partial class MEE : Form
    {
        private string[] files;
        private string[] forms;
        private string[] types;
        string[] specieslist;
        string[] itemlist;
        byte[] personalData;
        private GroupBox[] groupbox_spec = { };
        private ComboBox[] forme_spec = { };
        private ComboBox[] item_spec = { };
        private CheckBox[] checkbox_spec = { };
        private Control[][] all_spec = { };
        private PictureBox[][] picturebox_spec = { };
        private List<cbItem> monNames;
        private bool loaded = false;
        private string[][] AltForms;
        int entry = -1;

        public MEE()            //All the initial settings
        {
            InitializeComponent();
            CB_Species.DisplayMember = "Text";
            CB_Species.ValueMember = "Value";
            #region Intializations

            forms = Main.getText((Main.oras) ? 5 : 5);
            itemlist = Main.getText((Main.oras) ? 114 : 96);
            specieslist = Main.getText((Main.oras) ? 98 : 80);
            Array.Resize(ref specieslist, 722); specieslist[0] = itemlist[0] = "";
            specieslist[32] += "♂"; specieslist[29] += "♀";
            types = Main.getText((Main.oras) ? 18 : 17);
            files = Directory.GetFiles("megaevo");
            string[] personalList = Directory.GetFiles("personal");
            personalData = File.ReadAllBytes(personalList[personalList.Length - 1]);
            AltForms = Personal.getFormList(personalData, Main.oras, specieslist, forms, types, itemlist);

            groupbox_spec = new GroupBox[] {GB_MEvo1,GB_MEvo2,GB_MEvo3};
            item_spec = new ComboBox[] { CB_Item1, CB_Item2, CB_Item3 };
            forme_spec = new ComboBox[] { CB_Forme1, CB_Forme2, CB_Forme3 };
            checkbox_spec = new CheckBox[] { CHK_MEvo1, CHK_MEvo2, CHK_MEvo3 };
            picturebox_spec = new PictureBox[][] { new PictureBox[] { PB_S1, PB_S2, PB_S3 }, new PictureBox[] { PB_M1, PB_M2, PB_M3 } };
            #endregion
            Setup();
            CB_Species.SelectedIndex = 0;
        }
        private void Setup()
        {
            monNames = new List<cbItem>(); 
            List<string> temp_list = new List<string>(specieslist);
            temp_list.Sort();
            foreach (string mon in temp_list)
            {
                cbItem ncbi = new cbItem();
                ncbi.Text = mon;
                ncbi.Value = Array.IndexOf(specieslist, mon);
                monNames.Add(ncbi);
            }

            CB_Species.DataSource = monNames;

            List<string> items = new List<string>(itemlist);
            List<string> sorted_items = new List<string>(itemlist);
            List<cbItem>[] item_lists = new List<cbItem>[item_spec.Length];
            for (int i = 0; i < item_lists.Length; i++)
                item_lists[i] = new List<cbItem>();

            sorted_items.Sort();
            for (int i = 0; i < items.Count; i++)
            {
                int index = items.IndexOf(sorted_items[i]);
                {
                    cbItem ncbi = new cbItem();
                    if (sorted_items[i] == "???") continue; // Don't allow stubbed items.
                    ncbi.Text = sorted_items[i] + " - " + index.ToString("000");
                    ncbi.Value = index;
                    foreach (List<cbItem> l in item_lists)
                        l.Add(ncbi);
                }
                items[index] = "";
            }
            for (int i = 0; i < item_spec.Length; i++)
            {
                item_spec[i].DataSource = item_lists[i];
                item_spec[i].ValueMember = "Value";
                item_spec[i].DisplayMember = "Text";
                item_spec[i].SelectedValue = 0;
            }

            loaded = true;
        }
        private void CHK_Changed(object sender, EventArgs e)
        {
            for (int i = 0; i < groupbox_spec.Length;i++ )
            {
                groupbox_spec[i].Enabled = ((CheckBox)(checkbox_spec[i])).Checked;
                Update_PBs(i);
            }
        }

        private void changeIndex(object sender, EventArgs e)
        {
            setEntry();
            entry = (int)CB_Species.SelectedValue;
            getEntry();
        }
        bool dumping = false;
        private void getEntry()
        {
            if (loaded)
            {
                if (Main.oras && entry == 384 && !dumping) // Current Mon is Rayquaza
                    Util.Alert("Rayquaza is special and uses a different activator for its evolution. If it knows Dragon Ascent, it can Mega Evolve", "Don't edit its evolution table if you want to keep this functionality.");
                
                byte[] data = File.ReadAllBytes(files[entry]);

                foreach (ComboBox CB in forme_spec)
                    Personal.setForms(entry, CB, AltForms);

                for (int i = 0; i < 3; i++)
                {
                    ushort method = BitConverter.ToUInt16(data, 2 + (i * 8));
                    ushort form = BitConverter.ToUInt16(data, i * 8);
                    int item = (int)(BitConverter.ToUInt16(data, 4 + i * 8));
                    checkbox_spec[i].Checked = (method == 1);
                    forme_spec[i].SelectedIndex = form;
                    item_spec[i].SelectedValue = (int)item;
                }
            }
        }
        private void setEntry()
        {
            if (entry < 1 || entry == 384) return;
            string path = files[entry];
            byte[] data = File.ReadAllBytes(path);
            for (int i = 0; i < 3; i++)
            {
                bool isChecked = ((CheckBox)checkbox_spec[i]).Checked;
                Array.Copy(BitConverter.GetBytes(isChecked ? (ushort)1 : (ushort)0), 0, data, 2 + i * 8, 2);
                if (isChecked)
                {
                    int item = (int)(item_spec[i].SelectedValue);
                    int form = (ushort)forme_spec[i].SelectedIndex;
                    Array.Copy(BitConverter.GetBytes((ushort)item), 0, data, 4 + i * 8, 2);
                    Array.Copy(BitConverter.GetBytes((ushort)form), 0, data, i * 8, 2);
                }
                else
                {
                    Array.Copy(BitConverter.GetBytes((ushort)0), 0, data, i * 8, 2);
                    Array.Copy(BitConverter.GetBytes((ushort)0), 0, data, 4 + i * 8, 2);
                }
            }
            File.WriteAllBytes(path, data);
        }

        private void Update_PBs(object sender, EventArgs e)
        {
            if (loaded)
            {
                for (int i = 0; i < checkbox_spec.Length; i++)
                {
                    CheckBox CB = (CheckBox)checkbox_spec[i];
                    if (CB.Checked)
                    {
                        UpdateImage(picturebox_spec[0][i], entry, 0, (int)((ComboBox)item_spec[i]).SelectedValue, 0, false);
                        UpdateImage(picturebox_spec[1][i], entry, (int)((ComboBox)forme_spec[i]).SelectedIndex, (int)((ComboBox)item_spec[i]).SelectedValue, 0, false);
                    }
                    else
                    {
                        UpdateImage(picturebox_spec[0][i], 0, 0, (int)((ComboBox)item_spec[i]).SelectedValue, 0, false);
                        UpdateImage(picturebox_spec[1][i], 0, 0, (int)((ComboBox)item_spec[i]).SelectedValue, 0, false);
                    }
                }
            }
        }
        private void Update_PBs(int i)
        {
            if (loaded)
            {
                 CheckBox CB = (CheckBox)checkbox_spec[i];
                 if (CB.Checked)
                 {
                     UpdateImage(picturebox_spec[0][i], entry, 0, (int)((ComboBox)item_spec[i]).SelectedValue, 0, false);
                     UpdateImage(picturebox_spec[1][i], entry, (int)((ComboBox)forme_spec[i]).SelectedIndex, (int)((ComboBox)item_spec[i]).SelectedValue, 0, false);
                 }
                 else
                 {
                     UpdateImage(picturebox_spec[0][i], 0, 0, (int)((ComboBox)item_spec[i]).SelectedValue, 0, false);
                     UpdateImage(picturebox_spec[1][i], 0, 0, (int)((ComboBox)item_spec[i]).SelectedValue, 0, false);
                 }
            }
        }

        public class cbItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void UpdateImage(PictureBox bpkx, int species, int form, int item, int gender, bool shiny)
        {
            string file = "";

            Image baseImage;
            if (!bpkx.Enabled)
            {
                bpkx.Image = (Image)null;
                return;
            }

            if (species == 0)
            { baseImage = (Image)Properties.Resources.ResourceManager.GetObject("_0"); }
            else
            {
                file = "_" + species.ToString();
                if (form > 0) // Alt Form Handling
                    file = file + "_" + form.ToString();
                else if ((gender == 1) && (species == 521 || species == 668))   // Unfezant & Pyroar
                    file = file = "_" + species.ToString() + "f";
                { baseImage = (Image)Properties.Resources.ResourceManager.GetObject(file); }
            }
            if (item > 0)
            {
                // Has Item
                Image itemimg = (Image)Properties.Resources.ResourceManager.GetObject("item_" + item.ToString());
                if (itemimg == null) itemimg = Properties.Resources.helditem;
                // Redraw
                baseImage = Util.LayerImage(baseImage, itemimg, 22 + (15 - itemimg.Width) / 2, 15 + (15 - itemimg.Height), 1);
            }
            bpkx.Image = baseImage;
        }
        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setEntry();
        }

        private void B_Dump_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Dump all Mega Evolutions to Text File?"))
                return;
            dumping = true;
            string result = "";

            for (int i = 0; i < 722; i++)
            {
                CB_Species.SelectedValue = i; // Get new Species
                string header = "======" + Environment.NewLine + entry + " " + CB_Species.Text + Environment.NewLine + "======" + Environment.NewLine;
                bool headered = false;
                for (int j = 0; j < 3; j++)
                {
                    if (!checkbox_spec[j].Checked) continue;
                    else if (!headered) { result += header; headered = true; }
                    result += String.Format("Can Mega Evolve into {1} if its held item is {0}." + Environment.NewLine, itemlist[(int)item_spec[j].SelectedValue], forme_spec[j].Text);
                }

                if (headered)
                    result += Environment.NewLine;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Mega Evolutions.txt";
            sfd.Filter = "Text File|*.txt";

            System.Media.SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                File.WriteAllText(path, result, System.Text.Encoding.Unicode);
            }
            dumping = false; 
        }
    }
}