using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web.Services.Diagnostics;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000AC RID: 172
	internal class DynamicVirtualDiscoSearcher : DynamicDiscoSearcher
	{
		// Token: 0x06000494 RID: 1172 RVA: 0x00016DC8 File Offset: 0x00015DC8
		internal DynamicVirtualDiscoSearcher(string startDir, string[] excludedUrls, string rootUrl)
			: base(excludedUrls)
		{
			this.origUrl = rootUrl;
			this.entryPathPrefix = this.GetWebServerForUrl(rootUrl) + "/ROOT";
			this.startDir = startDir;
			string text = new Uri(rootUrl).LocalPath;
			if (text.Equals("/"))
			{
				text = "";
			}
			this.rootPathAsdi = this.entryPathPrefix + text;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00016E48 File Offset: 0x00015E48
		internal override void Search(string fileToSkipAtBegin)
		{
			this.SearchInit(fileToSkipAtBegin);
			base.ScanDirectory(this.rootPathAsdi);
			this.CleanupCache();
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00016E64 File Offset: 0x00015E64
		protected override void SearchSubDirectories(string nameAdsiDir)
		{
			bool traceVerbose = CompModSwitches.DynamicDiscoverySearcher.TraceVerbose;
			DirectoryEntry directoryEntry = (DirectoryEntry)this.Adsi[nameAdsiDir];
			if (directoryEntry == null)
			{
				if (!DirectoryEntry.Exists(nameAdsiDir))
				{
					return;
				}
				directoryEntry = new DirectoryEntry(nameAdsiDir);
				this.Adsi[nameAdsiDir] = directoryEntry;
			}
			foreach (object obj in directoryEntry.Children)
			{
				DirectoryEntry directoryEntry2 = (DirectoryEntry)obj;
				DirectoryEntry directoryEntry3 = (DirectoryEntry)this.Adsi[directoryEntry2.Path];
				if (directoryEntry3 == null)
				{
					directoryEntry3 = directoryEntry2;
					this.Adsi[directoryEntry2.Path] = directoryEntry2;
				}
				else
				{
					directoryEntry2.Dispose();
				}
				DynamicVirtualDiscoSearcher.AppSettings appSettings = this.GetAppSettings(directoryEntry3);
				if (appSettings != null)
				{
					base.ScanDirectory(directoryEntry3.Path);
				}
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00016F48 File Offset: 0x00015F48
		protected override DirectoryInfo GetPhysicalDir(string dir)
		{
			DirectoryEntry directoryEntry = (DirectoryEntry)this.Adsi[dir];
			if (directoryEntry == null)
			{
				if (!DirectoryEntry.Exists(dir))
				{
					return null;
				}
				directoryEntry = new DirectoryEntry(dir);
				this.Adsi[dir] = directoryEntry;
			}
			try
			{
				DynamicVirtualDiscoSearcher.AppSettings appSettings = this.GetAppSettings(directoryEntry);
				if (appSettings == null)
				{
					return null;
				}
				DirectoryInfo directoryInfo;
				if (appSettings.VPath == null)
				{
					if (!dir.StartsWith(this.rootPathAsdi, StringComparison.Ordinal))
					{
						throw new ArgumentException(Res.GetString("WebVirtualDisoRoot", new object[] { dir, this.rootPathAsdi }), "dir");
					}
					string text = dir.Substring(this.rootPathAsdi.Length);
					text = text.Replace('/', '\\');
					directoryInfo = new DirectoryInfo(this.startDir + text);
				}
				else
				{
					directoryInfo = new DirectoryInfo(appSettings.VPath);
				}
				if (directoryInfo.Exists)
				{
					return directoryInfo;
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				bool traceVerbose = CompModSwitches.DynamicDiscoverySearcher.TraceVerbose;
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, this, "GetPhysicalDir", ex);
				}
				return null;
			}
			catch
			{
				bool traceVerbose2 = CompModSwitches.DynamicDiscoverySearcher.TraceVerbose;
				return null;
			}
			return null;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000170A4 File Offset: 0x000160A4
		private string GetWebServerForUrl(string url)
		{
			Uri uri = new Uri(url);
			DirectoryEntry directoryEntry = new DirectoryEntry("IIS://" + uri.Host + "/W3SVC");
			foreach (object obj in directoryEntry.Children)
			{
				DirectoryEntry directoryEntry2 = (DirectoryEntry)obj;
				DirectoryEntry directoryEntry3 = (DirectoryEntry)this.Adsi[directoryEntry2.Path];
				if (directoryEntry3 == null)
				{
					directoryEntry3 = directoryEntry2;
					this.Adsi[directoryEntry2.Path] = directoryEntry2;
				}
				else
				{
					directoryEntry2.Dispose();
				}
				DynamicVirtualDiscoSearcher.AppSettings appSettings = this.GetAppSettings(directoryEntry3);
				if (appSettings != null && appSettings.Bindings != null)
				{
					foreach (string text in appSettings.Bindings)
					{
						bool traceVerbose = CompModSwitches.DynamicDiscoverySearcher.TraceVerbose;
						string[] array = text.Split(new char[] { ':' });
						string text2 = array[0];
						string text3 = array[1];
						string text4 = array[2];
						if (Convert.ToInt32(text3, CultureInfo.InvariantCulture) == uri.Port)
						{
							if (uri.HostNameType == UriHostNameType.Dns)
							{
								if (text4.Length == 0 || string.Compare(text4, uri.Host, StringComparison.OrdinalIgnoreCase) == 0)
								{
									return directoryEntry3.Path;
								}
							}
							else if (text2.Length == 0 || string.Compare(text2, uri.Host, StringComparison.OrdinalIgnoreCase) == 0)
							{
								return directoryEntry3.Path;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001724C File Offset: 0x0001624C
		protected override string MakeResultPath(string dirName, string fileName)
		{
			return string.Concat(new object[]
			{
				this.origUrl,
				dirName.Substring(this.rootPathAsdi.Length, dirName.Length - this.rootPathAsdi.Length),
				'/',
				fileName
			});
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x000172A5 File Offset: 0x000162A5
		protected override string MakeAbsExcludedPath(string pathRelativ)
		{
			return this.rootPathAsdi + '/' + pathRelativ.Replace('\\', '/');
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x000172C3 File Offset: 0x000162C3
		protected override bool IsVirtualSearch
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000172C8 File Offset: 0x000162C8
		private DynamicVirtualDiscoSearcher.AppSettings GetAppSettings(DirectoryEntry entry)
		{
			string path = entry.Path;
			DynamicVirtualDiscoSearcher.AppSettings appSettings = null;
			object obj = this.webApps[path];
			if (obj == null)
			{
				lock (this.webApps)
				{
					if (this.webApps[path] == null)
					{
						appSettings = new DynamicVirtualDiscoSearcher.AppSettings(entry);
						this.webApps[path] = appSettings;
					}
					goto IL_005A;
				}
			}
			appSettings = (DynamicVirtualDiscoSearcher.AppSettings)obj;
			IL_005A:
			if (!appSettings.AccessRead)
			{
				return null;
			}
			return appSettings;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001734C File Offset: 0x0001634C
		private void CleanupCache()
		{
			foreach (object obj in this.Adsi)
			{
				((DirectoryEntry)((DictionaryEntry)obj).Value).Dispose();
			}
			this.rootPathAsdi = null;
			this.entryPathPrefix = null;
			this.startDir = null;
			this.Adsi = null;
			this.webApps = null;
		}

		// Token: 0x040003C8 RID: 968
		private string rootPathAsdi;

		// Token: 0x040003C9 RID: 969
		private string entryPathPrefix;

		// Token: 0x040003CA RID: 970
		private string startDir;

		// Token: 0x040003CB RID: 971
		private Hashtable webApps = new Hashtable();

		// Token: 0x040003CC RID: 972
		private Hashtable Adsi = new Hashtable();

		// Token: 0x020000AD RID: 173
		private class AppSettings
		{
			// Token: 0x0600049E RID: 1182 RVA: 0x000173D4 File Offset: 0x000163D4
			internal AppSettings(DirectoryEntry entry)
			{
				string schemaClassName = entry.SchemaClassName;
				this.AccessRead = true;
				if (schemaClassName == "IIsWebVirtualDir" || schemaClassName == "IIsWebDirectory")
				{
					if (!(bool)entry.Properties["AccessRead"][0])
					{
						this.AccessRead = false;
						return;
					}
					if (schemaClassName == "IIsWebVirtualDir")
					{
						this.VPath = (string)entry.Properties["Path"][0];
						return;
					}
				}
				else
				{
					if (schemaClassName == "IIsWebServer")
					{
						this.Bindings = new string[entry.Properties["ServerBindings"].Count];
						for (int i = 0; i < this.Bindings.Length; i++)
						{
							this.Bindings[i] = (string)entry.Properties["ServerBindings"][i];
						}
						return;
					}
					this.AccessRead = false;
				}
			}

			// Token: 0x040003CD RID: 973
			internal readonly bool AccessRead;

			// Token: 0x040003CE RID: 974
			internal readonly string[] Bindings;

			// Token: 0x040003CF RID: 975
			internal readonly string VPath;
		}
	}
}
