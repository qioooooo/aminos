using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld.Debugger
{
	// Token: 0x020001AE RID: 430
	internal interface IXsltDebugger
	{
		// Token: 0x060011CB RID: 4555
		string GetBuiltInTemplatesUri();

		// Token: 0x060011CC RID: 4556
		void OnInstructionCompile(XPathNavigator styleSheetNavigator);

		// Token: 0x060011CD RID: 4557
		void OnInstructionExecute(IXsltProcessor xsltProcessor);
	}
}
