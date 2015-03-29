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
            this.label2 = new System.Windows.Forms.Label();
            this.NUD_Level = new System.Windows.Forms.NumericUpDown();
            this.CHK_Level = new System.Windows.Forms.CheckBox();
            this.GB_Tweak = new System.Windows.Forms.GroupBox();
            this.CHK_BST = new System.Windows.Forms.CheckBox();
            this.CHK_E = new System.Windows.Forms.CheckBox();
            this.CHK_L = new System.Windows.Forms.CheckBox();
            this.CHK_G6 = new System.Windows.Forms.CheckBox();
            this.CHK_G5 = new System.Windows.Forms.CheckBox();
            this.CHK_G4 = new System.Windows.Forms.CheckBox();
            this.CHK_G3 = new System.Windows.Forms.CheckBox();
            this.CHK_G2 = new System.Windows.Forms.CheckBox();
            this.CHK_G1 = new System.Windows.Forms.CheckBox();
            this.CHK_TypeTheme = new System.Windows.Forms.CheckBox();
            this.CHK_StoryMEvos = new System.Windows.Forms.CheckBox();
            this.CHK_GymTrainers = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GiftPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
            this.GB_Tweak.SuspendLayout();
            this.SuspendLayout();
            // 
            // CHK_RandomPKM
            // 
            this.CHK_RandomPKM.AutoSize = true;
            this.CHK_RandomPKM.Checked = true;
            this.CHK_RandomPKM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomPKM.Location = new System.Drawing.Point(12, 31);
            this.CHK_RandomPKM.Name = "CHK_RandomPKM";
            this.CHK_RandomPKM.Size = new System.Drawing.Size(114, 17);
            this.CHK_RandomPKM.TabIndex = 0;
            this.CHK_RandomPKM.Text = "Random Pokemon";
            this.CHK_RandomPKM.UseVisualStyleBackColor = true;
            this.CHK_RandomPKM.CheckedChanged += new System.EventHandler(this.CHK_RandomPKM_CheckedChanged);
            // 
            // CHK_RandomItems
            // 
            this.CHK_RandomItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_RandomItems.AutoSize = true;
            this.CHK_RandomItems.Checked = true;
            this.CHK_RandomItems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomItems.Location = new System.Drawing.Point(12, 265);
            this.CHK_RandomItems.Name = "CHK_RandomItems";
            this.CHK_RandomItems.Size = new System.Drawing.Size(119, 17);
            this.CHK_RandomItems.TabIndex = 6;
            this.CHK_RandomItems.Text = "Random Held Items";
            this.CHK_RandomItems.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomMoves
            // 
            this.CHK_RandomMoves.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_RandomMoves.AutoSize = true;
            this.CHK_RandomMoves.Checked = true;
            this.CHK_RandomMoves.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomMoves.Location = new System.Drawing.Point(12, 250);
            this.CHK_RandomMoves.Name = "CHK_RandomMoves";
            this.CHK_RandomMoves.Size = new System.Drawing.Size(101, 17);
            this.CHK_RandomMoves.TabIndex = 5;
            this.CHK_RandomMoves.Text = "Random Moves";
            this.CHK_RandomMoves.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomAbilities
            // 
            this.CHK_RandomAbilities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_RandomAbilities.AutoSize = true;
            this.CHK_RandomAbilities.Checked = true;
            this.CHK_RandomAbilities.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomAbilities.Location = new System.Drawing.Point(12, 280);
            this.CHK_RandomAbilities.Name = "CHK_RandomAbilities";
            this.CHK_RandomAbilities.Size = new System.Drawing.Size(193, 17);
            this.CHK_RandomAbilities.TabIndex = 7;
            this.CHK_RandomAbilities.Text = "Random Abilities (Including Hidden)";
            this.CHK_RandomAbilities.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomGift
            // 
            this.CHK_RandomGift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_RandomGift.AutoSize = true;
            this.CHK_RandomGift.Checked = true;
            this.CHK_RandomGift.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomGift.Location = new System.Drawing.Point(12, 330);
            this.CHK_RandomGift.Name = "CHK_RandomGift";
            this.CHK_RandomGift.Size = new System.Drawing.Size(145, 17);
            this.CHK_RandomGift.TabIndex = 10;
            this.CHK_RandomGift.Text = "Random After-Battle Gifts";
            this.CHK_RandomGift.UseVisualStyleBackColor = true;
            this.CHK_RandomGift.CheckedChanged += new System.EventHandler(this.CHK_RandomGift_CheckedChanged);
            // 
            // CHK_RandomClass
            // 
            this.CHK_RandomClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_RandomClass.AutoSize = true;
            this.CHK_RandomClass.Checked = true;
            this.CHK_RandomClass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomClass.Location = new System.Drawing.Point(12, 315);
            this.CHK_RandomClass.Name = "CHK_RandomClass";
            this.CHK_RandomClass.Size = new System.Drawing.Size(141, 17);
            this.CHK_RandomClass.TabIndex = 9;
            this.CHK_RandomClass.Text = "Random Trainer Classes";
            this.CHK_RandomClass.UseVisualStyleBackColor = true;
            // 
            // CHK_MaxDiffAI
            // 
            this.CHK_MaxDiffAI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_MaxDiffAI.AutoSize = true;
            this.CHK_MaxDiffAI.Checked = true;
            this.CHK_MaxDiffAI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_MaxDiffAI.Location = new System.Drawing.Point(12, 345);
            this.CHK_MaxDiffAI.Name = "CHK_MaxDiffAI";
            this.CHK_MaxDiffAI.Size = new System.Drawing.Size(144, 17);
            this.CHK_MaxDiffAI.TabIndex = 13;
            this.CHK_MaxDiffAI.Text = "Max Difficulty (Trainer AI)";
            this.CHK_MaxDiffAI.UseVisualStyleBackColor = true;
            // 
            // CHK_MaxDiffPKM
            // 
            this.CHK_MaxDiffPKM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_MaxDiffPKM.AutoSize = true;
            this.CHK_MaxDiffPKM.Checked = true;
            this.CHK_MaxDiffPKM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_MaxDiffPKM.Location = new System.Drawing.Point(12, 295);
            this.CHK_MaxDiffPKM.Name = "CHK_MaxDiffPKM";
            this.CHK_MaxDiffPKM.Size = new System.Drawing.Size(161, 17);
            this.CHK_MaxDiffPKM.TabIndex = 8;
            this.CHK_MaxDiffPKM.Text = "Max Difficulty (Pokemon IVs)";
            this.CHK_MaxDiffPKM.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(222, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(222, 342);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.B_Close_Click);
            // 
            // NUD_GiftPercent
            // 
            this.NUD_GiftPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NUD_GiftPercent.Location = new System.Drawing.Point(155, 327);
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
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 329);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "%";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "%";
            // 
            // NUD_Level
            // 
            this.NUD_Level.Location = new System.Drawing.Point(155, 9);
            this.NUD_Level.Minimum = new decimal(new int[] {
            75,
            0,
            0,
            -2147483648});
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
            this.CHK_Level.Location = new System.Drawing.Point(12, 12);
            this.CHK_Level.Name = "CHK_Level";
            this.CHK_Level.Size = new System.Drawing.Size(126, 17);
            this.CHK_Level.TabIndex = 2;
            this.CHK_Level.Text = "Level Boost Multiplier";
            this.CHK_Level.UseVisualStyleBackColor = true;
            this.CHK_Level.CheckedChanged += new System.EventHandler(this.CHK_Level_CheckedChanged);
            // 
            // GB_Tweak
            // 
            this.GB_Tweak.Controls.Add(this.CHK_GymTrainers);
            this.GB_Tweak.Controls.Add(this.CHK_StoryMEvos);
            this.GB_Tweak.Controls.Add(this.CHK_TypeTheme);
            this.GB_Tweak.Controls.Add(this.CHK_BST);
            this.GB_Tweak.Controls.Add(this.CHK_E);
            this.GB_Tweak.Controls.Add(this.CHK_L);
            this.GB_Tweak.Controls.Add(this.CHK_G6);
            this.GB_Tweak.Controls.Add(this.CHK_G5);
            this.GB_Tweak.Controls.Add(this.CHK_G4);
            this.GB_Tweak.Controls.Add(this.CHK_G3);
            this.GB_Tweak.Controls.Add(this.CHK_G2);
            this.GB_Tweak.Controls.Add(this.CHK_G1);
            this.GB_Tweak.Location = new System.Drawing.Point(12, 54);
            this.GB_Tweak.Name = "GB_Tweak";
            this.GB_Tweak.Size = new System.Drawing.Size(270, 120);
            this.GB_Tweak.TabIndex = 323;
            this.GB_Tweak.TabStop = false;
            this.GB_Tweak.Text = "Options";
            // 
            // CHK_BST
            // 
            this.CHK_BST.AutoSize = true;
            this.CHK_BST.Location = new System.Drawing.Point(141, 51);
            this.CHK_BST.Name = "CHK_BST";
            this.CHK_BST.Size = new System.Drawing.Size(117, 17);
            this.CHK_BST.TabIndex = 288;
            this.CHK_BST.Text = "Randomize by BST";
            this.CHK_BST.UseVisualStyleBackColor = true;
            // 
            // CHK_E
            // 
            this.CHK_E.AutoSize = true;
            this.CHK_E.Checked = true;
            this.CHK_E.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_E.Location = new System.Drawing.Point(141, 36);
            this.CHK_E.Name = "CHK_E";
            this.CHK_E.Size = new System.Drawing.Size(98, 17);
            this.CHK_E.TabIndex = 287;
            this.CHK_E.Text = "Event Legends";
            this.CHK_E.UseVisualStyleBackColor = true;
            // 
            // CHK_L
            // 
            this.CHK_L.AutoSize = true;
            this.CHK_L.Checked = true;
            this.CHK_L.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_L.Location = new System.Drawing.Point(141, 21);
            this.CHK_L.Name = "CHK_L";
            this.CHK_L.Size = new System.Drawing.Size(98, 17);
            this.CHK_L.TabIndex = 286;
            this.CHK_L.Text = "Game Legends";
            this.CHK_L.UseVisualStyleBackColor = true;
            // 
            // CHK_G6
            // 
            this.CHK_G6.AutoSize = true;
            this.CHK_G6.Checked = true;
            this.CHK_G6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_G6.Location = new System.Drawing.Point(75, 51);
            this.CHK_G6.Name = "CHK_G6";
            this.CHK_G6.Size = new System.Drawing.Size(55, 17);
            this.CHK_G6.TabIndex = 285;
            this.CHK_G6.Text = "Gen 6";
            this.CHK_G6.UseVisualStyleBackColor = true;
            // 
            // CHK_G5
            // 
            this.CHK_G5.AutoSize = true;
            this.CHK_G5.Checked = true;
            this.CHK_G5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_G5.Location = new System.Drawing.Point(75, 36);
            this.CHK_G5.Name = "CHK_G5";
            this.CHK_G5.Size = new System.Drawing.Size(55, 17);
            this.CHK_G5.TabIndex = 284;
            this.CHK_G5.Text = "Gen 5";
            this.CHK_G5.UseVisualStyleBackColor = true;
            // 
            // CHK_G4
            // 
            this.CHK_G4.AutoSize = true;
            this.CHK_G4.Checked = true;
            this.CHK_G4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_G4.Location = new System.Drawing.Point(75, 21);
            this.CHK_G4.Name = "CHK_G4";
            this.CHK_G4.Size = new System.Drawing.Size(55, 17);
            this.CHK_G4.TabIndex = 283;
            this.CHK_G4.Text = "Gen 4";
            this.CHK_G4.UseVisualStyleBackColor = true;
            // 
            // CHK_G3
            // 
            this.CHK_G3.AutoSize = true;
            this.CHK_G3.Checked = true;
            this.CHK_G3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_G3.Location = new System.Drawing.Point(9, 51);
            this.CHK_G3.Name = "CHK_G3";
            this.CHK_G3.Size = new System.Drawing.Size(55, 17);
            this.CHK_G3.TabIndex = 282;
            this.CHK_G3.Text = "Gen 3";
            this.CHK_G3.UseVisualStyleBackColor = true;
            // 
            // CHK_G2
            // 
            this.CHK_G2.AutoSize = true;
            this.CHK_G2.Checked = true;
            this.CHK_G2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_G2.Location = new System.Drawing.Point(9, 36);
            this.CHK_G2.Name = "CHK_G2";
            this.CHK_G2.Size = new System.Drawing.Size(55, 17);
            this.CHK_G2.TabIndex = 281;
            this.CHK_G2.Text = "Gen 2";
            this.CHK_G2.UseVisualStyleBackColor = true;
            // 
            // CHK_G1
            // 
            this.CHK_G1.AutoSize = true;
            this.CHK_G1.Checked = true;
            this.CHK_G1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_G1.Location = new System.Drawing.Point(9, 21);
            this.CHK_G1.Name = "CHK_G1";
            this.CHK_G1.Size = new System.Drawing.Size(55, 17);
            this.CHK_G1.TabIndex = 280;
            this.CHK_G1.Text = "Gen 1";
            this.CHK_G1.UseVisualStyleBackColor = true;
            // 
            // CHK_TypeTheme
            // 
            this.CHK_TypeTheme.AutoSize = true;
            this.CHK_TypeTheme.Location = new System.Drawing.Point(8, 74);
            this.CHK_TypeTheme.Name = "CHK_TypeTheme";
            this.CHK_TypeTheme.Size = new System.Drawing.Size(130, 17);
            this.CHK_TypeTheme.TabIndex = 289;
            this.CHK_TypeTheme.Text = "Type Theme Trainers:";
            this.CHK_TypeTheme.UseVisualStyleBackColor = true;
            this.CHK_TypeTheme.CheckedChanged += new System.EventHandler(this.CHK_TypeTheme_CheckedChanged);
            // 
            // CHK_StoryMEvos
            // 
            this.CHK_StoryMEvos.AutoSize = true;
            this.CHK_StoryMEvos.Checked = true;
            this.CHK_StoryMEvos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_StoryMEvos.Location = new System.Drawing.Point(141, 73);
            this.CHK_StoryMEvos.Name = "CHK_StoryMEvos";
            this.CHK_StoryMEvos.Size = new System.Drawing.Size(122, 17);
            this.CHK_StoryMEvos.TabIndex = 291;
            this.CHK_StoryMEvos.Text = "Ensure Story MEvos";
            this.CHK_StoryMEvos.UseVisualStyleBackColor = true;
            // 
            // CHK_GymTrainers
            // 
            this.CHK_GymTrainers.AutoSize = true;
            this.CHK_GymTrainers.Enabled = false;
            this.CHK_GymTrainers.Location = new System.Drawing.Point(8, 92);
            this.CHK_GymTrainers.Name = "CHK_GymTrainers";
            this.CHK_GymTrainers.Size = new System.Drawing.Size(124, 17);
            this.CHK_GymTrainers.TabIndex = 292;
            this.CHK_GymTrainers.Text = "Theme Gym Trainers";
            this.CHK_GymTrainers.UseVisualStyleBackColor = true;
            // 
            // TrainerRand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 377);
            this.Controls.Add(this.GB_Tweak);
            this.Controls.Add(this.NUD_GiftPercent);
            this.Controls.Add(this.CHK_MaxDiffAI);
            this.Controls.Add(this.CHK_MaxDiffPKM);
            this.Controls.Add(this.CHK_RandomAbilities);
            this.Controls.Add(this.CHK_RandomItems);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NUD_Level);
            this.Controls.Add(this.CHK_Level);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CHK_RandomGift);
            this.Controls.Add(this.CHK_RandomMoves);
            this.Controls.Add(this.CHK_RandomPKM);
            this.Controls.Add(this.CHK_RandomClass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 840);
            this.MinimumSize = new System.Drawing.Size(300, 240);
            this.Name = "TrainerRand";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Trainer Battle Randomizer";
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GiftPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).EndInit();
            this.GB_Tweak.ResumeLayout(false);
            this.GB_Tweak.PerformLayout();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NUD_Level;
        private System.Windows.Forms.CheckBox CHK_Level;
        private System.Windows.Forms.GroupBox GB_Tweak;
        private System.Windows.Forms.CheckBox CHK_BST;
        private System.Windows.Forms.CheckBox CHK_E;
        private System.Windows.Forms.CheckBox CHK_L;
        private System.Windows.Forms.CheckBox CHK_G6;
        private System.Windows.Forms.CheckBox CHK_G5;
        private System.Windows.Forms.CheckBox CHK_G4;
        private System.Windows.Forms.CheckBox CHK_G3;
        private System.Windows.Forms.CheckBox CHK_G2;
        private System.Windows.Forms.CheckBox CHK_G1;
        private System.Windows.Forms.CheckBox CHK_TypeTheme;
        private System.Windows.Forms.CheckBox CHK_StoryMEvos;
        private System.Windows.Forms.CheckBox CHK_GymTrainers;
    }
}