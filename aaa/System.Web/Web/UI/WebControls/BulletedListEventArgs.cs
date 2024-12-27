using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004D1 RID: 1233
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class BulletedListEventArgs : EventArgs
	{
		// Token: 0x06003B5C RID: 15196 RVA: 0x000FA159 File Offset: 0x000F9159
		public BulletedListEventArgs(int index)
		{
			this._index = index;
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06003B5D RID: 15197 RVA: 0x000FA168 File Offset: 0x000F9168
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x040026B9 RID: 9913
		private int _index;
	}
}
