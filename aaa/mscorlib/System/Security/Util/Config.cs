using System;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace System.Security.Util
{
	// Token: 0x0200046E RID: 1134
	internal static class Config
	{
		// Token: 0x06002D77 RID: 11639 RVA: 0x00098CF8 File Offset: 0x00097CF8
		private static void GetFileLocales()
		{
			if (Config.m_machineConfig == null)
			{
				Config.m_machineConfig = Config._GetMachineDirectory();
			}
			if (Config.m_userConfig == null)
			{
				Config.m_userConfig = Config._GetUserDirectory();
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002D78 RID: 11640 RVA: 0x00098D1C File Offset: 0x00097D1C
		internal static string MachineDirectory
		{
			get
			{
				Config.GetFileLocales();
				return Config.m_machineConfig;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002D79 RID: 11641 RVA: 0x00098D28 File Offset: 0x00097D28
		internal static string UserDirectory
		{
			get
			{
				Config.GetFileLocales();
				return Config.m_userConfig;
			}
		}

		// Token: 0x06002D7A RID: 11642
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SaveDataByte(string path, byte[] data, int offset, int length);

		// Token: 0x06002D7B RID: 11643
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool RecoverData(ConfigId id);

		// Token: 0x06002D7C RID: 11644
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetQuickCache(ConfigId id, QuickCacheEntryType quickCacheFlags);

		// Token: 0x06002D7D RID: 11645
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetCacheEntry(ConfigId id, int numKey, char[] key, out byte[] data);

		// Token: 0x06002D7E RID: 11646
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddCacheEntry(ConfigId id, int numKey, char[] key, byte[] data);

		// Token: 0x06002D7F RID: 11647
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ResetCacheData(ConfigId id);

		// Token: 0x06002D80 RID: 11648
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string _GetMachineDirectory();

		// Token: 0x06002D81 RID: 11649
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string _GetUserDirectory();

		// Token: 0x06002D82 RID: 11650
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool WriteToEventLog(string message);

		// Token: 0x04001751 RID: 5969
		private static string m_machineConfig;

		// Token: 0x04001752 RID: 5970
		private static string m_userConfig;
	}
}
