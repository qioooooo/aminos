using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000143 RID: 323
	internal class NamespaceInfo
	{
		// Token: 0x06000E4D RID: 3661 RVA: 0x0004932F File Offset: 0x0004832F
		internal NamespaceInfo(string prefix, string nameSpace, int stylesheetId)
		{
			this.prefix = prefix;
			this.nameSpace = nameSpace;
			this.stylesheetId = stylesheetId;
		}

		// Token: 0x04000941 RID: 2369
		internal string prefix;

		// Token: 0x04000942 RID: 2370
		internal string nameSpace;

		// Token: 0x04000943 RID: 2371
		internal int stylesheetId;
	}
}
