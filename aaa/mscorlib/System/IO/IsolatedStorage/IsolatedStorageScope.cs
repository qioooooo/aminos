using System;
using System.Runtime.InteropServices;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000794 RID: 1940
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum IsolatedStorageScope
	{
		// Token: 0x04002257 RID: 8791
		None = 0,
		// Token: 0x04002258 RID: 8792
		User = 1,
		// Token: 0x04002259 RID: 8793
		Domain = 2,
		// Token: 0x0400225A RID: 8794
		Assembly = 4,
		// Token: 0x0400225B RID: 8795
		Roaming = 8,
		// Token: 0x0400225C RID: 8796
		Machine = 16,
		// Token: 0x0400225D RID: 8797
		Application = 32
	}
}
