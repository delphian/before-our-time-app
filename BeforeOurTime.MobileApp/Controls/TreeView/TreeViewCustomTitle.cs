using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Controls.TreeView
{
    /// <summary>
    /// Argument to title formatter callback
    /// </summary>
    public class TreeViewCustomTitle
    {
        /// <summary>
        /// Object that treeview is currently obtaining display text for
        /// </summary>
        /// <remarks>
        /// Caller should know the type of objects being displayed in the
        /// tree view, therefore can cast as needed.
        /// </remarks>
        public object Obj { set; get; }
        /// <summary>
        /// Set title to value to be printed for current object in tree view
        /// </summary>
        public string Title { set; get; }
    }
}
