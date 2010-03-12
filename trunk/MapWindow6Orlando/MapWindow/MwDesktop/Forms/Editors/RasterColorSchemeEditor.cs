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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2008 11:02:31 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing.Design;
using MapWindow.Drawing;
using System.Windows.Forms.Design;
namespace MapWindow.Forms
{


    /// <summary>
    /// RasterColorSchemeEditor
    /// </summary>
    public class RasterColorSchemeEditor : UITypeEditor
    {
        

        /// <summary>
        /// This should launch a frmRasterSymbolizer
        /// </summary>
        /// <param name="context">System.ComponentModel.ITypeDescriptorContext context</param>
        /// <param name="provider">IServiceProvider provider</param>
        /// <param name="value">object value</param>
        /// <returns>A new RasterSymbolizer</returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            IRasterSymbolizer rs = value as IRasterSymbolizer;
            if (rs == null)
            {
                rs = new RasterSymbolizer();
            }
         
           
            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            IRasterLayer parent = rs.ParentLayer;
            RasterLayerDialog frm = new RasterLayerDialog(parent);
            dialogProvider.ShowDialog(frm);
            return parent.Symbolizer;
        }

        /// <summary>
        /// This controls the editor style and sets up a backup copy of the symbolizer
        /// </summary>
        /// <param name="context">System.ComponentModel.ITypeDescriptorContext</param>
        /// <returns>UITypeEditorEditStyle</returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
           
        }


    }
}
