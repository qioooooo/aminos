using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200007B RID: 123
	[ComVisible(true)]
	[Serializable]
	public class CannotUnloadAppDomainException : SystemException
	{
		// Token: 0x060006EE RID: 1774 RVA: 0x00016C82 File Offset: 0x00015C82
		public CannotUnloadAppDomainException()
			: base(Environment.GetResourceString("Arg_CannotUnloadAppDomainException"))
		{
			base.SetErrorCode(-2146234347);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00016C9F File Offset: 0x00015C9F
		public CannotUnloadAppDomainException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146234347);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00016CB3 File Offset: 0x00015CB3
		public CannotUnloadAppDomainException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146234347);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00016CC8 File Offset: 0x00015CC8
		protected CannotUnloadAppDomainException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
