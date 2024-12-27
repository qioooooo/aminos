using System;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x0200010B RID: 267
	internal class NsDecl
	{
		// Token: 0x06000BD5 RID: 3029 RVA: 0x0003D23D File Offset: 0x0003C23D
		public NsDecl(NsDecl prev, string prefix, string nsUri)
		{
			this.Prev = prev;
			this.Prefix = prefix;
			this.NsUri = nsUri;
		}

		// Token: 0x04000848 RID: 2120
		public readonly NsDecl Prev;

		// Token: 0x04000849 RID: 2121
		public readonly string Prefix;

		// Token: 0x0400084A RID: 2122
		public readonly string NsUri;
	}
}
