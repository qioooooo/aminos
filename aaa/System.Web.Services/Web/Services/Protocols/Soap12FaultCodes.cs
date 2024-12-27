using System;
using System.Xml;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200006A RID: 106
	public sealed class Soap12FaultCodes
	{
		// Token: 0x060002D6 RID: 726 RVA: 0x0000D692 File Offset: 0x0000C692
		private Soap12FaultCodes()
		{
		}

		// Token: 0x04000315 RID: 789
		public static readonly XmlQualifiedName ReceiverFaultCode = new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope");

		// Token: 0x04000316 RID: 790
		public static readonly XmlQualifiedName SenderFaultCode = new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope");

		// Token: 0x04000317 RID: 791
		public static readonly XmlQualifiedName VersionMismatchFaultCode = new XmlQualifiedName("VersionMismatch", "http://www.w3.org/2003/05/soap-envelope");

		// Token: 0x04000318 RID: 792
		public static readonly XmlQualifiedName MustUnderstandFaultCode = new XmlQualifiedName("MustUnderstand", "http://www.w3.org/2003/05/soap-envelope");

		// Token: 0x04000319 RID: 793
		public static readonly XmlQualifiedName DataEncodingUnknownFaultCode = new XmlQualifiedName("DataEncodingUnknown", "http://www.w3.org/2003/05/soap-envelope");

		// Token: 0x0400031A RID: 794
		public static readonly XmlQualifiedName RpcProcedureNotPresentFaultCode = new XmlQualifiedName("ProcedureNotPresent", "http://www.w3.org/2003/05/soap-rpc");

		// Token: 0x0400031B RID: 795
		public static readonly XmlQualifiedName RpcBadArgumentsFaultCode = new XmlQualifiedName("BadArguments", "http://www.w3.org/2003/05/soap-rpc");

		// Token: 0x0400031C RID: 796
		public static readonly XmlQualifiedName EncodingMissingIdFaultCode = new XmlQualifiedName("MissingID", "http://www.w3.org/2003/05/soap-encoding");

		// Token: 0x0400031D RID: 797
		public static readonly XmlQualifiedName EncodingUntypedValueFaultCode = new XmlQualifiedName("UntypedValue", "http://www.w3.org/2003/05/soap-encoding");

		// Token: 0x0400031E RID: 798
		internal static readonly XmlQualifiedName UnsupportedMediaTypeFaultCode = new XmlQualifiedName("UnsupportedMediaType", "http://microsoft.com/soap/");

		// Token: 0x0400031F RID: 799
		internal static readonly XmlQualifiedName MethodNotAllowed = new XmlQualifiedName("MethodNotAllowed", "http://microsoft.com/soap/");
	}
}
