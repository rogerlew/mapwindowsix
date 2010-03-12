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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2008 2:46:23 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Geometries;
using MapWindow.Components;
namespace MapWindow.Drawing
{


    /// <summary>
    /// GeoImageLayer
    /// </summary>
    public class ImageLayer : Layer, IImageLayer
    {
        

        #region Private Variables

        private IImageSymbolizer _symbolizer;



        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the ImageLayer by opening the specified filename
        /// </summary>
        /// <param name="filename"></param>
        public ImageLayer(string filename)
        {
            DataSet = DataManager.DefaultDataManager.OpenImage(filename);
            Configure();
        }

        /// <summary>
        /// Creates a new instance of the ImageLayer by opening the specified filename, relaying progress to the
        /// specified handler, and automatically adds the new layer to the specified container.
        /// </summary>
        /// <param name="filename">The filename to open</param>
        /// <param name="progressHandler">A ProgressHandler that can receive progress updates</param>
        /// <param name="container">The layer list that should contain this image layer</param>
        public ImageLayer(string filename, IProgressHandler progressHandler, ICollection<ILayer> container)
            : base(container)
        {
            DataSet = DataManager.DefaultDataManager.OpenImage(filename, progressHandler);
            Configure();
        }

        /// <summary>
        /// Creates a new instance of the image layer by opening the specified filename and relaying progress to the specified handler.
        /// </summary>
        /// <param name="filename">The filename to open</param>
        /// <param name="progressHandler">The progressHandler</param>
        public ImageLayer(string filename, IProgressHandler progressHandler)
        {
            DataSet = DataManager.DefaultDataManager.OpenImage(filename, progressHandler);
            Configure();
        }

        /// <summary>
        /// Creates a new instance of GeoImageLayer
        /// </summary>
        public ImageLayer(IImageData baseImage)
        {
            DataSet = baseImage;
            Configure();
        }

        /// <summary>
        /// Creates a new instance of a GeoImageLayer
        /// </summary>
        /// <param name="baseImage">The image to draw as a layer</param>
        /// <param name="container">The Layers collection that keeps track of the image layer</param>
        public ImageLayer(IImageData baseImage, ICollection<ILayer> container)
            : base(container)
        {
            DataSet = baseImage;
            Configure();
        }

        private void Configure()
        {
            base.IsVisible = true;
            Envelope = DataSet.Bounds.Envelope;
            base.LegendText = System.IO.Path.GetFileName(DataSet.Filename);
        }

        #endregion

        /// <summary>
        /// Gets or sets the underlying data for this object
        /// </summary>
        public new IImageData DataSet
        {
            get { return base.DataSet as IImageData; }
            set { base.DataSet = value; }
        }

        /// <summary>
        /// Gets the geographic bounding envelope for the image
        /// </summary>
        public override IEnvelope Envelope
        {
            get
            {
                if (DataSet == null) return null;
                return DataSet.Bounds.Envelope;
            }
            protected set
            {
                if (DataSet == null) return;
                DataSet.Bounds.Envelope = value;
            }
        }

        /// <summary>
        /// Gets or sets a class that has some basic parameters that control how the image layer
        /// is drawn.
        /// </summary>
        public IImageSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            set { _symbolizer = value; }
        }

        /// <summary>
        /// Gets or sets the image being drawn by this layer
        /// </summary>
        public IImageData Image
        {
            get { return DataSet; }
            set { DataSet = value; }
        }


    }
}
