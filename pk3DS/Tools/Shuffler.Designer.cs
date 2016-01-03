namespace pk3DS
{
    partial class Shuffler
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
            this.CB_a = new System.Windows.Forms.ComboBox();
            this.CB_b = new System.Windows.Forms.ComboBox();
            this.CB_c = new System.Windows.Forms.ComboBox();
            this.L_a = new System.Windows.Forms.Label();
            this.L_File = new System.Windows.Forms.Label();
            this.B_Shuffle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CB_a
            // 
            this.CB_a.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_a.FormattingEnabled = true;
            this.CB_a.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.CB_a.Location = new System.Drawing.Point(36, 11);
            this.CB_a.Name = "CB_a";
            this.CB_a.Size = new System.Drawing.Size(34, 21);
            this.CB_a.TabIndex = 0;
            this.CB_a.SelectedIndexChanged += new System.EventHandler(this.updateLabel);
            // 
            // CB_b
            // 
            this.CB_b.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_b.FormattingEnabled = true;
            this.CB_b.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.CB_b.Location = new System.Drawing.Point(76, 11);
            this.CB_b.Name = "CB_b";
            this.CB_b.Size = new System.Drawing.Size(34, 21);
            this.CB_b.TabIndex = 1;
            this.CB_b.SelectedIndexChanged += new System.EventHandler(this.updateLabel);
            // 
            // CB_c
            // 
            this.CB_c.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_c.FormattingEnabled = true;
            this.CB_c.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.CB_c.Location = new System.Drawing.Point(116, 11);
            this.CB_c.Name = "CB_c";
            this.CB_c.Size = new System.Drawing.Size(34, 21);
            this.CB_c.TabIndex = 2;
            this.CB_c.SelectedIndexChanged += new System.EventHandler(this.updateLabel);
            // 
            // L_a
            // 
            this.L_a.Location = new System.Drawing.Point(12, 9);
            this.L_a.Name = "L_a";
            this.L_a.Size = new System.Drawing.Size(18, 23);
            this.L_a.TabIndex = 3;
            this.L_a.Text = "a";
            this.L_a.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_File
            // 
            this.L_File.Location = new System.Drawing.Point(15, 35);
            this.L_File.Name = "L_File";
            this.L_File.Size = new System.Drawing.Size(135, 23);
            this.L_File.TabIndex = 4;
            this.L_File.Text = "FILENAME HERE";
            this.L_File.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // B_Shuffle
            // 
            this.B_Shuffle.Location = new System.Drawing.Point(76, 67);
            this.B_Shuffle.Name = "B_Shuffle";
            this.B_Shuffle.Size = new System.Drawing.Size(75, 23);
            this.B_Shuffle.TabIndex = 5;
            this.B_Shuffle.Text = "Shuffle!";
            this.B_Shuffle.UseVisualStyleBackColor = true;
            this.B_Shuffle.Click += new System.EventHandler(this.B_Shuffle_Click);
            // 
            // Shuffler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(164, 102);
            this.Controls.Add(this.B_Shuffle);
            this.Controls.Add(this.L_File);
            this.Controls.Add(this.L_a);
            this.Controls.Add(this.CB_c);
            this.Controls.Add(this.CB_b);
            this.Controls.Add(this.CB_a);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(180, 140);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(180, 140);
            this.Name = "Shuffler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shuffler";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_a;
        private System.Windows.Forms.ComboBox CB_b;
        private System.Windows.Forms.ComboBox CB_c;
        private System.Windows.Forms.Label L_a;
        private System.Windows.Forms.Label L_File;
        private System.Windows.Forms.Button B_Shuffle;
    }
}