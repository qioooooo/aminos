using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Cryptography
{
	// Token: 0x0200084E RID: 2126
	[ComVisible(true)]
	[Serializable]
	public class CryptographicUnexpectedOperationException : CryptographicException
	{
		// Token: 0x06004E1E RID: 19998 RVA: 0x0010FBA9 File Offset: 0x0010EBA9
		public CryptographicUnexpectedOperationException()
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x0010FBBC File Offset: 0x0010EBBC
		public CryptographicUnexpectedOperationException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x0010FBD0 File Offset: 0x0010EBD0
		public CryptographicUnexpectedOperationException(string format, string insert)
			: base(string.Format(CultureInfo.CurrentCulture, format, new object[] { insert }))
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x0010FC05 File Offset: 0x0010EC05
		public CryptographicUnexpectedOperationException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x0010FC1A File Offset: 0x0010EC1A
		protected CryptographicUnexpectedOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
