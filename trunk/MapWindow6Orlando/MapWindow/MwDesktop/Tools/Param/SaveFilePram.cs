//********************************************************************************************************
// Product Name: MapWindow.Tools.FeatureSetParam
// Description:  DataTable parameter allows ITools to specify that they require a DataTable as input or 
// output
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
// The Initial Developer of this Original Code is Jiri Kadlec. Created in Feb, 2010.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MapWindow.Tools.DialogComponents;

namespace MapWindow.Tools.Param
{
    /// <summary>
    /// Raster parameter allows ITools to specify that they require a Raster data set as input
    /// </summary>
    public class SaveFilePram : Parameter
    {
        /// <summary>
        /// Creates a new Feature Set Parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public SaveFilePram(string name)
        {
            this.Name = name;
            this.ParamVisible = ShowParamInModel.Always;
            this.ParamType = "MapWindow File Param";
        }

        /// <summary>
        /// Creates a new Feature Set Parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public SaveFilePram(string name, string value)
        {
            this.Name = name;
            this.ParamVisible = ShowParamInModel.Always;
            this.ParamType = "MapWindow File Param";
            this.Value = value;
        }


        /// <summary>
        /// Specifies the value of the parameter (This is also the default value for input)
        /// </summary>
        public new string Value
        {
            get { return (string)base.Value; }
            set
            {
                base.Value = (object)value;
                this.DefaultSpecified = true;
            }
        }



        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> DataSets)
        {
            return (new SaveFileElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> DataSets)
        {
            return (new SaveFileElement(this));

        }
    }
}
