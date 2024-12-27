using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000365 RID: 869
	[ComVisible(true)]
	[Serializable]
	public class SerializationException : SystemException
	{
		// Token: 0x0600229E RID: 8862 RVA: 0x00058076 File Offset: 0x00057076
		public SerializationException()
			: base(SerializationException._nullMessage)
		{
			base.SetErrorCode(-2146233076);
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x0005808E File Offset: 0x0005708E
		public SerializationException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233076);
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x000580A2 File Offset: 0x000570A2
		public SerializationException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233076);
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000580B7 File Offset: 0x000570B7
		protected SerializationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04000E5D RID: 3677
		private static string _nullMessage = Environment.GetResourceString("Arg_SerializationException");
	}
}
