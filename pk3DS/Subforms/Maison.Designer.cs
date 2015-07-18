namespace pk3DS
{
    partial class MaisonEditor
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
            this.CB_Trainer = new System.Windows.Forms.ComboBox();
            this.CB_Pokemon = new System.Windows.Forms.ComboBox();
            this.L_Trainer = new System.Windows.Forms.Label();
            this.L_Pokemon = new System.Windows.Forms.Label();
            this.GB_Trainer = new System.Windows.Forms.GroupBox();
            this.L_Class = new System.Windows.Forms.Label();
            this.B_Remove = new System.Windows.Forms.Button();
            this.B_Set = new System.Windows.Forms.Button();
            this.LB_Choices = new System.Windows.Forms.ListBox();
            this.CB_Class = new System.Windows.Forms.ComboBox();
            this.GB_Pokemon = new System.Windows.Forms.GroupBox();
            this.PB_PKM = new System.Windows.Forms.PictureBox();
            this.CHK_Spe = new System.Windows.Forms.CheckBox();
            this.CHK_SpD = new System.Windows.Forms.CheckBox();
            this.CHK_SpA = new System.Windows.Forms.CheckBox();
            this.CHK_DEF = new System.Windows.Forms.CheckBox();
            this.CHK_ATK = new System.Windows.Forms.CheckBox();
            this.CHK_HP = new System.Windows.Forms.CheckBox();
            this.L_Species = new System.Windows.Forms.Label();
            this.L_Item = new System.Windows.Forms.Label();
            this.L_Nature = new System.Windows.Forms.Label();
            this.L_Moves = new System.Windows.Forms.Label();
            this.CB_Item = new System.Windows.Forms.ComboBox();
            this.CB_Nature = new System.Windows.Forms.ComboBox();
            this.CB_Move4 = new System.Windows.Forms.ComboBox();
            this.CB_Move2 = new System.Windows.Forms.ComboBox();
            this.CB_Move3 = new System.Windows.Forms.ComboBox();
            this.CB_Move1 = new System.Windows.Forms.ComboBox();
            this.CB_Species = new System.Windows.Forms.ComboBox();
            this.B_DumpPKs = new System.Windows.Forms.Button();
            this.DumpTRs = new System.Windows.Forms.Button();
            this.GB_Trainer.SuspendLayout();
            this.GB_Pokemon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_PKM)).BeginInit();
            this.SuspendLayout();
            // 
            // CB_Trainer
            // 
            this.CB_Trainer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Trainer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Trainer.FormattingEnabled = true;
            this.CB_Trainer.Location = new System.Drawing.Point(67, 8);
            this.CB_Trainer.Name = "CB_Trainer";
            this.CB_Trainer.Size = new System.Drawing.Size(121, 21);
            this.CB_Trainer.TabIndex = 0;
            this.CB_Trainer.SelectedIndexChanged += new System.EventHandler(this.changeTrainer);
            // 
            // CB_Pokemon
            // 
            this.CB_Pokemon.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Pokemon.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Pokemon.FormattingEnabled = true;
            this.CB_Pokemon.Location = new System.Drawing.Point(361, 8);
            this.CB_Pokemon.Name = "CB_Pokemon";
            this.CB_Pokemon.Size = new System.Drawing.Size(106, 21);
            this.CB_Pokemon.TabIndex = 1;
            this.CB_Pokemon.SelectedIndexChanged += new System.EventHandler(this.changePokemon);
            // 
            // L_Trainer
            // 
            this.L_Trainer.AutoSize = true;
            this.L_Trainer.Location = new System.Drawing.Point(18, 11);
            this.L_Trainer.Name = "L_Trainer";
            this.L_Trainer.Size = new System.Drawing.Size(43, 13);
            this.L_Trainer.TabIndex = 2;
            this.L_Trainer.Text = "Trainer:";
            // 
            // L_Pokemon
            // 
            this.L_Pokemon.AutoSize = true;
            this.L_Pokemon.Location = new System.Drawing.Point(300, 11);
            this.L_Pokemon.Name = "L_Pokemon";
            this.L_Pokemon.Size = new System.Drawing.Size(55, 13);
            this.L_Pokemon.TabIndex = 3;
            this.L_Pokemon.Text = "Pokemon:";
            // 
            // GB_Trainer
            // 
            this.GB_Trainer.Controls.Add(this.L_Class);
            this.GB_Trainer.Controls.Add(this.B_Remove);
            this.GB_Trainer.Controls.Add(this.B_Set);
            this.GB_Trainer.Controls.Add(this.LB_Choices);
            this.GB_Trainer.Controls.Add(this.CB_Class);
            this.GB_Trainer.Location = new System.Drawing.Point(12, 33);
            this.GB_Trainer.Name = "GB_Trainer";
            this.GB_Trainer.Size = new System.Drawing.Size(276, 187);
            this.GB_Trainer.TabIndex = 4;
            this.GB_Trainer.TabStop = false;
            this.GB_Trainer.Text = "Trainer Summary";
            // 
            // L_Class
            // 
            this.L_Class.AutoSize = true;
            this.L_Class.Location = new System.Drawing.Point(14, 23);
            this.L_Class.Name = "L_Class";
            this.L_Class.Size = new System.Drawing.Size(35, 13);
            this.L_Class.TabIndex = 5;
            this.L_Class.Text = "Class:";
            // 
            // B_Remove
            // 
            this.B_Remove.Location = new System.Drawing.Point(182, 158);
            this.B_Remove.Name = "B_Remove";
            this.B_Remove.Size = new System.Drawing.Size(62, 23);
            this.B_Remove.TabIndex = 4;
            this.B_Remove.Text = "[X] Delete";
            this.B_Remove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.B_Remove.UseVisualStyleBackColor = true;
            this.B_Remove.Click += new System.EventHandler(this.B_Remove_Click);
            // 
            // B_Set
            // 
            this.B_Set.Location = new System.Drawing.Point(182, 47);
            this.B_Set.Name = "B_Set";
            this.B_Set.Size = new System.Drawing.Size(62, 23);
            this.B_Set.TabIndex = 2;
            this.B_Set.Text = "[<] Set";
            this.B_Set.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.B_Set.UseVisualStyleBackColor = true;
            this.B_Set.Click += new System.EventHandler(this.B_Set_Click);
            // 
            // LB_Choices
            // 
            this.LB_Choices.FormattingEnabled = true;
            this.LB_Choices.Location = new System.Drawing.Point(9, 47);
            this.LB_Choices.Name = "LB_Choices";
            this.LB_Choices.Size = new System.Drawing.Size(167, 134);
            this.LB_Choices.TabIndex = 1;
            this.LB_Choices.SelectedIndexChanged += new System.EventHandler(this.B_View_Click);
            // 
            // CB_Class
            // 
            this.CB_Class.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Class.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Class.FormattingEnabled = true;
            this.CB_Class.Location = new System.Drawing.Point(55, 20);
            this.CB_Class.Name = "CB_Class";
            this.CB_Class.Size = new System.Drawing.Size(121, 21);
            this.CB_Class.TabIndex = 0;
            // 
            // GB_Pokemon
            // 
            this.GB_Pokemon.Controls.Add(this.PB_PKM);
            this.GB_Pokemon.Controls.Add(this.CHK_Spe);
            this.GB_Pokemon.Controls.Add(this.CHK_SpD);
            this.GB_Pokemon.Controls.Add(this.CHK_SpA);
            this.GB_Pokemon.Controls.Add(this.CHK_DEF);
            this.GB_Pokemon.Controls.Add(this.CHK_ATK);
            this.GB_Pokemon.Controls.Add(this.CHK_HP);
            this.GB_Pokemon.Controls.Add(this.L_Species);
            this.GB_Pokemon.Controls.Add(this.L_Item);
            this.GB_Pokemon.Controls.Add(this.L_Nature);
            this.GB_Pokemon.Controls.Add(this.L_Moves);
            this.GB_Pokemon.Controls.Add(this.CB_Item);
            this.GB_Pokemon.Controls.Add(this.CB_Nature);
            this.GB_Pokemon.Controls.Add(this.CB_Move4);
            this.GB_Pokemon.Controls.Add(this.CB_Move2);
            this.GB_Pokemon.Controls.Add(this.CB_Move3);
            this.GB_Pokemon.Controls.Add(this.CB_Move1);
            this.GB_Pokemon.Controls.Add(this.CB_Species);
            this.GB_Pokemon.Location = new System.Drawing.Point(294, 33);
            this.GB_Pokemon.Name = "GB_Pokemon";
            this.GB_Pokemon.Size = new System.Drawing.Size(275, 187);
            this.GB_Pokemon.TabIndex = 5;
            this.GB_Pokemon.TabStop = false;
            this.GB_Pokemon.Text = "Pokemon Summary";
            // 
            // PB_PKM
            // 
            this.PB_PKM.Location = new System.Drawing.Point(179, 15);
            this.PB_PKM.Name = "PB_PKM";
            this.PB_PKM.Size = new System.Drawing.Size(40, 30);
            this.PB_PKM.TabIndex = 25;
            this.PB_PKM.TabStop = false;
            // 
            // CHK_Spe
            // 
            this.CHK_Spe.AutoSize = true;
            this.CHK_Spe.Location = new System.Drawing.Point(219, 157);
            this.CHK_Spe.Name = "CHK_Spe";
            this.CHK_Spe.Size = new System.Drawing.Size(45, 17);
            this.CHK_Spe.TabIndex = 24;
            this.CHK_Spe.Text = "Spe";
            this.CHK_Spe.UseVisualStyleBackColor = true;
            // 
            // CHK_SpD
            // 
            this.CHK_SpD.AutoSize = true;
            this.CHK_SpD.Location = new System.Drawing.Point(219, 143);
            this.CHK_SpD.Name = "CHK_SpD";
            this.CHK_SpD.Size = new System.Drawing.Size(47, 17);
            this.CHK_SpD.TabIndex = 23;
            this.CHK_SpD.Text = "SpD";
            this.CHK_SpD.UseVisualStyleBackColor = true;
            // 
            // CHK_SpA
            // 
            this.CHK_SpA.AutoSize = true;
            this.CHK_SpA.Location = new System.Drawing.Point(219, 129);
            this.CHK_SpA.Name = "CHK_SpA";
            this.CHK_SpA.Size = new System.Drawing.Size(46, 17);
            this.CHK_SpA.TabIndex = 22;
            this.CHK_SpA.Text = "SpA";
            this.CHK_SpA.UseVisualStyleBackColor = true;
            // 
            // CHK_DEF
            // 
            this.CHK_DEF.AutoSize = true;
            this.CHK_DEF.Location = new System.Drawing.Point(172, 157);
            this.CHK_DEF.Name = "CHK_DEF";
            this.CHK_DEF.Size = new System.Drawing.Size(47, 17);
            this.CHK_DEF.TabIndex = 21;
            this.CHK_DEF.Text = "DEF";
            this.CHK_DEF.UseVisualStyleBackColor = true;
            // 
            // CHK_ATK
            // 
            this.CHK_ATK.AutoSize = true;
            this.CHK_ATK.Location = new System.Drawing.Point(172, 143);
            this.CHK_ATK.Name = "CHK_ATK";
            this.CHK_ATK.Size = new System.Drawing.Size(47, 17);
            this.CHK_ATK.TabIndex = 20;
            this.CHK_ATK.Text = "ATK";
            this.CHK_ATK.UseVisualStyleBackColor = true;
            // 
            // CHK_HP
            // 
            this.CHK_HP.AutoSize = true;
            this.CHK_HP.Location = new System.Drawing.Point(172, 129);
            this.CHK_HP.Name = "CHK_HP";
            this.CHK_HP.Size = new System.Drawing.Size(41, 17);
            this.CHK_HP.TabIndex = 19;
            this.CHK_HP.Text = "HP";
            this.CHK_HP.UseVisualStyleBackColor = true;
            // 
            // L_Species
            // 
            this.L_Species.AutoSize = true;
            this.L_Species.Location = new System.Drawing.Point(13, 23);
            this.L_Species.Name = "L_Species";
            this.L_Species.Size = new System.Drawing.Size(48, 13);
            this.L_Species.TabIndex = 18;
            this.L_Species.Text = "Species:";
            // 
            // L_Item
            // 
            this.L_Item.AutoSize = true;
            this.L_Item.Location = new System.Drawing.Point(29, 159);
            this.L_Item.Name = "L_Item";
            this.L_Item.Size = new System.Drawing.Size(30, 13);
            this.L_Item.TabIndex = 17;
            this.L_Item.Text = "Item:";
            // 
            // L_Nature
            // 
            this.L_Nature.AutoSize = true;
            this.L_Nature.Location = new System.Drawing.Point(17, 132);
            this.L_Nature.Name = "L_Nature";
            this.L_Nature.Size = new System.Drawing.Size(42, 13);
            this.L_Nature.TabIndex = 16;
            this.L_Nature.Text = "Nature:";
            // 
            // L_Moves
            // 
            this.L_Moves.AutoSize = true;
            this.L_Moves.Location = new System.Drawing.Point(13, 48);
            this.L_Moves.Name = "L_Moves";
            this.L_Moves.Size = new System.Drawing.Size(42, 13);
            this.L_Moves.TabIndex = 15;
            this.L_Moves.Text = "Moves:";
            // 
            // CB_Item
            // 
            this.CB_Item.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Item.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Item.FormattingEnabled = true;
            this.CB_Item.Location = new System.Drawing.Point(65, 156);
            this.CB_Item.Name = "CB_Item";
            this.CB_Item.Size = new System.Drawing.Size(101, 21);
            this.CB_Item.TabIndex = 14;
            // 
            // CB_Nature
            // 
            this.CB_Nature.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Nature.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Nature.FormattingEnabled = true;
            this.CB_Nature.Location = new System.Drawing.Point(65, 129);
            this.CB_Nature.Name = "CB_Nature";
            this.CB_Nature.Size = new System.Drawing.Size(101, 21);
            this.CB_Nature.TabIndex = 13;
            // 
            // CB_Move4
            // 
            this.CB_Move4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Move4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Move4.FormattingEnabled = true;
            this.CB_Move4.Location = new System.Drawing.Point(140, 91);
            this.CB_Move4.Name = "CB_Move4";
            this.CB_Move4.Size = new System.Drawing.Size(121, 21);
            this.CB_Move4.TabIndex = 12;
            // 
            // CB_Move2
            // 
            this.CB_Move2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Move2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Move2.FormattingEnabled = true;
            this.CB_Move2.Location = new System.Drawing.Point(140, 64);
            this.CB_Move2.Name = "CB_Move2";
            this.CB_Move2.Size = new System.Drawing.Size(121, 21);
            this.CB_Move2.TabIndex = 11;
            // 
            // CB_Move3
            // 
            this.CB_Move3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Move3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Move3.FormattingEnabled = true;
            this.CB_Move3.Location = new System.Drawing.Point(13, 91);
            this.CB_Move3.Name = "CB_Move3";
            this.CB_Move3.Size = new System.Drawing.Size(121, 21);
            this.CB_Move3.TabIndex = 10;
            // 
            // CB_Move1
            // 
            this.CB_Move1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Move1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Move1.FormattingEnabled = true;
            this.CB_Move1.Location = new System.Drawing.Point(13, 64);
            this.CB_Move1.Name = "CB_Move1";
            this.CB_Move1.Size = new System.Drawing.Size(121, 21);
            this.CB_Move1.TabIndex = 9;
            // 
            // CB_Species
            // 
            this.CB_Species.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Species.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Species.FormattingEnabled = true;
            this.CB_Species.Location = new System.Drawing.Point(67, 20);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(106, 21);
            this.CB_Species.TabIndex = 8;
            this.CB_Species.SelectedIndexChanged += new System.EventHandler(this.changeSpecies);
            // 
            // B_DumpPKs
            // 
            this.B_DumpPKs.Location = new System.Drawing.Point(494, 7);
            this.B_DumpPKs.Name = "B_DumpPKs";
            this.B_DumpPKs.Size = new System.Drawing.Size(75, 23);
            this.B_DumpPKs.TabIndex = 6;
            this.B_DumpPKs.Text = "Dump PKs";
            this.B_DumpPKs.UseVisualStyleBackColor = true;
            this.B_DumpPKs.Click += new System.EventHandler(this.B_DumpPKs_Click);
            // 
            // DumpTRs
            // 
            this.DumpTRs.Location = new System.Drawing.Point(213, 6);
            this.DumpTRs.Name = "DumpTRs";
            this.DumpTRs.Size = new System.Drawing.Size(75, 23);
            this.DumpTRs.TabIndex = 7;
            this.DumpTRs.Text = "Dump TRs";
            this.DumpTRs.UseVisualStyleBackColor = true;
            this.DumpTRs.Click += new System.EventHandler(this.DumpTRs_Click);
            // 
            // Maison
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 227);
            this.Controls.Add(this.DumpTRs);
            this.Controls.Add(this.B_DumpPKs);
            this.Controls.Add(this.GB_Pokemon);
            this.Controls.Add(this.GB_Trainer);
            this.Controls.Add(this.L_Pokemon);
            this.Controls.Add(this.L_Trainer);
            this.Controls.Add(this.CB_Pokemon);
            this.Controls.Add(this.CB_Trainer);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(595, 265);
            this.MinimumSize = new System.Drawing.Size(595, 265);
            this.Name = "Maison";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Maison Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            this.GB_Trainer.ResumeLayout(false);
            this.GB_Trainer.PerformLayout();
            this.GB_Pokemon.ResumeLayout(false);
            this.GB_Pokemon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_PKM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_Trainer;
        private System.Windows.Forms.ComboBox CB_Pokemon;
        private System.Windows.Forms.Label L_Trainer;
        private System.Windows.Forms.Label L_Pokemon;
        private System.Windows.Forms.GroupBox GB_Trainer;
        private System.Windows.Forms.GroupBox GB_Pokemon;
        private System.Windows.Forms.Button B_DumpPKs;
        private System.Windows.Forms.Button DumpTRs;
        private System.Windows.Forms.Label L_Item;
        private System.Windows.Forms.Label L_Nature;
        private System.Windows.Forms.Label L_Moves;
        private System.Windows.Forms.ComboBox CB_Item;
        private System.Windows.Forms.ComboBox CB_Nature;
        private System.Windows.Forms.ComboBox CB_Move4;
        private System.Windows.Forms.ComboBox CB_Move2;
        private System.Windows.Forms.ComboBox CB_Move3;
        private System.Windows.Forms.ComboBox CB_Move1;
        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.Label L_Species;
        private System.Windows.Forms.CheckBox CHK_Spe;
        private System.Windows.Forms.CheckBox CHK_SpD;
        private System.Windows.Forms.CheckBox CHK_SpA;
        private System.Windows.Forms.CheckBox CHK_DEF;
        private System.Windows.Forms.CheckBox CHK_ATK;
        private System.Windows.Forms.CheckBox CHK_HP;
        private System.Windows.Forms.PictureBox PB_PKM;
        private System.Windows.Forms.Label L_Class;
        private System.Windows.Forms.Button B_Remove;
        private System.Windows.Forms.Button B_Set;
        private System.Windows.Forms.ListBox LB_Choices;
        private System.Windows.Forms.ComboBox CB_Class;
    }
}