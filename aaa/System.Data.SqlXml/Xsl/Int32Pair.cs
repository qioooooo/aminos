using System;

namespace System.Xml.Xsl
{
	// Token: 0x02000006 RID: 6
	internal struct Int32Pair
	{
		// Token: 0x06000013 RID: 19 RVA: 0x0000229D File Offset: 0x0000129D
		public Int32Pair(int left, int right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000022AD File Offset: 0x000012AD
		public int Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000022B5 File Offset: 0x000012B5
		public int Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022C0 File Offset: 0x000012C0
		public override bool Equals(object other)
		{
			if (other is Int32Pair)
			{
				Int32Pair int32Pair = (Int32Pair)other;
				return this.left == int32Pair.left && this.right == int32Pair.right;
			}
			return false;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000022FE File Offset: 0x000012FE
		public override int GetHashCode()
		{
			return this.left.GetHashCode() ^ this.right.GetHashCode();
		}

		// Token: 0x040000AA RID: 170
		private int left;

		// Token: 0x040000AB RID: 171
		private int right;
	}
}
