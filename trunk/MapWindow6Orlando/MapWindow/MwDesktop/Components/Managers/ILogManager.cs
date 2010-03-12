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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2008 9:39:29 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Drawing;
using System.Collections.Generic;
using MapWindow.Plugins;
using MapWindow.Main;
using System.Windows.Forms;
namespace MapWindow.Components
{
    /// <summary>
    /// This component allows customization of how log messages are sent
    /// </summary>
    public interface ILogManager : IProgressHandler
    {

        #region Methods


        /// <summary>
        /// To begin logging, create an implementation of the ILogHandler interface,
        /// or use the DefaultLogger class that is already implemented in this project.
        /// Then, call this function to add that logger to the list of active loggers.
        /// This function will return an integer key that you can use to keep track
        /// of your specific logger.
        /// </summary>
        int AddLogger(ILogger Logger);
       
        /// <summary>
        /// The key specified here is the key that was returned by the AddLogger method.
        /// </summary>
        /// <param name="Key">The integer key of the logger to remove.</param>
        /// <returns>True if the logger was successfully removed, or false if the key could not be found</returns>
        /// <exception cref="System.ArgumentNullException">key is null</exception>
        bool RemoveLogger(int Key);
      
        /// <summary>
        /// Adds all the loggers from directories.
        /// </summary>
        /// <returns></returns>
        List<ILogger> AddLoggersFromDirectories();
       

        /// <summary>
        /// The Complete exception is passed here.  To get the stack
        /// trace, be sure to call ex.ToString().
        /// </summary>
        /// <param name="ex">The exception that was thrown by MapWindow.</param>
        void Exception(Exception ex);
       

        /// <summary>
        /// This event will allow the registering of an entrance into a public Method of a "tools" related
        /// action to register its entrance into a function as well as logging the parameter names
        /// and a type specific indicator of their value.
        /// </summary>
        /// <param name="MethodName">The string name of the method</param>
        /// <param name="Parameters">The List&lt;string&gt; of Parameter names and string form values</param>
        void PublicMethodEntered(string MethodName, List<string> Parameters);
       

        /// <summary>
        /// This event will allow the registering of the exit from each public method
        /// </summary>
        /// <param name="MethodName">The Method name of the method being left</param>
        void PublicMethodLeft(string MethodName);
      
        /// <summary>
        /// A status message was sent.  Complex methods that have a few major steps will
        /// call a status message to show which step the process is in.  Loops will call
        /// the progress method instead.
        /// </summary>
        /// <param name="Message">The string message that was posted.</param>
       void Status(string Message);


       /// <summary>
       /// This is called by each of the LogMessageBox methods automatically, but if the user wants to use
       /// a custom messagebox and then log the message and result directly this is the technique.
       /// </summary>
       /// <param name="text">The string text of the message that needs to be logged.</param>
       /// <param name="result">The dialog result from the shown messagebox.</param>
        void LogMessage(string text, System.Windows.Forms.DialogResult result);

        /// <summary>
        /// This method echoes information about input boxes to all the loggers.
        /// </summary>
        /// <param name="text">The string message that appeared on the InputBox</param>
        /// <param name="result">The ystem.Windows.Forms.DialogResult describing if the value was cancelled </param>
        /// <param name="value">The string containing the value entered.</param>
        void LogInput(string text, System.Windows.Forms.DialogResult result, string value);
        

        #endregion

        #region InputBox Overloads

        /// <summary>
        /// Displays an InputBox form given the specified text string.  The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the outcome.</returns>
       DialogResult LogInputBox(string text, out string result);
       

        /// <summary>
        /// Displays an InputBox form given the specified text string.  The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the outcome.</returns>
        DialogResult LogInputBox(string text, string caption, out string result);
       

        /// <summary>
        /// Displays an InputBox form given the specified text string.  The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="validation">A MapWindow.Main.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the outcome.</returns>
        DialogResult LogInputBox(string text, string caption, ValidationTypes validation, out string result);
        

