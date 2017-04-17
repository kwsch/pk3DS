namespace pk3DS
{
    partial class ShinyRate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShinyRate));
            this.L_Note = new System.Windows.Forms.Label();
            this.NUD_Rerolls = new System.Windows.Forms.NumericUpDown();
            this.L_Overall = new System.Windows.Forms.Label();
            this.B_Cancel = new System.Windows.Forms.Button();
            this.B_Save = new System.Windows.Forms.Button();
            this.L_Rerolls = new System.Windows.Forms.Label();
            this.B_RestoreOriginal = new System.Windows.Forms.Button();
            this.NUD_Rate = new System.Windows.Forms.NumericUpDown();
            this.L_RerollCount = new System.Windows.Forms.Label();
            this.L_RerollOverall = new System.Windows.Forms.Label();
            this.GB_RerollHelper = new System.Windows.Forms.GroupBox();
            this.GB_Rerolls = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Rerolls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Rate)).BeginInit();
            this.GB_RerollHelper.SuspendLayout();
            this.GB_Rerolls.SuspendLayout();
            this.SuspendLayout();
            // 
            // L_Note
            // 
            this.L_Note.AutoSize = true;
            this.L_Note.Location = new System.Drawing.Point(12, 9);
            this.L_Note.Name = "L_Note";
            this.L_Note.Size = new System.Drawing.Size(340, 91);
            this.L_Note.TabIndex = 0;
            this.L_Note.Text = resources.GetString("L_Note.Text");
            // 
            // NUD_Rerolls
            // 
            this.NUD_Rerolls.Location = new System.Drawing.Point(54, 21);
            this.NUD_Rerolls.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NUD_Rerolls.Name = "NUD_Rerolls";
            this.NUD_Rerolls.Size = new System.Drawing.Size(65, 20);
            this.NUD_Rerolls.TabIndex = 1;
            this.NUD_Rerolls.Value = new decimal(new int[] {
            125,
            0,
            0,
            0});
            this.NUD_Rerolls.ValueChanged += new System.EventHandler(this.changeRerolls);
            // 
            // L_Overall
            // 
            this.L_Overall.AutoSize = true;
            this.L_Overall.ForeColor = System.Drawing.SystemColors.ControlText;
            this.L_Overall.Location = new System.Drawing.Point(122, 23);
            this.L_Overall.Name = "L_Overall";
            this.L_Overall.Size = new System.Drawing.Size(28, 13);
            this.L_Overall.TabIndex = 2;
            this.L_Overall.Text = "PCT";
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Cancel.Location = new System.Drawing.Point(216, 236);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(75, 23);
            this.B_Cancel.TabIndex = 4;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.UseVisualStyleBackColor = true;
            this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
            // 
            // B_Save
            // 
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(297, 236);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(75, 23);
            this.B_Save.TabIndex = 3;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // L_Rerolls
            // 
            this.L_Rerolls.AutoSize = true;
            this.L_Rerolls.ForeColor = System.Drawing.SystemColors.ControlText;
            this.L_Rerolls.Location = new System.Drawing.Point(6, 23);
            this.L_Rerolls.Name = "L_Rerolls";
            this.L_Rerolls.Size = new System.Drawing.Size(42, 13);
            this.L_Rerolls.TabIndex = 5;
            this.L_Rerolls.Text = "Rerolls:";
            // 
            // B_RestoreOriginal
            // 
            this.B_RestoreOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_RestoreOriginal.Location = new System.Drawing.Point(15, 236);
            this.B_RestoreOriginal.Name = "B_RestoreOriginal";
            this.B_RestoreOriginal.Size = new System.Drawing.Size(168, 23);
            this.B_RestoreOriginal.TabIndex = 6;
            this.B_RestoreOriginal.Text = "Restore Original Rate";
            this.B_RestoreOriginal.UseVisualStyleBackColor = true;
            this.B_RestoreOriginal.Click += new System.EventHandler(this.B_RestoreOriginal_Click);
            // 
            // NUD_Rate
            // 
            this.NUD_Rate.DecimalPlaces = 2;
            this.NUD_Rate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NUD_Rate.Location = new System.Drawing.Point(81, 13);
            this.NUD_Rate.Name = "NUD_Rate";
            this.NUD_Rate.Size = new System.Drawing.Size(65, 20);
            this.NUD_Rate.TabIndex = 7;
            this.NUD_Rate.ValueChanged += new System.EventHandler(this.changePercent);
            // 
            // L_RerollCount
            // 
            this.L_RerollCount.AutoSize = true;
            this.L_RerollCount.Location = new System.Drawing.Point(46, 36);
            this.L_RerollCount.Name = "L_RerollCount";
            this.L_RerollCount.Size = new System.Drawing.Size(47, 13);
            this.L_RerollCount.TabIndex = 8;
            this.L_RerollCount.Text = "Count: 0";
            // 
            // L_RerollOverall
            // 
            this.L_RerollOverall.AutoSize = true;
            this.L_RerollOverall.Location = new System.Drawing.Point(24, 15);
            this.L_RerollOverall.Name = "L_RerollOverall";
            this.L_RerollOverall.Size = new System.Drawing.Size(51, 13);
            this.L_RerollOverall.TabIndex = 9;
            this.L_RerollOverall.Text = "Overall%:";
            // 
            // GB_RerollHelper
            // 
            this.GB_RerollHelper.Controls.Add(this.NUD_Rate);
            this.GB_RerollHelper.Controls.Add(this.L_RerollOverall);
            this.GB_RerollHelper.Controls.Add(this.L_RerollCount);
            this.GB_RerollHelper.Location = new System.Drawing.Point(210, 125);
            this.GB_RerollHelper.Name = "GB_RerollHelper";
            this.GB_RerollHelper.Size = new System.Drawing.Size(156, 53);
            this.GB_RerollHelper.TabIndex = 11;
            this.GB_RerollHelper.TabStop = false;
            this.GB_RerollHelper.Text = "Reroll Helper";
            // 
            // GB_Rerolls
            // 
            this.GB_Rerolls.Controls.Add(this.NUD_Rerolls);
            this.GB_Rerolls.Controls.Add(this.L_Rerolls);
            this.GB_Rerolls.Controls.Add(this.L_Overall);
            this.GB_Rerolls.ForeColor = System.Drawing.Color.Red;
            this.GB_Rerolls.Location = new System.Drawing.Point(6, 125);
            this.GB_Rerolls.Name = "GB_Rerolls";
            this.GB_Rerolls.Size = new System.Drawing.Size(171, 53);
            this.GB_Rerolls.TabIndex = 12;
            this.GB_Rerolls.TabStop = false;
            this.GB_Rerolls.Text = "PID Generation Loop Rerolls";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 52);
            this.label1.TabIndex = 13;
            this.label1.Text = "Note:\r\nThe above reroll count will overwrite the existing code.\r\n\r\nTo revert chan" +
    "ges, use the button below.";
            // 
            // ShinyRate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 271);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GB_Rerolls);
            this.Controls.Add(this.GB_RerollHelper);
            this.Controls.Add(this.B_RestoreOriginal);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.L_Note);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 310);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 150);
            this.Name = "ShinyRate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shiny Rate Editor";
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Rerolls)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Rate)).EndInit();
            this.GB_RerollHelper.ResumeLayout(false);
            this.GB_RerollHelper.PerformLayout();
            this.GB_Rerolls.ResumeLayout(false);
            this.GB_Rerolls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label L_Note;
        private System.Windows.Forms.NumericUpDown NUD_Rerolls;
        private System.Windows.Forms.Label L_Overall;
        private System.Windows.Forms.Button B_Cancel;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.Label L_Rerolls;
        private System.Windows.Forms.Button B_RestoreOriginal;
        private System.Windows.Forms.NumericUpDown NUD_Rate;
        private System.Windows.Forms.Label L_RerollCount;
        private System.Windows.Forms.Label L_RerollOverall;
        private System.Windows.Forms.GroupBox GB_RerollHelper;
        private System.Windows.Forms.GroupBox GB_Rerolls;
        private System.Windows.Forms.Label label1;
    }
}