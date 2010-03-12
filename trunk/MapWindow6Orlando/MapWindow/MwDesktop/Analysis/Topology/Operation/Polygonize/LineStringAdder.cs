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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/6/2008 7:45:19 AM
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

namespace MapWindow.Analysis.Topology.Operation.Polygonize
{


    /// <summary>
    /// Add every linear element in a point into the polygonizer graph.
    /// </summary>
    public class LineStringAdder : IGeometryComponentFilter
    {

        #region Private Variables

        private Polygonizer container = null;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LineStringAdder
        /// </summary>
        public LineStringAdder(Polygonizer container)
        {
          this.container = container;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies the Filter to the specified geometry
        /// </summary>
        /// <param name="g"></param>
        public virtual void Filter(IGeometry g)
        {
            ILineString l = g as ILineString;
            if (l != null)
            {
                container.Add(l);
            }
        }
        
        #endregion

      


    }
}
