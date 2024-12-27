using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200059A RID: 1434
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewCancelEditEventArgs : CancelEventArgs
	{
		// Token: 0x060046E6 RID: 18150 RVA: 0x00123A7B File Offset: 0x00122A7B
		public GridViewCancelEditEventArgs(int rowIndex)
		{
			this._rowIndex = rowIndex;
		}

		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x060046E7 RID: 18151 RVA: 0x00123A8A File Offset: 0x00122A8A
		public int RowIndex
		{
			get
			{
				return this._rowIndex;
			}
		}

		// Token: 0x04002A74 RID: 10868
		private int _rowIndex;
	}
}
