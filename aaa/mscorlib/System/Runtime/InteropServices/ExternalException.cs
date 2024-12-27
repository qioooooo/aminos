using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F5 RID: 1269
	[ComVisible(true)]
	[Serializable]
	public class ExternalException : SystemException
	{
		// Token: 0x06003159 RID: 12633 RVA: 0x000A9560 File Offset: 0x000A8560
		public ExternalException()
			: base(Environment.GetResourceString("Arg_ExternalException"))
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x000A957D File Offset: 0x000A857D
		public ExternalException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x000A9591 File Offset: 0x000A8591
		public ExternalException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x000A95A6 File Offset: 0x000A85A6
		public ExternalException(string message, int errorCode)
			: base(message)
		{
			base.SetErrorCode(errorCode);
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x000A95B6 File Offset: 0x000A85B6
		protected ExternalException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x0600315E RID: 12638 RVA: 0x000A95C0 File Offset: 0x000A85C0
		public virtual int ErrorCode
		{
			get
			{
				return base.HResult;
			}
		}
	}
}
