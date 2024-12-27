using System;

namespace System.Web.UI
{
	// Token: 0x020003DB RID: 987
	[Flags]
	public enum DataSourceCapabilities
	{
		// Token: 0x04002200 RID: 8704
		None = 0,
		// Token: 0x04002201 RID: 8705
		Sort = 1,
		// Token: 0x04002202 RID: 8706
		Page = 2,
		// Token: 0x04002203 RID: 8707
		RetrieveTotalRowCount = 4
	}
}
