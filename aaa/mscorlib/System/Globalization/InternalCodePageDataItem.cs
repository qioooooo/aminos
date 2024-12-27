using System;

namespace System.Globalization
{
	// Token: 0x0200039E RID: 926
	internal struct InternalCodePageDataItem
	{
		// Token: 0x040010D1 RID: 4305
		internal int codePage;

		// Token: 0x040010D2 RID: 4306
		internal int uiFamilyCodePage;

		// Token: 0x040010D3 RID: 4307
		internal unsafe char* webName;

		// Token: 0x040010D4 RID: 4308
		internal unsafe char* headerName;

		// Token: 0x040010D5 RID: 4309
		internal unsafe char* bodyName;

		// Token: 0x040010D6 RID: 4310
		internal uint flags;
	}
}
