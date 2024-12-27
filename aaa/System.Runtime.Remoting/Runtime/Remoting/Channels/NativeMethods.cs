using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200001F RID: 31
	internal static class NativeMethods
	{
		// Token: 0x060000E6 RID: 230
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern bool IsValidSid(IntPtr sidPointer);

		// Token: 0x060000E7 RID: 231
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern IntPtr GetSidIdentifierAuthority(IntPtr sidPointer);

		// Token: 0x060000E8 RID: 232
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern IntPtr GetSidSubAuthorityCount(IntPtr sidPointer);

		// Token: 0x060000E9 RID: 233
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern IntPtr GetSidSubAuthority(IntPtr sidPointer, int count);

		// Token: 0x060000EA RID: 234
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern bool GetTokenInformation(IntPtr tokenHandle, int tokenInformationClass, IntPtr sidAndAttributesPointer, int tokenInformationLength, ref int returnLength);

		// Token: 0x040000B8 RID: 184
		private const string ADVAPI32 = "advapi32.dll";

		// Token: 0x040000B9 RID: 185
		internal const int ThreadTokenAllAccess = 983551;

		// Token: 0x040000BA RID: 186
		internal const int BufferTooSmall = 122;

		// Token: 0x02000020 RID: 32
		internal enum TokenInformationClass
		{
			// Token: 0x040000BC RID: 188
			TokenUser = 1,
			// Token: 0x040000BD RID: 189
			TokenGroups,
			// Token: 0x040000BE RID: 190
			TokenPrivileges,
			// Token: 0x040000BF RID: 191
			TokenOwner,
			// Token: 0x040000C0 RID: 192
			TokenPrimaryGroup,
			// Token: 0x040000C1 RID: 193
			TokenDefaultDacl,
			// Token: 0x040000C2 RID: 194
			TokenSource,
			// Token: 0x040000C3 RID: 195
			TokenType,
			// Token: 0x040000C4 RID: 196
			TokenImpersonationLevel,
			// Token: 0x040000C5 RID: 197
			TokenStatistics,
			// Token: 0x040000C6 RID: 198
			TokenRestrictedSids,
			// Token: 0x040000C7 RID: 199
			TokenSessionId,
			// Token: 0x040000C8 RID: 200
			TokenGroupsAndPrivileges,
			// Token: 0x040000C9 RID: 201
			TokenSessionReference,
			// Token: 0x040000CA RID: 202
			TokenSandBoxInert
		}
	}
}
