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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Main
{
    /// <summary>
    /// The easiest way to fully implement IObject is simply to inherit System.Object
    /// </summary>
    public interface IObject
    {
        /// <summary>
        /// Determines whether the specified System.Object is equal to the current System.Object
        /// </summary>
        /// <param name="obj"><I>Obj</I>: The System.Object to compare with the current System.Object</param>
        /// <returns>Boolean, true if the specified System.Object is equal to the current Sysetm.Object; otherwise, false.</returns>
        bool Equals(object obj);
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// Ssytem.Object.GetHashCode() is suitable for use in
        /// hashing algorithms and data structures like a hash Table.
        /// </summary>
        /// <returns>A hash code for the current System.Object</returns>
        int GetHashCode();

        /// <summary>
        /// Gets the System.Type of the current instance.
        /// </summary>
        /// <returns>The System.Type instance that represetns the exact runtime type of the current instance</returns>
        System.Type GetType();


        /// <summary>
        /// Returns a System.String that represents the current System.Object
        /// </summary>
        /// <returns>A System.String that represetns the current System.Object</returns>
        string ToString();

        
    }
}
