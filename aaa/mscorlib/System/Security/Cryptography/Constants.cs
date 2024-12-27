using System;

namespace System.Security.Cryptography
{
	// Token: 0x020008A3 RID: 2211
	internal static class Constants
	{
		// Token: 0x04002942 RID: 10562
		internal const int S_OK = 0;

		// Token: 0x04002943 RID: 10563
		internal const int NTE_NO_KEY = -2146893811;

		// Token: 0x04002944 RID: 10564
		internal const int NTE_BAD_KEYSET = -2146893802;

		// Token: 0x04002945 RID: 10565
		internal const int NTE_KEYSET_NOT_DEF = -2146893799;

		// Token: 0x04002946 RID: 10566
		internal const int KP_IV = 1;

		// Token: 0x04002947 RID: 10567
		internal const int KP_MODE = 4;

		// Token: 0x04002948 RID: 10568
		internal const int KP_MODE_BITS = 5;

		// Token: 0x04002949 RID: 10569
		internal const int KP_EFFECTIVE_KEYLEN = 19;

		// Token: 0x0400294A RID: 10570
		internal const int ALG_CLASS_SIGNATURE = 8192;

		// Token: 0x0400294B RID: 10571
		internal const int ALG_CLASS_DATA_ENCRYPT = 24576;

		// Token: 0x0400294C RID: 10572
		internal const int ALG_CLASS_HASH = 32768;

		// Token: 0x0400294D RID: 10573
		internal const int ALG_CLASS_KEY_EXCHANGE = 40960;

		// Token: 0x0400294E RID: 10574
		internal const int ALG_TYPE_DSS = 512;

		// Token: 0x0400294F RID: 10575
		internal const int ALG_TYPE_RSA = 1024;

		// Token: 0x04002950 RID: 10576
		internal const int ALG_TYPE_BLOCK = 1536;

		// Token: 0x04002951 RID: 10577
		internal const int ALG_TYPE_STREAM = 2048;

		// Token: 0x04002952 RID: 10578
		internal const int ALG_TYPE_ANY = 0;

		// Token: 0x04002953 RID: 10579
		internal const int CALG_MD5 = 32771;

		// Token: 0x04002954 RID: 10580
		internal const int CALG_SHA1 = 32772;

		// Token: 0x04002955 RID: 10581
		internal const int CALG_SHA_256 = 32780;

		// Token: 0x04002956 RID: 10582
		internal const int CALG_SHA_384 = 32781;

		// Token: 0x04002957 RID: 10583
		internal const int CALG_SHA_512 = 32782;

		// Token: 0x04002958 RID: 10584
		internal const int CALG_RSA_KEYX = 41984;

		// Token: 0x04002959 RID: 10585
		internal const int CALG_RSA_SIGN = 9216;

		// Token: 0x0400295A RID: 10586
		internal const int CALG_DSS_SIGN = 8704;

		// Token: 0x0400295B RID: 10587
		internal const int CALG_DES = 26113;

		// Token: 0x0400295C RID: 10588
		internal const int CALG_RC2 = 26114;

		// Token: 0x0400295D RID: 10589
		internal const int CALG_3DES = 26115;

		// Token: 0x0400295E RID: 10590
		internal const int CALG_3DES_112 = 26121;

		// Token: 0x0400295F RID: 10591
		internal const int CALG_AES_128 = 26126;

		// Token: 0x04002960 RID: 10592
		internal const int CALG_AES_192 = 26127;

		// Token: 0x04002961 RID: 10593
		internal const int CALG_AES_256 = 26128;

		// Token: 0x04002962 RID: 10594
		internal const int CALG_RC4 = 26625;

		// Token: 0x04002963 RID: 10595
		internal const int PROV_RSA_FULL = 1;

		// Token: 0x04002964 RID: 10596
		internal const int PROV_DSS_DH = 13;

		// Token: 0x04002965 RID: 10597
		internal const int PROV_RSA_AES = 24;

