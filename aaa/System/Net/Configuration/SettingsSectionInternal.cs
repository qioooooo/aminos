using System;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x0200065F RID: 1631
	internal sealed class SettingsSectionInternal
	{
		// Token: 0x06003261 RID: 12897 RVA: 0x000D65BC File Offset: 0x000D55BC
		internal SettingsSectionInternal(SettingsSection section)
		{
			if (section == null)
			{
				section = new SettingsSection();
			}
			this.alwaysUseCompletionPortsForConnect = section.Socket.AlwaysUseCompletionPortsForConnect;
			this.alwaysUseCompletionPortsForAccept = section.Socket.AlwaysUseCompletionPortsForAccept;
			this.checkCertificateName = section.ServicePointManager.CheckCertificateName;
			this.CheckCertificateRevocationList = section.ServicePointManager.CheckCertificateRevocationList;
			this.DnsRefreshTimeout = section.ServicePointManager.DnsRefreshTimeout;
			this.ipv6Enabled = section.Ipv6.Enabled;
			this.EnableDnsRoundRobin = section.ServicePointManager.EnableDnsRoundRobin;
			this.Expect100Continue = section.ServicePointManager.Expect100Continue;
			this.maximumUnauthorizedUploadLength = section.HttpWebRequest.MaximumUnauthorizedUploadLength;
			this.maximumResponseHeadersLength = section.HttpWebRequest.MaximumResponseHeadersLength;
			this.maximumErrorResponseLength = section.HttpWebRequest.MaximumErrorResponseLength;
			this.useUnsafeHeaderParsing = section.HttpWebRequest.UseUnsafeHeaderParsing;
			this.UseNagleAlgorithm = section.ServicePointManager.UseNagleAlgorithm;
			TimeSpan timeSpan = section.WebProxyScript.DownloadTimeout;
			this.downloadTimeout = ((timeSpan == TimeSpan.MaxValue || timeSpan == TimeSpan.Zero) ? (-1) : ((int)timeSpan.TotalMilliseconds));
			this.performanceCountersEnabled = section.PerformanceCounters.Enabled;
			NetworkingPerfCounters.Initialize();
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06003262 RID: 12898 RVA: 0x000D6704 File Offset: 0x000D5704
		internal static SettingsSectionInternal Section
		{
			get
			{
				if (SettingsSectionInternal.s_settings == null)
				{
					lock (SettingsSectionInternal.InternalSyncObject)
					{
						if (SettingsSectionInternal.s_settings == null)
						{
							SettingsSectionInternal.s_settings = new SettingsSectionInternal((SettingsSection)PrivilegedConfigurationManager.GetSection(ConfigurationStrings.SettingsSectionPath));
						}
					}
				}
				return SettingsSectionInternal.s_settings;
			}
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06003263 RID: 12899 RVA: 0x000D6764 File Offset: 0x000D5764
		private static object InternalSyncObject
		{
			get
			{
				if (SettingsSectionInternal.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref SettingsSectionInternal.s_InternalSyncObject, obj, null);
				}
				return SettingsSectionInternal.s_InternalSyncObject;
			}
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x000D6790 File Offset: 0x000D5790
		internal static SettingsSectionInternal GetSection()
		{
			return new SettingsSectionInternal((SettingsSection)PrivilegedConfigurationManager.GetSection(ConfigurationStrings.SettingsSectionPath));
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06003265 RID: 12901 RVA: 0x000D67A6 File Offset: 0x000D57A6
		internal bool AlwaysUseCompletionPortsForAccept
		{
			get
			{
				return this.alwaysUseCompletionPortsForAccept;
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06003266 RID: 12902 RVA: 0x000D67AE File Offset: 0x000D57AE
		internal bool AlwaysUseCompletionPortsForConnect
		{
			get
			{
				return this.alwaysUseCompletionPortsForConnect;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06003267 RID: 12903 RVA: 0x000D67B6 File Offset: 0x000D57B6
		internal bool CheckCertificateName
		{
			get
			{
				return this.checkCertificateName;
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06003268 RID: 12904 RVA: 0x000D67BE File Offset: 0x000D57BE
		// (set) Token: 0x06003269 RID: 12905 RVA: 0x000D67C6 File Offset: 0x000D57C6
		internal bool CheckCertificateRevocationList
		{
			get
			{
				return this.checkCertificateRevocationList;
			}
			set
			{
				this.checkCertificateRevocationList = value;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x0600326A RID: 12906 RVA: 0x000D67CF File Offset: 0x000D57CF
		// (set) Token: 0x0600326B RID: 12907 RVA: 0x000D67D7 File Offset: 0x000D57D7
		internal int DnsRefreshTimeout
		{
			get
			{
				return this.dnsRefreshTimeout;
			}
			set
			{
				this.dnsRefreshTimeout = value;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x0600326C RID: 12908 RVA: 0x000D67E0 File Offset: 0x000D57E0
		internal int DownloadTimeout
		{
			get
			{
				return this.downloadTimeout;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x0600326D RID: 12909 RVA: 0x000D67E8 File Offset: 0x000D57E8
		// (set) Token: 0x0600326E RID: 12910 RVA: 0x000D67F0 File Offset: 0x000D57F0
		internal bool EnableDnsRoundRobin
		{
			get
			{
				return this.enableDnsRoundRobin;
			}
			set
			{
				this.enableDnsRoundRobin = value;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x0600326F RID: 12911 RVA: 0x000D67F9 File Offset: 0x000D57F9
		// (set) Token: 0x06003270 RID: 12912 RVA: 0x000D6801 File Offset: 0x000D5801
		internal bool Expect100Continue
		{
			get
			{
				return this.expect100Continue;
			}
			set
			{
				this.expect100Continue = value;
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06003271 RID: 12913 RVA: 0x000D680A File Offset: 0x000D580A
		internal bool Ipv6Enabled
		{
			get
			{
				return this.ipv6Enabled;
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06003272 RID: 12914 RVA: 0x000D6812 File Offset: 0x000D5812
		// (set) Token: 0x06003273 RID: 12915 RVA: 0x000D681A File Offset: 0x000D581A
		internal int MaximumResponseHeadersLength
		{
			get
			{
				return this.maximumResponseHeadersLength;
			}
			set
			{
				this.maximumResponseHeadersLength = value;
			}
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06003274 RID: 12916 RVA: 0x000D6823 File Offset: 0x000D5823
		internal int MaximumUnauthorizedUploadLength
		{
			get
			{
				return this.maximumUnauthorizedUploadLength;
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06003275 RID: 12917 RVA: 0x000D682B File Offset: 0x000D582B
		// (set) Token: 0x06003276 RID: 12918 RVA: 0x000D6833 File Offset: 0x000D5833
		internal int MaximumErrorResponseLength
		{
			get
			{
				return this.maximumErrorResponseLength;
			}
			set
			{
				this.maximumErrorResponseLength = value;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06003277 RID: 12919 RVA: 0x000D683C File Offset: 0x000D583C
		internal bool UseUnsafeHeaderParsing
		{
			get
			{
				return this.useUnsafeHeaderParsing;
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06003278 RID: 12920 RVA: 0x000D6844 File Offset: 0x000D5844
		// (set) Token: 0x06003279 RID: 12921 RVA: 0x000D684C File Offset: 0x000D584C
		internal bool UseNagleAlgorithm
		{
			get
			{
				return this.useNagleAlgorithm;
			}
			set
			{
				this.useNagleAlgorithm = value;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x0600327A RID: 12922 RVA: 0x000D6855 File Offset: 0x000D5855
		internal bool PerformanceCountersEnabled
		{
			get
			{
				return this.performanceCountersEnabled;
			}
		}

		// Token: 0x04002F32 RID: 12082
		private static object s_InternalSyncObject;

		// Token: 0x04002F33 RID: 12083
		private static SettingsSectionInternal s_settings;

		// Token: 0x04002F34 RID: 12084
		private bool alwaysUseCompletionPortsForAccept;

		// Token: 0x04002F35 RID: 12085
		private bool alwaysUseCompletionPortsForConnect;

		// Token: 0x04002F36 RID: 12086
		private bool checkCertificateName;

		// Token: 0x04002F37 RID: 12087
		private bool checkCertificateRevocationList;

		// Token: 0x04002F38 RID: 12088
		private int downloadTimeout;

		// Token: 0x04002F39 RID: 12089
		private int dnsRefreshTimeout;

		// Token: 0x04002F3A RID: 12090
		private bool enableDnsRoundRobin;

		// Token: 0x04002F3B RID: 12091
		private bool expect100Continue;

		// Token: 0x04002F3C RID: 12092
		private bool ipv6Enabled;

		// Token: 0x04002F3D RID: 12093
		private int maximumResponseHeadersLength;

		// Token: 0x04002F3E RID: 12094
		private int maximumErrorResponseLength;

		// Token: 0x04002F3F RID: 12095
		private int maximumUnauthorizedUploadLength;

		// Token: 0x04002F40 RID: 12096
		private bool useUnsafeHeaderParsing;

		// Token: 0x04002F41 RID: 12097
		private bool useNagleAlgorithm;

		// Token: 0x04002F42 RID: 12098
		private bool performanceCountersEnabled;
	}
}
