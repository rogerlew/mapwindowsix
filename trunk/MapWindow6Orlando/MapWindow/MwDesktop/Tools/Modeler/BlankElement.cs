//********************************************************************************************************
// Product Name: MapWindow.Tools.DataElement
// Description:  This class represents data in the model
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
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    internal class BlankElement : ModelElement
    {
        #region --------------- class variables

        #endregion

        #region --------------- Constructors
        
        /// <summary>
        /// Creates an instance of the Data Element
        /// <param name="modelElements">A list of all the elements in the model</param>
        /// </summary>
        public BlankElement(List<ModelElement> modelElements) : base(modelElements)
        {
            this.Width = 0;
            this.Height = 0;
        }

        public override void Paint(System.Drawing.Graphics graph)
        {
        }

        #endregion
    }
}
