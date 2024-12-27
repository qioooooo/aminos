using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000041 RID: 65
	public class ModifyDNResponse : DirectoryResponse
	{
		// Token: 0x0600015C RID: 348 RVA: 0x000067D0 File Offset: 0x000057D0
		internal ModifyDNResponse(XmlNode node)
			: base(node)
		{
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000067D9 File Offset: 0x000057D9
		internal ModifyDNResponse(string dn, DirectoryControl[] controls, ResultCode result, string message, Uri[] referral)
			: base(dn, controls, result, message, referral)
		{
		}
	}
}
