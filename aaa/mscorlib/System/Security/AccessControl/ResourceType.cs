using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000906 RID: 2310
	public enum ResourceType
	{
		// Token: 0x04002B5C RID: 11100
		Unknown,
		// Token: 0x04002B5D RID: 11101
		FileObject,
		// Token: 0x04002B5E RID: 11102
		Service,
		// Token: 0x04002B5F RID: 11103
		Printer,
		// Token: 0x04002B60 RID: 11104
		RegistryKey,
		// Token: 0x04002B61 RID: 11105
		LMShare,
		// Token: 0x04002B62 RID: 11106
		KernelObject,
		// Token: 0x04002B63 RID: 11107
		WindowObject,
		// Token: 0x04002B64 RID: 11108
		DSObject,
		// Token: 0x04002B65 RID: 11109
		DSObjectAll,
		// Token: 0x04002B66 RID: 11110
		ProviderDefined,
		// Token: 0x04002B67 RID: 11111
		WmiGuidObject,
		// Token: 0x04002B68 RID: 11112
		RegistryWow6432Key
	}
}
