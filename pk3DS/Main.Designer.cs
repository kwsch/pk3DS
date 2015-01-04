namespace pk3DS
{
    partial class Main
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
            this.GB_Tools = new System.Windows.Forms.GroupBox();
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
            this.GB_Tools.SuspendLayout();
            this.SuspendLayout();
            // 
            // B_Open
            // 
            this.B_Open.Location = new System.Drawing.Point(12, 12);
            this.B_Open.Name = "B_Open";
            this.B_Open.Size = new System.Drawing.Size(75, 23);
            this.B_Open.TabIndex = 0;
            this.B_Open.Text = "Open \'a\'";
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
            this.TB_Path.Size = new System.Drawing.Size(244, 20);
            this.TB_Path.TabIndex = 1;
            // 
            // GB_Tools
            // 
            this.GB_Tools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Tools.Controls.Add(this.B_StoryText);
            this.GB_Tools.Controls.Add(this.B_Item);
            this.GB_Tools.Controls.Add(this.B_Move);
            this.GB_Tools.Controls.Add(this.B_Evolution);
            this.GB_Tools.Controls.Add(this.B_Personal);
            this.GB_Tools.Controls.Add(this.B_MegaEvo);
            this.GB_Tools.Controls.Add(this.B_Wild);
            this.GB_Tools.Controls.Add(this.B_Trainer);
            this.GB_Tools.Controls.Add(this.B_GameText);
            this.GB_Tools.Enabled = false;
            this.GB_Tools.Location = new System.Drawing.Point(12, 67);
            this.GB_Tools.Name = "GB_Tools";
            this.GB_Tools.Size = new System.Drawing.Size(325, 110);
            this.GB_Tools.TabIndex = 2;
            this.GB_Tools.TabStop = false;
            this.GB_Tools.Text = "ROM Editing Tools";
            // 
            // B_StoryText
            // 
            this.B_StoryText.Location = new System.Drawing.Point(6, 48);
            this.B_StoryText.Name = "B_StoryText";
            this.B_StoryText.Size = new System.Drawing.Size(100, 23);
            this.B_StoryText.TabIndex = 8;
            this.B_StoryText.Text = "Story Text Editor";
            this.B_StoryText.UseVisualStyleBackColor = true;
            this.B_StoryText.Click += new System.EventHandler(this.B_StoryText_Click);
            // 
            // B_Item
            // 
            this.B_Item.Location = new System.Drawing.Point(218, 48);
            this.B_Item.Name = "B_Item";
            this.B_Item.Size = new System.Drawing.Size(100, 23);
            this.B_Item.TabIndex = 7;
            this.B_Item.Text = "Item Editor";
            this.B_Item.UseVisualStyleBackColor = true;
            this.B_Item.Click += new System.EventHandler(this.B_Item_Click);
            // 
            // B_Move
            // 
            this.B_Move.Location = new System.Drawing.Point(218, 19);
            this.B_Move.Name = "B_Move";
            this.B_Move.Size = new System.Drawing.Size(100, 23);
            this.B_Move.TabIndex = 6;
            this.B_Move.Text = "Move Editor";
            this.B_Move.UseVisualStyleBackColor = true;
            this.B_Move.Click += new System.EventHandler(this.B_Move_Click);
            // 
            // B_Evolution
            // 
            this.B_Evolution.Location = new System.Drawing.Point(112, 48);
            this.B_Evolution.Name = "B_Evolution";
            this.B_Evolution.Size = new System.Drawing.Size(100, 23);
            this.B_Evolution.TabIndex = 5;
            this.B_Evolution.Text = "Evolution Editor";
            this.B_Evolution.UseVisualStyleBackColor = true;
            this.B_Evolution.Click += new System.EventHandler(this.B_Evolution_Click);
            // 
            // B_Personal
            // 
            this.B_Personal.Location = new System.Drawing.Point(112, 19);
            this.B_Personal.Name = "B_Personal";
            this.B_Personal.Size = new System.Drawing.Size(100, 23);
            this.B_Personal.TabIndex = 4;
            this.B_Personal.Text = "Personal Editor";
            this.B_Personal.UseVisualStyleBackColor = true;
            this.B_Personal.Click += new System.EventHandler(this.B_Personal_Click);
            // 
            // B_MegaEvo
            // 
            this.B_MegaEvo.Location = new System.Drawing.Point(112, 77);
            this.B_MegaEvo.Name = "B_MegaEvo";
            this.B_MegaEvo.Size = new System.Drawing.Size(100, 23);
            this.B_MegaEvo.TabIndex = 3;
            this.B_MegaEvo.Text = "Mega Evo Editor";
            this.B_MegaEvo.UseVisualStyleBackColor = true;
            this.B_MegaEvo.Click += new System.EventHandler(this.B_MegaEvo_Click);
            // 
            // B_Wild
            // 
            this.B_Wild.Location = new System.Drawing.Point(6, 77);
            this.B_Wild.Name = "B_Wild";
            this.B_Wild.Size = new System.Drawing.Size(100, 23);
            this.B_Wild.TabIndex = 2;
            this.B_Wild.Text = "Wild Editor";
            this.B_Wild.UseVisualStyleBackColor = true;
            this.B_Wild.Click += new System.EventHandler(this.B_Wild_Click);
            // 
            // B_Trainer
            // 
            this.B_Trainer.Location = new System.Drawing.Point(218, 77);
            this.B_Trainer.Name = "B_Trainer";
            this.B_Trainer.Size = new System.Drawing.Size(100, 23);
            this.B_Trainer.TabIndex = 1;
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
            this.CB_Lang.Location = new System.Drawing.Point(216, 40);
            this.CB_Lang.Name = "CB_Lang";
            this.CB_Lang.Size = new System.Drawing.Size(121, 21);
            this.CB_Lang.TabIndex = 3;
            this.CB_Lang.SelectedIndexChanged += new System.EventHandler(this.changeLanguage);
            // 
            // L_Lang
            // 
            this.L_Lang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Lang.AutoSize = true;
            this.L_Lang.Location = new System.Drawing.Point(152, 43);
            this.L_Lang.Name = "L_Lang";
            this.L_Lang.Size = new System.Drawing.Size(58, 13);
            this.L_Lang.TabIndex = 4;
            this.L_Lang.Text = "Language:";
            // 
            // L_Game
            // 
            this.L_Game.AutoSize = true;
            this.L_Game.ForeColor = System.Drawing.Color.Red;
            this.L_Game.Location = new System.Drawing.Point(15, 43);
            this.L_Game.Name = "L_Game";
            this.L_Game.Size = new System.Drawing.Size(91, 13);
            this.L_Game.TabIndex = 5;
            this.L_Game.Text = "No Game Loaded";
            // 
            // pBar1
            // 
            this.pBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBar1.Location = new System.Drawing.Point(12, 182);
            this.pBar1.Name = "pBar1";
            this.pBar1.Size = new System.Drawing.Size(325, 14);
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
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 202);
            this.Controls.Add(this.L_About);
            this.Controls.Add(this.pBar1);
            this.Controls.Add(this.L_Game);
            this.Controls.Add(this.L_Lang);
            this.Controls.Add(this.CB_Lang);
            this.Controls.Add(this.GB_Tools);
            this.Controls.Add(this.TB_Path);
            this.Controls.Add(this.B_Open);
            this.MaximumSize = new System.Drawing.Size(800, 240);
            this.MinimumSize = new System.Drawing.Size(365, 240);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "pk3DS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            this.GB_Tools.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_Open;
        private System.Windows.Forms.TextBox TB_Path;
        private System.Windows.Forms.GroupBox GB_Tools;
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
    }
}

