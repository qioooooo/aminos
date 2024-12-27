using System;

namespace System.Xml.Xsl
{
	// Token: 0x02000005 RID: 5
	internal interface ISourceLineInfo
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13
		string Uri { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14
		int StartLine { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15
		int StartPos { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16
		int EndLine { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17
		int EndPos { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000012 RID: 18
		bool IsNoSource { get; }
	}
}
