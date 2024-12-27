using System;
using System.Configuration;
using System.Net.Cache;
using System.Xml;

namespace System.Net.Configuration
{
	// Token: 0x02000651 RID: 1617
	public sealed class FtpCachePolicyElement : ConfigurationElement
	{
		// Token: 0x06003210 RID: 12816 RVA: 0x000D59A0 File Offset: 0x000D49A0
		public FtpCachePolicyElement()
		{
			this.properties.Add(this.policyLevel);
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06003211 RID: 12817 RVA: 0x000D59F0 File Offset: 0x000D49F0
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06003212 RID: 12818 RVA: 0x000D59F8 File Offset: 0x000D49F8
		// (set) Token: 0x06003213 RID: 12819 RVA: 0x000D5A0B File Offset: 0x000D4A0B
		[ConfigurationProperty("policyLevel", DefaultValue = RequestCacheLevel.Default)]
		public RequestCacheLevel PolicyLevel
		{
			get
			{
				return (RequestCacheLevel)base[this.policyLevel];
			}
			set
			{
				base[this.policyLevel] = value;
			}
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x000D5A1F File Offset: 0x000D4A1F
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			this.wasReadFromConfig = true;
			base.DeserializeElement(reader, serializeCollectionKey);
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x000D5A30 File Offset: 0x000D4A30
		protected override void Reset(ConfigurationElement parentElement)
		{
			if (parentElement != null)
			{
				FtpCachePolicyElement ftpCachePolicyElement = (FtpCachePolicyElement)parentElement;
				this.wasReadFromConfig = ftpCachePolicyElement.wasReadFromConfig;
			}
			base.Reset(parentElement);
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06003216 RID: 12822 RVA: 0x000D5A5A File Offset: 0x000D4A5A
		internal bool WasReadFromConfig
		{
			get
			{
				return this.wasReadFromConfig;
			}
		}

		// Token: 0x04002EFE RID: 12030
		private bool wasReadFromConfig;

		// Token: 0x04002EFF RID: 12031
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F00 RID: 12032
		private readonly ConfigurationProperty policyLevel = new ConfigurationProperty("policyLevel", typeof(RequestCacheLevel), RequestCacheLevel.Default, ConfigurationPropertyOptions.None);
	}
}
