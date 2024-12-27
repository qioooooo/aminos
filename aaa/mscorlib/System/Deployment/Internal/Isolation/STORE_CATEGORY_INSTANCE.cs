using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001D8 RID: 472
	internal struct STORE_CATEGORY_INSTANCE
	{
		// Token: 0x04000809 RID: 2057
		public IDefinitionAppId DefinitionAppId_Application;

		// Token: 0x0400080A RID: 2058
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XMLSnippet;
	}
}
