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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 2:01:53 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Legacy
{


    /// <summary>
    /// This interface is used to access the list of shapes that were found during an Identify function call.
    /// </summary>
    public interface IdentifiedShapes
    {
        /// <summary>
        /// Returns the number of shapes that were identified.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns the shape index of an identified that is stored at the position 
        /// specified by the Index parameter.
        /// </summary>
        int this[int Index] { get; }
    }
}
