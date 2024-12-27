using System;

namespace System.Security.Util
{
	// Token: 0x02000473 RID: 1139
	internal struct TokenBasedSetEnumerator
	{
		// Token: 0x06002DCF RID: 11727 RVA: 0x0009A917 File Offset: 0x00099917
		public bool MoveNext()
		{
			return this._tb != null && this._tb.MoveNext(ref this);
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x0009A92F File Offset: 0x0009992F
		public void Reset()
		{
			this.Index = -1;
			this.Current = null;
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x0009A93F File Offset: 0x0009993F
		public TokenBasedSetEnumerator(TokenBasedSet tb)
		{
			this.Index = -1;
			this.Current = null;
			this._tb = tb;
		}

		// Token: 0x04001766 RID: 5990
		public object Current;

		// Token: 0x04001767 RID: 5991
		public int Index;

		// Token: 0x04001768 RID: 5992
		private TokenBasedSet _tb;
	}
}
