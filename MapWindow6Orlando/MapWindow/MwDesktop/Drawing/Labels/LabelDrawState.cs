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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/21/2009 2:13:13 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Drawing
{


    /// <summary>
    /// LabelDrawState
    /// </summary>
    public class LabelDrawState
    {
        /// <summary>
        /// Creates a new instance of the LabelDrawState class where selected is false
        /// but visible is true.
        /// </summary>
        /// <param name="category">The category</param>
        public LabelDrawState(ILabelCategory category)
        {
            Category = category;
            Visible = true;
        }

        /// <summary>
        /// Creates a new instance of the LabelDrawState based on the specified parameters.
        /// </summary>
        /// <param name="category">The category</param>
        /// <param name="selected">Boolean, true if the label is selected</param>
        /// <param name="visible">Boolean, true if the label should be visible</param>
        public LabelDrawState(ILabelCategory category, bool selected, bool visible)
        {
            Category = category;
            Selected = selected;
            Visible = visible;
        }

        /// <summary>
        /// A boolean indicating whether or not this is selected.
        /// </summary>
        public bool Selected;

        /// <summary>
        /// A LabelCategory interface representing the drawing information for this label.
        /// </summary>
        public ILabelCategory Category;

        /// <summary>
        /// A boolean indicating whether the associated feature should be drawn.
        /// </summary>
        public bool Visible;


    }
}
