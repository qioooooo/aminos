using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F7 RID: 1271
	[ComVisible(true)]
	[Serializable]
	public enum GCHandleType
	{
		// Token: 0x0400196C RID: 6508
		Weak,
		// Token: 0x0400196D RID: 6509
		WeakTrackResurrection,
		// Token: 0x0400196E RID: 6510
		Normal,
		// Token: 0x0400196F RID: 6511
		Pinned
	}
}
