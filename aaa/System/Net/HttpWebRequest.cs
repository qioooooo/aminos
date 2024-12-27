using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Cache;
using System.Net.Configuration;
using System.Net.Security;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003E1 RID: 993
	[Serializable]
	public class HttpWebRequest : WebRequest, ISerializable
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x0007A478 File Offset: 0x00079478
		internal TimerThread.Timer RequestTimer
		{
			get
			{
				return this._Timer;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x0007A480 File Offset: 0x00079480
		internal bool Aborted
		{
			get
			{
				return this.m_Aborted != 0;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001F6F RID: 8047 RVA: 0x0007A48E File Offset: 0x0007948E
		// (set) Token: 0x06001F70 RID: 8048 RVA: 0x0007A49E File Offset: 0x0007949E
		public bool AllowAutoRedirect
		{
			get
			{
				return (this._Booleans & HttpWebRequest.Booleans.AllowAutoRedirect) != (HttpWebRequest.Booleans)0U;
			}
			set
			{
				if (value)
				{
					this._Booleans |= HttpWebRequest.Booleans.AllowAutoRedirect;
					return;
				}
				this._Booleans &= ~HttpWebRequest.Booleans.AllowAutoRedirect;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001F71 RID: 8049 RVA: 0x0007A4C1 File Offset: 0x000794C1
		// (set) Token: 0x06001F72 RID: 8050 RVA: 0x0007A4D1 File Offset: 0x000794D1
		public bool AllowWriteStreamBuffering
		{
			get
			{
				return (this._Booleans & HttpWebRequest.Booleans.AllowWriteStreamBuffering) != (HttpWebRequest.Booleans)0U;
			}
			set
			{
				if (value)
				{
					this._Booleans |= HttpWebRequest.Booleans.AllowWriteStreamBuffering;
					return;
				}
				this._Booleans &= ~HttpWebRequest.Booleans.AllowWriteStreamBuffering;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x0007A4F4 File Offset: 0x000794F4
		// (set) Token: 0x06001F74 RID: 8052 RVA: 0x0007A504 File Offset: 0x00079504
		private bool ExpectContinue
		{
			get
			{
				return (this._Booleans & HttpWebRequest.Booleans.ExpectContinue) != (HttpWebRequest.Booleans)0U;
			}
			set
			{
				if (value)
				{
					this._Booleans |= HttpWebRequest.Booleans.ExpectContinue;
					return;
				}
				this._Booleans &= ~HttpWebRequest.Booleans.ExpectContinue;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001F75 RID: 8053 RVA: 0x0007A527 File Offset: 0x00079527
		public bool HaveResponse
		{
			get
			{
				return this._ReadAResult != null && this._ReadAResult.InternalPeekCompleted;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001F76 RID: 8054 RVA: 0x0007A53E File Offset: 0x0007953E
		// (set) Token: 0x06001F77 RID: 8055 RVA: 0x0007A546 File Offset: 0x00079546
		internal bool NtlmKeepAlive
		{
			get
			{
				return this.m_NtlmKeepAlive;
			}
			set
			{
				this.m_NtlmKeepAlive = value;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001F78 RID: 8056 RVA: 0x0007A54F File Offset: 0x0007954F
		// (set) Token: 0x06001F79 RID: 8057 RVA: 0x0007A557 File Offset: 0x00079557
		internal bool SawInitialResponse
		{
			get
			{
				return this.m_SawInitialResponse;
			}
			set
			{
				this.m_SawInitialResponse = value;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001F7A RID: 8058 RVA: 0x0007A560 File Offset: 0x00079560
		internal bool BodyStarted
		{
			get
			{
				return this.m_BodyStarted;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001F7B RID: 8059 RVA: 0x0007A568 File Offset: 0x00079568
		// (set) Token: 0x06001F7C RID: 8060 RVA: 0x0007A570 File Offset: 0x00079570
		public bool KeepAlive
		{
			get
			{
				return this.m_KeepAlive;
			}
			set
			{
				this.m_KeepAlive = value;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x0007A579 File Offset: 0x00079579
		// (set) Token: 0x06001F7E RID: 8062 RVA: 0x0007A581 File Offset: 0x00079581
		internal bool LockConnection
		{
			get
			{
				return this.m_LockConnection;
			}
			set
			{
				this.m_LockConnection = value;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001F7F RID: 8063 RVA: 0x0007A58A File Offset: 0x0007958A
		// (set) Token: 0x06001F80 RID: 8064 RVA: 0x0007A592 File Offset: 0x00079592
		public bool Pipelined
		{
			get
			{
				return this.m_Pipelined;
			}
			set
			{
				this.m_Pipelined = value;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001F81 RID: 8065 RVA: 0x0007A59B File Offset: 0x0007959B
		// (set) Token: 0x06001F82 RID: 8066 RVA: 0x0007A5A3 File Offset: 0x000795A3
		public override bool PreAuthenticate
		{
			get
			{
				return this.m_PreAuthenticate;
			}
			set
			{
				this.m_PreAuthenticate = value;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001F83 RID: 8067 RVA: 0x0007A5AC File Offset: 0x000795AC
		// (set) Token: 0x06001F84 RID: 8068 RVA: 0x0007A5BD File Offset: 0x000795BD
		private bool ProxySet
		{
			get
			{
				return (this._Booleans & HttpWebRequest.Booleans.ProxySet) != (HttpWebRequest.Booleans)0U;
			}
			set
			{
				if (value)
				{
					this._Booleans |= HttpWebRequest.Booleans.ProxySet;
					return;
				}
				this._Booleans &= ~HttpWebRequest.Booleans.ProxySet;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x0007A5E1 File Offset: 0x000795E1
		private bool RequestSubmitted
		{
			get
			{
				return this.m_RequestSubmitted;
			}
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x0007A5EC File Offset: 0x000795EC
		private bool SetRequestSubmitted()
		{
			bool requestSubmitted = this.RequestSubmitted;
			this.m_RequestSubmitted = true;
			return requestSubmitted;
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x0007A608 File Offset: 0x00079608
		// (set) Token: 0x06001F88 RID: 8072 RVA: 0x0007A610 File Offset: 0x00079610
		internal bool Saw100Continue
		{
			get
			{
				return this.m_Saw100Continue;
			}
			set
			{
				this.m_Saw100Continue = value;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x0007A619 File Offset: 0x00079619
		// (set) Token: 0x06001F8A RID: 8074 RVA: 0x0007A62A File Offset: 0x0007962A
		public bool UnsafeAuthenticatedConnectionSharing
		{
			get
			{
				return (this._Booleans & HttpWebRequest.Booleans.UnsafeAuthenticatedConnectionSharing) != (HttpWebRequest.Booleans)0U;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (value)
				{
					this._Booleans |= HttpWebRequest.Booleans.UnsafeAuthenticatedConnectionSharing;
					return;
				}
				this._Booleans &= ~HttpWebRequest.Booleans.UnsafeAuthenticatedConnectionSharing;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001F8B RID: 8075 RVA: 0x0007A658 File Offset: 0x00079658
		internal bool UnsafeOrProxyAuthenticatedConnectionSharing
		{
			get
			{
				return this.m_IsCurrentAuthenticationStateProxy || this.UnsafeAuthenticatedConnectionSharing;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x0007A66A File Offset: 0x0007966A
		// (set) Token: 0x06001F8D RID: 8077 RVA: 0x0007A67E File Offset: 0x0007967E
		private bool IsVersionHttp10
		{
			get
			{
				return (this._Booleans & HttpWebRequest.Booleans.IsVersionHttp10) != (HttpWebRequest.Booleans)0U;
			}
			set
			{
				if (value)
				{
					this._Booleans |= HttpWebRequest.Booleans.IsVersionHttp10;
					return;
				}
				this._Booleans &= ~HttpWebRequest.Booleans.IsVersionHttp10;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x0007A6A8 File Offset: 0x000796A8
		// (set) Token: 0x06001F8F RID: 8079 RVA: 0x0007A6BC File Offset: 0x000796BC
		public bool SendChunked
		{
			get
			{
				return (this._Booleans & HttpWebRequest.Booleans.SendChunked) != (HttpWebRequest.Booleans)0U;
			}
			set
			{
				if (this.RequestSubmitted)
				{
					throw new InvalidOperationException(SR.GetString("net_writestarted"));
				}
				if (value)
				{
					this._Booleans |= HttpWebRequest.Booleans.SendChunked;
					return;
				}
				this._Booleans &= ~HttpWebRequest.Booleans.SendChunked;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x0007A709 File Offset: 0x00079709
		// (set) Token: 0x06001F91 RID: 8081 RVA: 0x0007A711 File Offset: 0x00079711
		public DecompressionMethods AutomaticDecompression
		{
			get
			{
				return this.m_AutomaticDecompression;
			}
			set
			{
				if (this.RequestSubmitted)
				{
					throw new InvalidOperationException(SR.GetString("net_writestarted"));
				}
				this.m_AutomaticDecompression = value;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001F92 RID: 8082 RVA: 0x0007A732 File Offset: 0x00079732
		// (set) Token: 0x06001F93 RID: 8083 RVA: 0x0007A73A File Offset: 0x0007973A
		internal HttpWriteMode HttpWriteMode
		{
			get
			{
				return this._HttpWriteMode;
			}
			set
			{
				this._HttpWriteMode = value;
			}
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0007A743 File Offset: 0x00079743
		internal string AuthHeader(HttpResponseHeader header)
		{
			if (this._HttpResponse == null)
			{
				return null;
			}
			return this._HttpResponse.Headers[header];
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x0007A760 File Offset: 0x00079760
		// (set) Token: 0x06001F96 RID: 8086 RVA: 0x0007A788 File Offset: 0x00079788
		public new static RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				RequestCachePolicy policy = RequestCacheManager.GetBinding(Uri.UriSchemeHttp).Policy;
				if (policy == null)
				{
					return WebRequest.DefaultCachePolicy;
				}
				return policy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				RequestCacheBinding binding = RequestCacheManager.GetBinding(Uri.UriSchemeHttp);
				RequestCacheManager.SetBinding(Uri.UriSchemeHttp, new RequestCacheBinding(binding.Cache, binding.Validator, value));
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001F97 RID: 8087 RVA: 0x0007A7C6 File Offset: 0x000797C6
		// (set) Token: 0x06001F98 RID: 8088 RVA: 0x0007A7D2 File Offset: 0x000797D2
		public static int DefaultMaximumResponseHeadersLength
		{
			get
			{
				return SettingsSectionInternal.Section.MaximumResponseHeadersLength;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_toosmall"));
				}
				SettingsSectionInternal.Section.MaximumResponseHeadersLength = value;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x0007A801 File Offset: 0x00079801
		// (set) Token: 0x06001F9A RID: 8090 RVA: 0x0007A80D File Offset: 0x0007980D
		public static int DefaultMaximumErrorResponseLength
		{
			get
			{
				return SettingsSectionInternal.Section.MaximumErrorResponseLength;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_toosmall"));
				}
				SettingsSectionInternal.Section.MaximumErrorResponseLength = value;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x0007A83C File Offset: 0x0007983C
		// (set) Token: 0x06001F9C RID: 8092 RVA: 0x0007A844 File Offset: 0x00079844
		public int MaximumResponseHeadersLength
		{
			get
			{
				return this._MaximumResponseHeadersLength;
			}
			set
			{
				if (this.RequestSubmitted)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_toosmall"));
				}
				this._MaximumResponseHeadersLength = value;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (set) Token: 0x06001F9D RID: 8093 RVA: 0x0007A87D File Offset: 0x0007987D
		internal HttpAbortDelegate AbortDelegate
		{
			set
			{
				this._AbortDelegate = value;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001F9E RID: 8094 RVA: 0x0007A886 File Offset: 0x00079886
		internal LazyAsyncResult ConnectionAsyncResult
		{
			get
			{
				return this._ConnectionAResult;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x0007A88E File Offset: 0x0007988E
		internal LazyAsyncResult ConnectionReaderAsyncResult
		{
			get
			{
				return this._ConnectionReaderAResult;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x0007A896 File Offset: 0x00079896
		private bool UserRetrievedWriteStream
		{
			get
			{
				return this._WriteAResult != null && this._WriteAResult.InternalPeekCompleted;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x0007A8AD File Offset: 0x000798AD
		// (set) Token: 0x06001FA2 RID: 8098 RVA: 0x0007A8BB File Offset: 0x000798BB
		internal bool Async
		{
			get
			{
				return this._RequestIsAsync != TriState.False;
			}
			set
			{
				if (this._RequestIsAsync == TriState.Unspecified)
				{
					this._RequestIsAsync = (value ? TriState.True : TriState.False);
				}
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x0007A8D3 File Offset: 0x000798D3
		// (set) Token: 0x06001FA4 RID: 8100 RVA: 0x0007A8DB File Offset: 0x000798DB
		internal UnlockConnectionDelegate UnlockConnectionDelegate
		{
			get
			{
				return this._UnlockDelegate;
			}
			set
			{
				this._UnlockDelegate = value;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x0007A8E4 File Offset: 0x000798E4
		private bool UsesProxy
		{
			get
			{
				return this.ServicePoint.InternalProxyServicePoint;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x0007A8F1 File Offset: 0x000798F1
		internal HttpStatusCode ResponseStatusCode
		{
			get
			{
				return this._HttpResponse.StatusCode;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x0007A8FE File Offset: 0x000798FE
		internal bool UsesProxySemantics
		{
			get
			{
				return this.ServicePoint.InternalProxyServicePoint && (this._Uri.Scheme != Uri.UriSchemeHttps || this.IsTunnelRequest);
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x0007A929 File Offset: 0x00079929
		internal Uri ChallengedUri
		{
			get
			{
				return this.CurrentAuthenticationState.ChallengedUri;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x0007A936 File Offset: 0x00079936
		internal AuthenticationState ProxyAuthenticationState
		{
			get
			{
				if (this._ProxyAuthenticationState == null)
				{
					this._ProxyAuthenticationState = new AuthenticationState(true);
				}
				return this._ProxyAuthenticationState;
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x0007A952 File Offset: 0x00079952
		// (set) Token: 0x06001FAB RID: 8107 RVA: 0x0007A96E File Offset: 0x0007996E
		internal AuthenticationState ServerAuthenticationState
		{
			get
			{
				if (this._ServerAuthenticationState == null)
				{
					this._ServerAuthenticationState = new AuthenticationState(false);
				}
				return this._ServerAuthenticationState;
			}
			set
			{
				this._ServerAuthenticationState = value;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06001FAC RID: 8108 RVA: 0x0007A977 File Offset: 0x00079977
		// (set) Token: 0x06001FAD RID: 8109 RVA: 0x0007A98E File Offset: 0x0007998E
		internal AuthenticationState CurrentAuthenticationState
		{
			get
			{
				if (!this.m_IsCurrentAuthenticationStateProxy)
				{
					return this._ServerAuthenticationState;
				}
				return this._ProxyAuthenticationState;
			}
			set
			{
				this.m_IsCurrentAuthenticationStateProxy = this._ProxyAuthenticationState == value;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001FAE RID: 8110 RVA: 0x0007A99F File Offset: 0x0007999F
		// (set) Token: 0x06001FAF RID: 8111 RVA: 0x0007A9BA File Offset: 0x000799BA
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this._ClientCertificates == null)
				{
					this._ClientCertificates = new X509CertificateCollection();
				}
				return this._ClientCertificates;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._ClientCertificates = value;
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x0007A9D1 File Offset: 0x000799D1
		// (set) Token: 0x06001FB1 RID: 8113 RVA: 0x0007A9D9 File Offset: 0x000799D9
		public CookieContainer CookieContainer
		{
			get
			{
				return this._CookieContainer;
			}
			set
			{
				this._CookieContainer = value;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x0007A9E2 File Offset: 0x000799E2
		public override Uri RequestUri
		{
			get
			{
				return this._OriginUri;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001FB3 RID: 8115 RVA: 0x0007A9EA File Offset: 0x000799EA
		// (set) Token: 0x06001FB4 RID: 8116 RVA: 0x0007A9F2 File Offset: 0x000799F2
		public override long ContentLength
		{
			get
			{
				return this._ContentLength;
			}
			set
			{
				if (this.RequestSubmitted)
				{
					throw new InvalidOperationException(SR.GetString("net_writestarted"));
				}
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_clsmall"));
				}
				this._ContentLength = value;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06001FB5 RID: 8117 RVA: 0x0007AA28 File Offset: 0x00079A28
		// (set) Token: 0x06001FB6 RID: 8118 RVA: 0x0007AA30 File Offset: 0x00079A30
		public override int Timeout
		{
			get
			{
				return this._Timeout;
			}
			set
			{
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_ge_zero"));
				}
				if (this._Timeout != value)
				{
					this._Timeout = value;
					this._TimerQueue = null;
				}
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06001FB7 RID: 8119 RVA: 0x0007AA64 File Offset: 0x00079A64
		private TimerThread.Queue TimerQueue
		{
			get
			{
				TimerThread.Queue queue = this._TimerQueue;
				if (queue == null)
				{
					queue = TimerThread.GetOrCreateQueue((this._Timeout == 0) ? 1 : this._Timeout);
					this._TimerQueue = queue;
				}
				return queue;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06001FB8 RID: 8120 RVA: 0x0007AA9A File Offset: 0x00079A9A
		// (set) Token: 0x06001FB9 RID: 8121 RVA: 0x0007AAA2 File Offset: 0x00079AA2
		public int ReadWriteTimeout
		{
			get
			{
				return this._ReadWriteTimeout;
			}
			set
			{
				if (this.RequestSubmitted)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_gt_zero"));
				}
				this._ReadWriteTimeout = value;
			}
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0007AADC File Offset: 0x00079ADC
		internal long SwitchToContentLength()
		{
			if (this.HaveResponse)
			{
				return -1L;
			}
			if (this.HttpWriteMode == HttpWriteMode.Chunked)
			{
				ConnectStream connectStream = this._OldSubmitWriteStream;
				if (connectStream == null)
				{
					connectStream = this._SubmitWriteStream;
				}
				if (connectStream.Connection != null && connectStream.Connection.IISVersion >= 6)
				{
					return -1L;
				}
			}
			long num = -1L;
			long contentLength = this._ContentLength;
			if (this.HttpWriteMode != HttpWriteMode.None)
			{
				if (this.HttpWriteMode == HttpWriteMode.Buffer)
				{
					this._ContentLength = (long)this._SubmitWriteStream.BufferedData.Length;
					this.m_OriginallyBuffered = true;
					this.HttpWriteMode = HttpWriteMode.ContentLength;
					return -1L;
				}
				if (this.NtlmKeepAlive && this._OldSubmitWriteStream == null)
				{
					this._ContentLength = 0L;
					this._SubmitWriteStream.SuppressWrite = true;
					if (!this._SubmitWriteStream.BufferOnly)
					{
						num = contentLength;
					}
					if (this.HttpWriteMode == HttpWriteMode.Chunked)
					{
						this.HttpWriteMode = HttpWriteMode.ContentLength;
						this._SubmitWriteStream.SwitchToContentLength();
						num = -2L;
						this._HttpRequestHeaders.RemoveInternal("Transfer-Encoding");
					}
				}
				if (this._OldSubmitWriteStream != null)
				{
					if (this.NtlmKeepAlive)
					{
						this._ContentLength = 0L;
					}
					else if (this._ContentLength == 0L || this.HttpWriteMode == HttpWriteMode.Chunked)
					{
						this._ContentLength = (long)this._OldSubmitWriteStream.BufferedData.Length;
					}
					if (this.HttpWriteMode == HttpWriteMode.Chunked)
					{
						this.HttpWriteMode = HttpWriteMode.ContentLength;
						this._SubmitWriteStream.SwitchToContentLength();
						this._HttpRequestHeaders.RemoveInternal("Transfer-Encoding");
					}
				}
			}
			return num;
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0007AC40 File Offset: 0x00079C40
		private void PostSwitchToContentLength(long value)
		{
			if (value > -1L)
			{
				this._ContentLength = value;
			}
			if (value == -2L)
			{
				this._ContentLength = -1L;
				this.HttpWriteMode = HttpWriteMode.Chunked;
			}
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0007AC64 File Offset: 0x00079C64
		private void ClearAuthenticatedConnectionResources()
		{
			if (this.ProxyAuthenticationState.UniqueGroupId != null || this.ServerAuthenticationState.UniqueGroupId != null)
			{
				this.ServicePoint.ReleaseConnectionGroup(this.GetConnectionGroupLine());
			}
			UnlockConnectionDelegate unlockConnectionDelegate = this.UnlockConnectionDelegate;
			try
			{
				if (unlockConnectionDelegate != null)
				{
					unlockConnectionDelegate();
				}
				this.UnlockConnectionDelegate = null;
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
			}
			catch
			{
			}
			this.ProxyAuthenticationState.ClearSession(this);
			this.ServerAuthenticationState.ClearSession(this);
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06001FBD RID: 8125 RVA: 0x0007ACFC File Offset: 0x00079CFC
		// (set) Token: 0x06001FBE RID: 8126 RVA: 0x0007AD04 File Offset: 0x00079D04
		internal bool HeadersCompleted
		{
			get
			{
				return this.m_HeadersCompleted;
			}
			set
			{
				this.m_HeadersCompleted = value;
			}
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0007AD10 File Offset: 0x00079D10
		private void CheckProtocol(bool onRequestStream)
		{
			if (!this.CanGetRequestStream)
			{
				if (onRequestStream)
				{
					throw new ProtocolViolationException(SR.GetString("net_nouploadonget"));
				}
				if (this.HttpWriteMode != HttpWriteMode.Unknown && this.HttpWriteMode != HttpWriteMode.None)
				{
					throw new ProtocolViolationException(SR.GetString("net_nocontentlengthonget"));
				}
				this.HttpWriteMode = HttpWriteMode.None;
			}
			else if (this.HttpWriteMode == HttpWriteMode.Unknown)
			{
				if (this.SendChunked)
				{
					if (this.ServicePoint.HttpBehaviour == HttpBehaviour.HTTP11 || this.ServicePoint.HttpBehaviour == HttpBehaviour.Unknown)
					{
						this.HttpWriteMode = HttpWriteMode.Chunked;
					}
					else
					{
						if (!this.AllowWriteStreamBuffering)
						{
							throw new ProtocolViolationException(SR.GetString("net_nochunkuploadonhttp10"));
						}
						this.HttpWriteMode = HttpWriteMode.Buffer;
					}
				}
				else
				{
					this.HttpWriteMode = ((this.ContentLength >= 0L) ? HttpWriteMode.ContentLength : (onRequestStream ? HttpWriteMode.Buffer : HttpWriteMode.None));
				}
			}
			if (this.HttpWriteMode != HttpWriteMode.Chunked)
			{
				if (onRequestStream && this.ContentLength == -1L && !this.AllowWriteStreamBuffering && this.KeepAlive)
				{
					throw new ProtocolViolationException(SR.GetString("net_contentlengthmissing"));
				}
				if (!ValidationHelper.IsBlankString(this.TransferEncoding))
				{
					throw new InvalidOperationException(SR.GetString("net_needchunked"));
				}
			}
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0007AE24 File Offset: 0x00079E24
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "BeginGetRequestStream", "");
			}
			this.CheckProtocol(true);
			ContextAwareResult contextAwareResult = new ContextAwareResult(this.IdentityRequired, true, this, state, callback);
			lock (contextAwareResult.StartPostingAsyncOp())
			{
				if (this._WriteAResult != null && this._WriteAResult.InternalPeekCompleted)
				{
					if (this._WriteAResult.Result is Exception)
					{
						throw (Exception)this._WriteAResult.Result;
					}
					try
					{
						contextAwareResult.InvokeCallback(this._WriteAResult.Result);
						goto IL_013E;
					}
					catch (Exception ex)
					{
						this.Abort(ex, 1);
						throw;
					}
				}
				if (!this.RequestSubmitted && NclUtilities.IsThreadPoolLow())
				{
					Exception ex2 = new InvalidOperationException(SR.GetString("net_needmorethreads"));
					this.Abort(ex2, 1);
					throw ex2;
				}
				lock (this)
				{
					if (this._WriteAResult != null)
					{
						throw new InvalidOperationException(SR.GetString("net_repcall"));
					}
					if (this.SetRequestSubmitted())
					{
						throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
					}
					if (this._ReadAResult != null)
					{
						throw (Exception)this._ReadAResult.Result;
					}
					this._WriteAResult = contextAwareResult;
					this.Async = true;
				}
				this.CurrentMethod = this._OriginVerb;
				this.BeginSubmitRequest();
				IL_013E:
				contextAwareResult.FinishPostingAsyncOp();
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "BeginGetRequestStream", contextAwareResult);
			}
			return contextAwareResult;
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0007AFE4 File Offset: 0x00079FE4
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			TransportContext transportContext;
			return this.EndGetRequestStream(asyncResult, out transportContext);
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0007AFFC File Offset: 0x00079FFC
		public Stream EndGetRequestStream(IAsyncResult asyncResult, out TransportContext context)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "EndGetRequestStream", "");
			}
			context = null;
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndGetRequestStream" }));
			}
			ConnectStream connectStream = lazyAsyncResult.InternalWaitForCompletion() as ConnectStream;
			lazyAsyncResult.EndCalled = true;
			if (connectStream == null)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetRequestStream", lazyAsyncResult.Result as Exception);
				}
				throw (Exception)lazyAsyncResult.Result;
			}
			context = new ConnectStreamContext(connectStream);
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "EndGetRequestStream", connectStream);
			}
			return connectStream;
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x0007B0EC File Offset: 0x0007A0EC
		public override Stream GetRequestStream()
		{
			TransportContext transportContext;
			return this.GetRequestStream(out transportContext);
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0007B104 File Offset: 0x0007A104
		public Stream GetRequestStream(out TransportContext context)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "GetRequestStream", "");
			}
			context = null;
			this.CheckProtocol(true);
			if (this._WriteAResult == null || !this._WriteAResult.InternalPeekCompleted)
			{
				lock (this)
				{
					if (this._WriteAResult != null)
					{
						throw new InvalidOperationException(SR.GetString("net_repcall"));
					}
					if (this.SetRequestSubmitted())
					{
						throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
					}
					if (this._ReadAResult != null)
					{
						throw (Exception)this._ReadAResult.Result;
					}
					this._WriteAResult = new LazyAsyncResult(this, null, null);
					this.Async = false;
				}
				this.CurrentMethod = this._OriginVerb;
				while (this.m_Retry)
				{
					if (this._WriteAResult.InternalPeekCompleted)
					{
						break;
					}
					this._OldSubmitWriteStream = null;
					this._SubmitWriteStream = null;
					this.BeginSubmitRequest();
				}
				while (this.Aborted && !this._WriteAResult.InternalPeekCompleted)
				{
					if (!(this._CoreResponse is Exception))
					{
						Thread.SpinWait(1);
					}
					else
					{
						this.CheckWriteSideResponseProcessing();
					}
				}
			}
			ConnectStream connectStream = this._WriteAResult.InternalWaitForCompletion() as ConnectStream;
			this._WriteAResult.EndCalled = true;
			if (connectStream == null)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetRequestStream", this._WriteAResult.Result as Exception);
				}
				throw (Exception)this._WriteAResult.Result;
			}
			context = new ConnectStreamContext(connectStream);
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "GetRequestStream", connectStream);
			}
			return connectStream;
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x0007B2B0 File Offset: 0x0007A2B0
		private bool CanGetRequestStream
		{
			get
			{
				return !this.CurrentMethod.ContentBodyNotAllowed;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x0007B2D0 File Offset: 0x0007A2D0
		internal bool CanGetResponseStream
		{
			get
			{
				return !this.CurrentMethod.ExpectNoContentResponse;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06001FC7 RID: 8135 RVA: 0x0007B2F0 File Offset: 0x0007A2F0
		internal bool RequireBody
		{
			get
			{
				return this.CurrentMethod.RequireContentBody;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x0007B30A File Offset: 0x0007A30A
		private bool HasEntityBody
		{
			get
			{
				return this.HttpWriteMode == HttpWriteMode.Chunked || this.HttpWriteMode == HttpWriteMode.Buffer || (this.HttpWriteMode == HttpWriteMode.ContentLength && this.ContentLength > 0L);
			}
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x0007B338 File Offset: 0x0007A338
		internal void ErrorStatusCodeNotify(Connection connection, bool isKeepAlive, bool fatal)
		{
			ConnectStream submitWriteStream = this._SubmitWriteStream;
			if (submitWriteStream != null && submitWriteStream.Connection == connection)
			{
				if (!fatal)
				{
					submitWriteStream.ErrorResponseNotify(isKeepAlive);
					return;
				}
				if (!this.Aborted)
				{
					submitWriteStream.FatalResponseNotify();
				}
			}
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x0007B374 File Offset: 0x0007A374
		private HttpProcessingResult DoSubmitRequestProcessing(ref Exception exception)
		{
			HttpProcessingResult httpProcessingResult = HttpProcessingResult.Continue;
			this.m_Retry = false;
			try
			{
				if (this._HttpResponse != null)
				{
					if (this._CookieContainer != null)
					{
						CookieModule.OnReceivedHeaders(this);
					}
					this.ProxyAuthenticationState.Update(this);
					this.ServerAuthenticationState.Update(this);
				}
				bool flag = false;
				bool flag2 = true;
				if (this._HttpResponse == null)
				{
					flag = true;
				}
				else if (this.CheckResubmitForCache(ref exception) || this.CheckResubmit(ref exception))
				{
					flag = true;
					flag2 = false;
				}
				ServicePoint servicePoint = null;
				if (flag2)
				{
					WebException ex = exception as WebException;
					if (ex != null && ex.InternalStatus == WebExceptionInternalStatus.ServicePointFatal)
					{
						ProxyChain proxyChain = this._ProxyChain;
						if (proxyChain != null)
						{
							servicePoint = ServicePointManager.FindServicePoint(proxyChain);
						}
						flag = servicePoint != null;
					}
				}
				if (flag)
				{
					if (base.CacheProtocol != null && this._HttpResponse != null)
					{
						base.CacheProtocol.Reset();
					}
					this.ClearRequestForResubmit();
					WebException ex2 = exception as WebException;
					if (ex2 != null && (ex2.Status == WebExceptionStatus.PipelineFailure || ex2.Status == WebExceptionStatus.KeepAliveFailure))
					{
						this.m_Extra401Retry = true;
					}
					if (servicePoint == null)
					{
						servicePoint = this.FindServicePoint(true);
					}
					else
					{
						this._ServicePoint = servicePoint;
					}
					if (this.Async)
					{
						this.SubmitRequest(servicePoint);
					}
					else
					{
						this.m_Retry = true;
					}
					httpProcessingResult = HttpProcessingResult.WriteWait;
				}
			}
			finally
			{
				if (httpProcessingResult == HttpProcessingResult.Continue)
				{
					this.ClearAuthenticatedConnectionResources();
				}
			}
			return httpProcessingResult;
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x0007B4BC File Offset: 0x0007A4BC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "BeginGetResponse", "");
			}
			this.CheckProtocol(false);
			ConnectStream connectStream = ((this._OldSubmitWriteStream != null) ? this._OldSubmitWriteStream : this._SubmitWriteStream);
			if (connectStream != null && !connectStream.IsClosed && connectStream.BytesLeftToWrite == 0L)
			{
				connectStream.Close();
			}
			ContextAwareResult contextAwareResult = new ContextAwareResult(this.IdentityRequired, true, this, state, callback);
			if (!this.RequestSubmitted && NclUtilities.IsThreadPoolLow())
			{
				Exception ex = new InvalidOperationException(SR.GetString("net_needmorethreads"));
				this.Abort(ex, 1);
				throw ex;
			}
			lock (contextAwareResult.StartPostingAsyncOp())
			{
				bool flag = false;
				bool flag2;
				lock (this)
				{
					flag2 = this.SetRequestSubmitted();
					if (this.HaveResponse)
					{
						flag = true;
					}
					else
					{
						if (this._ReadAResult != null)
						{
							throw new InvalidOperationException(SR.GetString("net_repcall"));
						}
						this._ReadAResult = contextAwareResult;
						this.Async = true;
					}
				}
				this.CheckDeferredCallDone(connectStream);
				if (flag)
				{
					if (Logging.On)
					{
						Logging.Exit(Logging.Web, this, "BeginGetResponse", this._ReadAResult.Result);
					}
					Exception ex2 = this._ReadAResult.Result as Exception;
					if (ex2 != null)
					{
						throw ex2;
					}
					try
					{
						contextAwareResult.InvokeCallback(this._ReadAResult.Result);
						goto IL_017D;
					}
					catch (Exception ex3)
					{
						this.Abort(ex3, 1);
						throw;
					}
				}
				if (!flag2)
				{
					this.CurrentMethod = this._OriginVerb;
				}
				if (this._RerequestCount <= 0)
				{
					if (flag2)
					{
						goto IL_017D;
					}
				}
				while (this.m_Retry)
				{
					this.BeginSubmitRequest();
				}
				IL_017D:
				contextAwareResult.FinishPostingAsyncOp();
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "BeginGetResponse", contextAwareResult);
			}
			return contextAwareResult;
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x0007B698 File Offset: 0x0007A698
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "EndGetResponse", "");
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndGetResponse" }));
			}
			HttpWebResponse httpWebResponse = lazyAsyncResult.InternalWaitForCompletion() as HttpWebResponse;
			lazyAsyncResult.EndCalled = true;
			if (httpWebResponse == null)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetResponse", lazyAsyncResult.Result as Exception);
				}
				throw (Exception)lazyAsyncResult.Result;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "EndGetResponse", httpWebResponse);
			}
			return httpWebResponse;
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0007B77C File Offset: 0x0007A77C
		private void CheckDeferredCallDone(ConnectStream stream)
		{
			object obj = Interlocked.Exchange(ref this.m_PendingReturnResult, DBNull.Value);
			if (obj == NclConstants.Sentinel)
			{
				this.EndSubmitRequest();
				return;
			}
			if (obj != null && obj != DBNull.Value)
			{
				stream.ProcessWriteCallDone(obj as ConnectionReturnResult);
			}
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0007B7C0 File Offset: 0x0007A7C0
		public override WebResponse GetResponse()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "GetResponse", "");
			}
			this.CheckProtocol(false);
			ConnectStream connectStream = ((this._OldSubmitWriteStream != null) ? this._OldSubmitWriteStream : this._SubmitWriteStream);
			if (connectStream != null && !connectStream.IsClosed && connectStream.BytesLeftToWrite == 0L)
			{
				connectStream.Close();
			}
			bool flag = false;
			HttpWebResponse httpWebResponse = null;
			bool flag2;
			lock (this)
			{
				flag2 = this.SetRequestSubmitted();
				if (this.HaveResponse)
				{
					flag = true;
					httpWebResponse = this._ReadAResult.Result as HttpWebResponse;
				}
				else
				{
					if (this._ReadAResult != null)
					{
						throw new InvalidOperationException(SR.GetString("net_repcall"));
					}
					this.Async = false;
					if (this.Async)
					{
						ContextAwareResult contextAwareResult = new ContextAwareResult(this.IdentityRequired, true, this, null, null);
						contextAwareResult.StartPostingAsyncOp(false);
						contextAwareResult.FinishPostingAsyncOp();
						this._ReadAResult = contextAwareResult;
					}
					else
					{
						this._ReadAResult = new LazyAsyncResult(this, null, null);
					}
				}
			}
			this.CheckDeferredCallDone(connectStream);
			if (!flag)
			{
				if (this._Timer == null)
				{
					this._Timer = this.TimerQueue.CreateTimer(HttpWebRequest.s_TimeoutCallback, this);
				}
				if (!flag2)
				{
					this.CurrentMethod = this._OriginVerb;
				}
				while (this.m_Retry)
				{
					this.BeginSubmitRequest();
				}
				while (!this.Async && this.Aborted && !this._ReadAResult.InternalPeekCompleted)
				{
					if (!(this._CoreResponse is Exception))
					{
						Thread.SpinWait(1);
					}
					else
					{
						this.CheckWriteSideResponseProcessing();
					}
				}
				httpWebResponse = this._ReadAResult.InternalWaitForCompletion() as HttpWebResponse;
				this._ReadAResult.EndCalled = true;
			}
			if (httpWebResponse == null)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetResponse", this._ReadAResult.Result as Exception);
				}
				throw (Exception)this._ReadAResult.Result;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "GetResponse", httpWebResponse);
			}
			return httpWebResponse;
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0007B9C4 File Offset: 0x0007A9C4
		internal void WriteCallDone(ConnectStream stream, ConnectionReturnResult returnResult)
		{
			if (!object.ReferenceEquals(stream, (this._OldSubmitWriteStream != null) ? this._OldSubmitWriteStream : this._SubmitWriteStream))
			{
				stream.ProcessWriteCallDone(returnResult);
				return;
			}
			if (!this.UserRetrievedWriteStream)
			{
				stream.ProcessWriteCallDone(returnResult);
				return;
			}
			object obj = ((returnResult == null) ? Missing.Value : returnResult);
			object obj2 = Interlocked.CompareExchange(ref this.m_PendingReturnResult, obj, null);
			if (obj2 == DBNull.Value)
			{
				stream.ProcessWriteCallDone(returnResult);
			}
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0007BA30 File Offset: 0x0007AA30
		internal void NeedEndSubmitRequest()
		{
			object obj = Interlocked.CompareExchange(ref this.m_PendingReturnResult, NclConstants.Sentinel, null);
			if (obj == DBNull.Value)
			{
				this.EndSubmitRequest();
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001FD1 RID: 8145 RVA: 0x0007BA5D File Offset: 0x0007AA5D
		public Uri Address
		{
			get
			{
				return this._Uri;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x0007BA65 File Offset: 0x0007AA65
		// (set) Token: 0x06001FD3 RID: 8147 RVA: 0x0007BA6D File Offset: 0x0007AA6D
		public HttpContinueDelegate ContinueDelegate
		{
			get
			{
				return this._ContinueDelegate;
			}
			set
			{
				this._ContinueDelegate = value;
			}
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0007BA78 File Offset: 0x0007AA78
		internal void CallContinueDelegateCallback(object state)
		{
			CoreResponseData coreResponseData = (CoreResponseData)state;
			this.ContinueDelegate((int)coreResponseData.m_StatusCode, coreResponseData.m_ResponseHeaders);
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x0007BAA3 File Offset: 0x0007AAA3
		public ServicePoint ServicePoint
		{
			get
			{
				return this.FindServicePoint(false);
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x0007BAAC File Offset: 0x0007AAAC
		// (set) Token: 0x06001FD7 RID: 8151 RVA: 0x0007BAB4 File Offset: 0x0007AAB4
		public int MaximumAutomaticRedirections
		{
			get
			{
				return this._MaximumAllowedRedirections;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException(SR.GetString("net_toosmall"), "value");
				}
				this._MaximumAllowedRedirections = value;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x0007BAD6 File Offset: 0x0007AAD6
		// (set) Token: 0x06001FD9 RID: 8153 RVA: 0x0007BAE4 File Offset: 0x0007AAE4
		public override string Method
		{
			get
			{
				return this._OriginVerb.Name;
			}
			set
			{
				if (ValidationHelper.IsBlankString(value))
				{
					throw new ArgumentException(SR.GetString("net_badmethod"), "value");
				}
				if (ValidationHelper.IsInvalidHttpString(value))
				{
					throw new ArgumentException(SR.GetString("net_badmethod"), "value");
				}
				this._OriginVerb = KnownHttpVerb.Parse(value);
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x0007BB37 File Offset: 0x0007AB37
		// (set) Token: 0x06001FDB RID: 8155 RVA: 0x0007BB4E File Offset: 0x0007AB4E
		internal KnownHttpVerb CurrentMethod
		{
			get
			{
				if (this._Verb == null)
				{
					return this._OriginVerb;
				}
				return this._Verb;
			}
			set
			{
				this._Verb = value;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x0007BB57 File Offset: 0x0007AB57
		// (set) Token: 0x06001FDD RID: 8157 RVA: 0x0007BB5F File Offset: 0x0007AB5F
		public override ICredentials Credentials
		{
			get
			{
				return this._AuthInfo;
			}
			set
			{
				this._AuthInfo = value;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x0007BB68 File Offset: 0x0007AB68
		// (set) Token: 0x06001FDF RID: 8159 RVA: 0x0007BB7A File Offset: 0x0007AB7A
		public override bool UseDefaultCredentials
		{
			get
			{
				return this.Credentials is SystemNetworkCredential;
			}
			set
			{
				if (this.RequestSubmitted)
				{
					throw new InvalidOperationException(SR.GetString("net_writestarted"));
				}
				this._AuthInfo = (value ? CredentialCache.DefaultCredentials : null);
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x0007BBA5 File Offset: 0x0007ABA5
		// (set) Token: 0x06001FE1 RID: 8161 RVA: 0x0007BBB9 File Offset: 0x0007ABB9
		internal bool IsTunnelRequest
		{
			get
			{
				return (this._Booleans & HttpWebRequest.Booleans.IsTunnelRequest) != (HttpWebRequest.Booleans)0U;
			}
			set
			{
				if (value)
				{
					this._Booleans |= HttpWebRequest.Booleans.IsTunnelRequest;
					return;
				}
				this._Booleans &= ~HttpWebRequest.Booleans.IsTunnelRequest;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06001FE2 RID: 8162 RVA: 0x0007BBE3 File Offset: 0x0007ABE3
		// (set) Token: 0x06001FE3 RID: 8163 RVA: 0x0007BBEB File Offset: 0x0007ABEB
		public override string ConnectionGroupName
		{
			get
			{
				return this._ConnectionGroupName;
			}
			set
			{
				this._ConnectionGroupName = value;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (set) Token: 0x06001FE4 RID: 8164 RVA: 0x0007BBF4 File Offset: 0x0007ABF4
		internal bool InternalConnectionGroup
		{
			set
			{
				this.m_InternalConnectionGroup = value;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x0007BBFD File Offset: 0x0007ABFD
		// (set) Token: 0x06001FE6 RID: 8166 RVA: 0x0007BC08 File Offset: 0x0007AC08
		public override WebHeaderCollection Headers
		{
			get
			{
				return this._HttpRequestHeaders;
			}
			set
			{
				if (this.RequestSubmitted)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				WebHeaderCollection webHeaderCollection = new WebHeaderCollection(WebHeaderCollectionType.HttpWebRequest);
				foreach (string text in value.AllKeys)
				{
					webHeaderCollection.Add(text, value[text]);
				}
				this._HttpRequestHeaders = webHeaderCollection;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06001FE7 RID: 8167 RVA: 0x0007BC69 File Offset: 0x0007AC69
		// (set) Token: 0x06001FE8 RID: 8168 RVA: 0x0007BC7B File Offset: 0x0007AC7B
		public override IWebProxy Proxy
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this._Proxy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (this.RequestSubmitted)
				{
					throw new InvalidOperationException(SR.GetString("net_reqsubmitted"));
				}
				this.InternalProxy = value;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001FE9 RID: 8169 RVA: 0x0007BCA6 File Offset: 0x0007ACA6
		// (set) Token: 0x06001FEA RID: 8170 RVA: 0x0007BCAE File Offset: 0x0007ACAE
		internal IWebProxy InternalProxy
		{
			get
			{
				return this._Proxy;
			}
			set
			{
				this.ProxySet = true;
				this._Proxy = value;
				if (this._ProxyChain != null)
				{
					this._ProxyChain.Dispose();
				}
				this._ProxyChain = null;
				this.FindServicePoint(true);
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06001FEB RID: 8171 RVA: 0x0007BCE0 File Offset: 0x0007ACE0
		// (set) Token: 0x06001FEC RID: 8172 RVA: 0x0007BCF5 File Offset: 0x0007ACF5
		public Version ProtocolVersion
		{
			get
			{
				if (!this.IsVersionHttp10)
				{
					return HttpVersion.Version11;
				}
				return HttpVersion.Version10;
			}
			set
			{
				if (value.Equals(HttpVersion.Version11))
				{
					this.IsVersionHttp10 = false;
					return;
				}
				if (value.Equals(HttpVersion.Version10))
				{
					this.IsVersionHttp10 = true;
					return;
				}
				throw new ArgumentException(SR.GetString("net_wrongversion"), "value");
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06001FED RID: 8173 RVA: 0x0007BD35 File Offset: 0x0007AD35
		// (set) Token: 0x06001FEE RID: 8174 RVA: 0x0007BD47 File Offset: 0x0007AD47
		public override string ContentType
		{
			get
			{
				return this._HttpRequestHeaders["Content-Type"];
			}
			set
			{
				this.SetSpecialHeaders("Content-Type", value);
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x0007BD55 File Offset: 0x0007AD55
		// (set) Token: 0x06001FF0 RID: 8176 RVA: 0x0007BD5D File Offset: 0x0007AD5D
		public string MediaType
		{
			get
			{
				return this._MediaType;
			}
			set
			{
				this._MediaType = value;
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x0007BD66 File Offset: 0x0007AD66
		// (set) Token: 0x06001FF2 RID: 8178 RVA: 0x0007BD78 File Offset: 0x0007AD78
		public string TransferEncoding
		{
			get
			{
				return this._HttpRequestHeaders["Transfer-Encoding"];
			}
			set
			{
				if (ValidationHelper.IsBlankString(value))
				{
					this._HttpRequestHeaders.RemoveInternal("Transfer-Encoding");
					return;
				}
				string text = value.ToLower(CultureInfo.InvariantCulture);
				bool flag = text.IndexOf("chunked") != -1;
				if (flag)
				{
					throw new ArgumentException(SR.GetString("net_nochunked"), "value");
				}
				if (!this.SendChunked)
				{
					throw new InvalidOperationException(SR.GetString("net_needchunked"));
				}
				this._HttpRequestHeaders.CheckUpdate("Transfer-Encoding", value);
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x0007BDFD File Offset: 0x0007ADFD
		// (set) Token: 0x06001FF4 RID: 8180 RVA: 0x0007BE10 File Offset: 0x0007AE10
		public string Connection
		{
			get
			{
				return this._HttpRequestHeaders["Connection"];
			}
			set
			{
				if (ValidationHelper.IsBlankString(value))
				{
					this._HttpRequestHeaders.Remove("Connection");
					return;
				}
				string text = value.ToLower(CultureInfo.InvariantCulture);
				bool flag = text.IndexOf("keep-alive") != -1;
				bool flag2 = text.IndexOf("close") != -1;
				if (flag || flag2)
				{
					throw new ArgumentException(SR.GetString("net_connarg"), "value");
				}
				this._HttpRequestHeaders.CheckUpdate("Connection", value);
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x0007BE92 File Offset: 0x0007AE92
		// (set) Token: 0x06001FF6 RID: 8182 RVA: 0x0007BEA4 File Offset: 0x0007AEA4
		public string Accept
		{
			get
			{
				return this._HttpRequestHeaders["Accept"];
			}
			set
			{
				this.SetSpecialHeaders("Accept", value);
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x0007BEB2 File Offset: 0x0007AEB2
		// (set) Token: 0x06001FF8 RID: 8184 RVA: 0x0007BEC4 File Offset: 0x0007AEC4
		public string Referer
		{
			get
			{
				return this._HttpRequestHeaders["Referer"];
			}
			set
			{
				this.SetSpecialHeaders("Referer", value);
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x0007BED2 File Offset: 0x0007AED2
		// (set) Token: 0x06001FFA RID: 8186 RVA: 0x0007BEE4 File Offset: 0x0007AEE4
		public string UserAgent
		{
			get
			{
				return this._HttpRequestHeaders["User-Agent"];
			}
			set
			{
				this.SetSpecialHeaders("User-Agent", value);
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x0007BEF2 File Offset: 0x0007AEF2
		// (set) Token: 0x06001FFC RID: 8188 RVA: 0x0007BF04 File Offset: 0x0007AF04
		public string Expect
		{
			get
			{
				return this._HttpRequestHeaders["Expect"];
			}
			set
			{
				if (ValidationHelper.IsBlankString(value))
				{
					this._HttpRequestHeaders.RemoveInternal("Expect");
					return;
				}
				string text = value.ToLower(CultureInfo.InvariantCulture);
				bool flag = text.IndexOf("100-continue") != -1;
				if (flag)
				{
					throw new ArgumentException(SR.GetString("net_no100"), "value");
				}
				this._HttpRequestHeaders.CheckUpdate("Expect", value);
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001FFD RID: 8189 RVA: 0x0007BF74 File Offset: 0x0007AF74
		// (set) Token: 0x06001FFE RID: 8190 RVA: 0x0007BFBA File Offset: 0x0007AFBA
		public DateTime IfModifiedSince
		{
			get
			{
				string text = this._HttpRequestHeaders["If-Modified-Since"];
				if (text == null)
				{
					return DateTime.Now;
				}
				if (this._CachedIfModifedSince != DateTime.MinValue)
				{
					return this._CachedIfModifedSince;
				}
				return HttpProtocolUtils.string2date(text);
			}
			set
			{
				this.SetSpecialHeaders("If-Modified-Since", HttpProtocolUtils.date2string(value));
				this._CachedIfModifedSince = value;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x0007BFD4 File Offset: 0x0007AFD4
		internal byte[] WriteBuffer
		{
			get
			{
				return this._WriteBuffer;
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x0007BFDC File Offset: 0x0007AFDC
		private void SetSpecialHeaders(string HeaderName, string value)
		{
			value = WebHeaderCollection.CheckBadChars(value, true);
			this._HttpRequestHeaders.RemoveInternal(HeaderName);
			if (value.Length != 0)
			{
				this._HttpRequestHeaders.AddInternal(HeaderName, value);
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x0007C008 File Offset: 0x0007B008
		public override void Abort()
		{
			this.Abort(null, 1);
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x0007C014 File Offset: 0x0007B014
		private void Abort(Exception exception, int abortState)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Abort", (exception == null) ? "" : exception.Message);
			}
			if (Interlocked.CompareExchange(ref this.m_Aborted, abortState, 0) == 0)
			{
				this.m_OnceFailed = true;
				this.CancelTimer();
				WebException ex = exception as WebException;
				if (exception == null)
				{
					ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
				}
				else if (ex == null)
				{
					ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), exception, WebExceptionStatus.RequestCanceled, this._HttpResponse);
				}
				try
				{
					Thread.MemoryBarrier();
					HttpAbortDelegate abortDelegate = this._AbortDelegate;
					if (abortDelegate == null || abortDelegate(this, ex))
					{
						LazyAsyncResult lazyAsyncResult = (this.Async ? null : this.ConnectionAsyncResult);
						LazyAsyncResult lazyAsyncResult2 = (this.Async ? null : this.ConnectionReaderAsyncResult);
						this.SetResponse(ex);
						if (lazyAsyncResult != null)
						{
							lazyAsyncResult.InvokeCallback(ex);
						}
						if (lazyAsyncResult2 != null)
						{
							lazyAsyncResult2.InvokeCallback(ex);
						}
					}
					this.ClearAuthenticatedConnectionResources();
				}
				catch (Exception ex2)
				{
					if (NclUtilities.IsFatal(ex2))
					{
						throw;
					}
				}
				catch
				{
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "Abort", "");
			}
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x0007C14C File Offset: 0x0007B14C
		private void CancelTimer()
		{
			TimerThread.Timer timer = this._Timer;
			if (timer != null)
			{
				timer.Cancel();
			}
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x0007C16A File Offset: 0x0007B16A
		private static void TimeoutCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			ThreadPool.UnsafeQueueUserWorkItem(HttpWebRequest.s_AbortWrapper, context);
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x0007C178 File Offset: 0x0007B178
		private static void AbortWrapper(object context)
		{
			((HttpWebRequest)context).Abort(new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout), 1);
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x0007C194 File Offset: 0x0007B194
		private ServicePoint FindServicePoint(bool forceFind)
		{
			ServicePoint servicePoint = this._ServicePoint;
			if (servicePoint == null || forceFind)
			{
				lock (this)
				{
					if (this._ServicePoint == null || forceFind)
					{
						if (!this.ProxySet)
						{
							this._Proxy = WebRequest.InternalDefaultWebProxy;
						}
						if (this._ProxyChain != null)
						{
							this._ProxyChain.Dispose();
						}
						this._ServicePoint = ServicePointManager.FindServicePoint(this._Uri, this._Proxy, out this._ProxyChain, ref this._AbortDelegate, ref this.m_Aborted);
						if (Logging.On)
						{
							Logging.Associate(Logging.Web, this, this._ServicePoint);
						}
					}
				}
				servicePoint = this._ServicePoint;
			}
			return servicePoint;
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x0007C24C File Offset: 0x0007B24C
		private void InvokeGetRequestStreamCallback()
		{
			LazyAsyncResult writeAResult = this._WriteAResult;
			if (writeAResult != null)
			{
				try
				{
					writeAResult.InvokeCallback(this._SubmitWriteStream);
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					this.Abort(ex, 1);
					throw;
				}
			}
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x0007C298 File Offset: 0x0007B298
		internal void SetRequestSubmitDone(ConnectStream submitStream)
		{
			if (!this.Async)
			{
				this.ConnectionAsyncResult.InvokeCallback();
			}
			if (this.AllowWriteStreamBuffering)
			{
				submitStream.EnableWriteBuffering();
			}
			if (submitStream.CanTimeout)
			{
				submitStream.ReadTimeout = this.ReadWriteTimeout;
				submitStream.WriteTimeout = this.ReadWriteTimeout;
			}
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, submitStream);
			}
			TransportContext transportContext = new ConnectStreamContext(submitStream);
			this.ServerAuthenticationState.TransportContext = transportContext;
			this.ProxyAuthenticationState.TransportContext = transportContext;
			this._SubmitWriteStream = submitStream;
			if (this.Async && this._CoreResponse != null && this._CoreResponse != DBNull.Value)
			{
				submitStream.CallDone();
				return;
			}
			this.EndSubmitRequest();
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x0007C34C File Offset: 0x0007B34C
		internal void WriteHeadersCallback(WebExceptionStatus errorStatus, ConnectStream stream, bool async)
		{
			if (errorStatus == WebExceptionStatus.Success)
			{
				if (!this.EndWriteHeaders(async))
				{
					errorStatus = WebExceptionStatus.Pending;
					return;
				}
				if (stream.BytesLeftToWrite == 0L)
				{
					stream.CallDone();
				}
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x0007C37C File Offset: 0x0007B37C
		internal void SetRequestContinue()
		{
			this.SetRequestContinue(null);
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x0007C388 File Offset: 0x0007B388
		internal void SetRequestContinue(CoreResponseData continueResponse)
		{
			this._RequestContinueCount++;
			if (this.HttpWriteMode == HttpWriteMode.None)
			{
				return;
			}
			if (this.m_ContinueGate.Complete())
			{
				TimerThread.Timer continueTimer = this.m_ContinueTimer;
				this.m_ContinueTimer = null;
				if (continueTimer == null || continueTimer.Cancel())
				{
					if (continueResponse != null && this.ContinueDelegate != null)
					{
						ExecutionContext executionContext = (this.Async ? this.GetWritingContext().ContextCopy : null);
						if (executionContext == null)
						{
							this.ContinueDelegate((int)continueResponse.m_StatusCode, continueResponse.m_ResponseHeaders);
						}
						else
						{
							ExecutionContext.Run(executionContext, new ContextCallback(this.CallContinueDelegateCallback), continueResponse);
						}
					}
					this.EndWriteHeaders_Part2();
				}
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x0600200C RID: 8204 RVA: 0x0007C42A File Offset: 0x0007B42A
		internal int RequestContinueCount
		{
			get
			{
				return this._RequestContinueCount;
			}
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x0007C432 File Offset: 0x0007B432
		internal void OpenWriteSideResponseWindow()
		{
			this._CoreResponse = DBNull.Value;
			this._NestedWriteSideCheck = 0;
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x0007C448 File Offset: 0x0007B448
		internal void CheckWriteSideResponseProcessing()
		{
			object obj = (this.Async ? Interlocked.CompareExchange(ref this._CoreResponse, null, DBNull.Value) : this._CoreResponse);
			if (obj == DBNull.Value)
			{
				return;
			}
			if (obj == null)
			{
				throw new InternalException();
			}
			if (!this.Async && ++this._NestedWriteSideCheck != 1)
			{
				return;
			}
			Exception ex = obj as Exception;
			if (ex != null)
			{
				this.SetResponse(ex);
				return;
			}
			this.SetResponse(obj as CoreResponseData);
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x0007C4C4 File Offset: 0x0007B4C4
		internal void SetAndOrProcessResponse(object responseOrException)
		{
			if (responseOrException == null)
			{
				throw new InternalException();
			}
			CoreResponseData coreResponseData = responseOrException as CoreResponseData;
			WebException ex = responseOrException as WebException;
			object obj = Interlocked.CompareExchange(ref this._CoreResponse, responseOrException, DBNull.Value);
			if (obj != null)
			{
				if (obj.GetType() == typeof(CoreResponseData))
				{
					if (coreResponseData != null)
					{
						throw new InternalException();
					}
					if (ex != null && ex.InternalStatus != WebExceptionInternalStatus.ServicePointFatal && ex.InternalStatus != WebExceptionInternalStatus.RequestFatal)
					{
						return;
					}
				}
				else if (obj.GetType() != typeof(DBNull))
				{
					if (coreResponseData == null)
					{
						throw new InternalException();
					}
					ICloseEx closeEx = coreResponseData.m_ConnectStream as ICloseEx;
					if (closeEx != null)
					{
						closeEx.CloseEx(CloseExState.Silent);
						return;
					}
					coreResponseData.m_ConnectStream.Close();
					return;
				}
			}
			if (obj == DBNull.Value)
			{
				if (!this.Async)
				{
					LazyAsyncResult connectionAsyncResult = this.ConnectionAsyncResult;
					LazyAsyncResult connectionReaderAsyncResult = this.ConnectionReaderAsyncResult;
					connectionAsyncResult.InvokeCallback(responseOrException);
					connectionReaderAsyncResult.InvokeCallback(responseOrException);
				}
				return;
			}
			if (obj != null)
			{
				Exception ex2 = responseOrException as Exception;
				if (ex2 != null)
				{
					this.SetResponse(ex2);
					return;
				}
				throw new InternalException();
			}
			else
			{
				obj = Interlocked.CompareExchange(ref this._CoreResponse, responseOrException, null);
				if (obj != null && coreResponseData != null)
				{
					ICloseEx closeEx2 = coreResponseData.m_ConnectStream as ICloseEx;
					if (closeEx2 != null)
					{
						closeEx2.CloseEx(CloseExState.Silent);
						return;
					}
					coreResponseData.m_ConnectStream.Close();
					return;
				}
				else
				{
					if (!this.Async)
					{
						throw new InternalException();
					}
					if (coreResponseData != null)
					{
						this.SetResponse(coreResponseData);
						return;
					}
					this.SetResponse(responseOrException as Exception);
					return;
				}
			}
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x0007C61C File Offset: 0x0007B61C
		private void SetResponse(CoreResponseData coreResponseData)
		{
			try
			{
				if (!this.Async)
				{
					LazyAsyncResult connectionAsyncResult = this.ConnectionAsyncResult;
					LazyAsyncResult connectionReaderAsyncResult = this.ConnectionReaderAsyncResult;
					connectionAsyncResult.InvokeCallback(coreResponseData);
					connectionReaderAsyncResult.InvokeCallback(coreResponseData);
				}
				if (coreResponseData != null)
				{
					if (coreResponseData.m_ConnectStream.CanTimeout)
					{
						coreResponseData.m_ConnectStream.WriteTimeout = this.ReadWriteTimeout;
						coreResponseData.m_ConnectStream.ReadTimeout = this.ReadWriteTimeout;
					}
					this._HttpResponse = new HttpWebResponse(this._Uri, this.CurrentMethod, coreResponseData, this._MediaType, this.UsesProxySemantics, this.AutomaticDecompression);
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, coreResponseData.m_ConnectStream);
					}
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, this._HttpResponse);
					}
					this.ProcessResponse();
				}
				else
				{
					this.Abort(null, 1);
				}
			}
			catch (Exception ex)
			{
				this.Abort(ex, 2);
			}
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x0007C70C File Offset: 0x0007B70C
		private void ProcessResponse()
		{
			Exception ex = null;
			if (this.DoSubmitRequestProcessing(ref ex) == HttpProcessingResult.Continue)
			{
				this.CancelTimer();
				object obj = ((ex != null) ? ex : this._HttpResponse);
				if (this._ReadAResult == null)
				{
					lock (this)
					{
						if (this._ReadAResult == null)
						{
							this._ReadAResult = new LazyAsyncResult(null, null, null, obj);
						}
					}
				}
				try
				{
					this.FinishRequest(this._HttpResponse, ex);
					this._ReadAResult.InvokeCallback(obj);
					try
					{
						this.SetRequestContinue();
					}
					catch
					{
					}
				}
				catch (Exception ex2)
				{
					this.Abort(ex2, 1);
					throw;
				}
				finally
				{
					if (ex == null && this._ReadAResult.Result != this._HttpResponse)
					{
						WebException ex3 = this._ReadAResult.Result as WebException;
						if (ex3 != null && ex3.Response != null)
						{
							this._HttpResponse.Abort();
						}
					}
				}
			}
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x0007C81C File Offset: 0x0007B81C
		private void SetResponse(Exception E)
		{
			HttpProcessingResult httpProcessingResult = HttpProcessingResult.Continue;
			WebException ex = (this.HaveResponse ? (this._ReadAResult.Result as WebException) : null);
			WebException ex2 = E as WebException;
			if (ex != null && (ex.InternalStatus == WebExceptionInternalStatus.RequestFatal || ex.InternalStatus == WebExceptionInternalStatus.ServicePointFatal) && (ex2 == null || ex2.InternalStatus != WebExceptionInternalStatus.RequestFatal))
			{
				E = ex;
			}
			else
			{
				ex = ex2;
			}
			if (E != null && Logging.On)
			{
				Logging.Exception(Logging.Web, this, "", ex);
			}
			try
			{
				if (ex != null && (ex.InternalStatus == WebExceptionInternalStatus.Isolated || ex.InternalStatus == WebExceptionInternalStatus.ServicePointFatal || (ex.InternalStatus == WebExceptionInternalStatus.Recoverable && !this.m_OnceFailed)))
				{
					if (ex.InternalStatus == WebExceptionInternalStatus.Recoverable)
					{
						this.m_OnceFailed = true;
					}
					this.Pipelined = false;
					if (this._SubmitWriteStream != null && this._OldSubmitWriteStream == null && this._SubmitWriteStream.BufferOnly)
					{
						this._OldSubmitWriteStream = this._SubmitWriteStream;
					}
					httpProcessingResult = this.DoSubmitRequestProcessing(ref E);
				}
			}
			catch (Exception ex3)
			{
				if (NclUtilities.IsFatal(ex3))
				{
					throw;
				}
				httpProcessingResult = HttpProcessingResult.Continue;
				E = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), ex3, WebExceptionStatus.RequestCanceled, this._HttpResponse);
			}
			finally
			{
				if (httpProcessingResult == HttpProcessingResult.Continue)
				{
					this.CancelTimer();
					if (!(E is WebException) && !(E is SecurityException))
					{
						if (this._HttpResponse == null)
						{
							E = new WebException(E.Message, E);
						}
						else
						{
							E = new WebException(SR.GetString("net_servererror", new object[] { NetRes.GetWebStatusCodeString(this.ResponseStatusCode, this._HttpResponse.StatusDescription) }), E, WebExceptionStatus.ProtocolError, this._HttpResponse);
						}
					}
					LazyAsyncResult lazyAsyncResult = null;
					HttpWebResponse httpWebResponse = this._HttpResponse;
					LazyAsyncResult writeAResult;
					lock (this)
					{
						writeAResult = this._WriteAResult;
						if (this._ReadAResult == null)
						{
							this._ReadAResult = new LazyAsyncResult(null, null, null, E);
						}
						else
						{
							lazyAsyncResult = this._ReadAResult;
						}
					}
					try
					{
						this.FinishRequest(httpWebResponse, E);
						try
						{
							if (writeAResult != null)
							{
								writeAResult.InvokeCallback(E);
							}
						}
						finally
						{
							if (lazyAsyncResult != null)
							{
								lazyAsyncResult.InvokeCallback(E);
							}
						}
					}
					finally
					{
						httpWebResponse = this._ReadAResult.Result as HttpWebResponse;
						if (httpWebResponse != null)
						{
							httpWebResponse.Abort();
						}
						if (base.CacheProtocol != null)
						{
							base.CacheProtocol.Abort();
						}
					}
				}
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x0007CABC File Offset: 0x0007BABC
		private bool IdentityRequired
		{
			get
			{
				CredentialCache credentialCache;
				return this.Credentials != null && ComNetOS.IsWinNt && (this.Credentials is SystemNetworkCredential || (!(this.Credentials is NetworkCredential) && ((credentialCache = this.Credentials as CredentialCache) == null || credentialCache.IsDefaultInCache)));
			}
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x0007CB10 File Offset: 0x0007BB10
		internal override ContextAwareResult GetConnectingContext()
		{
			if (!this.Async)
			{
				return null;
			}
			ContextAwareResult contextAwareResult = ((this.HttpWriteMode == HttpWriteMode.None || this._OldSubmitWriteStream != null || this._WriteAResult == null) ? this._ReadAResult : this._WriteAResult) as ContextAwareResult;
			if (contextAwareResult == null)
			{
				throw new InternalException();
			}
			return contextAwareResult;
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0007CB60 File Offset: 0x0007BB60
		internal override ContextAwareResult GetWritingContext()
		{
			if (!this.Async)
			{
				return null;
			}
			ContextAwareResult contextAwareResult = ((this.HttpWriteMode == HttpWriteMode.None || this.HttpWriteMode == HttpWriteMode.Buffer || this.m_PendingReturnResult == DBNull.Value || this.m_OriginallyBuffered || this._WriteAResult == null) ? this._ReadAResult : this._WriteAResult) as ContextAwareResult;
			if (contextAwareResult == null)
			{
				throw new InternalException();
			}
			return contextAwareResult;
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x0007CBC4 File Offset: 0x0007BBC4
		internal override ContextAwareResult GetReadingContext()
		{
			if (!this.Async)
			{
				return null;
			}
			ContextAwareResult contextAwareResult = this._ReadAResult as ContextAwareResult;
			if (contextAwareResult == null)
			{
				throw new InternalException();
			}
			return contextAwareResult;
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x0007CBF1 File Offset: 0x0007BBF1
		private void BeginSubmitRequest()
		{
			this.SubmitRequest(this.FindServicePoint(false));
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x0007CC00 File Offset: 0x0007BC00
		private void SubmitRequest(ServicePoint servicePoint)
		{
			if (!this.Async)
			{
				this._ConnectionAResult = new LazyAsyncResult(this, null, null);
				this._ConnectionReaderAResult = new LazyAsyncResult(this, null, null);
				this.OpenWriteSideResponseWindow();
			}
			if (this._Timer == null && !this.Async)
			{
				this._Timer = this.TimerQueue.CreateTimer(HttpWebRequest.s_TimeoutCallback, this);
			}
			try
			{
				if (this._SubmitWriteStream != null && this._SubmitWriteStream.IsPostStream)
				{
					if (this._OldSubmitWriteStream == null && !this._SubmitWriteStream.ErrorInStream)
					{
						this._OldSubmitWriteStream = this._SubmitWriteStream;
					}
					this._WriteBuffer = null;
				}
				this.m_Retry = false;
				if (this.PreAuthenticate)
				{
					if (this.UsesProxySemantics && this._Proxy != null && this._Proxy.Credentials != null)
					{
						this.ProxyAuthenticationState.PreAuthIfNeeded(this, this._Proxy.Credentials);
					}
					if (this.Credentials != null)
					{
						this.ServerAuthenticationState.PreAuthIfNeeded(this, this.Credentials);
					}
				}
				if (this.WriteBuffer == null)
				{
					this.UpdateHeaders();
				}
				if (!this.CheckCacheRetrieveBeforeSubmit())
				{
					servicePoint.SubmitRequest(this, this.GetConnectionGroupLine());
				}
			}
			finally
			{
				if (!this.Async)
				{
					this.CheckWriteSideResponseProcessing();
				}
			}
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x0007CD40 File Offset: 0x0007BD40
		private bool CheckCacheRetrieveBeforeSubmit()
		{
			if (base.CacheProtocol == null)
			{
				return false;
			}
			bool flag;
			try
			{
				Uri uri = this._Uri;
				if (uri.Fragment.Length != 0)
				{
					uri = new Uri(uri.GetParts(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query, UriFormat.SafeUnescaped));
				}
				base.CacheProtocol.GetRetrieveStatus(uri, this);
				if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Fail)
				{
					throw base.CacheProtocol.ProtocolException;
				}
				if (base.CacheProtocol.ProtocolStatus != CacheValidationStatus.ReturnCachedResponse)
				{
					flag = false;
				}
				else
				{
					if (this.HttpWriteMode != HttpWriteMode.None)
					{
						throw new NotSupportedException(SR.GetString("net_cache_not_supported_body"));
					}
					HttpRequestCacheValidator httpRequestCacheValidator = (HttpRequestCacheValidator)base.CacheProtocol.Validator;
					CoreResponseData coreResponseData = new CoreResponseData();
					coreResponseData.m_IsVersionHttp11 = httpRequestCacheValidator.CacheHttpVersion.Equals(HttpVersion.Version11);
					coreResponseData.m_StatusCode = httpRequestCacheValidator.CacheStatusCode;
					coreResponseData.m_StatusDescription = httpRequestCacheValidator.CacheStatusDescription;
					coreResponseData.m_ResponseHeaders = httpRequestCacheValidator.CacheHeaders;
					coreResponseData.m_ContentLength = base.CacheProtocol.ResponseStreamLength;
					coreResponseData.m_ConnectStream = base.CacheProtocol.ResponseStream;
					this._HttpResponse = new HttpWebResponse(this._Uri, this.CurrentMethod, coreResponseData, this._MediaType, this.UsesProxySemantics, this.AutomaticDecompression);
					this._HttpResponse.InternalSetFromCache = true;
					this._HttpResponse.InternalSetIsCacheFresh = httpRequestCacheValidator.CacheFreshnessStatus != CacheFreshnessStatus.Stale;
					this.ProcessResponse();
					flag = true;
				}
			}
			catch (Exception ex)
			{
				this.Abort(ex, 1);
				throw;
			}
			return flag;
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0007CEC8 File Offset: 0x0007BEC8
		private bool CheckCacheRetrieveOnResponse()
		{
			if (base.CacheProtocol == null)
			{
				return true;
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Fail)
			{
				throw base.CacheProtocol.ProtocolException;
			}
			Stream responseStream = this._HttpResponse.ResponseStream;
			base.CacheProtocol.GetRevalidateStatus(this._HttpResponse, this._HttpResponse.ResponseStream);
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.RetryResponseFromServer)
			{
				return false;
			}
			if (base.CacheProtocol.ProtocolStatus != CacheValidationStatus.ReturnCachedResponse && base.CacheProtocol.ProtocolStatus != CacheValidationStatus.CombineCachedAndServerResponse)
			{
				return true;
			}
			if (this.HttpWriteMode != HttpWriteMode.None)
			{
				throw new NotSupportedException(SR.GetString("net_cache_not_supported_body"));
			}
			CoreResponseData coreResponseData = new CoreResponseData();
			HttpRequestCacheValidator httpRequestCacheValidator = (HttpRequestCacheValidator)base.CacheProtocol.Validator;
			coreResponseData.m_IsVersionHttp11 = httpRequestCacheValidator.CacheHttpVersion.Equals(HttpVersion.Version11);
			coreResponseData.m_StatusCode = httpRequestCacheValidator.CacheStatusCode;
			coreResponseData.m_StatusDescription = httpRequestCacheValidator.CacheStatusDescription;
			coreResponseData.m_ResponseHeaders = ((base.CacheProtocol.ProtocolStatus == CacheValidationStatus.CombineCachedAndServerResponse) ? new WebHeaderCollection(httpRequestCacheValidator.CacheHeaders) : httpRequestCacheValidator.CacheHeaders);
			coreResponseData.m_ContentLength = base.CacheProtocol.ResponseStreamLength;
			coreResponseData.m_ConnectStream = base.CacheProtocol.ResponseStream;
			this._HttpResponse = new HttpWebResponse(this._Uri, this.CurrentMethod, coreResponseData, this._MediaType, this.UsesProxySemantics, this.AutomaticDecompression);
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				this._HttpResponse.InternalSetFromCache = true;
				this._HttpResponse.InternalSetIsCacheFresh = base.CacheProtocol.IsCacheFresh;
				if (responseStream != null)
				{
					try
					{
						responseStream.Close();
					}
					catch
					{
					}
				}
			}
			return true;
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0007D070 File Offset: 0x0007C070
		private void CheckCacheUpdateOnResponse()
		{
			if (base.CacheProtocol == null)
			{
				return;
			}
			if (base.CacheProtocol.GetUpdateStatus(this._HttpResponse, this._HttpResponse.ResponseStream) == CacheValidationStatus.UpdateResponseInformation)
			{
				this._HttpResponse.ResponseStream = base.CacheProtocol.ResponseStream;
				return;
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Fail)
			{
				throw base.CacheProtocol.ProtocolException;
			}
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x0007D0D8 File Offset: 0x0007C0D8
		private void EndSubmitRequest()
		{
			try
			{
				if (this.HttpWriteMode == HttpWriteMode.Buffer)
				{
					this.InvokeGetRequestStreamCallback();
				}
				else
				{
					if (this.WriteBuffer == null)
					{
						long num = this.SwitchToContentLength();
						this.SerializeHeaders();
						this.PostSwitchToContentLength(num);
					}
					this._SubmitWriteStream.WriteHeaders(this.Async);
				}
			}
			catch
			{
				ConnectStream submitWriteStream = this._SubmitWriteStream;
				if (submitWriteStream != null)
				{
					submitWriteStream.CallDone();
				}
				throw;
			}
			finally
			{
				if (!this.Async)
				{
					this.CheckWriteSideResponseProcessing();
				}
			}
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x0007D168 File Offset: 0x0007C168
		internal bool EndWriteHeaders(bool async)
		{
			try
			{
				if ((this.ContentLength > 0L || this.HttpWriteMode == HttpWriteMode.Chunked) && this.ExpectContinue && this._ServicePoint.Understands100Continue && (async ? this.m_ContinueGate.StartTrigger(true) : this.m_ContinueGate.Trigger(true)))
				{
					if (async)
					{
						try
						{
							this.m_ContinueTimer = HttpWebRequest.s_ContinueTimerQueue.CreateTimer(HttpWebRequest.s_ContinueTimeoutCallback, this);
						}
						finally
						{
							this.m_ContinueGate.FinishTrigger();
						}
						return false;
					}
					this._SubmitWriteStream.PollAndRead(this.UserRetrievedWriteStream);
					return true;
				}
				else
				{
					this.EndWriteHeaders_Part2();
				}
			}
			catch
			{
				ConnectStream submitWriteStream = this._SubmitWriteStream;
				if (submitWriteStream != null)
				{
					submitWriteStream.CallDone();
				}
				throw;
			}
			return true;
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x0007D238 File Offset: 0x0007C238
		private static void ContinueTimeoutCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)context;
			if (httpWebRequest.HttpWriteMode == HttpWriteMode.None)
			{
				return;
			}
			if (httpWebRequest.CompleteContinueGate())
			{
				httpWebRequest.m_ContinueTimer = null;
			}
			ThreadPool.UnsafeQueueUserWorkItem(HttpWebRequest.s_EndWriteHeaders_Part2Callback, httpWebRequest);
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x0007D271 File Offset: 0x0007C271
		private bool CompleteContinueGate()
		{
			return this.m_ContinueGate.Complete();
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0007D27E File Offset: 0x0007C27E
		private static void EndWriteHeaders_Part2Wrapper(object state)
		{
			((HttpWebRequest)state).EndWriteHeaders_Part2();
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x0007D28C File Offset: 0x0007C28C
		internal void EndWriteHeaders_Part2()
		{
			try
			{
				ConnectStream submitWriteStream = this._SubmitWriteStream;
				if (this.HttpWriteMode != HttpWriteMode.None)
				{
					this.m_BodyStarted = true;
					if (this.AllowWriteStreamBuffering)
					{
						if (submitWriteStream.BufferOnly)
						{
							this._OldSubmitWriteStream = submitWriteStream;
						}
						if (this._OldSubmitWriteStream != null)
						{
							submitWriteStream.ResubmitWrite(this._OldSubmitWriteStream, this.NtlmKeepAlive && this.ContentLength == 0L);
							submitWriteStream.CloseInternal(true);
						}
					}
				}
				else
				{
					if (submitWriteStream != null)
					{
						submitWriteStream.CloseInternal(true);
					}
					this._OldSubmitWriteStream = null;
				}
				this.InvokeGetRequestStreamCallback();
			}
			catch
			{
				ConnectStream submitWriteStream2 = this._SubmitWriteStream;
				if (submitWriteStream2 != null)
				{
					submitWriteStream2.CallDone();
				}
				throw;
			}
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0007D338 File Offset: 0x0007C338
		private int GenerateConnectRequestLine(int headersSize)
		{
			HostHeaderString hostHeaderString = new HostHeaderString(this.HostAndPort(true));
			int num = this.CurrentMethod.Name.Length + hostHeaderString.ByteCount + 12 + headersSize;
			this._WriteBuffer = new byte[num];
			int num2 = Encoding.ASCII.GetBytes(this.CurrentMethod.Name, 0, this.CurrentMethod.Name.Length, this.WriteBuffer, 0);
			this.WriteBuffer[num2++] = 32;
			hostHeaderString.Copy(this.WriteBuffer, num2);
			num2 += hostHeaderString.ByteCount;
			this.WriteBuffer[num2++] = 32;
			return num2;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x0007D3E0 File Offset: 0x0007C3E0
		internal string HostAndPort(bool addDefaultPort)
		{
			Uri uri;
			if (this.IsTunnelRequest)
			{
				uri = this._OriginUri;
			}
			else
			{
				uri = this._Uri;
			}
			string text;
			if (uri.HostNameType == UriHostNameType.IPv6)
			{
				text = "[" + this.TrimScopeID(uri.DnsSafeHost) + "]";
			}
			else
			{
				text = uri.DnsSafeHost;
			}
			if (addDefaultPort || !uri.IsDefaultPort)
			{
				return text + ":" + uri.Port;
			}
			return text;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x0007D458 File Offset: 0x0007C458
		private string TrimScopeID(string s)
		{
			int num = s.LastIndexOf('%');
			if (num > 0)
			{
				return s.Substring(0, num);
			}
			return s;
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0007D47C File Offset: 0x0007C47C
		private int GenerateProxyRequestLine(int headersSize)
		{
			if (this._Uri.Scheme == Uri.UriSchemeFtp)
			{
				return this.GenerateFtpProxyRequestLine(headersSize);
			}
			string components = this._Uri.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
			HostHeaderString hostHeaderString = new HostHeaderString(this.HostAndPort(false));
			string components2 = this._Uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped);
			int num = this.CurrentMethod.Name.Length + components.Length + hostHeaderString.ByteCount + components2.Length + 12 + headersSize;
			this._WriteBuffer = new byte[num];
			int num2 = Encoding.ASCII.GetBytes(this.CurrentMethod.Name, 0, this.CurrentMethod.Name.Length, this.WriteBuffer, 0);
			this.WriteBuffer[num2++] = 32;
			num2 += Encoding.ASCII.GetBytes(components, 0, components.Length, this.WriteBuffer, num2);
			hostHeaderString.Copy(this.WriteBuffer, num2);
			num2 += hostHeaderString.ByteCount;
			num2 += Encoding.ASCII.GetBytes(components2, 0, components2.Length, this.WriteBuffer, num2);
			this.WriteBuffer[num2++] = 32;
			return num2;
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0007D5A8 File Offset: 0x0007C5A8
		private int GenerateFtpProxyRequestLine(int headersSize)
		{
			string components = this._Uri.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
			string text = this._Uri.GetComponents(UriComponents.UserInfo | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
			HostHeaderString hostHeaderString = new HostHeaderString(this.HostAndPort(false));
			string components2 = this._Uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped);
			if (text == "")
			{
				string text2 = null;
				string text3 = null;
				NetworkCredential credential = this.Credentials.GetCredential(this._Uri, "basic");
				if (credential != null && credential != FtpWebRequest.DefaultNetworkCredential)
				{
					text2 = credential.InternalGetDomainUserName();
					text3 = credential.InternalGetPassword();
					text3 = ((text3 == null) ? string.Empty : text3);
				}
				if (text2 != null)
				{
					text2 = text2.Replace(":", "%3A");
					text3 = text3.Replace(":", "%3A");
					text2 = text2.Replace("\\", "%5C");
					text3 = text3.Replace("\\", "%5C");
					text2 = text2.Replace("/", "%2F");
					text3 = text3.Replace("/", "%2F");
					text2 = text2.Replace("?", "%3F");
					text3 = text3.Replace("?", "%3F");
					text2 = text2.Replace("#", "%23");
					text3 = text3.Replace("#", "%23");
					text2 = text2.Replace("%", "%25");
					text3 = text3.Replace("%", "%25");
					text2 = text2.Replace("@", "%40");
					text3 = text3.Replace("@", "%40");
					text = text2 + ":" + text3 + "@";
				}
			}
			int num = this.CurrentMethod.Name.Length + components.Length + text.Length + hostHeaderString.ByteCount + components2.Length + 12 + headersSize;
			this._WriteBuffer = new byte[num];
			int num2 = Encoding.ASCII.GetBytes(this.CurrentMethod.Name, 0, this.CurrentMethod.Name.Length, this.WriteBuffer, 0);
			this.WriteBuffer[num2++] = 32;
			num2 += Encoding.ASCII.GetBytes(components, 0, components.Length, this.WriteBuffer, num2);
			num2 += Encoding.ASCII.GetBytes(text, 0, text.Length, this.WriteBuffer, num2);
			hostHeaderString.Copy(this.WriteBuffer, num2);
			num2 += hostHeaderString.ByteCount;
			num2 += Encoding.ASCII.GetBytes(components2, 0, components2.Length, this.WriteBuffer, num2);
			this.WriteBuffer[num2++] = 32;
			return num2;
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x0007D874 File Offset: 0x0007C874
		private int GenerateRequestLine(int headersSize)
		{
			string pathAndQuery = this._Uri.PathAndQuery;
			int num = this.CurrentMethod.Name.Length + pathAndQuery.Length + 12 + headersSize;
			this._WriteBuffer = new byte[num];
			int num2 = Encoding.ASCII.GetBytes(this.CurrentMethod.Name, 0, this.CurrentMethod.Name.Length, this.WriteBuffer, 0);
			this.WriteBuffer[num2++] = 32;
			num2 += Encoding.ASCII.GetBytes(pathAndQuery, 0, pathAndQuery.Length, this.WriteBuffer, num2);
			this.WriteBuffer[num2++] = 32;
			return num2;
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0007D920 File Offset: 0x0007C920
		internal void UpdateHeaders()
		{
			HostHeaderString hostHeaderString = new HostHeaderString(this.HostAndPort(false));
			string @string = WebHeaderCollection.HeaderEncoding.GetString(hostHeaderString.Bytes, 0, hostHeaderString.ByteCount);
			this._HttpRequestHeaders.ChangeInternal("Host", @string);
			if (this._CookieContainer != null)
			{
				CookieModule.OnSendingHeaders(this);
			}
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0007D96C File Offset: 0x0007C96C
		internal void SerializeHeaders()
		{
			if (this.HttpWriteMode != HttpWriteMode.None)
			{
				if (this.HttpWriteMode == HttpWriteMode.Chunked)
				{
					this._HttpRequestHeaders.AddInternal("Transfer-Encoding", "chunked");
				}
				else if (this.ContentLength >= 0L)
				{
					this._HttpRequestHeaders.ChangeInternal("Content-Length", this._ContentLength.ToString(NumberFormatInfo.InvariantInfo));
				}
				this.ExpectContinue = this.ExpectContinue && !this.IsVersionHttp10 && this.ServicePoint.Expect100Continue;
				if ((this.ContentLength > 0L || this.HttpWriteMode == HttpWriteMode.Chunked) && this.ExpectContinue)
				{
					this._HttpRequestHeaders.AddInternal("Expect", "100-continue");
				}
			}
			if ((this.AutomaticDecompression & DecompressionMethods.GZip) != DecompressionMethods.None)
			{
				if ((this.AutomaticDecompression & DecompressionMethods.Deflate) != DecompressionMethods.None)
				{
					this._HttpRequestHeaders.AddInternal("Accept-Encoding", "gzip, deflate");
				}
				else
				{
					this._HttpRequestHeaders.AddInternal("Accept-Encoding", "gzip");
				}
			}
			else if ((this.AutomaticDecompression & DecompressionMethods.Deflate) != DecompressionMethods.None)
			{
				this._HttpRequestHeaders.AddInternal("Accept-Encoding", "deflate");
			}
			string text = "Connection";
			if (this.UsesProxySemantics || this.IsTunnelRequest)
			{
				this._HttpRequestHeaders.RemoveInternal("Connection");
				text = "Proxy-Connection";
				if (!ValidationHelper.IsBlankString(this.Connection))
				{
					this._HttpRequestHeaders.AddInternal("Proxy-Connection", this._HttpRequestHeaders["Connection"]);
				}
			}
			else
			{
				this._HttpRequestHeaders.RemoveInternal("Proxy-Connection");
			}
			if (this.KeepAlive || this.NtlmKeepAlive)
			{
				if (this.IsVersionHttp10 || this.ServicePoint.HttpBehaviour <= HttpBehaviour.HTTP10)
				{
					this._HttpRequestHeaders.AddInternal((this.UsesProxySemantics || this.IsTunnelRequest) ? "Proxy-Connection" : "Connection", "Keep-Alive");
				}
			}
			else if (!this.IsVersionHttp10)
			{
				this._HttpRequestHeaders.AddInternal(text, "Close");
			}
			string text2 = this._HttpRequestHeaders.ToString();
			int byteCount = WebHeaderCollection.HeaderEncoding.GetByteCount(text2);
			int num;
			if (this.CurrentMethod.ConnectRequest)
			{
				num = this.GenerateConnectRequestLine(byteCount);
			}
			else if (this.UsesProxySemantics)
			{
				num = this.GenerateProxyRequestLine(byteCount);
			}
			else
			{
				num = this.GenerateRequestLine(byteCount);
			}
			Buffer.BlockCopy(HttpWebRequest.HttpBytes, 0, this.WriteBuffer, num, HttpWebRequest.HttpBytes.Length);
			num += HttpWebRequest.HttpBytes.Length;
			this.WriteBuffer[num++] = 49;
			this.WriteBuffer[num++] = 46;
			this.WriteBuffer[num++] = (this.IsVersionHttp10 ? 48 : 49);
			this.WriteBuffer[num++] = 13;
			this.WriteBuffer[num++] = 10;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Request: " + Encoding.ASCII.GetString(this.WriteBuffer, 0, num));
			}
			WebHeaderCollection.HeaderEncoding.GetBytes(text2, 0, text2.Length, this.WriteBuffer, num);
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x0007DC58 File Offset: 0x0007CC58
		internal HttpWebRequest(Uri uri, ServicePoint servicePoint)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "HttpWebRequest", uri);
			}
			new WebPermission(NetworkAccess.Connect, uri).Demand();
			this._HttpRequestHeaders = new WebHeaderCollection(WebHeaderCollectionType.HttpWebRequest);
			this._Proxy = WebRequest.InternalDefaultWebProxy;
			this._HttpWriteMode = HttpWriteMode.Unknown;
			this._MaximumAllowedRedirections = 50;
			this._Timeout = 100000;
			this._TimerQueue = WebRequest.DefaultTimerQueue;
			this._ReadWriteTimeout = 300000;
			this._MaximumResponseHeadersLength = HttpWebRequest.DefaultMaximumResponseHeadersLength;
			this._ContentLength = -1L;
			this._OriginVerb = KnownHttpVerb.Get;
			this._OriginUri = uri;
			this._Uri = this._OriginUri;
			this._ServicePoint = servicePoint;
			this._RequestIsAsync = TriState.Unspecified;
			base.SetupCacheProtocol(this._OriginUri);
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "HttpWebRequest", null);
			}
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x0007DD64 File Offset: 0x0007CD64
		internal HttpWebRequest(Uri proxyUri, Uri requestUri, HttpWebRequest orginalRequest)
			: this(proxyUri, null)
		{
			this._OriginVerb = KnownHttpVerb.Parse("CONNECT");
			this.Pipelined = false;
			this._OriginUri = requestUri;
			this.IsTunnelRequest = true;
			this._ConnectionGroupName = ServicePointManager.SpecialConnectGroupName + "(" + HttpWebRequest.UniqueGroupId + ")";
			this.m_InternalConnectionGroup = true;
			this.ServerAuthenticationState = new AuthenticationState(true);
			base.CacheProtocol = null;
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x0007DDD8 File Offset: 0x0007CDD8
		[Obsolete("Serialization is obsoleted for this type.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected HttpWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "HttpWebRequest", serializationInfo);
			}
			this._HttpRequestHeaders = (WebHeaderCollection)serializationInfo.GetValue("_HttpRequestHeaders", typeof(WebHeaderCollection));
			this._Proxy = (IWebProxy)serializationInfo.GetValue("_Proxy", typeof(IWebProxy));
			this.KeepAlive = serializationInfo.GetBoolean("_KeepAlive");
			this.Pipelined = serializationInfo.GetBoolean("_Pipelined");
			this.AllowAutoRedirect = serializationInfo.GetBoolean("_AllowAutoRedirect");
			this.AllowWriteStreamBuffering = serializationInfo.GetBoolean("_AllowWriteStreamBuffering");
			this.HttpWriteMode = (HttpWriteMode)serializationInfo.GetInt32("_HttpWriteMode");
			this._MaximumAllowedRedirections = serializationInfo.GetInt32("_MaximumAllowedRedirections");
			this._AutoRedirects = serializationInfo.GetInt32("_AutoRedirects");
			this._Timeout = serializationInfo.GetInt32("_Timeout");
			try
			{
				this._ReadWriteTimeout = serializationInfo.GetInt32("_ReadWriteTimeout");
			}
			catch
			{
				this._ReadWriteTimeout = 300000;
			}
			try
			{
				this._MaximumResponseHeadersLength = serializationInfo.GetInt32("_MaximumResponseHeadersLength");
			}
			catch
			{
				this._MaximumResponseHeadersLength = HttpWebRequest.DefaultMaximumResponseHeadersLength;
			}
			this._ContentLength = serializationInfo.GetInt64("_ContentLength");
			this._MediaType = serializationInfo.GetString("_MediaType");
			this._OriginVerb = KnownHttpVerb.Parse(serializationInfo.GetString("_OriginVerb"));
			this._ConnectionGroupName = serializationInfo.GetString("_ConnectionGroupName");
			this.ProtocolVersion = (Version)serializationInfo.GetValue("_Version", typeof(Version));
			this._OriginUri = (Uri)serializationInfo.GetValue("_OriginUri", typeof(Uri));
			base.SetupCacheProtocol(this._OriginUri);
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "HttpWebRequest", null);
			}
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x0007E00C File Offset: 0x0007D00C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x0007E018 File Offset: 0x0007D018
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("_HttpRequestHeaders", this._HttpRequestHeaders, typeof(WebHeaderCollection));
			serializationInfo.AddValue("_Proxy", this._Proxy, typeof(IWebProxy));
			serializationInfo.AddValue("_KeepAlive", this.KeepAlive);
			serializationInfo.AddValue("_Pipelined", this.Pipelined);
			serializationInfo.AddValue("_AllowAutoRedirect", this.AllowAutoRedirect);
			serializationInfo.AddValue("_AllowWriteStreamBuffering", this.AllowWriteStreamBuffering);
			serializationInfo.AddValue("_HttpWriteMode", this.HttpWriteMode);
			serializationInfo.AddValue("_MaximumAllowedRedirections", this._MaximumAllowedRedirections);
			serializationInfo.AddValue("_AutoRedirects", this._AutoRedirects);
			serializationInfo.AddValue("_Timeout", this._Timeout);
			serializationInfo.AddValue("_ReadWriteTimeout", this._ReadWriteTimeout);
			serializationInfo.AddValue("_MaximumResponseHeadersLength", this._MaximumResponseHeadersLength);
			serializationInfo.AddValue("_ContentLength", this.ContentLength);
			serializationInfo.AddValue("_MediaType", this._MediaType);
			serializationInfo.AddValue("_OriginVerb", this._OriginVerb);
			serializationInfo.AddValue("_ConnectionGroupName", this._ConnectionGroupName);
			serializationInfo.AddValue("_Version", this.ProtocolVersion, typeof(Version));
			serializationInfo.AddValue("_OriginUri", this._OriginUri, typeof(Uri));
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x0007E18C File Offset: 0x0007D18C
		internal static StringBuilder GenerateConnectionGroup(string connectionGroupName, bool unsafeConnectionGroup, bool isInternalGroup)
		{
			StringBuilder stringBuilder = new StringBuilder(connectionGroupName);
			stringBuilder.Append(unsafeConnectionGroup ? "U>" : "S>");
			if (isInternalGroup)
			{
				stringBuilder.Append("I>");
			}
			return stringBuilder;
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x0007E1C8 File Offset: 0x0007D1C8
		internal string GetConnectionGroupLine()
		{
			StringBuilder stringBuilder = HttpWebRequest.GenerateConnectionGroup(this._ConnectionGroupName, this.UnsafeAuthenticatedConnectionSharing, this.m_InternalConnectionGroup);
			if (this._Uri.Scheme == Uri.UriSchemeHttps || this.IsTunnelRequest)
			{
				if (this.UsesProxy)
				{
					stringBuilder.Append(this.HostAndPort(true));
					stringBuilder.Append("$");
				}
				if (this._ClientCertificates != null && this.ClientCertificates.Count > 0)
				{
					stringBuilder.Append(this.ClientCertificates.GetHashCode().ToString(NumberFormatInfo.InvariantInfo));
				}
			}
			if (this.ProxyAuthenticationState.UniqueGroupId != null)
			{
				stringBuilder.Append(this.ProxyAuthenticationState.UniqueGroupId);
			}
			else if (this.ServerAuthenticationState.UniqueGroupId != null)
			{
				stringBuilder.Append(this.ServerAuthenticationState.UniqueGroupId);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x0007E2AC File Offset: 0x0007D2AC
		private bool CheckResubmitForAuth()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (this.UsesProxySemantics && this._Proxy != null && this._Proxy.Credentials != null)
			{
				try
				{
					flag |= this.ProxyAuthenticationState.AttemptAuthenticate(this, this._Proxy.Credentials);
				}
				catch (Win32Exception)
				{
					if (!this.m_Extra401Retry)
					{
						throw;
					}
					flag3 = true;
				}
				flag2 = true;
			}
			if (this.Credentials != null && !flag3)
			{
				try
				{
					flag |= this.ServerAuthenticationState.AttemptAuthenticate(this, this.Credentials);
				}
				catch (Win32Exception)
				{
					if (!this.m_Extra401Retry)
					{
						throw;
					}
					flag = false;
				}
				flag2 = true;
			}
			if (!flag && flag2 && this.m_Extra401Retry)
			{
				this.ClearAuthenticatedConnectionResources();
				this.m_Extra401Retry = false;
				flag = true;
			}
			return flag;
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x0007E378 File Offset: 0x0007D378
		private bool CheckResubmitForCache(ref Exception e)
		{
			if (this.CheckCacheRetrieveOnResponse())
			{
				this.CheckCacheUpdateOnResponse();
				return false;
			}
			if (this.AllowAutoRedirect)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, this, "", SR.GetString("net_log_cache_validation_failed_resubmit"));
				}
				return true;
			}
			if (Logging.On)
			{
				Logging.PrintError(Logging.Web, this, "", SR.GetString("net_log_cache_refused_server_response"));
			}
			e = new InvalidOperationException(SR.GetString("net_cache_not_accept_response"));
			return false;
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x0007E3F4 File Offset: 0x0007D3F4
		private bool CheckResubmit(ref Exception e)
		{
			bool flag = false;
			if (this.ResponseStatusCode != HttpStatusCode.Unauthorized)
			{
				if (this.ResponseStatusCode != HttpStatusCode.ProxyAuthenticationRequired)
				{
					goto IL_00B0;
				}
			}
			try
			{
				if (!(flag = this.CheckResubmitForAuth()))
				{
					e = new WebException(SR.GetString("net_servererror", new object[] { NetRes.GetWebStatusCodeString(this.ResponseStatusCode, this._HttpResponse.StatusDescription) }), null, WebExceptionStatus.ProtocolError, this._HttpResponse);
					return false;
				}
				goto IL_04CE;
			}
			catch (Win32Exception ex)
			{
				throw new WebException(SR.GetString("net_servererror", new object[] { NetRes.GetWebStatusCodeString(this.ResponseStatusCode, this._HttpResponse.StatusDescription) }), ex, WebExceptionStatus.ProtocolError, this._HttpResponse);
			}
			IL_00B0:
			if (this.ServerAuthenticationState != null && this.ServerAuthenticationState.Authorization != null)
			{
				HttpWebResponse httpResponse = this._HttpResponse;
				if (httpResponse != null)
				{
					httpResponse.InternalSetIsMutuallyAuthenticated = this.ServerAuthenticationState.Authorization.MutuallyAuthenticated;
					if (base.AuthenticationLevel == AuthenticationLevel.MutualAuthRequired && !httpResponse.IsMutuallyAuthenticated)
					{
						throw new WebException(SR.GetString("net_webstatus_RequestCanceled"), new ProtocolViolationException(SR.GetString("net_mutualauthfailed")), WebExceptionStatus.RequestCanceled, httpResponse);
					}
				}
			}
			if (this.ResponseStatusCode == HttpStatusCode.BadRequest && this.SendChunked && this.ServicePoint.InternalProxyServicePoint)
			{
				this.ClearAuthenticatedConnectionResources();
				return true;
			}
			if (this.AllowAutoRedirect && (this.ResponseStatusCode == HttpStatusCode.MultipleChoices || this.ResponseStatusCode == HttpStatusCode.MovedPermanently || this.ResponseStatusCode == HttpStatusCode.Found || this.ResponseStatusCode == HttpStatusCode.SeeOther || this.ResponseStatusCode == HttpStatusCode.TemporaryRedirect))
			{
				this._AutoRedirects++;
				if (this._AutoRedirects > this._MaximumAllowedRedirections)
				{
					e = new WebException(SR.GetString("net_tooManyRedirections"), null, WebExceptionStatus.ProtocolError, this._HttpResponse);
					return false;
				}
				string location = this._HttpResponse.Headers.Location;
				if (location == null)
				{
					e = new WebException(SR.GetString("net_servererror", new object[] { NetRes.GetWebStatusCodeString(this.ResponseStatusCode, this._HttpResponse.StatusDescription) }), null, WebExceptionStatus.ProtocolError, this._HttpResponse);
					return false;
				}
				Uri uri;
				try
				{
					uri = new Uri(this._Uri, location);
				}
				catch (UriFormatException ex2)
				{
					e = new WebException(SR.GetString("net_resubmitprotofailed"), ex2, WebExceptionStatus.ProtocolError, this._HttpResponse);
					return false;
				}
				if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
				{
					e = new WebException(SR.GetString("net_resubmitprotofailed"), null, WebExceptionStatus.ProtocolError, this._HttpResponse);
					return false;
				}
				try
				{
					ExecutionContext executionContext = (this.Async ? this.GetReadingContext().ContextCopy : null);
					CodeAccessPermission codeAccessPermission = new WebPermission(NetworkAccess.Connect, uri);
					if (executionContext == null)
					{
						codeAccessPermission.Demand();
					}
					else
					{
						ExecutionContext.Run(executionContext, NclUtilities.ContextRelativeDemandCallback, codeAccessPermission);
					}
				}
				catch (SecurityException ex3)
				{
					e = new SecurityException(SR.GetString("net_redirect_perm"), new WebException(SR.GetString("net_resubmitcanceled"), ex3, WebExceptionStatus.ProtocolError, this._HttpResponse));
					return false;
				}
				this._Uri = uri;
				bool flag2 = false;
				if (this.ResponseStatusCode > (HttpStatusCode)299 && Logging.On)
				{
					Logging.PrintWarning(Logging.Web, this, "", SR.GetString("net_log_server_response_error_code", new object[] { ((int)this.ResponseStatusCode).ToString(NumberFormatInfo.InvariantInfo) }));
				}
				HttpStatusCode responseStatusCode = this.ResponseStatusCode;
				switch (responseStatusCode)
				{
				case HttpStatusCode.MovedPermanently:
				case HttpStatusCode.Found:
					if (this.CurrentMethod.Equals(KnownHttpVerb.Post))
					{
						flag2 = true;
					}
					break;
				default:
					if (responseStatusCode != HttpStatusCode.TemporaryRedirect)
					{
						flag2 = true;
					}
					break;
				}
				if (flag2)
				{
					this.CurrentMethod = KnownHttpVerb.Get;
					this.ExpectContinue = false;
					this.HttpWriteMode = HttpWriteMode.None;
				}
				ICredentials credentials = this.Credentials as CredentialCache;
				if (credentials == null)
				{
					credentials = this.Credentials as SystemNetworkCredential;
				}
				if (credentials == null)
				{
					this.Credentials = null;
				}
				this.ProxyAuthenticationState.ClearAuthReq(this);
				this.ServerAuthenticationState.ClearAuthReq(this);
				if (this._OriginUri.Scheme == Uri.UriSchemeHttps)
				{
					this._HttpRequestHeaders.RemoveInternal("Referer");
				}
			}
			else
			{
				if (this.ResponseStatusCode > (HttpStatusCode)399)
				{
					e = new WebException(SR.GetString("net_servererror", new object[] { NetRes.GetWebStatusCodeString(this.ResponseStatusCode, this._HttpResponse.StatusDescription) }), null, WebExceptionStatus.ProtocolError, this._HttpResponse);
					return false;
				}
				if (this.AllowAutoRedirect && this.ResponseStatusCode > (HttpStatusCode)299)
				{
					e = new WebException(SR.GetString("net_servererror", new object[] { NetRes.GetWebStatusCodeString(this.ResponseStatusCode, this._HttpResponse.StatusDescription) }), null, WebExceptionStatus.ProtocolError, this._HttpResponse);
					return false;
				}
				return false;
			}
			IL_04CE:
			if (this.HttpWriteMode != HttpWriteMode.None && !this.AllowWriteStreamBuffering && (this.HttpWriteMode != HttpWriteMode.ContentLength || this.ContentLength != 0L))
			{
				e = new WebException(SR.GetString("net_need_writebuffering"), null, WebExceptionStatus.ProtocolError, this._HttpResponse);
				return false;
			}
			if (!flag)
			{
				this.ClearAuthenticatedConnectionResources();
			}
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, this, "", SR.GetString("net_log_resubmitting_request"));
			}
			return true;
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x0007E964 File Offset: 0x0007D964
		private void ClearRequestForResubmit()
		{
			this._HttpRequestHeaders.RemoveInternal("Host");
			this._HttpRequestHeaders.RemoveInternal("Connection");
			this._HttpRequestHeaders.RemoveInternal("Proxy-Connection");
			this._HttpRequestHeaders.RemoveInternal("Content-Length");
			this._HttpRequestHeaders.RemoveInternal("Transfer-Encoding");
			this._HttpRequestHeaders.RemoveInternal("Expect");
			if (this._HttpResponse != null && this._HttpResponse.ResponseStream != null)
			{
				if (!this._HttpResponse.KeepAlive)
				{
					ConnectStream connectStream = this._HttpResponse.ResponseStream as ConnectStream;
					if (connectStream != null)
					{
						connectStream.ErrorResponseNotify(false);
					}
				}
				ICloseEx closeEx = this._HttpResponse.ResponseStream as ICloseEx;
				if (closeEx != null)
				{
					closeEx.CloseEx(CloseExState.Silent);
				}
				else
				{
					this._HttpResponse.ResponseStream.Close();
				}
			}
			this._AbortDelegate = null;
			if (this._SubmitWriteStream != null)
			{
				if (((this._HttpResponse != null && this._HttpResponse.KeepAlive) || this._SubmitWriteStream.IgnoreSocketErrors) && this.HasEntityBody)
				{
					this.SetRequestContinue();
					if (!this.Async && this.UserRetrievedWriteStream)
					{
						this._SubmitWriteStream.CallDone();
					}
				}
				if ((this.Async || this.UserRetrievedWriteStream) && this._OldSubmitWriteStream != null && this._OldSubmitWriteStream != this._SubmitWriteStream)
				{
					this._SubmitWriteStream.CloseInternal(true);
				}
			}
			this.m_ContinueGate.Reset();
			this._RerequestCount++;
			this.m_BodyStarted = false;
			this.HeadersCompleted = false;
			this._WriteBuffer = null;
			this.m_Extra401Retry = false;
			this._HttpResponse = null;
			if (!this.Aborted && this.Async)
			{
				this._CoreResponse = null;
			}
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x0007EB1C File Offset: 0x0007DB1C
		private void FinishRequest(HttpWebResponse response, Exception errorException)
		{
			if (!this._ReadAResult.InternalPeekCompleted && this.m_Aborted != 1 && response != null && errorException != null)
			{
				response.ResponseStream = this.MakeMemoryStream(response.ResponseStream);
			}
			if (errorException != null && this._SubmitWriteStream != null && !this._SubmitWriteStream.IsClosed)
			{
				this._SubmitWriteStream.ErrorResponseNotify(this._SubmitWriteStream.Connection.KeepAlive);
			}
			if (errorException == null && this._HttpResponse != null && (this._HttpWriteMode == HttpWriteMode.Chunked || this._ContentLength > 0L) && this.ExpectContinue && !this.Saw100Continue && this._ServicePoint.Understands100Continue && !this.IsTunnelRequest && this.ResponseStatusCode <= (HttpStatusCode)299)
			{
				this._ServicePoint.Understands100Continue = false;
			}
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x0007EBE8 File Offset: 0x0007DBE8
		private Stream MakeMemoryStream(Stream stream)
		{
			if (stream == null || stream is SyncMemoryStream)
			{
				return stream;
			}
			SyncMemoryStream syncMemoryStream = new SyncMemoryStream(0);
			try
			{
				if (stream.CanRead)
				{
					byte[] array = new byte[1024];
					int num = ((HttpWebRequest.DefaultMaximumErrorResponseLength == -1) ? array.Length : (HttpWebRequest.DefaultMaximumErrorResponseLength * 1024));
					int num2;
					while ((num2 = stream.Read(array, 0, Math.Min(array.Length, num))) > 0)
					{
						syncMemoryStream.Write(array, 0, num2);
						if (HttpWebRequest.DefaultMaximumErrorResponseLength != -1)
						{
							num -= num2;
						}
					}
				}
				syncMemoryStream.Position = 0L;
			}
			catch
			{
			}
			finally
			{
				try
				{
					ICloseEx closeEx = stream as ICloseEx;
					if (closeEx != null)
					{
						closeEx.CloseEx(CloseExState.Silent);
					}
					else
					{
						stream.Close();
					}
				}
				catch
				{
				}
			}
			return syncMemoryStream;
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x0007ECC0 File Offset: 0x0007DCC0
		public void AddRange(int from, int to)
		{
			this.AddRange("bytes", from, to);
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x0007ECCF File Offset: 0x0007DCCF
		public void AddRange(int range)
		{
			this.AddRange("bytes", range);
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x0007ECE0 File Offset: 0x0007DCE0
		public void AddRange(string rangeSpecifier, int from, int to)
		{
			if (rangeSpecifier == null)
			{
				throw new ArgumentNullException("rangeSpecifier");
			}
			if (from < 0 || to < 0)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_rangetoosmall"));
			}
			if (from > to)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_fromto"));
			}
			if (!WebHeaderCollection.IsValidToken(rangeSpecifier))
			{
				throw new ArgumentException(SR.GetString("net_nottoken"), "rangeSpecifier");
			}
			if (!this.AddRange(rangeSpecifier, from.ToString(NumberFormatInfo.InvariantInfo), to.ToString(NumberFormatInfo.InvariantInfo)))
			{
				throw new InvalidOperationException(SR.GetString("net_rangetype"));
			}
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x0007ED78 File Offset: 0x0007DD78
		public void AddRange(string rangeSpecifier, int range)
		{
			if (rangeSpecifier == null)
			{
				throw new ArgumentNullException("rangeSpecifier");
			}
			if (!WebHeaderCollection.IsValidToken(rangeSpecifier))
			{
				throw new ArgumentException(SR.GetString("net_nottoken"), "rangeSpecifier");
			}
			if (!this.AddRange(rangeSpecifier, range.ToString(NumberFormatInfo.InvariantInfo), (range >= 0) ? "" : null))
			{
				throw new InvalidOperationException(SR.GetString("net_rangetype"));
			}
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x0007EDE4 File Offset: 0x0007DDE4
		private bool AddRange(string rangeSpecifier, string from, string to)
		{
			string text = this._HttpRequestHeaders["Range"];
			if (text == null || text.Length == 0)
			{
				text = rangeSpecifier + "=";
			}
			else
			{
				if (string.Compare(text.Substring(0, text.IndexOf('=')), rangeSpecifier, StringComparison.OrdinalIgnoreCase) != 0)
				{
					return false;
				}
				text = string.Empty;
			}
			text += from.ToString();
			if (to != null)
			{
				text = text + "-" + to;
			}
			this._HttpRequestHeaders.SetAddVerified("Range", text);
			return true;
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600203C RID: 8252 RVA: 0x0007EE6C File Offset: 0x0007DE6C
		private static string UniqueGroupId
		{
			get
			{
				return Interlocked.Increment(ref HttpWebRequest.s_UniqueGroupId).ToString(NumberFormatInfo.InvariantInfo);
			}
		}

		// Token: 0x04001F3B RID: 7995
		internal const HttpStatusCode MaxOkStatus = (HttpStatusCode)299;

		// Token: 0x04001F3C RID: 7996
		private const HttpStatusCode MaxRedirectionStatus = (HttpStatusCode)399;

		// Token: 0x04001F3D RID: 7997
		private const int RequestLineConstantSize = 12;

		// Token: 0x04001F3E RID: 7998
		private const string ContinueHeader = "100-continue";

		// Token: 0x04001F3F RID: 7999
		internal const string ChunkedHeader = "chunked";

		// Token: 0x04001F40 RID: 8000
		internal const string GZipHeader = "gzip";

		// Token: 0x04001F41 RID: 8001
		internal const string DeflateHeader = "deflate";

		// Token: 0x04001F42 RID: 8002
		private const int DefaultReadWriteTimeout = 300000;

		// Token: 0x04001F43 RID: 8003
		internal const int DefaultContinueTimeout = 350;

		// Token: 0x04001F44 RID: 8004
		private bool m_Saw100Continue;

		// Token: 0x04001F45 RID: 8005
		private bool m_KeepAlive = true;

		// Token: 0x04001F46 RID: 8006
		private bool m_LockConnection;

		// Token: 0x04001F47 RID: 8007
		private bool m_NtlmKeepAlive;

		// Token: 0x04001F48 RID: 8008
		private bool m_PreAuthenticate;

		// Token: 0x04001F49 RID: 8009
		private DecompressionMethods m_AutomaticDecompression;

		// Token: 0x04001F4A RID: 8010
		private int m_Aborted;

		// Token: 0x04001F4B RID: 8011
		private bool m_OnceFailed;

		// Token: 0x04001F4C RID: 8012
		private bool m_Pipelined = true;

		// Token: 0x04001F4D RID: 8013
		private bool m_Retry = true;

		// Token: 0x04001F4E RID: 8014
		private bool m_HeadersCompleted;

		// Token: 0x04001F4F RID: 8015
		private bool m_IsCurrentAuthenticationStateProxy;

		// Token: 0x04001F50 RID: 8016
		private bool m_SawInitialResponse;

		// Token: 0x04001F51 RID: 8017
		private bool m_BodyStarted;

		// Token: 0x04001F52 RID: 8018
		private bool m_RequestSubmitted;

		// Token: 0x04001F53 RID: 8019
		private bool m_OriginallyBuffered;

		// Token: 0x04001F54 RID: 8020
		private bool m_Extra401Retry;

		// Token: 0x04001F55 RID: 8021
		private static readonly byte[] HttpBytes = new byte[] { 72, 84, 84, 80, 47 };

		// Token: 0x04001F56 RID: 8022
		private static readonly WaitCallback s_EndWriteHeaders_Part2Callback = new WaitCallback(HttpWebRequest.EndWriteHeaders_Part2Wrapper);

		// Token: 0x04001F57 RID: 8023
		private static readonly TimerThread.Callback s_ContinueTimeoutCallback = new TimerThread.Callback(HttpWebRequest.ContinueTimeoutCallback);

		// Token: 0x04001F58 RID: 8024
		private static readonly TimerThread.Queue s_ContinueTimerQueue = TimerThread.GetOrCreateQueue(350);

		// Token: 0x04001F59 RID: 8025
		private static readonly TimerThread.Callback s_TimeoutCallback = new TimerThread.Callback(HttpWebRequest.TimeoutCallback);

		// Token: 0x04001F5A RID: 8026
		private static readonly WaitCallback s_AbortWrapper = new WaitCallback(HttpWebRequest.AbortWrapper);

		// Token: 0x04001F5B RID: 8027
		private static int s_UniqueGroupId;

		// Token: 0x04001F5C RID: 8028
		private HttpWebRequest.Booleans _Booleans = HttpWebRequest.Booleans.Default;

		// Token: 0x04001F5D RID: 8029
		private DateTime _CachedIfModifedSince = DateTime.MinValue;

		// Token: 0x04001F5E RID: 8030
		private TimerThread.Timer m_ContinueTimer;

		// Token: 0x04001F5F RID: 8031
		private InterlockedGate m_ContinueGate;

		// Token: 0x04001F60 RID: 8032
		private object m_PendingReturnResult;

		// Token: 0x04001F61 RID: 8033
		private LazyAsyncResult _WriteAResult;

		// Token: 0x04001F62 RID: 8034
		private LazyAsyncResult _ReadAResult;

		// Token: 0x04001F63 RID: 8035
		private LazyAsyncResult _ConnectionAResult;

		// Token: 0x04001F64 RID: 8036
		private LazyAsyncResult _ConnectionReaderAResult;

		// Token: 0x04001F65 RID: 8037
		private TriState _RequestIsAsync;

		// Token: 0x04001F66 RID: 8038
		private HttpContinueDelegate _ContinueDelegate;

		// Token: 0x04001F67 RID: 8039
		internal ServicePoint _ServicePoint;

		// Token: 0x04001F68 RID: 8040
		internal HttpWebResponse _HttpResponse;

		// Token: 0x04001F69 RID: 8041
		private object _CoreResponse;

		// Token: 0x04001F6A RID: 8042
		private int _NestedWriteSideCheck;

		// Token: 0x04001F6B RID: 8043
		private KnownHttpVerb _Verb;

		// Token: 0x04001F6C RID: 8044
		private KnownHttpVerb _OriginVerb;

		// Token: 0x04001F6D RID: 8045
		private WebHeaderCollection _HttpRequestHeaders;

		// Token: 0x04001F6E RID: 8046
		private byte[] _WriteBuffer;

		// Token: 0x04001F6F RID: 8047
		private HttpWriteMode _HttpWriteMode;

		// Token: 0x04001F70 RID: 8048
		private Uri _Uri;

		// Token: 0x04001F71 RID: 8049
		private Uri _OriginUri;

		// Token: 0x04001F72 RID: 8050
		private string _MediaType;

		// Token: 0x04001F73 RID: 8051
		private long _ContentLength;

		// Token: 0x04001F74 RID: 8052
		private IWebProxy _Proxy;

		// Token: 0x04001F75 RID: 8053
		private ProxyChain _ProxyChain;

		// Token: 0x04001F76 RID: 8054
		private string _ConnectionGroupName;

		// Token: 0x04001F77 RID: 8055
		private bool m_InternalConnectionGroup;

		// Token: 0x04001F78 RID: 8056
		private AuthenticationState _ProxyAuthenticationState;

		// Token: 0x04001F79 RID: 8057
		private AuthenticationState _ServerAuthenticationState;

		// Token: 0x04001F7A RID: 8058
		private ICredentials _AuthInfo;

		// Token: 0x04001F7B RID: 8059
		private HttpAbortDelegate _AbortDelegate;

		// Token: 0x04001F7C RID: 8060
		private ConnectStream _SubmitWriteStream;

		// Token: 0x04001F7D RID: 8061
		private ConnectStream _OldSubmitWriteStream;

		// Token: 0x04001F7E RID: 8062
		private int _MaximumAllowedRedirections;

		// Token: 0x04001F7F RID: 8063
		private int _AutoRedirects;

		// Token: 0x04001F80 RID: 8064
		private int _RerequestCount;

		// Token: 0x04001F81 RID: 8065
		private int _Timeout;

		// Token: 0x04001F82 RID: 8066
		private TimerThread.Timer _Timer;

		// Token: 0x04001F83 RID: 8067
		private TimerThread.Queue _TimerQueue;

		// Token: 0x04001F84 RID: 8068
		private int _RequestContinueCount;

		// Token: 0x04001F85 RID: 8069
		private int _ReadWriteTimeout;

		// Token: 0x04001F86 RID: 8070
		private CookieContainer _CookieContainer;

		// Token: 0x04001F87 RID: 8071
		private int _MaximumResponseHeadersLength;

		// Token: 0x04001F88 RID: 8072
		private UnlockConnectionDelegate _UnlockDelegate;

		// Token: 0x04001F89 RID: 8073
		private X509CertificateCollection _ClientCertificates;

		// Token: 0x020003E2 RID: 994
		private static class AbortState
		{
			// Token: 0x04001F8A RID: 8074
			public const int Public = 1;

			// Token: 0x04001F8B RID: 8075
			public const int Internal = 2;
		}

		// Token: 0x020003E3 RID: 995
		[Flags]
		private enum Booleans : uint
		{
			// Token: 0x04001F8D RID: 8077
			AllowAutoRedirect = 1U,
			// Token: 0x04001F8E RID: 8078
			AllowWriteStreamBuffering = 2U,
			// Token: 0x04001F8F RID: 8079
			ExpectContinue = 4U,
			// Token: 0x04001F90 RID: 8080
			ProxySet = 16U,
			// Token: 0x04001F91 RID: 8081
			UnsafeAuthenticatedConnectionSharing = 64U,
			// Token: 0x04001F92 RID: 8082
			IsVersionHttp10 = 128U,
			// Token: 0x04001F93 RID: 8083
			SendChunked = 256U,
			// Token: 0x04001F94 RID: 8084
			EnableDecompression = 512U,
			// Token: 0x04001F95 RID: 8085
			IsTunnelRequest = 1024U,
			// Token: 0x04001F96 RID: 8086
			Default = 7U
		}
	}
}
