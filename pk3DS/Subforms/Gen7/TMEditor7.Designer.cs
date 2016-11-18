namespace pk3DS
{
    partial class TMEditor7
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
            this.dgvTM = new System.Windows.Forms.DataGridView();
            this.L_TM = new System.Windows.Forms.Label();
            this.B_RTM = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTM)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTM
            // 
            this.dgvTM.AllowUserToAddRows = false;
            this.dgvTM.AllowUserToDeleteRows = false;
            this.dgvTM.AllowUserToResizeColumns = false;
            this.dgvTM.AllowUserToResizeRows = false;
            this.dgvTM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvTM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTM.Location = new System.Drawing.Point(9, 25);
            this.dgvTM.Name = "dgvTM";
            this.dgvTM.Size = new System.Drawing.Size(240, 299);
            this.dgvTM.TabIndex = 1;
            // 
            // L_TM
            // 
            this.L_TM.AutoSize = true;
            this.L_TM.Location = new System.Drawing.Point(9, 9);
            this.L_TM.Name = "L_TM";
            this.L_TM.Size = new System.Drawing.Size(26, 13);
            this.L_TM.TabIndex = 2;
            this.L_TM.Text = "TM:";
            // 
            // B_RTM
            // 
            this.B_RTM.Location = new System.Drawing.Point(41, 1);
            this.B_RTM.Name = "B_RTM";
            this.B_RTM.Size = new System.Drawing.Size(75, 23);
            this.B_RTM.TabIndex = 5;
            this.B_RTM.Text = "Randomize";
            this.B_RTM.UseVisualStyleBackColor = true;
            this.B_RTM.Click += new System.EventHandler(this.B_RandomTM_Click);
            // 
            // TMHMEditor7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 331);
            this.Controls.Add(this.B_RTM);
            this.Controls.Add(this.L_TM);
            this.Controls.Add(this.dgvTM);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(520, 670);
            this.MinimumSize = new System.Drawing.Size(275, 370);
            this.Name = "TMHMEditor7";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TM Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTM;
        private System.Windows.Forms.Label L_TM;
        private System.Windows.Forms.Button B_RTM;
    }
}