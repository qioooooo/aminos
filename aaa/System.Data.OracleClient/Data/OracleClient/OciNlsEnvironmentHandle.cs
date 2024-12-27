using System;

namespace System.Data.OracleClient
{
	// Token: 0x0200003F RID: 63
	internal sealed class OciNlsEnvironmentHandle : OciHandle
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0005B428 File Offset: 0x0005A828
		internal OciNlsEnvironmentHandle(OCI.MODE environmentMode)
			: base(null, OCI.HTYPE.OCI_HTYPE_ENV, environmentMode, OciHandle.HANDLEFLAG.NLS)
		{
		}
	}
}
