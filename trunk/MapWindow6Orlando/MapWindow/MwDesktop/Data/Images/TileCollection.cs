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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/8/2010 10:16:19 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.Collections.Generic;


namespace MapWindow.Data
{


    /// <summary>
    /// TileCollection
    /// </summary>
    public class TileCollection : IEnumerable<IImageData>
    {
        #region Private Variables

        private readonly int _tileWidth;
        private readonly int _tileHeight;
        private int _width;
        private int _height;
        private IImageData[,] _tiles;

        #endregion

       
        /// <summary>
        /// Enumerates the collection of tiles
        /// </summary>
        public class TileCollectionEnumerator: IEnumerator<IImageData>
        {
            private int _row;
            private int _col;
            private readonly IImageData[,] _tiles;

            /// <summary>
            /// Creates a new instance of hte TileCollectionEnumerator
            /// </summary>
            /// <param name="parent">The parent tileCollection</param>
            public TileCollectionEnumerator(TileCollection parent)
            {
                _tiles = parent.Tiles;
                _row = 0;
                _col = -1;
            }

            public IImageData  Current
            {
                get { return _tiles[_row, _col]; }
            }

            public void  Dispose()
            {
 	            // Does nothing
            }

            object  System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool  MoveNext()
            {
                do
                {
                    _col++;
                    if (_col > _tiles.GetUpperBound(1))
                    {
                        _row++;
                        _col = 0;
                    }
                    if (_row > _tiles.GetUpperBound(0)) return false;
                } while (_tiles[_row, _col] == null);
                return true;
            }

            public void  Reset()
            {
                _row = 0;
                _col = 0;
            }


        }

        #region Constructors

        /// <summary>
        /// Creates a new instance of TileCollection
        /// </summary>
        public TileCollection(int width, int height)
        {
            _tileWidth = 5000;
            _tileHeight = 5000;
            _width = width;
            _height = height;
            if (_width < _tileWidth) _tileWidth = width;
            if (_height < _tileHeight) _tileHeight = height;
            _tiles = new IImageData[NumTilesTall(),NumTilesWide()];
            
        }

        /// <summary>
        /// Calls a method that calculates the propper image bounds for each of the extents of the tiles,
        /// given the affine coefficients for the whole image.
        /// </summary>
        /// <param name="affine"> x' = A + Bx + Cy; y' = D + Ex + Fy</param>
        public void SetTileBounds(double[] affine)
        {
            double[] tileAffine = new double[6];
            for (int i = 0; i < 6; i++)
            {
                tileAffine[i] = affine[i];
            }
                if (_tiles == null) return;
            for (int row = 0; row < NumTilesTall(); row++)
            {
                for (int col = 0; col < NumTilesWide(); col++)
                {
                    int h = GetTileHeight(row);
                    int w = GetTileWidth(col);
                    // The rotation terms are the same, but the top-left values need to be shifted.
                    tileAffine[0] = affine[0] + affine[1]*col*_tileWidth + affine[2]*row*_tileHeight;
                    tileAffine[3] = affine[3] + affine[4]*col*_tileWidth + affine[5]*row*_tileHeight;
                    _tiles[row, col].Bounds = new RasterBounds(h, w, tileAffine);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the width of the tile
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public int GetTileWidth(int col)
        {
            if (col < NumTilesWide() - 1) return _tileWidth;
            return Width - (NumTilesWide() - 1)*_tileWidth;
        }

        /// <summary>
        /// Gets the height of the tile
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public int GetTileHeight(int row)
        {
            if (row < NumTilesTall() - 1) return _tileHeight;
            return Height - (NumTilesTall() - 1)*_tileHeight;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the integer height in pixels for the combined image at its maximum resolution
        /// </summary>
        public int Height
        {
            get { return _height;}
            set { _height = value;}
        }

        /// <summary>
        /// Gets or sets the integer pixel width for the combined image at its maximum resolution.
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the 2D array of tiles
        /// </summary>
        public IImageData[,] Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        /// <summary>
        /// The width of the standard sized tile (not counting the remainder)
        /// </summary>
        public int TileWidth
        {
            get { return _tileWidth; }
        }

        /// <summary>
        /// The height of the standard sized tile, not counting the remainder.
        /// </summary>
        public int TileHeight
        {
            get { return _tileHeight; }
        }

        /// <summary>
        /// The total number of tiles
        /// </summary>
        public int NumTiles
        {
            get
            {
                return NumTilesWide()*NumTilesTall();     
            }
        }

        /// <summary>
        /// Gets the number of tiles in the X direction
        /// </summary>
        /// <returns></returns>
        public int NumTilesWide()
        {
            return (int) Math.Ceiling(Width/(double) TileWidth);
        }

        /// <summary>
        /// Gets the integer number of tiles in the Y direction
        /// </summary>
        /// <returns></returns>
        public int NumTilesTall()
        {
            return (int) Math.Ceiling(Height/(double) TileHeight);
        }


        #endregion



    
        #region IEnumerable<IImageData> Members

        public IEnumerator<IImageData>  GetEnumerator()
        {
            return new TileCollectionEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator  System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


    }
}
