using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000480 RID: 1152
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class Triplet
	{
		// Token: 0x060035FF RID: 13823 RVA: 0x000E98E6 File Offset: 0x000E88E6
		public Triplet()
		{
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x000E98EE File Offset: 0x000E88EE
		public Triplet(object x, object y)
		{
			this.First = x;
			this.Second = y;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x000E9904 File Offset: 0x000E8904
		public Triplet(object x, object y, object z)
		{
			this.First = x;
			this.Second = y;
			this.Third = z;
		}

		// Token: 0x0400257A RID: 9594
		public object First;

		// Token: 0x0400257B RID: 9595
		public object Second;

		// Token: 0x0400257C RID: 9596
		public object Third;
	}
}
