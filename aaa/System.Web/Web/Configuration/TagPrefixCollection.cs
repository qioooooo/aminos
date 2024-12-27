using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000254 RID: 596
	[ConfigurationCollection(typeof(TagPrefixInfo), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TagPrefixCollection : ConfigurationElementCollection
	{
		// Token: 0x06001F87 RID: 8071 RVA: 0x0008B4C3 File Offset: 0x0008A4C3
		public TagPrefixCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06001F88 RID: 8072 RVA: 0x0008B4D0 File Offset: 0x0008A4D0
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TagPrefixCollection._properties;
			}
		}

		// Token: 0x170006B5 RID: 1717
		public TagPrefixInfo this[int index]
		{
			get
			{
				return (TagPrefixInfo)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06001F8B RID: 8075 RVA: 0x0008B4FF File Offset: 0x0008A4FF
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x0008B502 File Offset: 0x0008A502
		protected override bool ThrowOnDuplicate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0008B505 File Offset: 0x0008A505
		public void Add(TagPrefixInfo tagPrefixInformation)
		{
			this.BaseAdd(tagPrefixInformation);
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0008B50E File Offset: 0x0008A50E
		public void Remove(TagPrefixInfo tagPrefixInformation)
		{
			base.BaseRemove(this.GetElementKey(tagPrefixInformation));
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x0008B51D File Offset: 0x0008A51D
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x0008B525 File Offset: 0x0008A525
		protected override ConfigurationElement CreateNewElement()
		{
			return new TagPrefixInfo();
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x0008B52C File Offset: 0x0008A52C
		protected override string ElementName
		{
			get
			{
				return "add";
			}
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x0008B534 File Offset: 0x0008A534
		protected override object GetElementKey(ConfigurationElement element)
		{
			TagPrefixInfo tagPrefixInfo = (TagPrefixInfo)element;
			if (string.IsNullOrEmpty(tagPrefixInfo.TagName))
			{
				return string.Concat(new string[]
				{
					tagPrefixInfo.TagPrefix,
					":",
					tagPrefixInfo.Namespace,
					":",
					string.IsNullOrEmpty(tagPrefixInfo.Assembly) ? string.Empty : tagPrefixInfo.Assembly
				});
			}
			return tagPrefixInfo.TagPrefix + ":" + tagPrefixInfo.TagName;
		}

		// Token: 0x04001A57 RID: 6743
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
