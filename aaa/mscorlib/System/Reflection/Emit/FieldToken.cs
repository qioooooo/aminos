using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000810 RID: 2064
	[ComVisible(true)]
	[Serializable]
	public struct FieldToken
	{
		// Token: 0x060049FB RID: 18939 RVA: 0x00101E65 File Offset: 0x00100E65
		internal FieldToken(int field, Type fieldClass)
		{
			this.m_fieldTok = field;
			this.m_class = fieldClass;
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x060049FC RID: 18940 RVA: 0x00101E75 File Offset: 0x00100E75
		public int Token
		{
			get
			{
				return this.m_fieldTok;
			}
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x00101E7D File Offset: 0x00100E7D
		public override int GetHashCode()
		{
			return this.m_fieldTok;
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x00101E85 File Offset: 0x00100E85
		public override bool Equals(object obj)
		{
			return obj is FieldToken && this.Equals((FieldToken)obj);
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x00101E9D File Offset: 0x00100E9D
		public bool Equals(FieldToken obj)
		{
			return obj.m_fieldTok == this.m_fieldTok && obj.m_class == this.m_class;
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x00101EBF File Offset: 0x00100EBF
		public static bool operator ==(FieldToken a, FieldToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x00101EC9 File Offset: 0x00100EC9
		public static bool operator !=(FieldToken a, FieldToken b)
		{
			return !(a == b);
		}

		// Token: 0x0400259D RID: 9629
		public static readonly FieldToken Empty = default(FieldToken);

		// Token: 0x0400259E RID: 9630
		internal int m_fieldTok;

		// Token: 0x0400259F RID: 9631
		internal object m_class;
	}
}
