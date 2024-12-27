using System;

namespace System.Drawing
{
	// Token: 0x020000E7 RID: 231
	public struct CharacterRange
	{
		// Token: 0x06000D23 RID: 3363 RVA: 0x0002722F File Offset: 0x0002622F
		public CharacterRange(int First, int Length)
		{
			this.first = First;
			this.length = Length;
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x0002723F File Offset: 0x0002623F
		// (set) Token: 0x06000D25 RID: 3365 RVA: 0x00027247 File Offset: 0x00026247
		public int First
		{
			get
			{
				return this.first;
			}
			set
			{
				this.first = value;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x00027250 File Offset: 0x00026250
		// (set) Token: 0x06000D27 RID: 3367 RVA: 0x00027258 File Offset: 0x00026258
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00027264 File Offset: 0x00026264
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(CharacterRange))
			{
				return false;
			}
			CharacterRange characterRange = (CharacterRange)obj;
			return this.first == characterRange.First && this.length == characterRange.Length;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x000272AC File Offset: 0x000262AC
		public static bool operator ==(CharacterRange cr1, CharacterRange cr2)
		{
			return cr1.First == cr2.First && cr1.Length == cr2.Length;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000272D0 File Offset: 0x000262D0
		public static bool operator !=(CharacterRange cr1, CharacterRange cr2)
		{
			return !(cr1 == cr2);
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x000272DC File Offset: 0x000262DC
		public override int GetHashCode()
		{
			return this.first << 8 + this.length;
		}

		// Token: 0x04000B21 RID: 2849
		private int first;

		// Token: 0x04000B22 RID: 2850
		private int length;
	}
}
