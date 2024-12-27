using System;
using System.Collections;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000093 RID: 147
	internal class NamespaceSortOrder : IComparer
	{
		// Token: 0x060002A5 RID: 677 RVA: 0x0000E9C3 File Offset: 0x0000D9C3
		internal NamespaceSortOrder()
		{
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000E9CC File Offset: 0x0000D9CC
		public int Compare(object a, object b)
		{
			XmlNode xmlNode = a as XmlNode;
			XmlNode xmlNode2 = b as XmlNode;
			if (a == null || b == null)
			{
				throw new ArgumentException();
			}
			bool flag = Utils.IsDefaultNamespaceNode(xmlNode);
			bool flag2 = Utils.IsDefaultNamespaceNode(xmlNode2);
			if (flag && flag2)
			{
				return 0;
			}
			if (flag)
			{
				return -1;
			}
			if (flag2)
			{
				return 1;
			}
			return string.CompareOrdinal(xmlNode.LocalName, xmlNode2.LocalName);
		}
	}
}
