using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000E9 RID: 233
	[ComVisible(true)]
	[Serializable]
	public class PlatformNotSupportedException : NotSupportedException
	{
		// Token: 0x06000C85 RID: 3205 RVA: 0x00025889 File Offset: 0x00024889
		public PlatformNotSupportedException()
			: base(Environment.GetResourceString("Arg_PlatformNotSupported"))
		{
			base.SetErrorCode(-2146233031);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x000258A6 File Offset: 0x000248A6
		public PlatformNotSupportedException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233031);
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x000258BA File Offset: 0x000248BA
		public PlatformNotSupportedException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233031);
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000258CF File Offset: 0x000248CF
		protected PlatformNotSupportedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
