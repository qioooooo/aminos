using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008A7 RID: 2215
	internal static class X509Constants
	{
		// Token: 0x0400298E RID: 10638
		internal const uint CRYPT_EXPORTABLE = 1U;

		// Token: 0x0400298F RID: 10639
		internal const uint CRYPT_USER_PROTECTED = 2U;

		// Token: 0x04002990 RID: 10640
		internal const uint CRYPT_MACHINE_KEYSET = 32U;

		// Token: 0x04002991 RID: 10641
		internal const uint CRYPT_USER_KEYSET = 4096U;

		// Token: 0x04002992 RID: 10642
		internal const uint CERT_QUERY_CONTENT_CERT = 1U;

		// Token: 0x04002993 RID: 10643
		internal const uint CERT_QUERY_CONTENT_CTL = 2U;

		// Token: 0x04002994 RID: 10644
		internal const uint CERT_QUERY_CONTENT_CRL = 3U;

		// Token: 0x04002995 RID: 10645
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_STORE = 4U;

		// Token: 0x04002996 RID: 10646
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CERT = 5U;

		// Token: 0x04002997 RID: 10647
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CTL = 6U;

		// Token: 0x04002998 RID: 10648
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CRL = 7U;

		// Token: 0x04002999 RID: 10649
		internal const uint CERT_QUERY_CONTENT_PKCS7_SIGNED = 8U;

		// Token: 0x0400299A RID: 10650
		internal const uint CERT_QUERY_CONTENT_PKCS7_UNSIGNED = 9U;

		// Token: 0x0400299B RID: 10651
		internal const uint CERT_QUERY_CONTENT_PKCS7_SIGNED_EMBED = 10U;

		// Token: 0x0400299C RID: 10652
		internal const uint CERT_QUERY_CONTENT_PKCS10 = 11U;

		// Token: 0x0400299D RID: 10653
		internal const uint CERT_QUERY_CONTENT_PFX = 12U;

		// Token: 0x0400299E RID: 10654
		internal const uint CERT_QUERY_CONTENT_CERT_PAIR = 13U;

		// Token: 0x0400299F RID: 10655
		internal const uint CERT_STORE_PROV_MEMORY = 2U;

		// Token: 0x040029A0 RID: 10656
		internal const uint CERT_STORE_PROV_SYSTEM = 10U;

		// Token: 0x040029A1 RID: 10657
		internal const uint CERT_STORE_NO_CRYPT_RELEASE_FLAG = 1U;

		// Token: 0x040029A2 RID: 10658
		internal const uint CERT_STORE_SET_LOCALIZED_NAME_FLAG = 2U;

		// Token: 0x040029A3 RID: 10659
		internal const uint CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG = 4U;

		// Token: 0x040029A4 RID: 10660
		internal const uint CERT_STORE_DELETE_FLAG = 16U;

		// Token: 0x040029A5 RID: 10661
		internal const uint CERT_STORE_SHARE_STORE_FLAG = 64U;

		// Token: 0x040029A6 RID: 10662
		internal const uint CERT_STORE_SHARE_CONTEXT_FLAG = 128U;

		// Token: 0x040029A7 RID: 10663
		internal const uint CERT_STORE_MANIFOLD_FLAG = 256U;

		// Token: 0x040029A8 RID: 10664
		internal const uint CERT_STORE_ENUM_ARCHIVED_FLAG = 512U;

		// Token: 0x040029A9 RID: 10665
		internal const uint CERT_STORE_UPDATE_KEYID_FLAG = 1024U;

		// Token: 0x040029AA RID: 10666
		internal const uint CERT_STORE_BACKUP_RESTORE_FLAG = 2048U;

		// Token: 0x040029AB RID: 10667
		internal const uint CERT_STORE_READONLY_FLAG = 32768U;

		// Token: 0x040029AC RID: 10668
		internal const uint CERT_STORE_OPEN_EXISTING_FLAG = 16384U;

		// Token: 0x040029AD RID: 10669
		internal const uint CERT_STORE_CREATE_NEW_FLAG = 8192U;

		// Token: 0x040029AE RID: 10670
		internal const uint CERT_STORE_MAXIMUM_ALLOWED_FLAG = 4096U;

		// Token: 0x040029AF RID: 10671
		internal const uint CERT_NAME_EMAIL_TYPE = 1U;

		// Token: 0x040029B0 RID: 10672
		internal const uint CERT_NAME_RDN_TYPE = 2U;

		// Token: 0x040029B1 RID: 10673
		internal const uint CERT_NAME_SIMPLE_DISPLAY_TYPE = 4U;

		// Token: 0x040029B2 RID: 10674
		internal const uint CERT_NAME_FRIENDLY_DISPLAY_TYPE = 5U;

		// Token: 0x040029B3 RID: 10675
		internal const uint CERT_NAME_DNS_TYPE = 6U;

		// Token: 0x040029B4 RID: 10676
		internal const uint CERT_NAME_URL_TYPE = 7U;

		// Token: 0x040029B5 RID: 10677
		internal const uint CERT_NAME_UPN_TYPE = 8U;
	}
}
