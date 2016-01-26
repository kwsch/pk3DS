using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    /* GARC File Shuffler
     * Shuffles the FATB table references around (Start/End/Length)
     * Only shuffles nonfoldered files around.
     * Backs up the original file incase the user shuffles a file with ill-effect.
     */
    public partial class Shuffler : Form
    {
        public Shuffler()
        {
            InitializeComponent();
            CB_a.SelectedIndex = CB_b.SelectedIndex = CB_c.SelectedIndex = 0;

            // Ban Models, Encounters, TitleScreen etc
            banlist = Main.oras
                ? new[] { "a005", "a008", "a013", "a039", "a040", "a071", "a072", "a073", "a074", "a075", "a076", "a078", "a079", "a080", "a081", "a082", "a083", "a084", "a085", "a086", 
                    "a100", "a152", 
                    "a195" }
                : new[] { "a005", "a007", "a012", "a041", "a042", "a072", "a073", "a074", "a075", "a076", "a078", "a079", "a080", "a081", "a082", "a083", "a084", "a085", "a086", "a087", 
                    "a101", "a165", 
                    "a218" };
        }
        private string garc;
        private readonly string[] banlist;

        private void updateLabel(object sender, EventArgs e)
        {
            garc = Path.Combine(Main.RomFSPath, "a", 
                CB_a.SelectedIndex.ToString(), CB_b.SelectedIndex.ToString(), CB_c.SelectedIndex.ToString());

            if (File.Exists(garc))
            {
                L_File.Text = $"File: a\\{CB_a.SelectedIndex}\\{CB_b.SelectedIndex}\\{CB_c.SelectedIndex}";
                B_Shuffle.Enabled = true;
            }
            else
            {
                L_File.Text = "File does not exist!";
                B_Shuffle.Enabled = false;
                garc = null;
            }
        }

        private void B_Shuffle_Click(object sender, EventArgs e)
        {
            if (garc == null)
                return;

            string garcID = L_File.Text.Split(':')[1].Replace("\\", "").Replace(" ","");
            if (banlist.Contains(garcID))
            { Util.Alert("GARC is prevented from being shuffled."); return; }

            var g = CTR.GARC.unpackGARC(garc);
            
            // Build a list of all the files we can relocate.
            int[] randFiles = new int[g.fatb.FileCount];
            int ctr = 0;
            for (int i = 0; i < randFiles.Length; i++)
                if (!g.fatb.Entries[i].IsFolder)
                    randFiles[ctr++] = i;

            Array.Resize(ref randFiles, ctr);

            if (ctr == 0) 
            { Util.Alert("No files to shuffle...?"); return; }
            
            // Create backup
            string dest = "backup" + Path.DirectorySeparatorChar + $"PreShuffle {garcID}";
            if (!File.Exists(dest))
                File.Copy(garc, dest);
            
            var g2 = CTR.GARC.unpackGARC(garc);
            int[] newFileOffset = (int[])randFiles.Clone();
            Util.Shuffle(newFileOffset);

            for (int i = 0; i < randFiles.Length; i++)
                g.fatb.Entries[randFiles[i]] = g2.fatb.Entries[newFileOffset[i]];
            
            #region Re-write GARC Header information!
            using (var newGARC = File.OpenWrite(garc))
            using (BinaryWriter gw = new BinaryWriter(newGARC))
            {
                gw.Seek(7*4, SeekOrigin.Begin); // Skip GARC Header
                // Write GARC
                // gw.Write((uint)0x47415243); // GARC
                // gw.Write((uint)0x0000001C); // Header Length
                // gw.Write((ushort)0xFEFF);   // Endianness BOM
                // gw.Write((ushort)0x0400);   // Const (4)
                // gw.Write((uint)0x00000004); // Section Count (4)
                // gw.Write((uint)0x00000000); // Data Offset (temp)
                // gw.Write((uint)0x00000000); // File Length (temp)
                // gw.Write((uint)0x00000000); // Largest File Size (temp)

                // Write FATO
                gw.Write((uint)0x4641544F);  // FATO
                gw.Write(g.fato.HeaderSize); // Header Size 
                gw.Write(g.fato.EntryCount); // Entry Count
                gw.Write(g.fato.Padding);    // Padding
                for (int i = 0; i < g.fato.Entries.Length; i++)
                    gw.Write((uint)g.fato.Entries[i].Offset);

                // Write FATB
                gw.Write((uint)0x46415442);  // FATB
                gw.Write(g.fatb.HeaderSize); // Header Size
                gw.Write(g.fatb.FileCount);  // File Count
                foreach (var fe in g.fatb.Entries)
                {
                    gw.Write(fe.Vector);
                    foreach (var s in fe.SubEntries.Where(s => s.Exists))
                    { 
                        gw.Write((uint)s.Start); 
                        gw.Write((uint)s.End); 
                        gw.Write((uint)s.Length); 
                    }
                }
            }
            #endregion

            Util.Alert("GARC Shuffled!");
        }
    }
}
