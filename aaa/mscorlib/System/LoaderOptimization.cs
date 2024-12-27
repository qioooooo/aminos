using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200005F RID: 95
	[ComVisible(true)]
	[Serializable]
	public enum LoaderOptimization
	{
		// Token: 0x040001CF RID: 463
		NotSpecified,
		// Token: 0x040001D0 RID: 464
		SingleDomain,
		// Token: 0x040001D1 RID: 465
		MultiDomain,
		// Token: 0x040001D2 RID: 466
		MultiDomainHost,
		// Token: 0x040001D3 RID: 467
		[Obsolete("This method has been deprecated. Please use Assembly.Load() instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		DomainMask = 3,
		// Token: 0x040001D4 RID: 468
		[Obsolete("This method has been deprecated. Please use Assembly.Load() instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		DisallowBindings
	}
}
