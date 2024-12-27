using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x020000DB RID: 219
	[ComVisible(true)]
	[Serializable]
	public struct PointF
	{
		// Token: 0x06000CB4 RID: 3252 RVA: 0x00026261 File Offset: 0x00025261
		public PointF(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00026271 File Offset: 0x00025271
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.x == 0f && this.y == 0f;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0002628F File Offset: 0x0002528F
		// (set) Token: 0x06000CB7 RID: 3255 RVA: 0x00026297 File Offset: 0x00025297
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

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x000262A0 File Offset: 0x000252A0
		// (set) Token: 0x06000CB9 RID: 3257 RVA: 0x000262A8 File Offset: 0x000252A8
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

		// Token: 0x06000CBA RID: 3258 RVA: 0x000262B1 File Offset: 0x000252B1
		public static PointF operator +(PointF pt, Size sz)
		{
			return PointF.Add(pt, sz);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x000262BA File Offset: 0x000252BA
		public static PointF operator -(PointF pt, Size sz)
		{
			return PointF.Subtract(pt, sz);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x000262C3 File Offset: 0x000252C3
		public static PointF operator +(PointF pt, SizeF sz)
		{
			return PointF.Add(pt, sz);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x000262CC File Offset: 0x000252CC
		public static PointF operator -(PointF pt, SizeF sz)
		{
			return PointF.Subtract(pt, sz);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x000262D5 File Offset: 0x000252D5
		public static bool operator ==(PointF left, PointF right)
		{
			return left.X == right.X && left.Y == right.Y;
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x000262F9 File Offset: 0x000252F9
		public static bool operator !=(PointF left, PointF right)
		{
			return !(left == right);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00026305 File Offset: 0x00025305
		public static PointF Add(PointF pt, Size sz)
		{
			return new PointF(pt.X + (float)sz.Width, pt.Y + (float)sz.Height);
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0002632C File Offset: 0x0002532C
		public static PointF Subtract(PointF pt, Size sz)
		{
			return new PointF(pt.X - (float)sz.Width, pt.Y - (float)sz.Height);
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00026353 File Offset: 0x00025353
		public static PointF Add(PointF pt, SizeF sz)
		{
			return new PointF(pt.X + sz.Width, pt.Y + sz.Height);
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00026378 File Offset: 0x00025378
		public static PointF Subtract(PointF pt, SizeF sz)
		{
			return new PointF(pt.X - sz.Width, pt.Y - sz.Height);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x000263A0 File Offset: 0x000253A0
		public override bool Equals(object obj)
		{
			if (!(obj is PointF))
			{
				return false;
			}
			PointF pointF = (PointF)obj;
			return pointF.X == this.X && pointF.Y == this.Y && pointF.GetType().Equals(base.GetType());
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x000263FE File Offset: 0x000253FE
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00026410 File Offset: 0x00025410
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{X={0}, Y={1}}}", new object[] { this.x, this.y });
		}

		// Token: 0x04000AFC RID: 2812
		public static readonly PointF Empty = default(PointF);

		// Token: 0x04000AFD RID: 2813
		private float x;

		// Token: 0x04000AFE RID: 2814
		private float y;
	}
}
