using System;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000222 RID: 546
	internal sealed class ProcessHostServerConfig : IServerConfig
	{
		// Token: 0x06001D48 RID: 7496 RVA: 0x00085250 File Offset: 0x00084250
		internal static IServerConfig GetInstance()
		{
			if (ProcessHostServerConfig.s_instance == null)
			{
				lock (ProcessHostServerConfig.s_initLock)
				{
					if (ProcessHostServerConfig.s_instance == null)
					{
						ProcessHostServerConfig.s_instance = new ProcessHostServerConfig();
					}
				}
			}
			return ProcessHostServerConfig.s_instance;
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x000852A0 File Offset: 0x000842A0
		static ProcessHostServerConfig()
		{
			HttpRuntime.ForceStaticInit();
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x000852B4 File Offset: 0x000842B4
		private ProcessHostServerConfig()
		{
			if (HostingEnvironment.SupportFunctions == null)
			{
				ProcessHostConfigUtils.InitStandaloneConfig();
			}
			else
			{
				IProcessHostSupportFunctions supportFunctions = HostingEnvironment.SupportFunctions;
				if (supportFunctions != null)
				{
					IntPtr nativeConfigurationSystem = supportFunctions.GetNativeConfigurationSystem();
					if (IntPtr.Zero != nativeConfigurationSystem)
					{
						UnsafeIISMethods.MgdSetNativeConfiguration(nativeConfigurationSystem);
					}
				}
			}
			this._siteNameForCurrentApplication = HostingEnvironment.SiteNameNoDemand;
			if (this._siteNameForCurrentApplication == null)
			{
				this._siteNameForCurrentApplication = ProcessHostConfigUtils.GetSiteNameFromId(1U);
			}
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x00085318 File Offset: 0x00084318
		string IServerConfig.GetSiteNameFromSiteID(string siteID)
		{
			uint num;
			if (!uint.TryParse(siteID, out num))
			{
				return string.Empty;
			}
			return ProcessHostConfigUtils.GetSiteNameFromId(num);
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x0008533C File Offset: 0x0008433C
		string IServerConfig.MapPath(IApplicationHost appHost, VirtualPath path)
		{
			string text = ((appHost == null) ? this._siteNameForCurrentApplication : appHost.GetSiteName());
			string text2 = ProcessHostConfigUtils.MapPathActual(text, path);
			if (FileUtil.IsSuspiciousPhysicalPath(text2))
			{
				throw new InvalidOperationException(SR.GetString("Cannot_map_path", new object[] { path.VirtualPathString }));
			}
			return text2;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00085390 File Offset: 0x00084390
		string[] IServerConfig.GetVirtualSubdirs(VirtualPath path, bool inApp)
		{
			if (!inApp)
			{
				throw new NotSupportedException();
			}
			string virtualPathString = path.VirtualPathString;
			string[] array = null;
			int num = 0;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			int num2 = 0;
			try
			{
				int num3 = 0;
				int num4 = UnsafeIISMethods.MgdGetAppCollection(this._siteNameForCurrentApplication, virtualPathString, out intPtr2, out num2, out intPtr, out num3);
				if (num4 < 0 || intPtr2 == IntPtr.Zero)
				{
					throw new InvalidOperationException(SR.GetString("Cant_Enumerate_NativeDirs", new object[] { num4 }));
				}
				string text = StringUtil.StringFromWCharPtr(intPtr2, num2);
				Marshal.FreeBSTR(intPtr2);
				intPtr2 = IntPtr.Zero;
				num2 = 0;
				array = new string[num3];
				int num5 = virtualPathString.Length;
				if (virtualPathString[num5 - 1] == '/')
				{
					num5--;
				}
				int length = text.Length;
				string text2 = ((num5 > length) ? virtualPathString.Substring(length, num5 - length) : string.Empty);
				uint num6 = 0U;
				while ((ulong)num6 < (ulong)((long)num3))
				{
					num4 = UnsafeIISMethods.MgdGetNextVPath(intPtr, num6, out intPtr2, out num2);
					if (num4 < 0 || intPtr2 == IntPtr.Zero)
					{
						throw new InvalidOperationException(SR.GetString("Cant_Enumerate_NativeDirs", new object[] { num4 }));
					}
					string text3 = ((num2 > 1) ? StringUtil.StringFromWCharPtr(intPtr2, num2) : null);
					Marshal.FreeBSTR(intPtr2);
					intPtr2 = IntPtr.Zero;
					num2 = 0;
					if (text3 != null && text3.Length > text2.Length)
					{
						if (text2.Length == 0)
						{
							if (text3.IndexOf('/', 1) == -1)
							{
								array[num++] = text3.Substring(1);
							}
						}
						else if (StringUtil.EqualsIgnoreCase(text2, 0, text3, 0, text2.Length))
						{
							int num7 = text3.IndexOf('/', 1 + text2.Length);
							if (num7 > -1)
							{
								array[num++] = text3.Substring(text2.Length + 1, num7 - text2.Length);
							}
							else
							{
								array[num++] = text3.Substring(text2.Length + 1);
							}
						}
					}
					num6 += 1U;
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Release(intPtr);
					intPtr = IntPtr.Zero;
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeBSTR(intPtr2);
					intPtr2 = IntPtr.Zero;
				}
			}
			string[] array2 = null;
			if (num > 0)
			{
				array2 = new string[num];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = array[i];
				}
			}
			return array2;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x00085624 File Offset: 0x00084624
		internal bool IsWithinApp(string virtualPath)
		{
			return UnsafeIISMethods.MgdIsWithinApp(this._siteNameForCurrentApplication, HttpRuntime.AppDomainAppVirtualPathString, virtualPath);
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x00085638 File Offset: 0x00084638
		bool IServerConfig.GetUncUser(IApplicationHost appHost, VirtualPath path, out string username, out string password)
		{
			bool flag = false;
			username = null;
			password = null;
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			IntPtr zero2 = IntPtr.Zero;
			int num2 = 0;
			try
			{
				if (UnsafeIISMethods.MgdGetVrPathCreds(appHost.GetSiteName(), path.VirtualPathString, out zero, out num, out zero2, out num2) == 0)
				{
					username = ((num > 0) ? StringUtil.StringFromWCharPtr(zero, num) : null);
					password = ((num2 > 0) ? StringUtil.StringFromWCharPtr(zero2, num2) : null);
					flag = !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.FreeBSTR(zero);
				}
				if (zero2 != IntPtr.Zero)
				{
					Marshal.FreeBSTR(zero2);
				}
			}
			return flag;
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x000856F4 File Offset: 0x000846F4
		long IServerConfig.GetW3WPMemoryLimitInKB()
		{
			long num = 0L;
			int num2 = UnsafeIISMethods.MgdGetMemoryLimitKB(out num);
			if (num2 < 0)
			{
				return 0L;
			}
			return num;
		}

		// Token: 0x0400194C RID: 6476
		private static object s_initLock = new object();

		// Token: 0x0400194D RID: 6477
		private static ProcessHostServerConfig s_instance;

		// Token: 0x0400194E RID: 6478
		private string _siteNameForCurrentApplication;
	}
}
