using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Text;

namespace MapWindow.Main
{
        /// <summary>
        /// An enumerator for an EventDictionary
        /// </summary>
        public class OrderedDictionaryEnumerator<TKey, TValue> : System.Collections.IDictionaryEnumerator, IEnumerator<KeyValuePair<TKey, TValue>>
        {
            #region Variables

            private IEnumerator<KeyValuePair<TKey, TValue>> _listEnum;

            #endregion

            #region Constructor

            /// <summary>
            /// This should not be used directly, but rather the EventDictionary.GetEnumerator() method
            /// should be used.
            /// </summary>
            /// <param name="inListEnumerator"></param>
            public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> inListEnumerator)
            {
                _listEnum = inListEnumerator;
                
            }

            #endregion


            #region Methods

            /// <summary>
            /// Disposes the interior List Enumerator
            /// </summary>
            public void Dispose()
            {
                // I doubt this does anything but I am wrapping it anyway.
                _listEnum.Dispose();
            }



            /// <summary>
            /// Advances to the next member in the order that they are stored according to the list
            /// </summary>
            /// <returns>True if there was another member in the list, false otherwise</returns>
            public bool MoveNext()
            {
                return _listEnum.MoveNext();
            }
            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                _listEnum.Reset();
            }


            #endregion


            #region Properties

            /// <summary>
            /// Gets the Current member from this enumeration
            /// </summary>
            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    return _listEnum.Current;
                }

            }

            /// <summary>
            /// An object version of the Entry to satisfy IDictionaryEnumerator
            /// </summary>
            public DictionaryEntry Entry
            {
                get { return new DictionaryEntry(_listEnum.Current.Key, _listEnum.Current.Value); }
            }

            /// <summary>
            /// A strongly typed Key, Value pair
            /// </summary>
            public KeyValuePair<TKey, TValue> EntryT
            {
                get { return _listEnum.Current; }
            }

            /// <summary>
            /// An object version of the Key to satisfy IDictionaryEnumerator
            /// </summary>
            public object Key
            {
                get { return _listEnum.Current.Key; }
            }

            /// <summary>
            /// A strongly typed Key
            /// </summary>
            public TKey KeyT
            {
                get { return _listEnum.Current.Key; }
            }


            /// <summary>
            /// An object version of the Value to satisfy IDictionaryEnumerator
            /// </summary>
            public object Value
            {
                get { return _listEnum.Current.Value; }
            }


            /// <summary>
            /// A strongly typed value member 
            /// </summary>
            public TValue ValueT
            {
                get { return _listEnum.Current.Value; }
            }



            #endregion

          

            #region Hidden

            // There is a public, type specific "Current" but it throws exceptions without this here
            object IEnumerator.Current
            {
                get { return Current; } 
                
            }

            #endregion
        }

        
    }
