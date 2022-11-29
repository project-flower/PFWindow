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

        #endregion

        #region Public Methods

        public static void DoCentering(IntPtr handle)
        {
            if (!User32.GetWindowRect(handle, out RECT rect))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            int rectLeft = rect.Left;
            int rectTop = rect.Top;
            int rectWidth = (rect.Right - rectLeft);
            int rectHeight = (rect.Bottom - rectTop);
            var windowRectangle = new Rectangle(rectLeft, rectTop, rectWidth, rectHeight);
            uint area = 0;
            Rectangle screenBounds = Rectangle.Empty;

            foreach (Screen screen in Screen.AllScreens)
            {
                Rectangle bounds = screen.Bounds;
                uint area_ = GetArea(bounds, windowRectangle);

                if (area_ <= area) continue;

                screenBounds = bounds;
                area = area_;
            }

            if (!screenBounds.IsEmpty)
            {
                if (!User32.SetWindowPos(handle, IntPtr.Zero
                    , (screenBounds.Left + screenBounds.Width / 2 - rectWidth / 2)
                    , (screenBounds.Top + screenBounds.Height / 2 - rectHeight / 2)
                    , 0, 0, (SWP.ASYNCWINDOWPOS | SWP.NOSIZE)))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
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

            if (!User32.SetWindowPos(handle, IntPtr.Zero, 0 , 0
                , (size.Width - widthOffset), (size.Height - heightOffset), (SWP.ASYNCWINDOWPOS | SWP.NOMOVE)))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static void GetSize(IntPtr handle, GetWindowSizeCallback callback)
        {
            Size result;

            if (DwmApi.DwmGetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_EXTENDED_FRAME_BOUNDS, out RECT bounds, Marshal.SizeOf(typeof(RECT))) == 0)
            {
                result = new Size((bounds.Right - bounds.Left), (bounds.Bottom - bounds.Top));
            }
            else
            {
                result = Size.Empty;
            }

            callback(result);
        }

        public static void SetTopMost(IntPtr handle, bool enable)
        {
            if (!User32.SetWindowPos(handle
                , (enable ? HWND.TOPMOST : HWND.NOTOPMOST)
                , 0 , 0, 0, 0, (SWP.NOMOVE | SWP.NOSIZE)))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        #endregion

        #region Private Methods

        private static uint GetArea(Rectangle screen, Rectangle window)
        {
            Rectangle intersect = Rectangle.Intersect(screen, window);

            if (intersect.IsEmpty)
            {
                return 0;
            }

            return ((uint)intersect.Width * (uint)intersect.Height);
        }

        #endregion
    }
}
