using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x0200025F RID: 607
	[ConfigurationCollection(typeof(UrlMapping))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class UrlMappingCollection : ConfigurationElementCollection
	{
		// Token: 0x06002003 RID: 8195 RVA: 0x0008C5CB File Offset: 0x0008B5CB
		public UrlMappingCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x0008C5D8 File Offset: 0x0008B5D8
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return UrlMappingCollection._properties;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x0008C5DF File Offset: 0x0008B5DF
		public string[] AllKeys
		{
			get
			{
				return StringUtil.ObjectArrayToStringArray(base.BaseGetAllKeys());
			}
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x0008C5EC File Offset: 0x0008B5EC
		public string GetKey(int index)
		{
			return (string)base.BaseGetKey(index);
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x0008C5FA File Offset: 0x0008B5FA
		public void Add(UrlMapping urlMapping)
		{
			this.BaseAdd(urlMapping);
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x0008C603 File Offset: 0x0008B603
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x0008C60C File Offset: 0x0008B60C
		public void Remove(UrlMapping urlMapping)
		{
			base.BaseRemove(this.GetElementKey(urlMapping));
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x0008C61B File Offset: 0x0008B61B
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x170006E8 RID: 1768
		public UrlMapping this[string name]
		{
			get
			{
				return (UrlMapping)base.BaseGet(name);
			}
		}

		// Token: 0x170006E9 RID: 1769
		public UrlMapping this[int index]
		{
			get
			{
				return (UrlMapping)base.BaseGet(index);
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

		// Token: 0x0600200E RID: 8206 RVA: 0x0008C65A File Offset: 0x0008B65A
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x0008C662 File Offset: 0x0008B662
		protected override ConfigurationElement CreateNewElement()
		{
			return new UrlMapping();
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x0008C669 File Offset: 0x0008B669
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((UrlMapping)element).Url;
		}

		// Token: 0x04001A7C RID: 6780
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
