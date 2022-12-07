namespace The4Dimension.FormEditors
{
    partial class FrmAddFogSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddFogSettings));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.RedBar = new System.Windows.Forms.TrackBar();
            this.GreenUD = new System.Windows.Forms.NumericUpDown();
            this.BlueBar = new System.Windows.Forms.TrackBar();
            this.GreenBar = new System.Windows.Forms.TrackBar();
            this.BlueUD = new System.Windows.Forms.NumericUpDown();
            this.RedUD = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreenUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlueBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreenBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlueUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedUD)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Density:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.DecimalPlaces = 1;
            this.numericUpDown1.Location = new System.Drawing.Point(256, 37);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(67, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "FogArea Id:";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numericUpDown2.Location = new System.Drawing.Point(256, 63);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(67, 20);
            this.numericUpDown2.TabIndex = 4;
            this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Frame Interpolation:";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numericUpDown3.DecimalPlaces = 2;
            this.numericUpDown3.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown3.Location = new System.Drawing.Point(256, 89);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            950000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(67, 20);
            this.numericUpDown3.TabIndex = 6;
            this.numericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown3.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Max Depth:";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numericUpDown4.DecimalPlaces = 2;
            this.numericUpDown4.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown4.Location = new System.Drawing.Point(256, 115);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            950000,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(67, 20);
            this.numericUpDown4.TabIndex = 8;
            this.numericUpDown4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown4.Value = new decimal(new int[] {
            700,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Min Depth:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Fog colour (RGB):";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RedBar
            // 
            this.RedBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.RedBar.LargeChange = 10;
            this.RedBar.Location = new System.Drawing.Point(35, 207);
            this.RedBar.Maximum = 255;
            this.RedBar.Name = "RedBar";
            this.RedBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.RedBar.Size = new System.Drawing.Size(45, 129);
            this.RedBar.SmallChange = 5;
            this.RedBar.TabIndex = 10;
            this.RedBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.RedBar.ValueChanged += new System.EventHandler(this.RedBar_ValueChanged);
            // 
            // GreenUD
            // 
            this.GreenUD.Location = new System.Drawing.Point(103, 342);
            this.GreenUD.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.GreenUD.Name = "GreenUD";
            this.GreenUD.Size = new System.Drawing.Size(45, 20);
            this.GreenUD.TabIndex = 11;
            this.GreenUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BlueBar
            // 
            this.BlueBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.BlueBar.LargeChange = 10;
            this.BlueBar.Location = new System.Drawing.Point(173, 207);
            this.BlueBar.Maximum = 255;
            this.BlueBar.Name = "BlueBar";
            this.BlueBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.BlueBar.Size = new System.Drawing.Size(45, 129);
            this.BlueBar.SmallChange = 5;
            this.BlueBar.TabIndex = 12;
            this.BlueBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.BlueBar.ValueChanged += new System.EventHandler(this.BlueBar_ValueChanged);
            // 
            // GreenBar
            // 
            this.GreenBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.GreenBar.LargeChange = 10;
            this.GreenBar.Location = new System.Drawing.Point(103, 207);
            this.GreenBar.Maximum = 255;
            this.GreenBar.Name = "GreenBar";
            this.GreenBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.GreenBar.Size = new System.Drawing.Size(45, 129);
            this.GreenBar.SmallChange = 5;
            this.GreenBar.TabIndex = 13;
            this.GreenBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.GreenBar.ValueChanged += new System.EventHandler(this.GreenBar_ValueChanged);
            // 
            // BlueUD
            // 
            this.BlueUD.Location = new System.Drawing.Point(173, 342);
            this.BlueUD.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.BlueUD.Name = "BlueUD";
            this.BlueUD.Size = new System.Drawing.Size(45, 20);
            this.BlueUD.TabIndex = 14;
            this.BlueUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RedUD
            // 
            this.RedUD.Location = new System.Drawing.Point(35, 342);
            this.RedUD.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.RedUD.Name = "RedUD";
            this.RedUD.Size = new System.Drawing.Size(45, 20);
            this.RedUD.TabIndex = 15;
            this.RedUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(245, 207);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(78, 155);
            this.panel1.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(72, 149);
            this.panel2.TabIndex = 17;
            this.panel2.BackColorChanged += new System.EventHandler(this.panel2_BackColorChanged);
            // 
            // Ok
            // 
            this.Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ok.Location = new System.Drawing.Point(248, 374);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 17;
            this.Ok.Text = "Ok";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.Location = new System.Drawing.Point(12, 374);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 18;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Fog Type:";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "FOG_UPDATER_TYPE_EXPONENT",
            "FOG_UPDATER_TYPE_EXPONENT_SQUARE",
            "FOG_UPDATER_TYPE_LINEAR"});
            this.comboBox1.Location = new System.Drawing.Point(75, 151);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(248, 21);
            this.comboBox1.TabIndex = 20;
            // 
            // FrmAddFogSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(339, 409);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.RedUD);
            this.Controls.Add(this.BlueUD);
            this.Controls.Add(this.GreenBar);
            this.Controls.Add(this.BlueBar);
            this.Controls.Add(this.GreenUD);
            this.Controls.Add(this.RedBar);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddFogSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FogArea Settings";
            this.Load += new System.EventHandler(this.FrmAddFogSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreenUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlueBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreenBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlueUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedUD)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar RedBar;
        private System.Windows.Forms.NumericUpDown GreenUD;
        private System.Windows.Forms.TrackBar BlueBar;
        private System.Windows.Forms.TrackBar GreenBar;
        private System.Windows.Forms.NumericUpDown BlueUD;
        private System.Windows.Forms.NumericUpDown RedUD;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}