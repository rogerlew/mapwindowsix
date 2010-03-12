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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2008 3:21:30 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using MapWindow.Drawing;
using MapWindow.Geometries;
namespace MapWindow.Map
{


    /// <summary>
    /// SelectTool
    /// </summary>
    public class ZoomOutFunction : MapFunction
    {
        #region Private Variables

        //private bool _isDragging;
        //private System.Drawing.Point _startPoint;
        //private Coordinate _geoStartPoint;
        private System.Drawing.Point _currentPoint;
        
        //private Pen _selectionPen;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SelectTool
        /// </summary>
        public ZoomOutFunction(IMap inMap)
            : base(inMap)
        {
            //_selectionPen = new Pen(Color.Black);
            //_selectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        #endregion

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnDraw(MapDrawArgs e)
        //{
        //    if (_isDragging)
        //    {
        //        Rectangle r = Opp.RectangleFromPoints(_startPoint, _currentPoint);
        //        r.Width -= 1;
        //        r.Height -= 1;
        //        e.Graphics.DrawRectangle(Pens.White, r);
        //        e.Graphics.DrawRectangle(_selectionPen, r);
        //    }
        //    base.OnDraw(e);
        //}

        ///// <summary>
        ///// Handles the MouseDown 
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnMouseDown(GeoMouseArgs e)
        //{
        //    _startPoint = e.Location;
        //    _geoStartPoint = e.GeographicLocation;
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        _isDragging = true;
        //    }

            
        //    base.OnMouseDown(e);
        //}

        ///// <summary>
        ///// Handles MouseMove
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnMouseMove(GeoMouseArgs e)
        //{
        //    _currentPoint = e.Location;
        //    Map.Invalidate();
        //    base.OnMouseMove(e);
        //}

        /// <summary>
        /// Handles the Mouse Up situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            bool handled = false;
            _currentPoint = e.Location;
           
            Map.Invalidate();
            //if(_isDragging)
            //{
            //    if (_geoStartPoint != null && _startPoint != e.Location)
            //    {
            //        IEnvelope env = new Envelope(_geoStartPoint.X, e.GeographicLocation.X, _geoStartPoint.Y, e.GeographicLocation.Y);
            //        if (Math.Abs(e.X - _startPoint.X) > 1 && Math.Abs(e.Y - _startPoint.Y) > 1)
            //        {
            //            e.Map.Extents = env;
            //            handled = true;
            //        }
            //    }
            //} 
            //_isDragging = false;
            
            //if (handled == false)
            //{
                Rectangle r = e.Map.MapFrame.View;
                int w = r.Width;
                int h = r.Height;

                r.Inflate(r.Width / 2, r.Height / 2);
                r.X += w / 2 - e.X;
                r.Y += h / 2 - e.Y;
                e.Map.MapFrame.View = r;
                e.Map.MapFrame.ResetExtents();

                //if (e.Button == MouseButtons.Left)
                //{
                //    r.Inflate(-r.Width / 4, -r.Height / 4);
                //    // The mouse cursor should anchor the geographic location during zoom.
                //    r.X += (e.X/2) - w/4;
                //    r.Y += (e.Y/2) - h/4;
                //}
                //else
                //{
                //    r.Inflate(r.Width / 2, r.Height / 2);
                //    r.X += w / 2 - e.X;
                //    r.Y += h / 2 - e.Y;
                //}
                //e.Map.MapFrame.View = r;
                //e.Map.MapFrame.ResetExtents();
            //}
            
            base.OnMouseUp(e);
        }

    }
}
