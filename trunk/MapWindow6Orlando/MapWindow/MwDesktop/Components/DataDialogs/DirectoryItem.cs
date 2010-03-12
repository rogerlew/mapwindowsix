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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2008 2:06:42 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MapWindow.Components
{


    /// <summary>
    /// DirectoryItems can be either Files or Folders
    /// </summary>
    public class DirectoryItem
    {

        #region Events
        
       

        #endregion



        #region Private Variables

        private Image _customImage;
        private ItemTypes _itemType;
        private bool _showImage;
        private Color _fontColor;
        private bool _isHighlighted;
        private bool _isSelected;
        private Timer _highlightTimer;
        private string _path;
        private bool _isOutlined;
        private Rectangle _box;
        private Font _font;
        private string _text;
        private Color _backColor;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DirectoryItem
        /// </summary>
        public DirectoryItem()
        {
            Configure(null);
            
        }

        /// <summary>
        /// Creates a new instance of a directory item based on the specified path.
        /// </summary>
        /// <param name="path"></param>
        public DirectoryItem(string path)
        {
            Configure(path);
        }

        private void Configure(string path)
        {
            Width = 500;
            Height = 20;
            BackColor = Color.White;
            _fontColor = Color.Black;
            _highlightTimer = new Timer();
            _highlightTimer.Interval = 10;
            //_highlightTimer.Tick += new EventHandler(_highlightTimer_Tick);
            if (path == null) return;
            _path = path;
            string[] directoryParts = path.Split(System.IO.Path.DirectorySeparatorChar);
            _text = directoryParts[directoryParts.GetUpperBound(0)];
        }

        //void _highlightTimer_Tick(object sender, EventArgs e)
        //{
        //    Point mouse = this.PointToClient(Cursor.Position);
        //    if (ClientRectangle.Contains(mouse)) return;
        //    _highlightTimer.Stop();
        //    IsHighlighted = false;
        //}

        #endregion

        #region Methods

       

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color for this item.
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle in 
        /// </summary>
        public Rectangle Bounds
        {
            get { return _box; }
            set { _box = value; }
        }

        /// <summary>
        /// Gets a rectangle in control coordinates showing the size of this control
        /// </summary>
        public Rectangle ClientRectangle
        {
            get { return new Rectangle(0, 0, Width, Height); }
        }

        /// <summary>
        /// Gets or sets the custom icon that is used if the ItemType is set to custom
        /// </summary>
        public Image CustomImage
        {
            get { return _customImage; }
            set { _customImage = value; }
        }

        /// <summary>
        /// Gets or sets the font for this directory item
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// Gets or sets the color that should be used for drawing the fonts.
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        /// <summary>
        /// Gets the icon that should be used
        /// </summary>
        public virtual Image Image
        {
            get
            {
                switch (_itemType)
                {
                    case ItemTypes.Custom: return _customImage;
                    case ItemTypes.Folder: return DialogImages.FolderOpen;
                    case ItemTypes.Image: return DialogImages.FileImage;
                    case ItemTypes.Line: return DialogImages.FileLine;
                    case ItemTypes.Point: return DialogImages.FilePoint;
                    case ItemTypes.Polygon: return DialogImages.FilePolygon;
                    case ItemTypes.Raster: return DialogImages.FileGrid;
                }
                return _customImage;
            }
        }

        /// <summary>
        /// Gets or sets whether this specific item should be drawn highlighted
        /// </summary>
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set 
            {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
                    if (_isHighlighted == true) _highlightTimer.Start();
                }
            }
        }

        /// <summary>
        /// Gets or sets a boolean that controls whether or not a black dotted rectangle
        /// will surround this item.
        /// </summary>
        public bool IsOutlined
        {
            get { return _isOutlined; }
            set 
            { 
                _isOutlined = value;
            }
        }

        /// <summary>
        /// Gets or sets whether this specific item should be drawn highlighted
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                }
            }
        }

        /// <summary>
        /// Gets or set the ItemType for this particular directory item.
        /// </summary>
        public ItemTypes ItemType
        {
            get { return _itemType; }
            set 
            { 
                _itemType = value;
                if (_itemType != ItemTypes.Custom) _showImage = true;
            }
        }
        
        

        /// <summary>
        /// Gets or sets the complete path for this directory item.
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        

        /// <summary>
        /// Gets or sets a boolean governing whether or not an icon should be drawn.
        /// </summary>
        public bool ShowImage
        {
            get { return _showImage; }
            set { _showImage = value; }
        }

        /// <summary>
        /// Gets or sets the string text for this directory item.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or set the integer top of this item
        /// </summary>
        public int Top
        {
            get { return _box.Y; }
            set { _box.Y = value; }
        }

        /// <summary>
        /// Gets or sets the width of this control
        /// </summary>
        public int Width
        {
            get { return _box.Width; }
            set { _box.Width = value; }
        }

        /// <summary>
        /// Gets or sets the height of this control
        /// </summary>
        public int Height
        {
            get { return _box.Height; }
            set { _box.Height = value; }
        }

       
        #endregion

        #region Protected Methods

       
        
        ///// <summary>
        ///// Fires the HighlightChanged event
        ///// </summary>
        ///// <param name="isHighlighted">Specifies the new boolean for whether or not this control is highlighted</param>
        //protected virtual void OnHighlightChanged(bool isHighlighted)
        //{
        //    Invalidate();
        //    if (HighlightChanged != null) HighlightChanged(this, new HighlightEventArgs(isHighlighted));
        //}

        ///// <summary>
        ///// Handles the OnMouseMove event to initialize the Highlight Changed method
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        //    IsHighlighted = true;
        //}

        ///// <summary>
        ///// Overrides the base double click behavior to add the navigate functionality
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnDoubleClick(EventArgs e)
        //{
        //    OnNavigate(_path);
        //    base.OnDoubleClick(e);
        //}

        ///// <summary>
        ///// Fires the Navigate event
        ///// </summary>
        ///// <param name="path">The string path to navigate to</param>
        //protected virtual void OnNavigate(string path)
        //{
        //    if (Navigate != null) Navigate(this, new NavigateEventArgs(path));
        //}

        /// <summary>
        /// This method instructs this item to draw itself onto the specified graphics surface.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the Graphics object needed for drawing.</param>
        public virtual void Draw(PaintEventArgs e)
        {
            System.Drawing.Drawing2D.Matrix oldMatrix = e.Graphics.Transform;
            System.Drawing.Drawing2D.Matrix mat = new System.Drawing.Drawing2D.Matrix();
            mat.Translate(this.Bounds.Left, this.Bounds.Top);
            e.Graphics.Transform = mat;
            OnDraw(e);
            e.Graphics.Transform = oldMatrix;
        }

        /// <summary>
        /// This supplies the basic drawing for this one element where the graphics object has been transformed
        /// based on the position of this item.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the Graphics object needed for drawing.</param>
        protected virtual void OnDraw(PaintEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("top: " + this.Top.ToString());
            int left = 1;
            Pen border = Pens.White;
            Pen innerBorder = Pens.White;
            Brush fill = Brushes.White;
            Pen dots = new Pen(Color.Black);
            bool specialDrawing = false;
            dots.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            if (_isHighlighted == true && _isSelected == false)
            {
                border = new Pen(Color.FromArgb(216, 240, 250));
                innerBorder = new Pen(Color.FromArgb(248, 252, 254));
                fill = new System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, Color.FromArgb(245, 250, 253), Color.FromArgb(232, 245, 253), System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                specialDrawing = true;
            }
            if (_isSelected == true && _isHighlighted == false)
            {
                border = new Pen(Color.FromArgb(153, 222, 253));
                innerBorder = new Pen(Color.FromArgb(231, 245, 253));
                fill = new System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, Color.FromArgb(241, 248, 253), Color.FromArgb(213, 239, 252), System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                specialDrawing = true;
            }
            if (_isSelected == true && _isHighlighted == true)
            {
                border = new Pen(Color.FromArgb(182, 230, 251));
                innerBorder = new Pen(Color.FromArgb(242, 249, 253));
                fill = new System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, Color.FromArgb(232, 246, 253), Color.FromArgb(196, 232, 250), System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                specialDrawing = true;
            }

            e.Graphics.FillRectangle(fill, new Rectangle(1, 1, Width - 2, Height -2));
            DrawRoundedRectangle(e.Graphics, innerBorder, new Rectangle(2, 2, Width - 4, Height - 4));
            DrawRoundedRectangle(e.Graphics, border, new Rectangle(1, 1, Width - 2, Height - 2));

            if (_isOutlined)
            {
                e.Graphics.DrawRectangle(dots, new Rectangle(2, 2, Width - 4, Height - 4));
            }

            if (_showImage == true)
            {
                if (Height > 20)
                {
                    e.Graphics.DrawImage(Image, new Rectangle(1, 1, Height - 2, Height - 2));
                }
                else
                {
                    e.Graphics.DrawImage(Image, new Rectangle(1, 1, Image.Width, Image.Height));
                }
                left = Height + 2;
            }
            Brush b = new SolidBrush(_fontColor);
            e.Graphics.DrawString(Text, Font, b, new PointF(left, 1f));
            b.Dispose();

            if (specialDrawing == true)
            {
                if(border != null)border.Dispose();
                if(innerBorder != null)innerBorder.Dispose();
                if(fill != null)fill.Dispose();
                if(dots != null)dots.Dispose();
            }

            //base.OnPaint(e);
        }
     
        ///// <summary>
        ///// Fires the SelectedChanged event
        ///// </summary>
        ///// <param name="isSelected">Boolean that is true if the new state for this item is selected</param>
        //protected virtual void OnSelectChanged(bool isSelected)
        //{
        //    if(SelectChanged != null)SelectChanged(this, new SelectEventArgs(isSelected));
        //    Invalidate();
        //}



      
        #endregion

        #region Private Methods

        private void DrawRoundedRectangle(Graphics g, Pen pen, Rectangle rect)
        {
            int l = rect.Left;
            int r = rect.Right;
            int t = rect.Top;
            int b = rect.Bottom;
            g.DrawLine(pen, l + 1, t, r - 1, t);
            g.DrawLine(pen, l + 1, b, r - 1, b);
            g.DrawLine(pen, l, t + 1, l, b - 1);
            g.DrawLine(pen, r, t + 1, r, b - 1);

            g.DrawLine(pen, l, t + 2, l + 2, t);
            g.DrawLine(pen, r - 2, t, r, t + 2);
            g.DrawLine(pen, l, b - 2, l + 2, b);
            g.DrawLine(pen, r, b - 2, r - 2, b);

        }

        #endregion

    }
}
