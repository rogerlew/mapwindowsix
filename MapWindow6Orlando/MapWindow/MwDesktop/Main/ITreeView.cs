using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MapWindow.Main
{
    /// <summary>
    /// This exposes the major methods, properties and events that exist on the System.Windows.Forms.TreeView control
    /// as an interface.  The easiest way to implement this interface is to simply inherit a dot net TreeView.
    /// </summary>
    public interface ITreeView : IControl
    {
        #region Methods
        /// <summary>
        /// Disables any redrawing of the tree view.
        /// </summary>
        void BeginUpdate();

        /// <summary>
        /// Collapses all the tree nodes.
        /// </summary>
        void CollapseAll();

        /// <summary>
        /// Enables the redrawing of the tree view.
        /// </summary>
        void EndUpdate();

        /// <summary>
        /// Expands all the tree nodes.
        /// </summary>
        void ExpandAll();

        /// <summary>
        /// Retrieves the tree node at the point with the specified coordinates.
        /// </summary>
        /// <param name="x">The System.Drawing.Point.X position to evaluate and retrieve the node from.</param>
        /// <param name="y">The System.Drawing.Point.Y position to evaluate and retrieve the node from.</param>
        /// <returns>The System.Windows.Forms.TreeNode at the specified location, in tree view (client) coordinates, or null if there is no node at that location.</returns>
        System.Windows.Forms.TreeNode GetNodeAt(int x, int y);

        /// <summary>
        /// Retrieves the tree node that is at the specified point.
        /// </summary>
        /// <param name="pt">The System.Drawing.Point to evaluate and retrieve the node from.</param>
        /// <returns>The System.Windows.Forms.TreeNode at the specified point, in tree view (client) coordinates, or null if there is no node at that location.</returns>
        System.Windows.Forms.TreeNode GetNodeAt(System.Drawing.Point pt);


        /// <summary>
        /// Retrieves the number of tree nodes, optionally including those in all subtrees, assigned to the tree view control.
        /// </summary>
        /// <param name="includeSubTrees">true to count the System.Windows.Forms.TreeNode items that the subtrees contain; otherwise, false.</param>
        /// <returns>The number of tree nodes, optionally including those in all subtrees, assigned to the tree view control.</returns>
        int GetNodeCount(bool includeSubTrees);


        /// <summary>
        /// Provides node information, given x- and y-coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate at which to retrieve node information</param>
        /// <param name="y">The y-coordinate at which to retrieve node information.</param>
        /// <returns>A System.Windows.Forms.TreeViewHitTestInfo</returns>
        System.Windows.Forms.TreeViewHitTestInfo HitTest(int x, int y);

        /// <summary>
        /// Provides node information, given a point.
        /// </summary>
        /// <param name="pt">The System.Drawing.Point at which to retrieve node information.</param>
        /// <returns>A System.Windows.Forms.TreeViewHitTestInfo.</returns>
        System.Windows.Forms.TreeViewHitTestInfo HitTest(System.Drawing.Point pt);

        /// <summary>
        /// Sorts the items in System.Windows.Forms.TreeView control.
        /// </summary>
        void Sort();

       
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the border style of the tree view control.
        /// </summary>
        /// <returns>
        /// One of the System.Windows.Forms.BorderStyle values. The default is System.Windows.Forms.BorderStyle.Fixed3D.
        /// </returns>
        System.Windows.Forms.BorderStyle BorderStyle { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether check boxes are displayed next to the tree nodes in the tree view control.
        /// </summary>
        /// <returns>
        /// true if a check box is displayed next to each tree node in the tree view control; otherwise, false. The default is false.
        /// </returns>
        bool CheckBoxes { set; get; }

        /// <summary>
        /// Gets or sets the mode in which the control is drawn.
        /// </summary>
        /// <returns>
        /// One of the System.Windows.Forms.TreeViewDrawMode values. The default is System.Windows.Forms.TreeViewDrawMode.Normal.
        /// </returns>
        System.Windows.Forms.TreeViewDrawMode DrawMode { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the selection highlight spans the width of the tree view control.
        /// </summary>
        /// <returns>
        /// true if the selection highlight spans the width of the tree view control; otherwise, false. The default is false.
        /// </returns>
        bool FullRowSelect { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the selected tree node remains highlighted even when the tree view has lost the focus.
        /// </summary>
        /// <returns>
        /// true if the selected tree node is not highlighted when the tree view has lost the focus; otherwise, false. The default is true.
        /// </returns>
        bool HideSelection { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether a tree node label takes on the appearance of a hyperlink as the mouse pointer passes over it.
        /// </summary>
        /// <returns>
        /// true if a tree node label takes on the appearance of a hyperlink as the mouse pointer passes over it; otherwise, false. The default is false.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Set value is less than -1.</exception>
        bool HotTracking { set; get; }

        /// <summary>
        /// Gets or sets the image-list index value of the default image that is displayed by the tree nodes.
        /// </summary>
        /// <returns>
        /// A zero-based index that represents the position of an System.Drawing.Image in an System.Windows.Forms.ImageList. The default is zero.
        /// </returns>
        int ImageIndex { set; get; }

        /// <summary>
        /// Gets or sets the key of the default image for each node in the System.Windows.Forms.TreeView control when it is in an unselected state.
        /// </summary>
        /// <returns>
        /// The key of the default image shown for each node System.Windows.Forms.TreeView control when the node is in an unselected state.
        /// </returns>
        string ImageKey { set; get; }

        /// <summary>
        /// Gets or sets the System.Windows.Forms.ImageList that contains the System.Drawing.Image objects used by the tree nodes.
        /// </summary>
        /// <returns>
        /// The System.Windows.Forms.ImageList that contains the System.Drawing.Image objects used by the tree nodes. The default value is null.
        /// </returns>
        System.Windows.Forms.ImageList ImageList { set; get; }

        /// <summary>
        /// Gets or sets the distance to indent each of the child tree node levels.
        /// </summary>
        /// <returns>
        /// The distance, in pixels, to indent each of the child tree node levels. The default value is 19.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The assigned value is less than 0 (see Remarks).-or- The assigned value is greater than 32,000.</exception>
        int Indent { set; get; }

        /// <summary>
        /// Gets or sets the height of each tree node in the tree view control.
        /// </summary>
        /// <returns>
        /// The height, in pixels, of each tree node in the tree view.
        /// </returns>
        int ItemHeight { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the label text of the tree nodes can be edited.
        /// </summary>
        /// <remarks>
        /// true if the label text of the tree nodes can be edited; otherwise, false. The default is false.
        /// </remarks>
        bool LabelEdit { set; get; }

        /// <summary>
        /// Gets or sets the color of the lines connecting the nodes of the System.Windows.Forms.TreeView control.
        /// </summary>
        /// <returns>
        /// The System.Drawing.Color of the lines connecting the tree nodes.
        /// </returns>
        System.Drawing.Color LineColor { set; get; }

        /// <summary>
        /// Gets the collection of tree nodes that are assigned to the tree view control.
        /// </summary>
        /// <returns>
        /// A System.Windows.Forms.TreeNodeCollection that represents the tree nodes assigned to the tree view control.
        /// </returns>
        System.Windows.Forms.TreeNodeCollection Nodes { get; }

        /// <summary>
        /// Gets or sets the delimiter string that the tree node path uses.
        /// </summary>
        /// <returns>
        /// The delimiter string that the tree node System.Windows.Forms.TreeNode.FullPath property uses. The default is the backslash character (\).
        /// </returns>
        string PathSeparator { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the System.Windows.Forms.TreeView should be laid out from right-to-left.
        /// </summary>
        /// <returns>
        /// true to indicate the control should be laid out from right-to-left; otherwise, false. The default is false.
        /// </returns>
        bool RightToLeftLayout { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the tree view control displays scroll bars when they are needed.
        /// </summary>
        /// <returns>
        /// true if the tree view control displays scroll bars when they are needed; otherwise, false. The default is true.
        /// </returns>
        bool Scrollable { set; get; }

        /// <summary>
        /// Gets or sets the image list index value of the image that is displayed when a tree node is selected.
        /// </summary>
        /// <returns>
        /// A zero-based index value that represents the position of an System.Drawing.Image in an System.Windows.Forms.ImageList.
        /// </returns>
        int SelectedImageIndex { set; get; }

        /// <summary>
        /// Gets or sets the key of the default image shown when a System.Windows.Forms.TreeNode is in a selected state.
        /// </summary>
        /// <returns>
        /// The key of the default image shown when a System.Windows.Forms.TreeNode is in a selected state.
        /// </returns>
        string SelectedImageKey { set; get; }

        /// <summary>
        /// Gets or sets the tree node that is currently selected in the tree view control.
        /// </summary>
        /// <returns>
        /// The System.Windows.Forms.TreeNode that is currently selected in the tree view control.
        /// </returns>
        System.Windows.Forms.TreeNode SelectedNode { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether lines are drawn between tree nodes in the tree view control.
        /// <returns>
        /// true if lines are drawn between tree nodes in the tree view control; otherwise, false. The default is true.
        /// </returns>
        /// </summary>
        bool ShowLines { set; get; }

        /// <summary>
        /// Gets or sets a value indicating ToolTips are shown when the mouse pointer hovers over a System.Windows.Forms.TreeNode.
        /// </summary>
        /// <returns>
        /// true if ToolTips are shown when the mouse pointer hovers over a System.Windows.Forms.TreeNode; otherwise, false. The default is false.
        /// </returns>
        bool ShowNodeToolTips { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether plus-sign (+) and minus-sign (-) buttons are displayed next to tree nodes that contain child tree nodes.
        /// </summary>
        /// <returns>
        /// true if plus sign and minus sign buttons are displayed next to tree nodes that contain child tree nodes; otherwise, false. The default is true.
        /// </returns>
        bool ShowPlusMinus { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether lines are drawn between the tree nodes that are at the root of the tree view.
        /// </summary>
        /// <returns>
        /// true if lines are drawn between the tree nodes that are at the root of the tree view; otherwise, false. The default is true.
        /// </returns>
        bool ShowRootLines { set; get; }

        /// <summary>
        /// Gets or sets the image list used for indicating the state of the System.Windows.Forms.TreeView and its nodes.
        /// </summary>
        /// <returns>
        /// The System.Windows.Forms.ImageList used for indicating the state of the System.Windows.Forms.TreeView and its nodes.
        /// </returns>
        System.Windows.Forms.ImageList StateImageList { set; get; }

        /// <summary>
        /// Gets or sets the first fully-visible tree node in the tree view control.
        /// </summary>
        /// <returns>
        /// A System.Windows.Forms.TreeNode that represents the first fully-visible tree node in the tree view control.
        /// </returns>
        System.Windows.Forms.TreeNode TopNode { set; get; }

        /// <summary>
        /// Gets or sets the implementation of System.Collections.IComparer to perform a custom sort of the System.Windows.Forms.TreeView nodes.
        /// </summary>
        /// <returns>
        /// The System.Collections.IComparer to perform the custom sort.
        /// </returns>
        System.Collections.IComparer TreeViewNodeSorter { set; get; }

        /// <summary>
        /// Gets the number of tree nodes that can be fully visible in the tree view control.
        /// </summary>
        /// <returns>
        /// The number of System.Windows.Forms.TreeNode items that can be fully visible in the System.Windows.Forms.TreeView control.
        /// </returns>
        int VisibleCount { get; }

        #endregion

        #region Events
        /// <summary>
        /// Occurs after the tree node check box is checked.
        /// </summary>
        event System.Windows.Forms.TreeViewEventHandler AfterCheck;

        /// <summary>
        /// Occurs after the tree node is collapsed.
        /// </summary>
        event System.Windows.Forms.TreeViewEventHandler AfterCollapse;

        /// <summary>
        /// Occurs after the tree node is expanded.
        /// </summary>
        event System.Windows.Forms.TreeViewEventHandler AfterExpand;

        /// <summary>
        /// Occurs after the tree node label text is edited.
        /// </summary>
        event System.Windows.Forms.NodeLabelEditEventHandler AfterLabelEdit;

        /// <summary>
        /// Occurs after the tree node is selected.
        /// </summary>
        event System.Windows.Forms.TreeViewEventHandler AfterSelect;

        /// <summary>
        /// Occurs before the tree node check box is checked.
        /// </summary>
        event System.Windows.Forms.TreeViewCancelEventHandler BeforeCheck;

        /// <summary>
        /// Occurs before the tree node is collapsed.
        /// </summary>
        event System.Windows.Forms.TreeViewCancelEventHandler BeforeCollapse;

        /// <summary>
        /// Occurs before the tree node is expanded.
        /// </summary>
        event System.Windows.Forms.TreeViewCancelEventHandler BeforeExpand;

        /// <summary>
        /// Occurs before the tree node label text is edited.
        /// </summary>
        event System.Windows.Forms.NodeLabelEditEventHandler BeforeLabelEdit;

        /// <summary>
        /// Occurs before the tree node is selected.
        /// </summary>
        event System.Windows.Forms.TreeViewCancelEventHandler BeforeSelect;

        /// <summary>
        /// Occurs when a System.Windows.Forms.TreeView is drawn and the System.Windows.Forms.TreeView.DrawMode property is set to a System.Windows.Forms.TreeViewDrawMode value other than System.Windows.Forms.TreeViewDrawMode.Normal.
        /// </summary>
        event System.Windows.Forms.DrawTreeNodeEventHandler DrawNode;

        /// <summary>
        /// Occurs when the user begins dragging a node.
        /// </summary>
        event System.Windows.Forms.ItemDragEventHandler ItemDrag;

        /// <summary>
        /// Occurs when the user clicks a System.Windows.Forms.TreeNode with the mouse.
        /// </summary>
        event System.Windows.Forms.TreeNodeMouseClickEventHandler NodeMouseClick;

        /// <summary>
        /// Occurs when the user double-clicks a System.Windows.Forms.TreeNode with the mouse.
        /// </summary>
        event System.Windows.Forms.TreeNodeMouseClickEventHandler NodeMouseDoubleClick;

        /// <summary>
        /// Occurs when the mouse hovers over a System.Windows.Forms.TreeNode.
        /// </summary>
        event System.Windows.Forms.TreeNodeMouseHoverEventHandler NodeMouseHover;

        /// <summary>
        /// Occurs when the value of the System.Windows.Forms.TreeView.RightToLeftLayout property changes.
        /// </summary>
        event System.EventHandler RightToLeftLayoutChanged;


        #endregion

    }
}
