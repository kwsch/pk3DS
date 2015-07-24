namespace pk3DS
{
    partial class ItemEditor
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
            this.CB_Item = new System.Windows.Forms.ComboBox();
            this.L_Item = new System.Windows.Forms.Label();
            this.RTB = new System.Windows.Forms.RichTextBox();
            this.MT_Price = new System.Windows.Forms.MaskedTextBox();
            this.NUD_UseEffect = new System.Windows.Forms.NumericUpDown();
            this.L_UseEffect = new System.Windows.Forms.Label();
            this.L_Buy = new System.Windows.Forms.Label();
            this.L_Feedback = new System.Windows.Forms.Label();
            this.L_Sell = new System.Windows.Forms.Label();
            this.MT_Sell = new System.Windows.Forms.MaskedTextBox();
            this.L_Index = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_UseEffect)).BeginInit();
            this.SuspendLayout();
            // 
            // CB_Item
            // 
            this.CB_Item.DropDownWidth = 120;
            this.CB_Item.FormattingEnabled = true;
            this.CB_Item.Location = new System.Drawing.Point(71, 10);
            this.CB_Item.Name = "CB_Item";
            this.CB_Item.Size = new System.Drawing.Size(129, 21);
            this.CB_Item.TabIndex = 1;
            this.CB_Item.SelectedIndexChanged += new System.EventHandler(this.changeEntry);
            // 
            // L_Item
            // 
            this.L_Item.AutoSize = true;
            this.L_Item.Location = new System.Drawing.Point(33, 13);
            this.L_Item.Name = "L_Item";
            this.L_Item.Size = new System.Drawing.Size(30, 13);
            this.L_Item.TabIndex = 2;
            this.L_Item.Text = "Item:";
            this.L_Item.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RTB
            // 
            this.RTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RTB.Location = new System.Drawing.Point(12, 124);
            this.RTB.Name = "RTB";
            this.RTB.ReadOnly = true;
            this.RTB.Size = new System.Drawing.Size(316, 51);
            this.RTB.TabIndex = 38;
            this.RTB.Text = "";
            // 
            // MT_Price
            // 
            this.MT_Price.Location = new System.Drawing.Point(163, 37);
            this.MT_Price.Mask = "00000";
            this.MT_Price.Name = "MT_Price";
            this.MT_Price.Size = new System.Drawing.Size(37, 20);
            this.MT_Price.TabIndex = 39;
            this.MT_Price.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MT_Price.TextChanged += new System.EventHandler(this.changePrice);
            // 
            // NUD_UseEffect
            // 
            this.NUD_UseEffect.Location = new System.Drawing.Point(165, 82);
            this.NUD_UseEffect.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NUD_UseEffect.Name = "NUD_UseEffect";
            this.NUD_UseEffect.Size = new System.Drawing.Size(35, 20);
            this.NUD_UseEffect.TabIndex = 40;
            // 
            // L_UseEffect
            // 
            this.L_UseEffect.AutoSize = true;
            this.L_UseEffect.Location = new System.Drawing.Point(97, 84);
            this.L_UseEffect.Name = "L_UseEffect";
            this.L_UseEffect.Size = new System.Drawing.Size(60, 13);
            this.L_UseEffect.TabIndex = 41;
            this.L_UseEffect.Text = "Use Effect:";
            // 
            // L_Buy
            // 
            this.L_Buy.AutoSize = true;
            this.L_Buy.Location = new System.Drawing.Point(75, 40);
            this.L_Buy.Name = "L_Buy";
            this.L_Buy.Size = new System.Drawing.Size(82, 13);
            this.L_Buy.TabIndex = 42;
            this.L_Buy.Text = "Purchase Price:";
            // 
            // L_Feedback
            // 
            this.L_Feedback.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.L_Feedback.AutoSize = true;
            this.L_Feedback.Location = new System.Drawing.Point(32, 108);
            this.L_Feedback.Name = "L_Feedback";
            this.L_Feedback.Size = new System.Drawing.Size(274, 13);
            this.L_Feedback.TabIndex = 43;
            this.L_Feedback.Text = "Please give feedback on what is actually needed to edit.";
            // 
            // L_Sell
            // 
            this.L_Sell.AutoSize = true;
            this.L_Sell.Location = new System.Drawing.Point(103, 59);
            this.L_Sell.Name = "L_Sell";
            this.L_Sell.Size = new System.Drawing.Size(54, 13);
            this.L_Sell.TabIndex = 44;
            this.L_Sell.Text = "Sell Price:";
            // 
            // MT_Sell
            // 
            this.MT_Sell.Enabled = false;
            this.MT_Sell.Location = new System.Drawing.Point(163, 56);
            this.MT_Sell.Mask = "00000";
            this.MT_Sell.Name = "MT_Sell";
            this.MT_Sell.Size = new System.Drawing.Size(37, 20);
            this.MT_Sell.TabIndex = 45;
            this.MT_Sell.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // L_Index
            // 
            this.L_Index.AutoSize = true;
            this.L_Index.Location = new System.Drawing.Point(206, 13);
            this.L_Index.Name = "L_Index";
            this.L_Index.Size = new System.Drawing.Size(39, 13);
            this.L_Index.TabIndex = 46;
            this.L_Index.Text = "Index: ";
            // 
            // Item
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 182);
            this.Controls.Add(this.L_Index);
            this.Controls.Add(this.MT_Sell);
            this.Controls.Add(this.L_Sell);
            this.Controls.Add(this.L_Feedback);
            this.Controls.Add(this.L_Buy);
            this.Controls.Add(this.L_UseEffect);
            this.Controls.Add(this.NUD_UseEffect);
            this.Controls.Add(this.MT_Price);
            this.Controls.Add(this.RTB);
            this.Controls.Add(this.L_Item);
            this.Controls.Add(this.CB_Item);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(355, 420);
            this.MinimumSize = new System.Drawing.Size(355, 220);
            this.Name = "Item";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Item Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_UseEffect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_Item;
        private System.Windows.Forms.Label L_Item;
        private System.Windows.Forms.RichTextBox RTB;
        private System.Windows.Forms.MaskedTextBox MT_Price;
        private System.Windows.Forms.NumericUpDown NUD_UseEffect;
        private System.Windows.Forms.Label L_UseEffect;
        private System.Windows.Forms.Label L_Buy;
        private System.Windows.Forms.Label L_Feedback;
        private System.Windows.Forms.Label L_Sell;
        private System.Windows.Forms.MaskedTextBox MT_Sell;
        private System.Windows.Forms.Label L_Index;
    }
}