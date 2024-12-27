using System;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000287 RID: 647
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	[Serializable]
	public sealed class SqlMethodAttribute : SqlFunctionAttribute
	{
		// Token: 0x0600221F RID: 8735 RVA: 0x0026C9D8 File Offset: 0x0026BDD8
		public SqlMethodAttribute()
		{
			this.m_fCallOnNullInputs = true;
			this.m_fMutator = false;
			this.m_fInvokeIfReceiverIsNull = false;
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x0026CA00 File Offset: 0x0026BE00
		// (set) Token: 0x06002221 RID: 8737 RVA: 0x0026CA14 File Offset: 0x0026BE14
		public bool OnNullCall
		{
			get
			{
				return this.m_fCallOnNullInputs;
			}
			set
			{
				this.m_fCallOnNullInputs = value;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x0026CA28 File Offset: 0x0026BE28
		// (set) Token: 0x06002223 RID: 8739 RVA: 0x0026CA3C File Offset: 0x0026BE3C
		public bool IsMutator
		{
			get
			{
				return this.m_fMutator;
			}
			set
			{
				this.m_fMutator = value;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x0026CA50 File Offset: 0x0026BE50
		// (set) Token: 0x06002225 RID: 8741 RVA: 0x0026CA64 File Offset: 0x0026BE64
		public bool InvokeIfReceiverIsNull
		{
			get
			{
				return this.m_fInvokeIfReceiverIsNull;
			}
			set
			{
				this.m_fInvokeIfReceiverIsNull = value;
			}
		}

		// Token: 0x04001644 RID: 5700
		private bool m_fCallOnNullInputs;

		// Token: 0x04001645 RID: 5701
		private bool m_fMutator;

		// Token: 0x04001646 RID: 5702
		private bool m_fInvokeIfReceiverIsNull;
	}
}
