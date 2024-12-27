using System;

namespace System.Configuration.Internal
{
	// Token: 0x02000048 RID: 72
	public interface IConfigErrorInfo
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000329 RID: 809
		string Filename { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600032A RID: 810
		int LineNumber { get; }
	}
}
