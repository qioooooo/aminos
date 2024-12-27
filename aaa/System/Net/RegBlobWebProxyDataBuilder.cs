using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x0200050B RID: 1291
	internal class RegBlobWebProxyDataBuilder : WebProxyDataBuilder
	{
		// Token: 0x0600280D RID: 10253 RVA: 0x000A5322 File Offset: 0x000A4322
		public RegBlobWebProxyDataBuilder(string connectoid, SafeRegistryHandle registry)
		{
			this.m_Registry = registry;
			this.m_Connectoid = connectoid;
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000A5338 File Offset: 0x000A4338
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings")]
		private bool ReadRegSettings()
		{
			SafeRegistryHandle safeRegistryHandle = null;
			RegistryKey registryKey = null;
			try
			{
				bool flag = true;
				registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
				if (registryKey != null)
				{
					object value = registryKey.GetValue("ProxySettingsPerUser");
					if (value != null && value.GetType() == typeof(int) && (int)value == 0)
					{
						flag = false;
					}
				}
				uint num;
				if (flag)
				{
					if (this.m_Registry != null)
					{
						num = this.m_Registry.RegOpenKeyEx("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections", 0U, 131097U, out safeRegistryHandle);
					}
					else
					{
						num = 1168U;
					}
				}
				else
				{
					num = SafeRegistryHandle.RegOpenKeyEx(UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections", 0U, 131097U, out safeRegistryHandle);
				}
				if (num != 0U)
				{
					safeRegistryHandle = null;
				}
				object obj;
				if (safeRegistryHandle != null && safeRegistryHandle.QueryValue((this.m_Connectoid != null) ? this.m_Connectoid : "DefaultConnectionSettings", out obj) == 0U)
				{
					this.m_RegistryBytes = (byte[])obj;
				}
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (safeRegistryHandle != null)
				{
					safeRegistryHandle.RegCloseKey();
				}
			}
			return this.m_RegistryBytes != null;
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000A545C File Offset: 0x000A445C
		public string ReadString()
		{
			string text = null;
			int num = this.ReadInt32();
			if (num > 0)
			{
				int num2 = this.m_RegistryBytes.Length - this.m_ByteOffset;
				if (num >= num2)
				{
					num = num2;
				}
				text = Encoding.UTF8.GetString(this.m_RegistryBytes, this.m_ByteOffset, num);
				this.m_ByteOffset += num;
			}
			return text;
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x000A54B4 File Offset: 0x000A44B4
		internal unsafe int ReadInt32()
		{
			int num = 0;
			int num2 = this.m_RegistryBytes.Length - this.m_ByteOffset;
			if (num2 >= 4)
			{
				fixed (byte* registryBytes = this.m_RegistryBytes)
				{
					if (sizeof(IntPtr) == 4)
					{
						num = ((int*)registryBytes)[this.m_ByteOffset / 4];
					}
					else
					{
						num = Marshal.ReadInt32((IntPtr)((void*)registryBytes), this.m_ByteOffset);
					}
				}
				this.m_ByteOffset += 4;
			}
			return num;
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x000A5530 File Offset: 0x000A4530
		protected override void BuildInternal()
		{
			bool flag = this.ReadRegSettings();
			if (flag)
			{
				flag = this.ReadInt32() >= 60;
			}
			if (!flag)
			{
				base.SetAutoDetectSettings(true);
				return;
			}
			this.ReadInt32();
			RegBlobWebProxyDataBuilder.ProxyTypeFlags proxyTypeFlags = (RegBlobWebProxyDataBuilder.ProxyTypeFlags)this.ReadInt32();
			string text = this.ReadString();
			string text2 = this.ReadString();
			if ((proxyTypeFlags & RegBlobWebProxyDataBuilder.ProxyTypeFlags.PROXY_TYPE_PROXY) != (RegBlobWebProxyDataBuilder.ProxyTypeFlags)0)
			{
				base.SetProxyAndBypassList(text, text2);
			}
			base.SetAutoDetectSettings((proxyTypeFlags & RegBlobWebProxyDataBuilder.ProxyTypeFlags.PROXY_TYPE_AUTO_DETECT) != (RegBlobWebProxyDataBuilder.ProxyTypeFlags)0);
			string text3 = this.ReadString();
			if ((proxyTypeFlags & RegBlobWebProxyDataBuilder.ProxyTypeFlags.PROXY_TYPE_AUTO_PROXY_URL) != (RegBlobWebProxyDataBuilder.ProxyTypeFlags)0)
			{
				base.SetAutoProxyUrl(text3);
			}
		}

		// Token: 0x0400274F RID: 10063
		internal const string PolicyKey = "SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";

		// Token: 0x04002750 RID: 10064
		internal const string ProxyKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections";

		// Token: 0x04002751 RID: 10065
		private const string DefaultConnectionSettings = "DefaultConnectionSettings";

		// Token: 0x04002752 RID: 10066
		private const string ProxySettingsPerUser = "ProxySettingsPerUser";

		// Token: 0x04002753 RID: 10067
		private const int IE50StrucSize = 60;

		// Token: 0x04002754 RID: 10068
		private byte[] m_RegistryBytes;

		// Token: 0x04002755 RID: 10069
		private int m_ByteOffset;

		// Token: 0x04002756 RID: 10070
		private string m_Connectoid;

		// Token: 0x04002757 RID: 10071
		private SafeRegistryHandle m_Registry;

		// Token: 0x0200050C RID: 1292
		[Flags]
		private enum ProxyTypeFlags
		{
			// Token: 0x04002759 RID: 10073
			PROXY_TYPE_DIRECT = 1,
			// Token: 0x0400275A RID: 10074
			PROXY_TYPE_PROXY = 2,
			// Token: 0x0400275B RID: 10075
			PROXY_TYPE_AUTO_PROXY_URL = 4,
			// Token: 0x0400275C RID: 10076
			PROXY_TYPE_AUTO_DETECT = 8
		}
	}
}
