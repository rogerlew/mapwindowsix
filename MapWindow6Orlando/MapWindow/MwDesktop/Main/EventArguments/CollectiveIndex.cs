using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Main
{
    
    /// <summary>
    /// Carries event arguments for the generic IEventList
    /// </summary>
    public class CollectiveIndex<T> : System.EventArgs
    {

        private System.Collections.Generic.IEnumerable<T> _collection;
        private int _index = -1;
      

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inCollection">The IEnumerable&lt;T&gt; specified during the event"/></param>
        /// <param name="inIndex">The integer index associated with this event</param>
        public CollectiveIndex(System.Collections.Generic.IEnumerable<T> inCollection, int inIndex)
        {
            _index = inIndex;
            _collection = inCollection;
        }

       

     
      
        #endregion

        #region Properties

        /// <summary>
        /// Gets the IEnumerable&lt;T&gt; collection of items involved in this event
        /// </summary>
        public System.Collections.Generic.IEnumerable<T> Collection
        {
            get { return _collection; }
            protected set { _collection = value; }
        }

        /// <summary>
        /// Gets the index in the list where the event is associated
        /// </summary>
        public int Index
        {
            get { return _index; }
            protected set { _index = value; }
        }

        #endregion

    }
}
