namespace pk3DS
{
    partial class ToolsUI
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
            this.PB_Unpack = new System.Windows.Forms.Panel();
            this.L_DARCMini = new System.Windows.Forms.Label();
            this.CHK_PNG = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PB_Repack = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.CB_Repack = new System.Windows.Forms.ComboBox();
            this.PB_BCLIM = new System.Windows.Forms.PictureBox();
            this.B_Reset = new System.Windows.Forms.Button();
            this.CHK_Delete = new System.Windows.Forms.CheckBox();
            this.pBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.PB_BCLIM)).BeginInit();
            this.SuspendLayout();
            // 
            // PB_Unpack
            // 
            this.PB_Unpack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_Unpack.Location = new System.Drawing.Point(12, 25);
            this.PB_Unpack.Name = "PB_Unpack";
            this.PB_Unpack.Size = new System.Drawing.Size(300, 67);
            this.PB_Unpack.TabIndex = 0;
            this.PB_Unpack.MouseLeave += new System.EventHandler(this.dropLeave);
            this.PB_Unpack.MouseHover += new System.EventHandler(this.dropHover);
            // 
            // L_DARCMini
            // 
            this.L_DARCMini.AutoSize = true;
            this.L_DARCMini.Location = new System.Drawing.Point(9, 9);
            this.L_DARCMini.Name = "L_DARCMini";
            this.L_DARCMini.Size = new System.Drawing.Size(294, 13);
            this.L_DARCMini.TabIndex = 1;
            this.L_DARCMini.Text = "GARC, DARC && Mini Unpack (CTRL to Skip Decompression)";
            // 
            // CHK_PNG
            // 
            this.CHK_PNG.AutoSize = true;
            this.CHK_PNG.Location = new System.Drawing.Point(89, 209);
            this.CHK_PNG.Name = "CHK_PNG";
            this.CHK_PNG.Size = new System.Drawing.Size(97, 17);
            this.CHK_PNG.TabIndex = 2;
            this.CHK_PNG.Text = "Autosave PNG";
            this.CHK_PNG.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 210);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "BCLIM Viewer";
            // 
            // PB_Repack
            // 
            this.PB_Repack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_Repack.Location = new System.Drawing.Point(168, 124);
            this.PB_Repack.Name = "PB_Repack";
            this.PB_Repack.Size = new System.Drawing.Size(144, 67);
            this.PB_Repack.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Folder Repack";
            // 
            // CB_Repack
            // 
            this.CB_Repack.FormattingEnabled = true;
            this.CB_Repack.Location = new System.Drawing.Point(12, 124);
            this.CB_Repack.Name = "CB_Repack";
            this.CB_Repack.Size = new System.Drawing.Size(150, 21);
            this.CB_Repack.TabIndex = 8;
            // 
            // PB_BCLIM
            // 
            this.PB_BCLIM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PB_BCLIM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_BCLIM.Location = new System.Drawing.Point(12, 226);
            this.PB_BCLIM.Name = "PB_BCLIM";
            this.PB_BCLIM.Size = new System.Drawing.Size(300, 124);
            this.PB_BCLIM.TabIndex = 1;
            this.PB_BCLIM.TabStop = false;
            this.PB_BCLIM.Click += new System.EventHandler(this.PB_BCLIM_Click);
            // 
            // B_Reset
            // 
            this.B_Reset.Location = new System.Drawing.Point(12, 184);
            this.B_Reset.Name = "B_Reset";
            this.B_Reset.Size = new System.Drawing.Size(80, 23);
            this.B_Reset.TabIndex = 9;
            this.B_Reset.Text = "Reset View";
            this.B_Reset.UseVisualStyleBackColor = true;
            this.B_Reset.Click += new System.EventHandler(this.B_Reset_Click);
            // 
            // CHK_Delete
            // 
            this.CHK_Delete.AutoSize = true;
            this.CHK_Delete.Checked = true;
            this.CHK_Delete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_Delete.Location = new System.Drawing.Point(12, 148);
            this.CHK_Delete.Name = "CHK_Delete";
            this.CHK_Delete.Size = new System.Drawing.Size(155, 17);
            this.CHK_Delete.TabIndex = 10;
            this.CHK_Delete.Text = "Delete Folder after Packing";
            this.CHK_Delete.UseVisualStyleBackColor = true;
            // 
            // pBar1
            // 
            this.pBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBar1.Location = new System.Drawing.Point(12, 95);
            this.pBar1.Name = "pBar1";
            this.pBar1.Size = new System.Drawing.Size(300, 12);
            this.pBar1.TabIndex = 11;
            // 
            // ToolsUI
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 362);
            this.Controls.Add(this.pBar1);
            this.Controls.Add(this.CHK_Delete);
            this.Controls.Add(this.B_Reset);
            this.Controls.Add(this.CB_Repack);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PB_Repack);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CHK_PNG);
            this.Controls.Add(this.PB_BCLIM);
            this.Controls.Add(this.L_DARCMini);
            this.Controls.Add(this.PB_Unpack);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(340, 400);
            this.Name = "ToolsUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tools";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.closeForm);
            ((System.ComponentModel.ISupportInitialize)(this.PB_BCLIM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PB_Unpack;
        private System.Windows.Forms.Label L_DARCMini;
        private System.Windows.Forms.CheckBox CHK_PNG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel PB_Repack;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CB_Repack;
        private System.Windows.Forms.PictureBox PB_BCLIM;
        private System.Windows.Forms.Button B_Reset;
        private System.Windows.Forms.CheckBox CHK_Delete;
        private System.Windows.Forms.ProgressBar pBar1;
    }
}