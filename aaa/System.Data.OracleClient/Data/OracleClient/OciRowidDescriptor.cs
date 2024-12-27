using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000040 RID: 64
	internal sealed class OciRowidDescriptor : OciHandle
	{
		// Token: 0x06000207 RID: 519 RVA: 0x0005B440 File Offset: 0x0005A840
		internal OciRowidDescriptor(OciHandle parent)
			: base(parent, OCI.HTYPE.OCI_DTYPE_ROWID)
		{
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0005B458 File Offset: 0x0005A858
		internal void GetRowid(OciStatementHandle statementHandle, OciErrorHandle errorHandle)
		{
			uint num = 0U;
			int num2 = TracedNativeMethods.OCIAttrGet(statementHandle, this, out num, OCI.ATTR.OCI_ATTR_ROWID, errorHandle);
			if (100 == num2)
			{
				base.Dispose();
				return;
			}
			if (num2 != 0)
			{
				OracleException.Check(errorHandle, num2);
			}
		}
	}
}
