//********************************************************************************************************
// Product Name: MapWindow.Interfaces Alpha
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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using MapWindow.Main;
using System.ComponentModel;
namespace MapWindow.Main
{
    /// <summary>
    /// Represents a nongeneric collection of key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The specific type of a unique identifier.</typeparam>
    /// <typeparam name="TValue">The specific type of the items being stored in this dictionary</typeparam>
    public class EventDictionary<TKey, TValue> : OrderedDictionary<TKey, TValue>, IEventDictionary<TKey, TValue>
    {

        
        
       
        #region Constructors

        /// <summary>
        /// Constructor for an EventDictionary
        /// </summary>
        public EventDictionary()
        {
        }

        #endregion


        #region Methods

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value"> The value of the element to add. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentException">An element with the same key already exists in the System.Collections.Generic.Dictionary &lt;TKey,TValue&gt;."</exception>
        /// <exception cref="System.ArgumentNullException">key is null.</exception> 
        /// <returns>Integer, the index where the latest member was added.  Returns -1 if canceled</returns>
        public override int Add(TKey key, TValue value)
        {
            KeyValuePair<TKey, TValue> kvp = new KeyValuePair<TKey, TValue>(key, value);
            if (OnBeforeItemAdded(kvp) == true) return -1; // Canceled if True
            base.Add(key, value);
            OnAfterItemAdded(kvp, Count-1);
            return Count - 1;
        }

        /// <summary>
        /// Adds a key value pair together in one item
        /// </summary>
        /// <param name="item">A strong typed KeyValuePair&lt;TKey, TValue&gt; pair to add to the EventDictionary </param>
        public override void Add(KeyValuePair<TKey, TValue> item)
        {
            if (OnBeforeItemAdded(item) == true) return; // Canceled if True
            base.Add(item);
            OnAfterItemAdded(item, Count -1);
        }

        /// <summary>
        /// Removes all keys and values from the EventDictionary&lt;TKey,TValue&gt;.
        /// </summary>
        public override void Clear()
        {
            if (OnBeforeClearing() == true) return; // Canceled if true
            base.Clear();
            OnAfterClearing();
        }



       
        

        

        /// <summary>
        /// Keys allow you to track specific members, even if they move around in the list
        /// </summary>
        /// <param name="index">The index in the list, which keeps sequential track of the members</param>
        /// <param name="key">The key, which uniquely identifies the value member</param>
        /// <param name="value">The value, which is of a specific type associated with this EventDictionary</param>
        public override void Insert(int index, TKey key, TValue value)
        {
           
            if (index > Count || index < Count)
                throw new ArgumentOutOfRangeException("Index was out of range.");
            KeyValuePair<TKey, TValue> item = new KeyValuePair<TKey, TValue>(key, value);
            if(OnBeforeItemInserted(item, index) == true)return; // Canceled if true
            base.Insert(index, key, value);
            OnAfterItemInserted(item, index);
        }

       


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if the element is successfully removed; otherwise, false. This method also returns false if key was not found in the original System.Collections.Generic.IDictionary&lt;TKey,TValue&gt;.</returns>
        public override bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (OnBeforeItemRemoved(item, base.IndexOf(item.Key)) == true) return false; // Opperation canceled if true
            bool res = base.Remove(item);
            if (res == true)
            {
                OnAfterItemRemoved(item);
            }
            return res;
           
        }

        /// <summary>
        /// Removes the value with the specified key from the EventDictionary&lt;TKey,TValue&gt;
        /// </summary>
        /// <param name="key">The key of the element to remove</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method returns false if key is not found in the EventDictionary&lt;TKey,TValue&gt;</returns>
        public override bool Remove(TKey key)
        {
            TValue val = base.GetValue(key);
            KeyValuePair<TKey, TValue> item = new KeyValuePair<TKey, TValue>(key, val);
            if (OnBeforeItemRemoved(item, base.IndexOf(key)) == true) return false; // Opperation canceled if true
            bool res = base.Remove(key);
            if (res == true)
            {
                OnAfterItemRemoved(item);
            }
            return res;
        }


