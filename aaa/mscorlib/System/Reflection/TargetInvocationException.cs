using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x0200032B RID: 811
	[ComVisible(true)]
	[Serializable]
	public sealed class TargetInvocationException : ApplicationException
	{
		// Token: 0x06001F7D RID: 8061 RVA: 0x0004FB1D File Offset: 0x0004EB1D
		private TargetInvocationException()
			: base(Environment.GetResourceString("Arg_TargetInvocationException"))
		{
			base.SetErrorCode(-2146232828);
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0004FB3A File Offset: 0x0004EB3A
		private TargetInvocationException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146232828);
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x0004FB4E File Offset: 0x0004EB4E
		public TargetInvocationException(Exception inner)
			: base(Environment.GetResourceString("Arg_TargetInvocationException"), inner)
		{
			base.SetErrorCode(-2146232828);
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x0004FB6C File Offset: 0x0004EB6C
		public TargetInvocationException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146232828);
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0004FB81 File Offset: 0x0004EB81
		internal TargetInvocationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
