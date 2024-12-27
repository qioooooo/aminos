using System;
using System.ComponentModel;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000D4 RID: 212
	public sealed class PathGradientBrush : Brush
	{
		// Token: 0x06000C80 RID: 3200 RVA: 0x00025754 File Offset: 0x00024754
		public PathGradientBrush(PointF[] points)
			: this(points, WrapMode.Clamp)
		{
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00025760 File Offset: 0x00024760
		public PathGradientBrush(PointF[] points, WrapMode wrapMode)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (!ClientUtils.IsEnumValid(wrapMode, (int)wrapMode, 0, 4))
			{
				throw new InvalidEnumArgumentException("wrapMode", (int)wrapMode, typeof(WrapMode));
			}
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipCreatePathGradient(new HandleRef(null, intPtr), points.Length, (int)wrapMode, out zero);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				base.SetNativeBrushInternal(zero);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00025800 File Offset: 0x00024800
		public PathGradientBrush(Point[] points)
			: this(points, WrapMode.Clamp)
		{
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0002580C File Offset: 0x0002480C
		public PathGradientBrush(Point[] points, WrapMode wrapMode)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (!ClientUtils.IsEnumValid(wrapMode, (int)wrapMode, 0, 4))
			{
				throw new InvalidEnumArgumentException("wrapMode", (int)wrapMode, typeof(WrapMode));
			}
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipCreatePathGradientI(new HandleRef(null, intPtr), points.Length, (int)wrapMode, out zero);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				base.SetNativeBrushInternal(zero);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000258AC File Offset: 0x000248AC
		public PathGradientBrush(GraphicsPath path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreatePathGradientFromPath(new HandleRef(path, path.nativePath), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x000258F8 File Offset: 0x000248F8
		internal PathGradientBrush(IntPtr nativeBrush)
		{
			base.SetNativeBrushInternal(nativeBrush);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00025908 File Offset: 0x00024908
		public override object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBrush(new HandleRef(this, base.NativeBrush), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new PathGradientBrush(zero);
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00025940 File Offset: 0x00024940
		// (set) Token: 0x06000C88 RID: 3208 RVA: 0x00025974 File Offset: 0x00024974
		public Color CenterColor
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetPathGradientCenterColor(new HandleRef(this, base.NativeBrush), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return Color.FromArgb(num2);
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetPathGradientCenterColor(new HandleRef(this, base.NativeBrush), value.ToArgb());
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x000259A4 File Offset: 0x000249A4
		private void _SetSurroundColors(Color[] colors)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipGetPathGradientSurroundColorCount(new HandleRef(this, base.NativeBrush), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (colors.Length > num2 || num2 <= 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			num2 = colors.Length;
			int[] array = new int[num2];
			for (int i = 0; i < colors.Length; i++)
			{
				array[i] = colors[i].ToArgb();
			}
			num = SafeNativeMethods.Gdip.GdipSetPathGradientSurroundColorsWithCount(new HandleRef(this, base.NativeBrush), array, ref num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00025A28 File Offset: 0x00024A28
		private Color[] _GetSurroundColors()
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipGetPathGradientSurroundColorCount(new HandleRef(this, base.NativeBrush), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			int[] array = new int[num2];
			num = SafeNativeMethods.Gdip.GdipGetPathGradientSurroundColorsWithCount(new HandleRef(this, base.NativeBrush), array, ref num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			Color[] array2 = new Color[num2];
			for (int i = 0; i < num2; i++)
			{
				array2[i] = Color.FromArgb(array[i]);
			}
			return array2;
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00025AA7 File Offset: 0x00024AA7
		// (set) Token: 0x06000C8C RID: 3212 RVA: 0x00025AAF File Offset: 0x00024AAF
		public Color[] SurroundColors
		{
			get
			{
				return this._GetSurroundColors();
			}
			set
			{
				this._SetSurroundColors(value);
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x00025AB8 File Offset: 0x00024AB8
		// (set) Token: 0x06000C8E RID: 3214 RVA: 0x00025AF0 File Offset: 0x00024AF0
		public PointF CenterPoint
		{
			get
			{
				GPPOINTF gppointf = new GPPOINTF();
				int num = SafeNativeMethods.Gdip.GdipGetPathGradientCenterPoint(new HandleRef(this, base.NativeBrush), gppointf);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return gppointf.ToPoint();
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetPathGradientCenterPoint(new HandleRef(this, base.NativeBrush), new GPPOINTF(value));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00025B20 File Offset: 0x00024B20
		private RectangleF _GetRectangle()
		{
			GPRECTF gprectf = default(GPRECTF);
			int num = SafeNativeMethods.Gdip.GdipGetPathGradientRect(new HandleRef(this, base.NativeBrush), ref gprectf);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf.ToRectangleF();
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00025B5A File Offset: 0x00024B5A
		public RectangleF Rectangle
		{
			get
			{
				return this._GetRectangle();
			}
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00025B64 File Offset: 0x00024B64
		private Blend _GetBlend()
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetPathGradientBlendCount(new HandleRef(this, base.NativeBrush), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
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
				num2 = SafeNativeMethods.Gdip.GdipGetPathGradientBlend(new HandleRef(this, base.NativeBrush), intPtr, intPtr2, num3);
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

		// Token: 0x06000C92 RID: 3218 RVA: 0x00025C3C File Offset: 0x00024C3C
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
				int num3 = SafeNativeMethods.Gdip.GdipSetPathGradientBlend(new HandleRef(this, base.NativeBrush), new HandleRef(null, intPtr), new HandleRef(null, intPtr2), num);
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

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00025CF4 File Offset: 0x00024CF4
		// (set) Token: 0x06000C94 RID: 3220 RVA: 0x00025CFC File Offset: 0x00024CFC
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

		// Token: 0x06000C95 RID: 3221 RVA: 0x00025D05 File Offset: 0x00024D05
		public void SetSigmaBellShape(float focus)
		{
			this.SetSigmaBellShape(focus, 1f);
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00025D14 File Offset: 0x00024D14
		public void SetSigmaBellShape(float focus, float scale)
		{
			int num = SafeNativeMethods.Gdip.GdipSetPathGradientSigmaBlend(new HandleRef(this, base.NativeBrush), focus, scale);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00025D3F File Offset: 0x00024D3F
		public void SetBlendTriangularShape(float focus)
		{
			this.SetBlendTriangularShape(focus, 1f);
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00025D50 File Offset: 0x00024D50
		public void SetBlendTriangularShape(float focus, float scale)
		{
			int num = SafeNativeMethods.Gdip.GdipSetPathGradientLinearBlend(new HandleRef(this, base.NativeBrush), focus, scale);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00025D7C File Offset: 0x00024D7C
		private ColorBlend _GetInterpolationColors()
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetPathGradientPresetBlendCount(new HandleRef(this, base.NativeBrush), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			if (num == 0)
			{
				return new ColorBlend();
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
				num2 = SafeNativeMethods.Gdip.GdipGetPathGradientPresetBlend(new HandleRef(this, base.NativeBrush), intPtr, intPtr2, num3);
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

		// Token: 0x06000C9A RID: 3226 RVA: 0x00025EA0 File Offset: 0x00024EA0
		private void _SetInterpolationColors(ColorBlend blend)
		{
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
				int num3 = SafeNativeMethods.Gdip.GdipSetPathGradientPresetBlend(new HandleRef(this, base.NativeBrush), new HandleRef(null, intPtr), new HandleRef(null, intPtr2), num);
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

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00025F84 File Offset: 0x00024F84
		// (set) Token: 0x06000C9C RID: 3228 RVA: 0x00025F8C File Offset: 0x00024F8C
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

		// Token: 0x06000C9D RID: 3229 RVA: 0x00025F98 File Offset: 0x00024F98
		private void _SetTransform(Matrix matrix)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipSetPathGradientTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00025FDC File Offset: 0x00024FDC
		private Matrix _GetTransform()
		{
			Matrix matrix = new Matrix();
			int num = SafeNativeMethods.Gdip.GdipGetPathGradientTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return matrix;
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x00026018 File Offset: 0x00025018
		// (set) Token: 0x06000CA0 RID: 3232 RVA: 0x00026020 File Offset: 0x00025020
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

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0002602C File Offset: 0x0002502C
		public void ResetTransform()
		{
			int num = SafeNativeMethods.Gdip.GdipResetPathGradientTransform(new HandleRef(this, base.NativeBrush));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00026055 File Offset: 0x00025055
		public void MultiplyTransform(Matrix matrix)
		{
			this.MultiplyTransform(matrix, MatrixOrder.Prepend);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00026060 File Offset: 0x00025060
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipMultiplyPathGradientTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix), order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x000260A4 File Offset: 0x000250A4
		public void TranslateTransform(float dx, float dy)
		{
			this.TranslateTransform(dx, dy, MatrixOrder.Prepend);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x000260B0 File Offset: 0x000250B0
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslatePathGradientTransform(new HandleRef(this, base.NativeBrush), dx, dy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x000260DC File Offset: 0x000250DC
		public void ScaleTransform(float sx, float sy)
		{
			this.ScaleTransform(sx, sy, MatrixOrder.Prepend);
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x000260E8 File Offset: 0x000250E8
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipScalePathGradientTransform(new HandleRef(this, base.NativeBrush), sx, sy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00026114 File Offset: 0x00025114
		public void RotateTransform(float angle)
		{
			this.RotateTransform(angle, MatrixOrder.Prepend);
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00026120 File Offset: 0x00025120
		public void RotateTransform(float angle, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipRotatePathGradientTransform(new HandleRef(this, base.NativeBrush), angle, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0002614C File Offset: 0x0002514C
		// (set) Token: 0x06000CAB RID: 3243 RVA: 0x00026198 File Offset: 0x00025198
		public PointF FocusScales
		{
			get
			{
				float[] array = new float[1];
				float[] array2 = array;
				float[] array3 = new float[1];
				float[] array4 = array3;
				int num = SafeNativeMethods.Gdip.GdipGetPathGradientFocusScales(new HandleRef(this, base.NativeBrush), array2, array4);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return new PointF(array2[0], array4[0]);
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetPathGradientFocusScales(new HandleRef(this, base.NativeBrush), value.X, value.Y);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x000261D0 File Offset: 0x000251D0
		private void _SetWrapMode(WrapMode wrapMode)
		{
			int num = SafeNativeMethods.Gdip.GdipSetPathGradientWrapMode(new HandleRef(this, base.NativeBrush), (int)wrapMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x000261FC File Offset: 0x000251FC
		private WrapMode _GetWrapMode()
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetPathGradientWrapMode(new HandleRef(this, base.NativeBrush), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return (WrapMode)num;
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x0002622A File Offset: 0x0002522A
		// (set) Token: 0x06000CAF RID: 3247 RVA: 0x00026232 File Offset: 0x00025232
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
	}
}
