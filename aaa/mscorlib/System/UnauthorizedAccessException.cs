using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200011F RID: 287
	[ComVisible(true)]
	[Serializable]
	public class UnauthorizedAccessException : SystemException
	{
		// Token: 0x060010E6 RID: 4326 RVA: 0x0002F430 File Offset: 0x0002E430
		public UnauthorizedAccessException()
			: base(Environment.GetResourceString("Arg_UnauthorizedAccessException"))
		{
			base.SetErrorCode(-2147024891);
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0002F44D File Offset: 0x0002E44D
		public UnauthorizedAccessException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024891);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0002F461 File Offset: 0x0002E461
		public UnauthorizedAccessException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2147024891);
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0002F476 File Offset: 0x0002E476
		protected UnauthorizedAccessException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
