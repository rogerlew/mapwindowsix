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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MapWindow.Drawing;
using MapWindow.Geometries;
using MapWindow.Main;
namespace MapWindow.Map
{


    /// <summary>
    /// SelectTool
    /// </summary>
    public class SelectFunction : MapFunction
    {
        #region Private Variables

        private bool _isDragging;
        private System.Drawing.Point _startPoint;
        private Coordinate _geoStartPoint;
        private System.Drawing.Point _currentPoint;
        
        private readonly Pen _selectionPen;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SelectTool
        /// </summary>
        public SelectFunction(IMap inMap)
            : base(inMap)
        {
            _selectionPen = new Pen(Color.Black);
            _selectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDraw(MapDrawArgs e)
        {
            if (_isDragging) // don't draw anything unless we need to draw a select rectangle
            {
                Rectangle r = Opp.RectangleFromPoints(_startPoint, _currentPoint);
                r.Width -= 1;
                r.Height -= 1;
                e.Graphics.DrawRectangle(Pens.White, r);
                e.Graphics.DrawRectangle(_selectionPen, r);
            }
            base.OnDraw(e);
        }

        /// <summary>
        /// Handles the MouseDown 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(GeoMouseArgs e)
        {
           
            if (e.Button == MouseButtons.Left)
            {
                _startPoint = e.Location;
                _geoStartPoint = e.GeographicLocation;
                _isDragging = true;
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles MouseMove
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            int x = Math.Min(Math.Min(_startPoint.X, _currentPoint.X), e.X);
            int y = Math.Min(Math.Min(_startPoint.Y, _currentPoint.Y), e.Y);
            int mx = Math.Max(Math.Max(_startPoint.X, _currentPoint.X), e.X);
            int my = Math.Max(Math.Max(_startPoint.Y, _currentPoint.Y), e.Y);
            _currentPoint = e.Location;
            if(_isDragging)
            {
                Map.Invalidate(new Rectangle(x, y, mx - x, my - y));
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the Mouse Up situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (_isDragging == false) return;
            _currentPoint = e.Location;
            _isDragging = false;
            //Map.Invalidate(); // Get rid of the selection box
            //Application.DoEvents();
            IEnvelope env = new Envelope(_geoStartPoint.X, e.GeographicLocation.X, _geoStartPoint.Y, e.GeographicLocation.Y);
            IEnvelope tolerant = env;
           
            if(_startPoint.X == e.X && _startPoint.Y == e.Y)
            {
                // click selection doesn't work quite right without some tiny tolerance.
                double tol = Map.MapFrame.Extents.Width/10000;
                env.ExpandBy(tol);
            }

            if (Math.Abs(_startPoint.X - e.X) < 8 && Math.Abs(_startPoint.Y - e.Y) < 8)
            {
                Coordinate c1 = e.Map.PixelToProj(new System.Drawing.Point(e.X - 4, e.Y - 4));
                Coordinate c2 = e.Map.PixelToProj(new System.Drawing.Point(e.X + 4, e.Y + 4));
                tolerant = new Envelope(c1, c2);
            }

            Map.MapFrame.SuspendEvents();
            HandleSelection(tolerant, env);
            Map.MapFrame.ResumeEvents();
            // Force an invalidate to clear the dotted lines, even if we haven't changed anything.
            e.Map.Invalidate();
            //e.Map.MapFrame.Initialize();
            sw.Stop();
           
            Debug.WriteLine("Initialize: " + sw.ElapsedMilliseconds);
            base.OnMouseUp(e);
        }

        private void HandleSelection(IEnvelope tolerant, IEnvelope strict)
        {
            
            
            Keys key = Control.ModifierKeys;
            if ((((key & Keys.Shift) == Keys.Shift) == false)
                && (((key & Keys.Control) == Keys.Control) == false))
            {
                // If they are not pressing shift, then first clear the selection before adding new members to it.
                IEnvelope region;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Map.ClearSelection(out region);
                sw.Stop();
                Debug.WriteLine("Clear: " + sw.ElapsedMilliseconds);

            }

            if ((key & Keys.Control) == Keys.Control)
            {
                IEnvelope region;
                Map.InvertSelection(tolerant, strict, SelectionModes.Intersects, out region);
            }
            else
            {
                IEnvelope region;
                Map.Select(tolerant, strict, SelectionModes.Intersects, out region);
               
            }
          
        }

      
    }
}
