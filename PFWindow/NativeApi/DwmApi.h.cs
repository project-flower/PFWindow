using System;
using System.Runtime.InteropServices;

namespace NativeApi
{
    public static partial class DwmApi
    {
        #region Public Methods

        [DllImport(AssemblyName)]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out RECT pvAttribute, int cbAttribute);

        #endregion
    }

    /// <summary>
    /// Window attributes
    /// </summary>
    public enum DWMWINDOWATTRIBUTE
    {
        /// <summary>[get] Is non-client rendering enabled/disabled</summary>
        DWMWA_NCRENDERING_ENABLED = 1,
        /// <summary>[set] DWMNCRENDERINGPOLICY - Non-client rendering policy</summary>
        DWMWA_NCRENDERING_POLICY,
        /// <summary>[set] Potentially enable/forcibly disable transitions</summary>
        DWMWA_TRANSITIONS_FORCEDISABLED,
        /// <summary>[set] Allow contents rendered in the non-client area to be visible on the DWM-drawn frame.</summary>
        DWMWA_ALLOW_NCPAINT,
        /// <summary>[get] Bounds of the caption button area in window-relative space.</summary>
        DWMWA_CAPTION_BUTTON_BOUNDS,
        /// <summary>[set] Is non-client content RTL mirrored</summary>
        DWMWA_NONCLIENT_RTL_LAYOUT,
        /// <summary>[set] Force this window to display iconic thumbnails.</summary>
        DWMWA_FORCE_ICONIC_REPRESENTATION,
        /// <summary>[set] Designates how Flip3D will treat the window.</summary>
        DWMWA_FLIP3D_POLICY,
        /// <summary>[get] Gets the extended frame bounds rectangle in screen space</summary>
        DWMWA_EXTENDED_FRAME_BOUNDS,
        /// <summary>[set] Indicates an available bitmap when there is no better thumbnail representation.</summary>
        DWMWA_HAS_ICONIC_BITMAP,
        /// <summary>[set] Don't invoke Peek on the window.</summary>
        DWMWA_DISALLOW_PEEK,
        /// <summary>[set] LivePreview exclusion information</summary>
        DWMWA_EXCLUDED_FROM_PEEK,
        /// <summary>[set] Cloak or uncloak the window</summary>
        DWMWA_CLOAK,
        /// <summary>[get] Gets the cloaked state of the window</summary>
        DWMWA_CLOAKED,
        /// <summary>[set] BOOL, Force this window to freeze the thumbnail without live update</summary>
        DWMWA_FREEZE_REPRESENTATION,
        /// <summary>[set] BOOL, Updates the window only when desktop composition runs for other reasons</summary>
        DWMWA_PASSIVE_UPDATE_MODE,
        DWMWA_LAST
    }
}
