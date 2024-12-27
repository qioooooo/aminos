using System;

namespace System.Web.Services
{
	// Token: 0x02000008 RID: 8
	internal class Soap
	{
		// Token: 0x06000014 RID: 20 RVA: 0x0000233C File Offset: 0x0000133C
		private Soap()
		{
		}

		// Token: 0x040001CF RID: 463
		internal const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";

		// Token: 0x040001D0 RID: 464
		internal const string Encoding = "http://schemas.xmlsoap.org/soap/encoding/";

		// Token: 0x040001D1 RID: 465
		internal const string Namespace = "http://schemas.xmlsoap.org/soap/envelope/";

		// Token: 0x040001D2 RID: 466
		internal const string ConformanceClaim = "http://ws-i.org/schemas/conformanceClaim/";

		// Token: 0x040001D3 RID: 467
		internal const string BasicProfile1_1 = "http://ws-i.org/profiles/basic/1.1";

		// Token: 0x040001D4 RID: 468
		internal const string Action = "SOAPAction";

		// Token: 0x040001D5 RID: 469
		internal const string ArrayType = "Array";

		// Token: 0x040001D6 RID: 470
		internal const string Prefix = "soap";

		// Token: 0x040001D7 RID: 471
		internal const string ClaimPrefix = "wsi";

		// Token: 0x040001D8 RID: 472
		internal const string DimeContentType = "application/dime";

		// Token: 0x040001D9 RID: 473
		internal const string SoapContentType = "text/xml";

		// Token: 0x02000009 RID: 9
		internal class Attribute
		{
			// Token: 0x06000015 RID: 21 RVA: 0x00002344 File Offset: 0x00001344
			private Attribute()
			{
			}

			// Token: 0x040001DA RID: 474
			internal const string MustUnderstand = "mustUnderstand";

			// Token: 0x040001DB RID: 475
			internal const string Actor = "actor";

			// Token: 0x040001DC RID: 476
			internal const string EncodingStyle = "encodingStyle";

			// Token: 0x040001DD RID: 477
			internal const string Lang = "lang";

			// Token: 0x040001DE RID: 478
			internal const string ConformsTo = "conformsTo";
		}

		// Token: 0x0200000A RID: 10
		internal class Element
		{
			// Token: 0x06000016 RID: 22 RVA: 0x0000234C File Offset: 0x0000134C
			private Element()
			{
			}

			// Token: 0x040001DF RID: 479
			internal const string Envelope = "Envelope";

			// Token: 0x040001E0 RID: 480
			internal const string Header = "Header";

			// Token: 0x040001E1 RID: 481
			internal const string Body = "Body";

			// Token: 0x040001E2 RID: 482
			internal const string Fault = "Fault";

			// Token: 0x040001E3 RID: 483
			internal const string FaultActor = "faultactor";

			// Token: 0x040001E4 RID: 484
			internal const string FaultCode = "faultcode";

			// Token: 0x040001E5 RID: 485
			internal const string FaultDetail = "detail";

			// Token: 0x040001E6 RID: 486
			internal const string FaultString = "faultstring";

			// Token: 0x040001E7 RID: 487
			internal const string StackTrace = "StackTrace";

			// Token: 0x040001E8 RID: 488
			internal const string Message = "Message";

			// Token: 0x040001E9 RID: 489
			internal const string Claim = "Claim";
		}

		// Token: 0x0200000B RID: 11
		internal class Code
		{
			// Token: 0x06000017 RID: 23 RVA: 0x00002354 File Offset: 0x00001354
			private Code()
			{
			}

			// Token: 0x040001EA RID: 490
			internal const string Server = "Server";

			// Token: 0x040001EB RID: 491
			internal const string VersionMismatch = "VersionMismatch";

			// Token: 0x040001EC RID: 492
			internal const string MustUnderstand = "MustUnderstand";

			// Token: 0x040001ED RID: 493
			internal const string Client = "Client";
		}
	}
}
