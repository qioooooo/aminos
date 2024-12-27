using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000823 RID: 2083
	[ComVisible(true)]
	[Serializable]
	public struct MethodToken
	{
		// Token: 0x06004B11 RID: 19217 RVA: 0x00105638 File Offset: 0x00104638
		internal MethodToken(int str)
		{
			this.m_method = str;
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06004B12 RID: 19218 RVA: 0x00105641 File Offset: 0x00104641
		public int Token
		{
			get
			{
				return this.m_method;
			}
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x00105649 File Offset: 0x00104649
		public override int GetHashCode()
		{
			return this.m_method;
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x00105651 File Offset: 0x00104651
		public override bool Equals(object obj)
		{
			return obj is MethodToken && this.Equals((MethodToken)obj);
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x00105669 File Offset: 0x00104669
		public bool Equals(MethodToken obj)
		{
			return obj.m_method == this.m_method;
		}

		// Token: 0x06004B16 RID: 19222 RVA: 0x0010567A File Offset: 0x0010467A
		public static bool operator ==(MethodToken a, MethodToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x00105684 File Offset: 0x00104684
		public static bool operator !=(MethodToken a, MethodToken b)
		{
			return !(a == b);
		}

		// Token: 0x04002634 RID: 9780
		public static readonly MethodToken Empty = default(MethodToken);

		// Token: 0x04002635 RID: 9781
		internal int m_method;
	}
}
