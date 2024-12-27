using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000039 RID: 57
	[ComVisible(true)]
	[Serializable]
	public sealed class StackOverflowException : SystemException
	{
		// Token: 0x06000379 RID: 889 RVA: 0x0000E30E File Offset: 0x0000D30E
		public StackOverflowException()
			: base(Environment.GetResourceString("Arg_StackOverflowException"))
		{
			base.SetErrorCode(-2147023895);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000E32B File Offset: 0x0000D32B
		public StackOverflowException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147023895);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000E33F File Offset: 0x0000D33F
		public StackOverflowException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147023895);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000E354 File Offset: 0x0000D354
		internal StackOverflowException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
