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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2008 4:20:25 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MapWindow.Drawing;
using MapWindow.Main;
namespace MapWindow.Components
{
    /// <summary>
    /// Creates a new viewer control
    /// </summary>
    [Designer(typeof(ColorBreakViewerDesigner)),
    ToolboxBitmap(typeof(ColorBreakViewer), "UserControl.ico")]
    public class ColorBreakViewer : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs if someone changes the color breaks in use.
        /// </summary>
        public event EventHandler DataChanged;

        /// <summary>
        /// Occurs if the status changes from having an error to not having one.
        /// </summary>
        public event EventHandler StatusUpdated;

        #endregion


        #region Private Variables

        // Drawing related constants and variables
        private int _itemSpacing = 2;
        private int _itemHeight = 18;
      
   
        private IList<IColorCategory> _colorBreaks;
     
        // Coordinated properties 
        private Color _highlightColor = System.Drawing.SystemColors.Highlight;
        private Color _highlightTextColor = System.Drawing.SystemColors.HighlightText;
        private HorizontalAlignment _textAlignment; // for values & legend text

        // required designer variables
        private System.ComponentModel.IContainer components = null;
        private ThreeColumnHeaderPanel pnlHeader;
        private VScrollBar scrContentScrollBar;
        private ColorBreakPanel pnlColors;
        private ColorBreakPanel pnlValues;
        private ColorBreakPanel pnlCaptions;
        private Panel pnlContent;
        private Splitter divContentSplitter;
        private LayerTypes _layerType;
         

        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scrContentScrollBar = new System.Windows.Forms.VScrollBar();
            this.pnlHeader = new MapWindow.Components.ThreeColumnHeaderPanel();
            this.pnlColors = new MapWindow.Components.ColorBreakPanel();
            this.pnlValues = new MapWindow.Components.ColorBreakPanel();
            this.pnlCaptions = new MapWindow.Components.ColorBreakPanel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.divContentSplitter = new System.Windows.Forms.Splitter();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // scrContentScrollBar
            // 
            this.scrContentScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrContentScrollBar.Location = new System.Drawing.Point(337, 0);
            this.scrContentScrollBar.Name = "scrContentScrollBar";
            this.scrContentScrollBar.Size = new System.Drawing.Size(18, 423);
            this.scrContentScrollBar.TabIndex = 12;
            // 
            // pnlHeader
            // 
            this.pnlHeader.AutoSizeHeight = true;
            this.pnlHeader.AutoSizeLeftColumn = false;
            this.pnlHeader.BorderVisible = true;
            this.pnlHeader.CellPadding = 2;
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlHeader.LeftColumnText = "Colors";
            this.pnlHeader.LeftColumnWidth = 61;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.MiddleColumnText = "Values";
            this.pnlHeader.MiddleColumnWidth = 178;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.PaddingColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlHeader.RightColumnText = "Captions";
            this.pnlHeader.RightColumnWidth = 178;
            this.pnlHeader.Size = new System.Drawing.Size(418, 28);
            this.pnlHeader.TabIndex = 7;
            this.pnlHeader.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pnlColors
            // 
            this.pnlColors.ContentStyle = MapWindow.Components.ColorPanelStyle.Colors;
            this.pnlColors.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlColors.ErrorColor = System.Drawing.Color.Salmon;
            this.pnlColors.HasErrors = false;
            this.pnlColors.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.pnlColors.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.pnlColors.HorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.pnlColors.ItemSpacing = 2;
            this.pnlColors.Location = new System.Drawing.Point(0, 28);
            this.pnlColors.Name = "pnlColors";
            this.pnlColors.Size = new System.Drawing.Size(63, 423);
            this.pnlColors.StartIndex = 0;
            this.pnlColors.TabIndex = 13;
            // 
            // pnlValues
            // 
            this.pnlValues.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlValues.ContentStyle = MapWindow.Components.ColorPanelStyle.Colors;
            this.pnlValues.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlValues.ErrorColor = System.Drawing.Color.Salmon;
            this.pnlValues.HasErrors = false;
            this.pnlValues.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.pnlValues.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.pnlValues.HorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.pnlValues.ItemSpacing = 2;
            this.pnlValues.Location = new System.Drawing.Point(0, 0);
            this.pnlValues.Name = "pnlValues";
            this.pnlValues.Size = new System.Drawing.Size(173, 423);
            this.pnlValues.StartIndex = 0;
            this.pnlValues.TabIndex = 14;
            this.pnlValues.StatusUpdated += new System.EventHandler(this.pnlValues_StatusUpdated);
            // 
            // pnlCaptions
            // 
            this.pnlCaptions.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlCaptions.ContentStyle = MapWindow.Components.ColorPanelStyle.Colors;
            this.pnlCaptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCaptions.ErrorColor = System.Drawing.Color.Salmon;
            this.pnlCaptions.HasErrors = false;
            this.pnlCaptions.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.pnlCaptions.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.pnlCaptions.HorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.pnlCaptions.ItemSpacing = 2;
            this.pnlCaptions.Location = new System.Drawing.Point(173, 0);
            this.pnlCaptions.Name = "pnlCaptions";
            this.pnlCaptions.Size = new System.Drawing.Size(182, 423);
            this.pnlCaptions.StartIndex = 0;
            this.pnlCaptions.TabIndex = 15;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.divContentSplitter);
            this.pnlContent.Controls.Add(this.scrContentScrollBar);
            this.pnlContent.Controls.Add(this.pnlCaptions);
            this.pnlContent.Controls.Add(this.pnlValues);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(63, 28);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(355, 423);
            this.pnlContent.TabIndex = 16;
            // 
            // divContentSplitter
            // 
            this.divContentSplitter.Location = new System.Drawing.Point(173, 0);
            this.divContentSplitter.Name = "divContentSplitter";
            this.divContentSplitter.Size = new System.Drawing.Size(4, 423);
            this.divContentSplitter.TabIndex = 16;
            this.divContentSplitter.TabStop = false;
            // 
            // ColorBreakViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlColors);
            this.Controls.Add(this.pnlHeader);
            this.Name = "ColorBreakViewer";
            this.Size = new System.Drawing.Size(418, 451);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        
       

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ColorBreakViewer()
        {
            InitializeComponent();

            _colorBreaks = new List<IColorCategory>();
            base.AutoScaleMode = AutoScaleMode.None;
            pnlColors.ContentStyle = ColorPanelStyle.Colors;
            // The only one that needs to worry about validation is this one
            pnlValues.ContentStyle = ColorPanelStyle.Values;
            pnlCaptions.ContentStyle = ColorPanelStyle.Captions;
            System.Diagnostics.Debug.WriteLine("ColorBreakViewerConstructor");
            scrContentScrollBar.Visible = false; // initially this is not visible.
            pnlValues.Width += scrContentScrollBar.Width / 2; // in the designer it is visible so adjust these
            pnlCaptions.Width += scrContentScrollBar.Width / 2;
            ResetHeader();
        }

        

        void ColorBreakViewer_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show("Delete!");
        }

        /// <summary>
        /// Adds event handlers 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ColorBreakViewer-OnLoad");
            base.OnLoad(e);

            this.Click += new EventHandler(ColorBreakViewer_Click);
            this.Resize += new EventHandler(ColorSchemeViewer_Resize);

            // Scroll
            scrContentScrollBar.Scroll += new ScrollEventHandler(scrContentScrollBar_Scroll);

            // Resize
            pnlColors.Resize += new EventHandler(pnlColors_Resize);
            pnlValues.Resize += new EventHandler(pnlValues_Resize);
            pnlCaptions.Resize += new EventHandler(pnlCaptions_Resize);

            // Validation
            pnlCaptions.EditBoxHidden += new EventHandler(pnlCaptions_EditBoxHidden);
            pnlValues.EditBoxHidden += new EventHandler(pnlValues_EditBoxHidden);
            pnlColors.EditBoxHidden += new EventHandler(pnlColors_EditBoxHidden);

            // Refresh
            pnlCaptions.Refreshed += new EventHandler(pnlCaptions_Refreshed);
            pnlValues.Refreshed += new EventHandler(pnlValues_Refreshed);
            pnlColors.Refreshed += new EventHandler(pnlColors_Refreshed);

            this.KeyPress += new KeyPressEventHandler(ColorBreakViewer_KeyPress);
        }

        /// <summary>
        /// Resets all the sub-pannels
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            pnlCaptions.Invalidate();
            pnlColors.Invalidate();
            pnlHeader.Invalidate();
            pnlValues.Invalidate();
        }
       
       
       
       

       
      
        #endregion

        #region Methods

        /// <summary>
        /// Clears the selection 
        /// </summary>
        public void ClearSelection()
        {
            pnlColors.ClearSelection();
            pnlValues.ClearSelection();
            pnlCaptions.ClearSelection();
        }


         /// <summary>
        /// Obtains the IColorCategory being represented at the specified yPosition on this control.
        /// </summary>
        /// <param name="y">The integer specifying the position with 0 being the top of the control.</param>
        /// <returns>An IColorCategory</returns>
        public IColorCategory GetItemAt(int y)
        {
            int indx = GetIndexAt(y);
            if (indx == -1) return null;
            return _colorBreaks[indx];
        }
       
        /// <summary>
        /// Obtains the zero-based integer index of the color break located at the specified position.
        /// </summary>
        /// <param name="y">The integer coordinate from the top of this ColorBreakViewer control</param>
        /// <returns>an integer index in ColorBreaks, or -1 if the member was not found.</returns>
        public int GetIndexAt(int y)
        {
            if (y < pnlHeader.Height) return -1;
            int index = scrContentScrollBar.Value + Convert.ToInt32(Math.Floor((double)(y - pnlHeader.Height) / _itemHeight));
            if(index > _colorBreaks.Count) return -1;
            return index;
        }

        /// <summary>
        /// Assigns the colorbreaks to this control.
        /// </summary>
        /// <param name="colorBreaks"></param>
        public void IntializeControl(IList<IColorCategory> colorBreaks)
        {
            _colorBreaks = colorBreaks;
            pnlValues.ColorBreaks = colorBreaks;
            pnlColors.ColorBreaks = colorBreaks;
            pnlCaptions.ColorBreaks = colorBreaks;
        }



        
        #endregion

        #region Properties



        
        /// <summary>
        /// Gets the list of colorbreaks currently in this viewer.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal IList<IColorCategory> ColorBreaks
        {
            get { return _colorBreaks; }
            set 
            {
                _colorBreaks = value;
                pnlValues.ColorBreaks = value;
                pnlColors.ColorBreaks = value;
                pnlCaptions.ColorBreaks = value;
            }
        }

        /// <summary>
        /// The left panel, which draws colors.
        /// </summary>
        [Category("Panels"),Description("The left panel, which draws colors."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        internal ColorBreakPanel ColorPanel
        {
            get{ return pnlColors; }
            set { pnlColors = value; }
        }

     
        /// <summary>
        /// Gets or sets the font to use for the normal text of this control.
        /// </summary>
        [Category("Appearance"),Description(" Gets or sets the font to use for the normal text of this control.")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                SizeF ruler = this.CreateGraphics().MeasureString("Test", value);
                _itemHeight = Convert.ToInt32(ruler.Height) + _itemSpacing;
                base.Font = value;
                pnlValues.Font = value;
                pnlColors.Font = value;
                pnlCaptions.Font = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the color of the background of the header bar.
        /// </summary>
        [Category("Header"), Description("Gets or sets the color of the background of the header bar.")]
        public Color HeaderBackgroundColor
        {
            get { return pnlHeader.BackColor; }
            set
            {
                pnlHeader.BackColor = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets whether to draw the black border lines in the header.
        /// </summary>
        [Category("Header"), Description("Gets or sets whether to draw the black border lines in the header.")]
        public bool HeaderBorderVisible
        {
            get { return pnlHeader.BorderVisible; }
            set
            { 
                pnlHeader.BorderVisible = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the font to use in the Caption.
        /// </summary>
        [Category("Header"), Description("Gets or sets the font to use in the Caption.")]
        public Font HeaderFont
        {
            get { return pnlHeader.Font; }
            set
            {
                pnlHeader.Font = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the color for the text in the header bar.
        /// </summary>
        [Category("Header"), Description("Gets or sets the color for the text in the header bar.")]
        public Color HeaderForeColor
        {
            get { return pnlHeader.ForeColor; }
            set
            {
                pnlHeader.ForeColor = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment the text in the header bar.
        /// </summary>
        [Category("Header"), Description("Gets or sets the horizontal alignment the text in the header bar.")]
        public System.Windows.Forms.HorizontalAlignment HeaderTextAlignment
        {
            get { return pnlHeader.TextAlignment; }
            set { pnlHeader.TextAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the item spacing for this control
        /// </summary>
        [Category("Appearance"), Description("Gets or sets how far apart items are positioned in this control.")]
        public int ItemSpacing
        {
            get { return _itemSpacing; }
            set
            {
                _itemSpacing = value;
                pnlCaptions.ItemSpacing = value;
                pnlColors.ItemSpacing = value;
                pnlValues.ItemSpacing = value;
                SizeF ruler = this.CreateGraphics().MeasureString("Test", Font);
                ItemHeight = Convert.ToInt32(ruler.Height) + _itemSpacing * 2;
                this.Refresh();
            }
        }


        /// <summary>
        /// Gets or sets the height to use for each item for all the panels.
        /// Setting this will re-set the individual values on the panels as well.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the height to use for each item for all the panels.  Setting this will re-set the individual values on the panels as well.")]
        public int ItemHeight
        {
            get { return _itemHeight; }
            set 
            {
                _itemHeight = value;
                pnlCaptions.ItemHeight = value;
                pnlColors.ItemHeight = value;
                pnlValues.ItemHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets whether or not there are any errors on this control.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return pnlValues.HasErrors;
            }
        }


        /// <summary>
        /// Gets or sets the bacground color for selected items
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the color that should be used as the background color for selected items.")]
        public Color HighlightColor
        {
            get { return _highlightColor; }
            set
            {
                _highlightColor = value;
                // setting here sets the color for all of the content panels
                pnlColors.HighlightColor = _highlightColor;
                pnlValues.HighlightColor = _highlightColor;
                pnlCaptions.HighlightColor = _highlightColor;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the text color to use in selections.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the text color to use in selections.")]
        public Color HighlightTextColor
        {
            get { return _highlightTextColor; }
            set
            {
                _highlightTextColor = value;
                pnlColors.HighlightTextColor = _highlightTextColor;
                pnlValues.HighlightTextColor = _highlightTextColor;
                pnlCaptions.HighlightTextColor = _highlightTextColor;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets whether or not a text box is currently active on the control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEditing
        {
            get
            {
                if (pnlValues.IsEditing || pnlCaptions.IsEditing) return false;
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the LayerType (for validation purposes)
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LayerTypes LayerType
        {
            get { return _layerType; }
            set 
            { 
                _layerType = value;
                pnlValues.LayerType = _layerType;
                pnlCaptions.LayerType = _layerType;
                pnlColors.LayerType = _layerType;
            }
        }

        /// <summary>
        /// Gets or sets the width of the scroll bar when it becomes visible
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the width of the scroll bar when it becomes visible.")]
        public int ScrollBarWidth
        {
            get
            {
                return scrContentScrollBar.Width;
            }
            set
            {
                scrContentScrollBar.Width = value;
            }
        }


        /// <summary>
        /// Gets or sets the right panel, which shows the legend text.
        /// </summary>
        [Category("Panels"), Description("Gets or sets the right panel, which shows the legend text."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        internal ColorBreakPanel TextPanel
        {
            get { return pnlCaptions; }
            set { pnlCaptions = value; }
        }

        /// <summary>
        /// Gets or sets the text alignment for the value and text columns.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the text alignment for the value and text columns.")]
        public HorizontalAlignment TextAlignment
        {
            get { return _textAlignment; }
            set { _textAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the middle panel, which shows the actual values.
        /// </summary>
        [Category("Panels"), Description("Gets or sets the middle panel, which shows the actual values."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        internal ColorBreakPanel ValuePanel
        {
            get { return pnlValues; }
            set { pnlValues = value; }
        }


        #endregion

        #region Protected Methods

        

        /// <summary>
        /// Updates the scrollbar... not even sure this ever happens
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Not even sure this happens
            UpdateScrollBar();
        }

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
        /// Fires the DataChanged event.
        /// </summary>
        protected void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Handles normal resize, but then sets the splitter to the half way point for
        /// content besides the color panel.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            pnlValues.Width = (this.Width - pnlColors.Width) / 2;
        }

        /// <summary>
        /// Fires the StatusUpdated event
        /// </summary>
        protected virtual void OnStatusUpdate()
        {
            if (StatusUpdated != null) StatusUpdated(this, new EventArgs());
        }

        #endregion


     
        #region Event Handlers

        // Status update
        void pnlValues_StatusUpdated(object sender, EventArgs e)
        {
            OnStatusUpdate();
        }

        

        // Refresh

        void pnlColors_Refreshed(object sender, EventArgs e)
        {
            Refresh();
        }

        void pnlValues_Refreshed(object sender, EventArgs e)
        {
            Refresh();
        }

        void pnlCaptions_Refreshed(object sender, EventArgs e)
        {
            Refresh();
        }

        // Resize
        void pnlColors_Resize(object sender, EventArgs e)
        {
            ResetHeader();
            
        }

        void pnlCaptions_Resize(object sender, EventArgs e)
        {
            ResetHeader();
        }

        void pnlValues_Resize(object sender, EventArgs e)
        {
            ResetHeader();
        }

   
        
        void ColorSchemeViewer_Resize(object sender, EventArgs e)
        {
            pnlCaptions.EditBox.Visible = false;
            pnlColors.EditBox.Visible = false;
            ResetHeader();
            this.Refresh();
        }

        // Hide Edit Box

        void pnlColors_EditBoxHidden(object sender, EventArgs e)
        {
            if (pnlValues.EditBox.Focused == true)
            {
                pnlValues.HideEditBox();
            }
            if (pnlCaptions.EditBox.Focused == true)
            {
                pnlCaptions.HideEditBox();
            }
        }

        void pnlValues_EditBoxHidden(object sender, EventArgs e)
        {
            if (pnlCaptions.EditBox.Focused == true)
            {
                pnlCaptions.HideEditBox();
            }
        }

        void pnlCaptions_EditBoxHidden(object sender, EventArgs e)
        {
            if (pnlValues.EditBox.Focused == true)
            {
                pnlValues.HideEditBox();
            }
        }

       


        void ColorBreakViewer_Click(object sender, EventArgs e)
        {
            if (pnlValues.EditBox.Focused == true)
            {
                pnlValues.HideEditBox();
            }
            if (pnlCaptions.EditBox.Focused == true)
            {
                pnlCaptions.HideEditBox();
            }
        }

       


        void scrContentScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            pnlCaptions.StartIndex = scrContentScrollBar.Value;
            pnlCaptions.HideEditBox();
            pnlColors.StartIndex = scrContentScrollBar.Value;
            pnlValues.StartIndex = scrContentScrollBar.Value;
            pnlValues.HideEditBox();
            Refresh();
        }

     

        #endregion

        #region Private Functions

        private void ResetHeader()
        {
            pnlHeader.SuspendLayout();
            pnlHeader.LeftColumnWidth = pnlColors.Width;
            pnlHeader.MiddleColumnWidth = pnlValues.Width + divContentSplitter.Width / 2;
            pnlHeader.RightColumnWidth = pnlContent.Width - (pnlValues.Width + divContentSplitter.Width);
            pnlHeader.ResumeLayout();
            Refresh();
        }

        private void UpdateScrollBar()
        {
            if (this.Visible == false) return;
            int numOnScreen = Convert.ToInt32(Math.Floor((double)(this.ClientRectangle.Height / _itemHeight)));

            if (_colorBreaks.Count > numOnScreen)
            {
                if (scrContentScrollBar.Visible == false)
                {
                    scrContentScrollBar.Visible = true;
                    pnlValues.Width -= scrContentScrollBar.Width / 2;
                    pnlCaptions.Width -= scrContentScrollBar.Width / 2;
                    ResetHeader();
                }
                scrContentScrollBar.Maximum = _colorBreaks.Count - 1;
                scrContentScrollBar.LargeChange = numOnScreen;
            }
            else
            {
                if (scrContentScrollBar.Visible == true)
                {
                    scrContentScrollBar.Visible = false;
                    pnlValues.Width += scrContentScrollBar.Width / 2;
                    pnlCaptions.Width += scrContentScrollBar.Width / 2;
                    ResetHeader();
                }
            }

        }


        #endregion


    }

    /// <summary>
    /// This just ensures that in designer mode we can access most of the panels to control their properties. 
    /// </summary>
    public class ColorBreakViewerDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        /// <summary>
        /// Initializes the component but also ensures the availability of the internal panels
        /// </summary>
        /// <param name="component">The component to initialize</param>
        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);
            ColorBreakViewer myControl = (ColorBreakViewer)component;
            EnableDesignMode(myControl.ValuePanel, "ValuePanel");
            EnableDesignMode(myControl.ColorPanel, "ColorPanel");
            EnableDesignMode(myControl.TextPanel, "TextPanel");
            //EnableDesignMode(myControl.ContentSplitter, "ContentSplitter");
            // Don't enable Header Panel.  It is much easier to drag and drop
            // if you can grab the header and effectivley grab the whole control.
            // Just expose the relavent header panel properties as properties on the control.
        }

    }

}
