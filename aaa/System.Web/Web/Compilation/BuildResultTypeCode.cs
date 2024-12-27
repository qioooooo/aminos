using System;

namespace System.Web.Compilation
{
	// Token: 0x02000141 RID: 321
	internal enum BuildResultTypeCode
	{
		// Token: 0x040015C8 RID: 5576
		Invalid = -1,
		// Token: 0x040015C9 RID: 5577
		BuildResultCompiledAssembly = 1,
		// Token: 0x040015CA RID: 5578
		BuildResultCompiledType,
		// Token: 0x040015CB RID: 5579
		BuildResultCompiledTemplateType,
		// Token: 0x040015CC RID: 5580
		BuildResultCustomString = 5,
		// Token: 0x040015CD RID: 5581
		BuildResultMainCodeAssembly,
		// Token: 0x040015CE RID: 5582
		BuildResultCodeCompileUnit,
		// Token: 0x040015CF RID: 5583
		BuildResultCompiledGlobalAsaxType,
		// Token: 0x040015D0 RID: 5584
		BuildResultResourceAssembly
	}
}
