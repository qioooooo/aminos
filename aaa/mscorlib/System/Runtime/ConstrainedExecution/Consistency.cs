using System;

namespace System.Runtime.ConstrainedExecution
{
	// Token: 0x020004BF RID: 1215
	[Serializable]
	public enum Consistency
	{
		// Token: 0x0400188E RID: 6286
		MayCorruptProcess,
		// Token: 0x0400188F RID: 6287
		MayCorruptAppDomain,
		// Token: 0x04001890 RID: 6288
		MayCorruptInstance,
		// Token: 0x04001891 RID: 6289
		WillNotCorruptState
	}
}
