using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x0200033A RID: 826
	public class XmlSerializerNamespaces
	{
		// Token: 0x0600287B RID: 10363 RVA: 0x000D19C8 File Offset: 0x000D09C8
		public XmlSerializerNamespaces()
		{
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000D19D0 File Offset: 0x000D09D0
		public XmlSerializerNamespaces(XmlSerializerNamespaces namespaces)
		{
			this.namespaces = (Hashtable)namespaces.Namespaces.Clone();
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000D19F0 File Offset: 0x000D09F0
		public XmlSerializerNamespaces(XmlQualifiedName[] namespaces)
		{
			foreach (XmlQualifiedName xmlQualifiedName in namespaces)
			{
				this.Add(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
			}
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000D1A27 File Offset: 0x000D0A27
		public void Add(string prefix, string ns)
		{
			if (prefix != null && prefix.Length > 0)
			{
				XmlConvert.VerifyNCName(prefix);
			}
			if (ns != null && ns.Length > 0)
			{
				XmlConvert.ToUri(ns);
			}
			this.AddInternal(prefix, ns);
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000D1A57 File Offset: 0x000D0A57
		internal void AddInternal(string prefix, string ns)
		{
			this.Namespaces[prefix] = ns;
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000D1A66 File Offset: 0x000D0A66
		public XmlQualifiedName[] ToArray()
		{
			if (this.NamespaceList == null)
			{
				return new XmlQualifiedName[0];
			}
			return (XmlQualifiedName[])this.NamespaceList.ToArray(typeof(XmlQualifiedName));
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06002881 RID: 10369 RVA: 0x000D1A91 File Offset: 0x000D0A91
		public int Count
		{
			get
			{
				return this.Namespaces.Count;
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06002882 RID: 10370 RVA: 0x000D1AA0 File Offset: 0x000D0AA0
		internal ArrayList NamespaceList
		{
			get
			{
				if (this.namespaces == null || this.namespaces.Count == 0)
				{
					return null;
				}
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this.Namespaces.Keys)
				{
					string text = (string)obj;
					arrayList.Add(new XmlQualifiedName(text, (string)this.Namespaces[text]));
				}
				return arrayList;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06002883 RID: 10371 RVA: 0x000D1B34 File Offset: 0x000D0B34
		// (set) Token: 0x06002884 RID: 10372 RVA: 0x000D1B4F File Offset: 0x000D0B4F
		internal Hashtable Namespaces
		{
			get
			{
				if (this.namespaces == null)
				{
					this.namespaces = new Hashtable();
				}
				return this.namespaces;
			}
			set
			{
				this.namespaces = value;
			}
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000D1B58 File Offset: 0x000D0B58
		internal string LookupPrefix(string ns)
		{
			if (string.IsNullOrEmpty(ns))
			{
				return null;
			}
			if (this.namespaces == null || this.namespaces.Count == 0)
			{
				return null;
			}
			foreach (object obj in this.namespaces.Keys)
			{
				string text = (string)obj;
				if (!string.IsNullOrEmpty(text) && (string)this.namespaces[text] == ns)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x04001681 RID: 5761
		private Hashtable namespaces;
	}
}
