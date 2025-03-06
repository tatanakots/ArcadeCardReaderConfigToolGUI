# 开源街机游戏读卡器GUI配置程序

本项目是一个基于 C# 的 .NET Windows 窗体应用程序，用于配置街机游戏读卡器。该程序参考了 [Arduino-Aime-Reader 命令行配置工具](https://github.com/QHPaeek/Arduino-Aime-Reader/blob/develop/tools/BaudRateTool/baudrate_tool.c) 的设计理念，提供了直观的图形化配置界面，使用户能够更加便捷地完成设备配置。本项目仅支持 Windows 平台。

## 特性

- **Windows 窗体应用**：采用 C# 和 .NET 框架开发，专为 Windows 系统设计。
- **直观易用**：图形用户界面（GUI）简化了配置流程，降低了使用门槛。

## 环境要求

- Windows 操作系统（仅支持 Windows 平台）
- .NET Framework 或 .NET Core（具体版本请参考项目配置）
- Visual Studio 或其他支持 C# 开发的 IDE

## 安装与构建

1. 克隆本项目代码：
   ```bash
   git clone https://github.com/tatanakots/ArcadeCardReaderConfigToolGUI.git
   ```
2. 打开解决方案文件（.sln）：
   - 使用 Visual Studio 打开项目文件。
3. 编译解决方案：
   - 在 Visual Studio 中选择“生成解决方案”，或使用命令行工具进行构建。
4. 编译完成后，即可在 Windows 环境下运行生成的可执行文件。

## 使用方法

- 从[Release](https://github.com/tatanakots/ArcadeCardReaderConfigToolGUI/releases)中下载预编译的程序包并运行。

## 参考资料

- [Arduino-Aime-Reader 命令行配置工具](https://github.com/QHPaeek/Arduino-Aime-Reader/blob/develop/tools/BaudRateTool/baudrate_tool.c)

## 贡献

欢迎大家参与本项目的开发与优化！如有任何建议、问题或改进方案，请通过提交 Pull Request 或 Issue 与我们联系。

## 许可证

本项目遵循 MIT 许可证，详细信息请参见 [LICENSE](LICENSE) 文件。
