using System;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x0200010D RID: 269
	internal abstract class ProtoTemplate : XslNode
	{
		// Token: 0x06000BE0 RID: 3040 RVA: 0x0003D320 File Offset: 0x0003C320
		public ProtoTemplate(XslNodeType nt, QilName name, XslVersion xslVer)
			: base(nt, name, null, xslVer)
		{
		}

		// Token: 0x06000BE1 RID: 3041
		public abstract string GetDebugName();

		// Token: 0x04000854 RID: 2132
		public QilFunction Function;
	}
}
