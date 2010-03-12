//********************************************************************************************************
// Product Name: MapWindow.Tools.DataSetArray
// Description:  DataSetArray used to create an array of DataSets with their associated name
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
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWindow.Data;

namespace MapWindow.Tools
{
    /// <summary>
    /// Used to create arrays of data sets with an associated name to be passed into Tools to populate their dialog boxes
    /// </summary>
    public class DataSetArray
    {
        private string _name;
        private IDataSet _dataSet;

        /// <summary>
        /// Creates an instance of a simple object that holds a name and a dataset
        /// </summary>
        /// <param name="Name"> The name of the DataSet in this object</param>
        /// <param name="DataSet">The IDataSet in this object</param>
        public DataSetArray(string Name, IDataSet DataSet)
        {
            _name = Name;
            _dataSet = DataSet;
        }

        /// <summary>
        /// The name of the DataSet in this object
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The IDataSet in this object
        /// </summary>
        public IDataSet DataSet
        {
            get { return _dataSet; }
            set { _dataSet = value; }
        }

        /// <summary>
        /// Returns the Name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _name;
        }
   }
}
