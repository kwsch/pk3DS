namespace pk3DS
{
    partial class About
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
            this.B_Close = new System.Windows.Forms.Button();
            this.RTB = new System.Windows.Forms.RichTextBox();
            this.L_Thanks = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // B_Close
            // 
            this.B_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Close.Location = new System.Drawing.Point(412, 340);
            this.B_Close.Name = "B_Close";
            this.B_Close.Size = new System.Drawing.Size(75, 23);
            this.B_Close.TabIndex = 0;
            this.B_Close.Text = "Close";
            this.B_Close.UseVisualStyleBackColor = true;
            this.B_Close.Click += new System.EventHandler(this.B_Close_Click);
            // 
            // RTB
            // 
            this.RTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RTB.Location = new System.Drawing.Point(7, 9);
            this.RTB.Name = "RTB";
            this.RTB.ReadOnly = true;
            this.RTB.Size = new System.Drawing.Size(480, 322);
            this.RTB.TabIndex = 1;
            this.RTB.Text = "";
            this.RTB.WordWrap = false;
            // 
            // L_Thanks
            // 
            this.L_Thanks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.L_Thanks.AutoSize = true;
            this.L_Thanks.Location = new System.Drawing.Point(12, 345);
            this.L_Thanks.Name = "L_Thanks";
            this.L_Thanks.Size = new System.Drawing.Size(147, 13);
            this.L_Thanks.TabIndex = 2;
            this.L_Thanks.Text = "Thanks to all the researchers!";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 372);
            this.Controls.Add(this.L_Thanks);
            this.Controls.Add(this.RTB);
            this.Controls.Add(this.B_Close);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(910, 710);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(510, 410);
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_Close;
        private System.Windows.Forms.RichTextBox RTB;
        private System.Windows.Forms.Label L_Thanks;
    }
}