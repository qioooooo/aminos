using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x020000EE RID: 238
	public sealed class TextureBrush : Brush
	{
		// Token: 0x06000D4E RID: 3406 RVA: 0x00027A9C File Offset: 0x00026A9C
		public TextureBrush(Image bitmap)
			: this(bitmap, WrapMode.Tile)
		{
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00027AA8 File Offset: 0x00026AA8
		public TextureBrush(Image image, WrapMode wrapMode)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (!ClientUtils.IsEnumValid(wrapMode, (int)wrapMode, 0, 4))
			{
				throw new InvalidEnumArgumentException("wrapMode", (int)wrapMode, typeof(WrapMode));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateTexture(new HandleRef(image, image.nativeImage), (int)wrapMode, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00027B1C File Offset: 0x00026B1C
		public TextureBrush(Image image, WrapMode wrapMode, RectangleF dstRect)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (!ClientUtils.IsEnumValid(wrapMode, (int)wrapMode, 0, 4))
			{
				throw new InvalidEnumArgumentException("wrapMode", (int)wrapMode, typeof(WrapMode));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateTexture2(new HandleRef(image, image.nativeImage), (int)wrapMode, dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00027BAC File Offset: 0x00026BAC
		public TextureBrush(Image image, WrapMode wrapMode, Rectangle dstRect)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (!ClientUtils.IsEnumValid(wrapMode, (int)wrapMode, 0, 4))
			{
				throw new InvalidEnumArgumentException("wrapMode", (int)wrapMode, typeof(WrapMode));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateTexture2I(new HandleRef(image, image.nativeImage), (int)wrapMode, dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00027C3B File Offset: 0x00026C3B
		public TextureBrush(Image image, RectangleF dstRect)
			: this(image, dstRect, null)
		{
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00027C48 File Offset: 0x00026C48
		public TextureBrush(Image image, RectangleF dstRect, ImageAttributes imageAttr)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateTextureIA(new HandleRef(image, image.nativeImage), new HandleRef(imageAttr, (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes), dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00027CC6 File Offset: 0x00026CC6
		public TextureBrush(Image image, Rectangle dstRect)
			: this(image, dstRect, null)
		{
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00027CD4 File Offset: 0x00026CD4
		public TextureBrush(Image image, Rectangle dstRect, ImageAttributes imageAttr)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateTextureIAI(new HandleRef(image, image.nativeImage), new HandleRef(imageAttr, (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes), dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00027D52 File Offset: 0x00026D52
		internal TextureBrush(IntPtr nativeBrush)
		{
			base.SetNativeBrushInternal(nativeBrush);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00027D64 File Offset: 0x00026D64
		public override object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBrush(new HandleRef(this, base.NativeBrush), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new TextureBrush(zero);
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00027D9C File Offset: 0x00026D9C
		private void _SetTransform(Matrix matrix)
		{
			int num = SafeNativeMethods.Gdip.GdipSetTextureTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00027DD4 File Offset: 0x00026DD4
		private Matrix _GetTransform()
		{
			Matrix matrix = new Matrix();
			int num = SafeNativeMethods.Gdip.GdipGetTextureTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return matrix;
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00027E10 File Offset: 0x00026E10
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x00027E18 File Offset: 0x00026E18
		public Matrix Transform
		{
			get
			{
				return this._GetTransform();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._SetTransform(value);
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00027E30 File Offset: 0x00026E30
		private void _SetWrapMode(WrapMode wrapMode)
		{
			int num = SafeNativeMethods.Gdip.GdipSetTextureWrapMode(new HandleRef(this, base.NativeBrush), (int)wrapMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00027E5C File Offset: 0x00026E5C
		private WrapMode _GetWrapMode()
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetTextureWrapMode(new HandleRef(this, base.NativeBrush), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return (WrapMode)num;
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00027E8A File Offset: 0x00026E8A
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x00027E92 File Offset: 0x00026E92
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

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00027EC4 File Offset: 0x00026EC4
		public Image Image
		{
			get
			{
				IntPtr intPtr;
				int num = SafeNativeMethods.Gdip.GdipGetTextureImage(new HandleRef(this, base.NativeBrush), out intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return Image.CreateImageObject(intPtr);
			}
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00027EF8 File Offset: 0x00026EF8
		public void ResetTransform()
		{
			int num = SafeNativeMethods.Gdip.GdipResetTextureTransform(new HandleRef(this, base.NativeBrush));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00027F21 File Offset: 0x00026F21
		public void MultiplyTransform(Matrix matrix)
		{
			this.MultiplyTransform(matrix, MatrixOrder.Prepend);
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00027F2C File Offset: 0x00026F2C
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipMultiplyTextureTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix), order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00027F70 File Offset: 0x00026F70
		public void TranslateTransform(float dx, float dy)
		{
			this.TranslateTransform(dx, dy, MatrixOrder.Prepend);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00027F7C File Offset: 0x00026F7C
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateTextureTransform(new HandleRef(this, base.NativeBrush), dx, dy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00027FA8 File Offset: 0x00026FA8
		public void ScaleTransform(float sx, float sy)
		{
			this.ScaleTransform(sx, sy, MatrixOrder.Prepend);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00027FB4 File Offset: 0x00026FB4
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipScaleTextureTransform(new HandleRef(this, base.NativeBrush), sx, sy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00027FE0 File Offset: 0x00026FE0
		public void RotateTransform(float angle)
		{
			this.RotateTransform(angle, MatrixOrder.Prepend);
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00027FEC File Offset: 0x00026FEC
		public void RotateTransform(float angle, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipRotateTextureTransform(new HandleRef(this, base.NativeBrush), angle, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}
	}
}
