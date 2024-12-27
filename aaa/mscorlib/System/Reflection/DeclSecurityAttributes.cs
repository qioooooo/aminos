using System;

namespace System.Reflection
{
	// Token: 0x02000305 RID: 773
	[Flags]
	[Serializable]
	internal enum DeclSecurityAttributes
	{
		// Token: 0x04000B84 RID: 2948
		ActionMask = 31,
		// Token: 0x04000B85 RID: 2949
		ActionNil = 0,
		// Token: 0x04000B86 RID: 2950
		Request = 1,
		// Token: 0x04000B87 RID: 2951
		Demand = 2,
		// Token: 0x04000B88 RID: 2952
		Assert = 3,
		// Token: 0x04000B89 RID: 2953
		Deny = 4,
		// Token: 0x04000B8A RID: 2954
		PermitOnly = 5,
		// Token: 0x04000B8B RID: 2955
		LinktimeCheck = 6,
		// Token: 0x04000B8C RID: 2956
		InheritanceCheck = 7,
		// Token: 0x04000B8D RID: 2957
		RequestMinimum = 8,
		// Token: 0x04000B8E RID: 2958
		RequestOptional = 9,
		// Token: 0x04000B8F RID: 2959
		RequestRefuse = 10,
		// Token: 0x04000B90 RID: 2960
		PrejitGrant = 11,
		// Token: 0x04000B91 RID: 2961
		PrejitDenied = 12,
		// Token: 0x04000B92 RID: 2962
		NonCasDemand = 13,
		// Token: 0x04000B93 RID: 2963
		NonCasLinkDemand = 14,
		// Token: 0x04000B94 RID: 2964
		NonCasInheritance = 15,
		// Token: 0x04000B95 RID: 2965
		MaximumValue = 15
	}
}
