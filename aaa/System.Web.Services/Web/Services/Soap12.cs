using System;

namespace System.Web.Services
{
	// Token: 0x0200000C RID: 12
	internal sealed class Soap12
	{
		// Token: 0x06000018 RID: 24 RVA: 0x0000235C File Offset: 0x0000135C
		private Soap12()
		{
		}

		// Token: 0x040001EE RID: 494
		internal const string Namespace = "http://www.w3.org/2003/05/soap-envelope";

		// Token: 0x040001EF RID: 495
		internal const string Encoding = "http://www.w3.org/2003/05/soap-encoding";

		// Token: 0x040001F0 RID: 496
		internal const string RpcNamespace = "http://www.w3.org/2003/05/soap-rpc";

		// Token: 0x040001F1 RID: 497
		internal const string Prefix = "soap12";

		// Token: 0x0200000D RID: 13
		internal class Attribute
		{
			// Token: 0x06000019 RID: 25 RVA: 0x00002364 File Offset: 0x00001364
			private Attribute()
			{
			}

			// Token: 0x040001F2 RID: 498
			internal const string UpgradeEnvelopeQname = "qname";

			// Token: 0x040001F3 RID: 499
			internal const string Role = "role";

			// Token: 0x040001F4 RID: 500
			internal const string Relay = "relay";
		}

		// Token: 0x0200000E RID: 14
		internal sealed class Element
		{
			// Token: 0x0600001A RID: 26 RVA: 0x0000236C File Offset: 0x0000136C
			private Element()
			{
			}

			// Token: 0x040001F5 RID: 501
			internal const string Upgrade = "Upgrade";

			// Token: 0x040001F6 RID: 502
			internal const string UpgradeEnvelope = "SupportedEnvelope";

			// Token: 0x040001F7 RID: 503
			internal const string FaultRole = "Role";

			// Token: 0x040001F8 RID: 504
			internal const string FaultReason = "Reason";

			// Token: 0x040001F9 RID: 505
			internal const string FaultReasonText = "Text";

			// Token: 0x040001FA RID: 506
			internal const string FaultCode = "Code";

			// Token: 0x040001FB RID: 507
			internal const string FaultNode = "Node";

			// Token: 0x040001FC RID: 508
			internal const string FaultCodeValue = "Value";

			// Token: 0x040001FD RID: 509
			internal const string FaultSubcode = "Subcode";

			// Token: 0x040001FE RID: 510
			internal const string FaultDetail = "Detail";
		}

		// Token: 0x0200000F RID: 15
		internal sealed class Code
		{
			// Token: 0x0600001B RID: 27 RVA: 0x00002374 File Offset: 0x00001374
			private Code()
			{
			}

			// Token: 0x040001FF RID: 511
			internal const string VersionMismatch = "VersionMismatch";

			// Token: 0x04000200 RID: 512
			internal const string MustUnderstand = "MustUnderstand";

			// Token: 0x04000201 RID: 513
			internal const string DataEncodingUnknown = "DataEncodingUnknown";

			// Token: 0x04000202 RID: 514
			internal const string Sender = "Sender";

			// Token: 0x04000203 RID: 515
			internal const string Receiver = "Receiver";

			// Token: 0x04000204 RID: 516
			internal const string RpcProcedureNotPresentSubcode = "ProcedureNotPresent";

			// Token: 0x04000205 RID: 517
			internal const string RpcBadArgumentsSubcode = "BadArguments";

			// Token: 0x04000206 RID: 518
			internal const string EncodingMissingIDFaultSubcode = "MissingID";

			// Token: 0x04000207 RID: 519
			internal const string EncodingUntypedValueFaultSubcode = "UntypedValue";
		}
	}
}
