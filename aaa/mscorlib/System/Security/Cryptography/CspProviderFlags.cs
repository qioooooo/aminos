using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200085C RID: 2140
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum CspProviderFlags
	{
		// Token: 0x0400285C RID: 10332
		NoFlags = 0,
		// Token: 0x0400285D RID: 10333
		UseMachineKeyStore = 1,
		// Token: 0x0400285E RID: 10334
		UseDefaultKeyContainer = 2,
		// Token: 0x0400285F RID: 10335
		UseNonExportableKey = 4,
		// Token: 0x04002860 RID: 10336
		UseExistingKey = 8,
		// Token: 0x04002861 RID: 10337
		UseArchivableKey = 16,
		// Token: 0x04002862 RID: 10338
		UseUserProtectedKey = 32,
		// Token: 0x04002863 RID: 10339
		NoPrompt = 64
	}
}
