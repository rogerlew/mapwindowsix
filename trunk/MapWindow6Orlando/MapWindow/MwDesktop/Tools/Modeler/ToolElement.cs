//********************************************************************************************************
// Product Name: MapWindow.Tools.ModelElement
// Description:  An abstract class that handles drawing boxes for elements in the modeler window
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
using System.Text;
using System.Drawing;

namespace MapWindow.Tools
{
    internal class ToolElement : ModelElement
    {
        #region --------------- class variables

        private ITool _tool;
        private ToolStatus _toolStatus;
        private ToolExecuteStatus _toolExecuteStatus;

        #endregion

        #region --------------- Constructors

        /// <summary>
        /// Creates an instance of the tool element
        /// </summary>
        /// <param name="Tool">The tool that the tool element represents</param>
        /// <param name="modelElements">A list of all the elements in the model</param>
        public ToolElement(ITool Tool, List<ModelElement> modelElements): base(modelElements)
        {
            _tool = Tool;
            updateStatus();
        }

        #endregion

        #region --------------- Properties

        /// <summary>
        /// Gets or sets the status of the tools execution, used when the modeler runs the model.
        /// </summary>
        public ToolExecuteStatus ExecutionStatus
        {
            get{return _toolExecuteStatus;}
            set { _toolExecuteStatus = value; }
        }

        /// <summary>
        /// Gets the ITool this element represents
        /// </summary>
        public ITool Tool
        {
            get { return _tool; }
        }

        /// <summary>
        /// Gets or sets the list of Elements presently available in the Modeler
        /// </summary>
        public List<ModelElement> ElementsInModel
        {
            get { return ElementsInModel; }
            set { ElementsInModel = value; }
        }

        /// <summary>
        /// Gets the current status of the tool
        /// </summary>
        public ToolStatus ToolStatus
        {
            get { return _toolStatus; }
        }

        #endregion

        #region --------------- Methods

        /// <summary>
        /// Updates the status indicator
        /// </summary>
        public void updateStatus()
        {
            ToolDialog td = new ToolDialog(_tool, base.ModelElements);
            _toolStatus = td.ToolStatus;
        }

        /// <summary>
        /// Draws the status light on the background of the DataElement
        /// </summary>
        /// <param name="graph"></param>
        protected override void drawStatusLight(System.Drawing.Graphics graph)
        {
            Bitmap statusImage = MapWindow.Images.valid;
            if (ToolStatus == ToolStatus.Empty)
                statusImage = MapWindow.Images.Caution;
            else if (ToolStatus == ToolStatus.Error)
                statusImage = MapWindow.Images.Error;
            graph.DrawImage(statusImage, 5, 5, statusImage.Width, statusImage.Height);
        }

        /// <summary>
        /// When the user doulbe clicks on a tool call this method
        /// </summary>
        public override bool DoubleClick()
        {
            ToolDialog td = new ToolDialog(_tool, base.ModelElements);
            td.ShowDialog();
            if (td.DialogResult == System.Windows.Forms.DialogResult.OK)
                return true;
            return false;
        }

        #endregion

    }
}
