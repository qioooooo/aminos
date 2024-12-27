using System;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	// Token: 0x02000176 RID: 374
	public interface IXsltContextVariable
	{
		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060013F1 RID: 5105
		bool IsLocal { get; }

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060013F2 RID: 5106
		bool IsParam { get; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060013F3 RID: 5107
		XPathResultType VariableType { get; }

		// Token: 0x060013F4 RID: 5108
		object Evaluate(XsltContext xsltContext);
	}
}
