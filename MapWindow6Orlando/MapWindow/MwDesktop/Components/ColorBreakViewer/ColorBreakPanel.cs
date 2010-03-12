//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  Low level interfaces that allow separate components to use objects that are defined
//               in completely independant libraries so long as they satisfy the basic interface requirements.
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/2/2008 6:29:44 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//using MapWindow.PluginInterfaces;
using MapWindow.Main;
using MapWindow.Drawing;
namespace MapWindow.Components
{
    /// <summary>
    /// This component allows customization of how log messages are sent
    /// </summary>
    internal partial class ColorBreakPanel : Panel
    {
        #region Events

        /// <summary>
        /// Occurs if someone changes the color breaks in use.
        /// </summary>
        public event EventHandler DataChanged;

        /// <summary>
        /// Occurs when this entity was refreshed by internal actions
        /// </summary>
        public event EventHandler Refreshed;

        /// <summary>
        /// This is used to coordinate when the EditBox from other panels should be hidden.
        /// </summary>
        public event EventHandler EditBoxHidden;

        /// <summary>
        /// Occurs when the selection from this panel has been cleared
        /// </summary>
        public event EventHandler SelectionCleared;

        /// <summary>
        /// Occurs when the has-error condition for the whole pannel has changed.
        /// </summary>
        public event EventHandler StatusUpdated;

        #endregion

        #region Private Variables

        private IList<IColorCategory> _colorBreaks;
        private Color _highlightColor = SystemColors.Highlight;
        private Color _highlightTextColor = SystemColors.HighlightText;
        private Color _errorColor = Color.Salmon;
        private SolidBrush _highlightBrush;
        private SolidBrush _highlightTextBrush;
        private SolidBrush _backBrush;
        private SolidBrush _foreBrush;
        private SolidBrush _errorBrush;
        private ColorPanelStyle _contentStyle;
        private int _startIndex;
        private int _itemSpacing = 2;
        private HorizontalAlignment _horizontalAlignment;
        private int _itemHeight = -1; // use font size and item spacing to estimate this.
        private TextBox txtEditBox;
        private bool _textBoxEdited;
        private int _editIndex = -1; // The index in ColorBreaks of the member being currently edited
        private bool _isEditing;
        private LayerTypes _layerType;
        private ToolTip ttHelp;
        
        private int _lastMouseIndex;
        private Point _lastMousePosition;
        private bool _lastIndexTipShown;
        private Point _lastTickPosition;

        private bool _hasEditError;
        private bool _hasErrors;
        private Timer tmrTip;
       
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ColorBreakPanel
        /// </summary>
        public ColorBreakPanel()
        {
            base.AutoSize = false;
            ttHelp = new ToolTip();
            _highlightBrush = new SolidBrush(_highlightColor);
            _highlightTextBrush = new SolidBrush(_highlightTextColor);
            _errorBrush = new SolidBrush(_errorColor);
            _backBrush = new SolidBrush(BackColor);
            _foreBrush = new SolidBrush(ForeColor);
            _colorBreaks = new List<IColorCategory>();
            txtEditBox = new TextBox();
            txtEditBox.Validating += new CancelEventHandler(txtEditBox_Validating);
            txtEditBox.TextChanged += new EventHandler(txtEditBox_TextChanged);
            txtEditBox.Parent = this;
            txtEditBox.Visible = false;
            this.MouseDown += new MouseEventHandler(ColorBreakPanel_MouseDown);
            this.MouseMove += new MouseEventHandler(ColorBreakPanel_MouseMove);
            this.MouseLeave += new EventHandler(ColorBreakPanel_MouseLeave);
            tmrTip = new Timer();
            tmrTip.Tick += new EventHandler(tmrTip_Tick);
            _lastIndexTipShown = true;

        }

        void ColorBreakPanel_MouseLeave(object sender, EventArgs e)
        {
            tmrTip.Stop();
            _lastIndexTipShown = true;
            ttHelp.Hide(this);
        }

