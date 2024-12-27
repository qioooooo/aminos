using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000833 RID: 2099
	[ComVisible(true)]
	[Serializable]
	public struct StringToken
	{
		// Token: 0x06004C18 RID: 19480 RVA: 0x0010B768 File Offset: 0x0010A768
		internal StringToken(int str)
		{
			this.m_string = str;
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06004C19 RID: 19481 RVA: 0x0010B771 File Offset: 0x0010A771
		public int Token
		{
			get
			{
				return this.m_string;
			}
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x0010B779 File Offset: 0x0010A779
		public override int GetHashCode()
		{
			return this.m_string;
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x0010B781 File Offset: 0x0010A781
		public override bool Equals(object obj)
		{
			return obj is StringToken && this.Equals((StringToken)obj);
		}

		// Token: 0x06004C1C RID: 19484 RVA: 0x0010B799 File Offset: 0x0010A799
		public bool Equals(StringToken obj)
		{
			return obj.m_string == this.m_string;
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x0010B7AA File Offset: 0x0010A7AA
		public static bool operator ==(StringToken a, StringToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x0010B7B4 File Offset: 0x0010A7B4
		public static bool operator !=(StringToken a, StringToken b)
		{
			return !(a == b);
		}

		// Token: 0x040027CC RID: 10188
		internal int m_string;
	}
}
