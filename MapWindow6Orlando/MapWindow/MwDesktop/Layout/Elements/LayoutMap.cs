//********************************************************************************************************
// Product Name: MapWindow.Layout.Elements.LayoutMap
// Description:  The MapWindow LayoutMap element, the map 
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
using MapWindow.Geometries;

namespace MapWindow.Layout.Elements
{
    /// <summary>
    /// A layout control that draws the content from a map control so that it can be printed
    /// </summary>
    public class LayoutMap : LayoutElement 
    {
        private IEnvelope _envelope;
        private Map.Map _mapControl;
        private RectangleF _oldRectangle;
        private Bitmap _buffer;
        private bool _extentChanged = true;

        #region ------------------ Public Properties

        /// <summary>
        /// The geographic envelope to be shown by the layout
        /// </summary>
        [Browsable(false)]
        virtual public IEnvelope Envelope
        {
            get { return _envelope; }
            set 
            {
                IEnvelope tempEnv = new Envelope();
                if (value.Width / value.Height < Size.Width / Size.Height)
                {
                    double yCenter = value.Minimum.Y + (value.Height / 2.0);
                    double deltaY = (value.Width / Size.Width * Size.Height) / 2.0;
                    tempEnv.Minimum.Y = yCenter - deltaY;
                    tempEnv.Maximum.Y = yCenter + deltaY;
                    tempEnv.Minimum.X = value.Minimum.X;
                    tempEnv.Maximum.X = value.Maximum.X;
                }
                else
                {
                    double xCenter = value.Minimum.X + (value.Width / 2.0);
                    double deltaX = (value.Height / Size.Height * Size.Width) / 2.0;
                    tempEnv.Minimum.X = xCenter - deltaX;
                    tempEnv.Maximum.X = xCenter + deltaX;
                    tempEnv.Minimum.Y = value.Minimum.Y;
                    tempEnv.Maximum.Y = value.Maximum.Y;
                }
                _envelope = tempEnv;
                _extentChanged = true;
                OnThumbnailChanged();
                OnInvalidate();
            }
        }

        /// <summary>
        /// The map control that generates the printable content
        /// </summary>
        [Browsable(false)]
        public Map.Map MapControl
        {
            get { return _mapControl; }
            set {_mapControl = value; }
        }

        /// <summary>
        /// A mathematical calculation using the map
        /// </summary>
        [Browsable(true),Category("Map")]
        virtual public int Scale
        {
            get 
            {
                if (_mapControl.Layers.Count < 1)
                    return (100000);
                return Convert.ToInt32((UnitMeterConversion() * _envelope.Width * 39.3700787 * 100D) / Size.Width);
            }
            set 
            {
                if (_mapControl.Layers.Count < 1)
                    return;
                IEnvelope tempEnv = Envelope;
                double xtl = tempEnv.X;
                double ytl = tempEnv.Y;
                tempEnv.Width = (value * Size.Width) / (UnitMeterConversion() * 39.3700787 * 100D);
                tempEnv.Height = (value * Size.Height) / (UnitMeterConversion() * 39.3700787 * 100D);
                tempEnv.X = xtl;
                tempEnv.Y = ytl;
                Envelope = tempEnv;
            }
        }

        private double UnitMeterConversion()
        {
            if (_mapControl == null) return 1;
            if (_mapControl.Layers.Count == 0) return 1;
            if (_mapControl.Layers[0].DataSet == null) return 1;
            if (_mapControl.Layers[0].DataSet.Projection == null) return 1;
            if (_mapControl.Layers[0].DataSet.Projection.IsLatLon)
                return _mapControl.Layers[0].DataSet.Projection.GeographicInfo.Unit.Radians * 6354101.943;
            return _mapControl.Layers[0].DataSet.Projection.Unit.Meters;
        }

        #endregion

        #region ------------------- public methods

        /// <summary>
        /// The constructor to build a new LayoutMap control
        /// </summary>
        public LayoutMap()
        {
            Name = "Map";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LayoutMap(Map.Map mapControl)
        {
            Name = "Map";
            _mapControl = mapControl;
            _envelope = _mapControl.Extents;
            ResizeStyle = ResizeStyle.NoScaling;
        }

        /// <summary>
        /// Updates the size of the control
        /// </summary>
        protected override void OnSizeChanged()
        {
            if (Resizing == false)
            {                   
                //If the size has never been set before we set the maps extent to that of the map
                if (_oldRectangle.Width == 0 && _oldRectangle.Height == 0)
                    Envelope = MapControl.Extents;
                else
                {
                    double dx = Envelope.Width / _oldRectangle.Width;
                    double dy = Envelope.Height / _oldRectangle.Height;
                    IEnvelope newEnv = Envelope.Copy();

                    newEnv.Width = newEnv.Width + ((Rectangle.Width - _oldRectangle.Width) * dx);
                    newEnv.Height = newEnv.Height + ((Rectangle.Height - _oldRectangle.Height) * dy);
                    newEnv.X = Envelope.X;
                    newEnv.Y = Envelope.Y;

                    Envelope = newEnv;
                }
                _oldRectangle = new RectangleF(LocationF, Size);
             }

            base.OnSizeChanged();
        }

        /// <summary>
        /// Zooms the map to the fullextent of all available layers
        /// </summary>
        public virtual void ZoomToFullExtent()
        {
            Envelope = MapControl.Envelope;
            base.OnThumbnailChanged();
            base.OnInvalidate();
        }

        /// <summary>
        /// Zooms the map to the extent of the current view
        /// </summary>
        public virtual void ZoomViewExtent()
        {
            Envelope = _mapControl.Extents;
        }

        /// <summary>
        /// Pans the map
        /// </summary>
        /// <param name="x">The amount to pan the map in the X-axis in map coord</param>
        /// <param name="y">The amount to pan the map in the Y-axis in map coord</param>
        public virtual void PanMap(double x, double y)
        {
            Envelope = new Envelope(Envelope.Minimum.X - x, Envelope.Maximum.X - x, Envelope.Minimum.Y - y, Envelope.Maximum.Y - y);
        }

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">Boolean, true if the drawing is printing to an actual page</param>
        override public void Draw(Graphics g, bool printing)
        {
            if (printing == false)
            {
                if (MapControl.Layers.Count <= 0 || Convert.ToInt32(Size.Width) <= 0 || Convert.ToInt32(Size.Height) <= 0)
                    return;

                if (_buffer != null && ((_buffer.Width != Convert.ToInt32(Size.Width * 96 / 100) || _buffer.Height != Convert.ToInt32(Size.Height * 96 / 100)) || _extentChanged))
                {
                    _extentChanged = false;
                    _buffer.Dispose();
                    _buffer = null;
                }

                if (_buffer == null)
                {
                    _buffer = new Bitmap(Convert.ToInt32(Size.Width * 96 / 100), Convert.ToInt32(Size.Height * 96 / 100), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    _buffer.SetResolution(96, 96);
                    Graphics graph = Graphics.FromImage(_buffer);
                    MapControl.Print(graph, new Rectangle(0, 0, _buffer.Width, _buffer.Height), _envelope);
                    graph.Dispose();
                }
                g.DrawImage(_buffer, Rectangle);
            }
            else
            {
                MapControl.Print(g, new Rectangle(Location.X, Location.Y, Convert.ToInt32(Size.Width), Convert.ToInt32(Size.Height)), _envelope);
            }
        }

        #endregion
    }
}
