using System;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.XPath
{
	// Token: 0x020000D2 RID: 210
	internal interface IFocus
	{
		// Token: 0x060009DF RID: 2527
		QilNode GetCurrent();

		// Token: 0x060009E0 RID: 2528
		QilNode GetPosition();

		// Token: 0x060009E1 RID: 2529
		QilNode GetLast();
	}
}
