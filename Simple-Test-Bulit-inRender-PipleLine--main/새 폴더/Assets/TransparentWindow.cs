using UnityEngine;

using System;
using System.Runtime.InteropServices;

public class Window
{
    public struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    #region Win API
    [DllImport("User32.dll")]
    public static extern IntPtr GetActiveWindow();

    [DllImport("User32.dll", EntryPoint = "FindWindowA")]
    public static extern IntPtr FindWindow(string className, string windowName);

    [DllImport("User32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("User32.dll")]
    public static extern uint GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("User32.dll")]
    public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("User32.dll")]
    public static extern int SetWindowLongPtr(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("User32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("User32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

    [DllImport("User32.dll")]
    public static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("Dwmapi.dll")]
    public static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
    #endregion

    #region Const Value Struct
    public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
    public static readonly IntPtr HWND_TOP = new IntPtr(0);
    public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

    public struct GWL
    {
        public const int STYLE = -16;
        public const int EXSTYLE = -20;
    }

    public struct WS
    {
        public const uint POPUP = 0x80000000;
        public const uint VISIBLE = 0x10000000;
    }

    public struct WS_EX
    {
        public const uint LAYERED = 0x00080000;
        public const uint TRANSPARENT = 0x00000020;
        public const uint TOPMOST = 0x00000008;
    }

    public struct SWP
    {
        public const uint NOMOVE = 0x0002;
        public const uint NOSIZE = 0x0001;
        public const uint NOOWNERZORDER = 0x0200;
        public const uint SHOWWINDOW = 0x0040;
        public const uint NOACTIVATE = 0x0010;
        public const uint TOPMOST = NOMOVE | NOSIZE | NOOWNERZORDER;
    }

    public struct LWA
    {
        public const uint ALPHA = 0x00000002;
    }
    #endregion
}

public class TransparentWindow : MonoBehaviour
{
    protected IntPtr hWnd;
    protected uint oldWindowLong;

    protected void Awake()
    {
        hWnd = Window.GetActiveWindow();

        oldWindowLong = Window.GetWindowLong(hWnd, Window.GWL.EXSTYLE);
        // oldWindowLong = oldWindowLong | Window.WS_EX.LAYERED | Window.WS_EX.TRANSPARENT;
        oldWindowLong = oldWindowLong | Window.WS_EX.LAYERED;
        Window.SetWindowLong(hWnd, Window.GWL.EXSTYLE, oldWindowLong);
        Window.SetWindowPos(hWnd, Window.HWND_TOPMOST, 0, 0, 0, 0, Window.SWP.TOPMOST);

        Window.MARGINS margins = new Window.MARGINS { cxLeftWidth = -1 };
        Window.DwmExtendFrameIntoClientArea(hWnd, ref margins);
    }
}