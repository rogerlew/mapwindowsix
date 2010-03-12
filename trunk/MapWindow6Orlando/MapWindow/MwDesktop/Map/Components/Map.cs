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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/25/2008 9:37:01 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Jiri Kadlec        |  2/18/2010         |  Added zoom out button and custom mouse cursors
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using System.IO;
using MapWindow.Components;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using MapWindow.Main;
using MapWindow.Projections;
using MapWindow.Serialization;
using Point=System.Drawing.Point;

namespace MapWindow.Map
{
    [ToolboxBitmap(@"Map2D.ico")]
    public partial class Map : UserControl, IMap
    {
        #region Events

        /// <summary>
        /// Occurs after the map refreshes the image
        /// </summary>
        public event EventHandler FinishedRefresh;

        /// <summary>
        /// Occurs after a resize event
        /// </summary>
        public event EventHandler Resized;

        /// <summary>
        /// Public event advertising the mouse movement
        /// </summary>
        public event EventHandler<GeoMouseArgs> GeoMouseMove;


        /// <summary>
        /// Occurs after the selection has changed for all the layers
        /// </summary>
        public event EventHandler SelectionChanged;


        /// <summary>
        /// Occurs after a layer has been added to the mapframe, or any of the child groups of that mapframe.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerAdded;


        #endregion

        #region Private Variables


        private IMapFrame _geoMapFrame;

        //private Timer _resizeTimer;  
        private bool _resizing;
        private ILegend _legend;
        private bool _collisionDetection;
        private Dictionary<string, IMapFunction> _mapFunctions;
        private IProgressHandler _progressHandler;
        private FunctionModes _functionMode;
        private bool _collectAfterDraw;
      
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a map component that can be dropped on a form.
        /// </summary>
        public Map()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Handles the resizing in the case where the map uses docking, and therefore
        /// needs to be updated whenever the form changes size.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (ParentForm != null)
            {
                ParentForm.ResizeEnd += ParentForm_ResizeEnd;
                ParentForm.KeyDown += ParentForm_KeyDown;
                ParentForm.KeyUp += ParentForm_KeyUp;
                Parent.Resize += Parent_Resize;
            }
        }

        void Parent_Resize(object sender, EventArgs e)
        {
            if(Dock == DockStyle.Fill)
            {
                if(Width != Parent.Width || Height != Parent.Height)
                {
                    Width = Parent.Width;
                    Height = Parent.Height;
                }
            }
            _geoMapFrame.ResetExtents();
            Invalidate();
        }

       

        private void ParentForm_ResizeEnd(object sender, EventArgs e)
        {
            if (MapFrame != null)
            {
                _geoMapFrame.ResetExtents();
                Invalidate();
            }
        }




        private void Configure()
        {
            MapFrame = new MapFrame(this, new Envelope(-180, 180, -90, 90));

            _resizing = false;

            _mapFunctions = new Dictionary<string, IMapFunction>
                                  {
                                      {"Pan", new PanFunction(this)},
                                      {"Zoom", new ZoomFunction(this)}
                                  };
            SelectFunction s = new SelectFunction(this);
            _mapFunctions.Add("Select", s);
            _mapFunctions.Add("ClickZoom", new ClickZoomFunction(this));
            _mapFunctions.Add("ZoomOut", new ZoomOutFunction(this));
            _mapFunctions.Add("LabelSelect", new LabelSelectFunction(this));
            _mapFunctions.Add("Info", new IdentifyFunction(this));
            _mapFunctions.Add("Measure", new MeasureFunction(this));
            _collisionDetection = false;

            //changed by Jiri Kadlec - default function mode is none
            FunctionMode = FunctionModes.None;
           
            

        }

