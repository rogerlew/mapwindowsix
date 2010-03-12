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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 11:54:56 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using MapWindow.Main;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{


    /// <summary>
    /// ColorCategoryCollection
    /// </summary>
    public class ColorCategoryCollection : ChangeEventList<IColorCategory>
    {
       
        private IColorScheme _scheme;


		/// <summary>
		/// Default constructor for the PolygonCategoryCollection class.
		/// </summary>
		public ColorCategoryCollection()
		{
		}

		/// <summary>
		/// Initializes a new PolygonCategoryCollection instance with the supplied scheme.
		/// </summary>
		/// <param name="scheme">The scheme to use ofr this collection.</param>
        public ColorCategoryCollection(IColorScheme scheme)
            : this()
		{
			_scheme = scheme;
		}

    	/// <summary>
        /// Ensures that newly added categories can navigate to higher legend items.
        /// </summary>
        /// <param name="item">The newly added legend item.</param>
        protected override void OnInclude(IColorCategory item)
        {
            if(item != null)
            {
                if (_scheme != null) item.SetParentItem(_scheme.GetParentItem());
            }
            
            base.OnInclude(item);
        }

        
       

        

        /// <summary>
        /// Changes the parent item of the specified category
        /// </summary>
        /// <param name="item"></param>
        protected override void OnExclude(IColorCategory item)
        {
            if (item == null) return;
            item.SetParentItem(null);
            base.OnExclude(item);
        }


        /// <summary>
        /// Gets or sets the parent scheme for this collection
        /// </summary>
		[Serialize("Scheme", ConstructorArgumentIndex = 0), ShallowCopy]
		public IColorScheme Scheme
        {
            get { return _scheme; }
            set
            {
                _scheme = value;
                UpdateItemParentPointers();
            }
        }

        /// <summary>
        /// Updates all of the categories so that they have a parent item that matches the
        /// schemes parent item.
        /// </summary>
        public void UpdateItemParentPointers()
        {
            foreach (IColorCategory item in InnerList)
            {
                if (_scheme == null)
                {
                    item.SetParentItem(null);
                }
                else
                {
                    item.SetParentItem(_scheme.GetParentItem());
                }
            }
        }



    }
}
