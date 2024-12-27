using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007A8 RID: 1960
	[Serializable]
	internal enum BinaryHeaderEnum
	{
		// Token: 0x040022CD RID: 8909
		SerializedStreamHeader,
		// Token: 0x040022CE RID: 8910
		Object,
		// Token: 0x040022CF RID: 8911
		ObjectWithMap,
		// Token: 0x040022D0 RID: 8912
		ObjectWithMapAssemId,
		// Token: 0x040022D1 RID: 8913
		ObjectWithMapTyped,
		// Token: 0x040022D2 RID: 8914
		ObjectWithMapTypedAssemId,
		// Token: 0x040022D3 RID: 8915
		ObjectString,
		// Token: 0x040022D4 RID: 8916
		Array,
		// Token: 0x040022D5 RID: 8917
		MemberPrimitiveTyped,
		// Token: 0x040022D6 RID: 8918
		MemberReference,
		// Token: 0x040022D7 RID: 8919
		ObjectNull,
		// Token: 0x040022D8 RID: 8920
		MessageEnd,
		// Token: 0x040022D9 RID: 8921
		Assembly,
		// Token: 0x040022DA RID: 8922
		ObjectNullMultiple256,
		// Token: 0x040022DB RID: 8923
		ObjectNullMultiple,
		// Token: 0x040022DC RID: 8924
		ArraySinglePrimitive,
		// Token: 0x040022DD RID: 8925
		ArraySingleObject,
		// Token: 0x040022DE RID: 8926
		ArraySingleString,
		// Token: 0x040022DF RID: 8927
		CrossAppDomainMap,
		// Token: 0x040022E0 RID: 8928
		CrossAppDomainString,
		// Token: 0x040022E1 RID: 8929
		CrossAppDomainAssembly,
		// Token: 0x040022E2 RID: 8930
		MethodCall,
		// Token: 0x040022E3 RID: 8931
		MethodReturn
	}
}
