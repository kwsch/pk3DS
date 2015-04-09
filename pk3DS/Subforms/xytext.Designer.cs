namespace pk3DS
{
    partial class xytext
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
            this.CB_Entry = new System.Windows.Forms.ComboBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.B_AddLine = new System.Windows.Forms.Button();
            this.B_RemoveLine = new System.Windows.Forms.Button();
            this.B_Export = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.B_Import = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // CB_Entry
            // 
            this.CB_Entry.FormattingEnabled = true;
            this.CB_Entry.Location = new System.Drawing.Point(68, 7);
            this.CB_Entry.Name = "CB_Entry";
            this.CB_Entry.Size = new System.Drawing.Size(80, 21);
            this.CB_Entry.TabIndex = 5;
            this.CB_Entry.SelectedIndexChanged += new System.EventHandler(this.changeEntry);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(12, 33);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv.ShowEditingIcon = false;
            this.dgv.Size = new System.Drawing.Size(610, 317);
            this.dgv.TabIndex = 0;
            // 
            // B_AddLine
            // 
            this.B_AddLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_AddLine.Location = new System.Drawing.Point(437, 6);
            this.B_AddLine.Name = "B_AddLine";
            this.B_AddLine.Size = new System.Drawing.Size(90, 23);
            this.B_AddLine.TabIndex = 6;
            this.B_AddLine.Text = "Add Line After";
            this.B_AddLine.UseVisualStyleBackColor = true;
            this.B_AddLine.Click += new System.EventHandler(this.B_AddLine_Click);
            // 
            // B_RemoveLine
            // 
            this.B_RemoveLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_RemoveLine.Location = new System.Drawing.Point(533, 6);
            this.B_RemoveLine.Name = "B_RemoveLine";
            this.B_RemoveLine.Size = new System.Drawing.Size(90, 23);
            this.B_RemoveLine.TabIndex = 7;
            this.B_RemoveLine.Text = "Remove Line";
            this.B_RemoveLine.UseVisualStyleBackColor = true;
            this.B_RemoveLine.Click += new System.EventHandler(this.B_RemoveLine_Click);
            // 
            // B_Export
            // 
            this.B_Export.Location = new System.Drawing.Point(154, 6);
            this.B_Export.Name = "B_Export";
            this.B_Export.Size = new System.Drawing.Size(90, 23);
            this.B_Export.TabIndex = 8;
            this.B_Export.Text = "Export All (.txt)";
            this.B_Export.UseVisualStyleBackColor = true;
            this.B_Export.Click += new System.EventHandler(this.B_Export_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Text File:";
            // 
            // B_Import
            // 
            this.B_Import.Location = new System.Drawing.Point(250, 6);
            this.B_Import.Name = "B_Import";
            this.B_Import.Size = new System.Drawing.Size(90, 23);
            this.B_Import.TabIndex = 10;
            this.B_Import.Text = "Import All (.txt)";
            this.B_Import.UseVisualStyleBackColor = true;
            this.B_Import.Click += new System.EventHandler(this.B_Import_Click);
            // 
            // xytext
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 362);
            this.Controls.Add(this.B_Import);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.B_Export);
            this.Controls.Add(this.B_RemoveLine);
            this.Controls.Add(this.B_AddLine);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.CB_Entry);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "xytext";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Text Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.xytext_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_Entry;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button B_AddLine;
        private System.Windows.Forms.Button B_RemoveLine;
        private System.Windows.Forms.Button B_Export;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button B_Import;

    }
}
