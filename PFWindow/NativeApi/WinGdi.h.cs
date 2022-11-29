using System;
using System.Runtime.InteropServices;

namespace NativeApi
{
    public static partial class Gdi32
    {
        #region Public Methods

        [DllImport(AssemblyName, SetLastError = true)]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        #endregion
    }

    /// <summary>
    /// Ternary raster operations
    /// </summary>
    public static partial class WinGdi
    {
        #region Public Fields

        /// <summary>dest = source</summary>
        public const uint SRCCOPY = 0x00CC0020;
        /// <summary>dest = source OR dest</summary>
        public const uint SRCPAINT = 0x00EE0086;
        /// <summary>dest = source AND dest</summary>
        public const uint SRCAND = 0x008800C6;
        /// <summary>source XOR dest</summary>
        public const uint SRCINVERT = 0x00660046;
        /// <summary>dest = source AND (NOT dest )</summary>
        public const uint SRCERASE = 0x00440328;
        /// <summary>dest = (NOT source)</summary>
        public const uint NOTSRCCOPY = 0x00330008;
        /// <summary>dest = (NOT src) AND (NOT dest)</summary>
        public const uint NOTSRCERASE = 0x001100A6;
        /// <summary>dest = (source AND pattern)</summary>
        public const uint MERGECOPY = 0x00C000CA;
        /// <summary>dest = (NOT source) OR dest</summary>
        public const uint MERGEPAINT = 0x00BB0226;
        /// <summary>dest = pattern</summary>
        public const uint PATCOPY = 0x00F00021;
        /// <summary>dest = DPSnoo</summary>
        public const uint PATPAINT = 0x00FB0A09;
        /// <summary>dest = pattern XOR dest</summary>
        public const uint PATINVERT = 0x005A0049;
        /// <summary>dest = (NOT dest)</summary>
        public const uint DSTINVERT = 0x00550009;
        /// <summary>dest = BLACK</summary>
        public const uint BLACKNESS = 0x00000042;
        /// <summary>dest = WHITE</summary>
        public const uint WHITENESS = 0x00FF0062;

        #endregion
    }
}
