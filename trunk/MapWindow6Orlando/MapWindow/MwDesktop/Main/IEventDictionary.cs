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
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using MapWindow.Main;
using System.ComponentModel;
namespace MapWindow.Main
{
    /// <summary>
    /// Represents a nongeneric collection of key/value pairs.
    /// </summary>
    public interface IEventDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {

        /// <summary>
        /// Determines whether the IEventDictionary&lt;TKey,TValue&gt; contains a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the IEventDictionary&lt;TKey,TValue&gt;. The value can be null for reference types.</param>
        /// <returns>true if the IEventDictionary&lt;TKey,TValue&gt; contains an element with the specified value; otherwise, false.</returns>
        bool ContainsValue(TValue value);

        /// <summary>
        /// Two separate forms of count exist and are ambiguous so this provides a single new count
        /// </summary>
        new int Count
        {
            get;
        }

        /// <summary>
        /// Gets or sets the specific KeyValuePair for the specified index
        /// </summary>
        /// <param name="index">The integer index representing the order in the list</param>
        /// <returns>A KeyValuePair that is currently stored at the specified index </returns>
        KeyValuePair<TKey, TValue> this[int index]
        {
            get;
            set;
        }

        #region Events

        #region Add

        /// <summary>
        /// Occurs before an item is added to the List.
        /// There is no index yet specified because it will be added to the
        /// end of the list.
        /// </summary>
        event EventHandler<IndividualCancel<KeyValuePair<TKey, TValue>>> BeforeItemAdded;


        /// <summary>
        /// Occurs before a range of items is added to the list.
        /// There is no index yet, but this event can be cancelled.
        /// </summary>
        event EventHandler<CollectiveCancel<KeyValuePair<TKey, TValue>>> BeforeRangeAdded;

        /// <summary>
        /// Occurs after an item has already been added to the list.
        /// The index where the item was added is specified.
        /// </summary>
        event EventHandler<IndividualIndex<KeyValuePair<TKey, TValue>>> AfterItemAdded;


        /// <summary>
        /// Occurs after a range has already been added to the list.
        /// This reveals the index where the beginning of the range
        /// was added, but cannot be canceled.
        /// </summary>
        event EventHandler<CollectiveIndex<KeyValuePair<TKey, TValue>>> AfterRangeAdded;

        #endregion

        #region Clear

        /// <summary>
        /// Occurs before a clear action, allowing the event to be canceled.
        /// </summary>
        event CancelEventHandler BeforeClearing;

        /// <summary>
        /// Occurs after a clear action, allowing the event to be cancled
        /// </summary>
        event EventHandler AfterClearing;
        
        #endregion

        /// <summary>
        /// Occurs after a method that changes either the order or the members of this EventDictionary
        /// </summary>
        event EventHandler ListChanged;


        #region Insert

        /// <summary>
        /// Occurs before an item is inserted.  The index of the requested
        /// insertion as well as the item being inserted and an option to
        /// cancel the event are specified
        /// </summary>
        event EventHandler<IndividualIndexCancel<KeyValuePair<TKey, TValue>>> BeforeItemInserted;


        /// <summary>
        /// Occurs before a range is inserted.  The index of the requested
        /// insertion location as well as the item being inserted and an option to
        /// cancel the event are provided in the event arguments
        /// </summary>
        event EventHandler<CollectiveIndexCancel<KeyValuePair<TKey, TValue>>> BeforeRangeInserted;



        /// <summary>
        /// Occurs after an item is inserted.
        /// Shows the true index of the item after it was actually added.
        /// </summary>
        event EventHandler<IndividualIndex<KeyValuePair<TKey, TValue>>> AfterItemInserted;


        /// <summary>
        /// Occurs after an item is inserted.
        /// Shows the true index of the item after it was actually added.
        /// </summary>
        event EventHandler<CollectiveIndex<KeyValuePair<TKey, TValue>>> AfterRangeInserted;

        #endregion


        #region Remove

        /// <summary>
        /// Occurs before an item is removed from the List.
        /// Specifies the item, the current index and an option to cancel.
        /// </summary>
        event EventHandler<IndividualIndexCancel<KeyValuePair<TKey, TValue>>> BeforeItemRemoved;

        /// <summary>
        /// Occurs before a range is removed from the List.
        /// Specifies the range, the current index and an option to cancel.
        /// </summary>
        event EventHandler<CollectiveIndexCancel<KeyValuePair<TKey, TValue>>> BeforeRangeRemoved;

        /// <summary>
        /// Occurs after an item is removed from the List.
        /// Gives a handle to the item that was removed, but it no longer
        /// has a meaningful index value or an option to cancel.
        /// </summary>
        event EventHandler<Individual<KeyValuePair<TKey, TValue>>> AfterItemRemoved;

        /// <summary>
        /// Occurs after an item is removed from the List.
        /// Gives a handle to the range that was removed, but it no longer
        /// has a meaningful index value or an option to cancel.
        /// </summary>
        event EventHandler<Collective<KeyValuePair<TKey, TValue>>> AfterRangeRemoved;

        /// <summary>
        /// Occurs before all the elements that match a predicate are removed.
        /// Supplies an IEnumerable list in the event args of all the items
        /// that will match the expression.  This action can be cancelled.
        /// </summary>
        event EventHandler<CollectiveCancel<KeyValuePair<TKey, TValue>>> BeforeAllMatchingRemoved;

        /// <summary>
        /// Occurs after all the elements that matched a predicate were 
        /// removed.  The values are the items that were successfully removed.
        /// The action has already happened, and so cannot be cancelled here.
        /// </summary>
        event EventHandler<Collective<KeyValuePair<TKey, TValue>>> AfterAllMatchingRemoved;

        #endregion

        #region Set
        /// <summary>
        /// Occurs before an item is set.  This event can cancel the set opperation.
        /// </summary>
        event EventHandler<IndividualDictionaryCancel<TKey, TValue>> BeforeItemSet;

        /// <summary>
        /// Occurs after an item is successfully set
        /// </summary>
        event EventHandler<IndividualDictionary<TKey, TValue>> AfterItemSet;
        #endregion

        #endregion

    }
}
