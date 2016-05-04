namespace pk3DS
{
    sealed partial class Main
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
            this.TB_Path = new System.Windows.Forms.TextBox();
            this.GB_RomFS = new System.Windows.Forms.GroupBox();
            this.B_TitleScreen = new System.Windows.Forms.Button();
            this.B_Maison = new System.Windows.Forms.Button();
            this.B_EggMove = new System.Windows.Forms.Button();
            this.B_LevelUp = new System.Windows.Forms.Button();
            this.B_StoryText = new System.Windows.Forms.Button();
            this.B_Item = new System.Windows.Forms.Button();
            this.B_Move = new System.Windows.Forms.Button();
            this.B_Evolution = new System.Windows.Forms.Button();
            this.B_Personal = new System.Windows.Forms.Button();
            this.B_MegaEvo = new System.Windows.Forms.Button();
            this.B_Wild = new System.Windows.Forms.Button();
            this.B_Trainer = new System.Windows.Forms.Button();
            this.B_GameText = new System.Windows.Forms.Button();
            this.L_Game = new System.Windows.Forms.Label();
            this.pBar1 = new System.Windows.Forms.ProgressBar();
            this.GB_ExeFS = new System.Windows.Forms.GroupBox();
            this.B_OPower = new System.Windows.Forms.Button();
            this.B_Pickup = new System.Windows.Forms.Button();
            this.B_Mart = new System.Windows.Forms.Button();
            this.B_MoveTutor = new System.Windows.Forms.Button();
            this.B_TMHM = new System.Windows.Forms.Button();
            this.RTB_Status = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Restore = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Misc = new System.Windows.Forms.ToolStripMenuItem();
            this.unPackBCLIMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_BLZ = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_LZ11 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Shuffler = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Rebuild = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_RomFS = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_ExeFS = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_CRO = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_3DS = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Patch = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_SMDH = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Options = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Language = new System.Windows.Forms.ToolStripMenuItem();
            this.CB_Lang = new System.Windows.Forms.ToolStripComboBox();
            this.Menu_About = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_GARCs = new System.Windows.Forms.ToolStripMenuItem();
            this.GB_CRO = new System.Windows.Forms.GroupBox();
            this.B_Gift = new System.Windows.Forms.Button();
            this.B_TypeChart = new System.Windows.Forms.Button();
            this.B_Starter = new System.Windows.Forms.Button();
            this.B_Static = new System.Windows.Forms.Button();
            this.GB_RomFS.SuspendLayout();
            this.GB_ExeFS.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.GB_CRO.SuspendLayout();
            this.SuspendLayout();
            // 
            // TB_Path
            // 
            this.TB_Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Path.Location = new System.Drawing.Point(158, 3);
            this.TB_Path.Name = "TB_Path";
            this.TB_Path.ReadOnly = true;
            this.TB_Path.Size = new System.Drawing.Size(284, 20);
            this.TB_Path.TabIndex = 1;
            // 
            // GB_RomFS
            // 
            this.GB_RomFS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_RomFS.Controls.Add(this.B_TitleScreen);
            this.GB_RomFS.Controls.Add(this.B_Maison);
            this.GB_RomFS.Controls.Add(this.B_EggMove);
            this.GB_RomFS.Controls.Add(this.B_LevelUp);
            this.GB_RomFS.Controls.Add(this.B_StoryText);
            this.GB_RomFS.Controls.Add(this.B_Item);
            this.GB_RomFS.Controls.Add(this.B_Move);
            this.GB_RomFS.Controls.Add(this.B_Evolution);
            this.GB_RomFS.Controls.Add(this.B_Personal);
            this.GB_RomFS.Controls.Add(this.B_MegaEvo);
            this.GB_RomFS.Controls.Add(this.B_Wild);
            this.GB_RomFS.Controls.Add(this.B_Trainer);
            this.GB_RomFS.Controls.Add(this.B_GameText);
            this.GB_RomFS.Enabled = false;
            this.GB_RomFS.Location = new System.Drawing.Point(12, 30);
            this.GB_RomFS.Name = "GB_RomFS";
            this.GB_RomFS.Size = new System.Drawing.Size(430, 110);
            this.GB_RomFS.TabIndex = 5;
            this.GB_RomFS.TabStop = false;
            this.GB_RomFS.Text = "RomFS Editing Tools";
            // 
            // B_TitleScreen
            // 
            this.B_TitleScreen.Location = new System.Drawing.Point(350, 77);
            this.B_TitleScreen.Name = "B_TitleScreen";
            this.B_TitleScreen.Size = new System.Drawing.Size(74, 23);
            this.B_TitleScreen.TabIndex = 12;
            this.B_TitleScreen.Text = "Title Screen";
            this.B_TitleScreen.UseVisualStyleBackColor = true;
            this.B_TitleScreen.Click += new System.EventHandler(this.B_TitleScreen_Click);
            // 
            // B_Maison
            // 
            this.B_Maison.Location = new System.Drawing.Point(92, 77);
            this.B_Maison.Name = "B_Maison";
            this.B_Maison.Size = new System.Drawing.Size(80, 23);
            this.B_Maison.TabIndex = 5;
            this.B_Maison.Text = "Maison";
            this.B_Maison.UseVisualStyleBackColor = true;
            this.B_Maison.Click += new System.EventHandler(this.B_Maison_Click);
            // 
            // B_EggMove
            // 
            this.B_EggMove.Location = new System.Drawing.Point(324, 48);
            this.B_EggMove.Name = "B_EggMove";
            this.B_EggMove.Size = new System.Drawing.Size(100, 23);
            this.B_EggMove.TabIndex = 10;
            this.B_EggMove.Text = "Egg Moves";
            this.B_EggMove.UseVisualStyleBackColor = true;
            this.B_EggMove.Click += new System.EventHandler(this.B_EggMove_Click);
            // 
            // B_LevelUp
            // 
            this.B_LevelUp.Location = new System.Drawing.Point(324, 19);
            this.B_LevelUp.Name = "B_LevelUp";
            this.B_LevelUp.Size = new System.Drawing.Size(100, 23);
            this.B_LevelUp.TabIndex = 9;
            this.B_LevelUp.Text = "Level Up Moves";
            this.B_LevelUp.UseVisualStyleBackColor = true;
            this.B_LevelUp.Click += new System.EventHandler(this.B_LevelUp_Click);
            // 
            // B_StoryText
            // 
            this.B_StoryText.Location = new System.Drawing.Point(6, 48);
            this.B_StoryText.Name = "B_StoryText";
            this.B_StoryText.Size = new System.Drawing.Size(100, 23);
            this.B_StoryText.TabIndex = 1;
            this.B_StoryText.Text = "Story Text";
            this.B_StoryText.UseVisualStyleBackColor = true;
            this.B_StoryText.Click += new System.EventHandler(this.B_StoryText_Click);
            // 
            // B_Item
            // 
            this.B_Item.Location = new System.Drawing.Point(178, 77);
            this.B_Item.Name = "B_Item";
            this.B_Item.Size = new System.Drawing.Size(80, 23);
            this.B_Item.TabIndex = 8;
            this.B_Item.Text = "Item Stats";
            this.B_Item.UseVisualStyleBackColor = true;
            this.B_Item.Click += new System.EventHandler(this.B_Item_Click);
            // 
            // B_Move
            // 
            this.B_Move.Location = new System.Drawing.Point(264, 77);
            this.B_Move.Name = "B_Move";
            this.B_Move.Size = new System.Drawing.Size(80, 23);
            this.B_Move.TabIndex = 11;
            this.B_Move.Text = "Move Stats";
            this.B_Move.UseVisualStyleBackColor = true;
            this.B_Move.Click += new System.EventHandler(this.B_Move_Click);
            // 
            // B_Evolution
            // 
            this.B_Evolution.Location = new System.Drawing.Point(218, 19);
            this.B_Evolution.Name = "B_Evolution";
            this.B_Evolution.Size = new System.Drawing.Size(100, 23);
            this.B_Evolution.TabIndex = 6;
            this.B_Evolution.Text = "Evolutions";
            this.B_Evolution.UseVisualStyleBackColor = true;
            this.B_Evolution.Click += new System.EventHandler(this.B_Evolution_Click);
            // 
            // B_Personal
            // 
            this.B_Personal.Location = new System.Drawing.Point(112, 19);
            this.B_Personal.Name = "B_Personal";
            this.B_Personal.Size = new System.Drawing.Size(100, 23);
            this.B_Personal.TabIndex = 3;
            this.B_Personal.Text = "Personal Stats";
            this.B_Personal.UseVisualStyleBackColor = true;
            this.B_Personal.Click += new System.EventHandler(this.B_Personal_Click);
            // 
            // B_MegaEvo
            // 
            this.B_MegaEvo.Location = new System.Drawing.Point(218, 48);
            this.B_MegaEvo.Name = "B_MegaEvo";
            this.B_MegaEvo.Size = new System.Drawing.Size(100, 23);
            this.B_MegaEvo.TabIndex = 7;
            this.B_MegaEvo.Text = "Mega Evolutions";
            this.B_MegaEvo.UseVisualStyleBackColor = true;
            this.B_MegaEvo.Click += new System.EventHandler(this.B_MegaEvo_Click);
            // 
            // B_Wild
            // 
            this.B_Wild.Location = new System.Drawing.Point(112, 48);
            this.B_Wild.Name = "B_Wild";
            this.B_Wild.Size = new System.Drawing.Size(100, 23);
            this.B_Wild.TabIndex = 4;
            this.B_Wild.Text = "Wild Encounters";
            this.B_Wild.UseVisualStyleBackColor = true;
            this.B_Wild.Click += new System.EventHandler(this.B_Wild_Click);
            // 
            // B_Trainer
            // 
            this.B_Trainer.Location = new System.Drawing.Point(6, 77);
            this.B_Trainer.Name = "B_Trainer";
            this.B_Trainer.Size = new System.Drawing.Size(80, 23);
            this.B_Trainer.TabIndex = 2;
            this.B_Trainer.Text = "Trainer";
            this.B_Trainer.UseVisualStyleBackColor = true;
            this.B_Trainer.Click += new System.EventHandler(this.B_Trainer_Click);
            // 
            // B_GameText
            // 
            this.B_GameText.Location = new System.Drawing.Point(6, 19);
            this.B_GameText.Name = "B_GameText";
            this.B_GameText.Size = new System.Drawing.Size(100, 23);
            this.B_GameText.TabIndex = 0;
            this.B_GameText.Text = "Game Text";
            this.B_GameText.UseVisualStyleBackColor = true;
            this.B_GameText.Click += new System.EventHandler(this.B_GameText_Click);
            // 
            // L_Game
            // 
            this.L_Game.AutoSize = true;
            this.L_Game.ForeColor = System.Drawing.Color.Red;
            this.L_Game.Location = new System.Drawing.Point(157, 23);
            this.L_Game.Name = "L_Game";
            this.L_Game.Size = new System.Drawing.Size(91, 13);
            this.L_Game.TabIndex = 2;
            this.L_Game.Text = "No Game Loaded";
            // 
            // pBar1
            // 
            this.pBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBar1.Location = new System.Drawing.Point(12, 257);
            this.pBar1.Name = "pBar1";
            this.pBar1.Size = new System.Drawing.Size(430, 14);
            this.pBar1.TabIndex = 6;
            // 
            // GB_ExeFS
            // 
            this.GB_ExeFS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_ExeFS.Controls.Add(this.B_OPower);
            this.GB_ExeFS.Controls.Add(this.B_Pickup);
            this.GB_ExeFS.Controls.Add(this.B_Mart);
            this.GB_ExeFS.Controls.Add(this.B_MoveTutor);
            this.GB_ExeFS.Controls.Add(this.B_TMHM);
            this.GB_ExeFS.Enabled = false;
            this.GB_ExeFS.Location = new System.Drawing.Point(12, 146);
            this.GB_ExeFS.Name = "GB_ExeFS";
            this.GB_ExeFS.Size = new System.Drawing.Size(430, 50);
            this.GB_ExeFS.TabIndex = 6;
            this.GB_ExeFS.TabStop = false;
            this.GB_ExeFS.Text = "ExeFS Editing Tools";
            // 
            // B_OPower
            // 
            this.B_OPower.Location = new System.Drawing.Point(350, 19);
            this.B_OPower.Name = "B_OPower";
            this.B_OPower.Size = new System.Drawing.Size(74, 23);
            this.B_OPower.TabIndex = 4;
            this.B_OPower.Text = "O-Power";
            this.B_OPower.UseVisualStyleBackColor = true;
            this.B_OPower.Click += new System.EventHandler(this.B_OPower_Click);
            // 
            // B_Pickup
            // 
            this.B_Pickup.Location = new System.Drawing.Point(6, 19);
            this.B_Pickup.Name = "B_Pickup";
            this.B_Pickup.Size = new System.Drawing.Size(80, 23);
            this.B_Pickup.TabIndex = 0;
            this.B_Pickup.Text = "Pickup";
            this.B_Pickup.UseVisualStyleBackColor = true;
            this.B_Pickup.Click += new System.EventHandler(this.B_Pickup_Click);
            // 
            // B_Mart
            // 
            this.B_Mart.Location = new System.Drawing.Point(178, 19);
            this.B_Mart.Name = "B_Mart";
            this.B_Mart.Size = new System.Drawing.Size(80, 23);
            this.B_Mart.TabIndex = 2;
            this.B_Mart.Text = "Mart";
            this.B_Mart.UseVisualStyleBackColor = true;
            this.B_Mart.Click += new System.EventHandler(this.B_Mart_Click);
            // 
            // B_MoveTutor
            // 
            this.B_MoveTutor.Location = new System.Drawing.Point(264, 19);
            this.B_MoveTutor.Name = "B_MoveTutor";
            this.B_MoveTutor.Size = new System.Drawing.Size(80, 23);
            this.B_MoveTutor.TabIndex = 3;
            this.B_MoveTutor.Text = "Move Tutor";
            this.B_MoveTutor.UseVisualStyleBackColor = true;
            this.B_MoveTutor.Click += new System.EventHandler(this.B_MoveTutor_Click);
            // 
            // B_TMHM
            // 
            this.B_TMHM.Location = new System.Drawing.Point(92, 19);
            this.B_TMHM.Name = "B_TMHM";
            this.B_TMHM.Size = new System.Drawing.Size(80, 23);
            this.B_TMHM.TabIndex = 1;
            this.B_TMHM.Text = "TM/HM";
            this.B_TMHM.UseVisualStyleBackColor = true;
            this.B_TMHM.Click += new System.EventHandler(this.B_TMHM_Click);
            // 
            // RTB_Status
            // 
            this.RTB_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RTB_Status.Location = new System.Drawing.Point(12, 277);
            this.RTB_Status.Name = "RTB_Status";
            this.RTB_Status.ReadOnly = true;
            this.RTB_Status.Size = new System.Drawing.Size(430, 85);
            this.RTB_Status.TabIndex = 7;
            this.RTB_Status.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Tools,
            this.Menu_Options});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(454, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Open,
            this.Menu_Exit});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // Menu_Open
            // 
            this.Menu_Open.Name = "Menu_Open";
            this.Menu_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.Menu_Open.ShowShortcutKeys = false;
            this.Menu_Open.Size = new System.Drawing.Size(105, 22);
            this.Menu_Open.Text = "&Open...";
            this.Menu_Open.Click += new System.EventHandler(this.B_Open_Click);
            // 
            // Menu_Exit
            // 
            this.Menu_Exit.Name = "Menu_Exit";
            this.Menu_Exit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.Menu_Exit.ShowShortcutKeys = false;
            this.Menu_Exit.Size = new System.Drawing.Size(105, 22);
            this.Menu_Exit.Text = "&Exit";
            this.Menu_Exit.Click += new System.EventHandler(this.Menu_Exit_Click);
            // 
            // Menu_Tools
            // 
            this.Menu_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Restore,
            this.Menu_Misc,
            this.Menu_Rebuild,
            this.Menu_SMDH});
            this.Menu_Tools.Name = "Menu_Tools";
            this.Menu_Tools.Size = new System.Drawing.Size(47, 20);
            this.Menu_Tools.Text = "Tools";
            // 
            // Menu_Restore
            // 
            this.Menu_Restore.Enabled = false;
            this.Menu_Restore.Name = "Menu_Restore";
            this.Menu_Restore.Size = new System.Drawing.Size(184, 22);
            this.Menu_Restore.Text = "Restore Original Files";
            this.Menu_Restore.Click += new System.EventHandler(this.L_Game_Click);
            // 
            // Menu_Misc
            // 
            this.Menu_Misc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unPackBCLIMToolStripMenuItem,
            this.Menu_BLZ,
            this.Menu_LZ11,
            this.Menu_Shuffler});
            this.Menu_Misc.Name = "Menu_Misc";
            this.Menu_Misc.Size = new System.Drawing.Size(184, 22);
            this.Menu_Misc.Text = "Misc Tools";
            // 
            // unPackBCLIMToolStripMenuItem
            // 
            this.unPackBCLIMToolStripMenuItem.Name = "unPackBCLIMToolStripMenuItem";
            this.unPackBCLIMToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.unPackBCLIMToolStripMenuItem.Text = "(un)Pack + BCLIM";
            this.unPackBCLIMToolStripMenuItem.Click += new System.EventHandler(this.L_SubTools_Click);
            // 
            // Menu_BLZ
            // 
            this.Menu_BLZ.Name = "Menu_BLZ";
            this.Menu_BLZ.Size = new System.Drawing.Size(176, 22);
            this.Menu_BLZ.Text = "(de)Compress BLZ";
            this.Menu_BLZ.Click += new System.EventHandler(this.Menu_BLZ_Click);
            // 
            // Menu_LZ11
            // 
            this.Menu_LZ11.Name = "Menu_LZ11";
            this.Menu_LZ11.Size = new System.Drawing.Size(176, 22);
            this.Menu_LZ11.Text = "(de)Compress LZ11";
            this.Menu_LZ11.Click += new System.EventHandler(this.Menu_LZ11_Click);
            // 
            // Menu_Shuffler
            // 
            this.Menu_Shuffler.Enabled = false;
            this.Menu_Shuffler.Name = "Menu_Shuffler";
            this.Menu_Shuffler.Size = new System.Drawing.Size(176, 22);
            this.Menu_Shuffler.Text = "GARC Shuffler";
            this.Menu_Shuffler.Click += new System.EventHandler(this.Menu_Shuffler_Click);
            // 
            // Menu_Rebuild
            // 
            this.Menu_Rebuild.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_RomFS,
            this.Menu_ExeFS,
            this.Menu_CRO,
            this.Menu_3DS,
            this.Menu_Patch});
            this.Menu_Rebuild.Name = "Menu_Rebuild";
            this.Menu_Rebuild.Size = new System.Drawing.Size(184, 22);
            this.Menu_Rebuild.Text = "Rebuild...";
            // 
            // Menu_RomFS
            // 
            this.Menu_RomFS.Enabled = false;
            this.Menu_RomFS.Name = "Menu_RomFS";
            this.Menu_RomFS.Size = new System.Drawing.Size(111, 22);
            this.Menu_RomFS.Text = "RomFS";
            this.Menu_RomFS.Click += new System.EventHandler(this.rebuildRomFS);
            // 
            // Menu_ExeFS
            // 
            this.Menu_ExeFS.Enabled = false;
            this.Menu_ExeFS.Name = "Menu_ExeFS";
            this.Menu_ExeFS.Size = new System.Drawing.Size(111, 22);
            this.Menu_ExeFS.Text = "ExeFS";
            this.Menu_ExeFS.Click += new System.EventHandler(this.rebuildExeFS);
            // 
            // Menu_CRO
            // 
            this.Menu_CRO.Enabled = false;
            this.Menu_CRO.Name = "Menu_CRO";
            this.Menu_CRO.Size = new System.Drawing.Size(111, 22);
            this.Menu_CRO.Text = "CRO";
            this.Menu_CRO.Click += new System.EventHandler(this.patchCRO_CRR);
            // 
            // Menu_3DS
            // 
            this.Menu_3DS.Enabled = false;
            this.Menu_3DS.Name = "Menu_3DS";
            this.Menu_3DS.Size = new System.Drawing.Size(111, 22);
            this.Menu_3DS.Text = ".3DS";
            this.Menu_3DS.Click += new System.EventHandler(this.B_Rebuild3DS_Click);
            // 
            // Menu_Patch
            // 
            this.Menu_Patch.Enabled = false;
            this.Menu_Patch.Name = "Menu_Patch";
            this.Menu_Patch.Size = new System.Drawing.Size(111, 22);
            this.Menu_Patch.Text = "Patch";
            this.Menu_Patch.Click += new System.EventHandler(this.B_Patch_Click);
            // 
            // Menu_SMDH
            // 
            this.Menu_SMDH.Name = "Menu_SMDH";
            this.Menu_SMDH.Size = new System.Drawing.Size(184, 22);
            this.Menu_SMDH.Text = "SMDH Editor (Icon)";
            this.Menu_SMDH.Click += new System.EventHandler(this.Menu_SMDH_Click);
            // 
            // Menu_Options
            // 
            this.Menu_Options.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Language,
            this.Menu_About,
            this.Menu_GARCs});
            this.Menu_Options.Name = "Menu_Options";
            this.Menu_Options.Size = new System.Drawing.Size(61, 20);
            this.Menu_Options.Text = "Options";
            // 
            // Menu_Language
            // 
            this.Menu_Language.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CB_Lang});
            this.Menu_Language.Name = "Menu_Language";
            this.Menu_Language.Size = new System.Drawing.Size(146, 22);
            this.Menu_Language.Text = "Language";
            // 
            // CB_Lang
            // 
            this.CB_Lang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Lang.Items.AddRange(new object[] {
            "カタカナ",
            "漢字",
            "English",
            "Français",
            "Italiano",
            "Deutsch",
            "Español",
            "한국"});
            this.CB_Lang.Name = "CB_Lang";
            this.CB_Lang.Size = new System.Drawing.Size(121, 23);
            this.CB_Lang.SelectedIndexChanged += new System.EventHandler(this.changeLanguage);
            // 
            // Menu_About
            // 
            this.Menu_About.Name = "Menu_About";
            this.Menu_About.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.Menu_About.ShowShortcutKeys = false;
            this.Menu_About.Size = new System.Drawing.Size(146, 22);
            this.Menu_About.Text = "A&bout pk3DS";
            this.Menu_About.Click += new System.EventHandler(this.L_About_Click);
            // 
            // Menu_GARCs
            // 
            this.Menu_GARCs.Name = "Menu_GARCs";
            this.Menu_GARCs.Size = new System.Drawing.Size(146, 22);
            this.Menu_GARCs.Text = "About GARCs";
            this.Menu_GARCs.Click += new System.EventHandler(this.L_GARCInfo_Click);
            // 
            // GB_CRO
            // 
            this.GB_CRO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_CRO.Controls.Add(this.B_Static);
            this.GB_CRO.Controls.Add(this.B_Gift);
            this.GB_CRO.Controls.Add(this.B_TypeChart);
            this.GB_CRO.Controls.Add(this.B_Starter);
            this.GB_CRO.Enabled = false;
            this.GB_CRO.Location = new System.Drawing.Point(12, 202);
            this.GB_CRO.Name = "GB_CRO";
            this.GB_CRO.Size = new System.Drawing.Size(430, 50);
            this.GB_CRO.TabIndex = 7;
            this.GB_CRO.TabStop = false;
            this.GB_CRO.Text = "CRO Editing Tools";
            // 
            // B_Gift
            // 
            this.B_Gift.Location = new System.Drawing.Point(218, 19);
            this.B_Gift.Name = "B_Gift";
            this.B_Gift.Size = new System.Drawing.Size(100, 23);
            this.B_Gift.TabIndex = 2;
            this.B_Gift.Text = "Gift Pokémon";
            this.B_Gift.UseVisualStyleBackColor = true;
            this.B_Gift.Click += new System.EventHandler(this.B_Gift_Click);
            // 
            // B_TypeChart
            // 
            this.B_TypeChart.Location = new System.Drawing.Point(6, 19);
            this.B_TypeChart.Name = "B_TypeChart";
            this.B_TypeChart.Size = new System.Drawing.Size(100, 23);
            this.B_TypeChart.TabIndex = 0;
            this.B_TypeChart.Text = "Type Chart";
            this.B_TypeChart.UseVisualStyleBackColor = true;
            this.B_TypeChart.Click += new System.EventHandler(this.B_TypeChart_Click);
            // 
            // B_Starter
            // 
            this.B_Starter.Location = new System.Drawing.Point(112, 19);
            this.B_Starter.Name = "B_Starter";
            this.B_Starter.Size = new System.Drawing.Size(100, 23);
            this.B_Starter.TabIndex = 1;
            this.B_Starter.Text = "Starters";
            this.B_Starter.UseVisualStyleBackColor = true;
            this.B_Starter.Click += new System.EventHandler(this.B_Starter_Click);
            // 
            // B_Static
            // 
            this.B_Static.Location = new System.Drawing.Point(324, 19);
            this.B_Static.Name = "B_Static";
            this.B_Static.Size = new System.Drawing.Size(100, 23);
            this.B_Static.TabIndex = 3;
            this.B_Static.Text = "Static Encounters";
            this.B_Static.UseVisualStyleBackColor = true;
            this.B_Static.Click += new System.EventHandler(this.B_Static_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 372);
            this.Controls.Add(this.GB_CRO);
            this.Controls.Add(this.RTB_Status);
            this.Controls.Add(this.GB_ExeFS);
            this.Controls.Add(this.pBar1);
            this.Controls.Add(this.L_Game);
            this.Controls.Add(this.GB_RomFS);
            this.Controls.Add(this.TB_Path);
            this.Controls.Add(this.menuStrip1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(470, 555);
            this.MinimumSize = new System.Drawing.Size(470, 264);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "pk3DS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            this.GB_RomFS.ResumeLayout(false);
            this.GB_ExeFS.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.GB_CRO.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TB_Path;
        private System.Windows.Forms.GroupBox GB_RomFS;
        private System.Windows.Forms.Button B_Trainer;
        private System.Windows.Forms.Button B_GameText;
        private System.Windows.Forms.Button B_Wild;
        private System.Windows.Forms.Button B_Item;
        private System.Windows.Forms.Button B_Move;
        private System.Windows.Forms.Button B_Evolution;
        private System.Windows.Forms.Button B_Personal;
        private System.Windows.Forms.Button B_MegaEvo;
        private System.Windows.Forms.Button B_StoryText;
        private System.Windows.Forms.Label L_Game;
        private System.Windows.Forms.ProgressBar pBar1;
        private System.Windows.Forms.Button B_Maison;
        private System.Windows.Forms.Button B_EggMove;
        private System.Windows.Forms.Button B_LevelUp;
        private System.Windows.Forms.GroupBox GB_ExeFS;
        private System.Windows.Forms.Button B_Pickup;
        private System.Windows.Forms.Button B_Mart;
        private System.Windows.Forms.Button B_MoveTutor;
        private System.Windows.Forms.Button B_TMHM;
        public System.Windows.Forms.RichTextBox RTB_Status;
        private System.Windows.Forms.Button B_OPower;
        private System.Windows.Forms.Button B_TitleScreen;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_Open;
        private System.Windows.Forms.ToolStripMenuItem Menu_Exit;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools;
        private System.Windows.Forms.ToolStripMenuItem Menu_Restore;
        private System.Windows.Forms.ToolStripMenuItem Menu_Misc;
        private System.Windows.Forms.ToolStripMenuItem unPackBCLIMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_BLZ;
        private System.Windows.Forms.ToolStripMenuItem Menu_LZ11;
        private System.Windows.Forms.ToolStripMenuItem Menu_Rebuild;
        private System.Windows.Forms.ToolStripMenuItem Menu_RomFS;
        private System.Windows.Forms.ToolStripMenuItem Menu_ExeFS;
        private System.Windows.Forms.ToolStripMenuItem Menu_3DS;
        private System.Windows.Forms.ToolStripMenuItem Menu_Patch;
        private System.Windows.Forms.ToolStripMenuItem Menu_Options;
        private System.Windows.Forms.ToolStripMenuItem Menu_Language;
        private System.Windows.Forms.ToolStripComboBox CB_Lang;
        private System.Windows.Forms.ToolStripMenuItem Menu_About;
        private System.Windows.Forms.ToolStripMenuItem Menu_GARCs;
        private System.Windows.Forms.ToolStripMenuItem Menu_SMDH;
        private System.Windows.Forms.GroupBox GB_CRO;
        private System.Windows.Forms.Button B_TypeChart;
        private System.Windows.Forms.Button B_Starter;
        private System.Windows.Forms.ToolStripMenuItem Menu_CRO;
        private System.Windows.Forms.Button B_Gift;
        private System.Windows.Forms.ToolStripMenuItem Menu_Shuffler;
        private System.Windows.Forms.Button B_Static;
    }
}