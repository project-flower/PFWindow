namespace PFWindow
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxOperation = new System.Windows.Forms.ComboBox();
            this.comboBoxSizes = new System.Windows.Forms.ComboBox();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonOneMore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxOperation
            // 
            this.comboBoxOperation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOperation.FormattingEnabled = true;
            this.comboBoxOperation.Location = new System.Drawing.Point(12, 12);
            this.comboBoxOperation.Name = "comboBoxOperation";
            this.comboBoxOperation.Size = new System.Drawing.Size(304, 20);
            this.comboBoxOperation.TabIndex = 0;
            this.comboBoxOperation.SelectedIndexChanged += new System.EventHandler(this.comboBoxOperation_SelectedIndexChanged);
            // 
            // comboBoxSizes
            // 
            this.comboBoxSizes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSizes.FormattingEnabled = true;
            this.comboBoxSizes.Location = new System.Drawing.Point(12, 38);
            this.comboBoxSizes.Name = "comboBoxSizes";
            this.comboBoxSizes.Size = new System.Drawing.Size(304, 20);
            this.comboBoxSizes.TabIndex = 1;
            // 
            // buttonSelect
            // 
            this.buttonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelect.Font = new System.Drawing.Font("MS UI Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonSelect.Location = new System.Drawing.Point(322, 12);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(150, 46);
            this.buttonSelect.TabIndex = 2;
            this.buttonSelect.Text = "&Select";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonOneMore
            // 
            this.buttonOneMore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOneMore.Location = new System.Drawing.Point(397, 64);
            this.buttonOneMore.Name = "buttonOneMore";
            this.buttonOneMore.Size = new System.Drawing.Size(75, 23);
            this.buttonOneMore.TabIndex = 3;
            this.buttonOneMore.Text = "&One More";
            this.buttonOneMore.UseVisualStyleBackColor = true;
            this.buttonOneMore.Click += new System.EventHandler(this.buttonOneMore_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 99);
            this.Controls.Add(this.buttonOneMore);
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.comboBoxSizes);
            this.Controls.Add(this.comboBoxOperation);
            this.Name = "FormMain";
            this.Text = "PFWindow";
            this.Shown += new System.EventHandler(this.shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxOperation;
        private System.Windows.Forms.ComboBox comboBoxSizes;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonOneMore;
    }
}

