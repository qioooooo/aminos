using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006D4 RID: 1748
	[ComVisible(true)]
	[Serializable]
	public enum ServerProcessing
	{
		// Token: 0x04001FC5 RID: 8133
		Complete,
		// Token: 0x04001FC6 RID: 8134
		OneWay,
		// Token: 0x04001FC7 RID: 8135
		Async
	}
}
