using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005A2 RID: 1442
	[ComVisible(true)]
	[Serializable]
	public enum FileMode
	{
		// Token: 0x04001BE9 RID: 7145
		CreateNew = 1,
		// Token: 0x04001BEA RID: 7146
		Create,
		// Token: 0x04001BEB RID: 7147
		Open,
		// Token: 0x04001BEC RID: 7148
		OpenOrCreate,
		// Token: 0x04001BED RID: 7149
		Truncate,
		// Token: 0x04001BEE RID: 7150
		Append
	}
}
