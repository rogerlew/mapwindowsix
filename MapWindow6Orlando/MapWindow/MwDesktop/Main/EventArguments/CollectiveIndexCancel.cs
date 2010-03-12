using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Main
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event
    /// </summary>
    public class CollectiveIndexCancel<T> : System.ComponentModel.CancelEventArgs
    {

        private System.Collections.Generic.IEnumerable<T> _collection;
        private int _index = -1;
      
        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inCollection">the IEnumerable&lt;T&gt; responsible for the event</param>
        /// <param name="inIndex">the Integer index associated with this event</param>
        public CollectiveIndexCancel(System.Collections.Generic.IEnumerable<T> inCollection, int inIndex)
        {
            _collection = inCollection;
            _index = inIndex;
        }

     

        #endregion

        #region Properties

        /// <summary>
        /// Gets the IEnumerable&lt;T&gt; collection involved in this event
        /// </summary>
        public System.Collections.Generic.IEnumerable<T> Collection
        {
            get { return _collection; }
            protected set { _collection = value; }
        }

        /// <summary>
        /// Gets the integer index in the IEventList where this event occured
        /// </summary>
        public int Index
        {
            get { return _index; }
            protected set { _index = value; }
        }
      
        #endregion
    }
}
