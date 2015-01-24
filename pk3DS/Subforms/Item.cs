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
    public partial class Item : Form
    {
        public Item()
        {
            itemlist[0] = "";

            InitializeComponent();
            Setup();
        }
        string[] files = Directory.GetFiles("item");
        string[] itemlist = Main.getText((Main.oras) ? 114 : 96);
        string[] itemflavor = Main.getText((Main.oras) ? 117 : 99);
        
        private void Setup()
        {
            foreach (string s in itemlist) CB_Item.Items.Add(s);
            CB_Item.SelectedIndex = 1;
        }
        int entry = -1;
        private void changeEntry(object sender, EventArgs e)
        {
            setEntry();
            entry = CB_Item.SelectedIndex;
            getEntry();
        }
        private void getEntry()
        {
            if (entry < 1) return;
            byte[] data = File.ReadAllBytes(files[entry]);
            {
                RTB.Text = itemflavor[entry].Replace("\\n", Environment.NewLine);
                MT_Price.Text = (BitConverter.ToUInt16(data, 0) * 10).ToString();
                NUD_UseEffect.Value = data[0x0A];
            }
        }
        private void setEntry()
        {
            if (entry < 1) return;
            byte[] data = File.ReadAllBytes(files[entry]);
            {
                Array.Copy(BitConverter.GetBytes((ushort)(Util.ToUInt32(MT_Price) / 10)), 0, data, 0, 2);
                data[0x0A] = (byte)(int)NUD_UseEffect.Value;
            }
            File.WriteAllBytes(files[entry], data);
        }
        private void formClosing(object sender, FormClosingEventArgs e)
        {
            setEntry();
        }

        private void changePrice(object sender, EventArgs e)
        {
            MT_Sell.Text = ((Math.Min(Util.ToUInt32(MT_Price) / 10, 0x7FFF)) * 10 / 2).ToString();
        }
    }
}
