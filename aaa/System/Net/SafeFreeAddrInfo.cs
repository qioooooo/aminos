using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200050E RID: 1294
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeAddrInfo : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002814 RID: 10260 RVA: 0x000A5678 File Offset: 0x000A4678
		private SafeFreeAddrInfo()
			: base(true)
		{
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x000A5681 File Offset: 0x000A4681
		internal static int GetAddrInfo(string nodename, string servicename, ref AddressInfo hints, out SafeFreeAddrInfo outAddrInfo)
		{
			return UnsafeNclNativeMethods.SafeNetHandlesXPOrLater.getaddrinfo(nodename, servicename, ref hints, out outAddrInfo);
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000A568C File Offset: 0x000A468C
		protected override bool ReleaseHandle()
		{
			UnsafeNclNativeMethods.SafeNetHandlesXPOrLater.freeaddrinfo(this.handle);
			return true;
		}

		// Token: 0x0400275D RID: 10077
		private const string WS2_32 = "ws2_32.dll";
	}
}
