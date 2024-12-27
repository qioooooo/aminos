using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200040C RID: 1036
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ImageClickEventArgs : EventArgs
	{
		// Token: 0x060032A5 RID: 12965 RVA: 0x000DD655 File Offset: 0x000DC655
		public ImageClickEventArgs(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x040023CF RID: 9167
		public int X;

		// Token: 0x040023D0 RID: 9168
		public int Y;
	}
}
