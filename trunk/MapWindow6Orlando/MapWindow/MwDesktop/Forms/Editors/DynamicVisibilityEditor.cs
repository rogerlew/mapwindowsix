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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2009 5:08:52 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using MapWindow.Components;
using MapWindow.Drawing;

namespace MapWindow.Forms 
{


    /// <summary>
    /// DynamicVisibilityEditor
    /// </summary>
    public class DynamicVisibilityEditor : UITypeEditor
    {
        #region Private Variables

        private ILayer _layer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DynamicVisibilityEditor
        /// </summary>
        public DynamicVisibilityEditor()
        {
         
        }

        #endregion

        #region Methods

        /// <summary>
        /// Display a drop down when editing instead of the normal control, and allow the user to "grab" a 
        /// new dynamic visibility extent.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _layer = context.Instance as ILayer;
            IWindowsFormsEditorService dialogProvider;
            dialogProvider = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            DynamicVisibilityControl dvc = new DynamicVisibilityControl(dialogProvider, _layer);
            dialogProvider.DropDownControl(dvc);
            _layer.Invalidate();
            return dvc.UseDynamicVisibility;
        }

        /// <summary>
        /// Indicate that we should use a drop-down for controlling dynamic visibility.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
           
            return UITypeEditorEditStyle.DropDown;
        }

        #endregion

        #region Properties



        #endregion



    }
}
