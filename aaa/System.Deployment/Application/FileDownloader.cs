using System;
using System.Collections;
using System.Deployment.Application.Manifest;
using System.Deployment.Internal.Isolation;
using System.Globalization;
using System.IO;
using Microsoft.Internal.Performance;

namespace System.Deployment.Application
{
	// Token: 0x02000059 RID: 89
	internal abstract class FileDownloader
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060002A7 RID: 679 RVA: 0x0000FB45 File Offset: 0x0000EB45
		// (remove) Token: 0x060002A8 RID: 680 RVA: 0x0000FB5E File Offset: 0x0000EB5E
		public event FileDownloader.DownloadModifiedEventHandler DownloadModified;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060002A9 RID: 681 RVA: 0x0000FB77 File Offset: 0x0000EB77
		// (remove) Token: 0x060002AA RID: 682 RVA: 0x0000FB90 File Offset: 0x0000EB90
		public event FileDownloader.DownloadCompletedEventHandler DownloadCompleted;

		// Token: 0x060002AB RID: 683 RVA: 0x0000FBAC File Offset: 0x0000EBAC
		protected FileDownloader()
		{
			this._fileQueue = new Queue();
			this._eventArgs = new DownloadEventArgs();
			this._downloadResults = new ArrayList();
			this._buffer = new byte[4096];
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000FC06 File Offset: 0x0000EC06
		public static FileDownloader Create()
		{
			return new SystemNetDownloader();
		}

		// Token: 0x170000B9 RID: 185
		// (set) Token: 0x060002AD RID: 685 RVA: 0x0000FC0D File Offset: 0x0000EC0D
		public DownloadOptions Options
		{
			set
			{
				this._options = value;
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000FC18 File Offset: 0x0000EC18
		public void AddNotification(IDownloadNotification notification)
		{
			this.DownloadCompleted = (FileDownloader.DownloadCompletedEventHandler)Delegate.Combine(this.DownloadCompleted, new FileDownloader.DownloadCompletedEventHandler(notification.DownloadCompleted));
			this.DownloadModified = (FileDownloader.DownloadModifiedEventHandler)Delegate.Combine(this.DownloadModified, new FileDownloader.DownloadModifiedEventHandler(notification.DownloadModified));
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000FC6C File Offset: 0x0000EC6C
		public void RemoveNotification(IDownloadNotification notification)
		{
			this.DownloadModified = (FileDownloader.DownloadModifiedEventHandler)Delegate.Remove(this.DownloadModified, new FileDownloader.DownloadModifiedEventHandler(notification.DownloadModified));
			this.DownloadCompleted = (FileDownloader.DownloadCompletedEventHandler)Delegate.Remove(this.DownloadCompleted, new FileDownloader.DownloadCompletedEventHandler(notification.DownloadCompleted));
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000FCBF File Offset: 0x0000ECBF
		protected void OnModified()
		{
			if (this.DownloadModified != null)
			{
				this.DownloadModified(this, this._eventArgs);
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000FCDB File Offset: 0x0000ECDB
		protected void OnCompleted()
		{
			if (this.DownloadCompleted != null)
			{
				this.DownloadCompleted(this, this._eventArgs);
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000FCF7 File Offset: 0x0000ECF7
		public void AddFile(Uri sourceUri, string targetFilePath)
		{
			this.AddFile(sourceUri, targetFilePath, null, null);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000FD03 File Offset: 0x0000ED03
		public void AddFile(Uri sourceUri, string targetFilePath, int maxFileSize)
		{
			this.AddFile(sourceUri, targetFilePath, null, null, maxFileSize);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000FD10 File Offset: 0x0000ED10
		public void AddFile(Uri sourceUri, string targetFilePath, object cookie, HashCollection hashCollection)
		{
			this.AddFile(sourceUri, targetFilePath, cookie, hashCollection, -1);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000FD20 File Offset: 0x0000ED20
		public void AddFile(Uri sourceUri, string targetFilePath, object cookie, HashCollection hashCollection, int maxFileSize)
		{
			UriHelper.ValidateSupportedScheme(sourceUri);
			FileDownloader.DownloadQueueItem downloadQueueItem = new FileDownloader.DownloadQueueItem();
			downloadQueueItem._sourceUri = sourceUri;
			downloadQueueItem._targetPath = targetFilePath;
			downloadQueueItem._cookie = cookie;
			downloadQueueItem._hashCollection = hashCollection;
			downloadQueueItem._maxFileSize = maxFileSize;
			lock (this._fileQueue)
			{
				this._fileQueue.Enqueue(downloadQueueItem);
				this._eventArgs._filesTotal++;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000FDA4 File Offset: 0x0000EDA4
		public ComponentVerifier ComponentVerifier
		{
			get
			{
				return this._componentVerifier;
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000FDAC File Offset: 0x0000EDAC
		private static FileStream GetPatchSourceStream(string filePath)
		{
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			catch (IOException ex)
			{
				Logger.AddErrorInformation(ex, Resources.GetString("Ex_PatchSourceOpenFailed"), new object[] { Path.GetFileName(filePath) });
			}
			catch (UnauthorizedAccessException ex2)
			{
				Logger.AddErrorInformation(ex2, Resources.GetString("Ex_PatchSourceOpenFailed"), new object[] { Path.GetFileName(filePath) });
			}
			return fileStream;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000FE30 File Offset: 0x0000EE30
		private static FileStream GetPatchTargetStream(string filePath)
		{
			return new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000FE3C File Offset: 0x0000EE3C
		private static bool FileHashVerified(HashCollection hashCollection, string location)
		{
			try
			{
				ComponentVerifier.VerifyFileHash(location, hashCollection);
			}
			catch (InvalidDeploymentException ex)
			{
				if (ex.SubType == ExceptionTypes.HashValidation)
				{
					return false;
				}
				throw;
			}
			return true;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000FE78 File Offset: 0x0000EE78
		private static bool AddSingleFileInHashtable(Hashtable hashtable, HashCollection hashCollection, string location)
		{
			bool flag = false;
			if (global::System.IO.File.Exists(location) && FileDownloader.FileHashVerified(hashCollection, location))
			{
				foreach (Hash hash in hashCollection)
				{
					string compositString = hash.CompositString;
					if (!hashtable.Contains(compositString))
					{
						hashtable.Add(compositString, location);
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000FEF4 File Offset: 0x0000EEF4
		private static void AddFilesInHashtable(Hashtable hashtable, AssemblyManifest applicationManifest, string applicationFolder)
		{
			string text = null;
			global::System.Deployment.Application.Manifest.File[] files = applicationManifest.Files;
			foreach (global::System.Deployment.Application.Manifest.File file in files)
			{
				text = Path.Combine(applicationFolder, file.NameFS);
				try
				{
					FileDownloader.AddSingleFileInHashtable(hashtable, file.HashCollection, text);
				}
				catch (IOException ex)
				{
					Logger.AddErrorInformation(ex, Resources.GetString("Ex_PatchDependencyFailed"), new object[] { Path.GetFileName(text) });
				}
			}
			foreach (DependentAssembly dependentAssembly in applicationManifest.DependentAssemblies)
			{
				if (!dependentAssembly.IsPreRequisite)
				{
					text = Path.Combine(applicationFolder, dependentAssembly.Codebase);
					try
					{
						if (FileDownloader.AddSingleFileInHashtable(hashtable, dependentAssembly.HashCollection, text))
						{
							AssemblyManifest assemblyManifest = new AssemblyManifest(text);
							global::System.Deployment.Application.Manifest.File[] files2 = assemblyManifest.Files;
							for (int k = 0; k < files2.Length; k++)
							{
								string text2 = Path.Combine(Path.GetDirectoryName(text), files2[k].NameFS);
								FileDownloader.AddSingleFileInHashtable(hashtable, files2[k].HashCollection, text2);
							}
						}
					}
					catch (InvalidDeploymentException ex2)
					{
						Logger.AddErrorInformation(ex2, Resources.GetString("Ex_PatchDependencyFailed"), new object[] { Path.GetFileName(text) });
					}
					catch (IOException ex3)
					{
						Logger.AddErrorInformation(ex3, Resources.GetString("Ex_PatchDependencyFailed"), new object[] { Path.GetFileName(text) });
					}
				}
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0001008C File Offset: 0x0000F08C
		private bool PatchSingleFile(FileDownloader.DownloadQueueItem item, Hashtable dependencyTable)
		{
			if (item._hashCollection == null)
			{
				return false;
			}
			string text = null;
			foreach (Hash hash in item._hashCollection)
			{
				string compositString = hash.CompositString;
				if (dependencyTable.Contains(compositString))
				{
					text = (string)dependencyTable[compositString];
					break;
				}
			}
			if (text == null)
			{
				return false;
			}
			if (this._fCancelPending)
			{
				return false;
			}
			FileStream fileStream = null;
			FileStream fileStream2 = null;
			try
			{
				fileStream = FileDownloader.GetPatchSourceStream(text);
				if (fileStream == null)
				{
					return false;
				}
				Directory.CreateDirectory(Path.GetDirectoryName(item._targetPath));
				fileStream2 = FileDownloader.GetPatchTargetStream(item._targetPath);
				if (fileStream2 == null)
				{
					return false;
				}
				this._eventArgs._fileSourceUri = item._sourceUri;
				this._eventArgs.FileLocalPath = item._targetPath;
				this._eventArgs.Cookie = null;
				this._eventArgs._fileResponseUri = null;
				this.CheckForSizeLimit((ulong)fileStream.Length, true);
				this._accumulatedBytesTotal += fileStream.Length;
				this.SetBytesTotal();
				this.OnModified();
				int tickCount = Environment.TickCount;
				fileStream2.SetLength(fileStream.Length);
				fileStream2.Position = 0L;
				while (!this._fCancelPending)
				{
					int num = fileStream.Read(this._buffer, 0, this._buffer.Length);
					if (num > 0)
					{
						fileStream2.Write(this._buffer, 0, num);
					}
					this._eventArgs._bytesCompleted += (long)num;
					this._eventArgs._progress = (int)(this._eventArgs._bytesCompleted * 100L / this._eventArgs._bytesTotal);
					this.OnModifiedWithThrottle(ref tickCount);
					if (num <= 0)
					{
						goto IL_01D7;
					}
				}
				return false;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				if (fileStream2 != null)
				{
					fileStream2.Close();
				}
			}
			IL_01D7:
			this._eventArgs.Cookie = item._cookie;
			this._eventArgs._filesCompleted++;
			this.OnModified();
			DownloadResult downloadResult = new DownloadResult();
			downloadResult.ResponseUri = null;
			this._downloadResults.Add(downloadResult);
			return true;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000102F0 File Offset: 0x0000F2F0
		private void PatchFiles(SubscriptionState subState)
		{
			if (!subState.IsInstalled)
			{
				return;
			}
			Store.IPathLock pathLock = null;
			Store.IPathLock pathLock2 = null;
			using (subState.SubscriptionStore.AcquireSubscriptionReaderLock(subState))
			{
				if (!subState.IsInstalled)
				{
					return;
				}
				Hashtable hashtable = new Hashtable();
				try
				{
					pathLock = subState.SubscriptionStore.LockApplicationPath(subState.CurrentBind);
					FileDownloader.AddFilesInHashtable(hashtable, subState.CurrentApplicationManifest, pathLock.Path);
					try
					{
						if (subState.PreviousBind != null)
						{
							pathLock2 = subState.SubscriptionStore.LockApplicationPath(subState.PreviousBind);
							FileDownloader.AddFilesInHashtable(hashtable, subState.PreviousApplicationManifest, pathLock2.Path);
						}
						Queue queue = new Queue();
						do
						{
							FileDownloader.DownloadQueueItem downloadQueueItem = null;
							lock (this._fileQueue)
							{
								if (this._fileQueue.Count > 0)
								{
									downloadQueueItem = (FileDownloader.DownloadQueueItem)this._fileQueue.Dequeue();
								}
							}
							if (downloadQueueItem == null)
							{
								break;
							}
							if (!this.PatchSingleFile(downloadQueueItem, hashtable))
							{
								queue.Enqueue(downloadQueueItem);
							}
						}
						while (!this._fCancelPending);
						lock (this._fileQueue)
						{
							while (this._fileQueue.Count > 0)
							{
								queue.Enqueue(this._fileQueue.Dequeue());
							}
							this._fileQueue = queue;
						}
					}
					finally
					{
						if (pathLock2 != null)
						{
							pathLock2.Dispose();
						}
					}
				}
				finally
				{
					if (pathLock != null)
					{
						pathLock.Dispose();
					}
				}
			}
			if (this._fCancelPending)
			{
				throw new DownloadCancelledException();
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000104CC File Offset: 0x0000F4CC
		public void Download(SubscriptionState subState)
		{
			try
			{
				this.OnModified();
				if (subState != null)
				{
					CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfCopyBegin);
					this.PatchFiles(subState);
					CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfCopyEnd);
				}
				this.DownloadAllFiles();
			}
			finally
			{
				this.OnCompleted();
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002BF RID: 703 RVA: 0x00010528 File Offset: 0x0000F528
		public DownloadResult[] DownloadResults
		{
			get
			{
				return (DownloadResult[])this._downloadResults.ToArray(typeof(DownloadResult));
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00010544 File Offset: 0x0000F544
		public void SetExpectedBytesTotal(long total)
		{
			this._expectedBytesTotal = total;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0001054D File Offset: 0x0000F54D
		protected void SetBytesTotal()
		{
			if (this._expectedBytesTotal < this._accumulatedBytesTotal)
			{
				this._eventArgs._bytesTotal = this._accumulatedBytesTotal;
				return;
			}
			this._eventArgs._bytesTotal = this._expectedBytesTotal;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00010580 File Offset: 0x0000F580
		internal void CheckForSizeLimit(ulong bytesDownloaded, bool addToSize)
		{
			if (this._options == null)
			{
				return;
			}
			if (!this._options.EnforceSizeLimit)
			{
				return;
			}
			ulong num = ((this._options.SizeLimit > this._options.Size) ? (this._options.SizeLimit - this._options.Size) : 0UL);
			if (bytesDownloaded > num)
			{
				throw new DeploymentDownloadException(ExceptionTypes.SizeLimitForPartialTrustOnlineAppExceeded, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_OnlineSemiTrustAppSizeLimitExceeded"), new object[] { this._options.SizeLimit }));
			}
			if (addToSize && bytesDownloaded > 0UL)
			{
				this._options.Size = this._options.Size + bytesDownloaded;
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00010634 File Offset: 0x0000F634
		protected void OnModifiedWithThrottle(ref int lastTick)
		{
			int tickCount = Environment.TickCount;
			int num = tickCount - lastTick;
			if (num < 0)
			{
				num += int.MaxValue;
			}
			if (num >= 100)
			{
				this.OnModified();
				lastTick = tickCount;
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00010666 File Offset: 0x0000F666
		public virtual void Cancel()
		{
			this._fCancelPending = true;
		}

		// Token: 0x060002C5 RID: 709
		protected abstract void DownloadAllFiles();

		// Token: 0x04000219 RID: 537
		protected Queue _fileQueue;

		// Token: 0x0400021A RID: 538
		protected DownloadEventArgs _eventArgs;

		// Token: 0x0400021B RID: 539
		protected DownloadOptions _options = new DownloadOptions();

		// Token: 0x0400021C RID: 540
		protected ArrayList _downloadResults;

		// Token: 0x0400021D RID: 541
		protected long _accumulatedBytesTotal;

		// Token: 0x0400021E RID: 542
		protected long _expectedBytesTotal;

		// Token: 0x04000221 RID: 545
		protected ComponentVerifier _componentVerifier = new ComponentVerifier();

		// Token: 0x04000222 RID: 546
		protected bool _fCancelPending;

		// Token: 0x04000223 RID: 547
		protected byte[] _buffer;

		// Token: 0x0200005A RID: 90
		// (Invoke) Token: 0x060002C7 RID: 711
		public delegate void DownloadModifiedEventHandler(object sender, DownloadEventArgs e);

		// Token: 0x0200005B RID: 91
		// (Invoke) Token: 0x060002CB RID: 715
		public delegate void DownloadCompletedEventHandler(object sender, DownloadEventArgs e);

		// Token: 0x0200005C RID: 92
		protected class DownloadQueueItem
		{
			// Token: 0x04000224 RID: 548
			public const int FileOfAnySize = -1;

			// Token: 0x04000225 RID: 549
			public Uri _sourceUri;

			// Token: 0x04000226 RID: 550
			public string _targetPath;

			// Token: 0x04000227 RID: 551
			public object _cookie;

			// Token: 0x04000228 RID: 552
			public HashCollection _hashCollection;

			// Token: 0x04000229 RID: 553
			public int _maxFileSize;
		}
	}
}
