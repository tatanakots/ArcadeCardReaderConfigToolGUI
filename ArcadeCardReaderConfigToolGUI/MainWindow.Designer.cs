namespace ArcadeCardReaderConfigToolGUI
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            refreshDevicesButton = new System.Windows.Forms.Button();
            connectDeviceButton = new System.Windows.Forms.Button();
            devicesComboBox = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            disconnectDeviceButton = new System.Windows.Forms.Button();
            DeviceInfoBox = new System.Windows.Forms.GroupBox();
            CopyCardReadTestTextButton = new System.Windows.Forms.Button();
            CardReadTestButton = new System.Windows.Forms.Button();
            CardReadTestTextBox = new System.Windows.Forms.TextBox();
            DeviceVersionLabel = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            HardwareVisionLabel = new System.Windows.Forms.Label();
            FirmwareVisionLabel = new System.Windows.Forms.Label();
            LEDBrightnessLabel = new System.Windows.Forms.Label();
            IsLEDEnableLabel = new System.Windows.Forms.Label();
            IsHighBaudrateLabel = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            AboutButton = new System.Windows.Forms.Button();
            SegaModeSettingButton = new System.Windows.Forms.Button();
            EnteryNamcoModeButton = new System.Windows.Forms.Button();
            EnterySpiceModeButton = new System.Windows.Forms.Button();
            EnteryPN532ModeButton = new System.Windows.Forms.Button();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            FirmwareUpgreadButton = new System.Windows.Forms.Button();
            DeviceInfoBox.SuspendLayout();
            SuspendLayout();
            // 
            // refreshDevicesButton
            // 
            refreshDevicesButton.Location = new System.Drawing.Point(833, 378);
            refreshDevicesButton.Name = "refreshDevicesButton";
            refreshDevicesButton.Size = new System.Drawing.Size(75, 23);
            refreshDevicesButton.TabIndex = 0;
            refreshDevicesButton.Text = "刷新";
            refreshDevicesButton.UseVisualStyleBackColor = true;
            refreshDevicesButton.Click += refreshDevicesButton_Click;
            // 
            // connectDeviceButton
            // 
            connectDeviceButton.Location = new System.Drawing.Point(706, 415);
            connectDeviceButton.Name = "connectDeviceButton";
            connectDeviceButton.Size = new System.Drawing.Size(90, 23);
            connectDeviceButton.TabIndex = 1;
            connectDeviceButton.Text = "连接读卡器";
            connectDeviceButton.UseVisualStyleBackColor = true;
            connectDeviceButton.Click += connectDeviceButton_Click;
            // 
            // devicesComboBox
            // 
            devicesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            devicesComboBox.FormattingEnabled = true;
            devicesComboBox.Location = new System.Drawing.Point(706, 378);
            devicesComboBox.Name = "devicesComboBox";
            devicesComboBox.Size = new System.Drawing.Size(121, 25);
            devicesComboBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(632, 381);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 17);
            label1.TabIndex = 3;
            label1.Text = "设备串口：";
            // 
            // disconnectDeviceButton
            // 
            disconnectDeviceButton.Location = new System.Drawing.Point(802, 415);
            disconnectDeviceButton.Name = "disconnectDeviceButton";
            disconnectDeviceButton.Size = new System.Drawing.Size(85, 23);
            disconnectDeviceButton.TabIndex = 4;
            disconnectDeviceButton.Text = "断开读卡器";
            disconnectDeviceButton.UseVisualStyleBackColor = true;
            disconnectDeviceButton.Click += disconnectDeviceButton_Click;
            // 
            // DeviceInfoBox
            // 
            DeviceInfoBox.Controls.Add(CopyCardReadTestTextButton);
            DeviceInfoBox.Controls.Add(CardReadTestButton);
            DeviceInfoBox.Controls.Add(CardReadTestTextBox);
            DeviceInfoBox.Controls.Add(DeviceVersionLabel);
            DeviceInfoBox.Controls.Add(label7);
            DeviceInfoBox.Controls.Add(HardwareVisionLabel);
            DeviceInfoBox.Controls.Add(FirmwareVisionLabel);
            DeviceInfoBox.Controls.Add(LEDBrightnessLabel);
            DeviceInfoBox.Controls.Add(IsLEDEnableLabel);
            DeviceInfoBox.Controls.Add(IsHighBaudrateLabel);
            DeviceInfoBox.Controls.Add(label6);
            DeviceInfoBox.Controls.Add(label5);
            DeviceInfoBox.Controls.Add(label4);
            DeviceInfoBox.Controls.Add(label3);
            DeviceInfoBox.Controls.Add(label2);
            DeviceInfoBox.Location = new System.Drawing.Point(12, 12);
            DeviceInfoBox.Name = "DeviceInfoBox";
            DeviceInfoBox.Size = new System.Drawing.Size(614, 426);
            DeviceInfoBox.TabIndex = 5;
            DeviceInfoBox.TabStop = false;
            DeviceInfoBox.Text = "读卡器信息";
            // 
            // CopyCardReadTestTextButton
            // 
            CopyCardReadTestTextButton.Location = new System.Drawing.Point(146, 397);
            CopyCardReadTestTextButton.Name = "CopyCardReadTestTextButton";
            CopyCardReadTestTextButton.Size = new System.Drawing.Size(113, 23);
            CopyCardReadTestTextButton.TabIndex = 14;
            CopyCardReadTestTextButton.Text = "复制读卡测试信息";
            CopyCardReadTestTextButton.UseVisualStyleBackColor = true;
            CopyCardReadTestTextButton.Click += CopyCardReadTestTextButton_Click;
            // 
            // CardReadTestButton
            // 
            CardReadTestButton.Enabled = false;
            CardReadTestButton.Location = new System.Drawing.Point(146, 369);
            CardReadTestButton.Name = "CardReadTestButton";
            CardReadTestButton.Size = new System.Drawing.Size(113, 23);
            CardReadTestButton.TabIndex = 13;
            CardReadTestButton.Text = "读卡测试模式";
            CardReadTestButton.UseVisualStyleBackColor = true;
            CardReadTestButton.Click += CardReadTestButton_Click;
            // 
            // CardReadTestTextBox
            // 
            CardReadTestTextBox.Location = new System.Drawing.Point(265, 22);
            CardReadTestTextBox.Multiline = true;
            CardReadTestTextBox.Name = "CardReadTestTextBox";
            CardReadTestTextBox.ReadOnly = true;
            CardReadTestTextBox.Size = new System.Drawing.Size(343, 398);
            CardReadTestTextBox.TabIndex = 12;
            // 
            // DeviceVersionLabel
            // 
            DeviceVersionLabel.AutoSize = true;
            DeviceVersionLabel.Location = new System.Drawing.Point(125, 170);
            DeviceVersionLabel.Name = "DeviceVersionLabel";
            DeviceVersionLabel.Size = new System.Drawing.Size(32, 17);
            DeviceVersionLabel.TabIndex = 11;
            DeviceVersionLabel.Text = "未知";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(27, 170);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(56, 17);
            label7.TabIndex = 10;
            label7.Text = "版本号：";
            // 
            // HardwareVisionLabel
            // 
            HardwareVisionLabel.AutoSize = true;
            HardwareVisionLabel.Location = new System.Drawing.Point(125, 141);
            HardwareVisionLabel.Name = "HardwareVisionLabel";
            HardwareVisionLabel.Size = new System.Drawing.Size(32, 17);
            HardwareVisionLabel.TabIndex = 9;
            HardwareVisionLabel.Text = "未知";
            // 
            // FirmwareVisionLabel
            // 
            FirmwareVisionLabel.AutoSize = true;
            FirmwareVisionLabel.Location = new System.Drawing.Point(125, 114);
            FirmwareVisionLabel.Name = "FirmwareVisionLabel";
            FirmwareVisionLabel.Size = new System.Drawing.Size(32, 17);
            FirmwareVisionLabel.TabIndex = 8;
            FirmwareVisionLabel.Text = "未知";
            // 
            // LEDBrightnessLabel
            // 
            LEDBrightnessLabel.AutoSize = true;
            LEDBrightnessLabel.Location = new System.Drawing.Point(125, 85);
            LEDBrightnessLabel.Name = "LEDBrightnessLabel";
            LEDBrightnessLabel.Size = new System.Drawing.Size(32, 17);
            LEDBrightnessLabel.TabIndex = 7;
            LEDBrightnessLabel.Text = "未知";
            // 
            // IsLEDEnableLabel
            // 
            IsLEDEnableLabel.AutoSize = true;
            IsLEDEnableLabel.Location = new System.Drawing.Point(125, 58);
            IsLEDEnableLabel.Name = "IsLEDEnableLabel";
            IsLEDEnableLabel.Size = new System.Drawing.Size(32, 17);
            IsLEDEnableLabel.TabIndex = 6;
            IsLEDEnableLabel.Text = "未知";
            // 
            // IsHighBaudrateLabel
            // 
            IsHighBaudrateLabel.AutoSize = true;
            IsHighBaudrateLabel.Location = new System.Drawing.Point(125, 29);
            IsHighBaudrateLabel.Name = "IsHighBaudrateLabel";
            IsHighBaudrateLabel.Size = new System.Drawing.Size(32, 17);
            IsHighBaudrateLabel.TabIndex = 5;
            IsHighBaudrateLabel.Text = "未知";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(27, 141);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(68, 17);
            label6.TabIndex = 4;
            label6.Text = "硬件版本：";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(27, 114);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(68, 17);
            label5.TabIndex = 3;
            label5.Text = "固件版本：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(27, 85);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(66, 17);
            label4.TabIndex = 2;
            label4.Text = "LED亮度：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(27, 58);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(66, 17);
            label3.TabIndex = 1;
            label3.Text = "LED状态：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(27, 29);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(92, 17);
            label2.TabIndex = 0;
            label2.Text = "高波特率模式：";
            // 
            // AboutButton
            // 
            AboutButton.Location = new System.Drawing.Point(632, 41);
            AboutButton.Name = "AboutButton";
            AboutButton.Size = new System.Drawing.Size(276, 41);
            AboutButton.TabIndex = 6;
            AboutButton.Text = "关于本程序";
            AboutButton.UseVisualStyleBackColor = true;
            AboutButton.Click += AboutButton_Click;
            // 
            // SegaModeSettingButton
            // 
            SegaModeSettingButton.Enabled = false;
            SegaModeSettingButton.Location = new System.Drawing.Point(632, 88);
            SegaModeSettingButton.Name = "SegaModeSettingButton";
            SegaModeSettingButton.Size = new System.Drawing.Size(276, 41);
            SegaModeSettingButton.TabIndex = 7;
            SegaModeSettingButton.Text = "SEGA模式设置";
            SegaModeSettingButton.UseVisualStyleBackColor = true;
            SegaModeSettingButton.Click += SegaModeSettingButton_Click;
            // 
            // EnteryNamcoModeButton
            // 
            EnteryNamcoModeButton.Enabled = false;
            EnteryNamcoModeButton.Location = new System.Drawing.Point(632, 135);
            EnteryNamcoModeButton.Name = "EnteryNamcoModeButton";
            EnteryNamcoModeButton.Size = new System.Drawing.Size(276, 41);
            EnteryNamcoModeButton.TabIndex = 8;
            EnteryNamcoModeButton.Text = "进入Namco模式";
            EnteryNamcoModeButton.UseVisualStyleBackColor = true;
            EnteryNamcoModeButton.Click += EnteryNamcoModeButton_Click;
            // 
            // EnterySpiceModeButton
            // 
            EnterySpiceModeButton.Enabled = false;
            EnterySpiceModeButton.Location = new System.Drawing.Point(632, 182);
            EnterySpiceModeButton.Name = "EnterySpiceModeButton";
            EnterySpiceModeButton.Size = new System.Drawing.Size(276, 41);
            EnterySpiceModeButton.TabIndex = 9;
            EnterySpiceModeButton.Text = "进入Spice模式";
            EnterySpiceModeButton.UseVisualStyleBackColor = true;
            EnterySpiceModeButton.Click += EnterySpiceModeButton_Click;
            // 
            // EnteryPN532ModeButton
            // 
            EnteryPN532ModeButton.Enabled = false;
            EnteryPN532ModeButton.Location = new System.Drawing.Point(632, 229);
            EnteryPN532ModeButton.Name = "EnteryPN532ModeButton";
            EnteryPN532ModeButton.Size = new System.Drawing.Size(276, 41);
            EnteryPN532ModeButton.TabIndex = 10;
            EnteryPN532ModeButton.Text = "进入PN532直通模式";
            EnteryPN532ModeButton.UseVisualStyleBackColor = true;
            EnteryPN532ModeButton.Click += EnteryPN532ModeButton_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = System.Drawing.Color.Red;
            label8.Location = new System.Drawing.Point(632, 324);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(287, 51);
            label8.TabIndex = 11;
            label8.Text = "请注意！本工具连接到读卡器后读卡器会被重置到\n默认的SEGA模式！！！\n另外，读卡器EEPROM寿命有限，请不要频繁修改！";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            label9.Location = new System.Drawing.Point(651, 4);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(236, 34);
            label9.TabIndex = 12;
            label9.Text = "本工具完全免费，如果你从哪里购买到了，\n那你就是被骗了！";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FirmwareUpgreadButton
            // 
            FirmwareUpgreadButton.Enabled = false;
            FirmwareUpgreadButton.Location = new System.Drawing.Point(632, 276);
            FirmwareUpgreadButton.Name = "FirmwareUpgreadButton";
            FirmwareUpgreadButton.Size = new System.Drawing.Size(276, 41);
            FirmwareUpgreadButton.TabIndex = 13;
            FirmwareUpgreadButton.Text = "固件升级";
            FirmwareUpgreadButton.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(920, 450);
            Controls.Add(FirmwareUpgreadButton);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(EnteryPN532ModeButton);
            Controls.Add(EnterySpiceModeButton);
            Controls.Add(EnteryNamcoModeButton);
            Controls.Add(SegaModeSettingButton);
            Controls.Add(AboutButton);
            Controls.Add(DeviceInfoBox);
            Controls.Add(disconnectDeviceButton);
            Controls.Add(label1);
            Controls.Add(devicesComboBox);
            Controls.Add(connectDeviceButton);
            Controls.Add(refreshDevicesButton);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainWindow";
            Text = "街机游戏读卡器配置程序";
            Load += MainWindow_Load;
            DeviceInfoBox.ResumeLayout(false);
            DeviceInfoBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button refreshDevicesButton;
        private System.Windows.Forms.Button connectDeviceButton;
        private System.Windows.Forms.ComboBox devicesComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button disconnectDeviceButton;
        private System.Windows.Forms.GroupBox DeviceInfoBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label HardwareVisionLabel;
        private System.Windows.Forms.Label FirmwareVisionLabel;
        private System.Windows.Forms.Label LEDBrightnessLabel;
        private System.Windows.Forms.Label IsLEDEnableLabel;
        private System.Windows.Forms.Label IsHighBaudrateLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label DeviceVersionLabel;
        private System.Windows.Forms.Button CardReadTestButton;
        private System.Windows.Forms.TextBox CardReadTestTextBox;
        private System.Windows.Forms.Button CopyCardReadTestTextButton;
        private System.Windows.Forms.Button AboutButton;
        private System.Windows.Forms.Button SegaModeSettingButton;
        private System.Windows.Forms.Button EnteryNamcoModeButton;
        private System.Windows.Forms.Button EnterySpiceModeButton;
        private System.Windows.Forms.Button EnteryPN532ModeButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button FirmwareUpgreadButton;
    }
}
