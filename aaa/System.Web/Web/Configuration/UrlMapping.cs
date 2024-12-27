using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x0200025E RID: 606
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class UrlMapping : ConfigurationElement
	{
		// Token: 0x06001FFB RID: 8187 RVA: 0x0008C494 File Offset: 0x0008B494
		static UrlMapping()
		{
			UrlMapping._properties.Add(UrlMapping._propUrl);
			UrlMapping._properties.Add(UrlMapping._propMappedUrl);
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x0008C529 File Offset: 0x0008B529
		internal UrlMapping()
		{
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x0008C531 File Offset: 0x0008B531
		public UrlMapping(string url, string mappedUrl)
		{
			base[UrlMapping._propUrl] = url;
			base[UrlMapping._propMappedUrl] = mappedUrl;
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06001FFE RID: 8190 RVA: 0x0008C551 File Offset: 0x0008B551
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return UrlMapping._properties;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x0008C558 File Offset: 0x0008B558
		[ConfigurationProperty("url", IsRequired = true, IsKey = true)]
		public string Url
		{
			get
			{
				return (string)base[UrlMapping._propUrl];
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x0008C56A File Offset: 0x0008B56A
		[ConfigurationProperty("mappedUrl", IsRequired = true)]
		public string MappedUrl
		{
			get
			{
				return (string)base[UrlMapping._propMappedUrl];
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x0008C57C File Offset: 0x0008B57C
		private static void ValidateUrl(object value)
		{
			StdValidatorsAndConverters.NonEmptyStringValidator.Validate(value);
			string text = (string)value;
			if (!UrlPath.IsAppRelativePath(text))
			{
				throw new ConfigurationErrorsException(SR.GetString("UrlMappings_only_app_relative_url_allowed", new object[] { text }));
			}
		}

		// Token: 0x04001A79 RID: 6777
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A7A RID: 6778
		private static readonly ConfigurationProperty _propUrl = new ConfigurationProperty("url", typeof(string), null, StdValidatorsAndConverters.WhiteSpaceTrimStringConverter, new CallbackValidator(typeof(string), new ValidatorCallback(UrlMapping.ValidateUrl)), ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001A7B RID: 6779
		private static readonly ConfigurationProperty _propMappedUrl = new ConfigurationProperty("mappedUrl", typeof(string), null, StdValidatorsAndConverters.WhiteSpaceTrimStringConverter, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired);
	}
}
