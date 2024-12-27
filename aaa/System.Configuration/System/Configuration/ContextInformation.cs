using System;

namespace System.Configuration
{
	// Token: 0x02000054 RID: 84
	public sealed class ContextInformation
	{
		// Token: 0x0600036E RID: 878 RVA: 0x0001270A File Offset: 0x0001170A
		internal ContextInformation(BaseConfigurationRecord configRecord)
		{
			this._hostingContextEvaluated = false;
			this._hostingContext = null;
			this._configRecord = configRecord;
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00012727 File Offset: 0x00011727
		public object HostingContext
		{
			get
			{
				if (!this._hostingContextEvaluated)
				{
					this._hostingContext = this._configRecord.ConfigContext;
					this._hostingContextEvaluated = true;
				}
				return this._hostingContext;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0001274F File Offset: 0x0001174F
		public bool IsMachineLevel
		{
			get
			{
				return this._configRecord.IsMachineConfig;
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001275C File Offset: 0x0001175C
		public object GetSection(string sectionName)
		{
			return this._configRecord.GetSection(sectionName);
		}

		// Token: 0x040002D1 RID: 721
		private bool _hostingContextEvaluated;

		// Token: 0x040002D2 RID: 722
		private object _hostingContext;

		// Token: 0x040002D3 RID: 723
		private BaseConfigurationRecord _configRecord;
	}
}
