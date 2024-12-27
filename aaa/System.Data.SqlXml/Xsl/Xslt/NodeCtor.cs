using System;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000117 RID: 279
	internal class NodeCtor : XslNode
	{
		// Token: 0x06000BEF RID: 3055 RVA: 0x0003D61E File Offset: 0x0003C61E
		public NodeCtor(XslNodeType nt, string nameAvt, string nsAvt, XslVersion xslVer)
			: base(nt, null, null, xslVer)
		{
			this.NameAvt = nameAvt;
			this.NsAvt = nsAvt;
		}

		// Token: 0x04000876 RID: 2166
		public readonly string NameAvt;

		// Token: 0x04000877 RID: 2167
		public readonly string NsAvt;
	}
}
