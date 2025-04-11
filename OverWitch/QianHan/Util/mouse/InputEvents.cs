using System.Runtime.InteropServices;

namespace Assets.OverWitch.QianHan.Util.mouse
{
    public class InputEvents
    {
        [DllImport("user32.dll")]
        public static extern short getAsyncKeyState(int vKey);
        [DllImport("user32.dll")]
        public static extern short getKeyState(int vKey);
        // 常用的键盘和鼠标虚拟键码
        private const int VK_LBUTTON = 0x01;  // 左键
        private const int VK_RBUTTON = 0x02;  // 右键
        private const int VK_MBUTTON = 0x04;  // 中键
        private const int VK_ESCAPE = 0x1B;   // ESC 键
        private const int VK_SPACE = 0x20;    // 空格键
        private const int VK_ENTER = 0x0D;    // 回车键
        // 鼠标和键盘状态检测
        public bool IsLeftButtonPressed()
        {
            return (getAsyncKeyState(VK_LBUTTON) & 0x8000) != 0;
        }

        public bool IsRightButtonPressed()
        {
            return (getAsyncKeyState(VK_RBUTTON) & 0x8000) != 0;
        }

        public bool IsMiddleButtonPressed()
        {
            return (getAsyncKeyState(VK_MBUTTON) & 0x8000) != 0;
        }

        public bool IsEscapePressed()
        {
            return (getAsyncKeyState(VK_ESCAPE) & 0x8000) != 0;
        }

        public bool IsSpacePressed()
        {
            return (getAsyncKeyState(VK_SPACE) & 0x8000) != 0;
        }

        public bool IsEnterPressed()
        {
            return (getAsyncKeyState(VK_ENTER) & 0x8000) != 0;
        }

        // 获取鼠标坐标
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

        public System.Drawing.Point GetMousePosition()
        {
            System.Drawing.Point point;
            GetCursorPos(out point);
            return point;
        }

        // 监听键盘状态（获取其他键盘按键状态）
        public bool IsKeyPressed(int keyCode)
        {
            return (getAsyncKeyState(keyCode) & 0x8000) != 0;
        }
    }

        // 键盘常量
    public static class KeyCodes
    {
        public const int A = 0x41;
        public const int B = 0x42;
        public const int C = 0x43;
        public const int D = 0x44;
        // 添加其他键的常量...
    }
}
