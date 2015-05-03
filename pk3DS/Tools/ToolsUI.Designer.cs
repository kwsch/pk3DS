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
            this.PB_BCLIM = new System.Windows.Forms.Panel();
            this.CHK_PNG = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PB_Unpack
            // 
            this.PB_Unpack.Location = new System.Drawing.Point(12, 25);
            this.PB_Unpack.Name = "PB_Unpack";
            this.PB_Unpack.Size = new System.Drawing.Size(150, 67);
            this.PB_Unpack.TabIndex = 0;
            // 
            // L_DARCMini
            // 
            this.L_DARCMini.AutoSize = true;
            this.L_DARCMini.Location = new System.Drawing.Point(12, 9);
            this.L_DARCMini.Name = "L_DARCMini";
            this.L_DARCMini.Size = new System.Drawing.Size(109, 13);
            this.L_DARCMini.TabIndex = 1;
            this.L_DARCMini.Text = "DARC && Mini Unpack";
            // 
            // PB_BCLIM
            // 
            this.PB_BCLIM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PB_BCLIM.Location = new System.Drawing.Point(15, 283);
            this.PB_BCLIM.Name = "PB_BCLIM";
            this.PB_BCLIM.Size = new System.Drawing.Size(300, 67);
            this.PB_BCLIM.TabIndex = 1;
            this.PB_BCLIM.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_BCLIM_Paint);
            // 
            // CHK_PNG
            // 
            this.CHK_PNG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CHK_PNG.AutoSize = true;
            this.CHK_PNG.Location = new System.Drawing.Point(218, 266);
            this.CHK_PNG.Name = "CHK_PNG";
            this.CHK_PNG.Size = new System.Drawing.Size(97, 17);
            this.CHK_PNG.TabIndex = 2;
            this.CHK_PNG.Text = "Autosave PNG";
            this.CHK_PNG.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 267);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "BCLIM Converter";
            // 
            // ToolsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 362);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PB_Unpack;
        private System.Windows.Forms.Label L_DARCMini;
        private System.Windows.Forms.Panel PB_BCLIM;
        private System.Windows.Forms.CheckBox CHK_PNG;
        private System.Windows.Forms.Label label1;

    }
}