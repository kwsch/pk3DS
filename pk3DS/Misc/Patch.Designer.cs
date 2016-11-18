namespace pk3DS
{
    partial class Patch
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
            this.CHKLB_GARCs = new System.Windows.Forms.CheckedListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.B_PatchCIA = new System.Windows.Forms.Button();
            this.CHK_Lang = new System.Windows.Forms.CheckBox();
            this.RTB_GARCs = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.B_CheckAll = new System.Windows.Forms.Button();
            this.B_CheckNone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CHKLB_GARCs
            // 
            this.CHKLB_GARCs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CHKLB_GARCs.FormattingEnabled = true;
            this.CHKLB_GARCs.Location = new System.Drawing.Point(12, 12);
            this.CHKLB_GARCs.Name = "CHKLB_GARCs";
            this.CHKLB_GARCs.Size = new System.Drawing.Size(150, 259);
            this.CHKLB_GARCs.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(168, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(104, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "a/#/* -> a#/*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ExeFS Path Override:";
            // 
            // B_PatchCIA
            // 
            this.B_PatchCIA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_PatchCIA.Location = new System.Drawing.Point(168, 278);
            this.B_PatchCIA.Name = "B_PatchCIA";
            this.B_PatchCIA.Size = new System.Drawing.Size(104, 23);
            this.B_PatchCIA.TabIndex = 3;
            this.B_PatchCIA.Text = "Redirect [CIA]";
            this.B_PatchCIA.UseVisualStyleBackColor = true;
            this.B_PatchCIA.Click += new System.EventHandler(this.B_PatchCIA_Click);
            // 
            // CHK_Lang
            // 
            this.CHK_Lang.AutoSize = true;
            this.CHK_Lang.Location = new System.Drawing.Point(171, 54);
            this.CHK_Lang.Name = "CHK_Lang";
            this.CHK_Lang.Size = new System.Drawing.Size(110, 17);
            this.CHK_Lang.TabIndex = 4;
            this.CHK_Lang.Text = "Patch Languages";
            this.CHK_Lang.UseVisualStyleBackColor = true;
            // 
            // RTB_GARCs
            // 
            this.RTB_GARCs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.RTB_GARCs.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RTB_GARCs.Location = new System.Drawing.Point(168, 90);
            this.RTB_GARCs.Name = "RTB_GARCs";
            this.RTB_GARCs.Size = new System.Drawing.Size(75, 181);
            this.RTB_GARCs.TabIndex = 5;
            this.RTB_GARCs.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(165, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Extra GARCs:";
            // 
            // B_CheckAll
            // 
            this.B_CheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_CheckAll.Location = new System.Drawing.Point(12, 278);
            this.B_CheckAll.Name = "B_CheckAll";
            this.B_CheckAll.Size = new System.Drawing.Size(75, 23);
            this.B_CheckAll.TabIndex = 7;
            this.B_CheckAll.Text = "Check All";
            this.B_CheckAll.UseVisualStyleBackColor = true;
            this.B_CheckAll.Click += new System.EventHandler(this.B_CheckAll_Click);
            // 
            // B_CheckNone
            // 
            this.B_CheckNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_CheckNone.Location = new System.Drawing.Point(87, 278);
            this.B_CheckNone.Name = "B_CheckNone";
            this.B_CheckNone.Size = new System.Drawing.Size(75, 23);
            this.B_CheckNone.TabIndex = 8;
            this.B_CheckNone.Text = "Check None";
            this.B_CheckNone.UseVisualStyleBackColor = true;
            this.B_CheckNone.Click += new System.EventHandler(this.B_CheckNone_Click);
            // 
            // Patch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 312);
            this.Controls.Add(this.B_CheckNone);
            this.Controls.Add(this.B_CheckAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RTB_GARCs);
            this.Controls.Add(this.CHK_Lang);
            this.Controls.Add(this.B_PatchCIA);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.CHKLB_GARCs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Patch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Patch Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.savePatch);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox CHKLB_GARCs;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button B_PatchCIA;
        private System.Windows.Forms.CheckBox CHK_Lang;
        private System.Windows.Forms.RichTextBox RTB_GARCs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button B_CheckAll;
        private System.Windows.Forms.Button B_CheckNone;
    }
}