        void tmrTip_Tick(object sender, EventArgs e)
        {
            if (_lastMousePosition == _lastTickPosition)
            {
                if (_lastMouseIndex != -1)
                {
                    if (_contentStyle == ColorPanelStyle.Values)
                    {
                        ttHelp.Show(_colorBreaks[_lastMouseIndex].Status, this, _lastMousePosition.X, _lastMousePosition.Y + 20);
                    }
                    else if (_contentStyle == ColorPanelStyle.Captions)
                    {
                        ttHelp.Show(MessageStrings.ColorBreakCaptionHelp, this, _lastMousePosition.X, _lastMousePosition.Y + 20);
                    }
                    else
                    {
                        ttHelp.Show(MessageStrings.ColorBreakColorHelp, this, _lastMousePosition.X, _lastMousePosition.Y + 20);
                    }
                        _lastIndexTipShown = true;
                    tmrTip.Stop();
                    return;
                }
            }
            _lastTickPosition = _lastMousePosition;
           
        }

        void ColorBreakPanel_MouseMove(object sender, MouseEventArgs e)
        {

            int index = GetIndexAt(e.Y);
            if (index != _lastMouseIndex)
            {
                if(_lastIndexTipShown == true) tmrTip.Start();
                _lastIndexTipShown = false;
                _lastMouseIndex = index;
                _lastMousePosition = e.Location;
                return;
            }
            if (e.Location != _lastMousePosition)
            {
                ttHelp.Hide(this);
                _lastMousePosition = e.Location;
            }
           
           
            
        }

        void ShowTip()
        {

        }

