using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005A4 RID: 1444
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewPageEventArgs : CancelEventArgs
	{
		// Token: 0x06004710 RID: 18192 RVA: 0x00123BBD File Offset: 0x00122BBD
		public GridViewPageEventArgs(int newPageIndex)
		{
			this._newPageIndex = newPageIndex;
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x06004711 RID: 18193 RVA: 0x00123BCC File Offset: 0x00122BCC
		// (set) Token: 0x06004712 RID: 18194 RVA: 0x00123BD4 File Offset: 0x00122BD4
		public int NewPageIndex
		{
			get
			{
				return this._newPageIndex;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._newPageIndex = value;
			}
		}

		// Token: 0x04002A80 RID: 10880
		private int _newPageIndex;
	}
}