        void ParentForm_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (KeyValuePair<string, IMapFunction> toolPair in MapTools)
            {
                toolPair.Value.DoKeyUp(e);
            }
        }

        void ParentForm_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (KeyValuePair<string, IMapFunction> toolPair in MapTools)
            {
                toolPair.Value.DoKeyDown(e);
            }
        }

        private void MapFrame_ItemChanged(object sender, EventArgs e)
        {

            Invalidate();
        }

        private void MapFrame_UpdateMap(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void MapFrame_ScreenUpdated(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if(drgevent.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
            {
                drgevent.Effect = DragDropEffects.Copy;
            }
            else drgevent.Effect = DragDropEffects.None;

        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            string[] s = (string[])drgevent.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, false);
            int i;
            bool failed = false;
            for (i = 0; i < s.Length; i++)
            {
                try
                {
                    AddLayer(s[i]);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    // failed at least one effort
                    failed = true;
                }
            }
            if(failed)
            {
                MessageBox.Show("One or more of the files was not a valid data type for the map.");
            }
        }

        #endregion

        #region Methods

      
        /// <summary>
        /// This activates the labels for the specified feature layer that will be the specified expression
        /// where field names are in square brackets like "[Name]: [Value]".  This will label all the features,
        /// and remove any previous labeling.
        /// </summary>
        /// <param name="featureLayer">The FeatureLayer to apply the labels to.</param>
        /// <param name="expression">The string label expression to use where field names are in square brackets like [Name]</param>
        /// <param name="font">The font to use for these labels</param>
        /// <param name="fontColor">The color for the labels</param>
        public void AddLabels(IFeatureLayer featureLayer, string expression, Font font, Color fontColor)
        {
            featureLayer.ShowLabels = true;

            MapLabelLayer ll = new MapLabelLayer();
            ll.Symbology.Categories.Clear();
            LabelCategory lc = new LabelCategory();
            lc.Expression = expression;
            ll.Symbology.Categories.Add(lc);

            ILabelSymbolizer ls = ll.Symbolizer;
            ls.Orientation = ContentAlignment.MiddleCenter;
            ls.BackColorEnabled = false;
            ls.BackColorEnabled = false;
            ls.FontColor = fontColor;
            ls.FontFamily = font.FontFamily.ToString();
            ls.FontSize = font.Size;
            ls.FontStyle = font.Style;
            ls.LabelParts = LabelParts.LabelLargestPart;
            featureLayer.LabelLayer = ll;

            
        }

        /// <summary>
        /// Removes any existing label categories
        /// </summary>
        public void ClearLabels(IFeatureLayer featureLayer)
        {
            
            featureLayer.ShowLabels = false;
            if (featureLayer.LabelLayer == null) return;
            
        }

        /// <summary>
        /// This will add a new label category that will only apply to the specified filter expression.  This will not remove any existing categories.
        /// </summary>
        /// <param name="featureLayer">The feature layer that the labels should be applied to</param>
        /// <param name="expression">The string expression where field names are in square brackets</param>
        /// <param name="filterExpression">The string filter expression that controls which features are labeled.  Field names are in square brackets, strings in single quotes.</param>
        /// <param name="symbolizer">The label symbolizer that controls the basic appearance of the labels in this category.</param>
        public void AddLabels(IFeatureLayer featureLayer, string expression, string filterExpression, ILabelSymbolizer symbolizer)
        {
            if (featureLayer.LabelLayer == null) featureLayer.LabelLayer = new MapLabelLayer();
            featureLayer.ShowLabels = true;
            ILabelCategory lc = new LabelCategory();
            lc.Expression = expression;
            lc.FilterExpression = filterExpression;
            lc.Symbolizer = symbolizer;
            featureLayer.LabelLayer.Symbology.Categories.Add(lc);
            featureLayer.LabelLayer.CreateLabels();
        }

        /// <summary>
        /// This will add a new label category that will only apply to the specified filter expression.  This will not remove any existing categories.
        /// </summary>
        /// <param name="featureLayer">The feature layer that the labels should be applied to</param>
        /// <param name="expression">The string expression where field names are in square brackets</param>
        /// <param name="filterExpression">The string filter expression that controls which features are labeled.  Field names are in square brackets, strings in single quotes.</param>
        /// <param name="symbolizer">The label symbolizer that controls the basic appearance of the labels in this category.</param>
        /// <param name="width">A geographic width, so that if the map is zoomed to a geographic width smaller than this value, labels should appear.</param>
        public void AddLabels(IFeatureLayer featureLayer, string expression, string filterExpression, ILabelSymbolizer symbolizer, double width)
        {
            if (featureLayer.LabelLayer == null) featureLayer.LabelLayer = new MapLabelLayer();
            featureLayer.ShowLabels = true;
            ILabelCategory lc = new LabelCategory();
            lc.Expression = expression;
            lc.FilterExpression = filterExpression;
            lc.Symbolizer = symbolizer;
            featureLayer.LabelLayer.UseDynamicVisibility = true;
            featureLayer.LabelLayer.DynamicVisibilityWidth = width;
            featureLayer.LabelLayer.Symbology.Categories.Add(lc);
            featureLayer.LabelLayer.CreateLabels();
        }






        /// <summary>
        /// Gets the subset of layers that are specifically raster layers, allowing
        /// you to control their symbology.
        /// </summary>
        /// <returns></returns>
        public IMapImageLayer[] GetImageLayers()
        {
            List<IMapImageLayer> imageLayers = new List<IMapImageLayer>();
            foreach (IMapLayer layer in MapFrame.Layers)
            {
                IMapImageLayer imageLayer = layer as IMapImageLayer;
                if (imageLayer != null)
                {
                    imageLayers.Add(imageLayer);
                }
            }
            return imageLayers.ToArray();
        }

        /// <summary>
        /// Gets the subset of layers that are specifically raster layers, allowing
        /// you to control their symbology.
        /// </summary>
        /// <returns></returns>
        public IMapRasterLayer[] GetRasterLayers()
        {
            List<IMapRasterLayer> rasterLayers = new List<IMapRasterLayer>();
            foreach (IMapLayer layer in MapFrame.Layers)
            {
                IMapRasterLayer rasterLayer = layer as IMapRasterLayer;
                if (rasterLayer != null)
                {
                    rasterLayers.Add(rasterLayer);
                }
            }
            return rasterLayers.ToArray();
        }

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        public IMapLineLayer[] GetLineLayers()
        {
            List<IMapLineLayer> lineLayers = new List<IMapLineLayer>();
            foreach (IMapLayer layer in MapFrame.Layers)
            {
                IMapLineLayer lineLayer = layer as IMapLineLayer;
                if(lineLayer != null)
                {
                    lineLayers.Add(lineLayer);
                }
            }
            return lineLayers.ToArray();
        }

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        public IMapPolygonLayer[] GetPolygonLayers()
        {
            List<IMapPolygonLayer> polygonLayers = new List<IMapPolygonLayer>();
            foreach (IMapLayer layer in MapFrame.Layers)
            {
                IMapPolygonLayer polygonLayer = layer as IMapPolygonLayer;
                if (polygonLayer != null)
                {
                    polygonLayers.Add(polygonLayer);
                }
            }
            return polygonLayers.ToArray();
        }

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        public IMapPointLayer[] GetPointLayers()
        {
            List<IMapPointLayer> pointLayers = new List<IMapPointLayer>();
            foreach (IMapLayer layer in MapFrame.Layers)
            {
                IMapPointLayer pointLayer = layer as IMapPointLayer;
                if (pointLayer != null)
                {
                    pointLayers.Add(pointLayer);
                }
            }
            return pointLayers.ToArray();
        }

        /// <summary>
        /// Gets a list of just the feature layers regardless of whether they are lines, points, or polygons
        /// </summary>
        /// <returns>An array of IMapFeatureLayers</returns>
        public IMapFeatureLayer[] GetFeatureLayers()
        {
            List<IMapFeatureLayer> featureLayers = new List<IMapFeatureLayer>();
            foreach (IMapLayer layer in MapFrame.Layers)
            {
                IMapFeatureLayer featureLayer = layer as IMapFeatureLayer;
                if (featureLayer != null)
                {
                    featureLayers.Add(featureLayer);
                }
            }
            return featureLayers.ToArray();
        }



        /// <summary>
        /// Removes any members from existing in the selected state
        /// </summary>
        public bool ClearSelection(out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (MapFrame == null) return false;
            return MapFrame.ClearSelection(out affectedArea);
        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="strict">The tight envelope to use for polygons</param>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings</param>
        /// <param name="mode">The selection mode</param>
        /// <param name="affectedArea">The envelope affected area</param>
        /// <returns>Boolean, true if any members were added to the selection</returns>
        public bool Select(IEnvelope tolerant, IEnvelope strict, SelectionModes mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (MapFrame == null) return false;
            return MapFrame.Select(tolerant, strict, mode, out affectedArea);

        }

        /// <summary>
        /// Inverts the selected state of any members in the specified region.
        /// </summary>
        /// <param name="strict">The tight envelope to use for polygons</param>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings</param>
        /// <param name="mode">The selection mode determining how to test for intersection</param>
        /// <param name="affectedArea">The geographic region encapsulating the changed members</param>
        /// <returns>boolean, true if members were changed by the selection process.</returns>
        public bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionModes mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (MapFrame == null) return false;
            return MapFrame.InvertSelection(tolerant, strict, mode, out affectedArea);

        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="strict">The tight envelope to use for polygons</param>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings</param>
        /// <param name="mode">The selection mode</param>
        /// <param name="affectedArea">The envelope affected area</param>
        /// <returns>Boolean, true if any members were added to the selection</returns>
        public bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionModes mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (MapFrame == null) return false;
            return MapFrame.UnSelect(tolerant, strict, mode, out affectedArea);

        }

        /// <summary>
        /// Opens a new layer and automatically adds it to the map.
        /// </summary>
        [Obsolete("Use AddLayers() with no parameters")]
        public virtual void OpenLayer()
        {
            AddLayers();
        }

        /// <summary>
        /// Allows the user to add a new layer to the map using an open file dialog to choose a layer file.
        /// Multi-select is an option, so this return a list with all the layers.
        /// </summary>
        public virtual List<IMapLayer> AddLayers()
        {
            List<IDataSet> sets = DataManager.DefaultDataManager.OpenFiles();
            if (sets == null || sets.Count == 0) return null;
            List<IMapLayer> results = new List<IMapLayer>();
            foreach (IDataSet set in sets)
            {
                if (set == null) return null;

                IFeatureSet fs = set as IFeatureSet;
                if (fs != null)
                {
                    results.Add(Layers.Add(fs));
                    continue;
                }
                IImageData id = set as IImageData;
                if (id != null)
                {
                    results.Add(Layers.Add(id));
                    continue;
                }
                IRaster r = set as IRaster;
                if (r != null)
                {
                    results.Add(Layers.Add(r));
                    continue;
                }
            }
            return results;
        }

        /// <summary>
        /// Adds the filename as a new layer to the map, returning the new layer.
        /// </summary>
        /// <param name="filename">The string filename of the layer to add</param>
        /// <returns>The IMapLayer added to the file.</returns>
        public virtual IMapLayer AddLayer(string filename)
        {
            return Layers.Add(filename);
        }

        /// <summary>
        /// This is so that if you have a basic map interface you can still prompt
        /// to add a layer, you just won't get an IMapLayer back.
        /// </summary>
        ILayer IBasicMap.AddLayer()
        {
            return AddLayer();
        }

        /// <summary>
        /// Uses the file dialog to allow selection of a filename for opening the 
        /// new layer, but does not allow multiple files to be added at once.
        /// </summary>
        /// <returns>The newly opened IMapLayer</returns>
        public virtual IMapLayer AddLayer()
        {
            return Layers.Add(DataManager.DefaultDataManager.OpenFile());
        }



        /// <summary>
        /// Allows a multi-select file dialog to add raster layers, applying a 
        /// filter so that only supported raster formats will appear.
        /// </summary>
        /// <returns>A list of the IMapRasterLayers that were opened.</returns>
        public virtual List<IMapRasterLayer> AddRasterLayers()
        {
            List<IMapRasterLayer> result = new List<IMapRasterLayer>();
            List<IRaster> sets = DataManager.DefaultDataManager.OpenRasters();
            foreach (IRaster raster in sets)
            {
                result.Add(Layers.Add(raster));  
            }
            return result;
        }

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster to the map as a layer, and returns the added layer.
        /// </summary>
        /// <returns>The IMapRasterLayer that was added</returns>
        public virtual IMapRasterLayer AddRasterLayer()
        {
            return Layers.Add(DataManager.DefaultDataManager.OpenRaster());
        }
     

        /// <summary>
        /// Allows a mult-select open file dialog to specify several filenames to add.
        /// Only files with supported vector extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapFeatureLayers</returns>
        public virtual List<IMapFeatureLayer> AddFeatureLayers()
        {
            List<IMapFeatureLayer> result = new List<IMapFeatureLayer>();
            List<IFeatureSet> sets = DataManager.DefaultDataManager.OpenVectors();
            foreach (IFeatureSet featureSet in sets)
            {
                result.Add(Layers.Add(featureSet));
            }
            return result;
        }

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster tot he map as a layer, and returns the added layer.
        /// </summary>
        /// <returns></returns>
        public virtual IMapFeatureLayer AddFeatureLayer()
        {
            return Layers.Add(DataManager.DefaultDataManager.OpenVector());
        }


        /// <summary>
        /// Allows a mult-select open file dialog to specify several filenames to add.
        /// Only files with supported image extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapImageLayers</returns>
        public virtual List<IMapImageLayer> AddImageLayers()
        {
            List<IMapImageLayer> result = new List<IMapImageLayer>();
            List<IImageData> sets = DataManager.DefaultDataManager.OpenImages();
            foreach (IImageData imageData in sets)
            {
                result.Add(Layers.Add(imageData));
            }
            return result;
        }

        /// <summary>
        /// Allows an open dialog without multi-select to specify a single filename
        /// to be added to the map as a new layer and returns the newly added layer.
        /// </summary>
        /// <returns>The layer that was added to the map.</returns>
        public virtual IMapImageLayer AddImageLayer()
        {
            return Layers.Add(DataManager.DefaultDataManager.OpenImage());
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device">The graphics device to print to</param>
        /// <param name="targetRectangle">the rectangle where the map content should be drawn.</param>
        public void Print(Graphics device, Rectangle targetRectangle)
        {
            MapFrame.Print(device, targetRectangle);
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device">The graphics device to print to</param>
        /// <param name="targetRectangle">the rectangle where the map content should be drawn.</param>
        /// <param name="targetEnvelope">the extents to print in the target rectangle</param>
        public void Print(Graphics device, Rectangle targetRectangle, IEnvelope targetEnvelope)
        {
            MapFrame.Print(device, targetRectangle, targetEnvelope);
        }

        /// <summary>
        /// This can be called any time, and is currently being used to capture
        /// the end of a resize event when the actual data should be updated.
        /// </summary>
        public virtual void ResetBuffer()
        {
            _geoMapFrame.ResetBuffer();
        }

        /// <summary>
        /// I think this is temporary
        /// </summary>
        public virtual void SaveLayer()
        {
            MapPointLayer gp = _geoMapFrame.Layers[0] as MapPointLayer;
            if (gp == null)
            {
                MessageBox.Show("Ensure that the top layer is the point layer you wish to save.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Shapefiles (*.shp)|*.shp";
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            gp.DataSet.SaveAs(sfd.FileName, false);
        }

        /// <summary>
        /// Instructs the map to change the perspective to include the entire drawing content, and
        /// in the case of 3D maps, changes the perspective to look from directly overhead.
        /// </summary>
        public void ZoomToMaxExtent()
        {
            Extents = Envelope;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating whether or not the sel
        /// </summary>
        public bool SelectionEnabled
        {
            get 
            {
                if (MapFrame == null) return false;
                return MapFrame.SelectionEnabled;
            }
            set 
            {
                if (MapFrame == null) return;
                MapFrame.SelectionEnabled = value;

            }
        }

        /// <summary>
        /// Instructs the map to clear the layers.
        /// </summary>
        public void ClearLayers()
        {
            if (MapFrame == null) return;
            MapFrame.Layers.Clear();
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether the Garbage collector should collect after drawing.
        /// This can be disabled for fast-action panning, but should normally be enabled.
        /// </summary>
        public bool CollectAfterDraw
        {
            get { return _collectAfterDraw; }
            set { _collectAfterDraw = value; }
        }

        /// <summary>
        /// Gets or sets the dictionary of tools built into this project
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use MapFunctions")]
        public Dictionary<string, IMapFunction> MapTools
        {
            get { return _mapFunctions; }
            set { _mapFunctions = value; }
        }

        /// <summary>
        /// Gets or sets the dictionary of tools built into this project
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, IMapFunction> MapFunctions
        {
            get { return _mapFunctions; }
            set { _mapFunctions = value; }
        }


        /// <summary>
        /// Obsolete
        /// </summary>
        [Obsolete("Use FunctionMode")]
        public FunctionModes CursorMode
        {
            get { return FunctionMode; }
            set { FunctionMode = value; }
        }


        /// <summary>
        /// Gets or sets the current tool mode.  This rapidly enables or disables specific tools to give
        /// a combination of functionality.  Selecting None will disable all the tools, which can be
        /// enabled manually by enabling the specific tool in the GeoTools dictionary.
        /// </summary>
        public FunctionModes FunctionMode
        {
            get { return _functionMode; }
            set
            {
                _functionMode = value;
                switch (_functionMode)
                {
                    case FunctionModes.Zoom:
                        try
                        {
                            MemoryStream ms = new MemoryStream(Images.cursorZoomIn1);
                            Cursor = new Cursor(ms);
                        }
                        catch
                        {
                            Cursor = Cursors.Arrow;
                        }
                        break;
                    case FunctionModes.ZoomOut:
                        try
                        {
                            MemoryStream ms = new MemoryStream(Images.cursorZoomOut1);
                            Cursor = new Cursor(ms);
                        }
                        catch
                        {
                            Cursor = Cursors.Arrow;
                        }
                        break;
                    case FunctionModes.Info:
                        Cursor = Cursors.Help;
                        break;
                    case FunctionModes.Label:
                        Cursor = Cursors.IBeam;
                        break;
                    case FunctionModes.None:
                        Cursor = Cursors.Arrow;
                        break;
                    case FunctionModes.Pan:
                        Cursor = Cursors.Hand;
                        break;
                    case FunctionModes.Select:
                        try
                        {
                            MemoryStream ms = new MemoryStream(Images.cursorSelect);
                            Cursor = new Cursor(ms);
                        }
                        catch
                        {
                            Cursor = Cursors.Hand;
                        }
                        break;
                    case FunctionModes.Measure:
                        Cursor = Cursors.Cross;
                        break;
                }
                foreach (IMapFunction gt in MapFunctions.Values)
                {
                    gt.Deactivate();
                    if (_functionMode == FunctionModes.None) continue;
                    ZoomFunction zf = gt as ZoomFunction;
                    if (zf != null)
                    {
                        zf.Activate();
                        continue;
                    }
                    if (_functionMode == FunctionModes.Pan)
                    {
                        PanFunction pf = gt as PanFunction;
                        if (pf != null)
                        {
                            pf.Activate();
                            continue;
                        }
                    }
                    if (_functionMode == FunctionModes.Info)
                    {
                        IdentifyFunction idF = gt as IdentifyFunction;
                        if (idF != null)
                        {
                            idF.Activate();
                            continue;
                        }
                    }
                    if (_functionMode == FunctionModes.Select)
                    {
                        SelectFunction sf = gt as SelectFunction;
                        if (sf != null)
                        {
                            sf.Activate();
                            continue;
                        }
                    }
                    if (_functionMode == FunctionModes.Label)
                    {
                        LabelSelectFunction ls = gt as LabelSelectFunction;
                        if (ls != null)
                        {
                            ls.Activate();
                            continue;
                        }
                    }
                    if (_functionMode == FunctionModes.Zoom)
                    {
                        ClickZoomFunction cls = gt as ClickZoomFunction;
                        if (cls != null)
                        {
                            cls.Activate();
                            continue;
                        }
                    }
                    if (_functionMode == FunctionModes.ZoomOut)
                    {
                        ZoomOutFunction zof = gt as ZoomOutFunction;
                        if (zof != null)
                        {
                            zof.Activate();
                            continue;
                        }
                    }
                    if (_functionMode == FunctionModes.Measure)
                    {
                        MeasureFunction ms = gt as MeasureFunction;
                        if(ms != null)
                        {
                            ms.Activate();
                            continue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the back buffer.  The back buffer should be in Format32bbpArgb bitmap.
        /// If it is not, then the image on the back buffer will be copied from the supplied image.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BufferedImage
        {
            get { return _geoMapFrame.BufferImage; }
            set { _geoMapFrame.BufferImage = value; }
        }

        /// <summary>
        /// If this is true, then point layers in the map will only draw points that are 
        /// more than 50% revealed.  This should increase drawing speed for layers that have
        /// a large number of points.
        /// </summary>
        public bool CollisionDetection
        {
            get { return _collisionDetection; }
            set { _collisionDetection = true; }
        }


        /// <summary>
        /// Gets the geographic bounds of all of the different data layers currently visible on the map.
        /// </summary>
        [Category("Bounds"),
         Description("Gets the geographic bounds of all of the different data layers currently visible on the map."),
        Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnvelope Envelope
        {
            get { return _geoMapFrame.Envelope; }
        }

        /// <summary>
        /// Gets or sets the geographic extents to show in the view.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnvelope Extents
        {
            get { return _geoMapFrame.ViewExtents; }
            set
            {
                IEnvelope ext = value.Copy();
                if (_geoMapFrame.ExtendBuffer)
                {
                    ext.ExpandBy(ext.Width, ext.Height);
                }
                _geoMapFrame.Extents = ext;


                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether or not
        /// the drawing layers should cache off-screen data to
        /// the buffer.  Panning will be much more elegant,
        /// but zooming, selecting and resizing will take a 
        /// performance penalty.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets a boolean that indicates whether or not zooming will also retrieve content immediately around the map so that panning pulls new content onto the screen.")]
        public bool ExtendBuffer
        {
            get { return MapFrame.ExtendBuffer; }
            set { MapFrame.ExtendBuffer = value; }
        }

        /// <summary>
        /// Gets or sets the MapFrame that should be displayed in this map.
        /// </summary>
        [Serialize("MapFrame"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMapFrame MapFrame
        {
            get { return _geoMapFrame; }
            set
            {
                OnExcludeMapFrame(_geoMapFrame);
                _geoMapFrame = value;
                OnIncludeMapFrame(_geoMapFrame);
                MapFrame.ResetBuffer();
                Invalidate();
            }
        }
        /// <summary>
        /// Handles removing event handlers for the map frame
        /// </summary>
        /// <param name="mapFrame"></param>
        protected virtual void OnExcludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null) return;
            mapFrame.UpdateMap -= MapFrame_UpdateMap;
            mapFrame.ScreenUpdated -= MapFrame_ScreenUpdated;
            mapFrame.ItemChanged -= MapFrame_ItemChanged;
            mapFrame.BufferChanged -= MapFrame_BufferChanged;
            mapFrame.SelectionChanged -= MapFrame_SelectionChanged;
            mapFrame.LayerAdded -= MapFrame_LayerAdded;
            if(Legend != null)
            {
                Legend.RemoveMapFrame(mapFrame, true);
            }
        }

        /// <summary>
        /// Handles adding new event handlers to the map frame
        /// </summary>
        /// <param name="mapFrame"></param>
        protected virtual void OnIncludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null)
            {
                if (Legend == null) return;
                Legend.RefreshNodes();
                return;
            }
            mapFrame.Parent = this;
            mapFrame.UpdateMap += MapFrame_UpdateMap;
            mapFrame.ScreenUpdated += MapFrame_ScreenUpdated;
            mapFrame.ItemChanged += MapFrame_ItemChanged;
            mapFrame.BufferChanged += MapFrame_BufferChanged;
            mapFrame.SelectionChanged += MapFrame_SelectionChanged;
            mapFrame.LayerAdded += MapFrame_LayerAdded;
            if (Legend == null) return;
            Legend.AddMapFrame(mapFrame);
        
        }

        void MapFrame_LayerAdded(object sender, LayerEventArgs e)
        {
           OnLayerAdded(sender, e);
        }

        /// <summary>
        /// Fires the LayerAdded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLayerAdded(object sender, LayerEventArgs e)
        {
            if (LayerAdded != null) LayerAdded(sender, e);
        }

        void MapFrame_SelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged();
        }

  

        /// <summary>
        /// Occurs after the selection is updated on all the layers
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            if (SelectionChanged != null) SelectionChanged(this, new EventArgs());
        }

        private void MapFrame_BufferChanged(object sender, ClipArgs e)
        {
            Rectangle view = MapFrame.View;
            foreach (Rectangle clip in e.ClipRectangles)
            {
                if (clip.IsEmpty == false)
                {
                    Rectangle mapClip = new Rectangle(clip.X - view.X, clip.Y - view.Y, clip.Width, clip.Height);
                    Invalidate(mapClip);
                }
            }
        }

      


        /// <summary>
        /// Gets or sets the collection of layers
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMapLayerCollection Layers
        {
            get
            {
                if (_geoMapFrame != null) return _geoMapFrame.Layers;
                return null;
            }
        }

        /// <summary>
        /// returns a functional list of the ILayer members.  This list will be
        /// separate from the actual list stored, but contains a shallow copy
        /// of the members, so the layers themselves can be accessed directly.
        /// </summary>
        /// <returns></returns>
        public List<ILayer> GetLayers()
        {
            List<ILayer> result = new List<ILayer>();
            if (_geoMapFrame == null) return result;
            return _geoMapFrame.Layers.Cast<ILayer>().ToList();

        }

        /// <summary>
        /// Gets or sets the projection.  This should reflect the projection of the first data layer loaded.
        /// Loading subsequent, but non-matching projections should throw an alert, and allow reprojection.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProjectionInfo Projection
        {
            get
            {
                return _geoMapFrame != null ? _geoMapFrame.Projection : null;
            }
            set
            {
                if(_geoMapFrame != null)
                {
                    _geoMapFrame.Projection = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the legend to use when showing the layers from this map
        /// </summary>
        public ILegend Legend
        {
            get { return _legend; }
            set
            {
                _legend = value;
                if (_legend != null)
                {
                    _legend.AddMapFrame(_geoMapFrame);
                }
            }
        }


        IFrame IBasicMap.MapFrame
        {
            get { return _geoMapFrame; }
        }

        /// <summary>
        /// Gets or sets the progress handler for this component.
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set { _progressHandler = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when this control tries to paint the background.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        /// <summary>
        /// Perform custom drawing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_geoMapFrame.IsPanning) return;

            Rectangle clip = e.ClipRectangle;
            if (clip.IsEmpty) clip = ClientRectangle;
            Bitmap stencil = new Bitmap(clip.Width, clip.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(stencil);
            Brush b = new SolidBrush(BackColor);
            g.FillRectangle(b, new Rectangle(0, 0, stencil.Width, stencil.Height));
            b.Dispose();
            Matrix m = new Matrix();
            m.Translate(-clip.X, -clip.Y);
            g.Transform = m;


            _geoMapFrame.Draw(new PaintEventArgs(g, e.ClipRectangle));

            MapDrawArgs args = new MapDrawArgs(g, clip, _geoMapFrame);
            foreach (IMapFunction tool in _mapFunctions.Values)
            {
                if (tool.Enabled) tool.Draw(args);
            }

            PaintEventArgs pe = new PaintEventArgs(g, e.ClipRectangle);
            base.OnPaint(pe);

            g.Dispose();

            e.Graphics.DrawImageUnscaled(stencil, clip.X, clip.Y);
            stencil.Dispose();
            if (_collectAfterDraw) GC.Collect();
            
        }

        /// <summary>
        /// Captures an image of whatever the contents of the back buffer would be at the size of the screen.
        /// </summary>
        /// <returns></returns>
        public Bitmap SnapShot()
        {
            Rectangle clip = ClientRectangle;
            Bitmap stencil = new Bitmap(clip.Width, clip.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(stencil);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, stencil.Width, stencil.Height));

            // Translate the buffer so that drawing occurs in client coordinates, regardless of whether
            // there is a clip rectangle or not.
            Matrix m = new Matrix();
            m.Translate(-clip.X, -clip.Y);
            g.Transform = m;

            _geoMapFrame.Draw(new PaintEventArgs(g, clip));
            g.Dispose();
            return stencil;
        }

        /// <summary>
        /// Creates a snapshot that is scaled to fit to a bitmap of the specified width.
        /// </summary>
        /// <param name="width">The width of the desired bitmap</param>
        /// <returns>A bitmap with the specified width</returns>
        public Bitmap SnapShot(int width)
        {
            int height = (int) (width*(MapFrame.Extents.Height/MapFrame.Extents.Width));
            Bitmap bmp = new Bitmap(height, width);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            _geoMapFrame.Print(g, new Rectangle(0, 0, width, height));
            g.Dispose();
            return bmp;
        }


        /// <summary>
        /// Fires the DoMouseDoubleClick method on the ActiveTools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            GeoMouseArgs args = new GeoMouseArgs(e, this);
            foreach (IMapFunction tool in _mapFunctions.Values)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseDoubleClick(args);
                    if (args.Handled) break;
                }
                
            }

            base.OnMouseDoubleClick(e);
        }

        /// <summary>
        /// Fires the OnMouseDown event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            GeoMouseArgs args = new GeoMouseArgs(e, this);
            foreach (IMapFunction tool in _mapFunctions.Values)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseDown(args);
                    if (args.Handled) break;
                }
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Fires the OnMouseUp event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            GeoMouseArgs args = new GeoMouseArgs(e, this);
            if (_resizing && e.Button == MouseButtons.Left)
            {
                _resizing = false;
                OnResized();
            }
            foreach (IMapFunction tool in _mapFunctions.Values)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseUp(args);
                    if (args.Handled) break;
                }
            }
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the OnResized event on the Active Tools
        /// </summary>
        protected virtual void OnResized()
        {
            MapFrame.ResetExtents();
            //_geoMapFrame.ResetBuffer();
            if (Resized == null) return;
            Resized(this, new EventArgs());
        }

        /// <summary>
        /// Fires the OnMouseMove event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            GeoMouseArgs args = new GeoMouseArgs(e, this);
            foreach (IMapFunction tool in _mapFunctions.Values)
            {
                
                if (tool.Enabled)
                {
                    tool.DoMouseMove(args);
                    if (args.Handled) break;
                }
            }
            if (GeoMouseMove != null)
            {
                GeoMouseMove(this, new GeoMouseArgs(e, this));
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Fires the OnMouseWheel event for the active tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            GeoMouseArgs args = new GeoMouseArgs(e, this);
            foreach (IMapFunction tool in _mapFunctions.Values)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseWheel(args);
                    if (args.Handled) break;
                }
            }
            base.OnMouseWheel(e);
        }

        /// <summary>
        /// This causes all of the datalayers to re-draw themselves to the buffer, rather than just drawing
        /// the buffer itself like what happens during "Invalidate"
        /// </summary>
        override public void Refresh()
        {
            _geoMapFrame.Initialize();
            base.Refresh();
            Invalidate();
        }


        /// <summary>
        /// Instructs the map to update the specified clipRectangle by drawing it to the back buffer.
        /// </summary>
        /// <param name="clipRectangle"></param>
        public void RefreshMap(Rectangle clipRectangle)
        {
            IEnvelope region = _geoMapFrame.BufferToProj(clipRectangle);
            _geoMapFrame.Invalidate(region);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFinishedRefresh(EventArgs e)
        {
            if (FinishedRefresh != null)
            {
                FinishedRefresh(this, e);
            }
        }


        /// <summary>
        /// Converts a single point location into an equivalent geographic coordinate
        /// </summary>
        /// <param name="position">The client coordiante relative to the map control</param>
        /// <returns>The geogrpahic ICoordinate interface</returns>
        public Coordinate PixelToProj(Point position)
        {
            //if (ExtendBuffer)
            //{
            //    Point loc = position;
            //    loc.X += Width;
            //    loc.Y += Height;
            //    return _geoMapFrame.PixelToProj(loc);
            //}
            return _geoMapFrame.PixelToProj(position);
        }

        /// <summary>
        /// Converts a rectangle in pixel coordinates relative to the map control into
        /// a geographic envelope.
        /// </summary>
        /// <param name="rect">The rectangle to convert</param>
        /// <returns>An IEnvelope interface</returns>
        public IEnvelope PixelToProj(Rectangle rect)
        {
            //if (ExtendBuffer)
            //{
            //    Rectangle r2 = rect;
            //    r2.X += Width;
            //    r2.Y += Height;
            //    return _geoMapFrame.PixelToProj(r2);
            //}
            return _geoMapFrame.PixelToProj(rect);
        }

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the 
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A System.Drawing.Point with the new location.</returns>
        public Point ProjToPixel(Coordinate location)
        {
            //if (ExtendBuffer)
            //{

            //    Point loc = _geoMapFrame.ProjToPixel(location);
            //    loc.X -= Width;
            //    loc.Y -= Height;
            //    return loc;
            //}
            return _geoMapFrame.ProjToPixel(location);
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="env">The geographic IEnvelope</param>
        /// <returns>A System.Drawing.Rectangle</returns>
        public Rectangle ProjToPixel(IEnvelope env)
        {
            //if (ExtendBuffer)
            //{

            //    Rectangle rect = _geoMapFrame.ProjToPixel(env);
            //    rect.X -= Width;
            //    rect.Y -= Height;
            //    return rect;
            //}
            return _geoMapFrame.ProjToPixel(env);
        }

        /// <summary>
        /// Zooms in one notch, so that the scale becomes larger and the features become larger.
        /// </summary>
        public void ZoomIn()
        {
            MapFrame.ZoomIn();
        }

        /// <summary>
        /// Zooms out one notch so that the scale becomes smaller and the features become smaller.
        /// </summary>
        public void ZoomOut()
        {
            MapFrame.ZoomOut();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            if (_geoMapFrame == null) return;
            _geoMapFrame.Invalidate();
            Invalidate(); // invalidate using the existing back buffer
            _resizing = true;
            base.OnResize(e);
        }

        #endregion
    }
}