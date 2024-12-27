using System;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000297 RID: 663
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	[Serializable]
	public sealed class SqlProcedureAttribute : Attribute
	{
		// Token: 0x0600226D RID: 8813 RVA: 0x0026D8C8 File Offset: 0x0026CCC8
		public SqlProcedureAttribute()
		{
			this.m_fName = null;
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x0600226E RID: 8814 RVA: 0x0026D8E4 File Offset: 0x0026CCE4
		// (set) Token: 0x0600226F RID: 8815 RVA: 0x0026D8F8 File Offset: 0x0026CCF8
		public string Name
		{
			get
			{
				return this.m_fName;
			}
			set
			{
				this.m_fName = value;
			}
		}

		// Token: 0x04001653 RID: 5715
		private string m_fName;
	}
}
