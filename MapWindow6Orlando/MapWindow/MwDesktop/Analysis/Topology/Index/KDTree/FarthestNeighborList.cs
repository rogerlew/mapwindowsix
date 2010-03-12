//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the the original author of the file, which is an adaptation of the Java KDTree library implemented by Levy 
// and Heckel. This simplified version is written by Marco A. Alvarez
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from the KDTreeDll
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford, shortly after 
// Allen Anselmo first added it to MapWinGeoProc in August 2008.  
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Analysis.Topology.KDTree
{
    /// <summary>
    /// Bjoern Heckel's solution to the KD-Tree n-nearest-neighbor problem
    /// </summary>
    public class FarthestNeighborList
    {
        private SortedList<double, object> _list;
     
   
        /// <summary>
        /// Creates a new NearestNeighborList
        /// </summary>
        /// <param name="capacity">An integer indicating the maximum size for the cue</param>
        public FarthestNeighborList(int capacity)
        {
            _list = new SortedList<double, object>(capacity);
        }

        /// <summary>
        /// Gets the minimum priority, or distance.  Since we are looking for the maximum distance, or the 
        /// n maximum distances, we want to determine quickly the lowest distance currently contained
        /// in the cue.
        /// </summary>
        /// <returns></returns>
        public double MinimumPriority
        {
            get
            {
                if (_list.Count == 0) return 0;
                return _list.Keys[0];
            }
        }

        /// <summary>
        /// Inserts an object with a given priority
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public bool Insert(Object obj, double priority)
        {
            if (_list.Count < _list.Capacity)
            {
                // capacity not reached
                _list.Add(priority, obj);
                return true;
            }
            //if (priority > m_Queue.getMaxPriority())
            //{
            //    // do not insert - all elements in queue have lower priority
            //    return false;
            //}

            if (priority < _list.Keys[0])
            {
                // do not insert, all elements in queue have higher priority
                return false;
            }

            //// remove object with highest priority
            //m_Queue.remove();

            _list.RemoveAt(0);
            //m_Queue.remove();
            // add new object
            _list.Add(priority, obj);
            // m_Queue.add(_object, priority);
            return true;
        }

        /// <summary>
        /// Gets whether or not the length of the cue has reached the capacity
        /// </summary>
        public bool IsCapacityReached
        {
            get
            {
              //   return m_Queue.length() >= m_Capacity;
                return _list.Count == _list.Capacity;
            }
        }

        /// <summary>
        /// Gets the highest object in the nearest neighbor list
        /// </summary>
        /// <returns></returns>
        public object Farthest
        {
            get
            {
                //return m_Queue.front();
                if (_list.Count == 0) return null;
                return _list.Values[_list.Count - 1];
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether or not the cue is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty
        {
            get
            {
               //return m_Queue.length() == 0;
                return _list.Count == 0;
            }
        }

        /// <summary>
        /// Gets the length of the current list
        /// </summary>
        public int Size
        {
            get
            {
               // return m_Queue.length();
                return _list.Count;
            }
        }

        /// <summary>
        /// Removes the highest member from the cue and returns that object.
        /// </summary>
        /// <returns></returns>
        public object RemoveFarthest()
        {
            // remove object with highest priority
            //return m_Queue.remove();
            if (_list.Count > 0)
            {
                int i = _list.Count - 1;
                object result = _list.Values[i];
                _list.RemoveAt(i);
                return result;
            }
            return null;
        }

    }
}
