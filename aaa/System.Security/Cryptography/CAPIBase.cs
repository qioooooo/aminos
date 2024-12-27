using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Security.Cryptography
{
	// Token: 0x02000004 RID: 4
	internal abstract class CAPIBase
	{
		// Token: 0x04000007 RID: 7
		internal const string ADVAPI32 = "advapi32.dll";

		// Token: 0x04000008 RID: 8
		internal const string CRYPT32 = "crypt32.dll";

		// Token: 0x04000009 RID: 9
		internal const string CRYPTUI = "cryptui.dll";

		// Token: 0x0400000A RID: 10
		internal const string KERNEL32 = "kernel32.dll";

		// Token: 0x0400000B RID: 11
		internal const uint LMEM_FIXED = 0U;

		// Token: 0x0400000C RID: 12
		internal const uint LMEM_ZEROINIT = 64U;

		// Token: 0x0400000D RID: 13
		internal const uint LPTR = 64U;

		// Token: 0x0400000E RID: 14
		internal const int S_OK = 0;

		// Token: 0x0400000F RID: 15
		internal const int S_FALSE = 1;

		// Token: 0x04000010 RID: 16
		internal const uint FORMAT_MESSAGE_FROM_SYSTEM = 4096U;

		// Token: 0x04000011 RID: 17
		internal const uint FORMAT_MESSAGE_IGNORE_INSERTS = 512U;

		// Token: 0x04000012 RID: 18
		internal const uint VER_PLATFORM_WIN32s = 0U;

		// Token: 0x04000013 RID: 19
		internal const uint VER_PLATFORM_WIN32_WINDOWS = 1U;

		// Token: 0x04000014 RID: 20
		internal const uint VER_PLATFORM_WIN32_NT = 2U;

		// Token: 0x04000015 RID: 21
		internal const uint VER_PLATFORM_WINCE = 3U;

		// Token: 0x04000016 RID: 22
		internal const uint ASN_TAG_NULL = 5U;

		// Token: 0x04000017 RID: 23
		internal const uint ASN_TAG_OBJID = 6U;

		// Token: 0x04000018 RID: 24
		internal const uint CERT_QUERY_OBJECT_FILE = 1U;

		// Token: 0x04000019 RID: 25
		internal const uint CERT_QUERY_OBJECT_BLOB = 2U;

		// Token: 0x0400001A RID: 26
		internal const uint CERT_QUERY_CONTENT_CERT = 1U;

		// Token: 0x0400001B RID: 27
		internal const uint CERT_QUERY_CONTENT_CTL = 2U;

		// Token: 0x0400001C RID: 28
		internal const uint CERT_QUERY_CONTENT_CRL = 3U;

		// Token: 0x0400001D RID: 29
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_STORE = 4U;

		// Token: 0x0400001E RID: 30
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CERT = 5U;

		// Token: 0x0400001F RID: 31
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CTL = 6U;

		// Token: 0x04000020 RID: 32
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CRL = 7U;

		// Token: 0x04000021 RID: 33
		internal const uint CERT_QUERY_CONTENT_PKCS7_SIGNED = 8U;

		// Token: 0x04000022 RID: 34
		internal const uint CERT_QUERY_CONTENT_PKCS7_UNSIGNED = 9U;

		// Token: 0x04000023 RID: 35
		internal const uint CERT_QUERY_CONTENT_PKCS7_SIGNED_EMBED = 10U;

		// Token: 0x04000024 RID: 36
		internal const uint CERT_QUERY_CONTENT_PKCS10 = 11U;

		// Token: 0x04000025 RID: 37
		internal const uint CERT_QUERY_CONTENT_PFX = 12U;

		// Token: 0x04000026 RID: 38
		internal const uint CERT_QUERY_CONTENT_CERT_PAIR = 13U;

		// Token: 0x04000027 RID: 39
		internal const uint CERT_QUERY_CONTENT_FLAG_CERT = 2U;

		// Token: 0x04000028 RID: 40
		internal const uint CERT_QUERY_CONTENT_FLAG_CTL = 4U;

		// Token: 0x04000029 RID: 41
		internal const uint CERT_QUERY_CONTENT_FLAG_CRL = 8U;

		// Token: 0x0400002A RID: 42
		internal const uint CERT_QUERY_CONTENT_FLAG_SERIALIZED_STORE = 16U;

		// Token: 0x0400002B RID: 43
		internal const uint CERT_QUERY_CONTENT_FLAG_SERIALIZED_CERT = 32U;

		// Token: 0x0400002C RID: 44
		internal const uint CERT_QUERY_CONTENT_FLAG_SERIALIZED_CTL = 64U;

		// Token: 0x0400002D RID: 45
		internal const uint CERT_QUERY_CONTENT_FLAG_SERIALIZED_CRL = 128U;

		// Token: 0x0400002E RID: 46
		internal const uint CERT_QUERY_CONTENT_FLAG_PKCS7_SIGNED = 256U;

		// Token: 0x0400002F RID: 47
		internal const uint CERT_QUERY_CONTENT_FLAG_PKCS7_UNSIGNED = 512U;

		// Token: 0x04000030 RID: 48
		internal const uint CERT_QUERY_CONTENT_FLAG_PKCS7_SIGNED_EMBED = 1024U;

		// Token: 0x04000031 RID: 49
		internal const uint CERT_QUERY_CONTENT_FLAG_PKCS10 = 2048U;

		// Token: 0x04000032 RID: 50
		internal const uint CERT_QUERY_CONTENT_FLAG_PFX = 4096U;

		// Token: 0x04000033 RID: 51
		internal const uint CERT_QUERY_CONTENT_FLAG_CERT_PAIR = 8192U;

		// Token: 0x04000034 RID: 52
		internal const uint CERT_QUERY_CONTENT_FLAG_ALL = 16382U;

		// Token: 0x04000035 RID: 53
		internal const uint CERT_QUERY_FORMAT_BINARY = 1U;

		// Token: 0x04000036 RID: 54
		internal const uint CERT_QUERY_FORMAT_BASE64_ENCODED = 2U;

		// Token: 0x04000037 RID: 55
		internal const uint CERT_QUERY_FORMAT_ASN_ASCII_HEX_ENCODED = 3U;

		// Token: 0x04000038 RID: 56
		internal const uint CERT_QUERY_FORMAT_FLAG_BINARY = 2U;

		// Token: 0x04000039 RID: 57
		internal const uint CERT_QUERY_FORMAT_FLAG_BASE64_ENCODED = 4U;

		// Token: 0x0400003A RID: 58
		internal const uint CERT_QUERY_FORMAT_FLAG_ASN_ASCII_HEX_ENCODED = 8U;

		// Token: 0x0400003B RID: 59
		internal const uint CERT_QUERY_FORMAT_FLAG_ALL = 14U;

		// Token: 0x0400003C RID: 60
		internal const uint CRYPTPROTECT_UI_FORBIDDEN = 1U;

		// Token: 0x0400003D RID: 61
		internal const uint CRYPTPROTECT_LOCAL_MACHINE = 4U;

		// Token: 0x0400003E RID: 62
		internal const uint CRYPTPROTECT_CRED_SYNC = 8U;

		// Token: 0x0400003F RID: 63
		internal const uint CRYPTPROTECT_AUDIT = 16U;

		// Token: 0x04000040 RID: 64
		internal const uint CRYPTPROTECT_NO_RECOVERY = 32U;

		// Token: 0x04000041 RID: 65
		internal const uint CRYPTPROTECT_VERIFY_PROTECTION = 64U;

		// Token: 0x04000042 RID: 66
		internal const uint CRYPTPROTECTMEMORY_BLOCK_SIZE = 16U;

		// Token: 0x04000043 RID: 67
		internal const uint CRYPTPROTECTMEMORY_SAME_PROCESS = 0U;

		// Token: 0x04000044 RID: 68
		internal const uint CRYPTPROTECTMEMORY_CROSS_PROCESS = 1U;

		// Token: 0x04000045 RID: 69
		internal const uint CRYPTPROTECTMEMORY_SAME_LOGON = 2U;

		// Token: 0x04000046 RID: 70
		internal const uint CRYPT_OID_INFO_OID_KEY = 1U;

		// Token: 0x04000047 RID: 71
		internal const uint CRYPT_OID_INFO_NAME_KEY = 2U;

		// Token: 0x04000048 RID: 72
		internal const uint CRYPT_OID_INFO_ALGID_KEY = 3U;

		// Token: 0x04000049 RID: 73
		internal const uint CRYPT_OID_INFO_SIGN_KEY = 4U;

		// Token: 0x0400004A RID: 74
		internal const uint CRYPT_HASH_ALG_OID_GROUP_ID = 1U;

		// Token: 0x0400004B RID: 75
		internal const uint CRYPT_ENCRYPT_ALG_OID_GROUP_ID = 2U;

		// Token: 0x0400004C RID: 76
		internal const uint CRYPT_PUBKEY_ALG_OID_GROUP_ID = 3U;

		// Token: 0x0400004D RID: 77
		internal const uint CRYPT_SIGN_ALG_OID_GROUP_ID = 4U;

		// Token: 0x0400004E RID: 78
		internal const uint CRYPT_RDN_ATTR_OID_GROUP_ID = 5U;

		// Token: 0x0400004F RID: 79
		internal const uint CRYPT_EXT_OR_ATTR_OID_GROUP_ID = 6U;

		// Token: 0x04000050 RID: 80
		internal const uint CRYPT_ENHKEY_USAGE_OID_GROUP_ID = 7U;

		// Token: 0x04000051 RID: 81
		internal const uint CRYPT_POLICY_OID_GROUP_ID = 8U;

		// Token: 0x04000052 RID: 82
		internal const uint CRYPT_TEMPLATE_OID_GROUP_ID = 9U;

		// Token: 0x04000053 RID: 83
		internal const uint CRYPT_LAST_OID_GROUP_ID = 9U;

		// Token: 0x04000054 RID: 84
		internal const uint CRYPT_FIRST_ALG_OID_GROUP_ID = 1U;

		// Token: 0x04000055 RID: 85
		internal const uint CRYPT_LAST_ALG_OID_GROUP_ID = 4U;

		// Token: 0x04000056 RID: 86
		internal const uint CRYPT_ASN_ENCODING = 1U;

		// Token: 0x04000057 RID: 87
		internal const uint CRYPT_NDR_ENCODING = 2U;

		// Token: 0x04000058 RID: 88
		internal const uint X509_ASN_ENCODING = 1U;

		// Token: 0x04000059 RID: 89
		internal const uint X509_NDR_ENCODING = 2U;

		// Token: 0x0400005A RID: 90
		internal const uint PKCS_7_ASN_ENCODING = 65536U;

		// Token: 0x0400005B RID: 91
		internal const uint PKCS_7_NDR_ENCODING = 131072U;

		// Token: 0x0400005C RID: 92
		internal const uint PKCS_7_OR_X509_ASN_ENCODING = 65537U;

		// Token: 0x0400005D RID: 93
		internal const uint CERT_STORE_PROV_MSG = 1U;

		// Token: 0x0400005E RID: 94
		internal const uint CERT_STORE_PROV_MEMORY = 2U;

		// Token: 0x0400005F RID: 95
		internal const uint CERT_STORE_PROV_FILE = 3U;

		// Token: 0x04000060 RID: 96
		internal const uint CERT_STORE_PROV_REG = 4U;

		// Token: 0x04000061 RID: 97
		internal const uint CERT_STORE_PROV_PKCS7 = 5U;

		// Token: 0x04000062 RID: 98
		internal const uint CERT_STORE_PROV_SERIALIZED = 6U;

		// Token: 0x04000063 RID: 99
		internal const uint CERT_STORE_PROV_FILENAME_A = 7U;

		// Token: 0x04000064 RID: 100
		internal const uint CERT_STORE_PROV_FILENAME_W = 8U;

		// Token: 0x04000065 RID: 101
		internal const uint CERT_STORE_PROV_FILENAME = 8U;

		// Token: 0x04000066 RID: 102
		internal const uint CERT_STORE_PROV_SYSTEM_A = 9U;

		// Token: 0x04000067 RID: 103
		internal const uint CERT_STORE_PROV_SYSTEM_W = 10U;

		// Token: 0x04000068 RID: 104
		internal const uint CERT_STORE_PROV_SYSTEM = 10U;

		// Token: 0x04000069 RID: 105
		internal const uint CERT_STORE_PROV_COLLECTION = 11U;

		// Token: 0x0400006A RID: 106
		internal const uint CERT_STORE_PROV_SYSTEM_REGISTRY_A = 12U;

		// Token: 0x0400006B RID: 107
		internal const uint CERT_STORE_PROV_SYSTEM_REGISTRY_W = 13U;

		// Token: 0x0400006C RID: 108
		internal const uint CERT_STORE_PROV_SYSTEM_REGISTRY = 13U;

		// Token: 0x0400006D RID: 109
		internal const uint CERT_STORE_PROV_PHYSICAL_W = 14U;

		// Token: 0x0400006E RID: 110
		internal const uint CERT_STORE_PROV_PHYSICAL = 14U;

		// Token: 0x0400006F RID: 111
		internal const uint CERT_STORE_PROV_SMART_CARD_W = 15U;

		// Token: 0x04000070 RID: 112
		internal const uint CERT_STORE_PROV_SMART_CARD = 15U;

		// Token: 0x04000071 RID: 113
		internal const uint CERT_STORE_PROV_LDAP_W = 16U;

		// Token: 0x04000072 RID: 114
		internal const uint CERT_STORE_PROV_LDAP = 16U;

		// Token: 0x04000073 RID: 115
		internal const uint CERT_STORE_NO_CRYPT_RELEASE_FLAG = 1U;

		// Token: 0x04000074 RID: 116
		internal const uint CERT_STORE_SET_LOCALIZED_NAME_FLAG = 2U;

		// Token: 0x04000075 RID: 117
		internal const uint CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG = 4U;

		// Token: 0x04000076 RID: 118
		internal const uint CERT_STORE_DELETE_FLAG = 16U;

		// Token: 0x04000077 RID: 119
		internal const uint CERT_STORE_SHARE_STORE_FLAG = 64U;

		// Token: 0x04000078 RID: 120
		internal const uint CERT_STORE_SHARE_CONTEXT_FLAG = 128U;

		// Token: 0x04000079 RID: 121
		internal const uint CERT_STORE_MANIFOLD_FLAG = 256U;

		// Token: 0x0400007A RID: 122
		internal const uint CERT_STORE_ENUM_ARCHIVED_FLAG = 512U;

		// Token: 0x0400007B RID: 123
		internal const uint CERT_STORE_UPDATE_KEYID_FLAG = 1024U;

		// Token: 0x0400007C RID: 124
		internal const uint CERT_STORE_BACKUP_RESTORE_FLAG = 2048U;

		// Token: 0x0400007D RID: 125
		internal const uint CERT_STORE_READONLY_FLAG = 32768U;

		// Token: 0x0400007E RID: 126
		internal const uint CERT_STORE_OPEN_EXISTING_FLAG = 16384U;

		// Token: 0x0400007F RID: 127
		internal const uint CERT_STORE_CREATE_NEW_FLAG = 8192U;

		// Token: 0x04000080 RID: 128
		internal const uint CERT_STORE_MAXIMUM_ALLOWED_FLAG = 4096U;

		// Token: 0x04000081 RID: 129
		internal const uint CERT_SYSTEM_STORE_UNPROTECTED_FLAG = 1073741824U;

		// Token: 0x04000082 RID: 130
		internal const uint CERT_SYSTEM_STORE_LOCATION_MASK = 16711680U;

		// Token: 0x04000083 RID: 131
		internal const uint CERT_SYSTEM_STORE_LOCATION_SHIFT = 16U;

		// Token: 0x04000084 RID: 132
		internal const uint CERT_SYSTEM_STORE_CURRENT_USER_ID = 1U;

		// Token: 0x04000085 RID: 133
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE_ID = 2U;

		// Token: 0x04000086 RID: 134
		internal const uint CERT_SYSTEM_STORE_CURRENT_SERVICE_ID = 4U;

		// Token: 0x04000087 RID: 135
		internal const uint CERT_SYSTEM_STORE_SERVICES_ID = 5U;

		// Token: 0x04000088 RID: 136
		internal const uint CERT_SYSTEM_STORE_USERS_ID = 6U;

		// Token: 0x04000089 RID: 137
		internal const uint CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY_ID = 7U;

		// Token: 0x0400008A RID: 138
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY_ID = 8U;

		// Token: 0x0400008B RID: 139
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE_ID = 9U;

		// Token: 0x0400008C RID: 140
		internal const uint CERT_SYSTEM_STORE_CURRENT_USER = 65536U;

		// Token: 0x0400008D RID: 141
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE = 131072U;

		// Token: 0x0400008E RID: 142
		internal const uint CERT_SYSTEM_STORE_CURRENT_SERVICE = 262144U;

		// Token: 0x0400008F RID: 143
		internal const uint CERT_SYSTEM_STORE_SERVICES = 327680U;

		// Token: 0x04000090 RID: 144
		internal const uint CERT_SYSTEM_STORE_USERS = 393216U;

		// Token: 0x04000091 RID: 145
		internal const uint CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY = 458752U;

		// Token: 0x04000092 RID: 146
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY = 524288U;

		// Token: 0x04000093 RID: 147
		internal const uint CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE = 589824U;

		// Token: 0x04000094 RID: 148
		internal const uint CERT_NAME_EMAIL_TYPE = 1U;

		// Token: 0x04000095 RID: 149
		internal const uint CERT_NAME_RDN_TYPE = 2U;

		// Token: 0x04000096 RID: 150
		internal const uint CERT_NAME_ATTR_TYPE = 3U;

		// Token: 0x04000097 RID: 151
		internal const uint CERT_NAME_SIMPLE_DISPLAY_TYPE = 4U;

		// Token: 0x04000098 RID: 152
		internal const uint CERT_NAME_FRIENDLY_DISPLAY_TYPE = 5U;

		// Token: 0x04000099 RID: 153
		internal const uint CERT_NAME_DNS_TYPE = 6U;

		// Token: 0x0400009A RID: 154
		internal const uint CERT_NAME_URL_TYPE = 7U;

		// Token: 0x0400009B RID: 155
		internal const uint CERT_NAME_UPN_TYPE = 8U;

		// Token: 0x0400009C RID: 156
		internal const uint CERT_SIMPLE_NAME_STR = 1U;

		// Token: 0x0400009D RID: 157
		internal const uint CERT_OID_NAME_STR = 2U;

		// Token: 0x0400009E RID: 158
		internal const uint CERT_X500_NAME_STR = 3U;

		// Token: 0x0400009F RID: 159
		internal const uint CERT_NAME_STR_SEMICOLON_FLAG = 1073741824U;

		// Token: 0x040000A0 RID: 160
		internal const uint CERT_NAME_STR_NO_PLUS_FLAG = 536870912U;

		// Token: 0x040000A1 RID: 161
		internal const uint CERT_NAME_STR_NO_QUOTING_FLAG = 268435456U;

		// Token: 0x040000A2 RID: 162
		internal const uint CERT_NAME_STR_CRLF_FLAG = 134217728U;

		// Token: 0x040000A3 RID: 163
		internal const uint CERT_NAME_STR_COMMA_FLAG = 67108864U;

		// Token: 0x040000A4 RID: 164
		internal const uint CERT_NAME_STR_REVERSE_FLAG = 33554432U;

		// Token: 0x040000A5 RID: 165
		internal const uint CERT_NAME_ISSUER_FLAG = 1U;

		// Token: 0x040000A6 RID: 166
		internal const uint CERT_NAME_STR_DISABLE_IE4_UTF8_FLAG = 65536U;

		// Token: 0x040000A7 RID: 167
		internal const uint CERT_NAME_STR_ENABLE_T61_UNICODE_FLAG = 131072U;

		// Token: 0x040000A8 RID: 168
		internal const uint CERT_NAME_STR_ENABLE_UTF8_UNICODE_FLAG = 262144U;

		// Token: 0x040000A9 RID: 169
		internal const uint CERT_NAME_STR_FORCE_UTF8_DIR_STR_FLAG = 524288U;

		// Token: 0x040000AA RID: 170
		internal const uint CERT_KEY_PROV_HANDLE_PROP_ID = 1U;

		// Token: 0x040000AB RID: 171
		internal const uint CERT_KEY_PROV_INFO_PROP_ID = 2U;

		// Token: 0x040000AC RID: 172
		internal const uint CERT_SHA1_HASH_PROP_ID = 3U;

		// Token: 0x040000AD RID: 173
		internal const uint CERT_MD5_HASH_PROP_ID = 4U;

		// Token: 0x040000AE RID: 174
		internal const uint CERT_HASH_PROP_ID = 3U;

		// Token: 0x040000AF RID: 175
		internal const uint CERT_KEY_CONTEXT_PROP_ID = 5U;

		// Token: 0x040000B0 RID: 176
		internal const uint CERT_KEY_SPEC_PROP_ID = 6U;

		// Token: 0x040000B1 RID: 177
		internal const uint CERT_IE30_RESERVED_PROP_ID = 7U;

		// Token: 0x040000B2 RID: 178
		internal const uint CERT_PUBKEY_HASH_RESERVED_PROP_ID = 8U;

		// Token: 0x040000B3 RID: 179
		internal const uint CERT_ENHKEY_USAGE_PROP_ID = 9U;

		// Token: 0x040000B4 RID: 180
		internal const uint CERT_CTL_USAGE_PROP_ID = 9U;

		// Token: 0x040000B5 RID: 181
		internal const uint CERT_NEXT_UPDATE_LOCATION_PROP_ID = 10U;

		// Token: 0x040000B6 RID: 182
		internal const uint CERT_FRIENDLY_NAME_PROP_ID = 11U;

		// Token: 0x040000B7 RID: 183
		internal const uint CERT_PVK_FILE_PROP_ID = 12U;

		// Token: 0x040000B8 RID: 184
		internal const uint CERT_DESCRIPTION_PROP_ID = 13U;

		// Token: 0x040000B9 RID: 185
		internal const uint CERT_ACCESS_STATE_PROP_ID = 14U;

		// Token: 0x040000BA RID: 186
		internal const uint CERT_SIGNATURE_HASH_PROP_ID = 15U;

		// Token: 0x040000BB RID: 187
		internal const uint CERT_SMART_CARD_DATA_PROP_ID = 16U;

		// Token: 0x040000BC RID: 188
		internal const uint CERT_EFS_PROP_ID = 17U;

		// Token: 0x040000BD RID: 189
		internal const uint CERT_FORTEZZA_DATA_PROP_ID = 18U;

		// Token: 0x040000BE RID: 190
		internal const uint CERT_ARCHIVED_PROP_ID = 19U;

		// Token: 0x040000BF RID: 191
		internal const uint CERT_KEY_IDENTIFIER_PROP_ID = 20U;

		// Token: 0x040000C0 RID: 192
		internal const uint CERT_AUTO_ENROLL_PROP_ID = 21U;

		// Token: 0x040000C1 RID: 193
		internal const uint CERT_PUBKEY_ALG_PARA_PROP_ID = 22U;

		// Token: 0x040000C2 RID: 194
		internal const uint CERT_CROSS_CERT_DIST_POINTS_PROP_ID = 23U;

		// Token: 0x040000C3 RID: 195
		internal const uint CERT_ISSUER_PUBLIC_KEY_MD5_HASH_PROP_ID = 24U;

		// Token: 0x040000C4 RID: 196
		internal const uint CERT_SUBJECT_PUBLIC_KEY_MD5_HASH_PROP_ID = 25U;

		// Token: 0x040000C5 RID: 197
		internal const uint CERT_ENROLLMENT_PROP_ID = 26U;

		// Token: 0x040000C6 RID: 198
		internal const uint CERT_DATE_STAMP_PROP_ID = 27U;

		// Token: 0x040000C7 RID: 199
		internal const uint CERT_ISSUER_SERIAL_NUMBER_MD5_HASH_PROP_ID = 28U;

		// Token: 0x040000C8 RID: 200
		internal const uint CERT_SUBJECT_NAME_MD5_HASH_PROP_ID = 29U;

		// Token: 0x040000C9 RID: 201
		internal const uint CERT_EXTENDED_ERROR_INFO_PROP_ID = 30U;

		// Token: 0x040000CA RID: 202
		internal const uint CERT_RENEWAL_PROP_ID = 64U;

		// Token: 0x040000CB RID: 203
		internal const uint CERT_ARCHIVED_KEY_HASH_PROP_ID = 65U;

		// Token: 0x040000CC RID: 204
		internal const uint CERT_FIRST_RESERVED_PROP_ID = 66U;

		// Token: 0x040000CD RID: 205
		internal const uint CERT_DELETE_KEYSET_PROP_ID = 101U;

		// Token: 0x040000CE RID: 206
		internal const uint CERT_INFO_VERSION_FLAG = 1U;

		// Token: 0x040000CF RID: 207
		internal const uint CERT_INFO_SERIAL_NUMBER_FLAG = 2U;

		// Token: 0x040000D0 RID: 208
		internal const uint CERT_INFO_SIGNATURE_ALGORITHM_FLAG = 3U;

		// Token: 0x040000D1 RID: 209
		internal const uint CERT_INFO_ISSUER_FLAG = 4U;

		// Token: 0x040000D2 RID: 210
		internal const uint CERT_INFO_NOT_BEFORE_FLAG = 5U;

		// Token: 0x040000D3 RID: 211
		internal const uint CERT_INFO_NOT_AFTER_FLAG = 6U;

		// Token: 0x040000D4 RID: 212
		internal const uint CERT_INFO_SUBJECT_FLAG = 7U;

		// Token: 0x040000D5 RID: 213
		internal const uint CERT_INFO_SUBJECT_PUBLIC_KEY_INFO_FLAG = 8U;

		// Token: 0x040000D6 RID: 214
		internal const uint CERT_INFO_ISSUER_UNIQUE_ID_FLAG = 9U;

		// Token: 0x040000D7 RID: 215
		internal const uint CERT_INFO_SUBJECT_UNIQUE_ID_FLAG = 10U;

		// Token: 0x040000D8 RID: 216
		internal const uint CERT_INFO_EXTENSION_FLAG = 11U;

		// Token: 0x040000D9 RID: 217
		internal const uint CERT_COMPARE_MASK = 65535U;

		// Token: 0x040000DA RID: 218
		internal const uint CERT_COMPARE_SHIFT = 16U;

		// Token: 0x040000DB RID: 219
		internal const uint CERT_COMPARE_ANY = 0U;

		// Token: 0x040000DC RID: 220
		internal const uint CERT_COMPARE_SHA1_HASH = 1U;

		// Token: 0x040000DD RID: 221
		internal const uint CERT_COMPARE_NAME = 2U;

		// Token: 0x040000DE RID: 222
		internal const uint CERT_COMPARE_ATTR = 3U;

		// Token: 0x040000DF RID: 223
		internal const uint CERT_COMPARE_MD5_HASH = 4U;

		// Token: 0x040000E0 RID: 224
		internal const uint CERT_COMPARE_PROPERTY = 5U;

		// Token: 0x040000E1 RID: 225
		internal const uint CERT_COMPARE_PUBLIC_KEY = 6U;

		// Token: 0x040000E2 RID: 226
		internal const uint CERT_COMPARE_HASH = 1U;

		// Token: 0x040000E3 RID: 227
		internal const uint CERT_COMPARE_NAME_STR_A = 7U;

		// Token: 0x040000E4 RID: 228
		internal const uint CERT_COMPARE_NAME_STR_W = 8U;

		// Token: 0x040000E5 RID: 229
		internal const uint CERT_COMPARE_KEY_SPEC = 9U;

		// Token: 0x040000E6 RID: 230
		internal const uint CERT_COMPARE_ENHKEY_USAGE = 10U;

		// Token: 0x040000E7 RID: 231
		internal const uint CERT_COMPARE_CTL_USAGE = 10U;

		// Token: 0x040000E8 RID: 232
		internal const uint CERT_COMPARE_SUBJECT_CERT = 11U;

		// Token: 0x040000E9 RID: 233
		internal const uint CERT_COMPARE_ISSUER_OF = 12U;

		// Token: 0x040000EA RID: 234
		internal const uint CERT_COMPARE_EXISTING = 13U;

		// Token: 0x040000EB RID: 235
		internal const uint CERT_COMPARE_SIGNATURE_HASH = 14U;

		// Token: 0x040000EC RID: 236
		internal const uint CERT_COMPARE_KEY_IDENTIFIER = 15U;

		// Token: 0x040000ED RID: 237
		internal const uint CERT_COMPARE_CERT_ID = 16U;

		// Token: 0x040000EE RID: 238
		internal const uint CERT_COMPARE_CROSS_CERT_DIST_POINTS = 17U;

		// Token: 0x040000EF RID: 239
		internal const uint CERT_COMPARE_PUBKEY_MD5_HASH = 18U;

		// Token: 0x040000F0 RID: 240
		internal const uint CERT_FIND_ANY = 0U;

		// Token: 0x040000F1 RID: 241
		internal const uint CERT_FIND_SHA1_HASH = 65536U;

		// Token: 0x040000F2 RID: 242
		internal const uint CERT_FIND_MD5_HASH = 262144U;

		// Token: 0x040000F3 RID: 243
		internal const uint CERT_FIND_SIGNATURE_HASH = 917504U;

		// Token: 0x040000F4 RID: 244
		internal const uint CERT_FIND_KEY_IDENTIFIER = 983040U;

		// Token: 0x040000F5 RID: 245
		internal const uint CERT_FIND_HASH = 65536U;

		// Token: 0x040000F6 RID: 246
		internal const uint CERT_FIND_PROPERTY = 327680U;

		// Token: 0x040000F7 RID: 247
		internal const uint CERT_FIND_PUBLIC_KEY = 393216U;

		// Token: 0x040000F8 RID: 248
		internal const uint CERT_FIND_SUBJECT_NAME = 131079U;

		// Token: 0x040000F9 RID: 249
		internal const uint CERT_FIND_SUBJECT_ATTR = 196615U;

		// Token: 0x040000FA RID: 250
		internal const uint CERT_FIND_ISSUER_NAME = 131076U;

		// Token: 0x040000FB RID: 251
		internal const uint CERT_FIND_ISSUER_ATTR = 196612U;

		// Token: 0x040000FC RID: 252
		internal const uint CERT_FIND_SUBJECT_STR_A = 458759U;

		// Token: 0x040000FD RID: 253
		internal const uint CERT_FIND_SUBJECT_STR_W = 524295U;

		// Token: 0x040000FE RID: 254
		internal const uint CERT_FIND_SUBJECT_STR = 524295U;

		// Token: 0x040000FF RID: 255
		internal const uint CERT_FIND_ISSUER_STR_A = 458756U;

		// Token: 0x04000100 RID: 256
		internal const uint CERT_FIND_ISSUER_STR_W = 524292U;

		// Token: 0x04000101 RID: 257
		internal const uint CERT_FIND_ISSUER_STR = 524292U;

		// Token: 0x04000102 RID: 258
		internal const uint CERT_FIND_KEY_SPEC = 589824U;

		// Token: 0x04000103 RID: 259
		internal const uint CERT_FIND_ENHKEY_USAGE = 655360U;

		// Token: 0x04000104 RID: 260
		internal const uint CERT_FIND_CTL_USAGE = 655360U;

		// Token: 0x04000105 RID: 261
		internal const uint CERT_FIND_SUBJECT_CERT = 720896U;

		// Token: 0x04000106 RID: 262
		internal const uint CERT_FIND_ISSUER_OF = 786432U;

		// Token: 0x04000107 RID: 263
		internal const uint CERT_FIND_EXISTING = 851968U;

		// Token: 0x04000108 RID: 264
		internal const uint CERT_FIND_CERT_ID = 1048576U;

		// Token: 0x04000109 RID: 265
		internal const uint CERT_FIND_CROSS_CERT_DIST_POINTS = 1114112U;

		// Token: 0x0400010A RID: 266
		internal const uint CERT_FIND_PUBKEY_MD5_HASH = 1179648U;

		// Token: 0x0400010B RID: 267
		internal const uint CERT_ENCIPHER_ONLY_KEY_USAGE = 1U;

		// Token: 0x0400010C RID: 268
		internal const uint CERT_CRL_SIGN_KEY_USAGE = 2U;

		// Token: 0x0400010D RID: 269
		internal const uint CERT_KEY_CERT_SIGN_KEY_USAGE = 4U;

		// Token: 0x0400010E RID: 270
		internal const uint CERT_KEY_AGREEMENT_KEY_USAGE = 8U;

		// Token: 0x0400010F RID: 271
		internal const uint CERT_DATA_ENCIPHERMENT_KEY_USAGE = 16U;

		// Token: 0x04000110 RID: 272
		internal const uint CERT_KEY_ENCIPHERMENT_KEY_USAGE = 32U;

		// Token: 0x04000111 RID: 273
		internal const uint CERT_NON_REPUDIATION_KEY_USAGE = 64U;

		// Token: 0x04000112 RID: 274
		internal const uint CERT_DIGITAL_SIGNATURE_KEY_USAGE = 128U;

		// Token: 0x04000113 RID: 275
		internal const uint CERT_DECIPHER_ONLY_KEY_USAGE = 32768U;

		// Token: 0x04000114 RID: 276
		internal const uint CERT_STORE_ADD_NEW = 1U;

		// Token: 0x04000115 RID: 277
		internal const uint CERT_STORE_ADD_USE_EXISTING = 2U;

		// Token: 0x04000116 RID: 278
		internal const uint CERT_STORE_ADD_REPLACE_EXISTING = 3U;

		// Token: 0x04000117 RID: 279
		internal const uint CERT_STORE_ADD_ALWAYS = 4U;

		// Token: 0x04000118 RID: 280
		internal const uint CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES = 5U;

		// Token: 0x04000119 RID: 281
		internal const uint CERT_STORE_ADD_NEWER = 6U;

		// Token: 0x0400011A RID: 282
		internal const uint CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES = 7U;

		// Token: 0x0400011B RID: 283
		internal const uint CERT_STORE_SAVE_AS_STORE = 1U;

		// Token: 0x0400011C RID: 284
		internal const uint CERT_STORE_SAVE_AS_PKCS7 = 2U;

		// Token: 0x0400011D RID: 285
		internal const uint CERT_STORE_SAVE_TO_FILE = 1U;

		// Token: 0x0400011E RID: 286
		internal const uint CERT_STORE_SAVE_TO_MEMORY = 2U;

		// Token: 0x0400011F RID: 287
		internal const uint CERT_STORE_SAVE_TO_FILENAME_A = 3U;

		// Token: 0x04000120 RID: 288
		internal const uint CERT_STORE_SAVE_TO_FILENAME_W = 4U;

		// Token: 0x04000121 RID: 289
		internal const uint CERT_STORE_SAVE_TO_FILENAME = 4U;

		// Token: 0x04000122 RID: 290
		internal const uint CERT_CA_SUBJECT_FLAG = 128U;

		// Token: 0x04000123 RID: 291
		internal const uint CERT_END_ENTITY_SUBJECT_FLAG = 64U;

		// Token: 0x04000124 RID: 292
		internal const uint RSA_CSP_PUBLICKEYBLOB = 19U;

		// Token: 0x04000125 RID: 293
		internal const uint X509_MULTI_BYTE_UINT = 38U;

		// Token: 0x04000126 RID: 294
		internal const uint X509_DSS_PUBLICKEY = 38U;

		// Token: 0x04000127 RID: 295
		internal const uint X509_DSS_PARAMETERS = 39U;

		// Token: 0x04000128 RID: 296
		internal const uint X509_DSS_SIGNATURE = 40U;

		// Token: 0x04000129 RID: 297
		internal const uint X509_EXTENSIONS = 5U;

		// Token: 0x0400012A RID: 298
		internal const uint X509_NAME_VALUE = 6U;

		// Token: 0x0400012B RID: 299
		internal const uint X509_NAME = 7U;

		// Token: 0x0400012C RID: 300
		internal const uint X509_AUTHORITY_KEY_ID = 9U;

		// Token: 0x0400012D RID: 301
		internal const uint X509_KEY_USAGE_RESTRICTION = 11U;

		// Token: 0x0400012E RID: 302
		internal const uint X509_BASIC_CONSTRAINTS = 13U;

		// Token: 0x0400012F RID: 303
		internal const uint X509_KEY_USAGE = 14U;

		// Token: 0x04000130 RID: 304
		internal const uint X509_BASIC_CONSTRAINTS2 = 15U;

		// Token: 0x04000131 RID: 305
		internal const uint X509_CERT_POLICIES = 16U;

		// Token: 0x04000132 RID: 306
		internal const uint PKCS_UTC_TIME = 17U;

		// Token: 0x04000133 RID: 307
		internal const uint PKCS_ATTRIBUTE = 22U;

		// Token: 0x04000134 RID: 308
		internal const uint X509_UNICODE_NAME_VALUE = 24U;

		// Token: 0x04000135 RID: 309
		internal const uint X509_OCTET_STRING = 25U;

		// Token: 0x04000136 RID: 310
		internal const uint X509_BITS = 26U;

		// Token: 0x04000137 RID: 311
		internal const uint X509_ANY_STRING = 6U;

		// Token: 0x04000138 RID: 312
		internal const uint X509_UNICODE_ANY_STRING = 24U;

		// Token: 0x04000139 RID: 313
		internal const uint X509_ENHANCED_KEY_USAGE = 36U;

		// Token: 0x0400013A RID: 314
		internal const uint PKCS_RC2_CBC_PARAMETERS = 41U;

		// Token: 0x0400013B RID: 315
		internal const uint X509_CERTIFICATE_TEMPLATE = 64U;

		// Token: 0x0400013C RID: 316
		internal const uint PKCS7_SIGNER_INFO = 500U;

		// Token: 0x0400013D RID: 317
		internal const uint CMS_SIGNER_INFO = 501U;

		// Token: 0x0400013E RID: 318
		internal const string szOID_AUTHORITY_KEY_IDENTIFIER = "2.5.29.1";

		// Token: 0x0400013F RID: 319
		internal const string szOID_KEY_USAGE_RESTRICTION = "2.5.29.4";

		// Token: 0x04000140 RID: 320
		internal const string szOID_KEY_USAGE = "2.5.29.15";

		// Token: 0x04000141 RID: 321
		internal const string szOID_KEYID_RDN = "1.3.6.1.4.1.311.10.7.1";

		// Token: 0x04000142 RID: 322
		internal const string szOID_RDN_DUMMY_SIGNER = "1.3.6.1.4.1.311.21.9";

		// Token: 0x04000143 RID: 323
		internal const uint CERT_CHAIN_POLICY_BASE = 1U;

		// Token: 0x04000144 RID: 324
		internal const uint CERT_CHAIN_POLICY_AUTHENTICODE = 2U;

		// Token: 0x04000145 RID: 325
		internal const uint CERT_CHAIN_POLICY_AUTHENTICODE_TS = 3U;

		// Token: 0x04000146 RID: 326
		internal const uint CERT_CHAIN_POLICY_SSL = 4U;

		// Token: 0x04000147 RID: 327
		internal const uint CERT_CHAIN_POLICY_BASIC_CONSTRAINTS = 5U;

		// Token: 0x04000148 RID: 328
		internal const uint CERT_CHAIN_POLICY_NT_AUTH = 6U;

		// Token: 0x04000149 RID: 329
		internal const uint CERT_CHAIN_POLICY_MICROSOFT_ROOT = 7U;

		// Token: 0x0400014A RID: 330
		internal const uint USAGE_MATCH_TYPE_AND = 0U;

		// Token: 0x0400014B RID: 331
		internal const uint USAGE_MATCH_TYPE_OR = 1U;

		// Token: 0x0400014C RID: 332
		internal const uint CERT_CHAIN_REVOCATION_CHECK_END_CERT = 268435456U;

		// Token: 0x0400014D RID: 333
		internal const uint CERT_CHAIN_REVOCATION_CHECK_CHAIN = 536870912U;

		// Token: 0x0400014E RID: 334
		internal const uint CERT_CHAIN_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT = 1073741824U;

		// Token: 0x0400014F RID: 335
		internal const uint CERT_CHAIN_REVOCATION_CHECK_CACHE_ONLY = 2147483648U;

		// Token: 0x04000150 RID: 336
		internal const uint CERT_CHAIN_REVOCATION_ACCUMULATIVE_TIMEOUT = 134217728U;

		// Token: 0x04000151 RID: 337
		internal const uint CERT_TRUST_NO_ERROR = 0U;

		// Token: 0x04000152 RID: 338
		internal const uint CERT_TRUST_IS_NOT_TIME_VALID = 1U;

		// Token: 0x04000153 RID: 339
		internal const uint CERT_TRUST_IS_NOT_TIME_NESTED = 2U;

		// Token: 0x04000154 RID: 340
		internal const uint CERT_TRUST_IS_REVOKED = 4U;

		// Token: 0x04000155 RID: 341
		internal const uint CERT_TRUST_IS_NOT_SIGNATURE_VALID = 8U;

		// Token: 0x04000156 RID: 342
		internal const uint CERT_TRUST_IS_NOT_VALID_FOR_USAGE = 16U;

		// Token: 0x04000157 RID: 343
		internal const uint CERT_TRUST_IS_UNTRUSTED_ROOT = 32U;

		// Token: 0x04000158 RID: 344
		internal const uint CERT_TRUST_REVOCATION_STATUS_UNKNOWN = 64U;

		// Token: 0x04000159 RID: 345
		internal const uint CERT_TRUST_IS_CYCLIC = 128U;

		// Token: 0x0400015A RID: 346
		internal const uint CERT_TRUST_INVALID_EXTENSION = 256U;

		// Token: 0x0400015B RID: 347
		internal const uint CERT_TRUST_INVALID_POLICY_CONSTRAINTS = 512U;

		// Token: 0x0400015C RID: 348
		internal const uint CERT_TRUST_INVALID_BASIC_CONSTRAINTS = 1024U;

		// Token: 0x0400015D RID: 349
		internal const uint CERT_TRUST_INVALID_NAME_CONSTRAINTS = 2048U;

		// Token: 0x0400015E RID: 350
		internal const uint CERT_TRUST_HAS_NOT_SUPPORTED_NAME_CONSTRAINT = 4096U;

		// Token: 0x0400015F RID: 351
		internal const uint CERT_TRUST_HAS_NOT_DEFINED_NAME_CONSTRAINT = 8192U;

		// Token: 0x04000160 RID: 352
		internal const uint CERT_TRUST_HAS_NOT_PERMITTED_NAME_CONSTRAINT = 16384U;

		// Token: 0x04000161 RID: 353
		internal const uint CERT_TRUST_HAS_EXCLUDED_NAME_CONSTRAINT = 32768U;

		// Token: 0x04000162 RID: 354
		internal const uint CERT_TRUST_IS_OFFLINE_REVOCATION = 16777216U;

		// Token: 0x04000163 RID: 355
		internal const uint CERT_TRUST_NO_ISSUANCE_CHAIN_POLICY = 33554432U;

		// Token: 0x04000164 RID: 356
		internal const uint CERT_TRUST_IS_PARTIAL_CHAIN = 65536U;

		// Token: 0x04000165 RID: 357
		internal const uint CERT_TRUST_CTL_IS_NOT_TIME_VALID = 131072U;

		// Token: 0x04000166 RID: 358
		internal const uint CERT_TRUST_CTL_IS_NOT_SIGNATURE_VALID = 262144U;

		// Token: 0x04000167 RID: 359
		internal const uint CERT_TRUST_CTL_IS_NOT_VALID_FOR_USAGE = 524288U;

		// Token: 0x04000168 RID: 360
		internal const uint CERT_CHAIN_POLICY_IGNORE_NOT_TIME_VALID_FLAG = 1U;

		// Token: 0x04000169 RID: 361
		internal const uint CERT_CHAIN_POLICY_IGNORE_CTL_NOT_TIME_VALID_FLAG = 2U;

		// Token: 0x0400016A RID: 362
		internal const uint CERT_CHAIN_POLICY_IGNORE_NOT_TIME_NESTED_FLAG = 4U;

		// Token: 0x0400016B RID: 363
		internal const uint CERT_CHAIN_POLICY_IGNORE_INVALID_BASIC_CONSTRAINTS_FLAG = 8U;

		// Token: 0x0400016C RID: 364
		internal const uint CERT_CHAIN_POLICY_ALLOW_UNKNOWN_CA_FLAG = 16U;

		// Token: 0x0400016D RID: 365
		internal const uint CERT_CHAIN_POLICY_IGNORE_WRONG_USAGE_FLAG = 32U;

		// Token: 0x0400016E RID: 366
		internal const uint CERT_CHAIN_POLICY_IGNORE_INVALID_NAME_FLAG = 64U;

		// Token: 0x0400016F RID: 367
		internal const uint CERT_CHAIN_POLICY_IGNORE_INVALID_POLICY_FLAG = 128U;

		// Token: 0x04000170 RID: 368
		internal const uint CERT_CHAIN_POLICY_IGNORE_END_REV_UNKNOWN_FLAG = 256U;

		// Token: 0x04000171 RID: 369
		internal const uint CERT_CHAIN_POLICY_IGNORE_CTL_SIGNER_REV_UNKNOWN_FLAG = 512U;

		// Token: 0x04000172 RID: 370
		internal const uint CERT_CHAIN_POLICY_IGNORE_CA_REV_UNKNOWN_FLAG = 1024U;

		// Token: 0x04000173 RID: 371
		internal const uint CERT_CHAIN_POLICY_IGNORE_ROOT_REV_UNKNOWN_FLAG = 2048U;

		// Token: 0x04000174 RID: 372
		internal const uint CERT_CHAIN_POLICY_IGNORE_ALL_REV_UNKNOWN_FLAGS = 3840U;

		// Token: 0x04000175 RID: 373
		internal const uint CERT_TRUST_HAS_EXACT_MATCH_ISSUER = 1U;

		// Token: 0x04000176 RID: 374
		internal const uint CERT_TRUST_HAS_KEY_MATCH_ISSUER = 2U;

		// Token: 0x04000177 RID: 375
		internal const uint CERT_TRUST_HAS_NAME_MATCH_ISSUER = 4U;

		// Token: 0x04000178 RID: 376
		internal const uint CERT_TRUST_IS_SELF_SIGNED = 8U;

		// Token: 0x04000179 RID: 377
		internal const uint CERT_TRUST_HAS_PREFERRED_ISSUER = 256U;

		// Token: 0x0400017A RID: 378
		internal const uint CERT_TRUST_HAS_ISSUANCE_CHAIN_POLICY = 512U;

		// Token: 0x0400017B RID: 379
		internal const uint CERT_TRUST_HAS_VALID_NAME_CONSTRAINTS = 1024U;

		// Token: 0x0400017C RID: 380
		internal const uint CERT_TRUST_IS_COMPLEX_CHAIN = 65536U;

		// Token: 0x0400017D RID: 381
		internal const string szOID_PKIX_NO_SIGNATURE = "1.3.6.1.5.5.7.6.2";

		// Token: 0x0400017E RID: 382
		internal const string szOID_PKIX_KP_SERVER_AUTH = "1.3.6.1.5.5.7.3.1";

		// Token: 0x0400017F RID: 383
		internal const string szOID_PKIX_KP_CLIENT_AUTH = "1.3.6.1.5.5.7.3.2";

		// Token: 0x04000180 RID: 384
		internal const string szOID_PKIX_KP_CODE_SIGNING = "1.3.6.1.5.5.7.3.3";

		// Token: 0x04000181 RID: 385
		internal const string szOID_PKIX_KP_EMAIL_PROTECTION = "1.3.6.1.5.5.7.3.4";

		// Token: 0x04000182 RID: 386
		internal const string SPC_INDIVIDUAL_SP_KEY_PURPOSE_OBJID = "1.3.6.1.4.1.311.2.1.21";

		// Token: 0x04000183 RID: 387
		internal const string SPC_COMMERCIAL_SP_KEY_PURPOSE_OBJID = "1.3.6.1.4.1.311.2.1.22";

		// Token: 0x04000184 RID: 388
		internal const uint HCCE_CURRENT_USER = 0U;

		// Token: 0x04000185 RID: 389
		internal const uint HCCE_LOCAL_MACHINE = 1U;

		// Token: 0x04000186 RID: 390
		internal const string szOID_PKCS_1 = "1.2.840.113549.1.1";

		// Token: 0x04000187 RID: 391
		internal const string szOID_PKCS_2 = "1.2.840.113549.1.2";

		// Token: 0x04000188 RID: 392
		internal const string szOID_PKCS_3 = "1.2.840.113549.1.3";

		// Token: 0x04000189 RID: 393
		internal const string szOID_PKCS_4 = "1.2.840.113549.1.4";

		// Token: 0x0400018A RID: 394
		internal const string szOID_PKCS_5 = "1.2.840.113549.1.5";

		// Token: 0x0400018B RID: 395
		internal const string szOID_PKCS_6 = "1.2.840.113549.1.6";

		// Token: 0x0400018C RID: 396
		internal const string szOID_PKCS_7 = "1.2.840.113549.1.7";

		// Token: 0x0400018D RID: 397
		internal const string szOID_PKCS_8 = "1.2.840.113549.1.8";

		// Token: 0x0400018E RID: 398
		internal const string szOID_PKCS_9 = "1.2.840.113549.1.9";

		// Token: 0x0400018F RID: 399
		internal const string szOID_PKCS_10 = "1.2.840.113549.1.10";

		// Token: 0x04000190 RID: 400
		internal const string szOID_PKCS_12 = "1.2.840.113549.1.12";

		// Token: 0x04000191 RID: 401
		internal const string szOID_RSA_data = "1.2.840.113549.1.7.1";

		// Token: 0x04000192 RID: 402
		internal const string szOID_RSA_signedData = "1.2.840.113549.1.7.2";

		// Token: 0x04000193 RID: 403
		internal const string szOID_RSA_envelopedData = "1.2.840.113549.1.7.3";

		// Token: 0x04000194 RID: 404
		internal const string szOID_RSA_signEnvData = "1.2.840.113549.1.7.4";

		// Token: 0x04000195 RID: 405
		internal const string szOID_RSA_digestedData = "1.2.840.113549.1.7.5";

		// Token: 0x04000196 RID: 406
		internal const string szOID_RSA_hashedData = "1.2.840.113549.1.7.5";

		// Token: 0x04000197 RID: 407
		internal const string szOID_RSA_encryptedData = "1.2.840.113549.1.7.6";

		// Token: 0x04000198 RID: 408
		internal const string szOID_RSA_emailAddr = "1.2.840.113549.1.9.1";

		// Token: 0x04000199 RID: 409
		internal const string szOID_RSA_unstructName = "1.2.840.113549.1.9.2";

		// Token: 0x0400019A RID: 410
		internal const string szOID_RSA_contentType = "1.2.840.113549.1.9.3";

		// Token: 0x0400019B RID: 411
		internal const string szOID_RSA_messageDigest = "1.2.840.113549.1.9.4";

		// Token: 0x0400019C RID: 412
		internal const string szOID_RSA_signingTime = "1.2.840.113549.1.9.5";

		// Token: 0x0400019D RID: 413
		internal const string szOID_RSA_counterSign = "1.2.840.113549.1.9.6";

		// Token: 0x0400019E RID: 414
		internal const string szOID_RSA_challengePwd = "1.2.840.113549.1.9.7";

		// Token: 0x0400019F RID: 415
		internal const string szOID_RSA_unstructAddr = "1.2.840.113549.1.9.8";

		// Token: 0x040001A0 RID: 416
		internal const string szOID_RSA_extCertAttrs = "1.2.840.113549.1.9.9";

		// Token: 0x040001A1 RID: 417
		internal const string szOID_RSA_SMIMECapabilities = "1.2.840.113549.1.9.15";

		// Token: 0x040001A2 RID: 418
		internal const string szOID_CAPICOM = "1.3.6.1.4.1.311.88";

		// Token: 0x040001A3 RID: 419
		internal const string szOID_CAPICOM_version = "1.3.6.1.4.1.311.88.1";

		// Token: 0x040001A4 RID: 420
		internal const string szOID_CAPICOM_attribute = "1.3.6.1.4.1.311.88.2";

		// Token: 0x040001A5 RID: 421
		internal const string szOID_CAPICOM_documentName = "1.3.6.1.4.1.311.88.2.1";

		// Token: 0x040001A6 RID: 422
		internal const string szOID_CAPICOM_documentDescription = "1.3.6.1.4.1.311.88.2.2";

		// Token: 0x040001A7 RID: 423
		internal const string szOID_CAPICOM_encryptedData = "1.3.6.1.4.1.311.88.3";

		// Token: 0x040001A8 RID: 424
		internal const string szOID_CAPICOM_encryptedContent = "1.3.6.1.4.1.311.88.3.1";

		// Token: 0x040001A9 RID: 425
		internal const string szOID_OIWSEC_sha1 = "1.3.14.3.2.26";

		// Token: 0x040001AA RID: 426
		internal const string szOID_RSA_MD5 = "1.2.840.113549.2.5";

		// Token: 0x040001AB RID: 427
		internal const string szOID_OIWSEC_SHA256 = "2.16.840.1.101.3.4.1";

		// Token: 0x040001AC RID: 428
		internal const string szOID_OIWSEC_SHA384 = "2.16.840.1.101.3.4.2";

		// Token: 0x040001AD RID: 429
		internal const string szOID_OIWSEC_SHA512 = "2.16.840.1.101.3.4.3";

		// Token: 0x040001AE RID: 430
		internal const string szOID_RSA_RC2CBC = "1.2.840.113549.3.2";

		// Token: 0x040001AF RID: 431
		internal const string szOID_RSA_RC4 = "1.2.840.113549.3.4";

		// Token: 0x040001B0 RID: 432
		internal const string szOID_RSA_DES_EDE3_CBC = "1.2.840.113549.3.7";

		// Token: 0x040001B1 RID: 433
		internal const string szOID_OIWSEC_desCBC = "1.3.14.3.2.7";

		// Token: 0x040001B2 RID: 434
		internal const string szOID_RSA_SMIMEalg = "1.2.840.113549.1.9.16.3";

		// Token: 0x040001B3 RID: 435
		internal const string szOID_RSA_SMIMEalgESDH = "1.2.840.113549.1.9.16.3.5";

		// Token: 0x040001B4 RID: 436
		internal const string szOID_RSA_SMIMEalgCMS3DESwrap = "1.2.840.113549.1.9.16.3.6";

		// Token: 0x040001B5 RID: 437
		internal const string szOID_RSA_SMIMEalgCMSRC2wrap = "1.2.840.113549.1.9.16.3.7";

		// Token: 0x040001B6 RID: 438
		internal const string szOID_X957_DSA = "1.2.840.10040.4.1";

		// Token: 0x040001B7 RID: 439
		internal const string szOID_X957_sha1DSA = "1.2.840.10040.4.3";

		// Token: 0x040001B8 RID: 440
		internal const string szOID_OIWSEC_sha1RSASign = "1.3.14.3.2.29";

		// Token: 0x040001B9 RID: 441
		internal const uint CERT_ALT_NAME_OTHER_NAME = 1U;

		// Token: 0x040001BA RID: 442
		internal const uint CERT_ALT_NAME_RFC822_NAME = 2U;

		// Token: 0x040001BB RID: 443
		internal const uint CERT_ALT_NAME_DNS_NAME = 3U;

		// Token: 0x040001BC RID: 444
		internal const uint CERT_ALT_NAME_X400_ADDRESS = 4U;

		// Token: 0x040001BD RID: 445
		internal const uint CERT_ALT_NAME_DIRECTORY_NAME = 5U;

		// Token: 0x040001BE RID: 446
		internal const uint CERT_ALT_NAME_EDI_PARTY_NAME = 6U;

		// Token: 0x040001BF RID: 447
		internal const uint CERT_ALT_NAME_URL = 7U;

		// Token: 0x040001C0 RID: 448
		internal const uint CERT_ALT_NAME_IP_ADDRESS = 8U;

		// Token: 0x040001C1 RID: 449
		internal const uint CERT_ALT_NAME_REGISTERED_ID = 9U;

		// Token: 0x040001C2 RID: 450
		internal const uint CERT_RDN_ANY_TYPE = 0U;

		// Token: 0x040001C3 RID: 451
		internal const uint CERT_RDN_ENCODED_BLOB = 1U;

		// Token: 0x040001C4 RID: 452
		internal const uint CERT_RDN_OCTET_STRING = 2U;

		// Token: 0x040001C5 RID: 453
		internal const uint CERT_RDN_NUMERIC_STRING = 3U;

		// Token: 0x040001C6 RID: 454
		internal const uint CERT_RDN_PRINTABLE_STRING = 4U;

		// Token: 0x040001C7 RID: 455
		internal const uint CERT_RDN_TELETEX_STRING = 5U;

		// Token: 0x040001C8 RID: 456
		internal const uint CERT_RDN_T61_STRING = 5U;

		// Token: 0x040001C9 RID: 457
		internal const uint CERT_RDN_VIDEOTEX_STRING = 6U;

		// Token: 0x040001CA RID: 458
		internal const uint CERT_RDN_IA5_STRING = 7U;

		// Token: 0x040001CB RID: 459
		internal const uint CERT_RDN_GRAPHIC_STRING = 8U;

		// Token: 0x040001CC RID: 460
		internal const uint CERT_RDN_VISIBLE_STRING = 9U;

		// Token: 0x040001CD RID: 461
		internal const uint CERT_RDN_ISO646_STRING = 9U;

		// Token: 0x040001CE RID: 462
		internal const uint CERT_RDN_GENERAL_STRING = 10U;

		// Token: 0x040001CF RID: 463
		internal const uint CERT_RDN_UNIVERSAL_STRING = 11U;

		// Token: 0x040001D0 RID: 464
		internal const uint CERT_RDN_INT4_STRING = 11U;

		// Token: 0x040001D1 RID: 465
		internal const uint CERT_RDN_BMP_STRING = 12U;

		// Token: 0x040001D2 RID: 466
		internal const uint CERT_RDN_UNICODE_STRING = 12U;

		// Token: 0x040001D3 RID: 467
		internal const uint CERT_RDN_UTF8_STRING = 13U;

		// Token: 0x040001D4 RID: 468
		internal const uint CERT_RDN_TYPE_MASK = 255U;

		// Token: 0x040001D5 RID: 469
		internal const uint CERT_RDN_FLAGS_MASK = 4278190080U;

		// Token: 0x040001D6 RID: 470
		internal const uint CERT_STORE_CTRL_RESYNC = 1U;

		// Token: 0x040001D7 RID: 471
		internal const uint CERT_STORE_CTRL_NOTIFY_CHANGE = 2U;

		// Token: 0x040001D8 RID: 472
		internal const uint CERT_STORE_CTRL_COMMIT = 3U;

		// Token: 0x040001D9 RID: 473
		internal const uint CERT_STORE_CTRL_AUTO_RESYNC = 4U;

		// Token: 0x040001DA RID: 474
		internal const uint CERT_STORE_CTRL_CANCEL_NOTIFY = 5U;

		// Token: 0x040001DB RID: 475
		internal const uint CERT_ID_ISSUER_SERIAL_NUMBER = 1U;

		// Token: 0x040001DC RID: 476
		internal const uint CERT_ID_KEY_IDENTIFIER = 2U;

		// Token: 0x040001DD RID: 477
		internal const uint CERT_ID_SHA1_HASH = 3U;

		// Token: 0x040001DE RID: 478
		internal const string MS_ENHANCED_PROV = "Microsoft Enhanced Cryptographic Provider v1.0";

		// Token: 0x040001DF RID: 479
		internal const string MS_STRONG_PROV = "Microsoft Strong Cryptographic Provider";

		// Token: 0x040001E0 RID: 480
		internal const string MS_DEF_PROV = "Microsoft Base Cryptographic Provider v1.0";

		// Token: 0x040001E1 RID: 481
		internal const string MS_DEF_DSS_DH_PROV = "Microsoft Base DSS and Diffie-Hellman Cryptographic Provider";

		// Token: 0x040001E2 RID: 482
		internal const string MS_ENH_DSS_DH_PROV = "Microsoft Enhanced DSS and Diffie-Hellman Cryptographic Provider";

		// Token: 0x040001E3 RID: 483
		internal const string DummySignerCommonName = "CN=Dummy Signer";

		// Token: 0x040001E4 RID: 484
		internal const uint PROV_RSA_FULL = 1U;

		// Token: 0x040001E5 RID: 485
		internal const uint PROV_DSS_DH = 13U;

		// Token: 0x040001E6 RID: 486
		internal const uint ALG_TYPE_ANY = 0U;

		// Token: 0x040001E7 RID: 487
		internal const uint ALG_TYPE_DSS = 512U;

		// Token: 0x040001E8 RID: 488
		internal const uint ALG_TYPE_RSA = 1024U;

		// Token: 0x040001E9 RID: 489
		internal const uint ALG_TYPE_BLOCK = 1536U;

		// Token: 0x040001EA RID: 490
		internal const uint ALG_TYPE_STREAM = 2048U;

		// Token: 0x040001EB RID: 491
		internal const uint ALG_TYPE_DH = 2560U;

		// Token: 0x040001EC RID: 492
		internal const uint ALG_TYPE_SECURECHANNEL = 3072U;

		// Token: 0x040001ED RID: 493
		internal const uint ALG_CLASS_ANY = 0U;

		// Token: 0x040001EE RID: 494
		internal const uint ALG_CLASS_SIGNATURE = 8192U;

		// Token: 0x040001EF RID: 495
		internal const uint ALG_CLASS_MSG_ENCRYPT = 16384U;

		// Token: 0x040001F0 RID: 496
		internal const uint ALG_CLASS_DATA_ENCRYPT = 24576U;

		// Token: 0x040001F1 RID: 497
		internal const uint ALG_CLASS_HASH = 32768U;

		// Token: 0x040001F2 RID: 498
		internal const uint ALG_CLASS_KEY_EXCHANGE = 40960U;

		// Token: 0x040001F3 RID: 499
		internal const uint ALG_CLASS_ALL = 57344U;

		// Token: 0x040001F4 RID: 500
		internal const uint ALG_SID_ANY = 0U;

		// Token: 0x040001F5 RID: 501
		internal const uint ALG_SID_RSA_ANY = 0U;

		// Token: 0x040001F6 RID: 502
		internal const uint ALG_SID_RSA_PKCS = 1U;

		// Token: 0x040001F7 RID: 503
		internal const uint ALG_SID_RSA_MSATWORK = 2U;

		// Token: 0x040001F8 RID: 504
		internal const uint ALG_SID_RSA_ENTRUST = 3U;

		// Token: 0x040001F9 RID: 505
		internal const uint ALG_SID_RSA_PGP = 4U;

		// Token: 0x040001FA RID: 506
		internal const uint ALG_SID_DSS_ANY = 0U;

		// Token: 0x040001FB RID: 507
		internal const uint ALG_SID_DSS_PKCS = 1U;

		// Token: 0x040001FC RID: 508
		internal const uint ALG_SID_DSS_DMS = 2U;

		// Token: 0x040001FD RID: 509
		internal const uint ALG_SID_DES = 1U;

		// Token: 0x040001FE RID: 510
		internal const uint ALG_SID_3DES = 3U;

		// Token: 0x040001FF RID: 511
		internal const uint ALG_SID_DESX = 4U;

		// Token: 0x04000200 RID: 512
		internal const uint ALG_SID_IDEA = 5U;

		// Token: 0x04000201 RID: 513
		internal const uint ALG_SID_CAST = 6U;

		// Token: 0x04000202 RID: 514
		internal const uint ALG_SID_SAFERSK64 = 7U;

		// Token: 0x04000203 RID: 515
		internal const uint ALG_SID_SAFERSK128 = 8U;

		// Token: 0x04000204 RID: 516
		internal const uint ALG_SID_3DES_112 = 9U;

		// Token: 0x04000205 RID: 517
		internal const uint ALG_SID_CYLINK_MEK = 12U;

		// Token: 0x04000206 RID: 518
		internal const uint ALG_SID_RC5 = 13U;

		// Token: 0x04000207 RID: 519
		internal const uint ALG_SID_AES_128 = 14U;

		// Token: 0x04000208 RID: 520
		internal const uint ALG_SID_AES_192 = 15U;

		// Token: 0x04000209 RID: 521
		internal const uint ALG_SID_AES_256 = 16U;

		// Token: 0x0400020A RID: 522
		internal const uint ALG_SID_AES = 17U;

		// Token: 0x0400020B RID: 523
		internal const uint ALG_SID_SKIPJACK = 10U;

		// Token: 0x0400020C RID: 524
		internal const uint ALG_SID_TEK = 11U;

		// Token: 0x0400020D RID: 525
		internal const uint ALG_SID_RC2 = 2U;

		// Token: 0x0400020E RID: 526
		internal const uint ALG_SID_RC4 = 1U;

		// Token: 0x0400020F RID: 527
		internal const uint ALG_SID_SEAL = 2U;

		// Token: 0x04000210 RID: 528
		internal const uint ALG_SID_DH_SANDF = 1U;

		// Token: 0x04000211 RID: 529
		internal const uint ALG_SID_DH_EPHEM = 2U;

		// Token: 0x04000212 RID: 530
		internal const uint ALG_SID_AGREED_KEY_ANY = 3U;

		// Token: 0x04000213 RID: 531
		internal const uint ALG_SID_KEA = 4U;

		// Token: 0x04000214 RID: 532
		internal const uint ALG_SID_MD2 = 1U;

		// Token: 0x04000215 RID: 533
		internal const uint ALG_SID_MD4 = 2U;

		// Token: 0x04000216 RID: 534
		internal const uint ALG_SID_MD5 = 3U;

		// Token: 0x04000217 RID: 535
		internal const uint ALG_SID_SHA = 4U;

		// Token: 0x04000218 RID: 536
		internal const uint ALG_SID_SHA1 = 4U;

		// Token: 0x04000219 RID: 537
		internal const uint ALG_SID_MAC = 5U;

		// Token: 0x0400021A RID: 538
		internal const uint ALG_SID_RIPEMD = 6U;

		// Token: 0x0400021B RID: 539
		internal const uint ALG_SID_RIPEMD160 = 7U;

		// Token: 0x0400021C RID: 540
		internal const uint ALG_SID_SSL3SHAMD5 = 8U;

		// Token: 0x0400021D RID: 541
		internal const uint ALG_SID_HMAC = 9U;

		// Token: 0x0400021E RID: 542
		internal const uint ALG_SID_TLS1PRF = 10U;

		// Token: 0x0400021F RID: 543
		internal const uint ALG_SID_HASH_REPLACE_OWF = 11U;

		// Token: 0x04000220 RID: 544
		internal const uint ALG_SID_SSL3_MASTER = 1U;

		// Token: 0x04000221 RID: 545
		internal const uint ALG_SID_SCHANNEL_MASTER_HASH = 2U;

		// Token: 0x04000222 RID: 546
		internal const uint ALG_SID_SCHANNEL_MAC_KEY = 3U;

		// Token: 0x04000223 RID: 547
		internal const uint ALG_SID_PCT1_MASTER = 4U;

		// Token: 0x04000224 RID: 548
		internal const uint ALG_SID_SSL2_MASTER = 5U;

		// Token: 0x04000225 RID: 549
		internal const uint ALG_SID_TLS1_MASTER = 6U;

		// Token: 0x04000226 RID: 550
		internal const uint ALG_SID_SCHANNEL_ENC_KEY = 7U;

		// Token: 0x04000227 RID: 551
		internal const uint CALG_MD2 = 32769U;

		// Token: 0x04000228 RID: 552
		internal const uint CALG_MD4 = 32770U;

		// Token: 0x04000229 RID: 553
		internal const uint CALG_MD5 = 32771U;

		// Token: 0x0400022A RID: 554
		internal const uint CALG_SHA = 32772U;

		// Token: 0x0400022B RID: 555
		internal const uint CALG_SHA1 = 32772U;

		// Token: 0x0400022C RID: 556
		internal const uint CALG_MAC = 32773U;

		// Token: 0x0400022D RID: 557
		internal const uint CALG_RSA_SIGN = 9216U;

		// Token: 0x0400022E RID: 558
		internal const uint CALG_DSS_SIGN = 8704U;

		// Token: 0x0400022F RID: 559
		internal const uint CALG_NO_SIGN = 8192U;

		// Token: 0x04000230 RID: 560
		internal const uint CALG_RSA_KEYX = 41984U;

		// Token: 0x04000231 RID: 561
		internal const uint CALG_DES = 26113U;

		// Token: 0x04000232 RID: 562
		internal const uint CALG_3DES_112 = 26121U;

		// Token: 0x04000233 RID: 563
		internal const uint CALG_3DES = 26115U;

		// Token: 0x04000234 RID: 564
		internal const uint CALG_DESX = 26116U;

		// Token: 0x04000235 RID: 565
		internal const uint CALG_RC2 = 26114U;

		// Token: 0x04000236 RID: 566
		internal const uint CALG_RC4 = 26625U;

		// Token: 0x04000237 RID: 567
		internal const uint CALG_SEAL = 26626U;

		// Token: 0x04000238 RID: 568
		internal const uint CALG_DH_SF = 43521U;

		// Token: 0x04000239 RID: 569
		internal const uint CALG_DH_EPHEM = 43522U;

		// Token: 0x0400023A RID: 570
		internal const uint CALG_AGREEDKEY_ANY = 43523U;

		// Token: 0x0400023B RID: 571
		internal const uint CALG_KEA_KEYX = 43524U;

		// Token: 0x0400023C RID: 572
		internal const uint CALG_HUGHES_MD5 = 40963U;

		// Token: 0x0400023D RID: 573
		internal const uint CALG_SKIPJACK = 26122U;

		// Token: 0x0400023E RID: 574
		internal const uint CALG_TEK = 26123U;

		// Token: 0x0400023F RID: 575
		internal const uint CALG_CYLINK_MEK = 26124U;

		// Token: 0x04000240 RID: 576
		internal const uint CALG_SSL3_SHAMD5 = 32776U;

		// Token: 0x04000241 RID: 577
		internal const uint CALG_SSL3_MASTER = 19457U;

		// Token: 0x04000242 RID: 578
		internal const uint CALG_SCHANNEL_MASTER_HASH = 19458U;

		// Token: 0x04000243 RID: 579
		internal const uint CALG_SCHANNEL_MAC_KEY = 19459U;

		// Token: 0x04000244 RID: 580
		internal const uint CALG_SCHANNEL_ENC_KEY = 19463U;

		// Token: 0x04000245 RID: 581
		internal const uint CALG_PCT1_MASTER = 19460U;

		// Token: 0x04000246 RID: 582
		internal const uint CALG_SSL2_MASTER = 19461U;

		// Token: 0x04000247 RID: 583
		internal const uint CALG_TLS1_MASTER = 19462U;

		// Token: 0x04000248 RID: 584
		internal const uint CALG_RC5 = 26125U;

		// Token: 0x04000249 RID: 585
		internal const uint CALG_HMAC = 32777U;

		// Token: 0x0400024A RID: 586
		internal const uint CALG_TLS1PRF = 32778U;

		// Token: 0x0400024B RID: 587
		internal const uint CALG_HASH_REPLACE_OWF = 32779U;

		// Token: 0x0400024C RID: 588
		internal const uint CALG_AES_128 = 26126U;

		// Token: 0x0400024D RID: 589
		internal const uint CALG_AES_192 = 26127U;

		// Token: 0x0400024E RID: 590
		internal const uint CALG_AES_256 = 26128U;

		// Token: 0x0400024F RID: 591
		internal const uint CALG_AES = 26129U;

		// Token: 0x04000250 RID: 592
		internal const uint CRYPT_FIRST = 1U;

		// Token: 0x04000251 RID: 593
		internal const uint CRYPT_NEXT = 2U;

		// Token: 0x04000252 RID: 594
		internal const uint PP_ENUMALGS_EX = 22U;

		// Token: 0x04000253 RID: 595
		internal const uint CRYPT_VERIFYCONTEXT = 4026531840U;

		// Token: 0x04000254 RID: 596
		internal const uint CRYPT_NEWKEYSET = 8U;

		// Token: 0x04000255 RID: 597
		internal const uint CRYPT_DELETEKEYSET = 16U;

		// Token: 0x04000256 RID: 598
		internal const uint CRYPT_MACHINE_KEYSET = 32U;

		// Token: 0x04000257 RID: 599
		internal const uint CRYPT_SILENT = 64U;

		// Token: 0x04000258 RID: 600
		internal const uint CRYPT_USER_KEYSET = 4096U;

		// Token: 0x04000259 RID: 601
		internal const uint CRYPT_EXPORTABLE = 1U;

		// Token: 0x0400025A RID: 602
		internal const uint CRYPT_USER_PROTECTED = 2U;

		// Token: 0x0400025B RID: 603
		internal const uint CRYPT_CREATE_SALT = 4U;

		// Token: 0x0400025C RID: 604
		internal const uint CRYPT_UPDATE_KEY = 8U;

		// Token: 0x0400025D RID: 605
		internal const uint CRYPT_NO_SALT = 16U;

		// Token: 0x0400025E RID: 606
		internal const uint CRYPT_PREGEN = 64U;

		// Token: 0x0400025F RID: 607
		internal const uint CRYPT_RECIPIENT = 16U;

		// Token: 0x04000260 RID: 608
		internal const uint CRYPT_INITIATOR = 64U;

		// Token: 0x04000261 RID: 609
		internal const uint CRYPT_ONLINE = 128U;

		// Token: 0x04000262 RID: 610
		internal const uint CRYPT_SF = 256U;

		// Token: 0x04000263 RID: 611
		internal const uint CRYPT_CREATE_IV = 512U;

		// Token: 0x04000264 RID: 612
		internal const uint CRYPT_KEK = 1024U;

		// Token: 0x04000265 RID: 613
		internal const uint CRYPT_DATA_KEY = 2048U;

		// Token: 0x04000266 RID: 614
		internal const uint CRYPT_VOLATILE = 4096U;

		// Token: 0x04000267 RID: 615
		internal const uint CRYPT_SGCKEY = 8192U;

		// Token: 0x04000268 RID: 616
		internal const uint CRYPT_ARCHIVABLE = 16384U;

		// Token: 0x04000269 RID: 617
		internal const byte CUR_BLOB_VERSION = 2;

		// Token: 0x0400026A RID: 618
		internal const byte SIMPLEBLOB = 1;

		// Token: 0x0400026B RID: 619
		internal const byte PUBLICKEYBLOB = 6;

		// Token: 0x0400026C RID: 620
		internal const byte PRIVATEKEYBLOB = 7;

		// Token: 0x0400026D RID: 621
		internal const byte PLAINTEXTKEYBLOB = 8;

		// Token: 0x0400026E RID: 622
		internal const byte OPAQUEKEYBLOB = 9;

		// Token: 0x0400026F RID: 623
		internal const byte PUBLICKEYBLOBEX = 10;

		// Token: 0x04000270 RID: 624
		internal const byte SYMMETRICWRAPKEYBLOB = 11;

		// Token: 0x04000271 RID: 625
		internal const uint DSS_MAGIC = 827544388U;

		// Token: 0x04000272 RID: 626
		internal const uint DSS_PRIVATE_MAGIC = 844321604U;

		// Token: 0x04000273 RID: 627
		internal const uint DSS_PUB_MAGIC_VER3 = 861098820U;

		// Token: 0x04000274 RID: 628
		internal const uint DSS_PRIV_MAGIC_VER3 = 877876036U;

		// Token: 0x04000275 RID: 629
		internal const uint RSA_PUB_MAGIC = 826364754U;

		// Token: 0x04000276 RID: 630
		internal const uint RSA_PRIV_MAGIC = 843141970U;

		// Token: 0x04000277 RID: 631
		internal const uint CRYPT_ACQUIRE_CACHE_FLAG = 1U;

		// Token: 0x04000278 RID: 632
		internal const uint CRYPT_ACQUIRE_USE_PROV_INFO_FLAG = 2U;

		// Token: 0x04000279 RID: 633
		internal const uint CRYPT_ACQUIRE_COMPARE_KEY_FLAG = 4U;

		// Token: 0x0400027A RID: 634
		internal const uint CRYPT_ACQUIRE_SILENT_FLAG = 64U;

		// Token: 0x0400027B RID: 635
		internal const uint CMSG_BARE_CONTENT_FLAG = 1U;

		// Token: 0x0400027C RID: 636
		internal const uint CMSG_LENGTH_ONLY_FLAG = 2U;

		// Token: 0x0400027D RID: 637
		internal const uint CMSG_DETACHED_FLAG = 4U;

		// Token: 0x0400027E RID: 638
		internal const uint CMSG_AUTHENTICATED_ATTRIBUTES_FLAG = 8U;

		// Token: 0x0400027F RID: 639
		internal const uint CMSG_CONTENTS_OCTETS_FLAG = 16U;

		// Token: 0x04000280 RID: 640
		internal const uint CMSG_MAX_LENGTH_FLAG = 32U;

		// Token: 0x04000281 RID: 641
		internal const uint CMSG_TYPE_PARAM = 1U;

		// Token: 0x04000282 RID: 642
		internal const uint CMSG_CONTENT_PARAM = 2U;

		// Token: 0x04000283 RID: 643
		internal const uint CMSG_BARE_CONTENT_PARAM = 3U;

		// Token: 0x04000284 RID: 644
		internal const uint CMSG_INNER_CONTENT_TYPE_PARAM = 4U;

		// Token: 0x04000285 RID: 645
		internal const uint CMSG_SIGNER_COUNT_PARAM = 5U;

		// Token: 0x04000286 RID: 646
		internal const uint CMSG_SIGNER_INFO_PARAM = 6U;

		// Token: 0x04000287 RID: 647
		internal const uint CMSG_SIGNER_CERT_INFO_PARAM = 7U;

		// Token: 0x04000288 RID: 648
		internal const uint CMSG_SIGNER_HASH_ALGORITHM_PARAM = 8U;

		// Token: 0x04000289 RID: 649
		internal const uint CMSG_SIGNER_AUTH_ATTR_PARAM = 9U;

		// Token: 0x0400028A RID: 650
		internal const uint CMSG_SIGNER_UNAUTH_ATTR_PARAM = 10U;

		// Token: 0x0400028B RID: 651
		internal const uint CMSG_CERT_COUNT_PARAM = 11U;

		// Token: 0x0400028C RID: 652
		internal const uint CMSG_CERT_PARAM = 12U;

		// Token: 0x0400028D RID: 653
		internal const uint CMSG_CRL_COUNT_PARAM = 13U;

		// Token: 0x0400028E RID: 654
		internal const uint CMSG_CRL_PARAM = 14U;

		// Token: 0x0400028F RID: 655
		internal const uint CMSG_ENVELOPE_ALGORITHM_PARAM = 15U;

		// Token: 0x04000290 RID: 656
		internal const uint CMSG_RECIPIENT_COUNT_PARAM = 17U;

		// Token: 0x04000291 RID: 657
		internal const uint CMSG_RECIPIENT_INDEX_PARAM = 18U;

		// Token: 0x04000292 RID: 658
		internal const uint CMSG_RECIPIENT_INFO_PARAM = 19U;

		// Token: 0x04000293 RID: 659
		internal const uint CMSG_HASH_ALGORITHM_PARAM = 20U;

		// Token: 0x04000294 RID: 660
		internal const uint CMSG_HASH_DATA_PARAM = 21U;

		// Token: 0x04000295 RID: 661
		internal const uint CMSG_COMPUTED_HASH_PARAM = 22U;

		// Token: 0x04000296 RID: 662
		internal const uint CMSG_ENCRYPT_PARAM = 26U;

		// Token: 0x04000297 RID: 663
		internal const uint CMSG_ENCRYPTED_DIGEST = 27U;

		// Token: 0x04000298 RID: 664
		internal const uint CMSG_ENCODED_SIGNER = 28U;

		// Token: 0x04000299 RID: 665
		internal const uint CMSG_ENCODED_MESSAGE = 29U;

		// Token: 0x0400029A RID: 666
		internal const uint CMSG_VERSION_PARAM = 30U;

		// Token: 0x0400029B RID: 667
		internal const uint CMSG_ATTR_CERT_COUNT_PARAM = 31U;

		// Token: 0x0400029C RID: 668
		internal const uint CMSG_ATTR_CERT_PARAM = 32U;

		// Token: 0x0400029D RID: 669
		internal const uint CMSG_CMS_RECIPIENT_COUNT_PARAM = 33U;

		// Token: 0x0400029E RID: 670
		internal const uint CMSG_CMS_RECIPIENT_INDEX_PARAM = 34U;

		// Token: 0x0400029F RID: 671
		internal const uint CMSG_CMS_RECIPIENT_ENCRYPTED_KEY_INDEX_PARAM = 35U;

		// Token: 0x040002A0 RID: 672
		internal const uint CMSG_CMS_RECIPIENT_INFO_PARAM = 36U;

		// Token: 0x040002A1 RID: 673
		internal const uint CMSG_UNPROTECTED_ATTR_PARAM = 37U;

		// Token: 0x040002A2 RID: 674
		internal const uint CMSG_SIGNER_CERT_ID_PARAM = 38U;

		// Token: 0x040002A3 RID: 675
		internal const uint CMSG_CMS_SIGNER_INFO_PARAM = 39U;

		// Token: 0x040002A4 RID: 676
		internal const uint CMSG_CTRL_VERIFY_SIGNATURE = 1U;

		// Token: 0x040002A5 RID: 677
		internal const uint CMSG_CTRL_DECRYPT = 2U;

		// Token: 0x040002A6 RID: 678
		internal const uint CMSG_CTRL_VERIFY_HASH = 5U;

		// Token: 0x040002A7 RID: 679
		internal const uint CMSG_CTRL_ADD_SIGNER = 6U;

		// Token: 0x040002A8 RID: 680
		internal const uint CMSG_CTRL_DEL_SIGNER = 7U;

		// Token: 0x040002A9 RID: 681
		internal const uint CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR = 8U;

		// Token: 0x040002AA RID: 682
		internal const uint CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR = 9U;

		// Token: 0x040002AB RID: 683
		internal const uint CMSG_CTRL_ADD_CERT = 10U;

		// Token: 0x040002AC RID: 684
		internal const uint CMSG_CTRL_DEL_CERT = 11U;

		// Token: 0x040002AD RID: 685
		internal const uint CMSG_CTRL_ADD_CRL = 12U;

		// Token: 0x040002AE RID: 686
		internal const uint CMSG_CTRL_DEL_CRL = 13U;

		// Token: 0x040002AF RID: 687
		internal const uint CMSG_CTRL_ADD_ATTR_CERT = 14U;

		// Token: 0x040002B0 RID: 688
		internal const uint CMSG_CTRL_DEL_ATTR_CERT = 15U;

		// Token: 0x040002B1 RID: 689
		internal const uint CMSG_CTRL_KEY_TRANS_DECRYPT = 16U;

		// Token: 0x040002B2 RID: 690
		internal const uint CMSG_CTRL_KEY_AGREE_DECRYPT = 17U;

		// Token: 0x040002B3 RID: 691
		internal const uint CMSG_CTRL_MAIL_LIST_DECRYPT = 18U;

		// Token: 0x040002B4 RID: 692
		internal const uint CMSG_CTRL_VERIFY_SIGNATURE_EX = 19U;

		// Token: 0x040002B5 RID: 693
		internal const uint CMSG_CTRL_ADD_CMS_SIGNER_INFO = 20U;

		// Token: 0x040002B6 RID: 694
		internal const uint CMSG_VERIFY_SIGNER_PUBKEY = 1U;

		// Token: 0x040002B7 RID: 695
		internal const uint CMSG_VERIFY_SIGNER_CERT = 2U;

		// Token: 0x040002B8 RID: 696
		internal const uint CMSG_VERIFY_SIGNER_CHAIN = 3U;

		// Token: 0x040002B9 RID: 697
		internal const uint CMSG_VERIFY_SIGNER_NULL = 4U;

		// Token: 0x040002BA RID: 698
		internal const uint CMSG_DATA = 1U;

		// Token: 0x040002BB RID: 699
		internal const uint CMSG_SIGNED = 2U;

		// Token: 0x040002BC RID: 700
		internal const uint CMSG_ENVELOPED = 3U;

		// Token: 0x040002BD RID: 701
		internal const uint CMSG_SIGNED_AND_ENVELOPED = 4U;

		// Token: 0x040002BE RID: 702
		internal const uint CMSG_HASHED = 5U;

		// Token: 0x040002BF RID: 703
		internal const uint CMSG_ENCRYPTED = 6U;

		// Token: 0x040002C0 RID: 704
		internal const uint CMSG_KEY_TRANS_RECIPIENT = 1U;

		// Token: 0x040002C1 RID: 705
		internal const uint CMSG_KEY_AGREE_RECIPIENT = 2U;

		// Token: 0x040002C2 RID: 706
		internal const uint CMSG_MAIL_LIST_RECIPIENT = 3U;

		// Token: 0x040002C3 RID: 707
		internal const uint CMSG_KEY_AGREE_ORIGINATOR_CERT = 1U;

		// Token: 0x040002C4 RID: 708
		internal const uint CMSG_KEY_AGREE_ORIGINATOR_PUBLIC_KEY = 2U;

		// Token: 0x040002C5 RID: 709
		internal const uint CMSG_KEY_AGREE_EPHEMERAL_KEY_CHOICE = 1U;

		// Token: 0x040002C6 RID: 710
		internal const uint CMSG_KEY_AGREE_STATIC_KEY_CHOICE = 2U;

		// Token: 0x040002C7 RID: 711
		internal const uint CMSG_ENVELOPED_RECIPIENT_V0 = 0U;

		// Token: 0x040002C8 RID: 712
		internal const uint CMSG_ENVELOPED_RECIPIENT_V2 = 2U;

		// Token: 0x040002C9 RID: 713
		internal const uint CMSG_ENVELOPED_RECIPIENT_V3 = 3U;

		// Token: 0x040002CA RID: 714
		internal const uint CMSG_ENVELOPED_RECIPIENT_V4 = 4U;

		// Token: 0x040002CB RID: 715
		internal const uint CMSG_KEY_TRANS_PKCS_1_5_VERSION = 0U;

		// Token: 0x040002CC RID: 716
		internal const uint CMSG_KEY_TRANS_CMS_VERSION = 2U;

		// Token: 0x040002CD RID: 717
		internal const uint CMSG_KEY_AGREE_VERSION = 3U;

		// Token: 0x040002CE RID: 718
		internal const uint CMSG_MAIL_LIST_VERSION = 4U;

		// Token: 0x040002CF RID: 719
		internal const uint CRYPT_RC2_40BIT_VERSION = 160U;

		// Token: 0x040002D0 RID: 720
		internal const uint CRYPT_RC2_56BIT_VERSION = 52U;

		// Token: 0x040002D1 RID: 721
		internal const uint CRYPT_RC2_64BIT_VERSION = 120U;

		// Token: 0x040002D2 RID: 722
		internal const uint CRYPT_RC2_128BIT_VERSION = 58U;

		// Token: 0x040002D3 RID: 723
		internal const int E_NOTIMPL = -2147483647;

		// Token: 0x040002D4 RID: 724
		internal const int E_OUTOFMEMORY = -2147024882;

		// Token: 0x040002D5 RID: 725
		internal const int NTE_NO_KEY = -2146893811;

		// Token: 0x040002D6 RID: 726
		internal const int NTE_BAD_PUBLIC_KEY = -2146893803;

		// Token: 0x040002D7 RID: 727
		internal const int NTE_BAD_KEYSET = -2146893802;

		// Token: 0x040002D8 RID: 728
		internal const int CRYPT_E_MSG_ERROR = -2146889727;

		// Token: 0x040002D9 RID: 729
		internal const int CRYPT_E_UNKNOWN_ALGO = -2146889726;

		// Token: 0x040002DA RID: 730
		internal const int CRYPT_E_INVALID_MSG_TYPE = -2146889724;

		// Token: 0x040002DB RID: 731
		internal const int CRYPT_E_RECIPIENT_NOT_FOUND = -2146889717;

		// Token: 0x040002DC RID: 732
		internal const int CRYPT_E_SIGNER_NOT_FOUND = -2146889714;

		// Token: 0x040002DD RID: 733
		internal const int CRYPT_E_ATTRIBUTES_MISSING = -2146889713;

		// Token: 0x040002DE RID: 734
		internal const int CRYPT_E_BAD_ENCODE = -2146885630;

		// Token: 0x040002DF RID: 735
		internal const int CRYPT_E_NOT_FOUND = -2146885628;

		// Token: 0x040002E0 RID: 736
		internal const int CRYPT_E_NO_MATCH = -2146885623;

		// Token: 0x040002E1 RID: 737
		internal const int CRYPT_E_NO_SIGNER = -2146885618;

		// Token: 0x040002E2 RID: 738
		internal const int CRYPT_E_REVOKED = -2146885616;

		// Token: 0x040002E3 RID: 739
		internal const int CRYPT_E_NO_REVOCATION_CHECK = -2146885614;

		// Token: 0x040002E4 RID: 740
		internal const int CRYPT_E_REVOCATION_OFFLINE = -2146885613;

		// Token: 0x040002E5 RID: 741
		internal const int CRYPT_E_ASN1_BADTAG = -2146881269;

		// Token: 0x040002E6 RID: 742
		internal const int TRUST_E_CERT_SIGNATURE = -2146869244;

		// Token: 0x040002E7 RID: 743
		internal const int TRUST_E_BASIC_CONSTRAINTS = -2146869223;

		// Token: 0x040002E8 RID: 744
		internal const int CERT_E_EXPIRED = -2146762495;

		// Token: 0x040002E9 RID: 745
		internal const int CERT_E_VALIDITYPERIODNESTING = -2146762494;

		// Token: 0x040002EA RID: 746
		internal const int CERT_E_UNTRUSTEDROOT = -2146762487;

		// Token: 0x040002EB RID: 747
		internal const int CERT_E_CHAINING = -2146762486;

		// Token: 0x040002EC RID: 748
		internal const int TRUST_E_FAIL = -2146762485;

		// Token: 0x040002ED RID: 749
		internal const int CERT_E_REVOKED = -2146762484;

		// Token: 0x040002EE RID: 750
		internal const int CERT_E_UNTRUSTEDTESTROOT = -2146762483;

		// Token: 0x040002EF RID: 751
		internal const int CERT_E_REVOCATION_FAILURE = -2146762482;

		// Token: 0x040002F0 RID: 752
		internal const int CERT_E_WRONG_USAGE = -2146762480;

		// Token: 0x040002F1 RID: 753
		internal const int CERT_E_INVALID_POLICY = -2146762477;

		// Token: 0x040002F2 RID: 754
		internal const int CERT_E_INVALID_NAME = -2146762476;

		// Token: 0x040002F3 RID: 755
		internal const int ERROR_SUCCESS = 0;

		// Token: 0x040002F4 RID: 756
		internal const int ERROR_CALL_NOT_IMPLEMENTED = 120;

		// Token: 0x040002F5 RID: 757
		internal const int ERROR_CANCELLED = 1223;

		// Token: 0x02000005 RID: 5
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct BLOBHEADER
		{
			// Token: 0x040002F6 RID: 758
			internal byte bType;

			// Token: 0x040002F7 RID: 759
			internal byte bVersion;

			// Token: 0x040002F8 RID: 760
			internal short reserved;

			// Token: 0x040002F9 RID: 761
			internal uint aiKeyAlg;
		}

		// Token: 0x02000006 RID: 6
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_ALT_NAME_INFO
		{
			// Token: 0x040002FA RID: 762
			internal uint cAltEntry;

			// Token: 0x040002FB RID: 763
			internal IntPtr rgAltEntry;
		}

		// Token: 0x02000007 RID: 7
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_BASIC_CONSTRAINTS_INFO
		{
			// Token: 0x040002FC RID: 764
			internal CAPIBase.CRYPT_BIT_BLOB SubjectType;

			// Token: 0x040002FD RID: 765
			internal bool fPathLenConstraint;

			// Token: 0x040002FE RID: 766
			internal uint dwPathLenConstraint;

			// Token: 0x040002FF RID: 767
			internal uint cSubtreesConstraint;

			// Token: 0x04000300 RID: 768
			internal IntPtr rgSubtreesConstraint;
		}

		// Token: 0x02000008 RID: 8
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_BASIC_CONSTRAINTS2_INFO
		{
			// Token: 0x04000301 RID: 769
			internal int fCA;

			// Token: 0x04000302 RID: 770
			internal int fPathLenConstraint;

			// Token: 0x04000303 RID: 771
			internal uint dwPathLenConstraint;
		}

		// Token: 0x02000009 RID: 9
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_CHAIN_PARA
		{
			// Token: 0x04000304 RID: 772
			internal uint cbSize;

			// Token: 0x04000305 RID: 773
			internal CAPIBase.CERT_USAGE_MATCH RequestedUsage;

			// Token: 0x04000306 RID: 774
			internal CAPIBase.CERT_USAGE_MATCH RequestedIssuancePolicy;

			// Token: 0x04000307 RID: 775
			internal uint dwUrlRetrievalTimeout;

			// Token: 0x04000308 RID: 776
			internal bool fCheckRevocationFreshnessTime;

			// Token: 0x04000309 RID: 777
			internal uint dwRevocationFreshnessTime;
		}

		// Token: 0x0200000A RID: 10
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_CHAIN_POLICY_PARA
		{
			// Token: 0x0600001F RID: 31 RVA: 0x00002A15 File Offset: 0x00001A15
			internal CERT_CHAIN_POLICY_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.dwFlags = 0U;
				this.pvExtraPolicyPara = IntPtr.Zero;
			}

			// Token: 0x0400030A RID: 778
			internal uint cbSize;

			// Token: 0x0400030B RID: 779
			internal uint dwFlags;

			// Token: 0x0400030C RID: 780
			internal IntPtr pvExtraPolicyPara;
		}

		// Token: 0x0200000B RID: 11
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_CHAIN_POLICY_STATUS
		{
			// Token: 0x06000020 RID: 32 RVA: 0x00002A30 File Offset: 0x00001A30
			internal CERT_CHAIN_POLICY_STATUS(int size)
			{
				this.cbSize = (uint)size;
				this.dwError = 0U;
				this.lChainIndex = IntPtr.Zero;
				this.lElementIndex = IntPtr.Zero;
				this.pvExtraPolicyStatus = IntPtr.Zero;
			}

			// Token: 0x0400030D RID: 781
			internal uint cbSize;

			// Token: 0x0400030E RID: 782
			internal uint dwError;

			// Token: 0x0400030F RID: 783
			internal IntPtr lChainIndex;

			// Token: 0x04000310 RID: 784
			internal IntPtr lElementIndex;

			// Token: 0x04000311 RID: 785
			internal IntPtr pvExtraPolicyStatus;
		}

		// Token: 0x0200000C RID: 12
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_CONTEXT
		{
			// Token: 0x04000312 RID: 786
			internal uint dwCertEncodingType;

			// Token: 0x04000313 RID: 787
			internal IntPtr pbCertEncoded;

			// Token: 0x04000314 RID: 788
			internal uint cbCertEncoded;

			// Token: 0x04000315 RID: 789
			internal IntPtr pCertInfo;

			// Token: 0x04000316 RID: 790
			internal IntPtr hCertStore;
		}

		// Token: 0x0200000D RID: 13
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_DSS_PARAMETERS
		{
			// Token: 0x04000317 RID: 791
			internal CAPIBase.CRYPTOAPI_BLOB p;

			// Token: 0x04000318 RID: 792
			internal CAPIBase.CRYPTOAPI_BLOB q;

			// Token: 0x04000319 RID: 793
			internal CAPIBase.CRYPTOAPI_BLOB g;
		}

		// Token: 0x0200000E RID: 14
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_ENHKEY_USAGE
		{
			// Token: 0x0400031A RID: 794
			internal uint cUsageIdentifier;

			// Token: 0x0400031B RID: 795
			internal IntPtr rgpszUsageIdentifier;
		}

		// Token: 0x0200000F RID: 15
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_EXTENSION
		{
			// Token: 0x0400031C RID: 796
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x0400031D RID: 797
			internal bool fCritical;

			// Token: 0x0400031E RID: 798
			internal CAPIBase.CRYPTOAPI_BLOB Value;
		}

		// Token: 0x02000010 RID: 16
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_ID
		{
			// Token: 0x0400031F RID: 799
			internal uint dwIdChoice;

			// Token: 0x04000320 RID: 800
			internal CAPIBase.CERT_ID_UNION Value;
		}

		// Token: 0x02000011 RID: 17
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		internal struct CERT_ID_UNION
		{
			// Token: 0x04000321 RID: 801
			[FieldOffset(0)]
			internal CAPIBase.CERT_ISSUER_SERIAL_NUMBER IssuerSerialNumber;

			// Token: 0x04000322 RID: 802
			[FieldOffset(0)]
			internal CAPIBase.CRYPTOAPI_BLOB KeyId;

			// Token: 0x04000323 RID: 803
			[FieldOffset(0)]
			internal CAPIBase.CRYPTOAPI_BLOB HashId;
		}

		// Token: 0x02000012 RID: 18
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_ISSUER_SERIAL_NUMBER
		{
			// Token: 0x04000324 RID: 804
			internal CAPIBase.CRYPTOAPI_BLOB Issuer;

			// Token: 0x04000325 RID: 805
			internal CAPIBase.CRYPTOAPI_BLOB SerialNumber;
		}

		// Token: 0x02000013 RID: 19
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_INFO
		{
			// Token: 0x04000326 RID: 806
			internal uint dwVersion;

			// Token: 0x04000327 RID: 807
			internal CAPIBase.CRYPTOAPI_BLOB SerialNumber;

			// Token: 0x04000328 RID: 808
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER SignatureAlgorithm;

			// Token: 0x04000329 RID: 809
			internal CAPIBase.CRYPTOAPI_BLOB Issuer;

			// Token: 0x0400032A RID: 810
			internal global::System.Runtime.InteropServices.ComTypes.FILETIME NotBefore;

			// Token: 0x0400032B RID: 811
			internal global::System.Runtime.InteropServices.ComTypes.FILETIME NotAfter;

			// Token: 0x0400032C RID: 812
			internal CAPIBase.CRYPTOAPI_BLOB Subject;

			// Token: 0x0400032D RID: 813
			internal CAPIBase.CERT_PUBLIC_KEY_INFO SubjectPublicKeyInfo;

			// Token: 0x0400032E RID: 814
			internal CAPIBase.CRYPT_BIT_BLOB IssuerUniqueId;

			// Token: 0x0400032F RID: 815
			internal CAPIBase.CRYPT_BIT_BLOB SubjectUniqueId;

			// Token: 0x04000330 RID: 816
			internal uint cExtension;

			// Token: 0x04000331 RID: 817
			internal IntPtr rgExtension;
		}

		// Token: 0x02000014 RID: 20
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_KEY_USAGE_RESTRICTION_INFO
		{
			// Token: 0x04000332 RID: 818
			internal uint cCertPolicyId;

			// Token: 0x04000333 RID: 819
			internal IntPtr rgCertPolicyId;

			// Token: 0x04000334 RID: 820
			internal CAPIBase.CRYPT_BIT_BLOB RestrictedKeyUsage;
		}

		// Token: 0x02000015 RID: 21
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_NAME_INFO
		{
			// Token: 0x04000335 RID: 821
			internal uint cRDN;

			// Token: 0x04000336 RID: 822
			internal IntPtr rgRDN;
		}

		// Token: 0x02000016 RID: 22
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_NAME_VALUE
		{
			// Token: 0x04000337 RID: 823
			internal uint dwValueType;

			// Token: 0x04000338 RID: 824
			internal CAPIBase.CRYPTOAPI_BLOB Value;
		}

		// Token: 0x02000017 RID: 23
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_OTHER_NAME
		{
			// Token: 0x04000339 RID: 825
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x0400033A RID: 826
			internal CAPIBase.CRYPTOAPI_BLOB Value;
		}

		// Token: 0x02000018 RID: 24
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_POLICY_ID
		{
			// Token: 0x0400033B RID: 827
			internal uint cCertPolicyElementId;

			// Token: 0x0400033C RID: 828
			internal IntPtr rgpszCertPolicyElementId;
		}

		// Token: 0x02000019 RID: 25
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_POLICIES_INFO
		{
			// Token: 0x0400033D RID: 829
			internal uint cPolicyInfo;

			// Token: 0x0400033E RID: 830
			internal IntPtr rgPolicyInfo;
		}

		// Token: 0x0200001A RID: 26
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_POLICY_INFO
		{
			// Token: 0x0400033F RID: 831
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszPolicyIdentifier;

			// Token: 0x04000340 RID: 832
			internal uint cPolicyQualifier;

			// Token: 0x04000341 RID: 833
			internal IntPtr rgPolicyQualifier;
		}

		// Token: 0x0200001B RID: 27
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_POLICY_QUALIFIER_INFO
		{
			// Token: 0x04000342 RID: 834
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszPolicyQualifierId;

			// Token: 0x04000343 RID: 835
			private CAPIBase.CRYPTOAPI_BLOB Qualifier;
		}

		// Token: 0x0200001C RID: 28
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_PUBLIC_KEY_INFO
		{
			// Token: 0x04000344 RID: 836
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER Algorithm;

			// Token: 0x04000345 RID: 837
			internal CAPIBase.CRYPT_BIT_BLOB PublicKey;
		}

		// Token: 0x0200001D RID: 29
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_PUBLIC_KEY_INFO2
		{
			// Token: 0x04000346 RID: 838
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER2 Algorithm;

			// Token: 0x04000347 RID: 839
			internal CAPIBase.CRYPT_BIT_BLOB PublicKey;
		}

		// Token: 0x0200001E RID: 30
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_RDN
		{
			// Token: 0x04000348 RID: 840
			internal uint cRDNAttr;

			// Token: 0x04000349 RID: 841
			internal IntPtr rgRDNAttr;
		}

		// Token: 0x0200001F RID: 31
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_RDN_ATTR
		{
			// Token: 0x0400034A RID: 842
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x0400034B RID: 843
			internal uint dwValueType;

			// Token: 0x0400034C RID: 844
			internal CAPIBase.CRYPTOAPI_BLOB Value;
		}

		// Token: 0x02000020 RID: 32
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_TEMPLATE_EXT
		{
			// Token: 0x0400034D RID: 845
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x0400034E RID: 846
			internal uint dwMajorVersion;

			// Token: 0x0400034F RID: 847
			private bool fMinorVersion;

			// Token: 0x04000350 RID: 848
			private uint dwMinorVersion;
		}

		// Token: 0x02000021 RID: 33
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_TRUST_STATUS
		{
			// Token: 0x04000351 RID: 849
			internal uint dwErrorStatus;

			// Token: 0x04000352 RID: 850
			internal uint dwInfoStatus;
		}

		// Token: 0x02000022 RID: 34
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CERT_USAGE_MATCH
		{
			// Token: 0x04000353 RID: 851
			internal uint dwType;

			// Token: 0x04000354 RID: 852
			internal CAPIBase.CERT_ENHKEY_USAGE Usage;
		}

		// Token: 0x02000023 RID: 35
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CMS_RECIPIENT_INFO
		{
			// Token: 0x04000355 RID: 853
			internal uint dwRecipientChoice;

			// Token: 0x04000356 RID: 854
			internal IntPtr pRecipientInfo;
		}

		// Token: 0x02000024 RID: 36
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CMS_SIGNER_INFO
		{
			// Token: 0x04000357 RID: 855
			internal uint dwVersion;

			// Token: 0x04000358 RID: 856
			internal CAPIBase.CERT_ID SignerId;

			// Token: 0x04000359 RID: 857
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;

			// Token: 0x0400035A RID: 858
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;

			// Token: 0x0400035B RID: 859
			internal CAPIBase.CRYPTOAPI_BLOB EncryptedHash;

			// Token: 0x0400035C RID: 860
			internal CAPIBase.CRYPT_ATTRIBUTES AuthAttrs;

			// Token: 0x0400035D RID: 861
			internal CAPIBase.CRYPT_ATTRIBUTES UnauthAttrs;
		}

		// Token: 0x02000025 RID: 37
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA
		{
			// Token: 0x06000021 RID: 33 RVA: 0x00002A61 File Offset: 0x00001A61
			internal CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.dwSignerIndex = 0U;
				this.blob = default(CAPIBase.CRYPTOAPI_BLOB);
			}

			// Token: 0x0400035E RID: 862
			internal uint cbSize;

			// Token: 0x0400035F RID: 863
			internal uint dwSignerIndex;

			// Token: 0x04000360 RID: 864
			internal CAPIBase.CRYPTOAPI_BLOB blob;
		}

		// Token: 0x02000026 RID: 38
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_DECRYPT_PARA
		{
			// Token: 0x06000022 RID: 34 RVA: 0x00002A7D File Offset: 0x00001A7D
			internal CMSG_CTRL_DECRYPT_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.hCryptProv = IntPtr.Zero;
				this.dwKeySpec = 0U;
				this.dwRecipientIndex = 0U;
			}

			// Token: 0x04000361 RID: 865
			internal uint cbSize;

			// Token: 0x04000362 RID: 866
			internal IntPtr hCryptProv;

			// Token: 0x04000363 RID: 867
			internal uint dwKeySpec;

			// Token: 0x04000364 RID: 868
			internal uint dwRecipientIndex;
		}

		// Token: 0x02000027 RID: 39
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA
		{
			// Token: 0x06000023 RID: 35 RVA: 0x00002A9F File Offset: 0x00001A9F
			internal CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.dwSignerIndex = 0U;
				this.dwUnauthAttrIndex = 0U;
			}

			// Token: 0x04000365 RID: 869
			internal uint cbSize;

			// Token: 0x04000366 RID: 870
			internal uint dwSignerIndex;

			// Token: 0x04000367 RID: 871
			internal uint dwUnauthAttrIndex;
		}

		// Token: 0x02000028 RID: 40
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_KEY_TRANS_DECRYPT_PARA
		{
			// Token: 0x04000368 RID: 872
			internal uint cbSize;

			// Token: 0x04000369 RID: 873
			internal SafeCryptProvHandle hCryptProv;

			// Token: 0x0400036A RID: 874
			internal uint dwKeySpec;

			// Token: 0x0400036B RID: 875
			internal IntPtr pKeyTrans;

			// Token: 0x0400036C RID: 876
			internal uint dwRecipientIndex;
		}

		// Token: 0x02000029 RID: 41
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO
		{
			// Token: 0x0400036D RID: 877
			internal uint cbSize;

			// Token: 0x0400036E RID: 878
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;

			// Token: 0x0400036F RID: 879
			internal IntPtr pvKeyEncryptionAuxInfo;

			// Token: 0x04000370 RID: 880
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyWrapAlgorithm;

			// Token: 0x04000371 RID: 881
			internal IntPtr pvKeyWrapAuxInfo;

			// Token: 0x04000372 RID: 882
			internal IntPtr hCryptProv;

			// Token: 0x04000373 RID: 883
			internal uint dwKeySpec;

			// Token: 0x04000374 RID: 884
			internal uint dwKeyChoice;

			// Token: 0x04000375 RID: 885
			internal IntPtr pEphemeralAlgorithmOrSenderId;

			// Token: 0x04000376 RID: 886
			internal CAPIBase.CRYPTOAPI_BLOB UserKeyingMaterial;

			// Token: 0x04000377 RID: 887
			internal uint cRecipientEncryptedKeys;

			// Token: 0x04000378 RID: 888
			internal IntPtr rgpRecipientEncryptedKeys;
		}

		// Token: 0x0200002A RID: 42
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO
		{
			// Token: 0x04000379 RID: 889
			internal uint cbSize;

			// Token: 0x0400037A RID: 890
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;

			// Token: 0x0400037B RID: 891
			internal IntPtr pvKeyEncryptionAuxInfo;

			// Token: 0x0400037C RID: 892
			internal IntPtr hCryptProv;

			// Token: 0x0400037D RID: 893
			internal CAPIBase.CRYPT_BIT_BLOB RecipientPublicKey;

			// Token: 0x0400037E RID: 894
			internal CAPIBase.CERT_ID RecipientId;
		}

		// Token: 0x0200002B RID: 43
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_RC2_AUX_INFO
		{
			// Token: 0x06000024 RID: 36 RVA: 0x00002AB6 File Offset: 0x00001AB6
			internal CMSG_RC2_AUX_INFO(int size)
			{
				this.cbSize = (uint)size;
				this.dwBitLen = 0U;
			}

			// Token: 0x0400037F RID: 895
			internal uint cbSize;

			// Token: 0x04000380 RID: 896
			internal uint dwBitLen;
		}

		// Token: 0x0200002C RID: 44
		internal struct CMSG_RECIPIENT_ENCODE_INFO
		{
			// Token: 0x04000381 RID: 897
			internal uint dwRecipientChoice;

			// Token: 0x04000382 RID: 898
			internal IntPtr pRecipientInfo;
		}

		// Token: 0x0200002D RID: 45
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO
		{
			// Token: 0x04000383 RID: 899
			internal uint cbSize;

			// Token: 0x04000384 RID: 900
			internal CAPIBase.CRYPT_BIT_BLOB RecipientPublicKey;

			// Token: 0x04000385 RID: 901
			internal CAPIBase.CERT_ID RecipientId;

			// Token: 0x04000386 RID: 902
			internal global::System.Runtime.InteropServices.ComTypes.FILETIME Date;

			// Token: 0x04000387 RID: 903
			internal IntPtr pOtherAttr;
		}

		// Token: 0x0200002E RID: 46
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_ENVELOPED_ENCODE_INFO
		{
			// Token: 0x06000025 RID: 37 RVA: 0x00002AC8 File Offset: 0x00001AC8
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

			// Token: 0x04000388 RID: 904
			internal uint cbSize;

			// Token: 0x04000389 RID: 905
			internal IntPtr hCryptProv;

			// Token: 0x0400038A RID: 906
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER ContentEncryptionAlgorithm;

			// Token: 0x0400038B RID: 907
			internal IntPtr pvEncryptionAuxInfo;

			// Token: 0x0400038C RID: 908
			internal uint cRecipients;

			// Token: 0x0400038D RID: 909
			internal IntPtr rgpRecipients;

			// Token: 0x0400038E RID: 910
			internal IntPtr rgCmsRecipients;

			// Token: 0x0400038F RID: 911
			internal uint cCertEncoded;

			// Token: 0x04000390 RID: 912
			internal IntPtr rgCertEncoded;

			// Token: 0x04000391 RID: 913
			internal uint cCrlEncoded;

			// Token: 0x04000392 RID: 914
			internal IntPtr rgCrlEncoded;

			// Token: 0x04000393 RID: 915
			internal uint cAttrCertEncoded;

			// Token: 0x04000394 RID: 916
			internal IntPtr rgAttrCertEncoded;

			// Token: 0x04000395 RID: 917
			internal uint cUnprotectedAttr;

			// Token: 0x04000396 RID: 918
			internal IntPtr rgUnprotectedAttr;
		}

		// Token: 0x0200002F RID: 47
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_KEY_AGREE_DECRYPT_PARA
		{
			// Token: 0x06000026 RID: 38 RVA: 0x00002B63 File Offset: 0x00001B63
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

			// Token: 0x04000397 RID: 919
			internal uint cbSize;

			// Token: 0x04000398 RID: 920
			internal IntPtr hCryptProv;

			// Token: 0x04000399 RID: 921
			internal uint dwKeySpec;

			// Token: 0x0400039A RID: 922
			internal IntPtr pKeyAgree;

			// Token: 0x0400039B RID: 923
			internal uint dwRecipientIndex;

			// Token: 0x0400039C RID: 924
			internal uint dwRecipientEncryptedKeyIndex;

			// Token: 0x0400039D RID: 925
			internal CAPIBase.CRYPT_BIT_BLOB OriginatorPublicKey;
		}

		// Token: 0x02000030 RID: 48
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_AGREE_RECIPIENT_INFO
		{
			// Token: 0x0400039E RID: 926
			internal uint dwVersion;

			// Token: 0x0400039F RID: 927
			internal uint dwOriginatorChoice;
		}

		// Token: 0x02000031 RID: 49
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO
		{
			// Token: 0x040003A0 RID: 928
			internal uint dwVersion;

			// Token: 0x040003A1 RID: 929
			internal uint dwOriginatorChoice;

			// Token: 0x040003A2 RID: 930
			internal CAPIBase.CERT_ID OriginatorCertId;

			// Token: 0x040003A3 RID: 931
			internal IntPtr Padding;

			// Token: 0x040003A4 RID: 932
			internal CAPIBase.CRYPTOAPI_BLOB UserKeyingMaterial;

			// Token: 0x040003A5 RID: 933
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;

			// Token: 0x040003A6 RID: 934
			internal uint cRecipientEncryptedKeys;

			// Token: 0x040003A7 RID: 935
			internal IntPtr rgpRecipientEncryptedKeys;
		}

		// Token: 0x02000032 RID: 50
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO
		{
			// Token: 0x040003A8 RID: 936
			internal uint dwVersion;

			// Token: 0x040003A9 RID: 937
			internal uint dwOriginatorChoice;

			// Token: 0x040003AA RID: 938
			internal CAPIBase.CERT_PUBLIC_KEY_INFO OriginatorPublicKeyInfo;

			// Token: 0x040003AB RID: 939
			internal CAPIBase.CRYPTOAPI_BLOB UserKeyingMaterial;

			// Token: 0x040003AC RID: 940
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;

			// Token: 0x040003AD RID: 941
			internal uint cRecipientEncryptedKeys;

			// Token: 0x040003AE RID: 942
			internal IntPtr rgpRecipientEncryptedKeys;
		}

		// Token: 0x02000033 RID: 51
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_RECIPIENT_ENCRYPTED_KEY_INFO
		{
			// Token: 0x040003AF RID: 943
			internal CAPIBase.CERT_ID RecipientId;

			// Token: 0x040003B0 RID: 944
			internal CAPIBase.CRYPTOAPI_BLOB EncryptedKey;

			// Token: 0x040003B1 RID: 945
			internal global::System.Runtime.InteropServices.ComTypes.FILETIME Date;

			// Token: 0x040003B2 RID: 946
			internal IntPtr pOtherAttr;
		}

		// Token: 0x02000034 RID: 52
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_CTRL_VERIFY_SIGNATURE_EX_PARA
		{
			// Token: 0x06000027 RID: 39 RVA: 0x00002BA3 File Offset: 0x00001BA3
			internal CMSG_CTRL_VERIFY_SIGNATURE_EX_PARA(int size)
			{
				this.cbSize = (uint)size;
				this.hCryptProv = IntPtr.Zero;
				this.dwSignerIndex = 0U;
				this.dwSignerType = 0U;
				this.pvSigner = IntPtr.Zero;
			}

			// Token: 0x040003B3 RID: 947
			internal uint cbSize;

			// Token: 0x040003B4 RID: 948
			internal IntPtr hCryptProv;

			// Token: 0x040003B5 RID: 949
			internal uint dwSignerIndex;

			// Token: 0x040003B6 RID: 950
			internal uint dwSignerType;

			// Token: 0x040003B7 RID: 951
			internal IntPtr pvSigner;
		}

		// Token: 0x02000035 RID: 53
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_KEY_TRANS_RECIPIENT_INFO
		{
			// Token: 0x040003B8 RID: 952
			internal uint dwVersion;

			// Token: 0x040003B9 RID: 953
			internal CAPIBase.CERT_ID RecipientId;

			// Token: 0x040003BA RID: 954
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;

			// Token: 0x040003BB RID: 955
			internal CAPIBase.CRYPTOAPI_BLOB EncryptedKey;
		}

		// Token: 0x02000036 RID: 54
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_SIGNED_ENCODE_INFO
		{
			// Token: 0x06000028 RID: 40 RVA: 0x00002BD0 File Offset: 0x00001BD0
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

			// Token: 0x040003BC RID: 956
			internal uint cbSize;

			// Token: 0x040003BD RID: 957
			internal uint cSigners;

			// Token: 0x040003BE RID: 958
			internal IntPtr rgSigners;

			// Token: 0x040003BF RID: 959
			internal uint cCertEncoded;

			// Token: 0x040003C0 RID: 960
			internal IntPtr rgCertEncoded;

			// Token: 0x040003C1 RID: 961
			internal uint cCrlEncoded;

			// Token: 0x040003C2 RID: 962
			internal IntPtr rgCrlEncoded;

			// Token: 0x040003C3 RID: 963
			internal uint cAttrCertEncoded;

			// Token: 0x040003C4 RID: 964
			internal IntPtr rgAttrCertEncoded;
		}

		// Token: 0x02000037 RID: 55
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_SIGNER_ENCODE_INFO
		{
			// Token: 0x06000029 RID: 41
			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern IntPtr LocalFree(IntPtr hMem);

			// Token: 0x0600002A RID: 42
			[DllImport("advapi32.dll", SetLastError = true)]
			internal static extern bool CryptReleaseContext([In] IntPtr hProv, [In] uint dwFlags);

			// Token: 0x0600002B RID: 43 RVA: 0x00002C2C File Offset: 0x00001C2C
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

			// Token: 0x0600002C RID: 44 RVA: 0x00002CBC File Offset: 0x00001CBC
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

			// Token: 0x040003C5 RID: 965
			internal uint cbSize;

			// Token: 0x040003C6 RID: 966
			internal IntPtr pCertInfo;

			// Token: 0x040003C7 RID: 967
			internal IntPtr hCryptProv;

			// Token: 0x040003C8 RID: 968
			internal uint dwKeySpec;

			// Token: 0x040003C9 RID: 969
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;

			// Token: 0x040003CA RID: 970
			internal IntPtr pvHashAuxInfo;

			// Token: 0x040003CB RID: 971
			internal uint cAuthAttr;

			// Token: 0x040003CC RID: 972
			internal IntPtr rgAuthAttr;

			// Token: 0x040003CD RID: 973
			internal uint cUnauthAttr;

			// Token: 0x040003CE RID: 974
			internal IntPtr rgUnauthAttr;

			// Token: 0x040003CF RID: 975
			internal CAPIBase.CERT_ID SignerId;

			// Token: 0x040003D0 RID: 976
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;

			// Token: 0x040003D1 RID: 977
			internal IntPtr pvHashEncryptionAuxInfo;
		}

		// Token: 0x02000038 RID: 56
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CMSG_SIGNER_INFO
		{
			// Token: 0x040003D2 RID: 978
			internal uint dwVersion;

			// Token: 0x040003D3 RID: 979
			internal CAPIBase.CRYPTOAPI_BLOB Issuer;

			// Token: 0x040003D4 RID: 980
			internal CAPIBase.CRYPTOAPI_BLOB SerialNumber;

			// Token: 0x040003D5 RID: 981
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;

			// Token: 0x040003D6 RID: 982
			internal CAPIBase.CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;

			// Token: 0x040003D7 RID: 983
			internal CAPIBase.CRYPTOAPI_BLOB EncryptedHash;

			// Token: 0x040003D8 RID: 984
			internal CAPIBase.CRYPT_ATTRIBUTES AuthAttrs;

			// Token: 0x040003D9 RID: 985
			internal CAPIBase.CRYPT_ATTRIBUTES UnauthAttrs;
		}

		// Token: 0x02000039 RID: 57
		// (Invoke) Token: 0x0600002E RID: 46
		internal delegate bool PFN_CMSG_STREAM_OUTPUT(IntPtr pvArg, IntPtr pbData, uint cbData, bool fFinal);

		// Token: 0x0200003A RID: 58
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal class CMSG_STREAM_INFO
		{
			// Token: 0x06000031 RID: 49 RVA: 0x00002D60 File Offset: 0x00001D60
			internal CMSG_STREAM_INFO(uint cbContent, CAPIBase.PFN_CMSG_STREAM_OUTPUT pfnStreamOutput, IntPtr pvArg)
			{
				this.cbContent = cbContent;
				this.pfnStreamOutput = pfnStreamOutput;
				this.pvArg = pvArg;
			}

			// Token: 0x040003DA RID: 986
			internal uint cbContent;

			// Token: 0x040003DB RID: 987
			internal CAPIBase.PFN_CMSG_STREAM_OUTPUT pfnStreamOutput;

			// Token: 0x040003DC RID: 988
			internal IntPtr pvArg;
		}

		// Token: 0x0200003B RID: 59
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_ALGORITHM_IDENTIFIER
		{
			// Token: 0x040003DD RID: 989
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x040003DE RID: 990
			internal CAPIBase.CRYPTOAPI_BLOB Parameters;
		}

		// Token: 0x0200003C RID: 60
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_ALGORITHM_IDENTIFIER2
		{
			// Token: 0x040003DF RID: 991
			internal IntPtr pszObjId;

			// Token: 0x040003E0 RID: 992
			internal CAPIBase.CRYPTOAPI_BLOB Parameters;
		}

		// Token: 0x0200003D RID: 61
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_ATTRIBUTE
		{
			// Token: 0x040003E1 RID: 993
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x040003E2 RID: 994
			internal uint cValue;

			// Token: 0x040003E3 RID: 995
			internal IntPtr rgValue;
		}

		// Token: 0x0200003E RID: 62
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_ATTRIBUTES
		{
			// Token: 0x040003E4 RID: 996
			internal uint cAttr;

			// Token: 0x040003E5 RID: 997
			internal IntPtr rgAttr;
		}

		// Token: 0x0200003F RID: 63
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_ATTRIBUTE_TYPE_VALUE
		{
			// Token: 0x040003E6 RID: 998
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x040003E7 RID: 999
			internal CAPIBase.CRYPTOAPI_BLOB Value;
		}

		// Token: 0x02000040 RID: 64
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_BIT_BLOB
		{
			// Token: 0x040003E8 RID: 1000
			internal uint cbData;

			// Token: 0x040003E9 RID: 1001
			internal IntPtr pbData;

			// Token: 0x040003EA RID: 1002
			internal uint cUnusedBits;
		}

		// Token: 0x02000041 RID: 65
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_KEY_PROV_INFO
		{
			// Token: 0x040003EB RID: 1003
			internal string pwszContainerName;

			// Token: 0x040003EC RID: 1004
			internal string pwszProvName;

			// Token: 0x040003ED RID: 1005
			internal uint dwProvType;

			// Token: 0x040003EE RID: 1006
			internal uint dwFlags;

			// Token: 0x040003EF RID: 1007
			internal uint cProvParam;

			// Token: 0x040003F0 RID: 1008
			internal IntPtr rgProvParam;

			// Token: 0x040003F1 RID: 1009
			internal uint dwKeySpec;
		}

		// Token: 0x02000042 RID: 66
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_OID_INFO
		{
			// Token: 0x06000032 RID: 50 RVA: 0x00002D7D File Offset: 0x00001D7D
			internal CRYPT_OID_INFO(int size)
			{
				this.cbSize = (uint)size;
				this.pszOID = null;
				this.pwszName = null;
				this.dwGroupId = 0U;
				this.Algid = 0U;
				this.ExtraInfo = default(CAPIBase.CRYPTOAPI_BLOB);
			}

			// Token: 0x040003F2 RID: 1010
			internal uint cbSize;

			// Token: 0x040003F3 RID: 1011
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszOID;

			// Token: 0x040003F4 RID: 1012
			internal string pwszName;

			// Token: 0x040003F5 RID: 1013
			internal uint dwGroupId;

			// Token: 0x040003F6 RID: 1014
			internal uint Algid;

			// Token: 0x040003F7 RID: 1015
			internal CAPIBase.CRYPTOAPI_BLOB ExtraInfo;
		}

		// Token: 0x02000043 RID: 67
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_RC2_CBC_PARAMETERS
		{
			// Token: 0x040003F8 RID: 1016
			internal uint dwVersion;

			// Token: 0x040003F9 RID: 1017
			internal bool fIV;

			// Token: 0x040003FA RID: 1018
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			internal byte[] rgbIV;
		}

		// Token: 0x02000044 RID: 68
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPTOAPI_BLOB
		{
			// Token: 0x040003FB RID: 1019
			internal uint cbData;

			// Token: 0x040003FC RID: 1020
			internal IntPtr pbData;
		}

		// Token: 0x02000045 RID: 69
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal class CRYPTUI_SELECTCERTIFICATE_STRUCTW
		{
			// Token: 0x040003FD RID: 1021
			internal uint dwSize;

			// Token: 0x040003FE RID: 1022
			internal IntPtr hwndParent;

			// Token: 0x040003FF RID: 1023
			internal uint dwFlags;

			// Token: 0x04000400 RID: 1024
			internal string szTitle;

			// Token: 0x04000401 RID: 1025
			internal uint dwDontUseColumn;

			// Token: 0x04000402 RID: 1026
			internal string szDisplayString;

			// Token: 0x04000403 RID: 1027
			internal IntPtr pFilterCallback;

			// Token: 0x04000404 RID: 1028
			internal IntPtr pDisplayCallback;

			// Token: 0x04000405 RID: 1029
			internal IntPtr pvCallbackData;

			// Token: 0x04000406 RID: 1030
			internal uint cDisplayStores;

			// Token: 0x04000407 RID: 1031
			internal IntPtr rghDisplayStores;

			// Token: 0x04000408 RID: 1032
			internal uint cStores;

			// Token: 0x04000409 RID: 1033
			internal IntPtr rghStores;

			// Token: 0x0400040A RID: 1034
			internal uint cPropSheetPages;

			// Token: 0x0400040B RID: 1035
			internal IntPtr rgPropSheetPages;

			// Token: 0x0400040C RID: 1036
			internal IntPtr hSelectedCertStore;
		}

		// Token: 0x02000046 RID: 70
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal class CRYPTUI_VIEWCERTIFICATE_STRUCTW
		{
			// Token: 0x0400040D RID: 1037
			internal uint dwSize;

			// Token: 0x0400040E RID: 1038
			internal IntPtr hwndParent;

			// Token: 0x0400040F RID: 1039
			internal uint dwFlags;

			// Token: 0x04000410 RID: 1040
			internal string szTitle;

			// Token: 0x04000411 RID: 1041
			internal IntPtr pCertContext;

			// Token: 0x04000412 RID: 1042
			internal IntPtr rgszPurposes;

			// Token: 0x04000413 RID: 1043
			internal uint cPurposes;

			// Token: 0x04000414 RID: 1044
			internal IntPtr pCryptProviderData;

			// Token: 0x04000415 RID: 1045
			internal bool fpCryptProviderDataTrustedUsage;

			// Token: 0x04000416 RID: 1046
			internal uint idxSigner;

			// Token: 0x04000417 RID: 1047
			internal uint idxCert;

			// Token: 0x04000418 RID: 1048
			internal bool fCounterSigner;

			// Token: 0x04000419 RID: 1049
			internal uint idxCounterSigner;

			// Token: 0x0400041A RID: 1050
			internal uint cStores;

			// Token: 0x0400041B RID: 1051
			internal IntPtr rghStores;

			// Token: 0x0400041C RID: 1052
			internal uint cPropSheetPages;

			// Token: 0x0400041D RID: 1053
			internal IntPtr rgPropSheetPages;

			// Token: 0x0400041E RID: 1054
			internal uint nStartPage;
		}

		// Token: 0x02000047 RID: 71
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct DSSPUBKEY
		{
			// Token: 0x0400041F RID: 1055
			internal uint magic;

			// Token: 0x04000420 RID: 1056
			internal uint bitlen;
		}

		// Token: 0x02000048 RID: 72
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct PROV_ENUMALGS_EX
		{
			// Token: 0x04000421 RID: 1057
			internal uint aiAlgid;

			// Token: 0x04000422 RID: 1058
			internal uint dwDefaultLen;

			// Token: 0x04000423 RID: 1059
			internal uint dwMinLen;

			// Token: 0x04000424 RID: 1060
			internal uint dwMaxLen;

			// Token: 0x04000425 RID: 1061
			internal uint dwProtocols;

			// Token: 0x04000426 RID: 1062
			internal uint dwNameLen;

			// Token: 0x04000427 RID: 1063
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
			internal byte[] szName;

			// Token: 0x04000428 RID: 1064
			internal uint dwLongNameLen;

			// Token: 0x04000429 RID: 1065
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
			internal byte[] szLongName;
		}

		// Token: 0x02000049 RID: 73
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct RSAPUBKEY
		{
			// Token: 0x0400042A RID: 1066
			internal uint magic;

			// Token: 0x0400042B RID: 1067
			internal uint bitlen;

			// Token: 0x0400042C RID: 1068
			internal uint pubexp;
		}
	}
}
