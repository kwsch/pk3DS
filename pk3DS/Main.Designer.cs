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
            this.B_Open = new System.Windows.Forms.Button();
            this.TB_Path = new System.Windows.Forms.TextBox();
            this.GB_RomFS = new System.Windows.Forms.GroupBox();
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
            this.CB_Lang = new System.Windows.Forms.ComboBox();
            this.L_Lang = new System.Windows.Forms.Label();
            this.L_Game = new System.Windows.Forms.Label();
            this.pBar1 = new System.Windows.Forms.ProgressBar();
            this.L_About = new System.Windows.Forms.Label();
            this.GB_ExeFS = new System.Windows.Forms.GroupBox();
            this.B_OPower = new System.Windows.Forms.Button();
            this.B_Pickup = new System.Windows.Forms.Button();
            this.B_Mart = new System.Windows.Forms.Button();
            this.B_MoveTutor = new System.Windows.Forms.Button();
            this.B_TMHM = new System.Windows.Forms.Button();
            this.RTB_Status = new System.Windows.Forms.RichTextBox();
            this.GB_RomFS.SuspendLayout();
            this.GB_ExeFS.SuspendLayout();
            this.SuspendLayout();
            // 
            // B_Open
            // 
            this.B_Open.Location = new System.Drawing.Point(12, 12);
            this.B_Open.Name = "B_Open";
            this.B_Open.Size = new System.Drawing.Size(75, 23);
            this.B_Open.TabIndex = 0;
            this.B_Open.Text = "Open Dir";
            this.B_Open.UseVisualStyleBackColor = true;
            this.B_Open.Click += new System.EventHandler(this.B_Open_Click);
            // 
            // TB_Path
            // 
            this.TB_Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Path.Location = new System.Drawing.Point(93, 14);
            this.TB_Path.Name = "TB_Path";
            this.TB_Path.ReadOnly = true;
            this.TB_Path.Size = new System.Drawing.Size(349, 20);
            this.TB_Path.TabIndex = 1;
            // 
            // GB_RomFS
            // 
            this.GB_RomFS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.GB_RomFS.Location = new System.Drawing.Point(12, 67);
            this.GB_RomFS.Name = "GB_RomFS";
            this.GB_RomFS.Size = new System.Drawing.Size(430, 110);
            this.GB_RomFS.TabIndex = 5;
            this.GB_RomFS.TabStop = false;
            this.GB_RomFS.Text = "RomFS Editing Tools";
            // 
            // B_Maison
            // 
            this.B_Maison.Location = new System.Drawing.Point(112, 77);
            this.B_Maison.Name = "B_Maison";
            this.B_Maison.Size = new System.Drawing.Size(100, 23);
            this.B_Maison.TabIndex = 5;
            this.B_Maison.Text = "Maison Editor";
            this.B_Maison.UseVisualStyleBackColor = true;
            this.B_Maison.Click += new System.EventHandler(this.B_Maison_Click);
            // 
            // B_EggMove
            // 
            this.B_EggMove.Location = new System.Drawing.Point(324, 48);
            this.B_EggMove.Name = "B_EggMove";
            this.B_EggMove.Size = new System.Drawing.Size(100, 23);
            this.B_EggMove.TabIndex = 10;
            this.B_EggMove.Text = "Egg Move Editor";
            this.B_EggMove.UseVisualStyleBackColor = true;
            this.B_EggMove.Click += new System.EventHandler(this.B_EggMove_Click);
            // 
            // B_LevelUp
            // 
            this.B_LevelUp.Location = new System.Drawing.Point(324, 19);
            this.B_LevelUp.Name = "B_LevelUp";
            this.B_LevelUp.Size = new System.Drawing.Size(100, 23);
            this.B_LevelUp.TabIndex = 9;
            this.B_LevelUp.Text = "Level Up Editor";
            this.B_LevelUp.UseVisualStyleBackColor = true;
            this.B_LevelUp.Click += new System.EventHandler(this.B_LevelUp_Click);
            // 
            // B_StoryText
            // 
            this.B_StoryText.Location = new System.Drawing.Point(6, 48);
            this.B_StoryText.Name = "B_StoryText";
            this.B_StoryText.Size = new System.Drawing.Size(100, 23);
            this.B_StoryText.TabIndex = 1;
            this.B_StoryText.Text = "Story Text Editor";
            this.B_StoryText.UseVisualStyleBackColor = true;
            this.B_StoryText.Click += new System.EventHandler(this.B_StoryText_Click);
            // 
            // B_Item
            // 
            this.B_Item.Location = new System.Drawing.Point(218, 77);
            this.B_Item.Name = "B_Item";
            this.B_Item.Size = new System.Drawing.Size(100, 23);
            this.B_Item.TabIndex = 8;
            this.B_Item.Text = "Item Editor";
            this.B_Item.UseVisualStyleBackColor = true;
            this.B_Item.Click += new System.EventHandler(this.B_Item_Click);
            // 
            // B_Move
            // 
            this.B_Move.Location = new System.Drawing.Point(324, 77);
            this.B_Move.Name = "B_Move";
            this.B_Move.Size = new System.Drawing.Size(100, 23);
            this.B_Move.TabIndex = 11;
            this.B_Move.Text = "Move Editor";
            this.B_Move.UseVisualStyleBackColor = true;
            this.B_Move.Click += new System.EventHandler(this.B_Move_Click);
            // 
            // B_Evolution
            // 
            this.B_Evolution.Location = new System.Drawing.Point(218, 19);
            this.B_Evolution.Name = "B_Evolution";
            this.B_Evolution.Size = new System.Drawing.Size(100, 23);
            this.B_Evolution.TabIndex = 6;
            this.B_Evolution.Text = "Evolution Editor";
            this.B_Evolution.UseVisualStyleBackColor = true;
            this.B_Evolution.Click += new System.EventHandler(this.B_Evolution_Click);
            // 
            // B_Personal
            // 
            this.B_Personal.Location = new System.Drawing.Point(112, 19);
            this.B_Personal.Name = "B_Personal";
            this.B_Personal.Size = new System.Drawing.Size(100, 23);
            this.B_Personal.TabIndex = 3;
            this.B_Personal.Text = "Personal Editor";
            this.B_Personal.UseVisualStyleBackColor = true;
            this.B_Personal.Click += new System.EventHandler(this.B_Personal_Click);
            // 
            // B_MegaEvo
            // 
            this.B_MegaEvo.Location = new System.Drawing.Point(218, 48);
            this.B_MegaEvo.Name = "B_MegaEvo";
            this.B_MegaEvo.Size = new System.Drawing.Size(100, 23);
            this.B_MegaEvo.TabIndex = 7;
            this.B_MegaEvo.Text = "Mega Evo Editor";
            this.B_MegaEvo.UseVisualStyleBackColor = true;
            this.B_MegaEvo.Click += new System.EventHandler(this.B_MegaEvo_Click);
            // 
            // B_Wild
            // 
            this.B_Wild.Location = new System.Drawing.Point(112, 48);
            this.B_Wild.Name = "B_Wild";
            this.B_Wild.Size = new System.Drawing.Size(100, 23);
            this.B_Wild.TabIndex = 4;
            this.B_Wild.Text = "Wild Editor";
            this.B_Wild.UseVisualStyleBackColor = true;
            this.B_Wild.Click += new System.EventHandler(this.B_Wild_Click);
            // 
            // B_Trainer
            // 
            this.B_Trainer.Location = new System.Drawing.Point(6, 77);
            this.B_Trainer.Name = "B_Trainer";
            this.B_Trainer.Size = new System.Drawing.Size(100, 23);
            this.B_Trainer.TabIndex = 2;
            this.B_Trainer.Text = "Trainer Editor";
            this.B_Trainer.UseVisualStyleBackColor = true;
            this.B_Trainer.Click += new System.EventHandler(this.B_Trainer_Click);
            // 
            // B_GameText
            // 
            this.B_GameText.Location = new System.Drawing.Point(6, 19);
            this.B_GameText.Name = "B_GameText";
            this.B_GameText.Size = new System.Drawing.Size(100, 23);
            this.B_GameText.TabIndex = 0;
            this.B_GameText.Text = "Game Text Editor";
            this.B_GameText.UseVisualStyleBackColor = true;
            this.B_GameText.Click += new System.EventHandler(this.B_GameText_Click);
            // 
            // CB_Lang
            // 
            this.CB_Lang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Lang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Lang.FormattingEnabled = true;
            this.CB_Lang.Items.AddRange(new object[] {
            "カタカナ",
            "漢字",
            "English",
            "Français",
            "Italiano",
            "Deutsch",
            "Español",
            "한국"});
            this.CB_Lang.Location = new System.Drawing.Point(321, 40);
            this.CB_Lang.Name = "CB_Lang";
            this.CB_Lang.Size = new System.Drawing.Size(121, 21);
            this.CB_Lang.TabIndex = 4;
            this.CB_Lang.SelectedIndexChanged += new System.EventHandler(this.changeLanguage);
            // 
            // L_Lang
            // 
            this.L_Lang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Lang.AutoSize = true;
            this.L_Lang.Location = new System.Drawing.Point(257, 43);
            this.L_Lang.Name = "L_Lang";
            this.L_Lang.Size = new System.Drawing.Size(58, 13);
            this.L_Lang.TabIndex = 3;
            this.L_Lang.Text = "Language:";
            // 
            // L_Game
            // 
            this.L_Game.AutoSize = true;
            this.L_Game.ForeColor = System.Drawing.Color.Red;
            this.L_Game.Location = new System.Drawing.Point(15, 43);
            this.L_Game.Name = "L_Game";
            this.L_Game.Size = new System.Drawing.Size(91, 13);
            this.L_Game.TabIndex = 2;
            this.L_Game.Text = "No Game Loaded";
            this.L_Game.Click += new System.EventHandler(this.L_Game_Click);
            // 
            // pBar1
            // 
            this.pBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBar1.Location = new System.Drawing.Point(12, 241);
            this.pBar1.Name = "pBar1";
            this.pBar1.Size = new System.Drawing.Size(430, 14);
            this.pBar1.TabIndex = 6;
            // 
            // L_About
            // 
            this.L_About.AutoSize = true;
            this.L_About.Location = new System.Drawing.Point(-2, -1);
            this.L_About.Name = "L_About";
            this.L_About.Size = new System.Drawing.Size(20, 13);
            this.L_About.TabIndex = 7;
            this.L_About.Text = "[A]";
            this.L_About.Click += new System.EventHandler(this.L_About_Click);
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
            this.GB_ExeFS.Location = new System.Drawing.Point(12, 184);
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
            this.RTB_Status.Location = new System.Drawing.Point(12, 263);
            this.RTB_Status.Name = "RTB_Status";
            this.RTB_Status.ReadOnly = true;
            this.RTB_Status.Size = new System.Drawing.Size(430, 150);
            this.RTB_Status.TabIndex = 7;
            this.RTB_Status.Text = "";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 422);
            this.Controls.Add(this.RTB_Status);
            this.Controls.Add(this.GB_ExeFS);
            this.Controls.Add(this.L_About);
            this.Controls.Add(this.pBar1);
            this.Controls.Add(this.L_Game);
            this.Controls.Add(this.L_Lang);
            this.Controls.Add(this.CB_Lang);
            this.Controls.Add(this.GB_RomFS);
            this.Controls.Add(this.TB_Path);
            this.Controls.Add(this.B_Open);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(470, 700);
            this.MinimumSize = new System.Drawing.Size(470, 300);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "pk3DS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            this.GB_RomFS.ResumeLayout(false);
            this.GB_ExeFS.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_Open;
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
        private System.Windows.Forms.ComboBox CB_Lang;
        private System.Windows.Forms.Label L_Lang;
        private System.Windows.Forms.Label L_Game;
        private System.Windows.Forms.ProgressBar pBar1;
        private System.Windows.Forms.Label L_About;
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
    }
}