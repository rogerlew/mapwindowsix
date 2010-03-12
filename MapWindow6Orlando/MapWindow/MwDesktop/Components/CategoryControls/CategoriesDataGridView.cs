//********************************************************************************************************
// Product Name: MapWindow.Components.CategoriesViewer.dll Alpha
// Description:  The basic module for MapWindow.Components.CategoriesViewer version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.Components.CategoriesViewer.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/14/2009 4:12:16 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Components
{


    /// <summary>
    /// CategoriesDataGridView
    /// </summary>
    public class CategoriesDataGridView :DataGridView
    {
        #region Events

        #endregion

        #region Private Variables

        private FeatureCategoryCollection _categories;
        private bool _hasUniqueValues = false;
        private DataGridViewCheckBoxColumn _colShow;
        private DataGridViewTextBoxColumn _colSymbol;
        private DataGridViewTextBoxColumn _colValues;
        private DataGridViewTextBoxColumn _colLabels;
        private bool _columnsAdded = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CategoriesDataGridView
        /// </summary>
        public CategoriesDataGridView()
        {
            Initialize();
        }

        #endregion

        #region Methods


        /// <summary>
        /// Initializes the data grid view columns
        /// </summary>
        private void Initialize()
        {
            RowHeadersVisible = false;   
            AddColumns();          
        }

        #endregion

        /// <summary>
        /// Adds the columns to the data grid view.
        /// The columns are 'show', 'values', 'label', 'caption'
        /// </summary>
        private void AddColumns()
        {
            if (!_columnsAdded)
            {

                // 'show' check box column
                _colShow = new DataGridViewCheckBoxColumn();
                _colShow.HeaderText = "show";
                _colShow.Name = "showColumn";
                _colShow.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                Columns.Add(_colShow);

                // 'symbol preview' image column
                _colSymbol = new DataGridViewTextBoxColumn();
                _colSymbol.HeaderText = "symbol";
                _colSymbol.Name = "symbolColumn";
                Columns.Add(_colSymbol);

                // 'values ' check box column
                _colValues = new DataGridViewTextBoxColumn();
                _colValues.HeaderText = "values";
                _colValues.Name = "valuesColumn";
                Columns.Add(_colValues);

                // 'labels' check box column
                _colLabels = new DataGridViewTextBoxColumn();
                _colLabels.HeaderText = "labels";
                _colLabels.Name = "labelsColumn";
                Columns.Add(_colLabels);

                _columnsAdded = true;
            }
        }

        #region Properties

        /// <summary>
        /// Gets or sets the collection of categories displayed in this table
        /// </summary>
        public FeatureCategoryCollection Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        /// <summary>
        /// indicates whether the categories displayed in this table have unique values or 
        /// ranges
        /// </summary>
        public Boolean HasUniqueValues
        {
            get { return _hasUniqueValues; }
            set 
            { 
                _hasUniqueValues = value;
                if (_hasUniqueValues)
                {
                    _colValues.HeaderText = "value";
                }
                else
                {
                    _colValues.HeaderText = "values";
                }
            }
        }

        #endregion

        #region Protected Methods

        

        #endregion

    }
}
