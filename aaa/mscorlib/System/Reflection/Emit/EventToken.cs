using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200080E RID: 2062
	[ComVisible(true)]
	[Serializable]
	public struct EventToken
	{
		// Token: 0x060049D9 RID: 18905 RVA: 0x00101A48 File Offset: 0x00100A48
		internal EventToken(int str)
		{
			this.m_event = str;
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x060049DA RID: 18906 RVA: 0x00101A51 File Offset: 0x00100A51
		public int Token
		{
			get
			{
				return this.m_event;
			}
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x00101A59 File Offset: 0x00100A59
		public override int GetHashCode()
		{
			return this.m_event;
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x00101A61 File Offset: 0x00100A61
		public override bool Equals(object obj)
		{
			return obj is EventToken && this.Equals((EventToken)obj);
		}

		// Token: 0x060049DD RID: 18909 RVA: 0x00101A79 File Offset: 0x00100A79
		public bool Equals(EventToken obj)
		{
			return obj.m_event == this.m_event;
		}

		// Token: 0x060049DE RID: 18910 RVA: 0x00101A8A File Offset: 0x00100A8A
		public static bool operator ==(EventToken a, EventToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x060049DF RID: 18911 RVA: 0x00101A94 File Offset: 0x00100A94
		public static bool operator !=(EventToken a, EventToken b)
		{
			return !(a == b);
		}

		// Token: 0x04002594 RID: 9620
		public static readonly EventToken Empty = default(EventToken);

		// Token: 0x04002595 RID: 9621
		internal int m_event;
	}
}
