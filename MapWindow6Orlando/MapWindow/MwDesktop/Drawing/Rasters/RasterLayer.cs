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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2008 3:42:21 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MapWindow.Components;
using MapWindow.Data;
using MapWindow.Geometries;
using MapWindow.Main;
using MapWindow.Projections;
namespace MapWindow.Drawing
{


    /// <summary>
    /// A raster layer describes using a single raster, and the primary application will be using this as a texture.
    /// </summary>
    public class RasterLayer : Layer, IRasterLayer
    {
       

        

        #region Private Variables

       

        private IRasterSymbolizer _symbolizer;
       
        private string _legendText;
   

        #endregion

        #region Constructors

        /// <summary>
        /// Opens the specified filename using the layer manager.
        /// </summary>
        /// <param name="filename"></param>
        public RasterLayer(string filename)
        {
            IRaster r = DataManager.DefaultDataManager.OpenRaster(filename);
            Configure(r);
        }

        /// <summary>
        /// Opens the specified filename and automatically creates a raster that can be used by this raster layer.
        /// </summary>
        /// <param name="filename">The string filename to use in order to open the file</param>
        /// <param name="inProgressHandler">The progress handler to show progress messages</param>
        public RasterLayer(string filename, IProgressHandler inProgressHandler)
        {
            base.ProgressHandler = inProgressHandler;
            IRaster r = DataManager.DefaultDataManager.OpenRaster(filename, true, inProgressHandler);
            Configure(r);
        }

       

        /// <summary>
        /// Creates a new raster layer using the progress handler defined on the DefaultLayerManager
        /// </summary>
        /// <param name="raster">The raster to create this layer for</param>
        public RasterLayer(IRaster raster)
        {
            Configure(raster);
        }

      
        /// <summary>
        /// Creates a new instance of RasterLayer
        /// </summary>
        /// <param name="raster">The Raster</param>
        /// <param name="inProgressHandler">The Progress handler for any status updates</param>
        public RasterLayer(IRaster raster, IProgressHandler inProgressHandler)
        {
            base.ProgressHandler = inProgressHandler;
            Configure(raster);
        }


        private void Configure(IRaster raster)
        {
            DataSet = raster;
            _symbolizer = new RasterSymbolizer(this);
            _symbolizer.ColorSchemeUpdated += _symbolizer_SymbologyUpdated;
            
            
        }


        #endregion

        #region Methods

  

       


        
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            // Grid is 

            if (disposing)
            {
                DataSet = null;
                _symbolizer = null;
               
            }
          
        }


        /// <summary>
        /// Creates a bmp texture and saves it to the same filename as the raster, but with a bmp ending.
        /// This also generates a bpw world file for the texture.
        /// </summary>
        public void WriteBitmap()
        {
            WriteBitmap(Path.ChangeExtension(DataSet.Filename, ".bmp"), DataSet.ProgressHandler);
        }

        /// <summary>
        /// Creates a bmp texture and saves it to the same filename as the raster but with a bmp ending.
        /// This also generates a bpw world file for the texture.
        /// </summary>
        /// <param name="progressHandler">An implementation of IProgressHandler to recieve status messages</param>
        public void WriteBitmap(IProgressHandler progressHandler)
        {
            WriteBitmap(Path.ChangeExtension(DataSet.Filename, ".bmp"), progressHandler);
        }

        /// <summary>
        ///  Creates a bmp texture and saves it to the specified filename.  The filename should end in bmp.
        ///  This also generates a bpw world file for the texture.
        /// </summary>
        /// <param name="filename">The string filename to write to</param>
        public void WriteBitmap(string filename)
        {
            WriteBitmap(filename, DataSet.ProgressHandler);
        }

