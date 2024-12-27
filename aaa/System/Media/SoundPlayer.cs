using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Media
{
	// Token: 0x02000219 RID: 537
	[ToolboxItem(false)]
	[HostProtection(SecurityAction.LinkDemand, UI = true)]
	[Serializable]
	public class SoundPlayer : Component, ISerializable
	{
		// Token: 0x06001222 RID: 4642 RVA: 0x0003D303 File Offset: 0x0003C303
		public SoundPlayer()
		{
			this.loadAsyncOperationCompleted = new SendOrPostCallback(this.LoadAsyncOperationCompleted);
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0003D33F File Offset: 0x0003C33F
		public SoundPlayer(string soundLocation)
			: this()
		{
			if (soundLocation == null)
			{
				soundLocation = string.Empty;
			}
			this.SetupSoundLocation(soundLocation);
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0003D358 File Offset: 0x0003C358
		public SoundPlayer(Stream stream)
			: this()
		{
			this.stream = stream;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0003D368 File Offset: 0x0003C368
		protected SoundPlayer(SerializationInfo serializationInfo, StreamingContext context)
		{
			foreach (SerializationEntry serializationEntry in serializationInfo)
			{
				string name;
				if ((name = serializationEntry.Name) != null)
				{
					if (!(name == "SoundLocation"))
					{
						if (!(name == "Stream"))
						{
							if (name == "LoadTimeout")
							{
								this.LoadTimeout = (int)serializationEntry.Value;
							}
						}
						else
						{
							this.stream = (Stream)serializationEntry.Value;
							if (this.stream.CanSeek)
							{
								this.stream.Seek(0L, SeekOrigin.Begin);
							}
						}
					}
					else
					{
						this.SetupSoundLocation((string)serializationEntry.Value);
					}
				}
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001226 RID: 4646 RVA: 0x0003D445 File Offset: 0x0003C445
		// (set) Token: 0x06001227 RID: 4647 RVA: 0x0003D44D File Offset: 0x0003C44D
		public int LoadTimeout
		{
			get
			{
				return this.loadTimeout;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("LoadTimeout", value, SR.GetString("SoundAPILoadTimeout"));
				}
				this.loadTimeout = value;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x0003D478 File Offset: 0x0003C478
		// (set) Token: 0x06001229 RID: 4649 RVA: 0x0003D4BA File Offset: 0x0003C4BA
		public string SoundLocation
		{
			get
			{
				if (this.uri != null && this.uri.IsFile)
				{
					new FileIOPermission(PermissionState.None)
					{
						AllFiles = FileIOPermissionAccess.PathDiscovery
					}.Demand();
				}
				return this.soundLocation;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (this.soundLocation.Equals(value))
				{
					return;
				}
				this.SetupSoundLocation(value);
				this.OnSoundLocationChanged(EventArgs.Empty);
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x0003D4E7 File Offset: 0x0003C4E7
		// (set) Token: 0x0600122B RID: 4651 RVA: 0x0003D4FF File Offset: 0x0003C4FF
		public Stream Stream
		{
			get
			{
				if (this.uri != null)
				{
					return null;
				}
				return this.stream;
			}
			set
			{
				if (this.stream == value)
				{
					return;
				}
				this.SetupStream(value);
				this.OnStreamChanged(EventArgs.Empty);
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x0003D51D File Offset: 0x0003C51D
		public bool IsLoadCompleted
		{
			get
			{
				return this.isLoadCompleted;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x0600122D RID: 4653 RVA: 0x0003D525 File Offset: 0x0003C525
		// (set) Token: 0x0600122E RID: 4654 RVA: 0x0003D52D File Offset: 0x0003C52D
		public object Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				this.tag = value;
			}
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0003D538 File Offset: 0x0003C538
		public void LoadAsync()
		{
			if (this.uri != null && this.uri.IsFile)
			{
				this.isLoadCompleted = true;
				FileInfo fileInfo = new FileInfo(this.uri.LocalPath);
				if (!fileInfo.Exists)
				{
					throw new FileNotFoundException(SR.GetString("SoundAPIFileDoesNotExist"), this.soundLocation);
				}
				this.OnLoadCompleted(new AsyncCompletedEventArgs(null, false, null));
				return;
			}
			else
			{
				if (this.copyThread != null && this.copyThread.ThreadState == ThreadState.Running)
				{
					return;
				}
				this.isLoadCompleted = false;
				this.streamData = null;
				this.currentPos = 0;
				this.asyncOperation = AsyncOperationManager.CreateOperation(null);
				this.LoadStream(false);
				return;
			}
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0003D5E3 File Offset: 0x0003C5E3
		private void LoadAsyncOperationCompleted(object arg)
		{
			this.OnLoadCompleted((AsyncCompletedEventArgs)arg);
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x0003D5F1 File Offset: 0x0003C5F1
		private void CleanupStreamData()
		{
			this.currentPos = 0;
			this.streamData = null;
			this.isLoadCompleted = false;
			this.lastLoadException = null;
			this.doesLoadAppearSynchronous = false;
			this.copyThread = null;
			this.semaphore.Set();
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0003D62C File Offset: 0x0003C62C
		public void Load()
		{
			if (!(this.uri != null) || !this.uri.IsFile)
			{
				this.LoadSync();
				return;
			}
			FileInfo fileInfo = new FileInfo(this.uri.LocalPath);
			if (!fileInfo.Exists)
			{
				throw new FileNotFoundException(SR.GetString("SoundAPIFileDoesNotExist"), this.soundLocation);
			}
			this.isLoadCompleted = true;
			this.OnLoadCompleted(new AsyncCompletedEventArgs(null, false, null));
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0003D6A0 File Offset: 0x0003C6A0
		private void LoadAndPlay(int flags)
		{
			if (string.IsNullOrEmpty(this.soundLocation) && this.stream == null)
			{
				SystemSounds.Beep.Play();
				return;
			}
			if (this.uri != null && this.uri.IsFile)
			{
				string localPath = this.uri.LocalPath;
				FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, localPath);
				fileIOPermission.Demand();
				this.isLoadCompleted = true;
				SoundPlayer.IntSecurity.SafeSubWindows.Demand();
				global::System.ComponentModel.IntSecurity.UnmanagedCode.Assert();
				try
				{
					this.ValidateSoundFile(localPath);
					SoundPlayer.UnsafeNativeMethods.PlaySound(localPath, IntPtr.Zero, 2 | flags);
					return;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			this.LoadSync();
			SoundPlayer.ValidateSoundData(this.streamData);
			SoundPlayer.IntSecurity.SafeSubWindows.Demand();
			global::System.ComponentModel.IntSecurity.UnmanagedCode.Assert();
			try
			{
				SoundPlayer.UnsafeNativeMethods.PlaySound(this.streamData, IntPtr.Zero, 6 | flags);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0003D798 File Offset: 0x0003C798
		private void LoadSync()
		{
			if (!this.semaphore.WaitOne(this.LoadTimeout, false))
			{
				if (this.copyThread != null)
				{
					this.copyThread.Abort();
				}
				this.CleanupStreamData();
				throw new TimeoutException(SR.GetString("SoundAPILoadTimedOut"));
			}
			if (this.streamData != null)
			{
				return;
			}
			if (this.uri != null && !this.uri.IsFile && this.stream == null)
			{
				WebPermission webPermission = new WebPermission(NetworkAccess.Connect, this.uri.AbsolutePath);
				webPermission.Demand();
				WebRequest webRequest = WebRequest.Create(this.uri);
				webRequest.Timeout = this.LoadTimeout;
				WebResponse response = webRequest.GetResponse();
				this.stream = response.GetResponseStream();
			}
			if (this.stream.CanSeek)
			{
				this.LoadStream(true);
			}
			else
			{
				this.doesLoadAppearSynchronous = true;
				this.LoadStream(false);
				if (!this.semaphore.WaitOne(this.LoadTimeout, false))
				{
					if (this.copyThread != null)
					{
						this.copyThread.Abort();
					}
					this.CleanupStreamData();
					throw new TimeoutException(SR.GetString("SoundAPILoadTimedOut"));
				}
				this.doesLoadAppearSynchronous = false;
				if (this.lastLoadException != null)
				{
					throw this.lastLoadException;
				}
			}
			this.copyThread = null;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0003D8D0 File Offset: 0x0003C8D0
		private void LoadStream(bool loadSync)
		{
			if (loadSync && this.stream.CanSeek)
			{
				int num = (int)this.stream.Length;
				this.currentPos = 0;
				this.streamData = new byte[num];
				this.stream.Read(this.streamData, 0, num);
				this.isLoadCompleted = true;
				this.OnLoadCompleted(new AsyncCompletedEventArgs(null, false, null));
				return;
			}
			this.semaphore.Reset();
			this.copyThread = new Thread(new ThreadStart(this.WorkerThread));
			this.copyThread.Start();
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0003D965 File Offset: 0x0003C965
		public void Play()
		{
			this.LoadAndPlay(1);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0003D96E File Offset: 0x0003C96E
		public void PlaySync()
		{
			this.LoadAndPlay(0);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0003D977 File Offset: 0x0003C977
		public void PlayLooping()
		{
			this.LoadAndPlay(9);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0003D984 File Offset: 0x0003C984
		private static Uri ResolveUri(string partialUri)
		{
			Uri uri = null;
			try
			{
				uri = new Uri(partialUri);
			}
			catch (UriFormatException)
			{
			}
			if (uri == null)
			{
				try
				{
					uri = new Uri(Path.GetFullPath(partialUri));
				}
				catch (UriFormatException)
				{
				}
			}
			return uri;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0003D9D8 File Offset: 0x0003C9D8
		private void SetupSoundLocation(string soundLocation)
		{
			if (this.copyThread != null)
			{
				this.copyThread.Abort();
				this.CleanupStreamData();
			}
			this.uri = SoundPlayer.ResolveUri(soundLocation);
			this.soundLocation = soundLocation;
			this.stream = null;
			if (this.uri == null)
			{
				if (!string.IsNullOrEmpty(soundLocation))
				{
					throw new UriFormatException(SR.GetString("SoundAPIBadSoundLocation"));
				}
			}
			else if (!this.uri.IsFile)
			{
				this.streamData = null;
				this.currentPos = 0;
				this.isLoadCompleted = false;
			}
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0003DA60 File Offset: 0x0003CA60
		private void SetupStream(Stream stream)
		{
			if (this.copyThread != null)
			{
				this.copyThread.Abort();
				this.CleanupStreamData();
			}
			this.stream = stream;
			this.soundLocation = string.Empty;
			this.streamData = null;
			this.currentPos = 0;
			this.isLoadCompleted = false;
			if (stream != null)
			{
				this.uri = null;
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0003DAB7 File Offset: 0x0003CAB7
		public void Stop()
		{
			SoundPlayer.IntSecurity.SafeSubWindows.Demand();
			SoundPlayer.UnsafeNativeMethods.PlaySound(null, IntPtr.Zero, 64);
		}

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x0600123D RID: 4669 RVA: 0x0003DAD1 File Offset: 0x0003CAD1
		// (remove) Token: 0x0600123E RID: 4670 RVA: 0x0003DAE4 File Offset: 0x0003CAE4
		public event AsyncCompletedEventHandler LoadCompleted
		{
			add
			{
				base.Events.AddHandler(SoundPlayer.EventLoadCompleted, value);
			}
			remove
			{
				base.Events.RemoveHandler(SoundPlayer.EventLoadCompleted, value);
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x0600123F RID: 4671 RVA: 0x0003DAF7 File Offset: 0x0003CAF7
		// (remove) Token: 0x06001240 RID: 4672 RVA: 0x0003DB0A File Offset: 0x0003CB0A
		public event EventHandler SoundLocationChanged
		{
			add
			{
				base.Events.AddHandler(SoundPlayer.EventSoundLocationChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(SoundPlayer.EventSoundLocationChanged, value);
			}
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06001241 RID: 4673 RVA: 0x0003DB1D File Offset: 0x0003CB1D
		// (remove) Token: 0x06001242 RID: 4674 RVA: 0x0003DB30 File Offset: 0x0003CB30
		public event EventHandler StreamChanged
		{
			add
			{
				base.Events.AddHandler(SoundPlayer.EventStreamChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(SoundPlayer.EventStreamChanged, value);
			}
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0003DB44 File Offset: 0x0003CB44
		protected virtual void OnLoadCompleted(AsyncCompletedEventArgs e)
		{
			AsyncCompletedEventHandler asyncCompletedEventHandler = (AsyncCompletedEventHandler)base.Events[SoundPlayer.EventLoadCompleted];
			if (asyncCompletedEventHandler != null)
			{
				asyncCompletedEventHandler(this, e);
			}
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0003DB74 File Offset: 0x0003CB74
		protected virtual void OnSoundLocationChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[SoundPlayer.EventSoundLocationChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0003DBA4 File Offset: 0x0003CBA4
		protected virtual void OnStreamChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[SoundPlayer.EventStreamChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0003DBD4 File Offset: 0x0003CBD4
		private void WorkerThread()
		{
			try
			{
				if (this.uri != null && !this.uri.IsFile && this.stream == null)
				{
					WebRequest webRequest = WebRequest.Create(this.uri);
					WebResponse response = webRequest.GetResponse();
					this.stream = response.GetResponseStream();
				}
				this.streamData = new byte[1024];
				int i = this.stream.Read(this.streamData, this.currentPos, 1024);
				int num = i;
				while (i > 0)
				{
					this.currentPos += i;
					if (this.streamData.Length < this.currentPos + 1024)
					{
						byte[] array = new byte[this.streamData.Length * 2];
						Array.Copy(this.streamData, array, this.streamData.Length);
						this.streamData = array;
					}
					i = this.stream.Read(this.streamData, this.currentPos, 1024);
					num += i;
				}
				this.lastLoadException = null;
			}
			catch (Exception ex)
			{
				this.lastLoadException = ex;
			}
			catch
			{
				throw;
			}
			if (!this.doesLoadAppearSynchronous)
			{
				this.asyncOperation.PostOperationCompleted(this.loadAsyncOperationCompleted, new AsyncCompletedEventArgs(this.lastLoadException, false, null));
			}
			this.isLoadCompleted = true;
			this.semaphore.Set();
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0003DD38 File Offset: 0x0003CD38
		private unsafe void ValidateSoundFile(string fileName)
		{
			SoundPlayer.NativeMethods.MMCKINFO mmckinfo = new SoundPlayer.NativeMethods.MMCKINFO();
			SoundPlayer.NativeMethods.MMCKINFO mmckinfo2 = new SoundPlayer.NativeMethods.MMCKINFO();
			SoundPlayer.NativeMethods.WAVEFORMATEX waveformatex = null;
			IntPtr intPtr = SoundPlayer.UnsafeNativeMethods.mmioOpen(fileName, IntPtr.Zero, 65536);
			if (intPtr == IntPtr.Zero)
			{
				throw new FileNotFoundException(SR.GetString("SoundAPIFileDoesNotExist"), this.soundLocation);
			}
			try
			{
				mmckinfo.fccType = SoundPlayer.mmioFOURCC('W', 'A', 'V', 'E');
				if (SoundPlayer.UnsafeNativeMethods.mmioDescend(intPtr, mmckinfo, null, 32) != 0)
				{
					throw new InvalidOperationException(SR.GetString("SoundAPIInvalidWaveFile", new object[] { this.soundLocation }));
				}
				while (SoundPlayer.UnsafeNativeMethods.mmioDescend(intPtr, mmckinfo2, mmckinfo, 0) == 0)
				{
					if (mmckinfo2.dwDataOffset + mmckinfo2.cksize > mmckinfo.dwDataOffset + mmckinfo.cksize)
					{
						throw new InvalidOperationException(SR.GetString("SoundAPIInvalidWaveHeader"));
					}
					if (mmckinfo2.ckID == SoundPlayer.mmioFOURCC('f', 'm', 't', ' ') && waveformatex == null)
					{
						int num = mmckinfo2.cksize;
						if (num < Marshal.SizeOf(typeof(SoundPlayer.NativeMethods.WAVEFORMATEX)))
						{
							num = Marshal.SizeOf(typeof(SoundPlayer.NativeMethods.WAVEFORMATEX));
						}
						waveformatex = new SoundPlayer.NativeMethods.WAVEFORMATEX();
						byte[] array = new byte[num];
						if (SoundPlayer.UnsafeNativeMethods.mmioRead(intPtr, array, num) != num)
						{
							throw new InvalidOperationException(SR.GetString("SoundAPIReadError", new object[] { this.soundLocation }));
						}
						try
						{
							fixed (byte* ptr = array)
							{
								Marshal.PtrToStructure((IntPtr)((void*)ptr), waveformatex);
							}
						}
						finally
						{
							byte* ptr = null;
						}
					}
					SoundPlayer.UnsafeNativeMethods.mmioAscend(intPtr, mmckinfo2, 0);
				}
				if (waveformatex == null)
				{
					throw new InvalidOperationException(SR.GetString("SoundAPIInvalidWaveHeader"));
				}
				if (waveformatex.wFormatTag != 1 && waveformatex.wFormatTag != 2 && waveformatex.wFormatTag != 3)
				{
					throw new InvalidOperationException(SR.GetString("SoundAPIFormatNotSupported"));
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SoundPlayer.UnsafeNativeMethods.mmioClose(intPtr, 0);
				}
			}
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0003DF5C File Offset: 0x0003CF5C
		private static void ValidateSoundData(byte[] data)
		{
			short num = -1;
			bool flag = false;
			if (data.Length < 12)
			{
				throw new InvalidOperationException(SR.GetString("SoundAPIInvalidWaveHeader"));
			}
			if (data[0] != 82 || data[1] != 73 || data[2] != 70 || data[3] != 70)
			{
				throw new InvalidOperationException(SR.GetString("SoundAPIInvalidWaveHeader"));
			}
			if (data[8] != 87 || data[9] != 65 || data[10] != 86 || data[11] != 69)
			{
				throw new InvalidOperationException(SR.GetString("SoundAPIInvalidWaveHeader"));
			}
			int num2 = 12;
			int num3 = data.Length;
			while (!flag && num2 < num3 - 8)
			{
				if (data[num2] == 102 && data[num2 + 1] == 109 && data[num2 + 2] == 116 && data[num2 + 3] == 32)
				{
					flag = true;
					int num4 = SoundPlayer.BytesToInt(data[num2 + 7], data[num2 + 6], data[num2 + 5], data[num2 + 4]);
					int num5 = 16;
					if (num4 != num5)
					{
						int num6 = 18;
						if (num3 < num2 + 8 + num6 - 1)
						{
							throw new InvalidOperationException(SR.GetString("SoundAPIInvalidWaveHeader"));
						}
						short num7 = SoundPlayer.BytesToInt16(data[num2 + 8 + num6 - 1], data[num2 + 8 + num6 - 2]);
						if ((int)num7 + num6 != num4)
						{
							throw new InvalidOperationException(SR.GetString("SoundAPIInvalidWaveHeader"));
						}
					}
					if (num3 < num2 + 9)
					{
						throw new InvalidOperationException(SR.GetString("SoundAPIInvalidWaveHeader"));
					}
					num = SoundPlayer.BytesToInt16(data[num2 + 9], data[num2 + 8]);
					num2 += num4 + 8;
				}
				else
				{
					num2 += 8 + SoundPlayer.BytesToInt(data[num2 + 7], data[num2 + 6], data[num2 + 5], data[num2 + 4]);
				}
			}
			if (!flag)
			{
				throw new InvalidOperationException(SR.GetString("SoundAPIInvalidWaveHeader"));
			}
			if (num != 1 && num != 2 && num != 3)
			{
				throw new InvalidOperationException(SR.GetString("SoundAPIFormatNotSupported"));
			}
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0003E120 File Offset: 0x0003D120
		private static short BytesToInt16(byte ch0, byte ch1)
		{
			int num = (int)ch1 | ((int)ch0 << 8);
			return (short)num;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0003E137 File Offset: 0x0003D137
		private static int BytesToInt(byte ch0, byte ch1, byte ch2, byte ch3)
		{
			return SoundPlayer.mmioFOURCC((char)ch3, (char)ch2, (char)ch1, (char)ch0);
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0003E144 File Offset: 0x0003D144
		private static int mmioFOURCC(char ch0, char ch1, char ch2, char ch3)
		{
			int num = 0;
			num |= (int)ch0;
			num |= (int)((int)ch1 << 8);
			num |= (int)((int)ch2 << 16);
			return num | (int)((int)ch3 << 24);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0003E16C File Offset: 0x0003D16C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (!string.IsNullOrEmpty(this.soundLocation))
			{
				info.AddValue("SoundLocation", this.soundLocation);
			}
			if (this.stream != null)
			{
				info.AddValue("Stream", this.stream);
			}
			info.AddValue("LoadTimeout", this.loadTimeout);
		}

		// Token: 0x04001084 RID: 4228
		private const int blockSize = 1024;

		// Token: 0x04001085 RID: 4229
		private const int defaultLoadTimeout = 10000;

		// Token: 0x04001086 RID: 4230
		private Uri uri;

		// Token: 0x04001087 RID: 4231
		private string soundLocation = string.Empty;

		// Token: 0x04001088 RID: 4232
		private int loadTimeout = 10000;

		// Token: 0x04001089 RID: 4233
		private object tag;

		// Token: 0x0400108A RID: 4234
		private ManualResetEvent semaphore = new ManualResetEvent(true);

		// Token: 0x0400108B RID: 4235
		private Thread copyThread;

		// Token: 0x0400108C RID: 4236
		private int currentPos;

		// Token: 0x0400108D RID: 4237
		private Stream stream;

		// Token: 0x0400108E RID: 4238
		private bool isLoadCompleted;

		// Token: 0x0400108F RID: 4239
		private Exception lastLoadException;

		// Token: 0x04001090 RID: 4240
		private bool doesLoadAppearSynchronous;

		// Token: 0x04001091 RID: 4241
		private byte[] streamData;

		// Token: 0x04001092 RID: 4242
		private AsyncOperation asyncOperation;

		// Token: 0x04001093 RID: 4243
		private readonly SendOrPostCallback loadAsyncOperationCompleted;

		// Token: 0x04001094 RID: 4244
		private static readonly object EventLoadCompleted = new object();

		// Token: 0x04001095 RID: 4245
		private static readonly object EventSoundLocationChanged = new object();

		// Token: 0x04001096 RID: 4246
		private static readonly object EventStreamChanged = new object();

		// Token: 0x0200021A RID: 538
		private class IntSecurity
		{
			// Token: 0x0600124E RID: 4686 RVA: 0x0003E1E1 File Offset: 0x0003D1E1
			private IntSecurity()
			{
			}

			// Token: 0x170003A7 RID: 935
			// (get) Token: 0x0600124F RID: 4687 RVA: 0x0003E1E9 File Offset: 0x0003D1E9
			internal static CodeAccessPermission SafeSubWindows
			{
				get
				{
					if (SoundPlayer.IntSecurity.safeSubWindows == null)
					{
						SoundPlayer.IntSecurity.safeSubWindows = new UIPermission(UIPermissionWindow.SafeSubWindows);
					}
					return SoundPlayer.IntSecurity.safeSubWindows;
				}
			}

			// Token: 0x04001097 RID: 4247
			private static CodeAccessPermission safeSubWindows;
		}

		// Token: 0x0200021B RID: 539
		private class NativeMethods
		{
			// Token: 0x06001250 RID: 4688 RVA: 0x0003E202 File Offset: 0x0003D202
			private NativeMethods()
			{
			}

			// Token: 0x04001098 RID: 4248
			internal const int WAVE_FORMAT_PCM = 1;

			// Token: 0x04001099 RID: 4249
			internal const int WAVE_FORMAT_ADPCM = 2;

			// Token: 0x0400109A RID: 4250
			internal const int WAVE_FORMAT_IEEE_FLOAT = 3;

			// Token: 0x0400109B RID: 4251
			internal const int MMIO_READ = 0;

			// Token: 0x0400109C RID: 4252
			internal const int MMIO_ALLOCBUF = 65536;

			// Token: 0x0400109D RID: 4253
			internal const int MMIO_FINDRIFF = 32;

			// Token: 0x0400109E RID: 4254
			internal const int SND_SYNC = 0;

			// Token: 0x0400109F RID: 4255
			internal const int SND_ASYNC = 1;

			// Token: 0x040010A0 RID: 4256
			internal const int SND_NODEFAULT = 2;

			// Token: 0x040010A1 RID: 4257
			internal const int SND_MEMORY = 4;

			// Token: 0x040010A2 RID: 4258
			internal const int SND_LOOP = 8;

			// Token: 0x040010A3 RID: 4259
			internal const int SND_PURGE = 64;

			// Token: 0x040010A4 RID: 4260
			internal const int SND_FILENAME = 131072;

			// Token: 0x040010A5 RID: 4261
			internal const int SND_NOSTOP = 16;

			// Token: 0x0200021C RID: 540
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
			internal class MMCKINFO
			{
				// Token: 0x040010A6 RID: 4262
				internal int ckID;

				// Token: 0x040010A7 RID: 4263
				internal int cksize;

				// Token: 0x040010A8 RID: 4264
				internal int fccType;

				// Token: 0x040010A9 RID: 4265
				internal int dwDataOffset;

				// Token: 0x040010AA RID: 4266
				internal int dwFlags;
			}

			// Token: 0x0200021D RID: 541
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
			internal class WAVEFORMATEX
			{
				// Token: 0x040010AB RID: 4267
				internal short wFormatTag;

				// Token: 0x040010AC RID: 4268
				internal short nChannels;

				// Token: 0x040010AD RID: 4269
				internal int nSamplesPerSec;

				// Token: 0x040010AE RID: 4270
				internal int nAvgBytesPerSec;

				// Token: 0x040010AF RID: 4271
				internal short nBlockAlign;

				// Token: 0x040010B0 RID: 4272
				internal short wBitsPerSample;

				// Token: 0x040010B1 RID: 4273
				internal short cbSize;
			}
		}

		// Token: 0x0200021E RID: 542
		private class UnsafeNativeMethods
		{
			// Token: 0x06001253 RID: 4691 RVA: 0x0003E21A File Offset: 0x0003D21A
			private UnsafeNativeMethods()
			{
			}

			// Token: 0x06001254 RID: 4692
			[DllImport("winmm.dll", CharSet = CharSet.Auto)]
			internal static extern bool PlaySound([MarshalAs(UnmanagedType.LPWStr)] string soundName, IntPtr hmod, int soundFlags);

			// Token: 0x06001255 RID: 4693
			[DllImport("winmm.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			internal static extern bool PlaySound(byte[] soundName, IntPtr hmod, int soundFlags);

			// Token: 0x06001256 RID: 4694
			[DllImport("winmm.dll", CharSet = CharSet.Auto)]
			internal static extern IntPtr mmioOpen(string fileName, IntPtr not_used, int flags);

			// Token: 0x06001257 RID: 4695
			[DllImport("winmm.dll", CharSet = CharSet.Auto)]
			internal static extern int mmioAscend(IntPtr hMIO, SoundPlayer.NativeMethods.MMCKINFO lpck, int flags);

			// Token: 0x06001258 RID: 4696
			[DllImport("winmm.dll", CharSet = CharSet.Auto)]
			internal static extern int mmioDescend(IntPtr hMIO, [MarshalAs(UnmanagedType.LPStruct)] SoundPlayer.NativeMethods.MMCKINFO lpck, [MarshalAs(UnmanagedType.LPStruct)] SoundPlayer.NativeMethods.MMCKINFO lcpkParent, int flags);

			// Token: 0x06001259 RID: 4697
			[DllImport("winmm.dll", CharSet = CharSet.Auto)]
			internal static extern int mmioRead(IntPtr hMIO, [MarshalAs(UnmanagedType.LPArray)] byte[] wf, int cch);

			// Token: 0x0600125A RID: 4698
			[DllImport("winmm.dll", CharSet = CharSet.Auto)]
			internal static extern int mmioClose(IntPtr hMIO, int flags);
		}
	}
}
