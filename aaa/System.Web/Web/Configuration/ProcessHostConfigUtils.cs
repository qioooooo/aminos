using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x0200021F RID: 543
	internal static class ProcessHostConfigUtils
	{
		// Token: 0x06001D31 RID: 7473 RVA: 0x00084C3E File Offset: 0x00083C3E
		static ProcessHostConfigUtils()
		{
			HttpRuntime.ForceStaticInit();
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x00084C48 File Offset: 0x00083C48
		internal static void InitStandaloneConfig()
		{
			if (!HostingEnvironment.IsUnderIISProcess && !ServerConfig.UseMetabase && Interlocked.Exchange(ref ProcessHostConfigUtils.s_InitedExternalConfig, 1) == 0)
			{
				ProcessHostConfigUtils._configWrapper = new ProcessHostConfigUtils.NativeConfigWrapper();
			}
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x00084C7C File Offset: 0x00083C7C
		internal static string MapPathActual(string siteName, VirtualPath path)
		{
			string text = null;
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			try
			{
				int num2 = UnsafeIISMethods.MgdMapPathDirect(siteName, path.VirtualPathString, out zero, out num);
				if (num2 < 0)
				{
					throw new InvalidOperationException(SR.GetString("Cannot_map_path", new object[] { path.VirtualPathString }));
				}
				text = ((zero != IntPtr.Zero) ? StringUtil.StringFromWCharPtr(zero, num) : null);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.FreeBSTR(zero);
				}
			}
			return text;
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x00084D0C File Offset: 0x00083D0C
		internal static string GetSiteNameFromId(uint siteId)
		{
			if (siteId == 1U && ProcessHostConfigUtils.s_defaultSiteName != null)
			{
				return ProcessHostConfigUtils.s_defaultSiteName;
			}
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			string text = null;
			try
			{
				text = ((UnsafeIISMethods.MgdGetSiteNameFromId(siteId, out zero, out num) == 0 && zero != IntPtr.Zero) ? StringUtil.StringFromWCharPtr(zero, num) : string.Empty);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.FreeBSTR(zero);
				}
			}
			if (siteId == 1U)
			{
				ProcessHostConfigUtils.s_defaultSiteName = text;
			}
			return text;
		}

		// Token: 0x04001945 RID: 6469
		internal const uint DEFAULT_SITE_ID_UINT = 1U;

		// Token: 0x04001946 RID: 6470
		internal const string DEFAULT_SITE_ID_STRING = "1";

		// Token: 0x04001947 RID: 6471
		private static string s_defaultSiteName;

		// Token: 0x04001948 RID: 6472
		private static int s_InitedExternalConfig;

		// Token: 0x04001949 RID: 6473
		private static ProcessHostConfigUtils.NativeConfigWrapper _configWrapper;

		// Token: 0x02000220 RID: 544
		private class NativeConfigWrapper : CriticalFinalizerObject
		{
			// Token: 0x06001D35 RID: 7477 RVA: 0x00084D90 File Offset: 0x00083D90
			internal NativeConfigWrapper()
			{
				int num = UnsafeIISMethods.MgdInitNativeConfig();
				if (num < 0)
				{
					ProcessHostConfigUtils.s_InitedExternalConfig = 0;
					throw new InvalidOperationException(SR.GetString("Cant_Init_Native_Config", new object[] { num.ToString("X8", CultureInfo.InvariantCulture) }));
				}
			}

			// Token: 0x06001D36 RID: 7478 RVA: 0x00084DE0 File Offset: 0x00083DE0
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			~NativeConfigWrapper()
			{
				UnsafeIISMethods.MgdTerminateNativeConfig();
			}
		}
	}
}
