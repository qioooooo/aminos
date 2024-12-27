using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x0200060F RID: 1551
	[ComVisible(true)]
	public interface IStackWalk
	{
		// Token: 0x06003855 RID: 14421
		void Assert();

		// Token: 0x06003856 RID: 14422
		void Demand();

		// Token: 0x06003857 RID: 14423
		void Deny();

		// Token: 0x06003858 RID: 14424
		void PermitOnly();
	}
}
