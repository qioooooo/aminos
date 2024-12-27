using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001FA RID: 506
	[ConfigurationCollection(typeof(HttpHandlerAction), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMapAlternate)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpHandlerActionCollection : ConfigurationElementCollection
	{
		// Token: 0x06001B97 RID: 7063 RVA: 0x0007F938 File Offset: 0x0007E938
		public HttpHandlerActionCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001B98 RID: 7064 RVA: 0x0007F945 File Offset: 0x0007E945
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpHandlerActionCollection._properties;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001B99 RID: 7065 RVA: 0x0007F94C File Offset: 0x0007E94C
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.AddRemoveClearMapAlternate;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001B9A RID: 7066 RVA: 0x0007F94F File Offset: 0x0007E94F
		protected override bool ThrowOnDuplicate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000559 RID: 1369
		public HttpHandlerAction this[int index]
		{
			get
			{
				return (HttpHandlerAction)base.BaseGet(index);
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

		// Token: 0x06001B9D RID: 7069 RVA: 0x0007F97A File Offset: 0x0007E97A
		public int IndexOf(HttpHandlerAction action)
		{
			return base.BaseIndexOf(action);
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x0007F983 File Offset: 0x0007E983
		public void Add(HttpHandlerAction httpHandlerAction)
		{
			base.BaseAdd(httpHandlerAction, false);
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x0007F98D File Offset: 0x0007E98D
		public void Remove(HttpHandlerAction action)
		{
			base.BaseRemove(action.Key);
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0007F99B File Offset: 0x0007E99B
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0007F9A4 File Offset: 0x0007E9A4
		public void Remove(string verb, string path)
		{
			base.BaseRemove("verb=" + verb + " | path=" + path);
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0007F9BD File Offset: 0x0007E9BD
		protected override ConfigurationElement CreateNewElement()
		{
			return new HttpHandlerAction();
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0007F9C4 File Offset: 0x0007E9C4
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((HttpHandlerAction)element).Key;
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x0007F9D1 File Offset: 0x0007E9D1
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x04001879 RID: 6265
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
