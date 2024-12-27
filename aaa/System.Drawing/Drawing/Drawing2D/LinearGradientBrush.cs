using System;
using System.ComponentModel;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000C5 RID: 197
	public sealed class LinearGradientBrush : Brush
	{
		// Token: 0x06000BD3 RID: 3027 RVA: 0x000233E8 File Offset: 0x000223E8
		public LinearGradientBrush(PointF point1, PointF point2, Color color1, Color color2)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrush(new GPPOINTF(point1), new GPPOINTF(point2), color1.ToArgb(), color2.ToArgb(), 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00023438 File Offset: 0x00022438
		public LinearGradientBrush(Point point1, Point point2, Color color1, Color color2)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrushI(new GPPOINT(point1), new GPPOINT(point2), color1.ToArgb(), color2.ToArgb(), 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00023488 File Offset: 0x00022488
		public LinearGradientBrush(RectangleF rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
		{
			if (!ClientUtils.IsEnumValid(linearGradientMode, (int)linearGradientMode, 0, 3))
			{
				throw new InvalidEnumArgumentException("linearGradientMode", (int)linearGradientMode, typeof(LinearGradientMode));
			}
			if ((double)rect.Width == 0.0 || (double)rect.Height == 0.0)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[] { rect.ToString() }));
			}
			IntPtr zero = IntPtr.Zero;
			GPRECTF gprectf = new GPRECTF(rect);
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrushFromRect(ref gprectf, color1.ToArgb(), color2.ToArgb(), (int)linearGradientMode, 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00023554 File Offset: 0x00022554
		public LinearGradientBrush(Rectangle rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
		{
			if (!ClientUtils.IsEnumValid(linearGradientMode, (int)linearGradientMode, 0, 3))
			{
				throw new InvalidEnumArgumentException("linearGradientMode", (int)linearGradientMode, typeof(LinearGradientMode));
			}
			if (rect.Width == 0 || rect.Height == 0)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[] { rect.ToString() }));
			}
			IntPtr zero = IntPtr.Zero;
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrushFromRectI(ref gprect, color1.ToArgb(), color2.ToArgb(), (int)linearGradientMode, 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00023609 File Offset: 0x00022609
		public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle)
			: this(rect, color1, color2, angle, false)
		{
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00023618 File Offset: 0x00022618
		public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle, bool isAngleScaleable)
		{
			IntPtr zero = IntPtr.Zero;
			if ((double)rect.Width == 0.0 || (double)rect.Height == 0.0)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[] { rect.ToString() }));
			}
			GPRECTF gprectf = new GPRECTF(rect);
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrushFromRectWithAngle(ref gprectf, color1.ToArgb(), color2.ToArgb(), angle, isAngleScaleable, 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x000236BA File Offset: 0x000226BA
		public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle)
			: this(rect, color1, color2, angle, false)
		{
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x000236C8 File Offset: 0x000226C8
		public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle, bool isAngleScaleable)
		{
			IntPtr zero = IntPtr.Zero;
			if (rect.Width == 0 || rect.Height == 0)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[] { rect.ToString() }));
			}
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrushFromRectWithAngleI(ref gprect, color1.ToArgb(), color2.ToArgb(), angle, isAngleScaleable, 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00023756 File Offset: 0x00022756
		internal LinearGradientBrush(IntPtr nativeBrush)
		{
			base.SetNativeBrushInternal(nativeBrush);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00023768 File Offset: 0x00022768
		public override object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBrush(new HandleRef(this, base.NativeBrush), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new LinearGradientBrush(zero);
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x000237A0 File Offset: 0x000227A0
		private void _SetLinearColors(Color color1, Color color2)
		{
			int num = SafeNativeMethods.Gdip.GdipSetLineColors(new HandleRef(this, base.NativeBrush), color1.ToArgb(), color2.ToArgb());
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x000237D8 File Offset: 0x000227D8
		private Color[] _GetLinearColors()
		{
			int[] array = new int[2];
			int[] array2 = array;
			int num = SafeNativeMethods.Gdip.GdipGetLineColors(new HandleRef(this, base.NativeBrush), array2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Color[]
			{
				Color.FromArgb(array2[0]),
				Color.FromArgb(array2[1])
			};
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0002383B File Offset: 0x0002283B
		// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x00023843 File Offset: 0x00022843
		public Color[] LinearColors
		{
			get
			{
				return this._GetLinearColors();
			}
			set
			{
				this._SetLinearColors(value[0], value[1]);
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00023864 File Offset: 0x00022864
		private RectangleF _GetRectangle()
		{
			GPRECTF gprectf = default(GPRECTF);
			int num = SafeNativeMethods.Gdip.GdipGetLineRect(new HandleRef(this, base.NativeBrush), ref gprectf);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf.ToRectangleF();
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x0002389E File Offset: 0x0002289E
		public RectangleF Rectangle
		{
			get
			{
				return this._GetRectangle();
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x000238A8 File Offset: 0x000228A8
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x000238D4 File Offset: 0x000228D4
		public bool GammaCorrection
		{
			get
			{
				bool flag;
				int num = SafeNativeMethods.Gdip.GdipGetLineGammaCorrection(new HandleRef(this, base.NativeBrush), out flag);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return flag;
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetLineGammaCorrection(new HandleRef(this, base.NativeBrush), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00023900 File Offset: 0x00022900
		private Blend _GetBlend()
		{
			if (this.interpolationColorsWasSet)
			{
				return null;
			}
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetLineBlendCount(new HandleRef(this, base.NativeBrush), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			if (num <= 0)
			{
				return null;
			}
			int num3 = num;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			Blend blend;
			try
			{
				int num4 = checked(4 * num3);
				intPtr = Marshal.AllocHGlobal(num4);
				intPtr2 = Marshal.AllocHGlobal(num4);
				num2 = SafeNativeMethods.Gdip.GdipGetLineBlend(new HandleRef(this, base.NativeBrush), intPtr, intPtr2, num3);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				blend = new Blend(num3);
				Marshal.Copy(intPtr, blend.Factors, 0, num3);
				Marshal.Copy(intPtr2, blend.Positions, 0, num3);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
			}
			return blend;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x000239E8 File Offset: 0x000229E8
		private void _SetBlend(Blend blend)
		{
			int num = blend.Factors.Length;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				int num2 = checked(4 * num);
				intPtr = Marshal.AllocHGlobal(num2);
				intPtr2 = Marshal.AllocHGlobal(num2);
				Marshal.Copy(blend.Factors, 0, intPtr, num);
				Marshal.Copy(blend.Positions, 0, intPtr2, num);
				int num3 = SafeNativeMethods.Gdip.GdipSetLineBlend(new HandleRef(this, base.NativeBrush), new HandleRef(null, intPtr), new HandleRef(null, intPtr2), num);
				if (num3 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num3);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00023AA0 File Offset: 0x00022AA0
		// (set) Token: 0x06000BE8 RID: 3048 RVA: 0x00023AA8 File Offset: 0x00022AA8
		public Blend Blend
		{
			get
			{
				return this._GetBlend();
			}
			set
			{
				this._SetBlend(value);
			}
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00023AB1 File Offset: 0x00022AB1
		public void SetSigmaBellShape(float focus)
		{
			this.SetSigmaBellShape(focus, 1f);
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00023AC0 File Offset: 0x00022AC0
		public void SetSigmaBellShape(float focus, float scale)
		{
			int num = SafeNativeMethods.Gdip.GdipSetLineSigmaBlend(new HandleRef(this, base.NativeBrush), focus, scale);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00023AEB File Offset: 0x00022AEB
		public void SetBlendTriangularShape(float focus)
		{
			this.SetBlendTriangularShape(focus, 1f);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00023AFC File Offset: 0x00022AFC
		public void SetBlendTriangularShape(float focus, float scale)
		{
			int num = SafeNativeMethods.Gdip.GdipSetLineLinearBlend(new HandleRef(this, base.NativeBrush), focus, scale);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00023B28 File Offset: 0x00022B28
		private ColorBlend _GetInterpolationColors()
		{
			if (!this.interpolationColorsWasSet)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsColorBlendNotSet"),
					""
				}));
			}
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetLinePresetBlendCount(new HandleRef(this, base.NativeBrush), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			int num3 = num;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			ColorBlend colorBlend;
			try
			{
				int num4 = checked(4 * num3);
				intPtr = Marshal.AllocHGlobal(num4);
				intPtr2 = Marshal.AllocHGlobal(num4);
				num2 = SafeNativeMethods.Gdip.GdipGetLinePresetBlend(new HandleRef(this, base.NativeBrush), intPtr, intPtr2, num3);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				colorBlend = new ColorBlend(num3);
				int[] array = new int[num3];
				Marshal.Copy(intPtr, array, 0, num3);
				Marshal.Copy(intPtr2, colorBlend.Positions, 0, num3);
				colorBlend.Colors = new Color[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					colorBlend.Colors[i] = Color.FromArgb(array[i]);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
			}
			return colorBlend;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00023C7C File Offset: 0x00022C7C
		private void _SetInterpolationColors(ColorBlend blend)
		{
			this.interpolationColorsWasSet = true;
			if (blend == null)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsInvalidColorBlendObject"),
					""
				}));
			}
			if (blend.Colors.Length < 2)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsInvalidColorBlendObject"),
					SR.GetString("InterpolationColorsLength")
				}));
			}
			if (blend.Colors.Length != blend.Positions.Length)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsInvalidColorBlendObject"),
					SR.GetString("InterpolationColorsLengthsDiffer")
				}));
			}
			if (blend.Positions[0] != 0f)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsInvalidColorBlendObject"),
					SR.GetString("InterpolationColorsInvalidStartPosition")
				}));
			}
			if (blend.Positions[blend.Positions.Length - 1] != 1f)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsInvalidColorBlendObject"),
					SR.GetString("InterpolationColorsInvalidEndPosition")
				}));
			}
			int num = blend.Colors.Length;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				int num2 = checked(4 * num);
				intPtr = Marshal.AllocHGlobal(num2);
				intPtr2 = Marshal.AllocHGlobal(num2);
				int[] array = new int[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = blend.Colors[i].ToArgb();
				}
				Marshal.Copy(array, 0, intPtr, num);
				Marshal.Copy(blend.Positions, 0, intPtr2, num);
				int num3 = SafeNativeMethods.Gdip.GdipSetLinePresetBlend(new HandleRef(this, base.NativeBrush), new HandleRef(null, intPtr), new HandleRef(null, intPtr2), num);
				if (num3 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num3);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x00023EB8 File Offset: 0x00022EB8
		// (set) Token: 0x06000BF0 RID: 3056 RVA: 0x00023EC0 File Offset: 0x00022EC0
		public ColorBlend InterpolationColors
		{
			get
			{
				return this._GetInterpolationColors();
			}
			set
			{
				this._SetInterpolationColors(value);
			}
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00023ECC File Offset: 0x00022ECC
		private void _SetWrapMode(WrapMode wrapMode)
		{
			int num = SafeNativeMethods.Gdip.GdipSetLineWrapMode(new HandleRef(this, base.NativeBrush), (int)wrapMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00023EF8 File Offset: 0x00022EF8
		private WrapMode _GetWrapMode()
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetLineWrapMode(new HandleRef(this, base.NativeBrush), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return (WrapMode)num;
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x00023F26 File Offset: 0x00022F26
		// (set) Token: 0x06000BF4 RID: 3060 RVA: 0x00023F2E File Offset: 0x00022F2E
		public WrapMode WrapMode
		{
			get
			{
				return this._GetWrapMode();
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(WrapMode));
				}
				this._SetWrapMode(value);
			}
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00023F60 File Offset: 0x00022F60
		private void _SetTransform(Matrix matrix)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipSetLineTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00023FA4 File Offset: 0x00022FA4
		private Matrix _GetTransform()
		{
			Matrix matrix = new Matrix();
			int num = SafeNativeMethods.Gdip.GdipGetLineTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return matrix;
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x00023FE0 File Offset: 0x00022FE0
		// (set) Token: 0x06000BF8 RID: 3064 RVA: 0x00023FE8 File Offset: 0x00022FE8
		public Matrix Transform
		{
			get
			{
				return this._GetTransform();
			}
			set
			{
				this._SetTransform(value);
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00023FF4 File Offset: 0x00022FF4
		public void ResetTransform()
		{
			int num = SafeNativeMethods.Gdip.GdipResetLineTransform(new HandleRef(this, base.NativeBrush));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0002401D File Offset: 0x0002301D
		public void MultiplyTransform(Matrix matrix)
		{
			this.MultiplyTransform(matrix, MatrixOrder.Prepend);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00024028 File Offset: 0x00023028
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipMultiplyLineTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix), order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0002406C File Offset: 0x0002306C
		public void TranslateTransform(float dx, float dy)
		{
			this.TranslateTransform(dx, dy, MatrixOrder.Prepend);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00024078 File Offset: 0x00023078
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateLineTransform(new HandleRef(this, base.NativeBrush), dx, dy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x000240A4 File Offset: 0x000230A4
		public void ScaleTransform(float sx, float sy)
		{
			this.ScaleTransform(sx, sy, MatrixOrder.Prepend);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x000240B0 File Offset: 0x000230B0
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipScaleLineTransform(new HandleRef(this, base.NativeBrush), sx, sy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x000240DC File Offset: 0x000230DC
		public void RotateTransform(float angle)
		{
			this.RotateTransform(angle, MatrixOrder.Prepend);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x000240E8 File Offset: 0x000230E8
		public void RotateTransform(float angle, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipRotateLineTransform(new HandleRef(this, base.NativeBrush), angle, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x04000A70 RID: 2672
		private bool interpolationColorsWasSet;
	}
}
