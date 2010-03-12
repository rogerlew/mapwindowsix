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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2008 1:21:00 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.Drawing.Design;
using System.Windows.Forms.Design;
namespace MapWindow.Forms
{


    /// <summary>
    /// PropertyGridEditor
    /// </summary>
    public class PolygonSchemePropertyGridEditor : UITypeEditor
    {
        private System.ComponentModel.ITypeDescriptorContext _context;
        private IPolygonScheme _editCopy;
        private IPolygonScheme _original;


        /// <summary>
        /// This should launch an open file dialog instead of the usual thing.
        /// </summary>
        /// <param name="context">System.ComponentModel.ITypeDescriptorContext</param>
        /// <param name="provider">IServiceProvider</param>
        /// <param name="value">The object being displayed</param>
        /// <returns>A new version of the object if the dialog was ok.</returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _context = context;
            _original = value as IPolygonScheme;
            _editCopy = _original.Copy();

            
            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            NamedList<IPolygonCategory> cats = new NamedList<IPolygonCategory>(_editCopy.Categories, "Category");
            CollectionPropertyGrid frm = new CollectionPropertyGrid(cats);
            frm.ChangesApplied += new EventHandler(frm_ChangesApplied);
            frm.AddItemClicked += new EventHandler(frm_AddItemClicked);
            dialogProvider.ShowDialog(frm);
            return _original; // don't bother swapping out the edit copy, just store copies of the categories when changes are applied.
        }

        void frm_AddItemClicked(object sender, EventArgs e)
        {
            _editCopy.Categories.Add(new PolygonCategory());
        }

        void frm_ChangesApplied(object sender, EventArgs e)
        {
            // the scheme is a reference copy to the original layer.  This way,
            // applying the scheme here should also update the map.
            _original.CopyProperties(_editCopy);
        }

        /// <summary>
        /// Either allows the editor to work or else nips it in the butt
        /// </summary>
        /// <param name="context">System.ComponentModel.ITypeDescriptorContext</param>
        /// <returns>UITypeEditorEditStyle</returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            
            return UITypeEditorEditStyle.Modal;
        }




    }
}
