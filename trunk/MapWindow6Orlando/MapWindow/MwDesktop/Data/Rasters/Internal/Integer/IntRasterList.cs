//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 11:43:15 AM
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
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Data
{


    /// <summary>
    /// The goal of this list is to be polymorphic.  In a setting where you have an IList of the IRaster interface,
    /// it will behave like that.  If, on the otherhand, the variable is defined as a List of type specific rasters,
    /// it will function as that type of list.  It's primary mode of operation should be as a type-specific list.
    /// </summary>
    internal class IntRasterList:IList<IRaster>, IList<IntRaster>
    {
      
        #region Private Variables

        private List<IntRaster> _list;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RasterList
        /// </summary>
        public IntRasterList()
        {
            _list = new List<IntRaster>();
           

        }

        #endregion


        // ------------------------------- TYPE SPECIFIC REGION -------------------------------

        #region IList<IntRaster> Members

        /// <summary>
        /// Determines the index of a specific IntRaster in this RasterList
        /// </summary>
        /// <param name="item">a specific IntRaster to locate in the RasterList</param>
        /// <returns>The integer index of the value to find</returns>
        public int IndexOf(IntRaster item)
        {
           
            return _list.IndexOf(item);
        }

        /// <summary>
        /// Inserts a new IntRaster into this RasterList
        /// </summary>
        /// <param name="index">The zero-based, integer index where to insert the value.</param>
        /// <param name="item">The IntRaster to insert into the list.</param>
        public void Insert(int index, IntRaster item)
        {
             _list.Insert(index, item);
        }

        /// <summary>
        /// Removes the IntRaster from the specified index location
        /// </summary>
        /// <param name="index">The zero-based, integer index to remove </param>
        public void RemoveAt(int index)
        {
             _list.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the IntRaster at the specified index
        /// </summary>
        /// <param name="index">The zero-based integer index to access the IntRaster of</param>
        /// <returns>The IntRaster at the specified index</returns>
        public IntRaster this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        #endregion

        #region ICollection<Raster<T,Calc>> Members

        /// <summary>
        /// Adds a new IntRaster to this RasterList
        /// </summary>
        /// <param name="item">The IntRaster to add to the end of the list</param>
        public void Add(IntRaster item)
        {
            _list.Add(item);
        }

        /// <summary>
        /// Clears all the members from this RasterList
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Tests this RasterList to determine if the specified IntRaster is in the list
        /// </summary>
        /// <param name="item">the IntRaster to test.</param>
        /// <returns>Boolean, true if the specified IntRaster is in the RasterList</returns>
        public bool Contains(IntRaster item)
        {
            return _list.Contains(item);
        }

        /// <summary>
        /// Copies all the IntRaster elements from this RasterList into the specified array, starting at the arrayIndex position in the array.
        /// </summary>
        /// <param name="array">The array of IntRaster to add items to.</param>
        /// <param name="arrayIndex">Specifies the location in the array to start pasting values.</param>
        public void CopyTo(IntRaster[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the count of items in this RasterList
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Gets whether or not this list is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified IntRaster from this list.  This will return true if
        /// the item was successfully removed, and false otherwise.
        /// </summary>
        /// <param name="item">The IntRaster to remove from this RasterList.</param>
        /// <returns>Boolean, true if the item was successfully removed from the list.</returns>
        public bool Remove(IntRaster item)
        {
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable<Raster<T,Calc>> Members

        /// <summary>
        /// Creates an enumerator for cycling through this RasterList.
        /// </summary>
        /// <returns>An IEnumerator of IntRaster members.</returns>
        public IEnumerator<IntRaster> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns the type specific enumerator as a generic IEnumerator that will increment through this list. 
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator() as System.Collections.IEnumerator;
        }

        #endregion


        // --------------------------- NON - Type SPECIFIC -----------------------

        #region IList<IRaster> Members

        int IList<IRaster>.IndexOf(IRaster item)
        {
            if(item == null)return -1;
            IntRaster raster = item as IntRaster;
            if (raster == null)
            {
                throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2","IntRasterList"));
            }
            return _list.IndexOf(raster);

        }

        void IList<IRaster>.Insert(int index, IRaster item)
        {
            if (item == null) return;
            IntRaster raster = item as IntRaster;
            if (raster == null)
            {
                throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2","IntRasterList"));
            }
            _list.Insert(index, raster);
          
        }

        void IList<IRaster>.RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        IRaster IList<IRaster>.this[int index]
        {
            get
            {
                return _list[index] as IRaster;
            }
            set
            {
                if (value == null) return;
                IntRaster raster = value as IntRaster;
                if (raster == null)
                {
                    throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2", "IntRasterList"));
                }
                _list[index] = raster;
            }
        }

        #endregion

        #region ICollection<IRaster> Members

        void ICollection<IRaster>.Add(IRaster item)
        {
            if (item == null) return;
            IntRaster raster = item as IntRaster;
            if (raster == null)
            {
                throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2", "IntRasterList"));
            }
            _list.Add(raster);
        }

        void ICollection<IRaster>.Clear()
        {
            _list.Clear();
        }

        bool ICollection<IRaster>.Contains(IRaster item)
        {
            if (item == null) return false;
            IntRaster raster = item as IntRaster;
            if (raster == null)
            {
                throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2", "IntRasterList"));
            }
            return _list.Contains(raster);
        }

        void ICollection<IRaster>.CopyTo(IRaster[] array, int arrayIndex)
        {
            int index = arrayIndex;
            foreach(IntRaster raster in _list)
            {
                array[index] = raster as IRaster;
                index++;
            }

        }

        int ICollection<IRaster>.Count
        {
            get { return _list.Count; }
        }

        bool ICollection<IRaster>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<IRaster>.Remove(IRaster item)
        {
            if (item == null) return false;
            IntRaster raster = item as IntRaster;
            if (raster == null)
            {
                throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2","IntRasterList"));
            }
            return _list.Remove(raster);
        }

        #endregion

        #region IEnumerable<IRaster> Members

        IEnumerator<IRaster> IEnumerable<IRaster>.GetEnumerator()
        {
            return new IntRasterEnumerator(_list);
        }

        #endregion
    }
}
