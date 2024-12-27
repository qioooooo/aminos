using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000042 RID: 66
	internal sealed class OciServiceContextHandle : OciHandle
	{
		// Token: 0x0600020A RID: 522 RVA: 0x0005B4A4 File Offset: 0x0005A8A4
		internal OciServiceContextHandle(OciHandle parent)
			: base(parent, OCI.HTYPE.OCI_HTYPE_SVCCTX, OCI.MODE.OCI_DEFAULT, OciHandle.HANDLEFLAG.DEFAULT)
		{
		}
	}
}
