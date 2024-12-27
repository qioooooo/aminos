using System;

namespace System.Web.Compilation
{
	// Token: 0x0200013D RID: 317
	[Flags]
	public enum BuildProviderAppliesTo
	{
		// Token: 0x040015B0 RID: 5552
		Web = 1,
		// Token: 0x040015B1 RID: 5553
		Code = 2,
		// Token: 0x040015B2 RID: 5554
		Resources = 4,
		// Token: 0x040015B3 RID: 5555
		All = 7
	}
}
