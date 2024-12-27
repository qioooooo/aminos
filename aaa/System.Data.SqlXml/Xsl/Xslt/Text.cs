using System;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000118 RID: 280
	internal class Text : XslNode
	{
		// Token: 0x06000BF0 RID: 3056 RVA: 0x0003D639 File Offset: 0x0003C639
		public Text(string data, SerializationHints hints, XslVersion xslVer)
			: base(XslNodeType.Text, null, data, xslVer)
		{
			this.Hints = hints;
		}

		// Token: 0x04000878 RID: 2168
		public readonly SerializationHints Hints;
	}
}
