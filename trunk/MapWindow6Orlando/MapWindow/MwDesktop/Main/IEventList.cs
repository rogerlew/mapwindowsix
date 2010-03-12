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
using System.Text;
using System.Collections;
using System.ComponentModel;
using MapWindow.Main;
namespace MapWindow.Main
{
    /// <summary>
    /// This describes a generic tool to help with keeping track of strong typed lists.
    /// The usual list, however, provides no event handling whatsoever.
    /// This list also provides an event for each of the major actions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventList<T> : ICollection<T>, IList<T>, IEnumerable<T>
    {

        #region Methods

        #region add

        /// <summary>
        /// Adds the elements of the specified collection to the end of the System.Collections.Generic.List&lt;T&gt;
        /// </summary>
        /// <param name="collection">collection: The collection whose elements should be added to the end of the System.Collections.Generic.List&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        void AddRange(System.Collections.Generic.IEnumerable<T> collection);


        #endregion

        #region BinarySearch

        /// <summary>
        /// Searches the entire sorted System.Collections.Generic.List&lt;T&gt; for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; implementation to use when comparing elements.-or-null to use the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default.</param>
        /// <returns>The zero-based index of item in the sorted System.Collections.Generic.List&lt;T&gt; , if item is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, if there is no larger element, the bitwise complement of System.Collections.Generic.List&lt;T&gt;.Count.</returns>
        /// <exception cref="System.InvalidOperationException">comparer is null, and the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default cannot find an implementation of the System.IComparable&lt;T&gt;generic interface or the System.IComparable interface for type T.</exception>
        int BinarySearch(T item, System.Collections.Generic.IComparer<T> comparer);


        /// <summary>
        /// Searches the entire sorted System.Collections.Generic.List&lt;T&gt; for an element using the default comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <returns>The zero-based index of item in the sorted System.Collections.Generic.List&lt;T&gt;, if item is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, if there is no larger element, the bitwise complement of System.Collections.Generic.List&lt;T&gt;.Count.</returns>
        /// <exception cref="System.InvalidOperationException">The default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default cannot find an implementation of the System.IComparable&lt;T&gt; generic interface or the System.IComparable interface for type T.</exception> 
        int BinarySearch(T item);


        /// <summary>
        /// Searches a range of elements in the sorted System.Collections.Generic.List&lt;T&gt; for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; implementation to use when comparing elements, or null to use the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default.</param>
        /// <returns></returns>
        int BinarySearch(int index, int count, T item, System.Collections.Generic.IComparer<T> comparer);

        #endregion


        /// <summary>
        /// Converts the elements in the current MapWindow.Interfaces.Framework.IEventList&lt;T&gt; to another type, and returns a list containing the converted elements.
        /// </summary>
        /// <typeparam name="TOutput">The output type to convert to</typeparam>
        /// <param name="converter">A System.Converter&lt;TInput,TOutput&gt; delegate that converts each element from one type to another type.</param>
        /// <returns>A MapWindow.Interfaces.Framework.IEventList&lt;T&gt; of the target type containing the converted elements from the current MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">converter is null.</exception>
        System.Collections.Generic.List<TOutput> ConvertAll<TOutput>(System.Converter<T, TOutput> converter);

        #region CopyTo

        /// <summary>
        /// Copies a range of elements from the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="index">The zero-based index in the source MapWindow.Interfaces.Framework.IEventList&lt;T&gt; at which copying begins</param>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from MapWindow.Interfaces.Framework.IEventList&lt;T&gt;. The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> index is less than 0.-or-arrayIndex is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.ArgumentException">index is equal to or greater than the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.Count of the source MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements from index to the end of the source MapWindow.Interfaces.Framework.IEventList&lt;T&gt; is greater than the available space from arrayIndex to the end of the destination array.</exception> 
        void CopyTo(int index, T[] array, int arrayIndex, int count);

        /// <summary>
        /// Copies the entire MapWindow.Interfaces.Framework.IEventList&lt;T&gt; to a compatible one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from MapWindow.Interfaces.Framework.IEventList&lt;T&gt;. The System.Array must have zero-based indexing.</param>
        /// <exception cref="System.ArgumentException">The number of elements in the source MapWindow.Interfaces.Framework.IEventList&lt;T&gt; is greater than the number of elements that the destination array can contain.</exception>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        void CopyTo(T[] array);

        #endregion

        /// <summary>
        /// Determines whether the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; contains elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the elements to search for.</param>
        /// <returns>true if the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; contains one or more elements that match the conditions defined by the specified predicate; otherwise, false.</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        bool Exists(System.Predicate<T> match);

        #region Find

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match"> The System.Predicate&lt;T&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">startIndex is outside the range of valid indexes for the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.-or-count is less than 0.-or-startIndex and count do not specify a valid section in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        /// <exception cref="System.ArgumentNullException">match is null</exception>
        int FindIndex(int startIndex, int count, System.Predicate<T> match);


        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that extends from the specified index to the last element.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">startIndex is outside the range of valid indexes for the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        /// <exception cref="System.ArgumentNullException">match is null.</exception> 
        int FindIndex(int startIndex, System.Predicate<T> match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        int FindIndex(System.Predicate<T> match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type T.</returns>
        /// <exception cref= "System.ArgumentNullException">match is null."</exception>
        T FindLast(System.Predicate<T> match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match"></param>
        /// <returns>The System.Predicate&lt;T&gt; delegate that defines the conditions of the element to search for.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">startIndex is outside the range of valid indexes for the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.-or-count is less than 0.-or-startIndex and count do not specify a valid section in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        /// <exception cref="System.ArgumentNullException">match is null.</exception> 
        int FindLastIndex(int startIndex, int count, System.Predicate<T> match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that extends from the first element to the specified index.
        /// </summary>
        /// <param name="startIndex"> The zero-based starting index of the backward search.</param>
        /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">: startIndex is outside the range of valid indexes for the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        int FindLastIndex(int startIndex, System.Predicate<T> match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception> 
        int FindLastIndex(System.Predicate<T> match);

        #endregion

        /// <summary>
        /// Performs the specified action on each element of the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.
        /// </summary>
        /// <param name="action"> The System.Action&lt;T&gt; delegate to perform on each element of the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</param>
        /// <exception cref="System.ArgumentNullException"> action is null.</exception>
        void ForEach(System.Action<T> action);

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.
        /// </summary>
        /// <param name="index">The zero-based MapWindow.Interfaces.Framework.IEventList&lt;T&gt; index at which the range starts.</param>
        /// <param name="count"> The number of elements in the range.</param>
        /// <returns>A shallow copy of a range of elements in the source MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.ArgumentException">index and count do not denote a valid range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        List<T> GetRange(int index, int count);

        #region IndexOf

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="item">The object to locate in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;. The value can be null for reference types.</param>
        /// <param name="index"> The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that starts at index and contains count number of elements, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"> index is outside the range of valid indexes for the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.-or-count is less than 0.-or-index and count do not specify a valid section in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        int IndexOf(T item, int index, int count);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that extends from the specified index to the last element.
        /// </summary>
        /// <param name="item">The object to locate in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;. The value can be null for reference types.</param>
        /// <param name="index"> The zero-based starting index of the search.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that extends from index to the last element, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is outside the range of valid indexes for the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        int IndexOf(T item, int index);

        #endregion

        #region Insert

        /// <summary>
        /// Inserts the elements of a collection into the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-index is greater than MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.Count.</exception>
        /// <exception cref="System.ArgumentNullException">collection is null.</exception> 
        void InsertRange(int index, System.Collections.Generic.IEnumerable<T> collection);

        #endregion

        #region LastIndexOf

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that contains count number of elements and ends at index, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is outside the range of valid indexes for the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.-or-count is less than 0.-or-index and count do not specify a valid section in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        int LastIndexOf(T item, int index, int count);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that extends from the first element to the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; that extends from the first element to index, if found; otherwise, –1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is outside the range of valid indexes for the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        int LastIndexOf(T item, int index);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the entire MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.
        /// </summary>
        /// <param name="item">The object to locate in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the last occurrence of item within the entire the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;, if found; otherwise, –1.</returns>
        int LastIndexOf(T item);

        #endregion


        #region Remove

        /// <summary>
        /// Removes the all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements removed from the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        int RemoveAll(System.Predicate<T> match);

        /// <summary>
        /// Removes a range of elements from the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.ArgumentException">index and count do not denote a valid range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        void RemoveRange(int index, int count);
        
        #endregion


        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        /// <exception cref="System.ArgumentException">index and count do not denote a valid range of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0.</exception>
        void Reverse(int index, int count);


        /// <summary>
        /// Reverses the order of the elements in the entire MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.
        /// </summary>
        void Reverse();

        /// <summary>
        /// Sorts the elements in the entire MapWindow.Interfaces.Framework.IEventList&lt;T&gt; using the specified System.Comparison&lt;T&gt;.
        /// </summary>
        /// <param name="comparison">The System.Comparison&lt;T&gt; to use when comparing elements.</param>
        /// <exception cref="System.ArgumentException">The implementation of comparison caused an error during the sort. For example, comparison might not return 0 when comparing an item with itself.</exception>
        /// <exception cref="System.ArgumentNullException">comparison is null.</exception> 
        void Sort(System.Comparison<T> comparison);


        /// <summary>
        /// Sorts the elements in a range of elements in MapWindow.Interfaces.Framework.IEventList&lt;T&gt; using the specified comparer.
        /// </summary>
        /// <param name="index"> The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; implementation to use when comparing elements, or null to use the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default.</param>
        /// <exception cref="System.ArgumentException">index and count do not specify a valid range in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.-or-The implementation of comparer caused an error during the sort. For example, comparer might not return 0 when comparing an item with itself.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0.</exception>
        /// <exception cref="System.InvalidOperationException"> comparer is null, and the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default cannot find implementation of the System.IComparable&lt;T&gt; generic interface or the System.IComparable interface for type T.</exception>
        void Sort(int index, int count, System.Collections.Generic.IComparer<T> comparer);

        /// <summary>
        /// Sorts the elements in the entire MapWindow.Interfaces.Framework.IEventList&lt;T&gt; using the specified comparer.
        /// </summary>
        /// <param name="comparer"> The System.Collections.Generic.IComparer&lt;T&gt; implementation to use when comparing elements, or null to use the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default.</param>
        /// <exception cref="System.ArgumentException">The implementation of comparer caused an error during the sort. For example, comparer might not return 0 when comparing an item with itself.</exception>
        /// <exception cref="System.InvalidOperationException">comparer is null, and the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default cannot find implementation of the System.IComparable&lt;T&gt; generic interface or the System.IComparable interface for type T.</exception>
        void Sort(System.Collections.Generic.IComparer<T> comparer);


        /// <summary>
        /// Sorts the elements in the entire MapWindow.Interfaces.Framework.IEventList&lt;T&gt; using the default comparer.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default cannot find an implementation of the System.IComparable&lt;T&gt; generic interface or the System.IComparable interface for type T.</exception>
        void Sort();

        /// <summary>
        /// Copies the elements of the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.</returns>
        T[] ToArray();

        /// <summary>
        /// Sets the capacity to the actual number of elements in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt;, if that number is less than a threshold value.
        /// </summary>
        void TrimExcess();

        /// <summary>
        /// Determines whether every element in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions to check against the elements.</param>
        /// <returns>true if every element in the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; matches the conditions defined by the specified predicate; otherwise, false. If the list has no elements, the return value is true.</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        bool TrueForAll(System.Predicate<T> match);


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <returns>
        /// The number of elements that the MapWindow.Interfaces.Framework.IEventList&lt;T&gt; can contain before resizing is required.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.Capacity is set to a value that is less than MapWindow.Interfaces.Framework.IEventList&lt;T&gt;.Count.</exception>
        int Capacity
        {
            get;
            set;
        }

       
      
        #endregion

        #region Add

        /// <summary>
        /// Occurs before an item is added to the List.
        /// There is no index yet specified because it will be added to the
        /// end of the list.
        /// </summary>
        event EventHandler<IndividualCancel<T>> BeforeItemAdded;


        /// <summary>
        /// Occurs before a range of items is added to the list.
        /// There is no index yet, but this event can be cancelled.
        /// </summary>
        event EventHandler<CollectiveCancel<T>> BeforeRangeAdded;

        /// <summary>
        /// Occurs after an item has already been added to the list.
        /// The index where the item was added is specified.
        /// </summary>
        event EventHandler<IndividualIndex<T>> AfterItemAdded;


        /// <summary>
        /// Occurs after a range has already been added to the list.
        /// This reveals the index where the beginning of the range
        /// was added, but cannot be canceled.
        /// </summary>
        event EventHandler<CollectiveIndex<T>> AfterRangeAdded;

        #endregion


        #region Insert

        /// <summary>
        /// Occurs before an item is inserted.  The index of the requested
        /// insertion as well as the item being inserted and an option to
        /// cancel the event are specified
        /// </summary>
        event EventHandler<IndividualIndexCancel<T>> BeforeItemInserted;


        /// <summary>
        /// Occurs before a range is inserted.  The index of the requested
        /// insertion location as well as the item being inserted and an option to
        /// cancel the event are provided in the event arguments
        /// </summary>
        event EventHandler<CollectiveIndexCancel<T>> BeforeRangeInserted;

       

        /// <summary>
        /// Occurs after an item is inserted.
        /// Shows the true index of the item after it was actually added.
        /// </summary>
        event EventHandler<IndividualIndex<T>> AfterItemInserted;


        /// <summary>
        /// Occurs after an item is inserted.
        /// Shows the true index of the item after it was actually added.
        /// </summary>
        event EventHandler<CollectiveIndex<T>> AfterRangeInserted;


        #endregion

        #region Remove

        /// <summary>
        /// Occurs before an item is removed from the List.
        /// Specifies the item, the current index and an option to cancel.
        /// </summary>
        event EventHandler<IndividualIndexCancel<T>> BeforeItemRemoved;

        /// <summary>
        /// Occurs before a range is removed from the List.
        /// Specifies the range, the current index and an option to cancel.
        /// </summary>
        event EventHandler<CollectiveIndexCancel<T>> BeforeRangeRemoved;

        /// <summary>
        /// Occurs after an item is removed from the List.
        /// Gives a handle to the item that was removed, but it no longer
        /// has a meaningful index value or an option to cancel.
        /// </summary>
        event EventHandler<Individual<T>> AfterItemRemoved;

        /// <summary>
        /// Occurs after an item is removed from the List.
        /// Gives a handle to the range that was removed, but it no longer
        /// has a meaningful index value or an option to cancel.
        /// </summary>
        event EventHandler<Collective<T>> AfterRangeRemoved;

        /// <summary>
        /// Occurs before all the elements that match a predicate are removed.
        /// Supplies an IEnumerable list in the event args of all the items
        /// that will match the expression.  This action can be cancelled.
        /// </summary>
        event EventHandler<CollectiveCancel<T>> BeforeAllMatchingRemoved;

        /// <summary>
        /// Occurs after all the elements that matched a predicate were 
        /// removed.  The values are the items that were successfully removed.
        /// The action has already happened, and so cannot be cancelled here.
        /// </summary>
        event EventHandler<Collective<T>> AfterAllMatchingRemoved;

        #endregion

        /// <summary>
        /// Occurs before the list is cleared and can cancel the event.
        /// </summary>
        event CancelEventHandler BeforeListCleared;

        /// <summary>
        /// Occurs after the list is cleared and this cannot be canceled.
        /// </summary>
        event EventHandler AfterListCleared;

        /// <summary>
        /// Occurs after a method that sorts or reorganizes the list
        /// </summary>
        event EventHandler ListChanged;

        #region Reverse

        /// <summary>
        /// Occurs before a specific range is reversed
        /// </summary>
        event EventHandler<CollectiveIndexCancel<T>> BeforeRangeReversed;

        /// <summary>
        /// Occurs after a specific range is reversed
        /// </summary>
        event EventHandler<CollectiveIndex<T>> AfterRangeReversed;

        /// <summary>
        /// Occurs before the entire list is reversed
        /// </summary>
        event CancelEventHandler BeforeListReversed;

        /// <summary>
        /// Occurs after the entire list is reversed
        /// </summary>
        event EventHandler AfterListReversed;

        /// <summary>
        /// Occurs just after the list or any sub portion
        /// of the list is sorted.  This event occurs in
        /// addition to the specific reversal case.
        /// </summary>
        event EventHandler AfterReversed;

        /// <summary>
        /// Occurs just before the list or any sub portion
        /// of the list is sorted.  This event occurs in
        /// addition to the specific reversal case.
        /// </summary>
        event CancelEventHandler BeforeReversed;


        #endregion


        #region Sort

        /// <summary>
        /// Occurs just before any of the specific sorting methodsin addition
        /// to the event associated with the specific method.
        /// </summary>
        event CancelEventHandler BeforeSort;


        /// <summary>
        /// Occurs after any of the specific sorting methods in addition
        /// to the event associated with the specific method.
        /// </summary>
        event EventHandler AfterSort;


        /// <summary>
        /// Occurs just before the entire list is sorted
        /// </summary>
        event EventHandler<CompareCancel<T>> BeforeListSorted;

     
        /// <summary>
        /// Occurs after the entire list has been sorted
        /// </summary>
        event EventHandler<Compare<T>> AfterListSorted;


        /// <summary>
        /// Occurs just before the Sort method that uses a System.Comparison&lt;T&gt;
        /// This event can cancel the action.
        /// </summary>
        event EventHandler<ComparisonCancel<T>> BeforeSortByComparison;


        /// <summary>
        /// Occurs just after the Sort method that uses a System.Comparison&lt;T&gt;
        /// </summary>
        event EventHandler<ComparisonArgs<T>> AfterSortByComparison;

        /// <summary>
        /// Occurs just before the Sort method that only sorts a specified range.
        /// This event can cancel the action.
        /// </summary>
        event EventHandler<CollectiveIndexCompareCancel<T>> BeforeRangeSorted;


        /// <summary>
        /// Occurs just after the Sort method that only sorts a specified range.
        /// </summary>
        event EventHandler<CollectiveIndexCompare<T>> AfterRangeSorted;
       


        #endregion


    }
}
