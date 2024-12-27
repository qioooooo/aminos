using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000462 RID: 1122
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ListItemsCollectionEditor : CollectionEditor
	{
		// Token: 0x060028D8 RID: 10456 RVA: 0x000E0514 File Offset: 0x000DF514
		public ListItemsCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000E051D File Offset: 0x000DF51D
		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x000E0520 File Offset: 0x000DF520
		protected override string HelpTopic
		{
			get
			{
				return "net.ComponentModel.CollectionEditor";
			}
		}
	}
}
