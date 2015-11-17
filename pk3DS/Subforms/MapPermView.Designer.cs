namespace pk3DS.Subforms
{
    partial class MapPermView
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
            this.L_MapCoord = new System.Windows.Forms.Label();
            this.PAN_MAP = new System.Windows.Forms.Panel();
            this.PB_Map = new System.Windows.Forms.PictureBox();
            this.CHK_AutoDraw = new System.Windows.Forms.CheckBox();
            this.B_Redraw = new System.Windows.Forms.Button();
            this.L_Scale = new System.Windows.Forms.Label();
            this.NUD_Scale = new System.Windows.Forms.NumericUpDown();
            this.L_Flavor = new System.Windows.Forms.Label();
            this.NUD_Flavor = new System.Windows.Forms.NumericUpDown();
            this.PAN_MAP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Map)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Scale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Flavor)).BeginInit();
            this.SuspendLayout();
            // 
            // L_MapCoord
            // 
            this.L_MapCoord.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_MapCoord.Location = new System.Drawing.Point(349, 2);
            this.L_MapCoord.Name = "L_MapCoord";
            this.L_MapCoord.Size = new System.Drawing.Size(131, 28);
            this.L_MapCoord.TabIndex = 20;
            this.L_MapCoord.Text = "V:0x00000000\r\nX:  1  Y:  1";
            this.L_MapCoord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PAN_MAP
            // 
            this.PAN_MAP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PAN_MAP.AutoScroll = true;
            this.PAN_MAP.Controls.Add(this.PB_Map);
            this.PAN_MAP.Location = new System.Drawing.Point(9, 31);
            this.PAN_MAP.Name = "PAN_MAP";
            this.PAN_MAP.Size = new System.Drawing.Size(470, 430);
            this.PAN_MAP.TabIndex = 19;
            // 
            // PB_Map
            // 
            this.PB_Map.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PB_Map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_Map.Location = new System.Drawing.Point(0, 0);
            this.PB_Map.Name = "PB_Map";
            this.PB_Map.Size = new System.Drawing.Size(470, 430);
            this.PB_Map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PB_Map.TabIndex = 2;
            this.PB_Map.TabStop = false;
            this.PB_Map.DoubleClick += new System.EventHandler(this.dclickMap);
            this.PB_Map.MouseEnter += new System.EventHandler(this.focusPanel);
            this.PB_Map.MouseMove += new System.Windows.Forms.MouseEventHandler(this.hoverMap);
            // 
            // CHK_AutoDraw
            // 
            this.CHK_AutoDraw.AutoSize = true;
            this.CHK_AutoDraw.Location = new System.Drawing.Point(267, 8);
            this.CHK_AutoDraw.Name = "CHK_AutoDraw";
            this.CHK_AutoDraw.Size = new System.Drawing.Size(76, 17);
            this.CHK_AutoDraw.TabIndex = 18;
            this.CHK_AutoDraw.Text = "Auto-Draw";
            this.CHK_AutoDraw.UseVisualStyleBackColor = true;
            // 
            // B_Redraw
            // 
            this.B_Redraw.Location = new System.Drawing.Point(186, 5);
            this.B_Redraw.Name = "B_Redraw";
            this.B_Redraw.Size = new System.Drawing.Size(75, 23);
            this.B_Redraw.TabIndex = 17;
            this.B_Redraw.Text = "Redraw";
            this.B_Redraw.UseVisualStyleBackColor = true;
            this.B_Redraw.Click += new System.EventHandler(this.B_Redraw_Click);
            // 
            // L_Scale
            // 
            this.L_Scale.AutoSize = true;
            this.L_Scale.Location = new System.Drawing.Point(100, 9);
            this.L_Scale.Name = "L_Scale";
            this.L_Scale.Size = new System.Drawing.Size(37, 13);
            this.L_Scale.TabIndex = 16;
            this.L_Scale.Text = "Scale:";
            // 
            // NUD_Scale
            // 
            this.NUD_Scale.Location = new System.Drawing.Point(143, 7);
            this.NUD_Scale.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.NUD_Scale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUD_Scale.Name = "NUD_Scale";
            this.NUD_Scale.Size = new System.Drawing.Size(37, 20);
            this.NUD_Scale.TabIndex = 15;
            this.NUD_Scale.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // L_Flavor
            // 
            this.L_Flavor.AutoSize = true;
            this.L_Flavor.Location = new System.Drawing.Point(12, 9);
            this.L_Flavor.Name = "L_Flavor";
            this.L_Flavor.Size = new System.Drawing.Size(39, 13);
            this.L_Flavor.TabIndex = 14;
            this.L_Flavor.Text = "Flavor:";
            // 
            // NUD_Flavor
            // 
            this.NUD_Flavor.Location = new System.Drawing.Point(57, 7);
            this.NUD_Flavor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUD_Flavor.Name = "NUD_Flavor";
            this.NUD_Flavor.Size = new System.Drawing.Size(37, 20);
            this.NUD_Flavor.TabIndex = 13;
            this.NUD_Flavor.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // MapPermView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 467);
            this.Controls.Add(this.L_MapCoord);
            this.Controls.Add(this.PAN_MAP);
            this.Controls.Add(this.CHK_AutoDraw);
            this.Controls.Add(this.B_Redraw);
            this.Controls.Add(this.L_Scale);
            this.Controls.Add(this.NUD_Scale);
            this.Controls.Add(this.L_Flavor);
            this.Controls.Add(this.NUD_Flavor);
            this.MinimumSize = new System.Drawing.Size(505, 505);
            this.Name = "MapPermView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Map Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapPermView_FormClosing);
            this.PAN_MAP.ResumeLayout(false);
            this.PAN_MAP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Map)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Scale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Flavor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label L_MapCoord;
        private System.Windows.Forms.Panel PAN_MAP;
        private System.Windows.Forms.PictureBox PB_Map;
        private System.Windows.Forms.CheckBox CHK_AutoDraw;
        private System.Windows.Forms.Button B_Redraw;
        private System.Windows.Forms.Label L_Scale;
        private System.Windows.Forms.NumericUpDown NUD_Scale;
        private System.Windows.Forms.Label L_Flavor;
        private System.Windows.Forms.NumericUpDown NUD_Flavor;

    }
}