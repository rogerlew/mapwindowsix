//********************************************************************************************************
// Product Name: MapWindow.Tools.Parameters
// Description:  Parameters passed back from a ITool to the toolbox manager
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
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
    /// EventHandler delegate for the tool parameter when a value is changed.
    /// </summary>
    /// <param name="sender"></param>
    public delegate void EventHandlerValueChanged(Parameter sender);

    /// <summary>
    /// This is the base class for the parameter array to be passed into a ITool
    /// </summary>
    public abstract class Parameter : ICloneable
    {
        #region variables

        private string _name;
        private string _paramType;
        private object _dataValue;
        private System.Drawing.Bitmap _helpImage;
        private string _helpText = "";
        private bool _defaultSpecified = false;
        private string _modelName = "";
        private ShowParamInModel _paramVisible;

        /// <summary>
        /// Fires when the parameter's value is changed
        /// </summary>
        internal event EventHandlerValueChanged ValueChanged;

        #endregion variables

        #region events

        /// <summary>
        /// Call this when the parameter's value is changed
        /// </summary>
        protected void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        #endregion

        #region methods

        /// <summary>
        /// Populates the parameter with default values
        /// </summary>
        virtual public void GenerateDefaultOutput(string path)
        {

        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        virtual public DialogElement InputDialogElement(List<DataSetArray> DataSets)
        {
            return null;
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        virtual public DialogElement OutputDialogElement(List<DataSetArray> DataSets)
        {
            return null;
        }

        #endregion

        #region properties

        /// <summary>
        /// Specify if a graphic should be added to the model to represent the parameter
        /// </summary>
        public ShowParamInModel ParamVisible
        {
            get { return _paramVisible; }
            set { _paramVisible = value; }
        }

        /// <summary>
        /// Used to identify this parameter in the modeler, it will be set automatically by the modeler
        /// </summary>
        internal string ModelName
        {
            get { return _modelName; }
            set { _modelName = value; }
        }

        /// <summary>
        /// Returns a shallow copy of the Parameter class
        /// </summary>
        /// <returns>A new Parameters class that is a shallow copy of the original parameters class</returns>
        public Parameter Copy()
        {
            return MemberwiseClone() as Parameter;
        }

        /// <summary>
        /// This returns a duplicate of this object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return Copy() as object;
        }

        /// <summary>
        /// Gets true if the parameter contains a value and false if none has been specified
        /// </summary>
        public bool DefaultSpecified
        {
            get { return _defaultSpecified; }
            set { _defaultSpecified = value; } 
        }
        
        /// <summary>
        /// Gets or sets the value of the parameter
        /// </summary>
        public object Value
        {
            get { return _dataValue; }
            set 
            {
                _dataValue = value;
                if (_dataValue != null)
                    _defaultSpecified = true;
                else
                    _defaultSpecified = false;
                OnValueChanged();
            }
        }

        /// <summary>
        /// The name of that parameter that shows up in auto generated forms
        /// </summary>
        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }
                
        /// <summary>
        /// Gets the type of data used in this parameter
        /// </summary>
        public string ParamType
        {
            get { return _paramType; }
            protected set { _paramType = value; }
        }

        /// <summary>
        /// Gets or Sets the help image that will appear beside the parameter element when it is clicked
        /// </summary>
        public System.Drawing.Bitmap HelpImage
        {
            get { return _helpImage; }
            set { _helpImage = value; }
        }

        /// <summary>
        /// Gets or Sets the help text that will appear beside the parameter element when it is clicked
        /// </summary>
        public String HelpText
        {
            get { return _helpText; }
            set { _helpText = value; }
        }

        #endregion properties

    }
}
