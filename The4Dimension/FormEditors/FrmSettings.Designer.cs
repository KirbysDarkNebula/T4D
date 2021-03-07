﻿
namespace The4Dimension.FormEditors

{
    partial class Settings
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
        /// 

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ShowFPS = new System.Windows.Forms.CheckBox();
            this.ShowCamDBG = new System.Windows.Forms.CheckBox();
            this.ShowTri = new System.Windows.Forms.CheckBox();
            this.CamToObj = new System.Windows.Forms.CheckBox();
            this.AddOrigin = new System.Windows.Forms.CheckBox();
            this.ChckUpdStart = new System.Windows.Forms.CheckBox();
            this.DwnldObjDBStart = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SetDefaultUrl = new System.Windows.Forms.Button();
            this.URLtxtb = new System.Windows.Forms.TextBox();
            this.cbCameraMode = new System.Windows.Forms.ComboBox();
            this.CamInertiaUpDown = new System.Windows.Forms.NumericUpDown();
            this.ZoomSenUpDown = new System.Windows.Forms.NumericUpDown();
            this.RotSenUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.Save = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Default = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.dotcomma = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.CamInertiaUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomSenUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotSenUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "3D view settings:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Camera inertia factor:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Zoom sensitivity";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Camera mode:";
            // 
            // ShowFPS
            // 
            this.ShowFPS.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ShowFPS.AutoSize = true;
            this.ShowFPS.Location = new System.Drawing.Point(34, 242);
            this.ShowFPS.Name = "ShowFPS";
            this.ShowFPS.Size = new System.Drawing.Size(76, 17);
            this.ShowFPS.TabIndex = 6;
            this.ShowFPS.Text = "Show FPS";
            this.ShowFPS.UseVisualStyleBackColor = true;
            // 
            // ShowCamDBG
            // 
            this.ShowCamDBG.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ShowCamDBG.AutoSize = true;
            this.ShowCamDBG.Location = new System.Drawing.Point(291, 242);
            this.ShowCamDBG.Name = "ShowCamDBG";
            this.ShowCamDBG.Size = new System.Drawing.Size(144, 17);
            this.ShowCamDBG.TabIndex = 8;
            this.ShowCamDBG.Text = "Show camera debug info";
            this.ShowCamDBG.UseVisualStyleBackColor = true;
            // 
            // ShowTri
            // 
            this.ShowTri.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ShowTri.AutoSize = true;
            this.ShowTri.Location = new System.Drawing.Point(34, 265);
            this.ShowTri.Name = "ShowTri";
            this.ShowTri.Size = new System.Drawing.Size(120, 17);
            this.ShowTri.TabIndex = 9;
            this.ShowTri.Text = "Show triangle count";
            this.ShowTri.UseVisualStyleBackColor = true;
            // 
            // CamToObj
            // 
            this.CamToObj.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CamToObj.AutoSize = true;
            this.CamToObj.Location = new System.Drawing.Point(291, 258);
            this.CamToObj.Name = "CamToObj";
            this.CamToObj.Size = new System.Drawing.Size(192, 30);
            this.CamToObj.TabIndex = 10;
            this.CamToObj.Text = "When adding a new object,\r\nautomatically move the camera to it";
            this.CamToObj.UseVisualStyleBackColor = true;
            // 
            // AddOrigin
            // 
            this.AddOrigin.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AddOrigin.AutoSize = true;
            this.AddOrigin.Location = new System.Drawing.Point(34, 288);
            this.AddOrigin.Name = "AddOrigin";
            this.AddOrigin.Size = new System.Drawing.Size(121, 17);
            this.AddOrigin.TabIndex = 11;
            this.AddOrigin.Text = "Add objects at 0,0,0";
            this.AddOrigin.UseVisualStyleBackColor = true;
            // 
            // ChckUpdStart
            // 
            this.ChckUpdStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ChckUpdStart.AutoSize = true;
            this.ChckUpdStart.Location = new System.Drawing.Point(25, 357);
            this.ChckUpdStart.Name = "ChckUpdStart";
            this.ChckUpdStart.Size = new System.Drawing.Size(163, 17);
            this.ChckUpdStart.TabIndex = 12;
            this.ChckUpdStart.Text = "Check for updates on startup";
            this.ChckUpdStart.UseVisualStyleBackColor = true;
            // 
            // DwnldObjDBStart
            // 
            this.DwnldObjDBStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DwnldObjDBStart.AutoSize = true;
            this.DwnldObjDBStart.Location = new System.Drawing.Point(25, 389);
            this.DwnldObjDBStart.Name = "DwnldObjDBStart";
            this.DwnldObjDBStart.Size = new System.Drawing.Size(277, 17);
            this.DwnldObjDBStart.TabIndex = 13;
            this.DwnldObjDBStart.Text = "Automatically download latest objectdb.xml on startup";
            this.DwnldObjDBStart.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, 325);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Editor settings:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 426);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Database download link:";
            // 
            // SetDefaultUrl
            // 
            this.SetDefaultUrl.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SetDefaultUrl.Location = new System.Drawing.Point(416, 422);
            this.SetDefaultUrl.Name = "SetDefaultUrl";
            this.SetDefaultUrl.Size = new System.Drawing.Size(75, 23);
            this.SetDefaultUrl.TabIndex = 16;
            this.SetDefaultUrl.Text = "Default";
            this.SetDefaultUrl.UseVisualStyleBackColor = true;
            this.SetDefaultUrl.Click += new System.EventHandler(this.SetDefaultUrl_Click);
            // 
            // URLtxtb
            // 
            this.URLtxtb.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.URLtxtb.Location = new System.Drawing.Point(157, 424);
            this.URLtxtb.Name = "URLtxtb";
            this.URLtxtb.Size = new System.Drawing.Size(252, 20);
            this.URLtxtb.TabIndex = 20;
            // 
            // cbCameraMode
            // 
            this.cbCameraMode.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cbCameraMode.FormattingEnabled = true;
            this.cbCameraMode.Items.AddRange(new object[] {
            "Inspect (Default)",
            "WalkAround (Whitehole)"});
            this.cbCameraMode.Location = new System.Drawing.Point(311, 173);
            this.cbCameraMode.Name = "cbCameraMode";
            this.cbCameraMode.Size = new System.Drawing.Size(156, 21);
            this.cbCameraMode.TabIndex = 21;
            // 
            // CamInertiaUpDown
            // 
            this.CamInertiaUpDown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CamInertiaUpDown.DecimalPlaces = 2;
            this.CamInertiaUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.CamInertiaUpDown.Location = new System.Drawing.Point(392, 44);
            this.CamInertiaUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            this.CamInertiaUpDown.Name = "CamInertiaUpDown";
            this.CamInertiaUpDown.Size = new System.Drawing.Size(75, 20);
            this.CamInertiaUpDown.TabIndex = 23;
            this.CamInertiaUpDown.Value = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            // 
            // ZoomSenUpDown
            // 
            this.ZoomSenUpDown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ZoomSenUpDown.DecimalPlaces = 2;
            this.ZoomSenUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ZoomSenUpDown.Location = new System.Drawing.Point(392, 81);
            this.ZoomSenUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ZoomSenUpDown.Name = "ZoomSenUpDown";
            this.ZoomSenUpDown.Size = new System.Drawing.Size(75, 20);
            this.ZoomSenUpDown.TabIndex = 24;
            // 
            // RotSenUpDown
            // 
            this.RotSenUpDown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.RotSenUpDown.DecimalPlaces = 2;
            this.RotSenUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.RotSenUpDown.Location = new System.Drawing.Point(392, 115);
            this.RotSenUpDown.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.RotSenUpDown.Name = "RotSenUpDown";
            this.RotSenUpDown.Size = new System.Drawing.Size(75, 20);
            this.RotSenUpDown.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(268, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "(This controls how much the camera slips) Default: 0,92";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(31, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Default: 2";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(31, 128);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Default: 1";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(31, 115);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label11.Size = new System.Drawing.Size(95, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "Rotation sensitivity";
            // 
            // Save
            // 
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.Location = new System.Drawing.Point(415, 503);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 30;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.Location = new System.Drawing.Point(12, 503);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 31;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Default
            // 
            this.Default.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Default.Location = new System.Drawing.Point(397, 8);
            this.Default.Name = "Default";
            this.Default.Size = new System.Drawing.Size(94, 23);
            this.Default.TabIndex = 32;
            this.Default.Text = "Reset Settings";
            this.Default.UseVisualStyleBackColor = true;
            this.Default.Click += new System.EventHandler(this.Apply_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBox1.Location = new System.Drawing.Point(157, 464);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(285, 20);
            this.textBox1.TabIndex = 35;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.Location = new System.Drawing.Point(448, 462);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(42, 23);
            this.button1.TabIndex = 34;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(79, 467);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 13);
            this.label12.TabIndex = 33;
            this.label12.Text = "ROMFS path:";
            // 
            // dotcomma
            // 
            this.dotcomma.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dotcomma.AutoSize = true;
            this.dotcomma.Location = new System.Drawing.Point(291, 288);
            this.dotcomma.Name = "dotcomma";
            this.dotcomma.Size = new System.Drawing.Size(149, 17);
            this.dotcomma.TabIndex = 36;
            this.dotcomma.Text = "Use dot instead of comma";
            this.dotcomma.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(503, 538);
            this.Controls.Add(this.dotcomma);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.Default);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.RotSenUpDown);
            this.Controls.Add(this.ZoomSenUpDown);
            this.Controls.Add(this.CamInertiaUpDown);
            this.Controls.Add(this.cbCameraMode);
            this.Controls.Add(this.URLtxtb);
            this.Controls.Add(this.SetDefaultUrl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.DwnldObjDBStart);
            this.Controls.Add(this.ChckUpdStart);
            this.Controls.Add(this.AddOrigin);
            this.Controls.Add(this.CamToObj);
            this.Controls.Add(this.ShowTri);
            this.Controls.Add(this.ShowCamDBG);
            this.Controls.Add(this.ShowFPS);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.CamInertiaUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomSenUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotSenUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ShowFPS;
        private System.Windows.Forms.CheckBox ShowCamDBG;
        private System.Windows.Forms.CheckBox ShowTri;
        private System.Windows.Forms.CheckBox CamToObj;
        private System.Windows.Forms.CheckBox AddOrigin;
        private System.Windows.Forms.CheckBox ChckUpdStart;
        private System.Windows.Forms.CheckBox DwnldObjDBStart;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button SetDefaultUrl;
        private System.Windows.Forms.TextBox URLtxtb;
        private System.Windows.Forms.ComboBox cbCameraMode;
        private System.Windows.Forms.NumericUpDown CamInertiaUpDown;
        private System.Windows.Forms.NumericUpDown ZoomSenUpDown;
        private System.Windows.Forms.NumericUpDown RotSenUpDown;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Default;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox dotcomma;
    }
}