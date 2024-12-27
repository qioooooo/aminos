using System;
using System.Threading;

namespace System.Net.Security
{
	// Token: 0x0200053F RID: 1343
	internal static class SSPIHandleCache
	{
		// Token: 0x06002900 RID: 10496 RVA: 0x000AA7B0 File Offset: 0x000A97B0
		internal static void CacheCredential(SafeFreeCredentials newHandle)
		{
			try
			{
				SafeCredentialReference safeCredentialReference = SafeCredentialReference.CreateReference(newHandle);
				if (safeCredentialReference != null)
				{
					int num = Interlocked.Increment(ref SSPIHandleCache._Current) & 31;
					safeCredentialReference = Interlocked.Exchange<SafeCredentialReference>(ref SSPIHandleCache._CacheSlots[num], safeCredentialReference);
					if (safeCredentialReference != null)
					{
						safeCredentialReference.Close();
					}
				}
			}
			catch (Exception ex)
			{
				NclUtilities.IsFatal(ex);
			}
		}

		// Token: 0x040027D0 RID: 10192
		private const int c_MaxCacheSize = 31;

		// Token: 0x040027D1 RID: 10193
		private static SafeCredentialReference[] _CacheSlots = new SafeCredentialReference[32];

		// Token: 0x040027D2 RID: 10194
		private static int _Current = -1;
	}
}
