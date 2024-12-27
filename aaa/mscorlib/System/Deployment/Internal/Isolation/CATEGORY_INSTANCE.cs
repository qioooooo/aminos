using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001DB RID: 475
	internal struct CATEGORY_INSTANCE
	{
		// Token: 0x0400080D RID: 2061
		public IDefinitionAppId DefinitionAppId_Application;

		// Token: 0x0400080E RID: 2062
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XMLSnippet;
	}
}
