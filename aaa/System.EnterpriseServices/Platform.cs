using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000099 RID: 153
	internal class Platform
	{
		// Token: 0x0600039E RID: 926 RVA: 0x0000BD21 File Offset: 0x0000AD21
		private Platform()
		{
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0000BD29 File Offset: 0x0000AD29
		internal static Version MTS
		{
			get
			{
				Platform.Initialize();
				return Platform._mts;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000BD35 File Offset: 0x0000AD35
		internal static Version W2K
		{
			get
			{
				Platform.Initialize();
				return Platform._w2k;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000BD41 File Offset: 0x0000AD41
		internal static Version Whistler
		{
			get
			{
				Platform.Initialize();
				return Platform._whistler;
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000BD50 File Offset: 0x0000AD50
		private static void Initialize()
		{
			if (!Platform._initialized)
			{
				lock (typeof(Platform))
				{
					if (!Platform._initialized)
					{
						IntPtr intPtr = IntPtr.Zero;
						Platform._mts = new Version(2, 0);
						Platform._w2k = new Version(3, 0);
						Platform._whistler = new Version(4, 0);
						try
						{
							try
							{
								intPtr = Security.SuspendImpersonation();
								IMtsCatalog mtsCatalog = (IMtsCatalog)new xMtsCatalog();
								Platform._current = new Version(mtsCatalog.MajorVersion(), mtsCatalog.MinorVersion());
							}
							catch (COMException)
							{
								Platform._current = new Version(0, 0);
							}
							finally
							{
								Security.ResumeImpersonation(intPtr);
							}
						}
						catch
						{
							throw;
						}
						Platform._initialized = true;
					}
				}
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000BE38 File Offset: 0x0000AE38
		private static void SetFeatureData(PlatformFeature feature, object value)
		{
			lock (Platform._features)
			{
				if (Platform.FindFeatureData(feature) == null)
				{
					Platform._features.Add(feature, value);
				}
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000BE84 File Offset: 0x0000AE84
		private static object FindFeatureData(PlatformFeature feature)
		{
			return Platform._features[feature];
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000BE98 File Offset: 0x0000AE98
		internal static void Assert(Version platform, string function)
		{
			Platform.Initialize();
			if (Platform._current.Major < platform.Major || (Platform._current.Major == platform.Major && Platform._current.Minor < platform.Minor))
			{
				Platform.Assert(false, function);
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000BEE7 File Offset: 0x0000AEE7
		internal static void Assert(bool fSuccess, string function)
		{
			if (!fSuccess)
			{
				throw new PlatformNotSupportedException(Resource.FormatString("Err_PlatformSupport", function));
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000BF00 File Offset: 0x0000AF00
		internal static bool CheckUserContextPropertySupport()
		{
			bool flag = false;
			Util.OSVERSIONINFOEX osversioninfoex = new Util.OSVERSIONINFOEX();
			if (Util.GetVersionEx(osversioninfoex))
			{
				if (osversioninfoex.MajorVersion > 5)
				{
					return true;
				}
				if (osversioninfoex.MajorVersion == 5 && osversioninfoex.MinorVersion == 1 && osversioninfoex.ServicePackMajor >= 2)
				{
					flag = true;
				}
				else if (osversioninfoex.MajorVersion == 5 && osversioninfoex.MinorVersion == 2 && osversioninfoex.ServicePackMajor >= 1)
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000BF68 File Offset: 0x0000AF68
		internal static object GetFeatureData(PlatformFeature feature)
		{
			object obj = Platform.FindFeatureData(feature);
			if (obj != null)
			{
				return obj;
			}
			switch (feature)
			{
			case PlatformFeature.SWC:
				obj = SWCThunk.IsSWCSupported();
				break;
			case PlatformFeature.UserContextProperties:
				obj = Platform.CheckUserContextPropertySupport();
				break;
			default:
				return null;
			}
			Platform.SetFeatureData(feature, obj);
			return obj;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000BFB7 File Offset: 0x0000AFB7
		internal static bool Supports(PlatformFeature feature)
		{
			return (bool)Platform.GetFeatureData(feature);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000BFC4 File Offset: 0x0000AFC4
		internal static bool IsLessThan(Version platform)
		{
			Platform.Initialize();
			return Platform._current.Major < platform.Major || (Platform._current.Major == platform.Major && Platform._current.Minor < platform.Minor);
		}

		// Token: 0x0400019A RID: 410
		private static Version _mts;

		// Token: 0x0400019B RID: 411
		private static Version _w2k;

		// Token: 0x0400019C RID: 412
		private static Version _whistler;

		// Token: 0x0400019D RID: 413
		private static Version _current;

		// Token: 0x0400019E RID: 414
		private static volatile bool _initialized;

		// Token: 0x0400019F RID: 415
		private static Hashtable _features = new Hashtable();
	}
}
