using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005AC RID: 1452
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewSortEventArgs : CancelEventArgs
	{
		// Token: 0x0600473A RID: 18234 RVA: 0x00123D50 File Offset: 0x00122D50
		public GridViewSortEventArgs(string sortExpression, SortDirection sortDirection)
		{
			this._sortExpression = sortExpression;
			this._sortDirection = sortDirection;
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x0600473B RID: 18235 RVA: 0x00123D66 File Offset: 0x00122D66
		// (set) Token: 0x0600473C RID: 18236 RVA: 0x00123D6E File Offset: 0x00122D6E
		public SortDirection SortDirection
		{
			get
			{
				return this._sortDirection;
			}
			set
			{
				this._sortDirection = value;
			}
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x0600473D RID: 18237 RVA: 0x00123D77 File Offset: 0x00122D77
		// (set) Token: 0x0600473E RID: 18238 RVA: 0x00123D7F File Offset: 0x00122D7F
		public string SortExpression
		{
			get
			{
				return this._sortExpression;
			}
			set
			{
				this._sortExpression = value;
			}
		}

		// Token: 0x04002A89 RID: 10889
		private string _sortExpression;

		// Token: 0x04002A8A RID: 10890
		private SortDirection _sortDirection;
	}
}
