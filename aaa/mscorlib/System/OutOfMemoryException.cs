using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000038 RID: 56
	[ComVisible(true)]
	[Serializable]
	public class OutOfMemoryException : SystemException
	{
		// Token: 0x06000375 RID: 885 RVA: 0x0000E2C2 File Offset: 0x0000D2C2
		public OutOfMemoryException()
			: base(Exception.GetMessageFromNativeResources(Exception.ExceptionMessageKind.OutOfMemory))
		{
			base.SetErrorCode(-2147024882);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000E2DB File Offset: 0x0000D2DB
		public OutOfMemoryException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024882);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000E2EF File Offset: 0x0000D2EF
		public OutOfMemoryException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024882);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000E304 File Offset: 0x0000D304
		protected OutOfMemoryException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