		// Token: 0x04002966 RID: 10598
		internal const int AT_KEYEXCHANGE = 1;

		// Token: 0x04002967 RID: 10599
		internal const int AT_SIGNATURE = 2;

		// Token: 0x04002968 RID: 10600
		internal const int PUBLICKEYBLOB = 6;

		// Token: 0x04002969 RID: 10601
		internal const int PRIVATEKEYBLOB = 7;

		// Token: 0x0400296A RID: 10602
		internal const int CRYPT_OAEP = 64;

		// Token: 0x0400296B RID: 10603
		internal const uint CRYPT_VERIFYCONTEXT = 4026531840U;

		// Token: 0x0400296C RID: 10604
		internal const uint CRYPT_NEWKEYSET = 8U;

		// Token: 0x0400296D RID: 10605
		internal const uint CRYPT_DELETEKEYSET = 16U;

		// Token: 0x0400296E RID: 10606
		internal const uint CRYPT_MACHINE_KEYSET = 32U;

		// Token: 0x0400296F RID: 10607
		internal const uint CRYPT_SILENT = 64U;

		// Token: 0x04002970 RID: 10608
		internal const uint CRYPT_EXPORTABLE = 1U;

		// Token: 0x04002971 RID: 10609
		internal const uint CLR_KEYLEN = 1U;

		// Token: 0x04002972 RID: 10610
		internal const uint CLR_PUBLICKEYONLY = 2U;

		// Token: 0x04002973 RID: 10611
		internal const uint CLR_EXPORTABLE = 3U;

		// Token: 0x04002974 RID: 10612
		internal const uint CLR_REMOVABLE = 4U;

		// Token: 0x04002975 RID: 10613
		internal const uint CLR_HARDWARE = 5U;

		// Token: 0x04002976 RID: 10614
		internal const uint CLR_ACCESSIBLE = 6U;

		// Token: 0x04002977 RID: 10615
		internal const uint CLR_PROTECTED = 7U;

		// Token: 0x04002978 RID: 10616
		internal const uint CLR_UNIQUE_CONTAINER = 8U;

		// Token: 0x04002979 RID: 10617
		internal const uint CLR_ALGID = 9U;

		// Token: 0x0400297A RID: 10618
		internal const uint CLR_PP_CLIENT_HWND = 10U;

		// Token: 0x0400297B RID: 10619
		internal const uint CLR_PP_PIN = 11U;

		// Token: 0x0400297C RID: 10620
		internal const string OID_RSA_SMIMEalgCMS3DESwrap = "1.2.840.113549.1.9.16.3.6";

		// Token: 0x0400297D RID: 10621
		internal const string OID_RSA_MD5 = "1.2.840.113549.2.5";

		// Token: 0x0400297E RID: 10622
		internal const string OID_RSA_RC2CBC = "1.2.840.113549.3.2";

		// Token: 0x0400297F RID: 10623
		internal const string OID_RSA_DES_EDE3_CBC = "1.2.840.113549.3.7";

		// Token: 0x04002980 RID: 10624
		internal const string OID_OIWSEC_desCBC = "1.3.14.3.2.7";

		// Token: 0x04002981 RID: 10625
		internal const string OID_OIWSEC_SHA1 = "1.3.14.3.2.26";

		// Token: 0x04002982 RID: 10626
		internal const string OID_OIWSEC_SHA256 = "2.16.840.1.101.3.4.2.1";

		// Token: 0x04002983 RID: 10627
		internal const string OID_OIWSEC_SHA384 = "2.16.840.1.101.3.4.2.2";

		// Token: 0x04002984 RID: 10628
		internal const string OID_OIWSEC_SHA512 = "2.16.840.1.101.3.4.2.3";

		// Token: 0x04002985 RID: 10629
		internal const string OID_OIWSEC_RIPEMD160 = "1.3.36.3.2.1";
	}
}
