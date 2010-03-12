//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
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
// The Initial Developer of this Original Code is Ted Dunsford. Created in 2007.
// 
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Modified to support VersionID in July 2008 by Ted Dunsford
//********************************************************************************************************
using System.Collections.Generic;

namespace MapWindow.Geometries
{
    /// <summary>
    /// An ICoordinateSequence based on a list instead of an array.
    /// </summary>
    public class CoordinateListSequence: ICoordinateSequence
    {
        #region Variables

        private List<Coordinate> _internalList;
        private int _versionID;

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new instance of a CoordinateListSequence
        /// </summary>
        /// <param name="coordinate"></param>
        public CoordinateListSequence(Coordinate coordinate)
        {
            List<Coordinate> list = new List<Coordinate>();
            list.Add(coordinate);
            Configure(list);
         
        }

        /// <summary>
        /// Creates a new instance of a CoordinateListSequence
        /// </summary>
        public CoordinateListSequence()
        {
            Configure(new List<Coordinate>());
        }

       

        /// <summary>
        /// Creates a new instance of a CoordinateListSequence
        /// </summary>
        /// <param name="coordinates">If this is a List of ICoordinates, the sequence will be a shallow list.  
        /// Otherwise, a List is created and shallow copies of each coordinate is added.</param>
        public CoordinateListSequence(IEnumerable<Coordinate> coordinates)
        {
            List<Coordinate> list = coordinates as List<Coordinate>;
            if(list == null)
            {
                list = new List<Coordinate>();
                foreach(Coordinate c in coordinates)
                {
                    list.Add(c);
                }
            }
            
            Configure(list);
        }

        /// <summary>
        /// Creates a new instance of CoordinateListSequence
        /// </summary>
        /// <param name="sequence">The sequence to use</param>
        public CoordinateListSequence(ICoordinateSequence sequence)
        {
            List<Coordinate> list = new List<Coordinate>();
            for (int i = 0; i <= sequence.Count; i++)
            {
               list.Add(sequence[i].Clone() as Coordinate);
            }
            Configure(list);
        }

        // Do configuration here
        private void Configure(List<Coordinate> inCoords)
        {
            _internalList = inCoords;
            _versionID = 0;
        }

        #endregion

       

        #region Methods


        /// <summary>
        /// Adds an item to the end of the CoordinateList
        /// </summary>
        /// <param name="item">An ICoordinate to add</param>
        public void Add(Coordinate item)
        {
            _internalList.Add(item);
            IncrementVersion();
        }

        /// <summary>
        /// Clears the list entirely.
        /// </summary>
        public void Clear()
        {
            _internalList = new List<Coordinate>();
            IncrementVersion();
        }

        /// <summary>
        /// Creates a deep copy of this object
        /// </summary>
        /// <returns>CoordinateListSequence</returns>
        public object Clone()
        {
            return new CoordinateListSequence(_internalList);
        }

        /// <summary>
        /// Tests wheather or not the specified ICoordinate is contained in the list.
        /// </summary>
        /// <param name="item">An ICoordinate to test</param>
        /// <returns>Boolean, true if the list contains the item.</returns>
        public bool Contains(Coordinate item)
        {
            return _internalList.Contains(item);
        }

        /// <summary>
        /// Copies all the members of this list into the specified array.
        /// </summary>
        /// <param name="array">The array to copy values to.</param>
        /// <param name="arrayIndex">The 0 based integer index in the array at which copying begins</param>
        public void CopyTo(Coordinate[] array, int arrayIndex)
        {
            _internalList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Expands the given Envelope to include the coordinates in the sequence.
        /// Does not modify the original envelope, but rather returns a copy of the
        /// original envelope after being modified.
        /// </summary>
        /// <param name="env">The envelope use as the starting point for expansion.  This envelope will not be modified.</param>
        /// <returns>The newly created envelope that is the expanded version of the original.</returns>
        public IEnvelope ExpandEnvelope(IEnvelope env)
        {
            IEnvelope temp = env.Copy();
            foreach (Coordinate coord in _internalList)
            {
                temp.ExpandToInclude(coord);
            }
            return temp;
        }

        /// <summary>
        /// Obtains an enumerator for cycling through the list.  BEWARE!  This
        /// will return an underlying list enumerator, and so code that uses
        /// a foreach process or an enumerator will bypass the Version incrementing code.
        /// </summary>
        /// <returns>An Enumerator object for cycling through the list.</returns>
        public IEnumerator<Coordinate> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        /// <summary>
        /// Gets a specific X, Y or Z value depending on the index specified and the ordinate specified
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ordinate"></param>
        /// <returns></returns>
        public double GetOrdinate(int index, Ordinates ordinate)
        {
            return _internalList[index].Z;
        }

        /// <summary>
        /// Removes the specified ICoordinate from the list.
        /// </summary>
        /// <param name="item">An ICoordinate that should be removed from the list</param>
        /// <returns>Boolean, true if the item was successfully removed.</returns>
        public bool Remove(Coordinate item)
        {
            IncrementVersion();
            return _internalList.Remove(item);
        }

        /// <summary>
        /// Sets a specific X, Y or Z value depending on the index specified and the ordinate specified
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ordinate"></param>
        /// <param name="value"></param>
        public void SetOrdinate(int index, Ordinates ordinate, double value)
        {
            switch (ordinate)
            {
                case Ordinates.X: _internalList[index].X = value; break;
                case Ordinates.Y: _internalList[index].Y = value; break;
                case Ordinates.Z: _internalList[index].Z = value; break;
                case Ordinates.M: _internalList[index].M = value; break;
            }
            IncrementVersion();
        }

        /// <summary>
        /// Copies all the members to an ICoordiante[] 
        /// </summary>
        /// <returns></returns>
        public Coordinate[] ToCoordinateArray()
        {
            return _internalList.ToArray();
        }

       

        #endregion

        #region Properties

        /// <summary>
        /// Gets an integer representing the number of ICoordinates currently stored in this list
        /// </summary>
        public int Count
        {
            get { return _internalList.Count; }
        }

        /// <summary>
        /// This always returns 0
        /// </summary>
        public int Dimension
        {
            get { return 0; }
        }


        /// <summary>
        /// The CoordinateListSequence is not readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }


        /// <summary>
        /// Direct accessor to an ICoordinate
        /// </summary>
        /// <param name="Index">An integer index in this array sequence</param>
        /// <returns>An ICoordinate for the specified index</returns>
        public virtual Coordinate this[int Index]
        {
            get
            {
                return _internalList[Index];
            }
            set
            {
                _internalList[Index] = value;
                IncrementVersion();
            }
        }

        /// <summary>
        /// This only works as long as an enumeration is not used on the CoordinateListSequence.
        /// Since ICoordinateSequence does not support a GetEnumerator object, as long as you
        /// are using an ICoordinateSequence interface, VersionID should work.
        /// </summary>
        public int VersionID
        {
            get { return _versionID; }
        }

        #endregion

        /// <summary>
        /// Increments the version with the understanding that we are using integers.  If the user
        /// uses too many version, it will cycle around.  The statistical odds of accidentally
        /// reaching the same value as a previous version should be small enough to prevent
        /// conflicts.
        /// </summary>
        private void IncrementVersion()
        {
            if (_versionID == int.MaxValue)
            {
                _versionID = int.MinValue;
            }
            else
            {
                _versionID++;
            }
        }

       

        
        
    }
}
