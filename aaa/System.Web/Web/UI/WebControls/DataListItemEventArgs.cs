using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000550 RID: 1360
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataListItemEventArgs : EventArgs
	{
		// Token: 0x06004324 RID: 17188 RVA: 0x00115561 File Offset: 0x00114561
		public DataListItemEventArgs(DataListItem item)
		{
			this.item = item;
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06004325 RID: 17189 RVA: 0x00115570 File Offset: 0x00114570
		public DataListItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x04002948 RID: 10568
		private DataListItem item;
	}
}
