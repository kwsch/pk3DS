namespace pk3DS
{
    partial class Evolution
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
            this.CB_Species = new System.Windows.Forms.ComboBox();
            this.L_Species = new System.Windows.Forms.Label();
            this.B_Dump = new System.Windows.Forms.Button();
            this.CB_M1 = new System.Windows.Forms.ComboBox();
            this.L_M1 = new System.Windows.Forms.Label();
            this.CB_I1 = new System.Windows.Forms.ComboBox();
            this.CB_P1 = new System.Windows.Forms.ComboBox();
            this.CB_P2 = new System.Windows.Forms.ComboBox();
            this.CB_I2 = new System.Windows.Forms.ComboBox();
            this.L_M2 = new System.Windows.Forms.Label();
            this.CB_M2 = new System.Windows.Forms.ComboBox();
            this.CB_P3 = new System.Windows.Forms.ComboBox();
            this.CB_I3 = new System.Windows.Forms.ComboBox();
            this.L_M3 = new System.Windows.Forms.Label();
            this.CB_M3 = new System.Windows.Forms.ComboBox();
            this.CB_P4 = new System.Windows.Forms.ComboBox();
            this.CB_I4 = new System.Windows.Forms.ComboBox();
            this.L_M4 = new System.Windows.Forms.Label();
            this.CB_M4 = new System.Windows.Forms.ComboBox();
            this.CB_P5 = new System.Windows.Forms.ComboBox();
            this.CB_I5 = new System.Windows.Forms.ComboBox();
            this.L_M5 = new System.Windows.Forms.Label();
            this.CB_M5 = new System.Windows.Forms.ComboBox();
            this.CB_P8 = new System.Windows.Forms.ComboBox();
            this.CB_I8 = new System.Windows.Forms.ComboBox();
            this.L_M8 = new System.Windows.Forms.Label();
            this.CB_M8 = new System.Windows.Forms.ComboBox();
            this.CB_P7 = new System.Windows.Forms.ComboBox();
            this.CB_I7 = new System.Windows.Forms.ComboBox();
            this.L_M7 = new System.Windows.Forms.Label();
            this.CB_M7 = new System.Windows.Forms.ComboBox();
            this.CB_P6 = new System.Windows.Forms.ComboBox();
            this.CB_I6 = new System.Windows.Forms.ComboBox();
            this.L_M6 = new System.Windows.Forms.Label();
            this.CB_M6 = new System.Windows.Forms.ComboBox();
            this.PB_1 = new System.Windows.Forms.PictureBox();
            this.PB_2 = new System.Windows.Forms.PictureBox();
            this.PB_4 = new System.Windows.Forms.PictureBox();
            this.PB_3 = new System.Windows.Forms.PictureBox();
            this.PB_6 = new System.Windows.Forms.PictureBox();
            this.PB_5 = new System.Windows.Forms.PictureBox();
            this.PB_8 = new System.Windows.Forms.PictureBox();
            this.PB_7 = new System.Windows.Forms.PictureBox();
            this.B_RandAll = new System.Windows.Forms.Button();
            this.GB_Randomizer = new System.Windows.Forms.GroupBox();
            this.L_Protip = new System.Windows.Forms.Label();
            this.CHK_BST = new System.Windows.Forms.CheckBox();
            this.CHK_Type = new System.Windows.Forms.CheckBox();
            this.CHK_Exp = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.PB_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_7)).BeginInit();
            this.GB_Randomizer.SuspendLayout();
            this.SuspendLayout();
            // 
            // CB_Species
            // 
            this.CB_Species.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Species.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Species.FormattingEnabled = true;
            this.CB_Species.Location = new System.Drawing.Point(66, 12);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(121, 21);
            this.CB_Species.TabIndex = 1;
            this.CB_Species.SelectedIndexChanged += new System.EventHandler(this.changeEntry);
            // 
            // L_Species
            // 
            this.L_Species.AutoSize = true;
            this.L_Species.Location = new System.Drawing.Point(12, 15);
            this.L_Species.Name = "L_Species";
            this.L_Species.Size = new System.Drawing.Size(48, 13);
            this.L_Species.TabIndex = 2;
            this.L_Species.Text = "Species:";
            // 
            // B_Dump
            // 
            this.B_Dump.Location = new System.Drawing.Point(193, 11);
            this.B_Dump.Name = "B_Dump";
            this.B_Dump.Size = new System.Drawing.Size(59, 23);
            this.B_Dump.TabIndex = 5;
            this.B_Dump.Text = "Dump";
            this.B_Dump.UseVisualStyleBackColor = true;
            this.B_Dump.Click += new System.EventHandler(this.B_Dump_Click);
            // 
            // CB_M1
            // 
            this.CB_M1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_M1.DropDownWidth = 200;
            this.CB_M1.FormattingEnabled = true;
            this.CB_M1.Location = new System.Drawing.Point(79, 46);
            this.CB_M1.MaxDropDownItems = 10;
            this.CB_M1.Name = "CB_M1";
            this.CB_M1.Size = new System.Drawing.Size(150, 21);
            this.CB_M1.TabIndex = 6;
            this.CB_M1.SelectedIndexChanged += new System.EventHandler(this.changeMethod);
            // 
            // L_M1
            // 
            this.L_M1.AutoSize = true;
            this.L_M1.Location = new System.Drawing.Point(18, 49);
            this.L_M1.Name = "L_M1";
            this.L_M1.Size = new System.Drawing.Size(55, 13);
            this.L_M1.TabIndex = 7;
            this.L_M1.Text = "Method 1:";
            // 
            // CB_I1
            // 
            this.CB_I1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_I1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_I1.FormattingEnabled = true;
            this.CB_I1.Location = new System.Drawing.Point(277, 46);
            this.CB_I1.Name = "CB_I1";
            this.CB_I1.Size = new System.Drawing.Size(101, 21);
            this.CB_I1.TabIndex = 9;
            this.CB_I1.SelectedIndexChanged += new System.EventHandler(this.changeInto);
            // 
            // CB_P1
            // 
            this.CB_P1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_P1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_P1.FormattingEnabled = true;
            this.CB_P1.Location = new System.Drawing.Point(79, 69);
            this.CB_P1.Name = "CB_P1";
            this.CB_P1.Size = new System.Drawing.Size(121, 21);
            this.CB_P1.TabIndex = 10;
            // 
            // CB_P2
            // 
            this.CB_P2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_P2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_P2.FormattingEnabled = true;
            this.CB_P2.Location = new System.Drawing.Point(79, 123);
            this.CB_P2.Name = "CB_P2";
            this.CB_P2.Size = new System.Drawing.Size(121, 21);
            this.CB_P2.TabIndex = 16;
            // 
            // CB_I2
            // 
            this.CB_I2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_I2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_I2.FormattingEnabled = true;
            this.CB_I2.Location = new System.Drawing.Point(277, 100);
            this.CB_I2.Name = "CB_I2";
            this.CB_I2.Size = new System.Drawing.Size(101, 21);
            this.CB_I2.TabIndex = 15;
            this.CB_I2.SelectedIndexChanged += new System.EventHandler(this.changeInto);
            // 
            // L_M2
            // 
            this.L_M2.AutoSize = true;
            this.L_M2.Location = new System.Drawing.Point(18, 103);
            this.L_M2.Name = "L_M2";
            this.L_M2.Size = new System.Drawing.Size(55, 13);
            this.L_M2.TabIndex = 13;
            this.L_M2.Text = "Method 2:";
            // 
            // CB_M2
            // 
            this.CB_M2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_M2.DropDownWidth = 200;
            this.CB_M2.FormattingEnabled = true;
            this.CB_M2.Location = new System.Drawing.Point(79, 100);
            this.CB_M2.MaxDropDownItems = 10;
            this.CB_M2.Name = "CB_M2";
            this.CB_M2.Size = new System.Drawing.Size(150, 21);
            this.CB_M2.TabIndex = 12;
            this.CB_M2.SelectedIndexChanged += new System.EventHandler(this.changeMethod);
            // 
            // CB_P3
            // 
            this.CB_P3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_P3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_P3.FormattingEnabled = true;
            this.CB_P3.Location = new System.Drawing.Point(79, 177);
            this.CB_P3.Name = "CB_P3";
            this.CB_P3.Size = new System.Drawing.Size(121, 21);
            this.CB_P3.TabIndex = 22;
            // 
            // CB_I3
            // 
            this.CB_I3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_I3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_I3.FormattingEnabled = true;
            this.CB_I3.Location = new System.Drawing.Point(277, 154);
            this.CB_I3.Name = "CB_I3";
            this.CB_I3.Size = new System.Drawing.Size(101, 21);
            this.CB_I3.TabIndex = 21;
            this.CB_I3.SelectedIndexChanged += new System.EventHandler(this.changeInto);
            // 
            // L_M3
            // 
            this.L_M3.AutoSize = true;
            this.L_M3.Location = new System.Drawing.Point(18, 157);
            this.L_M3.Name = "L_M3";
            this.L_M3.Size = new System.Drawing.Size(55, 13);
            this.L_M3.TabIndex = 19;
            this.L_M3.Text = "Method 3:";
            // 
            // CB_M3
            // 
            this.CB_M3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_M3.DropDownWidth = 200;
            this.CB_M3.FormattingEnabled = true;
            this.CB_M3.Location = new System.Drawing.Point(79, 154);
            this.CB_M3.MaxDropDownItems = 10;
            this.CB_M3.Name = "CB_M3";
            this.CB_M3.Size = new System.Drawing.Size(150, 21);
            this.CB_M3.TabIndex = 18;
            this.CB_M3.SelectedIndexChanged += new System.EventHandler(this.changeMethod);
            // 
            // CB_P4
            // 
            this.CB_P4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_P4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_P4.FormattingEnabled = true;
            this.CB_P4.Location = new System.Drawing.Point(79, 231);
            this.CB_P4.Name = "CB_P4";
            this.CB_P4.Size = new System.Drawing.Size(121, 21);
            this.CB_P4.TabIndex = 28;
            // 
            // CB_I4
            // 
            this.CB_I4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_I4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_I4.FormattingEnabled = true;
            this.CB_I4.Location = new System.Drawing.Point(277, 208);
            this.CB_I4.Name = "CB_I4";
            this.CB_I4.Size = new System.Drawing.Size(101, 21);
            this.CB_I4.TabIndex = 27;
            this.CB_I4.SelectedIndexChanged += new System.EventHandler(this.changeInto);
            // 
            // L_M4
            // 
            this.L_M4.AutoSize = true;
            this.L_M4.Location = new System.Drawing.Point(18, 211);
            this.L_M4.Name = "L_M4";
            this.L_M4.Size = new System.Drawing.Size(55, 13);
            this.L_M4.TabIndex = 25;
            this.L_M4.Text = "Method 4:";
            // 
            // CB_M4
            // 
            this.CB_M4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_M4.DropDownWidth = 200;
            this.CB_M4.FormattingEnabled = true;
            this.CB_M4.Location = new System.Drawing.Point(79, 208);
            this.CB_M4.MaxDropDownItems = 10;
            this.CB_M4.Name = "CB_M4";
            this.CB_M4.Size = new System.Drawing.Size(150, 21);
            this.CB_M4.TabIndex = 24;
            this.CB_M4.SelectedIndexChanged += new System.EventHandler(this.changeMethod);
            // 
            // CB_P5
            // 
            this.CB_P5.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_P5.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_P5.FormattingEnabled = true;
            this.CB_P5.Location = new System.Drawing.Point(79, 285);
            this.CB_P5.Name = "CB_P5";
            this.CB_P5.Size = new System.Drawing.Size(121, 21);
            this.CB_P5.TabIndex = 34;
            // 
            // CB_I5
            // 
            this.CB_I5.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_I5.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_I5.FormattingEnabled = true;
            this.CB_I5.Location = new System.Drawing.Point(277, 262);
            this.CB_I5.Name = "CB_I5";
            this.CB_I5.Size = new System.Drawing.Size(101, 21);
            this.CB_I5.TabIndex = 33;
            this.CB_I5.SelectedIndexChanged += new System.EventHandler(this.changeInto);
            // 
            // L_M5
            // 
            this.L_M5.AutoSize = true;
            this.L_M5.Location = new System.Drawing.Point(18, 265);
            this.L_M5.Name = "L_M5";
            this.L_M5.Size = new System.Drawing.Size(55, 13);
            this.L_M5.TabIndex = 31;
            this.L_M5.Text = "Method 5:";
            // 
            // CB_M5
            // 
            this.CB_M5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_M5.DropDownWidth = 200;
            this.CB_M5.FormattingEnabled = true;
            this.CB_M5.Location = new System.Drawing.Point(79, 262);
            this.CB_M5.MaxDropDownItems = 10;
            this.CB_M5.Name = "CB_M5";
            this.CB_M5.Size = new System.Drawing.Size(150, 21);
            this.CB_M5.TabIndex = 30;
            this.CB_M5.SelectedIndexChanged += new System.EventHandler(this.changeMethod);
            // 
            // CB_P8
            // 
            this.CB_P8.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_P8.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_P8.FormattingEnabled = true;
            this.CB_P8.Location = new System.Drawing.Point(79, 447);
            this.CB_P8.Name = "CB_P8";
            this.CB_P8.Size = new System.Drawing.Size(121, 21);
            this.CB_P8.TabIndex = 52;
            // 
            // CB_I8
            // 
            this.CB_I8.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_I8.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_I8.FormattingEnabled = true;
            this.CB_I8.Location = new System.Drawing.Point(277, 424);
            this.CB_I8.Name = "CB_I8";
            this.CB_I8.Size = new System.Drawing.Size(101, 21);
            this.CB_I8.TabIndex = 51;
            this.CB_I8.SelectedIndexChanged += new System.EventHandler(this.changeInto);
            // 
            // L_M8
            // 
            this.L_M8.AutoSize = true;
            this.L_M8.Location = new System.Drawing.Point(18, 427);
            this.L_M8.Name = "L_M8";
            this.L_M8.Size = new System.Drawing.Size(55, 13);
            this.L_M8.TabIndex = 49;
            this.L_M8.Text = "Method 8:";
            // 
            // CB_M8
            // 
            this.CB_M8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_M8.DropDownWidth = 200;
            this.CB_M8.FormattingEnabled = true;
            this.CB_M8.Location = new System.Drawing.Point(79, 424);
            this.CB_M8.MaxDropDownItems = 10;
            this.CB_M8.Name = "CB_M8";
            this.CB_M8.Size = new System.Drawing.Size(150, 21);
            this.CB_M8.TabIndex = 48;
            this.CB_M8.SelectedIndexChanged += new System.EventHandler(this.changeMethod);
            // 
            // CB_P7
            // 
            this.CB_P7.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_P7.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_P7.FormattingEnabled = true;
            this.CB_P7.Location = new System.Drawing.Point(79, 393);
            this.CB_P7.Name = "CB_P7";
            this.CB_P7.Size = new System.Drawing.Size(121, 21);
            this.CB_P7.TabIndex = 46;
            // 
            // CB_I7
            // 
            this.CB_I7.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_I7.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_I7.FormattingEnabled = true;
            this.CB_I7.Location = new System.Drawing.Point(277, 370);
            this.CB_I7.Name = "CB_I7";
            this.CB_I7.Size = new System.Drawing.Size(101, 21);
            this.CB_I7.TabIndex = 45;
            this.CB_I7.SelectedIndexChanged += new System.EventHandler(this.changeInto);
            // 
            // L_M7
            // 
            this.L_M7.AutoSize = true;
            this.L_M7.Location = new System.Drawing.Point(18, 373);
            this.L_M7.Name = "L_M7";
            this.L_M7.Size = new System.Drawing.Size(55, 13);
            this.L_M7.TabIndex = 43;
            this.L_M7.Text = "Method 7:";
            // 
            // CB_M7
            // 
            this.CB_M7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_M7.DropDownWidth = 200;
            this.CB_M7.FormattingEnabled = true;
            this.CB_M7.Location = new System.Drawing.Point(79, 370);
            this.CB_M7.MaxDropDownItems = 10;
            this.CB_M7.Name = "CB_M7";
            this.CB_M7.Size = new System.Drawing.Size(150, 21);
            this.CB_M7.TabIndex = 42;
            this.CB_M7.SelectedIndexChanged += new System.EventHandler(this.changeMethod);
            // 
            // CB_P6
            // 
            this.CB_P6.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_P6.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_P6.FormattingEnabled = true;
            this.CB_P6.Location = new System.Drawing.Point(79, 339);
            this.CB_P6.Name = "CB_P6";
            this.CB_P6.Size = new System.Drawing.Size(121, 21);
            this.CB_P6.TabIndex = 40;
            // 
            // CB_I6
            // 
            this.CB_I6.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_I6.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_I6.FormattingEnabled = true;
            this.CB_I6.Location = new System.Drawing.Point(277, 316);
            this.CB_I6.Name = "CB_I6";
            this.CB_I6.Size = new System.Drawing.Size(101, 21);
            this.CB_I6.TabIndex = 39;
            this.CB_I6.SelectedIndexChanged += new System.EventHandler(this.changeInto);
            // 
            // L_M6
            // 
            this.L_M6.AutoSize = true;
            this.L_M6.Location = new System.Drawing.Point(18, 319);
            this.L_M6.Name = "L_M6";
            this.L_M6.Size = new System.Drawing.Size(55, 13);
            this.L_M6.TabIndex = 37;
            this.L_M6.Text = "Method 6:";
            // 
            // CB_M6
            // 
            this.CB_M6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_M6.DropDownWidth = 200;
            this.CB_M6.FormattingEnabled = true;
            this.CB_M6.Location = new System.Drawing.Point(79, 316);
            this.CB_M6.MaxDropDownItems = 10;
            this.CB_M6.Name = "CB_M6";
            this.CB_M6.Size = new System.Drawing.Size(150, 21);
            this.CB_M6.TabIndex = 36;
            this.CB_M6.SelectedIndexChanged += new System.EventHandler(this.changeMethod);
            // 
            // PB_1
            // 
            this.PB_1.Location = new System.Drawing.Point(235, 46);
            this.PB_1.Name = "PB_1";
            this.PB_1.Size = new System.Drawing.Size(40, 30);
            this.PB_1.TabIndex = 54;
            this.PB_1.TabStop = false;
            // 
            // PB_2
            // 
            this.PB_2.Location = new System.Drawing.Point(235, 100);
            this.PB_2.Name = "PB_2";
            this.PB_2.Size = new System.Drawing.Size(40, 30);
            this.PB_2.TabIndex = 55;
            this.PB_2.TabStop = false;
            // 
            // PB_4
            // 
            this.PB_4.Location = new System.Drawing.Point(235, 208);
            this.PB_4.Name = "PB_4";
            this.PB_4.Size = new System.Drawing.Size(40, 30);
            this.PB_4.TabIndex = 57;
            this.PB_4.TabStop = false;
            // 
            // PB_3
            // 
            this.PB_3.Location = new System.Drawing.Point(235, 154);
            this.PB_3.Name = "PB_3";
            this.PB_3.Size = new System.Drawing.Size(40, 30);
            this.PB_3.TabIndex = 56;
            this.PB_3.TabStop = false;
            // 
            // PB_6
            // 
            this.PB_6.Location = new System.Drawing.Point(235, 316);
            this.PB_6.Name = "PB_6";
            this.PB_6.Size = new System.Drawing.Size(40, 30);
            this.PB_6.TabIndex = 59;
            this.PB_6.TabStop = false;
            // 
            // PB_5
            // 
            this.PB_5.Location = new System.Drawing.Point(235, 262);
            this.PB_5.Name = "PB_5";
            this.PB_5.Size = new System.Drawing.Size(40, 30);
            this.PB_5.TabIndex = 58;
            this.PB_5.TabStop = false;
            // 
            // PB_8
            // 
            this.PB_8.Location = new System.Drawing.Point(235, 424);
            this.PB_8.Name = "PB_8";
            this.PB_8.Size = new System.Drawing.Size(40, 30);
            this.PB_8.TabIndex = 61;
            this.PB_8.TabStop = false;
            // 
            // PB_7
            // 
            this.PB_7.Location = new System.Drawing.Point(235, 370);
            this.PB_7.Name = "PB_7";
            this.PB_7.Size = new System.Drawing.Size(40, 30);
            this.PB_7.TabIndex = 60;
            this.PB_7.TabStop = false;
            // 
            // B_RandAll
            // 
            this.B_RandAll.Location = new System.Drawing.Point(251, 47);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(100, 23);
            this.B_RandAll.TabIndex = 62;
            this.B_RandAll.Text = "Randomize All";
            this.B_RandAll.UseVisualStyleBackColor = true;
            this.B_RandAll.Click += new System.EventHandler(this.B_RandAll_Click);
            // 
            // GB_Randomizer
            // 
            this.GB_Randomizer.Controls.Add(this.L_Protip);
            this.GB_Randomizer.Controls.Add(this.CHK_BST);
            this.GB_Randomizer.Controls.Add(this.CHK_Type);
            this.GB_Randomizer.Controls.Add(this.CHK_Exp);
            this.GB_Randomizer.Controls.Add(this.B_RandAll);
            this.GB_Randomizer.Location = new System.Drawing.Point(21, 474);
            this.GB_Randomizer.Name = "GB_Randomizer";
            this.GB_Randomizer.Size = new System.Drawing.Size(357, 76);
            this.GB_Randomizer.TabIndex = 63;
            this.GB_Randomizer.TabStop = false;
            this.GB_Randomizer.Text = "Randomizer Options";
            // 
            // L_Protip
            // 
            this.L_Protip.AutoSize = true;
            this.L_Protip.ForeColor = System.Drawing.Color.Red;
            this.L_Protip.Location = new System.Drawing.Point(198, 11);
            this.L_Protip.Name = "L_Protip";
            this.L_Protip.Size = new System.Drawing.Size(153, 13);
            this.L_Protip.TabIndex = 66;
            this.L_Protip.Text = "(Protip: Difficulty++ in Personal)";
            // 
            // CHK_BST
            // 
            this.CHK_BST.AutoSize = true;
            this.CHK_BST.Checked = true;
            this.CHK_BST.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_BST.Location = new System.Drawing.Point(6, 53);
            this.CHK_BST.Name = "CHK_BST";
            this.CHK_BST.Size = new System.Drawing.Size(179, 17);
            this.CHK_BST.TabIndex = 65;
            this.CHK_BST.Text = "Share a similar BST as Evolution";
            this.CHK_BST.UseVisualStyleBackColor = true;
            // 
            // CHK_Type
            // 
            this.CHK_Type.AutoSize = true;
            this.CHK_Type.Checked = true;
            this.CHK_Type.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_Type.Location = new System.Drawing.Point(6, 38);
            this.CHK_Type.Name = "CHK_Type";
            this.CHK_Type.Size = new System.Drawing.Size(200, 17);
            this.CHK_Type.TabIndex = 64;
            this.CHK_Type.Text = "Share at least one Type as Evolution";
            this.CHK_Type.UseVisualStyleBackColor = true;
            // 
            // CHK_Exp
            // 
            this.CHK_Exp.AutoSize = true;
            this.CHK_Exp.Checked = true;
            this.CHK_Exp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_Exp.Location = new System.Drawing.Point(6, 23);
            this.CHK_Exp.Name = "CHK_Exp";
            this.CHK_Exp.Size = new System.Drawing.Size(219, 17);
            this.CHK_Exp.TabIndex = 63;
            this.CHK_Exp.Text = "Share the same Exp Growth as Evolution";
            this.CHK_Exp.UseVisualStyleBackColor = true;
            // 
            // Evolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 562);
            this.Controls.Add(this.GB_Randomizer);
            this.Controls.Add(this.PB_8);
            this.Controls.Add(this.PB_7);
            this.Controls.Add(this.PB_6);
            this.Controls.Add(this.PB_5);
            this.Controls.Add(this.PB_4);
            this.Controls.Add(this.PB_3);
            this.Controls.Add(this.PB_2);
            this.Controls.Add(this.PB_1);
            this.Controls.Add(this.CB_P8);
            this.Controls.Add(this.CB_I8);
            this.Controls.Add(this.L_M8);
            this.Controls.Add(this.CB_M8);
            this.Controls.Add(this.CB_P7);
            this.Controls.Add(this.CB_I7);
            this.Controls.Add(this.L_M7);
            this.Controls.Add(this.CB_M7);
            this.Controls.Add(this.CB_P6);
            this.Controls.Add(this.CB_I6);
            this.Controls.Add(this.L_M6);
            this.Controls.Add(this.CB_M6);
            this.Controls.Add(this.CB_P5);
            this.Controls.Add(this.CB_I5);
            this.Controls.Add(this.L_M5);
            this.Controls.Add(this.CB_M5);
            this.Controls.Add(this.CB_P4);
            this.Controls.Add(this.CB_I4);
            this.Controls.Add(this.L_M4);
            this.Controls.Add(this.CB_M4);
            this.Controls.Add(this.CB_P3);
            this.Controls.Add(this.CB_I3);
            this.Controls.Add(this.L_M3);
            this.Controls.Add(this.CB_M3);
            this.Controls.Add(this.CB_P2);
            this.Controls.Add(this.CB_I2);
            this.Controls.Add(this.L_M2);
            this.Controls.Add(this.CB_M2);
            this.Controls.Add(this.CB_P1);
            this.Controls.Add(this.CB_I1);
            this.Controls.Add(this.L_M1);
            this.Controls.Add(this.CB_M1);
            this.Controls.Add(this.B_Dump);
            this.Controls.Add(this.L_Species);
            this.Controls.Add(this.CB_Species);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(410, 600);
            this.MinimumSize = new System.Drawing.Size(410, 515);
            this.Name = "Evolution";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Evolution Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PB_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_7)).EndInit();
            this.GB_Randomizer.ResumeLayout(false);
            this.GB_Randomizer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.Label L_Species;
        private System.Windows.Forms.Button B_Dump;
        private System.Windows.Forms.ComboBox CB_M1;
        private System.Windows.Forms.Label L_M1;
        private System.Windows.Forms.ComboBox CB_I1;
        private System.Windows.Forms.ComboBox CB_P1;
        private System.Windows.Forms.ComboBox CB_P2;
        private System.Windows.Forms.ComboBox CB_I2;
        private System.Windows.Forms.Label L_M2;
        private System.Windows.Forms.ComboBox CB_M2;
        private System.Windows.Forms.ComboBox CB_P3;
        private System.Windows.Forms.ComboBox CB_I3;
        private System.Windows.Forms.Label L_M3;
        private System.Windows.Forms.ComboBox CB_M3;
        private System.Windows.Forms.ComboBox CB_P4;
        private System.Windows.Forms.ComboBox CB_I4;
        private System.Windows.Forms.Label L_M4;
        private System.Windows.Forms.ComboBox CB_M4;
        private System.Windows.Forms.ComboBox CB_P5;
        private System.Windows.Forms.ComboBox CB_I5;
        private System.Windows.Forms.Label L_M5;
        private System.Windows.Forms.ComboBox CB_M5;
        private System.Windows.Forms.ComboBox CB_P8;
        private System.Windows.Forms.ComboBox CB_I8;
        private System.Windows.Forms.Label L_M8;
        private System.Windows.Forms.ComboBox CB_M8;
        private System.Windows.Forms.ComboBox CB_P7;
        private System.Windows.Forms.ComboBox CB_I7;
        private System.Windows.Forms.Label L_M7;
        private System.Windows.Forms.ComboBox CB_M7;
        private System.Windows.Forms.ComboBox CB_P6;
        private System.Windows.Forms.ComboBox CB_I6;
        private System.Windows.Forms.Label L_M6;
        private System.Windows.Forms.ComboBox CB_M6;
        private System.Windows.Forms.PictureBox PB_1;
        private System.Windows.Forms.PictureBox PB_2;
        private System.Windows.Forms.PictureBox PB_4;
        private System.Windows.Forms.PictureBox PB_3;
        private System.Windows.Forms.PictureBox PB_6;
        private System.Windows.Forms.PictureBox PB_5;
        private System.Windows.Forms.PictureBox PB_8;
        private System.Windows.Forms.PictureBox PB_7;
        private System.Windows.Forms.Button B_RandAll;
        private System.Windows.Forms.GroupBox GB_Randomizer;
        private System.Windows.Forms.CheckBox CHK_Exp;
        private System.Windows.Forms.CheckBox CHK_Type;
        private System.Windows.Forms.CheckBox CHK_BST;
        private System.Windows.Forms.Label L_Protip;
    }
}