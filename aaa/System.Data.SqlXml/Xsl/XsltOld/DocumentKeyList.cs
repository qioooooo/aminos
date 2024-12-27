using System;
using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000191 RID: 401
	internal struct DocumentKeyList
	{
		// Token: 0x06001120 RID: 4384 RVA: 0x000525E0 File Offset: 0x000515E0
		public DocumentKeyList(XPathNavigator rootNav, Hashtable keyTable)
		{
			this.rootNav = rootNav;
			this.keyTable = keyTable;
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x000525F0 File Offset: 0x000515F0
		public XPathNavigator RootNav
		{
			get
			{
				return this.rootNav;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x000525F8 File Offset: 0x000515F8
		public Hashtable KeyTable
		{
			get
			{
				return this.keyTable;
			}
		}

		// Token: 0x04000B57 RID: 2903
		private XPathNavigator rootNav;

		// Token: 0x04000B58 RID: 2904
		private Hashtable keyTable;
	}
}
