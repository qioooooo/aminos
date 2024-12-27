using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200013F RID: 319
	internal class Sort
	{
		// Token: 0x06000DDF RID: 3551 RVA: 0x00047BEC File Offset: 0x00046BEC
		public Sort(int sortkey, string xmllang, XmlDataType datatype, XmlSortOrder xmlorder, XmlCaseOrder xmlcaseorder)
		{
			this.select = sortkey;
			this.lang = xmllang;
			this.dataType = datatype;
			this.order = xmlorder;
			this.caseOrder = xmlcaseorder;
		}

		// Token: 0x0400091B RID: 2331
		internal int select;

		// Token: 0x0400091C RID: 2332
		internal string lang;

		// Token: 0x0400091D RID: 2333
		internal XmlDataType dataType;

		// Token: 0x0400091E RID: 2334
		internal XmlSortOrder order;

		// Token: 0x0400091F RID: 2335
		internal XmlCaseOrder caseOrder;
	}
}
