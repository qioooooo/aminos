using System;
using System.ComponentModel;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000B5 RID: 181
	public sealed class GraphicsPath : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x06000AF2 RID: 2802 RVA: 0x0002065A File Offset: 0x0001F65A
		public GraphicsPath()
			: this(FillMode.Alternate)
		{
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00020664 File Offset: 0x0001F664
		public GraphicsPath(FillMode fillMode)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreatePath((int)fillMode, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativePath = zero;
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00020697 File Offset: 0x0001F697
		public GraphicsPath(PointF[] pts, byte[] types)
			: this(pts, types, FillMode.Alternate)
		{
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x000206A4 File Offset: 0x0001F6A4
		public GraphicsPath(PointF[] pts, byte[] types, FillMode fillMode)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr zero = IntPtr.Zero;
			if (pts.Length != types.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			int num = types.Length;
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			IntPtr intPtr2 = Marshal.AllocHGlobal(num);
			try
			{
				Marshal.Copy(types, 0, intPtr2, num);
				int num2 = SafeNativeMethods.Gdip.GdipCreatePath2(new HandleRef(null, intPtr), new HandleRef(null, intPtr2), num, (int)fillMode, out zero);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
			this.nativePath = zero;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00020740 File Offset: 0x0001F740
		public GraphicsPath(Point[] pts, byte[] types)
			: this(pts, types, FillMode.Alternate)
		{
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002074C File Offset: 0x0001F74C
		public GraphicsPath(Point[] pts, byte[] types, FillMode fillMode)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr zero = IntPtr.Zero;
			if (pts.Length != types.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			int num = types.Length;
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			IntPtr intPtr2 = Marshal.AllocHGlobal(num);
			try
			{
				Marshal.Copy(types, 0, intPtr2, num);
				int num2 = SafeNativeMethods.Gdip.GdipCreatePath2I(new HandleRef(null, intPtr), new HandleRef(null, intPtr2), num, (int)fillMode, out zero);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
			this.nativePath = zero;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x000207E8 File Offset: 0x0001F7E8
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipClonePath(new HandleRef(this, this.nativePath), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new GraphicsPath(zero, 0);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00020820 File Offset: 0x0001F820
		private GraphicsPath(IntPtr nativePath, int extra)
		{
			if (nativePath == IntPtr.Zero)
			{
				throw new ArgumentNullException("nativePath");
			}
			this.nativePath = nativePath;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00020847 File Offset: 0x0001F847
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00020858 File Offset: 0x0001F858
		private void Dispose(bool disposing)
		{
			if (this.nativePath != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeletePath(new HandleRef(this, this.nativePath));
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
					this.nativePath = IntPtr.Zero;
				}
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x000208C4 File Offset: 0x0001F8C4
		~GraphicsPath()
		{
			this.Dispose(false);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x000208F4 File Offset: 0x0001F8F4
		public void Reset()
		{
			int num = SafeNativeMethods.Gdip.GdipResetPath(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x00020920 File Offset: 0x0001F920
		// (set) Token: 0x06000AFF RID: 2815 RVA: 0x00020950 File Offset: 0x0001F950
		public FillMode FillMode
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetPathFillMode(new HandleRef(this, this.nativePath), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (FillMode)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FillMode));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPathFillMode(new HandleRef(this, this.nativePath), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000209A0 File Offset: 0x0001F9A0
		private PathData _GetPathData()
		{
			int num = Marshal.SizeOf(typeof(GPPOINTF));
			int pointCount = this.PointCount;
			PathData pathData = new PathData();
			pathData.Types = new byte[pointCount];
			IntPtr intPtr = Marshal.AllocHGlobal(3 * IntPtr.Size);
			IntPtr intPtr2 = Marshal.AllocHGlobal(checked(num * pointCount));
			try
			{
				GCHandle gchandle = GCHandle.Alloc(pathData.Types, GCHandleType.Pinned);
				try
				{
					IntPtr intPtr3 = gchandle.AddrOfPinnedObject();
					Marshal.StructureToPtr(pointCount, intPtr, false);
					Marshal.StructureToPtr(intPtr2, (IntPtr)((long)intPtr + (long)IntPtr.Size), false);
					Marshal.StructureToPtr(intPtr3, (IntPtr)((long)intPtr + (long)(2 * IntPtr.Size)), false);
					int num2 = SafeNativeMethods.Gdip.GdipGetPathData(new HandleRef(this, this.nativePath), intPtr);
					if (num2 != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num2);
					}
					pathData.Points = SafeNativeMethods.Gdip.ConvertGPPOINTFArrayF(intPtr2, pointCount);
				}
				finally
				{
					gchandle.Free();
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
			return pathData;
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00020AB8 File Offset: 0x0001FAB8
		public PathData PathData
		{
			get
			{
				return this._GetPathData();
			}
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00020AC0 File Offset: 0x0001FAC0
		public void StartFigure()
		{
			int num = SafeNativeMethods.Gdip.GdipStartPathFigure(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00020AEC File Offset: 0x0001FAEC
		public void CloseFigure()
		{
			int num = SafeNativeMethods.Gdip.GdipClosePathFigure(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00020B18 File Offset: 0x0001FB18
		public void CloseAllFigures()
		{
			int num = SafeNativeMethods.Gdip.GdipClosePathFigures(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00020B44 File Offset: 0x0001FB44
		public void SetMarkers()
		{
			int num = SafeNativeMethods.Gdip.GdipSetPathMarker(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00020B70 File Offset: 0x0001FB70
		public void ClearMarkers()
		{
			int num = SafeNativeMethods.Gdip.GdipClearPathMarkers(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00020B9C File Offset: 0x0001FB9C
		public void Reverse()
		{
			int num = SafeNativeMethods.Gdip.GdipReversePath(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00020BC8 File Offset: 0x0001FBC8
		public PointF GetLastPoint()
		{
			GPPOINTF gppointf = new GPPOINTF();
			int num = SafeNativeMethods.Gdip.GdipGetPathLastPoint(new HandleRef(this, this.nativePath), gppointf);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gppointf.ToPoint();
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00020BFE File Offset: 0x0001FBFE
		public bool IsVisible(float x, float y)
		{
			return this.IsVisible(new PointF(x, y), null);
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00020C0E File Offset: 0x0001FC0E
		public bool IsVisible(PointF point)
		{
			return this.IsVisible(point, null);
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00020C18 File Offset: 0x0001FC18
		public bool IsVisible(float x, float y, Graphics graphics)
		{
			return this.IsVisible(new PointF(x, y), graphics);
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00020C28 File Offset: 0x0001FC28
		public bool IsVisible(PointF pt, Graphics graphics)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisiblePathPoint(new HandleRef(this, this.nativePath), pt.X, pt.Y, new HandleRef(graphics, (graphics != null) ? graphics.NativeGraphics : IntPtr.Zero), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00020C7E File Offset: 0x0001FC7E
		public bool IsVisible(int x, int y)
		{
			return this.IsVisible(new Point(x, y), null);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00020C8E File Offset: 0x0001FC8E
		public bool IsVisible(Point point)
		{
			return this.IsVisible(point, null);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00020C98 File Offset: 0x0001FC98
		public bool IsVisible(int x, int y, Graphics graphics)
		{
			return this.IsVisible(new Point(x, y), graphics);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00020CA8 File Offset: 0x0001FCA8
		public bool IsVisible(Point pt, Graphics graphics)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisiblePathPointI(new HandleRef(this, this.nativePath), pt.X, pt.Y, new HandleRef(graphics, (graphics != null) ? graphics.NativeGraphics : IntPtr.Zero), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00020CFE File Offset: 0x0001FCFE
		public bool IsOutlineVisible(float x, float y, Pen pen)
		{
			return this.IsOutlineVisible(new PointF(x, y), pen, null);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00020D0F File Offset: 0x0001FD0F
		public bool IsOutlineVisible(PointF point, Pen pen)
		{
			return this.IsOutlineVisible(point, pen, null);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00020D1A File Offset: 0x0001FD1A
		public bool IsOutlineVisible(float x, float y, Pen pen, Graphics graphics)
		{
			return this.IsOutlineVisible(new PointF(x, y), pen, graphics);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00020D2C File Offset: 0x0001FD2C
		public bool IsOutlineVisible(PointF pt, Pen pen, Graphics graphics)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsOutlineVisiblePathPoint(new HandleRef(this, this.nativePath), pt.X, pt.Y, new HandleRef(pen, pen.NativePen), new HandleRef(graphics, (graphics != null) ? graphics.NativeGraphics : IntPtr.Zero), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00020D9C File Offset: 0x0001FD9C
		public bool IsOutlineVisible(int x, int y, Pen pen)
		{
			return this.IsOutlineVisible(new Point(x, y), pen, null);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00020DAD File Offset: 0x0001FDAD
		public bool IsOutlineVisible(Point point, Pen pen)
		{
			return this.IsOutlineVisible(point, pen, null);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00020DB8 File Offset: 0x0001FDB8
		public bool IsOutlineVisible(int x, int y, Pen pen, Graphics graphics)
		{
			return this.IsOutlineVisible(new Point(x, y), pen, graphics);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00020DCC File Offset: 0x0001FDCC
		public bool IsOutlineVisible(Point pt, Pen pen, Graphics graphics)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsOutlineVisiblePathPointI(new HandleRef(this, this.nativePath), pt.X, pt.Y, new HandleRef(pen, pen.NativePen), new HandleRef(graphics, (graphics != null) ? graphics.NativeGraphics : IntPtr.Zero), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00020E3C File Offset: 0x0001FE3C
		public void AddLine(PointF pt1, PointF pt2)
		{
			this.AddLine(pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00020E60 File Offset: 0x0001FE60
		public void AddLine(float x1, float y1, float x2, float y2)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathLine(new HandleRef(this, this.nativePath), x1, y1, x2, y2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00020E90 File Offset: 0x0001FE90
		public void AddLines(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathLine2(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00020EF4 File Offset: 0x0001FEF4
		public void AddLine(Point pt1, Point pt2)
		{
			this.AddLine(pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00020F18 File Offset: 0x0001FF18
		public void AddLine(int x1, int y1, int x2, int y2)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathLineI(new HandleRef(this, this.nativePath), x1, y1, x2, y2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00020F48 File Offset: 0x0001FF48
		public void AddLines(Point[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathLine2I(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00020FAC File Offset: 0x0001FFAC
		public void AddArc(RectangleF rect, float startAngle, float sweepAngle)
		{
			this.AddArc(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00020FD4 File Offset: 0x0001FFD4
		public void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathArc(new HandleRef(this, this.nativePath), x, y, width, height, startAngle, sweepAngle);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00021006 File Offset: 0x00020006
		public void AddArc(Rectangle rect, float startAngle, float sweepAngle)
		{
			this.AddArc(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0002102C File Offset: 0x0002002C
		public void AddArc(int x, int y, int width, int height, float startAngle, float sweepAngle)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathArcI(new HandleRef(this, this.nativePath), x, y, width, height, startAngle, sweepAngle);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00021060 File Offset: 0x00020060
		public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
		{
			this.AddBezier(pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x000210AC File Offset: 0x000200AC
		public void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathBezier(new HandleRef(this, this.nativePath), x1, y1, x2, y2, x3, y3, x4, y4);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x000210E4 File Offset: 0x000200E4
		public void AddBeziers(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathBeziers(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00021148 File Offset: 0x00020148
		public void AddBezier(Point pt1, Point pt2, Point pt3, Point pt4)
		{
			this.AddBezier(pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00021194 File Offset: 0x00020194
		public void AddBezier(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathBezierI(new HandleRef(this, this.nativePath), x1, y1, x2, y2, x3, y3, x4, y4);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000211CC File Offset: 0x000201CC
		public void AddBeziers(params Point[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathBeziersI(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00021230 File Offset: 0x00020230
		public void AddCurve(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurve(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00021294 File Offset: 0x00020294
		public void AddCurve(PointF[] points, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurve2(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x000212F8 File Offset: 0x000202F8
		public void AddCurve(PointF[] points, int offset, int numberOfSegments, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurve3(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, offset, numberOfSegments, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00021360 File Offset: 0x00020360
		public void AddCurve(Point[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurveI(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x000213C4 File Offset: 0x000203C4
		public void AddCurve(Point[] points, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurve2I(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00021428 File Offset: 0x00020428
		public void AddCurve(Point[] points, int offset, int numberOfSegments, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurve3I(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, offset, numberOfSegments, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00021490 File Offset: 0x00020490
		public void AddClosedCurve(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathClosedCurve(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x000214F4 File Offset: 0x000204F4
		public void AddClosedCurve(PointF[] points, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathClosedCurve2(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00021558 File Offset: 0x00020558
		public void AddClosedCurve(Point[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathClosedCurveI(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x000215BC File Offset: 0x000205BC
		public void AddClosedCurve(Point[] points, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathClosedCurve2I(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00021620 File Offset: 0x00020620
		public void AddRectangle(RectangleF rect)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathRectangle(new HandleRef(this, this.nativePath), rect.X, rect.Y, rect.Width, rect.Height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00021668 File Offset: 0x00020668
		public void AddRectangles(RectangleF[] rects)
		{
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathRectangles(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), rects.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x000216CC File Offset: 0x000206CC
		public void AddRectangle(Rectangle rect)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathRectangleI(new HandleRef(this, this.nativePath), rect.X, rect.Y, rect.Width, rect.Height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00021714 File Offset: 0x00020714
		public void AddRectangles(Rectangle[] rects)
		{
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathRectanglesI(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), rects.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00021778 File Offset: 0x00020778
		public void AddEllipse(RectangleF rect)
		{
			this.AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0002179C File Offset: 0x0002079C
		public void AddEllipse(float x, float y, float width, float height)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathEllipse(new HandleRef(this, this.nativePath), x, y, width, height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x000217CA File Offset: 0x000207CA
		public void AddEllipse(Rectangle rect)
		{
			this.AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x000217F0 File Offset: 0x000207F0
		public void AddEllipse(int x, int y, int width, int height)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathEllipseI(new HandleRef(this, this.nativePath), x, y, width, height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0002181E File Offset: 0x0002081E
		public void AddPie(Rectangle rect, float startAngle, float sweepAngle)
		{
			this.AddPie(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00021844 File Offset: 0x00020844
		public void AddPie(float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathPie(new HandleRef(this, this.nativePath), x, y, width, height, startAngle, sweepAngle);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00021878 File Offset: 0x00020878
		public void AddPie(int x, int y, int width, int height, float startAngle, float sweepAngle)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathPieI(new HandleRef(this, this.nativePath), x, y, width, height, startAngle, sweepAngle);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x000218AC File Offset: 0x000208AC
		public void AddPolygon(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathPolygon(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00021910 File Offset: 0x00020910
		public void AddPolygon(Point[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathPolygonI(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00021974 File Offset: 0x00020974
		public void AddPath(GraphicsPath addingPath, bool connect)
		{
			if (addingPath == null)
			{
				throw new ArgumentNullException("addingPath");
			}
			int num = SafeNativeMethods.Gdip.GdipAddPathPath(new HandleRef(this, this.nativePath), new HandleRef(addingPath, addingPath.nativePath), connect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x000219B8 File Offset: 0x000209B8
		public void AddString(string s, FontFamily family, int style, float emSize, PointF origin, StringFormat format)
		{
			GPRECTF gprectf = new GPRECTF(origin.X, origin.Y, 0f, 0f);
			int num = SafeNativeMethods.Gdip.GdipAddPathString(new HandleRef(this, this.nativePath), s, s.Length, new HandleRef(family, (family != null) ? family.NativeFamily : IntPtr.Zero), style, emSize, ref gprectf, new HandleRef(format, (format != null) ? format.nativeFormat : IntPtr.Zero));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00021A40 File Offset: 0x00020A40
		public void AddString(string s, FontFamily family, int style, float emSize, Point origin, StringFormat format)
		{
			GPRECT gprect = new GPRECT(origin.X, origin.Y, 0, 0);
			int num = SafeNativeMethods.Gdip.GdipAddPathStringI(new HandleRef(this, this.nativePath), s, s.Length, new HandleRef(family, (family != null) ? family.NativeFamily : IntPtr.Zero), style, emSize, ref gprect, new HandleRef(format, (format != null) ? format.nativeFormat : IntPtr.Zero));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00021AC0 File Offset: 0x00020AC0
		public void AddString(string s, FontFamily family, int style, float emSize, RectangleF layoutRect, StringFormat format)
		{
			GPRECTF gprectf = new GPRECTF(layoutRect);
			int num = SafeNativeMethods.Gdip.GdipAddPathString(new HandleRef(this, this.nativePath), s, s.Length, new HandleRef(family, (family != null) ? family.NativeFamily : IntPtr.Zero), style, emSize, ref gprectf, new HandleRef(format, (format != null) ? format.nativeFormat : IntPtr.Zero));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00021B34 File Offset: 0x00020B34
		public void AddString(string s, FontFamily family, int style, float emSize, Rectangle layoutRect, StringFormat format)
		{
			GPRECT gprect = new GPRECT(layoutRect);
			int num = SafeNativeMethods.Gdip.GdipAddPathStringI(new HandleRef(this, this.nativePath), s, s.Length, new HandleRef(family, (family != null) ? family.NativeFamily : IntPtr.Zero), style, emSize, ref gprect, new HandleRef(format, (format != null) ? format.nativeFormat : IntPtr.Zero));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00021BA8 File Offset: 0x00020BA8
		public void Transform(Matrix matrix)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			if (matrix.nativeMatrix == IntPtr.Zero)
			{
				return;
			}
			int num = SafeNativeMethods.Gdip.GdipTransformPath(new HandleRef(this, this.nativePath), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00021BFE File Offset: 0x00020BFE
		public RectangleF GetBounds()
		{
			return this.GetBounds(null);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00021C07 File Offset: 0x00020C07
		public RectangleF GetBounds(Matrix matrix)
		{
			return this.GetBounds(matrix, null);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00021C14 File Offset: 0x00020C14
		public RectangleF GetBounds(Matrix matrix, Pen pen)
		{
			GPRECTF gprectf = default(GPRECTF);
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			if (matrix != null)
			{
				intPtr = matrix.nativeMatrix;
			}
			if (pen != null)
			{
				intPtr2 = pen.NativePen;
			}
			int num = SafeNativeMethods.Gdip.GdipGetPathWorldBounds(new HandleRef(this, this.nativePath), ref gprectf, new HandleRef(matrix, intPtr), new HandleRef(pen, intPtr2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf.ToRectangleF();
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00021C7C File Offset: 0x00020C7C
		public void Flatten()
		{
			this.Flatten(null);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00021C85 File Offset: 0x00020C85
		public void Flatten(Matrix matrix)
		{
			this.Flatten(matrix, 0.25f);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00021C94 File Offset: 0x00020C94
		public void Flatten(Matrix matrix, float flatness)
		{
			int num = SafeNativeMethods.Gdip.GdipFlattenPath(new HandleRef(this, this.nativePath), new HandleRef(matrix, (matrix == null) ? IntPtr.Zero : matrix.nativeMatrix), flatness);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00021CD4 File Offset: 0x00020CD4
		public void Widen(Pen pen)
		{
			float num = 0.6666667f;
			this.Widen(pen, null, num);
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00021CF0 File Offset: 0x00020CF0
		public void Widen(Pen pen, Matrix matrix)
		{
			float num = 0.6666667f;
			this.Widen(pen, matrix, num);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00021D0C File Offset: 0x00020D0C
		public void Widen(Pen pen, Matrix matrix, float flatness)
		{
			IntPtr intPtr;
			if (matrix == null)
			{
				intPtr = IntPtr.Zero;
			}
			else
			{
				intPtr = matrix.nativeMatrix;
			}
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num;
			SafeNativeMethods.Gdip.GdipGetPointCount(new HandleRef(this, this.nativePath), out num);
			if (num == 0)
			{
				return;
			}
			int num2 = SafeNativeMethods.Gdip.GdipWidenPath(new HandleRef(this, this.nativePath), new HandleRef(pen, pen.NativePen), new HandleRef(matrix, intPtr), flatness);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00021D81 File Offset: 0x00020D81
		public void Warp(PointF[] destPoints, RectangleF srcRect)
		{
			this.Warp(destPoints, srcRect, null);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00021D8C File Offset: 0x00020D8C
		public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix)
		{
			this.Warp(destPoints, srcRect, matrix, WarpMode.Perspective);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00021D98 File Offset: 0x00020D98
		public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix, WarpMode warpMode)
		{
			this.Warp(destPoints, srcRect, matrix, warpMode, 0.25f);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00021DAC File Offset: 0x00020DAC
		public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix, WarpMode warpMode, float flatness)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipWarpPath(new HandleRef(this, this.nativePath), new HandleRef(matrix, (matrix == null) ? IntPtr.Zero : matrix.nativeMatrix), new HandleRef(null, intPtr), destPoints.Length, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, warpMode, flatness);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00021E44 File Offset: 0x00020E44
		public int PointCount
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetPointCount(new HandleRef(this, this.nativePath), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return num;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00021E74 File Offset: 0x00020E74
		public byte[] PathTypes
		{
			get
			{
				int pointCount = this.PointCount;
				byte[] array = new byte[pointCount];
				int num = SafeNativeMethods.Gdip.GdipGetPathTypes(new HandleRef(this, this.nativePath), array, pointCount);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x00021EB0 File Offset: 0x00020EB0
		public PointF[] PathPoints
		{
			get
			{
				int pointCount = this.PointCount;
				int num = Marshal.SizeOf(typeof(GPPOINTF));
				IntPtr intPtr = Marshal.AllocHGlobal(checked(pointCount * num));
				PointF[] array2;
				try
				{
					int num2 = SafeNativeMethods.Gdip.GdipGetPathPoints(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), pointCount);
					if (num2 != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num2);
					}
					PointF[] array = SafeNativeMethods.Gdip.ConvertGPPOINTFArrayF(intPtr, pointCount);
					array2 = array;
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return array2;
			}
		}

		// Token: 0x040009E1 RID: 2529
		internal IntPtr nativePath;
	}
}
