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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/28/2009 10:45:02 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Drawing
{


    /// <summary>
    /// IntervalMethods
    /// </summary>
    public enum IntervalMethods
    {
        /// <summary>
        /// A numeric value fixes a constant separation between breaks.
        /// </summary>
        DefinedInterval,
        /// <summary>
        /// The breaks are set to being evenly spaced.
        /// </summary>
        EqualInterval,
        /// <summary>
        /// Breaks are calculated according to the following restrictions:
        /// 1) break sizes follow a geometric progression
        /// 2) the number of breaks is specified
        /// 3) the sum of squares of the counts per bin is minimized
        /// </summary>
        Geometrical,
        /// <summary>
        /// Breaks start equally placed, but can be positioned manually instead.
        /// </summary>
        Manual,
        /// <summary>
        /// Jenks natural breaks looks for "clumping" in the data and
        /// attempts to group according to the clumps.
        /// </summary>
        NaturalBreaks,
        /// <summary>
        /// The breaks are positioned to ensure close to equal quantities
        /// in each break.
        /// </summary>
        Quantile,
        /// <summary>
        /// Not sure how this works yet.  Something to do with standard deviations.
        /// </summary>
        StandardDeviation,
       


    }
}
