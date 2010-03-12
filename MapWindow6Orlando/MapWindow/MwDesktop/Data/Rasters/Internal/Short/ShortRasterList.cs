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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 12:54:08 PM
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

namespace MapWindow.Data
{


    /// <summary>
    /// The goal of this list is to be polymorphic.  In a setting where you have an IList of the IRaster interface,
    /// it will behave like that.  If, on the otherhand, the variable is defined as a List of type specific rasters,
    /// it will function as that type of list.  It's primary mode of operation should be as a type-specific list.
    /// </summary>
    internal class ShortRasterList : IList<IRaster>, IList<ShortRaster>
    {

        #region Private Variables

        private List<ShortRaster> _list;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RasterList
        /// </summary>
        public ShortRasterList()
        {
            _list = new List<ShortRaster>();


        }

        #endregion


        // ------------------------------- TYPE SPECIFIC REGION -------------------------------

        #region IList<ShortRaster> Members

        /// <summary>
        /// Determines the index of a specific ShortRaster in this RasterList
        /// </summary>
        /// <param name="item">a specific ShortRaster to locate in the RasterList</param>
        /// <returns>The integer index of the value to find</returns>
        public int IndexOf(ShortRaster item)
        {

            return _list.IndexOf(item);
        }

        /// <summary>
        /// Inserts a new ShortRaster into this RasterList
        /// </summary>
        /// <param name="index">The zero-based, integer index where to insert the value.</param>
        /// <param name="item">The ShortRaster to insert into the list.</param>
        public void Insert(int index, ShortRaster item)
        {
            _list.Insert(index, item);
        }

        /// <summary>
        /// Removes the ShortRaster from the specified index location
        /// </summary>
        /// <param name="index">The zero-based, integer index to remove </param>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the ShortRaster at the specified index
        /// </summary>
        /// <param name="index">The zero-based integer index to access the ShortRaster of</param>
        /// <returns>The ShortRaster at the specified index</returns>
        public ShortRaster this[int index]
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
        /// Adds a new ShortRaster to this RasterList
        /// </summary>
        /// <param name="item">The ShortRaster to add to the end of the list</param>
        public void Add(ShortRaster item)
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
        /// Tests this RasterList to determine if the specified ShortRaster is in the list
        /// </summary>
        /// <param name="item">the ShortRaster to test.</param>
        /// <returns>Boolean, true if the specified ShortRaster is in the RasterList</returns>
        public bool Contains(ShortRaster item)
        {
            return _list.Contains(item);
        }

        /// <summary>
        /// Copies all the ShortRaster elements from this RasterList into the specified array, starting at the arrayIndex position in the array.
        /// </summary>
        /// <param name="array">The array of ShortRaster to add items to.</param>
        /// <param name="arrayIndex">Specifies the location in the array to start pasting values.</param>
        public void CopyTo(ShortRaster[] array, int arrayIndex)
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
        /// Removes the specified ShortRaster from this list.  This will return true if
        /// the item was successfully removed, and false otherwise.
        /// </summary>
        /// <param name="item">The ShortRaster to remove from this RasterList.</param>
        /// <returns>Boolean, true if the item was successfully removed from the list.</returns>
        public bool Remove(ShortRaster item)
        {
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable<Raster<T,Calc>> Members

        /// <summary>
        /// Creates an enumerator for cycling through this RasterList.
        /// </summary>
        /// <returns>An IEnumerator of ShortRaster members.</returns>
        public IEnumerator<ShortRaster> GetEnumerator()
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
            if (item == null) return -1;
            ShortRaster raster = item as ShortRaster;
            if (raster == null)
            {
                throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2", "ShortRasterList"));
            }
            return _list.IndexOf(raster);

        }

        void IList<IRaster>.Insert(int index, IRaster item)
        {
            if (item == null) return;
            ShortRaster raster = item as ShortRaster;
            if (raster == null)
            {
                throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2", "ShortRasterList"));
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
                ShortRaster raster = value as ShortRaster;
                if (raster == null)
                {
                    throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2", "ShortRasterList"));
                }
                _list[index] = raster;
            }
        }

        #endregion

        #region ICollection<IRaster> Members

        void ICollection<IRaster>.Add(IRaster item)
        {
            if (item == null) return;
            ShortRaster raster = item as ShortRaster;
            if (raster == null)
            {
                throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2", "ShortRasterList"));
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
            ShortRaster raster = item as ShortRaster;
            if (raster == null)
            {
                throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2", "ShortRasterList"));
            }
            return _list.Contains(raster);
        }

        void ICollection<IRaster>.CopyTo(IRaster[] array, int arrayIndex)
        {
            int index = arrayIndex;
            foreach (ShortRaster raster in _list)
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
            ShortRaster raster = item as ShortRaster;
            if (raster == null)
            {
                throw new ArgumentException(MessageStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "item").Replace("%S2", "ShortRasterList"));
            }
            return _list.Remove(raster);
        }

        #endregion

        #region IEnumerable<IRaster> Members

        IEnumerator<IRaster> IEnumerable<IRaster>.GetEnumerator()
        {
            return new ShortRasterEnumerator(_list);
        }

        #endregion
    }
}
