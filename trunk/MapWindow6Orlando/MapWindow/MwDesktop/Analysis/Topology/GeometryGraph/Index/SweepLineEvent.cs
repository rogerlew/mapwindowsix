//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://www.gnu.org/licenses/lgpl.html and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections;
using System.Text;

namespace MapWindow.GeometriesGraph.Index
{
    /// <summary>
    /// 
    /// </summary>
    public class SweepLineEvent : IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        public const int Insert = 1;
        
        /// <summary>
        /// 
        /// </summary>
        public const int Delete = 2;

        private object edgeSet;    // used for red-blue intersection detection
        private double xValue;
        private int eventType;
        private SweepLineEvent insertEvent; // null if this is an Insert event
        private int deleteEventIndex;
        private object obj;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edgeSet"></param>
        /// <param name="x"></param>
        /// <param name="insertEvent"></param>
        /// <param name="obj"></param>
        public SweepLineEvent(object edgeSet, double x, SweepLineEvent insertEvent, object obj)
        {
            this.edgeSet = edgeSet;
            xValue = x;
            this.insertEvent = insertEvent;
            this.eventType = Insert;
            if (insertEvent != null)
                eventType = Delete;
            this.obj = obj;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual object EdgeSet
        {
            get
            {
                return this.edgeSet;
            }
            set
            {
                this.edgeSet = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsInsert 
        {
            get
            {
                return insertEvent == null; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsDelete
        {
            get
            {
                return insertEvent != null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SweepLineEvent InsertEvent
        {
            get
            {
                return insertEvent;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int DeleteEventIndex
        {
            get
            {
                return deleteEventIndex;
            }
            set
            {
                this.deleteEventIndex = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual object Object
        {
            get
            {
                return obj;
            }
        }

        /// <summary>
        /// ProjectionEvents are ordered first by their x-value, and then by their eventType.
        /// It is important that Insert events are sorted before Delete events, so that
        /// items whose Insert and Delete events occur at the same x-value will be
        /// correctly handled.
        /// </summary>
        /// <param name="o"></param>
        public virtual int CompareTo(object o)
        {
            SweepLineEvent pe = (SweepLineEvent)o;
            if (xValue < pe.xValue)
                return -1;
            if (xValue > pe.xValue)
                return 1;
            if (eventType < pe.eventType)
                return -1;
            if (eventType > pe.eventType)
                return 1;
            return 0;
        }
    }
}
