namespace pk3DS
{
    partial class Gifts
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
            this.LB_Gifts = new System.Windows.Forms.ListBox();
            this.CB_Species = new System.Windows.Forms.ComboBox();
            this.CB_HeldItem = new System.Windows.Forms.ComboBox();
            this.NUD_IV0 = new System.Windows.Forms.NumericUpDown();
            this.NUD_IV1 = new System.Windows.Forms.NumericUpDown();
            this.NUD_IV2 = new System.Windows.Forms.NumericUpDown();
            this.NUD_IV3 = new System.Windows.Forms.NumericUpDown();
            this.NUD_IV4 = new System.Windows.Forms.NumericUpDown();
            this.NUD_IV5 = new System.Windows.Forms.NumericUpDown();
            this.L_Species = new System.Windows.Forms.Label();
            this.L_HeldItem = new System.Windows.Forms.Label();
            this.NUD_Level = new System.Windows.Forms.NumericUpDown();
            this.L_Level = new System.Windows.Forms.Label();
            this.NUD_Form = new System.Windows.Forms.NumericUpDown();
            this.L_Form = new System.Windows.Forms.Label();
            this.L_HP = new System.Windows.Forms.Label();
            this.L_ATK = new System.Windows.Forms.Label();
            this.L_DEF = new System.Windows.Forms.Label();
            this.L_SPA = new System.Windows.Forms.Label();
            this.L_SPE = new System.Windows.Forms.Label();
            this.L_SPD = new System.Windows.Forms.Label();
            this.L_Nature = new System.Windows.Forms.Label();
            this.NUD_Nature = new System.Windows.Forms.NumericUpDown();
            this.L_Ability = new System.Windows.Forms.Label();
            this.NUD_Ability = new System.Windows.Forms.NumericUpDown();
            this.L_Gender = new System.Windows.Forms.Label();
            this.NUD_Gender = new System.Windows.Forms.NumericUpDown();
            this.B_RandAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Nature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Ability)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Gender)).BeginInit();
            this.SuspendLayout();
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Cancel.Location = new System.Drawing.Point(211, 269);
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
            this.B_Save.Location = new System.Drawing.Point(282, 269);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(70, 23);
            this.B_Save.TabIndex = 466;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // LB_Gifts
            // 
            this.LB_Gifts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_Gifts.FormattingEnabled = true;
            this.LB_Gifts.Location = new System.Drawing.Point(12, 12);
            this.LB_Gifts.Name = "LB_Gifts";
            this.LB_Gifts.Size = new System.Drawing.Size(110, 277);
            this.LB_Gifts.TabIndex = 468;
            this.LB_Gifts.SelectedIndexChanged += new System.EventHandler(this.changeIndex);
            // 
            // CB_Species
            // 
            this.CB_Species.FormattingEnabled = true;
            this.CB_Species.Location = new System.Drawing.Point(196, 12);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(121, 21);
            this.CB_Species.TabIndex = 469;
            this.CB_Species.SelectedIndexChanged += new System.EventHandler(this.changeSpecies);
            // 
            // CB_HeldItem
            // 
            this.CB_HeldItem.FormattingEnabled = true;
            this.CB_HeldItem.Location = new System.Drawing.Point(196, 39);
            this.CB_HeldItem.Name = "CB_HeldItem";
            this.CB_HeldItem.Size = new System.Drawing.Size(121, 21);
            this.CB_HeldItem.TabIndex = 470;
            // 
            // NUD_IV0
            // 
            this.NUD_IV0.Location = new System.Drawing.Point(237, 191);
            this.NUD_IV0.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUD_IV0.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NUD_IV0.Name = "NUD_IV0";
            this.NUD_IV0.Size = new System.Drawing.Size(34, 20);
            this.NUD_IV0.TabIndex = 471;
            this.NUD_IV0.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // NUD_IV1
            // 
            this.NUD_IV1.Location = new System.Drawing.Point(237, 217);
            this.NUD_IV1.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUD_IV1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NUD_IV1.Name = "NUD_IV1";
            this.NUD_IV1.Size = new System.Drawing.Size(34, 20);
            this.NUD_IV1.TabIndex = 472;
            this.NUD_IV1.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // NUD_IV2
            // 
            this.NUD_IV2.Location = new System.Drawing.Point(237, 243);
            this.NUD_IV2.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUD_IV2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NUD_IV2.Name = "NUD_IV2";
            this.NUD_IV2.Size = new System.Drawing.Size(34, 20);
            this.NUD_IV2.TabIndex = 473;
            this.NUD_IV2.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // NUD_IV3
            // 
            this.NUD_IV3.Location = new System.Drawing.Point(318, 191);
            this.NUD_IV3.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUD_IV3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NUD_IV3.Name = "NUD_IV3";
            this.NUD_IV3.Size = new System.Drawing.Size(34, 20);
            this.NUD_IV3.TabIndex = 474;
            this.NUD_IV3.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // NUD_IV4
            // 
            this.NUD_IV4.Location = new System.Drawing.Point(318, 217);
            this.NUD_IV4.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUD_IV4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NUD_IV4.Name = "NUD_IV4";
            this.NUD_IV4.Size = new System.Drawing.Size(34, 20);
            this.NUD_IV4.TabIndex = 475;
            this.NUD_IV4.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // NUD_IV5
            // 
            this.NUD_IV5.Location = new System.Drawing.Point(318, 243);
            this.NUD_IV5.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUD_IV5.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NUD_IV5.Name = "NUD_IV5";
            this.NUD_IV5.Size = new System.Drawing.Size(34, 20);
            this.NUD_IV5.TabIndex = 476;
            this.NUD_IV5.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // L_Species
            // 
            this.L_Species.Location = new System.Drawing.Point(96, 12);
            this.L_Species.Name = "L_Species";
            this.L_Species.Size = new System.Drawing.Size(94, 21);
            this.L_Species.TabIndex = 477;
            this.L_Species.Text = "Species:";
            this.L_Species.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_HeldItem
            // 
            this.L_HeldItem.Location = new System.Drawing.Point(96, 38);
            this.L_HeldItem.Name = "L_HeldItem";
            this.L_HeldItem.Size = new System.Drawing.Size(94, 21);
            this.L_HeldItem.TabIndex = 478;
            this.L_HeldItem.Text = "Held Item:";
            this.L_HeldItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_Level
            // 
            this.NUD_Level.Location = new System.Drawing.Point(196, 66);
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
            this.L_Level.Location = new System.Drawing.Point(97, 64);
            this.L_Level.Name = "L_Level";
            this.L_Level.Size = new System.Drawing.Size(94, 21);
            this.L_Level.TabIndex = 480;
            this.L_Level.Text = "Level:";
            this.L_Level.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_Form
            // 
            this.NUD_Form.Location = new System.Drawing.Point(283, 66);
            this.NUD_Form.Name = "NUD_Form";
            this.NUD_Form.Size = new System.Drawing.Size(34, 20);
            this.NUD_Form.TabIndex = 481;
            // 
            // L_Form
            // 
            this.L_Form.Location = new System.Drawing.Point(189, 64);
            this.L_Form.Name = "L_Form";
            this.L_Form.Size = new System.Drawing.Size(94, 21);
            this.L_Form.TabIndex = 482;
            this.L_Form.Text = "Form:";
            this.L_Form.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_HP
            // 
            this.L_HP.Location = new System.Drawing.Point(143, 189);
            this.L_HP.Name = "L_HP";
            this.L_HP.Size = new System.Drawing.Size(94, 21);
            this.L_HP.TabIndex = 483;
            this.L_HP.Text = "HP:";
            this.L_HP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_ATK
            // 
            this.L_ATK.Location = new System.Drawing.Point(143, 215);
            this.L_ATK.Name = "L_ATK";
            this.L_ATK.Size = new System.Drawing.Size(94, 21);
            this.L_ATK.TabIndex = 484;
            this.L_ATK.Text = "Atk:";
            this.L_ATK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_DEF
            // 
            this.L_DEF.Location = new System.Drawing.Point(143, 241);
            this.L_DEF.Name = "L_DEF";
            this.L_DEF.Size = new System.Drawing.Size(94, 21);
            this.L_DEF.TabIndex = 485;
            this.L_DEF.Text = "Def:";
            this.L_DEF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_SPA
            // 
            this.L_SPA.Location = new System.Drawing.Point(224, 189);
            this.L_SPA.Name = "L_SPA";
            this.L_SPA.Size = new System.Drawing.Size(94, 21);
            this.L_SPA.TabIndex = 486;
            this.L_SPA.Text = "SpA:";
            this.L_SPA.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_SPE
            // 
            this.L_SPE.Location = new System.Drawing.Point(224, 241);
            this.L_SPE.Name = "L_SPE";
            this.L_SPE.Size = new System.Drawing.Size(94, 21);
            this.L_SPE.TabIndex = 487;
            this.L_SPE.Text = "Spe:";
            this.L_SPE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_SPD
            // 
            this.L_SPD.Location = new System.Drawing.Point(224, 215);
            this.L_SPD.Name = "L_SPD";
            this.L_SPD.Size = new System.Drawing.Size(94, 21);
            this.L_SPD.TabIndex = 488;
            this.L_SPD.Text = "SpD:";
            this.L_SPD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_Nature
            // 
            this.L_Nature.Location = new System.Drawing.Point(97, 91);
            this.L_Nature.Name = "L_Nature";
            this.L_Nature.Size = new System.Drawing.Size(94, 21);
            this.L_Nature.TabIndex = 490;
            this.L_Nature.Text = "Nature:";
            this.L_Nature.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_Nature
            // 
            this.NUD_Nature.Location = new System.Drawing.Point(196, 91);
            this.NUD_Nature.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.NUD_Nature.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NUD_Nature.Name = "NUD_Nature";
            this.NUD_Nature.Size = new System.Drawing.Size(34, 20);
            this.NUD_Nature.TabIndex = 491;
            this.NUD_Nature.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // L_Ability
            // 
            this.L_Ability.Location = new System.Drawing.Point(190, 89);
            this.L_Ability.Name = "L_Ability";
            this.L_Ability.Size = new System.Drawing.Size(94, 21);
            this.L_Ability.TabIndex = 493;
            this.L_Ability.Text = "Ability:";
            this.L_Ability.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_Ability
            // 
            this.NUD_Ability.Location = new System.Drawing.Point(284, 91);
            this.NUD_Ability.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NUD_Ability.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NUD_Ability.Name = "NUD_Ability";
            this.NUD_Ability.Size = new System.Drawing.Size(34, 20);
            this.NUD_Ability.TabIndex = 492;
            // 
            // L_Gender
            // 
            this.L_Gender.Location = new System.Drawing.Point(190, 115);
            this.L_Gender.Name = "L_Gender";
            this.L_Gender.Size = new System.Drawing.Size(94, 21);
            this.L_Gender.TabIndex = 495;
            this.L_Gender.Text = "Gender:";
            this.L_Gender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NUD_Gender
            // 
            this.NUD_Gender.Location = new System.Drawing.Point(284, 117);
            this.NUD_Gender.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NUD_Gender.Name = "NUD_Gender";
            this.NUD_Gender.Size = new System.Drawing.Size(34, 20);
            this.NUD_Gender.TabIndex = 494;
            // 
            // B_RandAll
            // 
            this.B_RandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_RandAll.Location = new System.Drawing.Point(128, 269);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(83, 23);
            this.B_RandAll.TabIndex = 496;
            this.B_RandAll.Text = "Randomize All";
            this.B_RandAll.UseVisualStyleBackColor = true;
            this.B_RandAll.Click += new System.EventHandler(this.B_RandAll_Click);
            // 
            // Gifts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 304);
            this.Controls.Add(this.B_RandAll);
            this.Controls.Add(this.L_Gender);
            this.Controls.Add(this.NUD_Gender);
            this.Controls.Add(this.NUD_Nature);
            this.Controls.Add(this.LB_Gifts);
            this.Controls.Add(this.L_Nature);
            this.Controls.Add(this.L_DEF);
            this.Controls.Add(this.L_ATK);
            this.Controls.Add(this.L_HP);
            this.Controls.Add(this.NUD_Level);
            this.Controls.Add(this.L_Form);
            this.Controls.Add(this.NUD_Form);
            this.Controls.Add(this.NUD_IV5);
            this.Controls.Add(this.NUD_IV4);
            this.Controls.Add(this.NUD_IV3);
            this.Controls.Add(this.NUD_IV2);
            this.Controls.Add(this.NUD_IV1);
            this.Controls.Add(this.NUD_IV0);
            this.Controls.Add(this.CB_HeldItem);
            this.Controls.Add(this.CB_Species);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.L_Level);
            this.Controls.Add(this.L_HeldItem);
            this.Controls.Add(this.L_Species);
            this.Controls.Add(this.L_SPD);
            this.Controls.Add(this.L_SPE);
            this.Controls.Add(this.L_SPA);
            this.Controls.Add(this.L_Ability);
            this.Controls.Add(this.NUD_Ability);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 343);
            this.Name = "Gifts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gift Editor";
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_IV5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Nature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Ability)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Gender)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button B_Cancel;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.ListBox LB_Gifts;
        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.ComboBox CB_HeldItem;
        private System.Windows.Forms.NumericUpDown NUD_IV0;
        private System.Windows.Forms.NumericUpDown NUD_IV1;
        private System.Windows.Forms.NumericUpDown NUD_IV2;
        private System.Windows.Forms.NumericUpDown NUD_IV3;
        private System.Windows.Forms.NumericUpDown NUD_IV4;
        private System.Windows.Forms.NumericUpDown NUD_IV5;
        private System.Windows.Forms.Label L_Species;
        private System.Windows.Forms.Label L_HeldItem;
        private System.Windows.Forms.NumericUpDown NUD_Level;
        private System.Windows.Forms.Label L_Level;
        private System.Windows.Forms.NumericUpDown NUD_Form;
        private System.Windows.Forms.Label L_Form;
        private System.Windows.Forms.Label L_HP;
        private System.Windows.Forms.Label L_ATK;
        private System.Windows.Forms.Label L_DEF;
        private System.Windows.Forms.Label L_SPA;
        private System.Windows.Forms.Label L_SPE;
        private System.Windows.Forms.Label L_SPD;
        private System.Windows.Forms.Label L_Nature;
        private System.Windows.Forms.NumericUpDown NUD_Nature;
        private System.Windows.Forms.Label L_Ability;
        private System.Windows.Forms.NumericUpDown NUD_Ability;
        private System.Windows.Forms.Label L_Gender;
        private System.Windows.Forms.NumericUpDown NUD_Gender;
        private System.Windows.Forms.Button B_RandAll;
    }
}