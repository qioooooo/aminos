using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal class TreeViewDesigner : ControlDesigner
	{
		public TreeViewDesigner()
		{
			base.AutoResizeHandles = true;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.treeView != null)
			{
				this.treeView.AfterExpand -= this.TreeViewInvalidate;
				this.treeView.AfterCollapse -= this.TreeViewInvalidate;
				this.treeView = null;
			}
			base.Dispose(disposing);
		}

		protected override bool GetHitTest(Point point)
		{
			point = this.Control.PointToClient(point);
			this.tvhit.pt_x = point.X;
			this.tvhit.pt_y = point.Y;
			NativeMethods.SendMessage(this.Control.Handle, 4369, 0, this.tvhit);
			return this.tvhit.flags == 16;
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.treeView = component as TreeView;
			if (this.treeView != null)
			{
				this.treeView.AfterExpand += this.TreeViewInvalidate;
				this.treeView.AfterCollapse += this.TreeViewInvalidate;
			}
		}

		private void TreeViewInvalidate(object sender, TreeViewEventArgs e)
		{
			if (this.treeView != null)
			{
				this.treeView.Invalidate();
			}
		}

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this._actionLists == null)
				{
					this._actionLists = new DesignerActionListCollection();
					this._actionLists.Add(new TreeViewActionList(this));
				}
				return this._actionLists;
			}
		}

		private NativeMethods.TV_HITTESTINFO tvhit = new NativeMethods.TV_HITTESTINFO();

		private DesignerActionListCollection _actionLists;

		private TreeView treeView;
	}
}
