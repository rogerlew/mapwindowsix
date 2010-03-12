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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/29/2009 11:11:45 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Windows.Forms;
using MapWindow.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
namespace MapWindow.Forms
{


    /// <summary>
    /// LineSymbolizerEditor
    /// </summary>
    public class PointSymbolizerEditor : UITypeEditor
    {
        #region Private Variables

        private IPointSymbolizer _original;
        private IPointSymbolizer _copy;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LineSymbolizerEditor
        /// </summary>
        public PointSymbolizerEditor()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Launches a form for editing the line symbolizer
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _original = value as IPointSymbolizer;
            if (_original == null) return value;
            _copy = _original.Copy();
            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            DetailedPointSymbolDialog dialog = new DetailedPointSymbolDialog(_copy);
            dialog.ChangesApplied += new EventHandler(dialog_ChangesApplied);
            if (dialogProvider.ShowDialog(dialog) != DialogResult.OK) return value;
            _original.CopyProperties(_copy);
            return value;
        }

        void dialog_ChangesApplied(object sender, EventArgs e)
        {
            _original.CopyProperties(_copy);
        }

      
        /// <summary>
        /// Specifies that this should open a form and work using a modal behavior.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        #endregion

        #region Properties



        #endregion



    }
}
