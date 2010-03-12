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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/19/2009 10:59:47 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MapWindow.Geometries;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;

namespace MapWindow.Map
{


    /// <summary>
    /// MeasureFunction
    /// </summary>
    public class MeasureFunction : MapFunction
    {
        private IFeatureSet _featureSet;
        private List<Coordinate> _coordinates;
        private MeasureDialog _measureDialog;
        private System.Drawing.Point _mousePosition;
        private bool _standBy;
        private IMapLineLayer _tempLayer;
        private double _previousDistance; // the distance of all the segments except the current one.
        private bool _areaMode;
       
        #region Constructors

        /// <summary>
        /// Creates a new instance of AddShapeFunction
        /// </summary>
        public MeasureFunction()
        {

        }
        /// <summary>
        /// Creates a new instance of AddShapeFunction, but specifies
        /// the Map that this function should be applied to.
        /// </summary>
        /// <param name="map"></param>
        public MeasureFunction(IMap map)
            : base(map)
        {
        }

      

        #endregion

        #region Methods

        

        /// <summary>
        /// Forces this function to begin collecting points for building a new shape.
        /// </summary>
        protected override void OnActivate()
        {

            if (_measureDialog == null || _measureDialog.IsDisposed) _measureDialog = new MeasureDialog();
            _measureDialog.Show();
            _measureDialog.MeasureModeChanged += new EventHandler(_measureDialog_MeasureModeChanged);
            _measureDialog.FormClosing += CoordinateDialogFormClosing;
            if(_standBy == false) _coordinates = new List<Coordinate>();
            if (_tempLayer != null)
            {
                Map.MapFrame.DrawingLayers.Remove(_tempLayer);
                Map.MapFrame.Invalidate();
                Map.Invalidate();
                _tempLayer = null;
            }
            _standBy = false;
            base.OnActivate();
        }

        void _measureDialog_MeasureModeChanged(object sender, EventArgs e)
        {
            _areaMode = (_measureDialog.MeasureMode == MeasureModes.Area);
            if(_coordinates != null)_coordinates = new List<Coordinate>();
            Map.Invalidate();
        }

        /// <summary>
        /// Allows for new behavior during deactivation.
        /// </summary>
        protected override void OnDeactivate()
        {
            if (_standBy) return;
            // Don't completely deactivate, but rather go into standby mode
            // where we draw only the content that we have actually locked in.
            _standBy = true;
            // base.OnDeactivate();
            if(_measureDialog != null)_measureDialog.Hide();
            if (_coordinates != null && _coordinates.Count > 1)
            {
                LineString ls = new LineString(_coordinates);
                FeatureSet fs = new FeatureSet(FeatureTypes.Line);
                fs.Features.Add(new Feature(ls));
                MapLineLayer gll = new MapLineLayer(fs);
                //gll.Symbolizer.FillColor = Color.Blue;
                gll.Symbolizer.ScaleMode = ScaleModes.Symbolic;
                gll.Symbolizer.Smoothing = true;
                gll.MapFrame = Map.MapFrame;
               
                _tempLayer = gll;
                Map.MapFrame.DrawingLayers.Add(gll);
               // Map.MapFrame.Initialize(_tempLayer);
                Map.MapFrame.Invalidate();
                Map.Invalidate();
            }

        }

        /// <summary>
        /// Handles drawing of editing features
        /// </summary>
        /// <param name="e">The drawing args</param>
        protected override void OnDraw(MapDrawArgs e)
        {
   
            if (_standBy) return;
            Pen bluePen = new Pen(Color.Blue, 2F);
            Pen redPen = new Pen(Color.Red, 3F);
            Brush redBrush = new SolidBrush(Color.Red);
            List<System.Drawing.Point> points = new List<System.Drawing.Point>();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (_coordinates != null)
            {
                foreach (Coordinate coord in _coordinates)
                {
                    points.Add(Map.ProjToPixel(coord));
                }
                
                if (points.Count > 1)
                {
                    e.Graphics.DrawLines(bluePen, points.ToArray());
                    foreach (System.Drawing.Point pt in points)
                    {
                        e.Graphics.FillRectangle(redBrush, new Rectangle(pt.X - 2, pt.Y - 2, 4, 4));
                    }
                }
                
                if (points.Count > 0 && _standBy == false)
                {
                    e.Graphics.DrawLine(redPen, points[points.Count - 1], _mousePosition);
                    if(_areaMode && points.Count > 1)
                    {
                        e.Graphics.DrawLine(redPen, points[0], _mousePosition);
                    }
                }
                if (points.Count > 1 && _areaMode)
                {
                    points.Add(_mousePosition);
                    e.Graphics.FillPolygon(Brushes.Blue, points.ToArray());
                }
            }
            bluePen.Dispose();
            redPen.Dispose();
            redBrush.Dispose();
            base.OnDraw(e);
        }

        void CoordinateDialogFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            // This signals that we are done with editing, and should therefore close up shop
            Enabled = false;
        }

      

