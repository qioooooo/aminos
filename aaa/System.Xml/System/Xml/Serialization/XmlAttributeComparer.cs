using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x020002E1 RID: 737
	internal class XmlAttributeComparer : IComparer
	{
		// Token: 0x06002286 RID: 8838 RVA: 0x000A149C File Offset: 0x000A049C
		public int Compare(object o1, object o2)
		{
			XmlAttribute xmlAttribute = (XmlAttribute)o1;
			XmlAttribute xmlAttribute2 = (XmlAttribute)o2;
			int num = string.Compare(xmlAttribute.NamespaceURI, xmlAttribute2.NamespaceURI, StringComparison.Ordinal);
			if (num == 0)
			{
				return string.Compare(xmlAttribute.Name, xmlAttribute2.Name, StringComparison.Ordinal);
			}
			return num;
		}
	}
}
