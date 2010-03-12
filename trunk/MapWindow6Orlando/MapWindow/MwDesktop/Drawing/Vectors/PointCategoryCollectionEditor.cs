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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/31/2009 12:30:39 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.ComponentModel.Design;
namespace MapWindow.Drawing
{


    /// <summary>
    /// PointCategoryCollectionEditor
    /// </summary>
    public class PointCategoryCollectionEditor : CollectionEditor
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CharacterCollectionEditor
        /// </summary>
        public PointCategoryCollectionEditor(Type inType)
            : base(inType)
        {

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion

        /// <summary>
        /// Overrides the default behavior so that we automatically can handle 
        /// creating new instances of interfaces.
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        protected override object CreateInstance(Type itemType)
        {
            return new PointCategory();
            
        }

        /// <summary>
        /// This allows the item to be shown as a PointCategory, instead of just "Object"
        /// </summary>
        /// <returns></returns>
        protected override Type[] CreateNewItemTypes()
        {
            return new[] { typeof(PointCategory) };
        }

       


    }
}
