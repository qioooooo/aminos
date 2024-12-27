using System;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000111 RID: 273
	internal class VarPar : XslNode
	{
		// Token: 0x06000BE8 RID: 3048 RVA: 0x0003D4BE File Offset: 0x0003C4BE
		public VarPar(XslNodeType nt, QilName name, string select, XslVersion xslVer)
			: base(nt, name, select, xslVer)
		{
		}

		// Token: 0x04000860 RID: 2144
		public XslFlags DefValueFlags;

		// Token: 0x04000861 RID: 2145
		public QilNode Value;
	}
}
