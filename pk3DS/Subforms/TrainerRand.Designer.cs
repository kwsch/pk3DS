namespace pk3DS
{
    partial class TrainerRand
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
            this.CHK_RandomPKM = new System.Windows.Forms.CheckBox();
            this.CHK_RandomItems = new System.Windows.Forms.CheckBox();
            this.CHK_RandomMoves = new System.Windows.Forms.CheckBox();
            this.CHK_RandomAbilities = new System.Windows.Forms.CheckBox();
            this.CHK_RandomGift = new System.Windows.Forms.CheckBox();
            this.CHK_RandomClass = new System.Windows.Forms.CheckBox();
            this.CHK_MaxDiffAI = new System.Windows.Forms.CheckBox();
            this.CHK_MaxDiffPKM = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.NUD_GiftPercent = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GiftPercent)).BeginInit();
            this.SuspendLayout();
            // 
            // CHK_RandomPKM
            // 
            this.CHK_RandomPKM.AutoSize = true;
            this.CHK_RandomPKM.Checked = true;
            this.CHK_RandomPKM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomPKM.Location = new System.Drawing.Point(12, 12);
            this.CHK_RandomPKM.Name = "CHK_RandomPKM";
            this.CHK_RandomPKM.Size = new System.Drawing.Size(114, 17);
            this.CHK_RandomPKM.TabIndex = 0;
            this.CHK_RandomPKM.Text = "Random Pokemon";
            this.CHK_RandomPKM.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomItems
            // 
            this.CHK_RandomItems.AutoSize = true;
            this.CHK_RandomItems.Location = new System.Drawing.Point(12, 193);
            this.CHK_RandomItems.Name = "CHK_RandomItems";
            this.CHK_RandomItems.Size = new System.Drawing.Size(119, 17);
            this.CHK_RandomItems.TabIndex = 1;
            this.CHK_RandomItems.Text = "Random Held Items";
            this.CHK_RandomItems.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomMoves
            // 
            this.CHK_RandomMoves.AutoSize = true;
            this.CHK_RandomMoves.Checked = true;
            this.CHK_RandomMoves.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomMoves.Location = new System.Drawing.Point(12, 35);
            this.CHK_RandomMoves.Name = "CHK_RandomMoves";
            this.CHK_RandomMoves.Size = new System.Drawing.Size(101, 17);
            this.CHK_RandomMoves.TabIndex = 2;
            this.CHK_RandomMoves.Text = "Random Moves";
            this.CHK_RandomMoves.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomAbilities
            // 
            this.CHK_RandomAbilities.AutoSize = true;
            this.CHK_RandomAbilities.Checked = true;
            this.CHK_RandomAbilities.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomAbilities.Location = new System.Drawing.Point(12, 58);
            this.CHK_RandomAbilities.Name = "CHK_RandomAbilities";
            this.CHK_RandomAbilities.Size = new System.Drawing.Size(193, 17);
            this.CHK_RandomAbilities.TabIndex = 3;
            this.CHK_RandomAbilities.Text = "Random Abilities (Including Hidden)";
            this.CHK_RandomAbilities.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomGift
            // 
            this.CHK_RandomGift.AutoSize = true;
            this.CHK_RandomGift.Location = new System.Drawing.Point(12, 170);
            this.CHK_RandomGift.Name = "CHK_RandomGift";
            this.CHK_RandomGift.Size = new System.Drawing.Size(145, 17);
            this.CHK_RandomGift.TabIndex = 4;
            this.CHK_RandomGift.Text = "Random After-Battle Gifts";
            this.CHK_RandomGift.UseVisualStyleBackColor = true;
            this.CHK_RandomGift.CheckedChanged += new System.EventHandler(this.CHK_RandomGift_CheckedChanged);
            // 
            // CHK_RandomClass
            // 
            this.CHK_RandomClass.AutoSize = true;
            this.CHK_RandomClass.Location = new System.Drawing.Point(12, 147);
            this.CHK_RandomClass.Name = "CHK_RandomClass";
            this.CHK_RandomClass.Size = new System.Drawing.Size(141, 17);
            this.CHK_RandomClass.TabIndex = 5;
            this.CHK_RandomClass.Text = "Random Trainer Classes";
            this.CHK_RandomClass.UseVisualStyleBackColor = true;
            // 
            // CHK_MaxDiffAI
            // 
            this.CHK_MaxDiffAI.AutoSize = true;
            this.CHK_MaxDiffAI.Location = new System.Drawing.Point(12, 104);
            this.CHK_MaxDiffAI.Name = "CHK_MaxDiffAI";
            this.CHK_MaxDiffAI.Size = new System.Drawing.Size(219, 17);
            this.CHK_MaxDiffAI.TabIndex = 6;
            this.CHK_MaxDiffAI.Text = "[NOT VISIBLE] Max Difficulty (Trainer AI)";
            this.CHK_MaxDiffAI.UseVisualStyleBackColor = true;
            this.CHK_MaxDiffAI.Visible = false;
            // 
            // CHK_MaxDiffPKM
            // 
            this.CHK_MaxDiffPKM.AutoSize = true;
            this.CHK_MaxDiffPKM.Checked = true;
            this.CHK_MaxDiffPKM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_MaxDiffPKM.Location = new System.Drawing.Point(12, 81);
            this.CHK_MaxDiffPKM.Name = "CHK_MaxDiffPKM";
            this.CHK_MaxDiffPKM.Size = new System.Drawing.Size(161, 17);
            this.CHK_MaxDiffPKM.TabIndex = 7;
            this.CHK_MaxDiffPKM.Text = "Max Difficulty (Pokemon IVs)";
            this.CHK_MaxDiffPKM.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(212, 189);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(146, 189);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(60, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.B_Close_Click);
            // 
            // NUD_GiftPercent
            // 
            this.NUD_GiftPercent.Enabled = false;
            this.NUD_GiftPercent.Location = new System.Drawing.Point(163, 167);
            this.NUD_GiftPercent.Name = "NUD_GiftPercent";
            this.NUD_GiftPercent.Size = new System.Drawing.Size(43, 20);
            this.NUD_GiftPercent.TabIndex = 10;
            this.NUD_GiftPercent.ValueChanged += new System.EventHandler(this.changePercent);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "%";
            // 
            // TrainerRand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 222);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NUD_GiftPercent);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CHK_MaxDiffPKM);
            this.Controls.Add(this.CHK_MaxDiffAI);
            this.Controls.Add(this.CHK_RandomClass);
            this.Controls.Add(this.CHK_RandomGift);
            this.Controls.Add(this.CHK_RandomAbilities);
            this.Controls.Add(this.CHK_RandomMoves);
            this.Controls.Add(this.CHK_RandomItems);
            this.Controls.Add(this.CHK_RandomPKM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TrainerRand";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Randomizer";
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GiftPercent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CHK_RandomPKM;
        private System.Windows.Forms.CheckBox CHK_RandomItems;
        private System.Windows.Forms.CheckBox CHK_RandomMoves;
        private System.Windows.Forms.CheckBox CHK_RandomAbilities;
        private System.Windows.Forms.CheckBox CHK_RandomGift;
        private System.Windows.Forms.CheckBox CHK_RandomClass;
        private System.Windows.Forms.CheckBox CHK_MaxDiffAI;
        private System.Windows.Forms.CheckBox CHK_MaxDiffPKM;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown NUD_GiftPercent;
        private System.Windows.Forms.Label label1;
    }
}