using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002C3 RID: 707
	[ComVisible(true)]
	public struct SymbolToken
	{
		// Token: 0x06001BAE RID: 7086 RVA: 0x000482DC File Offset: 0x000472DC
		public SymbolToken(int val)
		{
			this.m_token = val;
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x000482E5 File Offset: 0x000472E5
		public int GetToken()
		{
			return this.m_token;
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x000482ED File Offset: 0x000472ED
		public override int GetHashCode()
		{
			return this.m_token;
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x000482F5 File Offset: 0x000472F5
		public override bool Equals(object obj)
		{
			return obj is SymbolToken && this.Equals((SymbolToken)obj);
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x0004830D File Offset: 0x0004730D
		public bool Equals(SymbolToken obj)
		{
			return obj.m_token == this.m_token;
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x0004831E File Offset: 0x0004731E
		public static bool operator ==(SymbolToken a, SymbolToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x00048328 File Offset: 0x00047328
		public static bool operator !=(SymbolToken a, SymbolToken b)
		{
			return !(a == b);
		}

		// Token: 0x04000A74 RID: 2676
		internal int m_token;
	}
}
