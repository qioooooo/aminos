using System;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	// Token: 0x02000175 RID: 373
	public interface IXsltContextFunction
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060013EC RID: 5100
		int Minargs { get; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060013ED RID: 5101
		int Maxargs { get; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060013EE RID: 5102
		XPathResultType ReturnType { get; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060013EF RID: 5103
		XPathResultType[] ArgTypes { get; }

		// Token: 0x060013F0 RID: 5104
		object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext);
	}
}
