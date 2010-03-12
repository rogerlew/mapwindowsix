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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/21/2009 5:05:56 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// Extension methods for the IProj interface
    /// </summary>
    public static class ProjEM
    {
    
        #region Methods

        /// <summary>
        /// Converts a single point location into an equivalent geographic coordinate
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="position">The client coordiante relative to the map control</param>
        /// <returns>The geogrpahic ICoordinate interface</returns>
        public static Coordinate PixelToProj(this IProj self, System.Drawing.Point position)
        {
            double x = Convert.ToDouble(position.X);
            double y = Convert.ToDouble(position.Y);
            if (self != null && self.GeographicExtents != null)
            {
                x = x*self.GeographicExtents.Width/self.ImageRectangle.Width + self.GeographicExtents.Minimum.X;
                y = self.GeographicExtents.Maximum.Y - y*self.GeographicExtents.Height/self.ImageRectangle.Height;
            }
            return new Coordinate(x, y, 0.0);
        }

        /// <summary>
        /// Converts a rectangle in pixel coordinates relative to the map control into
        /// a geographic envelope.
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="rect">The rectangle to convert</param>
        /// <returns>An IEnvelope interface</returns>
        public static IEnvelope PixelToProj(this IProj self, Rectangle rect)
        {
            System.Drawing.Point tl = new System.Drawing.Point(rect.X, rect.Y);
            System.Drawing.Point br = new System.Drawing.Point(rect.Right, rect.Bottom);
            Coordinate topLeft = PixelToProj(self, tl);
            Coordinate bottomRight = PixelToProj(self, br);
            return new Envelope(topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y);
        }

        /// <summary>
        /// Projects all of the rectangles int the specified list of rectangles into geographic regions.
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="clipRects">The clip rectangles</param>
        /// <returns>A List of IEnvelope geographic bounds that correspond to the specified clip rectangles.</returns>
        public static List<IEnvelope> PixelToProj(this IProj self, List<Rectangle> clipRects)
        {
            List<IEnvelope> result = new List<IEnvelope>();
            foreach (Rectangle r in clipRects)
            {
                result.Add(PixelToProj(self, r));
            }
            return result;
        }

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the 
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A System.Drawing.Point with the new location.</returns>
        public static System.Drawing.Point ProjToPixel(this IProj self, Coordinate location)
        {
            if (self.GeographicExtents.Width == 0 || self.GeographicExtents.Height == 0) return System.Drawing.Point.Empty;
            int x = Convert.ToInt32((location.X - self.GeographicExtents.Minimum.X) * (self.ImageRectangle.Width / self.GeographicExtents.Width));
            int y = Convert.ToInt32((self.GeographicExtents.Maximum.Y - location.Y) * (self.ImageRectangle.Height / self.GeographicExtents.Height));
            return new System.Drawing.Point(x, y);
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="env">The geographic IEnvelope</param>
        /// <returns>A System.Drawing.Rectangle</returns>
        public static Rectangle ProjToPixel(this IProj self, IEnvelope env)
        {
            Coordinate tl = new Coordinate(env.Minimum.X, env.Maximum.Y);
            Coordinate br = new Coordinate(env.Maximum.X, env.Minimum.Y);
            System.Drawing.Point topLeft = ProjToPixel(self, tl);
            System.Drawing.Point bottomRight = ProjToPixel(self, br);
            return new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        }

        /// <summary>
        /// Translates all of the geographic regions, forming an equivalent list of rectangles.
        /// </summary>
        /// <param name="self">This IProj</param>
        /// <param name="regions">The list of geographic regions to project</param>
        /// <returns>A list of pixel rectangles that describe the specified region</returns>
        public static List<Rectangle> ProjToPixel(this IProj self, List<IEnvelope> regions)
        {
            List<Rectangle> result = new List<Rectangle>();
            foreach (IEnvelope region in regions)
            {
                if(region == null) continue;
                result.Add(ProjToPixel(self, region));
            }
            return result;
        }

        /// <summary>
        /// Calculates an integer length distance in pixels that corresponds to the double
        /// length specified in the image.
        /// </summary>
        /// <param name="self">The IProj that this describes</param>
        /// <param name="distance">The double distance to obtain in pixels</param>
        /// <returns>The integer distance in pixels</returns>
        public static double ProjToPixel(this IProj self, double distance)
        {
            return (distance * self.ImageRectangle.Width / self.GeographicExtents.Width);
        }



        #endregion

        #region Properties



        #endregion



    }
}
