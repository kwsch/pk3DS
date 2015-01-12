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
    public partial class Moves : Form
    {
        public Moves(bool rom_oras)
        {
            Util.unpackMini(Directory.GetFiles("move")[0], "WD");
            files = Directory.GetFiles("move");
            InitializeComponent();
        }
        string[] files;
        string[] StatCategories = new string[] { "None", "Attack", "Defense", "Special Attack", "Special Defense", "Speed", "Accuracy", "Evasion", "All", };
        string[] TypeCategories = new string[] { "Status", "Physical", "Special", };
        string[] TargetingTypes = new string[] { "Single Adjacent Ally/Foe", 
                                                 "Any Ally", "Any Adjacent Ally", "Single Adjacent Foe", "Everyone but User", "All Foes", 
                                                 "All Allies", "Self", "All Pokemon on Field", "Single Adjacent Foe (2)", "Entire Field", 
                                                 "Opponent's Field", "User's Field", "Self", "14", "15",
                                               };

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            // setEntry();
            Util.packMini("move", "WD", "0", ".bin", "move");
        }
    }
}
