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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/19/2009 11:49:19 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Drawing
{


    /// <summary>
    /// PatternTypes
    /// </summary>
    public enum PatternTypes
    {
        /// <summary>
        /// A pattern that gradually changes from one color to another
        /// </summary>
        Gradient,
        /// <summary>
        /// A pattern comprised of evenly spaced lines
        /// </summary>
        Line,
        /// <summary>
        /// A pattern comprised of point symbolizers
        /// </summary>
        Marker,
        /// <summary>
        /// A pattern comprised of a tiled texture
        /// </summary>
        Picture,
        /// <summary>
        /// A pattern comprised strictly of a fill color.
        /// </summary>
        Simple,



    }
}
