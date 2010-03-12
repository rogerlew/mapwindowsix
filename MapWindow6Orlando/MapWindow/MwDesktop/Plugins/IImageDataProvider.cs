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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 6:44:01 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using MapWindow.Main;
using MapWindow.Data;

namespace MapWindow.Plugins
{


    /// <summary>
    /// IImageProvider
    /// </summary>
    public interface IImageDataProvider : IDataProvider
    {

        /// <summary>
        /// Creates a new instance of an Image.
        /// </summary>
        /// <param name="filename">The string filename to use</param>
        /// <param name="width">The integer width in pixels</param>
        /// <param name="height">The integer height in pixels</param>
        /// <param name="inRam">Boolean, true if the entire contents should be stored in memory</param>
        /// <param name="progHandler">A Progress handler to use</param>
        /// <returns>A New IImageData object allowing access to the content of the image</returns>
        IImageData Create(string filename, int width, int height, bool inRam, IProgressHandler progHandler);

        /// <summary>
        /// Opens a new Image with the specified filename
        /// </summary>
        /// <param name="filename">The string file to open</param>
        /// <returns>An IImageData object</returns>
        new IImageData Open(string filename);



    }
}
