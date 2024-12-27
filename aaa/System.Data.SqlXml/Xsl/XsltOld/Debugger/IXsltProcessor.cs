using System;

namespace System.Xml.Xsl.XsltOld.Debugger
{
	// Token: 0x02000188 RID: 392
	internal interface IXsltProcessor
	{
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001068 RID: 4200
		int StackDepth { get; }

		// Token: 0x06001069 RID: 4201
		IStackFrame GetStackFrame(int depth);
	}
}
