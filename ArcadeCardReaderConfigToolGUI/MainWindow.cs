using System;
using System.IO.Ports;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;

namespace ArcadeCardReaderConfigToolGUI
{
    public partial class MainWindow : Form
    {
        private SerialPort serialPort;
        private bool isConnected = false;
        private bool isHighBaudRate, isLEDEnabled;
        private int LEDBrightness = 0, firmwareVision = 0;
        private CancellationTokenSource? testCancellationToken;

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

        public MainWindow()
        {
            InitializeComponent();
            serialPort = new SerialPort();
        }

        // 使用 WMI 获取真实的串口设备
        private string[] GetWmiPorts()
        {
            var portList = new List<string>();
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SerialPort"))
                {
                    foreach (var port in searcher.Get())
                    {
                        portList.Add(port["DeviceID"].ToString() ?? "空端口号");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"WMI 读取错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return portList.ToArray();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            connectDeviceButton.Enabled = true;
            disconnectDeviceButton.Enabled = false;
            try
            {
                System.Threading.Thread.Sleep(500); // 等待设备管理器更新

                // string[] portNames = SerialPort.GetPortNames();
                devicesComboBox.Items.Clear();

                // 使用 WMI 获取最新的串口数据
                string[] wmiPorts = GetWmiPorts();

                if (wmiPorts.Length > 0)
                {
                    devicesComboBox.Items.AddRange(wmiPorts);
                    devicesComboBox.SelectedIndex = 0;
                }
                else
                {
                    devicesComboBox.Items.Add("无可用串口");
                    devicesComboBox.SelectedIndex = 0;
                }
            }
            catch (PlatformNotSupportedException ex)
            {
                MessageBox.Show($"平台不支持串口通信: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshDevicesButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(500); // 等待设备管理器更新

                // string[] portNames = SerialPort.GetPortNames();
                devicesComboBox.Items.Clear();

                // 使用 WMI 获取最新的串口数据
                string[] wmiPorts = GetWmiPorts();

                if (wmiPorts.Length > 0)
                {
                    devicesComboBox.Items.AddRange(wmiPorts);
                    devicesComboBox.SelectedIndex = 0;
                }
                else
                {
                    devicesComboBox.Items.Add("无可用串口");
                    devicesComboBox.SelectedIndex = 0;
                }
            }
            catch (PlatformNotSupportedException ex)
            {
                MessageBox.Show($"平台不支持串口通信: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔹 验证串口是否有效
        private bool IsPortValid(string portName)
        {
            return SerialPort.GetPortNames().Contains(portName);
        }

        private void connectDeviceButton_Click(object sender, EventArgs e)
        {
            if (devicesComboBox.SelectedItem == null || devicesComboBox.SelectedItem.ToString() == "无可用串口")
            {
                MessageBox.Show("请选择一个有效的串口！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedPort = devicesComboBox.SelectedItem.ToString()!;
            if (!IsPortValid(selectedPort))
            {
                MessageBox.Show("选择的串口无效，请重新选择！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                serialPort = new SerialPort(selectedPort, 115200, Parity.None, 8, StopBits.One);
                serialPort.ReadTimeout = 1000;
                serialPort.WriteTimeout = 1000;
                serialPort.Open();

                isConnected = true;
                connectDeviceButton.Enabled = false;
                disconnectDeviceButton.Enabled = true;
                CardReadTestButton.Enabled = true;
                devicesComboBox.Enabled = false;
                refreshDevicesButton.Enabled = false;
                SegaModeSettingButton.Enabled = true;
                EnteryNamcoModeButton.Enabled = true;
                EnterySpiceModeButton.Enabled = true;
                EnteryPN532ModeButton.Enabled = true;
                //MessageBox.Show($"成功连接到 {selectedPort}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔹 发送初始化命令
                InitializeDevice();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔹 发送初始化命令
        private void InitializeDevice()
        {
            if (!isConnected || serialPort == null || !serialPort.IsOpen) return;

            try
            {
                byte[] resetCmd = Enumerable.Repeat((byte)0xAF, 30).ToArray();
                SendData(resetCmd);

                ChangeBaudRate(38400);
                SendData(resetCmd);

                ChangeBaudRate(115200);
                SendData(resetCmd);

                // 发送读取 EEPROM 指令
                //ChangeBaudRate(38400);
                ChangeBaudRate(115200);
                byte[] eepromCmd = { 0xE0, 0x06, 0x00, 0x00, 0xF6, 0x00, 0x00, 0xFC };
                SendData(eepromCmd);

                Thread.Sleep(100);

                // 读取返回数据
                byte[]? response = ReceiveData(12);

                //string hexResponse = BitConverter.ToString(response).Replace("-", " ");

                //MessageBox.Show($"收到的设备返回数据: {hexResponse}", "返回数据", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (response != null && response.Length >= 4 && response[4] == 0xF6)
                {
                    //MessageBox.Show("读卡器初始化成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ParseEEPROMData(response);
                }
                else
                {
                    ChangeBaudRate(38400);
                    SendData(eepromCmd);

                    Thread.Sleep(100);

                    // 读取返回数据
                    response = ReceiveData(12);

                    //string hexResponse = BitConverter.ToString(response).Replace("-", " ");

                    //MessageBox.Show($"收到的设备返回数据: {hexResponse}", "返回数据", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (response != null && response.Length >= 4 && response[4] == 0xF6)
                    {
                        //MessageBox.Show("读卡器初始化成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ParseEEPROMData(response);
                    }
                    else
                    {
                        MessageBox.Show("读卡器连接错误，请检查设备！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        disconnectDeviceButton_Click(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设备初始化失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔹 解析 EEPROM 数据并显示
        private void ParseEEPROMData(byte[] data)
        {
            bool highBaudRate = (data[7] & 0x02) != 0;  // 第8个字节的第2位表示高波特率模式
            bool ledEnabled = (data[7] & 0x04) != 0;    // 第8个字节的第3位表示LED启用
            int ledBrightness = data[8];                // 第9个字节表示LED亮度
            int firmwareVersion = data[9];              // 第10个字节表示固件版本号
            int hardwareVersion = data[10];             // 第11个字节表示硬件版本号
            isHighBaudRate = highBaudRate;              // 获取到的信息存入全局变量
            isLEDEnabled = ledEnabled;
            LEDBrightness = ledBrightness;
            firmwareVision = firmwareVersion;

            string hardwareType = hardwareVersion switch
            {
                1 => "ATmega32U4",
                2 => "SAMD21",
                3 => "ESP8266",
                4 => "ESP32",
                5 => "AIR001/PY32F002",
                6 => "STM32F1",
                7 => "STM32F0",
                8 => "RP2040",
                9 => "ATmega328P",
                10 => "ESP32C3",
                _ => "未知"
            };

            // 🔹 在弹窗中显示设备信息
            //MessageBox.Show(
            //    $"读卡器状态信息：\n" +
            //    $"高波特率模式：{(highBaudRate ? "是" : "否")}\n" +
            //    $"LED 启用：{(ledEnabled ? "是" : "否")}\n" +
            //    $"LED 亮度：{ledBrightness}\n" +
            //    $"固件版本：v{firmwareVersion}\n" +
            //    $"硬件版本：{hardwareType}",
            //    "设备信息",
            //    MessageBoxButtons.OK,
            //    MessageBoxIcon.Information
            //);
            IsHighBaudrateLabel.Text = $"{(highBaudRate ? "已启用" : "未启用")}";
            IsLEDEnableLabel.Text = $"{(ledEnabled ? "已启用" : "未启用")}";
            LEDBrightnessLabel.Text = $"{ledBrightness}";
            FirmwareVisionLabel.Text = $"{firmwareVersion}";
            HardwareVisionLabel.Text = $"{hardwareType}";
            DeviceVersionLabel.Text = $"Ver. {firmwareVersion}-{hardwareType}";

        }

        // 🔹 发送数据到串口
        private void SendData(byte[] data)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Write(data, 0, data.Length);
            }
        }

        // 🔹 读取串口数据
        private byte[]? ReceiveData(int length)
        {
            try
            {
                byte[] buffer = new byte[length];
                int bytesRead = serialPort.Read(buffer, 0, length);
                if (bytesRead > 0)
                {
                    return buffer.Take(bytesRead).ToArray();
                }
            }
            catch (TimeoutException) { }
            return null;
        }

        // 🔹 修改波特率
        private void ChangeBaudRate(int baudrate)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.BaudRate = baudrate;
            }
        }

        // 🔹 断开连接
        private void disconnectDeviceButton_Click(object sender, EventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }

            isConnected = false;
            connectDeviceButton.Enabled = true;
            disconnectDeviceButton.Enabled = false;
            CardReadTestButton.Enabled = false;
            devicesComboBox.Enabled = true;
            refreshDevicesButton.Enabled = true;
            SegaModeSettingButton.Enabled = false;
            EnteryNamcoModeButton.Enabled = false;
            EnterySpiceModeButton.Enabled = false;
            EnteryPN532ModeButton.Enabled = false;
            IsHighBaudrateLabel.Text = "未知";
            IsLEDEnableLabel.Text = "未知";
            LEDBrightnessLabel.Text = "未知";
            FirmwareVisionLabel.Text = "未知";
            HardwareVisionLabel.Text = "未知";
            DeviceVersionLabel.Text = "未知";
            //MessageBox.Show("设备已断开连接", "断开连接", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void CardReadTestButton_Click(object sender, EventArgs e)
        {
            try
            {
                CardReadTestButton.Enabled = false;  // 禁用按钮，防止重复点击

                SegaModeSettingButton.Enabled = false;
                EnteryNamcoModeButton.Enabled = false;
                EnterySpiceModeButton.Enabled = false;
                EnteryPN532ModeButton.Enabled = false;

                if (firmwareVision <= 4)
                {
                    if (MessageBox.Show("固件版本v4以下不支持读卡测试模式，请按“取消”退出，如果确定将继续进入，可能会发生无法预计的结果。", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                    {
                        CardReadTestButton.Enabled = true;

                        SegaModeSettingButton.Enabled = true;
                        EnteryNamcoModeButton.Enabled = true;
                        EnterySpiceModeButton.Enabled = true;
                        EnteryPN532ModeButton.Enabled = true;

                        return;
                    }
                }

                if (MessageBox.Show("即将进入读卡测试模式，读卡测试模式必须通过拔掉USB线缆（断电后重新上电）才能退出，如果不退出会导致无法正常使用读卡器！\n现在点击取消还来得及~", "警告 - 即将进入读卡测试模式", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    CardReadTestButton.Enabled = true;

                    SegaModeSettingButton.Enabled = true;
                    EnteryNamcoModeButton.Enabled = true;
                    EnterySpiceModeButton.Enabled = true;
                    EnteryPN532ModeButton.Enabled = true;

                    return;
                }

                //MessageBox.Show($"成功连接到 {selectedPort}，正在进入读卡测试模式...", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (isHighBaudRate)
                {
                    ChangeBaudRate(115200);
                }
                else
                {
                    ChangeBaudRate(38400);
                }

                // 🔹 发送进入测试模式的指令
                byte[] testModeCommand = { 0xE0, 0x06, 0x00, 0x00, 0xF8, 0x01, 0x03, 0x02 };
                SendData(testModeCommand);

                // 🔹 启动后台线程持续读取数据
                testCancellationToken = new CancellationTokenSource();
                await Task.Run(() => ReadTestModeData(testCancellationToken.Token), testCancellationToken.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法进入读卡测试模式: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔹 持续读取测试模式数据
        private async Task ReadTestModeData(CancellationToken token)
        {
            StringBuilder dataBuffer = new StringBuilder(); // 🔹 用于存储完整数据
            DateTime lastReceivedTime = DateTime.Now; // 🔹 记录上次收到数据的时间
            TimeSpan timeout = TimeSpan.FromMilliseconds(500); // 🔹 设定 500ms 超时时间

            while (!token.IsCancellationRequested)
            {
                try
                {
                    // 🔹 读取数据
                    if (serialPort.BytesToRead > 0)
                    {
                        byte[] buffer = new byte[1024]; // 🔹 和 C++ 一样的缓冲区
                        int bytesRead = await serialPort.BaseStream.ReadAsync(buffer, 0, buffer.Length, token);

                        if (bytesRead > 0)
                        {
                            string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            dataBuffer.Append(receivedData);  // 🔹 缓存数据
                            lastReceivedTime = DateTime.Now;  // 🔹 更新上次收到数据的时间
                        }
                    }
                    else
                    {
                        // 🔹 如果超时未收到新数据，就清屏并显示
                        if ((DateTime.Now - lastReceivedTime) > timeout && dataBuffer.Length > 0)
                        {
                            string finalData = dataBuffer.ToString();
                            dataBuffer.Clear(); // 🔹 清空缓存

                            // 🚀 先清屏，再显示完整数据
                            Invoke(new Action(() =>
                            {
                                CardReadTestTextBox.Clear();
                                CardReadTestTextBox.AppendText(finalData + Environment.NewLine);
                            }));
                        }

                        await Task.Delay(50); // 🔹 防止 CPU 100% 占用
                    }
                }
                catch (TimeoutException) { }
                catch (Exception ex)
                {
                    Invoke(new Action(() => MessageBox.Show($"读取数据出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                    break;
                }
            }
        }

        private void CopyCardReadTestTextButton_Click(object sender, EventArgs e)
        {
            string text = CardReadTestTextBox.Text;
            if (!string.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text); // 🔹 将文本写入剪切板
                //MessageBox.Show("内容已复制到剪切板！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("复制的内容为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            // 获取当前正在执行的程序集
            Assembly assembly = Assembly.GetExecutingAssembly();
            // 通过程序集位置获取文件版本信息
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            Console.WriteLine("FileVersion: " + fvi.FileVersion);
            MessageBox.Show($"街机游戏读卡器配置程序（图形界面版）\n\n程序版本：Ver. {fvi.FileVersion}\n完全兼容的最高固件版本：10\n\n软件作者：Tatanako\n硬件作者：Qinh\n特别鸣谢：Soda（送我了读卡器硬件）\n\n本软件基于\nhttps://github.com/QHPaeek/Arduino-Aime-Reader/blob/develop/tools/BaudRateTool/baudrate_tool.c\n编写完成，感谢Qinh开源~\n\n本软件完全免费，代码开源于GitHub：\nhttps://github.com/tatanakots/ArcadeCardReaderConfigToolGUI", "关于本软件", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        public void ChangeMode(byte mode)
        {
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("串口未打开！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PacketRequest req = new PacketRequest
            {
                frame_len = 6 + 1,
                addr = 0,
                seq_no = 0,
                cmd = CMD_SW_MODE,
                payload_len = 1,
                mode = mode
            };

            PacketWrite(req);
            MessageBox.Show($"模式切换命令已发送: {mode}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EnteryNamcoModeButton_Click(object sender, EventArgs e)
        {
            EnteryNamcoModeButton.Enabled = false;
            ChangeMode(NAMCO_MODE); // Namco 模式
            disconnectDeviceButton_Click(this, EventArgs.Empty);
        }

        private void EnterySpiceModeButton_Click(object sender, EventArgs e)
        {
            EnterySpiceModeButton.Enabled = false;
            ChangeMode(SPICE_MODE); // Spice 模式
            disconnectDeviceButton_Click(this, EventArgs.Empty);
        }

        private void EnteryPN532ModeButton_Click(object sender, EventArgs e)
        {
            EnteryPN532ModeButton.Enabled = false;
            ChangeMode(RAW_MODE);
            disconnectDeviceButton_Click(this, EventArgs.Empty);
        }

        private void SegaModeSettingButton_Click(object sender, EventArgs e)
        {
            SegaModeSettingButton.Enabled = false;
            this.Enabled = false;
            SEGAModeSettingWindow segaModeSettingWindow = new SEGAModeSettingWindow(isHighBaudRate, isLEDEnabled, LEDBrightness, serialPort);
            segaModeSettingWindow.ShowDialog();
            this.Enabled = true;
            SegaModeSettingButton.Enabled = true;
            disconnectDeviceButton_Click(this, EventArgs.Empty);
            connectDeviceButton_Click(this, EventArgs.Empty);
        }
    }
}