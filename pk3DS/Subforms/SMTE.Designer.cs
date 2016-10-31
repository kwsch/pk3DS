namespace pk3DS
{
    partial class SMTE
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
            this.L_TrainerID = new System.Windows.Forms.Label();
            this.CB_TrainerID = new System.Windows.Forms.ComboBox();
            this.TC_Main = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox_Healer = new System.Windows.Forms.CheckBox();
            this.L_TPrize = new System.Windows.Forms.Label();
            this.CB_Prize = new System.Windows.Forms.ComboBox();
            this.L_AI = new System.Windows.Forms.Label();
            this.CB_AI = new System.Windows.Forms.ComboBox();
            this.L_Money = new System.Windows.Forms.Label();
            this.CB_Money = new System.Windows.Forms.ComboBox();
            this.L_Battle_Type = new System.Windows.Forms.Label();
            this.CB_Battle_Type = new System.Windows.Forms.ComboBox();
            this.L_Trainer_Class = new System.Windows.Forms.Label();
            this.CB_Trainer_Class = new System.Windows.Forms.ComboBox();
            this.checkBox_Moves = new System.Windows.Forms.CheckBox();
            this.checkBox_Item = new System.Windows.Forms.CheckBox();
            this.L_Item_4 = new System.Windows.Forms.Label();
            this.CB_Item_4 = new System.Windows.Forms.ComboBox();
            this.L_Item_3 = new System.Windows.Forms.Label();
            this.CB_Item_3 = new System.Windows.Forms.ComboBox();
            this.L_Item_2 = new System.Windows.Forms.Label();
            this.CB_Item_2 = new System.Windows.Forms.ComboBox();
            this.L_Item_1 = new System.Windows.Forms.Label();
            this.CB_Item_1 = new System.Windows.Forms.ComboBox();
            this.L_numPokemon = new System.Windows.Forms.Label();
            this.CB_numPokemon = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PB_Team1 = new System.Windows.Forms.PictureBox();
            this.PB_Team2 = new System.Windows.Forms.PictureBox();
            this.PB_Team3 = new System.Windows.Forms.PictureBox();
            this.PB_Team4 = new System.Windows.Forms.PictureBox();
            this.PB_Team5 = new System.Windows.Forms.PictureBox();
            this.PB_Team6 = new System.Windows.Forms.PictureBox();
            this.B_Randomize = new System.Windows.Forms.Button();
            this.B_Dump = new System.Windows.Forms.Button();
            this.TC_Main.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team6)).BeginInit();
            this.SuspendLayout();
            // 
            // L_TrainerID
            // 
            this.L_TrainerID.AutoSize = true;
            this.L_TrainerID.Location = new System.Drawing.Point(13, 15);
            this.L_TrainerID.Name = "L_TrainerID";
            this.L_TrainerID.Size = new System.Drawing.Size(57, 13);
            this.L_TrainerID.TabIndex = 65;
            this.L_TrainerID.Text = "Trainer ID:";
            // 
            // CB_TrainerID
            // 
            this.CB_TrainerID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_TrainerID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_TrainerID.FormattingEnabled = true;
            this.CB_TrainerID.Location = new System.Drawing.Point(76, 12);
            this.CB_TrainerID.Name = "CB_TrainerID";
            this.CB_TrainerID.Size = new System.Drawing.Size(134, 21);
            this.CB_TrainerID.TabIndex = 64;
            this.CB_TrainerID.SelectedIndexChanged += new System.EventHandler(this.changeTrainerIndex);
            // 
            // TC_Main
            // 
            this.TC_Main.Controls.Add(this.tabPage1);
            this.TC_Main.Location = new System.Drawing.Point(5, 39);
            this.TC_Main.Name = "TC_Main";
            this.TC_Main.SelectedIndex = 0;
            this.TC_Main.Size = new System.Drawing.Size(565, 174);
            this.TC_Main.TabIndex = 71;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.checkBox_Healer);
            this.tabPage1.Controls.Add(this.L_TPrize);
            this.tabPage1.Controls.Add(this.CB_Prize);
            this.tabPage1.Controls.Add(this.L_AI);
            this.tabPage1.Controls.Add(this.CB_AI);
            this.tabPage1.Controls.Add(this.L_Money);
            this.tabPage1.Controls.Add(this.CB_Money);
            this.tabPage1.Controls.Add(this.L_Battle_Type);
            this.tabPage1.Controls.Add(this.CB_Battle_Type);
            this.tabPage1.Controls.Add(this.L_Trainer_Class);
            this.tabPage1.Controls.Add(this.CB_Trainer_Class);
            this.tabPage1.Controls.Add(this.checkBox_Moves);
            this.tabPage1.Controls.Add(this.checkBox_Item);
            this.tabPage1.Controls.Add(this.L_Item_4);
            this.tabPage1.Controls.Add(this.CB_Item_4);
            this.tabPage1.Controls.Add(this.L_Item_3);
            this.tabPage1.Controls.Add(this.CB_Item_3);
            this.tabPage1.Controls.Add(this.L_Item_2);
            this.tabPage1.Controls.Add(this.CB_Item_2);
            this.tabPage1.Controls.Add(this.L_Item_1);
            this.tabPage1.Controls.Add(this.CB_Item_1);
            this.tabPage1.Controls.Add(this.L_numPokemon);
            this.tabPage1.Controls.Add(this.CB_numPokemon);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(557, 148);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Trainer info";
            // 
            // checkBox_Healer
            // 
            this.checkBox_Healer.AutoSize = true;
            this.checkBox_Healer.Enabled = false;
            this.checkBox_Healer.Location = new System.Drawing.Point(7, 35);
            this.checkBox_Healer.Name = "checkBox_Healer";
            this.checkBox_Healer.Size = new System.Drawing.Size(57, 17);
            this.checkBox_Healer.TabIndex = 58;
            this.checkBox_Healer.Text = "Healer";
            this.checkBox_Healer.UseVisualStyleBackColor = true;
            // 
            // L_TPrize
            // 
            this.L_TPrize.AutoSize = true;
            this.L_TPrize.Location = new System.Drawing.Point(454, 3);
            this.L_TPrize.Name = "L_TPrize";
            this.L_TPrize.Size = new System.Drawing.Size(33, 13);
            this.L_TPrize.TabIndex = 57;
            this.L_TPrize.Text = "Prize:";
            // 
            // CB_Prize
            // 
            this.CB_Prize.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Prize.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Prize.Enabled = false;
            this.CB_Prize.FormattingEnabled = true;
            this.CB_Prize.Location = new System.Drawing.Point(457, 19);
            this.CB_Prize.Name = "CB_Prize";
            this.CB_Prize.Size = new System.Drawing.Size(93, 21);
            this.CB_Prize.TabIndex = 56;
            // 
            // L_AI
            // 
            this.L_AI.AutoSize = true;
            this.L_AI.Enabled = false;
            this.L_AI.Location = new System.Drawing.Point(11, 103);
            this.L_AI.Name = "L_AI";
            this.L_AI.Size = new System.Drawing.Size(49, 13);
            this.L_AI.TabIndex = 55;
            this.L_AI.Text = "AI Level:";
            // 
            // CB_AI
            // 
            this.CB_AI.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_AI.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_AI.Enabled = false;
            this.CB_AI.FormattingEnabled = true;
            this.CB_AI.Location = new System.Drawing.Point(66, 100);
            this.CB_AI.Name = "CB_AI";
            this.CB_AI.Size = new System.Drawing.Size(74, 21);
            this.CB_AI.TabIndex = 54;
            // 
            // L_Money
            // 
            this.L_Money.AutoSize = true;
            this.L_Money.Enabled = false;
            this.L_Money.Location = new System.Drawing.Point(146, 103);
            this.L_Money.Name = "L_Money";
            this.L_Money.Size = new System.Drawing.Size(42, 13);
            this.L_Money.TabIndex = 53;
            this.L_Money.Text = "Money:";
            // 
            // CB_Money
            // 
            this.CB_Money.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Money.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Money.Enabled = false;
            this.CB_Money.FormattingEnabled = true;
            this.CB_Money.Location = new System.Drawing.Point(195, 100);
            this.CB_Money.Name = "CB_Money";
            this.CB_Money.Size = new System.Drawing.Size(77, 21);
            this.CB_Money.TabIndex = 52;
            // 
            // L_Battle_Type
            // 
            this.L_Battle_Type.AutoSize = true;
            this.L_Battle_Type.Enabled = false;
            this.L_Battle_Type.Location = new System.Drawing.Point(86, 76);
            this.L_Battle_Type.Name = "L_Battle_Type";
            this.L_Battle_Type.Size = new System.Drawing.Size(64, 13);
            this.L_Battle_Type.TabIndex = 51;
            this.L_Battle_Type.Text = "Battle Type:";
            // 
            // CB_Battle_Type
            // 
            this.CB_Battle_Type.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Battle_Type.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Battle_Type.Enabled = false;
            this.CB_Battle_Type.FormattingEnabled = true;
            this.CB_Battle_Type.Items.AddRange(new object[] {
            "Single Battle",
            "Double Battle",
            "Triple Battle",
            "Rotation Battle",
            "Horde Battle"});
            this.CB_Battle_Type.Location = new System.Drawing.Point(156, 73);
            this.CB_Battle_Type.Name = "CB_Battle_Type";
            this.CB_Battle_Type.Size = new System.Drawing.Size(116, 21);
            this.CB_Battle_Type.TabIndex = 50;
            // 
            // L_Trainer_Class
            // 
            this.L_Trainer_Class.AutoSize = true;
            this.L_Trainer_Class.Location = new System.Drawing.Point(79, 49);
            this.L_Trainer_Class.Name = "L_Trainer_Class";
            this.L_Trainer_Class.Size = new System.Drawing.Size(71, 13);
            this.L_Trainer_Class.TabIndex = 49;
            this.L_Trainer_Class.Text = "Trainer Class:";
            // 
            // CB_Trainer_Class
            // 
            this.CB_Trainer_Class.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Trainer_Class.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Trainer_Class.DropDownWidth = 165;
            this.CB_Trainer_Class.FormattingEnabled = true;
            this.CB_Trainer_Class.Location = new System.Drawing.Point(156, 46);
            this.CB_Trainer_Class.Name = "CB_Trainer_Class";
            this.CB_Trainer_Class.Size = new System.Drawing.Size(116, 21);
            this.CB_Trainer_Class.TabIndex = 48;
            // 
            // checkBox_Moves
            // 
            this.checkBox_Moves.AutoSize = true;
            this.checkBox_Moves.Enabled = false;
            this.checkBox_Moves.Location = new System.Drawing.Point(7, 58);
            this.checkBox_Moves.Name = "checkBox_Moves";
            this.checkBox_Moves.Size = new System.Drawing.Size(58, 17);
            this.checkBox_Moves.TabIndex = 40;
            this.checkBox_Moves.Text = "Moves";
            this.checkBox_Moves.UseVisualStyleBackColor = true;
            // 
            // checkBox_Item
            // 
            this.checkBox_Item.AutoSize = true;
            this.checkBox_Item.Enabled = false;
            this.checkBox_Item.Location = new System.Drawing.Point(7, 81);
            this.checkBox_Item.Name = "checkBox_Item";
            this.checkBox_Item.Size = new System.Drawing.Size(51, 17);
            this.checkBox_Item.TabIndex = 39;
            this.checkBox_Item.Text = "Items";
            this.checkBox_Item.UseVisualStyleBackColor = true;
            // 
            // L_Item_4
            // 
            this.L_Item_4.AutoSize = true;
            this.L_Item_4.Enabled = false;
            this.L_Item_4.Location = new System.Drawing.Point(278, 103);
            this.L_Item_4.Name = "L_Item_4";
            this.L_Item_4.Size = new System.Drawing.Size(39, 13);
            this.L_Item_4.TabIndex = 34;
            this.L_Item_4.Text = "Item 4:";
            // 
            // CB_Item_4
            // 
            this.CB_Item_4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Item_4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Item_4.Enabled = false;
            this.CB_Item_4.FormattingEnabled = true;
            this.CB_Item_4.Location = new System.Drawing.Point(330, 100);
            this.CB_Item_4.Name = "CB_Item_4";
            this.CB_Item_4.Size = new System.Drawing.Size(121, 21);
            this.CB_Item_4.TabIndex = 33;
            // 
            // L_Item_3
            // 
            this.L_Item_3.AutoSize = true;
            this.L_Item_3.Enabled = false;
            this.L_Item_3.Location = new System.Drawing.Point(278, 76);
            this.L_Item_3.Name = "L_Item_3";
            this.L_Item_3.Size = new System.Drawing.Size(39, 13);
            this.L_Item_3.TabIndex = 32;
            this.L_Item_3.Text = "Item 3:";
            // 
            // CB_Item_3
            // 
            this.CB_Item_3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Item_3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Item_3.Enabled = false;
            this.CB_Item_3.FormattingEnabled = true;
            this.CB_Item_3.Location = new System.Drawing.Point(330, 73);
            this.CB_Item_3.Name = "CB_Item_3";
            this.CB_Item_3.Size = new System.Drawing.Size(121, 21);
            this.CB_Item_3.TabIndex = 31;
            // 
            // L_Item_2
            // 
            this.L_Item_2.AutoSize = true;
            this.L_Item_2.Enabled = false;
            this.L_Item_2.Location = new System.Drawing.Point(278, 49);
            this.L_Item_2.Name = "L_Item_2";
            this.L_Item_2.Size = new System.Drawing.Size(39, 13);
            this.L_Item_2.TabIndex = 30;
            this.L_Item_2.Text = "Item 2:";
            // 
            // CB_Item_2
            // 
            this.CB_Item_2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Item_2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Item_2.Enabled = false;
            this.CB_Item_2.FormattingEnabled = true;
            this.CB_Item_2.Location = new System.Drawing.Point(330, 46);
            this.CB_Item_2.Name = "CB_Item_2";
            this.CB_Item_2.Size = new System.Drawing.Size(121, 21);
            this.CB_Item_2.TabIndex = 29;
            // 
            // L_Item_1
            // 
            this.L_Item_1.AutoSize = true;
            this.L_Item_1.Enabled = false;
            this.L_Item_1.Location = new System.Drawing.Point(278, 22);
            this.L_Item_1.Name = "L_Item_1";
            this.L_Item_1.Size = new System.Drawing.Size(39, 13);
            this.L_Item_1.TabIndex = 28;
            this.L_Item_1.Text = "Item 1:";
            // 
            // CB_Item_1
            // 
            this.CB_Item_1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Item_1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Item_1.Enabled = false;
            this.CB_Item_1.FormattingEnabled = true;
            this.CB_Item_1.Location = new System.Drawing.Point(330, 19);
            this.CB_Item_1.Name = "CB_Item_1";
            this.CB_Item_1.Size = new System.Drawing.Size(121, 21);
            this.CB_Item_1.TabIndex = 27;
            // 
            // L_numPokemon
            // 
            this.L_numPokemon.AutoSize = true;
            this.L_numPokemon.Location = new System.Drawing.Point(43, 22);
            this.L_numPokemon.Name = "L_numPokemon";
            this.L_numPokemon.Size = new System.Drawing.Size(107, 13);
            this.L_numPokemon.TabIndex = 22;
            this.L_numPokemon.Text = "Number of Pokemon:";
            // 
            // CB_numPokemon
            // 
            this.CB_numPokemon.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_numPokemon.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_numPokemon.FormattingEnabled = true;
            this.CB_numPokemon.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.CB_numPokemon.Location = new System.Drawing.Point(156, 19);
            this.CB_numPokemon.Name = "CB_numPokemon";
            this.CB_numPokemon.Size = new System.Drawing.Size(116, 21);
            this.CB_numPokemon.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(-15, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 15);
            this.label4.TabIndex = 445;
            this.label4.Text = "Team:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PB_Team1
            // 
            this.PB_Team1.Location = new System.Drawing.Point(57, 219);
            this.PB_Team1.Name = "PB_Team1";
            this.PB_Team1.Size = new System.Drawing.Size(80, 60);
            this.PB_Team1.TabIndex = 444;
            this.PB_Team1.TabStop = false;
            // 
            // PB_Team2
            // 
            this.PB_Team2.Location = new System.Drawing.Point(143, 219);
            this.PB_Team2.Name = "PB_Team2";
            this.PB_Team2.Size = new System.Drawing.Size(80, 60);
            this.PB_Team2.TabIndex = 443;
            this.PB_Team2.TabStop = false;
            // 
            // PB_Team3
            // 
            this.PB_Team3.Location = new System.Drawing.Point(229, 219);
            this.PB_Team3.Name = "PB_Team3";
            this.PB_Team3.Size = new System.Drawing.Size(80, 60);
            this.PB_Team3.TabIndex = 442;
            this.PB_Team3.TabStop = false;
            // 
            // PB_Team4
            // 
            this.PB_Team4.Location = new System.Drawing.Point(315, 219);
            this.PB_Team4.Name = "PB_Team4";
            this.PB_Team4.Size = new System.Drawing.Size(80, 60);
            this.PB_Team4.TabIndex = 441;
            this.PB_Team4.TabStop = false;
            // 
            // PB_Team5
            // 
            this.PB_Team5.Location = new System.Drawing.Point(401, 219);
            this.PB_Team5.Name = "PB_Team5";
            this.PB_Team5.Size = new System.Drawing.Size(80, 60);
            this.PB_Team5.TabIndex = 440;
            this.PB_Team5.TabStop = false;
            // 
            // PB_Team6
            // 
            this.PB_Team6.Location = new System.Drawing.Point(487, 219);
            this.PB_Team6.Name = "PB_Team6";
            this.PB_Team6.Size = new System.Drawing.Size(80, 60);
            this.PB_Team6.TabIndex = 439;
            this.PB_Team6.TabStop = false;
            // 
            // B_Randomize
            // 
            this.B_Randomize.Enabled = false;
            this.B_Randomize.Location = new System.Drawing.Point(376, 10);
            this.B_Randomize.Name = "B_Randomize";
            this.B_Randomize.Size = new System.Drawing.Size(93, 23);
            this.B_Randomize.TabIndex = 447;
            this.B_Randomize.Text = "Randomize All";
            this.B_Randomize.UseVisualStyleBackColor = true;
            // 
            // B_Dump
            // 
            this.B_Dump.Location = new System.Drawing.Point(475, 10);
            this.B_Dump.Name = "B_Dump";
            this.B_Dump.Size = new System.Drawing.Size(93, 23);
            this.B_Dump.TabIndex = 446;
            this.B_Dump.Text = "Dump to .TXT";
            this.B_Dump.UseVisualStyleBackColor = true;
            this.B_Dump.Click += new System.EventHandler(this.DumpTxt);
            // 
            // SMTE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 291);
            this.Controls.Add(this.B_Randomize);
            this.Controls.Add(this.B_Dump);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PB_Team1);
            this.Controls.Add(this.PB_Team2);
            this.Controls.Add(this.PB_Team3);
            this.Controls.Add(this.PB_Team4);
            this.Controls.Add(this.PB_Team5);
            this.Controls.Add(this.PB_Team6);
            this.Controls.Add(this.TC_Main);
            this.Controls.Add(this.L_TrainerID);
            this.Controls.Add(this.CB_TrainerID);
            this.Name = "SMTE";
            this.Text = "Trainer Editor";
            this.TC_Main.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Team6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label L_TrainerID;
        private System.Windows.Forms.ComboBox CB_TrainerID;
        private System.Windows.Forms.TabControl TC_Main;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox checkBox_Healer;
        private System.Windows.Forms.Label L_TPrize;
        private System.Windows.Forms.ComboBox CB_Prize;
        private System.Windows.Forms.Label L_AI;
        private System.Windows.Forms.ComboBox CB_AI;
        private System.Windows.Forms.Label L_Money;
        private System.Windows.Forms.ComboBox CB_Money;
        private System.Windows.Forms.Label L_Battle_Type;
        private System.Windows.Forms.ComboBox CB_Battle_Type;
        private System.Windows.Forms.Label L_Trainer_Class;
        private System.Windows.Forms.ComboBox CB_Trainer_Class;
        private System.Windows.Forms.CheckBox checkBox_Moves;
        private System.Windows.Forms.CheckBox checkBox_Item;
        private System.Windows.Forms.Label L_Item_4;
        private System.Windows.Forms.ComboBox CB_Item_4;
        private System.Windows.Forms.Label L_Item_3;
        private System.Windows.Forms.ComboBox CB_Item_3;
        private System.Windows.Forms.Label L_Item_2;
        private System.Windows.Forms.ComboBox CB_Item_2;
        private System.Windows.Forms.Label L_Item_1;
        private System.Windows.Forms.ComboBox CB_Item_1;
        private System.Windows.Forms.Label L_numPokemon;
        private System.Windows.Forms.ComboBox CB_numPokemon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox PB_Team1;
        private System.Windows.Forms.PictureBox PB_Team2;
        private System.Windows.Forms.PictureBox PB_Team3;
        private System.Windows.Forms.PictureBox PB_Team4;
        private System.Windows.Forms.PictureBox PB_Team5;
        private System.Windows.Forms.PictureBox PB_Team6;
        private System.Windows.Forms.Button B_Randomize;
        private System.Windows.Forms.Button B_Dump;
    }
}

