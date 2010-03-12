//********************************************************************************************************
// Product Name: MapWindow.PageSetupForm
// Description:  The MapWindow PageSetupForm used to setup printing layouts
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Aug, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Name                 |    Date    |  Commments
// ---------------------|------------|--------------------------------------------------------------------
// Ted Dunsford         | 8/28/2009  | Cleaned up some code using re-sharper
//********************************************************************************************************

using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace MapWindow.Forms
{
    /// <summary>
    /// A dialog that allows users to modify the size of the layout paper and margins
    /// </summary>
    public class PageSetupForm : Form
    {
           
        private readonly PrinterSettings _printerSettings;
        private Double _left;
        private Double _top;
        private Double _right;
        private Double _bottom;
        private GroupBox _groupBox2;
        private RadioButton _rdbLandscape;
        private RadioButton _rdbPortrait;

        


        ///<summary>
        /// Creates a new instance of the Page Setup Form
        ///</summary>
        ///<param name="settings"></param>
        public PageSetupForm(PrinterSettings settings)
        {
            //This call is required by the Windows Form Designer.
            InitializeComponent();

            //Store the printer settings
            _printerSettings = settings;

            //Gets the list of available paper sizes
            ComboPaperSizes.SuspendLayout();
            PrinterSettings.PaperSizeCollection paperSizes = _printerSettings.PaperSizes;
            foreach (PaperSize ps in paperSizes)
                ComboPaperSizes.Items.Add(ps.PaperName);
            ComboPaperSizes.SelectedItem = settings.DefaultPageSettings.PaperSize.PaperName;
            if (ComboPaperSizes.SelectedIndex == -1) ComboPaperSizes.SelectedIndex = 1;
            ComboPaperSizes.ResumeLayout();

            //Gets the paper orientation
            if (_printerSettings.DefaultPageSettings.Landscape)
                _rdbLandscape.Checked = true;
            else
                _rdbPortrait.Checked = true;

            //Gets the margins
            _left = settings.DefaultPageSettings.Margins.Left / 100.0;
            txtBoxLeft.Text = String.Format("{0:0.00}", _left);
            _top = settings.DefaultPageSettings.Margins.Top / 100.0;
            txtBoxTop.Text = String.Format("{0:0.00}", _top);
            _bottom = settings.DefaultPageSettings.Margins.Bottom / 100.0;
            txtBoxBottom.Text = String.Format("{0:0.00}", _bottom);
            _right = settings.DefaultPageSettings.Margins.Right / 100.0;
            txtBoxRight.Text = String.Format("{0:0.00}", _right);
        }

        private void UpdatePaperSize()
        {
            if (ComboPaperSizes.SelectedIndex == -1)
                ComboPaperSizes.SelectedIndex = 0;
            lblPaperDimension.Text = (_printerSettings.PaperSizes[ComboPaperSizes.SelectedIndex].Width / 100) + "\" x " + (_printerSettings.PaperSizes[ComboPaperSizes.SelectedIndex].Height / 100) + "\"";
        }

        void ComboPaperSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePaperSize();
        }

        void rdbLandscape_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePaperSize();
        }

        void rdbPortrait_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePaperSize();
        }


        void txtBoxRight_Leave(object sender, EventArgs e)
        {
            if (IsValidMargin(txtBoxRight.Text))
                _right = Convert.ToDouble(txtBoxRight.Text);
            txtBoxRight.Text = String.Format("{0:0.00}", _right);
        }

        void txtBoxLeft_Leave(object sender, EventArgs e)
        {
            if (IsValidMargin(txtBoxLeft.Text))
                _left = Convert.ToDouble(txtBoxLeft.Text);
            txtBoxLeft.Text = String.Format("{0:0.00}", _left);
        }

        void txtBoxTop_Leave(object sender, EventArgs e)
        {
            if (IsValidMargin(txtBoxTop.Text))
                _top = Convert.ToDouble(txtBoxTop.Text);
            txtBoxTop.Text = String.Format("{0:0.00}", _top);
        }

        void txtBoxBottom_Leave(object sender, EventArgs e)
        {
            if (IsValidMargin(txtBoxBottom.Text))
                _bottom = Convert.ToDouble(txtBoxBottom.Text);
            txtBoxBottom.Text = String.Format("{0:0.00}", _bottom);
        }

        /// <summary>
        /// Takes a string and returns true if it can be converted to a double
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool IsValidMargin(string input)
        {
            try {Convert.ToDouble(input);}
            catch {return false;}
            if (Convert.ToDouble(input) < 0) return false;
            return true;
        }

        /// <summary>
        /// Sets the printerSettings to the new settings and sets the result to OK
        /// </summary>
        public void OK_Button_Click(object sender, EventArgs e)
        {
            _printerSettings.DefaultPageSettings.PaperSize = _printerSettings.PaperSizes[ComboPaperSizes.SelectedIndex];
            _printerSettings.DefaultPageSettings.Margins.Left = Convert.ToInt32(_left * 100);
            _printerSettings.DefaultPageSettings.Margins.Top = Convert.ToInt32(_top * 100);
            _printerSettings.DefaultPageSettings.Margins.Bottom = Convert.ToInt32(_bottom * 100);
            _printerSettings.DefaultPageSettings.Margins.Right = Convert.ToInt32(_right * 100);
            _printerSettings.DefaultPageSettings.Landscape = _rdbLandscape.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Disgards the settings and sets dialogresult to cancel
        /// </summary>
        void Cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PageSetupForm));
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.OK_Button = new System.Windows.Forms.Button();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBoxBottom = new System.Windows.Forms.TextBox();
            this.txtBoxTop = new System.Windows.Forms.TextBox();
            this.txtBoxLeft = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtBoxRight = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.lblPaperDimension = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.ComboPaperSizes = new System.Windows.Forms.ComboBox();
            this._groupBox2 = new System.Windows.Forms.GroupBox();
            this._rdbLandscape = new System.Windows.Forms.RadioButton();
            this._rdbPortrait = new System.Windows.Forms.RadioButton();
            this.GroupBox1.SuspendLayout();
            this._groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.AccessibleDescription = null;
            this.Cancel_Button.AccessibleName = null;
            resources.ApplyResources(this.Cancel_Button, "Cancel_Button");
            this.Cancel_Button.BackgroundImage = null;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Font = null;
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // OK_Button
            // 
            this.OK_Button.AccessibleDescription = null;
            this.OK_Button.AccessibleName = null;
            resources.ApplyResources(this.OK_Button, "OK_Button");
            this.OK_Button.BackgroundImage = null;
            this.OK_Button.Font = null;
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.AccessibleDescription = null;
            this.GroupBox1.AccessibleName = null;
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.BackgroundImage = null;
            this.GroupBox1.Controls.Add(this.txtBoxBottom);
            this.GroupBox1.Controls.Add(this.txtBoxTop);
            this.GroupBox1.Controls.Add(this.txtBoxLeft);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.txtBoxRight);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Font = null;
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // txtBoxBottom
            // 
            this.txtBoxBottom.AccessibleDescription = null;
            this.txtBoxBottom.AccessibleName = null;
            resources.ApplyResources(this.txtBoxBottom, "txtBoxBottom");
            this.txtBoxBottom.BackgroundImage = null;
            this.txtBoxBottom.Font = null;
            this.txtBoxBottom.Name = "txtBoxBottom";
            this.txtBoxBottom.Leave += new System.EventHandler(this.txtBoxBottom_Leave);
            // 
            // txtBoxTop
            // 
            this.txtBoxTop.AccessibleDescription = null;
            this.txtBoxTop.AccessibleName = null;
            resources.ApplyResources(this.txtBoxTop, "txtBoxTop");
            this.txtBoxTop.BackgroundImage = null;
            this.txtBoxTop.Font = null;
            this.txtBoxTop.Name = "txtBoxTop";
            this.txtBoxTop.Leave += new System.EventHandler(this.txtBoxTop_Leave);
            // 
            // txtBoxLeft
            // 
            this.txtBoxLeft.AccessibleDescription = null;
            this.txtBoxLeft.AccessibleName = null;
            resources.ApplyResources(this.txtBoxLeft, "txtBoxLeft");
            this.txtBoxLeft.BackgroundImage = null;
            this.txtBoxLeft.Font = null;
            this.txtBoxLeft.Name = "txtBoxLeft";
            this.txtBoxLeft.Leave += new System.EventHandler(this.txtBoxLeft_Leave);
            // 
            // Label5
            // 
            this.Label5.AccessibleDescription = null;
            this.Label5.AccessibleName = null;
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.Font = null;
            this.Label5.Name = "Label5";
            // 
            // txtBoxRight
            // 
            this.txtBoxRight.AccessibleDescription = null;
            this.txtBoxRight.AccessibleName = null;
            resources.ApplyResources(this.txtBoxRight, "txtBoxRight");
            this.txtBoxRight.BackgroundImage = null;
            this.txtBoxRight.Font = null;
            this.txtBoxRight.Name = "txtBoxRight";
            this.txtBoxRight.Leave += new System.EventHandler(this.txtBoxRight_Leave);
            // 
            // Label3
            // 
            this.Label3.AccessibleDescription = null;
            this.Label3.AccessibleName = null;
            resources.ApplyResources(this.Label3, "Label3");
            this.Label3.Font = null;
            this.Label3.Name = "Label3";
            // 
            // Label6
            // 
            this.Label6.AccessibleDescription = null;
            this.Label6.AccessibleName = null;
            resources.ApplyResources(this.Label6, "Label6");
            this.Label6.Font = null;
            this.Label6.Name = "Label6";
            // 
            // Label4
            // 
            this.Label4.AccessibleDescription = null;
            this.Label4.AccessibleName = null;
            resources.ApplyResources(this.Label4, "Label4");
            this.Label4.Font = null;
            this.Label4.Name = "Label4";
            // 
            // lblPaperDimension
            // 
            this.lblPaperDimension.AccessibleDescription = null;
            this.lblPaperDimension.AccessibleName = null;
            resources.ApplyResources(this.lblPaperDimension, "lblPaperDimension");
            this.lblPaperDimension.Font = null;
            this.lblPaperDimension.Name = "lblPaperDimension";
            // 
            // Label2
            // 
            this.Label2.AccessibleDescription = null;
            this.Label2.AccessibleName = null;
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.Font = null;
            this.Label2.Name = "Label2";
            // 
            // Label1
            // 
            this.Label1.AccessibleDescription = null;
            this.Label1.AccessibleName = null;
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.Font = null;
            this.Label1.Name = "Label1";
            // 
            // ComboPaperSizes
            // 
            this.ComboPaperSizes.AccessibleDescription = null;
            this.ComboPaperSizes.AccessibleName = null;
            resources.ApplyResources(this.ComboPaperSizes, "ComboPaperSizes");
            this.ComboPaperSizes.BackgroundImage = null;
            this.ComboPaperSizes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboPaperSizes.Font = null;
            this.ComboPaperSizes.FormattingEnabled = true;
            this.ComboPaperSizes.Name = "ComboPaperSizes";
            this.ComboPaperSizes.SelectedIndexChanged += new System.EventHandler(this.ComboPaperSizes_SelectedIndexChanged);
            // 
            // _groupBox2
            // 
            this._groupBox2.AccessibleDescription = null;
            this._groupBox2.AccessibleName = null;
            resources.ApplyResources(this._groupBox2, "_groupBox2");
            this._groupBox2.BackgroundImage = null;
            this._groupBox2.Controls.Add(this._rdbLandscape);
            this._groupBox2.Controls.Add(this._rdbPortrait);
            this._groupBox2.Font = null;
            this._groupBox2.Name = "_groupBox2";
            this._groupBox2.TabStop = false;
            // 
            // _rdbLandscape
            // 
            this._rdbLandscape.AccessibleDescription = null;
            this._rdbLandscape.AccessibleName = null;
            resources.ApplyResources(this._rdbLandscape, "_rdbLandscape");
            this._rdbLandscape.BackgroundImage = null;
            this._rdbLandscape.Font = null;
            this._rdbLandscape.Name = "_rdbLandscape";
            this._rdbLandscape.TabStop = true;
            this._rdbLandscape.UseVisualStyleBackColor = true;
            this._rdbLandscape.CheckedChanged += new System.EventHandler(this.rdbLandscape_CheckedChanged);
            // 
            // _rdbPortrait
            // 
            this._rdbPortrait.AccessibleDescription = null;
            this._rdbPortrait.AccessibleName = null;
            resources.ApplyResources(this._rdbPortrait, "_rdbPortrait");
            this._rdbPortrait.BackgroundImage = null;
            this._rdbPortrait.Font = null;
            this._rdbPortrait.Name = "_rdbPortrait";
            this._rdbPortrait.TabStop = true;
            this._rdbPortrait.UseVisualStyleBackColor = true;
            this._rdbPortrait.CheckedChanged += new System.EventHandler(this.rdbPortrait_CheckedChanged);
            // 
            // PageSetupForm
            // 
            this.AcceptButton = this.OK_Button;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.Cancel_Button;
            this.Controls.Add(this._groupBox2);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.lblPaperDimension);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.ComboPaperSizes);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PageSetupForm";
            this.ShowInTaskbar = false;
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this._groupBox2.ResumeLayout(false);
            this._groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal System.Windows.Forms.Button Cancel_Button;
        internal System.Windows.Forms.Button OK_Button;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.TextBox txtBoxBottom;
        internal System.Windows.Forms.TextBox txtBoxTop;
        internal System.Windows.Forms.TextBox txtBoxLeft;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox txtBoxRight;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label lblPaperDimension;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox ComboPaperSizes;

        #endregion

    }
}
