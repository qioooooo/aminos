using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005C0 RID: 1472
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ImageMapEventArgs : EventArgs
	{
		// Token: 0x06004805 RID: 18437 RVA: 0x001267FD File Offset: 0x001257FD
		public ImageMapEventArgs(string value)
		{
			this._postBackValue = value;
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06004806 RID: 18438 RVA: 0x0012680C File Offset: 0x0012580C
		public string PostBackValue
		{
			get
			{
				return this._postBackValue;
			}
		}

		// Token: 0x04002AC2 RID: 10946
		private string _postBackValue;
	}
}
