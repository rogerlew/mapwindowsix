//********************************************************************************************************
// Product Name: MapWindow.Layout
// Description:  The MapWindow ILayoutElement the interface used by all elements that can be added to the layout
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
//********************************************************************************************************

using System;
using System.Drawing;
using System.ComponentModel;

namespace MapWindow.Layout.Elements
{
    /// <summary>
    /// The interface for all elements that can be added to the layout control
    /// </summary>
    [Serializable]
    public abstract class LayoutElement
    {
        #region Events

        /// <summary>
        /// Fires when the layout element is invalidated
        /// </summary>
        public event EventHandler Invalidated;
        /// <summary>
        /// Fires when the preview thumbnail for this element has been updated
        /// </summary>
        public event EventHandler ThumbnailChanged;
        /// <summary>
        /// Fires when the size of this element has been adjusted by the user
        /// </summary>
        public event EventHandler SizeChanged;


        #endregion

        private PointF _location;
        private SizeF _size;
        private String _name;
        private ResizeStyle _resizeStyle;
        private Bitmap _thumbNail;
        private bool _resizing;

        /// <summary>
        /// Gets or sets the name of the element
        /// </summary>
        [Browsable(true),Category("Layout")]
        public String Name
        {
            get { return _name; }
            set { _name = value; OnInvalidate(); }
        }

        /// <summary>
        /// Gets the thumbnail that appears in the LayoutListView
        /// </summary>
        [Browsable(false)]
        public Bitmap ThumbNail
        {
            get { return _thumbNail; }
            private set 
            {
                if (_thumbNail != null) _thumbNail.Dispose();
                _thumbNail = value;
                OnThumbnailChanged();
            }
        }

        /// <summary>
        /// Disables updating redraw when resizing.
        /// </summary>
        [Browsable(false)]
        public bool Resizing
        {
            get { return _resizing; }
            set { _resizing = value; }
        }

        /// <summary>
        /// Indicates if this element can handle redraw events on resize
        /// </summary>
        [Browsable(false)]
        public ResizeStyle ResizeStyle
        {
            get { return _resizeStyle; }
            set { _resizeStyle = value; }
        }

         /// <summary>
        /// Gets or sets the location of the top left corner of the control in 1/100 of an inch paper coordinants
        /// </summary>
        [Browsable(false)]
        public PointF LocationF
        {
            get { return _location; }
            set { _location = value; OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the location of the top left corner of the control in 1/100 of an inch paper coordinants
        /// </summary>
        [Browsable(true), Category("Layout")]
        public Point Location
        {
            get { return new Point(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y)); }
            set { _location = new PointF(value.X, value.Y); OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the size of the element in 1/100 of an inch paper coordinants
        /// </summary>
        [Browsable(true),Category("Layout")]
        public SizeF Size
        {
            get { return new SizeF(_size.Width,_size.Height); }
            set 
            {
                if (value.Width < 20)
                    value.Width = 20;
                if (value.Height < 20)
                    value.Height = 20;
                _size = value;
                OnSizeChanged();
                OnInvalidate();
                UpdateThumbnail();
            }
        }

        /// <summary>
        /// Gets or sets the rectangle of the element in 1/100th of an inch paper coordinants
        /// </summary>
        [Browsable(false)]
        public RectangleF Rectangle
        {
            get { return new RectangleF(_location, _size); }
            set
            {
                _location = value.Location;
                _size = value.Size;
                OnSizeChanged();
                OnInvalidate();
                UpdateThumbnail();
            }
        }

        /// <summary>
        /// Returns true if the point in paper coordinants intersects with the rectangle of the element
        /// </summary>
        /// <param name="paperPoint"></param>
        /// <returns></returns>
        public bool IntersectsWith(PointF paperPoint)
        {
            return IntersectsWith(new RectangleF(paperPoint.X, paperPoint.Y, 0F, 0F));
        }

        /// <summary>
        /// Returns true if the rectangle in paper coordinants intersects with the rectangle of the the element
        /// </summary>
        /// <param name="paperRectangle"></param>
        /// <returns></returns>
        public bool IntersectsWith(RectangleF paperRectangle)
        {
            return new RectangleF(LocationF, Size).IntersectsWith(paperRectangle);
        }

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">If true then we a actually printing not previewing so we should make it as high quality as possible</param>
        public abstract void Draw(Graphics g, bool printing);

        /// <summary>
        /// Causes the element to be refreshed
        /// </summary>
        public virtual void RefreshElement()
        {
            OnSizeChanged();
            OnInvalidate();
            UpdateThumbnail();
        }

        /// <summary>
        /// This returns the objects name as a string
        /// </summary>
        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Call this when it needs to updated
        /// </summary>
        protected virtual void OnInvalidate()
        {
            if (Invalidated != null)
                Invalidated(this, null);
        }

        /// <summary>
        /// Fires when the size of the element changes
        /// </summary>
        protected virtual void OnSizeChanged()
        {
            if (SizeChanged != null)
                SizeChanged(this, null);
        }

        /// <summary>
        /// Fires when the thumbnail gets modified
        /// </summary>
        protected virtual void OnThumbnailChanged()
        {
            if (ThumbnailChanged != null)
                ThumbnailChanged(this, null);
        }

        /// <summary>
        /// Updates the thumbnail when needed
        /// </summary>
        protected virtual void UpdateThumbnail()
        {
            if (Resizing) return;
            Bitmap tempThumbNail = new Bitmap(32, 32, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics graph = Graphics.FromImage(tempThumbNail);
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            if ((Size.Width / tempThumbNail.Width) > (Size.Height / tempThumbNail.Height))
            {
                graph.ScaleTransform(32F / Size.Width , 32F / Size.Width);
                graph.TranslateTransform(-LocationF.X , -LocationF.Y);
            }
            else
            {
                graph.ScaleTransform(32F / Size.Height , 32F /  Size.Height);
                graph.TranslateTransform(-LocationF.X, -LocationF.Y);
            }
            graph.Clip = new Region(Rectangle);
            Draw(graph, false);
            graph.Dispose();
            ThumbNail = tempThumbNail;
        }
    }
}
