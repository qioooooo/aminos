using System;

namespace System.Runtime.Versioning
{
	// Token: 0x02000937 RID: 2359
	[Flags]
	internal enum SxSRequirements
	{
		// Token: 0x04002C96 RID: 11414
		None = 0,
		// Token: 0x04002C97 RID: 11415
		AppDomainID = 1,
		// Token: 0x04002C98 RID: 11416
		ProcessID = 2,
		// Token: 0x04002C99 RID: 11417
		AssemblyName = 4,
		// Token: 0x04002C9A RID: 11418
		TypeName = 8
	}
}
