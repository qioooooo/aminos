using System;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x0200072D RID: 1837
	[Serializable]
	public class InternalBufferOverflowException : SystemException
	{
		// Token: 0x0600381F RID: 14367 RVA: 0x000ED110 File Offset: 0x000EC110
		public InternalBufferOverflowException()
		{
			base.HResult = -2146232059;
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x000ED123 File Offset: 0x000EC123
		public InternalBufferOverflowException(string message)
			: base(message)
		{
			base.HResult = -2146232059;
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x000ED137 File Offset: 0x000EC137
		public InternalBufferOverflowException(string message, Exception inner)
			: base(message, inner)
		{
			base.HResult = -2146232059;
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x000ED14C File Offset: 0x000EC14C
		protected InternalBufferOverflowException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
