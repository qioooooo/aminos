using System;
using System.Security.Permissions;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000B0 RID: 176
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class XmlDsigExcC14NWithCommentsTransform : XmlDsigExcC14NTransform
	{
		// Token: 0x060003F4 RID: 1012 RVA: 0x000145B7 File Offset: 0x000135B7
		public XmlDsigExcC14NWithCommentsTransform()
			: base(true)
		{
			base.Algorithm = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000145CB File Offset: 0x000135CB
		public XmlDsigExcC14NWithCommentsTransform(string inclusiveNamespacesPrefixList)
			: base(true, inclusiveNamespacesPrefixList)
		{
			base.Algorithm = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";
		}
	}
}
