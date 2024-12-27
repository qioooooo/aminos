using System;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000E6 RID: 230
	internal class NsAlias
	{
		// Token: 0x06000A9A RID: 2714 RVA: 0x00033363 File Offset: 0x00032363
		public NsAlias(string resultNsUri, string resultPrefix, int importPrecedence)
		{
			this.ResultNsUri = resultNsUri;
			this.ResultPrefix = resultPrefix;
			this.ImportPrecedence = importPrecedence;
		}

		// Token: 0x04000714 RID: 1812
		public readonly string ResultNsUri;

		// Token: 0x04000715 RID: 1813
		public readonly string ResultPrefix;

		// Token: 0x04000716 RID: 1814
		public readonly int ImportPrecedence;
	}
}
