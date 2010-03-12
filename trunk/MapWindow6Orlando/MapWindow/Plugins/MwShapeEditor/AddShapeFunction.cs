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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/11/2009 11:03:39 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using MapWindow.Map;
using Point=System.Drawing.Point;

namespace MapWindow.ShapeEditor
{

    /// <summary>
    /// AddShapeFunction
    /// </summary>
    public class AddShapeFunction : MapFunction
    {

        #region private variables

        private IFeatureSet _featureSet;
        private List<Coordinate> _coordinates;
        private CoordinateDialog _coordinateDialog;
        private System.Drawing.Point _mousePosition;
        private bool _standBy;
        private IMapLineLayer _tempLayer;
        private ContextMenu _context;
        private MenuItem _finishPart;
        private List<List<Coordinate>> _parts;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AddShapeFunction
        /// </summary>
        public AddShapeFunction()
        {
            Configure();
            
        }
        /// <summary>
        /// Creates a new instance of AddShapeFunction, but specifies
        /// the Map that this function should be applied to.
        /// </summary>
        /// <param name="map"></param>
        public AddShapeFunction(IMap map)
            : base(map)
        {
            Configure();
        }

        private void Configure()
        {
            _context = new ContextMenu();
            _context.MenuItems.Add("Delete", DeleteShape);
            _finishPart = new MenuItem("Finish Part", FinishPart);
            _context.MenuItems.Add(_finishPart);
            _context.MenuItems.Add("Finish Shape", FinishShape);
            _parts = new List<List<Coordinate>>();
        }
      

        #endregion

        #region Methods 

        /// <summary>
        /// Forces this function to begin collecting points for building a new shape.
        /// </summary>
        protected override void OnActivate()
        {
            if (_coordinateDialog == null) _coordinateDialog = new CoordinateDialog();
         
            if (_featureSet.CoordinateType == CoordinateTypes.Z)
            {
                _coordinateDialog.ShowZValues = true;
                _coordinateDialog.ShowMValues = true;
            }
            else if (_featureSet.CoordinateType == CoordinateTypes.M)
            {
                _coordinateDialog.ShowZValues = false;
                _coordinateDialog.ShowMValues = true;
            }
            else
            {
                _coordinateDialog.ShowZValues = false;
                _coordinateDialog.ShowMValues = false;
            }
            if(_featureSet.FeatureType == FeatureTypes.Point || _featureSet.FeatureType == FeatureTypes.MultiPoint)
            {
                if(_context.MenuItems.Contains(_finishPart))
                _context.MenuItems.Remove(_finishPart);
            }
            else
            {
                if (!_context.MenuItems.Contains(_finishPart))
                    _context.MenuItems.Add(1, _finishPart);
            }
            _coordinateDialog.Show();
            _coordinateDialog.FormClosing += CoordinateDialogFormClosing;
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
            if(_coordinateDialog != null)_coordinateDialog.Hide();
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
            if (_featureSet.FeatureType == FeatureTypes.Point) return;

            // Draw any completed parts first so that they are behind my active drawing content.
            if (_parts != null)
            {
                GraphicsPath gp = new GraphicsPath();

                List<Point> partPoints = new List<Point>();
                foreach (List<Coordinate> part in _parts)
                {
                    foreach (Coordinate c in part)
                    {
                        partPoints.Add(Map.ProjToPixel(c));
                    }
                    if (_featureSet.FeatureType == FeatureTypes.Line)
                    {
                        gp.AddLines(partPoints.ToArray());
                    }
                    if (_featureSet.FeatureType == FeatureTypes.Polygon)
                    {
                        gp.AddPolygon(partPoints.ToArray());
                    }
                    partPoints.Clear();
                }
                e.Graphics.DrawPath(Pens.Blue, gp);
                if (_featureSet.FeatureType == FeatureTypes.Polygon)
                {
                    Brush fill = new SolidBrush(Color.FromArgb(70, Color.LightCyan));
                    e.Graphics.FillPath(fill, gp);
                    fill.Dispose();
                }
            }

            Pen bluePen = new Pen(Color.Blue, 2F);
            Pen redPen = new Pen(Color.Red, 3F);
            Brush redBrush = new SolidBrush(Color.Red);
            List<Point> points = new List<Point>();
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (_coordinates != null)
            {
                foreach (Coordinate coord in _coordinates)
                {
                    points.Add(Map.ProjToPixel(coord));
                }
                foreach (Point pt in points)
                {
                    e.Graphics.FillRectangle(redBrush, new Rectangle(pt.X - 2, pt.Y - 2, 4, 4));
                }
                if (points.Count > 1)
                {
                    if (_featureSet.FeatureType != FeatureTypes.MultiPoint)
                    {
                        e.Graphics.DrawLines(bluePen, points.ToArray());
                    }
                    
                }
                if (points.Count > 0 && _standBy == false)
                {
                    if (_featureSet.FeatureType != FeatureTypes.MultiPoint)
                    {
                        e.Graphics.DrawLine(redPen, points[points.Count - 1], _mousePosition);
                    }
                }
            }
            
            
            bluePen.Dispose();
            redPen.Dispose();
            redBrush.Dispose();
            base.OnDraw(e);
        }

