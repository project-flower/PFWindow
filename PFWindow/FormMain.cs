using PFWindow.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PFWindow
{
    public partial class FormMain : Form
    {
        #region Public Classes

        public static class IndexOf
        {
            public const int Centering = 0;
            public const int Resize = 1;
            public const int GetSize = 2;
            public const int TopMost = 3;
            public const int NoTopMost = 4;
        }

        #endregion

        #region Private Fields

        private readonly FormPreview formPreview = new FormPreview();
        private readonly Settings settings;

        #endregion

        #region Public Methods

        public FormMain()
        {
            InitializeComponent();
            MaximumSize = new Size(int.MaxValue, Height);
            MinimumSize = Size;
            comboBoxOperation.SelectedIndex = 0;
            settings = Settings.Default;
            formPreview.Initialize(settings.PreviewShadowColor, settings.PreviewShadowAlpha, settings.PreviewFrameColor);
            formPreview.VisibleChanged += formPreview_VisibleChanged;
        }

        #endregion

        #region Private Methods

        private void formPreview_VisibleChanged(object sender, EventArgs e)
        {
            if (formPreview.IsDisposed)
            {
                return;
            }

            if (!formPreview.Visible && !Visible)
            {
                Show();
            }
        }

        private void GotWindowSize(Size size)
        {
            comboBoxSizes.Text = $"{size.Width}x{size.Height}";
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        private void buttonOneMore_Click(object sender, EventArgs e)
        {
            try
            {
                formPreview.DoOperation();
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception.Message);
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            Operations operation;
            Size size;
            int index = comboBoxOperation.SelectedIndex;

            if (index == IndexOf.Resize)
            {
                operation = Operations.Resize;

                if (comboBoxSizes.SelectedIndex < 0)
                {
                    try
                    {
                        size = TextParser.ToSize(comboBoxSizes.Text);
                    }
                    catch (Exception exception)
                    {
                        ShowErrorMessage(exception.Message);
                        return;
                    }
                }
                else if (comboBoxSizes.SelectedItem is ResizeOperationItem item)
                {
                    size = item.Size;
                }
                else return;
            }
            else
            {
                size = Size.Empty;

                switch (index)
                {
                    case IndexOf.Centering:
                        operation = Operations.Centering;
                        break;

                    case IndexOf.GetSize:
                        operation = Operations.GetSize;
                        formPreview.GotWindowSize = GotWindowSize;
                        break;

                    case IndexOf.TopMost:
                        operation = Operations.TopMost;
                        break;

                    case IndexOf.NoTopMost:
                        operation = Operations.NoTopMost;
                        break;

                    default:
                        return;
                }
            }

            Hide();
            formPreview.ShowPreview(operation, size);
        }

        private void comboBoxOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxSizes.Enabled = (comboBoxOperation.SelectedIndex == IndexOf.Resize);
        }

        private void shown(object sender, EventArgs e)
        {
            comboBoxSizes.BeginUpdate();

            try
            {
                foreach (ResizeOperationItem item in ResizeOperationItem.Load(settings.Sizes))
                {
                    comboBoxSizes.Items.Add(item);
                }
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception.Message);
            }
            finally
            {
                comboBoxSizes.EndUpdate();
            }
        }
    }
}
