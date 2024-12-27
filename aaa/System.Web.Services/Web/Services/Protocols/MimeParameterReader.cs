using System;
using System.Security.Permissions;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000035 RID: 53
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class MimeParameterReader : MimeFormatter
	{
		// Token: 0x06000133 RID: 307
		public abstract object[] Read(HttpRequest request);
	}
}
