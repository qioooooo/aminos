using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200053A RID: 1338
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataGridCommandEventArgs : CommandEventArgs
	{
		// Token: 0x06004211 RID: 16913 RVA: 0x001120D3 File Offset: 0x001110D3
		public DataGridCommandEventArgs(DataGridItem item, object commandSource, CommandEventArgs originalArgs)
			: base(originalArgs)
		{
			this.item = item;
			this.commandSource = commandSource;
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x001120EA File Offset: 0x001110EA
		public object CommandSource
		{
			get
			{
				return this.commandSource;
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06004213 RID: 16915 RVA: 0x001120F2 File Offset: 0x001110F2
		public DataGridItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x040028EE RID: 10478
		private DataGridItem item;

		// Token: 0x040028EF RID: 10479
		private object commandSource;
	}
}
