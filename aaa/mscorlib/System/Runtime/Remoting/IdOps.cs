using System;

namespace System.Runtime.Remoting
{
	// Token: 0x020006E8 RID: 1768
	internal struct IdOps
	{
		// Token: 0x06003F71 RID: 16241 RVA: 0x000D8ED2 File Offset: 0x000D7ED2
		internal static bool bStrongIdentity(int flags)
		{
			return (flags & 2) != 0;
		}

		// Token: 0x04001FE8 RID: 8168
		internal const int None = 0;

		// Token: 0x04001FE9 RID: 8169
		internal const int GenerateURI = 1;

		// Token: 0x04001FEA RID: 8170
		internal const int StrongIdentity = 2;
	}
}
