using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x0200059D RID: 1437
	[ComVisible(true)]
	[Serializable]
	public class EndOfStreamException : IOException
	{
		// Token: 0x0600355E RID: 13662 RVA: 0x000B2B95 File Offset: 0x000B1B95
		public EndOfStreamException()
			: base(Environment.GetResourceString("Arg_EndOfStreamException"))
		{
			base.SetErrorCode(-2147024858);
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x000B2BB2 File Offset: 0x000B1BB2
		public EndOfStreamException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024858);
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000B2BC6 File Offset: 0x000B1BC6
		public EndOfStreamException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024858);
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000B2BDB File Offset: 0x000B1BDB
		protected EndOfStreamException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
