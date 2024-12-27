using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x0200072F RID: 1839
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum SoapOption
	{
		// Token: 0x04002112 RID: 8466
		None = 0,
		// Token: 0x04002113 RID: 8467
		AlwaysIncludeTypes = 1,
		// Token: 0x04002114 RID: 8468
		XsdString = 2,
		// Token: 0x04002115 RID: 8469
		EmbedAll = 4,
		// Token: 0x04002116 RID: 8470
		Option1 = 8,
		// Token: 0x04002117 RID: 8471
		Option2 = 16
	}
}
