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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/16/2009 11:35:54 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;


namespace MapWindow.Data
{


    /// <summary>
    /// AttributePager
    /// </summary>
    public class AttributePager : IEnumerable<DataTable>, IEnumerator<DataTable>
    {
        #region Private Variables

        private readonly IAttributeSource _source;
        private int _pageSize;
        private readonly int _numRows;
        private int _pageIndex;
        private DataTable _currentTable;

        #endregion

        /// <summary>
        /// Creates a new instance of AttributePager
        /// </summary>
        public AttributePager(IAttributeSource source, int pageSize)
        {
            _numRows = source.NumRows();
            _source = source;
            _pageSize = pageSize;
        }

        /// <summary>
        /// Gets the current table
        /// </summary>
        public DataTable Current
        {
            get { return _currentTable; }
        }

        /// <summary>
        /// Returns the page that the specified row is on
        /// </summary>
        /// <param name="rowIndex">The integer row index</param>
        /// <returns>The page of the row in question</returns>
        public int PageOfRow(int rowIndex)
        {
            return (int)Math.Floor((double) rowIndex/_pageSize);
        }

        public void Dispose()
        {
            
        }

        object System.Collections.IEnumerator.Current
        {
            get { return _currentTable; }
        }

        public bool MoveNext()
        {
            _pageIndex += 1;
            if(_pageIndex >= NumPages()) return false;
            _currentTable = this[_pageIndex];
            return true;
        }

        public void Reset()
        {
            _currentTable = null;
            _pageIndex = -1;
        }


        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the starting row index of the current page.
        /// </summary>
        public int StartIndex
        {
            get { return _pageIndex*_pageSize; }
        }

        /// <summary>
        /// Gets the pages size as a count of the number of rows each data table page should hold
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            protected set { _pageSize = value; }
        }

        /// <summary>
        /// The integer number of pages
        /// </summary>
        /// <returns>the number of pages</returns>
        public int NumPages()
        {
            return (int)Math.Ceiling((double)_numRows/_pageSize);
        }

        /// <summary>
        /// Gets the number of rows on the specified page.
        /// </summary>
        /// <param name="pageindex">The page index</param>
        /// <returns>The number of rows that should be on that page.</returns>
        public int RowCount(int pageindex)
        {
            if(pageindex == NumPages() - 1) return _numRows%_pageSize; 
            return PageSize;
        }

        /// <summary>
        /// This returns the data table for the corresponding page index, but also sets the 
        /// Pager so that it is sitting on the specified page index.
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public DataTable this[int pageIndex]
        {
            get 
            {
                if(_pageIndex != pageIndex || _currentTable == null)
                {
                    _currentTable = _source.GetAttributes(pageIndex * PageSize, RowCount(pageIndex));
                    _pageIndex = pageIndex;
                }
                return _currentTable;
            }
        }

        /// <summary>
        /// Loads the appropriate page if it isn't loaded already and returns the DataRow that
        /// matches the specified index
        /// </summary>
        /// <param name="rowIndex">The integer row index</param>
        /// <returns>The DataRow</returns>
        public DataRow Row(int rowIndex)
        {
            int page = PageOfRow(rowIndex);
            if(_pageIndex != page || _currentTable == null)
            {
                _currentTable = _source.GetAttributes(page * PageSize, RowCount(page));
                _pageIndex = page;
            }
            return _currentTable.Rows[rowIndex % PageSize];
        }


        #endregion




        #region IEnumerable<DataTable> Members

        public IEnumerator<DataTable> GetEnumerator()
        {
            return this;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }

        #endregion
    }
}
