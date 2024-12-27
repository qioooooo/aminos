using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200083A RID: 2106
	[ComVisible(true)]
	[Serializable]
	public struct TypeToken
	{
		// Token: 0x06004D9D RID: 19869 RVA: 0x0010F0FB File Offset: 0x0010E0FB
		internal TypeToken(int str)
		{
			this.m_class = str;
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06004D9E RID: 19870 RVA: 0x0010F104 File Offset: 0x0010E104
		public int Token
		{
			get
			{
				return this.m_class;
			}
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x0010F10C File Offset: 0x0010E10C
		public override int GetHashCode()
		{
			return this.m_class;
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x0010F114 File Offset: 0x0010E114
		public override bool Equals(object obj)
		{
			return obj is TypeToken && this.Equals((TypeToken)obj);
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x0010F12C File Offset: 0x0010E12C
		public bool Equals(TypeToken obj)
		{
			return obj.m_class == this.m_class;
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x0010F13D File Offset: 0x0010E13D
		public static bool operator ==(TypeToken a, TypeToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x0010F147 File Offset: 0x0010E147
		public static bool operator !=(TypeToken a, TypeToken b)
		{
			return !(a == b);
		}

		// Token: 0x040027FE RID: 10238
		public static readonly TypeToken Empty = default(TypeToken);

		// Token: 0x040027FF RID: 10239
		internal int m_class;
	}
}
