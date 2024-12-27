using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200054C RID: 1356
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataListCommandEventArgs : CommandEventArgs
	{
		// Token: 0x06004308 RID: 17160 RVA: 0x00115307 File Offset: 0x00114307
		public DataListCommandEventArgs(DataListItem item, object commandSource, CommandEventArgs originalArgs)
			: base(originalArgs)
		{
			this.item = item;
			this.commandSource = commandSource;
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06004309 RID: 17161 RVA: 0x0011531E File Offset: 0x0011431E
		public DataListItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x0600430A RID: 17162 RVA: 0x00115326 File Offset: 0x00114326
		public object CommandSource
		{
			get
			{
				return this.commandSource;
			}
		}

		// Token: 0x04002942 RID: 10562
		private DataListItem item;

		// Token: 0x04002943 RID: 10563
		private object commandSource;
	}
}
