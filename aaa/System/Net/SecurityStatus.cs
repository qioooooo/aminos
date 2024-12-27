using System;

namespace System.Net
{
	// Token: 0x020003F4 RID: 1012
	internal enum SecurityStatus
	{
		// Token: 0x04001FE4 RID: 8164
		OK,
		// Token: 0x04001FE5 RID: 8165
		ContinueNeeded = 590610,
		// Token: 0x04001FE6 RID: 8166
		CompleteNeeded,
		// Token: 0x04001FE7 RID: 8167
		CompAndContinue,
		// Token: 0x04001FE8 RID: 8168
		ContextExpired = 590615,
		// Token: 0x04001FE9 RID: 8169
		CredentialsNeeded = 590624,
		// Token: 0x04001FEA RID: 8170
		Renegotiate,
		// Token: 0x04001FEB RID: 8171
		OutOfMemory = -2146893056,
		// Token: 0x04001FEC RID: 8172
		InvalidHandle,
		// Token: 0x04001FED RID: 8173
		Unsupported,
		// Token: 0x04001FEE RID: 8174
		TargetUnknown,
		// Token: 0x04001FEF RID: 8175
		InternalError,
		// Token: 0x04001FF0 RID: 8176
		PackageNotFound,
		// Token: 0x04001FF1 RID: 8177
		NotOwner,
		// Token: 0x04001FF2 RID: 8178
		CannotInstall,
		// Token: 0x04001FF3 RID: 8179
		InvalidToken,
		// Token: 0x04001FF4 RID: 8180
		CannotPack,
		// Token: 0x04001FF5 RID: 8181
		QopNotSupported,
		// Token: 0x04001FF6 RID: 8182
		NoImpersonation,
		// Token: 0x04001FF7 RID: 8183
		LogonDenied,
		// Token: 0x04001FF8 RID: 8184
		UnknownCredentials,
		// Token: 0x04001FF9 RID: 8185
		NoCredentials,
		// Token: 0x04001FFA RID: 8186
		MessageAltered,
		// Token: 0x04001FFB RID: 8187
		OutOfSequence,
		// Token: 0x04001FFC RID: 8188
		NoAuthenticatingAuthority,
		// Token: 0x04001FFD RID: 8189
		IncompleteMessage = -2146893032,
		// Token: 0x04001FFE RID: 8190
		IncompleteCredentials = -2146893024,
		// Token: 0x04001FFF RID: 8191
		BufferNotEnough,
		// Token: 0x04002000 RID: 8192
		WrongPrincipal,
		// Token: 0x04002001 RID: 8193
		TimeSkew = -2146893020,
		// Token: 0x04002002 RID: 8194
		UntrustedRoot,
		// Token: 0x04002003 RID: 8195
		IllegalMessage,
		// Token: 0x04002004 RID: 8196
		CertUnknown,
		// Token: 0x04002005 RID: 8197
		CertExpired,
		// Token: 0x04002006 RID: 8198
		AlgorithmMismatch = -2146893007,
		// Token: 0x04002007 RID: 8199
		SecurityQosFailed,
		// Token: 0x04002008 RID: 8200
		SmartcardLogonRequired = -2146892994,
		// Token: 0x04002009 RID: 8201
		UnsupportedPreauth = -2146892989,
		// Token: 0x0400200A RID: 8202
		BadBinding = -2146892986
	}
}
