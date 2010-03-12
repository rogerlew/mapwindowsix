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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/30/2009 11:32:00 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using MapWindow.Drawing;
namespace MapWindow.Forms
{


    /// <summary>
    /// ExpressionEditor
    /// </summary>
    public class ExpressionEditor : UITypeEditor
    {
        #region Private Variables

        System.ComponentModel.ITypeDescriptorContext _context;
       
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ExpressionEditor
        /// </summary>
        public ExpressionEditor()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// This describes how to launch the form etc.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _context = context;   
          

            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            SQLExpressionDialog dlgExpression = new SQLExpressionDialog();
            string original = (string)value;
            dlgExpression.Expression = (string)value;

            // Try to find the Table
            IFeatureCategory category = context.Instance as IFeatureCategory;
            if (category != null)
            {
                IFeatureScheme scheme = category.GetParentItem() as IFeatureScheme;
                if (scheme != null)
                {
                    IFeatureLayer layer = scheme.GetParentItem() as IFeatureLayer;
                    if (layer != null)
                    {
                        dlgExpression.Table = layer.DataSet.DataTable;
                    }
                }
                else
                {
                    IFeatureLayer layer = category.GetParentItem() as IFeatureLayer;
                    if (layer != null)
                    {
                        dlgExpression.Table = layer.DataSet.DataTable;
                    }
                }
            }

            dlgExpression.ChangesApplied += dlgExpression_ChangesApplied;
            if (dialogProvider.ShowDialog(dlgExpression) != System.Windows.Forms.DialogResult.OK)
            {
                return original;
            }
            else
            {
                return dlgExpression.Expression;
            }
            
        }

        void dlgExpression_ChangesApplied(object sender, EventArgs e)
        {
            SQLExpressionDialog dlg = sender as SQLExpressionDialog;
            if (dlg != null)
            {
                string exp = dlg.Expression;
                _context.PropertyDescriptor.SetValue(_context.Instance, exp);
            }

        }

        /// <summary>
        /// This tells the editor that it should open a dialog form when editing the value from a ... button
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
