using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200003E RID: 62
	public class DeleteResponse : DirectoryResponse
	{
		// Token: 0x06000156 RID: 342 RVA: 0x00006788 File Offset: 0x00005788
		internal DeleteResponse(XmlNode node)
			: base(node)
		{
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006791 File Offset: 0x00005791
		internal DeleteResponse(string dn, DirectoryControl[] controls, ResultCode result, string message, Uri[] referral)
			: base(dn, controls, result, message, referral)
		{
		}
	}
}
