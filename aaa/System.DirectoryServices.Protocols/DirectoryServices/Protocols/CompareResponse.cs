using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000042 RID: 66
	public class CompareResponse : DirectoryResponse
	{
		// Token: 0x0600015E RID: 350 RVA: 0x000067E8 File Offset: 0x000057E8
		internal CompareResponse(XmlNode node)
			: base(node)
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000067F1 File Offset: 0x000057F1
		internal CompareResponse(string dn, DirectoryControl[] controls, ResultCode result, string message, Uri[] referral)
			: base(dn, controls, result, message, referral)
		{
		}
	}
}
