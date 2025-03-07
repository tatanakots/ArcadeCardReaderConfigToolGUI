using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcadeCardReaderConfigToolGUI
{
    public partial class SEGAModeSettingWindow : Form
    {
        private SerialPort serialPort;
        private bool isHighBaudRate, isLEDEnabled;
        private int LEDBrightness = 0;

        // 命令定义
        private const byte CMD_READ_EEPROM = 0xF6;
        private const byte CMD_WRITE_EEPROM = 0xF7;
        private const byte CMD_SW_MODE = 0xF8;
        private const byte CMD_READ_MODE = 0xF9;

        // 模式定义
        private const byte SEGA_MODE = 0;
        private const byte SPICE_MODE = 1;
        private const byte NAMCO_MODE = 2;
        private const byte TEST_MODE = 3;
        private const byte RAW_MODE = 4;

        // **📌 结构体定义，完全匹配 C++**
        [StructLayout(LayoutKind.Explicit, Size = 128, Pack = 1)]
        public unsafe struct PacketRequest
        {
            // 整个128字节的数组视图
            [FieldOffset(0)]
            public fixed byte bytes[128];

            // 按字段访问的视图
            [FieldOffset(0)]
            public byte frame_len;
            [FieldOffset(1)]
            public byte addr;
            [FieldOffset(2)]
            public byte seq_no;
            [FieldOffset(3)]
            public byte cmd;
            [FieldOffset(4)]
            public byte payload_len;

            // 从偏移量5开始的union部分
            // 第一种视图：单个字节 mode
            [FieldOffset(5)]
            public byte mode;

            // 第二种视图：详细结构体
            [FieldOffset(5)]
            public InnerPayload payloadDetails;

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public unsafe struct InnerPayload
            {
                // 依次为 eeprom_data[2], mapped_IDm[8], target_accesscode[10]
                public fixed byte eeprom_data[2];
                public fixed byte mapped_IDm[8];
                public fixed byte target_accesscode[10];
            }
        }

        public SEGAModeSettingWindow(bool isHighBaudRate_, bool isLEDEnabled_, int LEDBrightness_, SerialPort serialPort_)
        {
            InitializeComponent();
            serialPort = serialPort_;
            isHighBaudRate = isHighBaudRate_;
            isLEDEnabled = isLEDEnabled_;
            LEDBrightness = LEDBrightness_;
            if (isHighBaudRate)
            {
                IsHighBaudrateComboBox.SelectedIndex = 0;
            }
            else
            {
                IsHighBaudrateComboBox.SelectedIndex = 1;
            }
            if (isLEDEnabled)
            {
                IsLEDEnableComboBox.SelectedIndex = 0;
            }
            else
            {
                IsLEDEnableComboBox.SelectedIndex = 1;
            }
            LEDBrightnessTrackBar.Value = LEDBrightness;
            LEDBrightnessLabel.Text = $"{LEDBrightness}";
            EnableCardReaderPlusComboBox.SelectedIndex = 1;
            EnableSpice2PComboBox.SelectedIndex = 1;
            EnableReCardNumberComboBox.SelectedIndex = 1;
        }

        private void LEDBrightnessTrackBar_DataContextChanged(object sender, EventArgs e)
        {
            LEDBrightnessLabel.Text = $"{LEDBrightnessTrackBar.Value}";
        }

        private void LEDBrightnessTrackBar_Scroll(object sender, EventArgs e)
        {
            LEDBrightnessLabel.Text = $"{LEDBrightnessTrackBar.Value}";
        }

        private void ChangeBaudRate(int baudrate)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.BaudRate = baudrate;
            }
        }

        private unsafe void PacketWrite(PacketRequest req)
        {
            byte checksum = 0, len = 0;
            List<byte> sendBuffer = new List<byte>(); // 使用 List<byte> 方便动态添加数据
            //byte send_len;
            if (req.cmd == 0)
            {
                return;
            }
            sendBuffer.Add(0xE0); // 起始字节
            while (len <= req.frame_len)
            {
                byte w;
                if (len == req.frame_len)
                {
                    w = checksum;
                }
                else
                {
                    w = req.bytes[len];
                    checksum += w;
                }
                if (w == 0xE0 || w == 0xD0)
                {
                    sendBuffer.Add(0xD0);
                    sendBuffer.Add(--w);
                }
                else
                {
                    sendBuffer.Add(w);
                }
                len++;
            }

            //checksum = req.FrameLength;
            //sendBuffer.Add(req.FrameLength);
            //sendBuffer.Add(req.Addr);
            //sendBuffer.Add(req.SeqNo);
            //sendBuffer.Add(req.Cmd);
            //sendBuffer.Add(req.PayloadLen);

            //checksum += (byte)(req.Addr + req.SeqNo + req.Cmd + req.PayloadLen);

            //byte[] payloadBytes = new byte[req.PayloadLen];
            //payloadBytes[0] = req.Mode;

            //int len = 0;
            //while (len <= req.FrameLength)
            //{
            //    byte w;
            //    if (len == req.FrameLength)
            //    {
            //        w = checksum; // 最后一个字节是校验和
            //    }
            //    else
            //    {
            //        w = req.Data[len]; // 读取数据
            //        checksum += w;
            //    }

            //    // **📌 处理 `E0` 和 `D0` 的转义**
            //    if (w == 0xE0 || w == 0xD0)
            //    {
            //        sendBuffer.Add(0xD0);
            //        sendBuffer.Add((byte)(w - 1)); // 递减 1 进行转义
            //    }
            //    else
            //    {
            //        sendBuffer.Add(w);
            //    }
            //    len++;
            //}

            try
            {
                if (isHighBaudRate)
                {
                    ChangeBaudRate(115200);
                }
                else
                {
                    ChangeBaudRate(38400);
                }
                serialPort.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"写入数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableReCardNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EnableReCardNumberComboBox.SelectedIndex == 1)
            {
                ReCardNumberIDmTextBox.Enabled = false;
                ReCardNumberTextBox.Enabled = false;
            }
            else
            {
                ReCardNumberIDmTextBox.Enabled = true;
                ReCardNumberTextBox.Enabled = true;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsValidIDmString(string input)
        {
            // 空字符串或长度不为16则返回false
            if (string.IsNullOrEmpty(input) || input.Length != 16)
                return false;
            // 正则表达式：^[0-9A-Fa-f]{16}$ 表示必须是16位16进制字符，无前缀和空格
            return Regex.IsMatch(input, @"^[0-9A-Fa-f]{16}$");
        }

        private bool IsValidIDString(string input)
        {
            // 如果为空或者长度不为20则返回false
            if (string.IsNullOrEmpty(input) || input.Length != 20)
                return false;

            // 正则表达式：^[0-9]{20}$ 表示必须是20位数字
            return Regex.IsMatch(input, @"^[0-9]{20}$");
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            byte[] system_setting_buffer = new byte[2];
            byte change_highbaudrate_mode = 0;
            if (IsHighBaudrateComboBox.SelectedIndex < 0 || IsHighBaudrateComboBox.SelectedIndex > 1)
            {
                MessageBox.Show("高波特率模式选择框格式错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (IsLEDEnableComboBox.SelectedIndex < 0 || IsLEDEnableComboBox.SelectedIndex > 1)
            {
                MessageBox.Show("LED状态选择框格式错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (LEDBrightnessTrackBar.Value < 0 || LEDBrightnessTrackBar.Value > 255)
            {
                MessageBox.Show("LED亮度数据格式错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (EnableCardReaderPlusComboBox.SelectedIndex < 0 || EnableCardReaderPlusComboBox.SelectedIndex > 1)
            {
                MessageBox.Show("扩展读卡选择框格式错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (EnableSpice2PComboBox.SelectedIndex < 0 || EnableSpice2PComboBox.SelectedIndex > 1)
            {
                MessageBox.Show("SPICE模式下进入2P刷卡模式选择框格式错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (EnableReCardNumberComboBox.SelectedIndex < 0 || EnableReCardNumberComboBox.SelectedIndex > 1)
            {
                MessageBox.Show("卡号映射选择框格式错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!IsValidIDmString(ReCardNumberIDmTextBox.Text) && EnableReCardNumberComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("卡号映射源卡片IDm格式错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!IsValidIDString(ReCardNumberTextBox.Text) && EnableReCardNumberComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("卡号映射后卡号格式错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (isHighBaudRate)
            {
                ChangeBaudRate(115200);
            }
            else
            {
                ChangeBaudRate(38400);
            }

            // 逻辑开始
            if (IsHighBaudrateComboBox.SelectedIndex == 0) // 如果启用高波特率模式
            {
                // 设置system_setting_buffer[0] 的第二位置1
                system_setting_buffer[0] |= 0x02;
                change_highbaudrate_mode = 1;
            }
            else
            {
                change_highbaudrate_mode = 0;
            }

            if (IsLEDEnableComboBox.SelectedIndex == 0) // 如果启用LED
            {
                // 设置system_setting_buffer[0] 的第三位置1
                system_setting_buffer[0] |= 0x04;
            }

            int brightness = LEDBrightnessTrackBar.Value;
            system_setting_buffer[1] = (byte)brightness; // 将输入的数值赋给system_setting_buffer[1]

            if (EnableCardReaderPlusComboBox.SelectedIndex == 0) // 如果启用扩展读卡
            {
                // 设置system_setting_buffer[0] 的第4位置1
                system_setting_buffer[0] |= 0b10000;
            }

            if (EnableSpice2PComboBox.SelectedIndex == 0) // 如果启用SPICE模式下进入2P刷卡模式
            {
                // 设置system_setting_buffer[0] 的第4位置1
                system_setting_buffer[0] |= 0b10000;
            }

            byte card_reflect = 0;

            if (EnableReCardNumberComboBox.SelectedIndex == 0) // 如果启用卡号映射功能
            {
                // 设置system_setting_buffer[0] 的第4位置1
                system_setting_buffer[0] |= 0b1000;
                card_reflect = 1;
            }

            //TODO
            byte[] card_IDm = new byte[8];
            byte[] card_accesscode = new byte[10]; // 用于存放转换后的10字节数据
            if (card_reflect == 1)
            {
                card_IDm = HexStringToByteArray(ReCardNumberIDmTextBox.Text);
                card_accesscode = ProcessDecimalStringToByteArray(ReCardNumberTextBox.Text);
            }

            // 构造发送数据的缓冲区（28字节）
            byte[] uart_send_buffer = new byte[28]
            {
                0xE0, 0x1A, 0x00, 0x00, 0xF7, 0x14, 0x0E, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00
            };

            // 将 system_setting_buffer 放入指定位置
            uart_send_buffer[6] = system_setting_buffer[0];
            uart_send_buffer[7] = system_setting_buffer[1];

            // 将 card_IDm (8字节) 填入 uart_send_buffer 从第8字节开始
            for (int i = 0; i < 8; i++)
            {
                uart_send_buffer[i + 8] = card_IDm[i];
            }

            // 将 card_accesscode (10字节) 填入 uart_send_buffer 从第16字节开始
            for (int i = 0; i < 10; i++)
            {
                uart_send_buffer[i + 16] = card_accesscode[i];
            }

            // 计算校验和：将 uart_send_buffer[1] 到 uart_send_buffer[26] 的所有字节相加后存入最后一个字节
            uart_send_buffer[27] = 0;
            for (int i = 0; i < 26; i++)
            {
                uart_send_buffer[27] += uart_send_buffer[i + 1];
            }

            // 发送数据到串口
            try
            {
                serialPort.Write(uart_send_buffer, 0, uart_send_buffer.Length);
                Thread.Sleep(100);
                if (change_highbaudrate_mode == 1)
                {
                    ChangeBaudRate(115200);
                }
                else
                {
                    ChangeBaudRate(38400);
                }
                MessageBox.Show("配置修改完成", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入串口数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        // ----- 辅助函数 -----
        /// <summary>
        /// 将16进制字符串（无前缀、无空格）转换为字节数组，每两个字符转换为1个字节
        /// </summary>
        private byte[] HexStringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            if (numberChars % 2 != 0)
                throw new ArgumentException("16进制字符串长度必须为偶数。");
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// 将20位纯数字字符串转换为10字节数组，每2个字符组合成1个字节（对应0~99的数值）
        /// </summary>
        private byte[] ProcessDecimalStringToByteArray(string dec)
        {
            if (dec.Length != 20)
                throw new ArgumentException("输入的数字字符串必须为20位。");
            byte[] result = new byte[10];
            for (int i = 0; i < 10; i++)
            {
                string pair = dec.Substring(i * 2, 2);
                result[i] = (byte)int.Parse(pair);
            }
            return result;
        }
    }
}
