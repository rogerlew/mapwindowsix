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
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Security;
using System.Security.Permissions;
namespace MapWindow.Main
{
    /// <summary>
    /// The fastest way to implement this is to inherit System.ComponentModel.Component
    /// </summary>
    public interface IControl: IComponent, IBindableComponent, IDropTarget, ISynchronizeInvoke, IWin32Window
    {
        #region Methods

        /// <summary>
        /// Brings the control to the front of the z-order.
        /// </summary>
        void BringToFront();

        /// <summary>
        /// Retrieves a value indicating whether the specified control is a child of the control.
        /// </summary>
        /// <param name="ctl"><I>ctl</I>: The System.Windows.Forms.Control to evaluate.</param>
        /// <returns>true if the specified control is a child of the control; otherwise, false.</returns>
        bool Contains(System.Windows.Forms.Control ctl);

        /// <summary>
        /// Forces the creation of the control, including the creation of the handle and any child controls.
        /// </summary>
        void CreateControl();

        /// <summary>
        /// Creates the System.Drawing.Graphics for the control.
        /// </summary>
        /// <returns>The System.Drawing.Graphics for the control.</returns>
        System.Drawing.Graphics CreateGraphics();

        /// <summary>
        /// Begins a drag-and-drop operation.
        /// </summary>
        /// <param name="data">The data to drag.</param>
        /// <param name="allowedEffects">One of the System.Windows.Forms.DragDropEffects values.</param>
        /// <returns>A value from the System.Windows.Forms.DragDropEffects enumeration that represents the final effect that was performed during the drag-and-drop operation.</returns>
        System.Windows.Forms.DragDropEffects DoDragDrop(object data, System.Windows.Forms.DragDropEffects allowedEffects);

        /// <summary>
        /// Supports rendering to the specified bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap to be drawn to.</param>
        /// <param name="targetBounds">The bounds within which the control is rendered.</param>
        void DrawToBitmap(System.Drawing.Bitmap bitmap, System.Drawing.Rectangle targetBounds);

        /// <summary>
        /// Retrieves the form that the control is on.
        /// </summary>
        /// <returns>The System.Windows.Forms.Form that the control is on.</returns>
        System.Windows.Forms.Form FindForm();


        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        /// <returns>true if the input focus request was successful; otherwise, false.</returns>
        bool Focus();

        /// <summary>
        /// Retrieves the child control that is located at the specified coordinates.
        /// </summary>
        /// <param name="pt">A System.Drawing.Point that contains the coordinates where you want to look for a control. Coordinates are expressed relative to the upper-left corner of the control's client area.</param>
        /// <returns>A System.Windows.Forms.Control that represents the control that is located at the specified point.</returns>
        System.Windows.Forms.Control GetChildAtPoint(System.Drawing.Point pt);

        /// <summary>
        /// Retrieves the child control that is located at the specified coordinates, specifying whether to ignore child controls of a certain type.
        /// </summary>
        /// <param name="pt">One of the values of System.Windows.Forms.GetChildAtPointSkip, determining whether to ignore child controls of a certain type.</param>
        /// <param name="skipValue">A System.Drawing.Point that contains the coordinates where you want to look for a control. Coordinates are expressed relative to the upper-left corner of the control's client area.</param>
        /// <returns>The child System.Windows.Forms.Control at the specified coordinates.</returns>
        System.Windows.Forms.Control GetChildAtPoint(System.Drawing.Point pt, System.Windows.Forms.GetChildAtPointSkip skipValue);

        /// <summary>
        /// Returns the next System.Windwos.Forms.ContainerControl up the control's chain of parent controls.
        /// </summary>
        /// <returns>An System.Windows.Forms.IContainerControl, that represents the parent of the System.Windows.Forms.Control.</returns>
        System.Windows.Forms.IContainerControl GetContainerControl();

        /// <summary>
        /// Retrieves the next control forward or back in the tab order of child controls.
        /// </summary>
        /// <param name="ctl"> The System.Windows.Forms.Control to start the search with. </param>
        /// <param name="forward">true to search forward in the tab order; false to search backward.</param>
        /// <returns>The next System.Windows.Forms.Control in the tab order.</returns>
        System.Windows.Forms.Control GetNextControl(System.Windows.Forms.Control ctl, bool forward);

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="proposedSize">The custom-sized area for a control.</param>
        /// <returns>An ordered pair of type System.Drawing.Size representing the width and height of a rectangle.</returns>
        System.Drawing.Size GetPreferredSize(System.Drawing.Size proposedSize);

        /// <summary>
        /// Conceals the control from the user.
        /// </summary>
        void Hide();

        /// <summary>
        /// Invalidates the specified region of the control (adds it to the control's update region, which is the area that 
        /// will be repainted at the next paint operation), and causes a paint message to be sent to the control. Optionally, invalidates the child controls assigned to the control.
        /// </summary>
        /// <param name="rc"> A System.Drawing.Rectangle that represents the region to invalidate.</param>
        /// <param name="invalidateChildren">true to invalidate the control's child controls; otherwise, false.</param>
        void Invalidate(System.Drawing.Rectangle rc, bool invalidateChildren);

