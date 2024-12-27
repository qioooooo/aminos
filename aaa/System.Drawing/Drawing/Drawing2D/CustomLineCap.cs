using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	// Token: 0x0200006C RID: 108
	public class CustomLineCap : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x060006F8 RID: 1784 RVA: 0x0001B298 File Offset: 0x0001A298
		internal CustomLineCap()
		{
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001B2A0 File Offset: 0x0001A2A0
		public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath)
			: this(fillPath, strokePath, LineCap.Flat)
		{
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001B2AB File Offset: 0x0001A2AB
		public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath, LineCap baseCap)
			: this(fillPath, strokePath, baseCap, 0f)
		{
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001B2BC File Offset: 0x0001A2BC
		public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath, LineCap baseCap, float baseInset)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateCustomLineCap(new HandleRef(fillPath, (fillPath == null) ? IntPtr.Zero : fillPath.nativePath), new HandleRef(strokePath, (strokePath == null) ? IntPtr.Zero : strokePath.nativePath), baseCap, baseInset, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativeLineCap(zero);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001B31D File Offset: 0x0001A31D
		internal CustomLineCap(IntPtr nativeLineCap)
		{
			this.SetNativeLineCap(nativeLineCap);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001B32C File Offset: 0x0001A32C
		internal void SetNativeLineCap(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			this.nativeCap = new SafeCustomLineCapHandle(handle);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001B352 File Offset: 0x0001A352
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001B361 File Offset: 0x0001A361
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing && this.nativeCap != null)
			{
				this.nativeCap.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001B38C File Offset: 0x0001A38C
		~CustomLineCap()
		{
			this.Dispose(false);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001B3BC File Offset: 0x0001A3BC
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneCustomLineCap(new HandleRef(this, this.nativeCap), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return CustomLineCap.CreateCustomLineCapObject(zero);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001B3F8 File Offset: 0x0001A3F8
		internal static CustomLineCap CreateCustomLineCapObject(IntPtr cap)
		{
			CustomLineCapType customLineCapType = CustomLineCapType.Default;
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapType(new HandleRef(null, cap), out customLineCapType);
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDeleteCustomLineCap(new HandleRef(null, cap));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			switch (customLineCapType)
			{
			case CustomLineCapType.Default:
				return new CustomLineCap(cap);
			case CustomLineCapType.AdjustableArrowCap:
				return new AdjustableArrowCap(cap);
			default:
				SafeNativeMethods.Gdip.GdipDeleteCustomLineCap(new HandleRef(null, cap));
				throw SafeNativeMethods.Gdip.StatusException(6);
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001B460 File Offset: 0x0001A460
		public void SetStrokeCaps(LineCap startCap, LineCap endCap)
		{
			int num = SafeNativeMethods.Gdip.GdipSetCustomLineCapStrokeCaps(new HandleRef(this, this.nativeCap), startCap, endCap);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001B490 File Offset: 0x0001A490
		public void GetStrokeCaps(out LineCap startCap, out LineCap endCap)
		{
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapStrokeCaps(new HandleRef(this, this.nativeCap), out startCap, out endCap);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001B4C0 File Offset: 0x0001A4C0
		private void _SetStrokeJoin(LineJoin lineJoin)
		{
			int num = SafeNativeMethods.Gdip.GdipSetCustomLineCapStrokeJoin(new HandleRef(this, this.nativeCap), lineJoin);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001B4F0 File Offset: 0x0001A4F0
		private LineJoin _GetStrokeJoin()
		{
			LineJoin lineJoin;
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapStrokeJoin(new HandleRef(this, this.nativeCap), out lineJoin);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return lineJoin;
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001B521 File Offset: 0x0001A521
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0001B529 File Offset: 0x0001A529
		public LineJoin StrokeJoin
		{
			get
			{
				return this._GetStrokeJoin();
			}
			set
			{
				this._SetStrokeJoin(value);
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001B534 File Offset: 0x0001A534
		private void _SetBaseCap(LineCap baseCap)
		{
			int num = SafeNativeMethods.Gdip.GdipSetCustomLineCapBaseCap(new HandleRef(this, this.nativeCap), baseCap);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001B564 File Offset: 0x0001A564
		private LineCap _GetBaseCap()
		{
			LineCap lineCap;
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapBaseCap(new HandleRef(this, this.nativeCap), out lineCap);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return lineCap;
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0001B595 File Offset: 0x0001A595
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0001B59D File Offset: 0x0001A59D
		public LineCap BaseCap
		{
			get
			{
				return this._GetBaseCap();
			}
			set
			{
				this._SetBaseCap(value);
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001B5A8 File Offset: 0x0001A5A8
		private void _SetBaseInset(float inset)
		{
			int num = SafeNativeMethods.Gdip.GdipSetCustomLineCapBaseInset(new HandleRef(this, this.nativeCap), inset);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001B5D8 File Offset: 0x0001A5D8
		private float _GetBaseInset()
		{
			float num2;
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapBaseInset(new HandleRef(this, this.nativeCap), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2;
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001B609 File Offset: 0x0001A609
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0001B611 File Offset: 0x0001A611
		public float BaseInset
		{
			get
			{
				return this._GetBaseInset();
			}
			set
			{
				this._SetBaseInset(value);
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001B61C File Offset: 0x0001A61C
		private void _SetWidthScale(float widthScale)
		{
			int num = SafeNativeMethods.Gdip.GdipSetCustomLineCapWidthScale(new HandleRef(this, this.nativeCap), widthScale);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001B64C File Offset: 0x0001A64C
		private float _GetWidthScale()
		{
			float num2;
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapWidthScale(new HandleRef(this, this.nativeCap), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001B67D File Offset: 0x0001A67D
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0001B685 File Offset: 0x0001A685
		public float WidthScale
		{
			get
			{
				return this._GetWidthScale();
			}
			set
			{
				this._SetWidthScale(value);
			}
		}

		// Token: 0x04000494 RID: 1172
		internal SafeCustomLineCapHandle nativeCap;

		// Token: 0x04000495 RID: 1173
		private bool disposed;
	}
}
