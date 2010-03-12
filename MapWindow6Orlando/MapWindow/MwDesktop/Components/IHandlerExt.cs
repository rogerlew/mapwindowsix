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
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Permissions;
namespace MapWindow.Components
{
    interface IHandlerExt
    {
        #region Control Events
        
        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.BackColor property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void BackColorChanged(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.BackgroundImage property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void BackgroundImageChanged(object sender, System.EventArgs e);


        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.BackgroundImageLayout property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void BackgroundImageLayoutChanged(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.BindingContext property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void BindingContextChanged(object sender, System.EventArgs e);


        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.CausesValidation property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void CausesValidationChanged(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when the focus or keyboard user interface (UI) cues change.
        /// </summary>
        event System.Windows.Forms.UICuesEventHandler ChangeUICues;

        /// <summary>
        /// event System.EventHandler Click
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void Clicked(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.ClientSize property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void ClientSizeChanged(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.ContextMenu property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void ContextMenuChanged(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.ContextMenuStrip property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void ContextMenuStripChanged(object sender, System.EventArgs e);

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
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void CursorChanged(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.Dock property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void DockChanged(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when the control is double-clicked.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void DoubleClick(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when a drag-and-drop operation is completed.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.DragEventArgs with the drag parameters</param>
        void DragDrop(object sender, System.Windows.Forms.DragEventArgs e);
        

        /// <summary>
        /// Occurs when an object is dragged into the control's bounds.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.DragEventArgs with the drag parameters</param>
        void DragEnter(object sender, System.Windows.Forms.DragEventArgs e);
        
        /// <summary>
        /// Occurs when an object is dragged out of the control's bounds.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void DragLeave(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when an object is dragged over the control's bounds.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.DragEventArgs with the drag parameters</param>
        void DragOver(object sender, System.Windows.Forms.DragEventArgs e);
        

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Enabled property value has changed.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void EnabledChanged(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when the control is entered.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void Enter(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Font property value changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void FontChanged(object sender, System.EventArgs e);
       


        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.ForeColor property value changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void ForeColorChanged(object sender, System.EventArgs e);
        

        /// <summary>
        /// Occurs during a drag operation.
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.GiveFeedbackEventArgs with any parameters</param>
        /// </summary>
        void GiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e);
        

        /// <summary>
        /// Occurs when the control receives focus.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void GotFocus(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when a handle is created for the control.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void HandleCreated(object sender, System.EventArgs e);
        

        /// <summary>
        /// Occurs when the control's handle is in the process of being destroyed.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void HandleDestroyed(object sender, System.EventArgs e);
        

        /// <summary>
        /// Occurs when the user requests help for a control.
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="hlpevent">A System.Windows.Forms.HelpEventArgs with any parameters </param>
        /// </summary>
        void HelpRequested(object sender, System.Windows.Forms.HelpEventArgs hlpevent);
        
            
        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.ImeMode property has changed.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void ImeModeChanged(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when a control's display requires redrawing.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void Invalidated(object sender, System.EventArgs e);
       
        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.KeyEventArgs with any parameters</param>
        void KeyDown(object sender, System.Windows.Forms.KeyEventArgs e);

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.KeyPressEventArgs with any parameters</param>
        void KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e);

        /// <summary>
        /// Occurs when a key is released while the control has focus.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.KeyEventArgs with any parameters</param>
        void KeyUP(object sender, System.Windows.Forms.KeyEventArgs e);
        

        /// <summary>
        /// Occurs when a control should reposition its child controls.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.LayoutEventArgs with any parameters</param>
        void Layout(object sender, System.Windows.Forms.LayoutEventArgs e);
        

        /// <summary>
        /// Occurs when the input focus leaves the control.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void Leave(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Location property value has changed.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void LocationChanged(object sender, System.EventArgs e);
      


        /// <summary>
        /// Occurs when the control loses focus.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void LostFocus(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the control's margin changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void MarginChanged(object sender, System.EventArgs e);
     
        /// <summary>
        /// Occurs when the control loses mouse capture.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void MouseCaptureChanged(object sender, System.EventArgs e);
        

        /// <summary>
        /// Occurs when the control is clicked by the mouse.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.MouseEventArgs with any parameters</param>
        void bob_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e);
        

        /// <summary>
        /// Occurs when the control is double clicked by the mouse.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.MouseEventArgs with any parameters</param>
        void MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e);
       

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is pressed.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.MouseEventArgs with any parameters</param>
        void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e);
        


        /// <summary>
        /// Occurs when the mouse pointer enters the control.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void MouseEnter(object sender, System.EventArgs e);
        
        /// <summary>
        /// Occurs when the mouse pointer rests on the control.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void MouseHover(object sender, System.EventArgs e);
      

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void MouseLeave(object sender, System.EventArgs e);
        


        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.MouseEventArgs with any parameters</param>
        void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e);
        

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.MouseEventArgs with any parameters</param>
        void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e);
       

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.MouseEventArgs with any parameters</param>
        void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e);
        

        /// <summary>
        /// Occurs when the control is moved.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void Move(object sender, System.EventArgs e);
        

        /// <summary>
        /// Occurs when the control's padding changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void PaddingChanged(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the control is redrawn.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.PaintEventArgs with any parameters</param>
        void bob_Paint(object sender, System.Windows.Forms.PaintEventArgs e);
       

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Parent property value changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void ParentChanged(object sender, System.EventArgs e);
        

        /// <summary>
        /// Occurs before the System.Windows.Forms.Control.KeyDown event when a key is pressed while focus is on this control.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.PreviewKeyDownEventArgs with any parameters</param>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        void PreviewKeyDown(object sender, System.Windows.Forms.PreviewKeyDownEventArgs e);
       

        /// <summary>
        /// Occurs when System.Windows.Forms.AccessibleObject is providing help to accessibility applications.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.QueryAccessibilityHelpEventArgs with any parameters</param>
        void QueryAccessibilityHelp(object sender, System.Windows.Forms.QueryAccessibilityHelpEventArgs e);
        

        /// <summary>
        /// Occurs during a drag-and-drop operation and enables the drag source to determine whether the drag-and-drop operation should be canceled.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.Windows.Forms.QueryContinueDragEventArgs with any parameters</param>
        void QueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e);
       
        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.Control.Region property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void RegionChanged(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the control is resized.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void Resize(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.RightToLeft property value changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void RightToLeftChanged(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Size property value changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void SizeChanged(object sender, System.EventArgs e);
      
        /// <summary>
        /// Occurs when the control style changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void StyleChanged(object sender, System.EventArgs e);
      

        /// <summary>
        /// Occurs when the system colors change.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void SystemColorsChanged(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.TabIndex property value changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void TabIndexChanged(object sender, System.EventArgs e);
        

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.TabStop property value changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void TabStopChanged(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Text property value changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void TextChanged(object sender, System.EventArgs e);
       
        /// <summary>
        /// Occurs when the control is finished validating.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void Validated(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs when the control is validating.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.ComponentModel.CancelEventArgs with any parameters</param>
        void bob_Validating(object sender, System.ComponentModel.CancelEventArgs e);

        /// <summary>
        /// Occurs when the System.Windows.Forms.Control.Visible property value changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void VisibleChangedChanged(object sender, System.EventArgs e);


        #endregion

        #region UserControl Events

        /// <summary>
        /// Occurs when the AutoSize changes
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void AutoSizeChanged(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs when the System.Windows.Forms.UserControl.AutoValidate property changes.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void AutoValidateChanged(object sender, System.EventArgs e);
       

        /// <summary>
        /// Occurs before the control becomes visible for the first time.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A System.EventArgs with any parameters</param>
        void Load(object sender, System.EventArgs e);
       
        #endregion
    }
}
