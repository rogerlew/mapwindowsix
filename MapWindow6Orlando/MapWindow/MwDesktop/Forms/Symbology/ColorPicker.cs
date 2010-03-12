//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created February 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
// 9/21/09: Chris Wilson - aligned controls, removed min/max buttons, added help button, border
//          now fixeddialog, showontaskbar set to false
//********************************************************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MapWindow.Drawing;
using MapWindow.Main;
namespace MapWindow.Forms
{
    /// <summary>
    /// A form for selecting colors
    /// </summary>
    public class ColorPicker : Form
    {
        #region Events

        /// <summary>
        /// Occurs when the Apply button is pressed, implying to set the changes using the current colorbreak.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Private Variables

       

        // Designer variables
        private GroupBox grpPreview;
        private Label lblPreview;
        private Components.mwStatusStrip mwStatusStrip1;
        private ToolStripStatusLabel statusText;
        private ToolStripProgressBar statusProgress;
        private Components.GradientControl grdSlider;
        private MapWindow.Components.DialogButtons dialogButtons1;
        private TableLayoutPanel tableLayoutPanel1;
        private HelpProvider helpProvider1;

        private readonly IColorCategory _rasterCategory;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of this form.
        /// </summary>
        public ColorPicker()
        {
            InitializeComponent();
            _rasterCategory = null; // this will be reset afterwards, but clear it in case we aren't calling with info.
            dialogButtons1.OkClicked += BtnOkClick;
            dialogButtons1.CancelClicked += BtnCancelClick;
            lblPreview.Paint += LblPreviewPaint;
            grdSlider.GradientChanged += GrdSliderGradientChanged;
           
        }

        void GrdSliderGradientChanged(object sender, EventArgs e)
        {
            lblPreview.Invalidate();
        }

        /// <summary>
        /// Constructs a new instance and sets it up for a specific color break
        /// </summary>
        /// <param name="rasterCategory"></param>
        public ColorPicker(IColorCategory rasterCategory)
            : this()
        {
            grdSlider.MinimumColor = rasterCategory.LowColor;
            grdSlider.MaximumColor = rasterCategory.HighColor;
            _rasterCategory = rasterCategory;
            lblPreview.Invalidate();
        }
        /// <summary>
        /// Constructs a new instance and sets up the colors, but won;t allow
        /// </summary>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        public ColorPicker(Color startColor, Color endColor)
            : this()
        {
            grdSlider.MinimumColor = startColor;
            grdSlider.MaximumColor = endColor;
            _rasterCategory = null;
            lblPreview.Invalidate();
        }

       

        #endregion



       

       