        /// <summary>
        /// Boolean, true if there is a non-numeric value field entered for a Raster
        /// </summary>
        public bool HasErrors
        {
            get { return _hasErrors; }
            set { _hasErrors = value; }
        }
       

       

        
        /// <summary>
        /// Captures the KeyDown event
        /// </summary>
        /// <param name="e">The key to capture.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                for (int index = _colorBreaks.Count - 1; index >= 0; index--)
                {
                    if (_colorBreaks[index].IsSelected) _colorBreaks.RemoveAt(index);
                }
                OnDataChanged();
                OnRefresh();
            }
            
        }

        

        void txtEditBox_TextChanged(object sender, EventArgs e)
        {
            _textBoxEdited = true;
            if (_contentStyle == ColorPanelStyle.Values && _layerType == LayerTypes.Raster)
            {
                CheckNumericValues();
                UpdateStatus();
            }
        }

        /// <summary>
        /// Fires the StatusUpdated event
        /// </summary>
        protected virtual void OnStatusUpdate()
        {
            if (StatusUpdated != null)
            {
                StatusUpdated(this, new EventArgs());
            }
        }

        /// <summary>
        /// This tests numeric values for rasters as the text is actively changed.
        /// </summary>
        void UpdateStatus()
        {


            // Don't worry about errors in the other panels
            if(_contentStyle != ColorPanelStyle.Values) return;

            if (txtEditBox.Focused)
            {
                // The panel being 
                if (_hasEditError)
                {
                    if (_hasErrors == false)
                    {
                        _hasErrors = true;
                        
                        OnStatusUpdate();
                    }
                    return;
                }
                  
            }

            
            if (_hasErrors)
            {
                _hasErrors = false;
                OnStatusUpdate();

            }
            
            
        }

        // Tests to see if the values are numeric and shows red if 
        // there is an error, updates status, tooltips but doesn't change break values
        void CheckNumericValues()
        {
            string val1, val2;
            if (_editIndex == -1) return;
            IColorCategory brk = _colorBreaks[_editIndex];

        
            if (txtEditBox.Text.Length == 0)
            {
                brk.Status = MessageStrings.ColorBreakValue;
                txtEditBox.BackColor = Color.White;
                _hasEditError = false;
                UpdateStatus();
                return;
            }

            int i = txtEditBox.Text.IndexOf(" - ");
            int j = txtEditBox.Text.IndexOf("-", 1);

            if (i > 0)
            {

                val1 = txtEditBox.Text.Substring(0, i).Trim();
                val2 = txtEditBox.Text.Substring(i + 3).Trim();
                if (Global.IsDouble(val1) == false || Global.IsDouble(val2) == false)
                {
                    brk.Status = MessageStrings.ColorBreakNotNumeric;
                    txtEditBox.BackColor = _errorColor;
                    ttHelp.SetToolTip(txtEditBox, MessageStrings.ColorBreakNotNumeric);
                    _hasEditError = true;
                    UpdateStatus();
                    return;
                }
                brk.Status = MessageStrings.ColorBreakValue;
                txtEditBox.BackColor = Color.White;
                _hasEditError = false;
                UpdateStatus();
              
            }
            if (j > 0)
            {
                val1 = txtEditBox.Text.Substring(0, j).Trim();
                val2 = txtEditBox.Text.Substring(j + 1).Trim();
                if (Global.IsDouble(val1) == false || Global.IsDouble(val2) == false)
                {
                    brk.Status = MessageStrings.ColorBreakNotNumeric;
                    txtEditBox.BackColor = _errorColor;
                    ttHelp.SetToolTip(txtEditBox, MessageStrings.ColorBreakNotNumeric);
                    _hasEditError = true;
                    UpdateStatus();
                    return;
                }
                brk.Status = MessageStrings.ColorBreakValue;
                txtEditBox.BackColor = Color.White;
                _hasEditError = false;
                UpdateStatus();

                
            }
            else
            {
                if (Global.IsDouble(txtEditBox.Text.Trim()))
                {
                    brk.Status = MessageStrings.ColorBreakValue;
                    txtEditBox.BackColor = Color.White;
                    _hasEditError = false;
                    UpdateStatus();
                }
                else
                {
                    brk.Status = MessageStrings.ColorBreakNotNumeric;
                    txtEditBox.BackColor = _errorColor;
                    ttHelp.SetToolTip(txtEditBox, MessageStrings.ColorBreakNotNumeric);
                    _hasEditError = true;
                    UpdateStatus();
                    return;
                }
            }

           
        }

        void txtEditBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateTextbox();
        }

        private void ValidateTextbox()
        {
            if (_textBoxEdited == true)
            {
                UpdateValues();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the selected breaks
        /// </summary>
        public virtual void ClearSelection()
        {
            OnClearSelection();
        }


        /// <summary>
        /// Hides the edit box, forcing validation if necessary
        /// </summary>
        public void HideEditBox()
        {
            OnHideEditBox();      
        }

        /// <summary>
        /// Obtains the IColorCategory being represented at the specified y Position on this control.
        /// </summary>
        /// <param name="y">The integer specifying the position with 0 being the top of the control.</param>
        /// <returns>An IColorCategory</returns>
        public IColorCategory GetItemAt(int y)
        {
            int index = GetIndexAt(y);
            if(index == -1)return null;
            return _colorBreaks[index];
        }

        /// <summary>
        /// Obtains the index of the IColorCategory being represeted at the specified y position on this control.
        /// </summary>
        /// <param name="y">The integer specifying the position with 0 being the top of the control</param>
        /// <returns>An IColorCategory</returns>
        public int GetIndexAt(int y)
        {
            if (y < 0) return -1;
            int index = _startIndex + Convert.ToInt32(Math.Floor((double)y / _itemHeight));
            if (index >= _colorBreaks.Count) return -1;
            return index;
        }

        #endregion

       


        #region Properties

        /// <summary>
        /// Gets or sets the background color for this panel
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the background color for this panel")]
        public override Color BackColor
        {
            get
            {

                return base.BackColor;
            }
            set
            {

                base.BackColor = value;
                _backBrush = new SolidBrush(value);
            }
        }

        /// <summary>
        /// Gets or sets the highlight color to use to mark errors.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the background for Non-numeric Raster values.")]
        public Color ErrorColor
        {
            get { return _errorColor; }
            set { _errorColor = value; }
        }

        /// <summary>
        /// Gets or sets the foreground color for this panel
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the foreground color for this panel")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                _foreBrush = new SolidBrush(value);
            }
        }



       


        /// <summary>
        /// A list of colorbreaks that are the inventory of what should be drawn on this panel.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<IColorCategory> ColorBreaks
        {
            get
            {
                return _colorBreaks; 
            }
            set
            {
                _colorBreaks = value;
            }
        }

        /// <summary>
        /// Gets or sets which characteristic of the ColorCategory will be displayed in this panel.
        /// </summary>
        [Category("Behavior"),Description("Gets or sets which characteristic of the ColorCategory will be displayed in this panel.")]
        public ColorPanelStyle ContentStyle
        {
            get { return _contentStyle; }
            set
            {
                _contentStyle = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the EditBox in case external events need to control this.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBox EditBox
        {
            get { return txtEditBox; }
            set { txtEditBox = value; }
        }

        /// <summary>
        /// Boolean, true if the edit box is visible
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEditing
        {
            get { return _isEditing; }
        }

        /// <summary>
        /// Gets or sets an integer value showing how much white space to attempt to surround each item with on the top and bottom.
        /// </summary>
        [Category("Appearance"),Description("Gets or sets an integer value showing how much white space to attempt to surround each item with on the top and bottom, or the justification side.")]
        public int ItemSpacing
        {
            get { return _itemSpacing; }
            set 
            { 
                _itemSpacing = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets the height of each item.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ItemHeight
        {
            get
            {
                if(_itemHeight > 0) return _itemHeight;
                // This can be set to override normal behavior... otherwise calculate it
                return Convert.ToInt32(this.CreateGraphics().MeasureString("Test", Font).Height) + ItemSpacing;
            }
            set
            {
                _itemHeight = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the color to use in the background when selecting a row.
        /// </summary>
        [Category("Appearance"),Description("Gets or sets the color to use in the background when selecting a row.")]
        public Color HighlightColor
        {
            get { return _highlightColor; }
            set
            {
                _highlightColor = value;
                _highlightBrush = new SolidBrush(value);
                Refresh();

            }
        }

        /// <summary>
        /// Gets or sets the color to use for the text in selected a rows.
        /// </summary>
        [Category("Appearance"),Description("Gets or sets the color to use for the text in selected a rows.")]
        public Color HighlightTextColor
        {
            get { return _highlightTextColor; }
            set
            { 
                _highlightTextColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets how the text or items should be positioned inside this panel.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets how the text or items should be positioned inside this panel.")]
        public HorizontalAlignment HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set { _horizontalAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the layer type (for validation purposes.)
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LayerTypes LayerType
        {
            get { return _layerType; }
            set { _layerType = value; }
        }


        /// <summary>
        /// Gets or sets the index of the first item to be displayed in this panel.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the index of the first item to be displayed in this panel.")]
        public int StartIndex
        {
            get { return _startIndex; }
            set { _startIndex = value; }
        }


        #region HiddenProperties

       
        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool AutoScroll
        {
            get
            {
                return base.AutoScroll;
            }
            set
            {
                base.AutoScroll = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new System.Drawing.Point AutoScrollOffset
        {
            get
            {
                return base.AutoScrollOffset;
            }
            set
            {
                base.AutoScrollOffset = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool AllowDrop
        {
            get
            {
                return base.AllowDrop;
            }
            set
            {
                base.AllowDrop = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new AutoSizeMode AutoSizeMode
        {
            get
            {
                return base.AutoSizeMode;
            }
            set
            {
                base.AutoSizeMode = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new BindingContext BindingContext
        {
            get
            {
                return base.BindingContext;
            }
            set
            {
                base.BindingContext = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new System.Windows.Forms.Layout.LayoutEngine LayoutEngine
        {
            get
            {
                return base.LayoutEngine;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MaximumSize
        {
            get
            {
                return base.MaximumSize;
            }
            set
            {
                base.MaximumSize = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MinimumSize
        {
            get
            {
                return base.MinimumSize;
            }
            set
            {
                base.MinimumSize = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
            }
        }

        /// <summary>
        /// Hidden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        #endregion

        #endregion

        

        #region Event Handlers

        // Receives the mouse down and determines whether to edit
        void ColorBreakPanel_MouseDown(object sender, MouseEventArgs e)
        {
            IColorCategory brk;
            int index;

            if (txtEditBox.Focused && e.Button == MouseButtons.Left)
            {
                txtEditBox_Validating(sender, new CancelEventArgs());
            }

            index = GetIndexAt(e.Y);

            if (index == -1 || index >= _colorBreaks.Count)
            {
                OnClearSelection();
                OnHideEditBox();
                OnRefresh();
                return;
            }

            if (e.Button == MouseButtons.Right && _contentStyle == ColorPanelStyle.Colors)
            {
                brk = _colorBreaks[index].Copy();
                MapWindow.Forms.ColorPicker frm = new MapWindow.Forms.ColorPicker(brk);
                if (frm.ShowDialog() != DialogResult.OK) return;
                _colorBreaks[index] = brk;
                OnDataChanged();
                OnRefresh();
            }

            

            if (e.Button == MouseButtons.Left)
            {
              
                brk = _colorBreaks[index];

                // Multi-select

                if ((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    _colorBreaks[index].IsSelected = !_colorBreaks[index].IsSelected;
                    OnRefresh();
                    return;
                }

                // If it isn't selected, don't try to change it yet.
                if (brk.IsSelected == false)
                {
                    OnHideEditBox();
                    OnClearSelection();
                    brk.IsSelected = true;
                    OnRefresh();
                    return;
                }
                
                

                // After we select it, then we can change it.

                if (_contentStyle == ColorPanelStyle.Colors)
                {
                    OnHideEditBox(); // hide the edit box from any other panels
                    ColorDialog dlg = new ColorDialog();
                    dlg.AllowFullOpen = true;
                    // to do frmMain.LoadCustomColors(dlg)
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        // to do frmMain.SaveCustomColors(dlg)

                        brk.LowColor = dlg.Color;
                        brk.HighColor = brk.LowColor;
                        OnDataChanged();
                        OnRefresh();
                    }
                    return;
                }

                if (_contentStyle == ColorPanelStyle.Values)
                {
                    OnHideEditBox(); // close the edit box of the oposing panel if it is open
                    txtEditBox.Text = brk.ToString();
                   
                }

                if (_contentStyle == ColorPanelStyle.Captions)
                {
                    OnHideEditBox(); // close the edit box of the opposing panel if it is open
                    txtEditBox.Text = brk.LegendText;
                }
            
                txtEditBox.SelectAll();
                txtEditBox.Location = new Point(0, (index - _startIndex) * _itemHeight);
                txtEditBox.Size = new Size(ClientRectangle.Width, _itemHeight);
                txtEditBox.Visible = true;
                txtEditBox.BringToFront();
                txtEditBox.Focus();
                _editIndex = index;
                OnRefresh();
            }
            

        }


        #endregion

        #region Protected Methods

        /// <summary>
        /// Disposes brushes
        /// </summary>
        /// <param name="disposing">Bool disposing</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_highlightBrush != null) _highlightBrush.Dispose();
            if (_highlightTextBrush != null) _highlightTextBrush.Dispose();
            if (_backBrush != null) _backBrush.Dispose();
            if (_foreBrush != null) _foreBrush.Dispose();
        }

        /// <summary>
        /// Fires the SelectionCleared event (and sets all members to the unselected state)
        /// </summary>
        protected virtual void OnClearSelection()
        {
            foreach (IColorCategory cb in _colorBreaks)
            {
                cb.IsSelected = false;
            }
            if (SelectionCleared != null)
            {
                SelectionCleared(this, new EventArgs());
            }
        }

        /// <summary>
        /// Fires the DataChanged event
        /// </summary>
        protected virtual void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Controls the drawing of each item.  Overridding this places you directly at the stage where
        /// the backbuffer is being drawn to and e.ClipRectangle is the item bounds.
        /// </summary>
        /// <param name="item">The Icolorbreak to draw</param>
        /// <param name="e">A PaintEventArgs with the Graphics surface and the ClipRectangle</param>
        protected virtual void OnDrawItem(IColorCategory item, PaintEventArgs e)
        {

            Rectangle box;
            string text;
            Brush brush;

            // Draw background
            if (item.IsSelected)
            {
                Rectangle selection = e.ClipRectangle;
                //selection.Y = e.ClipRectangle.Top + _itemSpacing;
                //selection.Height = _itemHeight;
                e.Graphics.FillRectangle(_highlightBrush, e.ClipRectangle);
                brush = _highlightTextBrush;
            }
            else
            {
                if (_contentStyle == ColorPanelStyle.Values && _layerType == LayerTypes.Raster)
                {
                    
                    e.Graphics.FillRectangle(_errorBrush, e.ClipRectangle);
                    
                }
                brush = _foreBrush;
            }


            // Establish bounds
            int left = Padding.Left;
            box = new Rectangle(e.ClipRectangle.Left + left, e.ClipRectangle.Top + _itemSpacing, e.ClipRectangle.Width - left - Padding.Right, ItemHeight - _itemSpacing * 2);


            if (_contentStyle == ColorPanelStyle.Colors)
            {
                brush = new System.Drawing.Drawing2D.LinearGradientBrush(box, item.LowColor, item.HighColor, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(brush, box);
                brush.Dispose(); // this was newly created, so dispose it
                e.Graphics.DrawRectangle(Pens.Black, box);
                return;
            }



            else if (_contentStyle == ColorPanelStyle.Captions)
            {

                text = item.LegendText;
            }
            else
            {
                text = item.ToString();
            }


            if (_horizontalAlignment == HorizontalAlignment.Left)
            {

                e.Graphics.DrawString(text, Font, brush, box);
            }
            else if (_horizontalAlignment == HorizontalAlignment.Right)
            {
                // clip the visible region so we don't mess up padding
                e.Graphics.SetClip(box);
                int right = box.Right;
                SizeF measure = e.Graphics.MeasureString(text, Font);
                left = right - Convert.ToInt32(measure.Width);
                Rectangle extendedBox = new Rectangle(left, box.Top, right - left, box.Height);
                e.Graphics.DrawString(text, Font, brush, extendedBox);
                e.Graphics.ResetClip();

            }
            else if (_horizontalAlignment == HorizontalAlignment.Center)
            {
                // clip the visible region so we don't mess up padding
                e.Graphics.SetClip(box);
                int mid = box.Left + box.Width / 2;
                int right = box.Right;
                SizeF measure = e.Graphics.MeasureString(text, Font);
                left = mid - Convert.ToInt32(measure.Width)/2;
                Rectangle extendedBox = new Rectangle(left, box.Top, right - left, box.Height);
                e.Graphics.DrawString(text, Font, brush, extendedBox);
                e.Graphics.ResetClip();

            }







        }
   


        /// <summary>
        /// Fires the EditBoxHidden event
        /// </summary>
        protected virtual void OnHideEditBox()
        {
            ValidateTextbox();
            _isEditing = false;
            _textBoxEdited = false;
            txtEditBox.Visible = false;
            if (EditBoxHidden != null)
            {
                EditBoxHidden(this, new EventArgs());
            }
            this.Focus();
        }

        /// <summary>
        /// Paints the background, or in the case of actual drawing, prevents it.
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (ColorBreaks == null || ColorBreaks.Count == 0)
            {
                base.OnPaintBackground(e);
            }
        }

      

        /// <summary>
        /// Paints the scene.  This can be overridden for custom drawing.
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (ColorBreaks == null || ColorBreaks.Count == 0)
            {
                base.OnPaint(e);
                return;
            }

            Bitmap bmp = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics backbuffer = Graphics.FromImage(bmp);

            // paint the entire background in the normal, non-selected color.
            backbuffer.FillRectangle(_backBrush, ClientRectangle);
            
            int yIndex = 0;



            for (int brk = _startIndex; brk < _colorBreaks.Count; brk++)
            {
                if (brk * ItemHeight > ClientRectangle.Height) break;

                Rectangle FullRectangle = new Rectangle(0, Padding.Top + ItemHeight * yIndex, ClientRectangle.Width, ItemHeight);

                OnDrawItem(_colorBreaks[brk], new PaintEventArgs(backbuffer, FullRectangle));

                yIndex++;
            }
            e.Graphics.DrawImage(bmp, 0, 0);


        }

        /// <summary>
        /// Fires the Refreshed event.
        /// </summary>
        protected virtual void OnRefresh()
        {
            this.Refresh();
            if (Refreshed != null) Refreshed(this, new EventArgs());
        }

        
        


        #endregion


        #region Private Methods

        /// <summary>
        /// Copies values from the text box if it is visible and fires the DataChanged event.
        /// </summary>
        private void UpdateValues()
        {
            if (_textBoxEdited == false || _editIndex == -1) return;
            IColorCategory brk = _colorBreaks[_editIndex];
            if (_contentStyle == ColorPanelStyle.Captions)
            {
                brk.LegendText = txtEditBox.Text;
                OnDataChanged();
                return;
            }

            string val1, val2;
            if (txtEditBox.Text.Length == 0)
            {
                brk.Minimum = 0.0;
                brk.Maximum = 0.0;
                return;
            }
            int i = txtEditBox.Text.IndexOf(" - ");
            int j = txtEditBox.Text.IndexOf("-", 1);
            if (i > 0)
            {
                val1 = txtEditBox.Text.Substring(0, i).Trim();
                val2 = txtEditBox.Text.Substring(i + 3).Trim();
                brk.Minimum = val1;
                brk.Maximum = val2;
            }
            if (j > 0)
            {
                val1 = txtEditBox.Text.Substring(0, j).Trim();
                val2 = txtEditBox.Text.Substring(j + 1).Trim();
                brk.LowValue = val1;
                brk.HighValue = val2;
            }
            else
            {
                brk.LowValue = txtEditBox.Text.Trim();
                brk.HighValue = txtEditBox.Text.Trim();
            }

            OnDataChanged();
        }

        #endregion

    }
}