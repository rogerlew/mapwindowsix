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
using System.IO;
using MapWindow.Components;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Map
{


    /// <summary>
    /// GeoImageLayer
    /// </summary>
    public class MapRasterLayer : RasterLayer, IMapRasterLayer
    {
        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Private Variables

        private bool _isInitialized;
        private ImageData _baseImage;
        private Image _backBuffer; // draw to the back buffer, and swap to the stencil when done.
        private Image _stencil; // draw features to the stencil
        private IEnvelope _bufferExtent; // the geographic extent of the current buffer.
        private Rectangle _bufferRectangle;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a MapRasterLayer and the specified image data to use for rendering it.
        /// </summary>
        public MapRasterLayer(IRaster baseRaster, ImageData baseImage):base(baseRaster)
        {
            Configure(baseImage, baseRaster);
        }

        /// <summary>
        /// Creates a new instance of a Raster layer, and will create a "FallLeaves" image based on the
        /// raster values.
        /// </summary>
        /// <param name="raster">The raster to use</param>
        public MapRasterLayer(IRaster raster):base(raster)
        {
            string imageFile = Path.ChangeExtension(raster.Filename, ".bmp");
            if (File.Exists(imageFile)) File.Delete(imageFile);

            IRasterSymbolizer rs = new RasterSymbolizer();
            rs.Raster = raster;
            rs.Scheme.ApplyScheme(ColorSchemes.FallLeaves, raster);
            Bitmap bmp = new Bitmap(raster.NumColumns, raster.NumRows);
            bmp.Save(imageFile);
            raster.PaintColorSchemeToBitmap(rs, bmp, raster.ProgressHandler);
            bmp.Save(imageFile);
            bmp.Dispose();

            ImageData id = new ImageData(imageFile);
            id.Bounds.AffineCoefficients = raster.Bounds.AffineCoefficients;
            id.WorldFile.Affine = raster.Bounds.AffineCoefficients;
            Configure(id, raster);
        }

        

        private void Configure(ImageData baseImage, IRaster raster)
        {
            _baseImage = baseImage;
            LegendText = Path.GetFileNameWithoutExtension(raster.Filename);
            base.OnItemChanged();
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
            DrawWindows(args, regions, clipRects);
        }



        /// <summary>
        /// Indicates that the drawing process has been finalized and swaps the back buffer
        /// to the front buffer.
        /// </summary>
        public void FinishDrawing()
        {
            OnFinishDrawing();
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
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ShallowCopy]
        public Image BackBuffer
        {
            get { return _backBuffer; }
            set { _backBuffer = value; }
        }

        /// <summary>
        /// Gets the current buffer.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ShallowCopy]
        public Image Buffer
        {
            get { return _stencil; }
            set { _stencil = value; }
        }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ShallowCopy]
        public IEnvelope BufferEnvelope
        {
            get { return _bufferExtent; }
            set { _bufferExtent = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ShallowCopy]
        public Rectangle BufferRectangle
        {
            get { return _bufferRectangle; }
            set { _bufferRectangle = value; }
        }

        /// <summary>
        /// Gets or sets whether the image layer is initialized
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
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
        /// Indiciates that whatever drawing is going to occur has finished and the contents
        /// are about to be flipped forward to the front buffer.
        /// </summary>
        protected virtual void OnFinishDrawing()
        {

        }


        /// <summary>
        /// This ensures that when the symbolic content for the layer is updated that we re-load the image.
        /// </summary>
        protected override void OnItemChanged()
        {
            if (_baseImage == null) return;
            string imgFile = _baseImage.Filename;
            _baseImage.Open(imgFile);
            base.OnItemChanged();
        }
        

        /// <summary>
        /// Occurs when a new drawing is started, but after the BackBuffer has been established.
        /// </summary>
        protected virtual void OnStartDrawing()
        {

        }

        

       
       

        #endregion

        #region Private Methods

        /// <summary>
        /// This draws to the back buffer.  If the Backbuffer doesn't exist, this will create one.
        /// This will not flip the back buffer to the front.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="regions"></param>
        /// <param name="clipRectangles"></param>
        private void DrawWindows(MapArgs args, IList<IEnvelope> regions, IList<Rectangle> clipRectangles)
        {
            
            Graphics g;
            if (args.Device != null)
            {
                g = args.Device; // A device on the MapArgs is optional, but overrides the normal buffering behaviors.
            }
            else
            {
                if (_backBuffer == null) _backBuffer = new Bitmap(_bufferRectangle.Width, _bufferRectangle.Height);
                g = Graphics.FromImage(_backBuffer);
            }

            int numBounds = Math.Min(regions.Count, clipRectangles.Count);

            for (int i = 0; i < numBounds; i++)
            {
                Bitmap bmp = _baseImage.GetBitmap(regions[i], clipRectangles[i]);
                if(bmp != null)g.DrawImage(bmp, clipRectangles[i]);
            }
            if (args.Device == null) g.Dispose();
        }

        #endregion

      
    }
}
