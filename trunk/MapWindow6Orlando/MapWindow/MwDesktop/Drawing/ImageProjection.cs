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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/5/2009 12:50:00 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// geoGraphics
    /// </summary>
    public class ImageProjection: IProj
    {
        #region Private Variables

        private readonly IEnvelope _extents;
        private readonly Rectangle _destRectangle;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the ImageGraphics class for assisting with drawing.
        /// </summary>
        /// <param name="inExtent"></param>
        /// <param name="inDestRectangle"></param>
        public ImageProjection(IEnvelope inExtent, Rectangle inDestRectangle)
        {
            _extents = inExtent;
            _destRectangle = inDestRectangle;
        }

        #endregion

        #region Methods

       

        
        

        #endregion

        #region Properties

        /// <summary>
        /// The destination rectangle on the bitmap where drawing should occur.
        /// </summary>
        public Rectangle ImageRectangle
        {
            get { return _destRectangle; }
        }

        /// <summary>
        /// The geographic extents where drawing will take place.
        /// </summary>
        public IEnvelope GeographicExtents
        {
            get { return _extents; }
        }

 
        
       

        #endregion



    }
}
