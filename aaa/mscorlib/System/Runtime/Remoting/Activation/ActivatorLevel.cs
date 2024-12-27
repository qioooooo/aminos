using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020006C8 RID: 1736
	[ComVisible(true)]
	[Serializable]
	public enum ActivatorLevel
	{
		// Token: 0x04001FBF RID: 8127
		Construction = 4,
		// Token: 0x04001FC0 RID: 8128
		Context = 8,
		// Token: 0x04001FC1 RID: 8129
		AppDomain = 12,
		// Token: 0x04001FC2 RID: 8130
		Process = 16,
		// Token: 0x04001FC3 RID: 8131
		Machine = 20
	}
}
