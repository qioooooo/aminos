using System;

namespace System.Diagnostics
{
	// Token: 0x0200075E RID: 1886
	public class InstanceData
	{
		// Token: 0x060039EB RID: 14827 RVA: 0x000F5348 File Offset: 0x000F4348
		public InstanceData(string instanceName, CounterSample sample)
		{
			this.instanceName = instanceName;
			this.sample = sample;
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x060039EC RID: 14828 RVA: 0x000F535E File Offset: 0x000F435E
		public string InstanceName
		{
			get
			{
				return this.instanceName;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x060039ED RID: 14829 RVA: 0x000F5366 File Offset: 0x000F4366
		public CounterSample Sample
		{
			get
			{
				return this.sample;
			}
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x060039EE RID: 14830 RVA: 0x000F536E File Offset: 0x000F436E
		public long RawValue
		{
			get
			{
				return this.sample.RawValue;
			}
		}

		// Token: 0x040032E4 RID: 13028
		private string instanceName;

		// Token: 0x040032E5 RID: 13029
		private CounterSample sample;
	}
}
