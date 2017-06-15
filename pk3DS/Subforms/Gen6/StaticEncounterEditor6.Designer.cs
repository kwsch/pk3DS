namespace pk3DS
{
    partial class StaticEncounterEditor6
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
            this.B_Cancel = new System.Windows.Forms.Button();
            this.B_Save = new System.Windows.Forms.Button();
            this.LB_Encounters = new System.Windows.Forms.ListBox();
            this.CB_Species = new System.Windows.Forms.ComboBox();
            this.L_Species = new System.Windows.Forms.Label();
            this.NUD_Level = new System.Windows.Forms.NumericUpDown();
            this.L_Level = new System.Windows.Forms.Label();
            this.NUD_Form = new System.Windows.Forms.NumericUpDown();
            this.L_Form = new System.Windows.Forms.Label();
            this.B_RandAll = new System.Windows.Forms.Button();
            this.L_HeldItem = new System.Windows.Forms.Label();
            this.CB_HeldItem = new System.Windows.Forms.ComboBox();
            this.NUD_Ability = new System.Windows.Forms.NumericUpDown();
            this.L_Ability = new System.Windows.Forms.Label();
            this.NUD_Gender = new System.Windows.Forms.NumericUpDown();
            this.L_Gender = new System.Windows.Forms.Label();
            this.CHK_3IV = new System.Windows.Forms.CheckBox();
            this.CHK_NoShiny = new System.Windows.Forms.CheckBox();
            this.CHK_3IV_2 = new System.Windows.Forms.CheckBox();
            this.L_Hint = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.GB_Tweak = new System.Windows.Forms.GroupBox();
            this.L_RandOpt = new System.Windows.Forms.Label();
            this.CHK_BST = new System.Windows.Forms.CheckBox();
            this.CHK_E = new System.Windows.Forms.CheckBox();
            this.CHK_L = new System.Windows.Forms.CheckBox();
            this.CHK_G6 = new System.Windows.Forms.CheckBox();
            this.CHK_G5 = new System.Windows.Forms.CheckBox();
            this.CHK_G4 = new System.Windows.Forms.CheckBox();
            this.CHK_G3 = new System.Windows.Forms.CheckBox();
            this.CHK_G2 = new System.Windows.Forms.CheckBox();
            this.CHK_G1 = new System.Windows.Forms.CheckBox();
            this.NUD_LevelBoost = new System.Windows.Forms.NumericUpDown();
            this.CHK_Level = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Ability)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Gender)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.GB_Tweak.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_LevelBoost)).BeginInit();
            this.SuspendLayout();
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Cancel.Location = new System.Drawing.Point(261, 296);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(70, 23);
            this.B_Cancel.TabIndex = 467;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.UseVisualStyleBackColor = true;
            this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
            // 
            // B_Save
            // 
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(332, 296);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(70, 23);
            this.B_Save.TabIndex = 466;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // LB_Encounters
            // 
            this.LB_Encounters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_Encounters.FormattingEnabled = true;
            this.LB_Encounters.Location = new System.Drawing.Point(12, 12);
            this.LB_Encounters.Name = "LB_Encounters";
            this.LB_Encounters.Size = new System.Drawing.Size(110, 303);
            this.LB_Encounters.TabIndex = 468;
            this.LB_Encounters.SelectedIndexChanged += new System.EventHandler(this.changeIndex);
            // 
            // CB_Species
            // 
            this.CB_Species.FormattingEnabled = true;
            this.CB_Species.Location = new System.Drawing.Point(106, 3);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(121, 21);
            this.CB_Species.TabIndex = 469;
            this.CB_Species.SelectedIndexChanged += new System.EventHandler(this.changeSpecies);
            // 
            // L_Species
            // 
            this.L_Species.Location = new System.Drawing.Point(6, 3);
            this.L_Species.Name = "L_Species";
            this.L_Species.Size = new System.Drawing.Size(94, 21);
            this.L_Species.TabIndex = 477;
            this.L_Species.Text = "Species:";
            this.L_Species.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_Level
            // 
            this.NUD_Level.Location = new System.Drawing.Point(106, 57);
            this.NUD_Level.Name = "NUD_Level";
            this.NUD_Level.Size = new System.Drawing.Size(41, 20);
            this.NUD_Level.TabIndex = 479;
            this.NUD_Level.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // L_Level
            // 
            this.L_Level.Location = new System.Drawing.Point(9, 55);
            this.L_Level.Name = "L_Level";
            this.L_Level.Size = new System.Drawing.Size(92, 21);
            this.L_Level.TabIndex = 480;
            this.L_Level.Text = "Level:";
            this.L_Level.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_Form
            // 
            this.NUD_Form.Location = new System.Drawing.Point(193, 57);
            this.NUD_Form.Name = "NUD_Form";
            this.NUD_Form.Size = new System.Drawing.Size(34, 20);
            this.NUD_Form.TabIndex = 481;
            // 
            // L_Form
            // 
            this.L_Form.Location = new System.Drawing.Point(99, 55);
            this.L_Form.Name = "L_Form";
            this.L_Form.Size = new System.Drawing.Size(94, 21);
            this.L_Form.TabIndex = 482;
            this.L_Form.Text = "Form:";
            this.L_Form.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // B_RandAll
            // 
            this.B_RandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_RandAll.Location = new System.Drawing.Point(178, 296);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(83, 23);
            this.B_RandAll.TabIndex = 496;
            this.B_RandAll.Text = "Randomize All";
            this.B_RandAll.UseVisualStyleBackColor = true;
            this.B_RandAll.Click += new System.EventHandler(this.B_RandAll_Click);
            // 
            // L_HeldItem
            // 
            this.L_HeldItem.Location = new System.Drawing.Point(6, 29);
            this.L_HeldItem.Name = "L_HeldItem";
            this.L_HeldItem.Size = new System.Drawing.Size(94, 21);
            this.L_HeldItem.TabIndex = 478;
            this.L_HeldItem.Text = "Held Item:";
            this.L_HeldItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_HeldItem
            // 
            this.CB_HeldItem.FormattingEnabled = true;
            this.CB_HeldItem.Location = new System.Drawing.Point(106, 30);
            this.CB_HeldItem.Name = "CB_HeldItem";
            this.CB_HeldItem.Size = new System.Drawing.Size(121, 21);
            this.CB_HeldItem.TabIndex = 470;
            // 
            // NUD_Ability
            // 
            this.NUD_Ability.Location = new System.Drawing.Point(194, 82);
            this.NUD_Ability.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NUD_Ability.Name = "NUD_Ability";
            this.NUD_Ability.Size = new System.Drawing.Size(34, 20);
            this.NUD_Ability.TabIndex = 492;
            // 
            // L_Ability
            // 
            this.L_Ability.Location = new System.Drawing.Point(100, 80);
            this.L_Ability.Name = "L_Ability";
            this.L_Ability.Size = new System.Drawing.Size(94, 21);
            this.L_Ability.TabIndex = 493;
            this.L_Ability.Text = "Ability:";
            this.L_Ability.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_Gender
            // 
            this.NUD_Gender.Location = new System.Drawing.Point(194, 108);
            this.NUD_Gender.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NUD_Gender.Name = "NUD_Gender";
            this.NUD_Gender.Size = new System.Drawing.Size(34, 20);
            this.NUD_Gender.TabIndex = 494;
            // 
            // L_Gender
            // 
            this.L_Gender.Location = new System.Drawing.Point(100, 106);
            this.L_Gender.Name = "L_Gender";
            this.L_Gender.Size = new System.Drawing.Size(94, 21);
            this.L_Gender.TabIndex = 495;
            this.L_Gender.Text = "Gender:";
            this.L_Gender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CHK_3IV
            // 
            this.CHK_3IV.AutoSize = true;
            this.CHK_3IV.Location = new System.Drawing.Point(9, 106);
            this.CHK_3IV.Name = "CHK_3IV";
            this.CHK_3IV.Size = new System.Drawing.Size(42, 17);
            this.CHK_3IV.TabIndex = 497;
            this.CHK_3IV.Text = "3IV";
            this.CHK_3IV.UseVisualStyleBackColor = true;
            // 
            // CHK_NoShiny
            // 
            this.CHK_NoShiny.AutoSize = true;
            this.CHK_NoShiny.Location = new System.Drawing.Point(9, 83);
            this.CHK_NoShiny.Name = "CHK_NoShiny";
            this.CHK_NoShiny.Size = new System.Drawing.Size(79, 17);
            this.CHK_NoShiny.TabIndex = 498;
            this.CHK_NoShiny.Text = "Shiny Lock";
            this.CHK_NoShiny.UseVisualStyleBackColor = true;
            // 
            // CHK_3IV_2
            // 
            this.CHK_3IV_2.AutoSize = true;
            this.CHK_3IV_2.Location = new System.Drawing.Point(9, 129);
            this.CHK_3IV_2.Name = "CHK_3IV_2";
            this.CHK_3IV_2.Size = new System.Drawing.Size(54, 17);
            this.CHK_3IV_2.TabIndex = 499;
            this.CHK_3IV_2.Text = "3IV_2";
            this.CHK_3IV_2.UseVisualStyleBackColor = true;
            // 
            // L_Hint
            // 
            this.L_Hint.AutoSize = true;
            this.L_Hint.Location = new System.Drawing.Point(161, 148);
            this.L_Hint.Name = "L_Hint";
            this.L_Hint.Size = new System.Drawing.Size(67, 52);
            this.L_Hint.TabIndex = 500;
            this.L_Hint.Text = "Gender:\r\n0: Random/-\r\n1: Male\r\n2: Female";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(128, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(280, 260);
            this.tabControl1.TabIndex = 501;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.L_Hint);
            this.tabPage1.Controls.Add(this.CB_Species);
            this.tabPage1.Controls.Add(this.NUD_Ability);
            this.tabPage1.Controls.Add(this.CHK_3IV_2);
            this.tabPage1.Controls.Add(this.CHK_NoShiny);
            this.tabPage1.Controls.Add(this.L_Species);
            this.tabPage1.Controls.Add(this.CHK_3IV);
            this.tabPage1.Controls.Add(this.L_HeldItem);
            this.tabPage1.Controls.Add(this.L_Level);
            this.tabPage1.Controls.Add(this.L_Gender);
            this.tabPage1.Controls.Add(this.CB_HeldItem);
            this.tabPage1.Controls.Add(this.NUD_Gender);
            this.tabPage1.Controls.Add(this.NUD_Form);
            this.tabPage1.Controls.Add(this.NUD_Level);
            this.tabPage1.Controls.Add(this.L_Ability);
            this.tabPage1.Controls.Add(this.L_Form);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(272, 234);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Editor";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(90, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 65);
            this.label1.TabIndex = 502;
            this.label1.Text = "Ability:\r\n0: Random\r\n1: Ability 0\r\n2: Ability 1\r\n3: Hidden";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.GB_Tweak);
            this.tabPage2.Controls.Add(this.NUD_LevelBoost);
            this.tabPage2.Controls.Add(this.CHK_Level);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(272, 234);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Randomizer Options";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // GB_Tweak
            // 
            this.GB_Tweak.Controls.Add(this.L_RandOpt);
            this.GB_Tweak.Controls.Add(this.CHK_BST);
            this.GB_Tweak.Controls.Add(this.CHK_E);
            this.GB_Tweak.Controls.Add(this.CHK_L);
            this.GB_Tweak.Controls.Add(this.CHK_G6);
            this.GB_Tweak.Controls.Add(this.CHK_G5);
            this.GB_Tweak.Controls.Add(this.CHK_G4);
            this.GB_Tweak.Controls.Add(this.CHK_G3);
            this.GB_Tweak.Controls.Add(this.CHK_G2);
            this.GB_Tweak.Controls.Add(this.CHK_G1);
            this.GB_Tweak.Location = new System.Drawing.Point(7, 67);
            this.GB_Tweak.Name = "GB_Tweak";
            this.GB_Tweak.Size = new System.Drawing.Size(258, 100);
            this.GB_Tweak.TabIndex = 509;
            this.GB_Tweak.TabStop = false;
            this.GB_Tweak.Text = "Extra Randomization Tweaks";
            // 
            // L_RandOpt
            // 
            this.L_RandOpt.AutoSize = true;
            this.L_RandOpt.Location = new System.Drawing.Point(6, 16);
            this.L_RandOpt.Name = "L_RandOpt";
            this.L_RandOpt.Size = new System.Drawing.Size(105, 13);
            this.L_RandOpt.TabIndex = 294;
            this.L_RandOpt.Text = "Randomizer Options:";
            // 
            // CHK_BST
            // 
            this.CHK_BST.AutoSize = true;
            this.CHK_BST.Location = new System.Drawing.Point(128, 64);
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
            this.CHK_E.Location = new System.Drawing.Point(128, 49);
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
            this.CHK_L.Location = new System.Drawing.Point(128, 34);
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
            this.CHK_G6.Location = new System.Drawing.Point(67, 64);
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
            this.CHK_G5.Location = new System.Drawing.Point(67, 49);
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
            this.CHK_G4.Location = new System.Drawing.Point(67, 34);
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
            this.CHK_G3.Location = new System.Drawing.Point(9, 64);
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
            this.CHK_G2.Location = new System.Drawing.Point(9, 49);
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
            this.CHK_G1.Location = new System.Drawing.Point(9, 34);
            this.CHK_G1.Name = "CHK_G1";
            this.CHK_G1.Size = new System.Drawing.Size(55, 17);
            this.CHK_G1.TabIndex = 280;
            this.CHK_G1.Text = "Gen 1";
            this.CHK_G1.UseVisualStyleBackColor = true;
            // 
            // NUD_LevelBoost
            // 
            this.NUD_LevelBoost.DecimalPlaces = 2;
            this.NUD_LevelBoost.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.NUD_LevelBoost.Location = new System.Drawing.Point(140, 6);
            this.NUD_LevelBoost.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NUD_LevelBoost.Name = "NUD_LevelBoost";
            this.NUD_LevelBoost.Size = new System.Drawing.Size(43, 20);
            this.NUD_LevelBoost.TabIndex = 303;
            this.NUD_LevelBoost.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CHK_Level
            // 
            this.CHK_Level.AutoSize = true;
            this.CHK_Level.Checked = true;
            this.CHK_Level.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_Level.Location = new System.Drawing.Point(8, 7);
            this.CHK_Level.Name = "CHK_Level";
            this.CHK_Level.Size = new System.Drawing.Size(130, 17);
            this.CHK_Level.TabIndex = 302;
            this.CHK_Level.Text = "Multiply PKM Level by";
            this.CHK_Level.UseVisualStyleBackColor = true;
            // 
            // StaticEncounterEditor6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 331);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.B_RandAll);
            this.Controls.Add(this.LB_Encounters);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_Save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 370);
            this.Name = "StaticEncounterEditor6";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Static Encounter Editor";
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Ability)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Gender)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.GB_Tweak.ResumeLayout(false);
            this.GB_Tweak.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_LevelBoost)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button B_Cancel;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.ListBox LB_Encounters;
        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.Label L_Species;
        private System.Windows.Forms.NumericUpDown NUD_Level;
        private System.Windows.Forms.Label L_Level;
        private System.Windows.Forms.NumericUpDown NUD_Form;
        private System.Windows.Forms.Label L_Form;
        private System.Windows.Forms.Button B_RandAll;
        private System.Windows.Forms.Label L_Gender;
        private System.Windows.Forms.NumericUpDown NUD_Gender;
        private System.Windows.Forms.Label L_Ability;
        private System.Windows.Forms.NumericUpDown NUD_Ability;
        private System.Windows.Forms.ComboBox CB_HeldItem;
        private System.Windows.Forms.Label L_HeldItem;
        private System.Windows.Forms.CheckBox CHK_3IV;
        private System.Windows.Forms.CheckBox CHK_NoShiny;
        private System.Windows.Forms.CheckBox CHK_3IV_2;
        private System.Windows.Forms.Label L_Hint;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NUD_LevelBoost;
        private System.Windows.Forms.CheckBox CHK_Level;
        private System.Windows.Forms.GroupBox GB_Tweak;
        private System.Windows.Forms.Label L_RandOpt;
        private System.Windows.Forms.CheckBox CHK_BST;
        private System.Windows.Forms.CheckBox CHK_E;
        private System.Windows.Forms.CheckBox CHK_L;
        private System.Windows.Forms.CheckBox CHK_G6;
        private System.Windows.Forms.CheckBox CHK_G5;
        private System.Windows.Forms.CheckBox CHK_G4;
        private System.Windows.Forms.CheckBox CHK_G3;
        private System.Windows.Forms.CheckBox CHK_G2;
        private System.Windows.Forms.CheckBox CHK_G1;
    }
}