using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace System.Net
{
	// Token: 0x020003D6 RID: 982
	public sealed class HttpListenerRequest
	{
		// Token: 0x06001EF8 RID: 7928 RVA: 0x00077CD4 File Offset: 0x00076CD4
		internal unsafe HttpListenerRequest(HttpListenerContext httpContext, RequestContextBase memoryBlob)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", "httpContext#" + ValidationHelper.HashString(httpContext) + " memoryBlob# " + ValidationHelper.HashString((IntPtr)((void*)memoryBlob.RequestBlob)));
			}
			if (Logging.On)
			{
				Logging.Associate(Logging.HttpListener, this, httpContext);
			}
			this.m_HttpContext = httpContext;
			this.m_MemoryBlob = memoryBlob;
			this.m_BoundaryType = BoundaryType.None;
			this.m_RequestId = memoryBlob.RequestBlob->RequestId;
			this.m_ConnectionId = memoryBlob.RequestBlob->ConnectionId;
			this.m_SslStatus = ((memoryBlob.RequestBlob->pSslInfo == null) ? HttpListenerRequest.SslStatus.Insecure : ((memoryBlob.RequestBlob->pSslInfo->SslClientCertNegotiated == 0U) ? HttpListenerRequest.SslStatus.NoClientCert : HttpListenerRequest.SslStatus.ClientCert));
			if (memoryBlob.RequestBlob->pRawUrl != null && memoryBlob.RequestBlob->RawUrlLength > 0)
			{
				this.m_RawUrl = Marshal.PtrToStringAnsi((IntPtr)((void*)memoryBlob.RequestBlob->pRawUrl), (int)memoryBlob.RequestBlob->RawUrlLength);
			}
			if (memoryBlob.RequestBlob->CookedUrl.pFullUrl != null && memoryBlob.RequestBlob->CookedUrl.FullUrlLength > 0)
			{
				this.m_CookedUrl = Marshal.PtrToStringUni((IntPtr)((void*)memoryBlob.RequestBlob->CookedUrl.pFullUrl), (int)(memoryBlob.RequestBlob->CookedUrl.FullUrlLength / 2));
			}
			this.m_Version = new Version((int)memoryBlob.RequestBlob->Version.MajorVersion, (int)memoryBlob.RequestBlob->Version.MinorVersion);
			this.m_ClientCertState = ListenerClientCertState.NotInitialized;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", string.Concat(new string[]
				{
					"httpContext#",
					ValidationHelper.HashString(httpContext),
					" RequestUri:",
					ValidationHelper.ToString(this.RequestUri),
					" Content-Length:",
					ValidationHelper.ToString(this.ContentLength64),
					" HTTP Method:",
					ValidationHelper.ToString(this.HttpMethod)
				}));
			}
			if (Logging.On)
			{
				StringBuilder stringBuilder = new StringBuilder("HttpListenerRequest Headers:\n");
				for (int i = 0; i < this.Headers.Count; i++)
				{
					stringBuilder.Append("\t");
					stringBuilder.Append(this.Headers.GetKey(i));
					stringBuilder.Append(" : ");
					stringBuilder.Append(this.Headers.Get(i));
					stringBuilder.Append("\n");
				}
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", stringBuilder.ToString());
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001EF9 RID: 7929 RVA: 0x00077F75 File Offset: 0x00076F75
		internal HttpListenerContext HttpListenerContext
		{
			get
			{
				return this.m_HttpContext;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001EFA RID: 7930 RVA: 0x00077F7D File Offset: 0x00076F7D
		internal byte[] RequestBuffer
		{
			get
			{
				this.CheckDisposed();
				return this.m_MemoryBlob.RequestBuffer;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001EFB RID: 7931 RVA: 0x00077F90 File Offset: 0x00076F90
		internal IntPtr OriginalBlobAddress
		{
			get
			{
				this.CheckDisposed();
				return this.m_MemoryBlob.OriginalBlobAddress;
			}
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x00077FA3 File Offset: 0x00076FA3
		internal void DetachBlob(RequestContextBase memoryBlob)
		{
			if (memoryBlob != null && memoryBlob == this.m_MemoryBlob)
			{
				this.m_MemoryBlob = null;
			}
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x00077FB8 File Offset: 0x00076FB8
		internal void ReleasePins()
		{
			this.m_MemoryBlob.ReleasePins();
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001EFE RID: 7934 RVA: 0x00077FC8 File Offset: 0x00076FC8
		public unsafe Guid RequestTraceIdentifier
		{
			get
			{
				Guid guid = default(Guid);
				8[&guid / 8] = (long)this.RequestId;
				return guid;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001EFF RID: 7935 RVA: 0x00077FEB File Offset: 0x00076FEB
		internal ulong RequestId
		{
			get
			{
				return this.m_RequestId;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x00077FF3 File Offset: 0x00076FF3
		public string[] AcceptTypes
		{
			get
			{
				return HttpListenerRequest.Helpers.ParseMultivalueHeader(this.GetKnownHeader(HttpRequestHeader.Accept));
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x00078004 File Offset: 0x00077004
		public Encoding ContentEncoding
		{
			get
			{
				if (this.UserAgent != null && CultureInfo.InvariantCulture.CompareInfo.IsPrefix(this.UserAgent, "UP"))
				{
					string text = this.Headers["x-up-devcap-post-charset"];
					if (text != null && text.Length > 0)
					{
						try
						{
							return Encoding.GetEncoding(text);
						}
						catch (ArgumentException)
						{
						}
					}
				}
				if (this.HasEntityBody && this.ContentType != null)
				{
					string attributeFromHeader = HttpListenerRequest.Helpers.GetAttributeFromHeader(this.ContentType, "charset");
					if (attributeFromHeader != null)
					{
						try
						{
							return Encoding.GetEncoding(attributeFromHeader);
						}
						catch (ArgumentException)
						{
						}
					}
				}
				return Encoding.Default;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x000780B4 File Offset: 0x000770B4
		public long ContentLength64
		{
			get
			{
				if (this.m_BoundaryType == BoundaryType.None)
				{
					if (this.GetKnownHeader(HttpRequestHeader.TransferEncoding) == "chunked")
					{
						this.m_BoundaryType = BoundaryType.Chunked;
						this.m_ContentLength = -1L;
					}
					else
					{
						this.m_ContentLength = 0L;
						this.m_BoundaryType = BoundaryType.ContentLength;
						string knownHeader = this.GetKnownHeader(HttpRequestHeader.ContentLength);
						if (knownHeader != null && !long.TryParse(knownHeader, NumberStyles.None, CultureInfo.InvariantCulture.NumberFormat, out this.m_ContentLength))
						{
							this.m_ContentLength = 0L;
							this.m_BoundaryType = BoundaryType.Invalid;
						}
					}
				}
				return this.m_ContentLength;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001F03 RID: 7939 RVA: 0x00078139 File Offset: 0x00077139
		public string ContentType
		{
			get
			{
				return this.GetKnownHeader(HttpRequestHeader.ContentType);
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x00078143 File Offset: 0x00077143
		public NameValueCollection Headers
		{
			get
			{
				if (this.m_WebHeaders == null)
				{
					this.m_WebHeaders = UnsafeNclNativeMethods.HttpApi.GetHeaders(this.RequestBuffer, this.OriginalBlobAddress);
				}
				return this.m_WebHeaders;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x0007816A File Offset: 0x0007716A
		public string HttpMethod
		{
			get
			{
				if (this.m_HttpMethod == null)
				{
					this.m_HttpMethod = UnsafeNclNativeMethods.HttpApi.GetVerb(this.RequestBuffer, this.OriginalBlobAddress);
				}
				return this.m_HttpMethod;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x00078194 File Offset: 0x00077194
		public Stream InputStream
		{
			get
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.HttpListener, this, "InputStream_get", "");
				}
				if (this.m_RequestStream == null)
				{
					this.m_RequestStream = (this.HasEntityBody ? new HttpRequestStream(this.HttpListenerContext) : Stream.Null);
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "InputStream_get", "");
				}
				return this.m_RequestStream;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001F07 RID: 7943 RVA: 0x00078208 File Offset: 0x00077208
		public bool IsAuthenticated
		{
			get
			{
				IPrincipal user = this.HttpListenerContext.User;
				return user != null && user.Identity != null && user.Identity.IsAuthenticated;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x00078239 File Offset: 0x00077239
		public bool IsLocal
		{
			get
			{
				return this.LocalEndPoint.Address == this.RemoteEndPoint.Address;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001F09 RID: 7945 RVA: 0x00078253 File Offset: 0x00077253
		internal bool InternalIsLocal
		{
			get
			{
				return this.LocalEndPoint.Address.Equals(this.RemoteEndPoint.Address);
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x00078270 File Offset: 0x00077270
		public bool IsSecureConnection
		{
			get
			{
				return this.m_SslStatus != HttpListenerRequest.SslStatus.Insecure;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001F0B RID: 7947 RVA: 0x00078280 File Offset: 0x00077280
		public NameValueCollection QueryString
		{
			get
			{
				NameValueCollection nameValueCollection = new NameValueCollection();
				HttpListenerRequest.Helpers.FillFromString(nameValueCollection, this.Url.Query, true, this.ContentEncoding);
				return nameValueCollection;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x000782AC File Offset: 0x000772AC
		public string RawUrl
		{
			get
			{
				return this.m_RawUrl;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001F0D RID: 7949 RVA: 0x000782B4 File Offset: 0x000772B4
		// (set) Token: 0x06001F0E RID: 7950 RVA: 0x000782BC File Offset: 0x000772BC
		public string ServiceName
		{
			get
			{
				return this.m_ServiceName;
			}
			internal set
			{
				this.m_ServiceName = value;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001F0F RID: 7951 RVA: 0x000782C5 File Offset: 0x000772C5
		public Uri Url
		{
			get
			{
				return this.RequestUri;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x000782D0 File Offset: 0x000772D0
		public Uri UrlReferrer
		{
			get
			{
				string knownHeader = this.GetKnownHeader(HttpRequestHeader.Referer);
				if (knownHeader == null)
				{
					return null;
				}
				Uri uri;
				if (!Uri.TryCreate(knownHeader, UriKind.RelativeOrAbsolute, out uri))
				{
					return null;
				}
				return uri;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001F11 RID: 7953 RVA: 0x000782FB File Offset: 0x000772FB
		public string UserAgent
		{
			get
			{
				return this.GetKnownHeader(HttpRequestHeader.UserAgent);
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x00078305 File Offset: 0x00077305
		public string UserHostAddress
		{
			get
			{
				return this.LocalEndPoint.ToString();
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001F13 RID: 7955 RVA: 0x00078312 File Offset: 0x00077312
		public string UserHostName
		{
			get
			{
				return this.GetKnownHeader(HttpRequestHeader.Host);
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001F14 RID: 7956 RVA: 0x0007831C File Offset: 0x0007731C
		public string[] UserLanguages
		{
			get
			{
				return HttpListenerRequest.Helpers.ParseMultivalueHeader(this.GetKnownHeader(HttpRequestHeader.AcceptLanguage));
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001F15 RID: 7957 RVA: 0x0007832C File Offset: 0x0007732C
		public int ClientCertificateError
		{
			get
			{
				if (this.m_ClientCertState == ListenerClientCertState.NotInitialized)
				{
					throw new InvalidOperationException(SR.GetString("net_listener_mustcall", new object[] { "GetClientCertificate()/BeginGetClientCertificate()" }));
				}
				if (this.m_ClientCertState == ListenerClientCertState.InProgress)
				{
					throw new InvalidOperationException(SR.GetString("net_listener_mustcompletecall", new object[] { "GetClientCertificate()/BeginGetClientCertificate()" }));
				}
				return this.m_ClientCertificateError;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (set) Token: 0x06001F16 RID: 7958 RVA: 0x00078390 File Offset: 0x00077390
		internal X509Certificate2 ClientCertificate
		{
			set
			{
				this.m_ClientCertificate = value;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (set) Token: 0x06001F17 RID: 7959 RVA: 0x00078399 File Offset: 0x00077399
		internal ListenerClientCertState ClientCertState
		{
			set
			{
				this.m_ClientCertState = value;
			}
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x000783A2 File Offset: 0x000773A2
		internal void SetClientCertificateError(int clientCertificateError)
		{
			this.m_ClientCertificateError = clientCertificateError;
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x000783AC File Offset: 0x000773AC
		public X509Certificate2 GetClientCertificate()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "GetClientCertificate", "");
			}
			try
			{
				this.ProcessClientCertificate();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "GetClientCertificate", ValidationHelper.ToString(this.m_ClientCertificate));
				}
			}
			return this.m_ClientCertificate;
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x00078418 File Offset: 0x00077418
		public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "BeginGetClientCertificate", "");
			}
			return this.AsyncProcessClientCertificate(requestCallback, state);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x00078440 File Offset: 0x00077440
		public X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "EndGetClientCertificate", "");
			}
			X509Certificate2 x509Certificate = null;
			try
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				ListenerClientCertAsyncResult listenerClientCertAsyncResult = asyncResult as ListenerClientCertAsyncResult;
				if (listenerClientCertAsyncResult == null || listenerClientCertAsyncResult.AsyncObject != this)
				{
					throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (listenerClientCertAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndGetClientCertificate" }));
				}
				listenerClientCertAsyncResult.EndCalled = true;
				x509Certificate = listenerClientCertAsyncResult.InternalWaitForCompletion() as X509Certificate2;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "EndGetClientCertificate", ValidationHelper.HashString(x509Certificate));
				}
			}
			return x509Certificate;
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001F1C RID: 7964 RVA: 0x00078510 File Offset: 0x00077510
		public TransportContext TransportContext
		{
			get
			{
				return new HttpListenerRequestContext(this);
			}
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x00078518 File Offset: 0x00077518
		private CookieCollection ParseCookies(Uri uri, string setCookieHeader)
		{
			CookieCollection cookieCollection = new CookieCollection();
			CookieParser cookieParser = new CookieParser(setCookieHeader);
			for (;;)
			{
				Cookie server = cookieParser.GetServer();
				if (server == null)
				{
					break;
				}
				if (server.Name.Length != 0)
				{
					cookieCollection.InternalAdd(server, true);
				}
			}
			return cookieCollection;
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001F1E RID: 7966 RVA: 0x00078558 File Offset: 0x00077558
		public CookieCollection Cookies
		{
			get
			{
				if (this.m_Cookies == null)
				{
					string knownHeader = this.GetKnownHeader(HttpRequestHeader.Cookie);
					if (knownHeader != null && knownHeader.Length > 0)
					{
						this.m_Cookies = this.ParseCookies(this.RequestUri, knownHeader);
					}
					if (this.m_Cookies == null)
					{
						this.m_Cookies = new CookieCollection();
					}
					if (this.HttpListenerContext.PromoteCookiesToRfc2965)
					{
						for (int i = 0; i < this.m_Cookies.Count; i++)
						{
							if (this.m_Cookies[i].Variant == CookieVariant.Rfc2109)
							{
								this.m_Cookies[i].Variant = CookieVariant.Rfc2965;
							}
						}
					}
				}
				return this.m_Cookies;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001F1F RID: 7967 RVA: 0x000785FA File Offset: 0x000775FA
		public Version ProtocolVersion
		{
			get
			{
				return this.m_Version;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001F20 RID: 7968 RVA: 0x00078602 File Offset: 0x00077602
		public bool HasEntityBody
		{
			get
			{
				return (this.ContentLength64 > 0L && this.m_BoundaryType == BoundaryType.ContentLength) || this.m_BoundaryType == BoundaryType.Chunked || this.m_BoundaryType == BoundaryType.Multipart;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001F21 RID: 7969 RVA: 0x0007862C File Offset: 0x0007762C
		public bool KeepAlive
		{
			get
			{
				if (this.m_KeepAlive == TriState.Unspecified)
				{
					string text = this.Headers["Proxy-Connection"];
					if (string.IsNullOrEmpty(text))
					{
						text = this.GetKnownHeader(HttpRequestHeader.Connection);
					}
					if (string.IsNullOrEmpty(text))
					{
						if (this.ProtocolVersion >= HttpVersion.Version11)
						{
							this.m_KeepAlive = TriState.True;
						}
						else
						{
							text = this.GetKnownHeader(HttpRequestHeader.KeepAlive);
							this.m_KeepAlive = (string.IsNullOrEmpty(text) ? TriState.False : TriState.True);
						}
					}
					else
					{
						this.m_KeepAlive = ((text.ToLower(CultureInfo.InvariantCulture).IndexOf("close") < 0 || text.ToLower(CultureInfo.InvariantCulture).IndexOf("keep-alive") >= 0) ? TriState.True : TriState.False);
					}
				}
				return this.m_KeepAlive == TriState.True;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001F22 RID: 7970 RVA: 0x000786E8 File Offset: 0x000776E8
		public IPEndPoint RemoteEndPoint
		{
			get
			{
				if (this.m_RemoteEndPoint == null)
				{
					this.m_RemoteEndPoint = UnsafeNclNativeMethods.HttpApi.GetRemoteEndPoint(this.RequestBuffer, this.OriginalBlobAddress);
				}
				return this.m_RemoteEndPoint;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x0007870F File Offset: 0x0007770F
		public IPEndPoint LocalEndPoint
		{
			get
			{
				if (this.m_LocalEndPoint == null)
				{
					this.m_LocalEndPoint = UnsafeNclNativeMethods.HttpApi.GetLocalEndPoint(this.RequestBuffer, this.OriginalBlobAddress);
				}
				return this.m_LocalEndPoint;
			}
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x00078738 File Offset: 0x00077738
		internal void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close", "");
			}
			RequestContextBase memoryBlob = this.m_MemoryBlob;
			if (memoryBlob != null)
			{
				memoryBlob.Close();
				this.m_MemoryBlob = null;
			}
			this.m_IsDisposed = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Close", "");
			}
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x0007879C File Offset: 0x0007779C
		private unsafe ListenerClientCertAsyncResult AsyncProcessClientCertificate(AsyncCallback requestCallback, object state)
		{
			if (this.m_ClientCertState == ListenerClientCertState.InProgress)
			{
				throw new InvalidOperationException(SR.GetString("net_listener_callinprogress", new object[] { "GetClientCertificate()/BeginGetClientCertificate()" }));
			}
			this.m_ClientCertState = ListenerClientCertState.InProgress;
			this.HttpListenerContext.EnsureBoundHandle();
			ListenerClientCertAsyncResult listenerClientCertAsyncResult = null;
			if (this.m_SslStatus != HttpListenerRequest.SslStatus.Insecure)
			{
				uint num = 1500U;
				listenerClientCertAsyncResult = new ListenerClientCertAsyncResult(this, state, requestCallback, num);
				try
				{
					uint num3;
					for (;;)
					{
						uint num2 = 0U;
						num3 = UnsafeNclNativeMethods.HttpApi.HttpReceiveClientCertificate(this.HttpListenerContext.RequestQueueHandle, this.m_ConnectionId, 0U, listenerClientCertAsyncResult.RequestBlob, num, &num2, listenerClientCertAsyncResult.NativeOverlapped);
						if (num3 != 234U)
						{
							break;
						}
						UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO* requestBlob = listenerClientCertAsyncResult.RequestBlob;
						num = num2 + requestBlob->CertEncodedSize;
						listenerClientCertAsyncResult.Reset(num);
					}
					if (num3 != 0U && num3 != 997U)
					{
						throw new HttpListenerException((int)num3);
					}
					return listenerClientCertAsyncResult;
				}
				catch
				{
					if (listenerClientCertAsyncResult != null)
					{
						listenerClientCertAsyncResult.InternalCleanup();
					}
					throw;
				}
			}
			listenerClientCertAsyncResult = new ListenerClientCertAsyncResult(this, state, requestCallback, 0U);
			listenerClientCertAsyncResult.InvokeCallback();
			return listenerClientCertAsyncResult;
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x00078890 File Offset: 0x00077890
		private unsafe void ProcessClientCertificate()
		{
			if (this.m_ClientCertState == ListenerClientCertState.InProgress)
			{
				throw new InvalidOperationException(SR.GetString("net_listener_callinprogress", new object[] { "GetClientCertificate()/BeginGetClientCertificate()" }));
			}
			this.m_ClientCertState = ListenerClientCertState.InProgress;
			if (this.m_SslStatus != HttpListenerRequest.SslStatus.Insecure)
			{
				uint num = 1500U;
				for (;;)
				{
					byte[] array = new byte[checked((int)num)];
					try
					{
						fixed (byte* ptr = array)
						{
							UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO* ptr2 = (UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO*)ptr;
							uint num2 = 0U;
							uint num3 = UnsafeNclNativeMethods.HttpApi.HttpReceiveClientCertificate(this.HttpListenerContext.RequestQueueHandle, this.m_ConnectionId, 0U, ptr2, num, &num2, null);
							if (num3 == 234U)
							{
								num = num2 + ptr2->CertEncodedSize;
								continue;
							}
							if (num3 == 0U && ptr2 != null)
							{
								if (ptr2->pCertEncoded != null)
								{
									try
									{
										byte[] array2 = new byte[ptr2->CertEncodedSize];
										Marshal.Copy((IntPtr)((void*)ptr2->pCertEncoded), array2, 0, array2.Length);
										this.m_ClientCertificate = new X509Certificate2(array2);
									}
									catch (CryptographicException)
									{
									}
									catch (SecurityException)
									{
									}
								}
								this.m_ClientCertificateError = (int)ptr2->CertFlags;
							}
						}
					}
					finally
					{
						byte* ptr = null;
					}
					break;
				}
			}
			this.m_ClientCertState = ListenerClientCertState.Completed;
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x000789CC File Offset: 0x000779CC
		private string RequestScheme
		{
			get
			{
				if (!this.IsSecureConnection)
				{
					return "http://";
				}
				return "https://";
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06001F28 RID: 7976 RVA: 0x000789E4 File Offset: 0x000779E4
		private Uri RequestUri
		{
			get
			{
				if (this.m_RequestUri == null)
				{
					bool flag = false;
					if (!string.IsNullOrEmpty(this.m_CookedUrl))
					{
						flag = Uri.TryCreate(this.m_CookedUrl, UriKind.Absolute, out this.m_RequestUri);
					}
					if (!flag && this.RawUrl != null && this.RawUrl.Length > 0 && this.RawUrl[0] != '/')
					{
						flag = Uri.TryCreate(this.RawUrl, UriKind.Absolute, out this.m_RequestUri);
					}
					if (!flag)
					{
						string text = this.RequestScheme + this.UserHostName;
						if (this.RawUrl != null && this.RawUrl.Length > 0)
						{
							if (this.RawUrl[0] != '/')
							{
								text = text + "/" + this.RawUrl;
							}
							else
							{
								text += this.RawUrl;
							}
						}
						else
						{
							text += "/";
						}
						flag = Uri.TryCreate(text, UriKind.Absolute, out this.m_RequestUri);
					}
				}
				return this.m_RequestUri;
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x00078ADE File Offset: 0x00077ADE
		private string GetKnownHeader(HttpRequestHeader header)
		{
			return UnsafeNclNativeMethods.HttpApi.GetKnownHeader(this.RequestBuffer, this.OriginalBlobAddress, (int)header);
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x00078AF2 File Offset: 0x00077AF2
		internal ChannelBinding GetChannelBinding()
		{
			return this.HttpListenerContext.Listener.GetChannelBindingFromTls(this.m_ConnectionId);
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x00078B0A File Offset: 0x00077B0A
		internal void CheckDisposed()
		{
			if (this.m_IsDisposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x04001E8A RID: 7818
		internal const uint CertBoblSize = 1500U;

		// Token: 0x04001E8B RID: 7819
		private Uri m_RequestUri;

		// Token: 0x04001E8C RID: 7820
		private ulong m_RequestId;

		// Token: 0x04001E8D RID: 7821
		internal ulong m_ConnectionId;

		// Token: 0x04001E8E RID: 7822
		private HttpListenerRequest.SslStatus m_SslStatus;

		// Token: 0x04001E8F RID: 7823
		private string m_RawUrl;

		// Token: 0x04001E90 RID: 7824
		private string m_CookedUrl;

		// Token: 0x04001E91 RID: 7825
		private long m_ContentLength;

		// Token: 0x04001E92 RID: 7826
		private Stream m_RequestStream;

		// Token: 0x04001E93 RID: 7827
		private string m_HttpMethod;

		// Token: 0x04001E94 RID: 7828
		private TriState m_KeepAlive;

		// Token: 0x04001E95 RID: 7829
		private Version m_Version;

		// Token: 0x04001E96 RID: 7830
		private WebHeaderCollection m_WebHeaders;

		// Token: 0x04001E97 RID: 7831
		private IPEndPoint m_LocalEndPoint;

		// Token: 0x04001E98 RID: 7832
		private IPEndPoint m_RemoteEndPoint;

		// Token: 0x04001E99 RID: 7833
		private BoundaryType m_BoundaryType;

		// Token: 0x04001E9A RID: 7834
		private ListenerClientCertState m_ClientCertState;

		// Token: 0x04001E9B RID: 7835
		private X509Certificate2 m_ClientCertificate;

		// Token: 0x04001E9C RID: 7836
		private int m_ClientCertificateError;

		// Token: 0x04001E9D RID: 7837
		private RequestContextBase m_MemoryBlob;

		// Token: 0x04001E9E RID: 7838
		private CookieCollection m_Cookies;

		// Token: 0x04001E9F RID: 7839
		private HttpListenerContext m_HttpContext;

		// Token: 0x04001EA0 RID: 7840
		private bool m_IsDisposed;

		// Token: 0x04001EA1 RID: 7841
		private string m_ServiceName;

		// Token: 0x020003D7 RID: 983
		private enum SslStatus : byte
		{
			// Token: 0x04001EA3 RID: 7843
			Insecure,
			// Token: 0x04001EA4 RID: 7844
			NoClientCert,
			// Token: 0x04001EA5 RID: 7845
			ClientCert
		}

		// Token: 0x020003D8 RID: 984
		private static class Helpers
		{
			// Token: 0x06001F2C RID: 7980 RVA: 0x00078B28 File Offset: 0x00077B28
			internal static string GetAttributeFromHeader(string headerValue, string attrName)
			{
				if (headerValue == null)
				{
					return null;
				}
				int length = headerValue.Length;
				int length2 = attrName.Length;
				int i;
				for (i = 1; i < length; i += length2)
				{
					i = CultureInfo.InvariantCulture.CompareInfo.IndexOf(headerValue, attrName, i, CompareOptions.IgnoreCase);
					if (i < 0 || i + length2 >= length)
					{
						break;
					}
					char c = headerValue[i - 1];
					char c2 = headerValue[i + length2];
					if ((c == ';' || c == ',' || char.IsWhiteSpace(c)) && (c2 == '=' || char.IsWhiteSpace(c2)))
					{
						break;
					}
				}
				if (i < 0 || i >= length)
				{
					return null;
				}
				i += length2;
				while (i < length && char.IsWhiteSpace(headerValue[i]))
				{
					i++;
				}
				if (i >= length || headerValue[i] != '=')
				{
					return null;
				}
				i++;
				while (i < length && char.IsWhiteSpace(headerValue[i]))
				{
					i++;
				}
				if (i >= length)
				{
					return null;
				}
				string text;
				if (i < length && headerValue[i] == '"')
				{
					if (i == length - 1)
					{
						return null;
					}
					int num = headerValue.IndexOf('"', i + 1);
					if (num < 0 || num == i + 1)
					{
						return null;
					}
					text = headerValue.Substring(i + 1, num - i - 1).Trim();
				}
				else
				{
					int num = i;
					while (num < length && headerValue[num] != ' ' && headerValue[num] != ',')
					{
						num++;
					}
					if (num == i)
					{
						return null;
					}
					text = headerValue.Substring(i, num - i).Trim();
				}
				return text;
			}

			// Token: 0x06001F2D RID: 7981 RVA: 0x00078C94 File Offset: 0x00077C94
			internal static string[] ParseMultivalueHeader(string s)
			{
				int num = ((s != null) ? s.Length : 0);
				if (num == 0)
				{
					return null;
				}
				ArrayList arrayList = new ArrayList();
				int i = 0;
				while (i < num)
				{
					int num2 = s.IndexOf(',', i);
					if (num2 < 0)
					{
						num2 = num;
					}
					arrayList.Add(s.Substring(i, num2 - i));
					i = num2 + 1;
					if (i < num && s[i] == ' ')
					{
						i++;
					}
				}
				int count = arrayList.Count;
				if (count == 0)
				{
					return null;
				}
				string[] array = new string[count];
				arrayList.CopyTo(0, array, 0, count);
				return array;
			}

			// Token: 0x06001F2E RID: 7982 RVA: 0x00078D20 File Offset: 0x00077D20
			private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
			{
				int length = s.Length;
				HttpListenerRequest.Helpers.UrlDecoder urlDecoder = new HttpListenerRequest.Helpers.UrlDecoder(length, e);
				int i = 0;
				while (i < length)
				{
					char c = s[i];
					if (c == '+')
					{
						c = ' ';
						goto IL_0106;
					}
					if (c != '%' || i >= length - 2)
					{
						goto IL_0106;
					}
					if (s[i + 1] == 'u' && i < length - 5)
					{
						int num = HttpListenerRequest.Helpers.HexToInt(s[i + 2]);
						int num2 = HttpListenerRequest.Helpers.HexToInt(s[i + 3]);
						int num3 = HttpListenerRequest.Helpers.HexToInt(s[i + 4]);
						int num4 = HttpListenerRequest.Helpers.HexToInt(s[i + 5]);
						if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0)
						{
							goto IL_0106;
						}
						c = (char)((num << 12) | (num2 << 8) | (num3 << 4) | num4);
						i += 5;
						urlDecoder.AddChar(c);
					}
					else
					{
						int num5 = HttpListenerRequest.Helpers.HexToInt(s[i + 1]);
						int num6 = HttpListenerRequest.Helpers.HexToInt(s[i + 2]);
						if (num5 < 0 || num6 < 0)
						{
							goto IL_0106;
						}
						byte b = (byte)((num5 << 4) | num6);
						i += 2;
						urlDecoder.AddByte(b);
					}
					IL_0120:
					i++;
					continue;
					IL_0106:
					if ((c & 'ﾀ') == '\0')
					{
						urlDecoder.AddByte((byte)c);
						goto IL_0120;
					}
					urlDecoder.AddChar(c);
					goto IL_0120;
				}
				return urlDecoder.GetString();
			}

			// Token: 0x06001F2F RID: 7983 RVA: 0x00078E5E File Offset: 0x00077E5E
			private static int HexToInt(char h)
			{
				if (h >= '0' && h <= '9')
				{
					return (int)(h - '0');
				}
				if (h >= 'a' && h <= 'f')
				{
					return (int)(h - 'a' + '\n');
				}
				if (h < 'A' || h > 'F')
				{
					return -1;
				}
				return (int)(h - 'A' + '\n');
			}

			// Token: 0x06001F30 RID: 7984 RVA: 0x00078E94 File Offset: 0x00077E94
			internal static void FillFromString(NameValueCollection nvc, string s, bool urlencoded, Encoding encoding)
			{
				int num = ((s != null) ? s.Length : 0);
				for (int i = ((s.Length > 0 && s[0] == '?') ? 1 : 0); i < num; i++)
				{
					int num2 = i;
					int num3 = -1;
					while (i < num)
					{
						char c = s[i];
						if (c == '=')
						{
							if (num3 < 0)
							{
								num3 = i;
							}
						}
						else if (c == '&')
						{
							break;
						}
						i++;
					}
					string text = null;
					string text2;
					if (num3 >= 0)
					{
						text = s.Substring(num2, num3 - num2);
						text2 = s.Substring(num3 + 1, i - num3 - 1);
					}
					else
					{
						text2 = s.Substring(num2, i - num2);
					}
					if (urlencoded)
					{
						nvc.Add((text == null) ? null : HttpListenerRequest.Helpers.UrlDecodeStringFromStringInternal(text, encoding), HttpListenerRequest.Helpers.UrlDecodeStringFromStringInternal(text2, encoding));
					}
					else
					{
						nvc.Add(text, text2);
					}
					if (i == num - 1 && s[i] == '&')
					{
						nvc.Add(null, "");
					}
				}
			}

			// Token: 0x020003D9 RID: 985
			private class UrlDecoder
			{
				// Token: 0x06001F31 RID: 7985 RVA: 0x00078F80 File Offset: 0x00077F80
				private void FlushBytes()
				{
					if (this._numBytes > 0)
					{
						this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
						this._numBytes = 0;
					}
				}

				// Token: 0x06001F32 RID: 7986 RVA: 0x00078FCE File Offset: 0x00077FCE
				internal UrlDecoder(int bufferSize, Encoding encoding)
				{
					this._bufferSize = bufferSize;
					this._encoding = encoding;
					this._charBuffer = new char[bufferSize];
				}

				// Token: 0x06001F33 RID: 7987 RVA: 0x00078FF0 File Offset: 0x00077FF0
				internal void AddChar(char ch)
				{
					if (this._numBytes > 0)
					{
						this.FlushBytes();
					}
					this._charBuffer[this._numChars++] = ch;
				}

				// Token: 0x06001F34 RID: 7988 RVA: 0x00079028 File Offset: 0x00078028
				internal void AddByte(byte b)
				{
					if (this._byteBuffer == null)
					{
						this._byteBuffer = new byte[this._bufferSize];
					}
					this._byteBuffer[this._numBytes++] = b;
				}

				// Token: 0x06001F35 RID: 7989 RVA: 0x00079067 File Offset: 0x00078067
				internal string GetString()
				{
					if (this._numBytes > 0)
					{
						this.FlushBytes();
					}
					if (this._numChars > 0)
					{
						return new string(this._charBuffer, 0, this._numChars);
					}
					return string.Empty;
				}

				// Token: 0x04001EA6 RID: 7846
				private int _bufferSize;

				// Token: 0x04001EA7 RID: 7847
				private int _numChars;

				// Token: 0x04001EA8 RID: 7848
				private char[] _charBuffer;

				// Token: 0x04001EA9 RID: 7849
				private int _numBytes;

				// Token: 0x04001EAA RID: 7850
				private byte[] _byteBuffer;

				// Token: 0x04001EAB RID: 7851
				private Encoding _encoding;
			}
		}
	}
}
