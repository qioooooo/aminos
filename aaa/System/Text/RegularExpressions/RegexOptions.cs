using System;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200002A RID: 42
	[Flags]
	public enum RegexOptions
	{
		// Token: 0x0400078E RID: 1934
		None = 0,
		// Token: 0x0400078F RID: 1935
		IgnoreCase = 1,
		// Token: 0x04000790 RID: 1936
		Multiline = 2,
		// Token: 0x04000791 RID: 1937
		ExplicitCapture = 4,
		// Token: 0x04000792 RID: 1938
		Compiled = 8,
		// Token: 0x04000793 RID: 1939
		Singleline = 16,
		// Token: 0x04000794 RID: 1940
		IgnorePatternWhitespace = 32,
		// Token: 0x04000795 RID: 1941
		RightToLeft = 64,
		// Token: 0x04000796 RID: 1942
		ECMAScript = 256,
		// Token: 0x04000797 RID: 1943
		CultureInvariant = 512
	}
}
