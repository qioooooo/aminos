using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008AB RID: 2219
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum X509KeyStorageFlags
	{
		// Token: 0x040029C3 RID: 10691
		DefaultKeySet = 0,
		// Token: 0x040029C4 RID: 10692
		UserKeySet = 1,
		// Token: 0x040029C5 RID: 10693
		MachineKeySet = 2,
		// Token: 0x040029C6 RID: 10694
		Exportable = 4,
		// Token: 0x040029C7 RID: 10695
		UserProtected = 8,
		// Token: 0x040029C8 RID: 10696
		PersistKeySet = 16
	}
}
