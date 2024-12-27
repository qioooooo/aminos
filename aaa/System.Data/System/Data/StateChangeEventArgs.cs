using System;

namespace System.Data
{
	// Token: 0x020000E5 RID: 229
	public sealed class StateChangeEventArgs : EventArgs
	{
		// Token: 0x06000D99 RID: 3481 RVA: 0x00200764 File Offset: 0x001FFB64
		public StateChangeEventArgs(ConnectionState originalState, ConnectionState currentState)
		{
			this.originalState = originalState;
			this.currentState = currentState;
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x00200788 File Offset: 0x001FFB88
		public ConnectionState CurrentState
		{
			get
			{
				return this.currentState;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x0020079C File Offset: 0x001FFB9C
		public ConnectionState OriginalState
		{
			get
			{
				return this.originalState;
			}
		}

		// Token: 0x0400094A RID: 2378
		private ConnectionState originalState;

		// Token: 0x0400094B RID: 2379
		private ConnectionState currentState;
	}
}
