using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security
{
	// Token: 0x02000681 RID: 1665
	[ComVisible(true)]
	[Serializable]
	public class VerificationException : SystemException
	{
		// Token: 0x06003CC6 RID: 15558 RVA: 0x000D0D8F File Offset: 0x000CFD8F
		public VerificationException()
			: base(Environment.GetResourceString("Verification_Exception"))
		{
			base.SetErrorCode(-2146233075);
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x000D0DAC File Offset: 0x000CFDAC
		public VerificationException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233075);
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x000D0DC0 File Offset: 0x000CFDC0
		public VerificationException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233075);
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x000D0DD5 File Offset: 0x000CFDD5
		protected VerificationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
