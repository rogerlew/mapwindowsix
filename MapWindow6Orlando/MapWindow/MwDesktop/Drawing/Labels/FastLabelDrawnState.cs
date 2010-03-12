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
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/3/2009 12:26:14 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************



namespace MapWindow.Drawing
{


    /// <summary>
    /// FastLabelDrawnState
    /// </summary>
    public class FastLabelDrawnState
    {
        /// <summary>
        /// Creates a new drawn state with the specified category
        /// </summary>
        /// <param name="category">The category</param>
        public FastLabelDrawnState(ILabelCategory category)
        {
            Category = category;
        }

        /// <summary>
        /// Gets or sets the category
        /// </summary>
        public ILabelCategory Category;

        /// <summary>
        /// Gets or sets whether the label is selected
        /// </summary>
        public bool Selected;
    }
}