        /// <summary>
        /// Displays an InputBox form given the specified text string.  The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="validation">A MapWindow.Main.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="icon">Specifies an icon to display on this form.</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the outcome.</returns>
        DialogResult LogInputBox(string text, string caption, ValidationTypes validation, Icon icon, out string result);
       

        /// <summary>
        /// Displays an InputBox form given the specified text string.  The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="owner">The window that owns this modal dialog.</param>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the outcome.</returns>
        DialogResult LogInputBox(Form owner, string text, out string result);
       

        /// <summary>
        /// Displays an InputBox form given the specified text string.  The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="owner">The window that owns this modal dialog.</param>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the outcome.</returns>
        DialogResult LogInputBox(Form owner, string text, string caption, out string result);
       

        /// <summary>
        /// Displays an InputBox form given the specified text string.  The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="owner">The window that owns this modal dialog.</param>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="validation">A MapWindow.Main.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the outcome.</returns>
        DialogResult LogInputBox(Form owner, string text, string caption, ValidationTypes validation, out string result);
        
        /// <summary>
        /// Displays an InputBox form given the specified text string.  The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="owner">The window that owns this modal dialog.</param>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="validation">A MapWindow.Main.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="icon">Specifies an icon to display on this form.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the outcome.</returns>
        DialogResult LogInputBox(Form owner, string text, string caption, ValidationTypes validation, Icon icon, out string result);
       

        #endregion


        #region MessageBox Overloads


        #region With Owner


        // 2
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the System.Windows.Forms.IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(IWin32Window owner, string text);
       

        // 4
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the System.Windows.Forms.IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(IWin32Window owner, string text, string caption);
        

        // 6
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the System.Windows.Forms.IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons);
        

        // 8
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the System.Windows.Forms.IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
       

        // 10
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the System.Windows.Forms.IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
      

        // 12
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the System.Windows.Forms.IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        /// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options);
       

        //// 15
        ///// <summary>
        ///// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        ///// </summary>
        ///// <param name="owner">An implementation of the System.Windows.Forms.IWin32Window that will own the modal form dialog box.</param>
        ///// <param name="text">The text to display in the MessageBox</param>
        ///// <param name="caption">The text to display in the title bar of the MessageBox</param>
        ///// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        ///// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        ///// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        ///// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        ///// <param name="helpFilePath">The path name of the help file to display when the user clicks the help button</param>
        ///// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        //DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath);
        

        //// 18
        ///// <summary>
        ///// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        ///// </summary>
        ///// <param name="owner">An implementation of the System.Windows.Forms.IWin32Window that will own the modal form dialog box.</param>
        ///// <param name="text">The text to display in the MessageBox</param>
        ///// <param name="caption">The text to display in the title bar of the MessageBox</param>
        ///// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        ///// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        ///// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        ///// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        ///// <param name="helpFilePath">The path name of the help file to display when the user clicks the help button</param>
        ///// <param name="navigator">One of the System.Windows.Forms.HelpNavigator values</param>
        ///// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        ///// <remarks>Not supported in Mono 2.0 </remarks>
        ///// DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator);
        

        //// 19
        ///// <summary>
        ///// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        ///// </summary>
        ///// <param name="owner">An implementation of the System.Windows.Forms.IWin32Window that will own the modal form dialog box.</param>
        ///// <param name="text">The text to display in the MessageBox</param>
        ///// <param name="caption">The text to display in the title bar of the MessageBox</param>
        ///// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        ///// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        ///// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        ///// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        ///// <param name="helpFilePath">The path name of the help file to display when the user clicks the help button</param>
        ///// <param name="navigator">One of the System.Windows.Forms.HelpNavigator values</param>
        ///// <param name="keyword">The help keyword to display when the user clicks the help button</param>
        ///// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        ///// <remarks>Not supported in Mono 2.0</remarks>
        ///// DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, string keyword);
       