        #region Windows Form Designer generated code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPicker));
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.lblPreview = new System.Windows.Forms.Label();
            this.dialogButtons1 = new MapWindow.Components.DialogButtons();
            this.grdSlider = new MapWindow.Components.GradientControl();
            this.mwStatusStrip1 = new MapWindow.Components.mwStatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.grpPreview.SuspendLayout();
            this.mwStatusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPreview
            // 
            this.grpPreview.AccessibleDescription = null;
            this.grpPreview.AccessibleName = null;
            resources.ApplyResources(this.grpPreview, "grpPreview");
            this.grpPreview.BackgroundImage = null;
            this.grpPreview.Controls.Add(this.lblPreview);
            this.helpProvider1.SetHelpKeyword(this.grpPreview, null);
            this.helpProvider1.SetHelpNavigator(this.grpPreview, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("grpPreview.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this.grpPreview, resources.GetString("grpPreview.HelpString"));
            this.grpPreview.Name = "grpPreview";
            this.helpProvider1.SetShowHelp(this.grpPreview, ((bool)(resources.GetObject("grpPreview.ShowHelp"))));
            this.grpPreview.TabStop = false;
            // 
            // lblPreview
            // 
            this.lblPreview.AccessibleDescription = null;
            this.lblPreview.AccessibleName = null;
            resources.ApplyResources(this.lblPreview, "lblPreview");
            this.lblPreview.BackColor = System.Drawing.Color.White;
            this.lblPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPreview.Font = null;
            this.helpProvider1.SetHelpKeyword(this.lblPreview, null);
            this.helpProvider1.SetHelpNavigator(this.lblPreview, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("lblPreview.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this.lblPreview, null);
            this.lblPreview.Name = "lblPreview";
            this.helpProvider1.SetShowHelp(this.lblPreview, ((bool)(resources.GetObject("lblPreview.ShowHelp"))));
            // 
            // dialogButtons1
            // 
            this.dialogButtons1.AccessibleDescription = null;
            this.dialogButtons1.AccessibleName = null;
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.BackgroundImage = null;
            this.dialogButtons1.Font = null;
            this.helpProvider1.SetHelpKeyword(this.dialogButtons1, null);
            this.helpProvider1.SetHelpNavigator(this.dialogButtons1, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("dialogButtons1.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this.dialogButtons1, null);
            this.dialogButtons1.Name = "dialogButtons1";
            this.helpProvider1.SetShowHelp(this.dialogButtons1, ((bool)(resources.GetObject("dialogButtons1.ShowHelp"))));
            // 
            // grdSlider
            // 
            this.grdSlider.AccessibleDescription = null;
            this.grdSlider.AccessibleName = null;
            resources.ApplyResources(this.grdSlider, "grdSlider");
            this.grdSlider.BackgroundImage = null;
            this.grdSlider.EndValue = 0.8F;
            this.grdSlider.Font = null;
            this.helpProvider1.SetHelpKeyword(this.grdSlider, null);
            this.helpProvider1.SetHelpNavigator(this.grdSlider, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("grdSlider.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this.grdSlider, resources.GetString("grdSlider.HelpString"));
            this.grdSlider.MaximumColor = System.Drawing.Color.Blue;
            this.grdSlider.MinimumColor = System.Drawing.Color.Lime;
            this.grdSlider.Name = "grdSlider";
            this.helpProvider1.SetShowHelp(this.grdSlider, ((bool)(resources.GetObject("grdSlider.ShowHelp"))));
            this.grdSlider.SlidersEnabled = true;
            this.grdSlider.StartValue = 0.2F;
            // 
            // mwStatusStrip1
            // 
            this.mwStatusStrip1.AccessibleDescription = null;
            this.mwStatusStrip1.AccessibleName = null;
            resources.ApplyResources(this.mwStatusStrip1, "mwStatusStrip1");
            this.mwStatusStrip1.BackgroundImage = null;
            this.mwStatusStrip1.Font = null;
            this.helpProvider1.SetHelpKeyword(this.mwStatusStrip1, null);
            this.helpProvider1.SetHelpNavigator(this.mwStatusStrip1, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("mwStatusStrip1.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this.mwStatusStrip1, null);
            this.mwStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText,
            this.statusProgress});
            this.mwStatusStrip1.Name = "mwStatusStrip1";
            this.helpProvider1.SetShowHelp(this.mwStatusStrip1, ((bool)(resources.GetObject("mwStatusStrip1.ShowHelp"))));
            // 
            // statusText
            // 
            this.statusText.AccessibleDescription = null;
            this.statusText.AccessibleName = null;
            resources.ApplyResources(this.statusText, "statusText");
            this.statusText.BackgroundImage = null;
            this.statusText.Name = "statusText";
            this.statusText.Spring = true;
            // 
            // statusProgress
            // 
            this.statusProgress.AccessibleDescription = null;
            this.statusProgress.AccessibleName = null;
            resources.ApplyResources(this.statusProgress, "statusProgress");
            this.statusProgress.Name = "statusProgress";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AccessibleDescription = null;
            this.tableLayoutPanel1.AccessibleName = null;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackgroundImage = null;
            this.tableLayoutPanel1.Controls.Add(this.dialogButtons1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.grdSlider, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpPreview, 0, 0);
            this.tableLayoutPanel1.Font = null;
            this.helpProvider1.SetHelpKeyword(this.tableLayoutPanel1, null);
            this.helpProvider1.SetHelpNavigator(this.tableLayoutPanel1, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("tableLayoutPanel1.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this.tableLayoutPanel1, null);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.helpProvider1.SetShowHelp(this.tableLayoutPanel1, ((bool)(resources.GetObject("tableLayoutPanel1.ShowHelp"))));
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = null;
            // 
            // ColorPicker
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.mwStatusStrip1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.helpProvider1.SetHelpKeyword(this, null);
            this.helpProvider1.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this, null);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorPicker";
            this.helpProvider1.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.ShowInTaskbar = false;
            this.grpPreview.ResumeLayout(false);
            this.mwStatusStrip1.ResumeLayout(false);
            this.mwStatusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

       
       

        

        

       
       

        #region Properties

        /// <summary>
        /// Gets or sets the start color for this dialog
        /// </summary>
        [Category("Appearance"),Description("Gets or sets the start color for this dialog")]
        public Color LowColor
        {
            get { return grdSlider.MinimumColor; }
            set 
            {
                grdSlider.MinimumColor = value;
                if (Visible) lblPreview.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the end color for this dialog
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the end color for this dialog")]
        public Color HighColor
        {
            get { return grdSlider.MaximumColor; }
            set 
            {
                grdSlider.MaximumColor = value;
                if (Visible) lblPreview.Invalidate();
            }
        }

        /// <summary>
        /// Gets the IProgressHandler version of the status bar on this form
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get
            {
                return mwStatusStrip1;
            }

        }

        #endregion

        #region Event Handlers

        void BtnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        void BtnOkClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (_rasterCategory != null)
            {
                _rasterCategory.LowColor = grdSlider.MinimumColor;
                _rasterCategory.HighColor = grdSlider.MaximumColor;
            }
            Close();
        }

        void LblPreviewPaint(object sender, PaintEventArgs e)
        {
        
            UpdatePreview(e);
        }

        #endregion


        #region Private Methods

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

        private void UpdatePreview(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if(grdSlider == null || lblPreview == null) return;
            System.Drawing.Drawing2D.LinearGradientBrush br = new System.Drawing.Drawing2D.LinearGradientBrush(lblPreview.ClientRectangle, grdSlider.MinimumColor, grdSlider.MaximumColor, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
            g.FillRectangle(br, e.ClipRectangle);
            br.Dispose();

        }


        #endregion

        private void btnApply_Click(object sender, EventArgs e)
        {
            OnChangesApplied();
        }

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnChangesApplied()
        {
            if (ChangesApplied != null)
            {
                ChangesApplied(this, new EventArgs());
            }
        }

     


    }
}