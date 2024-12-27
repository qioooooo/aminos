using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000043 RID: 67
	internal sealed class OciSessionHandle : OciHandle
	{
		// Token: 0x0600020B RID: 523 RVA: 0x0005B4BC File Offset: 0x0005A8BC
		internal OciSessionHandle(OciHandle parent)
			: base(parent, OCI.HTYPE.OCI_HTYPE_SESSION, OCI.MODE.OCI_DEFAULT, OciHandle.HANDLEFLAG.DEFAULT)
		{
		}
	}
}
