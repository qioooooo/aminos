using System;

namespace System.Data.OracleClient
{
	// Token: 0x0200003B RID: 59
	internal sealed class OciDateTimeDescriptor : OciHandle
	{
		// Token: 0x06000200 RID: 512 RVA: 0x0005B330 File Offset: 0x0005A730
		internal OciDateTimeDescriptor(OciHandle parent, OCI.HTYPE dateTimeType)
			: base(parent, OciDateTimeDescriptor.AssertDateTimeType(dateTimeType))
		{
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0005B34C File Offset: 0x0005A74C
		private static OCI.HTYPE AssertDateTimeType(OCI.HTYPE dateTimeType)
		{
			return dateTimeType;
		}
	}
}
