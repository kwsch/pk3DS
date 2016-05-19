using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class xytext : Form
    {
        public xytext(string[] infiles)
        {
            InitializeComponent();
            files = infiles;
            for (int i = 0; i < files.Length; i++)
                CB_Entry.Items.Add(i.ToString());
            CB_Entry.SelectedIndex = 0;
        }
        private readonly string[] files;
        private int entry = -1;
        // IO
        private void B_Export_Click(object sender, EventArgs e)
        {
            if (files.Length <= 0) return;
            SaveFileDialog Dump = new SaveFileDialog {Filter = "Text File|*.txt"};
            DialogResult sdr = Dump.ShowDialog();
            if (sdr != DialogResult.OK) return;
            bool newline = Util.Prompt(MessageBoxButtons.YesNo, "Remove newline formatting codes? (\\n,\\r,\\c)", "Removing newline formatting will make it more readable but will prevent any importing of that dump.") == DialogResult.Yes;
            string path = Dump.FileName;
            exportTextFile(path, newline);
        }
        private void B_Import_Click(object sender, EventArgs e)
        {
            if (files.Length <= 0) return;
            OpenFileDialog Dump = new OpenFileDialog { Filter = "Text File|*.txt" };
            DialogResult odr = Dump.ShowDialog();
            if (odr != DialogResult.OK) return;
            string path = Dump.FileName;
            
            if (!importTextFile(path)) return;

            // Reload the form with the new data.
            changeEntry(null, null);
            Util.Alert("Imported Text from Input Path:", path);
        }
        private void exportTextFile(string fileName, bool newline)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(new byte[] {0xFF, 0xFE}, 0, 2); // Write Unicode BOM
                using (TextWriter tw = new StreamWriter(ms, new UnicodeEncoding()))
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        // Get Strings for the File
                        string[] data = TextFile.getStrings(files[i]);
                        // Append the File Header
                        tw.WriteLine("~~~~~~~~~~~~~~~");
                        tw.WriteLine("Text File : " + i);
                        tw.WriteLine("~~~~~~~~~~~~~~~");
                        // Write the String to the File
                        if (data == null) continue;
                        foreach (string line in data)
                        {
                            tw.WriteLine(newline
                                ? line.Replace("\\n\\n", " ")
                                    .Replace("\\n", " ")
                                    .Replace("\\c", "")
                                    .Replace("\\r", "")
                                    .Replace("\\\\", "\\")
                                    .Replace("\\[", "[")
                                : line);
                        }
                    }
                }
                File.WriteAllBytes(fileName, ms.ToArray());
            }
        }
        private bool importTextFile(string fileName)
        {
            string[] fileText = File.ReadAllLines(fileName, Encoding.Unicode);
            string[][] textLines = new string[files.Length][];
            int ctr = 0;
            bool newlineFormatting = false;
            // Loop through all files
            for (int i = 0; i < fileText.Length; i++)
            {
                string line = fileText[i];
                if (line != "~~~~~~~~~~~~~~~")
                    continue;
                string[] brokenLine = fileText[i++ + 1].Split(new[] { " : " }, StringSplitOptions.None);
                if (brokenLine.Length != 2)
                { Util.Error($"Invalid Line @ {i}, expected Text File : {ctr}"); return false; }
                int file = Util.ToInt32(brokenLine[1]);
                if (file != ctr)
                { Util.Error($"Invalid Line @ {i}, expected Text File : {ctr}"); return false; }
                i += 2; // Skip over the other header line
                List<string> Lines = new List<string>();
                while (i < fileText.Length && fileText[i] != "~~~~~~~~~~~~~~~")
                {
                    Lines.Add(fileText[i]);
                    newlineFormatting |= fileText[i].Contains("\\n"); // Check if any line wasn't stripped of ingame formatting codes for human readability.
                    i++;
                }
                i--;
                textLines[ctr++] = Lines.ToArray();
            }

            // Error Check
            if (ctr != files.Length)
            { Util.Error("The amount of Text Files in the input file does not match the required for the text file.",
                $"Received: {ctr}, Expected: {files.Length}"); return false; }
            if (!newlineFormatting)
            { Util.Error("The input Text Files do not have the ingame newline formatting codes (\\n,\\r,\\c).",
                      "When exporting text, do not remove newline formatting."); return false; }

            // All Text Lines received. Store all back.
            for (int i = 0; i < files.Length; i++)
                try { File.WriteAllBytes(files[i], TextFile.getBytes(textLines[i])); }
                catch (Exception e) { Util.Error($"The input Text File (# {i}) failed to convert:", e.ToString()); return false; }
            return true;
        }
        private void changeEntry(object sender, EventArgs e)
        {
            // Save All the old text
            if (entry > -1 && sender != null)
            {
                try
                { 
                    byte[] bin = TextFile.getBytes(getCurrentDGLines());
                    File.WriteAllBytes(files[entry], bin);
                }
                catch (Exception ex) { Util.Error(ex.ToString()); }
            }

            // Reset
            entry = CB_Entry.SelectedIndex;
            string file = files[entry];
            string[] data = TextFile.getStrings(file);
            setStringsDataGridView(data);
        }
        // Main Handling
        private void setStringsDataGridView(string[] textArray)
        {
            // Clear the datagrid row content to remove all text lines.
            dgv.Rows.Clear();
            // Clear the header columns, these are repopulated every time.
            dgv.Columns.Clear();
            if (textArray == null || textArray.Length == 0)
                return;
            // Reset settings and columns.
            dgv.AllowUserToResizeColumns = false;
            DataGridViewColumn dgvLine = new DataGridViewTextBoxColumn
            {
                HeaderText = "Line",
                DisplayIndex = 0,
                Width = 32,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dgvLine.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            DataGridViewTextBoxColumn dgvText = new DataGridViewTextBoxColumn
            {
                HeaderText = "Text",
                DisplayIndex = 1,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            
            dgv.Columns.Add(dgvLine);
            dgv.Columns.Add(dgvText);
            dgv.Rows.Add(textArray.Length);

            // Add the text lines into their cells.
            for (int i = 0; i < textArray.Length; i++)
            {
                dgv.Rows[i].Cells[0].Value = i;
                dgv.Rows[i].Cells[1].Value = textArray[i];
            }
        }

        private string[] getCurrentDGLines()
        {
            // Get Line Count
            string[] lines = new string[dgv.RowCount];
            for (int i = 0; i < dgv.RowCount; i++)
                lines[i] = (string)dgv.Rows[i].Cells[1].Value;
            return lines;
        }
        // Meta Usage
        private void B_AddLine_Click(object sender, EventArgs e)
        {
            int currentRow = 0;
            try { currentRow = dgv.CurrentRow.Index; }
            catch { dgv.Rows.Add(); }
            if (dgv.Rows.Count == 1) { }
            else if (currentRow < dgv.Rows.Count - 1 || currentRow == 0)
            {
                if (ModifierKeys != Keys.Control && currentRow != 0)
                { 
                    if (Util.Prompt(MessageBoxButtons.YesNo,
                    "Inserting in between rows will shift all subsequent lines.", "Continue?") != DialogResult.Yes) 
                        return; 
                }
                // Insert new Row after current row.
                dgv.Rows.Insert(currentRow + 1);
            }

            for (int i = 0; i < dgv.Rows.Count; i++)
                dgv.Rows[i].Cells[0].Value = i.ToString();
        }
        private void B_RemoveLine_Click(object sender, EventArgs e)
        {
            int currentRow = dgv.CurrentRow.Index;
            if (currentRow < dgv.Rows.Count - 1)
            {
                if (ModifierKeys != Keys.Control && DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, 
                    "Deleting a row above other lines will shift all subsequent lines.", "Continue?"))
                    return;
            }
            dgv.Rows.RemoveAt(currentRow);

            // Resequence the Index Value column
            for (int i = 0; i < dgv.Rows.Count; i++)
                dgv.Rows[i].Cells[0].Value = i.ToString();
        }
        private void xytext_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save All the old text
            if (entry > -1) File.WriteAllBytes(files[entry], TextFile.getBytes(getCurrentDGLines()));
        }
    }
}