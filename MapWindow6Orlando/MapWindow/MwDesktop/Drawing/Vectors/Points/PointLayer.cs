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
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Modified to do 3D in January 2008 by Ted Dunsford
//********************************************************************************************************
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using MapWindow.Data;
using MapWindow.Forms;
using MapWindow.Geometries;
using MapWindow.Main;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{
    
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles point drawing
    /// </summary>
    public class PointLayer: FeatureLayer, IPointLayer
    {
     


        #region Constructors

        /// <summary>
        /// This creates a new layer with an empty dataset configured to the point feature type.
        /// </summary>
        public PointLayer(): this(new FeatureSet(FeatureTypes.Point), null)
        {
            
        }

        /// <summary>
        /// Creates a new instance of a PointLayer without sending any status messages
        /// </summary>
        /// <param name="inFeatureSet">The IFeatureLayer of data values to turn into a graphical PointLayer</param>
        /// <exception cref="PointFeatureTypeException">Thrown if the featureSet FeatureType is not point or multi-point</exception>
        public PointLayer(IFeatureSet inFeatureSet)
            : this(inFeatureSet, null)
        {
            // this simply handles the default case where no status messages are requested
           
        }

        /// <summary>
        /// Creates a new instance of a PointLayer for storing and drawing points
        /// </summary>
        /// <param name="inFeatureSet">Any implentation of an IFeatureLayer</param>
        /// <param name="progressHandler">A valid implementation of the IProgressHandler interface.</param>
        /// <exception cref="PointFeatureTypeException">Thrown if the featureSet FeatureType is not point or multi-point</exception>
        public PointLayer(IFeatureSet inFeatureSet, IProgressHandler progressHandler)
            : base(inFeatureSet, null, progressHandler)
        {
            Configure(inFeatureSet);
           
        }

        /// <summary>
        /// Creates a new instance of a PointLayer for storing and drawing points
        /// </summary>
        /// <param name="inFeatureSet">Any implentation of an IFeatureLayer</param>
        /// <param name="progressHandler">A valid implementation of the IProgressHandler interface.</param>
        /// <param name="container">An IContainer to contain this layer</param>
        /// <exception cref="PointFeatureTypeException">Thrown if the featureSet FeatureType is not point or multi-point</exception>
        public PointLayer(IFeatureSet inFeatureSet, ICollection<ILayer> container, IProgressHandler progressHandler)
            : base(inFeatureSet, container, progressHandler)
        {
            Configure(inFeatureSet);
        }

        private void Configure(IFeatureSet inFeatureSet)
        {
            if (inFeatureSet.FeatureType != FeatureTypes.Point && inFeatureSet.FeatureType != FeatureTypes.MultiPoint)
            {
                throw new PointFeatureTypeException();
            }
            if (inFeatureSet.NumRows() == 0)
            {
                Envelope = new Envelope(new Coordinate(-180, -90), new Coordinate(180, 90));
            }
            if (inFeatureSet.NumRows() == 1)
            {
                Envelope = inFeatureSet.Envelope;
                Envelope.ExpandBy(10);
            }
            Symbology = new PointScheme();
        }

        
        

        #endregion

        #region Methods

   

       
        /// <summary>
        /// Handles drawing point images to the graphics object.  The extent handling should be already figured out
        /// by using the projection interface.
        /// </summary>
        /// <param name="g">The Graphics object to draw to.</param>
        /// <param name="p">The projection tool that specifies how geographic coordinates should transform to visual coordinates.</param>
        public override void DrawSnapShot(Graphics g, IProj p)
        {
            //IFeatureList fList = this.DataSet.Features;
            //double dX = Symbolizer.GeographicSize.XSize / 2;
            //double dY = Symbolizer.GeographicSize.YSize / 2;
            //Rectangle r = p.ImageRectangle;
            //if (Symbolizer.ScaleMode != ScaleModes.Geographic)
            //{
            //    System.Drawing.Point center = new System.Drawing.Point(r.Left + r.Width / 2, r.Top + r.Height / 2);
            //    int offsetX = Convert.ToInt32(center.X + Symbolizer.SymbolSize.Width);
            //    int offsetY = Convert.ToInt32(center.Y + Symbolizer.SymbolSize.Height);
            //    System.Drawing.Point offset = new System.Drawing.Point(offsetX, offsetY);
            //    ICoordinate c = p.PixelToProj(center);
            //    ICoordinate cOffset = p.PixelToProj(offset);
            //    dX = (cOffset.X - c.X) / 2;
            //    dY = (cOffset.Y - c.Y) / 2;
            //}
            //Bitmap symbol = new Bitmap(Symbolizer.PixelSize.Width, Symbolizer.PixelSize.Height);
            //Rectangle symbolRect = new Rectangle(0, 0, Symbolizer.PixelSize.Width, Symbolizer.PixelSize.Height);
            //Graphics symbolG = Graphics.FromImage(symbol);
            //Symbolizer.Draw(symbolG, symbolRect);
            //symbolG.Dispose();
            //IEnvelope env = p.GeographicExtents.Copy();
            //env.ExpandBy(dX, dY); // expand the envelope so we don't accidentally crop off points just outside the envelope.
            //foreach (IFeature feature in fList)
            //{
            //    ICoordinate[] coords = feature.BasicGeometry.Coordinates;
            //    foreach (ICoordinate center in coords)
            //    {
            //        if (env.Contains(center) == false) continue; // don't try to draw points where no part falls in the image
            //        Envelope imageBounds = new Envelope(center.X - dX, center.X + dX, center.Y - dY, center.Y + dY);
            //        Rectangle imageRect = p.ProjToPixel(imageBounds);
            //        g.DrawImage(symbol, imageRect);
            //    }
            //}

        }


        #endregion


        #region Properties

        

        /// <summary>
        /// Gets or sets the pointSymbolizer characteristics to use for the selected features.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the symbolic characteristics to use for the selected features."),
        TypeConverter(typeof(GeneralTypeConverter)),
        Editor(typeof(PointSymbolizerEditor), typeof(UITypeEditor)),
        ShallowCopy]
        public new IPointSymbolizer SelectionSymbolizer
        {
            get { return base.SelectionSymbolizer as IPointSymbolizer; }
            set
            {
                base.SelectionSymbolizer = value;
            }
        }
       

       
        /// <summary>
        /// Gets or sets the symbolic characteristics for this layer.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the symbolic characteristics for this layer."),
        TypeConverter(typeof(GeneralTypeConverter)),
        Editor(typeof(PointSymbolizerEditor), typeof(UITypeEditor)),
        ShallowCopy]
        public new IPointSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPointSymbolizer;
            }
            set
            {
                base.Symbolizer = value;
            }
        }


        

        /// <summary>
        /// Gets the currently applied scheme.  Because setting the scheme requires a processor intensive
        /// method, we use the ApplyScheme method for assigning a new scheme.  This allows access
        /// to editing the members of an existing scheme directly, however.
        /// </summary>
        [Category("Appearance"), Description("Gets the currently applied scheme."),
        TypeConverter(typeof(GeneralTypeConverter)),
        Editor(typeof(PointSchemePropertyGridEditor), typeof(UITypeEditor)),
        Serialize("Symbology")]
        public new IPointScheme Symbology
        {
            get { return base.Symbology as IPointScheme; }
            set { base.Symbology = value; }
        }


        #endregion

        

        #region Static Methods

        /// <summary>
        /// Attempts to create a new PointLayer using the specified file.  If the filetype is not
        /// does not generate a point layer, an exception will be thrown.
        /// </summary>
        /// <param name="filename">A string filename to create a point layer for.</param>
        /// <param name="progressHandler">Any valid implementation of IProgressHandler for receiving progress messages</param>
        /// <returns>A PointLayer created from the specified filename.</returns>
        public new static IPointLayer OpenFile(string filename, IProgressHandler progressHandler)
        {
            ILayer fl = Components.LayerManager.DefaultLayerManager.OpenLayer(filename, progressHandler);
            return fl as PointLayer;
           
        }

        /// <summary>
        /// Attempts to create a new PointLayer using the specified file.  If the filetype is not
        /// does not generate a point layer, an exception will be thrown.
        /// </summary>
        /// <param name="filename">A string filename to create a point layer for.</param>
        /// <returns>A PointLayer created from the specified filename.</returns>
        public new static IPointLayer OpenFile(string filename)
        {
            IFeatureLayer fl = Components.LayerManager.DefaultLayerManager.OpenVectorLayer(filename);
            return fl as PointLayer;

        }




        #endregion

    }
}
