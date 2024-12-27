using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000260 RID: 608
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class UrlMappingsSection : ConfigurationSection
	{
		// Token: 0x06002011 RID: 8209 RVA: 0x0008C678 File Offset: 0x0008B678
		static UrlMappingsSection()
		{
			UrlMappingsSection._properties.Add(UrlMappingsSection._propMappings);
			UrlMappingsSection._properties.Add(UrlMappingsSection._propEnabled);
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002012 RID: 8210 RVA: 0x0008C6E4 File Offset: 0x0008B6E4
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return UrlMappingsSection._properties;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x0008C6EB File Offset: 0x0008B6EB
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public UrlMappingCollection UrlMappings
		{
			get
			{
				return (UrlMappingCollection)base[UrlMappingsSection._propMappings];
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x0008C6FD File Offset: 0x0008B6FD
		// (set) Token: 0x06002015 RID: 8213 RVA: 0x0008C70F File Offset: 0x0008B70F
		[ConfigurationProperty("enabled", DefaultValue = true)]
		public bool IsEnabled
		{
			get
			{
				return (bool)base[UrlMappingsSection._propEnabled];
			}
			set
			{
				base[UrlMappingsSection._propEnabled] = value;
			}
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x0008C724 File Offset: 0x0008B724
		internal string HttpResolveMapping(string path)
		{
			string text = null;
			string text2 = UrlPath.MakeVirtualPathAppRelative(path);
			UrlMapping urlMapping = this.UrlMappings[text2];
			if (urlMapping != null)
			{
				text = urlMapping.MappedUrl;
			}
			return text;
		}

		// Token: 0x04001A7D RID: 6781
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A7E RID: 6782
		private static readonly ConfigurationProperty _propEnabled = new ConfigurationProperty("enabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001A7F RID: 6783
		private static readonly ConfigurationProperty _propMappings = new ConfigurationProperty(null, typeof(UrlMappingCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
