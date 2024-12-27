using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000F8 RID: 248
	internal struct STORE_CATEGORY_INSTANCE
	{
		// Token: 0x040004E7 RID: 1255
		public IDefinitionAppId DefinitionAppId_Application;

		// Token: 0x040004E8 RID: 1256
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XMLSnippet;
	}
}
