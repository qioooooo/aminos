using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200005B RID: 91
	internal class DsmlConstants
	{
		// Token: 0x060001B1 RID: 433 RVA: 0x00007BBB File Offset: 0x00006BBB
		private DsmlConstants()
		{
		}

		// Token: 0x040001AB RID: 427
		public const string DsmlUri = "urn:oasis:names:tc:DSML:2:0:core";

		// Token: 0x040001AC RID: 428
		public const string XsiUri = "http://www.w3.org/2001/XMLSchema-instance";

		// Token: 0x040001AD RID: 429
		public const string XsdUri = "http://www.w3.org/2001/XMLSchema";

		// Token: 0x040001AE RID: 430
		public const string SoapUri = "http://schemas.xmlsoap.org/soap/envelope/";

		// Token: 0x040001AF RID: 431
		public const string ADSessionUri = "urn:schema-microsoft-com:activedirectory:dsmlv2";

		// Token: 0x040001B0 RID: 432
		public const string DefaultSearchFilter = "<present name='objectClass' xmlns=\"urn:oasis:names:tc:DSML:2:0:core\"/>";

		// Token: 0x040001B1 RID: 433
		public const string HttpPostMethod = "POST";

		// Token: 0x040001B2 RID: 434
		public const string SOAPEnvelopeBegin = "<se:Envelope xmlns:se=\"http://schemas.xmlsoap.org/soap/envelope/\">";

		// Token: 0x040001B3 RID: 435
		public const string SOAPEnvelopeEnd = "</se:Envelope>";

		// Token: 0x040001B4 RID: 436
		public const string SOAPBodyBegin = "<se:Body xmlns=\"urn:oasis:names:tc:DSML:2:0:core\">";

		// Token: 0x040001B5 RID: 437
		public const string SOAPBodyEnd = "</se:Body>";

		// Token: 0x040001B6 RID: 438
		public const string SOAPHeaderBegin = "<se:Header>";

		// Token: 0x040001B7 RID: 439
		public const string SOAPHeaderEnd = "</se:Header>";

		// Token: 0x040001B8 RID: 440
		public const string SOAPSession1 = "<ad:Session xmlns:ad=\"urn:schema-microsoft-com:activedirectory:dsmlv2\" ad:SessionID=\"";

		// Token: 0x040001B9 RID: 441
		public const string SOAPSession2 = "\" se:mustUnderstand=\"1\"/>";

		// Token: 0x040001BA RID: 442
		public const string SOAPBeginSession = "<ad:BeginSession xmlns:ad=\"urn:schema-microsoft-com:activedirectory:dsmlv2\" se:mustUnderstand=\"1\"/>";

		// Token: 0x040001BB RID: 443
		public const string SOAPEndSession1 = "<ad:EndSession xmlns:ad=\"urn:schema-microsoft-com:activedirectory:dsmlv2\" ad:SessionID=\"";

		// Token: 0x040001BC RID: 444
		public const string SOAPEndSession2 = "\" se:mustUnderstand=\"1\"/>";

		// Token: 0x040001BD RID: 445
		public const string DsmlErrorResponse = "errorResponse";

		// Token: 0x040001BE RID: 446
		public const string DsmlSearchResponse = "searchResponse";

		// Token: 0x040001BF RID: 447
		public const string DsmlModifyResponse = "modifyResponse";

		// Token: 0x040001C0 RID: 448
		public const string DsmlAddResponse = "addResponse";

		// Token: 0x040001C1 RID: 449
		public const string DsmlDelResponse = "delResponse";

		// Token: 0x040001C2 RID: 450
		public const string DsmlModDNResponse = "modDNResponse";

		// Token: 0x040001C3 RID: 451
		public const string DsmlCompareResponse = "compareResponse";

		// Token: 0x040001C4 RID: 452
		public const string DsmlExtendedResponse = "extendedResponse";

		// Token: 0x040001C5 RID: 453
		public const string DsmlAuthResponse = "authResponse";

		// Token: 0x040001C6 RID: 454
		public const string AttrTypePrefixedName = "xsi:type";

		// Token: 0x040001C7 RID: 455
		public const string AttrBinaryTypePrefixedValue = "xsd:base64Binary";

		// Token: 0x040001C8 RID: 456
		public const string AttrDsmlAttrName = "name";

		// Token: 0x040001C9 RID: 457
		public const string ElementDsmlAttrValue = "value";

		// Token: 0x040001CA RID: 458
		public const string ElementSearchReqFilter = "filter";

		// Token: 0x040001CB RID: 459
		public const string ElementSearchReqFilterAnd = "and";

		// Token: 0x040001CC RID: 460
		public const string ElementSearchReqFilterOr = "or";

		// Token: 0x040001CD RID: 461
		public const string ElementSearchReqFilterNot = "not";

		// Token: 0x040001CE RID: 462
		public const string ElementSearchReqFilterSubstr = "substrings";

		// Token: 0x040001CF RID: 463
		public const string ElementSearchReqFilterEqual = "equalityMatch";

		// Token: 0x040001D0 RID: 464
		public const string ElementSearchReqFilterGrteq = "greaterOrEqual";

		// Token: 0x040001D1 RID: 465
		public const string ElementSearchReqFilterLesseq = "lessOrEqual";

		// Token: 0x040001D2 RID: 466
		public const string ElementSearchReqFilterApprox = "approxMatch";

		// Token: 0x040001D3 RID: 467
		public const string ElementSearchReqFilterPresent = "present";

		// Token: 0x040001D4 RID: 468
		public const string ElementSearchReqFilterExtenmatch = "extensibleMatch";

		// Token: 0x040001D5 RID: 469
		public const string ElementSearchReqFilterExtenmatchValue = "value";

		// Token: 0x040001D6 RID: 470
		public const string AttrSearchReqFilterPresentName = "name";

		// Token: 0x040001D7 RID: 471
		public const string AttrSearchReqFilterExtenmatchName = "name";

		// Token: 0x040001D8 RID: 472
		public const string AttrSearchReqFilterExtenmatchMatchrule = "matchingRule";

		// Token: 0x040001D9 RID: 473
		public const string AttrSearchReqFilterExtenmatchDnattr = "dnAttributes";

		// Token: 0x040001DA RID: 474
		public const string AttrSearchReqFilterSubstrName = "name";

		// Token: 0x040001DB RID: 475
		public const string ElementSearchReqFilterSubstrInit = "initial";

		// Token: 0x040001DC RID: 476
		public const string ElementSearchReqFilterSubstrAny = "any";

		// Token: 0x040001DD RID: 477
		public const string ElementSearchReqFilterSubstrFinal = "final";
	}
}
