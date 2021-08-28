﻿namespace The4Dimension.FormEditors
{
    partial class FrmAddCameraSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddCameraSettings));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.ChckBxFixCam = new System.Windows.Forms.CheckBox();
            this.FixZ = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ChckBxRailCam = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.RailId = new System.Windows.Forms.NumericUpDown();
            this.FixY = new System.Windows.Forms.NumericUpDown();
            this.FixX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.FixedTypeBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.FixX2 = new System.Windows.Forms.NumericUpDown();
            this.FixY2 = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.FixZ2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RailId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixY2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixZ2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(349, 302);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(268, 302);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Camera Id:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(268, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Vertical angle:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Horizontal angle:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Distance from player:";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(345, 49);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(71, 20);
            this.numericUpDown2.TabIndex = 7;
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(104, 49);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(71, 20);
            this.numericUpDown3.TabIndex = 8;
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(124, 84);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(103, 20);
            this.numericUpDown4.TabIndex = 9;
            this.numericUpDown4.Value = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            // 
            // ChckBxFixCam
            // 
            this.ChckBxFixCam.AutoSize = true;
            this.ChckBxFixCam.Location = new System.Drawing.Point(12, 113);
            this.ChckBxFixCam.Name = "ChckBxFixCam";
            this.ChckBxFixCam.Size = new System.Drawing.Size(134, 17);
            this.ChckBxFixCam.TabIndex = 10;
            this.ChckBxFixCam.Text = "Fixed position camera?";
            this.ChckBxFixCam.UseVisualStyleBackColor = true;
            this.ChckBxFixCam.CheckedChanged += new System.EventHandler(this.ChckBxFixCam_CheckedChanged);
            // 
            // FixZ
            // 
            this.FixZ.DecimalPlaces = 2;
            this.FixZ.Enabled = false;
            this.FixZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FixZ.Location = new System.Drawing.Point(84, 250);
            this.FixZ.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.FixZ.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.FixZ.Name = "FixZ";
            this.FixZ.Size = new System.Drawing.Size(103, 20);
            this.FixZ.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(52, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "X:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Y:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(52, 252);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Z:";
            // 
            // ChckBxRailCam
            // 
            this.ChckBxRailCam.AutoSize = true;
            this.ChckBxRailCam.Location = new System.Drawing.Point(12, 282);
            this.ChckBxRailCam.Name = "ChckBxRailCam";
            this.ChckBxRailCam.Size = new System.Drawing.Size(78, 17);
            this.ChckBxRailCam.TabIndex = 17;
            this.ChckBxRailCam.Text = "Follow rail?";
            this.ChckBxRailCam.UseVisualStyleBackColor = true;
            this.ChckBxRailCam.CheckedChanged += new System.EventHandler(this.ChckBxRailCam_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 307);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Rail Id (I_id):";
            // 
            // RailId
            // 
            this.RailId.Enabled = false;
            this.RailId.Location = new System.Drawing.Point(84, 305);
            this.RailId.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.RailId.Name = "RailId";
            this.RailId.Size = new System.Drawing.Size(103, 20);
            this.RailId.TabIndex = 18;
            // 
            // FixY
            // 
            this.FixY.DecimalPlaces = 2;
            this.FixY.Enabled = false;
            this.FixY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FixY.Location = new System.Drawing.Point(84, 224);
            this.FixY.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.FixY.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.FixY.Name = "FixY";
            this.FixY.Size = new System.Drawing.Size(103, 20);
            this.FixY.TabIndex = 22;
            // 
            // FixX
            // 
            this.FixX.DecimalPlaces = 2;
            this.FixX.Enabled = false;
            this.FixX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FixX.Location = new System.Drawing.Point(84, 198);
            this.FixX.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.FixX.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.FixX.Name = "FixX";
            this.FixX.Size = new System.Drawing.Size(103, 20);
            this.FixX.TabIndex = 23;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(313, 84);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            400,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(103, 20);
            this.numericUpDown1.TabIndex = 25;
            this.numericUpDown1.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(262, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Rotator:";
            // 
            // FixedTypeBox
            // 
            this.FixedTypeBox.Enabled = false;
            this.FixedTypeBox.FormattingEnabled = true;
            this.FixedTypeBox.Items.AddRange(new object[] {
            "Fixed point",
            "Fixed box",
            "Fixed on Mario",
            "Rotate around point"});
            this.FixedTypeBox.Location = new System.Drawing.Point(15, 136);
            this.FixedTypeBox.Name = "FixedTypeBox";
            this.FixedTypeBox.Size = new System.Drawing.Size(132, 21);
            this.FixedTypeBox.TabIndex = 26;
            this.FixedTypeBox.SelectedIndexChanged += new System.EventHandler(this.ShowHideXYZ2);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(52, 173);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Point position:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(234, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Point position:";
            // 
            // FixX2
            // 
            this.FixX2.DecimalPlaces = 2;
            this.FixX2.Enabled = false;
            this.FixX2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FixX2.Location = new System.Drawing.Point(266, 198);
            this.FixX2.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.FixX2.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.FixX2.Name = "FixX2";
            this.FixX2.Size = new System.Drawing.Size(103, 20);
            this.FixX2.TabIndex = 33;
            // 
            // FixY2
            // 
            this.FixY2.DecimalPlaces = 2;
            this.FixY2.Enabled = false;
            this.FixY2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FixY2.Location = new System.Drawing.Point(266, 224);
            this.FixY2.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.FixY2.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.FixY2.Name = "FixY2";
            this.FixY2.Size = new System.Drawing.Size(103, 20);
            this.FixY2.TabIndex = 32;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(234, 252);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "Z:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(234, 226);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "Y:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(234, 200);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "X:";
            // 
            // FixZ2
            // 
            this.FixZ2.DecimalPlaces = 2;
            this.FixZ2.Enabled = false;
            this.FixZ2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FixZ2.Location = new System.Drawing.Point(266, 250);
            this.FixZ2.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.FixZ2.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.FixZ2.Name = "FixZ2";
            this.FixZ2.Size = new System.Drawing.Size(103, 20);
            this.FixZ2.TabIndex = 28;
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.Location = new System.Drawing.Point(313, 113);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numericUpDown5.Minimum = new decimal(new int[] {
            400,
            0,
            0,
            -2147483648});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(103, 20);
            this.numericUpDown5.TabIndex = 36;
            this.numericUpDown5.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(222, 117);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(85, 13);
            this.label15.TabIndex = 35;
            this.label15.Text = "Max dash angle:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.Location = new System.Drawing.Point(313, 139);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown6.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(103, 20);
            this.numericUpDown6.TabIndex = 38;
            this.numericUpDown6.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(176, 141);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(131, 13);
            this.label16.TabIndex = 37;
            this.label16.Text = "Max dash zoom out offset:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmAddCameraSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(436, 336);
            this.Controls.Add(this.numericUpDown6);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.numericUpDown5);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.FixX2);
            this.Controls.Add(this.FixY2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.FixZ2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.FixedTypeBox);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.FixX);
            this.Controls.Add(this.FixY);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.RailId);
            this.Controls.Add(this.ChckBxRailCam);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FixZ);
            this.Controls.Add(this.ChckBxFixCam);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddCameraSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add camera code";
            this.Load += new System.EventHandler(this.FrmAddCameraSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RailId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixY2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FixZ2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.CheckBox ChckBxFixCam;
        private System.Windows.Forms.NumericUpDown FixZ;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox ChckBxRailCam;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown RailId;
        private System.Windows.Forms.NumericUpDown FixY;
        private System.Windows.Forms.NumericUpDown FixX;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox FixedTypeBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown FixX2;
        private System.Windows.Forms.NumericUpDown FixY2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown FixZ2;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.Label label16;
    }
}