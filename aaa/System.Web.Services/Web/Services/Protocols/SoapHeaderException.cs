using System;
using System.Runtime.Serialization;
using System.Xml;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000071 RID: 113
	[Serializable]
	public class SoapHeaderException : SoapException
	{
		// Token: 0x0600030C RID: 780 RVA: 0x0000E413 File Offset: 0x0000D413
		public SoapHeaderException()
		{
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000E41B File Offset: 0x0000D41B
		public SoapHeaderException(string message, XmlQualifiedName code, string actor)
			: base(message, code, actor)
		{
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000E426 File Offset: 0x0000D426
		public SoapHeaderException(string message, XmlQualifiedName code, string actor, Exception innerException)
			: base(message, code, actor, innerException)
		{
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000E433 File Offset: 0x0000D433
		public SoapHeaderException(string message, XmlQualifiedName code)
			: base(message, code)
		{
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000E43D File Offset: 0x0000D43D
		public SoapHeaderException(string message, XmlQualifiedName code, Exception innerException)
			: base(message, code, innerException)
		{
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000E448 File Offset: 0x0000D448
		public SoapHeaderException(string message, XmlQualifiedName code, string actor, string role, SoapFaultSubCode subCode, Exception innerException)
			: base(message, code, actor, role, null, null, subCode, innerException)
		{
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000E468 File Offset: 0x0000D468
		public SoapHeaderException(string message, XmlQualifiedName code, string actor, string role, string lang, SoapFaultSubCode subCode, Exception innerException)
			: base(message, code, actor, role, lang, null, subCode, innerException)
		{
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000E487 File Offset: 0x0000D487
		protected SoapHeaderException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
