using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000086 RID: 134
	[ComVisible(true)]
	[Serializable]
	public class TypeUnloadedException : SystemException
	{
		// Token: 0x06000768 RID: 1896 RVA: 0x00018278 File Offset: 0x00017278
		public TypeUnloadedException()
			: base(Environment.GetResourceString("Arg_TypeUnloadedException"))
		{
			base.SetErrorCode(-2146234349);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00018295 File Offset: 0x00017295
		public TypeUnloadedException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146234349);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x000182A9 File Offset: 0x000172A9
		public TypeUnloadedException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146234349);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000182BE File Offset: 0x000172BE
		protected TypeUnloadedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
