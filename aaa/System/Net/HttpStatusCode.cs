using System;

namespace System.Net
{
	// Token: 0x020003DE RID: 990
	public enum HttpStatusCode
	{
		// Token: 0x04001F07 RID: 7943
		Continue = 100,
		// Token: 0x04001F08 RID: 7944
		SwitchingProtocols,
		// Token: 0x04001F09 RID: 7945
		OK = 200,
		// Token: 0x04001F0A RID: 7946
		Created,
		// Token: 0x04001F0B RID: 7947
		Accepted,
		// Token: 0x04001F0C RID: 7948
		NonAuthoritativeInformation,
		// Token: 0x04001F0D RID: 7949
		NoContent,
		// Token: 0x04001F0E RID: 7950
		ResetContent,
		// Token: 0x04001F0F RID: 7951
		PartialContent,
		// Token: 0x04001F10 RID: 7952
		MultipleChoices = 300,
		// Token: 0x04001F11 RID: 7953
		Ambiguous = 300,
		// Token: 0x04001F12 RID: 7954
		MovedPermanently,
		// Token: 0x04001F13 RID: 7955
		Moved = 301,
		// Token: 0x04001F14 RID: 7956
		Found,
		// Token: 0x04001F15 RID: 7957
		Redirect = 302,
		// Token: 0x04001F16 RID: 7958
		SeeOther,
		// Token: 0x04001F17 RID: 7959
		RedirectMethod = 303,
		// Token: 0x04001F18 RID: 7960
		NotModified,
		// Token: 0x04001F19 RID: 7961
		UseProxy,
		// Token: 0x04001F1A RID: 7962
		Unused,
		// Token: 0x04001F1B RID: 7963
		TemporaryRedirect,
		// Token: 0x04001F1C RID: 7964
		RedirectKeepVerb = 307,
		// Token: 0x04001F1D RID: 7965
		BadRequest = 400,
		// Token: 0x04001F1E RID: 7966
		Unauthorized,
		// Token: 0x04001F1F RID: 7967
		PaymentRequired,
		// Token: 0x04001F20 RID: 7968
		Forbidden,
		// Token: 0x04001F21 RID: 7969
		NotFound,
		// Token: 0x04001F22 RID: 7970
		MethodNotAllowed,
		// Token: 0x04001F23 RID: 7971
		NotAcceptable,
		// Token: 0x04001F24 RID: 7972
		ProxyAuthenticationRequired,
		// Token: 0x04001F25 RID: 7973
		RequestTimeout,
		// Token: 0x04001F26 RID: 7974
		Conflict,
		// Token: 0x04001F27 RID: 7975
		Gone,
		// Token: 0x04001F28 RID: 7976
		LengthRequired,
		// Token: 0x04001F29 RID: 7977
		PreconditionFailed,
		// Token: 0x04001F2A RID: 7978
		RequestEntityTooLarge,
		// Token: 0x04001F2B RID: 7979
		RequestUriTooLong,
		// Token: 0x04001F2C RID: 7980
		UnsupportedMediaType,
		// Token: 0x04001F2D RID: 7981
		RequestedRangeNotSatisfiable,
		// Token: 0x04001F2E RID: 7982
		ExpectationFailed,
		// Token: 0x04001F2F RID: 7983
		InternalServerError = 500,
		// Token: 0x04001F30 RID: 7984
		NotImplemented,
		// Token: 0x04001F31 RID: 7985
		BadGateway,
		// Token: 0x04001F32 RID: 7986
		ServiceUnavailable,
		// Token: 0x04001F33 RID: 7987
		GatewayTimeout,
		// Token: 0x04001F34 RID: 7988
		HttpVersionNotSupported
	}
}
