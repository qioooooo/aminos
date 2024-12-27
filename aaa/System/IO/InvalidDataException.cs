using System;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x02000213 RID: 531
	[Serializable]
	public sealed class InvalidDataException : SystemException
	{
		// Token: 0x060011FD RID: 4605 RVA: 0x0003CA32 File Offset: 0x0003BA32
		public InvalidDataException()
			: base(SR.GetString("GenericInvalidData"))
		{
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x0003CA44 File Offset: 0x0003BA44
		public InvalidDataException(string message)
			: base(message)
		{
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0003CA4D File Offset: 0x0003BA4D
		public InvalidDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0003CA57 File Offset: 0x0003BA57
		internal InvalidDataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
