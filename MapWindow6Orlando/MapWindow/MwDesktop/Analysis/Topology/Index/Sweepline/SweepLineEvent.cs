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

namespace MapWindow.Analysis.Topology.Index.Sweepline
{
    /// <summary>
    /// 
    /// </summary>
    public enum SweepLineEvents
    {
        /// <summary>
        /// 
        /// </summary>
        Insert = 1,

        /// <summary>
        /// 
        /// </summary>
        Delete = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public class SweepLineEvent : IComparable
    {
        private double xValue;
        private SweepLineEvents eventType;
        private SweepLineEvent insertEvent = null; // null if this is an Insert event
        private int deleteEventIndex;

        private SweepLineInterval sweepInt;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="insertEvent"></param>
        /// <param name="sweepInt"></param>
        public SweepLineEvent(double x, SweepLineEvent insertEvent, SweepLineInterval sweepInt)
        {
            xValue = x;
            this.insertEvent = insertEvent;            
            if (insertEvent != null)
                 eventType = SweepLineEvents.Delete;
            else eventType = SweepLineEvents.Insert;
            this.sweepInt = sweepInt;
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
        public virtual SweepLineEvent InsertEvent
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
        public virtual SweepLineInterval Interval
        {
            get
            {
                return sweepInt;
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
            SweepLineEvent pe = (SweepLineEvent) o;
            if (xValue < pe.xValue) return  -1;
            if (xValue > pe.xValue) return   1;
            if (eventType < pe.eventType) return  -1;
            if (eventType > pe.eventType) return   1;
            return 0;
        }
    }
}
