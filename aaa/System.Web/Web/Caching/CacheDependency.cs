using System;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.Util;

namespace System.Web.Caching
{
	// Token: 0x02000104 RID: 260
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CacheDependency : IDisposable
	{
		// Token: 0x06000C17 RID: 3095 RVA: 0x0003005D File Offset: 0x0002F05D
		private CacheDependency(int bogus)
		{
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00030065 File Offset: 0x0002F065
		protected CacheDependency()
		{
			this.Init(true, null, null, null, DateTime.MaxValue);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0003007C File Offset: 0x0002F07C
		public CacheDependency(string filename)
			: this(filename, DateTime.MaxValue)
		{
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0003008C File Offset: 0x0002F08C
		public CacheDependency(string filename, DateTime start)
		{
			if (filename == null)
			{
				return;
			}
			DateTime dateTime = DateTimeUtil.ConvertToUniversalTime(start);
			string[] array = new string[] { filename };
			this.Init(true, array, null, null, dateTime);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x000300C2 File Offset: 0x0002F0C2
		public CacheDependency(string[] filenames)
		{
			this.Init(true, filenames, null, null, DateTime.MaxValue);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x000300DC File Offset: 0x0002F0DC
		public CacheDependency(string[] filenames, DateTime start)
		{
			DateTime dateTime = DateTimeUtil.ConvertToUniversalTime(start);
			this.Init(true, filenames, null, null, dateTime);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00030101 File Offset: 0x0002F101
		public CacheDependency(string[] filenames, string[] cachekeys)
		{
			this.Init(true, filenames, cachekeys, null, DateTime.MaxValue);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00030118 File Offset: 0x0002F118
		public CacheDependency(string[] filenames, string[] cachekeys, DateTime start)
		{
			DateTime dateTime = DateTimeUtil.ConvertToUniversalTime(start);
			this.Init(true, filenames, cachekeys, null, dateTime);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0003013D File Offset: 0x0002F13D
		public CacheDependency(string[] filenames, string[] cachekeys, CacheDependency dependency)
		{
			this.Init(true, filenames, cachekeys, dependency, DateTime.MaxValue);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00030154 File Offset: 0x0002F154
		public CacheDependency(string[] filenames, string[] cachekeys, CacheDependency dependency, DateTime start)
		{
			DateTime dateTime = DateTimeUtil.ConvertToUniversalTime(start);
			this.Init(true, filenames, cachekeys, dependency, dateTime);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0003017A File Offset: 0x0002F17A
		internal CacheDependency(int dummy, string filename)
			: this(dummy, filename, DateTime.MaxValue)
		{
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0003018C File Offset: 0x0002F18C
		internal CacheDependency(int dummy, string filename, DateTime utcStart)
		{
			if (filename == null)
			{
				return;
			}
			string[] array = new string[] { filename };
			this.Init(false, array, null, null, utcStart);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000301BB File Offset: 0x0002F1BB
		internal CacheDependency(int dummy, string[] filenames)
		{
			this.Init(false, filenames, null, null, DateTime.MaxValue);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x000301D2 File Offset: 0x0002F1D2
		internal CacheDependency(int dummy, string[] filenames, DateTime utcStart)
		{
			this.Init(false, filenames, null, null, utcStart);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x000301E5 File Offset: 0x0002F1E5
		internal CacheDependency(int dummy, string[] filenames, string[] cachekeys)
		{
			this.Init(false, filenames, cachekeys, null, DateTime.MaxValue);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x000301FC File Offset: 0x0002F1FC
		internal CacheDependency(int dummy, string[] filenames, string[] cachekeys, DateTime utcStart)
		{
			this.Init(false, filenames, cachekeys, null, utcStart);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00030210 File Offset: 0x0002F210
		internal CacheDependency(int dummy, string[] filenames, string[] cachekeys, CacheDependency dependency)
		{
			this.Init(false, filenames, cachekeys, dependency, DateTime.MaxValue);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00030228 File Offset: 0x0002F228
		internal CacheDependency(int dummy, string[] filenames, string[] cachekeys, CacheDependency dependency, DateTime utcStart)
		{
			this.Init(false, filenames, cachekeys, dependency, utcStart);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00030240 File Offset: 0x0002F240
		private void Init(bool isPublic, string[] filenamesArg, string[] cachekeysArg, CacheDependency dependency, DateTime utcStart)
		{
			CacheDependency.DepFileInfo[] array = CacheDependency.s_depFileInfosEmpty;
			CacheEntry[] array2 = CacheDependency.s_entriesEmpty;
			this._bits = new SafeBitVector32(0);
			string[] array3;
			if (filenamesArg != null)
			{
				array3 = (string[])filenamesArg.Clone();
			}
			else
			{
				array3 = null;
			}
			string[] array4;
			if (cachekeysArg != null)
			{
				array4 = (string[])cachekeysArg.Clone();
			}
			else
			{
				array4 = null;
			}
			this._utcLastModified = DateTime.MinValue;
			try
			{
				if (array3 == null)
				{
					array3 = CacheDependency.s_stringsEmpty;
				}
				else
				{
					foreach (string text in array3)
					{
						if (text == null)
						{
							throw new ArgumentNullException("filenamesArg");
						}
						if (isPublic)
						{
							InternalSecurityPermissions.PathDiscovery(text).Demand();
						}
					}
				}
				if (array4 == null)
				{
					array4 = CacheDependency.s_stringsEmpty;
				}
				else
				{
					string[] array6 = array4;
					for (int j = 0; j < array6.Length; j++)
					{
						if (array6[j] == null)
						{
							throw new ArgumentNullException("cachekeysArg");
						}
					}
				}
				if (dependency == null)
				{
					dependency = CacheDependency.s_dependencyEmpty;
				}
				else
				{
					if (dependency.GetType() != CacheDependency.s_dependencyEmpty.GetType())
					{
						throw new ArgumentException(SR.GetString("Invalid_Dependency_Type"));
					}
					object depFileInfos = dependency._depFileInfos;
					object entries = dependency._entries;
					DateTime utcLastModified = dependency._utcLastModified;
					if (dependency._bits[4])
					{
						this._bits[4] = true;
						this.DisposeInternal();
						return;
					}
					if (depFileInfos != null)
					{
						if (depFileInfos is CacheDependency.DepFileInfo)
						{
							array = new CacheDependency.DepFileInfo[] { (CacheDependency.DepFileInfo)depFileInfos };
						}
						else
						{
							array = (CacheDependency.DepFileInfo[])depFileInfos;
						}
						foreach (CacheDependency.DepFileInfo depFileInfo in array)
						{
							string filename = depFileInfo._filename;
							if (filename == null)
							{
								this._bits[4] = true;
								this.DisposeInternal();
								return;
							}
							if (isPublic)
							{
								InternalSecurityPermissions.PathDiscovery(filename).Demand();
							}
						}
					}
					if (entries != null)
					{
						if (entries is CacheEntry)
						{
							array2 = new CacheEntry[] { (CacheEntry)entries };
						}
						else
						{
							array2 = (CacheEntry[])entries;
							CacheEntry[] array8 = array2;
							for (int l = 0; l < array8.Length; l++)
							{
								if (array8[l] == null)
								{
									this._bits[4] = true;
									this.DisposeInternal();
									return;
								}
							}
						}
					}
					this._utcLastModified = utcLastModified;
				}
				int num = array.Length + array3.Length;
				if (num > 0)
				{
					CacheDependency.DepFileInfo[] array9 = new CacheDependency.DepFileInfo[num];
					FileChangeEventHandler fileChangeEventHandler = new FileChangeEventHandler(this.FileChange);
					FileChangesMonitor fileChangesMonitor = HttpRuntime.FileChangesMonitor;
					int m;
					for (m = 0; m < num; m++)
					{
						array9[m] = new CacheDependency.DepFileInfo();
					}
					m = 0;
					foreach (CacheDependency.DepFileInfo depFileInfo2 in array)
					{
						string filename2 = depFileInfo2._filename;
						fileChangesMonitor.StartMonitoringPath(filename2, fileChangeEventHandler, out array9[m]._fad);
						array9[m]._filename = filename2;
						m++;
					}
					DateTime dateTime = DateTime.MinValue;
					foreach (string text2 in array3)
					{
						DateTime dateTime2 = fileChangesMonitor.StartMonitoringPath(text2, fileChangeEventHandler, out array9[m]._fad);
						array9[m]._filename = text2;
						m++;
						if (dateTime2 > this._utcLastModified)
						{
							this._utcLastModified = dateTime2;
						}
						if (utcStart < DateTime.MaxValue)
						{
							if (dateTime == DateTime.MinValue)
							{
								dateTime = DateTime.UtcNow;
							}
							if (dateTime2 >= utcStart && !(dateTime2 - dateTime > CacheDependency.FUTURE_FILETIME_BUFFER))
							{
								this._bits[4] = true;
								break;
							}
						}
					}
					if (array9.Length == 1)
					{
						this._depFileInfos = array9[0];
					}
					else
					{
						this._depFileInfos = array9;
					}
				}
				int num3 = array2.Length + array4.Length;
				if (num3 > 0 && !this._bits[4])
				{
					CacheEntry[] array12 = new CacheEntry[num3];
					int num4 = 0;
					foreach (CacheEntry cacheEntry in array2)
					{
						cacheEntry.AddCacheDependencyNotify(this);
						array12[num4++] = cacheEntry;
					}
					CacheInternal cacheInternal = HttpRuntime.CacheInternal;
					foreach (string text3 in array4)
					{
						CacheEntry cacheEntry2 = (CacheEntry)cacheInternal.DoGet(isPublic, text3, CacheGetOptions.ReturnCacheEntry);
						if (cacheEntry2 == null)
						{
							this._bits[4] = true;
							break;
						}
						cacheEntry2.AddCacheDependencyNotify(this);
						array12[num4++] = cacheEntry2;
						if (cacheEntry2.UtcCreated > this._utcLastModified)
						{
							this._utcLastModified = cacheEntry2.UtcCreated;
						}
						if (cacheEntry2.State != CacheEntry.EntryState.AddedToCache || cacheEntry2.UtcCreated > utcStart)
						{
							this._bits[4] = true;
							break;
						}
					}
					if (array12.Length == 1)
					{
						this._entries = array12[0];
					}
					else
					{
						this._entries = array12;
					}
				}
				this._bits[1] = true;
				if (dependency._bits[4])
				{
					this._bits[4] = true;
				}
				if (this._bits[16] || this._bits[4])
				{
					this.DisposeInternal();
				}
			}
			catch
			{
				this._bits[1] = true;
				this.DisposeInternal();
				throw;
			}
			finally
			{
				this.InitUniqueID();
			}
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x000307B0 File Offset: 0x0002F7B0
		public void Dispose()
		{
			this._bits[32] = true;
			if (this.Use())
			{
				this.DisposeInternal();
			}
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x000307CE File Offset: 0x0002F7CE
		protected internal void FinishInit()
		{
			this._bits[32] = true;
			if (this._bits[16])
			{
				this.DisposeInternal();
			}
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000307F4 File Offset: 0x0002F7F4
		internal void DisposeInternal()
		{
			this._bits[16] = true;
			if (this._bits[32] && this._bits.ChangeValue(64, true))
			{
				this.DependencyDispose();
			}
			if (this._bits[1] && this._bits.ChangeValue(8, true))
			{
				this.DisposeOurself();
			}
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00030857 File Offset: 0x0002F857
		protected virtual void DependencyDispose()
		{
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0003085C File Offset: 0x0002F85C
		private void DisposeOurself()
		{
			object depFileInfos = this._depFileInfos;
			object entries = this._entries;
			this._objNotify = null;
			this._depFileInfos = null;
			this._entries = null;
			if (depFileInfos != null)
			{
				FileChangesMonitor fileChangesMonitor = HttpRuntime.FileChangesMonitor;
				CacheDependency.DepFileInfo depFileInfo = depFileInfos as CacheDependency.DepFileInfo;
				if (depFileInfo != null)
				{
					fileChangesMonitor.StopMonitoringPath(depFileInfo._filename, this);
				}
				else
				{
					CacheDependency.DepFileInfo[] array = (CacheDependency.DepFileInfo[])depFileInfos;
					foreach (CacheDependency.DepFileInfo depFileInfo2 in array)
					{
						string filename = depFileInfo2._filename;
						if (filename != null)
						{
							fileChangesMonitor.StopMonitoringPath(filename, this);
						}
					}
				}
			}
			if (entries != null)
			{
				CacheEntry cacheEntry = entries as CacheEntry;
				if (cacheEntry != null)
				{
					cacheEntry.RemoveCacheDependencyNotify(this);
					return;
				}
				CacheEntry[] array3 = (CacheEntry[])entries;
				foreach (CacheEntry cacheEntry2 in array3)
				{
					if (cacheEntry2 != null)
					{
						cacheEntry2.RemoveCacheDependencyNotify(this);
					}
				}
			}
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00030934 File Offset: 0x0002F934
		internal bool Use()
		{
			return this._bits.ChangeValue(2, true);
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00030943 File Offset: 0x0002F943
		public bool HasChanged
		{
			get
			{
				return this._bits[4];
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00030951 File Offset: 0x0002F951
		public DateTime UtcLastModified
		{
			get
			{
				return this._utcLastModified;
			}
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00030959 File Offset: 0x0002F959
		protected void SetUtcLastModified(DateTime utcLastModified)
		{
			this._utcLastModified = utcLastModified;
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00030962 File Offset: 0x0002F962
		internal void SetCacheDependencyChanged(ICacheDependencyChanged objNotify)
		{
			this._bits[32] = true;
			if (!this._bits[8])
			{
				this._objNotify = objNotify;
			}
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00030988 File Offset: 0x0002F988
		internal void AppendFileUniqueId(CacheDependency.DepFileInfo depFileInfo, StringBuilder sb)
		{
			FileAttributesData fileAttributesData = depFileInfo._fad;
			if (fileAttributesData == null)
			{
				fileAttributesData = FileAttributesData.NonExistantAttributesData;
			}
			sb.Append(depFileInfo._filename);
			sb.Append(fileAttributesData.UtcLastWriteTime.Ticks.ToString("d", NumberFormatInfo.InvariantInfo));
			sb.Append(fileAttributesData.FileSize.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000309F4 File Offset: 0x0002F9F4
		private void InitUniqueID()
		{
			StringBuilder stringBuilder = null;
			object depFileInfos = this._depFileInfos;
			if (depFileInfos != null)
			{
				CacheDependency.DepFileInfo depFileInfo = depFileInfos as CacheDependency.DepFileInfo;
				if (depFileInfo != null)
				{
					stringBuilder = new StringBuilder();
					this.AppendFileUniqueId(depFileInfo, stringBuilder);
				}
				else
				{
					CacheDependency.DepFileInfo[] array = (CacheDependency.DepFileInfo[])depFileInfos;
					foreach (CacheDependency.DepFileInfo depFileInfo2 in array)
					{
						if (depFileInfo2._filename != null)
						{
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder();
							}
							this.AppendFileUniqueId(depFileInfo2, stringBuilder);
						}
					}
				}
			}
			object entries = this._entries;
			if (entries != null)
			{
				CacheEntry cacheEntry = entries as CacheEntry;
				if (cacheEntry != null)
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder();
					}
					stringBuilder.Append(cacheEntry.Key);
					stringBuilder.Append(cacheEntry.UtcCreated.Ticks.ToString(CultureInfo.InvariantCulture));
				}
				else
				{
					CacheEntry[] array3 = (CacheEntry[])entries;
					foreach (CacheEntry cacheEntry2 in array3)
					{
						if (cacheEntry2 != null)
						{
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder();
							}
							stringBuilder.Append(cacheEntry2.Key);
							stringBuilder.Append(cacheEntry2.UtcCreated.Ticks.ToString(CultureInfo.InvariantCulture));
						}
					}
				}
			}
			if (stringBuilder != null)
			{
				this._uniqueID = stringBuilder.ToString();
			}
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00030B34 File Offset: 0x0002FB34
		public virtual string GetUniqueID()
		{
			return this._uniqueID;
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x00030B3C File Offset: 0x0002FB3C
		internal CacheEntry[] CacheEntries
		{
			get
			{
				if (this._entries == null)
				{
					return null;
				}
				CacheEntry cacheEntry = this._entries as CacheEntry;
				if (cacheEntry != null)
				{
					return new CacheEntry[] { cacheEntry };
				}
				return (CacheEntry[])this._entries;
			}
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00030B7C File Offset: 0x0002FB7C
		protected void NotifyDependencyChanged(object sender, EventArgs e)
		{
			if (this._bits.ChangeValue(4, true))
			{
				this._utcLastModified = DateTime.UtcNow;
				ICacheDependencyChanged objNotify = this._objNotify;
				if (objNotify != null && !this._bits[8])
				{
					objNotify.DependencyChanged(sender, e);
				}
				this.DisposeInternal();
			}
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00030BC9 File Offset: 0x0002FBC9
		internal void ItemRemoved()
		{
			this.NotifyDependencyChanged(this, EventArgs.Empty);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00030BD7 File Offset: 0x0002FBD7
		private void FileChange(object sender, FileChangeEvent e)
		{
			this.NotifyDependencyChanged(sender, e);
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00030BE4 File Offset: 0x0002FBE4
		internal virtual bool IsFileDependency()
		{
			object entries = this._entries;
			if (entries != null)
			{
				CacheEntry cacheEntry = entries as CacheEntry;
				if (cacheEntry != null)
				{
					return false;
				}
				CacheEntry[] array = (CacheEntry[])entries;
				if (array != null && array.Length > 0)
				{
					return false;
				}
			}
			object depFileInfos = this._depFileInfos;
			if (depFileInfos != null)
			{
				CacheDependency.DepFileInfo depFileInfo = depFileInfos as CacheDependency.DepFileInfo;
				if (depFileInfo != null)
				{
					return true;
				}
				CacheDependency.DepFileInfo[] array2 = (CacheDependency.DepFileInfo[])depFileInfos;
				if (array2 != null && array2.Length > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00030C48 File Offset: 0x0002FC48
		internal virtual string[] GetFileDependencies()
		{
			object depFileInfos = this._depFileInfos;
			if (depFileInfos == null)
			{
				return null;
			}
			CacheDependency.DepFileInfo depFileInfo = depFileInfos as CacheDependency.DepFileInfo;
			if (depFileInfo != null)
			{
				return new string[] { depFileInfo._filename };
			}
			CacheDependency.DepFileInfo[] array = (CacheDependency.DepFileInfo[])depFileInfos;
			string[] array2 = new string[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i]._filename;
			}
			return array2;
		}

		// Token: 0x040013F7 RID: 5111
		private const int BASE_INIT = 1;

		// Token: 0x040013F8 RID: 5112
		private const int USED = 2;

		// Token: 0x040013F9 RID: 5113
		private const int CHANGED = 4;

		// Token: 0x040013FA RID: 5114
		private const int BASE_DISPOSED = 8;

		// Token: 0x040013FB RID: 5115
		private const int WANTS_DISPOSE = 16;

		// Token: 0x040013FC RID: 5116
		private const int DERIVED_INIT = 32;

		// Token: 0x040013FD RID: 5117
		private const int DERIVED_DISPOSED = 64;

		// Token: 0x040013FE RID: 5118
		private string _uniqueID;

		// Token: 0x040013FF RID: 5119
		private object _depFileInfos;

		// Token: 0x04001400 RID: 5120
		private object _entries;

		// Token: 0x04001401 RID: 5121
		private ICacheDependencyChanged _objNotify;

		// Token: 0x04001402 RID: 5122
		private SafeBitVector32 _bits;

		// Token: 0x04001403 RID: 5123
		private DateTime _utcLastModified;

		// Token: 0x04001404 RID: 5124
		private static readonly string[] s_stringsEmpty = new string[0];

		// Token: 0x04001405 RID: 5125
		private static readonly CacheEntry[] s_entriesEmpty = new CacheEntry[0];

		// Token: 0x04001406 RID: 5126
		private static readonly CacheDependency s_dependencyEmpty = new CacheDependency(0);

		// Token: 0x04001407 RID: 5127
		private static readonly CacheDependency.DepFileInfo[] s_depFileInfosEmpty = new CacheDependency.DepFileInfo[0];

		// Token: 0x04001408 RID: 5128
		private static readonly TimeSpan FUTURE_FILETIME_BUFFER = new TimeSpan(0, 1, 0);

		// Token: 0x02000105 RID: 261
		internal class DepFileInfo
		{
			// Token: 0x04001409 RID: 5129
			internal string _filename;

			// Token: 0x0400140A RID: 5130
			internal FileAttributesData _fad;
		}
	}
}
