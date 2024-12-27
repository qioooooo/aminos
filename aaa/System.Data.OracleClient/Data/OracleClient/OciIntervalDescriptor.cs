using System;

namespace System.Data.OracleClient
{
	// Token: 0x0200003D RID: 61
	internal sealed class OciIntervalDescriptor : OciHandle
	{
		// Token: 0x06000204 RID: 516 RVA: 0x0005B3F8 File Offset: 0x0005A7F8
		internal OciIntervalDescriptor(OciHandle parent)
			: base(parent, OCI.HTYPE.OCI_DTYPE_INTERVAL_DS)
		{
		}
	}
}