        /// <summary>
        /// Creates a bmp texture and saves it to the specified filename.  The filename should end in bmp.
        /// This also generates a bpw world file for the texture.
        /// </summary>
        /// <param name="filename">The string filename to write to</param>
        /// <param name="progressHandler">The progress handler for creating a new bitmap.</param>
        /// <exception cref="MapWindow.Main.FileCantBeDeletedException">The file could not be deleted because it is in use by another application.</exception>
        public void WriteBitmap(string filename, IProgressHandler progressHandler)
        {

            try
            {
                if (File.Exists(filename)) File.Delete(filename); // we expect textures to get overwritten a lot, so just delete this
            }
            catch
            {
                throw new FileCantBeDeletedException(filename);
            }
            Bitmap bmp = new Bitmap(DataSet.NumColumns, DataSet.NumRows, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bmp.Save(filename); // for some reason lockbits doesn't work unless we save a copy first.

            if (_symbolizer.DrapeVectorLayers == false)
            {
                // Generate the colorscheme, modified by hillshading if that hillshading is used all in one pass
              
                DataSet.DrawToBitmap(Symbolizer, bmp, progressHandler);
            }
            else
            {
                // work backwards.  when we get to this layer do the colorscheme.


                // First, use this raster and its colorschme to drop the background
                DataSet.PaintColorSchemeToBitmap(Symbolizer, bmp, progressHandler);

                

                // Set up a graphics object with a transformation pre-set so drawing a geographic coordinate
                // will draw to the correct location on the bitmap
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
               
                IEnvelope extents = DataSet.Bounds.Envelope;
                Rectangle target = new Rectangle(0, 0, bmp.Width, bmp.Height);
                ImageProjection ip = new ImageProjection(extents, target);
               


                // Cycle through each layer, and as long as it is not this layer, draw the bmp
                foreach (ILegendItem layer in GetParentItem().LegendItems)
                {
                    // Temporarilly I am only interested in doing this for vector datasets
                    IFeatureLayer fl = layer as IFeatureLayer;
                    if (fl == null) continue;

                    fl.DrawSnapShot(g, ip);
                    
                }

                if (Symbolizer.ShadedRelief.IsUsed)
                {
                    // After we have drawn the underlying texture, apply a hillshade if it is requested
                    Symbolizer.PaintShadingToBitmap(bmp, progressHandler);
                }
            }
            bmp.Save(Path.ChangeExtension(DataSet.Filename, ".bmp"), System.Drawing.Imaging.ImageFormat.Bmp);
            bmp.Dispose();


            // Create a world file so that the bmp itself becomes geo-referenced
            string worldFile = Path.ChangeExtension(DataSet.Filename, ".bpw");
            if (File.Exists(worldFile)) File.Delete(worldFile);
            FileStream fs = new FileStream(worldFile, FileMode.CreateNew, FileAccess.Write);
            StreamWriter tw = new StreamWriter(fs);

            // Affine Coefficients
            // X = 0 + 1 * column + 2 * row
            // Y = 3 + 4 * column + 5 * row
            double[] aff = DataSet.Bounds.AffineCoefficients;
            tw.WriteLine(aff[1].ToString());
            tw.WriteLine(aff[2].ToString());
            tw.WriteLine(aff[4].ToString());
            tw.WriteLine(aff[5].ToString());
            tw.WriteLine(aff[0].ToString());
            tw.WriteLine(aff[3].ToString());
            tw.Close(); // closes both tw and fs

            // World file organization (six values written in string format on separate lines)
            // -----------------------
            // dx
            // rotationX
            // rotationY
            // dy
            // X - position
            // Y - position
            Symbolizer.Validate();
            // Symbolizer.ColorSchemeHasChanged = false;
            //OnColorSchemeUpdated();
            OnInvalidate(this, new EventArgs());
            OnItemChanged();
        }
      

        /// <summary>
        /// Handles the situation for exporting the layer as a new source.
        /// </summary>
        protected override void OnExportData()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = DataManager.DefaultDataManager.RasterWriteFilter;
            if (sfd.ShowDialog() != DialogResult.OK) return;
            DataSet.SaveAs(sfd.FileName);
        }



        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the boundaries of the raster.
        /// </summary>
        [Category("Bounds"),
         Description("Shows more detail about the geographic position of the raster."),
         Editor(typeof(Forms.PropertyGridEditor),typeof(System.Drawing.Design.UITypeEditor)),
         TypeConverter(typeof(Forms.GeneralTypeConverter))]
        public virtual IRasterBounds Bounds
        {
            get
            {
                if(DataSet != null)return DataSet.Bounds;
                return null;
            }
            set
            {
                if(DataSet != null) DataSet.Bounds = value;
            }

        }

