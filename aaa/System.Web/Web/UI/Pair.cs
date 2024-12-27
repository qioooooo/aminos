using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200044D RID: 1101
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class Pair
	{
		// Token: 0x06003463 RID: 13411 RVA: 0x000E34DA File Offset: 0x000E24DA
		public Pair()
		{
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x000E34E2 File Offset: 0x000E24E2
		public Pair(object x, object y)
		{
			this.First = x;
			this.Second = y;
		}

		// Token: 0x040024B7 RID: 9399
		public object First;

		// Token: 0x040024B8 RID: 9400
		public object Second;
	}
}
