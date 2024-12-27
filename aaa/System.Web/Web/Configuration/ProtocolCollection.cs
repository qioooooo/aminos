using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000233 RID: 563
	[ConfigurationCollection(typeof(ProtocolElement))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProtocolCollection : ConfigurationElementCollection
	{
		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001E25 RID: 7717 RVA: 0x000873F7 File Offset: 0x000863F7
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProtocolCollection._properties;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001E26 RID: 7718 RVA: 0x000873FE File Offset: 0x000863FE
		public string[] AllKeys
		{
			get
			{
				return (string[])base.BaseGetAllKeys();
			}
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x0008740B File Offset: 0x0008640B
		public void Add(ProtocolElement protocolElement)
		{
			this.BaseAdd(protocolElement);
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x00087414 File Offset: 0x00086414
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x0008741D File Offset: 0x0008641D
		public void Remove(ProtocolElement protocolElement)
		{
			base.BaseRemove(this.GetElementKey(protocolElement));
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0008742C File Offset: 0x0008642C
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x17000628 RID: 1576
		public ProtocolElement this[string name]
		{
			get
			{
				return (ProtocolElement)base.BaseGet(name);
			}
		}

		// Token: 0x17000629 RID: 1577
		public ProtocolElement this[int index]
		{
			get
			{
				return (ProtocolElement)base.BaseGet(index);
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

		// Token: 0x06001E2E RID: 7726 RVA: 0x0008746B File Offset: 0x0008646B
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x00087473 File Offset: 0x00086473
		protected override ConfigurationElement CreateNewElement()
		{
			return new ProtocolElement();
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0008747C File Offset: 0x0008647C
		protected override object GetElementKey(ConfigurationElement element)
		{
			string name = ((ProtocolElement)element).Name;
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(SR.GetString("Config_collection_add_element_without_key"));
			}
			return name;
		}

		// Token: 0x040019AE RID: 6574
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
