using PFWindow.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PFWindow
{
    public partial class FormMain : Form
    {
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

            comboBoxOperation.Items.AddRange(
                new object[]
                {
                    new EnumAndStringPair<Operations>(Operations.MoveToTop, "Move to Top")
                    , new EnumAndStringPair<Operations>(Operations.MoveToBottom, "Move to Bottom")
                    , new EnumAndStringPair<Operations>(Operations.MoveToLeft, "Move to Left")
                    , new EnumAndStringPair<Operations>(Operations.MoveToRight, "Move to Right")
                    , new EnumAndStringPair<Operations>(Operations.Centering,"Centering")
                    , new EnumAndStringPair<Operations>(Operations.Resize,"Resize")
                    , new EnumAndStringPair<Operations>(Operations.GetSize,"Get Size")
                    , new EnumAndStringPair<Operations>(Operations.TopMost,"Top-Most")
                    , new EnumAndStringPair<Operations>(Operations.NoTopMost,"No Top-Most")
                    , new EnumAndStringPair<Operations>(Operations.Translucent,"Translucent")
                    , new EnumAndStringPair<Operations>(Operations.Opaque,"Opaque")
                });

            comboBoxOperation.SelectedIndex = 0;
            settings = Settings.Default;
            formPreview.Initialize(settings.PreviewShadowColor, settings.PreviewShadowAlpha, settings.PreviewFrameColor);
            formPreview.GotWindowSize = GotWindowSize;
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
            var selectedItem = comboBoxOperation.SelectedItem as EnumAndStringPair<Operations>;

            if (selectedItem == null) return;

            Operations operation = (Operations)selectedItem.Instance;
            Size size = Size.Empty;

            if (operation == Operations.Resize)
            {
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

            Hide();
            formPreview.ShowPreview(operation, size);
        }

        private void comboBoxOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = comboBoxOperation.SelectedItem as EnumAndStringPair<Operations>;
            comboBoxSizes.Enabled = (selectedItem != null && selectedItem.Instance == Operations.Resize);
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
