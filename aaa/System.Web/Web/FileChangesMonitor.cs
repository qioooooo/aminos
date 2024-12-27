using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000037 RID: 55
	internal sealed class FileChangesMonitor
	{
		// Token: 0x06000128 RID: 296 RVA: 0x000068FC File Offset: 0x000058FC
		internal static HttpException CreateFileMonitoringException(int hr, string path)
		{
			bool flag = false;
			string text;
			switch (hr)
			{
			case -2147024894:
			case -2147024893:
				text = "Directory_does_not_exist_for_monitoring";
				goto IL_005C;
			case -2147024892:
				break;
			case -2147024891:
				text = "Access_denied_for_monitoring";
				flag = true;
				goto IL_005C;
			default:
				if (hr == -2147024840)
				{
					text = "NetBios_command_limit_reached";
					flag = true;
					goto IL_005C;
				}
				if (hr == -2147024809)
				{
					text = "Invalid_file_name_for_monitoring";
					goto IL_005C;
				}
				break;
			}
			text = "Failed_to_start_monitoring";
			IL_005C:
			if (flag)
			{
				UnsafeNativeMethods.RaiseFileMonitoringEventlogEvent(SR.GetString(text, new object[] { HttpRuntime.GetSafePath(path) }) + "\n\r" + SR.GetString("App_Virtual_Path", new object[] { HttpRuntime.AppDomainAppVirtualPath }), path, HttpRuntime.AppDomainAppVirtualPath, hr);
			}
			return new HttpException(SR.GetString(text, new object[] { HttpRuntime.GetSafePath(path) }), hr);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000069D4 File Offset: 0x000059D4
		internal static string GetFullPath(string alias)
		{
			try
			{
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, alias).Assert();
			}
			catch
			{
				throw FileChangesMonitor.CreateFileMonitoringException(-2147024809, alias);
			}
			string fullPath = Path.GetFullPath(alias);
			return FileUtil.RemoveTrailingDirectoryBackSlash(fullPath);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006A1C File Offset: 0x00005A1C
		private bool IsBeneathAppPathInternal(string fullPathName)
		{
			return this._appPathInternal != null && fullPathName.Length > this._appPathInternal.Length + 1 && fullPathName.IndexOf(this._appPathInternal, StringComparison.OrdinalIgnoreCase) > -1 && fullPathName[this._appPathInternal.Length] == Path.DirectorySeparatorChar;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00006A71 File Offset: 0x00005A71
		private bool IsFCNDisabled
		{
			get
			{
				return this._FCNMode == 1;
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006A7C File Offset: 0x00005A7C
		internal FileChangesMonitor()
		{
			UnsafeNativeMethods.GetDirMonConfiguration(out this._FCNMode);
			if (this.IsFCNDisabled)
			{
				return;
			}
			this._aliases = Hashtable.Synchronized(new Hashtable(StringComparer.OrdinalIgnoreCase));
			this._dirs = new Hashtable(StringComparer.OrdinalIgnoreCase);
			this._subDirDirMons = new Hashtable(StringComparer.OrdinalIgnoreCase);
			if (this._FCNMode == 2 && HttpRuntime.AppDomainAppPathInternal != null)
			{
				this._appPathInternal = FileChangesMonitor.GetFullPath(HttpRuntime.AppDomainAppPathInternal);
				this._dirMonAppPathInternal = new DirectoryMonitor(this._appPathInternal);
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006B0C File Offset: 0x00005B0C
		private DirectoryMonitor FindDirectoryMonitor(string dir, bool addIfNotFound, bool throwOnError)
		{
			FileAttributesData fileAttributesData = null;
			DirectoryMonitor directoryMonitor = (DirectoryMonitor)this._dirs[dir];
			if (directoryMonitor != null && !directoryMonitor.IsMonitoring() && (FileAttributesData.GetFileAttributes(dir, out fileAttributesData) != 0 || (fileAttributesData.FileAttributes & FileAttributes.Directory) == (FileAttributes)0))
			{
				directoryMonitor = null;
			}
			if (directoryMonitor != null || !addIfNotFound)
			{
				return directoryMonitor;
			}
			lock (this._dirs.SyncRoot)
			{
				directoryMonitor = (DirectoryMonitor)this._dirs[dir];
				if (directoryMonitor != null)
				{
					if (!directoryMonitor.IsMonitoring())
					{
						int num = FileAttributesData.GetFileAttributes(dir, out fileAttributesData);
						if (num == 0 && (fileAttributesData.FileAttributes & FileAttributes.Directory) == (FileAttributes)0)
						{
							num = -2147024809;
						}
						if (num != 0)
						{
							this._dirs.Remove(dir);
							directoryMonitor.StopMonitoring();
							if (addIfNotFound && throwOnError)
							{
								throw FileChangesMonitor.CreateFileMonitoringException(num, dir);
							}
							return null;
						}
					}
				}
				else if (addIfNotFound)
				{
					int num = FileAttributesData.GetFileAttributes(dir, out fileAttributesData);
					if (num == 0 && (fileAttributesData.FileAttributes & FileAttributes.Directory) == (FileAttributes)0)
					{
						num = -2147024809;
					}
					if (num == 0)
					{
						directoryMonitor = new DirectoryMonitor(dir, false, 347U);
						this._dirs.Add(dir, directoryMonitor);
					}
					else if (throwOnError)
					{
						throw FileChangesMonitor.CreateFileMonitoringException(num, dir);
					}
				}
			}
			return directoryMonitor;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006C38 File Offset: 0x00005C38
		internal void RemoveAliases(FileMonitor fileMon)
		{
			if (this.IsFCNDisabled)
			{
				return;
			}
			foreach (object obj in fileMon.Aliases)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (this._aliases[dictionaryEntry.Key] == fileMon)
				{
					this._aliases.Remove(dictionaryEntry.Key);
				}
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006CBC File Offset: 0x00005CBC
		internal DateTime StartMonitoringFile(string alias, FileChangeEventHandler callback)
		{
			bool flag = false;
			if (alias == null)
			{
				throw FileChangesMonitor.CreateFileMonitoringException(-2147024809, alias);
			}
			string text2;
			if (!this.IsFCNDisabled)
			{
				DateTime dateTime;
				using (new ApplicationImpersonationContext())
				{
					this._lockDispose.AcquireReaderLock();
					FileMonitor fileMonitor;
					string text;
					try
					{
						if (this._disposed)
						{
							return DateTime.MinValue;
						}
						fileMonitor = (FileMonitor)this._aliases[alias];
						DirectoryMonitor directoryMonitor;
						if (fileMonitor != null)
						{
							directoryMonitor = fileMonitor.DirectoryMonitor;
							text = fileMonitor.FileNameLong;
						}
						else
						{
							flag = true;
							if (alias.Length == 0 || !UrlPath.IsAbsolutePhysicalPath(alias))
							{
								throw FileChangesMonitor.CreateFileMonitoringException(-2147024809, alias);
							}
							text2 = FileChangesMonitor.GetFullPath(alias);
							if (this.IsBeneathAppPathInternal(text2))
							{
								directoryMonitor = this._dirMonAppPathInternal;
								text = text2.Substring(this._appPathInternal.Length + 1);
							}
							else
							{
								string directoryOrRootName = UrlPath.GetDirectoryOrRootName(text2);
								text = Path.GetFileName(text2);
								if (string.IsNullOrEmpty(text))
								{
									throw FileChangesMonitor.CreateFileMonitoringException(-2147024809, alias);
								}
								directoryMonitor = this.FindDirectoryMonitor(directoryOrRootName, true, true);
							}
						}
						fileMonitor = directoryMonitor.StartMonitoringFile(text, callback, alias);
						if (flag)
						{
							this._aliases[alias] = fileMonitor;
						}
					}
					finally
					{
						this._lockDispose.ReleaseReaderLock();
					}
					FileAttributesData fileAttributesData;
					fileMonitor.DirectoryMonitor.GetFileAttributes(text, out fileAttributesData);
					if (fileAttributesData != null)
					{
						dateTime = fileAttributesData.UtcLastWriteTime;
					}
					else
					{
						dateTime = DateTime.MinValue;
					}
				}
				return dateTime;
			}
			text2 = FileChangesMonitor.GetFullPath(alias);
			FindFileData findFileData = null;
			if (FindFileData.FindFile(text2, out findFileData) == 0)
			{
				return findFileData.FileAttributesData.UtcLastWriteTime;
			}
			return DateTime.MinValue;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006E6C File Offset: 0x00005E6C
		internal DateTime StartMonitoringPath(string alias, FileChangeEventHandler callback, out FileAttributesData fad)
		{
			FileMonitor fileMonitor = null;
			string text = null;
			bool flag = false;
			fad = null;
			if (alias == null)
			{
				throw new HttpException(SR.GetString("Invalid_file_name_for_monitoring", new object[] { string.Empty }));
			}
			string text2;
			if (!this.IsFCNDisabled)
			{
				DateTime dateTime;
				using (new ApplicationImpersonationContext())
				{
					this._lockDispose.AcquireReaderLock();
					try
					{
						if (this._disposed)
						{
							return DateTime.MinValue;
						}
						fileMonitor = (FileMonitor)this._aliases[alias];
						if (fileMonitor != null)
						{
							text = fileMonitor.FileNameLong;
							fileMonitor = fileMonitor.DirectoryMonitor.StartMonitoringFile(text, callback, alias);
						}
						else
						{
							flag = true;
							if (alias.Length == 0 || !UrlPath.IsAbsolutePhysicalPath(alias))
							{
								throw new HttpException(SR.GetString("Invalid_file_name_for_monitoring", new object[] { HttpRuntime.GetSafePath(alias) }));
							}
							text2 = FileChangesMonitor.GetFullPath(alias);
							if (this.IsBeneathAppPathInternal(text2))
							{
								DirectoryMonitor directoryMonitor = this._dirMonAppPathInternal;
								text = text2.Substring(this._appPathInternal.Length + 1);
								fileMonitor = directoryMonitor.StartMonitoringFile(text, callback, alias);
							}
							else
							{
								DirectoryMonitor directoryMonitor = this.FindDirectoryMonitor(text2, false, false);
								if (directoryMonitor != null)
								{
									fileMonitor = directoryMonitor.StartMonitoringFile(null, callback, alias);
								}
								else
								{
									string directoryOrRootName = UrlPath.GetDirectoryOrRootName(text2);
									text = Path.GetFileName(text2);
									if (!string.IsNullOrEmpty(text))
									{
										directoryMonitor = this.FindDirectoryMonitor(directoryOrRootName, false, false);
										if (directoryMonitor != null)
										{
											try
											{
												fileMonitor = directoryMonitor.StartMonitoringFile(text, callback, alias);
											}
											catch
											{
											}
											if (fileMonitor != null)
											{
												goto IL_01C7;
											}
										}
									}
									directoryMonitor = this.FindDirectoryMonitor(text2, true, false);
									if (directoryMonitor != null)
									{
										text = null;
									}
									else
									{
										if (string.IsNullOrEmpty(text))
										{
											throw FileChangesMonitor.CreateFileMonitoringException(-2147024809, alias);
										}
										directoryMonitor = this.FindDirectoryMonitor(directoryOrRootName, true, true);
									}
									fileMonitor = directoryMonitor.StartMonitoringFile(text, callback, alias);
								}
							}
						}
						IL_01C7:
						if (!fileMonitor.IsDirectory)
						{
							fileMonitor.DirectoryMonitor.GetFileAttributes(text, out fad);
						}
						if (flag)
						{
							this._aliases[alias] = fileMonitor;
						}
					}
					finally
					{
						this._lockDispose.ReleaseReaderLock();
					}
					if (fad != null)
					{
						dateTime = fad.UtcLastWriteTime;
					}
					else
					{
						dateTime = DateTime.MinValue;
					}
				}
				return dateTime;
			}
			text2 = FileChangesMonitor.GetFullPath(alias);
			FindFileData findFileData = null;
			if (FindFileData.FindFile(text2, out findFileData) == 0)
			{
				fad = findFileData.FileAttributesData;
				return findFileData.FileAttributesData.UtcLastWriteTime;
			}
			return DateTime.MinValue;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000070E8 File Offset: 0x000060E8
		internal void StartMonitoringDirectoryRenamesAndBinDirectory(string dir, FileChangeEventHandler callback)
		{
			if (string.IsNullOrEmpty(dir))
			{
				throw new HttpException(SR.GetString("Invalid_file_name_for_monitoring", new object[] { string.Empty }));
			}
			if (this.IsFCNDisabled)
			{
				return;
			}
			using (new ApplicationImpersonationContext())
			{
				this._lockDispose.AcquireReaderLock();
				try
				{
					if (!this._disposed)
					{
						this._callbackRenameOrCriticaldirChange = callback;
						string fullPath = FileChangesMonitor.GetFullPath(dir);
						this._dirMonSubdirs = new DirectoryMonitor(fullPath, true, 2U);
						try
						{
							this._dirMonSubdirs.StartMonitoringFile(null, new FileChangeEventHandler(this.OnSubdirChange), fullPath);
						}
						catch
						{
							((IDisposable)this._dirMonSubdirs).Dispose();
							this._dirMonSubdirs = null;
							throw;
						}
						this._dirMonSpecialDirs = new ArrayList();
						for (int i = 0; i < FileChangesMonitor.s_dirsToMonitor.Length; i++)
						{
							this._dirMonSpecialDirs.Add(this.ListenToSubdirectoryChanges(fullPath, FileChangesMonitor.s_dirsToMonitor[i]));
						}
					}
				}
				finally
				{
					this._lockDispose.ReleaseReaderLock();
				}
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000720C File Offset: 0x0000620C
		internal void StartListeningToLocalResourcesDirectory(VirtualPath virtualDir)
		{
			if (this.IsFCNDisabled)
			{
				return;
			}
			if (this._callbackRenameOrCriticaldirChange == null || this._dirMonSpecialDirs == null)
			{
				return;
			}
			using (new ApplicationImpersonationContext())
			{
				this._lockDispose.AcquireReaderLock();
				try
				{
					if (!this._disposed)
					{
						string text = virtualDir.MapPath();
						text = FileUtil.RemoveTrailingDirectoryBackSlash(text);
						string fileName = Path.GetFileName(text);
						text = Path.GetDirectoryName(text);
						if (Directory.Exists(text))
						{
							this._dirMonSpecialDirs.Add(this.ListenToSubdirectoryChanges(text, fileName));
						}
					}
				}
				finally
				{
					this._lockDispose.ReleaseReaderLock();
				}
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000072C0 File Offset: 0x000062C0
		private DirectoryMonitor ListenToSubdirectoryChanges(string dirRoot, string dirToListenTo)
		{
			string text;
			if (StringUtil.StringEndsWith(dirRoot, '\\'))
			{
				text = dirRoot + dirToListenTo;
			}
			else
			{
				text = dirRoot + "\\" + dirToListenTo;
			}
			DirectoryMonitor directoryMonitor;
			if (this.IsBeneathAppPathInternal(text))
			{
				directoryMonitor = this._dirMonAppPathInternal;
				dirToListenTo = text.Substring(this._appPathInternal.Length + 1);
				directoryMonitor.StartMonitoringFile(dirToListenTo, new FileChangeEventHandler(this.OnCriticaldirChange), text);
			}
			else
			{
				if (Directory.Exists(text))
				{
					directoryMonitor = new DirectoryMonitor(text, true, 345U);
					try
					{
						directoryMonitor.StartMonitoringFile(null, new FileChangeEventHandler(this.OnCriticaldirChange), text);
						return directoryMonitor;
					}
					catch
					{
						((IDisposable)directoryMonitor).Dispose();
						directoryMonitor = null;
						throw;
					}
				}
				directoryMonitor = (DirectoryMonitor)this._subDirDirMons[dirRoot];
				if (directoryMonitor == null)
				{
					directoryMonitor = new DirectoryMonitor(dirRoot, false, 347U);
					this._subDirDirMons[dirRoot] = directoryMonitor;
				}
				try
				{
					directoryMonitor.StartMonitoringFile(dirToListenTo, new FileChangeEventHandler(this.OnCriticaldirChange), text);
				}
				catch
				{
					((IDisposable)directoryMonitor).Dispose();
					directoryMonitor = null;
					throw;
				}
			}
			return directoryMonitor;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000073D4 File Offset: 0x000063D4
		private void OnSubdirChange(object sender, FileChangeEvent e)
		{
			try
			{
				Interlocked.Increment(ref this._activeCallbackCount);
				if (!this._disposed)
				{
					FileChangeEventHandler callbackRenameOrCriticaldirChange = this._callbackRenameOrCriticaldirChange;
					if (callbackRenameOrCriticaldirChange != null && (e.Action == FileAction.Error || e.Action == FileAction.Overwhelming || e.Action == FileAction.RenamedOldName || e.Action == FileAction.Removed))
					{
						HttpRuntime.SetShutdownMessage(SR.GetString("Directory_rename_notification", new object[] { e.FileName }));
						callbackRenameOrCriticaldirChange(this, e);
					}
				}
			}
			finally
			{
				Interlocked.Decrement(ref this._activeCallbackCount);
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000746C File Offset: 0x0000646C
		private void OnCriticaldirChange(object sender, FileChangeEvent e)
		{
			try
			{
				Interlocked.Increment(ref this._activeCallbackCount);
				if (!this._disposed)
				{
					HttpRuntime.SetShutdownMessage(SR.GetString("Change_notification_critical_dir"));
					FileChangeEventHandler callbackRenameOrCriticaldirChange = this._callbackRenameOrCriticaldirChange;
					if (callbackRenameOrCriticaldirChange != null)
					{
						callbackRenameOrCriticaldirChange(this, e);
					}
				}
			}
			finally
			{
				Interlocked.Decrement(ref this._activeCallbackCount);
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000074D0 File Offset: 0x000064D0
		internal void StopMonitoringFile(string alias, object target)
		{
			if (this.IsFCNDisabled)
			{
				return;
			}
			if (alias == null)
			{
				throw new HttpException(SR.GetString("Invalid_file_name_for_monitoring", new object[] { string.Empty }));
			}
			using (new ApplicationImpersonationContext())
			{
				this._lockDispose.AcquireReaderLock();
				try
				{
					if (!this._disposed)
					{
						FileMonitor fileMonitor = (FileMonitor)this._aliases[alias];
						DirectoryMonitor directoryMonitor;
						string text;
						if (fileMonitor != null && !fileMonitor.IsDirectory)
						{
							directoryMonitor = fileMonitor.DirectoryMonitor;
							text = fileMonitor.FileNameLong;
						}
						else
						{
							if (alias.Length == 0 || !UrlPath.IsAbsolutePhysicalPath(alias))
							{
								throw new HttpException(SR.GetString("Invalid_file_name_for_monitoring", new object[] { HttpRuntime.GetSafePath(alias) }));
							}
							string fullPath = FileChangesMonitor.GetFullPath(alias);
							string directoryOrRootName = UrlPath.GetDirectoryOrRootName(fullPath);
							text = Path.GetFileName(fullPath);
							if (string.IsNullOrEmpty(text))
							{
								throw new HttpException(SR.GetString("Invalid_file_name_for_monitoring", new object[] { HttpRuntime.GetSafePath(alias) }));
							}
							directoryMonitor = this.FindDirectoryMonitor(directoryOrRootName, false, false);
						}
						if (directoryMonitor != null)
						{
							directoryMonitor.StopMonitoringFile(text, target);
						}
					}
				}
				finally
				{
					this._lockDispose.ReleaseReaderLock();
				}
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007624 File Offset: 0x00006624
		internal void StopMonitoringPath(string alias, object target)
		{
			if (this.IsFCNDisabled)
			{
				return;
			}
			string text = null;
			if (alias == null)
			{
				throw new HttpException(SR.GetString("Invalid_file_name_for_monitoring", new object[] { string.Empty }));
			}
			using (new ApplicationImpersonationContext())
			{
				this._lockDispose.AcquireReaderLock();
				try
				{
					if (!this._disposed)
					{
						FileMonitor fileMonitor = (FileMonitor)this._aliases[alias];
						DirectoryMonitor directoryMonitor;
						if (fileMonitor != null)
						{
							directoryMonitor = fileMonitor.DirectoryMonitor;
							text = fileMonitor.FileNameLong;
						}
						else
						{
							if (alias.Length == 0 || !UrlPath.IsAbsolutePhysicalPath(alias))
							{
								throw new HttpException(SR.GetString("Invalid_file_name_for_monitoring", new object[] { HttpRuntime.GetSafePath(alias) }));
							}
							string fullPath = FileChangesMonitor.GetFullPath(alias);
							directoryMonitor = this.FindDirectoryMonitor(fullPath, false, false);
							if (directoryMonitor == null)
							{
								string directoryOrRootName = UrlPath.GetDirectoryOrRootName(fullPath);
								text = Path.GetFileName(fullPath);
								if (!string.IsNullOrEmpty(text))
								{
									directoryMonitor = this.FindDirectoryMonitor(directoryOrRootName, false, false);
								}
							}
						}
						if (directoryMonitor != null)
						{
							directoryMonitor.StopMonitoringFile(text, target);
						}
					}
				}
				finally
				{
					this._lockDispose.ReleaseReaderLock();
				}
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00007758 File Offset: 0x00006758
		internal FileAttributesData GetFileAttributes(string alias)
		{
			DirectoryMonitor directoryMonitor = null;
			string text = null;
			FileAttributesData fileAttributesData = null;
			if (alias == null)
			{
				throw FileChangesMonitor.CreateFileMonitoringException(-2147024809, alias);
			}
			string text2;
			if (!this.IsFCNDisabled)
			{
				FileAttributesData fileAttributesData2;
				using (new ApplicationImpersonationContext())
				{
					this._lockDispose.AcquireReaderLock();
					try
					{
						if (!this._disposed)
						{
							FileMonitor fileMonitor = (FileMonitor)this._aliases[alias];
							if (fileMonitor != null && !fileMonitor.IsDirectory)
							{
								directoryMonitor = fileMonitor.DirectoryMonitor;
								text = fileMonitor.FileNameLong;
							}
							else
							{
								if (alias.Length == 0 || !UrlPath.IsAbsolutePhysicalPath(alias))
								{
									throw FileChangesMonitor.CreateFileMonitoringException(-2147024809, alias);
								}
								text2 = FileChangesMonitor.GetFullPath(alias);
								string directoryOrRootName = UrlPath.GetDirectoryOrRootName(text2);
								text = Path.GetFileName(text2);
								if (text != null || text.Length > 0)
								{
									directoryMonitor = this.FindDirectoryMonitor(directoryOrRootName, false, false);
								}
							}
						}
					}
					finally
					{
						this._lockDispose.ReleaseReaderLock();
					}
					if (directoryMonitor == null || !directoryMonitor.GetFileAttributes(text, out fileAttributesData))
					{
						FileAttributesData.GetFileAttributes(alias, out fileAttributesData);
					}
					fileAttributesData2 = fileAttributesData;
				}
				return fileAttributesData2;
			}
			if (alias.Length == 0 || !UrlPath.IsAbsolutePhysicalPath(alias))
			{
				throw FileChangesMonitor.CreateFileMonitoringException(-2147024809, alias);
			}
			text2 = FileChangesMonitor.GetFullPath(alias);
			FindFileData findFileData = null;
			if (FindFileData.FindFile(text2, out findFileData) == 0)
			{
				return findFileData.FileAttributesData;
			}
			return null;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000078A8 File Offset: 0x000068A8
		internal void Stop()
		{
			if (this.IsFCNDisabled)
			{
				return;
			}
			using (new ApplicationImpersonationContext())
			{
				this._lockDispose.AcquireWriterLock();
				try
				{
					this._disposed = true;
					goto IL_0039;
				}
				finally
				{
					this._lockDispose.ReleaseWriterLock();
				}
				IL_002F:
				Thread.Sleep(250);
				IL_0039:
				if (this._activeCallbackCount != 0)
				{
					goto IL_002F;
				}
				if (this._dirMonSubdirs != null)
				{
					this._dirMonSubdirs.StopMonitoring();
					this._dirMonSubdirs = null;
				}
				if (this._dirMonSpecialDirs != null)
				{
					foreach (object obj in this._dirMonSpecialDirs)
					{
						DirectoryMonitor directoryMonitor = (DirectoryMonitor)obj;
						if (directoryMonitor != null)
						{
							directoryMonitor.StopMonitoring();
						}
					}
					this._dirMonSpecialDirs = null;
				}
				this._callbackRenameOrCriticaldirChange = null;
				if (this._dirs != null)
				{
					IDictionaryEnumerator enumerator2 = this._dirs.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						DirectoryMonitor directoryMonitor2 = (DirectoryMonitor)enumerator2.Value;
						directoryMonitor2.StopMonitoring();
					}
				}
				this._dirs.Clear();
				this._aliases.Clear();
			}
		}

		// Token: 0x04000DCE RID: 3534
		internal const int MAX_PATH = 260;

		// Token: 0x04000DCF RID: 3535
		internal static string[] s_dirsToMonitor = new string[] { "bin", "App_GlobalResources", "App_Code", "App_WebReferences", "App_Browsers" };

		// Token: 0x04000DD0 RID: 3536
		private ReadWriteSpinLock _lockDispose;

		// Token: 0x04000DD1 RID: 3537
		private bool _disposed;

		// Token: 0x04000DD2 RID: 3538
		private Hashtable _aliases;

		// Token: 0x04000DD3 RID: 3539
		private Hashtable _dirs;

		// Token: 0x04000DD4 RID: 3540
		private DirectoryMonitor _dirMonSubdirs;

		// Token: 0x04000DD5 RID: 3541
		private Hashtable _subDirDirMons;

		// Token: 0x04000DD6 RID: 3542
		private ArrayList _dirMonSpecialDirs;

		// Token: 0x04000DD7 RID: 3543
		private FileChangeEventHandler _callbackRenameOrCriticaldirChange;

		// Token: 0x04000DD8 RID: 3544
		private int _activeCallbackCount;

		// Token: 0x04000DD9 RID: 3545
		private DirectoryMonitor _dirMonAppPathInternal;

		// Token: 0x04000DDA RID: 3546
		private string _appPathInternal;

		// Token: 0x04000DDB RID: 3547
		private int _FCNMode;
	}
}
