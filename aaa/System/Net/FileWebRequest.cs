using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003B1 RID: 945
	[Serializable]
	public class FileWebRequest : WebRequest, ISerializable
	{
		// Token: 0x06001D9D RID: 7581 RVA: 0x000707AC File Offset: 0x0006F7AC
		internal FileWebRequest(Uri uri)
		{
			if (uri.Scheme != Uri.UriSchemeFile)
			{
				throw new ArgumentOutOfRangeException("uri");
			}
			this.m_uri = uri;
			this.m_fileAccess = FileAccess.Read;
			this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.FileWebRequest);
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x00070808 File Offset: 0x0006F808
		[Obsolete("Serialization is obsoleted for this type. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected FileWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			this.m_headers = (WebHeaderCollection)serializationInfo.GetValue("headers", typeof(WebHeaderCollection));
			this.m_proxy = (IWebProxy)serializationInfo.GetValue("proxy", typeof(IWebProxy));
			this.m_uri = (Uri)serializationInfo.GetValue("uri", typeof(Uri));
			this.m_connectionGroupName = serializationInfo.GetString("connectionGroupName");
			this.m_method = serializationInfo.GetString("method");
			this.m_contentLength = serializationInfo.GetInt64("contentLength");
			this.m_timeout = serializationInfo.GetInt32("timeout");
			this.m_fileAccess = (FileAccess)serializationInfo.GetInt32("fileAccess");
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000708E8 File Offset: 0x0006F8E8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000708F4 File Offset: 0x0006F8F4
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("headers", this.m_headers, typeof(WebHeaderCollection));
			serializationInfo.AddValue("proxy", this.m_proxy, typeof(IWebProxy));
			serializationInfo.AddValue("uri", this.m_uri, typeof(Uri));
			serializationInfo.AddValue("connectionGroupName", this.m_connectionGroupName);
			serializationInfo.AddValue("method", this.m_method);
			serializationInfo.AddValue("contentLength", this.m_contentLength);
			serializationInfo.AddValue("timeout", this.m_timeout);
			serializationInfo.AddValue("fileAccess", this.m_fileAccess);
			serializationInfo.AddValue("preauthenticate", false);
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x000709C0 File Offset: 0x0006F9C0
		internal bool Aborted
		{
			get
			{
				return this.m_Aborted != 0;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001DA2 RID: 7586 RVA: 0x000709CE File Offset: 0x0006F9CE
		// (set) Token: 0x06001DA3 RID: 7587 RVA: 0x000709D6 File Offset: 0x0006F9D6
		public override string ConnectionGroupName
		{
			get
			{
				return this.m_connectionGroupName;
			}
			set
			{
				this.m_connectionGroupName = value;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001DA4 RID: 7588 RVA: 0x000709DF File Offset: 0x0006F9DF
		// (set) Token: 0x06001DA5 RID: 7589 RVA: 0x000709E7 File Offset: 0x0006F9E7
		public override long ContentLength
		{
			get
			{
				return this.m_contentLength;
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentException(SR.GetString("net_clsmall"), "value");
				}
				this.m_contentLength = value;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001DA6 RID: 7590 RVA: 0x00070A0A File Offset: 0x0006FA0A
		// (set) Token: 0x06001DA7 RID: 7591 RVA: 0x00070A1C File Offset: 0x0006FA1C
		public override string ContentType
		{
			get
			{
				return this.m_headers["Content-Type"];
			}
			set
			{
				this.m_headers["Content-Type"] = value;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x00070A2F File Offset: 0x0006FA2F
		// (set) Token: 0x06001DA9 RID: 7593 RVA: 0x00070A37 File Offset: 0x0006FA37
		public override ICredentials Credentials
		{
			get
			{
				return this.m_credentials;
			}
			set
			{
				this.m_credentials = value;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001DAA RID: 7594 RVA: 0x00070A40 File Offset: 0x0006FA40
		public override WebHeaderCollection Headers
		{
			get
			{
				return this.m_headers;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001DAB RID: 7595 RVA: 0x00070A48 File Offset: 0x0006FA48
		// (set) Token: 0x06001DAC RID: 7596 RVA: 0x00070A50 File Offset: 0x0006FA50
		public override string Method
		{
			get
			{
				return this.m_method;
			}
			set
			{
				if (ValidationHelper.IsBlankString(value))
				{
					throw new ArgumentException(SR.GetString("net_badmethod"), "value");
				}
				this.m_method = value;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001DAD RID: 7597 RVA: 0x00070A76 File Offset: 0x0006FA76
		// (set) Token: 0x06001DAE RID: 7598 RVA: 0x00070A7E File Offset: 0x0006FA7E
		public override bool PreAuthenticate
		{
			get
			{
				return this.m_preauthenticate;
			}
			set
			{
				this.m_preauthenticate = true;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001DAF RID: 7599 RVA: 0x00070A87 File Offset: 0x0006FA87
		// (set) Token: 0x06001DB0 RID: 7600 RVA: 0x00070A8F File Offset: 0x0006FA8F
		public override IWebProxy Proxy
		{
			get
			{
				return this.m_proxy;
			}
			set
			{
				this.m_proxy = value;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001DB1 RID: 7601 RVA: 0x00070A98 File Offset: 0x0006FA98
		// (set) Token: 0x06001DB2 RID: 7602 RVA: 0x00070AA0 File Offset: 0x0006FAA0
		public override int Timeout
		{
			get
			{
				return this.m_timeout;
			}
			set
			{
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_ge_zero"));
				}
				this.m_timeout = value;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x00070AC1 File Offset: 0x0006FAC1
		public override Uri RequestUri
		{
			get
			{
				return this.m_uri;
			}
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x00070ACC File Offset: 0x0006FACC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			try
			{
				if (this.Aborted)
				{
					throw ExceptionHelper.RequestAbortedException;
				}
				if (!this.CanGetRequestStream())
				{
					Exception ex = new ProtocolViolationException(SR.GetString("net_nouploadonget"));
					throw ex;
				}
				if (this.m_response != null)
				{
					Exception ex2 = new InvalidOperationException(SR.GetString("net_reqsubmitted"));
					throw ex2;
				}
				lock (this)
				{
					if (this.m_writePending)
					{
						Exception ex3 = new InvalidOperationException(SR.GetString("net_repcall"));
						throw ex3;
					}
					this.m_writePending = true;
				}
				this.m_ReadAResult = new LazyAsyncResult(this, state, callback);
				ThreadPool.QueueUserWorkItem(FileWebRequest.s_GetRequestStreamCallback, this.m_ReadAResult);
			}
			catch (Exception ex4)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "BeginGetRequestStream", ex4);
				}
				throw;
			}
			return this.m_ReadAResult;
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x00070BB0 File Offset: 0x0006FBB0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			try
			{
				if (this.Aborted)
				{
					throw ExceptionHelper.RequestAbortedException;
				}
				lock (this)
				{
					if (this.m_readPending)
					{
						Exception ex = new InvalidOperationException(SR.GetString("net_repcall"));
						throw ex;
					}
					this.m_readPending = true;
				}
				this.m_WriteAResult = new LazyAsyncResult(this, state, callback);
				ThreadPool.QueueUserWorkItem(FileWebRequest.s_GetResponseCallback, this.m_WriteAResult);
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "BeginGetResponse", ex2);
				}
				throw;
			}
			return this.m_WriteAResult;
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x00070C5C File Offset: 0x0006FC5C
		private bool CanGetRequestStream()
		{
			return !KnownHttpVerb.Parse(this.m_method).ContentBodyNotAllowed;
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x00070C74 File Offset: 0x0006FC74
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			Stream stream;
			try
			{
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				if (asyncResult == null || lazyAsyncResult == null)
				{
					Exception ex = ((asyncResult == null) ? new ArgumentNullException("asyncResult") : new ArgumentException(SR.GetString("InvalidAsyncResult"), "asyncResult"));
					throw ex;
				}
				object obj = lazyAsyncResult.InternalWaitForCompletion();
				if (obj is Exception)
				{
					throw (Exception)obj;
				}
				stream = (Stream)obj;
				this.m_writePending = false;
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetRequestStream", ex2);
				}
				throw;
			}
			return stream;
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x00070D08 File Offset: 0x0006FD08
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			WebResponse webResponse;
			try
			{
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				if (asyncResult == null || lazyAsyncResult == null)
				{
					Exception ex = ((asyncResult == null) ? new ArgumentNullException("asyncResult") : new ArgumentException(SR.GetString("InvalidAsyncResult"), "asyncResult"));
					throw ex;
				}
				object obj = lazyAsyncResult.InternalWaitForCompletion();
				if (obj is Exception)
				{
					throw (Exception)obj;
				}
				webResponse = (WebResponse)obj;
				this.m_readPending = false;
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetResponse", ex2);
				}
				throw;
			}
			return webResponse;
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x00070D9C File Offset: 0x0006FD9C
		public override Stream GetRequestStream()
		{
			IAsyncResult asyncResult;
			try
			{
				asyncResult = this.BeginGetRequestStream(null, null);
				if (this.Timeout != -1 && !asyncResult.IsCompleted && (!asyncResult.AsyncWaitHandle.WaitOne(this.Timeout, false) || !asyncResult.IsCompleted))
				{
					if (this.m_stream != null)
					{
						this.m_stream.Close();
					}
					Exception ex = new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
					throw ex;
				}
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "GetRequestStream", ex2);
				}
				throw;
			}
			return this.EndGetRequestStream(asyncResult);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x00070E38 File Offset: 0x0006FE38
		public override WebResponse GetResponse()
		{
			this.m_syncHint = true;
			IAsyncResult asyncResult;
			try
			{
				asyncResult = this.BeginGetResponse(null, null);
				if (this.Timeout != -1 && !asyncResult.IsCompleted && (!asyncResult.AsyncWaitHandle.WaitOne(this.Timeout, false) || !asyncResult.IsCompleted))
				{
					if (this.m_response != null)
					{
						this.m_response.Close();
					}
					Exception ex = new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
					throw ex;
				}
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "GetResponse", ex2);
				}
				throw;
			}
			return this.EndGetResponse(asyncResult);
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x00070EDC File Offset: 0x0006FEDC
		private static void GetRequestStreamCallback(object state)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)state;
			FileWebRequest fileWebRequest = (FileWebRequest)lazyAsyncResult.AsyncObject;
			try
			{
				if (fileWebRequest.m_stream == null)
				{
					fileWebRequest.m_stream = new FileWebStream(fileWebRequest, fileWebRequest.m_uri.LocalPath, FileMode.Create, FileAccess.Write, FileShare.Read);
					fileWebRequest.m_fileAccess = FileAccess.Write;
					fileWebRequest.m_writing = true;
					lazyAsyncResult.InvokeCallback(fileWebRequest.m_stream);
				}
			}
			catch (Exception ex)
			{
				if (lazyAsyncResult.IsCompleted || NclUtilities.IsFatal(ex))
				{
					throw;
				}
				Exception ex2 = new WebException(ex.Message, ex);
				lazyAsyncResult.InvokeCallback(ex2);
			}
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x00070F74 File Offset: 0x0006FF74
		private static void GetResponseCallback(object state)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)state;
			FileWebRequest fileWebRequest = (FileWebRequest)lazyAsyncResult.AsyncObject;
			if (fileWebRequest.m_writePending || fileWebRequest.m_writing)
			{
				lock (fileWebRequest)
				{
					if (fileWebRequest.m_writePending || fileWebRequest.m_writing)
					{
						fileWebRequest.m_readerEvent = new ManualResetEvent(false);
					}
				}
			}
			if (fileWebRequest.m_readerEvent != null)
			{
				fileWebRequest.m_readerEvent.WaitOne();
			}
			try
			{
				if (fileWebRequest.m_response == null)
				{
					fileWebRequest.m_response = new FileWebResponse(fileWebRequest, fileWebRequest.m_uri, fileWebRequest.m_fileAccess, !fileWebRequest.m_syncHint);
				}
				lazyAsyncResult.InvokeCallback(fileWebRequest.m_response);
			}
			catch (Exception ex)
			{
				if (lazyAsyncResult.IsCompleted || NclUtilities.IsFatal(ex))
				{
					throw;
				}
				Exception ex2 = new WebException(ex.Message, ex);
				lazyAsyncResult.InvokeCallback(ex2);
			}
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x00071064 File Offset: 0x00070064
		internal void UnblockReader()
		{
			lock (this)
			{
				if (this.m_readerEvent != null)
				{
					this.m_readerEvent.Set();
				}
			}
			this.m_writing = false;
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001DBE RID: 7614 RVA: 0x000710B0 File Offset: 0x000700B0
		// (set) Token: 0x06001DBF RID: 7615 RVA: 0x000710B7 File Offset: 0x000700B7
		public override bool UseDefaultCredentials
		{
			get
			{
				throw ExceptionHelper.PropertyNotSupportedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotSupportedException;
			}
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x000710C0 File Offset: 0x000700C0
		public override void Abort()
		{
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled));
			}
			try
			{
				if (Interlocked.Increment(ref this.m_Aborted) == 1)
				{
					LazyAsyncResult readAResult = this.m_ReadAResult;
					LazyAsyncResult writeAResult = this.m_WriteAResult;
					WebException ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
					Stream stream = this.m_stream;
					if (readAResult != null && !readAResult.IsCompleted)
					{
						readAResult.InvokeCallback(ex);
					}
					if (writeAResult != null && !writeAResult.IsCompleted)
					{
						writeAResult.InvokeCallback(ex);
					}
					if (stream != null)
					{
						if (stream is ICloseEx)
						{
							((ICloseEx)stream).CloseEx(CloseExState.Abort);
						}
						else
						{
							stream.Close();
						}
					}
					if (this.m_response != null)
					{
						((ICloseEx)this.m_response).CloseEx(CloseExState.Abort);
					}
				}
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "Abort", ex2);
				}
				throw;
			}
		}

		// Token: 0x04001D8B RID: 7563
		private static WaitCallback s_GetRequestStreamCallback = new WaitCallback(FileWebRequest.GetRequestStreamCallback);

		// Token: 0x04001D8C RID: 7564
		private static WaitCallback s_GetResponseCallback = new WaitCallback(FileWebRequest.GetResponseCallback);

		// Token: 0x04001D8D RID: 7565
		private static ContextCallback s_WrappedGetRequestStreamCallback = new ContextCallback(FileWebRequest.GetRequestStreamCallback);

		// Token: 0x04001D8E RID: 7566
		private static ContextCallback s_WrappedResponseCallback = new ContextCallback(FileWebRequest.GetResponseCallback);

		// Token: 0x04001D8F RID: 7567
		private string m_connectionGroupName;

		// Token: 0x04001D90 RID: 7568
		private long m_contentLength;

		// Token: 0x04001D91 RID: 7569
		private ICredentials m_credentials;

		// Token: 0x04001D92 RID: 7570
		private FileAccess m_fileAccess;

		// Token: 0x04001D93 RID: 7571
		private WebHeaderCollection m_headers;

		// Token: 0x04001D94 RID: 7572
		private string m_method = "GET";

		// Token: 0x04001D95 RID: 7573
		private bool m_preauthenticate;

		// Token: 0x04001D96 RID: 7574
		private IWebProxy m_proxy;

		// Token: 0x04001D97 RID: 7575
		private ManualResetEvent m_readerEvent;

		// Token: 0x04001D98 RID: 7576
		private bool m_readPending;

		// Token: 0x04001D99 RID: 7577
		private WebResponse m_response;

		// Token: 0x04001D9A RID: 7578
		private Stream m_stream;

		// Token: 0x04001D9B RID: 7579
		private bool m_syncHint;

		// Token: 0x04001D9C RID: 7580
		private int m_timeout = 100000;

		// Token: 0x04001D9D RID: 7581
		private Uri m_uri;

		// Token: 0x04001D9E RID: 7582
		private bool m_writePending;

		// Token: 0x04001D9F RID: 7583
		private bool m_writing;

		// Token: 0x04001DA0 RID: 7584
		private LazyAsyncResult m_WriteAResult;

		// Token: 0x04001DA1 RID: 7585
		private LazyAsyncResult m_ReadAResult;

		// Token: 0x04001DA2 RID: 7586
		private int m_Aborted;
	}
}
