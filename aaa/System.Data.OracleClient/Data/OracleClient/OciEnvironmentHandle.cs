using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000039 RID: 57
	internal sealed class OciEnvironmentHandle : OciHandle
	{
		// Token: 0x060001FC RID: 508 RVA: 0x0005B2D0 File Offset: 0x0005A6D0
		internal OciEnvironmentHandle(OCI.MODE environmentMode, bool unicode)
			: base(null, OCI.HTYPE.OCI_HTYPE_ENV, environmentMode, unicode ? OciHandle.HANDLEFLAG.UNICODE : OciHandle.HANDLEFLAG.DEFAULT)
		{
		}
	}
}
