//********************************************************************************************************
// Product Name: MapWindow.Interfaces Alpha
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
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.ComponentModel;
namespace MapWindow.Geometries
{
    /// <summary>
    /// Defines a rectangular region of the 2D coordinate plane.
    /// It is often used to represent the bounding box of a <c>Geometry</c>,
    /// e.g. the minimum and maximum x and y values of the <c>Coordinate</c>s.
    /// Notice that Envelopes support infinite or half-infinite regions, by using the values of
    /// <c>Double.PositiveInfinity</c> and <c>Double.NegativeInfinity</c>.
    /// When Envelope objects are created or initialized,
    /// the supplies extent values are automatically sorted into the correct order.    
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface IEnvelope: ICloneable, IRectangle
    {
        #region Methods

        

        /// <summary>
        /// Creates a copy of the current envelope.
        /// </summary>
        /// <returns>An IEnvelope Interface that is a duplicate of this envelope</returns>
        IEnvelope Copy();



        #region Init

        /// <summary>
        /// Because the envelope class attempts to protect users from accidentally manipulating
        /// the minimum/maximum conditions in an undesirable way, the init method should effectively
        /// initialize the existing envelope based on the new coordinates.  This is how values
        /// are "Set" for the envelope.  It is expected that this method will do a check
        /// to prevent larger values from being assigned to "maximum".
        /// </summary>
        /// <param name="p1">The first coordinate to test</param>
        /// <param name="p2">the second coordinate to test</param>
        void Init(Coordinate p1, Coordinate p2);
       

        #endregion

   
        /// <summary>
        /// Sets maxx to a value less than minx
        /// usually, maxx = -1 and minx = 0, maxy = -1 and miny = 0
        /// </summary>
        void SetToNull();

      

        #endregion


        #region Properties

        /// <summary>
        /// This is a coordinate defining the minimum bound in any number of dimensions
        /// as controlled by NumOrdinates.
        /// </summary>
        Coordinate Minimum { get; }

        /// <summary>
        /// This is a coordinate defining the maximum bound in any number of dimensions
        /// as controlled by NumOrdinates.
        /// </summary>
        Coordinate Maximum { get; }


        /// <summary>
        /// Gets the number of ordinates that define both coordinates of this envelope.
        /// </summary>
        int NumOrdinates { get; }


      
        /// <summary>
        /// Returns <c>true</c> if this <c>Envelope</c> is a "null" envelope.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this <c>Envelope</c> is uninitialized
        /// or is the envelope of the empty point.  This is usually defined
        /// as having a maxx less than a minx, like maxx = -1 and minx = 0.
        /// </returns>
        bool IsNull { get;}


        #endregion


        #region Events
        /// <summary>
        /// Occurs when there is a basic change in the envelope.
        /// </summary>
        event EventHandler EnvelopeChanged;

        #endregion


    }
}
