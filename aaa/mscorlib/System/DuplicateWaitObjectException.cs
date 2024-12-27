using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000AA RID: 170
	[ComVisible(true)]
	[Serializable]
	public class DuplicateWaitObjectException : ArgumentException
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0001F8A3 File Offset: 0x0001E8A3
		private static string DuplicateWaitObjectMessage
		{
			get
			{
				if (DuplicateWaitObjectException._duplicateWaitObjectMessage == null)
				{
					DuplicateWaitObjectException._duplicateWaitObjectMessage = Environment.GetResourceString("Arg_DuplicateWaitObjectException");
				}
				return DuplicateWaitObjectException._duplicateWaitObjectMessage;
			}
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0001F8C0 File Offset: 0x0001E8C0
		public DuplicateWaitObjectException()
			: base(DuplicateWaitObjectException.DuplicateWaitObjectMessage)
		{
			base.SetErrorCode(-2146233047);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0001F8D8 File Offset: 0x0001E8D8
		public DuplicateWaitObjectException(string parameterName)
			: base(DuplicateWaitObjectException.DuplicateWaitObjectMessage, parameterName)
		{
			base.SetErrorCode(-2146233047);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0001F8F1 File Offset: 0x0001E8F1
		public DuplicateWaitObjectException(string parameterName, string message)
			: base(message, parameterName)
		{
			base.SetErrorCode(-2146233047);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0001F906 File Offset: 0x0001E906
		public DuplicateWaitObjectException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233047);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0001F91B File Offset: 0x0001E91B
		protected DuplicateWaitObjectException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x040003A0 RID: 928
		private static string _duplicateWaitObjectMessage;
	}
}
