using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000043 RID: 67
	[ComVisible(true)]
	[Serializable]
	public enum CodeBinaryOperatorType
	{
		// Token: 0x040007EC RID: 2028
		Add,
		// Token: 0x040007ED RID: 2029
		Subtract,
		// Token: 0x040007EE RID: 2030
		Multiply,
		// Token: 0x040007EF RID: 2031
		Divide,
		// Token: 0x040007F0 RID: 2032
		Modulus,
		// Token: 0x040007F1 RID: 2033
		Assign,
		// Token: 0x040007F2 RID: 2034
		IdentityInequality,
		// Token: 0x040007F3 RID: 2035
		IdentityEquality,
		// Token: 0x040007F4 RID: 2036
		ValueEquality,
		// Token: 0x040007F5 RID: 2037
		BitwiseOr,
		// Token: 0x040007F6 RID: 2038
		BitwiseAnd,
		// Token: 0x040007F7 RID: 2039
		BooleanOr,
		// Token: 0x040007F8 RID: 2040
		BooleanAnd,
		// Token: 0x040007F9 RID: 2041
		LessThan,
		// Token: 0x040007FA RID: 2042
		LessThanOrEqual,
		// Token: 0x040007FB RID: 2043
		GreaterThan,
		// Token: 0x040007FC RID: 2044
		GreaterThanOrEqual
	}
}
