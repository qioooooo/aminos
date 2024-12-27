using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000CD RID: 205
	internal struct STORE_CATEGORY_INSTANCE
	{
		// Token: 0x04000D73 RID: 3443
		public IDefinitionAppId DefinitionAppId_Application;

		// Token: 0x04000D74 RID: 3444
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XMLSnippet;
	}
}
