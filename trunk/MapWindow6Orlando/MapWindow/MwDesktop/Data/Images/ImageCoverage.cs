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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/6/2010 11:21:10 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System.Collections.Generic;
using System.Drawing;
using MapWindow.Geometries;
using MapWindow.Serialization;

namespace MapWindow.Data
{


    /// <summary>
    /// TiledImage is a class for actually controlling the data in several tiles.  This does not supply direct accessors for
    /// modifying the bytes directly, and instead expects the user to edit the image on a tile-by-tile basis.  However,
    /// the GetBitmap method will produce a representation of the envelope scaled to the specified window.
    /// </summary>
    public class ImageCoverage : DataSet, IImageCoverage
    {
        #region Private Variables

        private List<IImageData> _images;
        private RasterBounds _bounds;
        [Serialize("FileName", ConstructorArgumentIndex = 0)]
        private string _filename;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TiledImage
        /// </summary>
        public ImageCoverage()
        {
            _images = new List<IImageData>();
        }

        #endregion

        #region Methods

       
        public virtual Bitmap GetBitmap(IEnvelope envelope, Size pixelSize)
        {
            Bitmap result = new Bitmap(pixelSize.Width, pixelSize.Height);
            Graphics g = Graphics.FromImage(result);
            foreach (ImageData image in _images)
            {
                IEnvelope bounds = envelope.Intersection(image.Envelope);
                
                Size ps = new Size((int)((double)pixelSize.Width * bounds.Width/envelope.Width),(int)((double)pixelSize.Height * bounds.Height/envelope.Height));
                int x = pixelSize.Width*(int)((bounds.X - envelope.X)/envelope.Width);
                int y = pixelSize.Height*(int) ((envelope.Y - bounds.Y)/envelope.Height);
                if(ps.Width > 0 && ps.Height > 0)
                {
                    Bitmap tile = image.GetBitmap(bounds, ps);
                    g.DrawImageUnscaled(tile, x, y);
                }
            }
            return result;
        }

        public IEnumerable<IImageData> GetImages()
        {
            return _images;
        }

        /// <summary>
        /// Cycles through each of the images and calls the open method on each one.
        /// </summary>
        public virtual void Open()
        {
            foreach (IImageData id in _images)
            {
                id.Open();
            }
            
        }

     

        /// <summary>
        /// Cycles through each of the images and calls the save method on each one
        /// </summary>
        public void Save()
        {
            foreach (IImageData id in _images)
            {
                id.Save();
            }
            
        }

        #endregion

        #region Properties

        
   
    

        public int Count
        {
            get { return _images.Count; }
        }


        public IEnvelope Envelope
        {
            get
            {
                IEnvelope env = new Envelope();
                foreach (IImageData tile in _images)
                {
                    env.ExpandToInclude(tile.Envelope);
                }
                return env;
            }
        }

    

        public virtual List<IImageData> Images
        {
            get { return _images; }
            set { _images = value; }
        }


        #endregion



    }
}
