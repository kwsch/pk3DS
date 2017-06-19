using pk3DS.Core;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class PickupEditor7 : Form
    {
        public PickupEditor7(lzGARCFile pickup)
        {
            InitializeComponent();
            g_pickup = pickup;
            var itemlist = Main.Config.getText(TextName.ItemNames);
            itemlist[0] = "";
            items = itemlist.Select((v, i) => $"{v} - {i:000}").ToArray();
            setupFLP();

            byte[] data = pickup.Files[0];
            getList(data);
        }

        private readonly lzGARCFile g_pickup;
        private readonly string[] items;
        
        private const int Columns = 10;
        private void setupFLP()
        {
            dgvCommon.Columns.Clear();
            // Add DataGrid
            var dgv = dgvCommon;
            {
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToResizeRows = false;
                dgv.AllowUserToResizeColumns = false;
                dgv.RowHeadersVisible = false;
                //dgv.ColumnHeadersVisible = false,
                dgv.MultiSelect = false;
                dgv.ShowEditingIcon = false;
                dgv.EditMode = DataGridViewEditMode.EditOnEnter;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }

            int c = 0;
            DataGridViewComboBoxColumn dgvItemVal = new DataGridViewComboBoxColumn
            {
                HeaderText = "Item",
                DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing,
                DisplayIndex = c++,
                Width = 135,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(dgvItemVal);

            for (int i = 0; i < Columns; i++)
            {
                string rate = $"{i*10 + 1}-{(i + 1)*10}";
                DataGridViewColumn dgvIndex = new DataGridViewTextBoxColumn();
                {
                    dgvIndex.HeaderText = rate;
                    dgvIndex.DisplayIndex = c++;
                    dgvIndex.Width = 45;
                    dgvIndex.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    ((DataGridViewTextBoxColumn)dgvIndex).MaxInputLength = 2;
                }
                dgv.Columns.Add(dgvIndex);
            }
            
            var combo = dgv.Columns[0] as DataGridViewComboBoxColumn;
            foreach (var i in items)
                combo.Items.Add(i); // add only the Item Names

            // disable sorting
            dgv.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
            dgv.CancelEdit();
        }

        private void getList(byte[] data)
        {
            // Fill Data
            int rows = BitConverter.ToUInt16(data, 0) - 1; // nice editor gamefreak
            dgvCommon.Rows.Add(rows);
            for (int i = 0; i < rows; i++)
            {
                int offset = 4 + i * (Columns + 2);
                int item = BitConverter.ToUInt16(data, offset);

                var r = dgvCommon.Rows[i];
                r.Cells[0].Value = items[item];
                for (int j = 0; j < Columns; j++)
                {
                    int rate = data[offset + 2 + j];
                    r.Cells[1 + j].Value = rate.ToString();
                }
            }
        }
        private byte[] setList()
        {
            int rows = dgvCommon.RowCount;
            int[][] rates = new int[rows][];
            for (int i = 0; i < rates.Length; i++)
                rates[i] = new int[Columns];
            for (int i = 0; i < Columns; i++)
            {
                // get column sum
                int sum = 0;
                for (int r = 0; r < rows; r++)
                {
                    var cell = dgvCommon.Rows[r].Cells[i+1];
                    int val;
                    if (!int.TryParse(cell.Value.ToString(), out val))
                    {
                        cell.Value = 0.ToString();
                        continue;
                    }
                    if (val > 100 || val < 0)
                    {
                        val = 0;
                        cell.Value = 0.ToString();
                    }
                    rates[r][i] = val;
                    sum += val;
                }
                if (sum == 100) // good
                    continue;

                WinFormsUtil.Alert($"Sum of Column {i+1} needs to equal 100.", $"Got {sum}.");
                return null;
            }

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(rates.Length + 1); // nice editor gamefreak
                for (int i = 0; i < rows; i++)
                {
                    var r = dgvCommon.Rows[i];
                    string item = r.Cells[0].Value.ToString();
                    int itemindex = Array.IndexOf(items, item);
                    bw.Write((ushort)itemindex);

                    foreach (var b in rates[i])
                        bw.Write((byte)b);
                }
                return ms.ToArray();
            }
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            byte[] result = setList();
            if (result == null)
                return;

            g_pickup[0] = result;
            g_pickup.Save();
            Close();
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void B_Randomize_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNoCancel, "Randomize pickup lists?"))
                return;

            int[] validItems = Randomizer.getRandomItemList();

            int ctr = 0;
            Util.Shuffle(validItems);
            for (int r = 0; r < dgvCommon.RowCount; r++)
            {
                dgvCommon.Rows[r].Cells[0].Value = items[validItems[ctr++]];
                if (ctr <= validItems.Length) continue;
                Util.Shuffle(validItems); ctr = 0;
            }
            WinFormsUtil.Alert("Randomized!");
        }

        private void B_AddRow_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Add a row at the bottom?"))
                return;

            int row = dgvCommon.RowCount;
            dgvCommon.Rows.Add();
            dgvCommon.Rows[row].Cells[0].Value = items[0];
            for (int i = 1; i < dgvCommon.ColumnCount; i++)
                dgvCommon.Rows[row].Cells[i].Value = 0.ToString();
        }

        private void B_DeleteRow_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Delete the bottom row?"))
                return;

            dgvCommon.Rows.RemoveAt(dgvCommon.RowCount - 1);
        }
    }
}