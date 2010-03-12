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
using MapWindow.Main;

namespace MapWindow.Scrap
{

    /// <summary>
    /// Provides accessors to the file handling and data elements without providing any 
    /// data access.
    /// </summary>
    public interface IDataSource
    {
       

        /// <summary>
        /// Gets a string DialogFilter that can be directly set to the DialogFilter property that automatically
        /// shows the valid file extensions that this FeatureDataSource can read.
        /// </summary>
        string DialogFilterRead
        {
            get;
        }

        /// <summary>
        /// Gets a string DialogFilter that can be directly set to the DialogFilter property that automatically
        /// shows the valid file extensions that this FeatureDataSource can write to.
        /// </summary>
        string DialogFilterWrite
        {
            get;
        }

        /// <summary>
        /// Returns a string matching the extension like ".bmp"
        /// </summary>
        string Extension
        {
            get;
        }

        /// <summary>
        /// The filename or connection to use for this datasource
        /// </summary>
        string FilenameOrConnection
        {
            get;
        }

        /// <summary>
        /// Gets an integer representing how many layers belong to this feature data source.
        /// </summary>
        int NumLayers
        {
            get;
        }

        /// <summary>
        /// Opens a the file or connection for read-write access
        /// </summary>
        /// <param name="FileOrConnection">Specifies a file path or else a database connection string</param>
        void Open(string FileOrConnection);
       


        /// <summary>
        /// Opens a data source 
        /// </summary>
        /// <param name="FileOrConnection">String, the file or connection to open</param>
        /// <param name="ReadOnly">Boolean, specifies whether the layer is editable </param>
        void Open(string FileOrConnection, bool ReadOnly);

        /// <summary>
        /// When a new datasource is opened, a boolean is passed specifying whether to
        /// open the datasource as readonly.  If the source is readonly, then this
        /// will be true, otherwise it will be false.
        /// </summary>
        bool ReadOnly
        {
            get;
        }


        /// <summary>
        /// Saves the information in the Layers provided by this datasource onto its existing file location
        /// </summary>
        void Save();

        /// <summary>
        /// Saves a datasource to the file.
        /// </summary>
        /// <param name="Filename">The string filename location to save to</param>
        /// <param name="Overwrite">Boolean, if this is true then it will overwrite a file of the existing name.</param>
        void SaveAs(string Filename, bool Overwrite);

    }
}
