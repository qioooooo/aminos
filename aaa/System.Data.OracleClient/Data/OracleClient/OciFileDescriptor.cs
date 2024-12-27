using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace System.Data.OracleClient
{
	// Token: 0x0200003C RID: 60
	internal sealed class OciFileDescriptor : OciHandle
	{
		// Token: 0x06000202 RID: 514 RVA: 0x0005B35C File Offset: 0x0005A75C
		internal OciFileDescriptor(OciHandle parent)
			: base(parent, OCI.HTYPE.OCI_DTYPE_FILE)
		{
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0005B374 File Offset: 0x0005A774
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal int OCILobFileSetNameWrapper(OciHandle envhp, OciHandle errhp, byte[] dirAlias, ushort dirAliasLength, byte[] fileName, ushort fileNameLength)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			int num;
			try
			{
				base.DangerousAddRef(ref flag);
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					IntPtr intPtr = base.DangerousGetHandle();
					num = UnsafeNativeMethods.OCILobFileSetName(envhp, errhp, ref intPtr, dirAlias, dirAliasLength, fileName, fileNameLength);
					this.handle = intPtr;
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return num;
		}
	}
}
