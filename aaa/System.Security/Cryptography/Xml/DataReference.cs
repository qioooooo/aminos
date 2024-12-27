using System;
using System.Security.Permissions;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000C0 RID: 192
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class DataReference : EncryptedReference
	{
		// Token: 0x0600049B RID: 1179 RVA: 0x0001722A File Offset: 0x0001622A
		public DataReference()
		{
			base.ReferenceType = "DataReference";
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001723D File Offset: 0x0001623D
		public DataReference(string uri)
			: base(uri)
		{
			base.ReferenceType = "DataReference";
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00017251 File Offset: 0x00016251
		public DataReference(string uri, TransformChain transformChain)
			: base(uri, transformChain)
		{
			base.ReferenceType = "DataReference";
		}
	}
}
