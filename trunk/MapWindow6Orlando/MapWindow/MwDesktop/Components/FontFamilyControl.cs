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
// The Initial Developer of this Original Code is Ted Dunsford. Created ? 
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MapWindow.Components
{
    [DefaultEvent("SelectedItemChanged"), DefaultProperty("SelectedFamily"),
    ToolboxBitmap(typeof(FontFamilyControl), "UserControl.ico")]
    public partial class FontFamilyControl : UserControl
    {
        /// <summary>
        /// Event
        /// </summary>
        public event EventHandler SelectedItemChanged;
    
        /// <summary>
        /// Creates a new instance of the Font Family control, pre-loading a font drop down.
        /// </summary>
        public FontFamilyControl()
        {
            InitializeComponent();
            foreach (FontFamily family in FontFamily.Families)
            {
                ffdNames.Items.Add(family.Name);
            }
            ffdNames.SelectedItem = "Arial";
            ffdNames.SelectedValueChanged += new EventHandler(ffdNames_SelectedValueChanged);
        }

        /// <summary>
        /// Gets or sets the currently selected font family name.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedFamily
        {
            get { return ffdNames.SelectedItem.ToString(); }
            set { ffdNames.SelectedItem = value; }
        }

        /// <summary>
        /// Gets the selected family name as a FontFamily object
        /// </summary>
        /// <returns>A FontFamily object</returns>
        public FontFamily GetSelectedFamily()
        {
            return new FontFamily(ffdNames.SelectedItem.ToString());
        }

        void ffdNames_SelectedValueChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        /// <summary>
        /// Throws a new event when the selected item changed.
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
            if (SelectedItemChanged != null) SelectedItemChanged(this, new EventArgs());
        }

    }
}
