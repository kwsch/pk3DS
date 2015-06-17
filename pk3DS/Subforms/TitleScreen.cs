using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class TitleScreen : Form
    {
        public TitleScreen()
        {

            InitializeComponent();
            Setup();
        }
        private string[] files = Directory.GetFiles("titlescreen");

        private void Setup()
        {
        }
    }
}