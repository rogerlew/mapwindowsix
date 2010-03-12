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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/18/2008 9:33:22 AM
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

namespace MapWindow.Drawing
{


    /// <summary>
    /// TextSymbolGroupCollection
    /// </summary>
    public class TextSymbolGroupCollection : ITextSymbolGroupCollection
    {
        #region Private Variables

        private List<ITextSymbolGroup> _list;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TextSymbolGroupCollection
        /// </summary>
        public TextSymbolGroupCollection()
        {
            _list = new List<ITextSymbolGroup>();
        }

        /// <summary>
        /// This will automatically set up the list, stripping out the unique symbols and
        /// associating each integer label with a specific symbol.  This will also add
        /// event handlers to the "SymbolChanged" events on the labels, so that it can 
        /// update the groupings as symbols are changed.  "Selection" symbol groups are
        /// dependent on the regular symbol groups.  When a new "regular" group is created,
        /// the selection symbolizer of the first member is used to define the symbolizer
        /// of the group.
        /// </summary>
        /// <param name="labels">The labels</param>
        public TextSymbolGroupCollection(IList<ILabel> labels)
        {
            _list = new List<ITextSymbolGroup>();
            for(int lbl = 0; lbl < labels.Count; lbl++)
            {
                ILabel label = labels[lbl];
                bool found = false;
                foreach(ITextSymbolGroup sg in _list)
                {
                    if (sg.Symbolizer == label.Symbolizer)
                    {
                        sg.RegularLabels.Add(lbl);
                        label.SelectionSymbolizer = sg.SelectionSymbolizer;
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                   
                    ITextSymbolGroup sg = new TextSymbolGroup();
                    sg.RegularLabels.Add(lbl);
                    sg.Symbolizer = label.Symbolizer;
                    sg.SelectionSymbolizer = label.Symbolizer;
                    
                }
            }

        }


        #endregion





        #region IList<ITextSymbolGroup> Members

        /// <summary>
        /// Returns the integer index of the specified item in the list
        /// </summary>
        /// <param name="item">The ITextSymbolGroup to investigate</param>
        /// <returns></returns>
        public int IndexOf(ITextSymbolGroup item)
        {
            return _list.IndexOf(item);
        }

        /// <summary>
        /// Inserts a new item at the specified index in the list
        /// </summary>
        /// <param name="index">The integer index to insert the item into</param>
        /// <param name="item">The ITextSymbolGroup to insert into the list</param>
        public void Insert(int index, ITextSymbolGroup item)
        {
            _list.Insert(index, item);
        }

        /// <summary>
        /// Removes the ITextSymbolGroup at the specified index
        /// </summary>
        /// <param name="index">The integer index to remove the item from</param>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the ITextSymbolGroup that is at the specified index
        /// </summary>
        /// <param name="index">The zero-based integer index of the ITextSymbolGroup</param>
        /// <returns>A valid ItextSymbolGroup that is required for fish.</returns>
        public ITextSymbolGroup this[int index]
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

        #region ICollection<ITextSymbolGroup> Members

        /// <summary>
        /// Adds the specified item to the end of the list
        /// </summary>
        /// <param name="item">The ITextSymbolGroup to add to the list</param>
        public void Add(ITextSymbolGroup item)
        {
            _list.Add(item);
        }

        /// <summary>
        /// Clears the list of the current members
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Returns true if the specified item is found within the list.
        /// </summary>
        /// <param name="item">Gets or sets whether the item is returned.</param>
        /// <returns>Boolean, true if the item was found.</returns>
        public bool Contains(ITextSymbolGroup item)
        {
            return _list.Contains(item);
        }

        /// <summary>
        /// Copies the members of this list into the specified array starting with the specified index in the destination array.
        /// </summary>
        /// <param name="array">The array of ITextSymbolGroup</param>
        /// <param name="arrayIndex">The zero based integer index in the destination array where copying should begin</param>
        public void CopyTo(ITextSymbolGroup[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the integer count of the members in the list
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Gets whether or not this collection is read only, this returns false
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified member from the list
        /// </summary>
        /// <param name="item">The ITextSymbolGroup to remove from the list</param>
        /// <returns>Boolean, true if the item was successfully found and removed</returns>
        public bool Remove(ITextSymbolGroup item)
        {
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable<ITextSymbolGroup> Members

        /// <summary>
        /// Returns an Enumerator for cycling through the members of this list.
        /// </summary>
        /// <returns>An IEnumerator</returns>
        public IEnumerator<ITextSymbolGroup> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator() as System.Collections.IEnumerator;
        }

        #endregion
    }
}
