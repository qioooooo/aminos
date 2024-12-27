using System;

namespace System.Xml.Schema
{
	// Token: 0x02000203 RID: 515
	internal class NamespaceListV1Compat : NamespaceList
	{
		// Token: 0x06001869 RID: 6249 RVA: 0x0006D59D File Offset: 0x0006C59D
		public NamespaceListV1Compat(string namespaces, string targetNamespace)
			: base(namespaces, targetNamespace)
		{
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x0006D5A7 File Offset: 0x0006C5A7
		public override bool Allows(string ns)
		{
			if (base.Type == NamespaceList.ListType.Other)
			{
				return ns != base.Excluded;
			}
			return base.Allows(ns);
		}
	}
}
