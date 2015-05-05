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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.PB_BCLIM = new System.Windows.Forms.PictureBox();
            this.B_Reset = new System.Windows.Forms.Button();
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
            this.L_DARCMini.Size = new System.Drawing.Size(145, 13);
            this.L_DARCMini.TabIndex = 1;
            this.L_DARCMini.Text = "GARC, DARC && Mini Unpack";
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
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(168, 124);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(144, 67);
            this.panel1.TabIndex = 5;
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
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 124);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(150, 21);
            this.comboBox1.TabIndex = 8;
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
            // ToolsUI
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 362);
            this.Controls.Add(this.B_Reset);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
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
            ((System.ComponentModel.ISupportInitialize)(this.PB_BCLIM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PB_Unpack;
        private System.Windows.Forms.Label L_DARCMini;
        private System.Windows.Forms.CheckBox CHK_PNG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox PB_BCLIM;
        private System.Windows.Forms.Button B_Reset;
    }
}