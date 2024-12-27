using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.Configuration
{
	// Token: 0x02000239 RID: 569
	[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
	internal class RemoteWebConfigurationHostStream : Stream
	{
		// Token: 0x06001E69 RID: 7785 RVA: 0x00088C5C File Offset: 0x00087C5C
		internal RemoteWebConfigurationHostStream(bool streamForWrite, string serverName, string streamName, string templateStreamName, string username, string domain, string password, WindowsIdentity identity)
		{
			this._Server = serverName;
			this._FileName = streamName;
			this._TemplateFileName = templateStreamName;
			this._Username = username;
			this._Domain = domain;
			this._Password = password;
			this._Identity = identity;
			this._streamForWrite = streamForWrite;
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x00088CAC File Offset: 0x00087CAC
		private void Init()
		{
			if (this._MemoryStream != null)
			{
				return;
			}
			byte[] array = null;
			WindowsImpersonationContext windowsImpersonationContext = null;
			try
			{
				if (this._Identity != null)
				{
					windowsImpersonationContext = this._Identity.Impersonate();
				}
				try
				{
					IRemoteWebConfigurationHostServer remoteWebConfigurationHostServer = RemoteWebConfigurationHost.CreateRemoteObject(this._Server, this._Username, this._Domain, this._Password);
					try
					{
						array = remoteWebConfigurationHostServer.GetData(this._FileName, this._streamForWrite, out this._ReadTime);
					}
					finally
					{
						while (Marshal.ReleaseComObject(remoteWebConfigurationHostServer) > 0)
						{
						}
					}
				}
				catch
				{
					throw;
				}
				finally
				{
					if (windowsImpersonationContext != null)
					{
						windowsImpersonationContext.Undo();
					}
				}
			}
			catch
			{
				throw;
			}
			if (array == null || array.Length < 1)
			{
				this._MemoryStream = new MemoryStream();
				return;
			}
			this._MemoryStream = new MemoryStream(array.Length);
			this._MemoryStream.Write(array, 0, array.Length);
			this._MemoryStream.Position = 0L;
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x00088DA8 File Offset: 0x00087DA8
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x00088DAB File Offset: 0x00087DAB
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x00088DAE File Offset: 0x00087DAE
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x00088DB1 File Offset: 0x00087DB1
		public override long Length
		{
			get
			{
				this.Init();
				return this._MemoryStream.Length;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x00088DC4 File Offset: 0x00087DC4
		// (set) Token: 0x06001E70 RID: 7792 RVA: 0x00088DD7 File Offset: 0x00087DD7
		public override long Position
		{
			get
			{
				this.Init();
				return this._MemoryStream.Position;
			}
			set
			{
				this.Init();
				this._MemoryStream.Position = value;
			}
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x00088DEB File Offset: 0x00087DEB
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.Init();
			return this._MemoryStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x00088E08 File Offset: 0x00087E08
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this._IsDirty = true;
			this.Init();
			if ((long)(offset + count) > this._MemoryStream.Length)
			{
				this._MemoryStream.SetLength((long)(offset + count));
			}
			return this._MemoryStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x00088E54 File Offset: 0x00087E54
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._MemoryStream != null)
				{
					this.Flush();
					this._MemoryStream.Close();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x00088E98 File Offset: 0x00087E98
		public override ObjRef CreateObjRef(Type requestedType)
		{
			throw new RemotingException();
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x00088E9F File Offset: 0x00087E9F
		public override int EndRead(IAsyncResult asyncResult)
		{
			this.Init();
			return this._MemoryStream.EndRead(asyncResult);
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x00088EB3 File Offset: 0x00087EB3
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.Init();
			this._MemoryStream.EndWrite(asyncResult);
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x00088EC7 File Offset: 0x00087EC7
		public override void Flush()
		{
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x00088ECC File Offset: 0x00087ECC
		internal void FlushForWriteCompleted()
		{
			if (this._IsDirty && this._MemoryStream != null)
			{
				WindowsImpersonationContext windowsImpersonationContext = null;
				try
				{
					if (this._Identity != null)
					{
						windowsImpersonationContext = this._Identity.Impersonate();
					}
					try
					{
						IRemoteWebConfigurationHostServer remoteWebConfigurationHostServer = RemoteWebConfigurationHost.CreateRemoteObject(this._Server, this._Username, this._Domain, this._Password);
						try
						{
							remoteWebConfigurationHostServer.WriteData(this._FileName, this._TemplateFileName, this._MemoryStream.ToArray(), ref this._ReadTime);
						}
						catch
						{
							throw;
						}
						finally
						{
							while (Marshal.ReleaseComObject(remoteWebConfigurationHostServer) > 0)
							{
							}
						}
					}
					catch
					{
						throw;
					}
					finally
					{
						if (windowsImpersonationContext != null)
						{
							windowsImpersonationContext.Undo();
						}
					}
				}
				catch
				{
					throw;
				}
				this._MemoryStream.Flush();
				this._IsDirty = false;
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x00088FC0 File Offset: 0x00087FC0
		public override object InitializeLifetimeService()
		{
			this.Init();
			return this._MemoryStream.InitializeLifetimeService();
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x00088FD3 File Offset: 0x00087FD3
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.Init();
			return this._MemoryStream.Read(buffer, offset, count);
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x00088FE9 File Offset: 0x00087FE9
		public override int ReadByte()
		{
			this.Init();
			return this._MemoryStream.ReadByte();
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x00088FFC File Offset: 0x00087FFC
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.Init();
			return this._MemoryStream.Seek(offset, origin);
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x00089011 File Offset: 0x00088011
		public override void SetLength(long val)
		{
			this._IsDirty = true;
			this.Init();
			this._MemoryStream.SetLength(val);
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0008902C File Offset: 0x0008802C
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._IsDirty = true;
			this.Init();
			if ((long)(offset + count) > this._MemoryStream.Length)
			{
				this._MemoryStream.SetLength((long)(offset + count));
			}
			this._MemoryStream.Write(buffer, offset, count);
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x00089069 File Offset: 0x00088069
		public override void WriteByte(byte val)
		{
			this._IsDirty = true;
			this.Init();
			this._MemoryStream.WriteByte(val);
		}

		// Token: 0x040019C6 RID: 6598
		private string _FileName;

		// Token: 0x040019C7 RID: 6599
		private string _TemplateFileName;

		// Token: 0x040019C8 RID: 6600
		private string _Server;

		// Token: 0x040019C9 RID: 6601
		private MemoryStream _MemoryStream;

		// Token: 0x040019CA RID: 6602
		private bool _IsDirty;

		// Token: 0x040019CB RID: 6603
		private long _ReadTime;

		// Token: 0x040019CC RID: 6604
		private WindowsIdentity _Identity;

		// Token: 0x040019CD RID: 6605
		private string _Username;

		// Token: 0x040019CE RID: 6606
		private string _Domain;

		// Token: 0x040019CF RID: 6607
		private string _Password;

		// Token: 0x040019D0 RID: 6608
		private bool _streamForWrite;
	}
}
