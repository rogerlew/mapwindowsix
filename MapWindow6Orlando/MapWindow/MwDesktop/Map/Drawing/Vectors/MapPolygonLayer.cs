//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using MapWindow.Data;
using MapWindow.Geometries;
using MapWindow.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Diagnostics;

namespace MapWindow.Map
{
    /// <summary>
    /// A layer with drawing characteristics for LineStrings
    /// </summary>
    public class MapPolygonLayer: PolygonLayer, IMapPolygonLayer
    {

        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;


        #endregion

        #region Private variables


        // All the lists of pens to use for drawing the various stages
        // private List<Pen> _scaledPens;


        const int SELECTED = 1;
        const int X = 0;
        const int Y = 1;
        private readonly Rectangle _drawingBounds = new Rectangle(-32000, -32000, 64000, 64000);
        private Image _backBuffer; // draw to the back buffer, and swap to the stencil when done.
        private Image _stencil; // draw features to the stencil
        private IEnvelope _bufferExtent; // the geographic extent of the current buffer.
        private Rectangle _bufferRectangle;
  
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new empty MapPolygonLayer with an empty FeatureSet of FeatureType Polygon
        /// </summary>
        public MapPolygonLayer():base(new FeatureSet(FeatureTypes.Polygon))
        {
            Configure();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inFeatureSet"></param>
        public MapPolygonLayer(IFeatureSet inFeatureSet):base(inFeatureSet)
        {
            Configure();
            OnFinishedLoading();
        }

      
        /// <summary>
        /// Constructor that also shows progress
        /// </summary>
        /// <param name="featureSet">A featureset that contains lines</param>
        /// <param name="container">An IContainer that the line layer should be created in</param>
        public MapPolygonLayer(IFeatureSet featureSet, ICollection<ILayer> container)
            : base(featureSet, container, null)
        {
            Configure();
            OnFinishedLoading();
        }

        /// <summary>
        /// Constructor that also shows progress
        /// </summary>
        /// <param name="featureSet">A featureset that contains lines</param>
        /// <param name="container">An IContainer that the line layer should be created in</param>
        /// <param name="notFinished"></param>
        public MapPolygonLayer(IFeatureSet featureSet, ICollection<ILayer> container, bool notFinished)
            : base(featureSet, container, null)
        {
            Configure();
            if (notFinished == false) OnFinishedLoading();
        }

        private void Configure()
        {
            ChunkSize = 25000;
        }


        #endregion

        #region Methods

        /// <summary>
        /// Call StartDrawing before using this.
        /// </summary>
        /// <param name="rectangles">The rectangular region in pixels to clear.</param>
        /// <param name= "color">The color to use when clearing.  Specifying transparent
        /// will replace content with transparent pixels.</param>
        public void Clear(List<Rectangle> rectangles, Color color)
        {
            if (_backBuffer == null) return;
            Graphics g = Graphics.FromImage(_backBuffer);
            foreach (Rectangle r in rectangles)
            {
                if (r.IsEmpty == false)
                {
                    g.Clip = new Region(r);
                    g.Clear(color);
                }
            }
            g.Dispose();

        }

        

        /// <summary>
        /// This is testing the idea of using an input parameter type that is marked as out
        /// instead of a return type.
        /// </summary>
        /// <param name="result">The result of the creation</param>
        /// <returns>Boolean, true if a layer can be created</returns>
        public override bool CreateLayerFromSelectedFeatures(out IFeatureLayer result)
        {
            MapPolygonLayer temp;
            bool resultOk = CreateLayerFromSelectedFeatures(out temp);
            result = temp;
            return resultOk;
        }
        /// <summary>
        /// This is the strong typed version of the same process that is specific to geo point layers.
        /// </summary>
        /// <param name="result">The new GeoPointLayer to be created</param>
        /// <returns>Boolean, true if there were any values in the selection</returns>
        public virtual bool CreateLayerFromSelectedFeatures(out MapPolygonLayer result)
        {
            result = null;
            if (Selection == null || Selection.Count == 0) return false;
            FeatureSet fs = Selection.ToFeatureSet();
            result = new MapPolygonLayer(fs);
            
            return true;
        }

        /// <summary>
        /// If useChunks is true, then this method
        /// </summary>
        /// <param name="args">The GeoArgs that control how these features should be drawn.</param>
        /// <param name="features">The features that should be drawn.</param>
        /// <param name="clipRectangles">If an entire chunk is drawn and an update is specified, this clarifies the changed rectangles.</param>
        /// <param name="useChunks">Boolean, if true, this will refresh the buffer in chunks.</param>
        public virtual void DrawFeatures(MapArgs args, List<IFeature> features, List<Rectangle> clipRectangles, bool useChunks)
        {
            if (useChunks == false)
            {
                DrawFeatures(args, features);
                return;
            }
           
            int count = features.Count;
            int numChunks = (int)Math.Ceiling(count / (double)ChunkSize);

            for (int chunk = 0; chunk < numChunks; chunk++)
            {
                int numFeatures = ChunkSize;
                if (chunk == numChunks - 1) numFeatures = features.Count - (chunk * ChunkSize);
                DrawFeatures(args, features.GetRange(chunk * ChunkSize, numFeatures));

                if (numChunks > 0 && chunk < numChunks - 1)
                {
                    FinishDrawing();
                    OnBufferChanged(clipRectangles);
                    System.Windows.Forms.Application.DoEvents();
                    //this.StartDrawing();
                }
            }
            
        }
        /// <summary>
        /// If useChunks is true, then this method
        /// </summary>
        /// <param name="args">The GeoArgs that control how these features should be drawn.</param>
        /// <param name="indices">The features that should be drawn.</param>
        /// <param name="clipRectangles">If an entire chunk is drawn and an update is specified, this clarifies the changed rectangles.</param>
        /// <param name="useChunks">Boolean, if true, this will refresh the buffer in chunks.</param>
        public virtual void DrawFeatures(MapArgs args, List<int> indices, List<Rectangle> clipRectangles, bool useChunks)
        {
            
            if (useChunks == false)
            {
                DrawFeatures(args, indices);
                return;
            }

            int count = indices.Count;
            int numChunks = (int)Math.Ceiling(count / (double)ChunkSize);

            for (int chunk = 0; chunk < numChunks; chunk++)
            {
                int numFeatures = ChunkSize;
                if (chunk == numChunks - 1) numFeatures = indices.Count - (chunk * ChunkSize);
                DrawFeatures(args, indices.GetRange(chunk * ChunkSize, numFeatures));

                if (numChunks > 0 && chunk < numChunks - 1)
                {
                    // FinishDrawing();
                    OnBufferChanged(clipRectangles);
                    System.Windows.Forms.Application.DoEvents();
                    // this.StartDrawing();
                }
            }
        }


        /// <summary>
        /// This will draw any features that intersect this region.  To specify the features
        /// directly, use OnDrawFeatures.  This will not clear existing buffer content.
        /// For that call Initialize instead.
        /// </summary>
        /// <param name="args">A GeoArgs clarifying the transformation from geographic to image space</param>
        /// <param name="regions">The geographic regions to draw</param>
        /// <returns>The list of rectangular areas that match the specified regions</returns>
        public void DrawRegions(MapArgs args, List<IEnvelope> regions)
        {
            List<Rectangle> clipRects = args.ProjToPixel(regions);
            if (EditMode)
            {
                List<IFeature> drawList = new List<IFeature>();
                foreach (IEnvelope region in regions)
                {
                    if (region != null)
                    {
                        // Use union to prevent duplicates.  No sense in drawing more than we have to.
                        drawList = drawList.Union(DataSet.Select(region)).ToList();
                    }
                }
                DrawFeatures(args, drawList, clipRects, true);
            }
            else
            {
                List<int> drawList = new List<int>();
                List<ShapeRange> shapes = DataSet.ShapeIndices;
                for (int shp = 0; shp < shapes.Count; shp++)
                {

                    foreach (IEnvelope region in regions)
                    {
                        if (!shapes[shp].Extent.Intersects(region)) continue;
                        drawList.Add(shp);
                        break;
                    }
                }
                DrawFeatures(args, drawList, clipRects, true);
            }
        }



        /// <summary>
        /// Indicates that the drawing process has been finalized and swaps the back buffer
        /// to the front buffer.
        /// </summary>
        public void FinishDrawing()
        {
            if (_stencil != null && _stencil != _backBuffer) _stencil.Dispose();
            _stencil = _backBuffer;
        }

        

        /// <summary>
        /// Copies any current content to the back buffer so that drawing should occur on the
        /// back buffer (instead of the fore-buffer).  Calling draw methods without
        /// calling this may cause exceptions.
        /// </summary>
        /// <param name="preserve">Boolean, true if the front buffer content should be copied to the back buffer 
        /// where drawing will be taking place.</param>
        public void StartDrawing(bool preserve)
        {
            Bitmap backBuffer = new Bitmap(BufferRectangle.Width, BufferRectangle.Height);
            if (Buffer != null)
            {
                if (Buffer.Width == backBuffer.Width && Buffer.Height == backBuffer.Height)
                {
                    if (preserve)
                    {
                        Graphics g = Graphics.FromImage(backBuffer);
                        g.DrawImageUnscaled(Buffer, 0, 0);
                    }
                }
            }
            if (BackBuffer != null && BackBuffer != Buffer) BackBuffer.Dispose();
            BackBuffer = backBuffer;
            OnStartDrawing();
        }

     
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back buffer that will be drawn to as part of the initialization process.
        /// </summary>
        [ShallowCopy,
         Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BackBuffer
        {
            get { return _backBuffer; }
            set { _backBuffer = value; }
        }

        /// <summary>
        /// Gets the current buffer.
        /// </summary>
        [ShallowCopy,
         Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image Buffer
        {
            get { return _stencil; }
            set { _stencil = value; }
        }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnvelope BufferEnvelope
        {
            get { return _bufferExtent; }
            set { _bufferExtent = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle BufferRectangle
        {
            get { return _bufferRectangle; }
            set { _bufferRectangle = value; }
        }

        /// <summary>
        /// Gets or sets the label layer that is associated with this polygon layer.
        /// </summary>
        [ShallowCopy,
        Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IMapLabelLayer LabelLayer
        {
            get { return base.LabelLayer as IMapLabelLayer; }
            set { base.LabelLayer = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the OnBufferChanged event
        /// </summary>
        /// <param name="clipRectangles">The System.Drawing.Rectangle in pixels</param>
        protected virtual void OnBufferChanged(List<Rectangle> clipRectangles)
        {
            if (BufferChanged != null)
            {
                ClipArgs e = new ClipArgs(clipRectangles);
                BufferChanged(this, e);
            }
        }

        

        /// <summary>
        /// A default method to generate a label layer.
        /// </summary>
        protected override void OnCreateLabels()
        {
            LabelLayer = new MapLabelLayer(this);
        }

        /// <summary>
        /// Occurs when a new drawing is started, but after the BackBuffer has been established.
        /// </summary>
        protected virtual void OnStartDrawing()
        {

        }

        #endregion

        #region Private Functions

        // This draws the individual line features
        private void DrawFeatures(MapArgs e, IEnumerable<IFeature> features)
        {

            List<GraphicsPath> paths;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // First, use the coordinates to build the drawing paths
            BuildPaths(e, features, out paths);
            // Next draw all the paths using the various category symbols.
            DrawPaths(e, paths);
            sw.Stop();
            Debug.WriteLine("Drawing time: " + sw.ElapsedMilliseconds);
           
            foreach (GraphicsPath path in paths)
            {
                path.Dispose();
            }
        }

        // This draws the individual line features
        private void DrawFeatures(MapArgs e, IEnumerable<int> indices)
        {
            if (DataSet.ShapeIndices == null) return;
            List<GraphicsPath> paths;
            // First, use the coordinates to build the drawing paths
            BuildPaths(e, indices, out paths);
            // Next draw all the paths using the various category symbols.
            DrawPaths(e, paths);
            foreach (GraphicsPath path in paths)
            {
                path.Dispose();
            }
           
        }

        /// <summary>
        /// Draws the GraphicsPaths.  Before we were effectively "re-creating" the same geometric
        /// </summary>
        /// <param name="e"></param>
        /// <param name="paths"></param>
        private void DrawPaths(MapArgs e, IList<GraphicsPath> paths)
        {
            Graphics g = e.Device ?? Graphics.FromImage(_backBuffer);
            int numCategories = Symbology.Categories.Count;
            
            if(!DrawnStatesNeeded && !EditMode)
            {
                IPolygonSymbolizer ps = Symbolizer;
                g.SmoothingMode = ps.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
                Extent catBounds = new Extent(DataSet.Envelope);
                RectangleF bounds = new RectangleF();
                bounds.X = Convert.ToSingle((catBounds.XMin - e.MinX) * e.Dx);
                bounds.Y = Convert.ToSingle((e.MaxY - catBounds.YMax) * e.Dy);
                float r = Convert.ToSingle((catBounds.XMax - e.MinX) * e.Dx);
                bounds.Width = r - bounds.X;
                float b = Convert.ToSingle((e.MaxY - catBounds.YMin) * e.Dy);
                bounds.Height = b - bounds.Y;

                foreach (IPattern pattern in ps.Patterns)
                {
                    IGradientPattern gp = pattern as IGradientPattern;
                    if (gp != null)
                    {
                        gp.Bounds = bounds;
                    }

                    pattern.FillPath(g, paths[0]);
                }

                double scale = 1;
                if (ps.ScaleMode == ScaleModes.Geographic)
                {
                    scale = e.ImageRectangle.Width / e.GeographicExtents.Width;
                }
                foreach (IPattern pattern in ps.Patterns)
                {
                    if (pattern.UseOutline)
                    {
                        pattern.DrawPath(g, paths[0], scale);
                    }
                }
            }
            else
            {
                for (int selectState = 0; selectState < 2; selectState++)
                {
                    int iCategory = 0;
                    foreach (IPolygonCategory category in Symbology.Categories)
                    {
                        Extent catBounds;
                        if (CategoryExtents.Keys.Contains(category))
                        {
                            catBounds = CategoryExtents[category];
                        }
                        else
                        {
                            catBounds = CalculateCategoryExtent(category);
                        }
                        if (catBounds == null) catBounds = new Extent(Envelope);
                        RectangleF bounds = new RectangleF();
                        bounds.X = Convert.ToSingle((catBounds.XMin - e.MinX) * e.Dx);
                        bounds.Y = Convert.ToSingle((e.MaxY - catBounds.YMax) * e.Dy);
                        float r = Convert.ToSingle((catBounds.XMax - e.MinX) * e.Dx);
                        bounds.Width = r - bounds.X;
                        float b = Convert.ToSingle((e.MaxY - catBounds.YMin) * e.Dy);
                        bounds.Height = b - bounds.Y;

                        int index = selectState * numCategories + iCategory;
                        // Define the symbology based on the category and selection state
                        IPolygonSymbolizer ps = category.Symbolizer;
                        if (selectState == SELECTED) ps = category.SelectionSymbolizer;

                        g.SmoothingMode = ps.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;

                        foreach (IPattern pattern in ps.Patterns)
                        {
                            IGradientPattern gp = pattern as IGradientPattern;
                            if (gp != null)
                            {
                                gp.Bounds = bounds;
                            }
                            paths[index].FillMode = FillMode.Winding;
                            pattern.FillPath(g, paths[index]);
                        }

                        double scale = 1;
                        if (ps.ScaleMode == ScaleModes.Geographic)
                        {
                            scale = e.ImageRectangle.Width / e.GeographicExtents.Width;
                        }
                        foreach (IPattern pattern in ps.Patterns)
                        {
                            if (pattern.UseOutline)
                            {
                                pattern.DrawPath(g, paths[index], scale);
                            }
                        }
                        iCategory++;
                    } // category
                } // selectState
            }

            for (int i = 0; i < Symbology.Categories.Count; i++)
            {
                paths[i].Dispose();
            }
            if(e.Device == null) g.Dispose();
        
        }

        private void BuildPaths(MapArgs e, IEnumerable<int> indices, out List<GraphicsPath> paths)
        {
            paths = new List<GraphicsPath>();     
            Extent drawExtents = new Extent(e.PixelToProj(_drawingBounds));
            Dictionary<FastDrawnState, GraphicsPath> borders = new Dictionary<FastDrawnState, GraphicsPath>();
            for (int selectState = 0; selectState < 2; selectState++)
            {
                foreach (IPolygonCategory category in Symbology.Categories)
                {
                    FastDrawnState state = new FastDrawnState(selectState == 1, category);
                    
                    GraphicsPath border = new GraphicsPath();
                    borders.Add(state, border);
                    paths.Add(border);
                }
            }
            List<ShapeRange> shapes = DataSet.ShapeIndices;
            double[] vertices = DataSet.Vertex;
            if(!DrawnStatesNeeded)
            {
                FastDrawnState state = new FastDrawnState(false, Symbology.Categories[0]);
                foreach (int shp in indices)
                {
                    ShapeRange shape = shapes[shp];
                    if (!shape.Extent.Intersects(e.GeographicExtents)) continue;
                    if(shp >= shapes.Count) continue;
                    if (!borders.ContainsKey(state)) continue;
                    if (drawExtents.Contains(shape.Extent))
                    {
                        BuildPolygon(vertices, shapes[shp], borders[state], e, false);
                    }
                    else
                    {
                        BuildPolygon(vertices, shapes[shp], borders[state], e, true);
                    }
                }
            }
            else
            {
                FastDrawnState[] states = DrawnStates;
                foreach (int shp in indices)
                {
                    if (shp >= shapes.Count) continue;
                    if (shp >= states.Length)
                    {
                        AssignFastDrawnStates();
                        states = DrawnStates;
                    }
                    if (states[shp].Visible == false) continue;
                    ShapeRange shape = shapes[shp];
                    if (!shape.Extent.Intersects(e.GeographicExtents)) continue;
                    if (drawExtents.Contains(shape.Extent))
                    {
                        FastDrawnState state = states[shp];
                        if (!borders.ContainsKey(state)) continue;
                        BuildPolygon(vertices, shapes[shp], borders[state], e, false);
                    }
                    else
                    {
                        FastDrawnState state = states[shp];
                        if (!borders.ContainsKey(state)) continue;
                        BuildPolygon(vertices, shapes[shp], borders[state], e, true);
                    }
                    
                }
               
            }
            
        }

        private void BuildPaths(MapArgs e, IEnumerable<IFeature> features, out List<GraphicsPath> borderPaths)
        {
            borderPaths = new List<GraphicsPath>();
            IEnvelope drawExtents = e.PixelToProj(_drawingBounds);
            for (int selectState = 0; selectState < 2; selectState++)
            {
                foreach (IPolygonCategory category in Symbology.Categories)
                {

                    // Determine the subset of the specified features that are visible and match the category
                    IPolygonCategory polygonCategory = category;
                    int i = selectState;
                    Func<IDrawnState, bool> isMember = state =>
                        state.SchemeCategory == polygonCategory &&
                        state.IsVisible &&
                        state.IsSelected == (i == 1);

                    var drawnFeatures = from feature in features
                                        where isMember(DrawingFilter[feature])
                                        select feature;

                    GraphicsPath borderPath = new GraphicsPath();
                    foreach (IFeature f in drawnFeatures)
                    {

                        if (drawExtents.Contains(f.Envelope))
                        {
                            // This optimization only works if we are zoomed out far enough
                            // that the whole linestring fits in the short integer
                            // drawing window.  Otherwise, we need to crop the line.
                            // FastBuildLine(graphPath, f, minX, maxY, dx, dy);
                            BuildPolygon(DataSet.Vertex, f.ShapeIndex, borderPath, e, false);
                        }
                        else
                        {
                            BuildPolygon(DataSet.Vertex, f.ShapeIndex, borderPath, e, true);
                        }
                    }
                    borderPaths.Add(borderPath);
                }
            }
        }

        /// <summary>
        /// Appends the specified polygon to the graphics path.
        /// </summary>
        private static void BuildPolygon(double[] vertices, ShapeRange shpx, GraphicsPath borderPath, MapArgs args, bool clip)
        {
            double minX = args.MinX;
            double maxY = args.MaxY;
            double dx = args.Dx;
            double dy = args.Dy;
            borderPath.FillMode = FillMode.Winding;
            for (int prt = 0; prt < shpx.Parts.Count; prt++)
            {
                PartRange prtx = shpx.Parts[prt];
                int start = prtx.StartIndex;
                int end = prtx.EndIndex;
                List<double[]> points = new List<double[]>();
               
                for (int i = start; i <= end; i++)
                {
                    double[] pt = new double[2];
                    pt[X] = (vertices[i * 2] - minX) * dx;
                    pt[Y] = (maxY - vertices[i*2+1]) * dy;
                    points.Add(pt);
                }
                if (clip)
                {
                    points = SoutherlandHodgman.Clip(points);
                }
                List<System.Drawing.Point> intPoints = DuplicationPreventer.Clean(points);
                if (intPoints.Count < 2)
                {
                    points.Clear();
                    continue;
                }
                borderPath.StartFigure();
                System.Drawing.Point[] pointArray = intPoints.ToArray();
                borderPath.AddLines(pointArray);
                
                points.Clear();

                
            }


        }


        

 
        #endregion


       



    }
}
