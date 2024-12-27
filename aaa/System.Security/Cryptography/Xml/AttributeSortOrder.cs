using System;
using System.Collections;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000092 RID: 146
	internal class AttributeSortOrder : IComparer
	{
		// Token: 0x060002A3 RID: 675 RVA: 0x0000E969 File Offset: 0x0000D969
		internal AttributeSortOrder()
		{
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000E974 File Offset: 0x0000D974
		public int Compare(object a, object b)
		{
			XmlNode xmlNode = a as XmlNode;
			XmlNode xmlNode2 = b as XmlNode;
			if (a == null || b == null)
			{
				throw new ArgumentException();
			}
			int num = string.CompareOrdinal(xmlNode.NamespaceURI, xmlNode2.NamespaceURI);
			if (num != 0)
			{
				return num;
			}
			return string.CompareOrdinal(xmlNode.LocalName, xmlNode2.LocalName);
		}
	}
}
