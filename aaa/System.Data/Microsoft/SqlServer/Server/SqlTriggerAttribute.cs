using System;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200029D RID: 669
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	[Serializable]
	public sealed class SqlTriggerAttribute : Attribute
	{
		// Token: 0x06002291 RID: 8849 RVA: 0x0026DCEC File Offset: 0x0026D0EC
		public SqlTriggerAttribute()
		{
			this.m_fName = null;
			this.m_fTarget = null;
			this.m_fEvent = null;
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06002292 RID: 8850 RVA: 0x0026DD14 File Offset: 0x0026D114
		// (set) Token: 0x06002293 RID: 8851 RVA: 0x0026DD28 File Offset: 0x0026D128
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

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x0026DD3C File Offset: 0x0026D13C
		// (set) Token: 0x06002295 RID: 8853 RVA: 0x0026DD50 File Offset: 0x0026D150
		public string Target
		{
			get
			{
				return this.m_fTarget;
			}
			set
			{
				this.m_fTarget = value;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06002296 RID: 8854 RVA: 0x0026DD64 File Offset: 0x0026D164
		// (set) Token: 0x06002297 RID: 8855 RVA: 0x0026DD78 File Offset: 0x0026D178
		public string Event
		{
			get
			{
				return this.m_fEvent;
			}
			set
			{
				this.m_fEvent = value;
			}
		}

		// Token: 0x0400165A RID: 5722
		private string m_fName;

		// Token: 0x0400165B RID: 5723
		private string m_fTarget;

		// Token: 0x0400165C RID: 5724
		private string m_fEvent;
	}
}
