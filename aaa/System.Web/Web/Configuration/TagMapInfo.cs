using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x02000253 RID: 595
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TagMapInfo : ConfigurationElement
	{
		// Token: 0x06001F7A RID: 8058 RVA: 0x0008B30C File Offset: 0x0008A30C
		static TagMapInfo()
		{
			TagMapInfo._properties.Add(TagMapInfo._propTagTypeName);
			TagMapInfo._properties.Add(TagMapInfo._propMappedTagTypeName);
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0008B383 File Offset: 0x0008A383
		internal TagMapInfo()
		{
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0008B38B File Offset: 0x0008A38B
		public TagMapInfo(string tagTypeName, string mappedTagTypeName)
			: this()
		{
			this.TagType = tagTypeName;
			this.MappedTagType = mappedTagTypeName;
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x0008B3A4 File Offset: 0x0008A3A4
		public override bool Equals(object o)
		{
			TagMapInfo tagMapInfo = o as TagMapInfo;
			return StringUtil.Equals(this.TagType, tagMapInfo.TagType) && StringUtil.Equals(this.MappedTagType, tagMapInfo.MappedTagType);
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0008B3DE File Offset: 0x0008A3DE
		public override int GetHashCode()
		{
			return this.TagType.GetHashCode() ^ this.MappedTagType.GetHashCode();
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06001F7F RID: 8063 RVA: 0x0008B3F7 File Offset: 0x0008A3F7
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TagMapInfo._properties;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06001F80 RID: 8064 RVA: 0x0008B3FE File Offset: 0x0008A3FE
		// (set) Token: 0x06001F81 RID: 8065 RVA: 0x0008B410 File Offset: 0x0008A410
		[ConfigurationProperty("mappedTagType")]
		[StringValidator(MinLength = 1)]
		public string MappedTagType
		{
			get
			{
				return (string)base[TagMapInfo._propMappedTagTypeName];
			}
			set
			{
				base[TagMapInfo._propMappedTagTypeName] = value;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001F82 RID: 8066 RVA: 0x0008B41E File Offset: 0x0008A41E
		// (set) Token: 0x06001F83 RID: 8067 RVA: 0x0008B430 File Offset: 0x0008A430
		[ConfigurationProperty("tagType", IsRequired = true, IsKey = true, DefaultValue = "")]
		[StringValidator(MinLength = 1)]
		public string TagType
		{
			get
			{
				return (string)base[TagMapInfo._propTagTypeName];
			}
			set
			{
				base[TagMapInfo._propTagTypeName] = value;
			}
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x0008B440 File Offset: 0x0008A440
		private void Verify()
		{
			if (string.IsNullOrEmpty(this.TagType))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_missing", new object[] { "tagType" }));
			}
			if (string.IsNullOrEmpty(this.MappedTagType))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_missing", new object[] { "mappedTagType" }));
			}
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0008B4A7 File Offset: 0x0008A4A7
		protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
		{
			this.Verify();
			return base.SerializeElement(writer, serializeCollectionKey);
		}

		// Token: 0x04001A54 RID: 6740
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A55 RID: 6741
		private static readonly ConfigurationProperty _propTagTypeName = new ConfigurationProperty("tagType", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001A56 RID: 6742
		private static readonly ConfigurationProperty _propMappedTagTypeName = new ConfigurationProperty("mappedTagType", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired);
	}
}
