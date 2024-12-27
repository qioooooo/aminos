using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002DB RID: 731
	internal class TreeViewDesigner : ControlDesigner
	{
		// Token: 0x06001C3A RID: 7226 RVA: 0x0009F178 File Offset: 0x0009E178
		public TreeViewDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x0009F194 File Offset: 0x0009E194
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

		// Token: 0x06001C3C RID: 7228 RVA: 0x0009F1E8 File Offset: 0x0009E1E8
		protected override bool GetHitTest(Point point)
		{
			point = this.Control.PointToClient(point);
			this.tvhit.pt_x = point.X;
			this.tvhit.pt_y = point.Y;
			NativeMethods.SendMessage(this.Control.Handle, 4369, 0, this.tvhit);
			return this.tvhit.flags == 16;
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x0009F258 File Offset: 0x0009E258
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

		// Token: 0x06001C3E RID: 7230 RVA: 0x0009F2AE File Offset: 0x0009E2AE
		private void TreeViewInvalidate(object sender, TreeViewEventArgs e)
		{
			if (this.treeView != null)
			{
				this.treeView.Invalidate();
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001C3F RID: 7231 RVA: 0x0009F2C3 File Offset: 0x0009E2C3
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

		// Token: 0x040015DB RID: 5595
		private NativeMethods.TV_HITTESTINFO tvhit = new NativeMethods.TV_HITTESTINFO();

		// Token: 0x040015DC RID: 5596
		private DesignerActionListCollection _actionLists;

		// Token: 0x040015DD RID: 5597
		private TreeView treeView;
	}
}
