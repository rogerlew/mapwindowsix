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
using System.Collections.Specialized;
namespace MapWindow.Main
{
    /// <summary>
    /// A Dictionary that can deal with some ordering methods
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>
    {

        #region Variables

      
        private Dictionary<TKey, TValue> _dictionary;
        private List<KeyValuePair<TKey, TValue>> _list;

        #endregion

        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderedDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
            _list = new List<KeyValuePair<TKey, TValue>>();
        }

        #endregion

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value"> The value of the element to add. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentException">An element with the same key already exists in the System.Collections.Generic.Dictionary &lt;TKey,TValue&gt;."</exception>
        /// <exception cref="System.ArgumentNullException">key is null.</exception> 
        /// <returns>Integer, the index where the latest member was added.  Returns -1 if canceled</returns>
        public virtual int Add(TKey key, TValue value)
        {
            KeyValuePair<TKey, TValue> kvp = new KeyValuePair<TKey, TValue>(key, value);
            _dictionary.Add(key, value);
            _list.Add(new KeyValuePair<TKey, TValue>(key, value));
            return Count - 1;
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            Add(key, value);
        }

        /// <summary>
        /// Adds a key value pair together in one item
        /// </summary>
        /// <param name="item">A strong typed KeyValuePair&lt;TKey, TValue&gt; pair to add to the EventDictionary </param>
        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionary.Add(item.Key, item.Value);
            _list.Add(item);
        }

        /// <summary>
        /// Removes all keys and values from the EventDictionary&lt;TKey,TValue&gt;.
        /// </summary>
        public virtual void Clear()
        {  
            _dictionary.Clear();
            _list.Clear();
        }

        /// <summary>
        /// Checks to determine if the specified key,value pair exists in the EventDictionary&lt;TKey,TValue&gt;.
        /// </summary>
        /// <param name="item">The TKey, TValue pair to check for</param>
        /// <returns>true if the item is found within this EventDictionary&lt;TKey,TValue&gt;</returns>
        public virtual bool Contains(KeyValuePair<TKey, TValue> item)
        {
            OrderedDictionaryEnumerator<TKey, TValue> Pair = GetEnumerator();
            while (Pair.MoveNext())
            {
                if ((object)item.Key == (object)Pair.Current.Key && (object)item.Value == (object)Pair.Current.Value) return true;
                //if ((object)Pair.Current.Key == (object)item.Key && (object)Pair.Current.Value == (object)item.Value) return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the EventDictionary&lt;TKey,TValue&gt; contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the EventDictionary&lt;TKey,TValue&gt;</param>
        /// <returns>true if the EventDictionary&lt;TKey,TValue&gt; contains an element with the specified key; otherwise, false.</returns>
        /// <exception cref="System.ArgumentNullException">key is null</exception>
        public virtual bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Determines whether the EventDictionary&lt;TKey,TValue&gt; contains a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the EventDictionary&lt;TKey,TValue&gt;. The value can be null for reference types.</param>
        /// <returns>true if the EventDictionary&lt;TKey,TValue&gt; contains an element with the specified value; otherwise, false.</returns>
        public virtual bool ContainsValue(TValue value)
        {
            Dictionary<TKey, TValue>.Enumerator Pair = _dictionary.GetEnumerator();
            while (Pair.MoveNext())
            {
                if ((object)Pair.Current.Value == (object)value) return true;
            }
            return false;
        }

        /// <summary>
        /// This attempts to copy values from this EventDictionary in the sequence
        /// given by an enumerator into the specified array, starting at the
        /// arrayIndex specified, so long as the starting index is less than
        /// or equal to the count of the specified array.
        /// </summary>
        /// <param name="array">The Array of KeyValuePairs</param>
        /// <param name="arrayIndex">The integer arrayIndex to start copying values to</param>
        /// <exception cref="System.IndexOutOfRangeException">The specified arrayIndex was outside the bounds of the specified array</exception>
        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            // We allow equal to count as a way to append to the array
            if (arrayIndex > array.GetUpperBound(0) + 1 || arrayIndex < 0)
            {
                throw new System.IndexOutOfRangeException("The specified arrayIndex was outside the bounds of the specified array");
            }
            // Resize the original array to fit the values from this enumerator
            if (arrayIndex + _dictionary.Count - 1 > array.GetUpperBound(0))
            {
                // resize preserve
                KeyValuePair<TKey, TValue>[] temp = new KeyValuePair<TKey, TValue>[arrayIndex + _dictionary.Count];
                for (int I = 0; I < arrayIndex; I++)
                {
                    temp[I] = array[I];
                }
                array = temp;
            }
            int J = arrayIndex;
           
            for (int I = 0; I < _list.Count; I++)
            {
                array[J] = _list[I];
                J++;
            }

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
        public virtual KeyValuePair<TKey, TValue> this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                if (index < 0 || index >= _list.Count)
                {
                    throw new IndexOutOfRangeException("The specified index was either less than 0 or greater than the the count - 1.");
                }

                TValue val;
                if (_dictionary.TryGetValue(value.Key, out val) == true)
                {
                    // the key exists in the dictinoary already, so this will set the value associated with the key
                    _dictionary[value.Key] = value.Value;

                }
                else
                {
                    _dictionary.Add(value.Key, value.Value);
                }
                _list[index] = value;
            }
        }

       

        

        /// <summary>
        /// Obtains a value in the dictionary based on a key.  This happens without consulting the
        /// index at all, and unlike TryGetValue, will not gracefully check to ensure that it is 
        /// possible.  This is faster if you know for a fact that the value is present, but
        /// be prepared to catch exceptions.
        /// </summary>
        /// <param name="key">The Tkey key to search for.</param>
        /// <returns>The TValue to obtain a value for. </returns>
        public virtual TValue GetValue(TKey key)
        {
            return _dictionary[key];
        }


        /// <summary>
        /// Sets the value associated with a key that is already in the dictionary,
        /// or adds the pair to the dictionary if they are not already a member.
        /// </summary>
        /// <param name="key">The key currently found in the index</param>
        /// <param name="value">the value to be changed</param>
        public virtual void SetValue(TKey key, TValue value)
        {
            TValue val;
            KeyValuePair<TKey, TValue> item;
            if (_dictionary.TryGetValue(key, out val))
            {
                // we successfully obtained a value, so now get the old pair in the list
                val = _dictionary[key];
                item = new KeyValuePair<TKey, TValue>(key, val);
                int index = _list.IndexOf(item); // if it is in the dictionary, it should be in the list
                item = new KeyValuePair<TKey, TValue>(key, value);
                _dictionary[key] = value;
                _list[index] = item;
            }
            _dictionary.Add(key, value);

            item = new KeyValuePair<TKey, TValue>(key, value);
            _list.Add(item);

        }


        /// <summary>
        /// This enumerator iterates through the index in sequence as if you were moving through
        /// the members in index order from the list.  It should satisfy IEnumerator, IDictionaryEnumerator
        /// and IEnumerator&lt;KeyValuePair&lt;&lt;TKey, TValue&gt;&gt;
        /// </summary>
        /// <returns>An Enumerator for moving through an EventDictionary</returns>
        public virtual OrderedDictionaryEnumerator<TKey, TValue> GetEnumerator()
        {
            return new OrderedDictionaryEnumerator<TKey, TValue>(_list.GetEnumerator());
        }

        /// <summary>
        /// Retrieves the current index value of the specified key
        /// </summary>
        /// <param name="key">The key to find the value of</param>
        /// <returns></returns>
        public virtual int IndexOf(TKey key)
        {
            return _list.IndexOf(new KeyValuePair<TKey, TValue>(key, _dictionary[key]));
        }

        /// <summary>
        /// Keys allow you to track specific members, even if they move around in the list
        /// </summary>
        /// <param name="index">The index in the list, which keeps sequential track of the members</param>
        /// <param name="key">The key, which uniquely identifies the value member</param>
        /// <param name="value">The value, which is of a specific type associated with this EventDictionary</param>
        public virtual void Insert(int index, TKey key, TValue value)
        {
            if (index > Count || index < Count)
                throw new ArgumentOutOfRangeException("Index was out of range.");

            _dictionary.Add(key, value);
            _list.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <summary>
        /// Keys allow you to track specific members, even if they move around in the list
        /// </summary>
        /// <param name="index">The index in the list, which keeps sequential track of the members</param>
        /// <param name="key">This should be of type TKey, and is the identifying key for the key value pair</param>
        /// <param name="value">The value, which is of a type TValue</param>
        public virtual void Insert(int index, object key, object value)
        {

            if (key is TKey && value is TValue)
            {
                Insert(index, (TKey)key, (TValue)value);
                
            }
            else
            {
                throw new NotImplementedException("The type of key or value did not match the specific type for this dictionary.");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if the element is successfully removed; otherwise, false. This method also returns false if key was not found in the original System.Collections.Generic.IDictionary&lt;TKey,TValue&gt;.</returns>
        public virtual bool Remove(KeyValuePair<TKey, TValue> item)
        {
            
            bool res = _dictionary.Remove(item.Key);
            
            return res;

        }

        /// <summary>
        /// Removes the value with the specified key from the EventDictionary&lt;TKey,TValue&gt;
        /// </summary>
        /// <param name="key">The key of the element to remove</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method returns false if key is not found in the EventDictionary&lt;TKey,TValue&gt;</returns>
        public virtual bool Remove(TKey key)
        {
            TValue val = _dictionary[key];
            KeyValuePair<TKey, TValue> item = new KeyValuePair<TKey, TValue>(key, val);
            bool res = _dictionary.Remove(key);
           
            return res;
        }

        /// <summary>
        /// Tries to retrieve a value given a specified key by setting the value of the
        /// out TValue value parameter.  I guess if it says "try" then it won't throw
        /// errors if the member isn't present.
        /// </summary>
        /// <param name="key">The key of the value to obtain</param>
        /// <param name="value">the out value of the key to obtain.</param>
        /// <returns>true if the retrieval was successful, false otherwise</returns>
        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        

        #region Properties

        /// <summary>
        /// Gets the number of key/value pairs contained in the EventDictionary&lt;TKey,TValue&gt;
        /// </summary>
        /// <returns>
        /// The number of key/value pairs contained in the EventDictionary&lt;TKey,TValue&gt;
        /// </returns>
        public virtual int Count
        {
            get { return _dictionary.Count; }
        }

        /// <summary>
        /// This always returns false
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a collection containing the keys in the EventDictionary&lt;TKey,TValue&gt;
        /// </summary>
        public virtual ICollection<TKey> Keys
        {
            get { return _dictionary.Keys; }
        }

       

        /// <summary>
        /// Gets a collection containing the values in the EventDictionary&lt;TKey,TValue&gt;
        /// </summary>
        /// <returns>a collection containing the values in the EventDictionary&lt;TKey,TValue&gt;</returns>
        public virtual ICollection<TValue> Values
        {
            get { return _dictionary.Values; }
        }


        #endregion

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="index"></param>
        public virtual void RemoveAt(int index)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }


        #region IDictionary Members

        /// <summary>
        /// This takes objects to satisfy IDictionary, but will throw an exception if the type is incorrect.
        /// </summary>
        /// <param name="key">The key of type TKey.  This will take objects, but throw an error if the type is wrong.</param>
        /// <param name="value">The Value of type TValue.  This will take objects, but throw an error if the type is wrong.</param>
        /// <exception cref="ArgumentException">The key or value specified were not of the specific type required by this generic OrderedDictionary</exception>
        public virtual void Add(object key, object value)
        {
            if (key is TKey && value is TValue)
            {
                Add((TKey)key, (TValue)value);
            }
            else
            {
                throw new ArgumentException("The key or value specified were not of the specific type required by this generic OrderedDictionary");
            }
        }

        /// <summary>
        /// Checks to determine if the specified key,value pair exists in the EventDictionary&lt;TKey,TValue&gt;.
        /// </summary>
        /// <param name="item">The TKey, TValue pair to check for</param>
        /// <returns>true if the item is found within this EventDictionary&lt;TKey,TValue&gt;</returns>
        public virtual bool Contains(object item)
        {
            if (item is KeyValuePair<TKey, TValue>)
            {
                KeyValuePair<TKey, TValue> tItem = (KeyValuePair<TKey, TValue>)item;
                // it is faster to hunt for a key and compare against the value to test this.
                TKey key = tItem.Key;
                TValue val = tItem.Value;
                if (_dictionary.TryGetValue(tItem.Key, out val) == false)
                {
                    // The key did not exist in the dictionary, so the pair definitely did not exist
                    return false;
                }
                else
                {
                    // The key existed, so now compare the value
                    if ((object)val == (object)tItem.Value)
                    {
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                throw new ArgumentException("The item specified was not the <Key, Value> Pair required by this generic OrderedDictionary");
            }
        }

      

        /// <summary>
        /// For the basic OrderedDictionary&lt;TKey, TValue&gt; this always returns false.
        /// </summary>
        public virtual bool IsFixedSize
        {
            get { return false; }
        }

       

        /// <summary>
        /// The will search the OrderedDictionary&lt;TKey, TValue&gt; and remove the member that has a key that matches the specified key.
        /// </summary>
        /// <param name="key">Despite accepting any object to satisfy IDictionary, this must be of type TKey.</param>
        /// <exception cref="System.ArgumentException">The key specified were not of the specific type required by this generic OrderedDictionary</exception>
        /// <exception cref="System.ApplicationException">The key specified was not found in the dictionary.</exception>
        public virtual void Remove(object key)
        {
            if (key is TKey)
            {
                TValue val;
                if (_dictionary.TryGetValue((TKey)key, out val) == false)
                {
                    KeyValuePair<TKey, TValue> item = new KeyValuePair<TKey, TValue>((TKey)key, val);
                    _dictionary.Remove((TKey)key);
                    _list.Remove(item);
                }
               
            }
            else
            {
                throw new ArgumentException("The key specified were not of the specific type required by this generic OrderedDictionary");
            }
        }

        /// <summary>
        /// This is provided for user friendly access to non-integer based keys.  Imagine storing a set of values
        /// based on a name.  This would allow quick retrieval of the value based on the name.  Using integer here
        /// will conflict with the this[int Index] method.
        /// </summary>
        /// <param name="key">The string key to access.  Be warned, using integer keys may cause a conflict.</param>
        /// <returns></returns>
        [Obsolete("Use the GetValue(TKey key) and SetValue(TKey key) methods instead.  Otherwise integer keys will not work correctly.")]
        public virtual TValue this[TKey key]
        {
            get
            {
                if (key.GetType() == typeof(int))
                {
                    // If the keys are integers, we have to assume they meant to access the index, not the key.
                    // For clarity, GetIndex and GetValue are provided.
                    int index = int.Parse(key.ToString());
                    return GetIndex(index).Value; 
                }
                else
                {
                    return _dictionary[key];
                }
            }
            set
            {
                // if we are here, they are sending a TValue, so assume they are trying to use the integer key
                
                SetValue(key, value);
                    
            }
        }

        
      
    


      

     
        #endregion

        #region ICollection Members

        /// <summary>
        /// This attempts to copy values from this EventDictionary in the sequence
        /// given by an enumerator into the specified array, starting at the
        /// arrayIndex specified, so long as the starting index is less than
        /// or equal to the count of the specified array.
        /// </summary>
        /// <param name="array">The Array of KeyValuePairs</param>
        /// <param name="arrayIndex">The integer arrayIndex to start copying values to</param>
        /// <exception cref="System.IndexOutOfRangeException">The specified arrayIndex was outside the bounds of the specified array</exception>
        public virtual void CopyTo(Array array, int arrayIndex)
        {
            // We allow equal to count as a way to append to the array
            if (arrayIndex > array.GetUpperBound(0) + 1 || arrayIndex < 0)
            {
                throw new System.IndexOutOfRangeException("The specified arrayIndex was outside the bounds of the specified array");
            }
            // Resize the original array to fit the values from this enumerator
            if (arrayIndex + _list.Count - 1 > array.GetUpperBound(0))
            {
                // resize preserve
                KeyValuePair<TKey, TValue>[] temp = new KeyValuePair<TKey, TValue>[arrayIndex + _dictionary.Count];
                for (int I = 0; I < arrayIndex; I++)
                {
                    temp[I] = (KeyValuePair<TKey, TValue>)array.GetValue(I);
                }
                array = temp;
            }
            int J = arrayIndex;
            for (int I = 0; I < _list.Count; I++)
            {
                array.SetValue(_list[I], J);
                J++;
            }
           
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public virtual bool IsSynchronized
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public virtual object SyncRoot
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

       


        /// <summary>
        /// Gets a pair based on it's location in the indexed list
        /// </summary>
        /// <param name="index">The integer index of the pair to retrieve</param>
        /// <returns>A Key-Value pair</returns>
        public virtual KeyValuePair<TKey, TValue> GetIndex(int index)
        {
            return _list[index];
        }

        /// <summary>
        /// Sets a pair based on the index value
        /// </summary>
        /// <param name="index">The numerical index being used.</param>
        /// <param name="item">The key value pair to assign to the specified index</param>
        /// <exception cref="System.IndexOutOfRangeException">The specified index was either less than 0 or greater than the the count - 1.</exception>
        public virtual void SetIndex(int index, KeyValuePair<TKey, TValue> item)
        {
            if (index < 0 || index > _list.Count)
            {
                throw new IndexOutOfRangeException("The index specified was not in the range.");
            }
            TValue val;
            if (_dictionary.TryGetValue(item.Key, out val) == true)
            {
                // the key exists in the dictinoary already, so this will set the value associated with the key
                _dictionary[item.Key] = item.Value;

            }
            else
            {
                _dictionary.Add(item.Key, item.Value);
            }
            _list[index] = item;
        }

    }
}
