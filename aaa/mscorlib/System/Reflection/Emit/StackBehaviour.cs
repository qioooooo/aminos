using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200082A RID: 2090
	[ComVisible(true)]
	[Serializable]
	public enum StackBehaviour
	{
		// Token: 0x0400273F RID: 10047
		Pop0,
		// Token: 0x04002740 RID: 10048
		Pop1,
		// Token: 0x04002741 RID: 10049
		Pop1_pop1,
		// Token: 0x04002742 RID: 10050
		Popi,
		// Token: 0x04002743 RID: 10051
		Popi_pop1,
		// Token: 0x04002744 RID: 10052
		Popi_popi,
		// Token: 0x04002745 RID: 10053
		Popi_popi8,
		// Token: 0x04002746 RID: 10054
		Popi_popi_popi,
		// Token: 0x04002747 RID: 10055
		Popi_popr4,
		// Token: 0x04002748 RID: 10056
		Popi_popr8,
		// Token: 0x04002749 RID: 10057
		Popref,
		// Token: 0x0400274A RID: 10058
		Popref_pop1,
		// Token: 0x0400274B RID: 10059
		Popref_popi,
		// Token: 0x0400274C RID: 10060
		Popref_popi_popi,
		// Token: 0x0400274D RID: 10061
		Popref_popi_popi8,
		// Token: 0x0400274E RID: 10062
		Popref_popi_popr4,
		// Token: 0x0400274F RID: 10063
		Popref_popi_popr8,
		// Token: 0x04002750 RID: 10064
		Popref_popi_popref,
		// Token: 0x04002751 RID: 10065
		Push0,
		// Token: 0x04002752 RID: 10066
		Push1,
		// Token: 0x04002753 RID: 10067
		Push1_push1,
		// Token: 0x04002754 RID: 10068
		Pushi,
		// Token: 0x04002755 RID: 10069
		Pushi8,
		// Token: 0x04002756 RID: 10070
		Pushr4,
		// Token: 0x04002757 RID: 10071
		Pushr8,
		// Token: 0x04002758 RID: 10072
		Pushref,
		// Token: 0x04002759 RID: 10073
		Varpop,
		// Token: 0x0400275A RID: 10074
		Varpush,
		// Token: 0x0400275B RID: 10075
		Popref_popi_pop1
	}
}
