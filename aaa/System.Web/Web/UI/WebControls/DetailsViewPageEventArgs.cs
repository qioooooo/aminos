using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000565 RID: 1381
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DetailsViewPageEventArgs : CancelEventArgs
	{
		// Token: 0x0600442C RID: 17452 RVA: 0x001198BA File Offset: 0x001188BA
		public DetailsViewPageEventArgs(int newPageIndex)
		{
			this._newPageIndex = newPageIndex;
		}

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x0600442D RID: 17453 RVA: 0x001198C9 File Offset: 0x001188C9
		// (set) Token: 0x0600442E RID: 17454 RVA: 0x001198D1 File Offset: 0x001188D1
		public int NewPageIndex
		{
			get
			{
				return this._newPageIndex;
			}
			set
			{
				this._newPageIndex = value;
			}
		}

		// Token: 0x040029A2 RID: 10658
		private int _newPageIndex;
	}
}
