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

namespace MapWindow.Scrap
{
    /// <summary>
    /// This searches the local MapWindow\Plugins directory for plugins that 
    /// implement IFactory.  It then combines those plugins along with
    /// the preferred settings to give the complete file handling capabilities
    /// for the application.
    /// </summary>
    public static class DataCatalog
    {
        /// <summary>
        /// This will open either a valid IFeatureDataSource or an IRasterDataSource
        /// depending on 
        /// </summary>
        /// <param name="Filename"></param>
        /// <returns></returns>
        static IDataSource Open(string Filename)
        {
            return null;
        }
    }
}
