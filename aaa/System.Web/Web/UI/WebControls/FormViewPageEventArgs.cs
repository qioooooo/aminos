using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200058F RID: 1423
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormViewPageEventArgs : CancelEventArgs
	{
		// Token: 0x060045E9 RID: 17897 RVA: 0x0011EA8A File Offset: 0x0011DA8A
		public FormViewPageEventArgs(int newPageIndex)
		{
			this._newPageIndex = newPageIndex;
		}

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x060045EA RID: 17898 RVA: 0x0011EA99 File Offset: 0x0011DA99
		// (set) Token: 0x060045EB RID: 17899 RVA: 0x0011EAA1 File Offset: 0x0011DAA1
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

		// Token: 0x04002A23 RID: 10787
		private int _newPageIndex;
	}
}
