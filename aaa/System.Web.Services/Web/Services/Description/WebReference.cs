using System;
using System.CodeDom;
using System.Collections.Specialized;
using System.Web.Services.Discovery;

namespace System.Web.Services.Description
{
	// Token: 0x02000127 RID: 295
	public sealed class WebReference
	{
		// Token: 0x060008FA RID: 2298 RVA: 0x000425B0 File Offset: 0x000415B0
		public WebReference(DiscoveryClientDocumentCollection documents, CodeNamespace proxyCode, string protocolName, string appSettingUrlKey, string appSettingBaseUrl)
		{
			if (documents == null)
			{
				throw new ArgumentNullException("documents");
			}
			if (proxyCode == null)
			{
				throw new ArgumentNullException("proxyCode");
			}
			if (appSettingBaseUrl != null && appSettingUrlKey == null)
			{
				throw new ArgumentNullException("appSettingUrlKey");
			}
			this.protocolName = protocolName;
			this.appSettingUrlKey = appSettingUrlKey;
			this.appSettingBaseUrl = appSettingBaseUrl;
			this.documents = documents;
			this.proxyCode = proxyCode;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00042617 File Offset: 0x00041617
		public WebReference(DiscoveryClientDocumentCollection documents, CodeNamespace proxyCode)
			: this(documents, proxyCode, null, null, null)
		{
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00042624 File Offset: 0x00041624
		public WebReference(DiscoveryClientDocumentCollection documents, CodeNamespace proxyCode, string appSettingUrlKey, string appSettingBaseUrl)
			: this(documents, proxyCode, null, appSettingUrlKey, appSettingBaseUrl)
		{
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00042632 File Offset: 0x00041632
		public string AppSettingBaseUrl
		{
			get
			{
				return this.appSettingBaseUrl;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0004263A File Offset: 0x0004163A
		public string AppSettingUrlKey
		{
			get
			{
				return this.appSettingUrlKey;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00042642 File Offset: 0x00041642
		public DiscoveryClientDocumentCollection Documents
		{
			get
			{
				return this.documents;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x0004264A File Offset: 0x0004164A
		public CodeNamespace ProxyCode
		{
			get
			{
				return this.proxyCode;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00042652 File Offset: 0x00041652
		public StringCollection ValidationWarnings
		{
			get
			{
				if (this.validationWarnings == null)
				{
					this.validationWarnings = new StringCollection();
				}
				return this.validationWarnings;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x0004266D File Offset: 0x0004166D
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x00042675 File Offset: 0x00041675
		public ServiceDescriptionImportWarnings Warnings
		{
			get
			{
				return this.warnings;
			}
			set
			{
				this.warnings = value;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x0004267E File Offset: 0x0004167E
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x00042694 File Offset: 0x00041694
		public string ProtocolName
		{
			get
			{
				if (this.protocolName != null)
				{
					return this.protocolName;
				}
				return string.Empty;
			}
			set
			{
				this.protocolName = value;
			}
		}

		// Token: 0x040005E0 RID: 1504
		private CodeNamespace proxyCode;

		// Token: 0x040005E1 RID: 1505
		private DiscoveryClientDocumentCollection documents;

		// Token: 0x040005E2 RID: 1506
		private string appSettingUrlKey;

		// Token: 0x040005E3 RID: 1507
		private string appSettingBaseUrl;

		// Token: 0x040005E4 RID: 1508
		private string protocolName;

		// Token: 0x040005E5 RID: 1509
		private ServiceDescriptionImportWarnings warnings;

		// Token: 0x040005E6 RID: 1510
		private StringCollection validationWarnings;
	}
}
