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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/8/2010 10:01:52 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using MapWindow.Geometries;
using MapWindow.Projections;

namespace MapWindow.Data
{


    /// <summary>
    /// TiledImage is a special kind of image coverage where the images specifically form tiles that
    /// are adjacent and perfectly aligned to represent a larger image.
    /// </summary>
    public class TiledImage : ITiledImage, IImageData
    {
        #region Private Variables


        private TileCollection _tiles;
        private RasterBounds _bounds;
        private string _filename;
        private string _typeName;
        private string _name;
        private ProjectionInfo _projection;
        private SpaceTimeSupport _support;
        private int _numBands;
        private WorldFile _worldFile;
        private int _stride;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the TiledImage where the filename is specified.
        /// This doesn't actually open the file until the Open method is called.
        /// </summary>
        /// <param name="filename"></param>
        public TiledImage(string filename)
        {
            _filename = filename;
        }

        /// <summary>
        /// Creates a new instance of TiledImage
        /// </summary>
        public TiledImage(int width, int height)
        {
            Init(width, height);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Even if this TiledImage has already been constructed, we can initialize the tile collection later.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Init(int width, int height)
        {
            _tiles = new TileCollection(width, height);
            _typeName = "TileImage";
            _support = SpaceTimeSupport.Spatial;
            

            
        }
        /// <summary>
        /// Calls a method that calculates the propper image bounds for each of the extents of the tiles,
        /// given the affine coefficients for the whole image.
        /// </summary>
        /// <param name="affine"> x' = A + Bx + Cy; y' = D + Ex + Fy</param>
        public void SetTileBounds(double[] affine)
        {
            _tiles.SetTileBounds(affine);
        }

        public virtual Bitmap GetBitmap(IEnvelope envelope, Size pixelSize)
        {
            Bitmap result = new Bitmap(pixelSize.Width, pixelSize.Height);
            Graphics g = Graphics.FromImage(result);
            IEnumerable<IImageData> images = GetImages();
            foreach (ImageData image in images)
            {
                IEnvelope bounds = envelope.Intersection(image.Envelope);

                Size ps = new Size((int)(pixelSize.Width * bounds.Width / envelope.Width), (int)(pixelSize.Height * bounds.Height / envelope.Height));
                int x = pixelSize.Width * (int)((bounds.X - envelope.X) / envelope.Width);
                int y = pixelSize.Height * (int)((envelope.Y - bounds.Y) / envelope.Height);
                if (ps.Width > 0 && ps.Height > 0)
                {
                    Bitmap tile = image.GetBitmap(bounds, ps);
                    g.DrawImageUnscaled(tile, x, y);
                }
            }
            return result;
        }

        /// <summary>
        /// This should be overridden with custom file handling
        /// </summary>
        public virtual void Open()
        {
            
        }

        public virtual void Close()
        {
            if (Tiles == null) return;
            foreach (IImageData image in Tiles)
            {
                if(image != null)image.Close();
            }
        }

        /// <summary>
        /// Gets or sets the array of tiles used for this 
        /// </summary>
        public virtual IImageData[,] Tiles
        {
            get { return _tiles.Tiles; }
            set { _tiles.Tiles = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected TileCollection TileCollection
        {
            get { return _tiles; }
            set { _tiles = value; }
        }


        #endregion

        #region Properties

        public RasterBounds Bounds
        {
            get
            {
                return _bounds;
            }
            set
            {
                _bounds = value;
            }
        }


        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public int Height
        {
            get
            {
                return _tiles.Height;
            }
            set
            {
                
            }
            
        }


        public int Stride
        {
            get { return _stride; }
            set { _stride = value; }
        }
        
        public int Width
        {
            get
            {
                return _tiles.Width;
            }
            set
            {
                
            }
        }

        #endregion




        #region ITiledImage Members

       

       

        public int TileWidth
        {
            get { return _tiles.TileWidth; }
        }

        public int TileHeight
        {
            get { return _tiles.TileHeight; }
        }

        #endregion

        #region IImageSet Members

        public int Count
        {
            get { return _tiles.NumTiles; }
        }

        public IEnvelope Envelope
        {
            get 
            {
                IEnvelope env = new Envelope();
                foreach (IImageData image in _tiles)
                {
                    env.ExpandToInclude(image.Envelope);
                }
                return env;
            }
        }

        public IEnumerable<IImageData> GetImages()
        {
            return _tiles;
        }

        #endregion

        #region IDataSet Members

        public string TypeName
        {
            get
            {
                return _typeName;
            }
            set
            {
                _typeName = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public ProjectionInfo Projection
        {
            get
            {
                return _projection;
            }
            set
            {
                _projection = value;
            }
        }

        public SpaceTimeSupport SpaceTimeSupport
        {
            get
            {
                return _support;
            }
            set
            {
                _support = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of bands
        /// </summary>
        public int NumBands
        {
            get { return _numBands; }
            set { _numBands = value; }
        }

        /// <summary>
        /// Gets or sets the WorldFile for this set of tiles.
        /// </summary>
        public WorldFile WorldFile
        {
            get { return _worldFile; }
            set { _worldFile = value; }
        }
       

        #endregion

        #region IImageData Members

        public int BytesPerPixel
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public void CopyValues(IImageData source)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            foreach (IImageData tile in _tiles)
            {
                tile.Dispose();
            }
        }

        public Bitmap GetBitmap()
        {
            throw new System.NotImplementedException();
        }

        public Bitmap GetBitmap(IEnvelope envelope, Rectangle window)
        {
            return GetBitmap(envelope, new Size(window.Width, window.Height));
        }

        public Color GetColor(int row, int column)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Open(string filename)
        {
            throw new System.NotImplementedException();
        }

        public void ReadBytes()
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void SaveAs(string filename)
        {
            throw new System.NotImplementedException();
        }

        public void SetColor(int row, int column, Color col)
        {
            throw new System.NotImplementedException();
        }

        public void WriteBytes()
        {
            throw new System.NotImplementedException();
        }

        IEnvelope IImageData.Envelope
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        

        public byte[] Values
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion
    }
}
