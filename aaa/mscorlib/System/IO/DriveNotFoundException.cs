using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x0200059C RID: 1436
	[ComVisible(true)]
	[Serializable]
	public class DriveNotFoundException : IOException
	{
		// Token: 0x0600355A RID: 13658 RVA: 0x000B2B45 File Offset: 0x000B1B45
		public DriveNotFoundException()
			: base(Environment.GetResourceString("Arg_DriveNotFoundException"))
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x000B2B62 File Offset: 0x000B1B62
		public DriveNotFoundException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x000B2B76 File Offset: 0x000B1B76
		public DriveNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000B2B8B File Offset: 0x000B1B8B
		protected DriveNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
