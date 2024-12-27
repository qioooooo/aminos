using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003EE RID: 1006
	internal struct InterlockedGate
	{
		// Token: 0x06002085 RID: 8325 RVA: 0x000803D2 File Offset: 0x0007F3D2
		internal void Reset()
		{
			this.m_State = 0;
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000803DC File Offset: 0x0007F3DC
		internal bool Trigger(bool exclusive)
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 2, 0);
			if (exclusive && (num == 1 || num == 2))
			{
				throw new InternalException();
			}
			return num == 0;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x0008040C File Offset: 0x0007F40C
		internal bool StartTrigger(bool exclusive)
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 1, 0);
			if (exclusive && (num == 1 || num == 2))
			{
				throw new InternalException();
			}
			return num == 0;
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x0008043C File Offset: 0x0007F43C
		internal void FinishTrigger()
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 2, 1);
			if (num != 1)
			{
				throw new InternalException();
			}
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x00080464 File Offset: 0x0007F464
		internal bool Complete()
		{
			int num;
			while ((num = Interlocked.CompareExchange(ref this.m_State, 3, 2)) != 2)
			{
				if (num == 3)
				{
					return false;
				}
				if (num == 0)
				{
					if (Interlocked.CompareExchange(ref this.m_State, 3, 0) == 0)
					{
						return false;
					}
				}
				else
				{
					Thread.SpinWait(1);
				}
			}
			return true;
		}

		// Token: 0x04001FB8 RID: 8120
		internal const int Open = 0;

		// Token: 0x04001FB9 RID: 8121
		internal const int Held = 1;

		// Token: 0x04001FBA RID: 8122
		internal const int Triggered = 2;

		// Token: 0x04001FBB RID: 8123
		internal const int Closed = 3;

		// Token: 0x04001FBC RID: 8124
		private int m_State;
	}
}
