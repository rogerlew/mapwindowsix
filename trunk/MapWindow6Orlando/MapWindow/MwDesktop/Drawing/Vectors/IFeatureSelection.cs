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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2009 4:16:28 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using MapWindow.Data;

namespace MapWindow.Drawing
{


    /// <summary>
    /// IFilterCollection
    /// </summary>
    public interface IFeatureSelection : ICollection<IFeature>, ISelection
    {

        /// <summary>
        /// Clears the selection
        /// </summary>
        new void Clear();

        /// <summary>
        /// Gets the integer count of the members in the collection 
        /// </summary>
        new int Count
        {
            get;
        }

        /// <summary>
        /// Gets the drawing filter used by this collection.
        /// </summary>
        IDrawingFilter Filter
        {
            get;
        }


      
    }
}