        /// <summary>
        /// updates the auto-filling X and Y coordinates
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            if (_standBy) return;
            if (_coordinates == null || _coordinates.Count == 0) return;
            Coordinate c1 = e.GeographicLocation;
            if (_measureDialog.MeasureMode == MeasureModes.Distance)
            {
                Coordinate c2 = _coordinates[_coordinates.Count-1];
                double dx = Math.Abs(c2.X - c1.X);
                double dy = Math.Abs(c2.Y - c1.Y);
                double dist;
                if(Map.Projection != null)
                {
                    if(Map.Projection.IsLatLon)
                    {
                        double y = (c2.Y + c1.Y) / 2;
                        double factor = Math.Cos(y * Math.PI / 180);
                        dx *= factor;
                        dist = Math.Sqrt(dx*dx + dy*dy);
                        dist = dist * 111319.5;
                    }
                    else
                    {
                        dist = Math.Sqrt(dx * dx + dy * dy);
                        dist *= Map.Projection.Unit.Meters;
                    }
                }
                else
                {

                    dist = Math.Sqrt(dx * dx + dy * dy);
                }

                _measureDialog.Distance = dist;
                _measureDialog.TotalDistance = _previousDistance + dist;
            }
            else
            {
                List<Coordinate> tempPolygon = _coordinates.ToList();
                tempPolygon.Add(c1);
                if (tempPolygon.Count < 3)
                {
                    _measureDialog.Area = 0;
                    _measureDialog.TotalArea = 0;
                    if(tempPolygon.Count == 2)
                    {
                        Rectangle r = Map.ProjToPixel(new LineString(tempPolygon).Envelope);
                        r.Inflate(20,20);
                        Map.Invalidate(r);
                    }
                    _mousePosition = e.Location;
                    return;
                }
                Polygon pg = new Polygon(new LinearRing(tempPolygon));
                double area = pg.Area;
                if (Map.Projection != null)
                {
                    if (Map.Projection.IsLatLon)
                    {
                        area = area * 111319.5 * 111319.5;
                    }
                    else
                    {
                        area *= Map.Projection.Unit.Meters * Map.Projection.Unit.Meters;
                    }
                }
                _measureDialog.Area = area;
                _measureDialog.TotalArea = area;
                Rectangle rr = Map.ProjToPixel(pg.Envelope);
                rr.Inflate(20, 20);
                Map.Invalidate(rr);
                _mousePosition = e.Location;
            }

       
            if (_coordinates != null && _coordinates.Count > 0)
            {
                List<System.Drawing.Point> points = new List<System.Drawing.Point>();
                foreach (Coordinate coord in _coordinates)
                {
                    points.Add(Map.ProjToPixel(coord));
                }
                Rectangle oldRect = Global.GetRectangle(_mousePosition, points[points.Count - 1]);
                Rectangle newRect = Global.GetRectangle(e.Location, points[points.Count - 1]);
                Rectangle invalid = Rectangle.Union(newRect, oldRect);
                invalid.Inflate(20, 20);
                Map.Invalidate(invalid);
            }
            _mousePosition = e.Location;
            base.OnMouseMove(e);
        }

        
        /// <summary>
        /// Handles the Mouse-Up situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (_standBy) return;
            // Add the current point to the featureset
           
            
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _previousDistance = 0;
                _coordinates = new List<Coordinate>();
                Map.Invalidate();
            }
            else
            {
                if (_coordinates == null) _coordinates = new List<Coordinate>();
               
                if (_coordinates.Count > 0)
                {
                    if (_measureDialog.MeasureMode == MeasureModes.Distance)
                    {
                        Coordinate c1 = e.GeographicLocation;
                        Coordinate c2 = _coordinates[_coordinates.Count - 1];
                        double dx = Math.Abs(c2.X - c1.X);
                        double dy = Math.Abs(c2.Y - c1.Y);
                        double dist;
                        if (Map.Projection != null)
                        {
                            if (Map.Projection.IsLatLon)
                            {
                                double y = (c2.Y + c1.Y) / 2;
                                double factor = Math.Cos(y * Math.PI / 180);
                                dx *= factor;
                                dist = Math.Sqrt(dx * dx + dy * dy);
                                dist = dist * 111319.5;
                            }
                            else
                            {
                                dist = Math.Sqrt(dx * dx + dy * dy);
                                dist *= Map.Projection.Unit.Meters;
                            }
                        }
                        else
                        {
                            dist = Math.Sqrt(dx * dx + dy * dy);
                        }
                        _measureDialog.Distance = dist;
                        _measureDialog.TotalDistance = _previousDistance + dist;
                        _previousDistance += dist;
                    }

                    System.Drawing.Point p1 = Map.ProjToPixel(_coordinates[_coordinates.Count - 1]);
                    System.Drawing.Point p2 = e.Location;
                    Rectangle invalid = Global.GetRectangle(p1, p2);
                    invalid.Inflate(20, 20);
                    Map.Invalidate(invalid);
                }
                _coordinates.Add(e.GeographicLocation);
            }
            
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Occurs when this function is removed.
        /// </summary>
        protected override void OnUnload()
        {
            if (Enabled)
            {
                _coordinates = null;
                _measureDialog.Hide();
            }
            if (_tempLayer != null)
            {
                Map.MapFrame.DrawingLayers.Remove(_tempLayer);
                Map.MapFrame.Invalidate();
                
                _tempLayer = null;
            }
            Map.Invalidate();
        }

        #endregion


        /// <summary>
        /// Gets or sets the featureset to modify
        /// </summary>
        public IFeatureSet FeatureSet
        {
            get { return _featureSet; }
            set { _featureSet = value; }
        }



    }
}