        /// <summary>
        /// Invalidates the specified region of the control (adds it to the control's update region, which is the area that will be repainted at the next paint operation), and causes a paint message to be sent to the control.
        /// </summary>
        /// <param name="rc">A System.Drawing.Rectangle that represents the region to invalidate.</param>
        void Invalidate(System.Drawing.Rectangle rc);

        /// <summary>
        /// Invalidates a specific region of the control and causes a paint message to be sent to the control. Optionally, invalidates the child controls assigned to the control.
        /// </summary>
        /// <param name="invalidateChildren">invalidateChildren: true to invalidate the control's child controls; otherwise, false.</param>
        void Invalidate(bool invalidateChildren);

        /// <summary>
        /// Invalidates the entire surface of the control and causes the control to be redrawn.
        /// </summary>
        void Invalidate();


        /// <summary>
        /// Invalidates the specified region of the control (adds it to the control's update region, which is the area that will be repainted 
        /// at the next paint operation), and causes a paint message to be sent to the control. Optionally, invalidates the child controls assigned to the control.
        /// </summary>
        /// <param name="region">The System.Drawing.Region to invalidate.</param>
        /// <param name="invalidateChildren">true to invalidate the control's child controls; otherwise, false.</param>
        void Invalidate(System.Drawing.Region region, bool invalidateChildren);


        /// <summary>
        /// Invalidates the specified region of the control (adds it to the control's update region, which is the area that will be 
        /// repainted at the next paint operation), and causes a paint message to be sent to the control.
        /// </summary>
        /// <param name="region">The System.Drawing.Region to invalidate.</param>
        void Invalidate(System.Drawing.Region region);


        /// <summary>
        /// Executes the specified delegate on the thread that owns the control's underlying window handle.
        /// </summary>
        /// <param name="method">method: A delegate that contains a method to be called in the control's thread context.</param>
        /// <returns>The return value from the delegate being invoked, or null if the delegate has no return value.</returns>
        object Invoke(System.Delegate method);

        /// <summary>
        /// Forces the control to apply layout logic to all its child controls.
        /// </summary>
        /// <param name="affectedControl">A System.Windows.Forms.Control that represents the most recently changed control.</param>
        /// <param name="affectedProperty">The name of the most recently changed property on the control.</param>
        void PerformLayout(System.Windows.Forms.Control affectedControl, string affectedProperty);

        /// <summary>
        /// Forces the control to apply layout logic to all its child controls.
        /// </summary>
        void PerformLayout();

        /// <summary>
        /// Computes the location of the specified screen point into client coordinates.
        /// </summary>
        /// <param name="p">p: The screen coordinate System.Drawing.Point to convert.</param>
        /// <returns>A System.Drawing.Point that represents the converted System.Drawing.Point, p, in client coordinates.</returns>
        System.Drawing.Point PointToClient(System.Drawing.Point p);


        /// <summary>
        /// Computes the location of the specified client point into screen coordinates.
        /// </summary>
        /// <param name="p">p: The client coordinate System.Drawing.Point to convert.</param>
        /// <returns>A System.Drawing.Point that represents the converted System.Drawing.Point, p, in screen coordinates.</returns>
        System.Drawing.Point PointToScreen(System.Drawing.Point p);

        /// <summary>
        /// Preprocesses keyboard or input messages within the message loop before they are dispatched.
        /// </summary>
        /// <param name="msg">msg: A System.Windows.Forms.Message that represents the message to process.</param>
        /// <returns>One of the System.Windows.Forms.PreProcessControlState values, depending on whether 
        /// System.Windows.Forms.Control.PreProcessMessage(System.Windows.Forms.Message@) is true or false and whether 
        /// System.Windows.Forms.Control.IsInputKey(System.Windows.Forms.Keys) or System.Windows.Forms.Control.IsInputChar(System.Char) 
        /// are true or false.</returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        System.Windows.Forms.PreProcessControlState PreProcessControlMessage(ref System.Windows.Forms.Message msg);

        /// <summary>
        /// Preprocesses keyboard or input messages within the message loop before they are dispatched.
        /// </summary>
        /// <param name="msg"> A System.Windows.Forms.Message, passed by reference, that represents the
        /// message to process. The possible values are WN_KEYDOWN, WM_SYSKEYDOWN, WM_cHAR, and WM_SYSCHAR.</param>
        /// <returns>true if the message was processed by the control; otherwise, false.</returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        bool PreProcessMessage(ref System.Windows.Forms.Message msg);


        /// <summary>
        /// Computes the size and location of the specified screen rectangle in client coordinates.
        /// </summary>
        /// <param name="r">r: The screen coordinate System.Drawing.Rectangle to convert.</param>
        /// <returns>A System.Drawing.Rectangle that represents the converted System.Drawing.Rectangle, r, in client coordinates.</returns>
        System.Drawing.Rectangle RectangleToClient(System.Drawing.Rectangle r);

