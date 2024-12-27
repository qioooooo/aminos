using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000044 RID: 68
	internal sealed class OciStatementHandle : OciHandle
	{
		// Token: 0x0600020C RID: 524 RVA: 0x0005B4D4 File Offset: 0x0005A8D4
		internal OciStatementHandle(OciHandle parent)
			: base(parent, OCI.HTYPE.OCI_HTYPE_STMT)
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0005B4EC File Offset: 0x0005A8EC
		internal OciParameterDescriptor GetDescriptor(int i, OciErrorHandle errorHandle)
		{
			IntPtr intPtr;
			int num = TracedNativeMethods.OCIParamGet(this, base.HandleType, errorHandle, out intPtr, i + 1);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
			return new OciParameterDescriptor(this, intPtr);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0005B520 File Offset: 0x0005A920
		internal OciRowidDescriptor GetRowid(OciHandle environmentHandle, OciErrorHandle errorHandle)
		{
			OciRowidDescriptor ociRowidDescriptor = new OciRowidDescriptor(environmentHandle);
			ociRowidDescriptor.GetRowid(this, errorHandle);
			return ociRowidDescriptor;
		}
	}
}
