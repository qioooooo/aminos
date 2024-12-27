using System;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000112 RID: 274
	internal class Sort : XslNode
	{
		// Token: 0x06000BE9 RID: 3049 RVA: 0x0003D4CB File Offset: 0x0003C4CB
		public Sort(string select, string lang, string dataType, string order, string caseOrder, XslVersion xslVer)
			: base(XslNodeType.Sort, null, select, xslVer)
		{
			this.Lang = lang;
			this.DataType = dataType;
			this.Order = order;
			this.CaseOrder = caseOrder;
		}

		// Token: 0x04000862 RID: 2146
		public readonly string Lang;

		// Token: 0x04000863 RID: 2147
		public readonly string DataType;

		// Token: 0x04000864 RID: 2148
		public readonly string Order;

		// Token: 0x04000865 RID: 2149
		public readonly string CaseOrder;
	}
}
