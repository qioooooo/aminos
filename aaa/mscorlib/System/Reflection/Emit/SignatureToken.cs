using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000832 RID: 2098
	[ComVisible(true)]
	public struct SignatureToken
	{
		// Token: 0x06004C10 RID: 19472 RVA: 0x0010B6FC File Offset: 0x0010A6FC
		internal SignatureToken(int str, ModuleBuilder mod)
		{
			this.m_signature = str;
			this.m_moduleBuilder = mod;
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06004C11 RID: 19473 RVA: 0x0010B70C File Offset: 0x0010A70C
		public int Token
		{
			get
			{
				return this.m_signature;
			}
		}

		// Token: 0x06004C12 RID: 19474 RVA: 0x0010B714 File Offset: 0x0010A714
		public override int GetHashCode()
		{
			return this.m_signature;
		}

		// Token: 0x06004C13 RID: 19475 RVA: 0x0010B71C File Offset: 0x0010A71C
		public override bool Equals(object obj)
		{
			return obj is SignatureToken && this.Equals((SignatureToken)obj);
		}

		// Token: 0x06004C14 RID: 19476 RVA: 0x0010B734 File Offset: 0x0010A734
		public bool Equals(SignatureToken obj)
		{
			return obj.m_signature == this.m_signature;
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x0010B745 File Offset: 0x0010A745
		public static bool operator ==(SignatureToken a, SignatureToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x0010B74F File Offset: 0x0010A74F
		public static bool operator !=(SignatureToken a, SignatureToken b)
		{
			return !(a == b);
		}

		// Token: 0x040027C9 RID: 10185
		public static readonly SignatureToken Empty = default(SignatureToken);

		// Token: 0x040027CA RID: 10186
		internal int m_signature;

		// Token: 0x040027CB RID: 10187
		internal ModuleBuilder m_moduleBuilder;
	}
}
