using System;
using System.Runtime.InteropServices;

namespace NativeApi
{
    public static partial class HWND
    {
        public static readonly IntPtr BOTTOM = new IntPtr(1);
        public static readonly IntPtr NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr TOP = IntPtr.Zero;
        public static readonly IntPtr TOPMOST = new IntPtr(-1);
    }

    /// <summary>
    /// SetWindowPos Flags
    /// </summary>
    public static partial class SWP
    {
        #region Public Fields

        public const uint ASYNCWINDOWPOS = 0x4000;
        public const uint DEFERERASE = 0x2000;
        public const uint DRAWFRAME = FRAMECHANGED;
        /// <summary>The frame changed: send WM_NCCALCSIZE</summary>
        public const uint FRAMECHANGED = 0x0020;
        public const uint HIDEWINDOW = 0x0080;
        public const uint NOACTIVATE = 0x0010;
        public const uint NOCOPYBITS = 0x0100;
        public const uint NOMOVE = 0x0002;
        /// <summary>Don't do owner Z ordering</summary>
        public const uint NOOWNERZORDER = 0x0200;
        /// <summary>Don't send WM_WINDOWPOSCHANGING</summary>
        public const uint NOSENDCHANGING = 0x0400;
        public const uint NOREDRAW = 0x0008;
        public const uint NOREPOSITION = NOOWNERZORDER;
        public const uint NOSIZE = 0x0001;
        public const uint NOZORDER = 0x0004;
        public const uint SHOWWINDOW = 0x0040;

        #endregion
    }

    public static partial class User32
    {
        #region Public Methods

        [DllImport(AssemblyName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);

        [DllImport(AssemblyName)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport(AssemblyName, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport(AssemblyName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport(AssemblyName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport(AssemblyName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        #endregion
    }

    public delegate bool WNDENUMPROC(IntPtr hWnd, IntPtr lParam);
}