        // 21
        ///// <summary>
        ///// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        ///// </summary>
        ///// <param name="owner">An implementation of the System.Windows.Forms.IWin32Window that will own the modal form dialog box.</param>
        ///// <param name="text">The text to display in the MessageBox</param>
        ///// <param name="caption">The text to display in the title bar of the MessageBox</param>
        ///// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        ///// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        ///// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        ///// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        ///// <param name="helpFilePath">The path name of the help file to display when the user clicks the help button</param>
        ///// <param name="navigator">One of the System.Windows.Forms.HelpNavigator values</param>
        ///// <param name="param">The numeric ID of the help object to display when the user clicks the help button</param>
        ///// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        ///// <remarks>Not supported in Mono 2.0</remarks>
        ///// DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param);
      


       



        #endregion

        #region Without Owner

        // 1
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(string text);
      

        //3
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(string text, string caption);
        

        // 5
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox( string text, string caption, MessageBoxButtons buttons);
       

        // 7
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
      

        // 9
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
        
        // ll
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        /// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options);
       

        // 13
        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        /// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        /// <param name="displayHelpButton">A boolean indicating whether or not to display a help button on the messagebox</param>
        /// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton);
      
        //// 14
        ///// <summary>
        ///// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        ///// </summary>
        ///// <param name="text">The text to display in the MessageBox</param>
        ///// <param name="caption">The text to display in the title bar of the MessageBox</param>
        ///// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        ///// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        ///// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        ///// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        ///// <param name="helpFilePath">The path name of the help file to display when the user clicks the help button</param>
        ///// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        //DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath);
       

        // 16
        ///// <summary>
        ///// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        ///// </summary>
        ///// <param name="text">The text to display in the MessageBox</param>
        ///// <param name="caption">The text to display in the title bar of the MessageBox</param>
        ///// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        ///// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        ///// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        ///// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        ///// <param name="helpFilePath">The path name of the help file to display when the user clicks the help button</param>
        ///// <param name="navigator">One of the System.Windows.Forms.HelpNavigator values</param>
        ///// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        ///// <remarks>Not supported in Mono 2.0</remarks>
        ///// DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator);
       

        // 17
        ///// <summary>
        ///// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        ///// </summary>
        ///// <param name="text">The text to display in the MessageBox</param>
        ///// <param name="caption">The text to display in the title bar of the MessageBox</param>
        ///// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        ///// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        ///// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        ///// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        ///// <param name="helpFilePath">The path name of the help file to display when the user clicks the help button</param>
        ///// <param name="navigator">One of the System.Windows.Forms.HelpNavigator values</param>
        ///// <param name="keyword">The help keyword to display when the user clicks the help button</param>
        ///// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        ///// <remarks>Not supported in Mono 2.0</remarks>
        ///// DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, string keyword);
        

        // 20
        ///// <summary>
        ///// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        ///// </summary>
        ///// <param name="text">The text to display in the MessageBox</param>
        ///// <param name="caption">The text to display in the title bar of the MessageBox</param>
        ///// <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons that describes which button to display in the MessageBox</param>
        ///// <param name="icon">One of the System.Windows.Forms.MessageBoxIcons that describes which icon to display in the MessageBox</param>
        ///// <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        ///// <param name="options">One of the System.Windows.Forms.MessageBoxOptions that describes which display and association options to use for the MessageBox.  You may pass 0 if you wish to use the defaults.</param>
        ///// <param name="helpFilePath">The path name of the help file to display when the user clicks the help button</param>
        ///// <param name="navigator">One of the System.Windows.Forms.HelpNavigator values</param>
        ///// <param name="param">The numeric ID of the help object to display when the user clicks the help button</param>
        ///// <returns>A System.Windows.Forms.DialogResult showing the user input from this messagebox.</returns>
        ///// <remarks>Not supported in Mono 2.0</remarks>
        ///// DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator,  object param);


        #endregion

        #endregion
    }
}
