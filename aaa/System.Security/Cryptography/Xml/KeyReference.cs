using System;
using System.Security.Permissions;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000C1 RID: 193
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class KeyReference : EncryptedReference
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x00017266 File Offset: 0x00016266
		public KeyReference()
		{
			base.ReferenceType = "KeyReference";
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00017279 File Offset: 0x00016279
		public KeyReference(string uri)
			: base(uri)
		{
			base.ReferenceType = "KeyReference";
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001728D File Offset: 0x0001628D
		public KeyReference(string uri, TransformChain transformChain)
			: base(uri, transformChain)
		{
			base.ReferenceType = "KeyReference";
		}
	}
}
