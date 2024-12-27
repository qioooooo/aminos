using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000830 RID: 2096
	[ComVisible(true)]
	[Serializable]
	public struct PropertyToken
	{
		// Token: 0x06004BCF RID: 19407 RVA: 0x0010A54B File Offset: 0x0010954B
		internal PropertyToken(int str)
		{
			this.m_property = str;
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06004BD0 RID: 19408 RVA: 0x0010A554 File Offset: 0x00109554
		public int Token
		{
			get
			{
				return this.m_property;
			}
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x0010A55C File Offset: 0x0010955C
		public override int GetHashCode()
		{
			return this.m_property;
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x0010A564 File Offset: 0x00109564
		public override bool Equals(object obj)
		{
			return obj is PropertyToken && this.Equals((PropertyToken)obj);
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x0010A57C File Offset: 0x0010957C
		public bool Equals(PropertyToken obj)
		{
			return obj.m_property == this.m_property;
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x0010A58D File Offset: 0x0010958D
		public static bool operator ==(PropertyToken a, PropertyToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x0010A597 File Offset: 0x00109597
		public static bool operator !=(PropertyToken a, PropertyToken b)
		{
			return !(a == b);
		}

		// Token: 0x0400278A RID: 10122
		public static readonly PropertyToken Empty = default(PropertyToken);

		// Token: 0x0400278B RID: 10123
		internal int m_property;
	}
}
