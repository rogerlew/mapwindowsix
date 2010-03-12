using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Main
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event
    /// </summary>
    public class ComparisonCancel<T> : System.ComponentModel.CancelEventArgs
    {

         /// <summary>
        /// The protected System.Collections.Generic.IComparer&lt;T&gt; being used by this action
        /// </summary>
        private System.Comparison<T> _comparison;
        
      
        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inComparison">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action </param>
        public ComparisonCancel(System.Comparison<T> inComparison)
        {
            _comparison = inComparison;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the comparer being used in this action
        /// </summary>
        public System.Comparison<T> Comparison
        {
            get { return _comparison; }
            set { _comparison = value; }
        }

      
        #endregion
    }
}
