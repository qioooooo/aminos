using System;

namespace System.Web.Services.Description
{
	// Token: 0x02000103 RID: 259
	[Flags]
	public enum ServiceDescriptionImportWarnings
	{
		// Token: 0x0400048C RID: 1164
		NoCodeGenerated = 1,
		// Token: 0x0400048D RID: 1165
		OptionalExtensionsIgnored = 2,
		// Token: 0x0400048E RID: 1166
		RequiredExtensionsIgnored = 4,
		// Token: 0x0400048F RID: 1167
		UnsupportedOperationsIgnored = 8,
		// Token: 0x04000490 RID: 1168
		UnsupportedBindingsIgnored = 16,
		// Token: 0x04000491 RID: 1169
		NoMethodsGenerated = 32,
		// Token: 0x04000492 RID: 1170
		SchemaValidation = 64,
		// Token: 0x04000493 RID: 1171
		WsiConformance = 128
	}
}
