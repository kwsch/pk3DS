namespace pk3DS
{
    partial class TutorEditor7
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.B_Randomize = new System.Windows.Forms.Button();
            this.B_Save = new System.Windows.Forms.Button();
            this.B_Cancel = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.CB_LocationBPMove = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvmv = new System.Windows.Forms.DataGridView();
            this.dgvmvBP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvmvMove = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvmvIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmv)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // B_Randomize
            // 
            this.B_Randomize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_Randomize.Location = new System.Drawing.Point(12, 493);
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
            this.B_Save.Location = new System.Drawing.Point(265, 493);
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
            this.B_Cancel.Location = new System.Drawing.Point(191, 493);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(68, 23);
            this.B_Cancel.TabIndex = 5;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.UseVisualStyleBackColor = true;
            this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgvmv);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.CB_LocationBPMove);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(317, 446);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Move Tutors";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // CB_LocationBPMove
            // 
            this.CB_LocationBPMove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_LocationBPMove.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_LocationBPMove.FormattingEnabled = true;
            this.CB_LocationBPMove.Location = new System.Drawing.Point(60, 3);
            this.CB_LocationBPMove.Name = "CB_LocationBPMove";
            this.CB_LocationBPMove.Size = new System.Drawing.Size(254, 21);
            this.CB_LocationBPMove.TabIndex = 11;
            this.CB_LocationBPMove.SelectedIndexChanged += new System.EventHandler(this.ChangeIndexBPMove);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Location:";
            // 
            // dgvmv
            // 
            this.dgvmv.AllowUserToAddRows = false;
            this.dgvmv.AllowUserToDeleteRows = false;
            this.dgvmv.AllowUserToResizeColumns = false;
            this.dgvmv.AllowUserToResizeRows = false;
            this.dgvmv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvmv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvmv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvmvIndex,
            this.dgvmvMove,
            this.dgvmvBP});
            this.dgvmv.Location = new System.Drawing.Point(0, 27);
            this.dgvmv.Name = "dgvmv";
            this.dgvmv.Size = new System.Drawing.Size(317, 419);
            this.dgvmv.TabIndex = 14;
            // 
            // dgvmvBP
            // 
            this.dgvmvBP.HeaderText = "Price";
            this.dgvmvBP.MaxInputLength = 3;
            this.dgvmvBP.Name = "dgvmvBP";
            this.dgvmvBP.Width = 65;
            // 
            // dgvmvMove
            // 
            this.dgvmvMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvmvMove.HeaderText = "Move";
            this.dgvmvMove.Name = "dgvmvMove";
            this.dgvmvMove.Width = 135;
            // 
            // dgvmvIndex
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvmvIndex.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvmvIndex.HeaderText = "Index";
            this.dgvmvIndex.MaxInputLength = 3;
            this.dgvmvIndex.Name = "dgvmvIndex";
            this.dgvmvIndex.ReadOnly = true;
            this.dgvmvIndex.Width = 45;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(325, 472);
            this.tabControl1.TabIndex = 15;
            // 
            // TutorEditor7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 528);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.B_Randomize);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(360, 300);
            this.Name = "TutorEditor7";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tutor Editor";
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmv)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button B_Randomize;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.Button B_Cancel;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvmv;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvmvIndex;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvmvMove;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvmvBP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CB_LocationBPMove;
        private System.Windows.Forms.TabControl tabControl1;
    }
}