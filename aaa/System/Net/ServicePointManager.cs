using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net.Configuration;
using System.Net.Security;
using System.Security.Authentication;
using System.Threading;

namespace System.Net
{
	// Token: 0x0200043C RID: 1084
	public class ServicePointManager
	{
		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060021C2 RID: 8642 RVA: 0x00085A07 File Offset: 0x00084A07
		// (set) Token: 0x060021C3 RID: 8643 RVA: 0x00085A1F File Offset: 0x00084A1F
		private static int InternalConnectionLimit
		{
			get
			{
				if (ServicePointManager.s_ConfigTable == null)
				{
					ServicePointManager.s_ConfigTable = ServicePointManager.ConfigTable;
				}
				return ServicePointManager.s_ConnectionLimit;
			}
			set
			{
				if (ServicePointManager.s_ConfigTable == null)
				{
					ServicePointManager.s_ConfigTable = ServicePointManager.ConfigTable;
				}
				ServicePointManager.s_UserChangedLimit = true;
				ServicePointManager.s_ConnectionLimit = value;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x00085A3E File Offset: 0x00084A3E
		private static int PersistentConnectionLimit
		{
			get
			{
				if (ComNetOS.IsAspNetServer)
				{
					return 10;
				}
				return 2;
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x00085A4C File Offset: 0x00084A4C
		[Conditional("DEBUG")]
		internal static void Debug(int requestHash)
		{
			try
			{
				foreach (object obj in ServicePointManager.s_ServicePointTable)
				{
					WeakReference weakReference = (WeakReference)obj;
					if (weakReference != null && weakReference.IsAlive)
					{
						ServicePoint servicePoint = (ServicePoint)weakReference.Target;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
			}
			catch
			{
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060021C6 RID: 8646 RVA: 0x00085AF8 File Offset: 0x00084AF8
		private static Hashtable ConfigTable
		{
			get
			{
				if (ServicePointManager.s_ConfigTable == null)
				{
					lock (ServicePointManager.s_ServicePointTable)
					{
						if (ServicePointManager.s_ConfigTable == null)
						{
							Hashtable hashtable2 = ConnectionManagementSectionInternal.GetSection().ConnectionManagement;
							if (hashtable2 == null)
							{
								hashtable2 = new Hashtable();
							}
							if (hashtable2.ContainsKey("*"))
							{
								int num = (int)hashtable2["*"];
								if (num < 1)
								{
									num = ServicePointManager.PersistentConnectionLimit;
								}
								ServicePointManager.s_ConnectionLimit = num;
							}
							ServicePointManager.s_ConfigTable = hashtable2;
						}
					}
				}
				return ServicePointManager.s_ConfigTable;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x00085B88 File Offset: 0x00084B88
		internal static TimerThread.Callback IdleServicePointTimeoutDelegate
		{
			get
			{
				return ServicePointManager.s_IdleServicePointTimeoutDelegate;
			}
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x00085B90 File Offset: 0x00084B90
		private static void IdleServicePointTimeoutCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			ServicePoint servicePoint = (ServicePoint)context;
			lock (ServicePointManager.s_ServicePointTable)
			{
				ServicePointManager.s_ServicePointTable.Remove(servicePoint.LookupString);
			}
			servicePoint.ReleaseAllConnectionGroups();
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x00085BE0 File Offset: 0x00084BE0
		private ServicePointManager()
		{
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x00085BE8 File Offset: 0x00084BE8
		// (set) Token: 0x060021CB RID: 8651 RVA: 0x00085BF4 File Offset: 0x00084BF4
		public static SecurityProtocolType SecurityProtocol
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_SecurityProtocolType;
			}
			set
			{
				ServicePointManager.EnsureConfigurationLoaded();
				ServicePointManager.ValidateSecurityProtocol(value);
				ServicePointManager.s_SecurityProtocolType = value;
			}
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x00085C08 File Offset: 0x00084C08
		private static void ValidateSecurityProtocol(SecurityProtocolType value)
		{
			SecurityProtocolType securityProtocolType = (SecurityProtocolType)4080;
			if ((value & ~(securityProtocolType != (SecurityProtocolType)0)) != (SecurityProtocolType)0)
			{
				throw new NotSupportedException(SR.GetString("net_securityprotocolnotsupported"));
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060021CD RID: 8653 RVA: 0x00085C31 File Offset: 0x00084C31
		internal static bool DisableStrongCrypto
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_disableStrongCrypto;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060021CE RID: 8654 RVA: 0x00085C3D File Offset: 0x00084C3D
		internal static bool DisableSystemDefaultTlsVersions
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_disableSystemDefaultTlsVersions;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x060021CF RID: 8655 RVA: 0x00085C49 File Offset: 0x00084C49
		internal static bool DisableSendAuxRecord
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_disableSendAuxRecord;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060021D0 RID: 8656 RVA: 0x00085C55 File Offset: 0x00084C55
		internal static bool DisableCertificateEKUs
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_dontCheckCertificateEKUs;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x060021D1 RID: 8657 RVA: 0x00085C61 File Offset: 0x00084C61
		internal static SslProtocols DefaultSslProtocols
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_defaultSslProtocols;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060021D2 RID: 8658 RVA: 0x00085C6D File Offset: 0x00084C6D
		internal static bool UseHttpPipeliningAndBufferPooling
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_useHttpPipeliningAndBufferPooling;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060021D3 RID: 8659 RVA: 0x00085C79 File Offset: 0x00084C79
		internal static bool UseStrictRfcInterimResponseHandling
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_useStrictRfcInterimResponseHandling;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060021D4 RID: 8660 RVA: 0x00085C85 File Offset: 0x00084C85
		internal static bool AllowDangerousUnicodeDecompositions
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_allowDangerousUnicodeDecompositions;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060021D5 RID: 8661 RVA: 0x00085C91 File Offset: 0x00084C91
		internal static bool UseStrictIPv6AddressParsing
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_useStrictIPv6AddressParsing;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060021D6 RID: 8662 RVA: 0x00085C9D File Offset: 0x00084C9D
		internal static bool AllowAllUriEncodingExpansion
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_allowAllUriEncodingExpansion;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x060021D7 RID: 8663 RVA: 0x00085CA9 File Offset: 0x00084CA9
		internal static bool AllowNewLineInFtpCommand
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_allowNewLineInFtpCommand;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x00085CB5 File Offset: 0x00084CB5
		// (set) Token: 0x060021D9 RID: 8665 RVA: 0x00085CBC File Offset: 0x00084CBC
		public static int MaxServicePoints
		{
			get
			{
				return ServicePointManager.s_MaxServicePoints;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (!ValidationHelper.ValidateRange(value, 0, 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				ServicePointManager.s_MaxServicePoints = value;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x060021DA RID: 8666 RVA: 0x00085CE7 File Offset: 0x00084CE7
		// (set) Token: 0x060021DB RID: 8667 RVA: 0x00085CEE File Offset: 0x00084CEE
		public static int DefaultConnectionLimit
		{
			get
			{
				return ServicePointManager.InternalConnectionLimit;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (value > 0)
				{
					ServicePointManager.InternalConnectionLimit = value;
					return;
				}
				throw new ArgumentOutOfRangeException(SR.GetString("net_toosmall"));
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x00085D14 File Offset: 0x00084D14
		// (set) Token: 0x060021DD RID: 8669 RVA: 0x00085D20 File Offset: 0x00084D20
		public static int MaxServicePointIdleTime
		{
			get
			{
				return ServicePointManager.s_ServicePointIdlingQueue.Duration;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (!ValidationHelper.ValidateRange(value, -1, 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (ServicePointManager.s_ServicePointIdlingQueue.Duration != value)
				{
					ServicePointManager.s_ServicePointIdlingQueue = TimerThread.GetOrCreateQueue(value);
				}
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x00085D5D File Offset: 0x00084D5D
		// (set) Token: 0x060021DF RID: 8671 RVA: 0x00085D69 File Offset: 0x00084D69
		public static bool UseNagleAlgorithm
		{
			get
			{
				return SettingsSectionInternal.Section.UseNagleAlgorithm;
			}
			set
			{
				SettingsSectionInternal.Section.UseNagleAlgorithm = value;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x060021E0 RID: 8672 RVA: 0x00085D76 File Offset: 0x00084D76
		// (set) Token: 0x060021E1 RID: 8673 RVA: 0x00085D82 File Offset: 0x00084D82
		public static bool Expect100Continue
		{
			get
			{
				return SettingsSectionInternal.Section.Expect100Continue;
			}
			set
			{
				SettingsSectionInternal.Section.Expect100Continue = value;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x060021E2 RID: 8674 RVA: 0x00085D8F File Offset: 0x00084D8F
		// (set) Token: 0x060021E3 RID: 8675 RVA: 0x00085D9B File Offset: 0x00084D9B
		public static bool EnableDnsRoundRobin
		{
			get
			{
				return SettingsSectionInternal.Section.EnableDnsRoundRobin;
			}
			set
			{
				SettingsSectionInternal.Section.EnableDnsRoundRobin = value;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060021E4 RID: 8676 RVA: 0x00085DA8 File Offset: 0x00084DA8
		// (set) Token: 0x060021E5 RID: 8677 RVA: 0x00085DB4 File Offset: 0x00084DB4
		public static int DnsRefreshTimeout
		{
			get
			{
				return SettingsSectionInternal.Section.DnsRefreshTimeout;
			}
			set
			{
				if (value < -1)
				{
					SettingsSectionInternal.Section.DnsRefreshTimeout = -1;
					return;
				}
				SettingsSectionInternal.Section.DnsRefreshTimeout = value;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060021E6 RID: 8678 RVA: 0x00085DD1 File Offset: 0x00084DD1
		// (set) Token: 0x060021E7 RID: 8679 RVA: 0x00085DD8 File Offset: 0x00084DD8
		[Obsolete("CertificatePolicy is obsoleted for this type, please use ServerCertificateValidationCallback instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static ICertificatePolicy CertificatePolicy
		{
			get
			{
				return ServicePointManager.GetLegacyCertificatePolicy();
			}
			set
			{
				ExceptionHelper.UnmanagedPermission.Demand();
				ServicePointManager.s_CertPolicyValidationCallback = new CertPolicyValidationCallback(value);
			}
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x00085DEF File Offset: 0x00084DEF
		internal static ICertificatePolicy GetLegacyCertificatePolicy()
		{
			if (ServicePointManager.s_CertPolicyValidationCallback == null)
			{
				return null;
			}
			return ServicePointManager.s_CertPolicyValidationCallback.CertificatePolicy;
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060021E9 RID: 8681 RVA: 0x00085E04 File Offset: 0x00084E04
		internal static CertPolicyValidationCallback CertPolicyValidationCallback
		{
			get
			{
				return ServicePointManager.s_CertPolicyValidationCallback;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060021EA RID: 8682 RVA: 0x00085E0B File Offset: 0x00084E0B
		// (set) Token: 0x060021EB RID: 8683 RVA: 0x00085E20 File Offset: 0x00084E20
		public static RemoteCertificateValidationCallback ServerCertificateValidationCallback
		{
			get
			{
				if (ServicePointManager.s_ServerCertValidationCallback == null)
				{
					return null;
				}
				return ServicePointManager.s_ServerCertValidationCallback.ValidationCallback;
			}
			set
			{
				ExceptionHelper.InfrastructurePermission.Demand();
				ServicePointManager.s_ServerCertValidationCallback = new ServerCertValidationCallback(value);
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x00085E37 File Offset: 0x00084E37
		internal static ServerCertValidationCallback ServerCertValidationCallback
		{
			get
			{
				return ServicePointManager.s_ServerCertValidationCallback;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060021ED RID: 8685 RVA: 0x00085E3E File Offset: 0x00084E3E
		// (set) Token: 0x060021EE RID: 8686 RVA: 0x00085E4A File Offset: 0x00084E4A
		public static bool CheckCertificateRevocationList
		{
			get
			{
				return SettingsSectionInternal.Section.CheckCertificateRevocationList;
			}
			set
			{
				ExceptionHelper.UnmanagedPermission.Demand();
				SettingsSectionInternal.Section.CheckCertificateRevocationList = value;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060021EF RID: 8687 RVA: 0x00085E61 File Offset: 0x00084E61
		internal static bool CheckCertificateName
		{
			get
			{
				return SettingsSectionInternal.Section.CheckCertificateName;
			}
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x00085E70 File Offset: 0x00084E70
		internal static string MakeQueryString(Uri address)
		{
			if (address.IsDefaultPort)
			{
				return address.Scheme + "://" + address.DnsSafeHost;
			}
			return string.Concat(new string[]
			{
				address.Scheme,
				"://",
				address.DnsSafeHost,
				":",
				address.Port.ToString()
			});
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00085EDC File Offset: 0x00084EDC
		internal static string MakeQueryString(Uri address1, bool isProxy)
		{
			if (isProxy)
			{
				return ServicePointManager.MakeQueryString(address1) + "://proxy";
			}
			return ServicePointManager.MakeQueryString(address1);
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00085EF8 File Offset: 0x00084EF8
		public static ServicePoint FindServicePoint(Uri address)
		{
			return ServicePointManager.FindServicePoint(address, null);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x00085F04 File Offset: 0x00084F04
		public static ServicePoint FindServicePoint(string uriString, IWebProxy proxy)
		{
			Uri uri = new Uri(uriString);
			return ServicePointManager.FindServicePoint(uri, proxy);
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00085F20 File Offset: 0x00084F20
		public static ServicePoint FindServicePoint(Uri address, IWebProxy proxy)
		{
			HttpAbortDelegate httpAbortDelegate = null;
			int num = 0;
			ProxyChain proxyChain;
			return ServicePointManager.FindServicePoint(address, proxy, out proxyChain, ref httpAbortDelegate, ref num);
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x00085F40 File Offset: 0x00084F40
		internal static ServicePoint FindServicePoint(Uri address, IWebProxy proxy, out ProxyChain chain, ref HttpAbortDelegate abortDelegate, ref int abortState)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			bool flag = false;
			chain = null;
			Uri uri = null;
			if (proxy != null && !address.IsLoopback)
			{
				IAutoWebProxy autoWebProxy = proxy as IAutoWebProxy;
				if (autoWebProxy != null)
				{
					chain = autoWebProxy.GetProxies(address);
					abortDelegate = chain.HttpAbortDelegate;
					try
					{
						Thread.MemoryBarrier();
						if (abortState != 0)
						{
							Exception ex = new WebException(NetRes.GetWebStatusString(WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
							throw ex;
						}
						chain.Enumerator.MoveNext();
						uri = chain.Enumerator.Current;
						goto IL_008C;
					}
					finally
					{
						abortDelegate = null;
					}
				}
				if (!proxy.IsBypassed(address))
				{
					uri = proxy.GetProxy(address);
				}
				IL_008C:
				if (uri != null)
				{
					address = uri;
					flag = true;
				}
			}
			return ServicePointManager.FindServicePointHelper(address, flag);
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x00086004 File Offset: 0x00085004
		internal static ServicePoint FindServicePoint(ProxyChain chain)
		{
			if (!chain.Enumerator.MoveNext())
			{
				return null;
			}
			Uri uri = chain.Enumerator.Current;
			return ServicePointManager.FindServicePointHelper((uri == null) ? chain.Destination : uri, uri != null);
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x0008604C File Offset: 0x0008504C
		private static ServicePoint FindServicePointHelper(Uri address, bool isProxyServicePoint)
		{
			if (isProxyServicePoint && address.Scheme != Uri.UriSchemeHttp)
			{
				Exception ex = new NotSupportedException(SR.GetString("net_proxyschemenotsupported", new object[] { address.Scheme }));
				throw ex;
			}
			string text = ServicePointManager.MakeQueryString(address, isProxyServicePoint);
			ServicePoint servicePoint = null;
			lock (ServicePointManager.s_ServicePointTable)
			{
				WeakReference weakReference = ServicePointManager.s_ServicePointTable[text] as WeakReference;
				if (weakReference != null)
				{
					servicePoint = (ServicePoint)weakReference.Target;
				}
				if (servicePoint == null)
				{
					if (ServicePointManager.s_MaxServicePoints > 0 && ServicePointManager.s_ServicePointTable.Count >= ServicePointManager.s_MaxServicePoints)
					{
						Exception ex2 = new InvalidOperationException(SR.GetString("net_maxsrvpoints"));
						throw ex2;
					}
					int num = ServicePointManager.InternalConnectionLimit;
					string text2 = ServicePointManager.MakeQueryString(address);
					bool flag = ServicePointManager.s_UserChangedLimit;
					if (ServicePointManager.ConfigTable.ContainsKey(text2))
					{
						num = (int)ServicePointManager.ConfigTable[text2];
						flag = true;
					}
					servicePoint = new ServicePoint(address, ServicePointManager.s_ServicePointIdlingQueue, num, text, flag, isProxyServicePoint);
					weakReference = new WeakReference(servicePoint);
					ServicePointManager.s_ServicePointTable[text] = weakReference;
				}
			}
			return servicePoint;
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x0008617C File Offset: 0x0008517C
		internal static ServicePoint FindServicePoint(string host, int port)
		{
			if (host == null)
			{
				throw new ArgumentNullException("address");
			}
			bool flag = false;
			string text = "ByHost:" + host + ":" + port.ToString(CultureInfo.InvariantCulture);
			ServicePoint servicePoint = null;
			lock (ServicePointManager.s_ServicePointTable)
			{
				WeakReference weakReference = ServicePointManager.s_ServicePointTable[text] as WeakReference;
				if (weakReference != null)
				{
					servicePoint = (ServicePoint)weakReference.Target;
				}
				if (servicePoint == null)
				{
					if (ServicePointManager.s_MaxServicePoints > 0 && ServicePointManager.s_ServicePointTable.Count >= ServicePointManager.s_MaxServicePoints)
					{
						Exception ex = new InvalidOperationException(SR.GetString("net_maxsrvpoints"));
						throw ex;
					}
					int num = ServicePointManager.InternalConnectionLimit;
					bool flag2 = ServicePointManager.s_UserChangedLimit;
					string text2 = host + ":" + port.ToString(CultureInfo.InvariantCulture);
					if (ServicePointManager.ConfigTable.ContainsKey(text2))
					{
						num = (int)ServicePointManager.ConfigTable[text2];
						flag2 = true;
					}
					servicePoint = new ServicePoint(host, port, ServicePointManager.s_ServicePointIdlingQueue, num, text, flag2, flag);
					weakReference = new WeakReference(servicePoint);
					ServicePointManager.s_ServicePointTable[text] = weakReference;
				}
			}
			return servicePoint;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x000862A8 File Offset: 0x000852A8
		public static void SetTcpKeepAlive(bool enabled, int keepAliveTime, int keepAliveInterval)
		{
			if (!enabled)
			{
				ServicePointManager.s_UseTcpKeepAlive = false;
				ServicePointManager.s_TcpKeepAliveTime = 0;
				ServicePointManager.s_TcpKeepAliveInterval = 0;
				return;
			}
			ServicePointManager.s_UseTcpKeepAlive = true;
			if (keepAliveTime <= 0)
			{
				throw new ArgumentOutOfRangeException("keepAliveTime");
			}
			if (keepAliveInterval <= 0)
			{
				throw new ArgumentOutOfRangeException("keepAliveInterval");
			}
			ServicePointManager.s_TcpKeepAliveTime = keepAliveTime;
			ServicePointManager.s_TcpKeepAliveInterval = keepAliveInterval;
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x000862FC File Offset: 0x000852FC
		private static void EnsureConfigurationLoaded()
		{
			if (ServicePointManager.configurationLoaded)
			{
				return;
			}
			lock (ServicePointManager.configurationLoadedLock)
			{
				if (!ServicePointManager.configurationLoaded)
				{
					ServicePointManager.s_useHttpPipeliningAndBufferPooling = ServicePointManager.TryInitialize<bool>(new ServicePointManager.ConfigurationLoaderDelegate<bool>(ServicePointManager.LoadUseHttpPipeliningAndBufferPoolingConfiguration), false);
					ServicePointManager.s_useStrictRfcInterimResponseHandling = ServicePointManager.TryInitialize<bool>(new ServicePointManager.ConfigurationLoaderDelegate<bool>(ServicePointManager.LoadUseStrictRfcInterimResponseHandlingConfiguration), true);
					ServicePointManager.s_allowDangerousUnicodeDecompositions = ServicePointManager.TryInitialize<bool>(new ServicePointManager.ConfigurationLoaderDelegate<bool>(ServicePointManager.LoadAllowDangerousUnicodeDecompositionsConfiguration), false);
					ServicePointManager.s_useStrictIPv6AddressParsing = ServicePointManager.TryInitialize<bool>(new ServicePointManager.ConfigurationLoaderDelegate<bool>(ServicePointManager.LoadUseStrictIPv6AddressParsingConfiguration), true);
					ServicePointManager.s_allowAllUriEncodingExpansion = ServicePointManager.TryInitialize<bool>(new ServicePointManager.ConfigurationLoaderDelegate<bool>(ServicePointManager.LoadAllowAllUriEncodingExpansionConfiguration), false);
					ServicePointManager.s_allowNewLineInFtpCommand = ServicePointManager.TryInitialize<bool>(new ServicePointManager.ConfigurationLoaderDelegate<bool>(ServicePointManager.LoadAllowNewLineInFtpCommandConfiguration), false);
					ServicePointManager.s_disableStrongCrypto = ServicePointManager.TryInitialize<bool>(new ServicePointManager.ConfigurationLoaderDelegate<bool>(ServicePointManager.LoadDisableStrongCryptoConfiguration), true);
					ServicePointManager.s_disableSendAuxRecord = ServicePointManager.TryInitialize<bool>(new ServicePointManager.ConfigurationLoaderDelegate<bool>(ServicePointManager.LoadDisableSendAuxRecordConfiguration), false);
					ServicePointManager.s_disableSystemDefaultTlsVersions = ServicePointManager.TryInitialize<bool>(new ServicePointManager.ConfigurationLoaderDelegate<bool>(ServicePointManager.LoadDisableSystemDefaultTlsVersionsConfiguration), true);
					ServicePointManager.s_dontCheckCertificateEKUs = ServicePointManager.TryInitialize<bool>(new ServicePointManager.ConfigurationLoaderDelegate<bool>(ServicePointManager.LoadDisableCertificateEKUsConfiguration), false);
					ServicePointManager.s_defaultSslProtocols = ServicePointManager.TryInitialize<SslProtocols>(new ServicePointManager.ConfigurationLoaderDelegate<SslProtocols>(ServicePointManager.LoadSecureProtocolConfiguration), SslProtocols.Default);
					ServicePointManager.s_SecurityProtocolType = (SecurityProtocolType)ServicePointManager.s_defaultSslProtocols;
					ServicePointManager.configurationLoaded = true;
				}
			}
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x00086468 File Offset: 0x00085468
		private static T TryInitialize<T>(ServicePointManager.ConfigurationLoaderDelegate<T> loadConfiguration, T fallbackDefault)
		{
			T t;
			try
			{
				t = loadConfiguration(fallbackDefault);
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				t = fallbackDefault;
			}
			return t;
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x000864A0 File Offset: 0x000854A0
		private static bool LoadDisableStrongCryptoConfiguration(bool disable)
		{
			int num = RegistryConfiguration.GlobalConfigReadInt("SchUseStrongCrypto", 0);
			return num != 1;
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000864C8 File Offset: 0x000854C8
		private static bool LoadDisableSendAuxRecordConfiguration(bool disable)
		{
			return RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.SchSendAuxRecord", 1) == 0 || RegistryConfiguration.GlobalConfigReadInt("SchSendAuxRecord", 1) == 0 || disable;
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x000864F8 File Offset: 0x000854F8
		private static bool LoadDisableSystemDefaultTlsVersionsConfiguration(bool disable)
		{
			int num = RegistryConfiguration.GlobalConfigReadInt("SystemDefaultTlsVersions", 0);
			disable = num != 1;
			if (!disable)
			{
				int num2 = RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.SystemDefaultTlsVersions", 1);
				disable = num2 != 1;
			}
			return disable;
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00086534 File Offset: 0x00085534
		private static SslProtocols LoadSecureProtocolConfiguration(SslProtocols defaultValue)
		{
			if (!ServicePointManager.s_disableSystemDefaultTlsVersions)
			{
				defaultValue = SslProtocols.None;
			}
			if (!ServicePointManager.s_disableStrongCrypto || !ServicePointManager.s_disableSystemDefaultTlsVersions)
			{
				string text = RegistryConfiguration.AppConfigReadString("System.Net.ServicePointManager.SecurityProtocol", null);
				try
				{
					SecurityProtocolType securityProtocolType = (SecurityProtocolType)Enum.Parse(typeof(SecurityProtocolType), text);
					ServicePointManager.ValidateSecurityProtocol(securityProtocolType);
					defaultValue = (SslProtocols)securityProtocolType;
				}
				catch (ArgumentNullException)
				{
				}
				catch (ArgumentException)
				{
				}
				catch (NotSupportedException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			return defaultValue;
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x000865C8 File Offset: 0x000855C8
		private static bool LoadDisableCertificateEKUsConfiguration(bool disable)
		{
			return RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.RequireCertificateEKUs", 1) == 0 || RegistryConfiguration.GlobalConfigReadInt("RequireCertificateEKUs", 1) == 0 || disable;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000865F8 File Offset: 0x000855F8
		private static bool LoadUseHttpPipeliningAndBufferPoolingConfiguration(bool useFeature)
		{
			int num = RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.UseHttpPipeliningAndBufferPooling", 0);
			if (num == 1)
			{
				return true;
			}
			num = RegistryConfiguration.GlobalConfigReadInt("UseHttpPipeliningAndBufferPooling", 0);
			return num == 1 || useFeature;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x0008662C File Offset: 0x0008562C
		private static bool LoadUseStrictRfcInterimResponseHandlingConfiguration(bool useFeature)
		{
			return RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.UseStrictRfcInterimResponseHandling", 1) != 0 && RegistryConfiguration.GlobalConfigReadInt("UseStrictRfcInterimResponseHandling", 1) != 0 && useFeature;
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x0008665C File Offset: 0x0008565C
		private static bool LoadAllowDangerousUnicodeDecompositionsConfiguration(bool useFeature)
		{
			int num = RegistryConfiguration.AppConfigReadInt("System.Uri.AllowDangerousUnicodeDecompositions", 0);
			if (num == 1)
			{
				return true;
			}
			num = RegistryConfiguration.GlobalConfigReadInt("AllowDangerousUnicodeDecompositions", 0);
			return num == 1 || useFeature;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x00086690 File Offset: 0x00085690
		private static bool LoadUseStrictIPv6AddressParsingConfiguration(bool useFeature)
		{
			return RegistryConfiguration.AppConfigReadInt("System.Uri.UseStrictIPv6AddressParsing", 1) != 0 && RegistryConfiguration.GlobalConfigReadInt("UseStrictIPv6AddressParsing", 1) != 0 && useFeature;
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000866C0 File Offset: 0x000856C0
		private static bool LoadAllowAllUriEncodingExpansionConfiguration(bool useFeature)
		{
			int num = RegistryConfiguration.AppConfigReadInt("System.Uri.AllowAllUriEncodingExpansion", 0);
			if (num == 1)
			{
				return true;
			}
			num = RegistryConfiguration.GlobalConfigReadInt("AllowAllUriEncodingExpansion", 0);
			return num == 1 || useFeature;
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000866F4 File Offset: 0x000856F4
		private static bool LoadAllowNewLineInFtpCommandConfiguration(bool useFeature)
		{
			int num = RegistryConfiguration.AppConfigReadInt("System.Net.AllowNewLineInFtpCommand", 0);
			if (num == 1)
			{
				return true;
			}
			num = RegistryConfiguration.GlobalConfigReadInt("AllowNewLineInFtpCommand", 0);
			return num == 1 || useFeature;
		}

		// Token: 0x040021D0 RID: 8656
		public const int DefaultNonPersistentConnectionLimit = 4;

		// Token: 0x040021D1 RID: 8657
		public const int DefaultPersistentConnectionLimit = 2;

		// Token: 0x040021D2 RID: 8658
		private const int DefaultAspPersistentConnectionLimit = 10;

		// Token: 0x040021D3 RID: 8659
		private const string RegistryGlobalStrongCryptoName = "SchUseStrongCrypto";

		// Token: 0x040021D4 RID: 8660
		private const string RegistryGlobalSendAuxRecordName = "SchSendAuxRecord";

		// Token: 0x040021D5 RID: 8661
		private const string RegistryLocalSendAuxRecordName = "System.Net.ServicePointManager.SchSendAuxRecord";

		// Token: 0x040021D6 RID: 8662
		private const string RegistryGlobalSystemDefaultTlsVersionsName = "SystemDefaultTlsVersions";

		// Token: 0x040021D7 RID: 8663
		private const string RegistryLocalSystemDefaultTlsVersionsName = "System.Net.ServicePointManager.SystemDefaultTlsVersions";

		// Token: 0x040021D8 RID: 8664
		private const string RegistryLocalSecureProtocolName = "System.Net.ServicePointManager.SecurityProtocol";

		// Token: 0x040021D9 RID: 8665
		private const string RegistryGlobalRequireCertificateEKUs = "RequireCertificateEKUs";

		// Token: 0x040021DA RID: 8666
		private const string RegistryLocalRequireCertificateEKUs = "System.Net.ServicePointManager.RequireCertificateEKUs";

		// Token: 0x040021DB RID: 8667
		private const string RegistryGlobalUseHttpPipeliningAndBufferPooling = "UseHttpPipeliningAndBufferPooling";

		// Token: 0x040021DC RID: 8668
		private const string RegistryLocalUseHttpPipeliningAndBufferPooling = "System.Net.ServicePointManager.UseHttpPipeliningAndBufferPooling";

		// Token: 0x040021DD RID: 8669
		private const string RegistryGlobalUseStrictRfcInterimResponseHandling = "UseStrictRfcInterimResponseHandling";

		// Token: 0x040021DE RID: 8670
		private const string RegistryLocalUseStrictRfcInterimResponseHandling = "System.Net.ServicePointManager.UseStrictRfcInterimResponseHandling";

		// Token: 0x040021DF RID: 8671
		private const string RegistryGlobalAllowDangerousUnicodeDecompositions = "AllowDangerousUnicodeDecompositions";

		// Token: 0x040021E0 RID: 8672
		private const string RegistryLocalAllowDangerousUnicodeDecompositions = "System.Uri.AllowDangerousUnicodeDecompositions";

		// Token: 0x040021E1 RID: 8673
		private const string RegistryGlobalUseStrictIPv6AddressParsing = "UseStrictIPv6AddressParsing";

		// Token: 0x040021E2 RID: 8674
		private const string RegistryLocalUseStrictIPv6AddressParsing = "System.Uri.UseStrictIPv6AddressParsing";

		// Token: 0x040021E3 RID: 8675
		private const string RegistryGlobalAllowAllUriEncodingExpansion = "AllowAllUriEncodingExpansion";

		// Token: 0x040021E4 RID: 8676
		private const string RegistryLocalAllowAllUriEncodingExpansion = "System.Uri.AllowAllUriEncodingExpansion";

		// Token: 0x040021E5 RID: 8677
		private const string RegistryGlobalAllowNewLineInFtpCommand = "AllowNewLineInFtpCommand";

		// Token: 0x040021E6 RID: 8678
		private const string RegistryLocalAllowNewLineInFtpCommand = "System.Net.AllowNewLineInFtpCommand";

		// Token: 0x040021E7 RID: 8679
		internal static readonly string SpecialConnectGroupName = "/.NET/NetClasses/HttpWebRequest/CONNECT__Group$$/";

		// Token: 0x040021E8 RID: 8680
		internal static readonly TimerThread.Callback s_IdleServicePointTimeoutDelegate = new TimerThread.Callback(ServicePointManager.IdleServicePointTimeoutCallback);

		// Token: 0x040021E9 RID: 8681
		private static Hashtable s_ServicePointTable = new Hashtable(10);

		// Token: 0x040021EA RID: 8682
		private static TimerThread.Queue s_ServicePointIdlingQueue = TimerThread.GetOrCreateQueue(100000);

		// Token: 0x040021EB RID: 8683
		private static int s_MaxServicePoints = 0;

		// Token: 0x040021EC RID: 8684
		private static CertPolicyValidationCallback s_CertPolicyValidationCallback = new CertPolicyValidationCallback();

		// Token: 0x040021ED RID: 8685
		private static ServerCertValidationCallback s_ServerCertValidationCallback = null;

		// Token: 0x040021EE RID: 8686
		private static bool s_disableStrongCrypto;

		// Token: 0x040021EF RID: 8687
		private static bool s_disableSendAuxRecord;

		// Token: 0x040021F0 RID: 8688
		private static bool s_disableSystemDefaultTlsVersions;

		// Token: 0x040021F1 RID: 8689
		private static SslProtocols s_defaultSslProtocols;

		// Token: 0x040021F2 RID: 8690
		private static bool s_dontCheckCertificateEKUs;

		// Token: 0x040021F3 RID: 8691
		private static bool s_useHttpPipeliningAndBufferPooling;

		// Token: 0x040021F4 RID: 8692
		private static bool s_useStrictRfcInterimResponseHandling;

		// Token: 0x040021F5 RID: 8693
		private static bool s_allowDangerousUnicodeDecompositions;

		// Token: 0x040021F6 RID: 8694
		private static bool s_useStrictIPv6AddressParsing;

		// Token: 0x040021F7 RID: 8695
		private static bool s_allowAllUriEncodingExpansion;

		// Token: 0x040021F8 RID: 8696
		private static bool s_allowNewLineInFtpCommand;

		// Token: 0x040021F9 RID: 8697
		private static Hashtable s_ConfigTable = null;

		// Token: 0x040021FA RID: 8698
		private static int s_ConnectionLimit = ServicePointManager.PersistentConnectionLimit;

		// Token: 0x040021FB RID: 8699
		private static SecurityProtocolType s_SecurityProtocolType;

		// Token: 0x040021FC RID: 8700
		internal static bool s_UseTcpKeepAlive = false;

		// Token: 0x040021FD RID: 8701
		internal static int s_TcpKeepAliveTime;

		// Token: 0x040021FE RID: 8702
		internal static int s_TcpKeepAliveInterval;

		// Token: 0x040021FF RID: 8703
		private static bool s_UserChangedLimit;

		// Token: 0x04002200 RID: 8704
		private static object configurationLoadedLock = new object();

		// Token: 0x04002201 RID: 8705
		private static volatile bool configurationLoaded = false;

		// Token: 0x0200043D RID: 1085
		// (Invoke) Token: 0x06002209 RID: 8713
		private delegate T ConfigurationLoaderDelegate<T>(T initialValue);
	}
}
