using System;
using System.Globalization;

namespace System.Net.Configuration
{
	// Token: 0x02000647 RID: 1607
	internal static class ConfigurationStrings
	{
		// Token: 0x060031C1 RID: 12737 RVA: 0x000D4AE8 File Offset: 0x000D3AE8
		private static string GetSectionPath(string sectionName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[] { "system.net", sectionName });
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x000D4B18 File Offset: 0x000D3B18
		private static string GetSectionPath(string sectionName, string subSectionName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}", new object[] { "system.net", sectionName, subSectionName });
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x060031C3 RID: 12739 RVA: 0x000D4B4C File Offset: 0x000D3B4C
		internal static string AuthenticationModulesSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("authenticationModules");
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x060031C4 RID: 12740 RVA: 0x000D4B58 File Offset: 0x000D3B58
		internal static string ConnectionManagementSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("connectionManagement");
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x060031C5 RID: 12741 RVA: 0x000D4B64 File Offset: 0x000D3B64
		internal static string DefaultProxySectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("defaultProxy");
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x060031C6 RID: 12742 RVA: 0x000D4B70 File Offset: 0x000D3B70
		internal static string SmtpSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("mailSettings", "smtp");
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x060031C7 RID: 12743 RVA: 0x000D4B81 File Offset: 0x000D3B81
		internal static string RequestCachingSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("requestCaching");
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x060031C8 RID: 12744 RVA: 0x000D4B8D File Offset: 0x000D3B8D
		internal static string SettingsSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("settings");
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x060031C9 RID: 12745 RVA: 0x000D4B99 File Offset: 0x000D3B99
		internal static string WebRequestModulesSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("webRequestModules");
			}
		}

		// Token: 0x04002E9A RID: 11930
		internal const string Address = "address";

		// Token: 0x04002E9B RID: 11931
		internal const string AutoDetect = "autoDetect";

		// Token: 0x04002E9C RID: 11932
		internal const string AlwaysUseCompletionPortsForAccept = "alwaysUseCompletionPortsForAccept";

		// Token: 0x04002E9D RID: 11933
		internal const string AlwaysUseCompletionPortsForConnect = "alwaysUseCompletionPortsForConnect";

		// Token: 0x04002E9E RID: 11934
		internal const string AuthenticationModulesSectionName = "authenticationModules";

		// Token: 0x04002E9F RID: 11935
		internal const string BypassList = "bypasslist";

		// Token: 0x04002EA0 RID: 11936
		internal const string BypassOnLocal = "bypassonlocal";

		// Token: 0x04002EA1 RID: 11937
		internal const string CheckCertificateName = "checkCertificateName";

		// Token: 0x04002EA2 RID: 11938
		internal const string CheckCertificateRevocationList = "checkCertificateRevocationList";

		// Token: 0x04002EA3 RID: 11939
		internal const string ClientDomain = "clientDomain";

		// Token: 0x04002EA4 RID: 11940
		internal const string ConnectionManagementSectionName = "connectionManagement";

		// Token: 0x04002EA5 RID: 11941
		internal const string DefaultCredentials = "defaultCredentials";

		// Token: 0x04002EA6 RID: 11942
		internal const string DefaultHttpCachePolicy = "defaultHttpCachePolicy";

		// Token: 0x04002EA7 RID: 11943
		internal const string DefaultFtpCachePolicy = "defaultFtpCachePolicy";

		// Token: 0x04002EA8 RID: 11944
		internal const string DefaultPolicyLevel = "defaultPolicyLevel";

		// Token: 0x04002EA9 RID: 11945
		internal const string DefaultProxySectionName = "defaultProxy";

		// Token: 0x04002EAA RID: 11946
		internal const string DeliveryMethod = "deliveryMethod";

		// Token: 0x04002EAB RID: 11947
		internal const string DisableAllCaching = "disableAllCaching";

		// Token: 0x04002EAC RID: 11948
		internal const string DnsRefreshTimeout = "dnsRefreshTimeout";

		// Token: 0x04002EAD RID: 11949
		internal const string DownloadTimeout = "downloadTimeout";

		// Token: 0x04002EAE RID: 11950
		internal const string Enabled = "enabled";

		// Token: 0x04002EAF RID: 11951
		internal const string EnableDnsRoundRobin = "enableDnsRoundRobin";

		// Token: 0x04002EB0 RID: 11952
		internal const string Expect100Continue = "expect100Continue";

		// Token: 0x04002EB1 RID: 11953
		internal const string File = "file:";

		// Token: 0x04002EB2 RID: 11954
		internal const string From = "from";

		// Token: 0x04002EB3 RID: 11955
		internal const string Ftp = "ftp:";

		// Token: 0x04002EB4 RID: 11956
		internal const string Host = "host";

		// Token: 0x04002EB5 RID: 11957
		internal const string HttpWebRequest = "httpWebRequest";

		// Token: 0x04002EB6 RID: 11958
		internal const string Http = "http:";

		// Token: 0x04002EB7 RID: 11959
		internal const string Https = "https:";

		// Token: 0x04002EB8 RID: 11960
		internal const string Ipv6 = "ipv6";

		// Token: 0x04002EB9 RID: 11961
		internal const string IsPrivateCache = "isPrivateCache";

		// Token: 0x04002EBA RID: 11962
		internal const string MailSettingsSectionName = "mailSettings";

		// Token: 0x04002EBB RID: 11963
		internal const string MaxConnection = "maxconnection";

		// Token: 0x04002EBC RID: 11964
		internal const string MaximumAge = "maximumAge";

		// Token: 0x04002EBD RID: 11965
		internal const string MaximumStale = "maximumStale";

		// Token: 0x04002EBE RID: 11966
		internal const string MaximumResponseHeadersLength = "maximumResponseHeadersLength";

		// Token: 0x04002EBF RID: 11967
		internal const string MaximumErrorResponseLength = "maximumErrorResponseLength";

		// Token: 0x04002EC0 RID: 11968
		internal const string MinimumFresh = "minimumFresh";

		// Token: 0x04002EC1 RID: 11969
		internal const string Module = "module";

		// Token: 0x04002EC2 RID: 11970
		internal const string Name = "name";

		// Token: 0x04002EC3 RID: 11971
		internal const string Network = "network";

		// Token: 0x04002EC4 RID: 11972
		internal const string Password = "password";

		// Token: 0x04002EC5 RID: 11973
		internal const string PerformanceCounters = "performanceCounters";

		// Token: 0x04002EC6 RID: 11974
		internal const string PickupDirectoryFromIis = "pickupDirectoryFromIis";

		// Token: 0x04002EC7 RID: 11975
		internal const string PickupDirectoryLocation = "pickupDirectoryLocation";

		// Token: 0x04002EC8 RID: 11976
		internal const string PolicyLevel = "policyLevel";

		// Token: 0x04002EC9 RID: 11977
		internal const string Port = "port";

		// Token: 0x04002ECA RID: 11978
		internal const string Prefix = "prefix";

		// Token: 0x04002ECB RID: 11979
		internal const string Proxy = "proxy";

		// Token: 0x04002ECC RID: 11980
		internal const string ProxyAddress = "proxyaddress";

		// Token: 0x04002ECD RID: 11981
		internal const string RequestCachingSectionName = "requestCaching";

		// Token: 0x04002ECE RID: 11982
		internal const string ScriptLocation = "scriptLocation";

		// Token: 0x04002ECF RID: 11983
		internal const string SectionGroupName = "system.net";

		// Token: 0x04002ED0 RID: 11984
		internal const string ServicePointManager = "servicePointManager";

		// Token: 0x04002ED1 RID: 11985
		internal const string SettingsSectionName = "settings";

		// Token: 0x04002ED2 RID: 11986
		internal const string SmtpSectionName = "smtp";

		// Token: 0x04002ED3 RID: 11987
		internal const string Socket = "socket";

		// Token: 0x04002ED4 RID: 11988
		internal const string SpecifiedPickupDirectory = "specifiedPickupDirectory";

		// Token: 0x04002ED5 RID: 11989
		internal const string TargetName = "targetName";

		// Token: 0x04002ED6 RID: 11990
		internal const string Type = "type";

		// Token: 0x04002ED7 RID: 11991
		internal const string UnspecifiedMaximumAge = "unspecifiedMaximumAge";

		// Token: 0x04002ED8 RID: 11992
		internal const string UseDefaultCredentials = "useDefaultCredentials";

		// Token: 0x04002ED9 RID: 11993
		internal const string UseNagleAlgorithm = "useNagleAlgorithm";

		// Token: 0x04002EDA RID: 11994
		internal const string UseSystemDefault = "usesystemdefault";

		// Token: 0x04002EDB RID: 11995
		internal const string UseUnsafeHeaderParsing = "useUnsafeHeaderParsing";

		// Token: 0x04002EDC RID: 11996
		internal const string UserName = "userName";

		// Token: 0x04002EDD RID: 11997
		internal const string WebProxyScript = "webProxyScript";

		// Token: 0x04002EDE RID: 11998
		internal const string WebRequestModulesSectionName = "webRequestModules";

		// Token: 0x04002EDF RID: 11999
		internal const string maximumUnauthorizedUploadLength = "maximumUnauthorizedUploadLength";
	}
}
