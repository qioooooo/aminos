using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Drawing.Printing
{
	// Token: 0x0200010B RID: 267
	[TypeConverter(typeof(MarginsConverter))]
	[Serializable]
	public class Margins : ICloneable
	{
		// Token: 0x06000E34 RID: 3636 RVA: 0x00029C84 File Offset: 0x00028C84
		public Margins()
			: this(100, 100, 100, 100)
		{
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00029C94 File Offset: 0x00028C94
		public Margins(int left, int right, int top, int bottom)
		{
			this.CheckMargin(left, "left");
			this.CheckMargin(right, "right");
			this.CheckMargin(top, "top");
			this.CheckMargin(bottom, "bottom");
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00029CF5 File Offset: 0x00028CF5
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x00029CFD File Offset: 0x00028CFD
		public int Left
		{
			get
			{
				return this.left;
			}
			set
			{
				this.CheckMargin(value, "Left");
				this.left = value;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00029D12 File Offset: 0x00028D12
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x00029D1A File Offset: 0x00028D1A
		public int Right
		{
			get
			{
				return this.right;
			}
			set
			{
				this.CheckMargin(value, "Right");
				this.right = value;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00029D2F File Offset: 0x00028D2F
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x00029D37 File Offset: 0x00028D37
		public int Top
		{
			get
			{
				return this.top;
			}
			set
			{
				this.CheckMargin(value, "Top");
				this.top = value;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x00029D4C File Offset: 0x00028D4C
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x00029D54 File Offset: 0x00028D54
		public int Bottom
		{
			get
			{
				return this.bottom;
			}
			set
			{
				this.CheckMargin(value, "Bottom");
				this.bottom = value;
			}
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00029D6C File Offset: 0x00028D6C
		private void CheckMargin(int margin, string name)
		{
			if (margin < 0)
			{
				throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", new object[] { name, margin, "0" }));
			}
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00029DAA File Offset: 0x00028DAA
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00029DB4 File Offset: 0x00028DB4
		public override bool Equals(object obj)
		{
			Margins margins = obj as Margins;
			return margins == this || (!(margins == null) && (margins.left == this.left && margins.right == this.right && margins.top == this.top) && margins.bottom == this.bottom);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00029E18 File Offset: 0x00028E18
		public override int GetHashCode()
		{
			uint num = (uint)this.left;
			uint num2 = (uint)this.right;
			uint num3 = (uint)this.top;
			uint num4 = (uint)this.bottom;
			return (int)(num ^ ((num2 << 13) | (num2 >> 19)) ^ ((num3 << 26) | (num3 >> 6)) ^ ((num4 << 7) | (num4 >> 25)));
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00029E64 File Offset: 0x00028E64
		public static bool operator ==(Margins m1, Margins m2)
		{
			return object.ReferenceEquals(m1, null) == object.ReferenceEquals(m2, null) && (object.ReferenceEquals(m1, null) || (m1.Left == m2.Left && m1.Top == m2.Top && m1.Right == m2.Right && m1.Bottom == m2.Bottom));
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00029EC8 File Offset: 0x00028EC8
		public static bool operator !=(Margins m1, Margins m2)
		{
			return !(m1 == m2);
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00029ED4 File Offset: 0x00028ED4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[Margins Left=",
				this.Left.ToString(CultureInfo.InvariantCulture),
				" Right=",
				this.Right.ToString(CultureInfo.InvariantCulture),
				" Top=",
				this.Top.ToString(CultureInfo.InvariantCulture),
				" Bottom=",
				this.Bottom.ToString(CultureInfo.InvariantCulture),
				"]"
			});
		}

		// Token: 0x04000B84 RID: 2948
		private int left;

		// Token: 0x04000B85 RID: 2949
		private int right;

		// Token: 0x04000B86 RID: 2950
		private int top;

		// Token: 0x04000B87 RID: 2951
		private int bottom;
	}
}
