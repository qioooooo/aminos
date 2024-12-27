using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld.Debugger
{
	// Token: 0x0200012D RID: 301
	internal interface IStackFrame
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000D3C RID: 3388
		XPathNavigator Instruction { get; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000D3D RID: 3389
		XPathNodeIterator NodeSet { get; }

		// Token: 0x06000D3E RID: 3390
		int GetVariablesCount();

		// Token: 0x06000D3F RID: 3391
		XPathNavigator GetVariable(int varIndex);

		// Token: 0x06000D40 RID: 3392
		object GetVariableValue(int varIndex);
	}
}
