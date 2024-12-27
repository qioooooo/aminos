using System;

namespace System.Reflection
{
	// Token: 0x020002F9 RID: 761
	[Flags]
	public enum GenericParameterAttributes
	{
		// Token: 0x04000B12 RID: 2834
		None = 0,
		// Token: 0x04000B13 RID: 2835
		VarianceMask = 3,
		// Token: 0x04000B14 RID: 2836
		Covariant = 1,
		// Token: 0x04000B15 RID: 2837
		Contravariant = 2,
		// Token: 0x04000B16 RID: 2838
		SpecialConstraintMask = 28,
		// Token: 0x04000B17 RID: 2839
		ReferenceTypeConstraint = 4,
		// Token: 0x04000B18 RID: 2840
		NotNullableValueTypeConstraint = 8,
		// Token: 0x04000B19 RID: 2841
		DefaultConstructorConstraint = 16
	}
}
