using System;
using System.Drawing.Drawing2D;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x0200005E RID: 94
	public sealed class Region : MarshalByRefObject, IDisposable
	{
		// Token: 0x060005D0 RID: 1488 RVA: 0x0001872C File Offset: 0x0001772C
		public Region()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateRegion(out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativeRegion(zero);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00018760 File Offset: 0x00017760
		public Region(RectangleF rect)
		{
			IntPtr zero = IntPtr.Zero;
			GPRECTF gprectf = rect.ToGPRECTF();
			int num = SafeNativeMethods.Gdip.GdipCreateRegionRect(ref gprectf, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativeRegion(zero);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001879C File Offset: 0x0001779C
		public Region(Rectangle rect)
		{
			IntPtr zero = IntPtr.Zero;
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipCreateRegionRectI(ref gprect, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativeRegion(zero);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000187E0 File Offset: 0x000177E0
		public Region(GraphicsPath path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateRegionPath(new HandleRef(path, path.nativePath), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativeRegion(zero);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001882C File Offset: 0x0001782C
		public Region(RegionData rgnData)
		{
			if (rgnData == null)
			{
				throw new ArgumentNullException("rgnData");
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateRegionRgnData(rgnData.Data, rgnData.Data.Length, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativeRegion(zero);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001887A File Offset: 0x0001787A
		internal Region(IntPtr nativeRegion)
		{
			this.SetNativeRegion(nativeRegion);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001888C File Offset: 0x0001788C
		public static Region FromHrgn(IntPtr hrgn)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateRegionHrgn(new HandleRef(null, hrgn), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Region(zero);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000188C8 File Offset: 0x000178C8
		private void SetNativeRegion(IntPtr nativeRegion)
		{
			if (nativeRegion == IntPtr.Zero)
			{
				throw new ArgumentNullException("nativeRegion");
			}
			this.nativeRegion = nativeRegion;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000188EC File Offset: 0x000178EC
		public Region Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneRegion(new HandleRef(this, this.nativeRegion), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Region(zero);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00018923 File Offset: 0x00017923
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00018934 File Offset: 0x00017934
		private void Dispose(bool disposing)
		{
			if (this.nativeRegion != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeleteRegion(new HandleRef(this, this.nativeRegion));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.nativeRegion = IntPtr.Zero;
				}
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x000189A0 File Offset: 0x000179A0
		~Region()
		{
			this.Dispose(false);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000189D0 File Offset: 0x000179D0
		public void MakeInfinite()
		{
			int num = SafeNativeMethods.Gdip.GdipSetInfinite(new HandleRef(this, this.nativeRegion));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000189FC File Offset: 0x000179FC
		public void MakeEmpty()
		{
			int num = SafeNativeMethods.Gdip.GdipSetEmpty(new HandleRef(this, this.nativeRegion));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00018A28 File Offset: 0x00017A28
		public void Intersect(RectangleF rect)
		{
			GPRECTF gprectf = rect.ToGPRECTF();
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRect(new HandleRef(this, this.nativeRegion), ref gprectf, CombineMode.Intersect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00018A5C File Offset: 0x00017A5C
		public void Intersect(Rectangle rect)
		{
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRectI(new HandleRef(this, this.nativeRegion), ref gprect, CombineMode.Intersect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00018A98 File Offset: 0x00017A98
		public void Intersect(GraphicsPath path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int num = SafeNativeMethods.Gdip.GdipCombineRegionPath(new HandleRef(this, this.nativeRegion), new HandleRef(path, path.nativePath), CombineMode.Intersect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00018ADC File Offset: 0x00017ADC
		public void Intersect(Region region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRegion(new HandleRef(this, this.nativeRegion), new HandleRef(region, region.nativeRegion), CombineMode.Intersect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00018B20 File Offset: 0x00017B20
		public void ReleaseHrgn(IntPtr regionHandle)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			if (regionHandle == IntPtr.Zero)
			{
				throw new ArgumentNullException("regionHandle");
			}
			SafeNativeMethods.IntDeleteObject(new HandleRef(this, regionHandle));
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00018B54 File Offset: 0x00017B54
		public void Union(RectangleF rect)
		{
			GPRECTF gprectf = new GPRECTF(rect);
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRect(new HandleRef(this, this.nativeRegion), ref gprectf, CombineMode.Union);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00018B90 File Offset: 0x00017B90
		public void Union(Rectangle rect)
		{
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRectI(new HandleRef(this, this.nativeRegion), ref gprect, CombineMode.Union);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00018BCC File Offset: 0x00017BCC
		public void Union(GraphicsPath path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int num = SafeNativeMethods.Gdip.GdipCombineRegionPath(new HandleRef(this, this.nativeRegion), new HandleRef(path, path.nativePath), CombineMode.Union);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00018C10 File Offset: 0x00017C10
		public void Union(Region region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRegion(new HandleRef(this, this.nativeRegion), new HandleRef(region, region.nativeRegion), CombineMode.Union);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00018C54 File Offset: 0x00017C54
		public void Xor(RectangleF rect)
		{
			GPRECTF gprectf = new GPRECTF(rect);
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRect(new HandleRef(this, this.nativeRegion), ref gprectf, CombineMode.Xor);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00018C90 File Offset: 0x00017C90
		public void Xor(Rectangle rect)
		{
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRectI(new HandleRef(this, this.nativeRegion), ref gprect, CombineMode.Xor);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00018CCC File Offset: 0x00017CCC
		public void Xor(GraphicsPath path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int num = SafeNativeMethods.Gdip.GdipCombineRegionPath(new HandleRef(this, this.nativeRegion), new HandleRef(path, path.nativePath), CombineMode.Xor);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00018D10 File Offset: 0x00017D10
		public void Xor(Region region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRegion(new HandleRef(this, this.nativeRegion), new HandleRef(region, region.nativeRegion), CombineMode.Xor);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00018D54 File Offset: 0x00017D54
		public void Exclude(RectangleF rect)
		{
			GPRECTF gprectf = new GPRECTF(rect);
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRect(new HandleRef(this, this.nativeRegion), ref gprectf, CombineMode.Exclude);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00018D90 File Offset: 0x00017D90
		public void Exclude(Rectangle rect)
		{
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRectI(new HandleRef(this, this.nativeRegion), ref gprect, CombineMode.Exclude);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00018DCC File Offset: 0x00017DCC
		public void Exclude(GraphicsPath path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int num = SafeNativeMethods.Gdip.GdipCombineRegionPath(new HandleRef(this, this.nativeRegion), new HandleRef(path, path.nativePath), CombineMode.Exclude);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00018E10 File Offset: 0x00017E10
		public void Exclude(Region region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRegion(new HandleRef(this, this.nativeRegion), new HandleRef(region, region.nativeRegion), CombineMode.Exclude);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00018E54 File Offset: 0x00017E54
		public void Complement(RectangleF rect)
		{
			GPRECTF gprectf = rect.ToGPRECTF();
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRect(new HandleRef(this, this.nativeRegion), ref gprectf, CombineMode.Complement);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00018E88 File Offset: 0x00017E88
		public void Complement(Rectangle rect)
		{
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRectI(new HandleRef(this, this.nativeRegion), ref gprect, CombineMode.Complement);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00018EC4 File Offset: 0x00017EC4
		public void Complement(GraphicsPath path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int num = SafeNativeMethods.Gdip.GdipCombineRegionPath(new HandleRef(this, this.nativeRegion), new HandleRef(path, path.nativePath), CombineMode.Complement);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00018F08 File Offset: 0x00017F08
		public void Complement(Region region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipCombineRegionRegion(new HandleRef(this, this.nativeRegion), new HandleRef(region, region.nativeRegion), CombineMode.Complement);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00018F4C File Offset: 0x00017F4C
		public void Translate(float dx, float dy)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateRegion(new HandleRef(this, this.nativeRegion), dx, dy);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00018F78 File Offset: 0x00017F78
		public void Translate(int dx, int dy)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateRegionI(new HandleRef(this, this.nativeRegion), dx, dy);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00018FA4 File Offset: 0x00017FA4
		public void Transform(Matrix matrix)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipTransformRegion(new HandleRef(this, this.nativeRegion), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00018FE8 File Offset: 0x00017FE8
		public RectangleF GetBounds(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			GPRECTF gprectf = default(GPRECTF);
			int num = SafeNativeMethods.Gdip.GdipGetRegionBounds(new HandleRef(this, this.nativeRegion), new HandleRef(g, g.NativeGraphics), ref gprectf);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf.ToRectangleF();
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001903C File Offset: 0x0001803C
		public IntPtr GetHrgn(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetRegionHRgn(new HandleRef(this, this.nativeRegion), new HandleRef(g, g.NativeGraphics), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00019088 File Offset: 0x00018088
		public bool IsEmpty(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsEmptyRegion(new HandleRef(this, this.nativeRegion), new HandleRef(g, g.NativeGraphics), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000190D4 File Offset: 0x000180D4
		public bool IsInfinite(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsInfiniteRegion(new HandleRef(this, this.nativeRegion), new HandleRef(g, g.NativeGraphics), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00019120 File Offset: 0x00018120
		public bool Equals(Region region, Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsEqualRegion(new HandleRef(this, this.nativeRegion), new HandleRef(region, region.nativeRegion), new HandleRef(g, g.NativeGraphics), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00019188 File Offset: 0x00018188
		public RegionData GetRegionData()
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetRegionDataSize(new HandleRef(this, this.nativeRegion), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			if (num == 0)
			{
				return null;
			}
			byte[] array = new byte[num];
			num2 = SafeNativeMethods.Gdip.GdipGetRegionData(new HandleRef(this, this.nativeRegion), array, num, out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return new RegionData(array);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000191E7 File Offset: 0x000181E7
		public bool IsVisible(float x, float y)
		{
			return this.IsVisible(new PointF(x, y), null);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000191F7 File Offset: 0x000181F7
		public bool IsVisible(PointF point)
		{
			return this.IsVisible(point, null);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00019201 File Offset: 0x00018201
		public bool IsVisible(float x, float y, Graphics g)
		{
			return this.IsVisible(new PointF(x, y), g);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00019214 File Offset: 0x00018214
		public bool IsVisible(PointF point, Graphics g)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisibleRegionPoint(new HandleRef(this, this.nativeRegion), point.X, point.Y, new HandleRef(g, (g == null) ? IntPtr.Zero : g.NativeGraphics), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001926A File Offset: 0x0001826A
		public bool IsVisible(float x, float y, float width, float height)
		{
			return this.IsVisible(new RectangleF(x, y, width, height), null);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001927D File Offset: 0x0001827D
		public bool IsVisible(RectangleF rect)
		{
			return this.IsVisible(rect, null);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00019287 File Offset: 0x00018287
		public bool IsVisible(float x, float y, float width, float height, Graphics g)
		{
			return this.IsVisible(new RectangleF(x, y, width, height), g);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001929C File Offset: 0x0001829C
		public bool IsVisible(RectangleF rect, Graphics g)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipIsVisibleRegionRect(new HandleRef(this, this.nativeRegion), rect.X, rect.Y, rect.Width, rect.Height, new HandleRef(g, (g == null) ? IntPtr.Zero : g.NativeGraphics), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num != 0;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00019302 File Offset: 0x00018302
		public bool IsVisible(int x, int y, Graphics g)
		{
			return this.IsVisible(new Point(x, y), g);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00019312 File Offset: 0x00018312
		public bool IsVisible(Point point)
		{
			return this.IsVisible(point, null);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001931C File Offset: 0x0001831C
		public bool IsVisible(Point point, Graphics g)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipIsVisibleRegionPointI(new HandleRef(this, this.nativeRegion), point.X, point.Y, new HandleRef(g, (g == null) ? IntPtr.Zero : g.NativeGraphics), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num != 0;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00019374 File Offset: 0x00018374
		public bool IsVisible(int x, int y, int width, int height)
		{
			return this.IsVisible(new Rectangle(x, y, width, height), null);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00019387 File Offset: 0x00018387
		public bool IsVisible(Rectangle rect)
		{
			return this.IsVisible(rect, null);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00019391 File Offset: 0x00018391
		public bool IsVisible(int x, int y, int width, int height, Graphics g)
		{
			return this.IsVisible(new Rectangle(x, y, width, height), g);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x000193A8 File Offset: 0x000183A8
		public bool IsVisible(Rectangle rect, Graphics g)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipIsVisibleRegionRectI(new HandleRef(this, this.nativeRegion), rect.X, rect.Y, rect.Width, rect.Height, new HandleRef(g, (g == null) ? IntPtr.Zero : g.NativeGraphics), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num != 0;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00019410 File Offset: 0x00018410
		public RectangleF[] GetRegionScans(Matrix matrix)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetRegionScansCount(new HandleRef(this, this.nativeRegion), out num, new HandleRef(matrix, matrix.nativeMatrix));
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			int num3 = Marshal.SizeOf(typeof(GPRECTF));
			IntPtr intPtr = Marshal.AllocHGlobal(checked(num3 * num));
			RectangleF[] array;
			try
			{
				num2 = SafeNativeMethods.Gdip.GdipGetRegionScans(new HandleRef(this, this.nativeRegion), intPtr, out num, new HandleRef(matrix, matrix.nativeMatrix));
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				GPRECTF gprectf = default(GPRECTF);
				array = new RectangleF[num];
				for (int i = 0; i < num; i++)
				{
					checked
					{
						array[i] = ((GPRECTF)UnsafeNativeMethods.PtrToStructure((IntPtr)((long)intPtr + unchecked((long)(checked(num3 * i)))), typeof(GPRECTF))).ToRectangleF();
					}
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return array;
		}

		// Token: 0x04000468 RID: 1128
		internal IntPtr nativeRegion;
	}
}
