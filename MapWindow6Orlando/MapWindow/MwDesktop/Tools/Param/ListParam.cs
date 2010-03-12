//********************************************************************************************************
// Product Name: MapWindow.Tools.ListParam
// Description:  Double Parameter returned by an ITool allows the tool to specify a range and default value
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
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Tools.Param
{
    /// <summary>
    /// List of strings parameter returned by an ITool allows the tool to specify a list of values and a default
    /// </summary>
    public class ListParam : Parameter 
    {
        #region variables

        private List<string> _valueList;

        #endregion

        /// <summary>
        /// Creates a new list parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public ListParam(string name)
        {
            this.Name = name;
            this.ParamVisible = ShowParamInModel.No;
            this.ParamType = "MapWindow List Param";
            this.Value = -1;
            _valueList = new List<string>();
        }

        /// <summary>
        /// Creates a new list parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="valueList">The list of string values to poluate the combo box</param>
        public ListParam(string name, List<string> valueList)
        {
            this.Name = name;
            this.ParamVisible = ShowParamInModel.No;
            this.ParamType = "MapWindow List Param";
            this.Value = -1;
            _valueList = valueList;
        }

        /// <summary>
        /// Creates a new list parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="valueList">The list of string values to poluate the combo box</param>
        /// <param name="value">The default item in the list</param>
        public ListParam(string name, List<string> valueList, int value)
        {
            this.Name = name;
            this.ParamVisible = ShowParamInModel.No;
            this.ParamType = "MapWindow List Param";
            this.Value = value;
            _valueList = valueList;
            this.DefaultSpecified = true;
        }

        /// <summary>
        /// Gets or sets the index of the list
        /// </summary>
        public new int Value
        {
            get { return (int)base.Value; }
            set { base.Value = (object)value; }
        }

        /// <summary>
        /// Gets or sets the list of items in the valuelist
        /// </summary>
        public List<string> ValueList
        {
            get { return _valueList; }
            set { _valueList = value; }
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> DataSets)
        {
            return (new ListElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> DataSets)
        {
            return (new ListElement(this));

        }
    }
}
