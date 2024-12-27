using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005AA RID: 1450
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewSelectEventArgs : CancelEventArgs
	{
		// Token: 0x06004733 RID: 18227 RVA: 0x00123D30 File Offset: 0x00122D30
		public GridViewSelectEventArgs(int newSelectedIndex)
		{
			this._newSelectedIndex = newSelectedIndex;
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x06004734 RID: 18228 RVA: 0x00123D3F File Offset: 0x00122D3F
		// (set) Token: 0x06004735 RID: 18229 RVA: 0x00123D47 File Offset: 0x00122D47
		public int NewSelectedIndex
		{
			get
			{
				return this._newSelectedIndex;
			}
			set
			{
				this._newSelectedIndex = value;
			}
		}

		// Token: 0x04002A88 RID: 10888
		private int _newSelectedIndex;
	}
}
