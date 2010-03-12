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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/5/2009 11:15:47 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using MapWindow.Data;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// IIndexSelection
    /// </summary>
    public interface IIndexSelection : ISelection, ICollection<int>
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
        /// Adds a range of indices all at once.
        /// </summary>
        /// <param name="indices">The indices to add</param>
        void AddRange(IEnumerable<int> indices);

        /// <summary>
        /// Removes a set of indices all at once
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        bool RemoveRange(IEnumerable<int> indices);
    }
}
