using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.XPath
{
	// Token: 0x020000D3 RID: 211
	internal interface IXPathEnvironment : IFocus
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060009E2 RID: 2530
		XPathQilFactory Factory { get; }

		// Token: 0x060009E3 RID: 2531
		QilNode ResolveVariable(string prefix, string name);

		// Token: 0x060009E4 RID: 2532
		QilNode ResolveFunction(string prefix, string name, IList<QilNode> args, IFocus env);

		// Token: 0x060009E5 RID: 2533
		string ResolvePrefix(string prefix);
	}
}
