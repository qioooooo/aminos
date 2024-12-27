using System;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x02000348 RID: 840
	public abstract class ChannelBinding : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001A77 RID: 6775 RVA: 0x0005C6D1 File Offset: 0x0005B6D1
		protected ChannelBinding()
			: base(true)
		{
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001A78 RID: 6776
		public abstract int Size { get; }
	}
}
