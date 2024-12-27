using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004EE RID: 1262
	internal interface SSPIInterface
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06002753 RID: 10067
		// (set) Token: 0x06002754 RID: 10068
		SecurityPackageInfoClass[] SecurityPackages { get; set; }

		// Token: 0x06002755 RID: 10069
		int EnumerateSecurityPackages(out int pkgnum, out SafeFreeContextBuffer pkgArray);

		// Token: 0x06002756 RID: 10070
		int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref AuthIdentity authdata, out SafeFreeCredentials outCredential);

		// Token: 0x06002757 RID: 10071
		int AcquireDefaultCredential(string moduleName, CredentialUse usage, out SafeFreeCredentials outCredential);

		// Token: 0x06002758 RID: 10072
		int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref SecureCredential authdata, out SafeFreeCredentials outCredential);

		// Token: 0x06002759 RID: 10073
		int AcceptSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer inputBuffer, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags);

		// Token: 0x0600275A RID: 10074
		int AcceptSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer[] inputBuffers, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags);

		// Token: 0x0600275B RID: 10075
		int InitializeSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlags outFlags);

		// Token: 0x0600275C RID: 10076
		int InitializeSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer[] inputBuffers, SecurityBuffer outputBuffer, ref ContextFlags outFlags);

		// Token: 0x0600275D RID: 10077
		int EncryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber);

		// Token: 0x0600275E RID: 10078
		int DecryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber);

		// Token: 0x0600275F RID: 10079
		int MakeSignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber);

		// Token: 0x06002760 RID: 10080
		int VerifySignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber);

		// Token: 0x06002761 RID: 10081
		int QueryContextChannelBinding(SafeDeleteContext phContext, ContextAttribute attribute, out SafeFreeContextBufferChannelBinding refHandle);

		// Token: 0x06002762 RID: 10082
		int QueryContextAttributes(SafeDeleteContext phContext, ContextAttribute attribute, byte[] buffer, Type handleType, out SafeHandle refHandle);

		// Token: 0x06002763 RID: 10083
		int QuerySecurityContextToken(SafeDeleteContext phContext, out SafeCloseHandle phToken);

		// Token: 0x06002764 RID: 10084
		int CompleteAuthToken(ref SafeDeleteContext refContext, SecurityBuffer[] inputBuffers);
	}
}