        /// <summary>
        /// Computes the size and location of the specified client rectangle in screen coordinates.
        /// </summary>
        /// <param name="r">r: The screen coordinate System.Drawing.Rectangle to convert.</param>
        /// <returns>A System.Drawing.Rectangle that represents the converted System.Drawing.Rectangle, p, in screen coordinates.</returns>
        System.Drawing.Rectangle RectangleToScreen(System.Drawing.Rectangle r);

        /// <summary>
        /// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Resets the System.Windows.Forms.Control.Text property to its default value.
        /// </summary>
        void ResetText();

        /// <summary>
        /// Resumes usual layout logic, optionally forcing an immediate layout of pending layout requests.
        /// </summary>
        /// <param name="performLayout">performLayout: true to execute pending layout requests; otherwise, false.</param>
        void ResumeLayout(bool performLayout);

        /// <summary>
        /// Resumes usual layout logic.
        /// </summary>
        void ResumeLayout();

        /// <summary>
        /// Scales the control and all child controls by the specified scaling factor.
        /// </summary>
        /// <param name="factor">factor: A System.Drawing.SizeF containing the horizontal and vertical scaling factors.</param>
        void Scale(System.Drawing.SizeF factor);

        /// <summary>
        /// Activates the control.
        /// </summary>
        void Select();

        /// <summary>
        /// Activates the next control.
        /// </summary>
        /// <param name="ctl">The System.Windows.Forms.Control at which to start the search.</param>
        /// <param name="forward">true to move forward in the tab order; false to move backward in the tab order.</param>
        /// <param name="tabStopOnly"> true to ignore the controls with the System.Windows.Forms.Control.TabStop property set to false; </param>
        /// <param name="nested">true to include nested (children of child controls) child controls; otherwise, false.</param>
        /// <param name="wrap">true to continue searching from the first control in the tab order after the last control has been reached;</param>
        /// <returns>true if a control was activated; otherwise, false.</returns>
        bool SelectNextControl(System.Windows.Forms.Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap);

        /// <summary>
        /// Sends the control to the back of the z-order.
        /// </summary>
        void SendToBack();


        /// <summary>
        /// Sets the specified bounds of the control to the specified location and size.
        /// </summary>
        /// <param name="x">The new System.Windows.Forms.Control.Left property value of the control.</param>
        /// <param name="y">The new System.Windows.Forms.Control.Top property value of the control.</param>
        /// <param name="width">The new System.Windows.Forms.Control.Width property value of the control.</param>
        /// <param name="height">The new System.Windows.Forms.Control.Height property value of the control.</param>
        /// <param name="specified">A bitwise combination of the System.Windows.Forms.BoundsSpecified values. For any parameter not specified, the current value will be used.</param>
        void SetBounds(int x, int y, int width, int height, System.Windows.Forms.BoundsSpecified specified);


        /// <summary>
        /// Sets the bounds of the control to the specified location and size.
        /// </summary>
        /// <param name="x">The new System.Windows.Forms.Control.Left property value of the control.</param>
        /// <param name="y">The new System.Windows.Forms.Control.Top property value of the control.</param>
        /// <param name="width">The new System.Windows.Forms.Control.Width property value of the control.</param>
        /// <param name="height">The new System.Windows.Forms.Control.Height property value of the control.</param>
        void SetBounds(int x, int y, int width, int height);


        /// <summary>
        /// Displays the control to the user.
        /// </summary>
        void Show();

        /// <summary>
        /// Temporarily suspends the layout logic for the control.
        /// </summary>
        void SuspendLayout();

        /// <summary>
        /// Causes the control to redraw the invalidated regions within its client area.
        /// </summary>
        void Update();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the System.Windows.Forms.AccessibleObject assigned to the control.
        /// </summary>
        /// <returns>The System.Windows.Forms.AccessibleObject assigned to the control.
        /// </returns>
        System.Windows.Forms.AccessibleObject AccessibilityObject { get; }

        /// <summary>
        /// Gets or sets the default action description of the control for use by accessibility client applications.
        /// </summary>
        /// <returns>The default action description of the control for use by accessibility client applications.
        /// </returns>
        string AccessibleDefaultActionDescription { set; get; }

        /// <summary>
        /// Gets or sets the description of the control used by accessibility client applications.
        /// </summary>
        /// <returns>The description of the control used by accessibility client applications. The default is null.
        /// </returns>
        string AccessibleDescription { set; get; }

        /// <summary>
        /// Gets or sets the name of the control used by accessibility client applications.
        /// </summary>
        /// <returns>The name of the control used by accessibility client applications. The default is null.
        /// </returns>
        string AccessibleName { set; get; }

