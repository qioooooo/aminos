using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020004DE RID: 1246
	internal class FtpDataStream : Stream, ICloseEx
	{
		// Token: 0x060026BE RID: 9918 RVA: 0x0009F8F0 File Offset: 0x0009E8F0
		internal FtpDataStream(NetworkStream networkStream, FtpWebRequest request, TriState writeOnly)
		{
			this.m_Readable = true;
			this.m_Writeable = true;
			if (writeOnly == TriState.True)
			{
				this.m_Readable = false;
			}
			else if (writeOnly == TriState.False)
			{
				this.m_Writeable = false;
			}
			this.m_NetworkStream = networkStream;
			this.m_Request = request;
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x0009F92C File Offset: 0x0009E92C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					((ICloseEx)this).CloseEx(CloseExState.Normal);
				}
				else
				{
					((ICloseEx)this).CloseEx(CloseExState.Abort | CloseExState.Silent);
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x0009F968 File Offset: 0x0009E968
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			lock (this)
			{
				if (this.m_Closing)
				{
					return;
				}
				this.m_Closing = true;
				this.m_Writeable = false;
				this.m_Readable = false;
			}
			try
			{
				try
				{
					if ((closeState & CloseExState.Abort) == CloseExState.Normal)
					{
						this.m_NetworkStream.Close(-1);
					}
					else
					{
						this.m_NetworkStream.Close(0);
					}
				}
				finally
				{
					this.m_Request.DataStreamClosed(closeState);
				}
			}
			catch (Exception ex)
			{
				bool flag = true;
				WebException ex2 = ex as WebException;
				if (ex2 != null)
				{
					FtpWebResponse ftpWebResponse = ex2.Response as FtpWebResponse;
					if (ftpWebResponse != null && !this.m_IsFullyRead && ftpWebResponse.StatusCode == FtpStatusCode.ConnectionClosed)
					{
						flag = false;
					}
				}
				if (flag && (closeState & CloseExState.Silent) == CloseExState.Normal)
				{
					throw;
				}
			}
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x0009FA44 File Offset: 0x0009EA44
		private void CheckError()
		{
			if (this.m_Request.Aborted)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x0009FA65 File Offset: 0x0009EA65
		public override bool CanRead
		{
			get
			{
				return this.m_Readable;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x060026C3 RID: 9923 RVA: 0x0009FA6D File Offset: 0x0009EA6D
		public override bool CanSeek
		{
			get
			{
				return this.m_NetworkStream.CanSeek;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x060026C4 RID: 9924 RVA: 0x0009FA7A File Offset: 0x0009EA7A
		public override bool CanWrite
		{
			get
			{
				return this.m_Writeable;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x060026C5 RID: 9925 RVA: 0x0009FA82 File Offset: 0x0009EA82
		public override long Length
		{
			get
			{
				return this.m_NetworkStream.Length;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060026C6 RID: 9926 RVA: 0x0009FA8F File Offset: 0x0009EA8F
		// (set) Token: 0x060026C7 RID: 9927 RVA: 0x0009FA9C File Offset: 0x0009EA9C
		public override long Position
		{
			get
			{
				return this.m_NetworkStream.Position;
			}
			set
			{
				this.m_NetworkStream.Position = value;
			}
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x0009FAAC File Offset: 0x0009EAAC
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.CheckError();
			long num;
			try
			{
				num = this.m_NetworkStream.Seek(offset, origin);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return num;
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x0009FAEC File Offset: 0x0009EAEC
		public override int Read(byte[] buffer, int offset, int size)
		{
			this.CheckError();
			int num;
			try
			{
				num = this.m_NetworkStream.Read(buffer, offset, size);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			if (num == 0)
			{
				this.m_IsFullyRead = true;
				this.Close();
			}
			return num;
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x0009FB3C File Offset: 0x0009EB3C
		public override void Write(byte[] buffer, int offset, int size)
		{
			this.CheckError();
			try
			{
				this.m_NetworkStream.Write(buffer, offset, size);
			}
			catch
			{
				this.CheckError();
				throw;
			}
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0009FB78 File Offset: 0x0009EB78
		private void AsyncReadCallback(IAsyncResult ar)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)ar.AsyncState;
			try
			{
				try
				{
					int num = this.m_NetworkStream.EndRead(ar);
					if (num == 0)
					{
						this.m_IsFullyRead = true;
						this.Close();
					}
					lazyAsyncResult.InvokeCallback(num);
				}
				catch (Exception ex)
				{
					if (!lazyAsyncResult.IsCompleted)
					{
						lazyAsyncResult.InvokeCallback(ex);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x0009FBF0 File Offset: 0x0009EBF0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			this.CheckError();
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this, state, callback);
			try
			{
				this.m_NetworkStream.BeginRead(buffer, offset, size, new AsyncCallback(this.AsyncReadCallback), lazyAsyncResult);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return lazyAsyncResult;
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x0009FC48 File Offset: 0x0009EC48
		public override int EndRead(IAsyncResult ar)
		{
			int num;
			try
			{
				object obj = ((LazyAsyncResult)ar).InternalWaitForCompletion();
				if (obj is Exception)
				{
					throw (Exception)obj;
				}
				num = (int)obj;
			}
			finally
			{
				this.CheckError();
			}
			return num;
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x0009FC94 File Offset: 0x0009EC94
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			this.CheckError();
			IAsyncResult asyncResult;
			try
			{
				asyncResult = this.m_NetworkStream.BeginWrite(buffer, offset, size, callback, state);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return asyncResult;
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x0009FCD8 File Offset: 0x0009ECD8
		public override void EndWrite(IAsyncResult asyncResult)
		{
			try
			{
				this.m_NetworkStream.EndWrite(asyncResult);
			}
			finally
			{
				this.CheckError();
			}
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x0009FD0C File Offset: 0x0009ED0C
		public override void Flush()
		{
			this.m_NetworkStream.Flush();
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x0009FD19 File Offset: 0x0009ED19
		public override void SetLength(long value)
		{
			this.m_NetworkStream.SetLength(value);
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060026D2 RID: 9938 RVA: 0x0009FD27 File Offset: 0x0009ED27
		public override bool CanTimeout
		{
			get
			{
				return this.m_NetworkStream.CanTimeout;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060026D3 RID: 9939 RVA: 0x0009FD34 File Offset: 0x0009ED34
		// (set) Token: 0x060026D4 RID: 9940 RVA: 0x0009FD41 File Offset: 0x0009ED41
		public override int ReadTimeout
		{
			get
			{
				return this.m_NetworkStream.ReadTimeout;
			}
			set
			{
				this.m_NetworkStream.ReadTimeout = value;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x0009FD4F File Offset: 0x0009ED4F
		// (set) Token: 0x060026D6 RID: 9942 RVA: 0x0009FD5C File Offset: 0x0009ED5C
		public override int WriteTimeout
		{
			get
			{
				return this.m_NetworkStream.WriteTimeout;
			}
			set
			{
				this.m_NetworkStream.WriteTimeout = value;
			}
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x0009FD6A File Offset: 0x0009ED6A
		internal void SetSocketTimeoutOption(SocketShutdown mode, int timeout, bool silent)
		{
			this.m_NetworkStream.SetSocketTimeoutOption(mode, timeout, silent);
		}

		// Token: 0x04002653 RID: 9811
		private FtpWebRequest m_Request;

		// Token: 0x04002654 RID: 9812
		private NetworkStream m_NetworkStream;

		// Token: 0x04002655 RID: 9813
		private bool m_Writeable;

		// Token: 0x04002656 RID: 9814
		private bool m_Readable;

		// Token: 0x04002657 RID: 9815
		private bool m_IsFullyRead;

		// Token: 0x04002658 RID: 9816
		private bool m_Closing;
	}
}
