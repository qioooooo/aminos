using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000036 RID: 54
	internal sealed class DirectoryMonitor : IDisposable
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00005DBD File Offset: 0x00004DBD
		internal DirectoryMonitor(string appPathInternal)
			: this(appPathInternal, true, 347U)
		{
			this._isDirMonAppPathInternal = true;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005DD3 File Offset: 0x00004DD3
		internal DirectoryMonitor(string dir, bool watchSubtree, uint notifyFilter)
		{
			this.Directory = dir;
			this._fileMons = new Hashtable(StringComparer.OrdinalIgnoreCase);
			this._watchSubtree = watchSubtree;
			this._notifyFilter = notifyFilter;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005E00 File Offset: 0x00004E00
		void IDisposable.Dispose()
		{
			if (this._dirMonCompletion != null)
			{
				((IDisposable)this._dirMonCompletion).Dispose();
				this._dirMonCompletion = null;
			}
			if (this._anyFileMon != null)
			{
				HttpRuntime.FileChangesMonitor.RemoveAliases(this._anyFileMon);
				this._anyFileMon = null;
			}
			foreach (object obj in this._fileMons)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				FileMonitor fileMonitor = (FileMonitor)dictionaryEntry.Value;
				if (fileMonitor.FileNameLong == text)
				{
					HttpRuntime.FileChangesMonitor.RemoveAliases(fileMonitor);
				}
			}
			this._fileMons.Clear();
			this._cShortNames = 0;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005ED4 File Offset: 0x00004ED4
		internal bool IsMonitoring()
		{
			return this.GetFileMonitorsCount() > 0;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005EDF File Offset: 0x00004EDF
		private void StartMonitoring()
		{
			if (this._dirMonCompletion == null)
			{
				this._dirMonCompletion = new DirMonCompletion(this, this.Directory, this._watchSubtree, this._notifyFilter);
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005F08 File Offset: 0x00004F08
		internal void StopMonitoring()
		{
			lock (this)
			{
				((IDisposable)this).Dispose();
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005F3C File Offset: 0x00004F3C
		private FileMonitor FindFileMonitor(string file)
		{
			FileMonitor fileMonitor;
			if (file == null)
			{
				fileMonitor = this._anyFileMon;
			}
			else
			{
				fileMonitor = (FileMonitor)this._fileMons[file];
			}
			return fileMonitor;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005F68 File Offset: 0x00004F68
		private FileMonitor AddFileMonitor(string file)
		{
			FindFileData findFileData = null;
			FileMonitor fileMonitor;
			if (string.IsNullOrEmpty(file))
			{
				fileMonitor = new FileMonitor(this, null, null, true, null, null);
				this._anyFileMon = fileMonitor;
			}
			else
			{
				string text = Path.Combine(this.Directory, file);
				int num;
				if (this._isDirMonAppPathInternal)
				{
					num = FindFileData.FindFile(text, this.Directory, out findFileData);
				}
				else
				{
					num = FindFileData.FindFile(text, out findFileData);
				}
				if (num == 0)
				{
					if (!this._isDirMonAppPathInternal && (findFileData.FileAttributesData.FileAttributes & FileAttributes.Directory) != (FileAttributes)0)
					{
						throw FileChangesMonitor.CreateFileMonitoringException(-2147024809, text);
					}
					byte[] dacl = FileSecurity.GetDacl(text);
					fileMonitor = new FileMonitor(this, findFileData.FileNameLong, findFileData.FileNameShort, true, findFileData.FileAttributesData, dacl);
					this._fileMons.Add(findFileData.FileNameLong, fileMonitor);
					this.UpdateFileNameShort(fileMonitor, null, findFileData.FileNameShort);
				}
				else
				{
					if (num != -2147024893 && num != -2147024894)
					{
						throw FileChangesMonitor.CreateFileMonitoringException(num, text);
					}
					if (file.IndexOf('~') != -1)
					{
						throw FileChangesMonitor.CreateFileMonitoringException(-2147024809, text);
					}
					fileMonitor = new FileMonitor(this, file, null, false, null, null);
					this._fileMons.Add(file, fileMonitor);
				}
			}
			return fileMonitor;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006080 File Offset: 0x00005080
		private void UpdateFileNameShort(FileMonitor fileMon, string oldFileNameShort, string newFileNameShort)
		{
			if (oldFileNameShort != null)
			{
				FileMonitor fileMonitor = (FileMonitor)this._fileMons[oldFileNameShort];
				if (fileMonitor != null)
				{
					if (fileMonitor != fileMon)
					{
						fileMonitor.RemoveFileNameShort();
					}
					this._fileMons.Remove(oldFileNameShort);
					this._cShortNames--;
				}
			}
			if (newFileNameShort != null)
			{
				this._fileMons.Add(newFileNameShort, fileMon);
				this._cShortNames++;
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000060E8 File Offset: 0x000050E8
		private void RemoveFileMonitor(FileMonitor fileMon)
		{
			if (fileMon == this._anyFileMon)
			{
				this._anyFileMon = null;
			}
			else
			{
				this._fileMons.Remove(fileMon.FileNameLong);
				if (fileMon.FileNameShort != null)
				{
					this._fileMons.Remove(fileMon.FileNameShort);
					this._cShortNames--;
				}
			}
			HttpRuntime.FileChangesMonitor.RemoveAliases(fileMon);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000614C File Offset: 0x0000514C
		private int GetFileMonitorsCount()
		{
			int num = this._fileMons.Count - this._cShortNames;
			if (this._anyFileMon != null)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000617C File Offset: 0x0000517C
		internal FileMonitor StartMonitoringFile(string file, FileChangeEventHandler callback, string alias)
		{
			FileMonitor fileMonitor = null;
			bool flag = false;
			lock (this)
			{
				fileMonitor = this.FindFileMonitor(file);
				if (fileMonitor == null)
				{
					fileMonitor = this.AddFileMonitor(file);
					if (this.GetFileMonitorsCount() == 1)
					{
						flag = true;
					}
				}
				fileMonitor.AddTarget(callback, alias, true);
				if (flag)
				{
					this.StartMonitoring();
				}
			}
			return fileMonitor;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000061E0 File Offset: 0x000051E0
		internal void StopMonitoringFile(string file, object target)
		{
			lock (this)
			{
				FileMonitor fileMonitor = this.FindFileMonitor(file);
				if (fileMonitor != null && fileMonitor.RemoveTarget(target) == 0)
				{
					this.RemoveFileMonitor(fileMonitor);
					if (this.GetFileMonitorsCount() == 0)
					{
						((IDisposable)this).Dispose();
					}
				}
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000623C File Offset: 0x0000523C
		internal bool GetFileAttributes(string file, out FileAttributesData fad)
		{
			fad = null;
			lock (this)
			{
				FileMonitor fileMonitor = this.FindFileMonitor(file);
				if (fileMonitor != null)
				{
					fad = fileMonitor.Attributes;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000628C File Offset: 0x0000528C
		private bool IsChangeAfterStartMonitoring(FileAttributesData fad, FileMonitorTarget target, DateTime utcCompletion)
		{
			return fad.UtcLastAccessTime.AddSeconds(60.0) < target.UtcStartMonitoring || utcCompletion > target.UtcStartMonitoring || fad.UtcLastAccessTime < fad.UtcLastWriteTime || fad.UtcLastAccessTime.TimeOfDay == TimeSpan.Zero || fad.UtcLastAccessTime >= target.UtcStartMonitoring;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006318 File Offset: 0x00005318
		private bool GetFileMonitorForSpecialDirectory(string fileName, ref FileMonitor fileMon)
		{
			for (int i = 0; i < FileChangesMonitor.s_dirsToMonitor.Length; i++)
			{
				if (StringUtil.StringStartsWithIgnoreCase(fileName, FileChangesMonitor.s_dirsToMonitor[i]))
				{
					fileMon = (FileMonitor)this._fileMons[FileChangesMonitor.s_dirsToMonitor[i]];
					return fileMon != null;
				}
			}
			int num = fileName.IndexOf("App_LocalResources", StringComparison.OrdinalIgnoreCase);
			if (num > -1)
			{
				int num2 = num + "App_LocalResources".Length;
				if (fileName.Length == num2 || fileName[num2] == Path.DirectorySeparatorChar)
				{
					string text = fileName.Substring(0, num2);
					fileMon = (FileMonitor)this._fileMons[text];
					return fileMon != null;
				}
			}
			return false;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000063C4 File Offset: 0x000053C4
		internal void OnFileChange(FileAction action, string fileName, DateTime utcCompletion)
		{
			try
			{
				FileMonitor fileMonitor = null;
				ArrayList arrayList = null;
				FileAttributesData fileAttributesData = null;
				FileAttributesData fileAttributesData2 = null;
				byte[] array = null;
				byte[] array2 = null;
				FileAction fileAction = FileAction.Error;
				DateTime dateTime = DateTime.MinValue;
				bool flag = false;
				if (this._dirMonCompletion != null)
				{
					lock (this)
					{
						if (this._fileMons.Count > 0)
						{
							if (action == FileAction.Error || action == FileAction.Overwhelming)
							{
								if (action == FileAction.Overwhelming)
								{
									HttpRuntime.SetShutdownMessage("Overwhelming Change Notification in " + this.Directory);
									if (Interlocked.Increment(ref DirectoryMonitor.s_notificationBufferSizeIncreased) == 1)
									{
										UnsafeNativeMethods.GrowFileNotificationBuffer(HttpRuntime.AppDomainAppIdInternal, this._watchSubtree);
									}
								}
								else if (action == FileAction.Error)
								{
									HttpRuntime.SetShutdownMessage("File Change Notification Error in " + this.Directory);
								}
								arrayList = new ArrayList();
								foreach (object obj in this._fileMons)
								{
									DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
									string text = (string)dictionaryEntry.Key;
									fileMonitor = (FileMonitor)dictionaryEntry.Value;
									if (fileMonitor.FileNameLong == text && fileMonitor.Exists)
									{
										fileMonitor.ResetCachedAttributes();
										fileMonitor.LastAction = action;
										fileMonitor.UtcLastCompletion = utcCompletion;
										ICollection collection = fileMonitor.Targets;
										arrayList.AddRange(collection);
									}
								}
								fileMonitor = null;
							}
							else
							{
								fileMonitor = (FileMonitor)this._fileMons[fileName];
								if (this._isDirMonAppPathInternal && fileMonitor == null)
								{
									flag = this.GetFileMonitorForSpecialDirectory(fileName, ref fileMonitor);
								}
								if (fileMonitor != null)
								{
									ICollection collection = fileMonitor.Targets;
									arrayList = new ArrayList(collection);
									fileAttributesData = fileMonitor.Attributes;
									array = fileMonitor.Dacl;
									fileAction = fileMonitor.LastAction;
									dateTime = fileMonitor.UtcLastCompletion;
									fileMonitor.LastAction = action;
									fileMonitor.UtcLastCompletion = utcCompletion;
									if (action == FileAction.Removed || action == FileAction.RenamedOldName)
									{
										fileMonitor.MakeExtinct();
									}
									else if (fileMonitor.Exists)
									{
										if (dateTime != utcCompletion)
										{
											fileMonitor.UpdateCachedAttributes();
										}
									}
									else
									{
										FindFileData findFileData = null;
										string text2 = Path.Combine(this.Directory, fileMonitor.FileNameLong);
										int num;
										if (this._isDirMonAppPathInternal)
										{
											num = FindFileData.FindFile(text2, this.Directory, out findFileData);
										}
										else
										{
											num = FindFileData.FindFile(text2, out findFileData);
										}
										if (num == 0)
										{
											string fileNameShort = fileMonitor.FileNameShort;
											byte[] dacl = FileSecurity.GetDacl(text2);
											fileMonitor.MakeExist(findFileData, dacl);
											this.UpdateFileNameShort(fileMonitor, fileNameShort, findFileData.FileNameShort);
										}
									}
									fileAttributesData2 = fileMonitor.Attributes;
									array2 = fileMonitor.Dacl;
								}
							}
						}
						if (this._anyFileMon != null)
						{
							ICollection collection = this._anyFileMon.Targets;
							if (arrayList != null)
							{
								arrayList.AddRange(collection);
							}
							else
							{
								arrayList = new ArrayList(collection);
							}
						}
						if (action == FileAction.Error || action == FileAction.Overwhelming)
						{
							((IDisposable)this).Dispose();
						}
					}
					bool flag2 = false;
					if (!flag && fileName != null && action == FileAction.Modified)
					{
						FileAttributesData fileAttributesData3 = fileAttributesData2;
						if (fileAttributesData3 == null)
						{
							string text3 = Path.Combine(this.Directory, fileName);
							FileAttributesData.GetFileAttributes(text3, out fileAttributesData3);
						}
						if (fileAttributesData3 != null && (fileAttributesData3.FileAttributes & FileAttributes.Directory) != (FileAttributes)0)
						{
							flag2 = true;
						}
					}
					if (arrayList != null && !flag2)
					{
						lock (DirectoryMonitor.s_notificationQueue.SyncRoot)
						{
							int i = 0;
							int count = arrayList.Count;
							while (i < count)
							{
								FileMonitorTarget fileMonitorTarget = (FileMonitorTarget)arrayList[i];
								bool flag3;
								if ((action != FileAction.Added && action != FileAction.Modified) || fileAttributesData2 == null)
								{
									flag3 = true;
								}
								else if (action == FileAction.Added)
								{
									flag3 = this.IsChangeAfterStartMonitoring(fileAttributesData2, fileMonitorTarget, utcCompletion);
								}
								else if (utcCompletion == dateTime)
								{
									flag3 = fileAction != FileAction.Modified;
								}
								else
								{
									flag3 = fileAttributesData == null || (array == null || array != array2) || this.IsChangeAfterStartMonitoring(fileAttributesData2, fileMonitorTarget, utcCompletion);
								}
								if (flag3)
								{
									DirectoryMonitor.s_notificationQueue.Enqueue(new NotificationQueueItem(fileMonitorTarget.Callback, action, fileMonitorTarget.Alias));
								}
								i++;
							}
						}
						if (DirectoryMonitor.s_notificationQueue.Count > 0 && DirectoryMonitor.s_inNotificationThread == 0 && Interlocked.Exchange(ref DirectoryMonitor.s_inNotificationThread, 1) == 0)
						{
							WorkItem.PostInternal(DirectoryMonitor.s_notificationCallback);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000680C File Offset: 0x0000580C
		private static void FireNotifications()
		{
			try
			{
				do
				{
					NotificationQueueItem notificationQueueItem = null;
					lock (DirectoryMonitor.s_notificationQueue.SyncRoot)
					{
						if (DirectoryMonitor.s_notificationQueue.Count > 0)
						{
							notificationQueueItem = (NotificationQueueItem)DirectoryMonitor.s_notificationQueue.Dequeue();
						}
					}
					if (notificationQueueItem != null)
					{
						try
						{
							notificationQueueItem.Callback(null, new FileChangeEvent(notificationQueueItem.Action, notificationQueueItem.Filename));
							continue;
						}
						catch (Exception)
						{
							continue;
						}
					}
					Interlocked.Exchange(ref DirectoryMonitor.s_inNotificationThread, 0);
				}
				while (DirectoryMonitor.s_notificationQueue.Count != 0 && Interlocked.Exchange(ref DirectoryMonitor.s_inNotificationThread, 1) == 0);
			}
			catch
			{
				Interlocked.Exchange(ref DirectoryMonitor.s_inNotificationThread, 0);
			}
		}

		// Token: 0x04000DC2 RID: 3522
		private static Queue s_notificationQueue = new Queue();

		// Token: 0x04000DC3 RID: 3523
		private static WorkItemCallback s_notificationCallback = new WorkItemCallback(DirectoryMonitor.FireNotifications);

		// Token: 0x04000DC4 RID: 3524
		private static int s_inNotificationThread;

		// Token: 0x04000DC5 RID: 3525
		private static int s_notificationBufferSizeIncreased = 0;

		// Token: 0x04000DC6 RID: 3526
		internal readonly string Directory;

		// Token: 0x04000DC7 RID: 3527
		private Hashtable _fileMons;

		// Token: 0x04000DC8 RID: 3528
		private int _cShortNames;

		// Token: 0x04000DC9 RID: 3529
		private FileMonitor _anyFileMon;

		// Token: 0x04000DCA RID: 3530
		private bool _watchSubtree;

		// Token: 0x04000DCB RID: 3531
		private uint _notifyFilter;

		// Token: 0x04000DCC RID: 3532
		private DirMonCompletion _dirMonCompletion;

		// Token: 0x04000DCD RID: 3533
		private bool _isDirMonAppPathInternal;
	}
}
