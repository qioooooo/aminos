using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000817 RID: 2071
	[ComVisible(true)]
	[Serializable]
	public struct Label
	{
		// Token: 0x06004A29 RID: 18985 RVA: 0x001028B0 File Offset: 0x001018B0
		internal Label(int label)
		{
			this.m_label = label;
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x001028B9 File Offset: 0x001018B9
		internal int GetLabelValue()
		{
			return this.m_label;
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x001028C1 File Offset: 0x001018C1
		public override int GetHashCode()
		{
			return this.m_label;
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x001028C9 File Offset: 0x001018C9
		public override bool Equals(object obj)
		{
			return obj is Label && this.Equals((Label)obj);
		}

		// Token: 0x06004A2D RID: 18989 RVA: 0x001028E1 File Offset: 0x001018E1
		public bool Equals(Label obj)
		{
			return obj.m_label == this.m_label;
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x001028F2 File Offset: 0x001018F2
		public static bool operator ==(Label a, Label b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004A2F RID: 18991 RVA: 0x001028FC File Offset: 0x001018FC
		public static bool operator !=(Label a, Label b)
		{
			return !(a == b);
		}

		// Token: 0x040025CF RID: 9679
		internal int m_label;
	}
}