        /// <summary>
        /// Gets or sets the accessible role of the control
        /// </summary>
        /// <returns>One of the values of System.Windows.Forms.AccessibleRole. The default is Default.
        /// </returns>
        System.Windows.Forms.AccessibleRole AccessibleRole { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control can accept data that the user drags onto it.
        /// </summary>
        /// <returns>true if drag-and-drop operations are allowed in the control; otherwise, false. The default is false.
        /// </returns>
        bool AllowDrop { set; get; }

        /// <summary>
        /// Gets or sets the edges of the container to which a control is bound and determines how a control is resized with its parent.
        /// </summary>
        /// <returns>A bitwise combination of the System.Windows.Forms.AnchorStyles values. The default is Top and Left.</returns>
        System.Windows.Forms.AnchorStyles Anchor { set; get; }

        /// <summary>
        /// Gets or sets where this control is scrolled to in System.Windows.Forms.ScrollableControl.ScrollControlIntoView(System.Windows.Forms.Control).
        /// </summary>
        /// <returns>A System.Drawing.Point specifying the scroll location. The default is the upper-left corner of the control.
        /// </returns>
        System.Drawing.Point AutoScrollOffset { set; get; }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <returns>A System.Drawing.Color that represents the background color of the control. The default is 
        /// the value of the System.Windows.Forms.Control.DefaultBackColor property.</returns>
        System.Drawing.Color BackColor { set; get; }


        /// <summary>
        /// Gets or sets the background image displayed in the control.
        /// </summary>
        /// <returns>An System.Drawing.Image that represents the image to display in the background of the control.
        /// </returns>
        System.Drawing.Image BackgroundImage { set; get; }

        /// <summary>
        /// Gets or sets the background image layout as defined in the System.Windows.Forms.ImageLayout enumeration.
        /// </summary>
        /// <returns>
        /// One of the values of System.Windows.Forms.ImageLayout (System.Windows.Forms.ImageLayout.Center ,
        /// System.Windows.Forms.ImageLayout.None, System.Windows.Forms.ImageLayout.Stretch, System.Windows.Forms.ImageLayout.Tile,
        /// or System.Windows.Forms.ImageLayout.Zoom). System.Windows.Forms.ImageLayout.Tile is the default value.
        /// </returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException"> the specified enumeration value does not exist</exception>
        System.Windows.Forms.ImageLayout BackgroundImageLayout { set; get; }


        /// <summary>
        /// Gets the distance, in pixels, between the bottom edge of the control and the top edge of its container's client area.
        /// </summary>
        /// <returns>An System.Int32 representing the distance, in pixels, between the bottom edge of the control and the top edge of its container's client area.
        /// </returns>
        int Bottom { get; }


        /// <summary>
        /// Gets or sets the size and location of the control including its nonclient elements, in pixels, relative to the parent control.
        /// </summary>
        /// <returns>
        /// A System.Drawing.Rectangle in pixels relative to the parent control that represents the size and location of the control including its nonclient elements.
        /// </returns>
        System.Drawing.Rectangle Bounds { set; get; }

        /// <summary>
        /// Gets a value indicating whether the control can receive focus.
        /// </summary>
        /// <returns>true if the control can receive focus; otherwise, false.
        /// </returns>
        bool CanFocus { get; }


        /// <summary>
        /// Gets a value indicating whether the control can be selected.
        /// </summary>
        /// <returns>true if the control can be selected; otherwise, false.
        /// </returns>
        bool CanSelect { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control has captured the mouse.
        /// </summary>
        /// <returns>true if the control has captured the mouse; otherwise, false.
        /// </returns>
        bool Capture { get; set;}

        /// <summary>
        /// Gets or sets a value indicating whether the control causes validation to be performed on any controls that require validation when it receives focus.
        /// </summary>
        /// <returns>true if the control causes validation to be performed on any controls requiring validation when it receives focus; otherwise, false. The default is true.
        /// </returns>
        bool CausesValidation { set; get; }

        /// <summary>
        /// Gets or sets the height and width of the client area of the control.
        /// </summary>
        /// <returns>
        /// A System.Drawing.Size that represents the dimensions of the client area of the control.
        /// </returns>
        System.Drawing.Size ClientSize { set; get; }

        /// <summary>
        /// Gets the name of the company or creator of the application containing the control.
        /// </summary>
        /// <returns>
        /// The company name or creator of the application containing the control.
        /// </returns>
        string CompanyName { get; }


        /// <summary>
        /// Gets a value indicating whether the control, or one of its child controls, currently has the input focus.
        /// </summary>
        /// <returns>
        /// true if the control or one of its child controls currently has the input focus; otherwise, false.
        /// </returns>
        bool ContainsFocus { get; }

        /// <summary>
        /// Gets or sets the shortcut menu associated with the control.
        /// </summary>
        /// <returns>
        /// A System.Windows.Forms.ContextMenu that represents the shortcut menu associated with the control.
        /// </returns>
        System.Windows.Forms.ContextMenu ContextMenu { set; get; }

        /// <summary>
        /// Gets or sets the System.Windows.Forms.ContextMenuStrip associated with this control.
        /// </summary>
        /// <returns>
        /// The System.Windows.Forms.ContextMenuStrip for this control, or null if there is no System.Windows.Forms.ContextMenuStrip. The default is null.
        /// </returns>
        System.Windows.Forms.ContextMenuStrip ContextMenuStrip { set; get; }

        /// <summary>
        /// Gets the collection of controls contained within the control.
        /// </summary>
        /// <returns>
        /// A System.Windows.Forms.Control.ControlCollection representing the collection of controls contained within the control.
        /// </returns>
        System.Windows.Forms.Control.ControlCollection Controls { get; }

        /// <summary>
        /// Gets a value indicating whether the control has been created.
        /// </summary>
        /// <returns>
        /// true if the control has been created; otherwise, false.
        /// </returns>
        bool Created { get; }


        /// <summary>
        /// Gets or sets the cursor that is displayed when the mouse pointer is over the control.
        /// </summary>
        /// <returns>
        /// A System.Windows.Forms.Cursor that represents the cursor to display when the mouse pointer is over the control.
        /// </returns>
        System.Windows.Forms.Cursor Cursor { set; get; }


        /// <summary>
        /// Gets the rectangle that represents the display area of the control.
        /// </summary>
        /// <returns>
        /// A System.Drawing.Rectangle that represents the display area of the control.
        /// </returns>
        System.Drawing.Rectangle DisplayRectangle { get; }


        /// <summary>
        /// Gets a value indicating whether the base System.Windows.Forms.Control class is in the process of disposing.
        /// </summary>
        /// <returns>
        /// true if the base System.Windows.Forms.Control class is in the process of disposing; otherwise, false.
        /// </returns>
        bool Disposing { get; }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent control and determines how a control is resized with its parent.
        /// </summary>
        /// <returns>One of the System.Windows.Forms.DockStyle values. The default is System.Windows.Forms.DockStyle.None.
        /// </returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
        /// The value assigned is not one of the System.Windows.Forms.DockStyle values.
        /// </exception>
        System.Windows.Forms.DockStyle Dock { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond to user interaction.
        /// </summary>
        /// <returns>
        /// true if the control can respond to user interaction; otherwise, false. The default is true.
        /// </returns>
        bool Enabled { set; get; }

        /// <summary>
        /// Gets a value indicating whether the control has input focus.
        /// </summary>
        /// <returns>
        /// true if the control has focus; otherwise, false.
        /// </returns>
        bool Focused { get; }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <returns>
        /// The System.Drawing.Font to apply to the text displayed by the control. The default is the value of the System.Windows.Forms.Control.DefaultFont property.
        /// </returns>
        System.Drawing.Font Font { set; get; }


        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        /// <returns>
        /// The foreground System.Drawing.Color of the control. The default is the value of the System.Windows.Forms.Control.DefaultForeColor property.
        /// </returns>
        System.Drawing.Color ForeColor { set; get; }

        /// <summary>
        /// Gets a value indicating whether the control contains one or more child controls
        /// </summary>
        /// <returns>
        /// true if the control contains one or more child controls; otherwise, false.
        /// </returns>
        bool HasChildren { get; }


        /// <summary>
        /// Gets or sets the height of the control.
        /// </summary>
        /// <returns>
        /// The height of the control in pixels.
        /// </returns>
        int Height { set; get; }


        /// <summary>
        /// Gets or sets the Input Method Editor (IME) mode of the control.
        /// </summary>
        /// <returns>
        /// One of the System.Windows.Forms.ImeMode values. The default is System.Windows.Forms.ImeMode.Inherit.
        /// </returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException"> The assigned value is not one of the System.Windows.Forms.ImeMode enumeration values.</exception>
        System.Windows.Forms.ImeMode ImeMode { set; get; }


        /// <summary>
        /// Gets or sets a value indicating whether the control is visible to accessibility applications.
        /// </summary>
        /// <returns>
        /// true if the control is visible to accessibility applications; otherwise, false.
        /// </returns>
        bool IsAccessible { set; get; }

        /// <summary>
        /// Gets a value indicating whether the control has been disposed of.
        /// </summary>
        /// <returns>
        /// true if the control has been disposed of; otherwise, false.
        /// </returns>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets a value indicating whether the control has a handle associated with it.
        /// </summary>
        /// <returns>
        /// true if a handle has been assigned to the control; otherwise, false.
        /// </returns>
        bool IsHandleCreated { get; }

        ///// <summary>
        ///// Gets a value indicating whether the control is mirrored.
        ///// </summary>
        ///// <returns>
        ///// true if the control is mirrored; otherwise, false.
        ///// </returns>
        ///// <remarks>Not supported by Mono 2.0</remarks>
        ///// bool IsMirrored { get; }

        /// <summary>
        /// Gets a cached instance of the control's layout engine.
        /// </summary>
        /// <returns>
        /// The System.Windows.Forms.Layout.LayoutEngine for the control's contents.
        /// </returns>
        System.Windows.Forms.Layout.LayoutEngine LayoutEngine { get; }


        /// <summary>
        /// Gets or sets the distance, in pixels, between the left edge of the control and the left edge of its container's client area.
        /// </summary>
        /// <returns>
        /// An System.Int32 representing the distance, in pixels, between the left edge of the control and the left edge of its container's client area.
        /// </returns>
        int Left { set; get; }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the control relative to the upper-left corner of its container.
        /// </summary>
        /// <returns>
        /// The System.Drawing.Point that represents the upper-left corner of the control relative to the upper-left corner of its container.
        /// </returns>
        System.Drawing.Point Location { set; get; }

        /// <summary>
        /// Gets or sets the space between controls.
        /// </summary>
        /// <returns>
        /// A System.Windows.Forms.Padding representing the space between controls.
        /// </returns>
        System.Windows.Forms.Padding Margin { set; get; }

        /// <summary>
        /// Gets or sets the size that is the upper limit that System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size) can specify.
        /// </summary>
        /// <returns>
        /// An ordered pair of type System.Drawing.Size representing the width and height of a rectangle.
        /// </returns>
        System.Drawing.Size MaximumSize { set; get; }

        /// <summary>
        /// Gets or sets the size that is the lower limit that System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size) can specify.
        /// </summary>
        /// <returns>
        /// An ordered pair of type System.Drawing.Size representing the width and height of a rectangle.
        /// </returns>
        System.Drawing.Size MinimumSize { set; get; }

        /// <summary>
        /// Gets or sets the name of the control.
        /// </summary>
        /// <returns>
        /// The name of the control. The default is an empty string ("").
        /// </returns>
        string Name { set; get; }

        /// <summary>
        /// Gets or sets padding within the control.
        /// </summary>
        /// <returns>
        /// A System.Windows.Forms.Padding representing the control's internal spacing characteristics.
        /// </returns>
        System.Windows.Forms.Padding Padding { set; get; }

        /// <summary>
        /// Gets or sets the parent container of the control.
        /// </summary>
        /// <returns>
        /// A System.Windows.Forms.Control that represents the parent or container control of the control.
        /// </returns>
        System.Windows.Forms.Control Parent { set; get; }

        /// <summary>
        /// Gets the size of a rectangular area into which the control can fit.
        /// </summary>
        /// <returns>
        /// A System.Drawing.Size containing the height and width, in pixels.
        /// </returns>
        System.Drawing.Size PreferredSize { get; }


        /// <summary>
        /// Gets the product name of the assembly containing the control.
        /// </summary>
        /// <returns>
        /// The product name of the assembly containing the control.
        /// </returns>
        string ProductName { get; }


        /// <summary>
        /// Gets the version of the assembly containing the control.
        /// </summary>
        /// <returns>
        /// The file version of the assembly containing the control.
        /// </returns>
        string ProductVersion { get; }

        /// <summary>
        /// Gets a value indicating whether the control is currently re-creating its handle.
        /// </summary>
        /// <returns>
        /// true if the control is currently re-creating its handle; otherwise, false.
        /// </returns>
        bool RecreatingHandle { get; }

        /// <summary>
        /// Gets or sets the window region associated with the control.
        /// </summary>
        /// <returns>
        /// The window System.Drawing.Region associated with the control.
        /// </returns>
        System.Drawing.Region Region { set; get; }


        /// <summary>
        /// Gets the distance, in pixels, between the right edge of the control and the left edge of its container's client area.
        /// </summary>
        /// <returns>
        /// An System.Int32 representing the distance, in pixels, between the right edge of the control and the left edge of its container's client area.
        /// </returns>
        int Right { get; }

        /// <summary>
        /// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
        /// </summary>
        /// <returns>
        /// One of the System.Windows.Forms.RightToLeft values. The default is System.Windows.Forms.RightToLeft.Inherit.
        /// </returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException"> The assigned value is not one of the System.Windows.Forms.RightToLeft values.</exception>
        System.Windows.Forms.RightToLeft RightToLeft { set; get; }

        /// <summary>
        /// Gets or sets the height and width of the control.
        /// </summary>
        /// <returns>
        /// The System.Drawing.Size that represents the height and width of the control in pixels.
        /// </returns>
        System.Drawing.Size Size { set; get; }

        /// <summary>
        /// Gets or sets the tab order of the control within its container.
        /// </summary>
        /// <returns>
        /// The index value of the control within the set of controls within its container. The controls in the container are included in the tab order.
        /// </returns>
        int TabIndex { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.
        /// </summary>
        /// <returns>
        /// true if the user can give the focus to the control using the TAB key; otherwise, false. The default is true.This property will always return true for an instance of the System.Windows.Forms.Form class.
        /// </returns>
        bool TabStop { set; get; }

        /// <summary>
        /// Gets or sets the object that contains data about the control.
        /// </summary>
        /// <returns>
        /// An System.Object that contains data about the control. The default is null.
        /// </returns>
        object Tag { set; get; }

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        /// <returns>
        /// The text associated with this control.
        /// </returns>
        string Text { set; get; }

        /// <summary>
        /// Gets or sets the distance, in pixels, between the top edge of the control and the top edge of its container's client area.
        /// </summary>
        /// <returns>
        /// An System.Int32 representing the distance, in pixels, between the bottom edge of the control and the top edge of its container's client area.
        /// </returns>
        int Top { set; get; }

        /// <summary>
        /// Gets the parent control that is not parented by another Windows Forms control. Typically, this is the outermost System.Windows.Forms.Form that the control is contained in.
        /// </summary>
        /// <returns>
        /// The System.Windows.Forms.Control that represents the top-level control that contains the current control.
        /// </returns>
        System.Windows.Forms.Control TopLevelControl { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the wait cursor for the current control and all child controls.
        /// </summary>
        /// <returns>
        /// true to use the wait cursor for the current control and all child controls; otherwise, false. The default is false.
        /// </returns>
        bool UseWaitCursor { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control is displayed.
        /// </summary>
        /// <returns>
        /// true if the control is displayed; otherwise, false. The default is true.
        /// </returns>
        bool Visible { set; get; }

        /// <summary>
        /// Gets or sets the width of the control.
        /// </summary>
        /// <returns>
        /// The width of the control in pixels.
        /// </returns>
        int Width { set; get; }


        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.BackColor property changes.
        /// </summary>
        event System.EventHandler BackColorChanged;

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.BackgroundImage property changes.
        /// </summary>
        event System.EventHandler BackgroundImageChanged;


        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.BackgroundImageLayout property changes.
        /// </summary>
        event System.EventHandler BackgroundImageLayoutChanged;

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.BindingContext property changes.
        /// </summary>
        event System.EventHandler BindingContextChanged;


        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.CausesValidation property changes.
        /// </summary>
        event System.EventHandler CausesValidationChanged;

        /// <summary>
        /// Occurs when the focus or keyboard user interface (UI) cues change.
        /// </summary>
        event System.Windows.Forms.UICuesEventHandler ChangeUICues;

        /// <summary>
        /// event System.EventHandler Click
        /// </summary>
        event System.EventHandler Click;

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.ClientSize property changes.
        /// </summary>
        event System.EventHandler ClientSizeChanged;

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.ContextMenu property changes.
        /// </summary>
        event System.EventHandler ContextMenuChanged;

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.ContextMenuStrip property changes.
        /// </summary>
        event System.EventHandler ContextMenuStripChanged;

        /// <summary>
        /// Occurs when a new control is added to the System.Windows.Forms.Control.ControlCollection.
        /// </summary>
        event System.Windows.Forms.ControlEventHandler ControlAdded;

        /// <summary>
        /// Occurs when a control is removed from the System.Windows.Forms.Control.ControlCollection.
        /// </summary>
        event System.Windows.Forms.ControlEventHandler ControlRemoved;

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.Cursor property changes.
        /// </summary>
        event System.EventHandler CursorChanged;

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.Dock property changes.
        /// </summary>
        event System.EventHandler DockChanged;

        /// <summary>
        /// Occurs when the control is double-clicked.
        /// </summary>
        event System.EventHandler DoubleClick;

        /// <summary>
        /// Occurs when a drag-and-drop operation is completed.
        /// </summary>
        event System.Windows.Forms.DragEventHandler DragDrop;

        /// <summary>
        /// Occurs when an object is dragged into the control's bounds.
        /// </summary>
        event System.Windows.Forms.DragEventHandler DragEnter;

        /// <summary>
        /// Occurs when an object is dragged out of the control's bounds.
        /// </summary>
        event System.EventHandler DragLeave;

        /// <summary>
        /// Occurs when an object is dragged over the control's bounds.
        /// </summary>
        event System.Windows.Forms.DragEventHandler DragOver;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Enabled property value has changed.
        /// </summary>
        event System.EventHandler EnabledChanged;

        /// <summary>
        /// Occurs when the control is entered.
        /// </summary>
        event System.EventHandler Enter;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Font property value changes.
        /// </summary>
        event System.EventHandler FontChanged;


        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.ForeColor property value changes.
        /// </summary>
        event System.EventHandler ForeColorChanged;

        /// <summary>
        /// Occurs during a drag operation.
        /// </summary>
        event System.Windows.Forms.GiveFeedbackEventHandler GiveFeedback;

        /// <summary>
        /// Occurs when the control receives focus.
        /// </summary>
        event System.EventHandler GotFocus;

        /// <summary>
        /// Occurs when a handle is created for the control.
        /// </summary>
        event System.EventHandler HandleCreated;

        /// <summary>
        /// Occurs when the control's handle is in the process of being destroyed.
        /// </summary>
        event System.EventHandler HandleDestroyed;

        /// <summary>
        /// Occurs when the user requests help for a control.
        /// </summary>
        event System.Windows.Forms.HelpEventHandler HelpRequested;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.ImeMode property has changed.
        /// </summary>
        event System.EventHandler ImeModeChanged;

        /// <summary>
        /// Occurs when a control's display requires redrawing.
        /// </summary>
        event System.Windows.Forms.InvalidateEventHandler Invalidated;

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        event System.Windows.Forms.KeyEventHandler KeyDown;

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        event System.Windows.Forms.KeyPressEventHandler KeyPress;

        /// <summary>
        /// Occurs when a key is released while the control has focus.
        /// </summary>
        event System.Windows.Forms.KeyEventHandler KeyUp;

        /// <summary>
        /// Occurs when a control should reposition its child controls.
        /// </summary>
        event System.Windows.Forms.LayoutEventHandler Layout;

        /// <summary>
        /// Occurs when the input focus leaves the control.
        /// </summary>
        event System.EventHandler Leave;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Location property value has changed.
        /// </summary>
        event System.EventHandler LocationChanged;


        /// <summary>
        /// Occurs when the control loses focus.
        /// </summary>
        event System.EventHandler LostFocus;

        /// <summary>
        /// Occurs when the control's margin changes.
        /// </summary>
        event System.EventHandler MarginChanged;

        /// <summary>
        /// Occurs when the control loses mouse capture.
        /// </summary>
        event System.EventHandler MouseCaptureChanged;

        /// <summary>
        /// Occurs when the control is clicked by the mouse.
        /// </summary>
        event System.Windows.Forms.MouseEventHandler MouseClick;


        /// <summary>
        /// Occurs when the control is double clicked by the mouse.
        /// </summary>
        event System.Windows.Forms.MouseEventHandler MouseDoubleClick;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is pressed.
        /// </summary>
        event System.Windows.Forms.MouseEventHandler MouseDown;


        /// <summary>
        /// Occurs when the mouse pointer enters the control.
        /// </summary>
        event System.EventHandler MouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer rests on the control.
        /// </summary>
        event System.EventHandler MouseHover;

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        event System.EventHandler MouseLeave;


        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        event System.Windows.Forms.MouseEventHandler MouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        event System.Windows.Forms.MouseEventHandler MouseUp;

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        event System.Windows.Forms.MouseEventHandler MouseWheel;

        /// <summary>
        /// Occurs when the control is moved.
        /// </summary>
        event System.EventHandler Move;

        /// <summary>
        /// Occurs when the control's padding changes.
        /// </summary>
        event System.EventHandler PaddingChanged;

        /// <summary>
        /// Occurs when the control is redrawn.
        /// </summary>
        event System.Windows.Forms.PaintEventHandler Paint;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Parent property value changes.
        /// </summary>
        event System.EventHandler ParentChanged;

        /// <summary>
        /// Occurs before the System.Windows.Forms.Control.KeyDown event when a key is pressed while focus is on this control.
        /// </summary>
        [method: SecurityPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        event System.Windows.Forms.PreviewKeyDownEventHandler PreviewKeyDown;

        /// <summary>
        /// Occurs when System.Windows.Forms.AccessibleObject is providing help to accessibility applications.
        /// </summary>
        event System.Windows.Forms.QueryAccessibilityHelpEventHandler QueryAccessibilityHelp;

        /// <summary>
        /// Occurs during a drag-and-drop operation and enables the drag source to determine whether the drag-and-drop operation should be canceled.
        /// </summary>
        event System.Windows.Forms.QueryContinueDragEventHandler QueryContinueDrag;

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.Region property changes.
        /// </summary>
        event System.EventHandler RegionChanged;

        /// <summary>
        /// Occurs when the control is resized.
        /// </summary>
        event System.EventHandler Resize;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.RightToLeft property value changes.
        /// </summary>
        event System.EventHandler RightToLeftChanged;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Size property value changes.
        /// </summary>
        event System.EventHandler SizeChanged;

        /// <summary>
        /// Occurs when the control style changes.
        /// </summary>
        event System.EventHandler StyleChanged;

        /// <summary>
        /// Occurs when the system colors change.
        /// </summary>
        event System.EventHandler SystemColorsChanged;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.TabIndex property value changes.
        /// </summary>
        event System.EventHandler TabIndexChanged;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.TabStop property value changes.
        /// </summary>
        event System.EventHandler TabStopChanged;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Text property value changes.
        /// </summary>
        event System.EventHandler TextChanged;

        /// <summary>
        /// Occurs when the control is finished validating.
        /// </summary>
        event System.EventHandler Validated;

        /// <summary>
        /// Occurs when the control is validating.
        /// </summary>
        event System.ComponentModel.CancelEventHandler Validating;

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.IsVisible property value changes.
        /// </summary>
        event System.EventHandler VisibleChanged;

        #endregion


        // IBindableComponent
        //   - BindingContext
        //   - DataBindings

        // IComponent
        //   - Site
        //   - Disposed event

        // IDisposable
        //   - Dispose


        // IDropTarget
        //   - OnDragDrop
        //   - OnDragEnter
        //   - OnDragLeave
        //   - OnDragOver

        // ISynchroinzeInvoke
        //   - BeginInvoke
        //   - EndInvoke
        //   - Invoke

        // IWin32Window
        //   - Handle
    }
}
