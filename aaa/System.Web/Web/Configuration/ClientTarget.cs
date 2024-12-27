using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001BC RID: 444
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ClientTarget : ConfigurationElement
	{
		// Token: 0x06001980 RID: 6528 RVA: 0x0007911C File Offset: 0x0007811C
		static ClientTarget()
		{
			ClientTarget._properties.Add(ClientTarget._propAlias);
			ClientTarget._properties.Add(ClientTarget._propUserAgent);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00079193 File Offset: 0x00078193
		internal ClientTarget()
		{
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x0007919B File Offset: 0x0007819B
		public ClientTarget(string alias, string userAgent)
		{
			base[ClientTarget._propAlias] = alias;
			base[ClientTarget._propUserAgent] = userAgent;
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001983 RID: 6531 RVA: 0x000791BB File Offset: 0x000781BB
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ClientTarget._properties;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001984 RID: 6532 RVA: 0x000791C2 File Offset: 0x000781C2
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("alias", IsRequired = true, IsKey = true)]
		public string Alias
		{
			get
			{
				return (string)base[ClientTarget._propAlias];
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001985 RID: 6533 RVA: 0x000791D4 File Offset: 0x000781D4
		[ConfigurationProperty("userAgent", IsRequired = true)]
		[StringValidator(MinLength = 1)]
		public string UserAgent
		{
			get
			{
				return (string)base[ClientTarget._propUserAgent];
			}
		}

		// Token: 0x04001749 RID: 5961
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400174A RID: 5962
		private static readonly ConfigurationProperty _propAlias = new ConfigurationProperty("alias", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x0400174B RID: 5963
		private static readonly ConfigurationProperty _propUserAgent = new ConfigurationProperty("userAgent", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired);
	}
}
