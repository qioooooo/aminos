using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005A8 RID: 1448
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewRowEventArgs : EventArgs
	{
		// Token: 0x0600472D RID: 18221 RVA: 0x00123D19 File Offset: 0x00122D19
		public GridViewRowEventArgs(GridViewRow row)
		{
			this._row = row;
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x0600472E RID: 18222 RVA: 0x00123D28 File Offset: 0x00122D28
		public GridViewRow Row
		{
			get
			{
				return this._row;
			}
		}

		// Token: 0x04002A87 RID: 10887
		private GridViewRow _row;
	}
}
