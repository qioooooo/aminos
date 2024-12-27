using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008BB RID: 2235
	internal static class KdfWorkLimiter
	{
		// Token: 0x06005209 RID: 21001 RVA: 0x0012738C File Offset: 0x0012638C
		internal static void SetIterationLimit(ulong workLimit)
		{
			KdfWorkLimiter.t_State = new KdfWorkLimiter.State
			{
				RemainingAllowedWork = workLimit
			};
		}

		// Token: 0x0600520A RID: 21002 RVA: 0x001273AC File Offset: 0x001263AC
		internal static bool WasWorkLimitExceeded()
		{
			return KdfWorkLimiter.t_State.WorkLimitWasExceeded;
		}

		// Token: 0x0600520B RID: 21003 RVA: 0x001273B8 File Offset: 0x001263B8
		internal static void ResetIterationLimit()
		{
			KdfWorkLimiter.t_State = null;
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x001273C0 File Offset: 0x001263C0
		internal static void RecordIterations(int workCount)
		{
			KdfWorkLimiter.RecordIterations((long)workCount);
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x001273CC File Offset: 0x001263CC
		internal static void RecordIterations(long workCount)
		{
			KdfWorkLimiter.State state = KdfWorkLimiter.t_State;
			bool flag = false;
			checked
			{
				try
				{
					if (!state.WorkLimitWasExceeded)
					{
						state.RemainingAllowedWork -= (ulong)workCount;
						flag = true;
					}
				}
				finally
				{
					if (!flag)
					{
						state.RemainingAllowedWork = 0UL;
						state.WorkLimitWasExceeded = true;
						throw new CryptographicException();
					}
				}
			}
		}

		// Token: 0x04002A0A RID: 10762
		[ThreadStatic]
		private static KdfWorkLimiter.State t_State;

		// Token: 0x020008BC RID: 2236
		private sealed class State
		{
			// Token: 0x04002A0B RID: 10763
			internal ulong RemainingAllowedWork;

			// Token: 0x04002A0C RID: 10764
			internal bool WorkLimitWasExceeded;
		}
	}
}
