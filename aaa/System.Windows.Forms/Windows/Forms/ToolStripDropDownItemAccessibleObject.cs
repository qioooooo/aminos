using System;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020004AA RID: 1194
	public class ToolStripDropDownItemAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
	{
		// Token: 0x060047EB RID: 18411 RVA: 0x001054C2 File Offset: 0x001044C2
		public ToolStripDropDownItemAccessibleObject(ToolStripDropDownItem item)
			: base(item)
		{
			this.owner = item;
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x060047EC RID: 18412 RVA: 0x001054D4 File Offset: 0x001044D4
		public override AccessibleRole Role
		{
			get
			{
				AccessibleRole accessibleRole = base.Owner.AccessibleRole;
				if (accessibleRole != AccessibleRole.Default)
				{
					return accessibleRole;
				}
				return AccessibleRole.MenuItem;
			}
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x001054F8 File Offset: 0x001044F8
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public override void DoDefaultAction()
		{
			ToolStripDropDownItem toolStripDropDownItem = base.Owner as ToolStripDropDownItem;
			if (toolStripDropDownItem != null && toolStripDropDownItem.HasDropDownItems)
			{
				toolStripDropDownItem.ShowDropDown();
				return;
			}
			base.DoDefaultAction();
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x00105529 File Offset: 0x00104529
		public override AccessibleObject GetChild(int index)
		{
			if (this.owner == null || !this.owner.HasDropDownItems)
			{
				return null;
			}
			return this.owner.DropDown.AccessibilityObject.GetChild(index);
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x00105558 File Offset: 0x00104558
		public override int GetChildCount()
		{
			if (this.owner == null || !this.owner.HasDropDownItems)
			{
				return -1;
			}
			if (this.owner.DropDown.LayoutRequired)
			{
				LayoutTransaction.DoLayout(this.owner.DropDown, this.owner.DropDown, PropertyNames.Items);
			}
			return this.owner.DropDown.AccessibilityObject.GetChildCount();
		}

		// Token: 0x040021F9 RID: 8697
		private ToolStripDropDownItem owner;
	}
}
