using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060A RID: 1546
	internal struct IPOptions
	{
		// Token: 0x06002FD2 RID: 12242 RVA: 0x000CF38C File Offset: 0x000CE38C
		internal IPOptions(PingOptions options)
		{
			this.ttl = 128;
			this.tos = 0;
			this.flags = 0;
			this.optionsSize = 0;
			this.optionsData = IntPtr.Zero;
			if (options != null)
			{
				this.ttl = (byte)options.Ttl;
				if (options.DontFragment)
				{
					this.flags = 2;
				}
			}
		}

		// Token: 0x04002DA9 RID: 11689
		internal byte ttl;

		// Token: 0x04002DAA RID: 11690
		internal byte tos;

		// Token: 0x04002DAB RID: 11691
		internal byte flags;

		// Token: 0x04002DAC RID: 11692
		internal byte optionsSize;

		// Token: 0x04002DAD RID: 11693
		internal IntPtr optionsData;
	}
}
