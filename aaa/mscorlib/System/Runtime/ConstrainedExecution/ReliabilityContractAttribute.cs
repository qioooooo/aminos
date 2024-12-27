using System;

namespace System.Runtime.ConstrainedExecution
{
	// Token: 0x020004C1 RID: 1217
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Interface, Inherited = false)]
	public sealed class ReliabilityContractAttribute : Attribute
	{
		// Token: 0x060030DE RID: 12510 RVA: 0x000A8AEF File Offset: 0x000A7AEF
		public ReliabilityContractAttribute(Consistency consistencyGuarantee, Cer cer)
		{
			this._consistency = consistencyGuarantee;
			this._cer = cer;
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x060030DF RID: 12511 RVA: 0x000A8B05 File Offset: 0x000A7B05
		public Consistency ConsistencyGuarantee
		{
			get
			{
				return this._consistency;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060030E0 RID: 12512 RVA: 0x000A8B0D File Offset: 0x000A7B0D
		public Cer Cer
		{
			get
			{
				return this._cer;
			}
		}

		// Token: 0x04001896 RID: 6294
		private Consistency _consistency;

		// Token: 0x04001897 RID: 6295
		private Cer _cer;
	}
}
