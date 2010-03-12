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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/17/2008 1:37:48 PM
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

namespace MapWindow.Map
{


    /// <summary>
    /// IGeoLabelLayer
    /// </summary>
    public interface IMapLabelLayer : ILabelLayer, IMapLayer
    {


        /// <summary>
        /// Resolves ambiguity
        /// </summary>
        new void Invalidate();

        /// <summary>
        /// Gets or sets the feature layer that this label layer is attached to.
        /// </summary>
        new IMapFeatureLayer FeatureLayer
        {
            get;
            set;
        }
    }
}
