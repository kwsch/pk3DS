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
            this.CHK_RandomAbilities = new System.Windows.Forms.CheckBox();
            this.CHK_RandomGift = new System.Windows.Forms.CheckBox();
            this.CHK_RandomClass = new System.Windows.Forms.CheckBox();
            this.CHK_MaxDiffAI = new System.Windows.Forms.CheckBox();
            this.CHK_MaxDiffPKM = new System.Windows.Forms.CheckBox();
            this.B_OK = new System.Windows.Forms.Button();
            this.B_Cancel = new System.Windows.Forms.Button();
            this.NUD_GiftPercent = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NUD_Level = new System.Windows.Forms.NumericUpDown();
            this.CHK_Level = new System.Windows.Forms.CheckBox();
            this.GB_Tweak = new System.Windows.Forms.GroupBox();
            this.CHK_RandomMegaForm = new System.Windows.Forms.CheckBox();
            this.CHK_6PKM = new System.Windows.Forms.CheckBox();
            this.CHK_GymTrainers = new System.Windows.Forms.CheckBox();
            this.CHK_StoryMEvos = new System.Windows.Forms.CheckBox();
            this.CHK_TypeTheme = new System.Windows.Forms.CheckBox();
            this.CHK_BST = new System.Windows.Forms.CheckBox();
            this.CHK_E = new System.Windows.Forms.CheckBox();
            this.CHK_L = new System.Windows.Forms.CheckBox();
            this.CHK_G6 = new System.Windows.Forms.CheckBox();
            this.CHK_G5 = new System.Windows.Forms.CheckBox();
            this.CHK_G4 = new System.Windows.Forms.CheckBox();
            this.CHK_G3 = new System.Windows.Forms.CheckBox();
            this.CHK_G2 = new System.Windows.Forms.CheckBox();
            this.CHK_G1 = new System.Windows.Forms.CheckBox();
            this.CHK_IgnoreSpecialClass = new System.Windows.Forms.CheckBox();
            this.CHK_OnlySingles = new System.Windows.Forms.CheckBox();
            this.NUD_Damage = new System.Windows.Forms.NumericUpDown();
            this.CHK_Damage = new System.Windows.Forms.CheckBox();
            this.CHK_STAB = new System.Windows.Forms.CheckBox();
            this.NUD_STAB = new System.Windows.Forms.NumericUpDown();
            this.CB_Moves = new System.Windows.Forms.ComboBox();
            this.L_Moves = new System.Windows.Forms.Label();
            this.CHK_GymE4Only = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GiftPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
            this.GB_Tweak.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Damage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_STAB)).BeginInit();
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
            this.CHK_RandomItems.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CHK_RandomItems.AutoSize = true;
            this.CHK_RandomItems.Checked = true;
            this.CHK_RandomItems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomItems.Location = new System.Drawing.Point(12, 264);
            this.CHK_RandomItems.Name = "CHK_RandomItems";
            this.CHK_RandomItems.Size = new System.Drawing.Size(119, 17);
            this.CHK_RandomItems.TabIndex = 6;
            this.CHK_RandomItems.Text = "Random Held Items";
            this.CHK_RandomItems.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomAbilities
            // 
            this.CHK_RandomAbilities.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CHK_RandomAbilities.AutoSize = true;
            this.CHK_RandomAbilities.Checked = true;
            this.CHK_RandomAbilities.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomAbilities.Location = new System.Drawing.Point(12, 279);
            this.CHK_RandomAbilities.Name = "CHK_RandomAbilities";
            this.CHK_RandomAbilities.Size = new System.Drawing.Size(193, 17);
            this.CHK_RandomAbilities.TabIndex = 7;
            this.CHK_RandomAbilities.Text = "Random Abilities (Including Hidden)";
            this.CHK_RandomAbilities.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomGift
            // 
            this.CHK_RandomGift.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CHK_RandomGift.AutoSize = true;
            this.CHK_RandomGift.Checked = true;
            this.CHK_RandomGift.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomGift.Location = new System.Drawing.Point(12, 363);
            this.CHK_RandomGift.Name = "CHK_RandomGift";
            this.CHK_RandomGift.Size = new System.Drawing.Size(145, 17);
            this.CHK_RandomGift.TabIndex = 10;
            this.CHK_RandomGift.Text = "Random After-Battle Gifts";
            this.CHK_RandomGift.UseVisualStyleBackColor = true;
            this.CHK_RandomGift.CheckedChanged += new System.EventHandler(this.CHK_RandomGift_CheckedChanged);
            // 
            // CHK_RandomClass
            // 
            this.CHK_RandomClass.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CHK_RandomClass.AutoSize = true;
            this.CHK_RandomClass.Checked = true;
            this.CHK_RandomClass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RandomClass.Location = new System.Drawing.Point(12, 317);
            this.CHK_RandomClass.Name = "CHK_RandomClass";
            this.CHK_RandomClass.Size = new System.Drawing.Size(141, 17);
            this.CHK_RandomClass.TabIndex = 9;
            this.CHK_RandomClass.Text = "Random Trainer Classes";
            this.CHK_RandomClass.UseVisualStyleBackColor = true;
            this.CHK_RandomClass.CheckedChanged += new System.EventHandler(this.CHK_RandomClass_CheckedChanged);
            // 
            // CHK_MaxDiffAI
            // 
            this.CHK_MaxDiffAI.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CHK_MaxDiffAI.AutoSize = true;
            this.CHK_MaxDiffAI.Checked = true;
            this.CHK_MaxDiffAI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_MaxDiffAI.Location = new System.Drawing.Point(12, 378);
            this.CHK_MaxDiffAI.Name = "CHK_MaxDiffAI";
            this.CHK_MaxDiffAI.Size = new System.Drawing.Size(144, 17);
            this.CHK_MaxDiffAI.TabIndex = 13;
            this.CHK_MaxDiffAI.Text = "Max Difficulty (Trainer AI)";
            this.CHK_MaxDiffAI.UseVisualStyleBackColor = true;
            // 
            // CHK_MaxDiffPKM
            // 
            this.CHK_MaxDiffPKM.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CHK_MaxDiffPKM.AutoSize = true;
            this.CHK_MaxDiffPKM.Checked = true;
            this.CHK_MaxDiffPKM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_MaxDiffPKM.Location = new System.Drawing.Point(12, 294);
            this.CHK_MaxDiffPKM.Name = "CHK_MaxDiffPKM";
            this.CHK_MaxDiffPKM.Size = new System.Drawing.Size(161, 17);
            this.CHK_MaxDiffPKM.TabIndex = 8;
            this.CHK_MaxDiffPKM.Text = "Max Difficulty (Pokemon IVs)";
            this.CHK_MaxDiffPKM.UseVisualStyleBackColor = true;
            // 
            // B_OK
            // 
            this.B_OK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.B_OK.Location = new System.Drawing.Point(222, 353);
            this.B_OK.Name = "B_OK";
            this.B_OK.Size = new System.Drawing.Size(50, 23);
            this.B_OK.TabIndex = 14;
            this.B_OK.Text = "OK";
            this.B_OK.UseVisualStyleBackColor = true;
            this.B_OK.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.B_Cancel.Location = new System.Drawing.Point(222, 375);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(50, 23);
            this.B_Cancel.TabIndex = 15;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.UseVisualStyleBackColor = true;
            this.B_Cancel.Click += new System.EventHandler(this.B_Close_Click);
            // 
            // NUD_GiftPercent
            // 
            this.NUD_GiftPercent.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NUD_GiftPercent.Location = new System.Drawing.Point(157, 362);
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
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(202, 364);
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
            this.GB_Tweak.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.GB_Tweak.Controls.Add(this.CHK_GymE4Only);
            this.GB_Tweak.Controls.Add(this.CHK_RandomMegaForm);
            this.GB_Tweak.Controls.Add(this.CHK_6PKM);
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
            this.GB_Tweak.Size = new System.Drawing.Size(270, 153);
            this.GB_Tweak.TabIndex = 323;
            this.GB_Tweak.TabStop = false;
            this.GB_Tweak.Text = "Options";
            // 
            // CHK_RandomMegaForm
            // 
            this.CHK_RandomMegaForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_RandomMegaForm.AutoSize = true;
            this.CHK_RandomMegaForm.Location = new System.Drawing.Point(9, 75);
            this.CHK_RandomMegaForm.Name = "CHK_RandomMegaForm";
            this.CHK_RandomMegaForm.Size = new System.Drawing.Size(103, 30);
            this.CHK_RandomMegaForm.TabIndex = 294;
            this.CHK_RandomMegaForm.Text = "Allow RandForm\r\nfor Mega PKM";
            this.CHK_RandomMegaForm.UseVisualStyleBackColor = true;
            // 
            // CHK_6PKM
            // 
            this.CHK_6PKM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_6PKM.AutoSize = true;
            this.CHK_6PKM.Location = new System.Drawing.Point(141, 75);
            this.CHK_6PKM.Name = "CHK_6PKM";
            this.CHK_6PKM.Size = new System.Drawing.Size(111, 30);
            this.CHK_6PKM.TabIndex = 293;
            this.CHK_6PKM.Text = "Have 6 PKM for \r\nImportant Trainers";
            this.CHK_6PKM.UseVisualStyleBackColor = true;
            this.CHK_6PKM.CheckedChanged += new System.EventHandler(this.CHK_6PKM_CheckedChanged);
            // 
            // CHK_GymTrainers
            // 
            this.CHK_GymTrainers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_GymTrainers.AutoSize = true;
            this.CHK_GymTrainers.Enabled = false;
            this.CHK_GymTrainers.Location = new System.Drawing.Point(9, 129);
            this.CHK_GymTrainers.Name = "CHK_GymTrainers";
            this.CHK_GymTrainers.Size = new System.Drawing.Size(124, 17);
            this.CHK_GymTrainers.TabIndex = 292;
            this.CHK_GymTrainers.Text = "Theme Gym Trainers";
            this.CHK_GymTrainers.UseVisualStyleBackColor = true;
            // 
            // CHK_StoryMEvos
            // 
            this.CHK_StoryMEvos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_StoryMEvos.AutoSize = true;
            this.CHK_StoryMEvos.Checked = true;
            this.CHK_StoryMEvos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_StoryMEvos.Location = new System.Drawing.Point(141, 113);
            this.CHK_StoryMEvos.Name = "CHK_StoryMEvos";
            this.CHK_StoryMEvos.Size = new System.Drawing.Size(122, 17);
            this.CHK_StoryMEvos.TabIndex = 291;
            this.CHK_StoryMEvos.Text = "Ensure Story MEvos";
            this.CHK_StoryMEvos.UseVisualStyleBackColor = true;
            // 
            // CHK_TypeTheme
            // 
            this.CHK_TypeTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_TypeTheme.AutoSize = true;
            this.CHK_TypeTheme.Location = new System.Drawing.Point(9, 113);
            this.CHK_TypeTheme.Name = "CHK_TypeTheme";
            this.CHK_TypeTheme.Size = new System.Drawing.Size(127, 17);
            this.CHK_TypeTheme.TabIndex = 289;
            this.CHK_TypeTheme.Text = "Type Theme Trainers";
            this.CHK_TypeTheme.UseVisualStyleBackColor = true;
            this.CHK_TypeTheme.CheckedChanged += new System.EventHandler(this.CHK_TypeTheme_CheckedChanged);
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
            // CHK_IgnoreSpecialClass
            // 
            this.CHK_IgnoreSpecialClass.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CHK_IgnoreSpecialClass.AutoSize = true;
            this.CHK_IgnoreSpecialClass.Checked = true;
            this.CHK_IgnoreSpecialClass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_IgnoreSpecialClass.Location = new System.Drawing.Point(31, 332);
            this.CHK_IgnoreSpecialClass.Name = "CHK_IgnoreSpecialClass";
            this.CHK_IgnoreSpecialClass.Size = new System.Drawing.Size(133, 17);
            this.CHK_IgnoreSpecialClass.TabIndex = 324;
            this.CHK_IgnoreSpecialClass.Text = "Ignore Special Classes";
            this.CHK_IgnoreSpecialClass.UseVisualStyleBackColor = true;
            // 
            // CHK_OnlySingles
            // 
            this.CHK_OnlySingles.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CHK_OnlySingles.AutoSize = true;
            this.CHK_OnlySingles.Checked = true;
            this.CHK_OnlySingles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_OnlySingles.Location = new System.Drawing.Point(31, 347);
            this.CHK_OnlySingles.Name = "CHK_OnlySingles";
            this.CHK_OnlySingles.Size = new System.Drawing.Size(114, 17);
            this.CHK_OnlySingles.TabIndex = 325;
            this.CHK_OnlySingles.Text = "Only Single Battles";
            this.CHK_OnlySingles.UseVisualStyleBackColor = true;
            // 
            // NUD_Damage
            // 
            this.NUD_Damage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NUD_Damage.Location = new System.Drawing.Point(204, 231);
            this.NUD_Damage.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NUD_Damage.Name = "NUD_Damage";
            this.NUD_Damage.Size = new System.Drawing.Size(35, 20);
            this.NUD_Damage.TabIndex = 326;
            this.NUD_Damage.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // CHK_Damage
            // 
            this.CHK_Damage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CHK_Damage.AutoSize = true;
            this.CHK_Damage.Checked = true;
            this.CHK_Damage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_Damage.Location = new System.Drawing.Point(12, 234);
            this.CHK_Damage.Name = "CHK_Damage";
            this.CHK_Damage.Size = new System.Drawing.Size(192, 17);
            this.CHK_Damage.TabIndex = 327;
            this.CHK_Damage.Text = "Ensure at least # Damaging Moves";
            this.CHK_Damage.UseVisualStyleBackColor = true;
            // 
            // CHK_STAB
            // 
            this.CHK_STAB.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CHK_STAB.AutoSize = true;
            this.CHK_STAB.Checked = true;
            this.CHK_STAB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_STAB.Location = new System.Drawing.Point(12, 249);
            this.CHK_STAB.Name = "CHK_STAB";
            this.CHK_STAB.Size = new System.Drawing.Size(172, 17);
            this.CHK_STAB.TabIndex = 328;
            this.CHK_STAB.Text = "Ensure at least # STAB Moves";
            this.CHK_STAB.UseVisualStyleBackColor = true;
            // 
            // NUD_STAB
            // 
            this.NUD_STAB.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NUD_STAB.Location = new System.Drawing.Point(179, 251);
            this.NUD_STAB.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NUD_STAB.Name = "NUD_STAB";
            this.NUD_STAB.Size = new System.Drawing.Size(35, 20);
            this.NUD_STAB.TabIndex = 329;
            this.NUD_STAB.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // CB_Moves
            // 
            this.CB_Moves.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CB_Moves.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Moves.FormattingEnabled = true;
            this.CB_Moves.Items.AddRange(new object[] {
            "Don\'t Modify",
            "Randomize All",
            "Use Levelup Only"});
            this.CB_Moves.Location = new System.Drawing.Point(60, 212);
            this.CB_Moves.Name = "CB_Moves";
            this.CB_Moves.Size = new System.Drawing.Size(121, 21);
            this.CB_Moves.TabIndex = 330;
            this.CB_Moves.SelectedIndexChanged += new System.EventHandler(this.changeMoveRandomization);
            // 
            // L_Moves
            // 
            this.L_Moves.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.L_Moves.AutoSize = true;
            this.L_Moves.Location = new System.Drawing.Point(12, 215);
            this.L_Moves.Name = "L_Moves";
            this.L_Moves.Size = new System.Drawing.Size(42, 13);
            this.L_Moves.TabIndex = 331;
            this.L_Moves.Text = "Moves:";
            // 
            // CHK_GymE4Only
            // 
            this.CHK_GymE4Only.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_GymE4Only.AutoSize = true;
            this.CHK_GymE4Only.Enabled = false;
            this.CHK_GymE4Only.Location = new System.Drawing.Point(141, 129);
            this.CHK_GymE4Only.Name = "CHK_GymE4Only";
            this.CHK_GymE4Only.Size = new System.Drawing.Size(125, 17);
            this.CHK_GymE4Only.TabIndex = 295;
            this.CHK_GymE4Only.Text = "Theme Gym/E4 Only";
            this.CHK_GymE4Only.UseVisualStyleBackColor = true;
            // 
            // TrainerRand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 410);
            this.Controls.Add(this.L_Moves);
            this.Controls.Add(this.CB_Moves);
            this.Controls.Add(this.CHK_OnlySingles);
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
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_OK);
            this.Controls.Add(this.CHK_RandomGift);
            this.Controls.Add(this.CHK_RandomPKM);
            this.Controls.Add(this.CHK_IgnoreSpecialClass);
            this.Controls.Add(this.CHK_RandomClass);
            this.Controls.Add(this.NUD_STAB);
            this.Controls.Add(this.CHK_STAB);
            this.Controls.Add(this.CHK_Damage);
            this.Controls.Add(this.NUD_Damage);
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
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Damage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_STAB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CHK_RandomPKM;
        private System.Windows.Forms.CheckBox CHK_RandomItems;
        private System.Windows.Forms.CheckBox CHK_RandomAbilities;
        private System.Windows.Forms.CheckBox CHK_RandomGift;
        private System.Windows.Forms.CheckBox CHK_RandomClass;
        private System.Windows.Forms.CheckBox CHK_MaxDiffAI;
        private System.Windows.Forms.CheckBox CHK_MaxDiffPKM;
        private System.Windows.Forms.Button B_OK;
        private System.Windows.Forms.Button B_Cancel;
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
        private System.Windows.Forms.CheckBox CHK_IgnoreSpecialClass;
        private System.Windows.Forms.CheckBox CHK_OnlySingles;
        private System.Windows.Forms.NumericUpDown NUD_Damage;
        private System.Windows.Forms.CheckBox CHK_Damage;
        private System.Windows.Forms.CheckBox CHK_STAB;
        private System.Windows.Forms.NumericUpDown NUD_STAB;
        private System.Windows.Forms.ComboBox CB_Moves;
        private System.Windows.Forms.Label L_Moves;
        private System.Windows.Forms.CheckBox CHK_6PKM;
        private System.Windows.Forms.CheckBox CHK_RandomMegaForm;
        private System.Windows.Forms.CheckBox CHK_GymE4Only;
    }
}