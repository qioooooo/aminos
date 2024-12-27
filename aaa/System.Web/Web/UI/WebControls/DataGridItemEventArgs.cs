using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200053E RID: 1342
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataGridItemEventArgs : EventArgs
	{
		// Token: 0x0600422B RID: 16939 RVA: 0x00112205 File Offset: 0x00111205
		public DataGridItemEventArgs(DataGridItem item)
		{
			this.item = item;
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x0600422C RID: 16940 RVA: 0x00112214 File Offset: 0x00111214
		public DataGridItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x040028F5 RID: 10485
		private DataGridItem item;
	}
}
