using System;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x020000C7 RID: 199
	internal class DomNameTable
	{
		// Token: 0x06000B85 RID: 2949 RVA: 0x000350EA File Offset: 0x000340EA
		public DomNameTable(XmlDocument document)
		{
			this.ownerDocument = document;
			this.nameTable = document.NameTable;
			this.entries = new XmlName[64];
			this.mask = 63;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0003511C File Offset: 0x0003411C
		public XmlName GetName(string prefix, string localName, string ns, IXmlSchemaInfo schemaInfo)
		{
			if (prefix == null)
			{
				prefix = string.Empty;
			}
			if (ns == null)
			{
				ns = string.Empty;
			}
			int hashCode = XmlName.GetHashCode(localName);
			for (XmlName xmlName = this.entries[hashCode & this.mask]; xmlName != null; xmlName = xmlName.next)
			{
				if (xmlName.HashCode == hashCode && (xmlName.LocalName == localName || xmlName.LocalName.Equals(localName)) && (xmlName.Prefix == prefix || xmlName.Prefix.Equals(prefix)) && (xmlName.NamespaceURI == ns || xmlName.NamespaceURI.Equals(ns)) && xmlName.Equals(schemaInfo))
				{
					return xmlName;
				}
			}
			return null;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x000351BC File Offset: 0x000341BC
		public XmlName AddName(string prefix, string localName, string ns, IXmlSchemaInfo schemaInfo)
		{
			if (prefix == null)
			{
				prefix = string.Empty;
			}
			if (ns == null)
			{
				ns = string.Empty;
			}
			int hashCode = XmlName.GetHashCode(localName);
			for (XmlName xmlName = this.entries[hashCode & this.mask]; xmlName != null; xmlName = xmlName.next)
			{
				if (xmlName.HashCode == hashCode && (xmlName.LocalName == localName || xmlName.LocalName.Equals(localName)) && (xmlName.Prefix == prefix || xmlName.Prefix.Equals(prefix)) && (xmlName.NamespaceURI == ns || xmlName.NamespaceURI.Equals(ns)) && xmlName.Equals(schemaInfo))
				{
					return xmlName;
				}
			}
			prefix = this.nameTable.Add(prefix);
			localName = this.nameTable.Add(localName);
			ns = this.nameTable.Add(ns);
			int num = hashCode & this.mask;
			XmlName xmlName2 = XmlName.Create(prefix, localName, ns, hashCode, this.ownerDocument, this.entries[num], schemaInfo);
			this.entries[num] = xmlName2;
			if (this.count++ == this.mask)
			{
				this.Grow();
			}
			return xmlName2;
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x000352D4 File Offset: 0x000342D4
		private void Grow()
		{
			int num = this.mask * 2 + 1;
			XmlName[] array = this.entries;
			XmlName[] array2 = new XmlName[num + 1];
			foreach (XmlName xmlName in array)
			{
				while (xmlName != null)
				{
					int num2 = xmlName.HashCode & num;
					XmlName next = xmlName.next;
					xmlName.next = array2[num2];
					array2[num2] = xmlName;
					xmlName = next;
				}
			}
			this.entries = array2;
			this.mask = num;
		}

		// Token: 0x040008E5 RID: 2277
		private const int InitialSize = 64;

		// Token: 0x040008E6 RID: 2278
		private XmlName[] entries;

		// Token: 0x040008E7 RID: 2279
		private int count;

		// Token: 0x040008E8 RID: 2280
		private int mask;

		// Token: 0x040008E9 RID: 2281
		private XmlDocument ownerDocument;

		// Token: 0x040008EA RID: 2282
		private XmlNameTable nameTable;
	}
}
