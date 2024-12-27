using System;

namespace System.Net.Mail
{
	// Token: 0x0200069F RID: 1695
	[Flags]
	public enum DeliveryNotificationOptions
	{
		// Token: 0x04003031 RID: 12337
		None = 0,
		// Token: 0x04003032 RID: 12338
		OnSuccess = 1,
		// Token: 0x04003033 RID: 12339
		OnFailure = 2,
		// Token: 0x04003034 RID: 12340
		Delay = 4,
		// Token: 0x04003035 RID: 12341
		Never = 134217728
	}
}
