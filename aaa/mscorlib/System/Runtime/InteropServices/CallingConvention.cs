using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F3 RID: 1267
	[ComVisible(true)]
	[Serializable]
	public enum CallingConvention
	{
		// Token: 0x04001961 RID: 6497
		Winapi = 1,
		// Token: 0x04001962 RID: 6498
		Cdecl,
		// Token: 0x04001963 RID: 6499
		StdCall,
		// Token: 0x04001964 RID: 6500
		ThisCall,
		// Token: 0x04001965 RID: 6501
		FastCall
	}
}
