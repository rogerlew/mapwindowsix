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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2009 10:26:39 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Drawing
{


    /// <summary>
    /// IDrawnFeature
    /// </summary>
    public interface IDrawnState
    {
       

        #region Methods

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the integer chunk that this item belongs to.
        /// </summary>
        int Chunk
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scheme category
        /// </summary>
        IFeatureCategory SchemeCategory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean, true if this feature is currently selected
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether this feature is currently being drawn.
        /// </summary>
        bool IsVisible
        {
            get;
            set;
        }


        #endregion



    }
}
