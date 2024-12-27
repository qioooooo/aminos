using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000C2 RID: 194
	[Serializable]
	public sealed class InsufficientMemoryException : OutOfMemoryException
	{
		// Token: 0x06000B03 RID: 2819 RVA: 0x000225AF File Offset: 0x000215AF
		public InsufficientMemoryException()
			: base(Exception.GetMessageFromNativeResources(Exception.ExceptionMessageKind.OutOfMemory))
		{
			base.SetErrorCode(-2146233027);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x000225C8 File Offset: 0x000215C8
		public InsufficientMemoryException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233027);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x000225DC File Offset: 0x000215DC
		public InsufficientMemoryException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233027);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x000225F1 File Offset: 0x000215F1
		private InsufficientMemoryException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
