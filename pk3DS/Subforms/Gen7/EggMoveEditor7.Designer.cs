namespace pk3DS
{
    partial class EggMoveEditor7
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
            this.L_STAB = new System.Windows.Forms.Label();
            this.NUD_STAB = new System.Windows.Forms.NumericUpDown();
            this.CHK_STAB = new System.Windows.Forms.CheckBox();
            this.PB_MonSprite = new System.Windows.Forms.PictureBox();
            this.NUD_FormTable = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.B_Goto = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Moves)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_STAB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MonSprite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_FormTable)).BeginInit();
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
            this.B_RandAll.Location = new System.Drawing.Point(300, 377);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(95, 23);
            this.B_RandAll.TabIndex = 4;
            this.B_RandAll.Text = "Randomize!";
            this.B_RandAll.UseVisualStyleBackColor = true;
            this.B_RandAll.Click += new System.EventHandler(this.B_RandAll_Click);
            // 
            // B_Dump
            // 
            this.B_Dump.Location = new System.Drawing.Point(300, 155);
            this.B_Dump.Name = "B_Dump";
            this.B_Dump.Size = new System.Drawing.Size(95, 23);
            this.B_Dump.TabIndex = 5;
            this.B_Dump.Text = "Dump All";
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
            this.groupBox1.Controls.Add(this.L_STAB);
            this.groupBox1.Controls.Add(this.NUD_STAB);
            this.groupBox1.Controls.Add(this.CHK_STAB);
            this.groupBox1.Location = new System.Drawing.Point(300, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(95, 187);
            this.groupBox1.TabIndex = 7;
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
            this.NUD_Moves.Location = new System.Drawing.Point(53, 128);
            this.NUD_Moves.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.NUD_Moves.Minimum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.NUD_Moves.Name = "NUD_Moves";
            this.NUD_Moves.Size = new System.Drawing.Size(36, 20);
            this.NUD_Moves.TabIndex = 9;
            this.NUD_Moves.Value = new decimal(new int[] {
            18,
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
            32,
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
            // 
            // PB_MonSprite
            // 
            this.PB_MonSprite.Location = new System.Drawing.Point(193, 5);
            this.PB_MonSprite.Name = "PB_MonSprite";
            this.PB_MonSprite.Size = new System.Drawing.Size(40, 30);
            this.PB_MonSprite.TabIndex = 91;
            this.PB_MonSprite.TabStop = false;
            // 
            // NUD_FormTable
            // 
            this.NUD_FormTable.Location = new System.Drawing.Point(305, 41);
            this.NUD_FormTable.Name = "NUD_FormTable";
            this.NUD_FormTable.Size = new System.Drawing.Size(47, 20);
            this.NUD_FormTable.TabIndex = 92;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(272, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 93;
            this.label1.Text = "FormTableReference";
            // 
            // B_Goto
            // 
            this.B_Goto.Location = new System.Drawing.Point(356, 41);
            this.B_Goto.Name = "B_Goto";
            this.B_Goto.Size = new System.Drawing.Size(39, 24);
            this.B_Goto.TabIndex = 94;
            this.B_Goto.Text = "goto";
            this.B_Goto.UseVisualStyleBackColor = true;
            this.B_Goto.Click += new System.EventHandler(this.B_Goto_Click);
            // 
            // EggMoveEditor7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 412);
            this.Controls.Add(this.B_Goto);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NUD_FormTable);
            this.Controls.Add(this.PB_MonSprite);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.B_Dump);
            this.Controls.Add(this.B_RandAll);
            this.Controls.Add(this.L_Species);
            this.Controls.Add(this.CB_Species);
            this.Controls.Add(this.dgv);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(322, 450);
            this.Name = "EggMoveEditor7";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Egg Move Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Moves)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_STAB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MonSprite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_FormTable)).EndInit();
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
        private System.Windows.Forms.Label L_Moves;
        private System.Windows.Forms.NumericUpDown NUD_Moves;
        private System.Windows.Forms.CheckBox CHK_Expand;
        private System.Windows.Forms.Label L_STAB;
        private System.Windows.Forms.NumericUpDown NUD_STAB;
        private System.Windows.Forms.CheckBox CHK_STAB;
        private System.Windows.Forms.PictureBox PB_MonSprite;
        private System.Windows.Forms.NumericUpDown NUD_FormTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button B_Goto;
    }
}