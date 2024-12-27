using System;

namespace System.Reflection.Emit
{
	// Token: 0x02000807 RID: 2055
	internal class GenericMethodInfo
	{
		// Token: 0x0600497F RID: 18815 RVA: 0x00100B3E File Offset: 0x000FFB3E
		internal GenericMethodInfo(RuntimeMethodHandle method, RuntimeTypeHandle context)
		{
			this.m_method = method;
			this.m_context = context;
		}

		// Token: 0x04002575 RID: 9589
		internal RuntimeMethodHandle m_method;

		// Token: 0x04002576 RID: 9590
		internal RuntimeTypeHandle m_context;
	}
}
