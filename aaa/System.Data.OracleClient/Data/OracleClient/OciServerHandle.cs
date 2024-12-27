using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000041 RID: 65
	internal sealed class OciServerHandle : OciHandle
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0005B48C File Offset: 0x0005A88C
		internal OciServerHandle(OciHandle parent)
			: base(parent, OCI.HTYPE.OCI_HTYPE_SERVER, OCI.MODE.OCI_DEFAULT, OciHandle.HANDLEFLAG.DEFAULT)
		{
		}
	}
}
