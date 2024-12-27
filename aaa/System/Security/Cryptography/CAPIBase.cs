using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Security.Cryptography
{
	// Token: 0x020002C9 RID: 713
	internal abstract class CAPIBase
	{
		// Token: 0x04001638 RID: 5688
		internal const string ADVAPI32 = "advapi32.dll";

		// Token: 0x04001639 RID: 5689
		internal const string CRYPT32 = "crypt32.dll";

		// Token: 0x0400163A RID: 5690
		internal const string KERNEL32 = "kernel32.dll";

		// Token: 0x0400163B RID: 5691
		internal const uint LMEM_FIXED = 0U;

		// Token: 0x0400163C RID: 5692
		internal const uint LMEM_ZEROINIT = 64U;

		// Token: 0x0400163D RID: 5693
		internal const uint LPTR = 64U;

		// Token: 0x0400163E RID: 5694
		internal const int S_OK = 0;

		// Token: 0x0400163F RID: 5695
		internal const int S_FALSE = 1;

		// Token: 0x04001640 RID: 5696
		internal const uint FORMAT_MESSAGE_FROM_SYSTEM = 4096U;

		// Token: 0x04001641 RID: 5697
		internal const uint FORMAT_MESSAGE_IGNORE_INSERTS = 512U;

		// Token: 0x04001642 RID: 5698
		internal const uint VER_PLATFORM_WIN32s = 0U;

		// Token: 0x04001643 RID: 5699
		internal const uint VER_PLATFORM_WIN32_WINDOWS = 1U;

		// Token: 0x04001644 RID: 5700
		internal const uint VER_PLATFORM_WIN32_NT = 2U;

		// Token: 0x04001645 RID: 5701
		internal const uint VER_PLATFORM_WINCE = 3U;

		// Token: 0x04001646 RID: 5702
		internal const uint ASN_TAG_NULL = 5U;

		// Token: 0x04001647 RID: 5703
		internal const uint ASN_TAG_OBJID = 6U;

		// Token: 0x04001648 RID: 5704
		internal const uint CERT_QUERY_OBJECT_FILE = 1U;

		// Token: 0x04001649 RID: 5705
		internal const uint CERT_QUERY_OBJECT_BLOB = 2U;

		// Token: 0x0400164A RID: 5706
		internal const uint CERT_QUERY_CONTENT_CERT = 1U;

		// Token: 0x0400164B RID: 5707
		internal const uint CERT_QUERY_CONTENT_CTL = 2U;

		// Token: 0x0400164C RID: 5708
		internal const uint CERT_QUERY_CONTENT_CRL = 3U;

		// Token: 0x0400164D RID: 5709
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_STORE = 4U;

		// Token: 0x0400164E RID: 5710
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CERT = 5U;

		// Token: 0x0400164F RID: 5711
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CTL = 6U;

		// Token: 0x04001650 RID: 5712
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CRL = 7U;

		// Token: 0x04001651 RID: 5713
		internal const uint CERT_QUERY_CONTENT_PKCS7_SIGNED = 8U;

		// Token: 0x04001652 RID: 5714
		internal const uint CERT_QUERY_CONTENT_PKCS7_UNSIGNED = 9U;

		// Token: 0x04001653 RID: 5715
		internal const uint CERT_QUERY_CONTENT_PKCS7_SIGNED_EMBED = 10U;

		// Token: 0x04001654 RID: 5716
		internal const uint CERT_QUERY_CONTENT_PKCS10 = 11U;

		// Token: 0x04001655 RID: 5717
		internal const uint CERT_QUERY_CONTENT_PFX = 12U;

		// Token: 0x04001656 RID: 5718
		internal const uint CERT_QUERY_CONTENT_CERT_PAIR = 13U;

		// Token: 0x04001657 RID: 5719
		internal const uint CERT_QUERY_CONTENT_FLAG_CERT = 2U;

		// Token: 0x04001658 RID: 5720
		internal const uint CERT_QUERY_CONTENT_FLAG_CTL = 4U;

		// Token: 0x04001659 RID: 5721
		internal const uint CERT_QUERY_CONTENT_FLAG_CRL = 8U;

		// Token: 0x0400165A RID: 5722
		internal const uint CERT_QUERY_CONTENT_FLAG_SERIALIZED_STORE = 16U;

		// Token: 0x0400165B RID: 5723
		internal const uint CERT_QUERY_CONTENT_FLAG_SERIALIZED_CERT = 32U;

		// Token: 0x0400165C RID: 5724
		internal const uint CERT_QUERY_CONTENT_FLAG_SERIALIZED_CTL = 64U;

		// Token: 0x0400165D RID: 5725
		internal const uint CERT_QUERY_CONTENT_FLAG_SERIALIZED_CRL = 128U;

		// Token: 0x0400165E RID: 5726
		internal const uint CERT_QUERY_CONTENT_FLAG_PKCS7_SIGNED = 256U;

		// Token: 0x0400165F RID: 5727
		internal const uint CERT_QUERY_CONTENT_FLAG_PKCS7_UNSIGNED = 512U;

		// Token: 0x04001660 RID: 5728
		internal const uint CERT_QUERY_CONTENT_FLAG_PKCS7_SIGNED_EMBED = 1024U;

		// Token: 0x04001661 RID: 5729
		internal const uint CERT_QUERY_CONTENT_FLAG_PKCS10 = 2048U;

		// Token: 0x04001662 RID: 5730
		internal const uint CERT_QUERY_CONTENT_FLAG_PFX = 4096U;

		// Token: 0x04001663 RID: 5731
		internal const uint CERT_QUERY_CONTENT_FLAG_CERT_PAIR = 8192U;

		// Token: 0x04001664 RID: 5732
		internal const uint CERT_QUERY_CONTENT_FLAG_ALL = 16382U;

		// Token: 0x04001665 RID: 5733
		internal const uint CERT_QUERY_FORMAT_BINARY = 1U;

		// Token: 0x04001666 RID: 5734
		internal const uint CERT_QUERY_FORMAT_BASE64_ENCODED = 2U;

		// Token: 0x04001667 RID: 5735
		internal const uint CERT_QUERY_FORMAT_ASN_ASCII_HEX_ENCODED = 3U;

		// Token: 0x04001668 RID: 5736
		internal const uint CERT_QUERY_FORMAT_FLAG_BINARY = 2U;

		// Token: 0x04001669 RID: 5737
		internal const uint CERT_QUERY_FORMAT_FLAG_BASE64_ENCODED = 4U;

		// Token: 0x0400166A RID: 5738
		internal const uint CERT_QUERY_FORMAT_FLAG_ASN_ASCII_HEX_ENCODED = 8U;

		// Token: 0x0400166B RID: 5739
		internal const uint CERT_QUERY_FORMAT_FLAG_ALL = 14U;

		// Token: 0x0400166C RID: 5740
		internal const uint CRYPT_OID_INFO_OID_KEY = 1U;

		// Token: 0x0400166D RID: 5741
		internal const uint CRYPT_OID_INFO_NAME_KEY = 2U;

		// Token: 0x0400166E RID: 5742
		internal const uint CRYPT_OID_INFO_ALGID_KEY = 3U;

		// Token: 0x0400166F RID: 5743
		internal const uint CRYPT_OID_INFO_SIGN_KEY = 4U;

		// Token: 0x04001670 RID: 5744
		internal const uint CRYPT_HASH_ALG_OID_GROUP_ID = 1U;

		// Token: 0x04001671 RID: 5745
		internal const uint CRYPT_ENCRYPT_ALG_OID_GROUP_ID = 2U;

		// Token: 0x04001672 RID: 5746
		internal const uint CRYPT_PUBKEY_ALG_OID_GROUP_ID = 3U;

		// Token: 0x04001673 RID: 5747
		internal const uint CRYPT_SIGN_ALG_OID_GROUP_ID = 4U;

		// Token: 0x04001674 RID: 5748
		internal const uint CRYPT_RDN_ATTR_OID_GROUP_ID = 5U;

		// Token: 0x04001675 RID: 5749
		internal const uint CRYPT_EXT_OR_ATTR_OID_GROUP_ID = 6U;

		// Token: 0x04001676 RID: 5750
		internal const uint CRYPT_ENHKEY_USAGE_OID_GROUP_ID = 7U;

		// Token: 0x04001677 RID: 5751
		internal const uint CRYPT_POLICY_OID_GROUP_ID = 8U;

		// Token: 0x04001678 RID: 5752
		internal const uint CRYPT_TEMPLATE_OID_GROUP_ID = 9U;

		// Token: 0x04001679 RID: 5753
		internal const uint CRYPT_LAST_OID_GROUP_ID = 9U;

		// Token: 0x0400167A RID: 5754
		internal const uint CRYPT_FIRST_ALG_OID_GROUP_ID = 1U;

		// Token: 0x0400167B RID: 5755
		internal const uint CRYPT_LAST_ALG_OID_GROUP_ID = 4U;

		// Token: 0x0400167C RID: 5756
		internal const uint CRYPT_ASN_ENCODING = 1U;

		// Token: 0x0400167D RID: 5757
		internal const uint CRYPT_NDR_ENCODING = 2U;

		// Token: 0x0400167E RID: 5758
		internal const uint X509_ASN_ENCODING = 1U;

		// Token: 0x0400167F RID: 5759
		internal const uint X509_NDR_ENCODING = 2U;

		// Token: 0x04001680 RID: 5760
		internal const uint PKCS_7_ASN_ENCODING = 65536U;

		// Token: 0x04001681 RID: 5761
		internal const uint PKCS_7_NDR_ENCODING = 131072U;

		// Token: 0x04001682 RID: 5762
		internal const uint PKCS_7_OR_X509_ASN_ENCODING = 65537U;

		// Token: 0x04001683 RID: 5763
		internal const uint CERT_STORE_PROV_MSG = 1U;

		// Token: 0x04001684 RID: 5764
		internal const uint CERT_STORE_PROV_MEMORY = 2U;

		// Token: 0x04001685 RID: 5765
		internal const uint CERT_STORE_PROV_FILE = 3U;

		// Token: 0x04001686 RID: 5766
		internal const uint CERT_STORE_PROV_REG = 4U;

		// Token: 0x04001687 RID: 5767
		internal const uint CERT_STORE_PROV_PKCS7 = 5U;

		// Token: 0x04001688 RID: 5768
		internal const uint CERT_STORE_PROV_SERIALIZED = 6U;

		// Token: 0x04001689 RID: 5769
		internal const uint CERT_STORE_PROV_FILENAME_A = 7U;

		// Token: 0x0400168A RID: 5770
		internal const uint CERT_STORE_PROV_FILENAME_W = 8U;

		// Token: 0x0400168B RID: 5771
		internal const uint CERT_STORE_PROV_FILENAME = 8U;

		// Token: 0x0400168C RID: 5772
		internal const uint CERT_STORE_PROV_SYSTEM_A = 9U;

		// Token: 0x0400168D RID: 5773
		internal const uint CERT_STORE_PROV_SYSTEM_W = 10U;

		// Token: 0x0400168E RID: 5774
		internal const uint CERT_STORE_PROV_SYSTEM = 10U;

		// Token: 0x0400168F RID: 5775
		internal const uint CERT_STORE_PROV_COLLECTION = 11U;

		// Token: 0x04001690 RID: 5776
		internal const uint CERT_STORE_PROV_SYSTEM_REGISTRY_A = 12U;

		// Token: 0x04001691 RID: 5777
		internal const uint CERT_STORE_PROV_SYSTEM_REGISTRY_W = 13U;

		// Token: 0x04001692 RID: 5778
		internal const uint CERT_STORE_PROV_SYSTEM_REGISTRY = 13U;

		// Token: 0x04001693 RID: 5779
		internal const uint CERT_STORE_PROV_PHYSICAL_W = 14U;

		// Token: 0x04001694 RID: 5780
		internal const uint CERT_STORE_PROV_PHYSICAL = 14U;

		// Token: 0x04001695 RID: 5781
		internal const uint CERT_STORE_PROV_SMART_CARD_W = 15U;

		// Token: 0x04001696 RID: 5782
		internal const uint CERT_STORE_PROV_SMART_CARD = 15U;

		// Token: 0x04001697 RID: 5783
		internal const uint CERT_STORE_PROV_LDAP_W = 16U;

		// Token: 0x04001698 RID: 5784
		internal const uint CERT_STORE_PROV_LDAP = 16U;

		// Token: 0x04001699 RID: 5785
		internal const uint CERT_STORE_NO_CRYPT_RELEASE_FLAG = 1U;

		// Token: 0x0400169A RID: 5786
		internal const uint CERT_STORE_SET_LOCALIZED_NAME_FLAG = 2U;

		// Token: 0x0400169B RID: 5787
		internal const uint CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG = 4U;

		// Token: 0x0400169C RID: 5788
		internal const uint CERT_STORE_DELETE_FLAG = 16U;

		// Token: 0x0400169D RID: 5789
		internal const uint CERT_STORE_SHARE_STORE_FLAG = 64U;

		// Token: 0x0400169E RID: 5790
		internal const uint CERT_STORE_SHARE_CONTEXT_FLAG = 128U;

		// Token: 0x0400169F RID: 5791
		internal const uint CERT_STORE_MANIFOLD_FLAG = 256U;

		// Token: 0x040016A0 RID: 5792
		internal const uint CERT_STORE_ENUM_ARCHIVED_FLAG = 512U;

		// Token: 0x040016A1 RID: 5793
		internal const uint CERT_STORE_UPDATE_KEYID_FLAG = 1024U;

		// Token: 0x040016A2 RID: 5794
		internal const uint CERT_STORE_BACKUP_RESTORE_FLAG = 2048U;

		// Token: 0x040016A3 RID: 5795
		internal const uint CERT_STORE_READONLY_FLAG = 32768U;

		// Token: 0x040016A4 RID: 5796
		internal const uint CERT_STORE_OPEN_EXISTING_FLAG = 16384U;

		// Token: 0x040016A5 RID: 5797
		internal const uint CERT_STORE_CREATE_NEW_FLAG = 8192U;

		// Token: 0x040016A6 RID: 5798
		internal const uint CERT_STORE_MAXIMUM_ALLOWED_FLAG = 4096U;

		// Token: 0x040016A7 RID: 5799
		internal const uint CERT_SYSTEM_STORE_UNPROTECTED_FLAG = 1073741824U;

		// Token: 0x040016A8 RID: 5800
		internal const uint CERT_SYSTEM_STORE_LOCATION_MASK = 16711680U;

		// Token: 0x040016A9 RID: 5801
		internal const uint CERT_SYSTEM_STORE_LOCATION_SHIFT = 16U;

		// Token: 0x040016AA RID: 5802
		internal const uint CERT_SYSTEM_STORE_CURRENT_USER_ID = 1U;

		// Token: 0x040016AB RID: 5803
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE_ID = 2U;

		// Token: 0x040016AC RID: 5804
		internal const uint CERT_SYSTEM_STORE_CURRENT_SERVICE_ID = 4U;

		// Token: 0x040016AD RID: 5805
		internal const uint CERT_SYSTEM_STORE_SERVICES_ID = 5U;

		// Token: 0x040016AE RID: 5806
		internal const uint CERT_SYSTEM_STORE_USERS_ID = 6U;

		// Token: 0x040016AF RID: 5807
		internal const uint CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY_ID = 7U;

		// Token: 0x040016B0 RID: 5808
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY_ID = 8U;

		// Token: 0x040016B1 RID: 5809
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE_ID = 9U;

		// Token: 0x040016B2 RID: 5810
		internal const uint CERT_SYSTEM_STORE_CURRENT_USER = 65536U;

		// Token: 0x040016B3 RID: 5811
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE = 131072U;

		// Token: 0x040016B4 RID: 5812
		internal const uint CERT_SYSTEM_STORE_CURRENT_SERVICE = 262144U;

		// Token: 0x040016B5 RID: 5813
		internal const uint CERT_SYSTEM_STORE_SERVICES = 327680U;

		// Token: 0x040016B6 RID: 5814
		internal const uint CERT_SYSTEM_STORE_USERS = 393216U;

		// Token: 0x040016B7 RID: 5815
		internal const uint CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY = 458752U;

		// Token: 0x040016B8 RID: 5816
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY = 524288U;

		// Token: 0x040016B9 RID: 5817
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE = 589824U;

		// Token: 0x040016BA RID: 5818
		internal const uint CERT_NAME_EMAIL_TYPE = 1U;

		// Token: 0x040016BB RID: 5819
		internal const uint CERT_NAME_RDN_TYPE = 2U;

		// Token: 0x040016BC RID: 5820
		internal const uint CERT_NAME_ATTR_TYPE = 3U;

		// Token: 0x040016BD RID: 5821
		internal const uint CERT_NAME_SIMPLE_DISPLAY_TYPE = 4U;

		// Token: 0x040016BE RID: 5822
		internal const uint CERT_NAME_FRIENDLY_DISPLAY_TYPE = 5U;

		// Token: 0x040016BF RID: 5823
		internal const uint CERT_NAME_DNS_TYPE = 6U;

		// Token: 0x040016C0 RID: 5824
		internal const uint CERT_NAME_URL_TYPE = 7U;

		// Token: 0x040016C1 RID: 5825
		internal const uint CERT_NAME_UPN_TYPE = 8U;

		// Token: 0x040016C2 RID: 5826
		internal const uint CERT_SIMPLE_NAME_STR = 1U;

		// Token: 0x040016C3 RID: 5827
		internal const uint CERT_OID_NAME_STR = 2U;

		// Token: 0x040016C4 RID: 5828
		internal const uint CERT_X500_NAME_STR = 3U;

		// Token: 0x040016C5 RID: 5829
		internal const uint CERT_NAME_STR_SEMICOLON_FLAG = 1073741824U;

		// Token: 0x040016C6 RID: 5830
		internal const uint CERT_NAME_STR_NO_PLUS_FLAG = 536870912U;

		// Token: 0x040016C7 RID: 5831
		internal const uint CERT_NAME_STR_NO_QUOTING_FLAG = 268435456U;

		// Token: 0x040016C8 RID: 5832
		internal const uint CERT_NAME_STR_CRLF_FLAG = 134217728U;

		// Token: 0x040016C9 RID: 5833
		internal const uint CERT_NAME_STR_COMMA_FLAG = 67108864U;

		// Token: 0x040016CA RID: 5834
		internal const uint CERT_NAME_STR_REVERSE_FLAG = 33554432U;

		// Token: 0x040016CB RID: 5835
		internal const uint CERT_NAME_ISSUER_FLAG = 1U;

		// Token: 0x040016CC RID: 5836
		internal const uint CERT_NAME_STR_DISABLE_IE4_UTF8_FLAG = 65536U;

		// Token: 0x040016CD RID: 5837
		internal const uint CERT_NAME_STR_ENABLE_T61_UNICODE_FLAG = 131072U;

		// Token: 0x040016CE RID: 5838
		internal const uint CERT_NAME_STR_ENABLE_UTF8_UNICODE_FLAG = 262144U;

		// Token: 0x040016CF RID: 5839
		internal const uint CERT_NAME_STR_FORCE_UTF8_DIR_STR_FLAG = 524288U;

		// Token: 0x040016D0 RID: 5840
		internal const uint CERT_KEY_PROV_HANDLE_PROP_ID = 1U;

		// Token: 0x040016D1 RID: 5841
		internal const uint CERT_KEY_PROV_INFO_PROP_ID = 2U;

		// Token: 0x040016D2 RID: 5842
		internal const uint CERT_SHA1_HASH_PROP_ID = 3U;

		// Token: 0x040016D3 RID: 5843
		internal const uint CERT_MD5_HASH_PROP_ID = 4U;

		// Token: 0x040016D4 RID: 5844
		internal const uint CERT_HASH_PROP_ID = 3U;

		// Token: 0x040016D5 RID: 5845
		internal const uint CERT_KEY_CONTEXT_PROP_ID = 5U;

		// Token: 0x040016D6 RID: 5846
		internal const uint CERT_KEY_SPEC_PROP_ID = 6U;

		// Token: 0x040016D7 RID: 5847
		internal const uint CERT_IE30_RESERVED_PROP_ID = 7U;

		// Token: 0x040016D8 RID: 5848
		internal const uint CERT_PUBKEY_HASH_RESERVED_PROP_ID = 8U;

		// Token: 0x040016D9 RID: 5849
		internal const uint CERT_ENHKEY_USAGE_PROP_ID = 9U;

		// Token: 0x040016DA RID: 5850
		internal const uint CERT_CTL_USAGE_PROP_ID = 9U;

		// Token: 0x040016DB RID: 5851
		internal const uint CERT_NEXT_UPDATE_LOCATION_PROP_ID = 10U;

		// Token: 0x040016DC RID: 5852
		internal const uint CERT_FRIENDLY_NAME_PROP_ID = 11U;

		// Token: 0x040016DD RID: 5853
		internal const uint CERT_PVK_FILE_PROP_ID = 12U;

		// Token: 0x040016DE RID: 5854
		internal const uint CERT_DESCRIPTION_PROP_ID = 13U;

		// Token: 0x040016DF RID: 5855
		internal const uint CERT_ACCESS_STATE_PROP_ID = 14U;

		// Token: 0x040016E0 RID: 5856
		internal const uint CERT_SIGNATURE_HASH_PROP_ID = 15U;

		// Token: 0x040016E1 RID: 5857
		internal const uint CERT_SMART_CARD_DATA_PROP_ID = 16U;

		// Token: 0x040016E2 RID: 5858
		internal const uint CERT_EFS_PROP_ID = 17U;

		// Token: 0x040016E3 RID: 5859
		internal const uint CERT_FORTEZZA_DATA_PROP_ID = 18U;

		// Token: 0x040016E4 RID: 5860
		internal const uint CERT_ARCHIVED_PROP_ID = 19U;

		// Token: 0x040016E5 RID: 5861
		internal const uint CERT_KEY_IDENTIFIER_PROP_ID = 20U;

		// Token: 0x040016E6 RID: 5862
		internal const uint CERT_AUTO_ENROLL_PROP_ID = 21U;

		// Token: 0x040016E7 RID: 5863
		internal const uint CERT_PUBKEY_ALG_PARA_PROP_ID = 22U;

		// Token: 0x040016E8 RID: 5864
		internal const uint CERT_CROSS_CERT_DIST_POINTS_PROP_ID = 23U;

		// Token: 0x040016E9 RID: 5865
		internal const uint CERT_ISSUER_PUBLIC_KEY_MD5_HASH_PROP_ID = 24U;

		// Token: 0x040016EA RID: 5866
		internal const uint CERT_SUBJECT_PUBLIC_KEY_MD5_HASH_PROP_ID = 25U;

		// Token: 0x040016EB RID: 5867
		internal const uint CERT_ENROLLMENT_PROP_ID = 26U;

		// Token: 0x040016EC RID: 5868
		internal const uint CERT_DATE_STAMP_PROP_ID = 27U;

		// Token: 0x040016ED RID: 5869
		internal const uint CERT_ISSUER_SERIAL_NUMBER_MD5_HASH_PROP_ID = 28U;

		// Token: 0x040016EE RID: 5870
		internal const uint CERT_SUBJECT_NAME_MD5_HASH_PROP_ID = 29U;

		// Token: 0x040016EF RID: 5871
		internal const uint CERT_EXTENDED_ERROR_INFO_PROP_ID = 30U;

		// Token: 0x040016F0 RID: 5872
		internal const uint CERT_RENEWAL_PROP_ID = 64U;

		// Token: 0x040016F1 RID: 5873
		internal const uint CERT_ARCHIVED_KEY_HASH_PROP_ID = 65U;

		// Token: 0x040016F2 RID: 5874
		internal const uint CERT_FIRST_RESERVED_PROP_ID = 66U;

		// Token: 0x040016F3 RID: 5875
		internal const uint CERT_DELETE_KEYSET_PROP_ID = 101U;

		// Token: 0x040016F4 RID: 5876
		internal const uint CERT_SET_PROPERTY_IGNORE_PERSIST_ERROR_FLAG = 2147483648U;

		// Token: 0x040016F5 RID: 5877
		internal const uint CERT_SET_PROPERTY_INHIBIT_PERSIST_FLAG = 1073741824U;

		// Token: 0x040016F6 RID: 5878
		internal const uint CERT_INFO_VERSION_FLAG = 1U;

		// Token: 0x040016F7 RID: 5879
		internal const uint CERT_INFO_SERIAL_NUMBER_FLAG = 2U;

		// Token: 0x040016F8 RID: 5880
		internal const uint CERT_INFO_SIGNATURE_ALGORITHM_FLAG = 3U;

		// Token: 0x040016F9 RID: 5881
		internal const uint CERT_INFO_ISSUER_FLAG = 4U;

		// Token: 0x040016FA RID: 5882
		internal const uint CERT_INFO_NOT_BEFORE_FLAG = 5U;

		// Token: 0x040016FB RID: 5883
		internal const uint CERT_INFO_NOT_AFTER_FLAG = 6U;

		// Token: 0x040016FC RID: 5884
		internal const uint CERT_INFO_SUBJECT_FLAG = 7U;

		// Token: 0x040016FD RID: 5885
		internal const uint CERT_INFO_SUBJECT_PUBLIC_KEY_INFO_FLAG = 8U;

		// Token: 0x040016FE RID: 5886
		internal const uint CERT_INFO_ISSUER_UNIQUE_ID_FLAG = 9U;

		// Token: 0x040016FF RID: 5887
		internal const uint CERT_INFO_SUBJECT_UNIQUE_ID_FLAG = 10U;

		// Token: 0x04001700 RID: 5888
		internal const uint CERT_INFO_EXTENSION_FLAG = 11U;

		// Token: 0x04001701 RID: 5889
		internal const uint CERT_COMPARE_MASK = 65535U;

		// Token: 0x04001702 RID: 5890
		internal const uint CERT_COMPARE_SHIFT = 16U;

		// Token: 0x04001703 RID: 5891
		internal const uint CERT_COMPARE_ANY = 0U;

		// Token: 0x04001704 RID: 5892
		internal const uint CERT_COMPARE_SHA1_HASH = 1U;

		// Token: 0x04001705 RID: 5893
		internal const uint CERT_COMPARE_NAME = 2U;

		// Token: 0x04001706 RID: 5894
		internal const uint CERT_COMPARE_ATTR = 3U;

		// Token: 0x04001707 RID: 5895
		internal const uint CERT_COMPARE_MD5_HASH = 4U;

		// Token: 0x04001708 RID: 5896
		internal const uint CERT_COMPARE_PROPERTY = 5U;

		// Token: 0x04001709 RID: 5897
		internal const uint CERT_COMPARE_PUBLIC_KEY = 6U;

		// Token: 0x0400170A RID: 5898
		internal const uint CERT_COMPARE_HASH = 1U;

		// Token: 0x0400170B RID: 5899
		internal const uint CERT_COMPARE_NAME_STR_A = 7U;

		// Token: 0x0400170C RID: 5900
		internal const uint CERT_COMPARE_NAME_STR_W = 8U;

		// Token: 0x0400170D RID: 5901
		internal const uint CERT_COMPARE_KEY_SPEC = 9U;

		// Token: 0x0400170E RID: 5902
		internal const uint CERT_COMPARE_ENHKEY_USAGE = 10U;

		// Token: 0x0400170F RID: 5903
		internal const uint CERT_COMPARE_CTL_USAGE = 10U;

		// Token: 0x04001710 RID: 5904
		internal const uint CERT_COMPARE_SUBJECT_CERT = 11U;

		// Token: 0x04001711 RID: 5905
		internal const uint CERT_COMPARE_ISSUER_OF = 12U;

		// Token: 0x04001712 RID: 5906
		internal const uint CERT_COMPARE_EXISTING = 13U;

		// Token: 0x04001713 RID: 5907
		internal const uint CERT_COMPARE_SIGNATURE_HASH = 14U;

		// Token: 0x04001714 RID: 5908
		internal const uint CERT_COMPARE_KEY_IDENTIFIER = 15U;

		// Token: 0x04001715 RID: 5909
		internal const uint CERT_COMPARE_CERT_ID = 16U;

		// Token: 0x04001716 RID: 5910
		internal const uint CERT_COMPARE_CROSS_CERT_DIST_POINTS = 17U;

		// Token: 0x04001717 RID: 5911
		internal const uint CERT_COMPARE_PUBKEY_MD5_HASH = 18U;

		// Token: 0x04001718 RID: 5912
		internal const uint CERT_FIND_ANY = 0U;

		// Token: 0x04001719 RID: 5913
		internal const uint CERT_FIND_SHA1_HASH = 65536U;

		// Token: 0x0400171A RID: 5914
		internal const uint CERT_FIND_MD5_HASH = 262144U;

		// Token: 0x0400171B RID: 5915
		internal const uint CERT_FIND_SIGNATURE_HASH = 917504U;

		// Token: 0x0400171C RID: 5916
		internal const uint CERT_FIND_KEY_IDENTIFIER = 983040U;

		// Token: 0x0400171D RID: 5917
		internal const uint CERT_FIND_HASH = 65536U;

		// Token: 0x0400171E RID: 5918
		internal const uint CERT_FIND_PROPERTY = 327680U;

		// Token: 0x0400171F RID: 5919
		internal const uint CERT_FIND_PUBLIC_KEY = 393216U;

		// Token: 0x04001720 RID: 5920
		internal const uint CERT_FIND_SUBJECT_NAME = 131079U;

		// Token: 0x04001721 RID: 5921
		internal const uint CERT_FIND_SUBJECT_ATTR = 196615U;

		// Token: 0x04001722 RID: 5922
		internal const uint CERT_FIND_ISSUER_NAME = 131076U;

		// Token: 0x04001723 RID: 5923
		internal const uint CERT_FIND_ISSUER_ATTR = 196612U;

		// Token: 0x04001724 RID: 5924
		internal const uint CERT_FIND_SUBJECT_STR_A = 458759U;

		// Token: 0x04001725 RID: 5925
		internal const uint CERT_FIND_SUBJECT_STR_W = 524295U;

		// Token: 0x04001726 RID: 5926
		internal const uint CERT_FIND_SUBJECT_STR = 524295U;

		// Token: 0x04001727 RID: 5927
		internal const uint CERT_FIND_ISSUER_STR_A = 458756U;

		// Token: 0x04001728 RID: 5928
		internal const uint CERT_FIND_ISSUER_STR_W = 524292U;

		// Token: 0x04001729 RID: 5929
		internal const uint CERT_FIND_ISSUER_STR = 524292U;

		// Token: 0x0400172A RID: 5930
		internal const uint CERT_FIND_KEY_SPEC = 589824U;

		// Token: 0x0400172B RID: 5931
		internal const uint CERT_FIND_ENHKEY_USAGE = 655360U;

		// Token: 0x0400172C RID: 5932
		internal const uint CERT_FIND_CTL_USAGE = 655360U;

		// Token: 0x0400172D RID: 5933
		internal const uint CERT_FIND_SUBJECT_CERT = 720896U;

		// Token: 0x0400172E RID: 5934
		internal const uint CERT_FIND_ISSUER_OF = 786432U;

		// Token: 0x0400172F RID: 5935
		internal const uint CERT_FIND_EXISTING = 851968U;

		// Token: 0x04001730 RID: 5936
		internal const uint CERT_FIND_CERT_ID = 1048576U;

		// Token: 0x04001731 RID: 5937
		internal const uint CERT_FIND_CROSS_CERT_DIST_POINTS = 1114112U;

		// Token: 0x04001732 RID: 5938
		internal const uint CERT_FIND_PUBKEY_MD5_HASH = 1179648U;

		// Token: 0x04001733 RID: 5939
		internal const uint CERT_ENCIPHER_ONLY_KEY_USAGE = 1U;

		// Token: 0x04001734 RID: 5940
		internal const uint CERT_CRL_SIGN_KEY_USAGE = 2U;

		// Token: 0x04001735 RID: 5941
		internal const uint CERT_KEY_CERT_SIGN_KEY_USAGE = 4U;

		// Token: 0x04001736 RID: 5942
		internal const uint CERT_KEY_AGREEMENT_KEY_USAGE = 8U;

		// Token: 0x04001737 RID: 5943
		internal const uint CERT_DATA_ENCIPHERMENT_KEY_USAGE = 16U;

		// Token: 0x04001738 RID: 5944
		internal const uint CERT_KEY_ENCIPHERMENT_KEY_USAGE = 32U;

		// Token: 0x04001739 RID: 5945
		internal const uint CERT_NON_REPUDIATION_KEY_USAGE = 64U;

		// Token: 0x0400173A RID: 5946
		internal const uint CERT_DIGITAL_SIGNATURE_KEY_USAGE = 128U;

		// Token: 0x0400173B RID: 5947
		internal const uint CERT_DECIPHER_ONLY_KEY_USAGE = 32768U;

		// Token: 0x0400173C RID: 5948
		internal const uint CERT_STORE_ADD_NEW = 1U;

		// Token: 0x0400173D RID: 5949
		internal const uint CERT_STORE_ADD_USE_EXISTING = 2U;

		// Token: 0x0400173E RID: 5950
		internal const uint CERT_STORE_ADD_REPLACE_EXISTING = 3U;

		// Token: 0x0400173F RID: 5951
		internal const uint CERT_STORE_ADD_ALWAYS = 4U;

		// Token: 0x04001740 RID: 5952
		internal const uint CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES = 5U;

		// Token: 0x04001741 RID: 5953
		internal const uint CERT_STORE_ADD_NEWER = 6U;

		// Token: 0x04001742 RID: 5954
		internal const uint CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES = 7U;

		// Token: 0x04001743 RID: 5955
		internal const uint CRYPT_FORMAT_STR_MULTI_LINE = 1U;

		// Token: 0x04001744 RID: 5956
		internal const uint CRYPT_FORMAT_STR_NO_HEX = 16U;

		// Token: 0x04001745 RID: 5957
		internal const uint CERT_STORE_SAVE_AS_STORE = 1U;

		// Token: 0x04001746 RID: 5958
		internal const uint CERT_STORE_SAVE_AS_PKCS7 = 2U;

		// Token: 0x04001747 RID: 5959
		internal const uint CERT_STORE_SAVE_TO_FILE = 1U;

		// Token: 0x04001748 RID: 5960
		internal const uint CERT_STORE_SAVE_TO_MEMORY = 2U;

		// Token: 0x04001749 RID: 5961
		internal const uint CERT_STORE_SAVE_TO_FILENAME_A = 3U;

		// Token: 0x0400174A RID: 5962
		internal const uint CERT_STORE_SAVE_TO_FILENAME_W = 4U;

		// Token: 0x0400174B RID: 5963
		internal const uint CERT_STORE_SAVE_TO_FILENAME = 4U;

		// Token: 0x0400174C RID: 5964
		internal const uint CERT_CA_SUBJECT_FLAG = 128U;

		// Token: 0x0400174D RID: 5965
		internal const uint CERT_END_ENTITY_SUBJECT_FLAG = 64U;

		// Token: 0x0400174E RID: 5966
		internal const uint REPORT_NO_PRIVATE_KEY = 1U;

		// Token: 0x0400174F RID: 5967
		internal const uint REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY = 2U;

		// Token: 0x04001750 RID: 5968
		internal const uint EXPORT_PRIVATE_KEYS = 4U;

		// Token: 0x04001751 RID: 5969
		internal const uint PKCS12_EXPORT_RESERVED_MASK = 4294901760U;

		// Token: 0x04001752 RID: 5970
		internal const uint RSA_CSP_PUBLICKEYBLOB = 19U;

		// Token: 0x04001753 RID: 5971
		internal const uint X509_MULTI_BYTE_UINT = 38U;

		// Token: 0x04001754 RID: 5972
		internal const uint X509_DSS_PUBLICKEY = 38U;

		// Token: 0x04001755 RID: 5973
		internal const uint X509_DSS_PARAMETERS = 39U;

		// Token: 0x04001756 RID: 5974
		internal const uint X509_DSS_SIGNATURE = 40U;

		// Token: 0x04001757 RID: 5975
		internal const uint X509_EXTENSIONS = 5U;

		// Token: 0x04001758 RID: 5976
		internal const uint X509_NAME_VALUE = 6U;

		// Token: 0x04001759 RID: 5977
		internal const uint X509_NAME = 7U;

		// Token: 0x0400175A RID: 5978
		internal const uint X509_AUTHORITY_KEY_ID = 9U;

		// Token: 0x0400175B RID: 5979
		internal const uint X509_KEY_USAGE_RESTRICTION = 11U;

		// Token: 0x0400175C RID: 5980
		internal const uint X509_BASIC_CONSTRAINTS = 13U;

		// Token: 0x0400175D RID: 5981
		internal const uint X509_KEY_USAGE = 14U;

		// Token: 0x0400175E RID: 5982
		internal const uint X509_BASIC_CONSTRAINTS2 = 15U;

		// Token: 0x0400175F RID: 5983
		internal const uint X509_CERT_POLICIES = 16U;

		// Token: 0x04001760 RID: 5984
		internal const uint PKCS_UTC_TIME = 17U;

		// Token: 0x04001761 RID: 5985
		internal const uint PKCS_ATTRIBUTE = 22U;

		// Token: 0x04001762 RID: 5986
		internal const uint X509_UNICODE_NAME_VALUE = 24U;

		// Token: 0x04001763 RID: 5987
		internal const uint X509_OCTET_STRING = 25U;

		// Token: 0x04001764 RID: 5988
		internal const uint X509_BITS = 26U;

		// Token: 0x04001765 RID: 5989
		internal const uint X509_ANY_STRING = 6U;

		// Token: 0x04001766 RID: 5990
		internal const uint X509_UNICODE_ANY_STRING = 24U;

		// Token: 0x04001767 RID: 5991
		internal const uint X509_ENHANCED_KEY_USAGE = 36U;

		// Token: 0x04001768 RID: 5992
		internal const uint PKCS_RC2_CBC_PARAMETERS = 41U;

		// Token: 0x04001769 RID: 5993
		internal const uint X509_CERTIFICATE_TEMPLATE = 64U;

		// Token: 0x0400176A RID: 5994
		internal const uint PKCS7_SIGNER_INFO = 500U;

		// Token: 0x0400176B RID: 5995
		internal const uint CMS_SIGNER_INFO = 501U;

		// Token: 0x0400176C RID: 5996
		internal const string szOID_COMMON_NAME = "2.5.4.3";

		// Token: 0x0400176D RID: 5997
		internal const string szOID_AUTHORITY_KEY_IDENTIFIER = "2.5.29.1";

		// Token: 0x0400176E RID: 5998
		internal const string szOID_KEY_USAGE_RESTRICTION = "2.5.29.4";

		// Token: 0x0400176F RID: 5999
		internal const string szOID_SUBJECT_ALT_NAME = "2.5.29.7";

		// Token: 0x04001770 RID: 6000
		internal const string szOID_ISSUER_ALT_NAME = "2.5.29.8";

		// Token: 0x04001771 RID: 6001
		internal const string szOID_BASIC_CONSTRAINTS = "2.5.29.10";

		// Token: 0x04001772 RID: 6002
		internal const string szOID_SUBJECT_KEY_IDENTIFIER = "2.5.29.14";

		// Token: 0x04001773 RID: 6003
		internal const string szOID_KEY_USAGE = "2.5.29.15";

		// Token: 0x04001774 RID: 6004
		internal const string szOID_SUBJECT_ALT_NAME2 = "2.5.29.17";

		// Token: 0x04001775 RID: 6005
		internal const string szOID_ISSUER_ALT_NAME2 = "2.5.29.18";

		// Token: 0x04001776 RID: 6006
		internal const string szOID_BASIC_CONSTRAINTS2 = "2.5.29.19";

		// Token: 0x04001777 RID: 6007
		internal const string szOID_CRL_DIST_POINTS = "2.5.29.31";

		// Token: 0x04001778 RID: 6008
		internal const string szOID_CERT_POLICIES = "2.5.29.32";

		// Token: 0x04001779 RID: 6009
		internal const string szOID_ENHANCED_KEY_USAGE = "2.5.29.37";

		// Token: 0x0400177A RID: 6010
		internal const string szOID_KEYID_RDN = "1.3.6.1.4.1.311.10.7.1";

		// Token: 0x0400177B RID: 6011
		internal const string szOID_ENROLL_CERTTYPE_EXTENSION = "1.3.6.1.4.1.311.20.2";

		// Token: 0x0400177C RID: 6012
		internal const string szOID_NT_PRINCIPAL_NAME = "1.3.6.1.4.1.311.20.2.3";

		// Token: 0x0400177D RID: 6013
		internal const string szOID_CERTIFICATE_TEMPLATE = "1.3.6.1.4.1.311.21.7";

		// Token: 0x0400177E RID: 6014
		internal const string szOID_RDN_DUMMY_SIGNER = "1.3.6.1.4.1.311.21.9";

		// Token: 0x0400177F RID: 6015
		internal const string szOID_AUTHORITY_INFO_ACCESS = "1.3.6.1.5.5.7.1.1";

		// Token: 0x04001780 RID: 6016
		internal const uint CERT_CHAIN_POLICY_BASE = 1U;

		// Token: 0x04001781 RID: 6017
		internal const uint CERT_CHAIN_POLICY_AUTHENTICODE = 2U;

		// Token: 0x04001782 RID: 6018
		internal const uint CERT_CHAIN_POLICY_AUTHENTICODE_TS = 3U;

		// Token: 0x04001783 RID: 6019
		internal const uint CERT_CHAIN_POLICY_SSL = 4U;

		// Token: 0x04001784 RID: 6020
		internal const uint CERT_CHAIN_POLICY_BASIC_CONSTRAINTS = 5U;

		// Token: 0x04001785 RID: 6021
		internal const uint CERT_CHAIN_POLICY_NT_AUTH = 6U;

		// Token: 0x04001786 RID: 6022
		internal const uint CERT_CHAIN_POLICY_MICROSOFT_ROOT = 7U;

		// Token: 0x04001787 RID: 6023
		internal const uint USAGE_MATCH_TYPE_AND = 0U;

		// Token: 0x04001788 RID: 6024
		internal const uint USAGE_MATCH_TYPE_OR = 1U;

		// Token: 0x04001789 RID: 6025
		internal const uint CERT_CHAIN_REVOCATION_CHECK_END_CERT = 268435456U;

		// Token: 0x0400178A RID: 6026
		internal const uint CERT_CHAIN_REVOCATION_CHECK_CHAIN = 536870912U;

		// Token: 0x0400178B RID: 6027
		internal const uint CERT_CHAIN_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT = 1073741824U;

		// Token: 0x0400178C RID: 6028
		internal const uint CERT_CHAIN_REVOCATION_CHECK_CACHE_ONLY = 2147483648U;

		// Token: 0x0400178D RID: 6029
		internal const uint CERT_CHAIN_REVOCATION_ACCUMULATIVE_TIMEOUT = 134217728U;

		// Token: 0x0400178E RID: 6030
		internal const uint CERT_TRUST_NO_ERROR = 0U;

		// Token: 0x0400178F RID: 6031
		internal const uint CERT_TRUST_IS_NOT_TIME_VALID = 1U;

		// Token: 0x04001790 RID: 6032
		internal const uint CERT_TRUST_IS_NOT_TIME_NESTED = 2U;

		// Token: 0x04001791 RID: 6033
		internal const uint CERT_TRUST_IS_REVOKED = 4U;

		// Token: 0x04001792 RID: 6034
		internal const uint CERT_TRUST_IS_NOT_SIGNATURE_VALID = 8U;

		// Token: 0x04001793 RID: 6035
		internal const uint CERT_TRUST_IS_NOT_VALID_FOR_USAGE = 16U;

		// Token: 0x04001794 RID: 6036
		internal const uint CERT_TRUST_IS_UNTRUSTED_ROOT = 32U;

		// Token: 0x04001795 RID: 6037
		internal const uint CERT_TRUST_REVOCATION_STATUS_UNKNOWN = 64U;

		// Token: 0x04001796 RID: 6038
		internal const uint CERT_TRUST_IS_CYCLIC = 128U;

		// Token: 0x04001797 RID: 6039
		internal const uint CERT_TRUST_INVALID_EXTENSION = 256U;

		// Token: 0x04001798 RID: 6040
		internal const uint CERT_TRUST_INVALID_POLICY_CONSTRAINTS = 512U;

		// Token: 0x04001799 RID: 6041
		internal const uint CERT_TRUST_INVALID_BASIC_CONSTRAINTS = 1024U;

		// Token: 0x0400179A RID: 6042
		internal const uint CERT_TRUST_INVALID_NAME_CONSTRAINTS = 2048U;

		// Token: 0x0400179B RID: 6043
		internal const uint CERT_TRUST_HAS_NOT_SUPPORTED_NAME_CONSTRAINT = 4096U;

		// Token: 0x0400179C RID: 6044
		internal const uint CERT_TRUST_HAS_NOT_DEFINED_NAME_CONSTRAINT = 8192U;

		// Token: 0x0400179D RID: 6045
		internal const uint CERT_TRUST_HAS_NOT_PERMITTED_NAME_CONSTRAINT = 16384U;

		// Token: 0x0400179E RID: 6046
		internal const uint CERT_TRUST_HAS_EXCLUDED_NAME_CONSTRAINT = 32768U;

		// Token: 0x0400179F RID: 6047
		internal const uint CERT_TRUST_IS_OFFLINE_REVOCATION = 16777216U;

		// Token: 0x040017A0 RID: 6048
		internal const uint CERT_TRUST_NO_ISSUANCE_CHAIN_POLICY = 33554432U;

		// Token: 0x040017A1 RID: 6049
		internal const uint CERT_TRUST_IS_PARTIAL_CHAIN = 65536U;

		// Token: 0x040017A2 RID: 6050
		internal const uint CERT_TRUST_CTL_IS_NOT_TIME_VALID = 131072U;

		// Token: 0x040017A3 RID: 6051
		internal const uint CERT_TRUST_CTL_IS_NOT_SIGNATURE_VALID = 262144U;

		// Token: 0x040017A4 RID: 6052
		internal const uint CERT_TRUST_CTL_IS_NOT_VALID_FOR_USAGE = 524288U;

		// Token: 0x040017A5 RID: 6053
		internal const uint CERT_CHAIN_POLICY_IGNORE_NOT_TIME_VALID_FLAG = 1U;

		// Token: 0x040017A6 RID: 6054
		internal const uint CERT_CHAIN_POLICY_IGNORE_CTL_NOT_TIME_VALID_FLAG = 2U;

		// Token: 0x040017A7 RID: 6055
		internal const uint CERT_CHAIN_POLICY_IGNORE_NOT_TIME_NESTED_FLAG = 4U;

		// Token: 0x040017A8 RID: 6056
		internal const uint CERT_CHAIN_POLICY_IGNORE_INVALID_BASIC_CONSTRAINTS_FLAG = 8U;

		// Token: 0x040017A9 RID: 6057
		internal const uint CERT_CHAIN_POLICY_ALLOW_UNKNOWN_CA_FLAG = 16U;

		// Token: 0x040017AA RID: 6058
		internal const uint CERT_CHAIN_POLICY_IGNORE_WRONG_USAGE_FLAG = 32U;

		// Token: 0x040017AB RID: 6059
		internal const uint CERT_CHAIN_POLICY_IGNORE_INVALID_NAME_FLAG = 64U;

		// Token: 0x040017AC RID: 6060
		internal const uint CERT_CHAIN_POLICY_IGNORE_INVALID_POLICY_FLAG = 128U;

		// Token: 0x040017AD RID: 6061
		internal const uint CERT_CHAIN_POLICY_IGNORE_END_REV_UNKNOWN_FLAG = 256U;

		// Token: 0x040017AE RID: 6062
		internal const uint CERT_CHAIN_POLICY_IGNORE_CTL_SIGNER_REV_UNKNOWN_FLAG = 512U;

		// Token: 0x040017AF RID: 6063
		internal const uint CERT_CHAIN_POLICY_IGNORE_CA_REV_UNKNOWN_FLAG = 1024U;

		// Token: 0x040017B0 RID: 6064
		internal const uint CERT_CHAIN_POLICY_IGNORE_ROOT_REV_UNKNOWN_FLAG = 2048U;

		// Token: 0x040017B1 RID: 6065
		internal const uint CERT_CHAIN_POLICY_IGNORE_ALL_REV_UNKNOWN_FLAGS = 3840U;

		// Token: 0x040017B2 RID: 6066
		internal const uint CERT_TRUST_HAS_EXACT_MATCH_ISSUER = 1U;

		// Token: 0x040017B3 RID: 6067
		internal const uint CERT_TRUST_HAS_KEY_MATCH_ISSUER = 2U;

		// Token: 0x040017B4 RID: 6068
		internal const uint CERT_TRUST_HAS_NAME_MATCH_ISSUER = 4U;

		// Token: 0x040017B5 RID: 6069
		internal const uint CERT_TRUST_IS_SELF_SIGNED = 8U;

		// Token: 0x040017B6 RID: 6070
		internal const uint CERT_TRUST_HAS_PREFERRED_ISSUER = 256U;

		// Token: 0x040017B7 RID: 6071
		internal const uint CERT_TRUST_HAS_ISSUANCE_CHAIN_POLICY = 512U;

		// Token: 0x040017B8 RID: 6072
		internal const uint CERT_TRUST_HAS_VALID_NAME_CONSTRAINTS = 1024U;

		// Token: 0x040017B9 RID: 6073
		internal const uint CERT_TRUST_IS_COMPLEX_CHAIN = 65536U;

		// Token: 0x040017BA RID: 6074
		internal const string szOID_PKIX_NO_SIGNATURE = "1.3.6.1.5.5.7.6.2";

		// Token: 0x040017BB RID: 6075
		internal const string szOID_PKIX_KP_SERVER_AUTH = "1.3.6.1.5.5.7.3.1";

		// Token: 0x040017BC RID: 6076
		internal const string szOID_PKIX_KP_CLIENT_AUTH = "1.3.6.1.5.5.7.3.2";

		// Token: 0x040017BD RID: 6077
		internal const string szOID_PKIX_KP_CODE_SIGNING = "1.3.6.1.5.5.7.3.3";

		// Token: 0x040017BE RID: 6078
		internal const string szOID_PKIX_KP_EMAIL_PROTECTION = "1.3.6.1.5.5.7.3.4";

		// Token: 0x040017BF RID: 6079
		internal const string SPC_INDIVIDUAL_SP_KEY_PURPOSE_OBJID = "1.3.6.1.4.1.311.2.1.21";

		// Token: 0x040017C0 RID: 6080
		internal const string SPC_COMMERCIAL_SP_KEY_PURPOSE_OBJID = "1.3.6.1.4.1.311.2.1.22";

		// Token: 0x040017C1 RID: 6081
		internal const uint HCCE_CURRENT_USER = 0U;

		// Token: 0x040017C2 RID: 6082
		internal const uint HCCE_LOCAL_MACHINE = 1U;

		// Token: 0x040017C3 RID: 6083
		internal const string szOID_PKCS_1 = "1.2.840.113549.1.1";

		// Token: 0x040017C4 RID: 6084
		internal const string szOID_PKCS_2 = "1.2.840.113549.1.2";

		// Token: 0x040017C5 RID: 6085
		internal const string szOID_PKCS_3 = "1.2.840.113549.1.3";

		// Token: 0x040017C6 RID: 6086
		internal const string szOID_PKCS_4 = "1.2.840.113549.1.4";

		// Token: 0x040017C7 RID: 6087
		internal const string szOID_PKCS_5 = "1.2.840.113549.1.5";

		// Token: 0x040017C8 RID: 6088
		internal const string szOID_PKCS_6 = "1.2.840.113549.1.6";

		// Token: 0x040017C9 RID: 6089
		internal const string szOID_PKCS_7 = "1.2.840.113549.1.7";

		// Token: 0x040017CA RID: 6090
		internal const string szOID_PKCS_8 = "1.2.840.113549.1.8";

		// Token: 0x040017CB RID: 6091
		internal const string szOID_PKCS_9 = "1.2.840.113549.1.9";

		// Token: 0x040017CC RID: 6092
		internal const string szOID_PKCS_10 = "1.2.840.113549.1.10";

		// Token: 0x040017CD RID: 6093
		internal const string szOID_PKCS_12 = "1.2.840.113549.1.12";

		// Token: 0x040017CE RID: 6094
		internal const string szOID_RSA_data = "1.2.840.113549.1.7.1";

		// Token: 0x040017CF RID: 6095
		internal const string szOID_RSA_signedData = "1.2.840.113549.1.7.2";

		// Token: 0x040017D0 RID: 6096
		internal const string szOID_RSA_envelopedData = "1.2.840.113549.1.7.3";

		// Token: 0x040017D1 RID: 6097
		internal const string szOID_RSA_signEnvData = "1.2.840.113549.1.7.4";

		// Token: 0x040017D2 RID: 6098
		internal const string szOID_RSA_digestedData = "1.2.840.113549.1.7.5";

		// Token: 0x040017D3 RID: 6099
		internal const string szOID_RSA_hashedData = "1.2.840.113549.1.7.5";

		// Token: 0x040017D4 RID: 6100
		internal const string szOID_RSA_encryptedData = "1.2.840.113549.1.7.6";

		// Token: 0x040017D5 RID: 6101
		internal const string szOID_RSA_emailAddr = "1.2.840.113549.1.9.1";

		// Token: 0x040017D6 RID: 6102
		internal const string szOID_RSA_unstructName = "1.2.840.113549.1.9.2";

		// Token: 0x040017D7 RID: 6103
		internal const string szOID_RSA_contentType = "1.2.840.113549.1.9.3";

		// Token: 0x040017D8 RID: 6104
		internal const string szOID_RSA_messageDigest = "1.2.840.113549.1.9.4";

		// Token: 0x040017D9 RID: 6105
		internal const string szOID_RSA_signingTime = "1.2.840.113549.1.9.5";

		// Token: 0x040017DA RID: 6106
		internal const string szOID_RSA_counterSign = "1.2.840.113549.1.9.6";

		// Token: 0x040017DB RID: 6107
		internal const string szOID_RSA_challengePwd = "1.2.840.113549.1.9.7";

		// Token: 0x040017DC RID: 6108
		internal const string szOID_RSA_unstructAddr = "1.2.840.113549.1.9.8";

		// Token: 0x040017DD RID: 6109
		internal const string szOID_RSA_extCertAttrs = "1.2.840.113549.1.9.9";

		// Token: 0x040017DE RID: 6110
		internal const string szOID_RSA_SMIMECapabilities = "1.2.840.113549.1.9.15";

		// Token: 0x040017DF RID: 6111
		internal const string szOID_CAPICOM = "1.3.6.1.4.1.311.88";

		// Token: 0x040017E0 RID: 6112
		internal const string szOID_CAPICOM_version = "1.3.6.1.4.1.311.88.1";

		// Token: 0x040017E1 RID: 6113
		internal const string szOID_CAPICOM_attribute = "1.3.6.1.4.1.311.88.2";

		// Token: 0x040017E2 RID: 6114
		internal const string szOID_CAPICOM_documentName = "1.3.6.1.4.1.311.88.2.1";

		// Token: 0x040017E3 RID: 6115
		internal const string szOID_CAPICOM_documentDescription = "1.3.6.1.4.1.311.88.2.2";

		// Token: 0x040017E4 RID: 6116
		internal const string szOID_CAPICOM_encryptedData = "1.3.6.1.4.1.311.88.3";

		// Token: 0x040017E5 RID: 6117
		internal const string szOID_CAPICOM_encryptedContent = "1.3.6.1.4.1.311.88.3.1";

		// Token: 0x040017E6 RID: 6118
		internal const string szOID_OIWSEC_sha1 = "1.3.14.3.2.26";

		// Token: 0x040017E7 RID: 6119
		internal const string szOID_RSA_MD5 = "1.2.840.113549.2.5";

		// Token: 0x040017E8 RID: 6120
		internal const string szOID_OIWSEC_SHA256 = "2.16.840.1.101.3.4.1";

		// Token: 0x040017E9 RID: 6121
		internal const string szOID_OIWSEC_SHA384 = "2.16.840.1.101.3.4.2";

		// Token: 0x040017EA RID: 6122
		internal const string szOID_OIWSEC_SHA512 = "2.16.840.1.101.3.4.3";

		// Token: 0x040017EB RID: 6123
		internal const string szOID_RSA_RC2CBC = "1.2.840.113549.3.2";

		// Token: 0x040017EC RID: 6124
		internal const string szOID_RSA_RC4 = "1.2.840.113549.3.4";

		// Token: 0x040017ED RID: 6125
		internal const string szOID_RSA_DES_EDE3_CBC = "1.2.840.113549.3.7";

		// Token: 0x040017EE RID: 6126
		internal const string szOID_OIWSEC_desCBC = "1.3.14.3.2.7";

		// Token: 0x040017EF RID: 6127
		internal const string szOID_RSA_SMIMEalg = "1.2.840.113549.1.9.16.3";

		// Token: 0x040017F0 RID: 6128
		internal const string szOID_RSA_SMIMEalgESDH = "1.2.840.113549.1.9.16.3.5";

		// Token: 0x040017F1 RID: 6129
		internal const string szOID_RSA_SMIMEalgCMS3DESwrap = "1.2.840.113549.1.9.16.3.6";

		// Token: 0x040017F2 RID: 6130
		internal const string szOID_RSA_SMIMEalgCMSRC2wrap = "1.2.840.113549.1.9.16.3.7";

		// Token: 0x040017F3 RID: 6131
		internal const string szOID_X957_DSA = "1.2.840.10040.4.1";

		// Token: 0x040017F4 RID: 6132
		internal const string szOID_X957_sha1DSA = "1.2.840.10040.4.3";

		// Token: 0x040017F5 RID: 6133
		internal const string szOID_OIWSEC_sha1RSASign = "1.3.14.3.2.29";

		// Token: 0x040017F6 RID: 6134
		internal const uint CERT_ALT_NAME_OTHER_NAME = 1U;

		// Token: 0x040017F7 RID: 6135
		internal const uint CERT_ALT_NAME_RFC822_NAME = 2U;

		// Token: 0x040017F8 RID: 6136
		internal const uint CERT_ALT_NAME_DNS_NAME = 3U;

		// Token: 0x040017F9 RID: 6137
		internal const uint CERT_ALT_NAME_X400_ADDRESS = 4U;

		// Token: 0x040017FA RID: 6138
		internal const uint CERT_ALT_NAME_DIRECTORY_NAME = 5U;

		// Token: 0x040017FB RID: 6139
		internal const uint CERT_ALT_NAME_EDI_PARTY_NAME = 6U;

		// Token: 0x040017FC RID: 6140
		internal const uint CERT_ALT_NAME_URL = 7U;

		// Token: 0x040017FD RID: 6141
		internal const uint CERT_ALT_NAME_IP_ADDRESS = 8U;

		// Token: 0x040017FE RID: 6142
		internal const uint CERT_ALT_NAME_REGISTERED_ID = 9U;

		// Token: 0x040017FF RID: 6143
		internal const uint CERT_RDN_ANY_TYPE = 0U;

		// Token: 0x04001800 RID: 6144
		internal const uint CERT_RDN_ENCODED_BLOB = 1U;

		// Token: 0x04001801 RID: 6145
		internal const uint CERT_RDN_OCTET_STRING = 2U;

		// Token: 0x04001802 RID: 6146
		internal const uint CERT_RDN_NUMERIC_STRING = 3U;

		// Token: 0x04001803 RID: 6147
		internal const uint CERT_RDN_PRINTABLE_STRING = 4U;

		// Token: 0x04001804 RID: 6148
		internal const uint CERT_RDN_TELETEX_STRING = 5U;

		// Token: 0x04001805 RID: 6149
		internal const uint CERT_RDN_T61_STRING = 5U;

		// Token: 0x04001806 RID: 6150
		internal const uint CERT_RDN_VIDEOTEX_STRING = 6U;

		// Token: 0x04001807 RID: 6151
		internal const uint CERT_RDN_IA5_STRING = 7U;

		// Token: 0x04001808 RID: 6152
		internal const uint CERT_RDN_GRAPHIC_STRING = 8U;

		// Token: 0x04001809 RID: 6153
		internal const uint CERT_RDN_VISIBLE_STRING = 9U;

		// Token: 0x0400180A RID: 6154
		internal const uint CERT_RDN_ISO646_STRING = 9U;

		// Token: 0x0400180B RID: 6155
		internal const uint CERT_RDN_GENERAL_STRING = 10U;

		// Token: 0x0400180C RID: 6156
		internal const uint CERT_RDN_UNIVERSAL_STRING = 11U;

		// Token: 0x0400180D RID: 6157
		internal const uint CERT_RDN_INT4_STRING = 11U;

		// Token: 0x0400180E RID: 6158
		internal const uint CERT_RDN_BMP_STRING = 12U;

		// Token: 0x0400180F RID: 6159
		internal const uint CERT_RDN_UNICODE_STRING = 12U;

		// Token: 0x04001810 RID: 6160
		internal const uint CERT_RDN_UTF8_STRING = 13U;

		// Token: 0x04001811 RID: 6161
		internal const uint CERT_RDN_TYPE_MASK = 255U;

		// Token: 0x04001812 RID: 6162
		internal const uint CERT_RDN_FLAGS_MASK = 4278190080U;

		// Token: 0x04001813 RID: 6163
		internal const uint CERT_STORE_CTRL_RESYNC = 1U;

		// Token: 0x04001814 RID: 6164
		internal const uint CERT_STORE_CTRL_NOTIFY_CHANGE = 2U;

		// Token: 0x04001815 RID: 6165
		internal const uint CERT_STORE_CTRL_COMMIT = 3U;

		// Token: 0x04001816 RID: 6166
		internal const uint CERT_STORE_CTRL_AUTO_RESYNC = 4U;

		// Token: 0x04001817 RID: 6167
		internal const uint CERT_STORE_CTRL_CANCEL_NOTIFY = 5U;

		// Token: 0x04001818 RID: 6168
		internal const uint CERT_ID_ISSUER_SERIAL_NUMBER = 1U;

		// Token: 0x04001819 RID: 6169
		internal const uint CERT_ID_KEY_IDENTIFIER = 2U;

		// Token: 0x0400181A RID: 6170
		internal const uint CERT_ID_SHA1_HASH = 3U;

		// Token: 0x0400181B RID: 6171
		internal const string MS_ENHANCED_PROV = "Microsoft Enhanced Cryptographic Provider v1.0";

		// Token: 0x0400181C RID: 6172
		internal const string MS_STRONG_PROV = "Microsoft Strong Cryptographic Provider";

		// Token: 0x0400181D RID: 6173
		internal const string MS_DEF_PROV = "Microsoft Base Cryptographic Provider v1.0";

		// Token: 0x0400181E RID: 6174
		internal const string MS_DEF_DSS_DH_PROV = "Microsoft Base DSS and Diffie-Hellman Cryptographic Provider";

		// Token: 0x0400181F RID: 6175
		internal const string MS_ENH_DSS_DH_PROV = "Microsoft Enhanced DSS and Diffie-Hellman Cryptographic Provider";

		// Token: 0x04001820 RID: 6176
		internal const string DummySignerCommonName = "CN=Dummy Signer";

		// Token: 0x04001821 RID: 6177
		internal const uint PROV_RSA_FULL = 1U;

		// Token: 0x04001822 RID: 6178
		internal const uint PROV_DSS_DH = 13U;

		// Token: 0x04001823 RID: 6179
		internal const uint ALG_TYPE_ANY = 0U;

		// Token: 0x04001824 RID: 6180
		internal const uint ALG_TYPE_DSS = 512U;

		// Token: 0x04001825 RID: 6181
		internal const uint ALG_TYPE_RSA = 1024U;

		// Token: 0x04001826 RID: 6182
		internal const uint ALG_TYPE_BLOCK = 1536U;

		// Token: 0x04001827 RID: 6183
		internal const uint ALG_TYPE_STREAM = 2048U;

		// Token: 0x04001828 RID: 6184
		internal const uint ALG_TYPE_DH = 2560U;

		// Token: 0x04001829 RID: 6185
		internal const uint ALG_TYPE_SECURECHANNEL = 3072U;

		// Token: 0x0400182A RID: 6186
		internal const uint ALG_CLASS_ANY = 0U;

		// Token: 0x0400182B RID: 6187
		internal const uint ALG_CLASS_SIGNATURE = 8192U;

		// Token: 0x0400182C RID: 6188
		internal const uint ALG_CLASS_MSG_ENCRYPT = 16384U;

		// Token: 0x0400182D RID: 6189
		internal const uint ALG_CLASS_DATA_ENCRYPT = 24576U;

		// Token: 0x0400182E RID: 6190
		internal const uint ALG_CLASS_HASH = 32768U;

		// Token: 0x0400182F RID: 6191
		internal const uint ALG_CLASS_KEY_EXCHANGE = 40960U;

		// Token: 0x04001830 RID: 6192
		internal const uint ALG_CLASS_ALL = 57344U;

		// Token: 0x04001831 RID: 6193
		internal const uint ALG_SID_ANY = 0U;

		// Token: 0x04001832 RID: 6194
		internal const uint ALG_SID_RSA_ANY = 0U;

		// Token: 0x04001833 RID: 6195
		internal const uint ALG_SID_RSA_PKCS = 1U;

		// Token: 0x04001834 RID: 6196
		internal const uint ALG_SID_RSA_MSATWORK = 2U;

		// Token: 0x04001835 RID: 6197
		internal const uint ALG_SID_RSA_ENTRUST = 3U;

		// Token: 0x04001836 RID: 6198
		internal const uint ALG_SID_RSA_PGP = 4U;

		// Token: 0x04001837 RID: 6199
		internal const uint ALG_SID_DSS_ANY = 0U;

		// Token: 0x04001838 RID: 6200
		internal const uint ALG_SID_DSS_PKCS = 1U;

		// Token: 0x04001839 RID: 6201
		internal const uint ALG_SID_DSS_DMS = 2U;

		// Token: 0x0400183A RID: 6202
		internal const uint ALG_SID_DES = 1U;

		// Token: 0x0400183B RID: 6203
		internal const uint ALG_SID_3DES = 3U;

		// Token: 0x0400183C RID: 6204
		internal const uint ALG_SID_DESX = 4U;

		// Token: 0x0400183D RID: 6205
		internal const uint ALG_SID_IDEA = 5U;

		// Token: 0x0400183E RID: 6206
		internal const uint ALG_SID_CAST = 6U;

		// Token: 0x0400183F RID: 6207
		internal const uint ALG_SID_SAFERSK64 = 7U;

		// Token: 0x04001840 RID: 6208
		internal const uint ALG_SID_SAFERSK128 = 8U;

		// Token: 0x04001841 RID: 6209
		internal const uint ALG_SID_3DES_112 = 9U;

		// Token: 0x04001842 RID: 6210
		internal const uint ALG_SID_CYLINK_MEK = 12U;

		// Token: 0x04001843 RID: 6211
		internal const uint ALG_SID_RC5 = 13U;

		// Token: 0x04001844 RID: 6212
		internal const uint ALG_SID_AES_128 = 14U;

		// Token: 0x04001845 RID: 6213
		internal const uint ALG_SID_AES_192 = 15U;

		// Token: 0x04001846 RID: 6214
		internal const uint ALG_SID_AES_256 = 16U;

		// Token: 0x04001847 RID: 6215
		internal const uint ALG_SID_AES = 17U;

		// Token: 0x04001848 RID: 6216
		internal const uint ALG_SID_SKIPJACK = 10U;

		// Token: 0x04001849 RID: 6217
		internal const uint ALG_SID_TEK = 11U;

		// Token: 0x0400184A RID: 6218
		internal const uint ALG_SID_RC2 = 2U;

		// Token: 0x0400184B RID: 6219
		internal const uint ALG_SID_RC4 = 1U;

		// Token: 0x0400184C RID: 6220
		internal const uint ALG_SID_SEAL = 2U;

		// Token: 0x0400184D RID: 6221
		internal const uint ALG_SID_DH_SANDF = 1U;

		// Token: 0x0400184E RID: 6222
		internal const uint ALG_SID_DH_EPHEM = 2U;

		// Token: 0x0400184F RID: 6223
		internal const uint ALG_SID_AGREED_KEY_ANY = 3U;

		// Token: 0x04001850 RID: 6224
		internal const uint ALG_SID_KEA = 4U;

		// Token: 0x04001851 RID: 6225
		internal const uint ALG_SID_MD2 = 1U;

		// Token: 0x04001852 RID: 6226
		internal const uint ALG_SID_MD4 = 2U;

		// Token: 0x04001853 RID: 6227
		internal const uint ALG_SID_MD5 = 3U;

		// Token: 0x04001854 RID: 6228
		internal const uint ALG_SID_SHA = 4U;

		// Token: 0x04001855 RID: 6229
		internal const uint ALG_SID_SHA1 = 4U;

		// Token: 0x04001856 RID: 6230
		internal const uint ALG_SID_MAC = 5U;

		// Token: 0x04001857 RID: 6231
		internal const uint ALG_SID_RIPEMD = 6U;

		// Token: 0x04001858 RID: 6232
		internal const uint ALG_SID_RIPEMD160 = 7U;

		// Token: 0x04001859 RID: 6233
		internal const uint ALG_SID_SSL3SHAMD5 = 8U;

		// Token: 0x0400185A RID: 6234
		internal const uint ALG_SID_HMAC = 9U;

		// Token: 0x0400185B RID: 6235
		internal const uint ALG_SID_TLS1PRF = 10U;

		// Token: 0x0400185C RID: 6236
		internal const uint ALG_SID_HASH_REPLACE_OWF = 11U;

		// Token: 0x0400185D RID: 6237
		internal const uint ALG_SID_SSL3_MASTER = 1U;

		// Token: 0x0400185E RID: 6238
		internal const uint ALG_SID_SCHANNEL_MASTER_HASH = 2U;

		// Token: 0x0400185F RID: 6239
		internal const uint ALG_SID_SCHANNEL_MAC_KEY = 3U;

		// Token: 0x04001860 RID: 6240
		internal const uint ALG_SID_PCT1_MASTER = 4U;

		// Token: 0x04001861 RID: 6241
		internal const uint ALG_SID_SSL2_MASTER = 5U;

		// Token: 0x04001862 RID: 6242
		internal const uint ALG_SID_TLS1_MASTER = 6U;

		// Token: 0x04001863 RID: 6243
		internal const uint ALG_SID_SCHANNEL_ENC_KEY = 7U;

		// Token: 0x04001864 RID: 6244
		internal const uint CALG_MD2 = 32769U;

		// Token: 0x04001865 RID: 6245
		internal const uint CALG_MD4 = 32770U;

		// Token: 0x04001866 RID: 6246
		internal const uint CALG_MD5 = 32771U;

		// Token: 0x04001867 RID: 6247
		internal const uint CALG_SHA = 32772U;

		// Token: 0x04001868 RID: 6248
		internal const uint CALG_SHA1 = 32772U;

		// Token: 0x04001869 RID: 6249
		internal const uint CALG_MAC = 32773U;

		// Token: 0x0400186A RID: 6250
		internal const uint CALG_RSA_SIGN = 9216U;

		// Token: 0x0400186B RID: 6251
		internal const uint CALG_DSS_SIGN = 8704U;

		// Token: 0x0400186C RID: 6252
		internal const uint CALG_NO_SIGN = 8192U;

		// Token: 0x0400186D RID: 6253
		internal const uint CALG_RSA_KEYX = 41984U;

		// Token: 0x0400186E RID: 6254
		internal const uint CALG_DES = 26113U;

		// Token: 0x0400186F RID: 6255
		internal const uint CALG_3DES_112 = 26121U;

		// Token: 0x04001870 RID: 6256
		internal const uint CALG_3DES = 26115U;

		// Token: 0x04001871 RID: 6257
		internal const uint CALG_DESX = 26116U;

		// Token: 0x04001872 RID: 6258
		internal const uint CALG_RC2 = 26114U;

		// Token: 0x04001873 RID: 6259
		internal const uint CALG_RC4 = 26625U;

		// Token: 0x04001874 RID: 6260
		internal const uint CALG_SEAL = 26626U;

		// Token: 0x04001875 RID: 6261
		internal const uint CALG_DH_SF = 43521U;

		// Token: 0x04001876 RID: 6262
		internal const uint CALG_DH_EPHEM = 43522U;

		// Token: 0x04001877 RID: 6263
		internal const uint CALG_AGREEDKEY_ANY = 43523U;

		// Token: 0x04001878 RID: 6264
		internal const uint CALG_KEA_KEYX = 43524U;

		// Token: 0x04001879 RID: 6265
		internal const uint CALG_HUGHES_MD5 = 40963U;

		// Token: 0x0400187A RID: 6266
		internal const uint CALG_SKIPJACK = 26122U;

		// Token: 0x0400187B RID: 6267
		internal const uint CALG_TEK = 26123U;

		// Token: 0x0400187C RID: 6268
		internal const uint CALG_CYLINK_MEK = 26124U;

		// Token: 0x0400187D RID: 6269
		internal const uint CALG_SSL3_SHAMD5 = 32776U;

		// Token: 0x0400187E RID: 6270
		internal const uint CALG_SSL3_MASTER = 19457U;

		// Token: 0x0400187F RID: 6271
		internal const uint CALG_SCHANNEL_MASTER_HASH = 19458U;

		// Token: 0x04001880 RID: 6272
		internal const uint CALG_SCHANNEL_MAC_KEY = 19459U;

		// Token: 0x04001881 RID: 6273
		internal const uint CALG_SCHANNEL_ENC_KEY = 19463U;

		// Token: 0x04001882 RID: 6274
		internal const uint CALG_PCT1_MASTER = 19460U;

		// Token: 0x04001883 RID: 6275
		internal const uint CALG_SSL2_MASTER = 19461U;

		// Token: 0x04001884 RID: 6276
		internal const uint CALG_TLS1_MASTER = 19462U;

		// Token: 0x04001885 RID: 6277
		internal const uint CALG_RC5 = 26125U;

		// Token: 0x04001886 RID: 6278
		internal const uint CALG_HMAC = 32777U;

		// Token: 0x04001887 RID: 6279
		internal const uint CALG_TLS1PRF = 32778U;

		// Token: 0x04001888 RID: 6280
		internal const uint CALG_HASH_REPLACE_OWF = 32779U;

		// Token: 0x04001889 RID: 6281
		internal const uint CALG_AES_128 = 26126U;

		// Token: 0x0400188A RID: 6282
		internal const uint CALG_AES_192 = 26127U;

		// Token: 0x0400188B RID: 6283
		internal const uint CALG_AES_256 = 26128U;

		// Token: 0x0400188C RID: 6284
		internal const uint CALG_AES = 26129U;

		// Token: 0x0400188D RID: 6285
		internal const uint CRYPT_FIRST = 1U;

		// Token: 0x0400188E RID: 6286
		internal const uint CRYPT_NEXT = 2U;

		// Token: 0x0400188F RID: 6287
		internal const uint PP_ENUMALGS_EX = 22U;

		// Token: 0x04001890 RID: 6288
		internal const uint CRYPT_VERIFYCONTEXT = 4026531840U;

		// Token: 0x04001891 RID: 6289
		internal const uint CRYPT_NEWKEYSET = 8U;

		// Token: 0x04001892 RID: 6290
		internal const uint CRYPT_DELETEKEYSET = 16U;

		// Token: 0x04001893 RID: 6291
		internal const uint CRYPT_MACHINE_KEYSET = 32U;

		// Token: 0x04001894 RID: 6292
		internal const uint CRYPT_SILENT = 64U;

		// Token: 0x04001895 RID: 6293
		internal const uint CRYPT_USER_KEYSET = 4096U;

		// Token: 0x04001896 RID: 6294
		internal const uint CRYPT_EXPORTABLE = 1U;

		// Token: 0x04001897 RID: 6295
		internal const uint CRYPT_USER_PROTECTED = 2U;

		// Token: 0x04001898 RID: 6296
		internal const uint CRYPT_CREATE_SALT = 4U;

		// Token: 0x04001899 RID: 6297
		internal const uint CRYPT_UPDATE_KEY = 8U;

		// Token: 0x0400189A RID: 6298
		internal const uint CRYPT_NO_SALT = 16U;

		// Token: 0x0400189B RID: 6299
		internal const uint CRYPT_PREGEN = 64U;

		// Token: 0x0400189C RID: 6300
		internal const uint CRYPT_RECIPIENT = 16U;

		// Token: 0x0400189D RID: 6301
		internal const uint CRYPT_INITIATOR = 64U;

		// Token: 0x0400189E RID: 6302
		internal const uint CRYPT_ONLINE = 128U;

		// Token: 0x0400189F RID: 6303
		internal const uint CRYPT_SF = 256U;

		// Token: 0x040018A0 RID: 6304
		internal const uint CRYPT_CREATE_IV = 512U;

		// Token: 0x040018A1 RID: 6305
		internal const uint CRYPT_KEK = 1024U;

		// Token: 0x040018A2 RID: 6306
		internal const uint CRYPT_DATA_KEY = 2048U;

		// Token: 0x040018A3 RID: 6307
		internal const uint CRYPT_VOLATILE = 4096U;

		// Token: 0x040018A4 RID: 6308
		internal const uint CRYPT_SGCKEY = 8192U;

		// Token: 0x040018A5 RID: 6309
		internal const uint CRYPT_ARCHIVABLE = 16384U;

		// Token: 0x040018A6 RID: 6310
		internal const byte CUR_BLOB_VERSION = 2;

		// Token: 0x040018A7 RID: 6311
		internal const byte SIMPLEBLOB = 1;

		// Token: 0x040018A8 RID: 6312
		internal const byte PUBLICKEYBLOB = 6;

		// Token: 0x040018A9 RID: 6313
		internal const byte PRIVATEKEYBLOB = 7;

		// Token: 0x040018AA RID: 6314
		internal const byte PLAINTEXTKEYBLOB = 8;

		// Token: 0x040018AB RID: 6315
		internal const byte OPAQUEKEYBLOB = 9;

		// Token: 0x040018AC RID: 6316
		internal const byte PUBLICKEYBLOBEX = 10;

		// Token: 0x040018AD RID: 6317
		internal const byte SYMMETRICWRAPKEYBLOB = 11;

		// Token: 0x040018AE RID: 6318
		internal const uint DSS_MAGIC = 827544388U;

		// Token: 0x040018AF RID: 6319
		internal const uint DSS_PRIVATE_MAGIC = 844321604U;

		// Token: 0x040018B0 RID: 6320
		internal const uint DSS_PUB_MAGIC_VER3 = 861098820U;

		// Token: 0x040018B1 RID: 6321
		internal const uint DSS_PRIV_MAGIC_VER3 = 877876036U;

		// Token: 0x040018B2 RID: 6322
		internal const uint RSA_PUB_MAGIC = 826364754U;

		// Token: 0x040018B3 RID: 6323
		internal const uint RSA_PRIV_MAGIC = 843141970U;

		// Token: 0x040018B4 RID: 6324
		internal const uint CRYPT_ACQUIRE_CACHE_FLAG = 1U;

		// Token: 0x040018B5 RID: 6325
		internal const uint CRYPT_ACQUIRE_USE_PROV_INFO_FLAG = 2U;

		// Token: 0x040018B6 RID: 6326
		internal const uint CRYPT_ACQUIRE_COMPARE_KEY_FLAG = 4U;

		// Token: 0x040018B7 RID: 6327
		internal const uint CRYPT_ACQUIRE_SILENT_FLAG = 64U;

		// Token: 0x040018B8 RID: 6328
		internal const uint CMSG_BARE_CONTENT_FLAG = 1U;

		// Token: 0x040018B9 RID: 6329
		internal const uint CMSG_LENGTH_ONLY_FLAG = 2U;

		// Token: 0x040018BA RID: 6330
		internal const uint CMSG_DETACHED_FLAG = 4U;

		// Token: 0x040018BB RID: 6331
		internal const uint CMSG_AUTHENTICATED_ATTRIBUTES_FLAG = 8U;

		// Token: 0x040018BC RID: 6332
		internal const uint CMSG_CONTENTS_OCTETS_FLAG = 16U;

		// Token: 0x040018BD RID: 6333
		internal const uint CMSG_MAX_LENGTH_FLAG = 32U;

		// Token: 0x040018BE RID: 6334
		internal const uint CMSG_TYPE_PARAM = 1U;

		// Token: 0x040018BF RID: 6335
		internal const uint CMSG_CONTENT_PARAM = 2U;

		// Token: 0x040018C0 RID: 6336
		internal const uint CMSG_BARE_CONTENT_PARAM = 3U;

		// Token: 0x040018C1 RID: 6337
		internal const uint CMSG_INNER_CONTENT_TYPE_PARAM = 4U;

		// Token: 0x040018C2 RID: 6338
		internal const uint CMSG_SIGNER_COUNT_PARAM = 5U;

		// Token: 0x040018C3 RID: 6339
		internal const uint CMSG_SIGNER_INFO_PARAM = 6U;

		// Token: 0x040018C4 RID: 6340
		internal const uint CMSG_SIGNER_CERT_INFO_PARAM = 7U;

		// Token: 0x040018C5 RID: 6341
		internal const uint CMSG_SIGNER_HASH_ALGORITHM_PARAM = 8U;

		// Token: 0x040018C6 RID: 6342
		internal const uint CMSG_SIGNER_AUTH_ATTR_PARAM = 9U;

		// Token: 0x040018C7 RID: 6343
		internal const uint CMSG_SIGNER_UNAUTH_ATTR_PARAM = 10U;

		// Token: 0x040018C8 RID: 6344
		internal const uint CMSG_CERT_COUNT_PARAM = 11U;

		// Token: 0x040018C9 RID: 6345
		internal const uint CMSG_CERT_PARAM = 12U;

		// Token: 0x040018CA RID: 6346
		internal const uint CMSG_CRL_COUNT_PARAM = 13U;

		// Token: 0x040018CB RID: 6347
		internal const uint CMSG_CRL_PARAM = 14U;

		// Token: 0x040018CC RID: 6348
		internal const uint CMSG_ENVELOPE_ALGORITHM_PARAM = 15U;

		// Token: 0x040018CD RID: 6349
		internal const uint CMSG_RECIPIENT_COUNT_PARAM = 17U;

		// Token: 0x040018CE RID: 6350
		internal const uint CMSG_RECIPIENT_INDEX_PARAM = 18U;

		// Token: 0x040018CF RID: 6351
		internal const uint CMSG_RECIPIENT_INFO_PARAM = 19U;

		// Token: 0x040018D0 RID: 6352
		internal const uint CMSG_HASH_ALGORITHM_PARAM = 20U;

		// Token: 0x040018D1 RID: 6353
		internal const uint CMSG_HASH_DATA_PARAM = 21U;

		// Token: 0x040018D2 RID: 6354
		internal const uint CMSG_COMPUTED_HASH_PARAM = 22U;

		// Token: 0x040018D3 RID: 6355
		internal const uint CMSG_ENCRYPT_PARAM = 26U;

		// Token: 0x040018D4 RID: 6356
		internal const uint CMSG_ENCRYPTED_DIGEST = 27U;

		// Token: 0x040018D5 RID: 6357
		internal const uint CMSG_ENCODED_SIGNER = 28U;

		// Token: 0x040018D6 RID: 6358
		internal const uint CMSG_ENCODED_MESSAGE = 29U;

		// Token: 0x040018D7 RID: 6359
		internal const uint CMSG_VERSION_PARAM = 30U;

		// Token: 0x040018D8 RID: 6360
		internal const uint CMSG_ATTR_CERT_COUNT_PARAM = 31U;

		// Token: 0x040018D9 RID: 6361
		internal const uint CMSG_ATTR_CERT_PARAM = 32U;

		// Token: 0x040018DA RID: 6362
		internal const uint CMSG_CMS_RECIPIENT_COUNT_PARAM = 33U;

		// Token: 0x040018DB RID: 6363
		internal const uint CMSG_CMS_RECIPIENT_INDEX_PARAM = 34U;

		// Token: 0x040018DC RID: 6364
		internal const uint CMSG_CMS_RECIPIENT_ENCRYPTED_KEY_INDEX_PARAM = 35U;

		// Token: 0x040018DD RID: 6365
		internal const uint CMSG_CMS_RECIPIENT_INFO_PARAM = 36U;

		// Token: 0x040018DE RID: 6366
		internal const uint CMSG_UNPROTECTED_ATTR_PARAM = 37U;

		// Token: 0x040018DF RID: 6367
		internal const uint CMSG_SIGNER_CERT_ID_PARAM = 38U;

		// Token: 0x040018E0 RID: 6368
		internal const uint CMSG_CMS_SIGNER_INFO_PARAM = 39U;

		// Token: 0x040018E1 RID: 6369
		internal const uint CMSG_CTRL_VERIFY_SIGNATURE = 1U;

		// Token: 0x040018E2 RID: 6370
		internal const uint CMSG_CTRL_DECRYPT = 2U;

		// Token: 0x040018E3 RID: 6371
		internal const uint CMSG_CTRL_VERIFY_HASH = 5U;

		// Token: 0x040018E4 RID: 6372
		internal const uint CMSG_CTRL_ADD_SIGNER = 6U;

		// Token: 0x040018E5 RID: 6373
		internal const uint CMSG_CTRL_DEL_SIGNER = 7U;

		// Token: 0x040018E6 RID: 6374
		internal const uint CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR = 8U;

		// Token: 0x040018E7 RID: 6375
		internal const uint CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR = 9U;

		// Token: 0x040018E8 RID: 6376
		internal const uint CMSG_CTRL_ADD_CERT = 10U;

		// Token: 0x040018E9 RID: 6377
		internal const uint CMSG_CTRL_DEL_CERT = 11U;

		// Token: 0x040018EA RID: 6378
		internal const uint CMSG_CTRL_ADD_CRL = 12U;

		// Token: 0x040018EB RID: 6379
		internal const uint CMSG_CTRL_DEL_CRL = 13U;

		// Token: 0x040018EC RID: 6380
		internal const uint CMSG_CTRL_ADD_ATTR_CERT = 14U;

		// Token: 0x040018ED RID: 6381
		internal const uint CMSG_CTRL_DEL_ATTR_CERT = 15U;

		// Token: 0x040018EE RID: 6382
		internal const uint CMSG_CTRL_KEY_TRANS_DECRYPT = 16U;

		// Token: 0x040018EF RID: 6383
		internal const uint CMSG_CTRL_KEY_AGREE_DECRYPT = 17U;

		// Token: 0x040018F0 RID: 6384
		internal const uint CMSG_CTRL_MAIL_LIST_DECRYPT = 18U;

		// Token: 0x040018F1 RID: 6385
		internal const uint CMSG_CTRL_VERIFY_SIGNATURE_EX = 19U;

		// Token: 0x040018F2 RID: 6386
		internal const uint CMSG_CTRL_ADD_CMS_SIGNER_INFO = 20U;

		// Token: 0x040018F3 RID: 6387
		internal const uint CMSG_VERIFY_SIGNER_PUBKEY = 1U;

		// Token: 0x040018F4 RID: 6388
		internal const uint CMSG_VERIFY_SIGNER_CERT = 2U;

		// Token: 0x040018F5 RID: 6389
		internal const uint CMSG_VERIFY_SIGNER_CHAIN = 3U;

		// Token: 0x040018F6 RID: 6390
		internal const uint CMSG_VERIFY_SIGNER_NULL = 4U;

		// Token: 0x040018F7 RID: 6391
		internal const uint CMSG_DATA = 1U;

		// Token: 0x040018F8 RID: 6392
		internal const uint CMSG_SIGNED = 2U;

		// Token: 0x040018F9 RID: 6393
		internal const uint CMSG_ENVELOPED = 3U;

		// Token: 0x040018FA RID: 6394
		internal const uint CMSG_SIGNED_AND_ENVELOPED = 4U;

		// Token: 0x040018FB RID: 6395
		internal const uint CMSG_HASHED = 5U;

		// Token: 0x040018FC RID: 6396
		internal const uint CMSG_ENCRYPTED = 6U;

		// Token: 0x040018FD RID: 6397
		internal const uint CMSG_KEY_TRANS_RECIPIENT = 1U;

		// Token: 0x040018FE RID: 6398
		internal const uint CMSG_KEY_AGREE_RECIPIENT = 2U;

		// Token: 0x040018FF RID: 6399
		internal const uint CMSG_MAIL_LIST_RECIPIENT = 3U;

		// Token: 0x04001900 RID: 6400
		internal const uint CMSG_KEY_AGREE_ORIGINATOR_CERT = 1U;

		// Token: 0x04001901 RID: 6401
		internal const uint CMSG_KEY_AGREE_ORIGINATOR_PUBLIC_KEY = 2U;

		// Token: 0x04001902 RID: 6402
		internal const uint CMSG_KEY_AGREE_EPHEMERAL_KEY_CHOICE = 1U;

		// Token: 0x04001903 RID: 6403
		internal const uint CMSG_KEY_AGREE_STATIC_KEY_CHOICE = 2U;

		// Token: 0x04001904 RID: 6404
		internal const uint CMSG_ENVELOPED_RECIPIENT_V0 = 0U;

		// Token: 0x04001905 RID: 6405
		internal const uint CMSG_ENVELOPED_RECIPIENT_V2 = 2U;

		// Token: 0x04001906 RID: 6406
		internal const uint CMSG_ENVELOPED_RECIPIENT_V3 = 3U;

		// Token: 0x04001907 RID: 6407
		internal const uint CMSG_ENVELOPED_RECIPIENT_V4 = 4U;

		// Token: 0x04001908 RID: 6408
		internal const uint CMSG_KEY_TRANS_PKCS_1_5_VERSION = 0U;

		// Token: 0x04001909 RID: 6409
		internal const uint CMSG_KEY_TRANS_CMS_VERSION = 2U;

		// Token: 0x0400190A RID: 6410
		internal const uint CMSG_KEY_AGREE_VERSION = 3U;

		// Token: 0x0400190B RID: 6411
		internal const uint CMSG_MAIL_LIST_VERSION = 4U;

		// Token: 0x0400190C RID: 6412
		internal const uint CRYPT_RC2_40BIT_VERSION = 160U;

		// Token: 0x0400190D RID: 6413
		internal const uint CRYPT_RC2_56BIT_VERSION = 52U;

		// Token: 0x0400190E RID: 6414
		internal const uint CRYPT_RC2_64BIT_VERSION = 120U;

		// Token: 0x0400190F RID: 6415
		internal const uint CRYPT_RC2_128BIT_VERSION = 58U;

		// Token: 0x04001910 RID: 6416
		internal const int E_NOTIMPL = -2147483647;

		// Token: 0x04001911 RID: 6417
		internal const int E_OUTOFMEMORY = -2147024882;

		// Token: 0x04001912 RID: 6418
		internal const int NTE_NO_KEY = -2146893811;

		// Token: 0x04001913 RID: 6419
		internal const int NTE_BAD_PUBLIC_KEY = -2146893803;

		// Token: 0x04001914 RID: 6420
		internal const int NTE_BAD_KEYSET = -2146893802;

		// Token: 0x04001915 RID: 6421
		internal const int CRYPT_E_MSG_ERROR = -2146889727;

		// Token: 0x04001916 RID: 6422
		internal const int CRYPT_E_UNKNOWN_ALGO = -2146889726;

		// Token: 0x04001917 RID: 6423
		internal const int CRYPT_E_INVALID_MSG_TYPE = -2146889724;

		// Token: 0x04001918 RID: 6424
		internal const int CRYPT_E_RECIPIENT_NOT_FOUND = -2146889717;

		// Token: 0x04001919 RID: 6425
		internal const int CRYPT_E_ISSUER_SERIALNUMBER = -2146889715;

		// Token: 0x0400191A RID: 6426
		internal const int CRYPT_E_SIGNER_NOT_FOUND = -2146889714;

		// Token: 0x0400191B RID: 6427
		internal const int CRYPT_E_ATTRIBUTES_MISSING = -2146889713;

		// Token: 0x0400191C RID: 6428
		internal const int CRYPT_E_BAD_ENCODE = -2146885630;

		// Token: 0x0400191D RID: 6429
		internal const int CRYPT_E_NOT_FOUND = -2146885628;

		// Token: 0x0400191E RID: 6430
		internal const int CRYPT_E_NO_MATCH = -2146885623;

		// Token: 0x0400191F RID: 6431
		internal const int CRYPT_E_NO_SIGNER = -2146885618;

		// Token: 0x04001920 RID: 6432
		internal const int CRYPT_E_REVOKED = -2146885616;

		// Token: 0x04001921 RID: 6433
		internal const int CRYPT_E_NO_REVOCATION_CHECK = -2146885614;

		// Token: 0x04001922 RID: 6434
		internal const int CRYPT_E_REVOCATION_OFFLINE = -2146885613;

		// Token: 0x04001923 RID: 6435
		internal const int CRYPT_E_ASN1_BADTAG = -2146881269;

		// Token: 0x04001924 RID: 6436
		internal const int TRUST_E_CERT_SIGNATURE = -2146869244;

		// Token: 0x04001925 RID: 6437
		internal const int TRUST_E_BASIC_CONSTRAINTS = -2146869223;

		// Token: 0x04001926 RID: 6438
		internal const int CERT_E_EXPIRED = -2146762495;

		// Token: 0x04001927 RID: 6439
		internal const int CERT_E_VALIDITYPERIODNESTING = -2146762494;

		// Token: 0x04001928 RID: 6440
		internal const int CERT_E_UNTRUSTEDROOT = -2146762487;

		// Token: 0x04001929 RID: 6441
		internal const int CERT_E_CHAINING = -2146762486;

		// Token: 0x0400192A RID: 6442
		internal const int TRUST_E_FAIL = -2146762485;

		// Token: 0x0400192B RID: 6443
		internal const int CERT_E_REVOKED = -2146762484;

		// Token: 0x0400192C RID: 6444
		internal const int CERT_E_UNTRUSTEDTESTROOT = -2146762483;

		// Token: 0x0400192D RID: 6445
		internal const int CERT_E_REVOCATION_FAILURE = -2146762482;

		// Token: 0x0400192E RID: 6446
		internal const int CERT_E_WRONG_USAGE = -2146762480;

		// Token: 0x0400192F RID: 6447
		internal const int CERT_E_INVALID_POLICY = -2146762477;

		// Token: 0x04001930 RID: 6448
		internal const int CERT_E_INVALID_NAME = -2146762476;

		// Token: 0x04001931 RID: 6449
		internal const int ERROR_SUCCESS = 0;

		// Token: 0x04001932 RID: 6450
		internal const int ERROR_CALL_NOT_IMPLEMENTED = 120;

		// Token: 0x04001933 RID: 6451
		internal const int ERROR_CANCELLED = 1223;

		// Token: 0x020002CA RID: 714
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct BLOBHEADER
		{
			// Token: 0x04001934 RID: 6452
			internal byte bType;

			// Token: 0x04001935 RID: 6453
			internal byte bVersion;

			// Token: 0x04001936 RID: 6454
			internal short reserved;

			// Token: 0x04001937 RID: 6455
			internal uint aiKeyAlg;
		}

		// Token: 0x020002CB RID: 715
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_ALT_NAME_INFO
		{
			// Token: 0x04001938 RID: 6456
			internal uint cAltEntry;

			// Token: 0x04001939 RID: 6457
			internal IntPtr rgAltEntry;
		}

		// Token: 0x020002CC RID: 716
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_ALT_NAME_ENTRY
		{
			// Token: 0x0400193A RID: 6458
			internal uint dwAltNameChoice;

			// Token: 0x0400193B RID: 6459
			internal CAPIBase.CERT_ALT_NAME_ENTRY_UNION Value;
		}

		// Token: 0x020002CD RID: 717
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		internal struct CERT_ALT_NAME_ENTRY_UNION
		{
			// Token: 0x0400193C RID: 6460
			[FieldOffset(0)]
			internal IntPtr pOtherName;

			// Token: 0x0400193D RID: 6461
			[FieldOffset(0)]
			internal IntPtr pwszRfc822Name;

			// Token: 0x0400193E RID: 6462
			[FieldOffset(0)]
			internal IntPtr pwszDNSName;

			// Token: 0x0400193F RID: 6463
			[FieldOffset(0)]
			internal CAPIBase.CRYPTOAPI_BLOB DirectoryName;

			// Token: 0x04001940 RID: 6464
			[FieldOffset(0)]
			internal IntPtr pwszURL;

			// Token: 0x04001941 RID: 6465
			[FieldOffset(0)]
			internal CAPIBase.CRYPTOAPI_BLOB IPAddress;

			// Token: 0x04001942 RID: 6466
			[FieldOffset(0)]
			internal IntPtr pszRegisteredID;
		}

		// Token: 0x020002CE RID: 718
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_BASIC_CONSTRAINTS_INFO
		{
			// Token: 0x04001943 RID: 6467
			internal CAPIBase.CRYPT_BIT_BLOB SubjectType;

			// Token: 0x04001944 RID: 6468
			internal bool fPathLenConstraint;

			// Token: 0x04001945 RID: 6469
			internal uint dwPathLenConstraint;

			// Token: 0x04001946 RID: 6470
			internal uint cSubtreesConstraint;

			// Token: 0x04001947 RID: 6471
			internal IntPtr rgSubtreesConstraint;
		}

		// Token: 0x020002CF RID: 719
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_BASIC_CONSTRAINTS2_INFO
		{
			// Token: 0x04001948 RID: 6472
			internal int fCA;

			// Token: 0x04001949 RID: 6473
			internal int fPathLenConstraint;

			// Token: 0x0400194A RID: 6474
			internal uint dwPathLenConstraint;
		}

		// Token: 0x020002D0 RID: 720
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_CHAIN_CONTEXT
		{
			// Token: 0x06001872 RID: 6258 RVA: 0x00054338 File Offset: 0x00053338
			internal CERT_CHAIN_CONTEXT(int size)
			{
				this.cbSize = (uint)size;
				this.dwErrorStatus = 0U;
				this.dwInfoStatus = 0U;
				this.cChain = 0U;
				this.rgpChain = IntPtr.Zero;
				this.cLowerQualityChainContext = 0U;
				this.rgpLowerQualityChainContext = IntPtr.Zero;
				this.fHasRevocationFreshnessTime = 0U;
				this.dwRevocationFreshnessTime = 0U;
			}

			// Token: 0x0400194B RID: 6475
			internal uint cbSize;

			// Token: 0x0400194C RID: 6476
			internal uint dwErrorStatus;

			// Token: 0x0400194D RID: 6477
			internal uint dwInfoStatus;

			// Token: 0x0400194E RID: 6478
			internal uint cChain;

			// Token: 0x0400194F RID: 6479
			internal IntPtr rgpChain;

			// Token: 0x04001950 RID: 6480
			internal uint cLowerQualityChainContext;

			// Token: 0x04001951 RID: 6481
			internal IntPtr rgpLowerQualityChainContext;

			// Token: 0x04001952 RID: 6482
			internal uint fHasRevocationFreshnessTime;

			// Token: 0x04001953 RID: 6483
			internal uint dwRevocationFreshnessTime;
		}

		// Token: 0x020002D1 RID: 721
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_CHAIN_ELEMENT
		{
			// Token: 0x06001873 RID: 6259 RVA: 0x0005438C File Offset: 0x0005338C
			internal CERT_CHAIN_ELEMENT(int size)
			{
				this.cbSize = (uint)size;
				this.pCertContext = IntPtr.Zero;
				this.dwErrorStatus = 0U;
				this.dwInfoStatus = 0U;
				this.pRevocationInfo = IntPtr.Zero;
				this.pIssuanceUsage = IntPtr.Zero;
				this.pApplicationUsage = IntPtr.Zero;
				this.pwszExtendedErrorInfo = IntPtr.Zero;
			}

			// Token: 0x04001954 RID: 6484
			internal uint cbSize;

			// Token: 0x04001955 RID: 6485
			internal IntPtr pCertContext;

			// Token: 0x04001956 RID: 6486
			internal uint dwErrorStatus;

			// Token: 0x04001957 RID: 6487
			internal uint dwInfoStatus;

			// Token: 0x04001958 RID: 6488
			internal IntPtr pRevocationInfo;

			// Token: 0x04001959 RID: 6489
			internal IntPtr pIssuanceUsage;

			// Token: 0x0400195A RID: 6490
			internal IntPtr pApplicationUsage;

			// Token: 0x0400195B RID: 6491
			internal IntPtr pwszExtendedErrorInfo;
		}

		// Token: 0x020002D2 RID: 722
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_CHAIN_PARA
		{
			// Token: 0x0400195C RID: 6492
			internal uint cbSize;

			// Token: 0x0400195D RID: 6493
			internal CAPIBase.CERT_USAGE_MATCH RequestedUsage;

			// Token: 0x0400195E RID: 6494
			internal CAPIBase.CERT_USAGE_MATCH RequestedIssuancePolicy;

			// Token: 0x0400195F RID: 6495
			internal uint dwUrlRetrievalTimeout;

			// Token: 0x04001960 RID: 6496
			internal bool fCheckRevocationFreshnessTime;

			// Token: 0x04001961 RID: 6497
			internal uint dwRevocationFreshnessTime;
		}

		// Token: 0x020002D3 RID: 723
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_CHAIN_POLICY_PARA
		{
			// Token: 0x06001874 RID: 6260 RVA: 0x000543E5 File Offset: 0x000533E5
			internal CERT_CHAIN_POLICY_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.dwFlags = 0U;
				this.pvExtraPolicyPara = IntPtr.Zero;
			}

			// Token: 0x04001962 RID: 6498
			internal uint cbSize;

			// Token: 0x04001963 RID: 6499
			internal uint dwFlags;

			// Token: 0x04001964 RID: 6500
			internal IntPtr pvExtraPolicyPara;
		}

		// Token: 0x020002D4 RID: 724
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_CHAIN_POLICY_STATUS
		{
			// Token: 0x06001875 RID: 6261 RVA: 0x00054400 File Offset: 0x00053400
			internal CERT_CHAIN_POLICY_STATUS(int size)
			{
				this.cbSize = (uint)size;
				this.dwError = 0U;
				this.lChainIndex = IntPtr.Zero;
				this.lElementIndex = IntPtr.Zero;
				this.pvExtraPolicyStatus = IntPtr.Zero;
			}

			// Token: 0x04001965 RID: 6501
			internal uint cbSize;

			// Token: 0x04001966 RID: 6502
			internal uint dwError;

			// Token: 0x04001967 RID: 6503
			internal IntPtr lChainIndex;

			// Token: 0x04001968 RID: 6504
			internal IntPtr lElementIndex;

			// Token: 0x04001969 RID: 6505
			internal IntPtr pvExtraPolicyStatus;
		}

		// Token: 0x020002D5 RID: 725
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_CONTEXT
		{
			// Token: 0x0400196A RID: 6506
			internal uint dwCertEncodingType;

			// Token: 0x0400196B RID: 6507
			internal IntPtr pbCertEncoded;

			// Token: 0x0400196C RID: 6508
			internal uint cbCertEncoded;

			// Token: 0x0400196D RID: 6509
			internal IntPtr pCertInfo;

			// Token: 0x0400196E RID: 6510
			internal IntPtr hCertStore;
		}

		// Token: 0x020002D6 RID: 726
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_DSS_PARAMETERS
		{
			// Token: 0x0400196F RID: 6511
			internal CAPIBase.CRYPTOAPI_BLOB p;

			// Token: 0x04001970 RID: 6512
			internal CAPIBase.CRYPTOAPI_BLOB q;

			// Token: 0x04001971 RID: 6513
			internal CAPIBase.CRYPTOAPI_BLOB g;
		}

		// Token: 0x020002D7 RID: 727
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_ENHKEY_USAGE
		{
			// Token: 0x04001972 RID: 6514
			internal uint cUsageIdentifier;

			// Token: 0x04001973 RID: 6515
			internal IntPtr rgpszUsageIdentifier;
		}

		// Token: 0x020002D8 RID: 728
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_EXTENSION
		{
			// Token: 0x04001974 RID: 6516
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x04001975 RID: 6517
			internal bool fCritical;

			// Token: 0x04001976 RID: 6518
			internal CAPIBase.CRYPTOAPI_BLOB Value;
		}

		// Token: 0x020002D9 RID: 729
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_ID
		{
			// Token: 0x04001977 RID: 6519
			internal uint dwIdChoice;

			// Token: 0x04001978 RID: 6520
			internal CAPIBase.CERT_ID_UNION Value;
		}

		// Token: 0x020002DA RID: 730
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		internal struct CERT_ID_UNION
		{
			// Token: 0x04001979 RID: 6521
			[FieldOffset(0)]
			internal CAPIBase.CERT_ISSUER_SERIAL_NUMBER IssuerSerialNumber;

			// Token: 0x0400197A RID: 6522
			[FieldOffset(0)]
			internal CAPIBase.CRYPTOAPI_BLOB KeyId;

			// Token: 0x0400197B RID: 6523
			[FieldOffset(0)]
			internal CAPIBase.CRYPTOAPI_BLOB HashId;
		}

		// Token: 0x020002DB RID: 731
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_ISSUER_SERIAL_NUMBER
		{
			// Token: 0x0400197C RID: 6524
			internal CAPIBase.CRYPTOAPI_BLOB Issuer;

			// Token: 0x0400197D RID: 6525
			internal CAPIBase.CRYPTOAPI_BLOB SerialNumber;
		}

		// Token: 0x020002DC RID: 732
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_INFO
		{
			// Token: 0x0400197E RID: 6526
			internal uint dwVersion;

			// Token: 0x0400197F RID: 6527
			internal CAPIBase.CRYPTOAPI_BLOB SerialNumber;

			// Token: 0x04001980 RID: 6528
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER SignatureAlgorithm;

			// Token: 0x04001981 RID: 6529
			internal CAPIBase.CRYPTOAPI_BLOB Issuer;

			// Token: 0x04001982 RID: 6530
			internal global::System.Runtime.InteropServices.ComTypes.FILETIME NotBefore;

			// Token: 0x04001983 RID: 6531
			internal global::System.Runtime.InteropServices.ComTypes.FILETIME NotAfter;

			// Token: 0x04001984 RID: 6532
			internal CAPIBase.CRYPTOAPI_BLOB Subject;

			// Token: 0x04001985 RID: 6533
			internal CAPIBase.CERT_PUBLIC_KEY_INFO SubjectPublicKeyInfo;

			// Token: 0x04001986 RID: 6534
			internal CAPIBase.CRYPT_BIT_BLOB IssuerUniqueId;

			// Token: 0x04001987 RID: 6535
			internal CAPIBase.CRYPT_BIT_BLOB SubjectUniqueId;

			// Token: 0x04001988 RID: 6536
			internal uint cExtension;

			// Token: 0x04001989 RID: 6537
			internal IntPtr rgExtension;
		}

		// Token: 0x020002DD RID: 733
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_KEY_USAGE_RESTRICTION_INFO
		{
			// Token: 0x0400198A RID: 6538
			internal uint cCertPolicyId;

			// Token: 0x0400198B RID: 6539
			internal IntPtr rgCertPolicyId;

			// Token: 0x0400198C RID: 6540
			internal CAPIBase.CRYPT_BIT_BLOB RestrictedKeyUsage;
		}

		// Token: 0x020002DE RID: 734
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_NAME_INFO
		{
			// Token: 0x0400198D RID: 6541
			internal uint cRDN;

			// Token: 0x0400198E RID: 6542
			internal IntPtr rgRDN;
		}

		// Token: 0x020002DF RID: 735
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_NAME_VALUE
		{
			// Token: 0x0400198F RID: 6543
			internal uint dwValueType;

			// Token: 0x04001990 RID: 6544
			internal CAPIBase.CRYPTOAPI_BLOB Value;
		}

		// Token: 0x020002E0 RID: 736
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_OTHER_NAME
		{
			// Token: 0x04001991 RID: 6545
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x04001992 RID: 6546
			internal CAPIBase.CRYPTOAPI_BLOB Value;
		}

		// Token: 0x020002E1 RID: 737
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_POLICY_ID
		{
			// Token: 0x04001993 RID: 6547
			internal uint cCertPolicyElementId;

			// Token: 0x04001994 RID: 6548
			internal IntPtr rgpszCertPolicyElementId;
		}

		// Token: 0x020002E2 RID: 738
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_POLICIES_INFO
		{
			// Token: 0x04001995 RID: 6549
			internal uint cPolicyInfo;

			// Token: 0x04001996 RID: 6550
			internal IntPtr rgPolicyInfo;
		}

		// Token: 0x020002E3 RID: 739
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_POLICY_INFO
		{
			// Token: 0x04001997 RID: 6551
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszPolicyIdentifier;

			// Token: 0x04001998 RID: 6552
			internal uint cPolicyQualifier;

			// Token: 0x04001999 RID: 6553
			internal IntPtr rgPolicyQualifier;
		}

		// Token: 0x020002E4 RID: 740
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_POLICY_QUALIFIER_INFO
		{
			// Token: 0x0400199A RID: 6554
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszPolicyQualifierId;

			// Token: 0x0400199B RID: 6555
			private CAPIBase.CRYPTOAPI_BLOB Qualifier;
		}

		// Token: 0x020002E5 RID: 741
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_PUBLIC_KEY_INFO
		{
			// Token: 0x0400199C RID: 6556
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER Algorithm;

			// Token: 0x0400199D RID: 6557
			internal CAPIBase.CRYPT_BIT_BLOB PublicKey;
		}

		// Token: 0x020002E6 RID: 742
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_PUBLIC_KEY_INFO2
		{
			// Token: 0x0400199E RID: 6558
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER2 Algorithm;

			// Token: 0x0400199F RID: 6559
			internal CAPIBase.CRYPT_BIT_BLOB PublicKey;
		}

		// Token: 0x020002E7 RID: 743
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_RDN
		{
			// Token: 0x040019A0 RID: 6560
			internal uint cRDNAttr;

			// Token: 0x040019A1 RID: 6561
			internal IntPtr rgRDNAttr;
		}

		// Token: 0x020002E8 RID: 744
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_RDN_ATTR
		{
			// Token: 0x040019A2 RID: 6562
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x040019A3 RID: 6563
			internal uint dwValueType;

			// Token: 0x040019A4 RID: 6564
			internal CAPIBase.CRYPTOAPI_BLOB Value;
		}

		// Token: 0x020002E9 RID: 745
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_SIMPLE_CHAIN
		{
			// Token: 0x06001876 RID: 6262 RVA: 0x00054434 File Offset: 0x00053434
			internal CERT_SIMPLE_CHAIN(int size)
			{
				this.cbSize = (uint)size;
				this.dwErrorStatus = 0U;
				this.dwInfoStatus = 0U;
				this.cElement = 0U;
				this.rgpElement = IntPtr.Zero;
				this.pTrustListInfo = IntPtr.Zero;
				this.fHasRevocationFreshnessTime = 0U;
				this.dwRevocationFreshnessTime = 0U;
			}

			// Token: 0x040019A5 RID: 6565
			internal uint cbSize;

			// Token: 0x040019A6 RID: 6566
			internal uint dwErrorStatus;

			// Token: 0x040019A7 RID: 6567
			internal uint dwInfoStatus;

			// Token: 0x040019A8 RID: 6568
			internal uint cElement;

			// Token: 0x040019A9 RID: 6569
			internal IntPtr rgpElement;

			// Token: 0x040019AA RID: 6570
			internal IntPtr pTrustListInfo;

			// Token: 0x040019AB RID: 6571
			internal uint fHasRevocationFreshnessTime;

			// Token: 0x040019AC RID: 6572
			internal uint dwRevocationFreshnessTime;
		}

		// Token: 0x020002EA RID: 746
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_TEMPLATE_EXT
		{
			// Token: 0x040019AD RID: 6573
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x040019AE RID: 6574
			internal uint dwMajorVersion;

			// Token: 0x040019AF RID: 6575
			private bool fMinorVersion;

			// Token: 0x040019B0 RID: 6576
			private uint dwMinorVersion;
		}

		// Token: 0x020002EB RID: 747
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_TRUST_STATUS
		{
			// Token: 0x040019B1 RID: 6577
			internal uint dwErrorStatus;

			// Token: 0x040019B2 RID: 6578
			internal uint dwInfoStatus;
		}

		// Token: 0x020002EC RID: 748
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_USAGE_MATCH
		{
			// Token: 0x040019B3 RID: 6579
			internal uint dwType;

			// Token: 0x040019B4 RID: 6580
			internal CAPIBase.CERT_ENHKEY_USAGE Usage;
		}

		// Token: 0x020002ED RID: 749
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CMS_RECIPIENT_INFO
		{
			// Token: 0x040019B5 RID: 6581
			internal uint dwRecipientChoice;

			// Token: 0x040019B6 RID: 6582
			internal IntPtr pRecipientInfo;
		}

		// Token: 0x020002EE RID: 750
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CMS_SIGNER_INFO
		{
			// Token: 0x040019B7 RID: 6583
			internal uint dwVersion;

			// Token: 0x040019B8 RID: 6584
			internal CAPIBase.CERT_ID SignerId;

			// Token: 0x040019B9 RID: 6585
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;

			// Token: 0x040019BA RID: 6586
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;

			// Token: 0x040019BB RID: 6587
			internal CAPIBase.CRYPTOAPI_BLOB EncryptedHash;

			// Token: 0x040019BC RID: 6588
			internal CAPIBase.CRYPT_ATTRIBUTES AuthAttrs;

			// Token: 0x040019BD RID: 6589
			internal CAPIBase.CRYPT_ATTRIBUTES UnauthAttrs;
		}

		// Token: 0x020002EF RID: 751
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA
		{
			// Token: 0x06001877 RID: 6263 RVA: 0x00054481 File Offset: 0x00053481
			internal CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.dwSignerIndex = 0U;
				this.blob = default(CAPIBase.CRYPTOAPI_BLOB);
			}

			// Token: 0x040019BE RID: 6590
			internal uint cbSize;

			// Token: 0x040019BF RID: 6591
			internal uint dwSignerIndex;

			// Token: 0x040019C0 RID: 6592
			internal CAPIBase.CRYPTOAPI_BLOB blob;
		}

		// Token: 0x020002F0 RID: 752
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_DECRYPT_PARA
		{
			// Token: 0x06001878 RID: 6264 RVA: 0x0005449D File Offset: 0x0005349D
			internal CMSG_CTRL_DECRYPT_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.hCryptProv = IntPtr.Zero;
				this.dwKeySpec = 0U;
				this.dwRecipientIndex = 0U;
			}

			// Token: 0x040019C1 RID: 6593
			internal uint cbSize;

			// Token: 0x040019C2 RID: 6594
			internal IntPtr hCryptProv;

			// Token: 0x040019C3 RID: 6595
			internal uint dwKeySpec;

			// Token: 0x040019C4 RID: 6596
			internal uint dwRecipientIndex;
		}

		// Token: 0x020002F1 RID: 753
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA
		{
			// Token: 0x06001879 RID: 6265 RVA: 0x000544BF File Offset: 0x000534BF
			internal CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.dwSignerIndex = 0U;
				this.dwUnauthAttrIndex = 0U;
			}

			// Token: 0x040019C5 RID: 6597
			internal uint cbSize;

			// Token: 0x040019C6 RID: 6598
			internal uint dwSignerIndex;

			// Token: 0x040019C7 RID: 6599
			internal uint dwUnauthAttrIndex;
		}

		// Token: 0x020002F2 RID: 754
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_KEY_TRANS_DECRYPT_PARA
		{
			// Token: 0x040019C8 RID: 6600
			internal uint cbSize;

			// Token: 0x040019C9 RID: 6601
			internal SafeCryptProvHandle hCryptProv;

			// Token: 0x040019CA RID: 6602
			internal uint dwKeySpec;

			// Token: 0x040019CB RID: 6603
			internal IntPtr pKeyTrans;

			// Token: 0x040019CC RID: 6604
			internal uint dwRecipientIndex;
		}

		// Token: 0x020002F3 RID: 755
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO
		{
			// Token: 0x040019CD RID: 6605
			internal uint cbSize;

			// Token: 0x040019CE RID: 6606
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;

			// Token: 0x040019CF RID: 6607
			internal IntPtr pvKeyEncryptionAuxInfo;

			// Token: 0x040019D0 RID: 6608
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyWrapAlgorithm;

			// Token: 0x040019D1 RID: 6609
			internal IntPtr pvKeyWrapAuxInfo;

			// Token: 0x040019D2 RID: 6610
			internal IntPtr hCryptProv;

			// Token: 0x040019D3 RID: 6611
			internal uint dwKeySpec;

			// Token: 0x040019D4 RID: 6612
			internal uint dwKeyChoice;

			// Token: 0x040019D5 RID: 6613
			internal IntPtr pEphemeralAlgorithmOrSenderId;

			// Token: 0x040019D6 RID: 6614
			internal CAPIBase.CRYPTOAPI_BLOB UserKeyingMaterial;

			// Token: 0x040019D7 RID: 6615
			internal uint cRecipientEncryptedKeys;

			// Token: 0x040019D8 RID: 6616
			internal IntPtr rgpRecipientEncryptedKeys;
		}

		// Token: 0x020002F4 RID: 756
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO
		{
			// Token: 0x040019D9 RID: 6617
			internal uint cbSize;

			// Token: 0x040019DA RID: 6618
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;

			// Token: 0x040019DB RID: 6619
			internal IntPtr pvKeyEncryptionAuxInfo;

			// Token: 0x040019DC RID: 6620
			internal IntPtr hCryptProv;

			// Token: 0x040019DD RID: 6621
			internal CAPIBase.CRYPT_BIT_BLOB RecipientPublicKey;

			// Token: 0x040019DE RID: 6622
			internal CAPIBase.CERT_ID RecipientId;
		}

		// Token: 0x020002F5 RID: 757
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_RC2_AUX_INFO
		{
			// Token: 0x0600187A RID: 6266 RVA: 0x000544D6 File Offset: 0x000534D6
			internal CMSG_RC2_AUX_INFO(int size)
			{
				this.cbSize = (uint)size;
				this.dwBitLen = 0U;
			}

			// Token: 0x040019DF RID: 6623
			internal uint cbSize;

			// Token: 0x040019E0 RID: 6624
			internal uint dwBitLen;
		}

		// Token: 0x020002F6 RID: 758
		internal struct CMSG_RECIPIENT_ENCODE_INFO
		{
			// Token: 0x040019E1 RID: 6625
			internal uint dwRecipientChoice;

			// Token: 0x040019E2 RID: 6626
			internal IntPtr pRecipientInfo;
		}

		// Token: 0x020002F7 RID: 759
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO
		{
			// Token: 0x040019E3 RID: 6627
			internal uint cbSize;

			// Token: 0x040019E4 RID: 6628
			internal CAPIBase.CRYPT_BIT_BLOB RecipientPublicKey;

			// Token: 0x040019E5 RID: 6629
			internal CAPIBase.CERT_ID RecipientId;

			// Token: 0x040019E6 RID: 6630
			internal global::System.Runtime.InteropServices.ComTypes.FILETIME Date;

			// Token: 0x040019E7 RID: 6631
			internal IntPtr pOtherAttr;
		}

		// Token: 0x020002F8 RID: 760
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_ENVELOPED_ENCODE_INFO
		{
			// Token: 0x0600187B RID: 6267 RVA: 0x000544E8 File Offset: 0x000534E8
			internal CMSG_ENVELOPED_ENCODE_INFO(int size)
			{
				this.cbSize = (uint)size;
				this.hCryptProv = IntPtr.Zero;
				this.ContentEncryptionAlgorithm = default(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER);
				this.pvEncryptionAuxInfo = IntPtr.Zero;
				this.cRecipients = 0U;
				this.rgpRecipients = IntPtr.Zero;
				this.rgCmsRecipients = IntPtr.Zero;
				this.cCertEncoded = 0U;
				this.rgCertEncoded = IntPtr.Zero;
				this.cCrlEncoded = 0U;
				this.rgCrlEncoded = IntPtr.Zero;
				this.cAttrCertEncoded = 0U;
				this.rgAttrCertEncoded = IntPtr.Zero;
				this.cUnprotectedAttr = 0U;
				this.rgUnprotectedAttr = IntPtr.Zero;
			}

			// Token: 0x040019E8 RID: 6632
			internal uint cbSize;

			// Token: 0x040019E9 RID: 6633
			internal IntPtr hCryptProv;

			// Token: 0x040019EA RID: 6634
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER ContentEncryptionAlgorithm;

			// Token: 0x040019EB RID: 6635
			internal IntPtr pvEncryptionAuxInfo;

			// Token: 0x040019EC RID: 6636
			internal uint cRecipients;

			// Token: 0x040019ED RID: 6637
			internal IntPtr rgpRecipients;

			// Token: 0x040019EE RID: 6638
			internal IntPtr rgCmsRecipients;

			// Token: 0x040019EF RID: 6639
			internal uint cCertEncoded;

			// Token: 0x040019F0 RID: 6640
			internal IntPtr rgCertEncoded;

			// Token: 0x040019F1 RID: 6641
			internal uint cCrlEncoded;

			// Token: 0x040019F2 RID: 6642
			internal IntPtr rgCrlEncoded;

			// Token: 0x040019F3 RID: 6643
			internal uint cAttrCertEncoded;

			// Token: 0x040019F4 RID: 6644
			internal IntPtr rgAttrCertEncoded;

			// Token: 0x040019F5 RID: 6645
			internal uint cUnprotectedAttr;

			// Token: 0x040019F6 RID: 6646
			internal IntPtr rgUnprotectedAttr;
		}

		// Token: 0x020002F9 RID: 761
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_KEY_AGREE_DECRYPT_PARA
		{
			// Token: 0x0600187C RID: 6268 RVA: 0x00054583 File Offset: 0x00053583
			internal CMSG_CTRL_KEY_AGREE_DECRYPT_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.hCryptProv = IntPtr.Zero;
				this.dwKeySpec = 0U;
				this.pKeyAgree = IntPtr.Zero;
				this.dwRecipientIndex = 0U;
				this.dwRecipientEncryptedKeyIndex = 0U;
				this.OriginatorPublicKey = default(CAPIBase.CRYPT_BIT_BLOB);
			}

			// Token: 0x040019F7 RID: 6647
			internal uint cbSize;

			// Token: 0x040019F8 RID: 6648
			internal IntPtr hCryptProv;

			// Token: 0x040019F9 RID: 6649
			internal uint dwKeySpec;

			// Token: 0x040019FA RID: 6650
			internal IntPtr pKeyAgree;

			// Token: 0x040019FB RID: 6651
			internal uint dwRecipientIndex;

			// Token: 0x040019FC RID: 6652
			internal uint dwRecipientEncryptedKeyIndex;

			// Token: 0x040019FD RID: 6653
			internal CAPIBase.CRYPT_BIT_BLOB OriginatorPublicKey;
		}

		// Token: 0x020002FA RID: 762
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_AGREE_RECIPIENT_INFO
		{
			// Token: 0x040019FE RID: 6654
			internal uint dwVersion;

			// Token: 0x040019FF RID: 6655
			internal uint dwOriginatorChoice;
		}

		// Token: 0x020002FB RID: 763
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO
		{
			// Token: 0x04001A00 RID: 6656
			internal uint dwVersion;

			// Token: 0x04001A01 RID: 6657
			internal uint dwOriginatorChoice;

			// Token: 0x04001A02 RID: 6658
			internal CAPIBase.CERT_ID OriginatorCertId;

			// Token: 0x04001A03 RID: 6659
			internal IntPtr Padding;

			// Token: 0x04001A04 RID: 6660
			internal CAPIBase.CRYPTOAPI_BLOB UserKeyingMaterial;

			// Token: 0x04001A05 RID: 6661
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;

			// Token: 0x04001A06 RID: 6662
			internal uint cRecipientEncryptedKeys;

			// Token: 0x04001A07 RID: 6663
			internal IntPtr rgpRecipientEncryptedKeys;
		}

		// Token: 0x020002FC RID: 764
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO
		{
			// Token: 0x04001A08 RID: 6664
			internal uint dwVersion;

			// Token: 0x04001A09 RID: 6665
			internal uint dwOriginatorChoice;

			// Token: 0x04001A0A RID: 6666
			internal CAPIBase.CERT_PUBLIC_KEY_INFO OriginatorPublicKeyInfo;

			// Token: 0x04001A0B RID: 6667
			internal CAPIBase.CRYPTOAPI_BLOB UserKeyingMaterial;

			// Token: 0x04001A0C RID: 6668
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;

			// Token: 0x04001A0D RID: 6669
			internal uint cRecipientEncryptedKeys;

			// Token: 0x04001A0E RID: 6670
			internal IntPtr rgpRecipientEncryptedKeys;
		}

		// Token: 0x020002FD RID: 765
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_RECIPIENT_ENCRYPTED_KEY_INFO
		{
			// Token: 0x04001A0F RID: 6671
			internal CAPIBase.CERT_ID RecipientId;

			// Token: 0x04001A10 RID: 6672
			internal CAPIBase.CRYPTOAPI_BLOB EncryptedKey;

			// Token: 0x04001A11 RID: 6673
			internal global::System.Runtime.InteropServices.ComTypes.FILETIME Date;

			// Token: 0x04001A12 RID: 6674
			internal IntPtr pOtherAttr;
		}

		// Token: 0x020002FE RID: 766
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_VERIFY_SIGNATURE_EX_PARA
		{
			// Token: 0x0600187D RID: 6269 RVA: 0x000545C3 File Offset: 0x000535C3
			internal CMSG_CTRL_VERIFY_SIGNATURE_EX_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.hCryptProv = IntPtr.Zero;
				this.dwSignerIndex = 0U;
				this.dwSignerType = 0U;
				this.pvSigner = IntPtr.Zero;
			}

			// Token: 0x04001A13 RID: 6675
			internal uint cbSize;

			// Token: 0x04001A14 RID: 6676
			internal IntPtr hCryptProv;

			// Token: 0x04001A15 RID: 6677
			internal uint dwSignerIndex;

			// Token: 0x04001A16 RID: 6678
			internal uint dwSignerType;

			// Token: 0x04001A17 RID: 6679
			internal IntPtr pvSigner;
		}

		// Token: 0x020002FF RID: 767
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_TRANS_RECIPIENT_INFO
		{
			// Token: 0x04001A18 RID: 6680
			internal uint dwVersion;

			// Token: 0x04001A19 RID: 6681
			internal CAPIBase.CERT_ID RecipientId;

			// Token: 0x04001A1A RID: 6682
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;

			// Token: 0x04001A1B RID: 6683
			internal CAPIBase.CRYPTOAPI_BLOB EncryptedKey;
		}

		// Token: 0x02000300 RID: 768
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_SIGNED_ENCODE_INFO
		{
			// Token: 0x0600187E RID: 6270 RVA: 0x000545F0 File Offset: 0x000535F0
			internal CMSG_SIGNED_ENCODE_INFO(int size)
			{
				this.cbSize = (uint)size;
				this.cSigners = 0U;
				this.rgSigners = IntPtr.Zero;
				this.cCertEncoded = 0U;
				this.rgCertEncoded = IntPtr.Zero;
				this.cCrlEncoded = 0U;
				this.rgCrlEncoded = IntPtr.Zero;
				this.cAttrCertEncoded = 0U;
				this.rgAttrCertEncoded = IntPtr.Zero;
			}

			// Token: 0x04001A1C RID: 6684
			internal uint cbSize;

			// Token: 0x04001A1D RID: 6685
			internal uint cSigners;

			// Token: 0x04001A1E RID: 6686
			internal IntPtr rgSigners;

			// Token: 0x04001A1F RID: 6687
			internal uint cCertEncoded;

			// Token: 0x04001A20 RID: 6688
			internal IntPtr rgCertEncoded;

			// Token: 0x04001A21 RID: 6689
			internal uint cCrlEncoded;

			// Token: 0x04001A22 RID: 6690
			internal IntPtr rgCrlEncoded;

			// Token: 0x04001A23 RID: 6691
			internal uint cAttrCertEncoded;

			// Token: 0x04001A24 RID: 6692
			internal IntPtr rgAttrCertEncoded;
		}

		// Token: 0x02000301 RID: 769
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_SIGNER_ENCODE_INFO
		{
			// Token: 0x0600187F RID: 6271
			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern IntPtr LocalFree(IntPtr hMem);

			// Token: 0x06001880 RID: 6272
			[DllImport("advapi32.dll", SetLastError = true)]
			internal static extern bool CryptReleaseContext([In] IntPtr hProv, [In] uint dwFlags);

			// Token: 0x06001881 RID: 6273 RVA: 0x0005464C File Offset: 0x0005364C
			internal CMSG_SIGNER_ENCODE_INFO(int size)
			{
				this.cbSize = (uint)size;
				this.pCertInfo = IntPtr.Zero;
				this.hCryptProv = IntPtr.Zero;
				this.dwKeySpec = 0U;
				this.HashAlgorithm = default(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER);
				this.pvHashAuxInfo = IntPtr.Zero;
				this.cAuthAttr = 0U;
				this.rgAuthAttr = IntPtr.Zero;
				this.cUnauthAttr = 0U;
				this.rgUnauthAttr = IntPtr.Zero;
				this.SignerId = default(CAPIBase.CERT_ID);
				this.HashEncryptionAlgorithm = default(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER);
				this.pvHashEncryptionAuxInfo = IntPtr.Zero;
			}

			// Token: 0x06001882 RID: 6274 RVA: 0x000546DC File Offset: 0x000536DC
			internal void Dispose()
			{
				if (this.hCryptProv != IntPtr.Zero)
				{
					CAPIBase.CMSG_SIGNER_ENCODE_INFO.CryptReleaseContext(this.hCryptProv, 0U);
				}
				if (this.SignerId.Value.KeyId.pbData != IntPtr.Zero)
				{
					CAPIBase.CMSG_SIGNER_ENCODE_INFO.LocalFree(this.SignerId.Value.KeyId.pbData);
				}
				if (this.rgAuthAttr != IntPtr.Zero)
				{
					CAPIBase.CMSG_SIGNER_ENCODE_INFO.LocalFree(this.rgAuthAttr);
				}
				if (this.rgUnauthAttr != IntPtr.Zero)
				{
					CAPIBase.CMSG_SIGNER_ENCODE_INFO.LocalFree(this.rgUnauthAttr);
				}
			}

			// Token: 0x04001A25 RID: 6693
			internal uint cbSize;

			// Token: 0x04001A26 RID: 6694
			internal IntPtr pCertInfo;

			// Token: 0x04001A27 RID: 6695
			internal IntPtr hCryptProv;

			// Token: 0x04001A28 RID: 6696
			internal uint dwKeySpec;

			// Token: 0x04001A29 RID: 6697
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;

			// Token: 0x04001A2A RID: 6698
			internal IntPtr pvHashAuxInfo;

			// Token: 0x04001A2B RID: 6699
			internal uint cAuthAttr;

			// Token: 0x04001A2C RID: 6700
			internal IntPtr rgAuthAttr;

			// Token: 0x04001A2D RID: 6701
			internal uint cUnauthAttr;

			// Token: 0x04001A2E RID: 6702
			internal IntPtr rgUnauthAttr;

			// Token: 0x04001A2F RID: 6703
			internal CAPIBase.CERT_ID SignerId;

			// Token: 0x04001A30 RID: 6704
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;

			// Token: 0x04001A31 RID: 6705
			internal IntPtr pvHashEncryptionAuxInfo;
		}

		// Token: 0x02000302 RID: 770
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_SIGNER_INFO
		{
			// Token: 0x04001A32 RID: 6706
			internal uint dwVersion;

			// Token: 0x04001A33 RID: 6707
			internal CAPIBase.CRYPTOAPI_BLOB Issuer;

			// Token: 0x04001A34 RID: 6708
			internal CAPIBase.CRYPTOAPI_BLOB SerialNumber;

			// Token: 0x04001A35 RID: 6709
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;

			// Token: 0x04001A36 RID: 6710
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;

			// Token: 0x04001A37 RID: 6711
			internal CAPIBase.CRYPTOAPI_BLOB EncryptedHash;

			// Token: 0x04001A38 RID: 6712
			internal CAPIBase.CRYPT_ATTRIBUTES AuthAttrs;

			// Token: 0x04001A39 RID: 6713
			internal CAPIBase.CRYPT_ATTRIBUTES UnauthAttrs;
		}

		// Token: 0x02000303 RID: 771
		// (Invoke) Token: 0x06001884 RID: 6276
		internal delegate bool PFN_CMSG_STREAM_OUTPUT(IntPtr pvArg, IntPtr pbData, uint cbData, bool fFinal);

		// Token: 0x02000304 RID: 772
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal class CMSG_STREAM_INFO
		{
			// Token: 0x06001887 RID: 6279 RVA: 0x00054780 File Offset: 0x00053780
			internal CMSG_STREAM_INFO(uint cbContent, CAPIBase.PFN_CMSG_STREAM_OUTPUT pfnStreamOutput, IntPtr pvArg)
			{
				this.cbContent = cbContent;
				this.pfnStreamOutput = pfnStreamOutput;
				this.pvArg = pvArg;
			}

			// Token: 0x04001A3A RID: 6714
			internal uint cbContent;

			// Token: 0x04001A3B RID: 6715
			internal CAPIBase.PFN_CMSG_STREAM_OUTPUT pfnStreamOutput;

			// Token: 0x04001A3C RID: 6716
			internal IntPtr pvArg;
		}

		// Token: 0x02000305 RID: 773
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_ALGORITHM_IDENTIFIER
		{
			// Token: 0x04001A3D RID: 6717
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x04001A3E RID: 6718
			internal CAPIBase.CRYPTOAPI_BLOB Parameters;
		}

		// Token: 0x02000306 RID: 774
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_ALGORITHM_IDENTIFIER2
		{
			// Token: 0x04001A3F RID: 6719
			internal IntPtr pszObjId;

			// Token: 0x04001A40 RID: 6720
			internal CAPIBase.CRYPTOAPI_BLOB Parameters;
		}

		// Token: 0x02000307 RID: 775
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_ATTRIBUTE
		{
			// Token: 0x04001A41 RID: 6721
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x04001A42 RID: 6722
			internal uint cValue;

			// Token: 0x04001A43 RID: 6723
			internal IntPtr rgValue;
		}

		// Token: 0x02000308 RID: 776
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_ATTRIBUTES
		{
			// Token: 0x04001A44 RID: 6724
			internal uint cAttr;

			// Token: 0x04001A45 RID: 6725
			internal IntPtr rgAttr;
		}

		// Token: 0x02000309 RID: 777
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_ATTRIBUTE_TYPE_VALUE
		{
			// Token: 0x04001A46 RID: 6726
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x04001A47 RID: 6727
			internal CAPIBase.CRYPTOAPI_BLOB Value;
		}

		// Token: 0x0200030A RID: 778
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_BIT_BLOB
		{
			// Token: 0x04001A48 RID: 6728
			internal uint cbData;

			// Token: 0x04001A49 RID: 6729
			internal IntPtr pbData;

			// Token: 0x04001A4A RID: 6730
			internal uint cUnusedBits;
		}

		// Token: 0x0200030B RID: 779
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_KEY_PROV_INFO
		{
			// Token: 0x04001A4B RID: 6731
			internal string pwszContainerName;

			// Token: 0x04001A4C RID: 6732
			internal string pwszProvName;

			// Token: 0x04001A4D RID: 6733
			internal uint dwProvType;

			// Token: 0x04001A4E RID: 6734
			internal uint dwFlags;

			// Token: 0x04001A4F RID: 6735
			internal uint cProvParam;

			// Token: 0x04001A50 RID: 6736
			internal IntPtr rgProvParam;

			// Token: 0x04001A51 RID: 6737
			internal uint dwKeySpec;
		}

		// Token: 0x0200030C RID: 780
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_OID_INFO
		{
			// Token: 0x06001888 RID: 6280 RVA: 0x0005479D File Offset: 0x0005379D
			internal CRYPT_OID_INFO(int size)
			{
				this.cbSize = (uint)size;
				this.pszOID = null;
				this.pwszName = null;
				this.dwGroupId = 0U;
				this.Algid = 0U;
				this.ExtraInfo = default(CAPIBase.CRYPTOAPI_BLOB);
			}

			// Token: 0x04001A52 RID: 6738
			internal uint cbSize;

			// Token: 0x04001A53 RID: 6739
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszOID;

			// Token: 0x04001A54 RID: 6740
			internal string pwszName;

			// Token: 0x04001A55 RID: 6741
			internal uint dwGroupId;

			// Token: 0x04001A56 RID: 6742
			internal uint Algid;

			// Token: 0x04001A57 RID: 6743
			internal CAPIBase.CRYPTOAPI_BLOB ExtraInfo;
		}

		// Token: 0x0200030D RID: 781
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_RC2_CBC_PARAMETERS
		{
			// Token: 0x04001A58 RID: 6744
			internal uint dwVersion;

			// Token: 0x04001A59 RID: 6745
			internal bool fIV;

			// Token: 0x04001A5A RID: 6746
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			internal byte[] rgbIV;
		}

		// Token: 0x0200030E RID: 782
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPTOAPI_BLOB
		{
			// Token: 0x04001A5B RID: 6747
			internal uint cbData;

			// Token: 0x04001A5C RID: 6748
			internal IntPtr pbData;
		}

		// Token: 0x0200030F RID: 783
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct DSSPUBKEY
		{
			// Token: 0x04001A5D RID: 6749
			internal uint magic;

			// Token: 0x04001A5E RID: 6750
			internal uint bitlen;
		}

		// Token: 0x02000310 RID: 784
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct KEY_USAGE_STRUCT
		{
			// Token: 0x06001889 RID: 6281 RVA: 0x000547CE File Offset: 0x000537CE
			internal KEY_USAGE_STRUCT(string pwszKeyUsage, uint dwKeyUsageBit)
			{
				this.pwszKeyUsage = pwszKeyUsage;
				this.dwKeyUsageBit = dwKeyUsageBit;
			}

			// Token: 0x04001A5F RID: 6751
			internal string pwszKeyUsage;

			// Token: 0x04001A60 RID: 6752
			internal uint dwKeyUsageBit;
		}

		// Token: 0x02000311 RID: 785
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct PROV_ENUMALGS_EX
		{
			// Token: 0x04001A61 RID: 6753
			internal uint aiAlgid;

			// Token: 0x04001A62 RID: 6754
			internal uint dwDefaultLen;

			// Token: 0x04001A63 RID: 6755
			internal uint dwMinLen;

			// Token: 0x04001A64 RID: 6756
			internal uint dwMaxLen;

			// Token: 0x04001A65 RID: 6757
			internal uint dwProtocols;

			// Token: 0x04001A66 RID: 6758
			internal uint dwNameLen;

			// Token: 0x04001A67 RID: 6759
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
			internal byte[] szName;

			// Token: 0x04001A68 RID: 6760
			internal uint dwLongNameLen;

			// Token: 0x04001A69 RID: 6761
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
			internal byte[] szLongName;
		}

		// Token: 0x02000312 RID: 786
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct RSAPUBKEY
		{
			// Token: 0x04001A6A RID: 6762
			internal uint magic;

			// Token: 0x04001A6B RID: 6763
			internal uint bitlen;

			// Token: 0x04001A6C RID: 6764
			internal uint pubexp;
		}
	}
}
