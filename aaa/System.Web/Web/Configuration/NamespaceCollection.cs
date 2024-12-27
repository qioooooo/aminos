using System;
using System.Collections;
using System.Configuration;
using System.Security.Permissions;
using System.Web.UI;

namespace System.Web.Configuration
{
	// Token: 0x02000215 RID: 533
	[ConfigurationCollection(typeof(NamespaceInfo))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class NamespaceCollection : ConfigurationElementCollection
	{
		// Token: 0x06001C9C RID: 7324 RVA: 0x000831AA File Offset: 0x000821AA
		static NamespaceCollection()
		{
			NamespaceCollection._properties.Add(NamespaceCollection._propAutoImportVBNamespace);
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x000831E5 File Offset: 0x000821E5
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return NamespaceCollection._properties;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001C9E RID: 7326 RVA: 0x000831EC File Offset: 0x000821EC
		// (set) Token: 0x06001C9F RID: 7327 RVA: 0x000831FE File Offset: 0x000821FE
		[ConfigurationProperty("autoImportVBNamespace", DefaultValue = true)]
		public bool AutoImportVBNamespace
		{
			get
			{
				return (bool)base[NamespaceCollection._propAutoImportVBNamespace];
			}
			set
			{
				base[NamespaceCollection._propAutoImportVBNamespace] = value;
			}
		}

		// Token: 0x17000597 RID: 1431
		public NamespaceInfo this[int index]
		{
			get
			{
				return (NamespaceInfo)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
				this._namespaceEntries = null;
			}
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x00083240 File Offset: 0x00082240
		public void Add(NamespaceInfo namespaceInformation)
		{
			this.BaseAdd(namespaceInformation);
			this._namespaceEntries = null;
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x00083250 File Offset: 0x00082250
		public void Remove(string s)
		{
			base.BaseRemove(s);
			this._namespaceEntries = null;
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x00083260 File Offset: 0x00082260
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
			this._namespaceEntries = null;
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x00083270 File Offset: 0x00082270
		protected override ConfigurationElement CreateNewElement()
		{
			return new NamespaceInfo();
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x00083277 File Offset: 0x00082277
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((NamespaceInfo)element).Namespace;
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x00083284 File Offset: 0x00082284
		public void Clear()
		{
			base.BaseClear();
			this._namespaceEntries = null;
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001CA8 RID: 7336 RVA: 0x00083294 File Offset: 0x00082294
		internal Hashtable NamespaceEntries
		{
			get
			{
				if (this._namespaceEntries == null)
				{
					lock (this)
					{
						if (this._namespaceEntries == null)
						{
							this._namespaceEntries = new Hashtable(StringComparer.OrdinalIgnoreCase);
							foreach (object obj in this)
							{
								NamespaceInfo namespaceInfo = (NamespaceInfo)obj;
								NamespaceEntry namespaceEntry = new NamespaceEntry();
								namespaceEntry.Namespace = namespaceInfo.Namespace;
								namespaceEntry.Line = namespaceInfo.ElementInformation.Properties["namespace"].LineNumber;
								namespaceEntry.VirtualPath = namespaceInfo.ElementInformation.Properties["namespace"].Source;
								if (namespaceEntry.Line == 0)
								{
									namespaceEntry.Line = 1;
								}
								this._namespaceEntries[namespaceInfo.Namespace] = namespaceEntry;
							}
						}
					}
				}
				return this._namespaceEntries;
			}
		}

		// Token: 0x040018FA RID: 6394
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040018FB RID: 6395
		private static readonly ConfigurationProperty _propAutoImportVBNamespace = new ConfigurationProperty("autoImportVBNamespace", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x040018FC RID: 6396
		private Hashtable _namespaceEntries;
	}
}
