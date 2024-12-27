using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F6 RID: 1270
	[ComVisible(true)]
	[Serializable]
	public class COMException : ExternalException
	{
		// Token: 0x0600315F RID: 12639 RVA: 0x000A95C8 File Offset: 0x000A85C8
		public COMException()
			: base(Environment.GetResourceString("Arg_COMException"))
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x000A95E5 File Offset: 0x000A85E5
		public COMException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x000A95F9 File Offset: 0x000A85F9
		public COMException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x000A960E File Offset: 0x000A860E
		public COMException(string message, int errorCode)
			: base(message)
		{
			base.SetErrorCode(errorCode);
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x000A961E File Offset: 0x000A861E
		protected COMException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x000A9628 File Offset: 0x000A8628
		public override string ToString()
		{
			string message = this.Message;
			string text = base.GetType().ToString();
			string text2 = text + " (0x" + base.HResult.ToString("X8", CultureInfo.InvariantCulture) + ")";
			if (message != null && message.Length > 0)
			{
				text2 = text2 + ": " + message;
			}
			Exception innerException = base.InnerException;
			if (innerException != null)
			{
				text2 = text2 + " ---> " + innerException.ToString();
			}
			if (this.StackTrace != null)
			{
				text2 = text2 + Environment.NewLine + this.StackTrace;
			}
			return text2;
		}
	}
}
