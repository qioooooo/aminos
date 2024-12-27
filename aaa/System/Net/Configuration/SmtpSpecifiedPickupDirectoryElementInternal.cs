using System;

namespace System.Net.Configuration
{
	// Token: 0x02000667 RID: 1639
	internal sealed class SmtpSpecifiedPickupDirectoryElementInternal
	{
		// Token: 0x060032B7 RID: 12983 RVA: 0x000D724F File Offset: 0x000D624F
		internal SmtpSpecifiedPickupDirectoryElementInternal(SmtpSpecifiedPickupDirectoryElement element)
		{
			this.pickupDirectoryLocation = element.PickupDirectoryLocation;
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060032B8 RID: 12984 RVA: 0x000D7263 File Offset: 0x000D6263
		internal string PickupDirectoryLocation
		{
			get
			{
				return this.pickupDirectoryLocation;
			}
		}

		// Token: 0x04002F63 RID: 12131
		private string pickupDirectoryLocation;
	}
}
