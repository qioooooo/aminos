using System;
using System.Runtime.InteropServices;

namespace Microsoft.Vsa
{
	// Token: 0x02000005 RID: 5
	[Guid("a9f6f86e-fcf5-3d8d-97e4-0fe6c7fe2274")]
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	public enum VsaItemType
	{
		// Token: 0x0400000F RID: 15
		Reference,
		// Token: 0x04000010 RID: 16
		AppGlobal,
		// Token: 0x04000011 RID: 17
		Code
	}
}
