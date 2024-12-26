using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x020000AC RID: 172
	[ComVisible(true)]
	[Flags]
	[Guid("BA5ED019-F669-3C35-93AC-3ABF776B62B3")]
	public enum JSFunctionAttributeEnum
	{
		// Token: 0x04000437 RID: 1079
		None = 0,
		// Token: 0x04000438 RID: 1080
		HasArguments = 1,
		// Token: 0x04000439 RID: 1081
		HasThisObject = 2,
		// Token: 0x0400043A RID: 1082
		IsNested = 4,
		// Token: 0x0400043B RID: 1083
		HasStackFrame = 8,
		// Token: 0x0400043C RID: 1084
		HasVarArgs = 16,
		// Token: 0x0400043D RID: 1085
		HasEngine = 32,
		// Token: 0x0400043E RID: 1086
		IsExpandoMethod = 64,
		// Token: 0x0400043F RID: 1087
		IsInstanceNestedClassConstructor = 128,
		// Token: 0x04000440 RID: 1088
		ClassicFunction = 35,
		// Token: 0x04000441 RID: 1089
		NestedFunction = 44,
		// Token: 0x04000442 RID: 1090
		ClassicNestedFunction = 47
	}
}
