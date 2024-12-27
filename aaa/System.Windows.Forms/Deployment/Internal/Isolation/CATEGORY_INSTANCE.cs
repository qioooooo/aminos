using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000D0 RID: 208
	internal struct CATEGORY_INSTANCE
	{
		// Token: 0x04000D77 RID: 3447
		public IDefinitionAppId DefinitionAppId_Application;

		// Token: 0x04000D78 RID: 3448
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XMLSnippet;
	}
}
