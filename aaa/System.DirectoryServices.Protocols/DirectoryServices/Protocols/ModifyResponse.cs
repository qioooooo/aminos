using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000040 RID: 64
	public class ModifyResponse : DirectoryResponse
	{
		// Token: 0x0600015A RID: 346 RVA: 0x000067B8 File Offset: 0x000057B8
		internal ModifyResponse(XmlNode node)
			: base(node)
		{
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000067C1 File Offset: 0x000057C1
		internal ModifyResponse(string dn, DirectoryControl[] controls, ResultCode result, string message, Uri[] referral)
			: base(dn, controls, result, message, referral)
		{
		}
	}
}
