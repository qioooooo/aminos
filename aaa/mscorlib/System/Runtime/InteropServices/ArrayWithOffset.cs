using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004C3 RID: 1219
	[ComVisible(true)]
	[Serializable]
	public struct ArrayWithOffset
	{
		// Token: 0x060030E2 RID: 12514 RVA: 0x000A8B1D File Offset: 0x000A7B1D
		public ArrayWithOffset(object array, int offset)
		{
			this.m_array = array;
			this.m_offset = offset;
			this.m_count = 0;
			this.m_count = this.CalculateCount();
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x000A8B40 File Offset: 0x000A7B40
		public object GetArray()
		{
			return this.m_array;
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x000A8B48 File Offset: 0x000A7B48
		public int GetOffset()
		{
			return this.m_offset;
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x000A8B50 File Offset: 0x000A7B50
		public override int GetHashCode()
		{
			return this.m_count + this.m_offset;
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000A8B5F File Offset: 0x000A7B5F
		public override bool Equals(object obj)
		{
			return obj is ArrayWithOffset && this.Equals((ArrayWithOffset)obj);
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x000A8B77 File Offset: 0x000A7B77
		public bool Equals(ArrayWithOffset obj)
		{
			return obj.m_array == this.m_array && obj.m_offset == this.m_offset && obj.m_count == this.m_count;
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000A8BA8 File Offset: 0x000A7BA8
		public static bool operator ==(ArrayWithOffset a, ArrayWithOffset b)
		{
			return a.Equals(b);
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x000A8BB2 File Offset: 0x000A7BB2
		public static bool operator !=(ArrayWithOffset a, ArrayWithOffset b)
		{
			return !(a == b);
		}

		// Token: 0x060030EA RID: 12522
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int CalculateCount();

		// Token: 0x04001898 RID: 6296
		private object m_array;

		// Token: 0x04001899 RID: 6297
		private int m_offset;

		// Token: 0x0400189A RID: 6298
		private int m_count;
	}
}
