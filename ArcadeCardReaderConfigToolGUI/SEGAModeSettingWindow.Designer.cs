namespace ArcadeCardReaderConfigToolGUI
{
    partial class SEGAModeSettingWindow
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SEGAModeSettingWindow));
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            IsHighBaudrateComboBox = new System.Windows.Forms.ComboBox();
            IsLEDEnableComboBox = new System.Windows.Forms.ComboBox();
            LEDBrightnessTrackBar = new System.Windows.Forms.TrackBar();
            LEDBrightnessLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            EnableCardReaderPlusComboBox = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            EnableSpice2PComboBox = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            EnableReCardNumberComboBox = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            ReCardNumberIDmTextBox = new System.Windows.Forms.TextBox();
            ReCardNumberTextBox = new System.Windows.Forms.TextBox();
            CancelButton = new System.Windows.Forms.Button();
            SubmitButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)LEDBrightnessTrackBar).BeginInit();
            SuspendLayout();
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(46, 101);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(66, 17);
            label4.TabIndex = 5;
            label4.Text = "LED亮度：";
            toolTip1.SetToolTip(label4, "范围为0~255");
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(46, 63);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(66, 17);
            label3.TabIndex = 4;
            label3.Text = "LED状态：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(46, 34);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(92, 17);
            label2.TabIndex = 3;
            label2.Text = "高波特率模式：";
            // 
            // IsHighBaudrateComboBox
            // 
            IsHighBaudrateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            IsHighBaudrateComboBox.FormattingEnabled = true;
            IsHighBaudrateComboBox.Items.AddRange(new object[] { "启用", "不启用" });
            IsHighBaudrateComboBox.Location = new System.Drawing.Point(144, 31);
            IsHighBaudrateComboBox.Name = "IsHighBaudrateComboBox";
            IsHighBaudrateComboBox.Size = new System.Drawing.Size(121, 25);
            IsHighBaudrateComboBox.TabIndex = 6;
            // 
            // IsLEDEnableComboBox
            // 
            IsLEDEnableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            IsLEDEnableComboBox.FormattingEnabled = true;
            IsLEDEnableComboBox.Items.AddRange(new object[] { "启用", "不启用" });
            IsLEDEnableComboBox.Location = new System.Drawing.Point(144, 60);
            IsLEDEnableComboBox.Name = "IsLEDEnableComboBox";
            IsLEDEnableComboBox.Size = new System.Drawing.Size(121, 25);
            IsLEDEnableComboBox.TabIndex = 7;
            // 
            // LEDBrightnessTrackBar
            // 
            LEDBrightnessTrackBar.Location = new System.Drawing.Point(144, 91);
            LEDBrightnessTrackBar.Maximum = 255;
            LEDBrightnessTrackBar.Name = "LEDBrightnessTrackBar";
            LEDBrightnessTrackBar.Size = new System.Drawing.Size(583, 45);
            LEDBrightnessTrackBar.TabIndex = 8;
            LEDBrightnessTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            LEDBrightnessTrackBar.Scroll += LEDBrightnessTrackBar_Scroll;
            LEDBrightnessTrackBar.DataContextChanged += LEDBrightnessTrackBar_DataContextChanged;
            // 
            // LEDBrightnessLabel
            // 
            LEDBrightnessLabel.AutoSize = true;
            LEDBrightnessLabel.Location = new System.Drawing.Point(69, 119);
            LEDBrightnessLabel.Name = "LEDBrightnessLabel";
            LEDBrightnessLabel.Size = new System.Drawing.Size(15, 17);
            LEDBrightnessLabel.TabIndex = 9;
            LEDBrightnessLabel.Text = "0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(46, 157);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 17);
            label1.TabIndex = 10;
            label1.Text = "扩展读卡：";
            // 
            // EnableCardReaderPlusComboBox
            // 
            EnableCardReaderPlusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            EnableCardReaderPlusComboBox.FormattingEnabled = true;
            EnableCardReaderPlusComboBox.Items.AddRange(new object[] { "启用", "不启用" });
            EnableCardReaderPlusComboBox.Location = new System.Drawing.Point(144, 154);
            EnableCardReaderPlusComboBox.Name = "EnableCardReaderPlusComboBox";
            EnableCardReaderPlusComboBox.Size = new System.Drawing.Size(121, 25);
            EnableCardReaderPlusComboBox.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(46, 186);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(175, 17);
            label5.TabIndex = 12;
            label5.Text = "SPICE模式下进入2P刷卡模式：";
            // 
            // EnableSpice2PComboBox
            // 
            EnableSpice2PComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            EnableSpice2PComboBox.FormattingEnabled = true;
            EnableSpice2PComboBox.Items.AddRange(new object[] { "启用", "不启用" });
            EnableSpice2PComboBox.Location = new System.Drawing.Point(227, 183);
            EnableSpice2PComboBox.Name = "EnableSpice2PComboBox";
            EnableSpice2PComboBox.Size = new System.Drawing.Size(121, 25);
            EnableSpice2PComboBox.TabIndex = 13;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(46, 220);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(68, 17);
            label6.TabIndex = 14;
            label6.Text = "卡号映射：";
            toolTip1.SetToolTip(label6, "固件版本v7以下不支持卡号映射功能");
            // 
            // EnableReCardNumberComboBox
            // 
            EnableReCardNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            EnableReCardNumberComboBox.FormattingEnabled = true;
            EnableReCardNumberComboBox.Items.AddRange(new object[] { "启用", "不启用" });
            EnableReCardNumberComboBox.Location = new System.Drawing.Point(144, 217);
            EnableReCardNumberComboBox.Name = "EnableReCardNumberComboBox";
            EnableReCardNumberComboBox.Size = new System.Drawing.Size(121, 25);
            EnableReCardNumberComboBox.TabIndex = 15;
            EnableReCardNumberComboBox.SelectedIndexChanged += EnableReCardNumberComboBox_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(95, 254);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(104, 17);
            label7.TabIndex = 16;
            label7.Text = "被转换卡的IDm：";
            toolTip1.SetToolTip(label7, "16进制格式，16位，无前缀，中间不需要空格\r\nIDm可以通过本工具的读卡测试模式获取");
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(95, 283);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(116, 17);
            label8.TabIndex = 17;
            label8.Text = "目标转换卡的卡号：";
            toolTip1.SetToolTip(label8, "10进制格式，20位，无前缀，中间不需要空格");
            // 
            // ReCardNumberIDmTextBox
            // 
            ReCardNumberIDmTextBox.Enabled = false;
            ReCardNumberIDmTextBox.Location = new System.Drawing.Point(227, 248);
            ReCardNumberIDmTextBox.Name = "ReCardNumberIDmTextBox";
            ReCardNumberIDmTextBox.Size = new System.Drawing.Size(500, 23);
            ReCardNumberIDmTextBox.TabIndex = 18;
            toolTip1.SetToolTip(ReCardNumberIDmTextBox, "16进制格式，16位，无前缀，中间不需要空格\r\nIDm可以通过本工具的读卡测试模式获取");
            // 
            // ReCardNumberTextBox
            // 
            ReCardNumberTextBox.Enabled = false;
            ReCardNumberTextBox.Location = new System.Drawing.Point(227, 280);
            ReCardNumberTextBox.Name = "ReCardNumberTextBox";
            ReCardNumberTextBox.Size = new System.Drawing.Size(500, 23);
            ReCardNumberTextBox.TabIndex = 19;
            toolTip1.SetToolTip(ReCardNumberTextBox, "10进制格式，20位，无前缀，中间不需要空格");
            // 
            // CancelButton
            // 
            CancelButton.Location = new System.Drawing.Point(676, 388);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new System.Drawing.Size(112, 50);
            CancelButton.TabIndex = 20;
            CancelButton.Text = "取消";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // SubmitButton
            // 
            SubmitButton.Location = new System.Drawing.Point(558, 388);
            SubmitButton.Name = "SubmitButton";
            SubmitButton.Size = new System.Drawing.Size(112, 50);
            SubmitButton.TabIndex = 21;
            SubmitButton.Text = "保存并应用";
            SubmitButton.UseVisualStyleBackColor = true;
            SubmitButton.Click += SubmitButton_Click;
            // 
            // SEGAModeSettingWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(SubmitButton);
            Controls.Add(CancelButton);
            Controls.Add(ReCardNumberTextBox);
            Controls.Add(ReCardNumberIDmTextBox);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(EnableReCardNumberComboBox);
            Controls.Add(label6);
            Controls.Add(EnableSpice2PComboBox);
            Controls.Add(label5);
            Controls.Add(EnableCardReaderPlusComboBox);
            Controls.Add(label1);
            Controls.Add(LEDBrightnessLabel);
            Controls.Add(LEDBrightnessTrackBar);
            Controls.Add(IsLEDEnableComboBox);
            Controls.Add(IsHighBaudrateComboBox);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "SEGAModeSettingWindow";
            Text = "SEGA模式设置";
            ((System.ComponentModel.ISupportInitialize)LEDBrightnessTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox IsHighBaudrateComboBox;
        private System.Windows.Forms.ComboBox IsLEDEnableComboBox;
        private System.Windows.Forms.TrackBar LEDBrightnessTrackBar;
        private System.Windows.Forms.Label LEDBrightnessLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox EnableCardReaderPlusComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox EnableSpice2PComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox EnableReCardNumberComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox ReCardNumberIDmTextBox;
        private System.Windows.Forms.TextBox ReCardNumberTextBox;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button SubmitButton;
    }
}