        /// <summary>
        /// This Gets or sets the key value pair for the indexed list, as well as updating
        /// the internal dictionary if necessary.  This enables later access by key as well
        /// as by the index values.  Since Keys are unique, setting a pair with a key that
        /// already exists will replace the value stored with that key.  However, it will
        /// add the item at the index with the specified index value, regardless of whether
        /// the indexed items already has a reference to the dictionary, effectively allowing
        /// the same key value pair to appear multiple times in the index.
        /// </summary>
        /// <param name="index">The integer index of this key value pair to get or set.</param>
        /// <returns>A KeyValuePair&lt;TKey, TValue&gt;</returns>
        /// <exception cref="System.IndexOutOfRangeException">The specified index was either less than 0 or greater than the the count - 1.</exception> 
        public override KeyValuePair<TKey, TValue> this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                if (index < 0 || index >= base.Count)
                {
                    throw new IndexOutOfRangeException("The specified index was either less than 0 or greater than the the count - 1.");
                }

                // TO DO: ON BEFORE ITEM SET
                base[index] = value;

                // TO DO: ON AFTER ITEM SET
            }
        }

     


        #endregion

        #region Properties

       
       


        /// <summary>
        /// Gets the Value in this EventDictionary&lt;TKey,TValue&gt; corresponding to the specified key
        /// </summary>
        /// <param name="key">The index of the item to retrieve</param>
        /// <returns>The value associated with the specified key. If the specified key is not found,
        /// a get operation throws a System.Collections.Generic.KeyNotFoundException, and a set operation
        /// creates a new element with the specified key.</returns>
        /// <exception cref="System.ArgumentNullException">key is null</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">The property is retrieved and key does not exist in the collection.</exception>
        [Obsolete("Use the GetValue(TKey key) and SetValue(TKey key) methods instead.  Otherwise integer keys will not work correctly.")]
        public override TValue this[TKey key]
        {
            set
            {
                if (base.ContainsKey(key))
                {
                    OnBeforeItemSet(key, value);
                    base[key] = value;
                    OnAfterItemSet(key, value);
                   // To DO: IndexOfKey   new KeyValuePair<TKey, TValue>(key, value);
                }
                else
                {
                    KeyValuePair<TKey, TValue> item = new KeyValuePair<TKey, TValue>(key, value);
                    
                    Add(item); // this handles the add events so don't do it twice
                    
                }
               
            }
        }

       

        #endregion


        #region Events

        #region Add

        /// <summary>
        /// Occurs after an item has already been added to the list.
        /// The index where the item was added is specified.
        /// </summary>
        public virtual event EventHandler<IndividualIndex<KeyValuePair<TKey, TValue>>> AfterItemAdded;

        /// <summary>
        /// Fires the AfterItemAdded event
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Index"></param>
        protected void OnAfterItemAdded(KeyValuePair<TKey, TValue> Item, int Index)
        {
            if (AfterItemAdded != null) AfterItemAdded(this, new IndividualIndex<KeyValuePair<TKey, TValue>>(Item, Index));
        }


        /// <summary>
        /// Occurs after a range has already been added to the list.
        /// This reveals the index where the beginning of the range
        /// was added, but cannot be canceled.
        /// </summary>
        public virtual event EventHandler<CollectiveIndex<KeyValuePair<TKey, TValue>>> AfterRangeAdded;

        /// <summary>
        /// Fires the AfterRangeAdded event
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="Index"></param>
        void OnAfterRangeAdded(IEnumerable<KeyValuePair<TKey, TValue>> collection, int Index)
        {
            if (AfterRangeAdded != null)
            {
                AfterRangeAdded(this, new CollectiveIndex<KeyValuePair<TKey, TValue>>(collection, Index));
            }
        }

        /// <summary>
        /// Occurs before an item is added to the List.
        /// There is no index yet specified because it will be added to the
        /// end of the list.
        /// </summary>
        public virtual event EventHandler<IndividualCancel<KeyValuePair<TKey, TValue>>> BeforeItemAdded;

        /// <summary>
        /// Fires the BeforeItemAdded event
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        protected bool OnBeforeItemAdded(KeyValuePair<TKey, TValue> Item)
        {
            if (BeforeItemAdded != null)
            {
                IndividualCancel<KeyValuePair<TKey, TValue>> e = new IndividualCancel<KeyValuePair<TKey, TValue>>(Item);
                BeforeItemAdded(this, e);
                return e.Cancel;
            }
            return false;
        }


        /// <summary>
        /// Occurs before a range of items is added to the list.
        /// There is no index yet, but this event can be cancelled.
        /// </summary>
        public virtual event EventHandler<CollectiveCancel<KeyValuePair<TKey, TValue>>> BeforeRangeAdded;

        /// <summary>
        /// Fires the BeforeRangeAdded event
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        protected bool OnBeforeRangeAdded(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            if (BeforeRangeAdded != null)
            {
                CollectiveCancel<KeyValuePair<TKey, TValue>> e = new CollectiveCancel<KeyValuePair<TKey, TValue>>(collection);
                BeforeRangeAdded(this, e);
                return e.Cancel;
            }
            return false;
        }

        #endregion

        #region Clear

        /// <summary>
        /// Occurs before all the members of the dictionary are cleared
        /// </summary>
        public event CancelEventHandler BeforeClearing;

        /// <summary>
        /// Fires the BeforeClearing event
        /// </summary>
        /// <returns></returns>
        public bool OnBeforeClearing()
        {
            if (BeforeClearing == null) return false;
            CancelEventArgs e = new CancelEventArgs();
            BeforeClearing(this, e);
            return e.Cancel;
        }

        /// <summary>
        /// Occurs after the members have been cleared
        /// </summary>
        public event EventHandler AfterClearing;

        /// <summary>
        /// Fires the AfterClearing event
        /// </summary>
        public void OnAfterClearing()
        {
            if (AfterClearing == null) return;
            AfterClearing(this, new System.EventArgs());
        }

        #endregion

        

        #region Insert

        /// <summary>
        /// Occurs after an item is inserted.
        /// Shows the true index of the item after it was actually added.
        /// </summary>
        public virtual event EventHandler<IndividualIndex<KeyValuePair<TKey, TValue>>> AfterItemInserted;

        /// <summary>
        /// Fires the AfterItemInserted event
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Index"></param>
        protected void OnAfterItemInserted(KeyValuePair<TKey, TValue> Item, int Index)
        {
            if (AfterItemInserted != null) AfterItemInserted(this, new IndividualIndex<KeyValuePair<TKey, TValue>>(Item, Index));
        }

        /// <summary>
        /// Occurs after an item is inserted.
        /// Shows the true index of the item after it was actually added.
        /// </summary>
        public virtual event EventHandler<CollectiveIndex<KeyValuePair<TKey, TValue>>> AfterRangeInserted;

        /// <summary>
        /// Fires the AfterRangeInserted event
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="Index"></param>
        protected void OnAfterRangeInserted(IEnumerable<KeyValuePair<TKey, TValue>> collection, int Index)
        {
            if (AfterRangeInserted != null) AfterRangeInserted(this, new CollectiveIndex<KeyValuePair<TKey, TValue>>(collection, Index));
        }


        /// <summary>
        /// Occurs before an item is inserted.  The index of the requested
        /// insertion as well as the item being inserted and an option to
        /// cancel the event are specified
        /// </summary>
        public virtual event EventHandler<IndividualIndexCancel<KeyValuePair<TKey, TValue>>> BeforeItemInserted;

        /// <summary>
        /// Fires the BeforeItemInserted event
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        protected bool OnBeforeItemInserted(KeyValuePair<TKey, TValue> Item, int Index)
        {
            if (BeforeItemInserted != null)
            {
                IndividualIndexCancel<KeyValuePair<TKey, TValue>> e = new IndividualIndexCancel<KeyValuePair<TKey, TValue>>(Item, Index);
                BeforeItemInserted(this, e);
                return e.Cancel;
            }
            return false;
        }

        /// <summary>
        /// Occurs before a range is inserted.  The index of the requested
        /// insertion location as well as the item being inserted and an option to
        /// cancel the event are provided in the event arguments
        /// </summary>
        public virtual event EventHandler<CollectiveIndexCancel<KeyValuePair<TKey, TValue>>> BeforeRangeInserted;
        
        /// <summary>
        /// Fires the BeforeRangeInserted event
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        protected bool OnBeforeRangeInserted(IEnumerable<KeyValuePair<TKey, TValue>> collection, int Index)
        {
            if (BeforeRangeInserted != null)
            {
                CollectiveIndexCancel<KeyValuePair<TKey, TValue>> e = new CollectiveIndexCancel<KeyValuePair<TKey, TValue>>(collection, Index);
                BeforeRangeInserted(this, e);
                return e.Cancel;
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Occurs after a method that changes either the order or the members of this EventDictionary
        /// </summary>
        public virtual event EventHandler ListChanged;

        /// <summary>
        /// Firew the ListChanged event
        /// </summary>
        protected void OnListChanged()
        {
            if (ListChanged != null) ListChanged(this, new System.EventArgs()); ;
        }

        #region Reverse

        /// <summary>
        /// Occurs just after the list or any sub portion
        /// of the list is sorted.  This event occurs in
        /// addition to the specific reversal case.
        /// </summary>
        public virtual event EventHandler AfterReversed;

        /// <summary>
        /// Firew the AfterReversed event
        /// </summary>
        protected void OnAfterReversed()
        {
            if (AfterReversed == null) return;
            AfterReversed(this, new System.EventArgs());
        }



        /// <summary>
        /// Occurs just before the list or any sub portion
        /// of the list is sorted.  This event occurs in
        /// addition to the specific reversal case.
        /// </summary>
        public virtual event CancelEventHandler BeforeReversed;

        /// <summary>
        /// Fires the BeforeReversed event
        /// </summary>
        /// <returns></returns>
        protected bool OnBeforeReversed()
        {
            if (BeforeReversed == null) return false;
            CancelEventArgs e = new CancelEventArgs();
            BeforeReversed(this, e);
            return e.Cancel;
        }

        /// <summary>
        /// Occurs before a specific range is reversed
        /// </summary>
        public virtual event EventHandler<CollectiveIndexCancel<KeyValuePair<TKey, TValue>>> BeforeRangeReversed;

        /// <summary>
        /// Fires the BeforeRangeReversed event
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        protected bool OnBeforeRangeReversed(int Index, IEnumerable<KeyValuePair<TKey, TValue>> range)
        {
            if (BeforeRangeReversed == null) return false;
            CollectiveIndexCancel<KeyValuePair<TKey, TValue>> e = new CollectiveIndexCancel<KeyValuePair<TKey, TValue>>(range, Index);
            BeforeRangeReversed(this, e);
            return e.Cancel;
        }


        /// <summary>
        /// Occurs after a specific range is reversed
        /// </summary>
        public virtual event EventHandler<CollectiveIndex<KeyValuePair<TKey, TValue>>> AfterRangeReversed;

        /// <summary>
        /// Fires the AfterRangeReversed event
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="collection"></param>
        protected void OnAfterRangeReversed(int Index, IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            if (AfterRangeReversed == null) return;
            AfterRangeReversed(this, new CollectiveIndex<KeyValuePair<TKey, TValue>>(collection, Index));
        }


        /// <summary>
        /// Occurs before the entire list is reversed
        /// </summary>
        public virtual event CancelEventHandler BeforeListReversed;
        
        /// <summary>
        /// Fires the BeforeListReversed event
        /// </summary>
        /// <returns></returns>
        protected bool OnBeforeListReversed()
        {
            if (BeforeListReversed == null) return false;
            CancelEventArgs e = new CancelEventArgs();
            BeforeListReversed(this, e);
            return e.Cancel;
        }


        /// <summary>
        /// Occurs after the entire list is reversed
        /// </summary>
        public virtual event EventHandler AfterListReversed;

        /// <summary>
        /// Fires the AfterListReversed event
        /// </summary>
        protected void OnAfterListReversed()
        {
            if (AfterListReversed == null) return;
            AfterListReversed(this, new System.EventArgs());
        }

        #endregion

        #region Remove

        /// <summary>
        /// Occurs before an item is removed from the List.
        /// Specifies the item, the current index and an option to cancel.
        /// </summary>
        public virtual event EventHandler<IndividualIndexCancel<KeyValuePair<TKey, TValue>>> BeforeItemRemoved;

        /// <summary>
        /// Fires the BeforeItemRemoved event
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        protected bool OnBeforeItemRemoved(KeyValuePair<TKey, TValue> Item, int Index)
        {
            if (BeforeItemRemoved != null)
            {
                IndividualIndexCancel<KeyValuePair<TKey, TValue>> e = new IndividualIndexCancel<KeyValuePair<TKey, TValue>>(Item, Index);
                BeforeItemRemoved(this, e);
                return e.Cancel;
            }
            return false;
        }

        /// <summary>
        /// Occurs before a range is removed from the List.
        /// Specifies the range, the current index and an option to cancel.
        /// </summary>
        public virtual event EventHandler<CollectiveIndexCancel<KeyValuePair<TKey, TValue>>> BeforeRangeRemoved;

        /// <summary>
        /// Fires the BeforeRangeRemoved event
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        protected bool OnBeforeRangeRemoved(IEnumerable<KeyValuePair<TKey, TValue>> collection, int Index)
        {
            if (BeforeRangeRemoved != null)
            {
                CollectiveIndexCancel<KeyValuePair<TKey, TValue>> e = new CollectiveIndexCancel<KeyValuePair<TKey, TValue>>(collection, Index);
                BeforeRangeRemoved(this, e);
                return e.Cancel;
            }
            return false;
        }

        /// <summary>
        /// Occurs before all the elements that match a predicate are removed.
        /// Supplies an IEnumerable list in the event args of all the items
        /// that will match the expression.  This action can be cancelled.
        /// </summary>
        public virtual event EventHandler<CollectiveCancel<KeyValuePair<TKey, TValue>>> BeforeAllMatchingRemoved;

        /// <summary>
        /// Fires the BeforeAllMatchingRemoved event
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public virtual bool OnBeforeAllMatchingRemoved(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            if (BeforeAllMatchingRemoved != null)
            {
                CollectiveCancel<KeyValuePair<TKey, TValue>> e = new CollectiveCancel<KeyValuePair<TKey, TValue>>(collection);
                BeforeAllMatchingRemoved(this, e);
                return e.Cancel;
            }
            return false;
        }

        /// <summary>
        /// Occurs after all the elements that matched a predicate were 
        /// removed.  The values are the items that were successfully removed.
        /// The action has already happened, and so cannot be cancelled here.
        /// </summary>
        public virtual event EventHandler<Collective<KeyValuePair<TKey, TValue>>> AfterAllMatchingRemoved;

        /// <summary>
        /// fires the AfterAllMatchingRemoved event
        /// </summary>
        /// <param name="collection"></param>
        public virtual void OnAfterAllMatchingRemoved(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            if (AfterAllMatchingRemoved != null)
            {
                AfterAllMatchingRemoved(this, new Collective<KeyValuePair<TKey, TValue>>(collection));
            }

        }


        /// <summary>
        /// Occurs after an item is removed from the List.
        /// Gives a handle to the item that was removed, but it no longer
        /// has a meaningful index value or an option to cancel.
        /// </summary>
        public virtual event EventHandler<Individual<KeyValuePair<TKey, TValue>>> AfterItemRemoved;

        /// <summary>
        /// Fires the AfterItemRemoved event
        /// </summary>
        /// <param name="Item"></param>
        protected void OnAfterItemRemoved(KeyValuePair<TKey, TValue> Item)
        {
            if (AfterItemRemoved != null) AfterItemRemoved(this, new Individual<KeyValuePair<TKey, TValue>>(Item));
        }


        /// <summary>
        /// Occurs after an item is removed from the List.
        /// Gives a handle to the range that was removed, but it no longer
        /// has a meaningful index value or an option to cancel.
        /// </summary>
        public virtual event EventHandler<Collective<KeyValuePair<TKey, TValue>>> AfterRangeRemoved;

        /// <summary>
        /// Fires the AfterRangeRemoved event
        /// </summary>
        /// <param name="collection"></param>
        protected void OnAfterRangeRemoved(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            if (AfterRangeRemoved != null) AfterRangeRemoved(this, new Collective<KeyValuePair<TKey, TValue>>(collection));
        }


        #endregion

        #region Set
        /// <summary>
        /// Occurs before an item is set.  This event can cancel the set opperation.
        /// </summary>
        public event EventHandler<IndividualDictionaryCancel<TKey, TValue>> BeforeItemSet;

        /// <summary>
        /// Fires the BeforeItemSet event
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool OnBeforeItemSet(TKey Key, TValue Value)
        {
            if (BeforeItemSet == null) return false;
            IndividualDictionaryCancel<TKey, TValue> e = new IndividualDictionaryCancel<TKey, TValue>(Key, Value);
            BeforeItemSet(this, e);
            return e.Cancel;
        }

        /// <summary>
        /// Occurs after an item is successfully set
        /// </summary>
        public event EventHandler<IndividualDictionary<TKey, TValue>> AfterItemSet;

        /// <summary>
        /// Fires the AfterItemSet event
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void OnAfterItemSet(TKey Key, TValue Value)
        {
            if (AfterItemSet == null) return;
            IndividualDictionary<TKey, TValue> e = new IndividualDictionary<TKey, TValue>(Key, Value);
            AfterItemSet(this, e);
        }

        #endregion

        #endregion


       


    }
}
