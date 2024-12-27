using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007B5 RID: 1973
	[Flags]
	[Serializable]
	internal enum MessageEnum
	{
		// Token: 0x0400233D RID: 9021
		NoArgs = 1,
		// Token: 0x0400233E RID: 9022
		ArgsInline = 2,
		// Token: 0x0400233F RID: 9023
		ArgsIsArray = 4,
		// Token: 0x04002340 RID: 9024
		ArgsInArray = 8,
		// Token: 0x04002341 RID: 9025
		NoContext = 16,
		// Token: 0x04002342 RID: 9026
		ContextInline = 32,
		// Token: 0x04002343 RID: 9027
		ContextInArray = 64,
		// Token: 0x04002344 RID: 9028
		MethodSignatureInArray = 128,
		// Token: 0x04002345 RID: 9029
		PropertyInArray = 256,
		// Token: 0x04002346 RID: 9030
		NoReturnValue = 512,
		// Token: 0x04002347 RID: 9031
		ReturnValueVoid = 1024,
		// Token: 0x04002348 RID: 9032
		ReturnValueInline = 2048,
		// Token: 0x04002349 RID: 9033
		ReturnValueInArray = 4096,
		// Token: 0x0400234A RID: 9034
		ExceptionInArray = 8192,
		// Token: 0x0400234B RID: 9035
		GenericMethod = 32768
	}
}
