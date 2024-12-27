using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005A2 RID: 1442
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewEditEventArgs : CancelEventArgs
	{
		// Token: 0x06004709 RID: 18185 RVA: 0x00123B9D File Offset: 0x00122B9D
		public GridViewEditEventArgs(int newEditIndex)
		{
			this._newEditIndex = newEditIndex;
		}

		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x0600470A RID: 18186 RVA: 0x00123BAC File Offset: 0x00122BAC
		// (set) Token: 0x0600470B RID: 18187 RVA: 0x00123BB4 File Offset: 0x00122BB4
		public int NewEditIndex
		{
			get
			{
				return this._newEditIndex;
			}
			set
			{
				this._newEditIndex = value;
			}
		}

		// Token: 0x04002A7F RID: 10879
		private int _newEditIndex;
	}
}
