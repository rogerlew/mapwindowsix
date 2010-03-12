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

using System.Drawing;
using System.Windows.Forms;
using MapWindow.Drawing;
using MapWindow.Geometries;
namespace MapWindow.Map
{


    /// <summary>
    /// SelectTool
    /// </summary>
    public class LabelSelectFunction : MapFunction
    {
        #region Private Variables

        private bool _isDragging;
        private System.Drawing.Point _startPoint;
        private Coordinate _geoStartPoint;
        private System.Drawing.Point _currentPoint;
        
        private readonly Pen _selectionPen;
        private bool _doSelect;
        private readonly Timer _selectTimer;
        private IEnvelope _selectionEnvelope;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SelectTool
        /// </summary>
        public LabelSelectFunction(IMap inMap)
            : base(inMap)
        {
            _selectionPen = new Pen(Color.Black);
            _selectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            _doSelect = false;
            _selectTimer = new Timer();
            _selectTimer.Interval = 10;
            _selectTimer.Tick += SelectTimerTick;
        }

        /// <summary>
        /// Selection envelope
        /// </summary>
        public IEnvelope SelectionEnvelope
        {
            get { return _selectionEnvelope; }
        }

        void SelectTimerTick(object sender, System.EventArgs e)
        {
            _selectTimer.Stop();
            Map.ResetBuffer();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDraw(MapDrawArgs e)
        {
            if (_isDragging)
            {
                Rectangle r = Opp.RectangleFromPoints(_startPoint, _currentPoint);
                r.Width -= 1;
                r.Height -= 1;
                e.Graphics.DrawRectangle(Pens.White, r);
                e.Graphics.DrawRectangle(_selectionPen, r);
            }
            if (_doSelect)
            {

                foreach (IMapLayer lyr in Map.MapFrame.Layers)
                {
                    IMapFeatureLayer fl = lyr as IMapFeatureLayer;
                    if (fl == null) continue;
                    IMapLabelLayer gll = fl.LabelLayer;
                    //gll.Select(_selectionEnvelope, e); // using this form of selection can test the actual pixel rectangles
                    if(gll != null) gll.Invalidate();
                }
                _doSelect = false;
                _selectTimer.Start();
                
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
            _currentPoint = e.Location;
            Map.Invalidate();
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the Mouse Up situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            _currentPoint = e.Location;
            _isDragging = false;
            Map.Invalidate();
            if (_geoStartPoint != null)
            {
                _selectionEnvelope = new Envelope(_geoStartPoint.X, e.GeographicLocation.X, _geoStartPoint.Y, e.GeographicLocation.Y); 
            }
            

            // If they are not pressing shift, then first clear the selection before adding new members to it.
            if (((Control.ModifierKeys & Keys.Shift) == Keys.Shift) == false)
            {
                foreach (IMapLayer lyr in Map.MapFrame.Layers)
                {
                    IMapFeatureLayer fl = lyr as IMapFeatureLayer;
                    if (fl == null) continue;
                    IMapLabelLayer gll = fl.LabelLayer;
                    if(gll!=null)gll.ClearSelection();
                }
                
            }
           
            _doSelect = true;
            e.Map.MapFrame.ResetBuffer();
            e.Map.Invalidate();
            base.OnMouseUp(e);
        }

        ///// <summary>
        ///// Disposes the selection pen
        ///// </summary>
        ///// <param name="disposing"></param>
        //protected override void Dispose(bool disposing)
        //{
        //    _selectionPen.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}
