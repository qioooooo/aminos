using System;

namespace System
{
	// Token: 0x02000103 RID: 259
	internal static class LOGIC
	{
		// Token: 0x06000EDA RID: 3802 RVA: 0x0002C4D7 File Offset: 0x0002B4D7
		internal static bool IMPLIES(bool p, bool q)
		{
			return !p || q;
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0002C4DF File Offset: 0x0002B4DF
		internal static bool BIJECTION(bool p, bool q)
		{
			return LOGIC.IMPLIES(p, q) && LOGIC.IMPLIES(q, p);
		}
	}
}
