using System.Windows.Forms;

// 显式声明 DPI 感知：否则 Windows 会把 1000×800 的无感知窗口按 150% 拉伸到 1500×1200。
Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

using var game = new MonoCoC.CoC();

game.Run();