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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 12:00:28 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using MapWindow.Projections;
using MapWindow.Serialization;
namespace MapWindow.Data
{


    /// <summary>
    /// DataSet
    /// </summary>
    public class DataSet : IDataSet
    {
        #region Private Variables
        private string _name;
        private ProjectionInfo _projection;
        private SpaceTimeSupport _spaceTimeSupport;
        private string _typeName;
        private IDataSet _internalDataSet;
     
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DataSet
        /// </summary>
        protected DataSet()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Overridable in specific classes if necessary
        /// </summary>
        public virtual void Close()
        {
            if (_internalDataSet != null) _internalDataSet.Close();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string name
        /// </summary>
        public string Name
        {
            get 
            {
                if (_internalDataSet != null) return _internalDataSet.Name;
                return _name; 
            }
            set 
            {
                if (_internalDataSet != null)
                {
                    _internalDataSet.Name = value;
                    return;
                }
                _name = value;

            }
        }

        /// <summary>
        /// Gets or set the projection string
        /// </summary>
        public ProjectionInfo Projection
        {
            get
            {
                if (_internalDataSet != null) return _internalDataSet.Projection;
                return _projection;
            }
            set
            {
                if (_internalDataSet != null)
                {
                    _internalDataSet.Projection = value;
                    return;
                }
                _projection = value;
            }
        }

        /// <summary>
        /// Gets an enumeration specifying if this data supports time, space, both or neither.
        /// </summary>
        public SpaceTimeSupport SpaceTimeSupport
        {
            get
            {
                if (_internalDataSet != null) return _internalDataSet.SpaceTimeSupport;
                return _spaceTimeSupport;
            }
            set
            {
                if (_internalDataSet != null)
                {
                    _internalDataSet.SpaceTimeSupport = value;
                    return;
                }
                _spaceTimeSupport = value; 
            }
        }

        /// <summary>
        /// Gets or sets the string type name that identifies this dataset
        /// </summary>
        public string TypeName
        {
            get
            {
                if (_internalDataSet != null) return _internalDataSet.TypeName;
                return _typeName;
            }
            set
            {
                if (_internalDataSet != null)
                {
                    _internalDataSet.TypeName = value;
                    return;
                }
                _typeName = value;
            }
        }

        /// <summary>
        /// Gets or sets the internal dataset
        /// </summary>
        [Serialize("InternalDataSet")]
        protected IDataSet InternalDataSet
        {
            get { return _internalDataSet; }
            set { _internalDataSet = value; }
        }

        #endregion



    }
}
