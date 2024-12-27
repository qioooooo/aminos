using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004D3 RID: 1235
	[ComVisible(true)]
	[Obsolete("The IDispatchImplAttribute is deprecated.", false)]
	[Serializable]
	public enum IDispatchImplType
	{
		// Token: 0x040018B2 RID: 6322
		SystemDefinedImpl,
		// Token: 0x040018B3 RID: 6323
		InternalImpl,
		// Token: 0x040018B4 RID: 6324
		CompatibleImpl
	}
}
