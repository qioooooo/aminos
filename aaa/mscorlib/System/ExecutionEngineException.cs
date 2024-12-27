using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200003B RID: 59
	[ComVisible(true)]
	[Serializable]
	public sealed class ExecutionEngineException : SystemException
	{
		// Token: 0x06000381 RID: 897 RVA: 0x0000E3AE File Offset: 0x0000D3AE
		public ExecutionEngineException()
			: base(Environment.GetResourceString("Arg_ExecutionEngineException"))
		{
			base.SetErrorCode(-2146233082);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000E3CB File Offset: 0x0000D3CB
		public ExecutionEngineException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233082);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000E3DF File Offset: 0x0000D3DF
		public ExecutionEngineException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233082);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000E3F4 File Offset: 0x0000D3F4
		internal ExecutionEngineException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
