using System;

namespace System.Reflection.Emit
{
	// Token: 0x02000809 RID: 2057
	internal class VarArgMethod
	{
		// Token: 0x06004981 RID: 18817 RVA: 0x00100B6A File Offset: 0x000FFB6A
		internal VarArgMethod(MethodInfo method, SignatureHelper signature)
		{
			this.m_method = method;
			this.m_signature = signature;
		}

		// Token: 0x04002579 RID: 9593
		internal MethodInfo m_method;

		// Token: 0x0400257A RID: 9594
		internal SignatureHelper m_signature;
	}
}
