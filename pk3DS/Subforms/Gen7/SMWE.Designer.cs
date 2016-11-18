namespace pk3DS
{
    partial class SMWE
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
            this.B_Randomize = new System.Windows.Forms.Button();
            this.B_Dump = new System.Windows.Forms.Button();
            this.L_Location = new System.Windows.Forms.Label();
            this.B_Save = new System.Windows.Forms.Button();
            this.CB_LocationID = new System.Windows.Forms.ComboBox();
            this.NUP_Min = new System.Windows.Forms.NumericUpDown();
            this.NUP_Max = new System.Windows.Forms.NumericUpDown();
            this.L_Min = new System.Windows.Forms.Label();
            this.L_Max = new System.Windows.Forms.Label();
            this.L_Table = new System.Windows.Forms.Label();
            this.CB_SlotType = new System.Windows.Forms.ComboBox();
            this.GB_Encounters = new System.Windows.Forms.GroupBox();
            this.NUP_Forme10 = new System.Windows.Forms.NumericUpDown();
            this.CB_Enc10 = new System.Windows.Forms.ComboBox();
            this.L_Rate10 = new System.Windows.Forms.Label();
            this.NUP_Forme9 = new System.Windows.Forms.NumericUpDown();
            this.CB_Enc9 = new System.Windows.Forms.ComboBox();
            this.L_Rate9 = new System.Windows.Forms.Label();
            this.NUP_Forme8 = new System.Windows.Forms.NumericUpDown();
            this.CB_Enc8 = new System.Windows.Forms.ComboBox();
            this.L_Rate8 = new System.Windows.Forms.Label();
            this.NUP_Forme7 = new System.Windows.Forms.NumericUpDown();
            this.CB_Enc7 = new System.Windows.Forms.ComboBox();
            this.L_Rate7 = new System.Windows.Forms.Label();
            this.NUP_Forme6 = new System.Windows.Forms.NumericUpDown();
            this.CB_Enc6 = new System.Windows.Forms.ComboBox();
            this.L_Rate6 = new System.Windows.Forms.Label();
            this.NUP_Forme5 = new System.Windows.Forms.NumericUpDown();
            this.CB_Enc5 = new System.Windows.Forms.ComboBox();
            this.L_Rate5 = new System.Windows.Forms.Label();
            this.NUP_Forme4 = new System.Windows.Forms.NumericUpDown();
            this.CB_Enc4 = new System.Windows.Forms.ComboBox();
            this.L_Rate4 = new System.Windows.Forms.Label();
            this.NUP_Forme3 = new System.Windows.Forms.NumericUpDown();
            this.CB_Enc3 = new System.Windows.Forms.ComboBox();
            this.L_Rate3 = new System.Windows.Forms.Label();
            this.NUP_Forme2 = new System.Windows.Forms.NumericUpDown();
            this.CB_Enc2 = new System.Windows.Forms.ComboBox();
            this.L_Rate2 = new System.Windows.Forms.Label();
            this.L_Rate = new System.Windows.Forms.Label();
            this.L_Grass = new System.Windows.Forms.Label();
            this.L_Forme = new System.Windows.Forms.Label();
            this.NUP_Forme1 = new System.Windows.Forms.NumericUpDown();
            this.CB_Enc1 = new System.Windows.Forms.ComboBox();
            this.L_Rate1 = new System.Windows.Forms.Label();
            this.CB_TableID = new System.Windows.Forms.ComboBox();
            this.B_Export = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Max)).BeginInit();
            this.GB_Encounters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme1)).BeginInit();
            this.SuspendLayout();
            // 
            // B_Randomize
            // 
            this.B_Randomize.Enabled = false;
            this.B_Randomize.Location = new System.Drawing.Point(14, 39);
            this.B_Randomize.Name = "B_Randomize";
            this.B_Randomize.Size = new System.Drawing.Size(102, 23);
            this.B_Randomize.TabIndex = 421;
            this.B_Randomize.Text = "Randomize All";
            this.B_Randomize.UseVisualStyleBackColor = true;
            // 
            // B_Dump
            // 
            this.B_Dump.Enabled = false;
            this.B_Dump.Location = new System.Drawing.Point(122, 39);
            this.B_Dump.Name = "B_Dump";
            this.B_Dump.Size = new System.Drawing.Size(108, 23);
            this.B_Dump.TabIndex = 420;
            this.B_Dump.Text = "Dump Tables";
            this.B_Dump.UseVisualStyleBackColor = true;
            this.B_Dump.Click += new System.EventHandler(this.DumpTables);
            // 
            // L_Location
            // 
            this.L_Location.AutoSize = true;
            this.L_Location.Location = new System.Drawing.Point(11, 16);
            this.L_Location.Name = "L_Location";
            this.L_Location.Size = new System.Drawing.Size(28, 13);
            this.L_Location.TabIndex = 419;
            this.L_Location.Text = "Loc:";
            // 
            // B_Save
            // 
            this.B_Save.Enabled = false;
            this.B_Save.Location = new System.Drawing.Point(213, 11);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(135, 23);
            this.B_Save.TabIndex = 418;
            this.B_Save.Text = "Save Current Table";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // CB_LocationID
            // 
            this.CB_LocationID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_LocationID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_LocationID.Enabled = false;
            this.CB_LocationID.FormattingEnabled = true;
            this.CB_LocationID.Location = new System.Drawing.Point(43, 12);
            this.CB_LocationID.Name = "CB_LocationID";
            this.CB_LocationID.Size = new System.Drawing.Size(164, 21);
            this.CB_LocationID.TabIndex = 417;
            this.CB_LocationID.SelectedIndexChanged += new System.EventHandler(this.updateMap);
            // 
            // NUP_Min
            // 
            this.NUP_Min.Enabled = false;
            this.NUP_Min.Location = new System.Drawing.Point(234, 69);
            this.NUP_Min.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Min.Name = "NUP_Min";
            this.NUP_Min.Size = new System.Drawing.Size(41, 20);
            this.NUP_Min.TabIndex = 423;
            this.NUP_Min.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Min.ValueChanged += new System.EventHandler(this.updateMinMax);
            // 
            // NUP_Max
            // 
            this.NUP_Max.Enabled = false;
            this.NUP_Max.Location = new System.Drawing.Point(305, 68);
            this.NUP_Max.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Max.Name = "NUP_Max";
            this.NUP_Max.Size = new System.Drawing.Size(41, 20);
            this.NUP_Max.TabIndex = 424;
            this.NUP_Max.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NUP_Max.ValueChanged += new System.EventHandler(this.updateMinMax);
            // 
            // L_Min
            // 
            this.L_Min.AutoSize = true;
            this.L_Min.Enabled = false;
            this.L_Min.Location = new System.Drawing.Point(206, 71);
            this.L_Min.Name = "L_Min";
            this.L_Min.Size = new System.Drawing.Size(24, 13);
            this.L_Min.TabIndex = 425;
            this.L_Min.Text = "Min";
            // 
            // L_Max
            // 
            this.L_Max.AutoSize = true;
            this.L_Max.Enabled = false;
            this.L_Max.Location = new System.Drawing.Point(276, 71);
            this.L_Max.Name = "L_Max";
            this.L_Max.Size = new System.Drawing.Size(27, 13);
            this.L_Max.TabIndex = 426;
            this.L_Max.Text = "Max";
            // 
            // L_Table
            // 
            this.L_Table.AutoSize = true;
            this.L_Table.Enabled = false;
            this.L_Table.Location = new System.Drawing.Point(12, 71);
            this.L_Table.Name = "L_Table";
            this.L_Table.Size = new System.Drawing.Size(34, 13);
            this.L_Table.TabIndex = 428;
            this.L_Table.Text = "Table";
            // 
            // CB_SlotType
            // 
            this.CB_SlotType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_SlotType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_SlotType.Enabled = false;
            this.CB_SlotType.FormattingEnabled = true;
            this.CB_SlotType.Location = new System.Drawing.Point(94, 67);
            this.CB_SlotType.Name = "CB_SlotType";
            this.CB_SlotType.Size = new System.Drawing.Size(106, 21);
            this.CB_SlotType.TabIndex = 429;
            this.CB_SlotType.SelectedIndexChanged += new System.EventHandler(this.updatePanel);
            // 
            // GB_Encounters
            // 
            this.GB_Encounters.Controls.Add(this.NUP_Forme10);
            this.GB_Encounters.Controls.Add(this.CB_Enc10);
            this.GB_Encounters.Controls.Add(this.L_Rate10);
            this.GB_Encounters.Controls.Add(this.NUP_Forme9);
            this.GB_Encounters.Controls.Add(this.CB_Enc9);
            this.GB_Encounters.Controls.Add(this.L_Rate9);
            this.GB_Encounters.Controls.Add(this.NUP_Forme8);
            this.GB_Encounters.Controls.Add(this.CB_Enc8);
            this.GB_Encounters.Controls.Add(this.L_Rate8);
            this.GB_Encounters.Controls.Add(this.NUP_Forme7);
            this.GB_Encounters.Controls.Add(this.CB_Enc7);
            this.GB_Encounters.Controls.Add(this.L_Rate7);
            this.GB_Encounters.Controls.Add(this.NUP_Forme6);
            this.GB_Encounters.Controls.Add(this.CB_Enc6);
            this.GB_Encounters.Controls.Add(this.L_Rate6);
            this.GB_Encounters.Controls.Add(this.NUP_Forme5);
            this.GB_Encounters.Controls.Add(this.CB_Enc5);
            this.GB_Encounters.Controls.Add(this.L_Rate5);
            this.GB_Encounters.Controls.Add(this.NUP_Forme4);
            this.GB_Encounters.Controls.Add(this.CB_Enc4);
            this.GB_Encounters.Controls.Add(this.L_Rate4);
            this.GB_Encounters.Controls.Add(this.NUP_Forme3);
            this.GB_Encounters.Controls.Add(this.CB_Enc3);
            this.GB_Encounters.Controls.Add(this.L_Rate3);
            this.GB_Encounters.Controls.Add(this.NUP_Forme2);
            this.GB_Encounters.Controls.Add(this.CB_Enc2);
            this.GB_Encounters.Controls.Add(this.L_Rate2);
            this.GB_Encounters.Controls.Add(this.L_Rate);
            this.GB_Encounters.Controls.Add(this.L_Grass);
            this.GB_Encounters.Controls.Add(this.L_Forme);
            this.GB_Encounters.Controls.Add(this.NUP_Forme1);
            this.GB_Encounters.Controls.Add(this.CB_Enc1);
            this.GB_Encounters.Controls.Add(this.L_Rate1);
            this.GB_Encounters.Enabled = false;
            this.GB_Encounters.Location = new System.Drawing.Point(15, 94);
            this.GB_Encounters.Name = "GB_Encounters";
            this.GB_Encounters.Size = new System.Drawing.Size(331, 319);
            this.GB_Encounters.TabIndex = 430;
            this.GB_Encounters.TabStop = false;
            this.GB_Encounters.Text = "Encounters";
            // 
            // NUP_Forme10
            // 
            this.NUP_Forme10.Location = new System.Drawing.Point(230, 282);
            this.NUP_Forme10.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUP_Forme10.Name = "NUP_Forme10";
            this.NUP_Forme10.Size = new System.Drawing.Size(41, 20);
            this.NUP_Forme10.TabIndex = 340;
            // 
            // CB_Enc10
            // 
            this.CB_Enc10.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Enc10.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Enc10.FormattingEnabled = true;
            this.CB_Enc10.Location = new System.Drawing.Point(103, 282);
            this.CB_Enc10.Name = "CB_Enc10";
            this.CB_Enc10.Size = new System.Drawing.Size(121, 21);
            this.CB_Enc10.TabIndex = 339;
            // 
            // L_Rate10
            // 
            this.L_Rate10.AutoSize = true;
            this.L_Rate10.Location = new System.Drawing.Point(60, 285);
            this.L_Rate10.Name = "L_Rate10";
            this.L_Rate10.Size = new System.Drawing.Size(27, 13);
            this.L_Rate10.TabIndex = 341;
            this.L_Rate10.Text = "10%";
            // 
            // NUP_Forme9
            // 
            this.NUP_Forme9.Location = new System.Drawing.Point(230, 255);
            this.NUP_Forme9.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUP_Forme9.Name = "NUP_Forme9";
            this.NUP_Forme9.Size = new System.Drawing.Size(41, 20);
            this.NUP_Forme9.TabIndex = 337;
            // 
            // CB_Enc9
            // 
            this.CB_Enc9.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Enc9.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Enc9.FormattingEnabled = true;
            this.CB_Enc9.Location = new System.Drawing.Point(103, 255);
            this.CB_Enc9.Name = "CB_Enc9";
            this.CB_Enc9.Size = new System.Drawing.Size(121, 21);
            this.CB_Enc9.TabIndex = 336;
            // 
            // L_Rate9
            // 
            this.L_Rate9.AutoSize = true;
            this.L_Rate9.Location = new System.Drawing.Point(60, 258);
            this.L_Rate9.Name = "L_Rate9";
            this.L_Rate9.Size = new System.Drawing.Size(27, 13);
            this.L_Rate9.TabIndex = 338;
            this.L_Rate9.Text = "10%";
            // 
            // NUP_Forme8
            // 
            this.NUP_Forme8.Location = new System.Drawing.Point(230, 228);
            this.NUP_Forme8.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUP_Forme8.Name = "NUP_Forme8";
            this.NUP_Forme8.Size = new System.Drawing.Size(41, 20);
            this.NUP_Forme8.TabIndex = 334;
            // 
            // CB_Enc8
            // 
            this.CB_Enc8.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Enc8.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Enc8.FormattingEnabled = true;
            this.CB_Enc8.Location = new System.Drawing.Point(103, 228);
            this.CB_Enc8.Name = "CB_Enc8";
            this.CB_Enc8.Size = new System.Drawing.Size(121, 21);
            this.CB_Enc8.TabIndex = 333;
            // 
            // L_Rate8
            // 
            this.L_Rate8.AutoSize = true;
            this.L_Rate8.Location = new System.Drawing.Point(60, 231);
            this.L_Rate8.Name = "L_Rate8";
            this.L_Rate8.Size = new System.Drawing.Size(27, 13);
            this.L_Rate8.TabIndex = 335;
            this.L_Rate8.Text = "10%";
            // 
            // NUP_Forme7
            // 
            this.NUP_Forme7.Location = new System.Drawing.Point(230, 201);
            this.NUP_Forme7.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUP_Forme7.Name = "NUP_Forme7";
            this.NUP_Forme7.Size = new System.Drawing.Size(41, 20);
            this.NUP_Forme7.TabIndex = 331;
            // 
            // CB_Enc7
            // 
            this.CB_Enc7.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Enc7.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Enc7.FormattingEnabled = true;
            this.CB_Enc7.Location = new System.Drawing.Point(103, 201);
            this.CB_Enc7.Name = "CB_Enc7";
            this.CB_Enc7.Size = new System.Drawing.Size(121, 21);
            this.CB_Enc7.TabIndex = 330;
            // 
            // L_Rate7
            // 
            this.L_Rate7.AutoSize = true;
            this.L_Rate7.Location = new System.Drawing.Point(60, 204);
            this.L_Rate7.Name = "L_Rate7";
            this.L_Rate7.Size = new System.Drawing.Size(27, 13);
            this.L_Rate7.TabIndex = 332;
            this.L_Rate7.Text = "10%";
            // 
            // NUP_Forme6
            // 
            this.NUP_Forme6.Location = new System.Drawing.Point(230, 174);
            this.NUP_Forme6.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUP_Forme6.Name = "NUP_Forme6";
            this.NUP_Forme6.Size = new System.Drawing.Size(41, 20);
            this.NUP_Forme6.TabIndex = 328;
            // 
            // CB_Enc6
            // 
            this.CB_Enc6.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Enc6.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Enc6.FormattingEnabled = true;
            this.CB_Enc6.Location = new System.Drawing.Point(103, 174);
            this.CB_Enc6.Name = "CB_Enc6";
            this.CB_Enc6.Size = new System.Drawing.Size(121, 21);
            this.CB_Enc6.TabIndex = 327;
            // 
            // L_Rate6
            // 
            this.L_Rate6.AutoSize = true;
            this.L_Rate6.Location = new System.Drawing.Point(60, 177);
            this.L_Rate6.Name = "L_Rate6";
            this.L_Rate6.Size = new System.Drawing.Size(27, 13);
            this.L_Rate6.TabIndex = 329;
            this.L_Rate6.Text = "10%";
            // 
            // NUP_Forme5
            // 
            this.NUP_Forme5.Location = new System.Drawing.Point(230, 147);
            this.NUP_Forme5.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUP_Forme5.Name = "NUP_Forme5";
            this.NUP_Forme5.Size = new System.Drawing.Size(41, 20);
            this.NUP_Forme5.TabIndex = 325;
            // 
            // CB_Enc5
            // 
            this.CB_Enc5.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Enc5.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Enc5.FormattingEnabled = true;
            this.CB_Enc5.Location = new System.Drawing.Point(103, 147);
            this.CB_Enc5.Name = "CB_Enc5";
            this.CB_Enc5.Size = new System.Drawing.Size(121, 21);
            this.CB_Enc5.TabIndex = 324;
            // 
            // L_Rate5
            // 
            this.L_Rate5.AutoSize = true;
            this.L_Rate5.Location = new System.Drawing.Point(60, 150);
            this.L_Rate5.Name = "L_Rate5";
            this.L_Rate5.Size = new System.Drawing.Size(27, 13);
            this.L_Rate5.TabIndex = 326;
            this.L_Rate5.Text = "10%";
            // 
            // NUP_Forme4
            // 
            this.NUP_Forme4.Location = new System.Drawing.Point(230, 120);
            this.NUP_Forme4.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUP_Forme4.Name = "NUP_Forme4";
            this.NUP_Forme4.Size = new System.Drawing.Size(41, 20);
            this.NUP_Forme4.TabIndex = 322;
            // 
            // CB_Enc4
            // 
            this.CB_Enc4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Enc4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Enc4.FormattingEnabled = true;
            this.CB_Enc4.Location = new System.Drawing.Point(103, 120);
            this.CB_Enc4.Name = "CB_Enc4";
            this.CB_Enc4.Size = new System.Drawing.Size(121, 21);
            this.CB_Enc4.TabIndex = 321;
            // 
            // L_Rate4
            // 
            this.L_Rate4.AutoSize = true;
            this.L_Rate4.Location = new System.Drawing.Point(60, 123);
            this.L_Rate4.Name = "L_Rate4";
            this.L_Rate4.Size = new System.Drawing.Size(27, 13);
            this.L_Rate4.TabIndex = 323;
            this.L_Rate4.Text = "10%";
            // 
            // NUP_Forme3
            // 
            this.NUP_Forme3.Location = new System.Drawing.Point(230, 93);
            this.NUP_Forme3.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUP_Forme3.Name = "NUP_Forme3";
            this.NUP_Forme3.Size = new System.Drawing.Size(41, 20);
            this.NUP_Forme3.TabIndex = 319;
            // 
            // CB_Enc3
            // 
            this.CB_Enc3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Enc3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Enc3.FormattingEnabled = true;
            this.CB_Enc3.Location = new System.Drawing.Point(103, 93);
            this.CB_Enc3.Name = "CB_Enc3";
            this.CB_Enc3.Size = new System.Drawing.Size(121, 21);
            this.CB_Enc3.TabIndex = 318;
            // 
            // L_Rate3
            // 
            this.L_Rate3.AutoSize = true;
            this.L_Rate3.Location = new System.Drawing.Point(60, 96);
            this.L_Rate3.Name = "L_Rate3";
            this.L_Rate3.Size = new System.Drawing.Size(27, 13);
            this.L_Rate3.TabIndex = 320;
            this.L_Rate3.Text = "10%";
            // 
            // NUP_Forme2
            // 
            this.NUP_Forme2.Location = new System.Drawing.Point(230, 66);
            this.NUP_Forme2.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUP_Forme2.Name = "NUP_Forme2";
            this.NUP_Forme2.Size = new System.Drawing.Size(41, 20);
            this.NUP_Forme2.TabIndex = 316;
            // 
            // CB_Enc2
            // 
            this.CB_Enc2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Enc2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Enc2.FormattingEnabled = true;
            this.CB_Enc2.Location = new System.Drawing.Point(103, 66);
            this.CB_Enc2.Name = "CB_Enc2";
            this.CB_Enc2.Size = new System.Drawing.Size(121, 21);
            this.CB_Enc2.TabIndex = 315;
            // 
            // L_Rate2
            // 
            this.L_Rate2.AutoSize = true;
            this.L_Rate2.Location = new System.Drawing.Point(60, 69);
            this.L_Rate2.Name = "L_Rate2";
            this.L_Rate2.Size = new System.Drawing.Size(27, 13);
            this.L_Rate2.TabIndex = 317;
            this.L_Rate2.Text = "10%";
            // 
            // L_Rate
            // 
            this.L_Rate.AutoSize = true;
            this.L_Rate.Location = new System.Drawing.Point(60, 23);
            this.L_Rate.Name = "L_Rate";
            this.L_Rate.Size = new System.Drawing.Size(30, 13);
            this.L_Rate.TabIndex = 314;
            this.L_Rate.Text = "Rate";
            // 
            // L_Grass
            // 
            this.L_Grass.AutoSize = true;
            this.L_Grass.Location = new System.Drawing.Point(100, 23);
            this.L_Grass.Name = "L_Grass";
            this.L_Grass.Size = new System.Drawing.Size(45, 13);
            this.L_Grass.TabIndex = 313;
            this.L_Grass.Text = "Species";
            // 
            // L_Forme
            // 
            this.L_Forme.AutoSize = true;
            this.L_Forme.Location = new System.Drawing.Point(227, 23);
            this.L_Forme.Name = "L_Forme";
            this.L_Forme.Size = new System.Drawing.Size(36, 13);
            this.L_Forme.TabIndex = 312;
            this.L_Forme.Text = "Forme";
            // 
            // NUP_Forme1
            // 
            this.NUP_Forme1.Location = new System.Drawing.Point(230, 39);
            this.NUP_Forme1.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NUP_Forme1.Name = "NUP_Forme1";
            this.NUP_Forme1.Size = new System.Drawing.Size(41, 20);
            this.NUP_Forme1.TabIndex = 289;
            // 
            // CB_Enc1
            // 
            this.CB_Enc1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Enc1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Enc1.FormattingEnabled = true;
            this.CB_Enc1.Location = new System.Drawing.Point(103, 39);
            this.CB_Enc1.Name = "CB_Enc1";
            this.CB_Enc1.Size = new System.Drawing.Size(121, 21);
            this.CB_Enc1.TabIndex = 288;
            // 
            // L_Rate1
            // 
            this.L_Rate1.AutoSize = true;
            this.L_Rate1.Location = new System.Drawing.Point(60, 42);
            this.L_Rate1.Name = "L_Rate1";
            this.L_Rate1.Size = new System.Drawing.Size(27, 13);
            this.L_Rate1.TabIndex = 290;
            this.L_Rate1.Text = "10%";
            // 
            // CB_TableID
            // 
            this.CB_TableID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_TableID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_TableID.Enabled = false;
            this.CB_TableID.FormattingEnabled = true;
            this.CB_TableID.Location = new System.Drawing.Point(45, 67);
            this.CB_TableID.Name = "CB_TableID";
            this.CB_TableID.Size = new System.Drawing.Size(45, 21);
            this.CB_TableID.TabIndex = 431;
            this.CB_TableID.SelectedIndexChanged += new System.EventHandler(this.updatePanel);
            // 
            // B_Export
            // 
            this.B_Export.Enabled = false;
            this.B_Export.Location = new System.Drawing.Point(236, 39);
            this.B_Export.Name = "B_Export";
            this.B_Export.Size = new System.Drawing.Size(112, 23);
            this.B_Export.TabIndex = 432;
            this.B_Export.Text = "Export Tables";
            this.B_Export.UseVisualStyleBackColor = true;
            this.B_Export.Click += new System.EventHandler(this.B_Export_Click);
            // 
            // SMWE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 425);
            this.Controls.Add(this.B_Export);
            this.Controls.Add(this.CB_TableID);
            this.Controls.Add(this.GB_Encounters);
            this.Controls.Add(this.CB_SlotType);
            this.Controls.Add(this.L_Table);
            this.Controls.Add(this.NUP_Min);
            this.Controls.Add(this.NUP_Max);
            this.Controls.Add(this.L_Min);
            this.Controls.Add(this.L_Max);
            this.Controls.Add(this.B_Randomize);
            this.Controls.Add(this.B_Dump);
            this.Controls.Add(this.L_Location);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.CB_LocationID);
            this.Name = "SMWE";
            this.Text = "Sun Moon Wilds Editor";
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Max)).EndInit();
            this.GB_Encounters.ResumeLayout(false);
            this.GB_Encounters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Forme1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_Randomize;
        private System.Windows.Forms.Button B_Dump;
        private System.Windows.Forms.Label L_Location;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.ComboBox CB_LocationID;
        private System.Windows.Forms.NumericUpDown NUP_Min;
        private System.Windows.Forms.NumericUpDown NUP_Max;
        private System.Windows.Forms.Label L_Min;
        private System.Windows.Forms.Label L_Max;
        private System.Windows.Forms.Label L_Table;
        private System.Windows.Forms.ComboBox CB_SlotType;
        private System.Windows.Forms.GroupBox GB_Encounters;
        private System.Windows.Forms.NumericUpDown NUP_Forme8;
        private System.Windows.Forms.ComboBox CB_Enc8;
        private System.Windows.Forms.Label L_Rate8;
        private System.Windows.Forms.NumericUpDown NUP_Forme7;
        private System.Windows.Forms.ComboBox CB_Enc7;
        private System.Windows.Forms.Label L_Rate7;
        private System.Windows.Forms.NumericUpDown NUP_Forme6;
        private System.Windows.Forms.ComboBox CB_Enc6;
        private System.Windows.Forms.Label L_Rate6;
        private System.Windows.Forms.NumericUpDown NUP_Forme5;
        private System.Windows.Forms.ComboBox CB_Enc5;
        private System.Windows.Forms.Label L_Rate5;
        private System.Windows.Forms.NumericUpDown NUP_Forme4;
        private System.Windows.Forms.ComboBox CB_Enc4;
        private System.Windows.Forms.Label L_Rate4;
        private System.Windows.Forms.NumericUpDown NUP_Forme3;
        private System.Windows.Forms.ComboBox CB_Enc3;
        private System.Windows.Forms.Label L_Rate3;
        private System.Windows.Forms.NumericUpDown NUP_Forme2;
        private System.Windows.Forms.ComboBox CB_Enc2;
        private System.Windows.Forms.Label L_Rate2;
        private System.Windows.Forms.Label L_Rate;
        private System.Windows.Forms.Label L_Grass;
        private System.Windows.Forms.Label L_Forme;
        private System.Windows.Forms.NumericUpDown NUP_Forme1;
        private System.Windows.Forms.ComboBox CB_Enc1;
        private System.Windows.Forms.Label L_Rate1;
        private System.Windows.Forms.NumericUpDown NUP_Forme10;
        private System.Windows.Forms.ComboBox CB_Enc10;
        private System.Windows.Forms.Label L_Rate10;
        private System.Windows.Forms.NumericUpDown NUP_Forme9;
        private System.Windows.Forms.ComboBox CB_Enc9;
        private System.Windows.Forms.Label L_Rate9;
        private System.Windows.Forms.ComboBox CB_TableID;
        private System.Windows.Forms.Button B_Export;
    }
}

