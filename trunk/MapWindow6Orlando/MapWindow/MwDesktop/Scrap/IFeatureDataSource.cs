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
using System.IO;
using MapWindow.Data;
namespace MapWindow.Scrap
{

    /// <summary>
    /// Data Sources act like Data Adapters, basically providing a host of "fast" methods for
    /// accessing specific information without necessarilly loading the whole thing at once.
    /// Drawing is done from the copy currently loaded, so "changes" will take place visually,
    /// even if the shapefile itself hasn't been updated.
    /// </summary>
    public interface IFeatureDataSource: IDataSource
    {

        

        /// <summary>
        /// Contains a list of all the layers opened from a single source.
        /// </summary>
        /// <exception cref="System.ApplicationException"></exception>
        IEventDictionary<int, IFeatureSet> FeatureLayers
        {
            // trying with just a get to see if we can change a value using a layer directly
            get;
        }

        // Data adapter like commands that can tollerate the high-speed format of data access/editing
        // The FeatureLayer itself just forwards instructions to the datasource


        
      


       
       

    }
}
