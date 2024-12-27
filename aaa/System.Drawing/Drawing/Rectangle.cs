using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x0200005C RID: 92
	[ComVisible(true)]
	[TypeConverter(typeof(RectangleConverter))]
	[Serializable]
	public struct Rectangle
	{
		// Token: 0x0600059E RID: 1438 RVA: 0x00017C47 File Offset: 0x00016C47
		public Rectangle(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00017C66 File Offset: 0x00016C66
		public Rectangle(Point location, Size size)
		{
			this.x = location.X;
			this.y = location.Y;
			this.width = size.Width;
			this.height = size.Height;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00017C9C File Offset: 0x00016C9C
		public static Rectangle FromLTRB(int left, int top, int right, int bottom)
		{
			return new Rectangle(left, top, right - left, bottom - top);
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00017CAB File Offset: 0x00016CAB
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x00017CBE File Offset: 0x00016CBE
		[Browsable(false)]
		public Point Location
		{
			get
			{
				return new Point(this.X, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00017CDA File Offset: 0x00016CDA
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x00017CED File Offset: 0x00016CED
		[Browsable(false)]
		public Size Size
		{
			get
			{
				return new Size(this.Width, this.Height);
			}
			set
			{
				this.Width = value.Width;
				this.Height = value.Height;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00017D09 File Offset: 0x00016D09
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x00017D11 File Offset: 0x00016D11
		public int X
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

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00017D1A File Offset: 0x00016D1A
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00017D22 File Offset: 0x00016D22
		public int Y
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

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00017D2B File Offset: 0x00016D2B
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x00017D33 File Offset: 0x00016D33
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

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00017D3C File Offset: 0x00016D3C
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00017D44 File Offset: 0x00016D44
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

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00017D4D File Offset: 0x00016D4D
		[Browsable(false)]
		public int Left
		{
			get
			{
				return this.X;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00017D55 File Offset: 0x00016D55
		[Browsable(false)]
		public int Top
		{
			get
			{
				return this.Y;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x00017D5D File Offset: 0x00016D5D
		[Browsable(false)]
		public int Right
		{
			get
			{
				return this.X + this.Width;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00017D6C File Offset: 0x00016D6C
		[Browsable(false)]
		public int Bottom
		{
			get
			{
				return this.Y + this.Height;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x00017D7B File Offset: 0x00016D7B
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.height == 0 && this.width == 0 && this.x == 0 && this.y == 0;
			}
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00017DA0 File Offset: 0x00016DA0
		public override bool Equals(object obj)
		{
			if (!(obj is Rectangle))
			{
				return false;
			}
			Rectangle rectangle = (Rectangle)obj;
			return rectangle.X == this.X && rectangle.Y == this.Y && rectangle.Width == this.Width && rectangle.Height == this.Height;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00017DFC File Offset: 0x00016DFC
		public static bool operator ==(Rectangle left, Rectangle right)
		{
			return left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00017E4B File Offset: 0x00016E4B
		public static bool operator !=(Rectangle left, Rectangle right)
		{
			return !(left == right);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00017E57 File Offset: 0x00016E57
		public static Rectangle Ceiling(RectangleF value)
		{
			return new Rectangle((int)Math.Ceiling((double)value.X), (int)Math.Ceiling((double)value.Y), (int)Math.Ceiling((double)value.Width), (int)Math.Ceiling((double)value.Height));
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00017E96 File Offset: 0x00016E96
		public static Rectangle Truncate(RectangleF value)
		{
			return new Rectangle((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00017EBD File Offset: 0x00016EBD
		public static Rectangle Round(RectangleF value)
		{
			return new Rectangle((int)Math.Round((double)value.X), (int)Math.Round((double)value.Y), (int)Math.Round((double)value.Width), (int)Math.Round((double)value.Height));
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00017EFC File Offset: 0x00016EFC
		public bool Contains(int x, int y)
		{
			return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00017F32 File Offset: 0x00016F32
		public bool Contains(Point pt)
		{
			return this.Contains(pt.X, pt.Y);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00017F48 File Offset: 0x00016F48
		public bool Contains(Rectangle rect)
		{
			return this.X <= rect.X && rect.X + rect.Width <= this.X + this.Width && this.Y <= rect.Y && rect.Y + rect.Height <= this.Y + this.Height;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00017FB4 File Offset: 0x00016FB4
		public override int GetHashCode()
		{
			return this.X ^ ((this.Y << 13) | (int)((uint)this.Y >> 19)) ^ ((this.Width << 26) | (int)((uint)this.Width >> 6)) ^ ((this.Height << 7) | (int)((uint)this.Height >> 25));
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00018001 File Offset: 0x00017001
		public void Inflate(int width, int height)
		{
			this.X -= width;
			this.Y -= height;
			this.Width += 2 * width;
			this.Height += 2 * height;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001803F File Offset: 0x0001703F
		public void Inflate(Size size)
		{
			this.Inflate(size.Width, size.Height);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00018058 File Offset: 0x00017058
		public static Rectangle Inflate(Rectangle rect, int x, int y)
		{
			Rectangle rectangle = rect;
			rectangle.Inflate(x, y);
			return rectangle;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00018074 File Offset: 0x00017074
		public void Intersect(Rectangle rect)
		{
			Rectangle rectangle = Rectangle.Intersect(rect, this);
			this.X = rectangle.X;
			this.Y = rectangle.Y;
			this.Width = rectangle.Width;
			this.Height = rectangle.Height;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000180C4 File Offset: 0x000170C4
		public static Rectangle Intersect(Rectangle a, Rectangle b)
		{
			int num = Math.Max(a.X, b.X);
			int num2 = Math.Min(a.X + a.Width, b.X + b.Width);
			int num3 = Math.Max(a.Y, b.Y);
			int num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
			if (num2 >= num && num4 >= num3)
			{
				return new Rectangle(num, num3, num2 - num, num4 - num3);
			}
			return Rectangle.Empty;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001815C File Offset: 0x0001715C
		public bool IntersectsWith(Rectangle rect)
		{
			return rect.X < this.X + this.Width && this.X < rect.X + rect.Width && rect.Y < this.Y + this.Height && this.Y < rect.Y + rect.Height;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000181C8 File Offset: 0x000171C8
		public static Rectangle Union(Rectangle a, Rectangle b)
		{
			int num = Math.Min(a.X, b.X);
			int num2 = Math.Max(a.X + a.Width, b.X + b.Width);
			int num3 = Math.Min(a.Y, b.Y);
			int num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
			return new Rectangle(num, num3, num2 - num, num4 - num3);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00018252 File Offset: 0x00017252
		public void Offset(Point pos)
		{
			this.Offset(pos.X, pos.Y);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00018268 File Offset: 0x00017268
		public void Offset(int x, int y)
		{
			this.X += x;
			this.Y += y;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00018288 File Offset: 0x00017288
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

		// Token: 0x04000463 RID: 1123
		public static readonly Rectangle Empty = default(Rectangle);

		// Token: 0x04000464 RID: 1124
		private int x;

		// Token: 0x04000465 RID: 1125
		private int y;

		// Token: 0x04000466 RID: 1126
		private int width;

		// Token: 0x04000467 RID: 1127
		private int height;
	}
}
