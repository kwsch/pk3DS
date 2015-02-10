namespace pk3DS
{
    partial class Tutors
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
            this.CB_Location = new System.Windows.Forms.ComboBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.L_Mart = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // CB_Location
            // 
            this.CB_Location.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Location.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Location.FormattingEnabled = true;
            this.CB_Location.Location = new System.Drawing.Point(69, 6);
            this.CB_Location.Name = "CB_Location";
            this.CB_Location.Size = new System.Drawing.Size(243, 21);
            this.CB_Location.TabIndex = 0;
            this.CB_Location.SelectedIndexChanged += new System.EventHandler(this.changeIndex);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(12, 33);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(300, 317);
            this.dgv.TabIndex = 1;
            // 
            // L_Mart
            // 
            this.L_Mart.AutoSize = true;
            this.L_Mart.Location = new System.Drawing.Point(12, 9);
            this.L_Mart.Name = "L_Mart";
            this.L_Mart.Size = new System.Drawing.Size(51, 13);
            this.L_Mart.TabIndex = 2;
            this.L_Mart.Text = "Location:";
            // 
            // Tutors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 362);
            this.Controls.Add(this.L_Mart);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.CB_Location);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(340, 600);
            this.MinimumSize = new System.Drawing.Size(340, 400);
            this.Name = "Tutors";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Move Tutor Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_Location;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label L_Mart;
    }
}