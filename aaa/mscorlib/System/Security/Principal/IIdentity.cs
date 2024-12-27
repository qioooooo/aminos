using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004AE RID: 1198
	[ComVisible(true)]
	public interface IIdentity
	{
		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06003092 RID: 12434
		string Name { get; }

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06003093 RID: 12435
		string AuthenticationType { get; }

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06003094 RID: 12436
		bool IsAuthenticated { get; }
	}
}
