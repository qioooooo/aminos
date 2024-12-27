using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200082E RID: 2094
	[ComVisible(true)]
	[Serializable]
	public struct ParameterToken
	{
		// Token: 0x06004BA6 RID: 19366 RVA: 0x0010A108 File Offset: 0x00109108
		internal ParameterToken(int tkParam)
		{
			this.m_tkParameter = tkParam;
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06004BA7 RID: 19367 RVA: 0x0010A111 File Offset: 0x00109111
		public int Token
		{
			get
			{
				return this.m_tkParameter;
			}
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x0010A119 File Offset: 0x00109119
		public override int GetHashCode()
		{
			return this.m_tkParameter;
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x0010A121 File Offset: 0x00109121
		public override bool Equals(object obj)
		{
			return obj is ParameterToken && this.Equals((ParameterToken)obj);
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x0010A139 File Offset: 0x00109139
		public bool Equals(ParameterToken obj)
		{
			return obj.m_tkParameter == this.m_tkParameter;
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x0010A14A File Offset: 0x0010914A
		public static bool operator ==(ParameterToken a, ParameterToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x0010A154 File Offset: 0x00109154
		public static bool operator !=(ParameterToken a, ParameterToken b)
		{
			return !(a == b);
		}

		// Token: 0x0400277E RID: 10110
		public static readonly ParameterToken Empty = default(ParameterToken);

		// Token: 0x0400277F RID: 10111
		internal int m_tkParameter;
	}
}
