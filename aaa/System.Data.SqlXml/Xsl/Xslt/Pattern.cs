using System;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000F7 RID: 247
	internal struct Pattern
	{
		// Token: 0x06000AEA RID: 2794 RVA: 0x00034E6E File Offset: 0x00033E6E
		public Pattern(TemplateMatch match, int priority)
		{
			this.Match = match;
			this.Priority = priority;
		}

		// Token: 0x040007A7 RID: 1959
		public readonly TemplateMatch Match;

		// Token: 0x040007A8 RID: 1960
		public readonly int Priority;
	}
}
