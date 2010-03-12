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
// The Initial Developer of this Original Code is Ted Dunsford. Created in July, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using MapWindow.Geometries;
namespace MapWindow.Drawing
{
    /// <summary>
    /// Anything that can draw itself to the map is an IRenderable.  This is implemented by RenderBase.
    /// </summary>
    public interface IRenderable 
    {

         #region Events

       
        /// <summary>
        /// Occurs whenever the geographic bounds for this renderable object have changed
        /// </summary>
        event EventHandler<EnvelopeArgs> EnvelopeChanged;

   

        /// <summary>
        /// Occurs when an outside request is sent to invalidate this object
        /// </summary>
        event EventHandler Invalidated;

        /// <summary>
        /// Occurs immediately after the visible parameter has been adjusted.
        /// </summary>
        event EventHandler VisibleChanged;

        

        #endregion


        #region Methods

       

        /// <summary>
        /// Invalidates the drawing methods
        /// </summary>
        void Invalidate();


        #endregion

        #region Properties

        

        /// <summary>
        /// Obtains an IEnvelope in world coordinates that contains this object
        /// </summary>
        /// <returns></returns>
        IEnvelope Envelope
        {
            get;
        }

        /// <summary>
        /// Gets whether or not the unmanaged drawing structures have been created for this item
        /// </summary>
        bool IsInitialized
        {
            get;
        }

        /// <summary>
        /// If this is false, then the drawing function will not render anything.
        /// Warning!  This will also prevent any execution of calculations that take place
        /// as part of the drawing methods and will also abort the drawing methods of any
        /// sub-members to this IRenderable.
        /// </summary>
        bool IsVisible
        {
            get;
            set;
        }

       
       


        #endregion

        
    }
}
