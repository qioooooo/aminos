using System;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000119 RID: 281
	internal class XslNodeEx : XslNode
	{
		// Token: 0x06000BF1 RID: 3057 RVA: 0x0003D64D File Offset: 0x0003C64D
		public XslNodeEx(XslNodeType t, QilName name, object arg, XsltInput.ContextInfo ctxInfo, XslVersion xslVer)
			: base(t, name, arg, xslVer)
		{
			this.ElemNameLi = ctxInfo.elemNameLi;
			this.EndTagLi = ctxInfo.endTagLi;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0003D674 File Offset: 0x0003C674
		public XslNodeEx(XslNodeType t, QilName name, object arg, XslVersion xslVer)
			: base(t, name, arg, xslVer)
		{
		}

		// Token: 0x04000879 RID: 2169
		public readonly ISourceLineInfo ElemNameLi;

		// Token: 0x0400087A RID: 2170
		public readonly ISourceLineInfo EndTagLi;
	}
}
