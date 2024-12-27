using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x02000742 RID: 1858
	[TypeConverter("System.Diagnostics.Design.CounterCreationDataConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Serializable]
	public class CounterCreationData
	{
		// Token: 0x0600389E RID: 14494 RVA: 0x000EF1CC File Offset: 0x000EE1CC
		public CounterCreationData()
		{
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x000EF1F5 File Offset: 0x000EE1F5
		public CounterCreationData(string counterName, string counterHelp, PerformanceCounterType counterType)
		{
			this.CounterType = counterType;
			this.CounterName = counterName;
			this.CounterHelp = counterHelp;
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x060038A0 RID: 14496 RVA: 0x000EF233 File Offset: 0x000EE233
		// (set) Token: 0x060038A1 RID: 14497 RVA: 0x000EF23B File Offset: 0x000EE23B
		[DefaultValue(PerformanceCounterType.NumberOfItems32)]
		[MonitoringDescription("CounterType")]
		public PerformanceCounterType CounterType
		{
			get
			{
				return this.counterType;
			}
			set
			{
				if (!Enum.IsDefined(typeof(PerformanceCounterType), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PerformanceCounterType));
				}
				this.counterType = value;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x060038A2 RID: 14498 RVA: 0x000EF271 File Offset: 0x000EE271
		// (set) Token: 0x060038A3 RID: 14499 RVA: 0x000EF279 File Offset: 0x000EE279
		[DefaultValue("")]
		[MonitoringDescription("CounterName")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string CounterName
		{
			get
			{
				return this.counterName;
			}
			set
			{
				PerformanceCounterCategory.CheckValidCounter(value);
				this.counterName = value;
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x060038A4 RID: 14500 RVA: 0x000EF288 File Offset: 0x000EE288
		// (set) Token: 0x060038A5 RID: 14501 RVA: 0x000EF290 File Offset: 0x000EE290
		[DefaultValue("")]
		[MonitoringDescription("CounterHelp")]
		public string CounterHelp
		{
			get
			{
				return this.counterHelp;
			}
			set
			{
				PerformanceCounterCategory.CheckValidHelp(value);
				this.counterHelp = value;
			}
		}

		// Token: 0x0400325A RID: 12890
		private PerformanceCounterType counterType = PerformanceCounterType.NumberOfItems32;

		// Token: 0x0400325B RID: 12891
		private string counterName = string.Empty;

		// Token: 0x0400325C RID: 12892
		private string counterHelp = string.Empty;
	}
}
