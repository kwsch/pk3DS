using System;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            RTB.Text = Properties.Resources.changelog;
        }
        private void B_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
