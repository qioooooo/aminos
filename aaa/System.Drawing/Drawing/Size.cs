using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x02000060 RID: 96
	[ComVisible(true)]
	[TypeConverter(typeof(SizeConverter))]
	[Serializable]
	public struct Size
	{
		// Token: 0x0600060C RID: 1548 RVA: 0x00019514 File Offset: 0x00018514
		public Size(Point pt)
		{
			this.width = pt.X;
			this.height = pt.Y;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00019530 File Offset: 0x00018530
		public Size(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00019540 File Offset: 0x00018540
		public static implicit operator SizeF(Size p)
		{
			return new SizeF((float)p.Width, (float)p.Height);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00019557 File Offset: 0x00018557
		public static Size operator +(Size sz1, Size sz2)
		{
			return Size.Add(sz1, sz2);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00019560 File Offset: 0x00018560
		public static Size operator -(Size sz1, Size sz2)
		{
			return Size.Subtract(sz1, sz2);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00019569 File Offset: 0x00018569
		public static bool operator ==(Size sz1, Size sz2)
		{
			return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001958D File Offset: 0x0001858D
		public static bool operator !=(Size sz1, Size sz2)
		{
			return !(sz1 == sz2);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00019599 File Offset: 0x00018599
		public static explicit operator Point(Size size)
		{
			return new Point(size.Width, size.Height);
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x000195AE File Offset: 0x000185AE
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.width == 0 && this.height == 0;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x000195C3 File Offset: 0x000185C3
		// (set) Token: 0x06000616 RID: 1558 RVA: 0x000195CB File Offset: 0x000185CB
		public int Width
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

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x000195D4 File Offset: 0x000185D4
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x000195DC File Offset: 0x000185DC
		public int Height
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

		// Token: 0x06000619 RID: 1561 RVA: 0x000195E5 File Offset: 0x000185E5
		public static Size Add(Size sz1, Size sz2)
		{
			return new Size(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001960A File Offset: 0x0001860A
		public static Size Ceiling(SizeF value)
		{
			return new Size((int)Math.Ceiling((double)value.Width), (int)Math.Ceiling((double)value.Height));
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0001962D File Offset: 0x0001862D
		public static Size Subtract(Size sz1, Size sz2)
		{
			return new Size(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00019652 File Offset: 0x00018652
		public static Size Truncate(SizeF value)
		{
			return new Size((int)value.Width, (int)value.Height);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00019669 File Offset: 0x00018669
		public static Size Round(SizeF value)
		{
			return new Size((int)Math.Round((double)value.Width), (int)Math.Round((double)value.Height));
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001968C File Offset: 0x0001868C
		public override bool Equals(object obj)
		{
			if (!(obj is Size))
			{
				return false;
			}
			Size size = (Size)obj;
			return size.width == this.width && size.height == this.height;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x000196CA File Offset: 0x000186CA
		public override int GetHashCode()
		{
			return this.width ^ this.height;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000196DC File Offset: 0x000186DC
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

		// Token: 0x0400047A RID: 1146
		public static readonly Size Empty = default(Size);

		// Token: 0x0400047B RID: 1147
		private int width;

		// Token: 0x0400047C RID: 1148
		private int height;
	}
}
