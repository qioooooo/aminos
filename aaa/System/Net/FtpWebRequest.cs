using System;
using System.IO;
using System.Net.Cache;
using System.Net.Sockets;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003C0 RID: 960
	public sealed class FtpWebRequest : WebRequest
	{
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001DFA RID: 7674 RVA: 0x00071957 File Offset: 0x00070957
		internal FtpMethodInfo MethodInfo
		{
			get
			{
				return this.m_MethodInfo;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001DFB RID: 7675 RVA: 0x0007195F File Offset: 0x0007095F
		internal static NetworkCredential DefaultNetworkCredential
		{
			get
			{
				return FtpWebRequest.DefaultFtpNetworkCredential;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001DFC RID: 7676 RVA: 0x00071968 File Offset: 0x00070968
		// (set) Token: 0x06001DFD RID: 7677 RVA: 0x00071990 File Offset: 0x00070990
		public new static RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				RequestCachePolicy policy = RequestCacheManager.GetBinding(Uri.UriSchemeFtp).Policy;
				if (policy == null)
				{
					return WebRequest.DefaultCachePolicy;
				}
				return policy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				RequestCacheBinding binding = RequestCacheManager.GetBinding(Uri.UriSchemeFtp);
				RequestCacheManager.SetBinding(Uri.UriSchemeFtp, new RequestCacheBinding(binding.Cache, binding.Validator, value));
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001DFE RID: 7678 RVA: 0x000719CE File Offset: 0x000709CE
		// (set) Token: 0x06001DFF RID: 7679 RVA: 0x000719DC File Offset: 0x000709DC
		public override string Method
		{
			get
			{
				return this.m_MethodInfo.Method;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException(SR.GetString("net_ftp_invalid_method_name"), "value");
				}
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				try
				{
					this.m_MethodInfo = FtpMethodInfo.GetMethodInfo(value);
				}
				catch (ArgumentException)
				{
					throw new ArgumentException(SR.GetString("net_ftp_unsupported_method"), "value");
				}
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001E00 RID: 7680 RVA: 0x00071A54 File Offset: 0x00070A54
		// (set) Token: 0x06001E01 RID: 7681 RVA: 0x00071A5C File Offset: 0x00070A5C
		public string RenameTo
		{
			get
			{
				return this.m_RenameTo;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException(SR.GetString("net_ftp_invalid_renameto"), "value");
				}
				this.m_RenameTo = value;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001E02 RID: 7682 RVA: 0x00071A9A File Offset: 0x00070A9A
		// (set) Token: 0x06001E03 RID: 7683 RVA: 0x00071AA4 File Offset: 0x00070AA4
		public override ICredentials Credentials
		{
			get
			{
				return this.m_AuthInfo;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value is SystemNetworkCredential)
				{
					throw new ArgumentException(SR.GetString("net_ftp_no_defaultcreds"), "value");
				}
				this.m_AuthInfo = value;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001E04 RID: 7684 RVA: 0x00071AFB File Offset: 0x00070AFB
		public override Uri RequestUri
		{
			get
			{
				return this.m_Uri;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001E05 RID: 7685 RVA: 0x00071B03 File Offset: 0x00070B03
		// (set) Token: 0x06001E06 RID: 7686 RVA: 0x00071B0C File Offset: 0x00070B0C
		public override int Timeout
		{
			get
			{
				return this.m_Timeout;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_ge_zero"));
				}
				if (this.m_Timeout != value)
				{
					this.m_Timeout = value;
					this.m_TimerQueue = null;
				}
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001E07 RID: 7687 RVA: 0x00071B60 File Offset: 0x00070B60
		internal int RemainingTimeout
		{
			get
			{
				return this.m_RemainingTimeout;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x00071B68 File Offset: 0x00070B68
		// (set) Token: 0x06001E09 RID: 7689 RVA: 0x00071B70 File Offset: 0x00070B70
		public int ReadWriteTimeout
		{
			get
			{
				return this.m_ReadWriteTimeout;
			}
			set
			{
				if (this.m_GetResponseStarted)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_gt_zero"));
				}
				this.m_ReadWriteTimeout = value;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x00071BA9 File Offset: 0x00070BA9
		// (set) Token: 0x06001E0B RID: 7691 RVA: 0x00071BB1 File Offset: 0x00070BB1
		public long ContentOffset
		{
			get
			{
				return this.m_ContentOffset;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_ContentOffset = value;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x00071BE2 File Offset: 0x00070BE2
		// (set) Token: 0x06001E0D RID: 7693 RVA: 0x00071BEA File Offset: 0x00070BEA
		public override long ContentLength
		{
			get
			{
				return this.m_ContentLength;
			}
			set
			{
				this.m_ContentLength = value;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001E0E RID: 7694 RVA: 0x00071BF3 File Offset: 0x00070BF3
		// (set) Token: 0x06001E0F RID: 7695 RVA: 0x00071C05 File Offset: 0x00070C05
		public override IWebProxy Proxy
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_Proxy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				this.m_ProxyUserSet = true;
				this.m_Proxy = value;
				this.m_ServicePoint = null;
				ServicePoint servicePoint = this.ServicePoint;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001E10 RID: 7696 RVA: 0x00071C45 File Offset: 0x00070C45
		// (set) Token: 0x06001E11 RID: 7697 RVA: 0x00071C4D File Offset: 0x00070C4D
		public override string ConnectionGroupName
		{
			get
			{
				return this.m_ConnectionGroupName;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				this.m_ConnectionGroupName = value;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001E12 RID: 7698 RVA: 0x00071C70 File Offset: 0x00070C70
		public ServicePoint ServicePoint
		{
			get
			{
				if (this.m_ServicePoint == null)
				{
					IWebProxy webProxy = this.m_Proxy;
					if (!this.m_ProxyUserSet)
					{
						webProxy = WebRequest.InternalDefaultWebProxy;
					}
					ServicePoint servicePoint = ServicePointManager.FindServicePoint(this.m_Uri, webProxy);
					lock (this.m_SyncObject)
					{
						if (this.m_ServicePoint == null)
						{
							this.m_ServicePoint = servicePoint;
							this.m_Proxy = webProxy;
						}
					}
				}
				return this.m_ServicePoint;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001E13 RID: 7699 RVA: 0x00071CEC File Offset: 0x00070CEC
		internal bool Aborted
		{
			get
			{
				return this.m_Aborted;
			}
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x00071CF4 File Offset: 0x00070CF4
		internal FtpWebRequest(Uri uri)
		{
			new WebPermission(NetworkAccess.Connect, uri).Demand();
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, ".ctor", uri.ToString());
			}
			if (uri.Scheme != Uri.UriSchemeFtp)
			{
				throw new ArgumentOutOfRangeException("uri");
			}
			this.m_TimerCallback = new TimerThread.Callback(this.TimerCallback);
			this.m_SyncObject = new object();
			NetworkCredential networkCredential = null;
			this.m_Uri = uri;
			this.m_MethodInfo = FtpMethodInfo.GetMethodInfo("RETR");
			if (this.m_Uri.UserInfo != null && this.m_Uri.UserInfo.Length != 0)
			{
				string userInfo = this.m_Uri.UserInfo;
				string text = userInfo;
				string text2 = "";
				int num = userInfo.IndexOf(':');
				if (num != -1)
				{
					text = Uri.UnescapeDataString(userInfo.Substring(0, num));
					num++;
					text2 = Uri.UnescapeDataString(userInfo.Substring(num, userInfo.Length - num));
				}
				networkCredential = new NetworkCredential(text, text2);
			}
			if (networkCredential == null)
			{
				networkCredential = FtpWebRequest.DefaultFtpNetworkCredential;
			}
			this.m_AuthInfo = networkCredential;
			base.SetupCacheProtocol(this.m_Uri);
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x00071E48 File Offset: 0x00070E48
		public override WebResponse GetResponse()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "GetResponse", "");
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "GetResponse", SR.GetString("net_log_method_equal", new object[] { this.m_MethodInfo.Method }));
			}
			try
			{
				this.CheckError();
				if (this.m_FtpWebResponse != null)
				{
					return this.m_FtpWebResponse;
				}
				if (this.m_GetResponseStarted)
				{
					throw new InvalidOperationException(SR.GetString("net_repcall"));
				}
				this.m_GetResponseStarted = true;
				this.m_StartTime = DateTime.UtcNow;
				this.m_RemainingTimeout = this.Timeout;
				ServicePoint servicePoint = this.ServicePoint;
				if (this.Timeout != -1)
				{
					this.m_RemainingTimeout = this.Timeout - (int)(DateTime.UtcNow - this.m_StartTime).TotalMilliseconds;
					if (this.m_RemainingTimeout <= 0)
					{
						throw new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
					}
				}
				if (this.ServicePoint.InternalProxyServicePoint)
				{
					if (this.EnableSsl)
					{
						this.m_GetResponseStarted = false;
						throw new WebException(SR.GetString("net_ftp_proxy_does_not_support_ssl"));
					}
					try
					{
						HttpWebRequest httpWebRequest = this.GetHttpWebRequest();
						if (Logging.On)
						{
							Logging.Associate(Logging.Web, this, httpWebRequest);
						}
						this.m_FtpWebResponse = new FtpWebResponse((HttpWebResponse)httpWebRequest.GetResponse());
						goto IL_0256;
					}
					catch (WebException ex)
					{
						if (ex.Response != null && ex.Response is HttpWebResponse)
						{
							ex = new WebException(ex.Message, null, ex.Status, new FtpWebResponse((HttpWebResponse)ex.Response), ex.InternalStatus);
						}
						this.SetException(ex);
						throw ex;
					}
				}
				FtpWebRequest.RequestStage requestStage = this.FinishRequestStage(FtpWebRequest.RequestStage.RequestStarted);
				if (requestStage >= FtpWebRequest.RequestStage.RequestStarted)
				{
					if (requestStage < FtpWebRequest.RequestStage.ReadReady)
					{
						lock (this.m_SyncObject)
						{
							if (this.m_RequestStage < FtpWebRequest.RequestStage.ReadReady)
							{
								this.m_ReadAsyncResult = new LazyAsyncResult(null, null, null);
							}
						}
						if (this.m_ReadAsyncResult != null)
						{
							this.m_ReadAsyncResult.InternalWaitForCompletion();
						}
						this.CheckError();
					}
				}
				else
				{
					do
					{
						this.SubmitRequest(false);
						if (this.m_MethodInfo.IsUpload)
						{
							this.FinishRequestStage(FtpWebRequest.RequestStage.WriteReady);
						}
						else
						{
							this.FinishRequestStage(FtpWebRequest.RequestStage.ReadReady);
						}
						this.CheckError();
					}
					while (!this.CheckCacheRetrieveOnResponse());
					this.EnsureFtpWebResponse(null);
					this.CheckCacheUpdateOnResponse();
					if (this.m_FtpWebResponse.IsFromCache)
					{
						this.FinishRequestStage(FtpWebRequest.RequestStage.ReleaseConnection);
					}
				}
				IL_0256:;
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "GetResponse", ex2);
				}
				if (this.m_Exception == null)
				{
					if (Logging.On)
					{
						Logging.PrintWarning(Logging.Web, SR.GetString("net_log_unexpected_exception", new object[] { "GetResponse()" }));
					}
					NclUtilities.IsFatal(ex2);
					this.SetException(ex2);
					this.FinishRequestStage(FtpWebRequest.RequestStage.CheckForError);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "GetResponse", "");
				}
			}
			return this.m_FtpWebResponse;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000721A0 File Offset: 0x000711A0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "BeginGetResponse", "");
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "BeginGetResponse", SR.GetString("net_log_method_equal", new object[] { this.m_MethodInfo.Method }));
			}
			ContextAwareResult contextAwareResult;
			try
			{
				if (this.m_FtpWebResponse != null)
				{
					contextAwareResult = new ContextAwareResult(this, state, callback);
					contextAwareResult.InvokeCallback(this.m_FtpWebResponse);
					return contextAwareResult;
				}
				if (this.m_GetResponseStarted)
				{
					throw new InvalidOperationException(SR.GetString("net_repcall"));
				}
				this.m_GetResponseStarted = true;
				this.CheckError();
				if (this.ServicePoint.InternalProxyServicePoint)
				{
					HttpWebRequest httpWebRequest = this.GetHttpWebRequest();
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, httpWebRequest);
					}
					contextAwareResult = (ContextAwareResult)httpWebRequest.BeginGetResponse(callback, state);
				}
				else
				{
					FtpWebRequest.RequestStage requestStage = this.FinishRequestStage(FtpWebRequest.RequestStage.RequestStarted);
					contextAwareResult = new ContextAwareResult(true, true, this, state, callback);
					this.m_ReadAsyncResult = contextAwareResult;
					if (requestStage >= FtpWebRequest.RequestStage.RequestStarted)
					{
						contextAwareResult.StartPostingAsyncOp();
						contextAwareResult.FinishPostingAsyncOp();
						if (requestStage >= FtpWebRequest.RequestStage.ReadReady)
						{
							contextAwareResult = null;
						}
						else
						{
							lock (this.m_SyncObject)
							{
								if (this.m_RequestStage >= FtpWebRequest.RequestStage.ReadReady)
								{
									contextAwareResult = null;
								}
							}
						}
						if (contextAwareResult == null)
						{
							contextAwareResult = (ContextAwareResult)this.m_ReadAsyncResult;
							if (!contextAwareResult.InternalPeekCompleted)
							{
								contextAwareResult.InvokeCallback();
							}
						}
					}
					else
					{
						lock (contextAwareResult.StartPostingAsyncOp())
						{
							this.SubmitRequest(true);
							contextAwareResult.FinishPostingAsyncOp();
						}
						this.FinishRequestStage(FtpWebRequest.RequestStage.CheckForError);
					}
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "BeginGetResponse", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "BeginGetResponse", "");
				}
			}
			return contextAwareResult;
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x000723D0 File Offset: 0x000713D0
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "EndGetResponse", "");
			}
			try
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				if (lazyAsyncResult == null)
				{
					throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (this.HttpProxyMode ? (lazyAsyncResult.AsyncObject != this.GetHttpWebRequest()) : (lazyAsyncResult.AsyncObject != this))
				{
					throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (lazyAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndGetResponse" }));
				}
				if (this.HttpProxyMode)
				{
					try
					{
						this.CheckError();
						if (this.m_FtpWebResponse == null)
						{
							this.m_FtpWebResponse = new FtpWebResponse((HttpWebResponse)this.GetHttpWebRequest().EndGetResponse(asyncResult));
						}
						goto IL_0138;
					}
					catch (WebException ex)
					{
						if (ex.Response != null && ex.Response is HttpWebResponse)
						{
							throw new WebException(ex.Message, null, ex.Status, new FtpWebResponse((HttpWebResponse)ex.Response), ex.InternalStatus);
						}
						throw;
					}
				}
				lazyAsyncResult.InternalWaitForCompletion();
				lazyAsyncResult.EndCalled = true;
				this.CheckError();
				IL_0138:;
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetResponse", ex2);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "EndGetResponse", "");
				}
			}
			return this.m_FtpWebResponse;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x000725A4 File Offset: 0x000715A4
		public override Stream GetRequestStream()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "GetRequestStream", "");
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "GetRequestStream", SR.GetString("net_log_method_equal", new object[] { this.m_MethodInfo.Method }));
			}
			try
			{
				if (this.m_GetRequestStreamStarted)
				{
					throw new InvalidOperationException(SR.GetString("net_repcall"));
				}
				this.m_GetRequestStreamStarted = true;
				if (!this.m_MethodInfo.IsUpload)
				{
					throw new ProtocolViolationException(SR.GetString("net_nouploadonget"));
				}
				this.CheckError();
				this.m_StartTime = DateTime.UtcNow;
				this.m_RemainingTimeout = this.Timeout;
				ServicePoint servicePoint = this.ServicePoint;
				if (this.Timeout != -1)
				{
					this.m_RemainingTimeout = this.Timeout - (int)(DateTime.UtcNow - this.m_StartTime).TotalMilliseconds;
					if (this.m_RemainingTimeout <= 0)
					{
						throw new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
					}
				}
				if (this.ServicePoint.InternalProxyServicePoint)
				{
					HttpWebRequest httpWebRequest = this.GetHttpWebRequest();
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, httpWebRequest);
					}
					this.m_Stream = httpWebRequest.GetRequestStream();
				}
				else
				{
					this.FinishRequestStage(FtpWebRequest.RequestStage.RequestStarted);
					this.SubmitRequest(false);
					this.FinishRequestStage(FtpWebRequest.RequestStage.WriteReady);
					this.CheckError();
				}
				if (this.m_Stream.CanTimeout)
				{
					this.m_Stream.WriteTimeout = this.ReadWriteTimeout;
					this.m_Stream.ReadTimeout = this.ReadWriteTimeout;
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "GetRequestStream", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "GetRequestStream", "");
				}
			}
			return this.m_Stream;
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000727A4 File Offset: 0x000717A4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "BeginGetRequestStream", "");
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "BeginGetRequestStream", SR.GetString("net_log_method_equal", new object[] { this.m_MethodInfo.Method }));
			}
			ContextAwareResult contextAwareResult = null;
			try
			{
				if (this.m_GetRequestStreamStarted)
				{
					throw new InvalidOperationException(SR.GetString("net_repcall"));
				}
				this.m_GetRequestStreamStarted = true;
				if (!this.m_MethodInfo.IsUpload)
				{
					throw new ProtocolViolationException(SR.GetString("net_nouploadonget"));
				}
				this.CheckError();
				if (this.ServicePoint.InternalProxyServicePoint)
				{
					HttpWebRequest httpWebRequest = this.GetHttpWebRequest();
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, httpWebRequest);
					}
					contextAwareResult = (ContextAwareResult)httpWebRequest.BeginGetRequestStream(callback, state);
				}
				else
				{
					this.FinishRequestStage(FtpWebRequest.RequestStage.RequestStarted);
					contextAwareResult = new ContextAwareResult(true, true, this, state, callback);
					lock (contextAwareResult.StartPostingAsyncOp())
					{
						this.m_WriteAsyncResult = contextAwareResult;
						this.SubmitRequest(true);
						contextAwareResult.FinishPostingAsyncOp();
						this.FinishRequestStage(FtpWebRequest.RequestStage.CheckForError);
					}
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "BeginGetRequestStream", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "BeginGetRequestStream", "");
				}
			}
			return contextAwareResult;
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x0007292C File Offset: 0x0007192C
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "EndGetRequestStream", "");
			}
			Stream stream = null;
			try
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				if (lazyAsyncResult == null || (this.HttpProxyMode ? (lazyAsyncResult.AsyncObject != this.GetHttpWebRequest()) : (lazyAsyncResult.AsyncObject != this)))
				{
					throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (lazyAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndGetResponse" }));
				}
				if (this.HttpProxyMode)
				{
					stream = this.GetHttpWebRequest().EndGetRequestStream(asyncResult);
				}
				else
				{
					lazyAsyncResult.InternalWaitForCompletion();
					lazyAsyncResult.EndCalled = true;
					this.CheckError();
					stream = this.m_Stream;
					lazyAsyncResult.EndCalled = true;
				}
				if (stream.CanTimeout)
				{
					stream.WriteTimeout = this.ReadWriteTimeout;
					stream.ReadTimeout = this.ReadWriteTimeout;
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetRequestStream", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "EndGetRequestStream", "");
				}
			}
			return stream;
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x00072A88 File Offset: 0x00071A88
		private void SubmitRequest(bool async)
		{
			try
			{
				this.m_Async = async;
				if (!this.CheckCacheRetrieveBeforeSubmit())
				{
					if (this.m_ConnectionPool == null)
					{
						this.m_ConnectionPool = ConnectionPoolManager.GetConnectionPool(this.ServicePoint, this.GetConnectionGroupLine(), FtpWebRequest.m_CreateConnectionCallback);
					}
					for (;;)
					{
						FtpControlStream ftpControlStream = this.m_Connection;
						if (ftpControlStream == null)
						{
							ftpControlStream = this.QueueOrCreateConnection();
							if (ftpControlStream == null)
							{
								break;
							}
						}
						if (!async && this.Timeout != -1)
						{
							this.m_RemainingTimeout = this.Timeout - (int)(DateTime.UtcNow - this.m_StartTime).TotalMilliseconds;
							if (this.m_RemainingTimeout <= 0)
							{
								goto Block_8;
							}
						}
						ftpControlStream.SetSocketTimeoutOption(SocketShutdown.Both, this.RemainingTimeout, false);
						try
						{
							this.TimedSubmitRequestHelper(async);
						}
						catch (Exception ex)
						{
							if (this.AttemptedRecovery(ex))
							{
								if (!async && this.Timeout != -1)
								{
									this.m_RemainingTimeout = this.Timeout - (int)(DateTime.UtcNow - this.m_StartTime).TotalMilliseconds;
									if (this.m_RemainingTimeout <= 0)
									{
										throw;
									}
								}
								continue;
							}
							throw;
						}
						break;
					}
					return;
					Block_8:
					throw new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
				}
				this.RequestCallback(null);
			}
			catch (WebException ex2)
			{
				IOException ex3 = ex2.InnerException as IOException;
				if (ex3 != null)
				{
					SocketException ex4 = ex3.InnerException as SocketException;
					if (ex4 != null && ex4.ErrorCode == 10060)
					{
						this.SetException(new WebException(SR.GetString("net_timeout"), WebExceptionStatus.Timeout));
					}
				}
				this.SetException(ex2);
			}
			catch (Exception ex5)
			{
				this.SetException(ex5);
			}
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x00072C48 File Offset: 0x00071C48
		private FtpControlStream QueueOrCreateConnection()
		{
			FtpControlStream ftpControlStream = (FtpControlStream)this.m_ConnectionPool.GetConnection(this, this.m_Async ? FtpWebRequest.m_AsyncCallback : null, this.m_Async ? (-1) : this.RemainingTimeout);
			if (ftpControlStream == null)
			{
				return null;
			}
			lock (this.m_SyncObject)
			{
				if (this.m_Aborted)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, "", SR.GetString("net_log_releasing_connection", new object[] { ValidationHelper.HashString(ftpControlStream) }));
					}
					this.m_ConnectionPool.PutConnection(ftpControlStream, this, this.RemainingTimeout);
					this.CheckError();
					throw new InternalException();
				}
				this.m_Connection = ftpControlStream;
				if (Logging.On)
				{
					Logging.Associate(Logging.Web, this, this.m_Connection);
				}
			}
			return ftpControlStream;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x00072D2C File Offset: 0x00071D2C
		private Stream TimedSubmitRequestHelper(bool async)
		{
			if (async)
			{
				if (this.m_RequestCompleteAsyncResult == null)
				{
					this.m_RequestCompleteAsyncResult = new LazyAsyncResult(null, null, null);
				}
				return this.m_Connection.SubmitRequest(this, true, true);
			}
			Stream stream = null;
			bool flag = false;
			TimerThread.Timer timer = this.TimerQueue.CreateTimer(this.m_TimerCallback, null);
			try
			{
				stream = this.m_Connection.SubmitRequest(this, false, true);
			}
			catch (Exception ex)
			{
				if ((!(ex is SocketException) && !(ex is ObjectDisposedException)) || !timer.HasExpired)
				{
					timer.Cancel();
					throw;
				}
				flag = true;
			}
			if (flag || !timer.Cancel())
			{
				this.m_TimedOut = true;
				throw new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
			}
			if (stream != null)
			{
				lock (this.m_SyncObject)
				{
					if (this.m_Aborted)
					{
						((ICloseEx)stream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						this.CheckError();
						throw new InternalException();
					}
					this.m_Stream = stream;
				}
			}
			return stream;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x00072E30 File Offset: 0x00071E30
		private void TimerCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			FtpControlStream connection = this.m_Connection;
			if (connection != null)
			{
				connection.AbortConnect();
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001E1F RID: 7711 RVA: 0x00072E4D File Offset: 0x00071E4D
		private TimerThread.Queue TimerQueue
		{
			get
			{
				if (this.m_TimerQueue == null)
				{
					this.m_TimerQueue = TimerThread.GetOrCreateQueue(this.RemainingTimeout);
				}
				return this.m_TimerQueue;
			}
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x00072E70 File Offset: 0x00071E70
		private bool AttemptedRecovery(Exception e)
		{
			if (!(e is WebException) || ((WebException)e).InternalStatus != WebExceptionInternalStatus.Isolated)
			{
				if (e is ThreadAbortException || e is StackOverflowException || e is OutOfMemoryException || this.m_OnceFailed || this.m_Aborted || this.m_TimedOut || this.m_Connection == null || !this.m_Connection.RecoverableFailure)
				{
					return false;
				}
				this.m_OnceFailed = true;
			}
			lock (this.m_SyncObject)
			{
				if (this.m_ConnectionPool == null || this.m_Connection == null)
				{
					return false;
				}
				this.m_Connection.CloseSocket();
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.Web, this, "", SR.GetString("net_log_releasing_connection", new object[] { ValidationHelper.HashString(this.m_Connection) }));
				}
				this.m_ConnectionPool.PutConnection(this.m_Connection, this, this.RemainingTimeout);
				this.m_Connection = null;
			}
			return true;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x00072F84 File Offset: 0x00071F84
		private void SetException(Exception exception)
		{
			if (exception is ThreadAbortException || exception is StackOverflowException || exception is OutOfMemoryException)
			{
				this.m_Exception = exception;
				throw exception;
			}
			FtpControlStream connection = this.m_Connection;
			if (this.m_Exception == null)
			{
				if (exception is WebException)
				{
					this.EnsureFtpWebResponse(exception);
					this.m_Exception = new WebException(exception.Message, null, ((WebException)exception).Status, this.m_FtpWebResponse);
				}
				else if (exception is AuthenticationException || exception is SecurityException)
				{
					this.m_Exception = exception;
				}
				else if (connection != null && connection.StatusCode != FtpStatusCode.Undefined)
				{
					this.EnsureFtpWebResponse(exception);
					this.m_Exception = new WebException(SR.GetString("net_servererror", new object[] { connection.StatusLine }), exception, WebExceptionStatus.ProtocolError, this.m_FtpWebResponse);
				}
				else
				{
					this.m_Exception = new WebException(exception.Message, exception);
				}
				if (connection != null && this.m_FtpWebResponse != null)
				{
					this.m_FtpWebResponse.UpdateStatus(connection.StatusCode, connection.StatusLine, connection.ExitMessage);
				}
			}
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x0007308D File Offset: 0x0007208D
		private void CheckError()
		{
			if (this.m_Exception != null)
			{
				throw this.m_Exception;
			}
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x0007309E File Offset: 0x0007209E
		internal override ContextAwareResult GetWritingContext()
		{
			if (this.m_ReadAsyncResult != null && this.m_ReadAsyncResult is ContextAwareResult)
			{
				return (ContextAwareResult)this.m_ReadAsyncResult;
			}
			if (this.m_WriteAsyncResult != null)
			{
				return this.m_WriteAsyncResult;
			}
			return null;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000730D1 File Offset: 0x000720D1
		internal override void RequestCallback(object obj)
		{
			if (this.m_Async)
			{
				this.AsyncRequestCallback(obj);
				return;
			}
			this.SyncRequestCallback(obj);
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x000730EC File Offset: 0x000720EC
		private void SyncRequestCallback(object obj)
		{
			FtpWebRequest.RequestStage requestStage = FtpWebRequest.RequestStage.CheckForError;
			try
			{
				bool flag = obj == null;
				Exception ex = obj as Exception;
				if (ex != null)
				{
					this.SetException(ex);
				}
				else
				{
					if (!flag)
					{
						throw new InternalException();
					}
					FtpControlStream connection = this.m_Connection;
					bool flag2 = false;
					if (connection != null)
					{
						this.EnsureFtpWebResponse(null);
						this.m_FtpWebResponse.UpdateStatus(connection.StatusCode, connection.StatusLine, connection.ExitMessage);
						flag2 = !this.m_CacheDone && (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Continue || base.CacheProtocol.ProtocolStatus == CacheValidationStatus.RetryResponseFromServer);
						if (this.m_MethodInfo.IsUpload)
						{
							this.CheckCacheRetrieveOnResponse();
							this.CheckCacheUpdateOnResponse();
						}
					}
					if (!flag2)
					{
						requestStage = FtpWebRequest.RequestStage.ReleaseConnection;
					}
				}
			}
			catch (Exception ex2)
			{
				this.SetException(ex2);
			}
			finally
			{
				this.FinishRequestStage(requestStage);
				this.CheckError();
			}
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x000731D8 File Offset: 0x000721D8
		private void AsyncRequestCallback(object obj)
		{
			FtpWebRequest.RequestStage requestStage = FtpWebRequest.RequestStage.CheckForError;
			try
			{
				FtpControlStream ftpControlStream = obj as FtpControlStream;
				FtpDataStream ftpDataStream = obj as FtpDataStream;
				Exception ex = obj as Exception;
				bool flag = obj == null;
				bool flag2;
				for (;;)
				{
					if (ex != null)
					{
						if (this.AttemptedRecovery(ex))
						{
							ftpControlStream = this.QueueOrCreateConnection();
							if (ftpControlStream == null)
							{
								break;
							}
							ex = null;
						}
						if (ex != null)
						{
							goto Block_7;
						}
					}
					if (ftpControlStream != null)
					{
						lock (this.m_SyncObject)
						{
							if (this.m_Aborted)
							{
								if (Logging.On)
								{
									Logging.PrintInfo(Logging.Web, this, "", SR.GetString("net_log_releasing_connection", new object[] { ValidationHelper.HashString(ftpControlStream) }));
								}
								this.m_ConnectionPool.PutConnection(ftpControlStream, this, this.Timeout);
								break;
							}
							this.m_Connection = ftpControlStream;
							if (Logging.On)
							{
								Logging.Associate(Logging.Web, this, this.m_Connection);
							}
						}
						try
						{
							ftpDataStream = (FtpDataStream)this.TimedSubmitRequestHelper(true);
						}
						catch (Exception ex2)
						{
							ex = ex2;
							continue;
						}
						break;
					}
					if (ftpDataStream != null)
					{
						goto Block_11;
					}
					if (!flag)
					{
						goto IL_020E;
					}
					ftpControlStream = this.m_Connection;
					flag2 = false;
					if (ftpControlStream != null)
					{
						this.EnsureFtpWebResponse(null);
						this.m_FtpWebResponse.UpdateStatus(ftpControlStream.StatusCode, ftpControlStream.StatusLine, ftpControlStream.ExitMessage);
						flag2 = !this.m_CacheDone && (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Continue || base.CacheProtocol.ProtocolStatus == CacheValidationStatus.RetryResponseFromServer);
						lock (this.m_SyncObject)
						{
							if (!this.CheckCacheRetrieveOnResponse())
							{
								continue;
							}
							if (this.m_FtpWebResponse.IsFromCache)
							{
								flag2 = false;
							}
							this.CheckCacheUpdateOnResponse();
						}
						goto IL_0206;
					}
					goto IL_0206;
				}
				return;
				Block_7:
				this.SetException(ex);
				return;
				Block_11:
				lock (this.m_SyncObject)
				{
					if (this.m_Aborted)
					{
						((ICloseEx)ftpDataStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						goto IL_0214;
					}
					this.m_Stream = ftpDataStream;
				}
				ftpDataStream.SetSocketTimeoutOption(SocketShutdown.Both, this.Timeout, true);
				this.EnsureFtpWebResponse(null);
				this.CheckCacheRetrieveOnResponse();
				this.CheckCacheUpdateOnResponse();
				requestStage = (ftpDataStream.CanRead ? FtpWebRequest.RequestStage.ReadReady : FtpWebRequest.RequestStage.WriteReady);
				goto IL_0214;
				IL_0206:
				if (!flag2)
				{
					requestStage = FtpWebRequest.RequestStage.ReleaseConnection;
					goto IL_0214;
				}
				goto IL_0214;
				IL_020E:
				throw new InternalException();
				IL_0214:;
			}
			catch (Exception ex3)
			{
				this.SetException(ex3);
			}
			catch
			{
				this.SetException(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			finally
			{
				this.FinishRequestStage(requestStage);
			}
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x000734D8 File Offset: 0x000724D8
		private FtpWebRequest.RequestStage FinishRequestStage(FtpWebRequest.RequestStage stage)
		{
			if (this.m_Exception != null)
			{
				stage = FtpWebRequest.RequestStage.ReleaseConnection;
			}
			FtpWebRequest.RequestStage requestStage;
			LazyAsyncResult writeAsyncResult;
			LazyAsyncResult readAsyncResult;
			FtpControlStream connection;
			lock (this.m_SyncObject)
			{
				requestStage = this.m_RequestStage;
				if (stage == FtpWebRequest.RequestStage.CheckForError)
				{
					return requestStage;
				}
				if (requestStage == FtpWebRequest.RequestStage.ReleaseConnection && stage == FtpWebRequest.RequestStage.ReleaseConnection)
				{
					return FtpWebRequest.RequestStage.ReleaseConnection;
				}
				if (stage > requestStage)
				{
					this.m_RequestStage = stage;
				}
				if (stage <= FtpWebRequest.RequestStage.RequestStarted)
				{
					return requestStage;
				}
				writeAsyncResult = this.m_WriteAsyncResult;
				readAsyncResult = this.m_ReadAsyncResult;
				connection = this.m_Connection;
				if (stage == FtpWebRequest.RequestStage.ReleaseConnection)
				{
					if (this.m_Exception == null && !this.m_Aborted && requestStage != FtpWebRequest.RequestStage.ReadReady && this.m_MethodInfo.IsDownload && !this.m_FtpWebResponse.IsFromCache)
					{
						return requestStage;
					}
					if (this.m_Exception != null || !this.m_FtpWebResponse.IsFromCache || this.KeepAlive)
					{
						this.m_Connection = null;
					}
				}
			}
			FtpWebRequest.RequestStage requestStage2;
			try
			{
				if ((stage == FtpWebRequest.RequestStage.ReleaseConnection || requestStage == FtpWebRequest.RequestStage.ReleaseConnection) && connection != null)
				{
					try
					{
						if (this.m_Exception != null)
						{
							connection.Abort(this.m_Exception);
						}
						else if (this.m_FtpWebResponse.IsFromCache && !this.KeepAlive)
						{
							connection.Quit();
						}
					}
					finally
					{
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.Web, this, "", SR.GetString("net_log_releasing_connection", new object[] { ValidationHelper.HashString(connection) }));
						}
						this.m_ConnectionPool.PutConnection(connection, this, this.RemainingTimeout);
						if (this.m_Async && this.m_RequestCompleteAsyncResult != null)
						{
							this.m_RequestCompleteAsyncResult.InvokeCallback();
						}
					}
				}
				requestStage2 = requestStage;
			}
			finally
			{
				try
				{
					if (stage >= FtpWebRequest.RequestStage.WriteReady)
					{
						if (this.m_MethodInfo.IsUpload && !this.m_GetRequestStreamStarted)
						{
							if (this.m_Stream != null)
							{
								this.m_Stream.Close();
							}
						}
						else if (writeAsyncResult != null && !writeAsyncResult.InternalPeekCompleted)
						{
							writeAsyncResult.InvokeCallback();
						}
					}
				}
				finally
				{
					if (stage >= FtpWebRequest.RequestStage.ReadReady && readAsyncResult != null && !readAsyncResult.InternalPeekCompleted)
					{
						readAsyncResult.InvokeCallback();
					}
				}
			}
			return requestStage2;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x000736F0 File Offset: 0x000726F0
		private static void AsyncCallbackWrapper(object request, object state)
		{
			FtpWebRequest ftpWebRequest = (FtpWebRequest)request;
			ftpWebRequest.RequestCallback(state);
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x0007370B File Offset: 0x0007270B
		private static PooledStream CreateFtpConnection(ConnectionPool pool)
		{
			return new FtpControlStream(pool, TimeSpan.MaxValue, false);
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0007371C File Offset: 0x0007271C
		public override void Abort()
		{
			if (this.m_Aborted)
			{
				return;
			}
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Abort", "");
			}
			try
			{
				if (this.HttpProxyMode)
				{
					this.GetHttpWebRequest().Abort();
				}
				else
				{
					if (base.CacheProtocol != null)
					{
						base.CacheProtocol.Abort();
					}
					Stream stream;
					FtpControlStream connection;
					lock (this.m_SyncObject)
					{
						if (this.m_RequestStage >= FtpWebRequest.RequestStage.ReleaseConnection)
						{
							return;
						}
						this.m_Aborted = true;
						stream = this.m_Stream;
						connection = this.m_Connection;
						this.m_Exception = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
					}
					if (stream != null)
					{
						((ICloseEx)stream).CloseEx(CloseExState.Abort | CloseExState.Silent);
					}
					if (connection != null)
					{
						connection.Abort(ExceptionHelper.RequestAbortedException);
					}
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "Abort", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "Abort", "");
				}
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001E2B RID: 7723 RVA: 0x0007384C File Offset: 0x0007284C
		// (set) Token: 0x06001E2C RID: 7724 RVA: 0x00073854 File Offset: 0x00072854
		public bool KeepAlive
		{
			get
			{
				return this.m_KeepAlive;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				this.m_KeepAlive = value;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x00073875 File Offset: 0x00072875
		// (set) Token: 0x06001E2E RID: 7726 RVA: 0x0007387D File Offset: 0x0007287D
		public bool UseBinary
		{
			get
			{
				return this.m_Binary;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				this.m_Binary = value;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001E2F RID: 7727 RVA: 0x0007389E File Offset: 0x0007289E
		// (set) Token: 0x06001E30 RID: 7728 RVA: 0x000738A6 File Offset: 0x000728A6
		public bool UsePassive
		{
			get
			{
				return this.m_Passive;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				this.m_Passive = value;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x000738C8 File Offset: 0x000728C8
		// (set) Token: 0x06001E32 RID: 7730 RVA: 0x0007391C File Offset: 0x0007291C
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.m_ClientCertificates == null)
				{
					lock (this.m_SyncObject)
					{
						if (this.m_ClientCertificates == null)
						{
							this.m_ClientCertificates = new X509CertificateCollection();
						}
					}
				}
				return this.m_ClientCertificates;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_ClientCertificates = value;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001E33 RID: 7731 RVA: 0x00073933 File Offset: 0x00072933
		// (set) Token: 0x06001E34 RID: 7732 RVA: 0x0007393B File Offset: 0x0007293B
		public bool EnableSsl
		{
			get
			{
				return this.m_EnableSsl;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				this.m_EnableSsl = value;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x0007395C File Offset: 0x0007295C
		// (set) Token: 0x06001E36 RID: 7734 RVA: 0x0007398C File Offset: 0x0007298C
		public override WebHeaderCollection Headers
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.GetHttpWebRequest().Headers;
				}
				if (this.m_FtpRequestHeaders == null)
				{
					this.m_FtpRequestHeaders = new WebHeaderCollection(WebHeaderCollectionType.FtpWebRequest);
				}
				return this.m_FtpRequestHeaders;
			}
			set
			{
				if (this.HttpProxyMode)
				{
					this.GetHttpWebRequest().Headers = value;
				}
				this.m_FtpRequestHeaders = value;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001E37 RID: 7735 RVA: 0x000739A9 File Offset: 0x000729A9
		// (set) Token: 0x06001E38 RID: 7736 RVA: 0x000739B0 File Offset: 0x000729B0
		public override string ContentType
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

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001E39 RID: 7737 RVA: 0x000739B7 File Offset: 0x000729B7
		// (set) Token: 0x06001E3A RID: 7738 RVA: 0x000739BE File Offset: 0x000729BE
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

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001E3B RID: 7739 RVA: 0x000739C5 File Offset: 0x000729C5
		// (set) Token: 0x06001E3C RID: 7740 RVA: 0x000739CC File Offset: 0x000729CC
		public override bool PreAuthenticate
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

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x000739D3 File Offset: 0x000729D3
		private bool InUse
		{
			get
			{
				return this.m_GetRequestStreamStarted || this.m_GetResponseStarted;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x000739E8 File Offset: 0x000729E8
		private bool HttpProxyMode
		{
			get
			{
				return this.m_HttpWebRequest != null;
			}
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x000739F8 File Offset: 0x000729F8
		private void EnsureFtpWebResponse(Exception exception)
		{
			if (this.m_FtpWebResponse == null || (this.m_FtpWebResponse.GetResponseStream() is FtpWebResponse.EmptyStream && this.m_Stream != null))
			{
				lock (this.m_SyncObject)
				{
					if (this.m_FtpWebResponse == null || (this.m_FtpWebResponse.GetResponseStream() is FtpWebResponse.EmptyStream && this.m_Stream != null))
					{
						Stream stream = this.m_Stream;
						if (this.m_MethodInfo.IsUpload)
						{
							stream = null;
						}
						if (this.m_Stream != null && this.m_Stream.CanRead && this.m_Stream.CanTimeout)
						{
							this.m_Stream.ReadTimeout = this.ReadWriteTimeout;
							this.m_Stream.WriteTimeout = this.ReadWriteTimeout;
						}
						FtpControlStream connection = this.m_Connection;
						long num = ((connection != null) ? connection.ContentLength : (-1L));
						if (stream == null && num < 0L)
						{
							num = 0L;
						}
						if (this.m_FtpWebResponse != null)
						{
							this.m_FtpWebResponse.SetResponseStream(stream);
						}
						else if (connection != null)
						{
							this.m_FtpWebResponse = new FtpWebResponse(stream, num, connection.ResponseUri, connection.StatusCode, connection.StatusLine, connection.LastModified, connection.BannerMessage, connection.WelcomeMessage, connection.ExitMessage);
						}
						else
						{
							this.m_FtpWebResponse = new FtpWebResponse(stream, -1L, this.m_Uri, FtpStatusCode.Undefined, null, DateTime.Now, null, null, null);
						}
					}
				}
			}
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x00073B74 File Offset: 0x00072B74
		private HttpWebRequest GetHttpWebRequest()
		{
			lock (this.m_SyncObject)
			{
				if (this.m_HttpWebRequest == null)
				{
					if (this.m_ContentOffset > 0L)
					{
						throw new InvalidOperationException(SR.GetString("net_ftp_no_offsetforhttp"));
					}
					if (!this.m_MethodInfo.HasHttpCommand)
					{
						throw new InvalidOperationException(SR.GetString("net_ftp_no_http_cmd"));
					}
					this.m_HttpWebRequest = new HttpWebRequest(this.m_Uri, this.ServicePoint);
					this.m_HttpWebRequest.Credentials = this.Credentials;
					this.m_HttpWebRequest.InternalProxy = this.m_Proxy;
					this.m_HttpWebRequest.KeepAlive = this.KeepAlive;
					this.m_HttpWebRequest.Timeout = this.Timeout;
					this.m_HttpWebRequest.Method = this.m_MethodInfo.HttpCommand;
					this.m_HttpWebRequest.CacheProtocol = base.CacheProtocol;
					RequestCacheLevel requestCacheLevel;
					if (this.CachePolicy == null)
					{
						requestCacheLevel = RequestCacheLevel.BypassCache;
					}
					else
					{
						requestCacheLevel = this.CachePolicy.Level;
					}
					if (requestCacheLevel == RequestCacheLevel.Revalidate)
					{
						requestCacheLevel = RequestCacheLevel.Reload;
					}
					this.m_HttpWebRequest.CachePolicy = new HttpRequestCachePolicy((HttpRequestCacheLevel)requestCacheLevel);
					base.CacheProtocol = null;
				}
			}
			return this.m_HttpWebRequest;
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x00073CA8 File Offset: 0x00072CA8
		private string GetConnectionGroupLine()
		{
			return this.ConnectionGroupName;
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x00073CB0 File Offset: 0x00072CB0
		internal string GetUserString()
		{
			string text = null;
			if (this.Credentials != null)
			{
				NetworkCredential credential = this.Credentials.GetCredential(this.m_Uri, "basic");
				if (credential != null)
				{
					text = credential.InternalGetUserName();
					string text2 = credential.InternalGetDomain();
					if (!ValidationHelper.IsBlankString(text2))
					{
						text = text2 + "\\" + text;
					}
				}
			}
			if (text == null)
			{
				return null;
			}
			if (string.Compare(text, "anonymous", StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				return text;
			}
			return null;
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x00073D1C File Offset: 0x00072D1C
		private bool CheckCacheRetrieveBeforeSubmit()
		{
			if (base.CacheProtocol == null || this.m_CacheDone)
			{
				this.m_CacheDone = true;
				return false;
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.CombineCachedAndServerResponse || base.CacheProtocol.ProtocolStatus == CacheValidationStatus.DoNotTakeFromCache)
			{
				return false;
			}
			Uri uri = this.RequestUri;
			string text = this.GetUserString();
			if (text != null)
			{
				text = Uri.EscapeDataString(text);
			}
			if (uri.Fragment.Length != 0 || text != null)
			{
				if (text == null)
				{
					uri = new Uri(uri.GetParts(UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped));
				}
				else
				{
					text = uri.GetParts(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.SafeUnescaped) + text + '@';
					text += uri.GetParts(UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query, UriFormat.SafeUnescaped);
					uri = new Uri(text);
				}
			}
			base.CacheProtocol.GetRetrieveStatus(uri, this);
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Fail)
			{
				throw base.CacheProtocol.ProtocolException;
			}
			if (base.CacheProtocol.ProtocolStatus != CacheValidationStatus.ReturnCachedResponse)
			{
				return false;
			}
			if (this.m_MethodInfo.Operation != FtpOperation.DownloadFile)
			{
				throw new NotSupportedException(SR.GetString("net_cache_not_supported_command"));
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				FtpRequestCacheValidator ftpRequestCacheValidator = (FtpRequestCacheValidator)base.CacheProtocol.Validator;
				this.m_FtpWebResponse = new FtpWebResponse(base.CacheProtocol.ResponseStream, base.CacheProtocol.ResponseStreamLength, this.RequestUri, this.UsePassive ? FtpStatusCode.DataAlreadyOpen : FtpStatusCode.OpeningData, (this.UsePassive ? FtpStatusCode.DataAlreadyOpen : FtpStatusCode.OpeningData).ToString(), (ftpRequestCacheValidator.CacheEntry.LastModifiedUtc == DateTime.MinValue) ? DateTime.Now : ftpRequestCacheValidator.CacheEntry.LastModifiedUtc.ToLocalTime(), string.Empty, string.Empty, string.Empty);
				this.m_FtpWebResponse.InternalSetFromCache = true;
				this.m_FtpWebResponse.InternalSetIsCacheFresh = ftpRequestCacheValidator.CacheFreshnessStatus != CacheFreshnessStatus.Stale;
			}
			return true;
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x00073EFC File Offset: 0x00072EFC
		private bool CheckCacheRetrieveOnResponse()
		{
			if (base.CacheProtocol == null || this.m_CacheDone)
			{
				return true;
			}
			if (base.CacheProtocol.ProtocolStatus != CacheValidationStatus.Continue)
			{
				return true;
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Fail)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "CheckCacheRetrieveOnResponse", base.CacheProtocol.ProtocolException);
				}
				throw base.CacheProtocol.ProtocolException;
			}
			base.CacheProtocol.GetRevalidateStatus(this.m_FtpWebResponse, null);
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.RetryResponseFromServer)
			{
				if (this.m_FtpWebResponse != null)
				{
					this.m_FtpWebResponse.SetResponseStream(null);
				}
				return false;
			}
			if (base.CacheProtocol.ProtocolStatus != CacheValidationStatus.ReturnCachedResponse)
			{
				return false;
			}
			if (this.m_MethodInfo.Operation != FtpOperation.DownloadFile)
			{
				throw new NotSupportedException(SR.GetString("net_cache_not_supported_command"));
			}
			FtpRequestCacheValidator ftpRequestCacheValidator = (FtpRequestCacheValidator)base.CacheProtocol.Validator;
			FtpWebResponse ftpWebResponse = this.m_FtpWebResponse;
			this.m_Stream = base.CacheProtocol.ResponseStream;
			this.m_FtpWebResponse = new FtpWebResponse(base.CacheProtocol.ResponseStream, base.CacheProtocol.ResponseStreamLength, this.RequestUri, this.UsePassive ? FtpStatusCode.DataAlreadyOpen : FtpStatusCode.OpeningData, (this.UsePassive ? FtpStatusCode.DataAlreadyOpen : FtpStatusCode.OpeningData).ToString(), (ftpRequestCacheValidator.CacheEntry.LastModifiedUtc == DateTime.MinValue) ? DateTime.Now : ftpRequestCacheValidator.CacheEntry.LastModifiedUtc.ToLocalTime(), string.Empty, string.Empty, string.Empty);
			this.m_FtpWebResponse.InternalSetFromCache = true;
			this.m_FtpWebResponse.InternalSetIsCacheFresh = base.CacheProtocol.IsCacheFresh;
			ftpWebResponse.Close();
			return true;
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000740B4 File Offset: 0x000730B4
		private void CheckCacheUpdateOnResponse()
		{
			if (base.CacheProtocol == null || this.m_CacheDone)
			{
				return;
			}
			this.m_CacheDone = true;
			if (this.m_Connection != null)
			{
				this.m_FtpWebResponse.UpdateStatus(this.m_Connection.StatusCode, this.m_Connection.StatusLine, this.m_Connection.ExitMessage);
				if (this.m_Connection.StatusCode == FtpStatusCode.OpeningData && this.m_FtpWebResponse.ContentLength == 0L)
				{
					this.m_FtpWebResponse.SetContentLength(this.m_Connection.ContentLength);
				}
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.CombineCachedAndServerResponse)
			{
				this.m_Stream = new CombinedReadStream(base.CacheProtocol.Validator.CacheStream, this.m_FtpWebResponse.GetResponseStream());
				FtpStatusCode ftpStatusCode = (this.UsePassive ? FtpStatusCode.DataAlreadyOpen : FtpStatusCode.OpeningData);
				this.m_FtpWebResponse.UpdateStatus(ftpStatusCode, ftpStatusCode.ToString(), string.Empty);
				this.m_FtpWebResponse.SetResponseStream(this.m_Stream);
			}
			if (base.CacheProtocol.GetUpdateStatus(this.m_FtpWebResponse, this.m_FtpWebResponse.GetResponseStream()) == CacheValidationStatus.UpdateResponseInformation)
			{
				this.m_Stream = base.CacheProtocol.ResponseStream;
				this.m_FtpWebResponse.SetResponseStream(this.m_Stream);
				return;
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Fail)
			{
				throw base.CacheProtocol.ProtocolException;
			}
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x00074214 File Offset: 0x00073214
		internal void DataStreamClosed(CloseExState closeState)
		{
			if ((closeState & CloseExState.Abort) == CloseExState.Normal)
			{
				if (this.m_Async)
				{
					this.m_RequestCompleteAsyncResult.InternalWaitForCompletion();
					this.CheckError();
					return;
				}
				if (this.m_Connection != null)
				{
					this.m_Connection.CheckContinuePipeline();
					return;
				}
			}
			else
			{
				FtpControlStream connection = this.m_Connection;
				if (connection != null)
				{
					connection.Abort(ExceptionHelper.RequestAbortedException);
				}
			}
		}

		// Token: 0x04001E07 RID: 7687
		private object m_SyncObject;

		// Token: 0x04001E08 RID: 7688
		private ICredentials m_AuthInfo;

		// Token: 0x04001E09 RID: 7689
		private readonly Uri m_Uri;

		// Token: 0x04001E0A RID: 7690
		private FtpMethodInfo m_MethodInfo;

		// Token: 0x04001E0B RID: 7691
		private string m_RenameTo;

		// Token: 0x04001E0C RID: 7692
		private bool m_GetRequestStreamStarted;

		// Token: 0x04001E0D RID: 7693
		private bool m_GetResponseStarted;

		// Token: 0x04001E0E RID: 7694
		private DateTime m_StartTime;

		// Token: 0x04001E0F RID: 7695
		private int m_Timeout = FtpWebRequest.s_DefaultTimeout;

		// Token: 0x04001E10 RID: 7696
		private int m_RemainingTimeout;

		// Token: 0x04001E11 RID: 7697
		private long m_ContentLength;

		// Token: 0x04001E12 RID: 7698
		private long m_ContentOffset;

		// Token: 0x04001E13 RID: 7699
		private IWebProxy m_Proxy;

		// Token: 0x04001E14 RID: 7700
		private X509CertificateCollection m_ClientCertificates;

		// Token: 0x04001E15 RID: 7701
		private bool m_KeepAlive = true;

		// Token: 0x04001E16 RID: 7702
		private bool m_Passive = true;

		// Token: 0x04001E17 RID: 7703
		private bool m_Binary = true;

		// Token: 0x04001E18 RID: 7704
		private string m_ConnectionGroupName;

		// Token: 0x04001E19 RID: 7705
		private ServicePoint m_ServicePoint;

		// Token: 0x04001E1A RID: 7706
		private bool m_CacheDone;

		// Token: 0x04001E1B RID: 7707
		private bool m_Async;

		// Token: 0x04001E1C RID: 7708
		private bool m_Aborted;

		// Token: 0x04001E1D RID: 7709
		private bool m_TimedOut;

		// Token: 0x04001E1E RID: 7710
		private HttpWebRequest m_HttpWebRequest;

		// Token: 0x04001E1F RID: 7711
		private Exception m_Exception;

		// Token: 0x04001E20 RID: 7712
		private TimerThread.Queue m_TimerQueue = FtpWebRequest.s_DefaultTimerQueue;

		// Token: 0x04001E21 RID: 7713
		private TimerThread.Callback m_TimerCallback;

		// Token: 0x04001E22 RID: 7714
		private bool m_EnableSsl;

		// Token: 0x04001E23 RID: 7715
		private bool m_ProxyUserSet;

		// Token: 0x04001E24 RID: 7716
		private ConnectionPool m_ConnectionPool;

		// Token: 0x04001E25 RID: 7717
		private FtpControlStream m_Connection;

		// Token: 0x04001E26 RID: 7718
		private Stream m_Stream;

		// Token: 0x04001E27 RID: 7719
		private FtpWebRequest.RequestStage m_RequestStage;

		// Token: 0x04001E28 RID: 7720
		private bool m_OnceFailed;

		// Token: 0x04001E29 RID: 7721
		private WebHeaderCollection m_FtpRequestHeaders;

		// Token: 0x04001E2A RID: 7722
		private FtpWebResponse m_FtpWebResponse;

		// Token: 0x04001E2B RID: 7723
		private int m_ReadWriteTimeout = 300000;

		// Token: 0x04001E2C RID: 7724
		private ContextAwareResult m_WriteAsyncResult;

		// Token: 0x04001E2D RID: 7725
		private LazyAsyncResult m_ReadAsyncResult;

		// Token: 0x04001E2E RID: 7726
		private LazyAsyncResult m_RequestCompleteAsyncResult;

		// Token: 0x04001E2F RID: 7727
		private static readonly GeneralAsyncDelegate m_AsyncCallback = new GeneralAsyncDelegate(FtpWebRequest.AsyncCallbackWrapper);

		// Token: 0x04001E30 RID: 7728
		private static readonly CreateConnectionDelegate m_CreateConnectionCallback = new CreateConnectionDelegate(FtpWebRequest.CreateFtpConnection);

		// Token: 0x04001E31 RID: 7729
		private static readonly NetworkCredential DefaultFtpNetworkCredential = new NetworkCredential("anonymous", "anonymous@", string.Empty, false);

		// Token: 0x04001E32 RID: 7730
		private static readonly int s_DefaultTimeout = 100000;

		// Token: 0x04001E33 RID: 7731
		private static readonly TimerThread.Queue s_DefaultTimerQueue = TimerThread.GetOrCreateQueue(FtpWebRequest.s_DefaultTimeout);

		// Token: 0x020003C1 RID: 961
		private enum RequestStage
		{
			// Token: 0x04001E35 RID: 7733
			CheckForError,
			// Token: 0x04001E36 RID: 7734
			RequestStarted,
			// Token: 0x04001E37 RID: 7735
			WriteReady,
			// Token: 0x04001E38 RID: 7736
			ReadReady,
			// Token: 0x04001E39 RID: 7737
			ReleaseConnection
		}
	}
}
