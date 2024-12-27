using System;
using System.Security.Permissions;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000081 RID: 129
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class SoapServerProtocolFactory : ServerProtocolFactory
	{
		// Token: 0x06000363 RID: 867 RVA: 0x000100D6 File Offset: 0x0000F0D6
		protected override ServerProtocol CreateIfRequestCompatible(HttpRequest request)
		{
			if (request.PathInfo.Length > 0)
			{
				return null;
			}
			if (request.HttpMethod != "POST")
			{
				return new UnsupportedRequestProtocol(405);
			}
			return new SoapServerProtocol();
		}
	}
}
