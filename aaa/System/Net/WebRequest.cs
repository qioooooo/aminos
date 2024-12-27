using System;
using System.Collections;
using System.IO;
using System.Net.Cache;
using System.Net.Configuration;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003AC RID: 940
	[Serializable]
	public abstract class WebRequest : MarshalByRefObject, ISerializable
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001D50 RID: 7504 RVA: 0x00070128 File Offset: 0x0006F128
		private static object InternalSyncObject
		{
			get
			{
				if (WebRequest.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref WebRequest.s_InternalSyncObject, obj, null);
				}
				return WebRequest.s_InternalSyncObject;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x00070154 File Offset: 0x0006F154
		internal static TimerThread.Queue DefaultTimerQueue
		{
			get
			{
				return WebRequest.s_DefaultTimerQueue;
			}
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0007015C File Offset: 0x0006F15C
		private static WebRequest Create(Uri requestUri, bool useUriBase)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "WebRequest", "Create", requestUri.ToString());
			}
			WebRequestPrefixElement webRequestPrefixElement = null;
			bool flag = false;
			string text;
			if (!useUriBase)
			{
				text = requestUri.AbsoluteUri;
			}
			else
			{
				text = requestUri.Scheme + ':';
			}
			int length = text.Length;
			ArrayList prefixList = WebRequest.PrefixList;
			for (int i = 0; i < prefixList.Count; i++)
			{
				webRequestPrefixElement = (WebRequestPrefixElement)prefixList[i];
				if (length >= webRequestPrefixElement.Prefix.Length && string.Compare(webRequestPrefixElement.Prefix, 0, text, 0, webRequestPrefixElement.Prefix.Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				WebRequest webRequest = webRequestPrefixElement.Creator.Create(requestUri);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "WebRequest", "Create", webRequest);
				}
				return webRequest;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, "WebRequest", "Create", null);
			}
			throw new NotSupportedException(SR.GetString("net_unknown_prefix"));
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x0007026D File Offset: 0x0006F26D
		public static WebRequest Create(string requestUriString)
		{
			if (requestUriString == null)
			{
				throw new ArgumentNullException("requestUriString");
			}
			return WebRequest.Create(new Uri(requestUriString), false);
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x00070289 File Offset: 0x0006F289
		public static WebRequest Create(Uri requestUri)
		{
			if (requestUri == null)
			{
				throw new ArgumentNullException("requestUri");
			}
			return WebRequest.Create(requestUri, false);
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x000702A6 File Offset: 0x0006F2A6
		public static WebRequest CreateDefault(Uri requestUri)
		{
			if (requestUri == null)
			{
				throw new ArgumentNullException("requestUri");
			}
			return WebRequest.Create(requestUri, true);
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x000702C4 File Offset: 0x0006F2C4
		public static bool RegisterPrefix(string prefix, IWebRequestCreate creator)
		{
			bool flag = false;
			if (prefix == null)
			{
				throw new ArgumentNullException("prefix");
			}
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			lock (WebRequest.InternalSyncObject)
			{
				ArrayList arrayList = (ArrayList)WebRequest.PrefixList.Clone();
				int i;
				for (i = 0; i < arrayList.Count; i++)
				{
					WebRequestPrefixElement webRequestPrefixElement = (WebRequestPrefixElement)arrayList[i];
					if (prefix.Length > webRequestPrefixElement.Prefix.Length)
					{
						break;
					}
					if (prefix.Length == webRequestPrefixElement.Prefix.Length && string.Compare(webRequestPrefixElement.Prefix, prefix, StringComparison.OrdinalIgnoreCase) == 0)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					arrayList.Insert(i, new WebRequestPrefixElement(prefix, creator));
					WebRequest.PrefixList = arrayList;
				}
			}
			return !flag;
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001D57 RID: 7511 RVA: 0x000703A4 File Offset: 0x0006F3A4
		// (set) Token: 0x06001D58 RID: 7512 RVA: 0x000703F8 File Offset: 0x0006F3F8
		internal static ArrayList PrefixList
		{
			get
			{
				if (WebRequest.s_PrefixList == null)
				{
					lock (WebRequest.InternalSyncObject)
					{
						if (WebRequest.s_PrefixList == null)
						{
							WebRequest.s_PrefixList = WebRequestModulesSectionInternal.GetSection().WebRequestModules;
						}
					}
				}
				return WebRequest.s_PrefixList;
			}
			set
			{
				WebRequest.s_PrefixList = value;
			}
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x00070400 File Offset: 0x0006F400
		protected WebRequest()
		{
			this.m_ImpersonationLevel = TokenImpersonationLevel.Delegation;
			this.m_AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x00070416 File Offset: 0x0006F416
		protected WebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x0007041E File Offset: 0x0006F41E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x00070428 File Offset: 0x0006F428
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001D5D RID: 7517 RVA: 0x0007042A File Offset: 0x0006F42A
		// (set) Token: 0x06001D5E RID: 7518 RVA: 0x0007043C File Offset: 0x0006F43C
		public static RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				return RequestCacheManager.GetBinding(string.Empty).Policy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				RequestCacheBinding binding = RequestCacheManager.GetBinding(string.Empty);
				RequestCacheManager.SetBinding(string.Empty, new RequestCacheBinding(binding.Cache, binding.Validator, value));
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001D5F RID: 7519 RVA: 0x0007047A File Offset: 0x0006F47A
		// (set) Token: 0x06001D60 RID: 7520 RVA: 0x00070482 File Offset: 0x0006F482
		public virtual RequestCachePolicy CachePolicy
		{
			get
			{
				return this.m_CachePolicy;
			}
			set
			{
				this.InternalSetCachePolicy(value);
			}
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x0007048C File Offset: 0x0006F48C
		private void InternalSetCachePolicy(RequestCachePolicy policy)
		{
			if (this.m_CacheBinding != null && this.m_CacheBinding.Cache != null && this.m_CacheBinding.Validator != null && this.CacheProtocol == null && policy != null && policy.Level != RequestCacheLevel.BypassCache)
			{
				this.CacheProtocol = new RequestCacheProtocol(this.m_CacheBinding.Cache, this.m_CacheBinding.Validator.CreateValidator());
			}
			this.m_CachePolicy = policy;
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001D62 RID: 7522 RVA: 0x000704FC File Offset: 0x0006F4FC
		// (set) Token: 0x06001D63 RID: 7523 RVA: 0x00070503 File Offset: 0x0006F503
		public virtual string Method
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001D64 RID: 7524 RVA: 0x0007050A File Offset: 0x0006F50A
		public virtual Uri RequestUri
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001D65 RID: 7525 RVA: 0x00070511 File Offset: 0x0006F511
		// (set) Token: 0x06001D66 RID: 7526 RVA: 0x00070518 File Offset: 0x0006F518
		public virtual string ConnectionGroupName
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001D67 RID: 7527 RVA: 0x0007051F File Offset: 0x0006F51F
		// (set) Token: 0x06001D68 RID: 7528 RVA: 0x00070526 File Offset: 0x0006F526
		public virtual WebHeaderCollection Headers
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x0007052D File Offset: 0x0006F52D
		// (set) Token: 0x06001D6A RID: 7530 RVA: 0x00070534 File Offset: 0x0006F534
		public virtual long ContentLength
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x0007053B File Offset: 0x0006F53B
		// (set) Token: 0x06001D6C RID: 7532 RVA: 0x00070542 File Offset: 0x0006F542
		public virtual string ContentType
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x00070549 File Offset: 0x0006F549
		// (set) Token: 0x06001D6E RID: 7534 RVA: 0x00070550 File Offset: 0x0006F550
		public virtual ICredentials Credentials
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x00070557 File Offset: 0x0006F557
		// (set) Token: 0x06001D70 RID: 7536 RVA: 0x0007055E File Offset: 0x0006F55E
		public virtual bool UseDefaultCredentials
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x00070565 File Offset: 0x0006F565
		// (set) Token: 0x06001D72 RID: 7538 RVA: 0x0007056C File Offset: 0x0006F56C
		public virtual IWebProxy Proxy
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x00070573 File Offset: 0x0006F573
		// (set) Token: 0x06001D74 RID: 7540 RVA: 0x0007057A File Offset: 0x0006F57A
		public virtual bool PreAuthenticate
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x00070581 File Offset: 0x0006F581
		// (set) Token: 0x06001D76 RID: 7542 RVA: 0x00070588 File Offset: 0x0006F588
		public virtual int Timeout
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x0007058F File Offset: 0x0006F58F
		public virtual Stream GetRequestStream()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x00070596 File Offset: 0x0006F596
		public virtual WebResponse GetResponse()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x0007059D File Offset: 0x0006F59D
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000705A4 File Offset: 0x0006F5A4
		public virtual WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x000705AB File Offset: 0x0006F5AB
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x000705B2 File Offset: 0x0006F5B2
		public virtual Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x000705B9 File Offset: 0x0006F5B9
		public virtual void Abort()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001D7E RID: 7550 RVA: 0x000705C0 File Offset: 0x0006F5C0
		// (set) Token: 0x06001D7F RID: 7551 RVA: 0x000705C8 File Offset: 0x0006F5C8
		internal RequestCacheProtocol CacheProtocol
		{
			get
			{
				return this.m_CacheProtocol;
			}
			set
			{
				this.m_CacheProtocol = value;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x000705D1 File Offset: 0x0006F5D1
		// (set) Token: 0x06001D81 RID: 7553 RVA: 0x000705D9 File Offset: 0x0006F5D9
		public AuthenticationLevel AuthenticationLevel
		{
			get
			{
				return this.m_AuthenticationLevel;
			}
			set
			{
				this.m_AuthenticationLevel = value;
			}
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000705E2 File Offset: 0x0006F5E2
		internal virtual ContextAwareResult GetConnectingContext()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x000705E9 File Offset: 0x0006F5E9
		internal virtual ContextAwareResult GetWritingContext()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x000705F0 File Offset: 0x0006F5F0
		internal virtual ContextAwareResult GetReadingContext()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001D85 RID: 7557 RVA: 0x000705F7 File Offset: 0x0006F5F7
		// (set) Token: 0x06001D86 RID: 7558 RVA: 0x000705FF File Offset: 0x0006F5FF
		public TokenImpersonationLevel ImpersonationLevel
		{
			get
			{
				return this.m_ImpersonationLevel;
			}
			set
			{
				this.m_ImpersonationLevel = value;
			}
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x00070608 File Offset: 0x0006F608
		internal virtual void RequestCallback(object obj)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x00070610 File Offset: 0x0006F610
		// (set) Token: 0x06001D89 RID: 7561 RVA: 0x00070670 File Offset: 0x0006F670
		internal static IWebProxy InternalDefaultWebProxy
		{
			get
			{
				if (!WebRequest.s_DefaultWebProxyInitialized)
				{
					lock (WebRequest.InternalSyncObject)
					{
						if (!WebRequest.s_DefaultWebProxyInitialized)
						{
							DefaultProxySectionInternal section = DefaultProxySectionInternal.GetSection();
							if (section != null)
							{
								WebRequest.s_DefaultWebProxy = section.WebProxy;
							}
							WebRequest.s_DefaultWebProxyInitialized = true;
						}
					}
				}
				return WebRequest.s_DefaultWebProxy;
			}
			set
			{
				if (!WebRequest.s_DefaultWebProxyInitialized)
				{
					lock (WebRequest.InternalSyncObject)
					{
						WebRequest.s_DefaultWebProxy = value;
						WebRequest.s_DefaultWebProxyInitialized = true;
						return;
					}
				}
				WebRequest.s_DefaultWebProxy = value;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x000706BC File Offset: 0x0006F6BC
		// (set) Token: 0x06001D8B RID: 7563 RVA: 0x000706CD File Offset: 0x0006F6CD
		public static IWebProxy DefaultWebProxy
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return WebRequest.InternalDefaultWebProxy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				WebRequest.InternalDefaultWebProxy = value;
			}
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x000706DF File Offset: 0x0006F6DF
		public static IWebProxy GetSystemWebProxy()
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			return WebRequest.InternalGetSystemWebProxy();
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000706F0 File Offset: 0x0006F6F0
		internal static IWebProxy InternalGetSystemWebProxy()
		{
			return new WebRequest.WebProxyWrapperOpaque(new WebProxy(true));
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x000706FD File Offset: 0x0006F6FD
		internal void SetupCacheProtocol(Uri uri)
		{
			this.m_CacheBinding = RequestCacheManager.GetBinding(uri.Scheme);
			this.InternalSetCachePolicy(this.m_CacheBinding.Policy);
			if (this.m_CachePolicy == null)
			{
				this.InternalSetCachePolicy(WebRequest.DefaultCachePolicy);
			}
		}

		// Token: 0x04001D7F RID: 7551
		internal const int DefaultTimeout = 100000;

		// Token: 0x04001D80 RID: 7552
		private static ArrayList s_PrefixList;

		// Token: 0x04001D81 RID: 7553
		private static object s_InternalSyncObject;

		// Token: 0x04001D82 RID: 7554
		private static TimerThread.Queue s_DefaultTimerQueue = TimerThread.CreateQueue(100000);

		// Token: 0x04001D83 RID: 7555
		private AuthenticationLevel m_AuthenticationLevel;

		// Token: 0x04001D84 RID: 7556
		private TokenImpersonationLevel m_ImpersonationLevel;

		// Token: 0x04001D85 RID: 7557
		private RequestCachePolicy m_CachePolicy;

		// Token: 0x04001D86 RID: 7558
		private RequestCacheProtocol m_CacheProtocol;

		// Token: 0x04001D87 RID: 7559
		private RequestCacheBinding m_CacheBinding;

		// Token: 0x04001D88 RID: 7560
		private static IWebProxy s_DefaultWebProxy;

		// Token: 0x04001D89 RID: 7561
		private static bool s_DefaultWebProxyInitialized;

		// Token: 0x020003AF RID: 943
		internal class WebProxyWrapperOpaque : IAutoWebProxy, IWebProxy
		{
			// Token: 0x06001D95 RID: 7573 RVA: 0x00070745 File Offset: 0x0006F745
			internal WebProxyWrapperOpaque(WebProxy webProxy)
			{
				this.webProxy = webProxy;
			}

			// Token: 0x06001D96 RID: 7574 RVA: 0x00070754 File Offset: 0x0006F754
			public Uri GetProxy(Uri destination)
			{
				return this.webProxy.GetProxy(destination);
			}

			// Token: 0x06001D97 RID: 7575 RVA: 0x00070762 File Offset: 0x0006F762
			public bool IsBypassed(Uri host)
			{
				return this.webProxy.IsBypassed(host);
			}

			// Token: 0x170005C5 RID: 1477
			// (get) Token: 0x06001D98 RID: 7576 RVA: 0x00070770 File Offset: 0x0006F770
			// (set) Token: 0x06001D99 RID: 7577 RVA: 0x0007077D File Offset: 0x0006F77D
			public ICredentials Credentials
			{
				get
				{
					return this.webProxy.Credentials;
				}
				set
				{
					this.webProxy.Credentials = value;
				}
			}

			// Token: 0x06001D9A RID: 7578 RVA: 0x0007078B File Offset: 0x0006F78B
			public ProxyChain GetProxies(Uri destination)
			{
				return ((IAutoWebProxy)this.webProxy).GetProxies(destination);
			}

			// Token: 0x04001D8A RID: 7562
			protected readonly WebProxy webProxy;
		}

		// Token: 0x020003B0 RID: 944
		internal class WebProxyWrapper : WebRequest.WebProxyWrapperOpaque
		{
			// Token: 0x06001D9B RID: 7579 RVA: 0x00070799 File Offset: 0x0006F799
			internal WebProxyWrapper(WebProxy webProxy)
				: base(webProxy)
			{
			}

			// Token: 0x170005C6 RID: 1478
			// (get) Token: 0x06001D9C RID: 7580 RVA: 0x000707A2 File Offset: 0x0006F7A2
			internal WebProxy WebProxy
			{
				get
				{
					return this.webProxy;
				}
			}
		}
	}
}