        /// <summary>
        /// Gets the geographic height of the cells for this raster (North-South)
        /// </summary>
        [Category("Raster Properties"), Description("The geographic width of each cell in this raster.")]
        public virtual double CellHeight
        {
            get
            {
                if(DataSet != null) return DataSet.CellHeight;
                return 0;
            }
        }

       
        /// <summary>
        /// Gets the geographic width of the cells for this raster (East-West)
        /// </summary>
        [Category("Raster Properties"), Description("The geographic width of each cell in this raster.")]
        public virtual double CellWidth
        {
            get
            {
                if(DataSet != null)return DataSet.CellWidth;
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets whether this should appear as checked in the legend.  This is also how the
        /// layer will
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool Checked
        {
            get
            {
                if (_symbolizer == null) return true;
                return _symbolizer.IsVisible;
            }
            set
            {
                if (_symbolizer == null) return;
                if (value != _symbolizer.IsVisible)
                {
                    IsVisible = value;


                }

            }
        }

        /// <summary>
        /// Gets the data type of the values in this raster.
        /// </summary>
        [Category("Raster Properties"), Description("The numeric data type of the values in this raster.")]
        public Type DataType
        {
            get
            {
                if(DataSet != null)return DataSet.DataType;
                return null;
            }
        }

     

      

        /// <summary>
        /// Gets the eastern boundary of this raster.
        /// </summary>
        [Category("Bounds"), Description("The East boundary of this raster.")]
        public virtual double East
        {
            get
            {
                if (DataSet != null)
                {
                    if (DataSet.Bounds != null)
                    {
                        return DataSet.Bounds.Right();
                    }
                }
                return 0;
            }
        }



        /// <summary>
        /// This is a conversion factor that is required in order to convert the elevation units into the same units as the geospatial projection for the latitude and logitude values of the grid.
        /// </summary>
        [DisplayName("Elevation Factor"), Category("Symbology"), Description("This is a conversion factor that is required in order to convert the elevation units into the same units as the geospatial projection for the latitude and logitude values of the grid.")]
        public virtual float ElevationFactor
        {
            get
            {
                if(_symbolizer != null)return _symbolizer.ElevationFactor;
                return 0f;
            }
            set
            {
                if (_symbolizer == null) return;
                if (_symbolizer.ElevationFactor != value)
                {
                    _symbolizer.ElevationFactor = value;

                }
            }
        }

        /// <summary>
        /// Obtains an envelope
        /// </summary>
        /// <returns></returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override IEnvelope Envelope
        {
            get
            {
                if (DataSet != null)
                {
                    if (DataSet.Bounds != null)
                    {
                        return DataSet.Bounds.Envelope;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the exageration beyond normal elevation values.  A value of 1 is normal elevation, a vlaue of 0 would be flat,
        /// while a value of 2 would be twice the normal elevation.  This applies to the three-dimensional rendering and is
        /// not related to the shaded relief pattern created by the texture.
        /// </summary>
        [DisplayName("Extrusion"), Category("Symbology"), Description("the exageration beyond normal elevation values.  A value of 1 is normal elevation, a vlaue of 0 would be flat, while a value of 2 would be twice the normal elevation.  This applies to the three-dimensional rendering and is not related to the shaded relief pattern created by the texture.")]
        public virtual float Extrusion
        {
            get
            {
                if (_symbolizer != null) return _symbolizer.Extrusion;
                return 0f;
            }
            set
            {
                if (_symbolizer == null) return;
                if (_symbolizer.Extrusion != value)
                {
                    _symbolizer.Extrusion = value;
                }
            }
        }

       

        /// <summary>
        /// Gets the filename where this raster is saved.
        /// </summary>
        [Category("Raster Properties"), Description("The filename of this raster.")]
        public virtual string Filename
        {
            get
            {
                if(DataSet != null)return DataSet.Filename;
                return "No Raster Specified";
            }
        }

  

        /// <summary>
        /// If this is false, then the drawing function will not render anything.
        /// Warning!  This will also prevent any execution of calculations that take place
        /// as part of the drawing methods and will also abort the drawing methods of any
        /// sub-members to this IRenderable.
        /// </summary>
        [Category("Symbology"), DisplayName("Visible"),
         Description("Controls whether or not this layer will be drawn.")]
        public override bool IsVisible
        {
            get 
            {
                if (_symbolizer != null) return _symbolizer.IsVisible;
                return false;
            }
            set 
            { 
                _symbolizer.IsVisible = value;
                OnVisibleChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets or sets the complete list of legend items contained within this legend item
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override IEnumerable<ILegendItem> LegendItems
        {
            get 
            {
                if (_symbolizer == null) return null;
                return _symbolizer.Scheme.Categories.Cast<ILegendItem>();
            }
        }

        

       
        /// <summary>
        /// The text that will appear in the legend 
        /// </summary>
        [Category("Appearance"),DisplayName("Caption"),Description(" The text that will appear in the legend")]
        public override string LegendText
        {
            get 
            {
                if(_legendText == null && DataSet != null) return DataSet.Name;
                return _legendText;
            }
            set { _legendText = value; }
        }


        /// <summary>
        /// Gets the maximum value of this raster.  If this is an elevation raster, this is also the top.
        /// </summary>
        [Category("Raster Properties"), Description("The maximum value of this raster.  If this is an elevation raster, this is also the top.")]
        public virtual double Maximum
        {
            get
            {
                if(DataSet != null)return DataSet.Maximum;
                return 0;
            }
        }

        /// <summary>
        /// Gets the minimum value of this raster.  If this is an elevation raster, this is also the bottom.
        /// </summary>
        [Category("Raster Properties"), Description("The minimum value of this raster.  If this is an elevation raster, this is also the bottom.")]
        public virtual double Minimum
        {
            get
            {
                if(DataSet != null)return DataSet.Minimum;
                return 0;
            }
        }

        /// <summary>
        /// Gets the value that is used when no actual data exists for the specified location.
        /// </summary>
        [Category("Raster Properties"), Description("The value that is used when no actual data exists for the specified location.")]
        public virtual double NoDataValue
        {
            get
            {
                if(DataSet != null) return DataSet.NoDataValue;
                return 0;
            }
        }

        /// <summary>
        /// Gets the northern boundary of this raster.
        /// </summary>
        [Category("Bounds"), Description("The North boundary of this raster.")]
        public virtual double North
        {
            get
            {
                if (DataSet != null)
                {
                    if (DataSet.Bounds != null)
                    {
                        return DataSet.Bounds.Top();
                    }
                }
                return 0;
            }
        }

       

        /// <summary>
        /// Gets the number of bands in this raster.
        /// </summary>
        [DisplayName("Number of Bands"), Category("Raster Properties"), Description("Gets the number of bands in this raster.")]
        public virtual int NumBands
        {
            get
            {
                if (DataSet != null) return DataSet.NumBands;
                return 0;
            }
        }
        /// <summary>
        /// Gets the number of columns in this raster.
        /// </summary>
        [DisplayName("Number of Columns"), Category("Raster Properties"), Description("Gets the number of columns in this raster.")]
        public virtual int NumColumns
        {
            get
            {
                if (DataSet != null) return DataSet.NumColumns;
                return 0;
            }
        }
        /// <summary>
        /// Gets the number of rows in this raster.
        /// </summary>
        [DisplayName("Number of Rows"), Category("Raster Properties"), Description("Gets the number of rows in this raster.")]
        public virtual int NumRows
        {
            get
            {
                if (DataSet != null) return DataSet.NumRows;
                return 0;
            }
        }

        /// <summary>
        /// Gets the geographic projection that this raster is using.
        /// </summary>
        [Category("Raster Properties"), Description("The geographic projection that this raster is using.")]
        public virtual ProjectionInfo Projection
        {
            get
            {
                if (DataSet != null) return DataSet.Projection;
                return null;
            }
        }


        /// <summary>
        /// Gets or sets the underlying dataset
        /// </summary>
        [Category("Data"), DisplayName("Raster Properties"),
        TypeConverter(typeof(Forms.GeneralTypeConverter)),
        Editor(typeof(Forms.PropertyGridEditor), typeof(System.Drawing.Design.UITypeEditor)),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Description("This gives access to more comprehensive information about the underlying data."),
        ShallowCopy]
        public new IRaster DataSet
        {
            get { return base.DataSet as IRaster; }
            set { base.DataSet = value; }
        }

      

        /// <summary>
        /// Gets or sets the collection of symbolzier properties to use for this raster.
        /// </summary>
        [Category("Symbology"), DisplayName("Color Scheme"),
        Editor(typeof(Forms.RasterColorSchemeEditor),typeof(System.Drawing.Design.UITypeEditor)),
        TypeConverter(typeof(Forms.GeneralTypeConverter)),
        ShallowCopy]
        public IRasterSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            set 
            {
                if (_symbolizer == value) return;
                _symbolizer = value;
                _symbolizer.ParentLayer = this;
                _symbolizer.Scheme.SetParentItem(this);
                _symbolizer.ColorSchemeUpdated += _symbolizer_SymbologyUpdated;
            }
        }

       

        /// <summary>
        /// Gets the southern boundary of this raster.
        /// </summary>
        [Category("Bounds"), Description("The South boundary of this raster.")]
        public virtual double South
        {
            get
            {
                if (DataSet != null)
                {
                    if (DataSet.Bounds != null)
                    {
                        return DataSet.Bounds.Bottom();
                    }
                }
                return 0;
            }
        }


        /// <summary>
        /// Gets the western boundary of this raster.
        /// </summary>
        [Category("Bounds"), Description("The West boundary of this raster.")]
        public virtual double West
        {
            get
            {
                if (DataSet != null)
                {
                    if (DataSet.Bounds != null)
                    {
                        return DataSet.Bounds.Left();
                    }
                }
                return 0;
            }
        }

        #endregion

 

        #region Event Handlers

        void _symbolizer_SymbologyUpdated(object sender, EventArgs e)
        {
            OnItemChanged();
        }

        #endregion

    }
}
