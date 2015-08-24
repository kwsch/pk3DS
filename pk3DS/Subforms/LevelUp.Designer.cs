namespace pk3DS
{
    partial class LevelUp
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.CB_Species = new System.Windows.Forms.ComboBox();
            this.L_Species = new System.Windows.Forms.Label();
            this.B_RandAll = new System.Windows.Forms.Button();
            this.B_Dump = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.L_Moves = new System.Windows.Forms.Label();
            this.NUD_Moves = new System.Windows.Forms.NumericUpDown();
            this.CHK_Expand = new System.Windows.Forms.CheckBox();
            this.L_Scale2 = new System.Windows.Forms.Label();
            this.NUD_Level = new System.Windows.Forms.NumericUpDown();
            this.L_Scale1 = new System.Windows.Forms.Label();
            this.CHK_Spread = new System.Windows.Forms.CheckBox();
            this.L_STAB = new System.Windows.Forms.Label();
            this.NUD_STAB = new System.Windows.Forms.NumericUpDown();
            this.CHK_STAB = new System.Windows.Forms.CheckBox();
            this.CHK_HMs = new System.Windows.Forms.CheckBox();
            this.PB_MonSprite = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Moves)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_STAB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MonSprite)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(12, 41);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(282, 359);
            this.dgv.TabIndex = 0;
            // 
            // CB_Species
            // 
            this.CB_Species.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Species.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Species.FormattingEnabled = true;
            this.CB_Species.Location = new System.Drawing.Point(66, 12);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(121, 21);
            this.CB_Species.TabIndex = 1;
            this.CB_Species.SelectedIndexChanged += new System.EventHandler(this.changeEntry);
            // 
            // L_Species
            // 
            this.L_Species.AutoSize = true;
            this.L_Species.Location = new System.Drawing.Point(12, 15);
            this.L_Species.Name = "L_Species";
            this.L_Species.Size = new System.Drawing.Size(48, 13);
            this.L_Species.TabIndex = 2;
            this.L_Species.Text = "Species:";
            // 
            // B_RandAll
            // 
            this.B_RandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_RandAll.Location = new System.Drawing.Point(300, 10);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(95, 23);
            this.B_RandAll.TabIndex = 4;
            this.B_RandAll.Text = "Randomize!";
            this.B_RandAll.UseVisualStyleBackColor = true;
            this.B_RandAll.Click += new System.EventHandler(this.B_RandAll_Click);
            // 
            // B_Dump
            // 
            this.B_Dump.Location = new System.Drawing.Point(248, 10);
            this.B_Dump.Name = "B_Dump";
            this.B_Dump.Size = new System.Drawing.Size(46, 23);
            this.B_Dump.TabIndex = 5;
            this.B_Dump.Text = "Dump";
            this.B_Dump.UseVisualStyleBackColor = true;
            this.B_Dump.Click += new System.EventHandler(this.B_Dump_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.L_Moves);
            this.groupBox1.Controls.Add(this.NUD_Moves);
            this.groupBox1.Controls.Add(this.CHK_Expand);
            this.groupBox1.Controls.Add(this.L_Scale2);
            this.groupBox1.Controls.Add(this.NUD_Level);
            this.groupBox1.Controls.Add(this.L_Scale1);
            this.groupBox1.Controls.Add(this.CHK_Spread);
            this.groupBox1.Controls.Add(this.L_STAB);
            this.groupBox1.Controls.Add(this.NUD_STAB);
            this.groupBox1.Controls.Add(this.CHK_STAB);
            this.groupBox1.Controls.Add(this.CHK_HMs);
            this.groupBox1.Location = new System.Drawing.Point(300, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(95, 362);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // L_Moves
            // 
            this.L_Moves.AutoSize = true;
            this.L_Moves.Location = new System.Drawing.Point(10, 130);
            this.L_Moves.Name = "L_Moves";
            this.L_Moves.Size = new System.Drawing.Size(42, 13);
            this.L_Moves.TabIndex = 10;
            this.L_Moves.Text = "Moves:";
            // 
            // NUD_Moves
            // 
            this.NUD_Moves.Enabled = false;
            this.NUD_Moves.Location = new System.Drawing.Point(53, 128);
            this.NUD_Moves.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.NUD_Moves.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.NUD_Moves.Name = "NUD_Moves";
            this.NUD_Moves.Size = new System.Drawing.Size(36, 20);
            this.NUD_Moves.TabIndex = 9;
            this.NUD_Moves.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // CHK_Expand
            // 
            this.CHK_Expand.AutoSize = true;
            this.CHK_Expand.Location = new System.Drawing.Point(5, 110);
            this.CHK_Expand.Name = "CHK_Expand";
            this.CHK_Expand.Size = new System.Drawing.Size(86, 17);
            this.CHK_Expand.TabIndex = 8;
            this.CHK_Expand.Text = "Expand Pool";
            this.CHK_Expand.UseVisualStyleBackColor = true;
            // 
            // L_Scale2
            // 
            this.L_Scale2.AutoSize = true;
            this.L_Scale2.Location = new System.Drawing.Point(2, 242);
            this.L_Scale2.Name = "L_Scale2";
            this.L_Scale2.Size = new System.Drawing.Size(50, 13);
            this.L_Scale2.TabIndex = 7;
            this.L_Scale2.Text = "@ Level:";
            // 
            // NUD_Level
            // 
            this.NUD_Level.Location = new System.Drawing.Point(54, 240);
            this.NUD_Level.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.NUD_Level.Name = "NUD_Level";
            this.NUD_Level.Size = new System.Drawing.Size(36, 20);
            this.NUD_Level.TabIndex = 6;
            this.NUD_Level.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // L_Scale1
            // 
            this.L_Scale1.AutoSize = true;
            this.L_Scale1.Location = new System.Drawing.Point(5, 224);
            this.L_Scale1.Name = "L_Scale1";
            this.L_Scale1.Size = new System.Drawing.Size(73, 13);
            this.L_Scale1.TabIndex = 5;
            this.L_Scale1.Text = "Stop Learning";
            // 
            // CHK_Spread
            // 
            this.CHK_Spread.AutoSize = true;
            this.CHK_Spread.Location = new System.Drawing.Point(5, 204);
            this.CHK_Spread.Name = "CHK_Spread";
            this.CHK_Spread.Size = new System.Drawing.Size(95, 17);
            this.CHK_Spread.TabIndex = 4;
            this.CHK_Spread.Text = "Spread Evenly";
            this.CHK_Spread.UseVisualStyleBackColor = true;
            // 
            // L_STAB
            // 
            this.L_STAB.AutoSize = true;
            this.L_STAB.Location = new System.Drawing.Point(6, 74);
            this.L_STAB.Name = "L_STAB";
            this.L_STAB.Size = new System.Drawing.Size(46, 13);
            this.L_STAB.TabIndex = 3;
            this.L_STAB.Text = "% STAB";
            // 
            // NUD_STAB
            // 
            this.NUD_STAB.Location = new System.Drawing.Point(53, 72);
            this.NUD_STAB.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NUD_STAB.Name = "NUD_STAB";
            this.NUD_STAB.Size = new System.Drawing.Size(36, 20);
            this.NUD_STAB.TabIndex = 2;
            this.NUD_STAB.Value = new decimal(new int[] {
            52,
            0,
            0,
            0});
            // 
            // CHK_STAB
            // 
            this.CHK_STAB.AutoSize = true;
            this.CHK_STAB.Location = new System.Drawing.Point(5, 54);
            this.CHK_STAB.Name = "CHK_STAB";
            this.CHK_STAB.Size = new System.Drawing.Size(87, 17);
            this.CHK_STAB.TabIndex = 1;
            this.CHK_STAB.Text = "Bias by Type";
            this.CHK_STAB.UseVisualStyleBackColor = true;
            this.CHK_STAB.CheckedChanged += new System.EventHandler(this.CHK_TypeBias_CheckedChanged);
            // 
            // CHK_HMs
            // 
            this.CHK_HMs.AutoSize = true;
            this.CHK_HMs.Location = new System.Drawing.Point(5, 19);
            this.CHK_HMs.Name = "CHK_HMs";
            this.CHK_HMs.Size = new System.Drawing.Size(76, 17);
            this.CHK_HMs.TabIndex = 0;
            this.CHK_HMs.Text = "Allow HMs";
            this.CHK_HMs.UseVisualStyleBackColor = true;
            // 
            // PB_MonSprite
            // 
            this.PB_MonSprite.Location = new System.Drawing.Point(193, 5);
            this.PB_MonSprite.Name = "PB_MonSprite";
            this.PB_MonSprite.Size = new System.Drawing.Size(40, 30);
            this.PB_MonSprite.TabIndex = 90;
            this.PB_MonSprite.TabStop = false;
            // 
            // LevelUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 412);
            this.Controls.Add(this.PB_MonSprite);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.B_Dump);
            this.Controls.Add(this.B_RandAll);
            this.Controls.Add(this.L_Species);
            this.Controls.Add(this.CB_Species);
            this.Controls.Add(this.dgv);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(322, 450);
            this.Name = "LevelUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Level Up Move Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Moves)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_STAB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MonSprite)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.Label L_Species;
        private System.Windows.Forms.Button B_RandAll;
        private System.Windows.Forms.Button B_Dump;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CHK_STAB;
        private System.Windows.Forms.CheckBox CHK_HMs;
        private System.Windows.Forms.Label L_STAB;
        private System.Windows.Forms.NumericUpDown NUD_STAB;
        private System.Windows.Forms.Label L_Scale2;
        private System.Windows.Forms.NumericUpDown NUD_Level;
        private System.Windows.Forms.Label L_Scale1;
        private System.Windows.Forms.CheckBox CHK_Spread;
        private System.Windows.Forms.Label L_Moves;
        private System.Windows.Forms.NumericUpDown NUD_Moves;
        private System.Windows.Forms.CheckBox CHK_Expand;
        private System.Windows.Forms.PictureBox PB_MonSprite;
    }
}