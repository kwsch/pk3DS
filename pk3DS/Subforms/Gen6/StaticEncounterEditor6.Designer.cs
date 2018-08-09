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
            this.B_RandAll = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.L_Gender = new System.Windows.Forms.Label();
            this.CB_Gender = new System.Windows.Forms.ComboBox();
            this.L_Ability = new System.Windows.Forms.Label();
            this.CB_Ability = new System.Windows.Forms.ComboBox();
            this.CHK_IV3 = new System.Windows.Forms.CheckBox();
            this.CHK_ShinyLock = new System.Windows.Forms.CheckBox();
            this.CB_Species = new System.Windows.Forms.ComboBox();
            this.L_Species = new System.Windows.Forms.Label();
            this.L_HeldItem = new System.Windows.Forms.Label();
            this.NUD_Level = new System.Windows.Forms.NumericUpDown();
            this.L_Level = new System.Windows.Forms.Label();
            this.L_Form = new System.Windows.Forms.Label();
            this.CB_HeldItem = new System.Windows.Forms.ComboBox();
            this.NUD_Form = new System.Windows.Forms.NumericUpDown();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.B_ModifyLevel = new System.Windows.Forms.Button();
            this.GB_Tweak = new System.Windows.Forms.GroupBox();
            this.CHK_ReplaceLegend = new System.Windows.Forms.CheckBox();
            this.CHK_RandomAbility = new System.Windows.Forms.CheckBox();
            this.CHK_RemoveShinyLock = new System.Windows.Forms.CheckBox();
            this.CHK_AllowMega = new System.Windows.Forms.CheckBox();
            this.CHK_Item = new System.Windows.Forms.CheckBox();
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
            this.NUD_ForceFullyEvolved = new System.Windows.Forms.NumericUpDown();
            this.CHK_ForceFullyEvolved = new System.Windows.Forms.CheckBox();
            this.NUD_LevelBoost = new System.Windows.Forms.NumericUpDown();
            this.CHK_Level = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.GB_Tweak.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ForceFullyEvolved)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_LevelBoost)).BeginInit();
            this.SuspendLayout();
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Cancel.Location = new System.Drawing.Point(264, 338);
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
            this.B_Save.Location = new System.Drawing.Point(335, 338);
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
            this.LB_Encounters.Size = new System.Drawing.Size(110, 342);
            this.LB_Encounters.TabIndex = 468;
            this.LB_Encounters.SelectedIndexChanged += new System.EventHandler(this.ChangeIndex);
            // 
            // B_RandAll
            // 
            this.B_RandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_RandAll.Location = new System.Drawing.Point(181, 338);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(83, 23);
            this.B_RandAll.TabIndex = 496;
            this.B_RandAll.Text = "Randomize All";
            this.B_RandAll.UseVisualStyleBackColor = true;
            this.B_RandAll.Click += new System.EventHandler(this.B_RandAll_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(128, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(277, 322);
            this.tabControl1.TabIndex = 501;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.L_Gender);
            this.tabPage1.Controls.Add(this.CB_Gender);
            this.tabPage1.Controls.Add(this.L_Ability);
            this.tabPage1.Controls.Add(this.CB_Ability);
            this.tabPage1.Controls.Add(this.CHK_IV3);
            this.tabPage1.Controls.Add(this.CHK_ShinyLock);
            this.tabPage1.Controls.Add(this.CB_Species);
            this.tabPage1.Controls.Add(this.L_Species);
            this.tabPage1.Controls.Add(this.L_HeldItem);
            this.tabPage1.Controls.Add(this.NUD_Level);
            this.tabPage1.Controls.Add(this.L_Level);
            this.tabPage1.Controls.Add(this.L_Form);
            this.tabPage1.Controls.Add(this.CB_HeldItem);
            this.tabPage1.Controls.Add(this.NUD_Form);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(269, 296);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Editor";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // L_Gender
            // 
            this.L_Gender.Location = new System.Drawing.Point(9, 92);
            this.L_Gender.Name = "L_Gender";
            this.L_Gender.Size = new System.Drawing.Size(55, 23);
            this.L_Gender.TabIndex = 526;
            this.L_Gender.Text = "Gender:";
            this.L_Gender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_Gender
            // 
            this.CB_Gender.FormattingEnabled = true;
            this.CB_Gender.Location = new System.Drawing.Point(65, 93);
            this.CB_Gender.Name = "CB_Gender";
            this.CB_Gender.Size = new System.Drawing.Size(136, 21);
            this.CB_Gender.TabIndex = 525;
            // 
            // L_Ability
            // 
            this.L_Ability.Location = new System.Drawing.Point(9, 70);
            this.L_Ability.Name = "L_Ability";
            this.L_Ability.Size = new System.Drawing.Size(55, 23);
            this.L_Ability.TabIndex = 524;
            this.L_Ability.Text = "Ability:";
            this.L_Ability.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_Ability
            // 
            this.CB_Ability.FormattingEnabled = true;
            this.CB_Ability.Location = new System.Drawing.Point(65, 71);
            this.CB_Ability.Name = "CB_Ability";
            this.CB_Ability.Size = new System.Drawing.Size(136, 21);
            this.CB_Ability.TabIndex = 523;
            // 
            // CHK_IV3
            // 
            this.CHK_IV3.AutoSize = true;
            this.CHK_IV3.Location = new System.Drawing.Point(65, 153);
            this.CHK_IV3.Name = "CHK_IV3";
            this.CHK_IV3.Size = new System.Drawing.Size(42, 17);
            this.CHK_IV3.TabIndex = 522;
            this.CHK_IV3.Text = "3IV";
            this.CHK_IV3.UseVisualStyleBackColor = true;
            // 
            // CHK_ShinyLock
            // 
            this.CHK_ShinyLock.AutoSize = true;
            this.CHK_ShinyLock.Location = new System.Drawing.Point(65, 138);
            this.CHK_ShinyLock.Name = "CHK_ShinyLock";
            this.CHK_ShinyLock.Size = new System.Drawing.Size(79, 17);
            this.CHK_ShinyLock.TabIndex = 519;
            this.CHK_ShinyLock.Text = "Shiny Lock";
            this.CHK_ShinyLock.UseVisualStyleBackColor = true;
            // 
            // CB_Species
            // 
            this.CB_Species.FormattingEnabled = true;
            this.CB_Species.Location = new System.Drawing.Point(65, 7);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(136, 21);
            this.CB_Species.TabIndex = 506;
            this.CB_Species.SelectedIndexChanged += new System.EventHandler(this.ChangeSpecies);
            // 
            // L_Species
            // 
            this.L_Species.Location = new System.Drawing.Point(9, 6);
            this.L_Species.Name = "L_Species";
            this.L_Species.Size = new System.Drawing.Size(55, 23);
            this.L_Species.TabIndex = 508;
            this.L_Species.Text = "Species:";
            this.L_Species.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_HeldItem
            // 
            this.L_HeldItem.Location = new System.Drawing.Point(9, 114);
            this.L_HeldItem.Name = "L_HeldItem";
            this.L_HeldItem.Size = new System.Drawing.Size(55, 23);
            this.L_HeldItem.TabIndex = 509;
            this.L_HeldItem.Text = "Held Item:";
            this.L_HeldItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_Level
            // 
            this.NUD_Level.Location = new System.Drawing.Point(65, 29);
            this.NUD_Level.Name = "NUD_Level";
            this.NUD_Level.Size = new System.Drawing.Size(41, 20);
            this.NUD_Level.TabIndex = 510;
            this.NUD_Level.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // L_Level
            // 
            this.L_Level.Location = new System.Drawing.Point(9, 26);
            this.L_Level.Name = "L_Level";
            this.L_Level.Size = new System.Drawing.Size(55, 23);
            this.L_Level.TabIndex = 511;
            this.L_Level.Text = "Level:";
            this.L_Level.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_Form
            // 
            this.L_Form.Location = new System.Drawing.Point(9, 47);
            this.L_Form.Name = "L_Form";
            this.L_Form.Size = new System.Drawing.Size(55, 23);
            this.L_Form.TabIndex = 513;
            this.L_Form.Text = "Form:";
            this.L_Form.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_HeldItem
            // 
            this.CB_HeldItem.FormattingEnabled = true;
            this.CB_HeldItem.Location = new System.Drawing.Point(65, 115);
            this.CB_HeldItem.Name = "CB_HeldItem";
            this.CB_HeldItem.Size = new System.Drawing.Size(136, 21);
            this.CB_HeldItem.TabIndex = 507;
            // 
            // NUD_Form
            // 
            this.NUD_Form.Location = new System.Drawing.Point(65, 50);
            this.NUD_Form.Name = "NUD_Form";
            this.NUD_Form.Size = new System.Drawing.Size(41, 20);
            this.NUD_Form.TabIndex = 512;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.B_ModifyLevel);
            this.tabPage2.Controls.Add(this.GB_Tweak);
            this.tabPage2.Controls.Add(this.NUD_LevelBoost);
            this.tabPage2.Controls.Add(this.CHK_Level);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(269, 296);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Randomizer Options";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // B_ModifyLevel
            // 
            this.B_ModifyLevel.Location = new System.Drawing.Point(189, 16);
            this.B_ModifyLevel.Name = "B_ModifyLevel";
            this.B_ModifyLevel.Size = new System.Drawing.Size(70, 23);
            this.B_ModifyLevel.TabIndex = 502;
            this.B_ModifyLevel.Text = "× Current";
            this.B_ModifyLevel.UseVisualStyleBackColor = true;
            this.B_ModifyLevel.Click += new System.EventHandler(this.ModifyLevels);
            // 
            // GB_Tweak
            // 
            this.GB_Tweak.Controls.Add(this.CHK_ReplaceLegend);
            this.GB_Tweak.Controls.Add(this.CHK_AllowMega);
            this.GB_Tweak.Controls.Add(this.CHK_RandomAbility);
            this.GB_Tweak.Controls.Add(this.CHK_RemoveShinyLock);
            this.GB_Tweak.Controls.Add(this.CHK_Item);
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
            this.GB_Tweak.Controls.Add(this.NUD_ForceFullyEvolved);
            this.GB_Tweak.Controls.Add(this.CHK_ForceFullyEvolved);
            this.GB_Tweak.Location = new System.Drawing.Point(5, 66);
            this.GB_Tweak.Name = "GB_Tweak";
            this.GB_Tweak.Size = new System.Drawing.Size(258, 185);
            this.GB_Tweak.TabIndex = 509;
            this.GB_Tweak.TabStop = false;
            this.GB_Tweak.Text = "Extra Randomization Tweaks";
            // 
            // CHK_ReplaceLegend
            // 
            this.CHK_ReplaceLegend.AutoSize = true;
            this.CHK_ReplaceLegend.Checked = true;
            this.CHK_ReplaceLegend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_ReplaceLegend.Location = new System.Drawing.Point(9, 140);
            this.CHK_ReplaceLegend.Name = "CHK_ReplaceLegend";
            this.CHK_ReplaceLegend.Size = new System.Drawing.Size(242, 17);
            this.CHK_ReplaceLegend.TabIndex = 304;
            this.CHK_ReplaceLegend.Text = "Replace Legendaries with Another Legendary";
            this.CHK_ReplaceLegend.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomAbility
            // 
            this.CHK_RandomAbility.AutoSize = true;
            this.CHK_RandomAbility.Location = new System.Drawing.Point(9, 110);
            this.CHK_RandomAbility.Name = "CHK_RandomAbility";
            this.CHK_RandomAbility.Size = new System.Drawing.Size(183, 17);
            this.CHK_RandomAbility.TabIndex = 303;
            this.CHK_RandomAbility.Text = "Random Abilities (1, 2, or Hidden)";
            this.CHK_RandomAbility.UseVisualStyleBackColor = true;
            // 
            // CHK_RemoveShinyLock
            // 
            this.CHK_RemoveShinyLock.AutoSize = true;
            this.CHK_RemoveShinyLock.Location = new System.Drawing.Point(9, 95);
            this.CHK_RemoveShinyLock.Name = "CHK_RemoveShinyLock";
            this.CHK_RemoveShinyLock.Size = new System.Drawing.Size(127, 17);
            this.CHK_RemoveShinyLock.TabIndex = 297;
            this.CHK_RemoveShinyLock.Text = "Remove Shiny Locks";
            this.CHK_RemoveShinyLock.UseVisualStyleBackColor = true;
            // 
            // CHK_AllowMega
            // 
            this.CHK_AllowMega.AutoSize = true;
            this.CHK_AllowMega.Location = new System.Drawing.Point(9, 125);
            this.CHK_AllowMega.Name = "CHK_AllowMega";
            this.CHK_AllowMega.Size = new System.Drawing.Size(155, 17);
            this.CHK_AllowMega.TabIndex = 296;
            this.CHK_AllowMega.Text = "Allow Random Mega Forms";
            this.CHK_AllowMega.UseVisualStyleBackColor = true;
            // 
            // CHK_Item
            // 
            this.CHK_Item.AutoSize = true;
            this.CHK_Item.Checked = true;
            this.CHK_Item.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_Item.Location = new System.Drawing.Point(9, 80);
            this.CHK_Item.Name = "CHK_Item";
            this.CHK_Item.Size = new System.Drawing.Size(119, 17);
            this.CHK_Item.TabIndex = 295;
            this.CHK_Item.Text = "Random Held Items";
            this.CHK_Item.UseVisualStyleBackColor = true;
            // 
            // L_RandOpt
            // 
            this.L_RandOpt.AutoSize = true;
            this.L_RandOpt.Location = new System.Drawing.Point(6, 17);
            this.L_RandOpt.Name = "L_RandOpt";
            this.L_RandOpt.Size = new System.Drawing.Size(105, 13);
            this.L_RandOpt.TabIndex = 294;
            this.L_RandOpt.Text = "Randomizer Options:";
            // 
            // CHK_BST
            // 
            this.CHK_BST.AutoSize = true;
            this.CHK_BST.Location = new System.Drawing.Point(128, 63);
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
            this.CHK_L.Location = new System.Drawing.Point(128, 35);
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
            this.CHK_G6.Location = new System.Drawing.Point(67, 63);
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
            this.CHK_G4.Location = new System.Drawing.Point(67, 35);
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
            this.CHK_G3.Location = new System.Drawing.Point(9, 63);
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
            this.CHK_G1.Location = new System.Drawing.Point(9, 35);
            this.CHK_G1.Name = "CHK_G1";
            this.CHK_G1.Size = new System.Drawing.Size(55, 17);
            this.CHK_G1.TabIndex = 280;
            this.CHK_G1.Text = "Gen 1";
            this.CHK_G1.UseVisualStyleBackColor = true;
            // 
            // NUD_ForceFullyEvolved
            // 
            this.NUD_ForceFullyEvolved.Location = new System.Drawing.Point(168, 157);
            this.NUD_ForceFullyEvolved.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUD_ForceFullyEvolved.Name = "NUD_ForceFullyEvolved";
            this.NUD_ForceFullyEvolved.Size = new System.Drawing.Size(40, 20);
            this.NUD_ForceFullyEvolved.TabIndex = 516;
            this.NUD_ForceFullyEvolved.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // CHK_ForceFullyEvolved
            // 
            this.CHK_ForceFullyEvolved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CHK_ForceFullyEvolved.AutoSize = true;
            this.CHK_ForceFullyEvolved.Location = new System.Drawing.Point(9, 159);
            this.CHK_ForceFullyEvolved.Name = "CHK_ForceFullyEvolved";
            this.CHK_ForceFullyEvolved.Size = new System.Drawing.Size(160, 17);
            this.CHK_ForceFullyEvolved.TabIndex = 515;
            this.CHK_ForceFullyEvolved.Text = "Force Fully Evolved at Level";
            this.CHK_ForceFullyEvolved.UseVisualStyleBackColor = true;
            // 
            // NUD_LevelBoost
            // 
            this.NUD_LevelBoost.DecimalPlaces = 2;
            this.NUD_LevelBoost.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.NUD_LevelBoost.Location = new System.Drawing.Point(140, 17);
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
            this.CHK_Level.Location = new System.Drawing.Point(9, 18);
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
            this.ClientSize = new System.Drawing.Size(417, 370);
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
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.GB_Tweak.ResumeLayout(false);
            this.GB_Tweak.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ForceFullyEvolved)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_LevelBoost)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button B_Cancel;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.ListBox LB_Encounters;
        private System.Windows.Forms.Button B_RandAll;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
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
        private System.Windows.Forms.CheckBox CHK_Item;
        private System.Windows.Forms.CheckBox CHK_AllowMega;
        private System.Windows.Forms.CheckBox CHK_RemoveShinyLock;
        private System.Windows.Forms.CheckBox CHK_ShinyLock;
        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.Label L_Species;
        private System.Windows.Forms.Label L_HeldItem;
        private System.Windows.Forms.NumericUpDown NUD_Level;
        private System.Windows.Forms.Label L_Level;
        private System.Windows.Forms.Label L_Form;
        private System.Windows.Forms.ComboBox CB_HeldItem;
        private System.Windows.Forms.NumericUpDown NUD_Form;
        private System.Windows.Forms.Button B_ModifyLevel;
        private System.Windows.Forms.CheckBox CHK_IV3;
        private System.Windows.Forms.Label L_Gender;
        private System.Windows.Forms.ComboBox CB_Gender;
        private System.Windows.Forms.Label L_Ability;
        private System.Windows.Forms.ComboBox CB_Ability;
        private System.Windows.Forms.CheckBox CHK_RandomAbility;
        private System.Windows.Forms.CheckBox CHK_ReplaceLegend;
        private System.Windows.Forms.NumericUpDown NUD_ForceFullyEvolved;
        private System.Windows.Forms.CheckBox CHK_ForceFullyEvolved;
    }
}