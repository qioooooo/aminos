using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x0200016F RID: 367
	[ComVisible(true)]
	[Serializable]
	public enum ApartmentState
	{
		// Token: 0x04000694 RID: 1684
		STA,
		// Token: 0x04000695 RID: 1685
		MTA,
		// Token: 0x04000696 RID: 1686
		Unknown
	}
}
