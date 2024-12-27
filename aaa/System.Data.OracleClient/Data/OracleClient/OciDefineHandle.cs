using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000047 RID: 71
	internal sealed class OciDefineHandle : OciSimpleHandle
	{
		// Token: 0x06000212 RID: 530 RVA: 0x0005B584 File Offset: 0x0005A984
		internal OciDefineHandle(OciHandle parent, IntPtr value)
			: base(parent, OCI.HTYPE.OCI_HTYPE_DEFINE, value)
		{
		}
	}
}
