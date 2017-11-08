namespace pk3DS
{
    partial class StaticEncounterEditor7
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
            this.TC_Tabs = new System.Windows.Forms.TabControl();
            this.Tab_Gifts = new System.Windows.Forms.TabPage();
            this.CB_GHeldItem = new System.Windows.Forms.ComboBox();
            this.L_GHeldItem = new System.Windows.Forms.Label();
            this.CB_GSpecies = new System.Windows.Forms.ComboBox();
            this.L_GForm = new System.Windows.Forms.Label();
            this.NUD_GForm = new System.Windows.Forms.NumericUpDown();
            this.L_GLevel = new System.Windows.Forms.Label();
            this.L_GSpecies = new System.Windows.Forms.Label();
            this.NUD_GLevel = new System.Windows.Forms.NumericUpDown();
            this.LB_Gift = new System.Windows.Forms.ListBox();
            this.Tab_Encounters = new System.Windows.Forms.TabPage();
            this.CHK_ShinyLock = new System.Windows.Forms.CheckBox();
            this.GB_EMoves = new System.Windows.Forms.GroupBox();
            this.CB_EMove3 = new System.Windows.Forms.ComboBox();
            this.CB_EMove2 = new System.Windows.Forms.ComboBox();
            this.CB_EMove1 = new System.Windows.Forms.ComboBox();
            this.CB_EMove0 = new System.Windows.Forms.ComboBox();
            this.CB_EHeldItem = new System.Windows.Forms.ComboBox();
            this.L_EHeldItem = new System.Windows.Forms.Label();
            this.CB_ESpecies = new System.Windows.Forms.ComboBox();
            this.L_EForm = new System.Windows.Forms.Label();
            this.NUD_EForm = new System.Windows.Forms.NumericUpDown();
            this.L_ELevel = new System.Windows.Forms.Label();
            this.L_ESpecies = new System.Windows.Forms.Label();
            this.NUD_ELevel = new System.Windows.Forms.NumericUpDown();
            this.LB_Encounter = new System.Windows.Forms.ListBox();
            this.Tab_Trades = new System.Windows.Forms.TabPage();
            this.CB_TRequest = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.L_TTID = new System.Windows.Forms.Label();
            this.L_TID = new System.Windows.Forms.Label();
            this.NUD_TID = new System.Windows.Forms.NumericUpDown();
            this.CB_THeldItem = new System.Windows.Forms.ComboBox();
            this.L_THeldItem = new System.Windows.Forms.Label();
            this.CB_TSpecies = new System.Windows.Forms.ComboBox();
            this.L_TForm = new System.Windows.Forms.Label();
            this.NUD_TForm = new System.Windows.Forms.NumericUpDown();
            this.L_TLevel = new System.Windows.Forms.Label();
            this.L_TSpecies = new System.Windows.Forms.Label();
            this.NUD_TLevel = new System.Windows.Forms.NumericUpDown();
            this.LB_Trade = new System.Windows.Forms.ListBox();
            this.Tab_Randomizer = new System.Windows.Forms.TabPage();
            this.NUD_LevelBoost = new System.Windows.Forms.NumericUpDown();
            this.CHK_Level = new System.Windows.Forms.CheckBox();
            this.B_RandAll = new System.Windows.Forms.Button();
            this.GB_Tweak = new System.Windows.Forms.GroupBox();
            this.CHK_G7 = new System.Windows.Forms.CheckBox();
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
            this.B_Starters = new System.Windows.Forms.Button();
            this.B_Save = new System.Windows.Forms.Button();
            this.B_Cancel = new System.Windows.Forms.Button();
            this.CHK_G_Lock = new System.Windows.Forms.CheckBox();
            this.NUD_GGender = new System.Windows.Forms.NumericUpDown();
            this.TC_Tabs.SuspendLayout();
            this.Tab_Gifts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GLevel)).BeginInit();
            this.Tab_Encounters.SuspendLayout();
            this.GB_EMoves.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_EForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ELevel)).BeginInit();
            this.Tab_Trades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TLevel)).BeginInit();
            this.Tab_Randomizer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_LevelBoost)).BeginInit();
            this.GB_Tweak.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GGender)).BeginInit();
            this.SuspendLayout();
            // 
            // TC_Tabs
            // 
            this.TC_Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TC_Tabs.Controls.Add(this.Tab_Gifts);
            this.TC_Tabs.Controls.Add(this.Tab_Encounters);
            this.TC_Tabs.Controls.Add(this.Tab_Trades);
            this.TC_Tabs.Controls.Add(this.Tab_Randomizer);
            this.TC_Tabs.Location = new System.Drawing.Point(12, 12);
            this.TC_Tabs.Name = "TC_Tabs";
            this.TC_Tabs.SelectedIndex = 0;
            this.TC_Tabs.Size = new System.Drawing.Size(395, 347);
            this.TC_Tabs.TabIndex = 0;
            // 
            // Tab_Gifts
            // 
            this.Tab_Gifts.Controls.Add(this.NUD_GGender);
            this.Tab_Gifts.Controls.Add(this.CHK_G_Lock);
            this.Tab_Gifts.Controls.Add(this.CB_GHeldItem);
            this.Tab_Gifts.Controls.Add(this.L_GHeldItem);
            this.Tab_Gifts.Controls.Add(this.CB_GSpecies);
            this.Tab_Gifts.Controls.Add(this.L_GForm);
            this.Tab_Gifts.Controls.Add(this.NUD_GForm);
            this.Tab_Gifts.Controls.Add(this.L_GLevel);
            this.Tab_Gifts.Controls.Add(this.L_GSpecies);
            this.Tab_Gifts.Controls.Add(this.NUD_GLevel);
            this.Tab_Gifts.Controls.Add(this.LB_Gift);
            this.Tab_Gifts.Location = new System.Drawing.Point(4, 22);
            this.Tab_Gifts.Name = "Tab_Gifts";
            this.Tab_Gifts.Size = new System.Drawing.Size(387, 321);
            this.Tab_Gifts.TabIndex = 2;
            this.Tab_Gifts.Text = "Gifts";
            this.Tab_Gifts.UseVisualStyleBackColor = true;
            // 
            // CB_GHeldItem
            // 
            this.CB_GHeldItem.FormattingEnabled = true;
            this.CB_GHeldItem.Location = new System.Drawing.Point(230, 87);
            this.CB_GHeldItem.Name = "CB_GHeldItem";
            this.CB_GHeldItem.Size = new System.Drawing.Size(121, 21);
            this.CB_GHeldItem.TabIndex = 8;
            // 
            // L_GHeldItem
            // 
            this.L_GHeldItem.Location = new System.Drawing.Point(124, 85);
            this.L_GHeldItem.Name = "L_GHeldItem";
            this.L_GHeldItem.Size = new System.Drawing.Size(100, 23);
            this.L_GHeldItem.TabIndex = 7;
            this.L_GHeldItem.Text = "Held Item:";
            this.L_GHeldItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_GSpecies
            // 
            this.CB_GSpecies.FormattingEnabled = true;
            this.CB_GSpecies.Location = new System.Drawing.Point(230, 18);
            this.CB_GSpecies.Name = "CB_GSpecies";
            this.CB_GSpecies.Size = new System.Drawing.Size(121, 21);
            this.CB_GSpecies.TabIndex = 6;
            this.CB_GSpecies.SelectedIndexChanged += new System.EventHandler(this.changeSpecies);
            // 
            // L_GForm
            // 
            this.L_GForm.Location = new System.Drawing.Point(124, 62);
            this.L_GForm.Name = "L_GForm";
            this.L_GForm.Size = new System.Drawing.Size(100, 23);
            this.L_GForm.TabIndex = 5;
            this.L_GForm.Text = "Form:";
            this.L_GForm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_GForm
            // 
            this.NUD_GForm.Location = new System.Drawing.Point(230, 65);
            this.NUD_GForm.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NUD_GForm.Name = "NUD_GForm";
            this.NUD_GForm.Size = new System.Drawing.Size(48, 20);
            this.NUD_GForm.TabIndex = 4;
            this.NUD_GForm.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // L_GLevel
            // 
            this.L_GLevel.Location = new System.Drawing.Point(124, 39);
            this.L_GLevel.Name = "L_GLevel";
            this.L_GLevel.Size = new System.Drawing.Size(100, 23);
            this.L_GLevel.TabIndex = 3;
            this.L_GLevel.Text = "Level:";
            this.L_GLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_GSpecies
            // 
            this.L_GSpecies.Location = new System.Drawing.Point(124, 16);
            this.L_GSpecies.Name = "L_GSpecies";
            this.L_GSpecies.Size = new System.Drawing.Size(100, 23);
            this.L_GSpecies.TabIndex = 2;
            this.L_GSpecies.Text = "Species:";
            this.L_GSpecies.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_GLevel
            // 
            this.NUD_GLevel.Location = new System.Drawing.Point(230, 42);
            this.NUD_GLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NUD_GLevel.Name = "NUD_GLevel";
            this.NUD_GLevel.Size = new System.Drawing.Size(48, 20);
            this.NUD_GLevel.TabIndex = 1;
            this.NUD_GLevel.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LB_Gift
            // 
            this.LB_Gift.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_Gift.FormattingEnabled = true;
            this.LB_Gift.Location = new System.Drawing.Point(3, 3);
            this.LB_Gift.Name = "LB_Gift";
            this.LB_Gift.Size = new System.Drawing.Size(115, 316);
            this.LB_Gift.TabIndex = 0;
            this.LB_Gift.SelectedIndexChanged += new System.EventHandler(this.LB_Gift_SelectedIndexChanged);
            // 
            // Tab_Encounters
            // 
            this.Tab_Encounters.Controls.Add(this.CHK_ShinyLock);
            this.Tab_Encounters.Controls.Add(this.GB_EMoves);
            this.Tab_Encounters.Controls.Add(this.CB_EHeldItem);
            this.Tab_Encounters.Controls.Add(this.L_EHeldItem);
            this.Tab_Encounters.Controls.Add(this.CB_ESpecies);
            this.Tab_Encounters.Controls.Add(this.L_EForm);
            this.Tab_Encounters.Controls.Add(this.NUD_EForm);
            this.Tab_Encounters.Controls.Add(this.L_ELevel);
            this.Tab_Encounters.Controls.Add(this.L_ESpecies);
            this.Tab_Encounters.Controls.Add(this.NUD_ELevel);
            this.Tab_Encounters.Controls.Add(this.LB_Encounter);
            this.Tab_Encounters.Location = new System.Drawing.Point(4, 22);
            this.Tab_Encounters.Name = "Tab_Encounters";
            this.Tab_Encounters.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_Encounters.Size = new System.Drawing.Size(387, 321);
            this.Tab_Encounters.TabIndex = 0;
            this.Tab_Encounters.Text = "Encounters";
            this.Tab_Encounters.UseVisualStyleBackColor = true;
            // 
            // CHK_ShinyLock
            // 
            this.CHK_ShinyLock.AutoSize = true;
            this.CHK_ShinyLock.Location = new System.Drawing.Point(218, 232);
            this.CHK_ShinyLock.Name = "CHK_ShinyLock";
            this.CHK_ShinyLock.Size = new System.Drawing.Size(79, 17);
            this.CHK_ShinyLock.TabIndex = 18;
            this.CHK_ShinyLock.Text = "Shiny Lock";
            this.CHK_ShinyLock.UseVisualStyleBackColor = true;
            // 
            // GB_EMoves
            // 
            this.GB_EMoves.Controls.Add(this.CB_EMove3);
            this.GB_EMoves.Controls.Add(this.CB_EMove2);
            this.GB_EMoves.Controls.Add(this.CB_EMove1);
            this.GB_EMoves.Controls.Add(this.CB_EMove0);
            this.GB_EMoves.Location = new System.Drawing.Point(218, 114);
            this.GB_EMoves.Name = "GB_EMoves";
            this.GB_EMoves.Size = new System.Drawing.Size(133, 112);
            this.GB_EMoves.TabIndex = 17;
            this.GB_EMoves.TabStop = false;
            this.GB_EMoves.Text = "Moves";
            // 
            // CB_EMove3
            // 
            this.CB_EMove3.FormattingEnabled = true;
            this.CB_EMove3.Location = new System.Drawing.Point(6, 85);
            this.CB_EMove3.Name = "CB_EMove3";
            this.CB_EMove3.Size = new System.Drawing.Size(121, 21);
            this.CB_EMove3.TabIndex = 20;
            // 
            // CB_EMove2
            // 
            this.CB_EMove2.FormattingEnabled = true;
            this.CB_EMove2.Location = new System.Drawing.Point(6, 63);
            this.CB_EMove2.Name = "CB_EMove2";
            this.CB_EMove2.Size = new System.Drawing.Size(121, 21);
            this.CB_EMove2.TabIndex = 19;
            // 
            // CB_EMove1
            // 
            this.CB_EMove1.FormattingEnabled = true;
            this.CB_EMove1.Location = new System.Drawing.Point(6, 41);
            this.CB_EMove1.Name = "CB_EMove1";
            this.CB_EMove1.Size = new System.Drawing.Size(121, 21);
            this.CB_EMove1.TabIndex = 18;
            // 
            // CB_EMove0
            // 
            this.CB_EMove0.FormattingEnabled = true;
            this.CB_EMove0.Location = new System.Drawing.Point(6, 19);
            this.CB_EMove0.Name = "CB_EMove0";
            this.CB_EMove0.Size = new System.Drawing.Size(121, 21);
            this.CB_EMove0.TabIndex = 17;
            // 
            // CB_EHeldItem
            // 
            this.CB_EHeldItem.FormattingEnabled = true;
            this.CB_EHeldItem.Location = new System.Drawing.Point(230, 87);
            this.CB_EHeldItem.Name = "CB_EHeldItem";
            this.CB_EHeldItem.Size = new System.Drawing.Size(121, 21);
            this.CB_EHeldItem.TabIndex = 16;
            // 
            // L_EHeldItem
            // 
            this.L_EHeldItem.Location = new System.Drawing.Point(124, 85);
            this.L_EHeldItem.Name = "L_EHeldItem";
            this.L_EHeldItem.Size = new System.Drawing.Size(100, 23);
            this.L_EHeldItem.TabIndex = 15;
            this.L_EHeldItem.Text = "Held Item:";
            this.L_EHeldItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_ESpecies
            // 
            this.CB_ESpecies.FormattingEnabled = true;
            this.CB_ESpecies.Location = new System.Drawing.Point(230, 18);
            this.CB_ESpecies.Name = "CB_ESpecies";
            this.CB_ESpecies.Size = new System.Drawing.Size(121, 21);
            this.CB_ESpecies.TabIndex = 14;
            this.CB_ESpecies.SelectedIndexChanged += new System.EventHandler(this.changeSpecies);
            // 
            // L_EForm
            // 
            this.L_EForm.Location = new System.Drawing.Point(124, 62);
            this.L_EForm.Name = "L_EForm";
            this.L_EForm.Size = new System.Drawing.Size(100, 23);
            this.L_EForm.TabIndex = 13;
            this.L_EForm.Text = "Form:";
            this.L_EForm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_EForm
            // 
            this.NUD_EForm.Location = new System.Drawing.Point(230, 65);
            this.NUD_EForm.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NUD_EForm.Name = "NUD_EForm";
            this.NUD_EForm.Size = new System.Drawing.Size(48, 20);
            this.NUD_EForm.TabIndex = 12;
            this.NUD_EForm.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // L_ELevel
            // 
            this.L_ELevel.Location = new System.Drawing.Point(124, 39);
            this.L_ELevel.Name = "L_ELevel";
            this.L_ELevel.Size = new System.Drawing.Size(100, 23);
            this.L_ELevel.TabIndex = 11;
            this.L_ELevel.Text = "Level:";
            this.L_ELevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_ESpecies
            // 
            this.L_ESpecies.Location = new System.Drawing.Point(124, 16);
            this.L_ESpecies.Name = "L_ESpecies";
            this.L_ESpecies.Size = new System.Drawing.Size(100, 23);
            this.L_ESpecies.TabIndex = 10;
            this.L_ESpecies.Text = "Species:";
            this.L_ESpecies.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_ELevel
            // 
            this.NUD_ELevel.Location = new System.Drawing.Point(230, 42);
            this.NUD_ELevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NUD_ELevel.Name = "NUD_ELevel";
            this.NUD_ELevel.Size = new System.Drawing.Size(48, 20);
            this.NUD_ELevel.TabIndex = 9;
            this.NUD_ELevel.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LB_Encounter
            // 
            this.LB_Encounter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_Encounter.FormattingEnabled = true;
            this.LB_Encounter.Location = new System.Drawing.Point(3, 3);
            this.LB_Encounter.Name = "LB_Encounter";
            this.LB_Encounter.Size = new System.Drawing.Size(115, 316);
            this.LB_Encounter.TabIndex = 1;
            this.LB_Encounter.SelectedIndexChanged += new System.EventHandler(this.LB_Encounter_SelectedIndexChanged);
            // 
            // Tab_Trades
            // 
            this.Tab_Trades.Controls.Add(this.CB_TRequest);
            this.Tab_Trades.Controls.Add(this.label1);
            this.Tab_Trades.Controls.Add(this.L_TTID);
            this.Tab_Trades.Controls.Add(this.L_TID);
            this.Tab_Trades.Controls.Add(this.NUD_TID);
            this.Tab_Trades.Controls.Add(this.CB_THeldItem);
            this.Tab_Trades.Controls.Add(this.L_THeldItem);
            this.Tab_Trades.Controls.Add(this.CB_TSpecies);
            this.Tab_Trades.Controls.Add(this.L_TForm);
            this.Tab_Trades.Controls.Add(this.NUD_TForm);
            this.Tab_Trades.Controls.Add(this.L_TLevel);
            this.Tab_Trades.Controls.Add(this.L_TSpecies);
            this.Tab_Trades.Controls.Add(this.NUD_TLevel);
            this.Tab_Trades.Controls.Add(this.LB_Trade);
            this.Tab_Trades.Location = new System.Drawing.Point(4, 22);
            this.Tab_Trades.Name = "Tab_Trades";
            this.Tab_Trades.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_Trades.Size = new System.Drawing.Size(387, 321);
            this.Tab_Trades.TabIndex = 1;
            this.Tab_Trades.Text = "Trades";
            this.Tab_Trades.UseVisualStyleBackColor = true;
            // 
            // CB_TRequest
            // 
            this.CB_TRequest.FormattingEnabled = true;
            this.CB_TRequest.Location = new System.Drawing.Point(230, 137);
            this.CB_TRequest.Name = "CB_TRequest";
            this.CB_TRequest.Size = new System.Drawing.Size(121, 21);
            this.CB_TRequest.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(121, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 23);
            this.label1.TabIndex = 28;
            this.label1.Text = "Requested Species:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_TTID
            // 
            this.L_TTID.AutoSize = true;
            this.L_TTID.Location = new System.Drawing.Point(268, 113);
            this.L_TTID.Name = "L_TTID";
            this.L_TTID.Size = new System.Drawing.Size(28, 13);
            this.L_TTID.TabIndex = 27;
            this.L_TTID.Text = "TID:";
            this.L_TTID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // L_TID
            // 
            this.L_TID.Location = new System.Drawing.Point(124, 108);
            this.L_TID.Name = "L_TID";
            this.L_TID.Size = new System.Drawing.Size(50, 23);
            this.L_TID.TabIndex = 26;
            this.L_TID.Text = "ID:";
            this.L_TID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_TID
            // 
            this.NUD_TID.Location = new System.Drawing.Point(180, 111);
            this.NUD_TID.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.NUD_TID.Name = "NUD_TID";
            this.NUD_TID.Size = new System.Drawing.Size(82, 20);
            this.NUD_TID.TabIndex = 25;
            this.NUD_TID.Value = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.NUD_TID.ValueChanged += new System.EventHandler(this.changeTID);
            // 
            // CB_THeldItem
            // 
            this.CB_THeldItem.FormattingEnabled = true;
            this.CB_THeldItem.Location = new System.Drawing.Point(230, 87);
            this.CB_THeldItem.Name = "CB_THeldItem";
            this.CB_THeldItem.Size = new System.Drawing.Size(121, 21);
            this.CB_THeldItem.TabIndex = 24;
            // 
            // L_THeldItem
            // 
            this.L_THeldItem.Location = new System.Drawing.Point(124, 85);
            this.L_THeldItem.Name = "L_THeldItem";
            this.L_THeldItem.Size = new System.Drawing.Size(100, 23);
            this.L_THeldItem.TabIndex = 23;
            this.L_THeldItem.Text = "Held Item:";
            this.L_THeldItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_TSpecies
            // 
            this.CB_TSpecies.FormattingEnabled = true;
            this.CB_TSpecies.Location = new System.Drawing.Point(230, 18);
            this.CB_TSpecies.Name = "CB_TSpecies";
            this.CB_TSpecies.Size = new System.Drawing.Size(121, 21);
            this.CB_TSpecies.TabIndex = 22;
            this.CB_TSpecies.SelectedIndexChanged += new System.EventHandler(this.changeSpecies);
            // 
            // L_TForm
            // 
            this.L_TForm.Location = new System.Drawing.Point(124, 62);
            this.L_TForm.Name = "L_TForm";
            this.L_TForm.Size = new System.Drawing.Size(100, 23);
            this.L_TForm.TabIndex = 21;
            this.L_TForm.Text = "Form:";
            this.L_TForm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_TForm
            // 
            this.NUD_TForm.Location = new System.Drawing.Point(230, 65);
            this.NUD_TForm.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NUD_TForm.Name = "NUD_TForm";
            this.NUD_TForm.Size = new System.Drawing.Size(48, 20);
            this.NUD_TForm.TabIndex = 20;
            this.NUD_TForm.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // L_TLevel
            // 
            this.L_TLevel.Location = new System.Drawing.Point(124, 39);
            this.L_TLevel.Name = "L_TLevel";
            this.L_TLevel.Size = new System.Drawing.Size(100, 23);
            this.L_TLevel.TabIndex = 19;
            this.L_TLevel.Text = "Level:";
            this.L_TLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_TSpecies
            // 
            this.L_TSpecies.Location = new System.Drawing.Point(124, 16);
            this.L_TSpecies.Name = "L_TSpecies";
            this.L_TSpecies.Size = new System.Drawing.Size(100, 23);
            this.L_TSpecies.TabIndex = 18;
            this.L_TSpecies.Text = "Species:";
            this.L_TSpecies.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_TLevel
            // 
            this.NUD_TLevel.Location = new System.Drawing.Point(230, 42);
            this.NUD_TLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NUD_TLevel.Name = "NUD_TLevel";
            this.NUD_TLevel.Size = new System.Drawing.Size(48, 20);
            this.NUD_TLevel.TabIndex = 17;
            this.NUD_TLevel.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LB_Trade
            // 
            this.LB_Trade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_Trade.FormattingEnabled = true;
            this.LB_Trade.Location = new System.Drawing.Point(3, 3);
            this.LB_Trade.Name = "LB_Trade";
            this.LB_Trade.Size = new System.Drawing.Size(115, 316);
            this.LB_Trade.TabIndex = 2;
            this.LB_Trade.SelectedIndexChanged += new System.EventHandler(this.LB_Trade_SelectedIndexChanged);
            // 
            // Tab_Randomizer
            // 
            this.Tab_Randomizer.Controls.Add(this.NUD_LevelBoost);
            this.Tab_Randomizer.Controls.Add(this.CHK_Level);
            this.Tab_Randomizer.Controls.Add(this.B_RandAll);
            this.Tab_Randomizer.Controls.Add(this.GB_Tweak);
            this.Tab_Randomizer.Controls.Add(this.B_Starters);
            this.Tab_Randomizer.Location = new System.Drawing.Point(4, 22);
            this.Tab_Randomizer.Name = "Tab_Randomizer";
            this.Tab_Randomizer.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_Randomizer.Size = new System.Drawing.Size(387, 321);
            this.Tab_Randomizer.TabIndex = 3;
            this.Tab_Randomizer.Text = "Randomizer Options";
            this.Tab_Randomizer.UseVisualStyleBackColor = true;
            // 
            // NUD_LevelBoost
            // 
            this.NUD_LevelBoost.DecimalPlaces = 2;
            this.NUD_LevelBoost.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.NUD_LevelBoost.Location = new System.Drawing.Point(231, 74);
            this.NUD_LevelBoost.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NUD_LevelBoost.Name = "NUD_LevelBoost";
            this.NUD_LevelBoost.Size = new System.Drawing.Size(43, 20);
            this.NUD_LevelBoost.TabIndex = 511;
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
            this.CHK_Level.Location = new System.Drawing.Point(99, 75);
            this.CHK_Level.Name = "CHK_Level";
            this.CHK_Level.Size = new System.Drawing.Size(130, 17);
            this.CHK_Level.TabIndex = 510;
            this.CHK_Level.Text = "Multiply PKM Level by";
            this.CHK_Level.UseVisualStyleBackColor = true;
            // 
            // B_RandAll
            // 
            this.B_RandAll.Location = new System.Drawing.Point(132, 243);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(122, 23);
            this.B_RandAll.TabIndex = 509;
            this.B_RandAll.Text = "Randomize All";
            this.B_RandAll.UseVisualStyleBackColor = true;
            this.B_RandAll.Click += new System.EventHandler(this.B_RandAll_Click);
            // 
            // GB_Tweak
            // 
            this.GB_Tweak.Controls.Add(this.CHK_G7);
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
            this.GB_Tweak.Location = new System.Drawing.Point(52, 100);
            this.GB_Tweak.Name = "GB_Tweak";
            this.GB_Tweak.Size = new System.Drawing.Size(258, 100);
            this.GB_Tweak.TabIndex = 508;
            this.GB_Tweak.TabStop = false;
            this.GB_Tweak.Text = "Extra Randomization Tweaks";
            // 
            // CHK_G7
            // 
            this.CHK_G7.AutoSize = true;
            this.CHK_G7.Checked = true;
            this.CHK_G7.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_G7.Location = new System.Drawing.Point(9, 79);
            this.CHK_G7.Name = "CHK_G7";
            this.CHK_G7.Size = new System.Drawing.Size(55, 17);
            this.CHK_G7.TabIndex = 296;
            this.CHK_G7.Text = "Gen 7";
            this.CHK_G7.UseVisualStyleBackColor = true;
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
            // B_Starters
            // 
            this.B_Starters.Location = new System.Drawing.Point(132, 272);
            this.B_Starters.Name = "B_Starters";
            this.B_Starters.Size = new System.Drawing.Size(122, 23);
            this.B_Starters.TabIndex = 9;
            this.B_Starters.Text = "Randomize Starters";
            this.B_Starters.UseVisualStyleBackColor = true;
            this.B_Starters.Click += new System.EventHandler(this.B_Starters_Click);
            // 
            // B_Save
            // 
            this.B_Save.Location = new System.Drawing.Point(328, 365);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(75, 23);
            this.B_Save.TabIndex = 1;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // B_Cancel
            // 
            this.B_Cancel.Location = new System.Drawing.Point(247, 365);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(75, 23);
            this.B_Cancel.TabIndex = 2;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.UseVisualStyleBackColor = true;
            this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
            // 
            // CHK_G_Lock
            // 
            this.CHK_G_Lock.AutoSize = true;
            this.CHK_G_Lock.Location = new System.Drawing.Point(230, 114);
            this.CHK_G_Lock.Name = "CHK_G_Lock";
            this.CHK_G_Lock.Size = new System.Drawing.Size(79, 17);
            this.CHK_G_Lock.TabIndex = 19;
            this.CHK_G_Lock.Text = "Shiny Lock";
            this.CHK_G_Lock.UseVisualStyleBackColor = true;
            // 
            // NUD_GGender
            // 
            this.NUD_GGender.Location = new System.Drawing.Point(230, 137);
            this.NUD_GGender.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NUD_GGender.Name = "NUD_GGender";
            this.NUD_GGender.Size = new System.Drawing.Size(48, 20);
            this.NUD_GGender.TabIndex = 20;
            this.NUD_GGender.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // StaticEncounterEditor7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 395);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.TC_Tabs);
            this.Name = "StaticEncounterEditor7";
            this.Text = "StaticEncounterEditor7";
            this.TC_Tabs.ResumeLayout(false);
            this.Tab_Gifts.ResumeLayout(false);
            this.Tab_Gifts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GLevel)).EndInit();
            this.Tab_Encounters.ResumeLayout(false);
            this.Tab_Encounters.PerformLayout();
            this.GB_EMoves.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_EForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ELevel)).EndInit();
            this.Tab_Trades.ResumeLayout(false);
            this.Tab_Trades.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TLevel)).EndInit();
            this.Tab_Randomizer.ResumeLayout(false);
            this.Tab_Randomizer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_LevelBoost)).EndInit();
            this.GB_Tweak.ResumeLayout(false);
            this.GB_Tweak.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_GGender)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TC_Tabs;
        private System.Windows.Forms.TabPage Tab_Encounters;
        private System.Windows.Forms.TabPage Tab_Trades;
        private System.Windows.Forms.TabPage Tab_Gifts;
        private System.Windows.Forms.ListBox LB_Gift;
        private System.Windows.Forms.ListBox LB_Encounter;
        private System.Windows.Forms.ListBox LB_Trade;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.NumericUpDown NUD_GLevel;
        private System.Windows.Forms.Label L_GSpecies;
        private System.Windows.Forms.Label L_GForm;
        private System.Windows.Forms.NumericUpDown NUD_GForm;
        private System.Windows.Forms.Label L_GLevel;
        private System.Windows.Forms.ComboBox CB_GSpecies;
        private System.Windows.Forms.ComboBox CB_GHeldItem;
        private System.Windows.Forms.Label L_GHeldItem;
        private System.Windows.Forms.ComboBox CB_EHeldItem;
        private System.Windows.Forms.Label L_EHeldItem;
        private System.Windows.Forms.ComboBox CB_ESpecies;
        private System.Windows.Forms.Label L_EForm;
        private System.Windows.Forms.NumericUpDown NUD_EForm;
        private System.Windows.Forms.Label L_ELevel;
        private System.Windows.Forms.Label L_ESpecies;
        private System.Windows.Forms.NumericUpDown NUD_ELevel;
        private System.Windows.Forms.ComboBox CB_THeldItem;
        private System.Windows.Forms.Label L_THeldItem;
        private System.Windows.Forms.ComboBox CB_TSpecies;
        private System.Windows.Forms.Label L_TForm;
        private System.Windows.Forms.NumericUpDown NUD_TForm;
        private System.Windows.Forms.Label L_TLevel;
        private System.Windows.Forms.Label L_TSpecies;
        private System.Windows.Forms.NumericUpDown NUD_TLevel;
        private System.Windows.Forms.Button B_Starters;
        private System.Windows.Forms.GroupBox GB_EMoves;
        private System.Windows.Forms.ComboBox CB_EMove3;
        private System.Windows.Forms.ComboBox CB_EMove2;
        private System.Windows.Forms.ComboBox CB_EMove1;
        private System.Windows.Forms.ComboBox CB_EMove0;
        private System.Windows.Forms.NumericUpDown NUD_TID;
        private System.Windows.Forms.Label L_TID;
        private System.Windows.Forms.Label L_TTID;
        private System.Windows.Forms.ComboBox CB_TRequest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage Tab_Randomizer;
        private System.Windows.Forms.GroupBox GB_Tweak;
        private System.Windows.Forms.CheckBox CHK_G7;
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
        private System.Windows.Forms.Button B_RandAll;
        private System.Windows.Forms.Button B_Cancel;
        private System.Windows.Forms.NumericUpDown NUD_LevelBoost;
        private System.Windows.Forms.CheckBox CHK_Level;
        private System.Windows.Forms.CheckBox CHK_ShinyLock;
        private System.Windows.Forms.NumericUpDown NUD_GGender;
        private System.Windows.Forms.CheckBox CHK_G_Lock;
    }
}