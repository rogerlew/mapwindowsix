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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/8/2010 10:28:33 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************



namespace MapWindow.Data
{


    /// <summary>
    /// ITiledImage
    /// </summary>
    public interface ITiledImage : IImageSet
    {
       

        #region Properties

        /// <summary>
        /// Gets or sets the bounds for this image
        /// </summary>
        RasterBounds Bounds
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the filename for this tiled image.
        /// </summary>
        string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the integer height in pixels for the combined image at its maximum resolution
        /// </summary>
        int Height
        {
            get;
        }

        /// <summary>
        /// Gets the stride, or total width in pixels of the byte data, which might not match exactly with the visible width.
        /// </summary>
        int Stride
        {
            get; set;
        }

        /// <summary>
        /// Gets the tile width 
        /// </summary>
        int TileWidth
        { 
            get;
        }

        /// <summary>
        /// Gets the tile height
        /// </summary>
        int TileHeight
        { 
            get;
        }

        /// <summary>
        /// Gets or sets the integer pixel width for the combined image at its maximum resolution.
        /// </summary>
        int Width
        {
            get;
        }


        /// <summary>
        /// Gets or sets the WorldFile for this set of tiles.
        /// </summary>
        WorldFile WorldFile
        {
            get; set;
        }

        #endregion



    }
}
