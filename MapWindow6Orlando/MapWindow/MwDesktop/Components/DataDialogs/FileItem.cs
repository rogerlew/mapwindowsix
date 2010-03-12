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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/14/2008 2:22:32 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Components
{


    /// <summary>
    /// FileItem
    /// </summary>
    public class FileItem : DirectoryItem
    {
        #region Private Variables

        private FileInfo _info;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FileItem
        /// </summary>
        public FileItem()
        {

        }

        /// <summary>
        /// Creates a new insteance of a FileItem associated with the specified path.
        /// </summary>
        /// <param name="path">Gets or sets a string path</param>
        public FileItem(string path)
            : base(path)
        {
            IDataManager defDM = DataManager.DefaultDataManager;
            DataFormats df = defDM.GetFileFormat(path);
            if (df == DataFormats.Vector)
            {
                FeatureTypes ft = defDM.GetFeatureType(path);
                switch (ft)
                {
                    case FeatureTypes.Polygon: ItemType = ItemTypes.Polygon; break;
                    case FeatureTypes.Line: ItemType = ItemTypes.Line; break;
                    case FeatureTypes.Point: ItemType = ItemTypes.Point; break;
                    case FeatureTypes.MultiPoint: ItemType = ItemTypes.Point; break;
                    default: ItemType = ItemTypes.Custom; break;
                }
            }
            if (df == DataFormats.Raster)
            {
                ItemType = ItemTypes.Raster;
            }
            if (df == DataFormats.Image)
            {
                ItemType = ItemTypes.Image;
            }
            if (df == DataFormats.Custom)
            {
                ItemType = ItemTypes.Custom;
            }
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the FileInfo
        /// </summary>
        public FileInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }

        #endregion



    }
}
