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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/6/2010 11:56:43 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Map.Drawing.Images
{


    /// <summary>
    /// MapTiledImageLayer
    /// </summary>
    public class MapTiledImageLayer : TiledImageLayer
    {
        #region Private Variables




        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of MapTiledImageLayer
        /// </summary>
        public MapTiledImageLayer(ITiledImage baseImage):base(baseImage)
        {
            Configure(baseImage);
        }

        private void Configure(ITiledImage baseImage)
        {
            base.IsVisible = true;
            base.LegendText = System.IO.Path.GetFileName(baseImage.Filename);
            OnFinishedLoading();
        }

        #endregion

        #region Methods

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
        /// This draws to the back buffer.  If the Backbuffer doesn't exist, this will create one.
        /// This will not flip the back buffer to the front.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="regions"></param>
        /// <param name="clipRectangles"></param>
        private void DrawWindows(MapArgs args, IList<IEnvelope> regions, IList<Rectangle> clipRectangles)
        {
            Graphics g = args.Device;
            int numBounds = Math.Min(regions.Count, clipRectangles.Count);

            for (int i = 0; i < numBounds; i++)
            {
                Bitmap bmp = DataSet.GetBitmap(regions[i], clipRectangles[i].Size);
                if (bmp != null) g.DrawImage(bmp, clipRectangles[i]);
            }
            if (args.Device == null) g.Dispose();
        }


        #endregion

        #region Properties

        



        #endregion



    }
}
