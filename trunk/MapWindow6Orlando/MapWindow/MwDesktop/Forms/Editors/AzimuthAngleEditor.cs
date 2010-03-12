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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/7/2009 2:14:56 PM
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
    /// CharacterCodeEditor
    /// </summary>
    public class AzimuthAngleEditor : UITypeEditor
    {
        IWindowsFormsEditorService _dialogProvider;
      
        #region Constructors

        /// <summary>
        /// Creates a new instance of CharacterCodeEditor
        /// </summary>
        public AzimuthAngleEditor()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Edits a value based on some user input which is collected from a character control.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _dialogProvider = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            AngleControl ac = new AngleControl();
            ac.StartAngle = 90;
            ac.Clockwise = true;
            ac.Angle = Convert.ToInt32(value);
            ac.AngleChosen += new EventHandler(ac_AngleChosen);
            _dialogProvider.DropDownControl(ac as System.Windows.Forms.Control);
            return (object)(double)ac.Angle;
        }

        void ac_AngleChosen(object sender, EventArgs e)
        {
            _dialogProvider.CloseDropDown();
        }

       

        /// <summary>
        /// Gets the UITypeEditorEditStyle, which in this case is drop down.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        

        #endregion

        #region Properties

        /// <summary>
        /// Ensures that we can widen the drop-down without having to close the drop down,
        /// widen the control, and re-open it again.
        /// </summary>
        public override bool IsDropDownResizable
        {
            get
            {
                return true;
            }
        }

        #endregion


    }
}
