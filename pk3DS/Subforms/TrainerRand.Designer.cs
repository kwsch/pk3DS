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
            this.CHK_Smart = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NUD_Level = new System.Windows.Forms.NumericUpDown();
            this.CHK_Level = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GiftPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
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
            this.CHK_RandomPKM.CheckedChanged += new System.EventHandler(this.CHK_RandomPKM_CheckedChanged);
            // 
            // CHK_RandomItems
            // 
            this.CHK_RandomItems.AutoSize = true;
            this.CHK_RandomItems.Checked = true;
            this.CHK_RandomItems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomItems.Location = new System.Drawing.Point(12, 76);
            this.CHK_RandomItems.Name = "CHK_RandomItems";
            this.CHK_RandomItems.Size = new System.Drawing.Size(119, 17);
            this.CHK_RandomItems.TabIndex = 6;
            this.CHK_RandomItems.Text = "Random Held Items";
            this.CHK_RandomItems.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomMoves
            // 
            this.CHK_RandomMoves.AutoSize = true;
            this.CHK_RandomMoves.Checked = true;
            this.CHK_RandomMoves.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomMoves.Location = new System.Drawing.Point(12, 61);
            this.CHK_RandomMoves.Name = "CHK_RandomMoves";
            this.CHK_RandomMoves.Size = new System.Drawing.Size(101, 17);
            this.CHK_RandomMoves.TabIndex = 5;
            this.CHK_RandomMoves.Text = "Random Moves";
            this.CHK_RandomMoves.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomAbilities
            // 
            this.CHK_RandomAbilities.AutoSize = true;
            this.CHK_RandomAbilities.Checked = true;
            this.CHK_RandomAbilities.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomAbilities.Location = new System.Drawing.Point(12, 91);
            this.CHK_RandomAbilities.Name = "CHK_RandomAbilities";
            this.CHK_RandomAbilities.Size = new System.Drawing.Size(193, 17);
            this.CHK_RandomAbilities.TabIndex = 7;
            this.CHK_RandomAbilities.Text = "Random Abilities (Including Hidden)";
            this.CHK_RandomAbilities.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomGift
            // 
            this.CHK_RandomGift.AutoSize = true;
            this.CHK_RandomGift.Checked = true;
            this.CHK_RandomGift.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomGift.Location = new System.Drawing.Point(12, 155);
            this.CHK_RandomGift.Name = "CHK_RandomGift";
            this.CHK_RandomGift.Size = new System.Drawing.Size(145, 17);
            this.CHK_RandomGift.TabIndex = 10;
            this.CHK_RandomGift.Text = "Random After-Battle Gifts";
            this.CHK_RandomGift.UseVisualStyleBackColor = true;
            this.CHK_RandomGift.CheckedChanged += new System.EventHandler(this.CHK_RandomGift_CheckedChanged);
            // 
            // CHK_RandomClass
            // 
            this.CHK_RandomClass.AutoSize = true;
            this.CHK_RandomClass.Checked = true;
            this.CHK_RandomClass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomClass.Location = new System.Drawing.Point(12, 140);
            this.CHK_RandomClass.Name = "CHK_RandomClass";
            this.CHK_RandomClass.Size = new System.Drawing.Size(141, 17);
            this.CHK_RandomClass.TabIndex = 9;
            this.CHK_RandomClass.Text = "Random Trainer Classes";
            this.CHK_RandomClass.UseVisualStyleBackColor = true;
            // 
            // CHK_MaxDiffAI
            // 
            this.CHK_MaxDiffAI.AutoSize = true;
            this.CHK_MaxDiffAI.Checked = true;
            this.CHK_MaxDiffAI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_MaxDiffAI.Location = new System.Drawing.Point(12, 170);
            this.CHK_MaxDiffAI.Name = "CHK_MaxDiffAI";
            this.CHK_MaxDiffAI.Size = new System.Drawing.Size(144, 17);
            this.CHK_MaxDiffAI.TabIndex = 13;
            this.CHK_MaxDiffAI.Text = "Max Difficulty (Trainer AI)";
            this.CHK_MaxDiffAI.UseVisualStyleBackColor = true;
            // 
            // CHK_MaxDiffPKM
            // 
            this.CHK_MaxDiffPKM.AutoSize = true;
            this.CHK_MaxDiffPKM.Checked = true;
            this.CHK_MaxDiffPKM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_MaxDiffPKM.Location = new System.Drawing.Point(12, 106);
            this.CHK_MaxDiffPKM.Name = "CHK_MaxDiffPKM";
            this.CHK_MaxDiffPKM.Size = new System.Drawing.Size(161, 17);
            this.CHK_MaxDiffPKM.TabIndex = 8;
            this.CHK_MaxDiffPKM.Text = "Max Difficulty (Pokemon IVs)";
            this.CHK_MaxDiffPKM.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(222, 145);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(222, 167);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.B_Close_Click);
            // 
            // NUD_GiftPercent
            // 
            this.NUD_GiftPercent.Location = new System.Drawing.Point(155, 152);
            this.NUD_GiftPercent.Name = "NUD_GiftPercent";
            this.NUD_GiftPercent.Size = new System.Drawing.Size(43, 20);
            this.NUD_GiftPercent.TabIndex = 11;
            this.NUD_GiftPercent.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NUD_GiftPercent.ValueChanged += new System.EventHandler(this.changeGiftPercent);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "%";
            // 
            // CHK_Smart
            // 
            this.CHK_Smart.AutoSize = true;
            this.CHK_Smart.Checked = true;
            this.CHK_Smart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_Smart.Location = new System.Drawing.Point(132, 12);
            this.CHK_Smart.Name = "CHK_Smart";
            this.CHK_Smart.Size = new System.Drawing.Size(112, 17);
            this.CHK_Smart.TabIndex = 1;
            this.CHK_Smart.Text = "Smart Rand (BST)";
            this.CHK_Smart.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(200, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "%";
            // 
            // NUD_Level
            // 
            this.NUD_Level.Location = new System.Drawing.Point(155, 27);
            this.NUD_Level.Name = "NUD_Level";
            this.NUD_Level.Size = new System.Drawing.Size(43, 20);
            this.NUD_Level.TabIndex = 3;
            this.NUD_Level.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NUD_Level.ValueChanged += new System.EventHandler(this.changeLevelPercent);
            // 
            // CHK_Level
            // 
            this.CHK_Level.AutoSize = true;
            this.CHK_Level.Checked = true;
            this.CHK_Level.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_Level.Location = new System.Drawing.Point(12, 27);
            this.CHK_Level.Name = "CHK_Level";
            this.CHK_Level.Size = new System.Drawing.Size(126, 17);
            this.CHK_Level.TabIndex = 2;
            this.CHK_Level.Text = "Level Boost Multiplier";
            this.CHK_Level.UseVisualStyleBackColor = true;
            this.CHK_Level.CheckedChanged += new System.EventHandler(this.CHK_Level_CheckedChanged);
            // 
            // TrainerRand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 202);
            this.Controls.Add(this.NUD_GiftPercent);
            this.Controls.Add(this.CHK_MaxDiffAI);
            this.Controls.Add(this.CHK_MaxDiffPKM);
            this.Controls.Add(this.CHK_RandomAbilities);
            this.Controls.Add(this.CHK_RandomItems);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NUD_Level);
            this.Controls.Add(this.CHK_Level);
            this.Controls.Add(this.CHK_Smart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CHK_RandomGift);
            this.Controls.Add(this.CHK_RandomMoves);
            this.Controls.Add(this.CHK_RandomPKM);
            this.Controls.Add(this.CHK_RandomClass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TrainerRand";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Trainer Battle Randomizer";
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GiftPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).EndInit();
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
        private System.Windows.Forms.CheckBox CHK_Smart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NUD_Level;
        private System.Windows.Forms.CheckBox CHK_Level;
    }
}