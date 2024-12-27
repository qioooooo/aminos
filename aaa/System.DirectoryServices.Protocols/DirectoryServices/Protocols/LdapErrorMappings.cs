using System;
using System.Collections;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000077 RID: 119
	internal class LdapErrorMappings
	{
		// Token: 0x0600027F RID: 639 RVA: 0x0000D330 File Offset: 0x0000C330
		static LdapErrorMappings()
		{
			LdapErrorMappings.ResultCodeHash.Add(LdapError.IsLeaf, Res.GetString("LDAP_IS_LEAF"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.InvalidCredentials, Res.GetString("LDAP_INVALID_CREDENTIALS"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.ServerDown, Res.GetString("LDAP_SERVER_DOWN"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.LocalError, Res.GetString("LDAP_LOCAL_ERROR"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.EncodingError, Res.GetString("LDAP_ENCODING_ERROR"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.DecodingError, Res.GetString("LDAP_DECODING_ERROR"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.TimeOut, Res.GetString("LDAP_TIMEOUT"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.AuthUnknown, Res.GetString("LDAP_AUTH_UNKNOWN"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.FilterError, Res.GetString("LDAP_FILTER_ERROR"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.UserCancelled, Res.GetString("LDAP_USER_CANCELLED"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.ParameterError, Res.GetString("LDAP_PARAM_ERROR"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.NoMemory, Res.GetString("LDAP_NO_MEMORY"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.ConnectError, Res.GetString("LDAP_CONNECT_ERROR"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.NotSupported, Res.GetString("LDAP_NOT_SUPPORTED"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.NoResultsReturned, Res.GetString("LDAP_NO_RESULTS_RETURNED"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.ControlNotFound, Res.GetString("LDAP_CONTROL_NOT_FOUND"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.MoreResults, Res.GetString("LDAP_MORE_RESULTS_TO_RETURN"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.ClientLoop, Res.GetString("LDAP_CLIENT_LOOP"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.ReferralLimitExceeded, Res.GetString("LDAP_REFERRAL_LIMIT_EXCEEDED"));
			LdapErrorMappings.ResultCodeHash.Add(LdapError.SendTimeOut, Res.GetString("LDAP_SEND_TIMEOUT"));
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000D563 File Offset: 0x0000C563
		public static string MapResultCode(int errorCode)
		{
			return (string)LdapErrorMappings.ResultCodeHash[(LdapError)errorCode];
		}

		// Token: 0x0400026B RID: 619
		private static Hashtable ResultCodeHash = new Hashtable();
	}
}
