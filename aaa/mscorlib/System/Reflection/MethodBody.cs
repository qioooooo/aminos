using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000321 RID: 801
	[ComVisible(true)]
	public sealed class MethodBody
	{
		// Token: 0x06001F51 RID: 8017 RVA: 0x0004F566 File Offset: 0x0004E566
		private MethodBody()
		{
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x0004F56E File Offset: 0x0004E56E
		public int LocalSignatureMetadataToken
		{
			get
			{
				return this.m_localSignatureMetadataToken;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001F53 RID: 8019 RVA: 0x0004F576 File Offset: 0x0004E576
		public IList<LocalVariableInfo> LocalVariables
		{
			get
			{
				return Array.AsReadOnly<LocalVariableInfo>(this.m_localVariables);
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x0004F583 File Offset: 0x0004E583
		public int MaxStackSize
		{
			get
			{
				return this.m_maxStackSize;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x0004F58B File Offset: 0x0004E58B
		public bool InitLocals
		{
			get
			{
				return this.m_initLocals;
			}
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0004F593 File Offset: 0x0004E593
		public byte[] GetILAsByteArray()
		{
			return this.m_IL;
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x0004F59B File Offset: 0x0004E59B
		public IList<ExceptionHandlingClause> ExceptionHandlingClauses
		{
			get
			{
				return Array.AsReadOnly<ExceptionHandlingClause>(this.m_exceptionHandlingClauses);
			}
		}

		// Token: 0x04000D46 RID: 3398
		private byte[] m_IL;

		// Token: 0x04000D47 RID: 3399
		private ExceptionHandlingClause[] m_exceptionHandlingClauses;

		// Token: 0x04000D48 RID: 3400
		private LocalVariableInfo[] m_localVariables;

		// Token: 0x04000D49 RID: 3401
		internal MethodBase m_methodBase;

		// Token: 0x04000D4A RID: 3402
		private int m_localSignatureMetadataToken;

		// Token: 0x04000D4B RID: 3403
		private int m_maxStackSize;

		// Token: 0x04000D4C RID: 3404
		private bool m_initLocals;
	}
}
