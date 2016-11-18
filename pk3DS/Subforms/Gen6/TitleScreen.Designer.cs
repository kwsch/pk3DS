namespace pk3DS
{
    partial class TitleScreen
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
            this.PB_Image = new System.Windows.Forms.PictureBox();
            this.CB_DARC = new System.Windows.Forms.ComboBox();
            this.CB_File = new System.Windows.Forms.ComboBox();
            this.L_DARCSelect = new System.Windows.Forms.Label();
            this.L_Dimensions = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Image)).BeginInit();
            this.SuspendLayout();
            // 
            // PB_Image
            // 
            this.PB_Image.Location = new System.Drawing.Point(12, 66);
            this.PB_Image.Name = "PB_Image";
            this.PB_Image.Size = new System.Drawing.Size(400, 240);
            this.PB_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PB_Image.TabIndex = 0;
            this.PB_Image.TabStop = false;
            this.PB_Image.Click += new System.EventHandler(this.PB_Image_Click);
            // 
            // CB_DARC
            // 
            this.CB_DARC.FormattingEnabled = true;
            this.CB_DARC.Location = new System.Drawing.Point(58, 8);
            this.CB_DARC.Name = "CB_DARC";
            this.CB_DARC.Size = new System.Drawing.Size(100, 21);
            this.CB_DARC.TabIndex = 1;
            this.CB_DARC.SelectedIndexChanged += new System.EventHandler(this.changeDARC);
            // 
            // CB_File
            // 
            this.CB_File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_File.FormattingEnabled = true;
            this.CB_File.Location = new System.Drawing.Point(12, 35);
            this.CB_File.Name = "CB_File";
            this.CB_File.Size = new System.Drawing.Size(400, 21);
            this.CB_File.TabIndex = 2;
            this.CB_File.SelectedIndexChanged += new System.EventHandler(this.changeFile);
            // 
            // L_DARCSelect
            // 
            this.L_DARCSelect.AutoSize = true;
            this.L_DARCSelect.Location = new System.Drawing.Point(12, 11);
            this.L_DARCSelect.Name = "L_DARCSelect";
            this.L_DARCSelect.Size = new System.Drawing.Size(40, 13);
            this.L_DARCSelect.TabIndex = 3;
            this.L_DARCSelect.Text = "DARC:";
            // 
            // L_Dimensions
            // 
            this.L_Dimensions.AutoSize = true;
            this.L_Dimensions.Location = new System.Drawing.Point(164, 11);
            this.L_Dimensions.Name = "L_Dimensions";
            this.L_Dimensions.Size = new System.Drawing.Size(61, 13);
            this.L_Dimensions.TabIndex = 4;
            this.L_Dimensions.Text = "Dimensions";
            // 
            // TitleScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 316);
            this.Controls.Add(this.L_Dimensions);
            this.Controls.Add(this.L_DARCSelect);
            this.Controls.Add(this.CB_File);
            this.Controls.Add(this.CB_DARC);
            this.Controls.Add(this.PB_Image);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(555, 420);
            this.MinimumSize = new System.Drawing.Size(440, 354);
            this.Name = "TitleScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Title Screen Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PB_Image;
        private System.Windows.Forms.ComboBox CB_DARC;
        private System.Windows.Forms.ComboBox CB_File;
        private System.Windows.Forms.Label L_DARCSelect;
        private System.Windows.Forms.Label L_Dimensions;

    }
}