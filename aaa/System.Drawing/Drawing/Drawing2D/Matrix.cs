using System;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000C9 RID: 201
	public sealed class Matrix : MarshalByRefObject, IDisposable
	{
		// Token: 0x06000C02 RID: 3074 RVA: 0x00024114 File Offset: 0x00023114
		public Matrix()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMatrix(out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativeMatrix = zero;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00024148 File Offset: 0x00023148
		public Matrix(float m11, float m12, float m21, float m22, float dx, float dy)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMatrix2(m11, m12, m21, m22, dx, dy, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativeMatrix = zero;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00024184 File Offset: 0x00023184
		public Matrix(RectangleF rect, PointF[] plgpts)
		{
			if (plgpts == null)
			{
				throw new ArgumentNullException("plgpts");
			}
			if (plgpts.Length != 3)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(plgpts);
			try
			{
				IntPtr zero = IntPtr.Zero;
				GPRECTF gprectf = new GPRECTF(rect);
				int num = SafeNativeMethods.Gdip.GdipCreateMatrix3(ref gprectf, new HandleRef(null, intPtr), out zero);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				this.nativeMatrix = zero;
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00024208 File Offset: 0x00023208
		public Matrix(Rectangle rect, Point[] plgpts)
		{
			if (plgpts == null)
			{
				throw new ArgumentNullException("plgpts");
			}
			if (plgpts.Length != 3)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(plgpts);
			try
			{
				IntPtr zero = IntPtr.Zero;
				GPRECT gprect = new GPRECT(rect);
				int num = SafeNativeMethods.Gdip.GdipCreateMatrix3I(ref gprect, new HandleRef(null, intPtr), out zero);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				this.nativeMatrix = zero;
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0002428C File Offset: 0x0002328C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0002429B File Offset: 0x0002329B
		private void Dispose(bool disposing)
		{
			if (this.nativeMatrix != IntPtr.Zero)
			{
				SafeNativeMethods.Gdip.GdipDeleteMatrix(new HandleRef(this, this.nativeMatrix));
				this.nativeMatrix = IntPtr.Zero;
			}
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000242CC File Offset: 0x000232CC
		~Matrix()
		{
			this.Dispose(false);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x000242FC File Offset: 0x000232FC
		public Matrix Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneMatrix(new HandleRef(this, this.nativeMatrix), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Matrix(zero);
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00024334 File Offset: 0x00023334
		public float[] Elements
		{
			get
			{
				IntPtr intPtr = Marshal.AllocHGlobal(48);
				float[] array;
				try
				{
					int num = SafeNativeMethods.Gdip.GdipGetMatrixElements(new HandleRef(this, this.nativeMatrix), intPtr);
					if (num != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num);
					}
					array = new float[6];
					Marshal.Copy(intPtr, array, 0, 6);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return array;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x00024390 File Offset: 0x00023390
		public float OffsetX
		{
			get
			{
				return this.Elements[4];
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0002439A File Offset: 0x0002339A
		public float OffsetY
		{
			get
			{
				return this.Elements[5];
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000243A4 File Offset: 0x000233A4
		public void Reset()
		{
			int num = SafeNativeMethods.Gdip.GdipSetMatrixElements(new HandleRef(this, this.nativeMatrix), 1f, 0f, 0f, 1f, 0f, 0f);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000243EB File Offset: 0x000233EB
		public void Multiply(Matrix matrix)
		{
			this.Multiply(matrix, MatrixOrder.Prepend);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000243F8 File Offset: 0x000233F8
		public void Multiply(Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipMultiplyMatrix(new HandleRef(this, this.nativeMatrix), new HandleRef(matrix, matrix.nativeMatrix), order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0002443C File Offset: 0x0002343C
		public void Translate(float offsetX, float offsetY)
		{
			this.Translate(offsetX, offsetY, MatrixOrder.Prepend);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00024448 File Offset: 0x00023448
		public void Translate(float offsetX, float offsetY, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateMatrix(new HandleRef(this, this.nativeMatrix), offsetX, offsetY, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00024474 File Offset: 0x00023474
		public void Scale(float scaleX, float scaleY)
		{
			this.Scale(scaleX, scaleY, MatrixOrder.Prepend);
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00024480 File Offset: 0x00023480
		public void Scale(float scaleX, float scaleY, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipScaleMatrix(new HandleRef(this, this.nativeMatrix), scaleX, scaleY, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x000244AC File Offset: 0x000234AC
		public void Rotate(float angle)
		{
			this.Rotate(angle, MatrixOrder.Prepend);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x000244B8 File Offset: 0x000234B8
		public void Rotate(float angle, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipRotateMatrix(new HandleRef(this, this.nativeMatrix), angle, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x000244E3 File Offset: 0x000234E3
		public void RotateAt(float angle, PointF point)
		{
			this.RotateAt(angle, point, MatrixOrder.Prepend);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x000244F0 File Offset: 0x000234F0
		public void RotateAt(float angle, PointF point, MatrixOrder order)
		{
			int num;
			if (order == MatrixOrder.Prepend)
			{
				num = SafeNativeMethods.Gdip.GdipTranslateMatrix(new HandleRef(this, this.nativeMatrix), point.X, point.Y, order);
				num |= SafeNativeMethods.Gdip.GdipRotateMatrix(new HandleRef(this, this.nativeMatrix), angle, order);
				num |= SafeNativeMethods.Gdip.GdipTranslateMatrix(new HandleRef(this, this.nativeMatrix), -point.X, -point.Y, order);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipTranslateMatrix(new HandleRef(this, this.nativeMatrix), -point.X, -point.Y, order);
				num |= SafeNativeMethods.Gdip.GdipRotateMatrix(new HandleRef(this, this.nativeMatrix), angle, order);
				num |= SafeNativeMethods.Gdip.GdipTranslateMatrix(new HandleRef(this, this.nativeMatrix), point.X, point.Y, order);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x000245C4 File Offset: 0x000235C4
		public void Shear(float shearX, float shearY)
		{
			int num = SafeNativeMethods.Gdip.GdipShearMatrix(new HandleRef(this, this.nativeMatrix), shearX, shearY, MatrixOrder.Prepend);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x000245F0 File Offset: 0x000235F0
		public void Shear(float shearX, float shearY, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipShearMatrix(new HandleRef(this, this.nativeMatrix), shearX, shearY, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0002461C File Offset: 0x0002361C
		public void Invert()
		{
			int num = SafeNativeMethods.Gdip.GdipInvertMatrix(new HandleRef(this, this.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00024648 File Offset: 0x00023648
		public void TransformPoints(PointF[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipTransformMatrixPoints(new HandleRef(this, this.nativeMatrix), new HandleRef(null, intPtr), pts.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				PointF[] array = SafeNativeMethods.Gdip.ConvertGPPOINTFArrayF(intPtr, pts.Length);
				for (int i = 0; i < pts.Length; i++)
				{
					pts[i] = array[i];
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x000246DC File Offset: 0x000236DC
		public void TransformPoints(Point[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipTransformMatrixPointsI(new HandleRef(this, this.nativeMatrix), new HandleRef(null, intPtr), pts.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				Point[] array = SafeNativeMethods.Gdip.ConvertGPPOINTArray(intPtr, pts.Length);
				for (int i = 0; i < pts.Length; i++)
				{
					pts[i] = array[i];
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00024770 File Offset: 0x00023770
		public void TransformVectors(PointF[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipVectorTransformMatrixPoints(new HandleRef(this, this.nativeMatrix), new HandleRef(null, intPtr), pts.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				PointF[] array = SafeNativeMethods.Gdip.ConvertGPPOINTFArrayF(intPtr, pts.Length);
				for (int i = 0; i < pts.Length; i++)
				{
					pts[i] = array[i];
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00024804 File Offset: 0x00023804
		public void VectorTransformPoints(Point[] pts)
		{
			this.TransformVectors(pts);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00024810 File Offset: 0x00023810
		public void TransformVectors(Point[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipVectorTransformMatrixPointsI(new HandleRef(this, this.nativeMatrix), new HandleRef(null, intPtr), pts.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				Point[] array = SafeNativeMethods.Gdip.ConvertGPPOINTArray(intPtr, pts.Length);
				for (int i = 0; i < pts.Length; i++)
				{
					pts[i] = array[i];
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x000248A4 File Offset: 0x000238A4
		public bool IsInvertible
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipIsMatrixInvertible(new HandleRef(this, this.nativeMatrix), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2 != 0;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x000248D8 File Offset: 0x000238D8
		public bool IsIdentity
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipIsMatrixIdentity(new HandleRef(this, this.nativeMatrix), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0002490C File Offset: 0x0002390C
		public override bool Equals(object obj)
		{
			Matrix matrix = obj as Matrix;
			if (matrix == null)
			{
				return false;
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsMatrixEqual(new HandleRef(this, this.nativeMatrix), new HandleRef(matrix, matrix.nativeMatrix), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00024956 File Offset: 0x00023956
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0002495E File Offset: 0x0002395E
		internal Matrix(IntPtr nativeMatrix)
		{
			this.SetNativeMatrix(nativeMatrix);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0002496D File Offset: 0x0002396D
		internal void SetNativeMatrix(IntPtr nativeMatrix)
		{
			this.nativeMatrix = nativeMatrix;
		}

		// Token: 0x04000A87 RID: 2695
		internal IntPtr nativeMatrix;
	}
}
