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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2009 3:09:31 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Windows.Forms;
using MapWindow.Map;
using MapWindow.Components;
using MapWindow.Data;
using MapWindow.Drawing;
namespace MapWindow.ShapeEditor
{


    /// <summary>
    /// ShapeEditorToolBar
    /// </summary>
    public class ShapeEditorToolStrip : ToolStrip
    {
        #region Private Variables

        private ToolStripButton cmdNew;
        private ToolStripButton cmdAddShape;
        private ToolStripButton cmdMoveVertex;
        private System.ComponentModel.IContainer components;
        private IMap _geoMap;
        private IFeatureLayer _activeLayer;
        private AddShapeFunction _addShapeFunction;
        private MoveVertexFunction _moveVertexFunction;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ShapeEditorToolBar
        /// </summary>
        public ShapeEditorToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Instructs this control to initiate cleanup where necessary
        /// </summary>
        public void Deactivate()
        {
            if (_addShapeFunction != null)
            {
                _addShapeFunction.Unload();
                _geoMap.MapTools.Remove(_addShapeFunction.Name);
            }
            if(_moveVertexFunction != null)
            {
                _moveVertexFunction.Unload();
                _geoMap.MapTools.Remove(_moveVertexFunction.Name);
            }
        }

        #endregion

        #region Properties



        #endregion
        /// <summary>
        /// Warning: Using the normal resource manager that is used by default by the program will cause
        /// the entire application to crash in mono.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmdNew = new System.Windows.Forms.ToolStripButton();
            this.cmdAddShape = new System.Windows.Forms.ToolStripButton();
            this.cmdMoveVertex = new System.Windows.Forms.ToolStripButton();
            this.SuspendLayout();
            // 
            // cmdNew
            // 
            this.cmdNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdNew.Image = ShapeEditorResources.NewShapefile.ToBitmap();
            this.cmdNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(23, 69);
            this.cmdNew.ToolTipText = "New";
            this.cmdNew.Click += new EventHandler(cmdNew_Click);
            //
            // cmdAddNew
            //
            this.cmdAddShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdAddShape.Image = ShapeEditorResources.NewShape.ToBitmap();
            this.cmdAddShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdAddShape.Name = "cmdAddShape";
            this.cmdAddShape.Size = new System.Drawing.Size(23, 69);
            this.cmdAddShape.ToolTipText = "Add Shape";
            this.cmdAddShape.Click += new EventHandler(cmdAddShape_Click);
            // 
            // cmdNew
            // 
            this.cmdMoveVertex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdMoveVertex.Image = ShapeEditorResources.move;
            this.cmdMoveVertex.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdMoveVertex.Name = "cmdMoveVertex";
            this.cmdMoveVertex.Size = new System.Drawing.Size(23, 69);
            this.cmdMoveVertex.ToolTipText = "Move Vertex";
            this.cmdMoveVertex.Click += new EventHandler(cmdMoveVertex_Click);

            cmdAddShape.Enabled = false;
            
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdNew,
            this.cmdAddShape,
            this.cmdMoveVertex
            });
            this.Size = new System.Drawing.Size(100, 72);
            this.ResumeLayout(false);
        }

        void cmdMoveVertex_Click(object sender, EventArgs e)
        {
            if (_geoMap == null) return;
            if (_activeLayer == null) return;
            if(_moveVertexFunction == null)
            {
                _moveVertexFunction = new MoveVertexFunction(_geoMap);
                _moveVertexFunction.Name = "MoveVertex";
            }
            if(_geoMap.MapTools.ContainsValue(_moveVertexFunction) == false)
            {
                _geoMap.MapTools.Add(_moveVertexFunction.Name, _moveVertexFunction);
            }
            _geoMap.FunctionMode = FunctionModes.None;
            _geoMap.Cursor = Cursors.Cross;
            _geoMap.MapFrame.LayerSelected += MapFrame_LayerSelected;
            _moveVertexFunction.Layer = _activeLayer;
            _moveVertexFunction.Activate();
        }

        void MapFrame_LayerSelected(object sender, MapWindow.Main.LayerSelectedEventArgs e)
        {
            if (e.IsSelected == false && e.Layer == _activeLayer)
            {
                if (_moveVertexFunction != null) _moveVertexFunction.UnSelectFeature();
                return;
            }
            IFeatureLayer fl = e.Layer as IFeatureLayer;
            _activeLayer = null;
            if (fl == null) return;
            _activeLayer = fl;
            if (_moveVertexFunction == null) return;
            _moveVertexFunction.UnSelectFeature();
            _moveVertexFunction.Layer = fl;
        }

        void cmdAddShape_Click(object sender, EventArgs e)
        {
            if (_geoMap == null) return;
            
            if(_geoMap.Layers.SelectedLayer != null)
            {
                _activeLayer = _geoMap.Layers.SelectedLayer as IFeatureLayer;
                if (_activeLayer == null) return;
            }
            
            if (_addShapeFunction == null)
            {
                _addShapeFunction = new AddShapeFunction(_geoMap);
                _addShapeFunction.Name = "AddShape";
            }
            if (_geoMap.MapFunctions.ContainsValue(_addShapeFunction) == false)
            {
                _geoMap.MapFunctions.Add(_addShapeFunction.Name, _addShapeFunction);
            }
            _geoMap.FunctionMode = FunctionModes.None;
            _geoMap.Cursor = Cursors.Hand;
            _addShapeFunction.FeatureSet = _activeLayer.DataSet;
            _addShapeFunction.Activate();
 
        }

        
        void cmdNew_Click(object sender, EventArgs e)
        {
            FeatureTypeDialog dlg = new FeatureTypeDialog();
            if (dlg.ShowDialog() != DialogResult.OK) return;
            FeatureSet fs = new FeatureSet(dlg.FeatureType);
            if(_geoMap.Projection != null) fs.Projection = _geoMap.Projection;
            fs.CoordinateType = dlg.CoordinateType;
            fs.IndexMode = false;
            IMapFeatureLayer layer = _geoMap.Layers.Add(fs);
            layer.EditMode = true;
            _geoMap.Layers.SelectedLayer = layer;
            layer.LegendText = _geoMap.Layers.UnusedName("New Layer");
         
        }

        #region Properties

        /// <summary>
        /// Gets or sets the 2D Geographic map to use with this feature editing toolkit.
        /// </summary>
        public IMap Map
        {
            get { return _geoMap; }
            set 
            { 
                _geoMap = value;
                if (_geoMap != null && _geoMap.Layers != null && _geoMap.Layers.SelectedLayer != null)
                {
                    SetActiveLayer(_geoMap.Layers.SelectedLayer);
                }
                if (_geoMap != null && _geoMap.Layers != null)
                {
                    _geoMap.Layers.LayerSelected += Layers_LayerSelected;
                }
            }
        }

        void Layers_LayerSelected(object sender, MapWindow.Main.LayerSelectedEventArgs e)
        {
            SetActiveLayer(e.Layer);
        }

        private void SetActiveLayer(ILayer layer)
        {
            IFeatureLayer fl = _geoMap.Layers.SelectedLayer as IFeatureLayer;
            if (fl != null)
            {
                _activeLayer = fl;
                cmdAddShape.Enabled = true;
            }
            else
            {
                cmdAddShape.Enabled = false;
            }
        }

        

        #endregion


    }
}
