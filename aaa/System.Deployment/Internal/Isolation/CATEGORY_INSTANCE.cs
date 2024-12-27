using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000FB RID: 251
	internal struct CATEGORY_INSTANCE
	{
		// Token: 0x040004EB RID: 1259
		public IDefinitionAppId DefinitionAppId_Application;

		// Token: 0x040004EC RID: 1260
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XMLSnippet;
	}
}
