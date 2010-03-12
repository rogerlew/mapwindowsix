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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/2/2009 9:08:29 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.Linq;
namespace MapWindow.Geometries
{


    /// <summary>
    /// Extension Methods for the RectangleF class
    /// </summary>
    public static class RectangleFEM
    {
        /// <summary>
        /// Converts this RectangleF to an integer rectangle where the rectangle coordinates are
        /// calculated by rounding the values.  If the values can't be converted to an integer,
        /// this will return an empty rectangle.
        /// </summary>
        /// <param name="rect">The floating point rectangle.</param>
        /// <returns></returns>
        public static Rectangle ToRectangle(this RectangleF rect)
        {
            int x = (int)Math.Round(rect.X);
            int y = (int)Math.Round(rect.Y);
            int dx = (int)Math.Round(rect.Width);
            int dy = (int)Math.Round(rect.Height);
            return new Rectangle(x, y, dx, dy);
        }

       

    }
}
