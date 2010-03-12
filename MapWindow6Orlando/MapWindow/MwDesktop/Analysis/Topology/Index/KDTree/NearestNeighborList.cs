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
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Analysis.Topology.KDTree
{
    /// <summary>
    /// Bjoern Heckel's solution to the KD-Tree n-nearest-neighbor problem
    /// </summary>
    public class NearestNeighborList
    {
        /// <summary>
        /// Indicates whether removal should occur from the highest value or lowest value
        /// </summary>
        public static int REMOVE_HIGHEST = 1;

        /// <summary>
        /// 
        /// </summary>
        public static int REMOVE_LOWEST = 2;

        PriorityQueue m_Queue = null;
        int m_Capacity = 0;

        /// <summary>
        /// Creates a new NearestNeighborList
        /// </summary>
        /// <param name="capacity">An integer indicating the maximum size for the cue</param>
        public NearestNeighborList(int capacity)
        {
            m_Capacity = capacity;
            m_Queue = new PriorityQueue(m_Capacity, Double.PositiveInfinity);
        }

        /// <summary>
        /// Gets the maximum priority
        /// </summary>
        /// <returns></returns>
        public double MaxPriority
        {
            get
            {
                if (m_Queue.length() == 0)
                {
                    return Double.PositiveInfinity;
                }
                return m_Queue.getMaxPriority();
            }
        }

        /// <summary>
        /// Inserts an object with a given priority
        /// </summary>
        /// <param name="_object"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public bool Insert(Object _object, double priority)
        {
            if (m_Queue.length() < m_Capacity)
            {
                // capacity not reached
                m_Queue.add(_object, priority);
                return true;
            }
            if (priority > m_Queue.getMaxPriority())
            {
                // do not insert - all elements in queue have lower priority
                return false;
            }
            // remove object with highest priority
            m_Queue.remove();
            // add new object
            m_Queue.add(_object, priority);
            return true;
        }

        /// <summary>
        /// Gets whether or not the length fo the cue has reached the capacity
        /// </summary>
        public bool IsCapacityReached
        {
            get
            {
                return m_Queue.length() >= m_Capacity;
            }
        }

        /// <summary>
        /// Gets the highest object in the nearest neighbor list
        /// </summary>
        /// <returns></returns>
        public Object Highest
        {
            get
            {
                return m_Queue.front();
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
                return m_Queue.length() == 0;
            }
        }

        /// <summary>
        /// Gets the length of the current list
        /// </summary>
        public int Size
        {
            get
            {
                return m_Queue.length();
            }
        }

        /// <summary>
        /// Removes the highest member from the cue and returns that object.
        /// </summary>
        /// <returns></returns>
        public Object RemoveHighest()
        {
            // remove object with highest priority
            return m_Queue.remove();
        }

    }
}
