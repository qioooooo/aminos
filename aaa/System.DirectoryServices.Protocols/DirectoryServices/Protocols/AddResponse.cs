using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200003F RID: 63
	public class AddResponse : DirectoryResponse
	{
		// Token: 0x06000158 RID: 344 RVA: 0x000067A0 File Offset: 0x000057A0
		internal AddResponse(XmlNode node)
			: base(node)
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000067A9 File Offset: 0x000057A9
		internal AddResponse(string dn, DirectoryControl[] controls, ResultCode result, string message, Uri[] referral)
			: base(dn, controls, result, message, referral)
		{
		}
	}
}
