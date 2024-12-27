using System;

namespace System.Management.Instrumentation
{
	// Token: 0x020000A7 RID: 167
	[InstrumentationClass(InstrumentationType.Event)]
	public abstract class BaseEvent : IEvent
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0002386B File Offset: 0x0002286B
		private ProvisionFunction FireFunction
		{
			get
			{
				if (this.fireFunction == null)
				{
					this.fireFunction = Instrumentation.GetFireFunction(base.GetType());
				}
				return this.fireFunction;
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0002388C File Offset: 0x0002288C
		public void Fire()
		{
			this.FireFunction(this);
		}

		// Token: 0x0400029E RID: 670
		private ProvisionFunction fireFunction;
	}
}
