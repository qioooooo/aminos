using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Data
{
	// Token: 0x02000338 RID: 824
	internal static class LocalDBAPI
	{
		// Token: 0x06002B2A RID: 11050 RVA: 0x002A124C File Offset: 0x002A064C
		internal static string GetLocalDbInstanceNameFromServerName(string serverName)
		{
			if (serverName == null)
			{
				return null;
			}
			serverName = serverName.TrimStart(new char[0]);
			if (!serverName.StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			string text = serverName.Substring("(localdb)\\".Length).Trim();
			if (text.Length == 0)
			{
				return null;
			}
			return text;
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x002A12A0 File Offset: 0x002A06A0
		internal static void ReleaseDLLHandles()
		{
			LocalDBAPI.s_userInstanceDLLHandle = IntPtr.Zero;
			LocalDBAPI.s_localDBFormatMessage = null;
			LocalDBAPI.s_localDBCreateInstance = null;
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002B2C RID: 11052 RVA: 0x002A12C4 File Offset: 0x002A06C4
		private static IntPtr UserInstanceDLLHandle
		{
			get
			{
				if (LocalDBAPI.s_userInstanceDLLHandle == IntPtr.Zero)
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						Monitor.Enter(LocalDBAPI.s_dllLock);
						if (LocalDBAPI.s_userInstanceDLLHandle == IntPtr.Zero)
						{
							SNINativeMethodWrapper.SNIQueryInfo(SNINativeMethodWrapper.QTypes.SNI_QUERY_LOCALDB_HMODULE, ref LocalDBAPI.s_userInstanceDLLHandle);
							if (!(LocalDBAPI.s_userInstanceDLLHandle != IntPtr.Zero))
							{
								SNINativeMethodWrapper.SNI_Error sni_Error = new SNINativeMethodWrapper.SNI_Error();
								SNINativeMethodWrapper.SNIGetLastError(sni_Error);
								throw LocalDBAPI.CreateLocalDBException(Res.GetString("LocalDB_FailedGetDLLHandle"), null, 0, (int)sni_Error.sniError);
							}
							Bid.Trace("<sc.LocalDBAPI.UserInstanceDLLHandle> LocalDB - handle obtained");
						}
					}
					finally
					{
						Monitor.Exit(LocalDBAPI.s_dllLock);
					}
				}
				return LocalDBAPI.s_userInstanceDLLHandle;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002B2D RID: 11053 RVA: 0x002A1384 File Offset: 0x002A0784
		private static LocalDBAPI.LocalDBCreateInstanceDelegate LocalDBCreateInstance
		{
			get
			{
				if (LocalDBAPI.s_localDBCreateInstance == null)
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						Monitor.Enter(LocalDBAPI.s_dllLock);
						if (LocalDBAPI.s_localDBCreateInstance == null)
						{
							IntPtr procAddress = SafeNativeMethods.GetProcAddress(LocalDBAPI.UserInstanceDLLHandle, "LocalDBCreateInstance");
							if (procAddress == IntPtr.Zero)
							{
								int lastWin32Error = Marshal.GetLastWin32Error();
								Bid.Trace("<sc.LocalDBAPI.LocalDBCreateInstance> GetProcAddress for LocalDBCreateInstance error 0x{%X}", lastWin32Error);
								throw LocalDBAPI.CreateLocalDBException(Res.GetString("LocalDB_MethodNotFound"));
							}
							LocalDBAPI.s_localDBCreateInstance = (LocalDBAPI.LocalDBCreateInstanceDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(LocalDBAPI.LocalDBCreateInstanceDelegate));
						}
					}
					finally
					{
						Monitor.Exit(LocalDBAPI.s_dllLock);
					}
				}
				return LocalDBAPI.s_localDBCreateInstance;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002B2E RID: 11054 RVA: 0x002A1438 File Offset: 0x002A0838
		private static LocalDBAPI.LocalDBFormatMessageDelegate LocalDBFormatMessage
		{
			get
			{
				if (LocalDBAPI.s_localDBFormatMessage == null)
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						Monitor.Enter(LocalDBAPI.s_dllLock);
						if (LocalDBAPI.s_localDBFormatMessage == null)
						{
							IntPtr procAddress = SafeNativeMethods.GetProcAddress(LocalDBAPI.UserInstanceDLLHandle, "LocalDBFormatMessage");
							if (procAddress == IntPtr.Zero)
							{
								int lastWin32Error = Marshal.GetLastWin32Error();
								Bid.Trace("<sc.LocalDBAPI.LocalDBFormatMessage> GetProcAddress for LocalDBFormatMessage error 0x{%X}", lastWin32Error);
								throw LocalDBAPI.CreateLocalDBException(Res.GetString("LocalDB_MethodNotFound"));
							}
							LocalDBAPI.s_localDBFormatMessage = (LocalDBAPI.LocalDBFormatMessageDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(LocalDBAPI.LocalDBFormatMessageDelegate));
						}
					}
					finally
					{
						Monitor.Exit(LocalDBAPI.s_dllLock);
					}
				}
				return LocalDBAPI.s_localDBFormatMessage;
			}
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x002A14EC File Offset: 0x002A08EC
		internal static string GetLocalDBMessage(int hrCode)
		{
			string text;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				uint num = (uint)stringBuilder.Capacity;
				int num2 = LocalDBAPI.LocalDBFormatMessage(hrCode, 1U, (uint)CultureInfo.CurrentCulture.LCID, stringBuilder, ref num);
				if (num2 >= 0)
				{
					text = stringBuilder.ToString();
				}
				else
				{
					stringBuilder = new StringBuilder(1024);
					num = (uint)stringBuilder.Capacity;
					num2 = LocalDBAPI.LocalDBFormatMessage(hrCode, 1U, 0U, stringBuilder, ref num);
					if (num2 >= 0)
					{
						text = stringBuilder.ToString();
					}
					else
					{
						text = string.Format(CultureInfo.CurrentCulture, "{0} (0x{1:X}).", new object[]
						{
							Res.GetString("LocalDB_UnobtainableMessage"),
							num2
						});
					}
				}
			}
			catch (SqlException ex)
			{
				text = string.Format(CultureInfo.CurrentCulture, "{0} ({1}).", new object[]
				{
					Res.GetString("LocalDB_UnobtainableMessage"),
					ex.Message
				});
			}
			return text;
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x002A15EC File Offset: 0x002A09EC
		private static SqlException CreateLocalDBException(string errorMessage)
		{
			return LocalDBAPI.CreateLocalDBException(errorMessage, null, 0, 0);
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x002A1604 File Offset: 0x002A0A04
		private static SqlException CreateLocalDBException(string errorMessage, string instance, int localDbError, int sniError)
		{
			SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
			int num = ((localDbError == 0) ? sniError : localDbError);
			if (sniError != 0)
			{
				string text = string.Format(null, "SNI_ERROR_{0}", new object[] { sniError });
				errorMessage = string.Format(null, "{0} (error: {1} - {2})", new object[]
				{
					errorMessage,
					sniError,
					Res.GetString(text)
				});
			}
			sqlErrorCollection.Add(new SqlError(num, 0, 20, instance, errorMessage, null, 0));
			if (localDbError != 0)
			{
				sqlErrorCollection.Add(new SqlError(num, 0, 20, instance, LocalDBAPI.GetLocalDBMessage(localDbError), null, 0));
			}
			SqlException ex = SqlException.CreateException(sqlErrorCollection, null);
			ex._doNotReconnect = true;
			return ex;
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x002A16B0 File Offset: 0x002A0AB0
		internal static void DemandLocalDBPermissions()
		{
			if (!LocalDBAPI._partialTrustAllowed)
			{
				if (!LocalDBAPI._partialTrustFlagChecked)
				{
					object data = AppDomain.CurrentDomain.GetData("ALLOW_LOCALDB_IN_PARTIAL_TRUST");
					if (data != null && data is bool)
					{
						LocalDBAPI._partialTrustAllowed = (bool)data;
					}
					LocalDBAPI._partialTrustFlagChecked = true;
					if (LocalDBAPI._partialTrustAllowed)
					{
						return;
					}
				}
				if (LocalDBAPI._fullTrust == null)
				{
					LocalDBAPI._fullTrust = new NamedPermissionSet("FullTrust");
				}
				LocalDBAPI._fullTrust.Demand();
			}
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x002A1720 File Offset: 0x002A0B20
		internal static void AssertLocalDBPermissions()
		{
			LocalDBAPI._partialTrustAllowed = true;
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x002A1734 File Offset: 0x002A0B34
		internal static void CreateLocalDBInstance(string instance)
		{
			LocalDBAPI.DemandLocalDBPermissions();
			if (LocalDBAPI.s_configurableInstances == null)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(LocalDBAPI.s_configLock);
					if (LocalDBAPI.s_configurableInstances == null)
					{
						Dictionary<string, LocalDBAPI.InstanceInfo> dictionary = new Dictionary<string, LocalDBAPI.InstanceInfo>(StringComparer.OrdinalIgnoreCase);
						object section = PrivilegedConfigurationManager.GetSection("system.data.localdb");
						if (section != null)
						{
							LocalDBConfigurationSection localDBConfigurationSection = section as LocalDBConfigurationSection;
							if (localDBConfigurationSection == null)
							{
								throw LocalDBAPI.CreateLocalDBException(Res.GetString("LocalDB_BadConfigSectionType"));
							}
							using (IEnumerator enumerator = localDBConfigurationSection.LocalDbInstances.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									object obj = enumerator.Current;
									LocalDBInstanceElement localDBInstanceElement = (LocalDBInstanceElement)obj;
									dictionary.Add(localDBInstanceElement.Name.Trim(), new LocalDBAPI.InstanceInfo(localDBInstanceElement.Version.Trim()));
								}
								goto IL_00C9;
							}
						}
						Bid.Trace("<sc.LocalDBAPI.CreateLocalDBInstance> No system.data.localdb section found in configuration");
						IL_00C9:
						LocalDBAPI.s_configurableInstances = dictionary;
					}
				}
				finally
				{
					Monitor.Exit(LocalDBAPI.s_configLock);
				}
			}
			LocalDBAPI.InstanceInfo instanceInfo = null;
			if (!LocalDBAPI.s_configurableInstances.TryGetValue(instance, out instanceInfo))
			{
				return;
			}
			if (instanceInfo.created)
			{
				return;
			}
			if (instanceInfo.version.Contains("\0"))
			{
				throw LocalDBAPI.CreateLocalDBException(Res.GetString("LocalDB_InvalidVersion"), instance, 0, 0);
			}
			int num = LocalDBAPI.LocalDBCreateInstance(instanceInfo.version, instance, 0U);
			Bid.Trace("<sc.LocalDBAPI.CreateLocalDBInstance> Starting creation of instance %ls version %ls", instance, instanceInfo.version);
			if (num < 0)
			{
				throw LocalDBAPI.CreateLocalDBException(Res.GetString("LocalDB_CreateFailed"), instance, num, 0);
			}
			Bid.Trace("<sc.LocalDBAPI.CreateLocalDBInstance> Finished creation of instance %ls", instance);
			instanceInfo.created = true;
		}

		// Token: 0x04001C3F RID: 7231
		private const string const_localDbPrefix = "(localdb)\\";

		// Token: 0x04001C40 RID: 7232
		private const string const_partialTrustFlagKey = "ALLOW_LOCALDB_IN_PARTIAL_TRUST";

		// Token: 0x04001C41 RID: 7233
		private const uint const_LOCALDB_TRUNCATE_ERR_MESSAGE = 1U;

		// Token: 0x04001C42 RID: 7234
		private const int const_ErrorMessageBufferSize = 1024;

		// Token: 0x04001C43 RID: 7235
		private static PermissionSet _fullTrust = null;

		// Token: 0x04001C44 RID: 7236
		private static bool _partialTrustFlagChecked = false;

		// Token: 0x04001C45 RID: 7237
		private static bool _partialTrustAllowed = false;

		// Token: 0x04001C46 RID: 7238
		private static IntPtr s_userInstanceDLLHandle = IntPtr.Zero;

		// Token: 0x04001C47 RID: 7239
		private static object s_dllLock = new object();

		// Token: 0x04001C48 RID: 7240
		private static LocalDBAPI.LocalDBCreateInstanceDelegate s_localDBCreateInstance = null;

		// Token: 0x04001C49 RID: 7241
		private static LocalDBAPI.LocalDBFormatMessageDelegate s_localDBFormatMessage = null;

		// Token: 0x04001C4A RID: 7242
		private static object s_configLock = new object();

		// Token: 0x04001C4B RID: 7243
		private static Dictionary<string, LocalDBAPI.InstanceInfo> s_configurableInstances = null;

		// Token: 0x02000339 RID: 825
		// (Invoke) Token: 0x06002B37 RID: 11063
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		[SuppressUnmanagedCodeSecurity]
		private delegate int LocalDBCreateInstanceDelegate([MarshalAs(UnmanagedType.LPWStr)] string version, [MarshalAs(UnmanagedType.LPWStr)] string instance, uint flags);

		// Token: 0x0200033A RID: 826
		// (Invoke) Token: 0x06002B3B RID: 11067
		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private delegate int LocalDBFormatMessageDelegate(int hrLocalDB, uint dwFlags, uint dwLanguageId, StringBuilder buffer, ref uint buflen);

		// Token: 0x0200033B RID: 827
		private class InstanceInfo
		{
			// Token: 0x06002B3E RID: 11070 RVA: 0x002A1930 File Offset: 0x002A0D30
			internal InstanceInfo(string version)
			{
				this.version = version;
				this.created = false;
			}

			// Token: 0x04001C4C RID: 7244
			internal readonly string version;

			// Token: 0x04001C4D RID: 7245
			internal bool created;
		}
	}
}
