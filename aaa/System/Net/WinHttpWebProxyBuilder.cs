using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200050D RID: 1293
	internal sealed class WinHttpWebProxyBuilder : WebProxyDataBuilder
	{
		// Token: 0x06002812 RID: 10258 RVA: 0x000A55B0 File Offset: 0x000A45B0
		protected override void BuildInternal()
		{
			UnsafeNclNativeMethods.WinHttp.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG winhttp_CURRENT_USER_IE_PROXY_CONFIG = default(UnsafeNclNativeMethods.WinHttp.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (UnsafeNclNativeMethods.WinHttp.WinHttpGetIEProxyConfigForCurrentUser(ref winhttp_CURRENT_USER_IE_PROXY_CONFIG))
				{
					string text = Marshal.PtrToStringUni(winhttp_CURRENT_USER_IE_PROXY_CONFIG.Proxy);
					string text2 = Marshal.PtrToStringUni(winhttp_CURRENT_USER_IE_PROXY_CONFIG.ProxyBypass);
					string text3 = Marshal.PtrToStringUni(winhttp_CURRENT_USER_IE_PROXY_CONFIG.AutoConfigUrl);
					base.SetProxyAndBypassList(text, text2);
					base.SetAutoDetectSettings(winhttp_CURRENT_USER_IE_PROXY_CONFIG.AutoDetect);
					base.SetAutoProxyUrl(text3);
				}
				else
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error == 8)
					{
						throw new OutOfMemoryException();
					}
					base.SetAutoDetectSettings(true);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(winhttp_CURRENT_USER_IE_PROXY_CONFIG.Proxy);
				Marshal.FreeHGlobal(winhttp_CURRENT_USER_IE_PROXY_CONFIG.ProxyBypass);
				Marshal.FreeHGlobal(winhttp_CURRENT_USER_IE_PROXY_CONFIG.AutoConfigUrl);
			}
		}
	}
}
