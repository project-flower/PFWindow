using NativeApi;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PFCentering
{
    public static class WindowManager
    {
        #region Public Classes

        public delegate void GetWindowSizeCallback(Size size);
        public enum MoveDirection { Top, Bottom, Left, Right };

        #endregion

        #region Public Methods

        public static void DoCentering(IntPtr handle)
        {
            Rectangle screenBounds;
            Rectangle windowRectangle;

            try
            {
                screenBounds = GetScreenBounds(handle, out windowRectangle);
            }
            catch
            {
                throw;
            }

            Rectangle logicalWindowRectangle = GetLogicalWindowRectangle(handle);

            if (!screenBounds.IsEmpty)
            {
                try
                {
                    DoMove(
                        handle
                        , ((screenBounds.Left + screenBounds.Width / 2 - logicalWindowRectangle.Width / 2) - (logicalWindowRectangle.Left - windowRectangle.Left))
                        , ((screenBounds.Top + screenBounds.Height / 2 - logicalWindowRectangle.Height / 2) - (logicalWindowRectangle.Top - windowRectangle.Top)));
                }
                catch
                {
                    throw;
                }
            }
        }

        public static void DoMove(IntPtr handle, MoveDirection direction)
        {
            Rectangle screenBounds;
            Rectangle windowRectangle;

            try
            {
                screenBounds = GetScreenBounds(handle, out windowRectangle);
            }
            catch
            {
                throw;
            }

            if (screenBounds.IsEmpty) return;

            Rectangle logicalWindowRectangle = GetLogicalWindowRectangle(handle);
            int x = windowRectangle.Left;
            int y = windowRectangle.Top;

            switch (direction)
            {
                case MoveDirection.Bottom:
                    y = (screenBounds.Bottom - windowRectangle.Height - (logicalWindowRectangle.Bottom - windowRectangle.Bottom));
                    break;
                case MoveDirection.Left:
                    x = (screenBounds.Left - (logicalWindowRectangle.Left - windowRectangle.Left));
                    break;
                case MoveDirection.Right:
                    x = (screenBounds.Right - windowRectangle.Width - (logicalWindowRectangle.Right - windowRectangle.Right));
                    break;
                case MoveDirection.Top:
                    y = (screenBounds.Top - (logicalWindowRectangle.Top - windowRectangle.Top));
                    break;
            }

            try
            {
                DoMove(handle, x, y);
            }
            catch
            {
                throw;
            }
        }

        public static void DoResize(IntPtr handle, Size size)
        {
            if (!User32.GetWindowRect(handle, out RECT systemSize))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            if (DwmApi.DwmGetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_EXTENDED_FRAME_BOUNDS, out RECT screenSize, Marshal.SizeOf(typeof(RECT))) != 0)
            {
                screenSize = systemSize;
            }

            int widthOffset = (screenSize.Right - screenSize.Left - (systemSize.Right - systemSize.Left));
            int heightOffset = (screenSize.Bottom - screenSize.Top - (systemSize.Bottom - systemSize.Top));

            if (!User32.SetWindowPos(handle, IntPtr.Zero, 0, 0
                , (size.Width - widthOffset), (size.Height - heightOffset), (SWP.ASYNCWINDOWPOS | SWP.NOMOVE)))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static void GetSize(IntPtr handle, GetWindowSizeCallback callback)
        {
            callback(GetLogicalWindowRectangle(handle).Size);
        }

        public static void SetOpacity(IntPtr handle, byte value)
        {
            IntPtr style = User32.GetWindowLongPtr(handle, GWL.EXSTYLE);

            if (style == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            if (((int)style & WS.EX_LAYERED) == 0)
            {
                if (IntPtr.Size == 8)
                {
                    style = new IntPtr(style.ToInt64() | WS.EX_LAYERED);
                }
                else
                {
                    style = new IntPtr(style.ToInt32() | WS.EX_LAYERED);
                }

                if (User32.SetWindowLongPtr(handle, GWL.EXSTYLE, style) == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            if (!User32.SetLayeredWindowAttributes(handle, 0, value, LWA.ALPHA))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static void SetTopMost(IntPtr handle, bool enable)
        {
            if (!User32.SetWindowPos(handle
                , (enable ? HWND.TOPMOST : HWND.NOTOPMOST)
                , 0, 0, 0, 0, (SWP.NOMOVE | SWP.NOSIZE)))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        #endregion

        #region Private Methods

        private static void DoMove(IntPtr handle, int x, int y)
        {
            if (!User32.SetWindowPos(handle, IntPtr.Zero, x, y, 0, 0, (SWP.ASYNCWINDOWPOS | SWP.NOSIZE)))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private static uint GetArea(Rectangle screen, Rectangle window)
        {
            Rectangle intersect = Rectangle.Intersect(screen, window);

            if (intersect.IsEmpty)
            {
                return 0;
            }

            return ((uint)intersect.Width * (uint)intersect.Height);
        }

        /// <summary>
        /// ウィンドウの論理領域を取得します。
        /// <para>
        /// Windows 10 等では実際に描画されないボーダーを含めた領域を取得します。
        /// </para>
        /// </summary>
        /// <param name="handle">ウィンドウのハンドル。</param>
        /// <returns>ウィンドウの論理領域。</returns>
        private static Rectangle GetLogicalWindowRectangle(IntPtr handle)
        {
            if (DwmApi.DwmGetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_EXTENDED_FRAME_BOUNDS, out RECT rect, Marshal.SizeOf(typeof(RECT))) != 0)
            {
                return Rectangle.Empty;
            }

            return rect.ToRectangle();
        }

        private static Rectangle GetScreenBounds(IntPtr windowHandle, out Rectangle windowRectangle)
        {
            if (!User32.GetWindowRect(windowHandle, out RECT rect))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            windowRectangle = rect.ToRectangle();
            uint area = 0;
            Rectangle result = Rectangle.Empty;

            foreach (Screen screen in Screen.AllScreens)
            {
                Rectangle bounds = screen.Bounds;
                uint area_ = GetArea(bounds, windowRectangle);

                if (area_ <= area) continue;

                result = bounds;
                area = area_;
            }

            return result;
        }

        private static Rectangle ToRectangle(this RECT rect)
        {
            return new Rectangle(rect.Left, rect.Top, (rect.Right - rect.Left), (rect.Bottom - rect.Top));
        }

        #endregion
    }
}
