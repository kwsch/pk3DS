namespace pk3DS
{
    partial class ShinyRate
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
            this.label1 = new System.Windows.Forms.Label();
            this.NUD_Rerolls = new System.Windows.Forms.NumericUpDown();
            this.L_Overall = new System.Windows.Forms.Label();
            this.B_Cancel = new System.Windows.Forms.Button();
            this.B_Save = new System.Windows.Forms.Button();
            this.L_Rerolls = new System.Windows.Forms.Label();
            this.B_RestoreOriginal = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Rerolls)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 65);
            this.label1.TabIndex = 0;
            this.label1.Text = "Note: \r\nChanging the rate only changes the amount of PID rerolls\r\nChanging the ra" +
    "te does not alter the \"IsShiny\" determination.\r\n\r\nThink of it like a frozen Shin" +
    "y Charm.";
            // 
            // NUD_Rerolls
            // 
            this.NUD_Rerolls.Location = new System.Drawing.Point(67, 95);
            this.NUD_Rerolls.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NUD_Rerolls.Name = "NUD_Rerolls";
            this.NUD_Rerolls.Size = new System.Drawing.Size(65, 20);
            this.NUD_Rerolls.TabIndex = 1;
            this.NUD_Rerolls.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.NUD_Rerolls.ValueChanged += new System.EventHandler(this.changeRerolls);
            // 
            // L_Overall
            // 
            this.L_Overall.AutoSize = true;
            this.L_Overall.Location = new System.Drawing.Point(138, 97);
            this.L_Overall.Name = "L_Overall";
            this.L_Overall.Size = new System.Drawing.Size(35, 13);
            this.L_Overall.TabIndex = 2;
            this.L_Overall.Text = "label2";
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Cancel.Location = new System.Drawing.Point(216, 136);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(75, 23);
            this.B_Cancel.TabIndex = 4;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.UseVisualStyleBackColor = true;
            this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
            // 
            // B_Save
            // 
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(297, 136);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(75, 23);
            this.B_Save.TabIndex = 3;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // L_Rerolls
            // 
            this.L_Rerolls.AutoSize = true;
            this.L_Rerolls.Location = new System.Drawing.Point(19, 97);
            this.L_Rerolls.Name = "L_Rerolls";
            this.L_Rerolls.Size = new System.Drawing.Size(42, 13);
            this.L_Rerolls.TabIndex = 5;
            this.L_Rerolls.Text = "Rerolls:";
            // 
            // B_RestoreOriginal
            // 
            this.B_RestoreOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_RestoreOriginal.Location = new System.Drawing.Point(15, 136);
            this.B_RestoreOriginal.Name = "B_RestoreOriginal";
            this.B_RestoreOriginal.Size = new System.Drawing.Size(104, 23);
            this.B_RestoreOriginal.TabIndex = 6;
            this.B_RestoreOriginal.Text = "Restore Original";
            this.B_RestoreOriginal.UseVisualStyleBackColor = true;
            this.B_RestoreOriginal.Click += new System.EventHandler(this.B_RestoreOriginal_Click);
            // 
            // ShinyRate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 171);
            this.Controls.Add(this.B_RestoreOriginal);
            this.Controls.Add(this.L_Rerolls);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.L_Overall);
            this.Controls.Add(this.NUD_Rerolls);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 210);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 150);
            this.Name = "ShinyRate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shiny Rate Editor";
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Rerolls)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NUD_Rerolls;
        private System.Windows.Forms.Label L_Overall;
        private System.Windows.Forms.Button B_Cancel;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.Label L_Rerolls;
        private System.Windows.Forms.Button B_RestoreOriginal;
    }
}