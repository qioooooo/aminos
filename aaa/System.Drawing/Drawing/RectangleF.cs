using System;
using System.ComponentModel;
using System.Drawing.Internal;
using System.Globalization;

namespace System.Drawing
{
	// Token: 0x020000DF RID: 223
	[Serializable]
	public struct RectangleF
	{
		// Token: 0x06000CD5 RID: 3285 RVA: 0x000265BF File Offset: 0x000255BF
		public RectangleF(float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x000265DE File Offset: 0x000255DE
		public RectangleF(PointF location, SizeF size)
		{
			this.x = location.X;
			this.y = location.Y;
			this.width = size.Width;
			this.height = size.Height;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00026614 File Offset: 0x00025614
		public static RectangleF FromLTRB(float left, float top, float right, float bottom)
		{
			return new RectangleF(left, top, right - left, bottom - top);
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00026623 File Offset: 0x00025623
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x00026636 File Offset: 0x00025636
		[Browsable(false)]
		public PointF Location
		{
			get
			{
				return new PointF(this.X, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00026652 File Offset: 0x00025652
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x00026665 File Offset: 0x00025665
		[Browsable(false)]
		public SizeF Size
		{
			get
			{
				return new SizeF(this.Width, this.Height);
			}
			set
			{
				this.Width = value.Width;
				this.Height = value.Height;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x00026681 File Offset: 0x00025681
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x00026689 File Offset: 0x00025689
		public float X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00026692 File Offset: 0x00025692
		// (set) Token: 0x06000CDF RID: 3295 RVA: 0x0002669A File Offset: 0x0002569A
		public float Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x000266A3 File Offset: 0x000256A3
		// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x000266AB File Offset: 0x000256AB
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x000266B4 File Offset: 0x000256B4
		// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x000266BC File Offset: 0x000256BC
		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x000266C5 File Offset: 0x000256C5
		[Browsable(false)]
		public float Left
		{
			get
			{
				return this.X;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x000266CD File Offset: 0x000256CD
		[Browsable(false)]
		public float Top
		{
			get
			{
				return this.Y;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x000266D5 File Offset: 0x000256D5
		[Browsable(false)]
		public float Right
		{
			get
			{
				return this.X + this.Width;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x000266E4 File Offset: 0x000256E4
		[Browsable(false)]
		public float Bottom
		{
			get
			{
				return this.Y + this.Height;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x000266F3 File Offset: 0x000256F3
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.Width <= 0f || this.Height <= 0f;
			}
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00026714 File Offset: 0x00025714
		public override bool Equals(object obj)
		{
			if (!(obj is RectangleF))
			{
				return false;
			}
			RectangleF rectangleF = (RectangleF)obj;
			return rectangleF.X == this.X && rectangleF.Y == this.Y && rectangleF.Width == this.Width && rectangleF.Height == this.Height;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00026770 File Offset: 0x00025770
		public static bool operator ==(RectangleF left, RectangleF right)
		{
			return left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x000267BF File Offset: 0x000257BF
		public static bool operator !=(RectangleF left, RectangleF right)
		{
			return !(left == right);
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x000267CB File Offset: 0x000257CB
		public bool Contains(float x, float y)
		{
			return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00026801 File Offset: 0x00025801
		public bool Contains(PointF pt)
		{
			return this.Contains(pt.X, pt.Y);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00026818 File Offset: 0x00025818
		public bool Contains(RectangleF rect)
		{
			return this.X <= rect.X && rect.X + rect.Width <= this.X + this.Width && this.Y <= rect.Y && rect.Y + rect.Height <= this.Y + this.Height;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00026884 File Offset: 0x00025884
		public override int GetHashCode()
		{
			return (int)((uint)this.X ^ (((uint)this.Y << 13) | ((uint)this.Y >> 19)) ^ (((uint)this.Width << 26) | ((uint)this.Width >> 6)) ^ (((uint)this.Height << 7) | ((uint)this.Height >> 25)));
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x000268D8 File Offset: 0x000258D8
		public void Inflate(float x, float y)
		{
			this.X -= x;
			this.Y -= y;
			this.Width += 2f * x;
			this.Height += 2f * y;
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00026929 File Offset: 0x00025929
		public void Inflate(SizeF size)
		{
			this.Inflate(size.Width, size.Height);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00026940 File Offset: 0x00025940
		public static RectangleF Inflate(RectangleF rect, float x, float y)
		{
			RectangleF rectangleF = rect;
			rectangleF.Inflate(x, y);
			return rectangleF;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002695C File Offset: 0x0002595C
		public void Intersect(RectangleF rect)
		{
			RectangleF rectangleF = RectangleF.Intersect(rect, this);
			this.X = rectangleF.X;
			this.Y = rectangleF.Y;
			this.Width = rectangleF.Width;
			this.Height = rectangleF.Height;
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000269AC File Offset: 0x000259AC
		public static RectangleF Intersect(RectangleF a, RectangleF b)
		{
			float num = Math.Max(a.X, b.X);
			float num2 = Math.Min(a.X + a.Width, b.X + b.Width);
			float num3 = Math.Max(a.Y, b.Y);
			float num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
			if (num2 >= num && num4 >= num3)
			{
				return new RectangleF(num, num3, num2 - num, num4 - num3);
			}
			return RectangleF.Empty;
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00026A44 File Offset: 0x00025A44
		public bool IntersectsWith(RectangleF rect)
		{
			return rect.X < this.X + this.Width && this.X < rect.X + rect.Width && rect.Y < this.Y + this.Height && this.Y < rect.Y + rect.Height;
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00026AB0 File Offset: 0x00025AB0
		public static RectangleF Union(RectangleF a, RectangleF b)
		{
			float num = Math.Min(a.X, b.X);
			float num2 = Math.Max(a.X + a.Width, b.X + b.Width);
			float num3 = Math.Min(a.Y, b.Y);
			float num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
			return new RectangleF(num, num3, num2 - num, num4 - num3);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00026B3A File Offset: 0x00025B3A
		public void Offset(PointF pos)
		{
			this.Offset(pos.X, pos.Y);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00026B50 File Offset: 0x00025B50
		public void Offset(float x, float y)
		{
			this.X += x;
			this.Y += y;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00026B6E File Offset: 0x00025B6E
		internal GPRECTF ToGPRECTF()
		{
			return new GPRECTF(this.X, this.Y, this.Width, this.Height);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00026B8D File Offset: 0x00025B8D
		public static implicit operator RectangleF(Rectangle r)
		{
			return new RectangleF((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00026BB4 File Offset: 0x00025BB4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{X=",
				this.X.ToString(CultureInfo.CurrentCulture),
				",Y=",
				this.Y.ToString(CultureInfo.CurrentCulture),
				",Width=",
				this.Width.ToString(CultureInfo.CurrentCulture),
				",Height=",
				this.Height.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		// Token: 0x04000B08 RID: 2824
		public static readonly RectangleF Empty = default(RectangleF);

		// Token: 0x04000B09 RID: 2825
		private float x;

		// Token: 0x04000B0A RID: 2826
		private float y;

		// Token: 0x04000B0B RID: 2827
		private float width;

		// Token: 0x04000B0C RID: 2828
		private float height;
	}
}
