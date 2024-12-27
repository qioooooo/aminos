using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000048 RID: 72
	internal sealed class OciParameterDescriptor : OciSimpleHandle
	{
		// Token: 0x06000213 RID: 531 RVA: 0x0005B59C File Offset: 0x0005A99C
		internal OciParameterDescriptor(OciHandle parent, IntPtr value)
			: base(parent, OCI.HTYPE.OCI_DTYPE_PARAM, value)
		{
		}
	}
}
