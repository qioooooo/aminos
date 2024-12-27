using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x0200018D RID: 397
	[ComVisible(true)]
	public interface IRootDesigner : IDesigner, IDisposable
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000C95 RID: 3221
		ViewTechnology[] SupportedTechnologies { get; }

		// Token: 0x06000C96 RID: 3222
		object GetView(ViewTechnology technology);
	}
}
