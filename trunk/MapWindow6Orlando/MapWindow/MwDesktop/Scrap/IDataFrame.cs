//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using MapWindow.Main;
using MapWindow.Geometries;
using MapWindow.Drawing;
namespace MapWindow.Scrap
{
    /// <summary>
    /// This interface stores a single extent window describing a view, and also contains
    /// the list of all the layers associated with that view.  The layers are ordered.
    /// </summary>
    public interface IDataFrame
    {
        /// <summary>
        /// The envelope that contains all of the layers for this data frame.  Essentially this would be
        /// the extents to use if you want to zoom to the world view.
        /// </summary>
        MapWindow.Geometries.IEnvelope Envelope
        {
            get;
        }

        
        // TO DO: Some kind of list of ILayer

        /// <summary>
        /// This describes the current viewing extents.  Changing this will change the data
        /// that appears when the data frame renders itself.
        /// </summary>
        MapWindow.Geometries.IEnvelope Extents
        {
            get;
            set;
        }

        


    }
}
