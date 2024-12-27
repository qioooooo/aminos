using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x02000059 RID: 89
	[ComVisible(true)]
	[TypeConverter(typeof(PointConverter))]
	[Serializable]
	public struct Point
	{
		// Token: 0x06000573 RID: 1395 RVA: 0x000174A3 File Offset: 0x000164A3
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x000174B3 File Offset: 0x000164B3
		public Point(Size sz)
		{
			this.x = sz.Width;
			this.y = sz.Height;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000174CF File Offset: 0x000164CF
		public Point(int dw)
		{
			this.x = (int)((short)Point.LOWORD(dw));
			this.y = (int)((short)Point.HIWORD(dw));
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x000174EB File Offset: 0x000164EB
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.x == 0 && this.y == 0;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00017500 File Offset: 0x00016500
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x00017508 File Offset: 0x00016508
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

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00017511 File Offset: 0x00016511
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x00017519 File Offset: 0x00016519
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

		// Token: 0x0600057B RID: 1403 RVA: 0x00017522 File Offset: 0x00016522
		public static implicit operator PointF(Point p)
		{
			return new PointF((float)p.X, (float)p.Y);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00017539 File Offset: 0x00016539
		public static explicit operator Size(Point p)
		{
			return new Size(p.X, p.Y);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001754E File Offset: 0x0001654E
		public static Point operator +(Point pt, Size sz)
		{
			return Point.Add(pt, sz);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00017557 File Offset: 0x00016557
		public static Point operator -(Point pt, Size sz)
		{
			return Point.Subtract(pt, sz);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00017560 File Offset: 0x00016560
		public static bool operator ==(Point left, Point right)
		{
			return left.X == right.X && left.Y == right.Y;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00017584 File Offset: 0x00016584
		public static bool operator !=(Point left, Point right)
		{
			return !(left == right);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00017590 File Offset: 0x00016590
		public static Point Add(Point pt, Size sz)
		{
			return new Point(pt.X + sz.Width, pt.Y + sz.Height);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000175B5 File Offset: 0x000165B5
		public static Point Subtract(Point pt, Size sz)
		{
			return new Point(pt.X - sz.Width, pt.Y - sz.Height);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000175DA File Offset: 0x000165DA
		public static Point Ceiling(PointF value)
		{
			return new Point((int)Math.Ceiling((double)value.X), (int)Math.Ceiling((double)value.Y));
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000175FD File Offset: 0x000165FD
		public static Point Truncate(PointF value)
		{
			return new Point((int)value.X, (int)value.Y);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00017614 File Offset: 0x00016614
		public static Point Round(PointF value)
		{
			return new Point((int)Math.Round((double)value.X), (int)Math.Round((double)value.Y));
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00017638 File Offset: 0x00016638
		public override bool Equals(object obj)
		{
			if (!(obj is Point))
			{
				return false;
			}
			Point point = (Point)obj;
			return point.X == this.X && point.Y == this.Y;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00017676 File Offset: 0x00016676
		public override int GetHashCode()
		{
			return this.x ^ this.y;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00017685 File Offset: 0x00016685
		public void Offset(int dx, int dy)
		{
			this.X += dx;
			this.Y += dy;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x000176A3 File Offset: 0x000166A3
		public void Offset(Point p)
		{
			this.Offset(p.X, p.Y);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x000176BC File Offset: 0x000166BC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{X=",
				this.X.ToString(CultureInfo.CurrentCulture),
				",Y=",
				this.Y.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001771A File Offset: 0x0001671A
		private static int HIWORD(int n)
		{
			return (n >> 16) & 65535;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00017726 File Offset: 0x00016726
		private static int LOWORD(int n)
		{
			return n & 65535;
		}

		// Token: 0x0400045C RID: 1116
		public static readonly Point Empty = default(Point);

		// Token: 0x0400045D RID: 1117
		private int x;

		// Token: 0x0400045E RID: 1118
		private int y;
	}
}
