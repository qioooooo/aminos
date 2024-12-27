using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200004A RID: 74
	[ComVisible(true)]
	[Serializable]
	public class ApplicationException : Exception
	{
		// Token: 0x0600041D RID: 1053 RVA: 0x0001098F File Offset: 0x0000F98F
		public ApplicationException()
			: base(Environment.GetResourceString("Arg_ApplicationException"))
		{
			base.SetErrorCode(-2146232832);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000109AC File Offset: 0x0000F9AC
		public ApplicationException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146232832);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000109C0 File Offset: 0x0000F9C0
		public ApplicationException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146232832);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x000109D5 File Offset: 0x0000F9D5
		protected ApplicationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