        /// <summary>
        /// updates the auto-filling X and Y coordinates
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            
            if (_standBy) return;
            _coordinateDialog.X = e.GeographicLocation.X;
            _coordinateDialog.Y = e.GeographicLocation.Y;
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
            if (_featureSet == null) return;
            // Add the current point to the featureset
            if (_featureSet.FeatureType == FeatureTypes.Point)
            {
                MapWindow.Geometries.Point pt = new MapWindow.Geometries.Point(_coordinateDialog.Coordinate);
                Feature f = new Feature(pt);
                _featureSet.Features.Add(f);
                _featureSet.UpdateEnvelopes();
                _featureSet.InvalidateVertices();
                return;
            }
            
            if (e.Button == MouseButtons.Right)
            {
                _context.Show((Control)Map, e.Location);
            }
            else
            {
                if (_coordinates == null) _coordinates = new List<Coordinate>();
                _coordinates.Add(e.GeographicLocation);
                if (_coordinates.Count > 1)
                {
                    System.Drawing.Point p1 = Map.ProjToPixel(_coordinates[_coordinates.Count - 1]);
                    System.Drawing.Point p2 = Map.ProjToPixel(_coordinates[_coordinates.Count - 2]);
                    Rectangle invalid = Global.GetRectangle(p1, p2);
                    invalid.Inflate(20, 20);
                    Map.Invalidate(invalid);
                }
                   
                
                
               
            }
            
            base.OnMouseUp(e);
        }

        public void DeleteShape(object sender, EventArgs e)
        {
            Feature f = null;
            _coordinates = new List<Coordinate>();
            _parts = new List<List<Coordinate>>();
            Map.Invalidate();
        }

        public void FinishShape(object sender, EventArgs e)
        {
            Feature f = null;
            if (_featureSet.FeatureType == FeatureTypes.MultiPoint)
            {
               f = new Feature(new MultiPoint(_coordinates));
            }
            if (_featureSet.FeatureType == FeatureTypes.Line || _featureSet.FeatureType == FeatureTypes.Polygon)
            {
                FinishPart(sender, e);
                Shape shp = new Shape(_featureSet.FeatureType);
                foreach (List<Coordinate> part in _parts)
                {
                    shp.AddPart(part, _featureSet.CoordinateType);
                }
                f = new Feature(shp);
            }
            if (f != null)
            {
                
                _featureSet.Features.Add(f);
                _featureSet.UpdateEnvelopes();
            }
            _featureSet.InvalidateVertices();
            _coordinates = new List<Coordinate>();
            _parts = new List<List<Coordinate>>();
        }

        public void FinishPart(object sender, EventArgs e)
        {
            _parts.Add(_coordinates);
            _coordinates = new List<Coordinate>();
            Map.Invalidate();
        }

        /// <summary>
        /// Occurs when this function is removed.
        /// </summary>
        protected override void OnUnload()
        {
            if (Enabled)
            {
                _coordinates = null;
                _coordinateDialog.Hide();
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

        #region Properties

        /// <summary>
        /// Gets or sets the featureset to modify
        /// </summary>
        public IFeatureSet FeatureSet
        {
            get { return _featureSet; }
            set { _featureSet = value; }
        }

        
    

        #endregion

        void CoordinateDialogFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            // This signals that we are done with editing, and should therefore close up shop
            Enabled = false;
        }
    }
}
