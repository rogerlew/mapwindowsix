//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/2/2008 2:21:55 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Components
{


    /// <summary>
    /// ColorHeaderPanel
    /// </summary>
    internal class ThreeColumnHeaderPanel : Panel
    {
        #region Private Variables

        private bool _autoSizeLeftColumn = true;
        private bool _autoSizeHeight = true;
        private string _leftColumnText = "Colors";
        private int _leftColumnWidth = 100;
        private string _middleColumnText = "Values";
        private int _middleColumnWidth;
        private int _padding;
        private string _rightColumnText = "Captions";
        private int _rightColumnWidth;
        private HorizontalAlignment _textAlignment;
        private bool _borderVisible;
        private Color _paddingColor = SystemColors.ButtonFace;
       
        private SolidBrush _backBrush;
        private SolidBrush _foreBrush;
        private SolidBrush _paddingBrush;
        private bool _isSuspended;
       

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ColorHeaderPanel
        /// </summary>
        public ThreeColumnHeaderPanel()
        {
         
        }


        /// <summary>
        /// Disposes brushes
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(_foreBrush != null)_foreBrush.Dispose();
            if (_paddingBrush != null) _paddingBrush.Dispose();
            if (_backBrush != null) _backBrush.Dispose();
        }

        #endregion

       

        #region Methods

       

        /// <summary>
        /// Actually performs the autosize.
        /// </summary>
        public void AdjustSize()
        {
            if (_autoSizeHeight == false && _autoSizeLeftColumn == false) return;
            SizeF txtSize = new SizeF(50,20);
            if (_autoSizeHeight == true)
            {
                if (_leftColumnText != null)
                {
                    txtSize = this.CreateGraphics().MeasureString(_leftColumnText, Font);
                }
                else if (_middleColumnText != null)
                {
                    txtSize = this.CreateGraphics().MeasureString(_middleColumnText, Font);
                }
                else if (_rightColumnText != null)
                {
                    txtSize = this.CreateGraphics().MeasureString(_middleColumnText, Font);
                }
                //if (BorderStyle == BorderStyle.Fixed3D)
                //{
                //    txtSize.Height += 4;
                //}
                //else if (BorderStyle == BorderStyle.FixedSingle)
                //{
                //    txtSize.Height += 2;
                //}
                txtSize.Height += _padding * 2 + 4;
                this.Height = Convert.ToInt32(txtSize.Height);
            }

            //if (_autoSizeLeftColumn == true)
            //{
            //    if (_leftColumnText != null)
            //    {
            //        txtSize = this.CreateGraphics().MeasureString(_leftColumnText, Font);
            //        txtSize.Width += _padding * 2 + 4; // use an extra two padding here.
            //        _leftColumnWidth = Convert.ToInt32(txtSize.Width);
            //        AdjustColumnSize();
            //    }
                
            //}

        }

        /// <summary>
        /// Prevents flicker by preventing this from doing anything
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Prevent flickering
        }

        /// <summary>
        /// Overrides the basic paint method and draws the caption content.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_isSuspended == true) return;
            Bitmap bmp = new Bitmap(ClientRectangle.Width+1, ClientRectangle.Height+1);
            Graphics backBuffer = Graphics.FromImage(bmp);
            
            if(_backBrush == null)_backBrush = new SolidBrush(this.BackColor);
            if (_backBrush.Color != this.BackColor) _backBrush = new SolidBrush(this.BackColor);
         
            if (_foreBrush == null) _foreBrush = new SolidBrush(this.ForeColor);
            if (_foreBrush.Color != this.ForeColor) _foreBrush = new SolidBrush(this.ForeColor);

            if (_paddingColor == Color.Transparent) _paddingColor = Parent.BackColor;
            if (_paddingBrush == null) _paddingBrush = new SolidBrush(_paddingColor);
            if (_paddingColor != _paddingBrush.Color) _paddingBrush = new SolidBrush(_paddingColor);

            // Paint the whole rectangle to do the padding
            backBuffer.FillRectangle(_paddingBrush, ClientRectangle);

            int height = ClientRectangle.Height -1;
            int delta = 1;
            if (BorderStyle == BorderStyle.FixedSingle)
            { 
                delta = 2;
            }
            if (BorderStyle == BorderStyle.Fixed3D)
            {
                delta = 4;
            }
            if (height <= 0) return;

            RectangleF leftBox = new Rectangle(_padding, _padding, _leftColumnWidth - _padding * 2, height - _padding * 2);
            RectangleF middleBox = new Rectangle(_leftColumnWidth + _padding, _padding, _middleColumnWidth - _padding * 2 - delta, height - _padding * 2);
            RectangleF rightBox = new Rectangle(_leftColumnWidth + _middleColumnWidth + _padding - delta/2, _padding, _rightColumnWidth - _padding * 2 - delta, height - _padding * 2);

            if (_padding == 0)
            {
                middleBox.X = leftBox.Right;
                middleBox.Width = rightBox.Left - middleBox.X;
            }

            // Painting over individual boxes accomplishes the non-padding background.
            backBuffer.FillRectangle(_backBrush, leftBox);
            backBuffer.FillRectangle(_backBrush, middleBox);
            backBuffer.FillRectangle(_backBrush, rightBox);


            
            // Draw Text
            if (_textAlignment == HorizontalAlignment.Left)
            {
                backBuffer.DrawString(_leftColumnText, Font, _foreBrush, leftBox);
                backBuffer.DrawString(_middleColumnText, Font, _foreBrush, middleBox);
                backBuffer.DrawString(_rightColumnText, Font, _foreBrush, rightBox);
            }
            else if (_textAlignment == HorizontalAlignment.Center)
            {
                DrawCentered(backBuffer, _leftColumnText, leftBox);
                backBuffer.ResetClip();
                DrawCentered(backBuffer, _middleColumnText, middleBox);
                backBuffer.ResetClip();
                DrawCentered(backBuffer, _rightColumnText, rightBox);
                backBuffer.ResetClip();
            }
            else
            {
                DrawRight(backBuffer, _leftColumnText, leftBox);
                backBuffer.ResetClip();
                DrawRight(backBuffer, _middleColumnText, middleBox);
                backBuffer.ResetClip();
                DrawRight(backBuffer, _rightColumnText, rightBox);
                backBuffer.ResetClip();
            }

            if (_borderVisible == true)
            {
                backBuffer.DrawRectangle(Pens.Black, leftBox.X, leftBox.Y, leftBox.Width, leftBox.Height);
                backBuffer.DrawRectangle(Pens.Black, middleBox.X, middleBox.Y, middleBox.Width, middleBox.Height);
                backBuffer.DrawRectangle(Pens.Black, rightBox.X, rightBox.Y, rightBox.Width, rightBox.Height);
            }

            e.Graphics.DrawImage(bmp, 0F, 0F);
            backBuffer.Dispose();
           
        }

        
        private void DrawCentered(Graphics g, string text, RectangleF box)
        {
            g.SetClip(box);
            SizeF txt = g.MeasureString(text, Font);
            float x = box.X + box.Width / 2 - txt.Width / 2;
            g.DrawString(text, Font, _foreBrush, new PointF(x, box.Y));
        }

        private void DrawRight(Graphics g, string text, RectangleF box)
        {
            g.SetClip(box);
            SizeF txt = g.MeasureString(text, Font);
            float x = box.X + box.Width - txt.Width;
            g.DrawString(text, Font, _foreBrush, new PointF(x, box.Y));
        }

        /// <summary>
        /// Controls the resizing so that the text is drawn correctly.
        /// </summary>
        /// <param name="eventargs">EventArgs</param>
        protected override void OnResize(EventArgs eventargs)
        {
 	         base.OnResize(eventargs);
             AdjustColumnSize();
        }

        private void AdjustColumnSize()
        {
            if (_autoSizeLeftColumn == true)
            {
                _middleColumnWidth = (this.Width - _leftColumnWidth) / 2;
                _rightColumnWidth = _middleColumnWidth;
            }
        }

        /// <summary>
        /// Allows drawing
        /// </summary>
        public new void ResumeLayout()
        {
            _isSuspended = false;
        }

        /// <summary>
        /// Temporarilly prevents drawing while several parameters are adjusted
        /// </summary>
        public new void SuspendLayout()
        {
            _isSuspended = true;

        }

        #endregion

        #region Properties


     

        /// <summary>
        /// Gets or sets whether to draw a black pen border around each caption
        /// </summary>
        [Category("Appearance"), Description("Gets or sets whether to draw a black pen border around each caption")]
        public bool BorderVisible
        {
            get { return _borderVisible; }
            set 
            { 
                _borderVisible = value;
                Refresh();
            }
        }

        

        /// <summary>
        /// Gets or sets the caption for the left column
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the caption for the left column")]
        public string LeftColumnText
        {
            get { return _leftColumnText; }
            set 
            { 
                _leftColumnText = value;
                AdjustSize();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the integer width of the left column.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the integer width of the left column.")]
        public int LeftColumnWidth
        {
            get { return _leftColumnWidth; }
            set 
            { 
                _leftColumnWidth = value;
                AdjustColumnSize();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets a boolean.  If true, the left column will adjust to the size of the text.
        /// </summary>
        [Category("Appearance"), Description(" Gets or sets whether the left column should adjust to the size of the text.")]
        public bool AutoSizeLeftColumn
        {
            get { return _autoSizeLeftColumn; }
            set 
            { 
                _autoSizeLeftColumn = value;
                AdjustSize();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets a boolean.  If true, the header will automatically adjust to fit the text.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets a boolean.  If true, the header will automatically adjust to fit the text.")]
        public bool AutoSizeHeight
        {
            get { return _autoSizeHeight; }
            set
            {
                _autoSizeHeight = value;
                AdjustSize();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the font being used for this Header Panel.  If either of the AutoSize properties are true,
        /// setting this will resize the panel.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the font being used for this Header Panel.")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                AdjustSize();
                Refresh();
            }
        }

        
        /// <summary>
        /// Gets or sets the caption for the middle column
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the caption for the middle column")]
        public string MiddleColumnText
        {
            get { return _middleColumnText; }
            set 
            { 
                _middleColumnText = value;
                AdjustSize();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the integer width of the middle column.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the integer width of the middle column.")]
        public int MiddleColumnWidth
        {
            get { return _middleColumnWidth; }
            set 
            { 
                _middleColumnWidth = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the integer spacing to use around the text.  The height of the panel
        /// will adjust to fit the text plus padding on either side.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the integer width of the middle column.")]
        public int CellPadding
        {
            get { return _padding; }
            set 
            { 
                _padding = value;
                AdjustSize();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the color of the padding region.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the color of any visible padding.")]
        public Color PaddingColor
        {
            get { return _paddingColor; }
            set 
            {
                _paddingColor = value;
                _paddingBrush = new SolidBrush(_paddingColor);
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the caption for the value column.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the caption for the right column")]
        public string RightColumnText
        {
            get { return _rightColumnText; }
            set 
            { 
                _rightColumnText = value;
                AdjustSize();
                Refresh();
            }
        }

       
        /// <summary>
        /// Gets or sets the integer width of the right column.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the integer width of the right column.")]
        public int RightColumnWidth
        {
            get { return _rightColumnWidth; }
            set 
            { 
                _rightColumnWidth = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment for the text.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the horziontal alignment for the text.")]
        public HorizontalAlignment TextAlignment
        {
            get { return _textAlignment; }
            set 
            {
                _textAlignment = value;
                Refresh();
            }
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

     

    }
}
