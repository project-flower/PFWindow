using System.Runtime.InteropServices;

namespace NativeApi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        #region Public Fields

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        #endregion
    }
}
