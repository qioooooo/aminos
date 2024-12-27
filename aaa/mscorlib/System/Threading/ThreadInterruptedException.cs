using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000158 RID: 344
	[ComVisible(true)]
	[Serializable]
	public class ThreadInterruptedException : SystemException
	{
		// Token: 0x06001352 RID: 4946 RVA: 0x00035125 File Offset: 0x00034125
		public ThreadInterruptedException()
			: base(Exception.GetMessageFromNativeResources(Exception.ExceptionMessageKind.ThreadInterrupted))
		{
			base.SetErrorCode(-2146233063);
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0003513E File Offset: 0x0003413E
		public ThreadInterruptedException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233063);
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00035152 File Offset: 0x00034152
		public ThreadInterruptedException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233063);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00035167 File Offset: 0x00034167
		protected ThreadInterruptedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
