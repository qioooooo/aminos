using System;

namespace System.Reflection.Emit
{
	// Token: 0x02000808 RID: 2056
	internal class GenericFieldInfo
	{
		// Token: 0x06004980 RID: 18816 RVA: 0x00100B54 File Offset: 0x000FFB54
		internal GenericFieldInfo(RuntimeFieldHandle field, RuntimeTypeHandle context)
		{
			this.m_field = field;
			this.m_context = context;
		}

		// Token: 0x04002577 RID: 9591
		internal RuntimeFieldHandle m_field;

		// Token: 0x04002578 RID: 9592
		internal RuntimeTypeHandle m_context;
	}
}
