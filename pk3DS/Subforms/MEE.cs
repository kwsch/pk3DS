using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class MEE : Form
    {
        private readonly string[] files = Directory.GetFiles("megaevo");
        private readonly string[] forms = Main.getText(Main.oras ? 5 : 5);
        private readonly string[] types = Main.getText(Main.oras ? 18 : 17);
        private readonly string[] specieslist = Main.getText(Main.oras ? 98 : 80);
        private readonly string[] itemlist = Main.getText(Main.oras ? 114 : 96);
        private readonly byte[] personalData = File.ReadAllBytes(Directory.GetFiles("personal").Last());
        private readonly GroupBox[] groupbox_spec;
        private readonly ComboBox[] forme_spec;
        private readonly ComboBox[] item_spec;
        private readonly CheckBox[] checkbox_spec;
        private readonly PictureBox[][] picturebox_spec;
        private bool loaded;
        private readonly string[][] AltForms;
        private int entry = -1;
        private bool dumping;
        private MegaEvolutions me;

        public MEE()            //All the initial settings
        {
            InitializeComponent();
            CB_Species.DisplayMember = "Text";
            CB_Species.ValueMember = "Value";
            #region Intializations

            Array.Resize(ref specieslist, 722); specieslist[0] = itemlist[0] = "";
            specieslist[32] += "♂"; specieslist[29] += "♀";
            AltForms = Personal.getFormList(personalData, Main.oras, specieslist, forms, types, itemlist);

            groupbox_spec = new[] { GB_MEvo1, GB_MEvo2, GB_MEvo3 };
            item_spec = new[] { CB_Item1, CB_Item2, CB_Item3 };
            forme_spec = new[] { CB_Forme1, CB_Forme2, CB_Forme3 };
            checkbox_spec = new[] { CHK_MEvo1, CHK_MEvo2, CHK_MEvo3 };
            picturebox_spec = new[] { new[] { PB_S1, PB_S2, PB_S3 }, new[] { PB_M1, PB_M2, PB_M3 } };
            #endregion
            Setup();
            CB_Species.SelectedIndex = 0;
        }
        private void Setup()
        {
            List<string> temp_list = new List<string>(specieslist);
            temp_list.Sort();

            CB_Species.DataSource = temp_list.Select(mon => new Util.cbItem { Text = mon, Value = Array.IndexOf(specieslist, mon) }).ToList();

            List<string> items = new List<string>(itemlist);
            List<string> sorted_items = new List<string>(itemlist);
            List<Util.cbItem>[] item_lists = new List<Util.cbItem>[item_spec.Length];
            for (int i = 0; i < item_lists.Length; i++)
                item_lists[i] = new List<Util.cbItem>();

            sorted_items.Sort();
            for (int i = 0; i < items.Count; i++)
            {
                int index = items.IndexOf(sorted_items[i]);
                {
                    Util.cbItem ncbi = new Util.cbItem();
                    if (sorted_items[i] == "???") continue; // Don't allow stubbed items.
                    ncbi.Text = sorted_items[i] + " - " + index.ToString("000");
                    ncbi.Value = index;
                    foreach (List<Util.cbItem> l in item_lists)
                        l.Add(ncbi);
                }
                items[index] = "";
            }
            for (int i = 0; i < item_spec.Length; i++)
            {
                item_spec[i].ValueMember = "Value";
                item_spec[i].DisplayMember = "Text";
                item_spec[i].DataSource = item_lists[i];
                item_spec[i].SelectedValue = 0;
            }

            loaded = true;
        }
        private void CHK_Changed(object sender, EventArgs e)
        {
            for (int i = 0; i < groupbox_spec.Length; i++)
            {
                groupbox_spec[i].Enabled = checkbox_spec[i].Checked;
                Update_PBs(i);
            }
        }

        private void changeIndex(object sender, EventArgs e)
        {
            setEntry();
            entry = (int)CB_Species.SelectedValue;
            getEntry();
        }
        private void getEntry()
        {
            if (!loaded) return;
            if (Main.oras && entry == 384 && !dumping) // Current Mon is Rayquaza
                Util.Alert("Rayquaza is special and uses a different activator for its evolution. If it knows Dragon Ascent, it can Mega Evolve", "Don't edit its evolution table if you want to keep this functionality.");

            byte[] data = File.ReadAllBytes(files[entry]);

            foreach (ComboBox CB in forme_spec)
                Personal.setForms(entry, CB, AltForms);

            me = new MegaEvolutions(data);
            for (int i = 0; i < 3; i++)
            {
                checkbox_spec[i].Checked = me.Method[i] == 1;
                item_spec[i].SelectedValue = (int)me.Argument[i];
                forme_spec[i].SelectedIndex = me.Form[i];
            }
        }
        private void setEntry()
        {
            if (entry < 1 || entry == 384) return; // Don't edit invalid / Rayquaza.
            for (int i = 0; i < 3; i++)
            {
                if (me.Method[i] > 1) 
                    return; // Shouldn't hit this.
                me.Method[i] = (ushort)(checkbox_spec[i].Checked ? 1 : 0);
                me.Argument[i] = (ushort)Util.getIndex(item_spec[i]);
                me.Form[i] = (ushort)forme_spec[i].SelectedIndex;
            }
            File.WriteAllBytes(files[entry], me.Write());
        }

        private void Update_PBs(object sender, EventArgs e)
        {
            if (!loaded) return;
            for (int i = 0; i < checkbox_spec.Length; i++)
            {
                CheckBox CB = checkbox_spec[i];
                if (CB.Checked)
                {
                    UpdateImage(picturebox_spec[0][i], entry, 0, Util.getIndex(item_spec[i]), 0);
                    UpdateImage(picturebox_spec[1][i], entry, forme_spec[i].SelectedIndex, Util.getIndex(item_spec[i]), 0);
                }
                else
                {
                    UpdateImage(picturebox_spec[0][i], 0, 0, Util.getIndex(item_spec[i]), 0);
                    UpdateImage(picturebox_spec[1][i], 0, 0, Util.getIndex(item_spec[i]), 0);
                }
            }
        }

        private void Update_PBs(int i)
        {
            if (!loaded) return;
            CheckBox CB = checkbox_spec[i];
            if (CB.Checked)
            {
                UpdateImage(picturebox_spec[0][i], entry, 0, Util.getIndex(item_spec[i]), 0);
                UpdateImage(picturebox_spec[1][i], entry, forme_spec[i].SelectedIndex, Util.getIndex(item_spec[i]), 0);
            }
            else
            {
                UpdateImage(picturebox_spec[0][i], 0, 0, Util.getIndex(item_spec[i]), 0);
                UpdateImage(picturebox_spec[1][i], 0, 0, Util.getIndex(item_spec[i]), 0);
            }
        }
        
        private void UpdateImage(PictureBox pb, int species, int form, int item, int gender)
        {
            if (!pb.Enabled)
            {
                pb.Image = null;
                return;
            }
            pb.Image = Util.getSprite(species, form, gender, item);
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
                    if (!headered) { result += header; headered = true; }
                    result += string.Format("Can Mega Evolve into {1} if its held item is {0}." + Environment.NewLine, itemlist[(int)item_spec[j].SelectedValue], forme_spec[j].Text);
                }

                if (headered)
                    result += Environment.NewLine;
            }
            SaveFileDialog sfd = new SaveFileDialog {FileName = "Mega Evolutions.txt", Filter = "Text File|*.txt"};

            SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
                File.WriteAllText(sfd.FileName, result, Encoding.Unicode);

            dumping = false;
        }
    }
}