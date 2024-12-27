using System;
using System.Configuration;
using System.Configuration.Internal;
using System.Threading;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000014 RID: 20
	internal class CachedPathData
	{
		// Token: 0x06000031 RID: 49 RVA: 0x0000273C File Offset: 0x0000173C
		internal CachedPathData(string configPath, VirtualPath virtualPath, string physicalPath, bool exists)
		{
			this._runtimeConfig = RuntimeConfig.GetErrorRuntimeConfig();
			this._configPath = configPath;
			this._virtualPath = virtualPath;
			this._physicalPath = physicalPath;
			this._flags[4] = exists;
			string schemeDelimiter = Uri.SchemeDelimiter;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002778 File Offset: 0x00001778
		internal static CachedPathData GetMachinePathData()
		{
			return CachedPathData.GetConfigPathData("machine");
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002784 File Offset: 0x00001784
		internal static CachedPathData GetRootWebPathData()
		{
			return CachedPathData.GetConfigPathData("machine/webroot");
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002790 File Offset: 0x00001790
		internal static CachedPathData GetApplicationPathData()
		{
			if (!HostingEnvironment.IsHosted)
			{
				return CachedPathData.GetRootWebPathData();
			}
			return CachedPathData.GetConfigPathData(HostingEnvironment.AppConfigPath);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000027AC File Offset: 0x000017AC
		internal static CachedPathData GetVirtualPathData(VirtualPath virtualPath, bool permitPathsOutsideApp)
		{
			if (!HostingEnvironment.IsHosted)
			{
				return CachedPathData.GetRootWebPathData();
			}
			if (virtualPath != null)
			{
				virtualPath.FailIfRelativePath();
			}
			if (!(virtualPath == null) && virtualPath.IsWithinAppRoot)
			{
				string configPathFromSiteIDAndVPath = WebConfigurationHost.GetConfigPathFromSiteIDAndVPath(HostingEnvironment.SiteID, virtualPath);
				return CachedPathData.GetConfigPathData(configPathFromSiteIDAndVPath);
			}
			if (permitPathsOutsideApp)
			{
				return CachedPathData.GetApplicationPathData();
			}
			throw new ArgumentException(SR.GetString("Cross_app_not_allowed", new object[] { (virtualPath != null) ? virtualPath.VirtualPathString : "null" }));
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002834 File Offset: 0x00001834
		private static CachedPathData GetConfigPathData(string configPath)
		{
			string text = CachedPathData.CreateKey(configPath);
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			CachedPathData cachedPathData = (CachedPathData)cacheInternal.Get(text);
			if (cachedPathData != null)
			{
				cachedPathData.WaitForInit();
				return cachedPathData;
			}
			CachedPathData cachedPathData2 = null;
			CacheDependency cacheDependency = null;
			VirtualPath virtualPath = null;
			string text2 = null;
			bool flag = false;
			string[] array = null;
			string[] array2 = null;
			string text3 = null;
			bool flag2 = false;
			if (WebConfigurationHost.IsMachineConfigPath(configPath))
			{
				flag = true;
				flag2 = true;
			}
			else
			{
				string parent = ConfigPathUtility.GetParent(configPath);
				cachedPathData2 = CachedPathData.GetConfigPathData(parent);
				string text4 = CachedPathData.CreateKey(parent);
				array2 = new string[] { text4 };
				if (!WebConfigurationHost.IsVirtualPathConfigPath(configPath))
				{
					flag = true;
					flag2 = true;
				}
				else
				{
					WebConfigurationHost.GetSiteIDAndVPathFromConfigPath(configPath, out text3, out virtualPath);
					try
					{
						text2 = virtualPath.MapPathInternal(true);
					}
					catch (HttpException ex)
					{
						if (ex.GetHttpCode() == 500)
						{
							throw new HttpException(404, string.Empty);
						}
						throw;
					}
					FileUtil.CheckSuspiciousPhysicalPath(text2);
					bool flag3 = false;
					if (string.IsNullOrEmpty(text2))
					{
						flag = false;
					}
					else
					{
						FileUtil.PhysicalPathStatus(text2, false, false, out flag, out flag3);
					}
					if (flag && !flag3)
					{
						array = new string[] { text2 };
					}
				}
				try
				{
					cacheDependency = new CacheDependency(0, array, array2);
				}
				catch
				{
				}
			}
			CachedPathData cachedPathData3 = null;
			bool flag4 = false;
			bool flag5 = false;
			CacheItemPriority cacheItemPriority = (flag2 ? CacheItemPriority.NotRemovable : CacheItemPriority.Normal);
			try
			{
				using (cacheDependency)
				{
					cachedPathData3 = new CachedPathData(configPath, virtualPath, text2, flag);
					try
					{
					}
					finally
					{
						cachedPathData = (CachedPathData)cacheInternal.UtcAdd(text, cachedPathData3, cacheDependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, cacheItemPriority, CachedPathData.s_callback);
						if (cachedPathData == null)
						{
							flag4 = true;
						}
					}
				}
				if (!flag4)
				{
					cachedPathData.WaitForInit();
					return cachedPathData;
				}
				lock (cachedPathData3)
				{
					try
					{
						cachedPathData3.Init(cachedPathData2);
						flag5 = true;
					}
					finally
					{
						cachedPathData3._flags[1] = true;
						Monitor.PulseAll(cachedPathData3);
						if (cachedPathData3._flags[64])
						{
							cachedPathData3.Close();
						}
					}
				}
			}
			finally
			{
				if (flag4)
				{
					if (!cachedPathData3._flags[1])
					{
						lock (cachedPathData3)
						{
							cachedPathData3._flags[1] = true;
							Monitor.PulseAll(cachedPathData3);
							if (cachedPathData3._flags[64])
							{
								cachedPathData3.Close();
							}
						}
					}
					if (!flag5 || (cachedPathData3.ConfigRecord != null && cachedPathData3.ConfigRecord.HasInitErrors))
					{
						if (cacheDependency != null)
						{
							if (!flag5)
							{
								cacheDependency = new CacheDependency(0, null, array2);
							}
							else
							{
								cacheDependency = new CacheDependency(0, array, array2);
							}
						}
						using (cacheDependency)
						{
							cacheInternal.UtcInsert(text, cachedPathData3, cacheDependency, DateTime.UtcNow.AddSeconds(5.0), Cache.NoSlidingExpiration, CacheItemPriority.Normal, CachedPathData.s_callback);
						}
					}
				}
			}
			return cachedPathData3;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002B60 File Offset: 0x00001B60
		internal static void RemoveBadPathData(CachedPathData pathData)
		{
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			string text = pathData._configPath;
			string text2 = CachedPathData.CreateKey(text);
			while (pathData != null && !pathData.CompletedFirstRequest && !pathData.Exists)
			{
				cacheInternal.Remove(text2);
				text = ConfigPathUtility.GetParent(text);
				if (text == null)
				{
					return;
				}
				text2 = CachedPathData.CreateKey(text);
				pathData = (CachedPathData)cacheInternal.Get(text2);
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002BC0 File Offset: 0x00001BC0
		internal static void MarkCompleted(CachedPathData pathData)
		{
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			string text = pathData._configPath;
			for (;;)
			{
				pathData.CompletedFirstRequest = true;
				text = ConfigPathUtility.GetParent(text);
				if (text == null)
				{
					break;
				}
				string text2 = CachedPathData.CreateKey(text);
				pathData = (CachedPathData)cacheInternal.Get(text2);
				if (pathData == null || pathData.CompletedFirstRequest)
				{
					return;
				}
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002C0C File Offset: 0x00001C0C
		private void Close()
		{
			if (this._flags[1] && this._flags.ChangeValue(32, true) && this._flags[16])
			{
				this.ConfigRecord.Remove();
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002C48 File Offset: 0x00001C48
		private static void OnCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
		{
			CachedPathData cachedPathData = (CachedPathData)value;
			cachedPathData._flags[64] = true;
			cachedPathData.Close();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002C70 File Offset: 0x00001C70
		private static string CreateKey(string configPath)
		{
			return "d" + configPath;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002C80 File Offset: 0x00001C80
		private void Init(CachedPathData parentData)
		{
			if (!HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				this._runtimeConfig = null;
				return;
			}
			IInternalConfigRecord uniqueConfigRecord = HttpConfigurationSystem.GetUniqueConfigRecord(this._configPath);
			if (uniqueConfigRecord.ConfigPath.Length == this._configPath.Length)
			{
				this._flags[16] = true;
				this._runtimeConfig = new RuntimeConfig(uniqueConfigRecord);
				return;
			}
			this._runtimeConfig = parentData._runtimeConfig;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002CE8 File Offset: 0x00001CE8
		private void WaitForInit()
		{
			if (!this._flags[1])
			{
				lock (this)
				{
					if (!this._flags[1])
					{
						Monitor.Wait(this);
					}
				}
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002D3C File Offset: 0x00001D3C
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002D4A File Offset: 0x00001D4A
		internal bool CompletedFirstRequest
		{
			get
			{
				return this._flags[2];
			}
			set
			{
				this._flags[2] = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002D59 File Offset: 0x00001D59
		internal VirtualPath Path
		{
			get
			{
				return this._virtualPath;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002D61 File Offset: 0x00001D61
		internal string PhysicalPath
		{
			get
			{
				return this._physicalPath;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002D69 File Offset: 0x00001D69
		internal bool Exists
		{
			get
			{
				return this._flags[4];
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002D77 File Offset: 0x00001D77
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002D7F File Offset: 0x00001D7F
		internal HandlerMappingMemo CachedHandler
		{
			get
			{
				return this._handlerMemo;
			}
			set
			{
				this._handlerMemo = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002D88 File Offset: 0x00001D88
		internal IInternalConfigRecord ConfigRecord
		{
			get
			{
				if (this._runtimeConfig == null)
				{
					return null;
				}
				return this._runtimeConfig.ConfigRecord;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002D9F File Offset: 0x00001D9F
		internal RuntimeConfig RuntimeConfig
		{
			get
			{
				return this._runtimeConfig;
			}
		}

		// Token: 0x04000D02 RID: 3330
		internal const int FInited = 1;

		// Token: 0x04000D03 RID: 3331
		internal const int FCompletedFirstRequest = 2;

		// Token: 0x04000D04 RID: 3332
		internal const int FExists = 4;

		// Token: 0x04000D05 RID: 3333
		internal const int FOwnsConfigRecord = 16;

		// Token: 0x04000D06 RID: 3334
		internal const int FClosed = 32;

		// Token: 0x04000D07 RID: 3335
		internal const int FCloseNeeded = 64;

		// Token: 0x04000D08 RID: 3336
		private static CacheItemRemovedCallback s_callback = new CacheItemRemovedCallback(CachedPathData.OnCacheItemRemoved);

		// Token: 0x04000D09 RID: 3337
		private SafeBitVector32 _flags;

		// Token: 0x04000D0A RID: 3338
		private string _configPath;

		// Token: 0x04000D0B RID: 3339
		private VirtualPath _virtualPath;

		// Token: 0x04000D0C RID: 3340
		private string _physicalPath;

		// Token: 0x04000D0D RID: 3341
		private RuntimeConfig _runtimeConfig;

		// Token: 0x04000D0E RID: 3342
		private HandlerMappingMemo _handlerMemo;
	}
}
