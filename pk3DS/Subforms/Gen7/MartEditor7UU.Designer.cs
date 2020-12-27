namespace pk3DS
{
    partial class MartEditor7UU
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.CB_Location = new System.Windows.Forms.ComboBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.dgvIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvItem = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.L_Mart = new System.Windows.Forms.Label();
            this.B_Randomize = new System.Windows.Forms.Button();
            this.B_Save = new System.Windows.Forms.Button();
            this.B_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CB_LocationBPItem = new System.Windows.Forms.ComboBox();
            this.dgvbp = new System.Windows.Forms.DataGridView();
            this.dgvIndexBP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvItemBP = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvPriceBP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.CHK_XItems = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvbp)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CB_Location
            // 
            this.CB_Location.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Location.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Location.FormattingEnabled = true;
            this.CB_Location.Location = new System.Drawing.Point(60, 3);
            this.CB_Location.Name = "CB_Location";
            this.CB_Location.Size = new System.Drawing.Size(254, 21);
            this.CB_Location.TabIndex = 0;
            this.CB_Location.SelectedIndexChanged += new System.EventHandler(this.ChangeIndexItem);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvIndex,
            this.dgvItem});
            this.dgv.Location = new System.Drawing.Point(0, 27);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(317, 265);
            this.dgv.TabIndex = 1;
            // 
            // dgvIndex
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvIndex.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvIndex.HeaderText = "Index";
            this.dgvIndex.Name = "dgvIndex";
            this.dgvIndex.ReadOnly = true;
            this.dgvIndex.Width = 45;
            // 
            // dgvItem
            // 
            this.dgvItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvItem.HeaderText = "Item";
            this.dgvItem.Name = "dgvItem";
            this.dgvItem.Width = 135;
            // 
            // L_Mart
            // 
            this.L_Mart.AutoSize = true;
            this.L_Mart.Location = new System.Drawing.Point(6, 6);
            this.L_Mart.Name = "L_Mart";
            this.L_Mart.Size = new System.Drawing.Size(51, 13);
            this.L_Mart.TabIndex = 2;
            this.L_Mart.Text = "Location:";
            // 
            // B_Randomize
            // 
            this.B_Randomize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_Randomize.Location = new System.Drawing.Point(12, 336);
            this.B_Randomize.Name = "B_Randomize";
            this.B_Randomize.Size = new System.Drawing.Size(87, 23);
            this.B_Randomize.TabIndex = 3;
            this.B_Randomize.Text = "Randomize";
            this.B_Randomize.UseVisualStyleBackColor = true;
            this.B_Randomize.Click += new System.EventHandler(this.B_Randomize_Click);
            // 
            // B_Save
            // 
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(265, 336);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(68, 23);
            this.B_Save.TabIndex = 4;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Cancel.Location = new System.Drawing.Point(191, 336);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(68, 23);
            this.B_Cancel.TabIndex = 5;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.UseVisualStyleBackColor = true;
            this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Location:";
            // 
            // CB_LocationBPItem
            // 
            this.CB_LocationBPItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_LocationBPItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_LocationBPItem.FormattingEnabled = true;
            this.CB_LocationBPItem.Location = new System.Drawing.Point(60, 3);
            this.CB_LocationBPItem.Name = "CB_LocationBPItem";
            this.CB_LocationBPItem.Size = new System.Drawing.Size(254, 21);
            this.CB_LocationBPItem.TabIndex = 6;
            this.CB_LocationBPItem.SelectedIndexChanged += new System.EventHandler(this.ChangeIndexBPItem);
            // 
            // dgvbp
            // 
            this.dgvbp.AllowUserToAddRows = false;
            this.dgvbp.AllowUserToDeleteRows = false;
            this.dgvbp.AllowUserToResizeColumns = false;
            this.dgvbp.AllowUserToResizeRows = false;
            this.dgvbp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvbp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvbp.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvIndexBP,
            this.dgvItemBP,
            this.dgvPriceBP});
            this.dgvbp.Location = new System.Drawing.Point(0, 27);
            this.dgvbp.Name = "dgvbp";
            this.dgvbp.Size = new System.Drawing.Size(317, 241);
            this.dgvbp.TabIndex = 10;
            // 
            // dgvIndexBP
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvIndexBP.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvIndexBP.HeaderText = "Index";
            this.dgvIndexBP.MaxInputLength = 3;
            this.dgvIndexBP.Name = "dgvIndexBP";
            this.dgvIndexBP.ReadOnly = true;
            this.dgvIndexBP.Width = 45;
            // 
            // dgvItemBP
            // 
            this.dgvItemBP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvItemBP.HeaderText = "Item";
            this.dgvItemBP.Name = "dgvItemBP";
            this.dgvItemBP.Width = 135;
            // 
            // dgvPriceBP
            // 
            this.dgvPriceBP.HeaderText = "Price";
            this.dgvPriceBP.MaxInputLength = 3;
            this.dgvPriceBP.Name = "dgvPriceBP";
            this.dgvPriceBP.Width = 65;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(325, 318);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.L_Mart);
            this.tabPage1.Controls.Add(this.CB_Location);
            this.tabPage1.Controls.Add(this.dgv);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(317, 292);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Poké Mart";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.CB_LocationBPItem);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.dgvbp);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(317, 268);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "BP Items";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // CHK_XItems
            // 
            this.CHK_XItems.AutoSize = true;
            this.CHK_XItems.Checked = true;
            this.CHK_XItems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_XItems.Location = new System.Drawing.Point(13, 364);
            this.CHK_XItems.Name = "CHK_XItems";
            this.CHK_XItems.Size = new System.Drawing.Size(200, 17);
            this.CHK_XItems.TabIndex = 304;
            this.CHK_XItems.Text = "Don\'t Randomize X Items (Speedrun)";
            this.CHK_XItems.UseVisualStyleBackColor = true;
            // 
            // MartEditor7UU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 387);
            this.Controls.Add(this.CHK_XItems);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.B_Randomize);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(360, 300);
            this.Name = "MartEditor7UU";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mart Editor";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvbp)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_Location;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label L_Mart;
        private System.Windows.Forms.Button B_Randomize;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.Button B_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CB_LocationBPItem;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvIndex;
        private System.Windows.Forms.DataGridView dgvbp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvIndexBP;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvItemBP;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPriceBP;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox CHK_XItems;
    }
}