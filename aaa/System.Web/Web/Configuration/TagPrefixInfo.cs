using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000255 RID: 597
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TagPrefixInfo : ConfigurationElement
	{
		// Token: 0x06001F93 RID: 8083 RVA: 0x0008B5B8 File Offset: 0x0008A5B8
		static TagPrefixInfo()
		{
			TagPrefixInfo._properties.Add(TagPrefixInfo._propTagPrefix);
			TagPrefixInfo._properties.Add(TagPrefixInfo._propTagName);
			TagPrefixInfo._properties.Add(TagPrefixInfo._propNamespace);
			TagPrefixInfo._properties.Add(TagPrefixInfo._propAssembly);
			TagPrefixInfo._properties.Add(TagPrefixInfo._propSource);
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0008B6E8 File Offset: 0x0008A6E8
		internal TagPrefixInfo()
		{
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0008B6F0 File Offset: 0x0008A6F0
		public TagPrefixInfo(string tagPrefix, string nameSpace, string assembly, string tagName, string source)
			: this()
		{
			this.TagPrefix = tagPrefix;
			this.Namespace = nameSpace;
			this.Assembly = assembly;
			this.TagName = tagName;
			this.Source = source;
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x0008B720 File Offset: 0x0008A720
		public override bool Equals(object prefix)
		{
			TagPrefixInfo tagPrefixInfo = prefix as TagPrefixInfo;
			return StringUtil.Equals(this.TagPrefix, tagPrefixInfo.TagPrefix) && StringUtil.Equals(this.TagName, tagPrefixInfo.TagName) && StringUtil.Equals(this.Namespace, tagPrefixInfo.Namespace) && StringUtil.Equals(this.Assembly, tagPrefixInfo.Assembly) && StringUtil.Equals(this.Source, tagPrefixInfo.Source);
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x0008B793 File Offset: 0x0008A793
		public override int GetHashCode()
		{
			return this.TagPrefix.GetHashCode() ^ this.TagName.GetHashCode() ^ this.Namespace.GetHashCode() ^ this.Assembly.GetHashCode() ^ this.Source.GetHashCode();
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001F98 RID: 8088 RVA: 0x0008B7D0 File Offset: 0x0008A7D0
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TagPrefixInfo._properties;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x0008B7D7 File Offset: 0x0008A7D7
		// (set) Token: 0x06001F9A RID: 8090 RVA: 0x0008B7E9 File Offset: 0x0008A7E9
		[ConfigurationProperty("tagPrefix", IsRequired = true, DefaultValue = "/")]
		[StringValidator(MinLength = 1)]
		public string TagPrefix
		{
			get
			{
				return (string)base[TagPrefixInfo._propTagPrefix];
			}
			set
			{
				base[TagPrefixInfo._propTagPrefix] = value;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x0008B7F7 File Offset: 0x0008A7F7
		// (set) Token: 0x06001F9C RID: 8092 RVA: 0x0008B809 File Offset: 0x0008A809
		[ConfigurationProperty("tagName")]
		public string TagName
		{
			get
			{
				return (string)base[TagPrefixInfo._propTagName];
			}
			set
			{
				base[TagPrefixInfo._propTagName] = value;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x0008B817 File Offset: 0x0008A817
		// (set) Token: 0x06001F9E RID: 8094 RVA: 0x0008B829 File Offset: 0x0008A829
		[ConfigurationProperty("namespace")]
		public string Namespace
		{
			get
			{
				return (string)base[TagPrefixInfo._propNamespace];
			}
			set
			{
				base[TagPrefixInfo._propNamespace] = value;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x0008B837 File Offset: 0x0008A837
		// (set) Token: 0x06001FA0 RID: 8096 RVA: 0x0008B849 File Offset: 0x0008A849
		[ConfigurationProperty("assembly")]
		public string Assembly
		{
			get
			{
				return (string)base[TagPrefixInfo._propAssembly];
			}
			set
			{
				base[TagPrefixInfo._propAssembly] = value;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x0008B857 File Offset: 0x0008A857
		// (set) Token: 0x06001FA2 RID: 8098 RVA: 0x0008B869 File Offset: 0x0008A869
		[ConfigurationProperty("src")]
		public string Source
		{
			get
			{
				return (string)base[TagPrefixInfo._propSource];
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					base[TagPrefixInfo._propSource] = value;
					return;
				}
				base[TagPrefixInfo._propSource] = null;
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x0008B88C File Offset: 0x0008A88C
		protected override ConfigurationElementProperty ElementProperty
		{
			get
			{
				return TagPrefixInfo.s_elemProperty;
			}
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0008B894 File Offset: 0x0008A894
		private static void Validate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("control");
			}
			TagPrefixInfo tagPrefixInfo = (TagPrefixInfo)value;
			if (Util.ContainsWhiteSpace(tagPrefixInfo.TagPrefix))
			{
				throw new ConfigurationErrorsException(SR.GetString("Space_attribute", new object[] { "tagPrefix" }));
			}
			bool flag = false;
			if (!string.IsNullOrEmpty(tagPrefixInfo.Namespace))
			{
				if (!string.IsNullOrEmpty(tagPrefixInfo.TagName) || !string.IsNullOrEmpty(tagPrefixInfo.Source))
				{
					flag = true;
				}
			}
			else if (!string.IsNullOrEmpty(tagPrefixInfo.TagName))
			{
				if (!string.IsNullOrEmpty(tagPrefixInfo.Namespace) || !string.IsNullOrEmpty(tagPrefixInfo.Assembly) || string.IsNullOrEmpty(tagPrefixInfo.Source))
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_tagprefix_entry"));
			}
		}

		// Token: 0x04001A58 RID: 6744
		private static readonly ConfigurationElementProperty s_elemProperty = new ConfigurationElementProperty(new CallbackValidator(typeof(TagPrefixInfo), new ValidatorCallback(TagPrefixInfo.Validate)));

		// Token: 0x04001A59 RID: 6745
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A5A RID: 6746
		private static readonly ConfigurationProperty _propTagPrefix = new ConfigurationProperty("tagPrefix", typeof(string), "/", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04001A5B RID: 6747
		private static readonly ConfigurationProperty _propTagName = new ConfigurationProperty("tagName", typeof(string), string.Empty, null, null, ConfigurationPropertyOptions.None);

		// Token: 0x04001A5C RID: 6748
		private static readonly ConfigurationProperty _propNamespace = new ConfigurationProperty("namespace", typeof(string), string.Empty, null, null, ConfigurationPropertyOptions.None);

		// Token: 0x04001A5D RID: 6749
		private static readonly ConfigurationProperty _propAssembly = new ConfigurationProperty("assembly", typeof(string), string.Empty, null, null, ConfigurationPropertyOptions.None);

		// Token: 0x04001A5E RID: 6750
		private static readonly ConfigurationProperty _propSource = new ConfigurationProperty("src", typeof(string), string.Empty, null, null, ConfigurationPropertyOptions.None);
	}
}
