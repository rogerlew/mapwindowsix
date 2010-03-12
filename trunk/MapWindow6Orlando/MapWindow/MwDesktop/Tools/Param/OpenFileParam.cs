//********************************************************************************************************
// Product Name: MapWindow.Tools.OpenFileParam
// Description:  Double Parameters returned by an ITool allows the tool to specify a range and default value
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
// The Initial Developer of this Original Code is Teva Veluppillai. Created in Feb , 2010.
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
    public class OpenFileParam : Parameter
    {
        string addedvalue;

        /// <summary>
        /// Creates a new Feature Set Parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public OpenFileParam(string name)
        {
            this.Name = name;
            this.ParamVisible = ShowParamInModel.No;
            this.ParamType = "MapWindow OpenFile Param";
        }

        /// <summary>
        /// Creates a new Feature Set Parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public OpenFileParam(string name, string value)
        {
            this.Name = name;
            this.ParamVisible = ShowParamInModel.No;
            this.ParamType = "MapWindow OpenFile Param";
            this.Value = value;
        }

        /// <summary>
        /// Generates a default instance of the data type so that tools have something to write too
        /// </summary>
        /// <param name="path"></param>
        public override void GenerateDefaultOutput(string path)
        {

            addedvalue = System.IO.Path.GetDirectoryName(path) + System.IO.Path.DirectorySeparatorChar + base.ModelName;
            this.Value = addedvalue;
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
            return (new OpenFileElement(this, addedvalue));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> DataSets)
        {
            return (new OpenFileElement(this, addedvalue));

        }
    }
}
