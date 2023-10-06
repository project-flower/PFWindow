using PFCentering;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PFWindow
{
    public partial class FormPreview : Form
    {
        #region Private Fields

        private Image baseImage = null;
        private Size newSize = Size.Empty;
        private Operations operation = Operations.None;
        private Color previewFrameColor;
        private Color previewShadowColor;
        private int prevX = int.MinValue;
        private int prevY = int.MinValue;
        private int windowIndex = -1;
        private (IntPtr, Rectangle, int)[] windows = new (IntPtr, Rectangle, int)[0];

        #endregion

        #region Public Properties

        public WindowManager.GetWindowSizeCallback GotWindowSize;

        #endregion

        #region Public Methods

        public FormPreview()
        {
            InitializeComponent();

#if DEBUG
            TopMost = false;
#endif
        }

        public void DoOperation()
        {
            try
            {
                switch (operation)
                {
                    case Operations.Centering:
                        DoCentering();
                        break;

                    case Operations.GetSize:
                        GetWindowSize();
                        break;

                    case Operations.Resize:
                        DoResize();
                        break;

                    case Operations.NoTopMost:
                        SetTopMost(false);
                        break;

                    case Operations.TopMost:
                        SetTopMost(true);
                        break;

                    case Operations.Translucent:
                        SetOpacity(0x80);
                        break;

                    case Operations.Opaque:
                        SetOpacity(0xFF);
                        break;

                    default:
                        return;
                }
            }
            catch
            {
                throw;
            }
        }

        public void Initialize(Color previewShadowColor, byte previewShadowAlpha, Color previewFrameColor)
        {
            this.previewFrameColor = previewFrameColor;
            this.previewShadowColor = Color.FromArgb(previewShadowAlpha, previewShadowColor);
        }

        public void ShowPreview(Operations operation, Size newSize)
        {
            this.operation = operation;
            this.newSize = newSize;
            windows = CaptureEngine.CollectWindows(out Bitmap bitmap, out Rectangle totalScreenSize);
            windowIndex = -1;
            Image image = pictureBox.Image;
            pictureBox.Image = bitmap;
            baseImage = bitmap.Clone() as Image;
            image?.Dispose();

            if (!Visible)
            {
                Show();
            }

            Left = totalScreenSize.Left;
            Top = totalScreenSize.Top;
            Width = totalScreenSize.Width;
            Height = totalScreenSize.Height;
        }

        #endregion

        #region Private Methods

        private void DoCentering()
        {
            if (windowIndex < 0)
            {
                return;
            }

            try
            {
                WindowManager.DoCentering(windows[windowIndex].Item1);
            }
            catch
            {
                throw;
            }
        }

        private void DoResize()
        {
            if ((windowIndex < 0) || (newSize.IsEmpty))
            {
                return;
            }

            try
            {
                WindowManager.DoResize(windows[windowIndex].Item1, newSize);
            }
            catch
            {
                throw;
            }
        }

        private void EmphasizeRectangle(Rectangle rectangle)
        {
            if (baseImage == null)
            {
                return;
            }

            pictureBox.Image.Dispose();
            Bitmap bitmap = baseImage.Clone() as Bitmap;

            if (!rectangle.IsEmpty)
            {
                DrawingEngine.FillHollowRectangle(bitmap, rectangle, previewShadowColor, previewFrameColor);
            }

            pictureBox.Image = bitmap;
        }

        private void GetWindowSize()
        {
            if ((windowIndex < 0) || (GotWindowSize == null))
            {
                return;
            }

            try
            {
                WindowManager.GetSize(windows[windowIndex].Item1, GotWindowSize);
            }
            catch
            {
                throw;
            }
        }

        private Rectangle PointToClient(Rectangle rectangle)
        {
            return new Rectangle(pictureBox.PointToClient(rectangle.Location), rectangle.Size);
        }

        private void SetOpacity(byte value)
        {
            if (windowIndex < 0)
            {
                return;
            }

            try
            {
                WindowManager.SetOpacity(windows[windowIndex].Item1, value);
            }
            catch
            {
                throw;
            }
        }
        
        private void SetTopMost(bool enable)
        {
            if (windowIndex < 0)
            {
                return;
            }

            try
            {
                WindowManager.SetTopMost(windows[windowIndex].Item1, enable);
            }
            catch
            {
                throw;
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        // Designer's Methods

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                windowIndex = -1;
                Close();
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                DoOperation();
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception.Message);
            }

            Close();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.X == prevX) && (e.Y == prevY))
            {
                return;
            }

            prevX = e.X;
            prevY = e.Y;
            int candidate = -1;

            for (int i = 0; i < windows.Length; ++i)
            {
                (IntPtr, Rectangle, int) window = windows[i];
                Rectangle bounds = window.Item2;
                Point point = pictureBox.PointToScreen(e.Location);
                int x = point.X;
                int y = point.Y;

                if ((x < bounds.Left)
                    || (y < bounds.Top)
                    || (x > bounds.Right)
                    || (y > bounds.Bottom)) continue;

                if (candidate >= 0 && (window.Item3 > windows[candidate].Item3))
                {
                    continue;
                }

                candidate = i;
            }

            if ((candidate == -1) && (windowIndex != -1))
            {
                EmphasizeRectangle(Rectangle.Empty);
                windowIndex = -1;
            }
            else if ((candidate != -1) && (candidate != windowIndex))
            {
                EmphasizeRectangle(PointToClient(windows[candidate].Item2));
                windowIndex = candidate;
            }
        }
    }
}
