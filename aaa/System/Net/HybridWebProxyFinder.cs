using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x020003E7 RID: 999
	internal sealed class HybridWebProxyFinder : IWebProxyFinder, IDisposable
	{
		// Token: 0x06002066 RID: 8294 RVA: 0x0007FB90 File Offset: 0x0007EB90
		static HybridWebProxyFinder()
		{
			HybridWebProxyFinder.InitializeFallbackSettings();
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x0007FB97 File Offset: 0x0007EB97
		public HybridWebProxyFinder(AutoWebProxyScriptEngine engine)
		{
			this.engine = engine;
			this.winHttpFinder = new WinHttpWebProxyFinder(engine);
			this.currentFinder = this.winHttpFinder;
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x0007FBBE File Offset: 0x0007EBBE
		public bool IsValid
		{
			get
			{
				return this.currentFinder.IsValid;
			}
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x0007FBCC File Offset: 0x0007EBCC
		public bool GetProxies(Uri destination, out IList<string> proxyList)
		{
			if (this.currentFinder.GetProxies(destination, out proxyList))
			{
				return true;
			}
			if (HybridWebProxyFinder.allowFallback && this.currentFinder.IsUnrecognizedScheme && this.currentFinder == this.winHttpFinder)
			{
				if (this.netFinder == null)
				{
					this.netFinder = new NetWebProxyFinder(this.engine);
				}
				this.currentFinder = this.netFinder;
				return this.currentFinder.GetProxies(destination, out proxyList);
			}
			return false;
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x0007FC40 File Offset: 0x0007EC40
		public void Abort()
		{
			this.currentFinder.Abort();
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x0007FC4D File Offset: 0x0007EC4D
		public void Reset()
		{
			this.winHttpFinder.Reset();
			if (this.netFinder != null)
			{
				this.netFinder.Reset();
			}
			this.currentFinder = this.winHttpFinder;
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x0007FC79 File Offset: 0x0007EC79
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x0007FC82 File Offset: 0x0007EC82
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.winHttpFinder.Dispose();
				if (this.netFinder != null)
				{
					this.netFinder.Dispose();
				}
			}
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x0007FCA8 File Offset: 0x0007ECA8
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		private static void InitializeFallbackSettings()
		{
			HybridWebProxyFinder.allowFallback = false;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework"))
				{
					try
					{
						if (registryKey.GetValueKind("LegacyWPADSupport") == RegistryValueKind.DWord)
						{
							HybridWebProxyFinder.allowFallback = (int)registryKey.GetValue("LegacyWPADSupport") == 1;
						}
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (IOException)
					{
					}
				}
			}
			catch (SecurityException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x04001FA7 RID: 8103
		private const string allowFallbackKey = "SOFTWARE\\Microsoft\\.NETFramework";

		// Token: 0x04001FA8 RID: 8104
		private const string allowFallbackKeyPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework";

		// Token: 0x04001FA9 RID: 8105
		private const string allowFallbackValueName = "LegacyWPADSupport";

		// Token: 0x04001FAA RID: 8106
		private static bool allowFallback;

		// Token: 0x04001FAB RID: 8107
		private NetWebProxyFinder netFinder;

		// Token: 0x04001FAC RID: 8108
		private WinHttpWebProxyFinder winHttpFinder;

		// Token: 0x04001FAD RID: 8109
		private BaseWebProxyFinder currentFinder;

		// Token: 0x04001FAE RID: 8110
		private AutoWebProxyScriptEngine engine;
	}
}
