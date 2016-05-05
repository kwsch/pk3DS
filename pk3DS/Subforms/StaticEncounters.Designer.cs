namespace pk3DS
{
    partial class StaticEncounters
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
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Ability)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Gender)).BeginInit();
            this.SuspendLayout();
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Cancel.Location = new System.Drawing.Point(211, 296);
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
            this.B_Save.Location = new System.Drawing.Point(282, 296);
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
            this.CB_Species.Location = new System.Drawing.Point(196, 12);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(121, 21);
            this.CB_Species.TabIndex = 469;
            this.CB_Species.SelectedIndexChanged += new System.EventHandler(this.changeSpecies);
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
            // B_RandAll
            // 
            this.B_RandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_RandAll.Location = new System.Drawing.Point(128, 296);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(83, 23);
            this.B_RandAll.TabIndex = 496;
            this.B_RandAll.Text = "Randomize All";
            this.B_RandAll.UseVisualStyleBackColor = true;
            this.B_RandAll.Click += new System.EventHandler(this.B_RandAll_Click);
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
            // CB_HeldItem
            // 
            this.CB_HeldItem.FormattingEnabled = true;
            this.CB_HeldItem.Location = new System.Drawing.Point(196, 39);
            this.CB_HeldItem.Name = "CB_HeldItem";
            this.CB_HeldItem.Size = new System.Drawing.Size(121, 21);
            this.CB_HeldItem.TabIndex = 470;
            // 
            // NUD_Ability
            // 
            this.NUD_Ability.Location = new System.Drawing.Point(284, 91);
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
            this.L_Ability.Location = new System.Drawing.Point(190, 89);
            this.L_Ability.Name = "L_Ability";
            this.L_Ability.Size = new System.Drawing.Size(94, 21);
            this.L_Ability.TabIndex = 493;
            this.L_Ability.Text = "Ability:";
            this.L_Ability.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // L_Gender
            // 
            this.L_Gender.Location = new System.Drawing.Point(190, 115);
            this.L_Gender.Name = "L_Gender";
            this.L_Gender.Size = new System.Drawing.Size(94, 21);
            this.L_Gender.TabIndex = 495;
            this.L_Gender.Text = "Gender:";
            this.L_Gender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CHK_3IV
            // 
            this.CHK_3IV.AutoSize = true;
            this.CHK_3IV.Location = new System.Drawing.Point(132, 118);
            this.CHK_3IV.Name = "CHK_3IV";
            this.CHK_3IV.Size = new System.Drawing.Size(42, 17);
            this.CHK_3IV.TabIndex = 497;
            this.CHK_3IV.Text = "3IV";
            this.CHK_3IV.UseVisualStyleBackColor = true;
            // 
            // CHK_NoShiny
            // 
            this.CHK_NoShiny.AutoSize = true;
            this.CHK_NoShiny.Location = new System.Drawing.Point(132, 92);
            this.CHK_NoShiny.Name = "CHK_NoShiny";
            this.CHK_NoShiny.Size = new System.Drawing.Size(79, 17);
            this.CHK_NoShiny.TabIndex = 498;
            this.CHK_NoShiny.Text = "Shiny Lock";
            this.CHK_NoShiny.UseVisualStyleBackColor = true;
            // 
            // CHK_3IV_2
            // 
            this.CHK_3IV_2.AutoSize = true;
            this.CHK_3IV_2.Location = new System.Drawing.Point(132, 141);
            this.CHK_3IV_2.Name = "CHK_3IV_2";
            this.CHK_3IV_2.Size = new System.Drawing.Size(54, 17);
            this.CHK_3IV_2.TabIndex = 499;
            this.CHK_3IV_2.Text = "3IV_2";
            this.CHK_3IV_2.UseVisualStyleBackColor = true;
            // 
            // L_Hint
            // 
            this.L_Hint.AutoSize = true;
            this.L_Hint.Location = new System.Drawing.Point(201, 142);
            this.L_Hint.Name = "L_Hint";
            this.L_Hint.Size = new System.Drawing.Size(117, 130);
            this.L_Hint.TabIndex = 500;
            this.L_Hint.Text = "Ability:\r\n0: Random\r\n1: Ability 0\r\n2: Ability 1\r\n3: Hidden\r\n\r\nGender:\r\n0: Random/" +
    "Genderless\r\n1: Male\r\n2: Female";
            // 
            // StaticEncounters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 331);
            this.Controls.Add(this.L_Hint);
            this.Controls.Add(this.CHK_3IV_2);
            this.Controls.Add(this.CHK_NoShiny);
            this.Controls.Add(this.CHK_3IV);
            this.Controls.Add(this.B_RandAll);
            this.Controls.Add(this.L_Gender);
            this.Controls.Add(this.NUD_Gender);
            this.Controls.Add(this.LB_Encounters);
            this.Controls.Add(this.NUD_Level);
            this.Controls.Add(this.L_Form);
            this.Controls.Add(this.NUD_Form);
            this.Controls.Add(this.CB_HeldItem);
            this.Controls.Add(this.CB_Species);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.L_Level);
            this.Controls.Add(this.L_HeldItem);
            this.Controls.Add(this.L_Species);
            this.Controls.Add(this.L_Ability);
            this.Controls.Add(this.NUD_Ability);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 370);
            this.Name = "StaticEncounters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Static Encounter Editor";
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Ability)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Gender)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}