using System.Drawing;
using System.Windows.Forms;

namespace PFWindow
{
    public static class DrawingEngine
    {
        #region Public Methods

        public static void FillHollowRectangle(Bitmap bitmap, Rectangle rectangle, Color shadowColor, Color frameColor)
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (var brush = new SolidBrush(shadowColor))
                {
                    int outerRight = bitmap.Width;
                    int innerTop = rectangle.Top;
                    int innerHeight = rectangle.Height;

                    var rectangles = new Rectangle[]
                    {
                        new Rectangle(0, 0, outerRight, innerTop),
                        new Rectangle(0, innerTop, rectangle.Left, innerHeight),
                        new Rectangle(rectangle.Right, innerTop, outerRight, innerHeight),
                        new Rectangle(0, rectangle.Bottom, outerRight, bitmap.Height)
                    };

                    graphics.FillRectangles(brush, rectangles);
                }

                using (var pen = new Pen(frameColor))
                {
                    graphics.DrawRectangle(pen, rectangle);
                }
            }
        }

        public static void FillReversibleRectangle(Rectangle rectangle, Color color)
        {
            ControlPaint.FillReversibleRectangle(rectangle, color);
        }

        #endregion
    }
}
