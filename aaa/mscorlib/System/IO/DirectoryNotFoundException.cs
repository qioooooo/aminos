using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x02000599 RID: 1433
	[ComVisible(true)]
	[Serializable]
	public class DirectoryNotFoundException : IOException
	{
		// Token: 0x06003547 RID: 13639 RVA: 0x000B2681 File Offset: 0x000B1681
		public DirectoryNotFoundException()
			: base(Environment.GetResourceString("Arg_DirectoryNotFoundException"))
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x000B269E File Offset: 0x000B169E
		public DirectoryNotFoundException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x000B26B2 File Offset: 0x000B16B2
		public DirectoryNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x000B26C7 File Offset: 0x000B16C7
		protected DirectoryNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
