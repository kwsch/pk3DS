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
            trFiles = Directory.GetFiles((super) ? "maisontrS" : "maisontrN");
            pkFiles = Directory.GetFiles((super) ? "maisonpkS" : "maisontrS");
            InitializeComponent();
        }
        string[] trFiles;
        string[] pkFiles;
        bool oras = false;
        string[] movelist;
        string[] trainerlist;
        string[] itemlist;
        string[] trnames;
        string[] trsay1;
        string[] trsay2;
        string[] trsay3;

        int entry = -1;
        private void changeTrainer(object sender, EventArgs e)
        {
            setTrainer();
            entry = Array.IndexOf(trainerlist, CB_Trainer.Text);
            getTrainer();
        }
        private void getTrainer()
        {
            if (entry < 1) return;
            byte[] data = File.ReadAllBytes(files[entry]);
        }
        private void setTrainer()
        {

        }
    }
}
