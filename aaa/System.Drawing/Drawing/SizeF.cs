using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x020000E2 RID: 226
	[TypeConverter(typeof(SizeFConverter))]
	[ComVisible(true)]
	[Serializable]
	public struct SizeF
	{
		// Token: 0x06000D05 RID: 3333 RVA: 0x00026D2B File Offset: 0x00025D2B
		public SizeF(SizeF size)
		{
			this.width = size.width;
			this.height = size.height;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00026D47 File Offset: 0x00025D47
		public SizeF(PointF pt)
		{
			this.width = pt.X;
			this.height = pt.Y;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00026D63 File Offset: 0x00025D63
		public SizeF(float width, float height)
		{
			this.width = width;
			this.height = height;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00026D73 File Offset: 0x00025D73
		public static SizeF operator +(SizeF sz1, SizeF sz2)
		{
			return SizeF.Add(sz1, sz2);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00026D7C File Offset: 0x00025D7C
		public static SizeF operator -(SizeF sz1, SizeF sz2)
		{
			return SizeF.Subtract(sz1, sz2);
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00026D85 File Offset: 0x00025D85
		public static bool operator ==(SizeF sz1, SizeF sz2)
		{
			return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00026DA9 File Offset: 0x00025DA9
		public static bool operator !=(SizeF sz1, SizeF sz2)
		{
			return !(sz1 == sz2);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00026DB5 File Offset: 0x00025DB5
		public static explicit operator PointF(SizeF size)
		{
			return new PointF(size.Width, size.Height);
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00026DCA File Offset: 0x00025DCA
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.width == 0f && this.height == 0f;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00026DE8 File Offset: 0x00025DE8
		// (set) Token: 0x06000D0F RID: 3343 RVA: 0x00026DF0 File Offset: 0x00025DF0
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

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00026DF9 File Offset: 0x00025DF9
		// (set) Token: 0x06000D11 RID: 3345 RVA: 0x00026E01 File Offset: 0x00025E01
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

		// Token: 0x06000D12 RID: 3346 RVA: 0x00026E0A File Offset: 0x00025E0A
		public static SizeF Add(SizeF sz1, SizeF sz2)
		{
			return new SizeF(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00026E2F File Offset: 0x00025E2F
		public static SizeF Subtract(SizeF sz1, SizeF sz2)
		{
			return new SizeF(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00026E54 File Offset: 0x00025E54
		public override bool Equals(object obj)
		{
			if (!(obj is SizeF))
			{
				return false;
			}
			SizeF sizeF = (SizeF)obj;
			return sizeF.Width == this.Width && sizeF.Height == this.Height && sizeF.GetType().Equals(base.GetType());
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00026EB2 File Offset: 0x00025EB2
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00026EC4 File Offset: 0x00025EC4
		public PointF ToPointF()
		{
			return (PointF)this;
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00026ED1 File Offset: 0x00025ED1
		public Size ToSize()
		{
			return Size.Truncate(this);
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00026EE0 File Offset: 0x00025EE0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{Width=",
				this.width.ToString(CultureInfo.CurrentCulture),
				", Height=",
				this.height.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		// Token: 0x04000B0E RID: 2830
		public static readonly SizeF Empty = default(SizeF);

		// Token: 0x04000B0F RID: 2831
		private float width;

		// Token: 0x04000B10 RID: 2832
		private float height;
	}
}
