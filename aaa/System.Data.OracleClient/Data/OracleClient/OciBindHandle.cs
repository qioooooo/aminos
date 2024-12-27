using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000046 RID: 70
	internal sealed class OciBindHandle : OciSimpleHandle
	{
		// Token: 0x06000211 RID: 529 RVA: 0x0005B56C File Offset: 0x0005A96C
		internal OciBindHandle(OciHandle parent, IntPtr value)
			: base(parent, OCI.HTYPE.OCI_HTYPE_BIND, value)
		{
		}
	}
}
