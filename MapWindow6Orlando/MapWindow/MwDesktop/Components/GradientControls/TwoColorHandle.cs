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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/26/2009 2:13:31 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
using MapWindow.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
namespace MapWindow.Components
{


    /// <summary>
    /// RoundedSlider
    /// </summary>
    public class TwoColorHandle
    {
        #region Private Variables

        private float _position;
        private Color _color;
        private int _width;
        private int _roundingRadius;
        private TwoColorSlider _parent;
        private bool _isDragging;
        private bool _visible;
        private bool _left;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RoundedSlider
        /// </summary>
        public TwoColorHandle()
        {
            _width = 5;
            _roundingRadius = 2;
            _color = Color.SteelBlue;
            _visible = true;
        }

        /// <summary>
        /// Creates a new instance of a rounded handle, specifying the parent gradient slider 
        /// </summary>
        /// <param name="parent"></param>
        public TwoColorHandle(TwoColorSlider parent)
        {
            _parent = parent;
            _width = 5;
            _roundingRadius = 2;
            _color = Color.SteelBlue;
            _visible = true;
        }


        #endregion

        #region Methods

        /// <summary>
        /// Draws this slider on the specified graphics object
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            if (_visible == false) return;
            if (_width == 0) return;
            Rectangle bounds = GetBounds();
            GraphicsPath gp = new GraphicsPath();
            gp.AddRoundedRectangle(bounds, _roundingRadius);
            LinearGradientBrush lgb = new LinearGradientBrush(bounds, Color.Lighter(.3F), Color.Darker(.3F), LinearGradientMode.ForwardDiagonal);
            g.FillPath(lgb, gp);
            Pen l = new Pen(Color.Darker(.2f), 2);
            Pen r = new Pen(Color.Lighter(.2f), 2);
            if (_left)
            {
                g.DrawLine(l, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Height);
            }
            else
            {
                g.DrawLine(r, bounds.Left + 1, bounds.Top, bounds.Left + 1, bounds.Right);
            }
            l.Dispose();
            r.Dispose();
            lgb.Dispose();
            gp.Dispose();
 
        }

        /// <summary>
        /// Gets the bounds of this handle in the coordinates of the parent slider.
        /// </summary>
        public Rectangle GetBounds()
        {
            float sx = (_position - _parent.Minimum) / (_parent.Maximum - _parent.Minimum);
            int x = Convert.ToInt32(sx * (_parent.Width - _width));
            Rectangle bounds = new Rectangle(x, 0, _width, _parent.Height);
            return bounds;
        }

    

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating whether this is the left handle or not.
        /// </summary>
        public bool IsLeft
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether this is visible or not
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDragging
        {
            get { return _isDragging; }
            set { _isDragging = value; }
        }
        

        /// <summary>
        /// Gets or sets the parent
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TwoColorSlider Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the Position
        /// </summary>
        [Description("Gets or sets the Position")]
        public float Position
        {
            get
            {
                float w = Width * (_parent.Maximum - _parent.Minimum) / (float)_parent.Width;
                return _left ? _position + w : _position;
            }
            set
            {
                float w = Width * (_parent.Maximum - _parent.Minimum) / (float)_parent.Width;
                _position = _left ? value - w : value;
                if (_parent != null)
                {
                    if (_position > _parent.Maximum) _position = _parent.Maximum;
                    if (_position < _parent.Minimum) _position = _parent.Minimum;
                }
            }
        }

        /// <summary>
        /// Gets the color at the handles current position
        /// </summary>
        public Color GetColor()
        {
            Color min = _parent.MinimumColor;
            Color max = _parent.MaximumColor;
            float p = _position;
            int r = min.R + (int)((max.R - min.R)*p);
            int g = min.G + (int)((max.G - min.G) * p);
            int b = min.B + (int)((max.B - min.B) * p);
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Gets or sets the integer width of this slider in pixels.
        /// </summary>
        [Description("Gets or sets the integer width of this slider in pixels.")]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the basic color of the slider
        /// </summary>
        [Description("Gets or sets the basic color of the slider")]
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the integer describing the radius of the curves in the corners of the slider
        /// </summary>
        [Description("Gets or sets the integer describing the radius of the curves in the corners of the slider")]
        public int RoundingRadius
        {
            get { return _roundingRadius; }
            set { _roundingRadius = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that controls whether or not this handle is drawn and visible.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        

        #endregion



    }
}