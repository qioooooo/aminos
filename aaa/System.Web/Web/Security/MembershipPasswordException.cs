using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web.Security
{
	// Token: 0x02000346 RID: 838
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public class MembershipPasswordException : Exception
	{
		// Token: 0x060028B7 RID: 10423 RVA: 0x000B2B41 File Offset: 0x000B1B41
		public MembershipPasswordException(string message)
			: base(message)
		{
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000B2B4A File Offset: 0x000B1B4A
		protected MembershipPasswordException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x000B2B54 File Offset: 0x000B1B54
		public MembershipPasswordException()
		{
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x000B2B5C File Offset: 0x000B1B5C
		public MembershipPasswordException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
