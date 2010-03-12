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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:47:11 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Legacy
{

    /// <summary>
    /// Interface for manipulating the PreviewMap
    /// </summary>
    public interface IPreviewMap
    {
        /// <summary>
        /// Gets/Sets the back color
        /// </summary>
        System.Drawing.Color BackColor { get; set; }

        /// <summary>
        /// Gets/Sets the Picture to be displayed
        /// </summary>
        System.Drawing.Image Picture { get; set; }
        /// <summary>
        /// Gets/Sets the color used for the LocatorBox
        /// </summary>
        System.Drawing.Color LocatorBoxColor { get; set; }

        /// <summary>
        /// Tells the PreviewMap to rebuild itself by getting new data from the main view
        /// </summary>
        void GetPictureFromMap();

        /// <summary>
        /// Tells the PreviewMap to rebuild itself by getting new data from the main view (current extents).
        /// </summary>
        void Update();

        /// <summary>
        /// Tells the PreviewMap to rebuild itself by getting new data from the main view.
        /// <param name="UpdateExtents">Update from full extent or current view?</param>
        /// </summary>
        void Update(PreviewExtentModes UpdateExtents);

        /// <summary>
        /// Loads a picture into the PreviewMap from a specified file
        /// </summary>
        /// <param name="Filename">The path to the file to load</param>
        /// <returns>true on success, false on failure</returns>
        bool GetPictureFromFile(string Filename);
    }


    
}
