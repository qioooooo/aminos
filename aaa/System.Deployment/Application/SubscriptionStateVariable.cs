using System;

namespace System.Deployment.Application
{
	// Token: 0x0200002E RID: 46
	internal class SubscriptionStateVariable
	{
		// Token: 0x0600019E RID: 414 RVA: 0x0000B907 File Offset: 0x0000A907
		public SubscriptionStateVariable(string name, object newValue, object oldValue)
		{
			this.PropertyName = name;
			this.NewValue = newValue;
			this.OldValue = oldValue;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000B924 File Offset: 0x0000A924
		public bool IsUnchanged
		{
			get
			{
				if (this.NewValue == null)
				{
					return this.OldValue == null;
				}
				return this.NewValue.Equals(this.OldValue);
			}
		}

		// Token: 0x04000106 RID: 262
		public string PropertyName;

		// Token: 0x04000107 RID: 263
		public object NewValue;

		// Token: 0x04000108 RID: 264
		public object OldValue;
	}
}
