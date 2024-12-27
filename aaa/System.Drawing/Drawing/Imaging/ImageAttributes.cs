using System;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x020000BB RID: 187
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ImageAttributes : ICloneable, IDisposable
	{
		// Token: 0x06000B6D RID: 2925 RVA: 0x000224D9 File Offset: 0x000214D9
		internal void SetNativeImageAttributes(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			this.nativeImageAttributes = handle;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x000224FC File Offset: 0x000214FC
		public ImageAttributes()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateImageAttributes(out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativeImageAttributes(zero);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0002252E File Offset: 0x0002152E
		internal ImageAttributes(IntPtr newNativeImageAttributes)
		{
			this.SetNativeImageAttributes(newNativeImageAttributes);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0002253D File Offset: 0x0002153D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0002254C File Offset: 0x0002154C
		private void Dispose(bool disposing)
		{
			if (this.nativeImageAttributes != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDisposeImageAttributes(new HandleRef(this, this.nativeImageAttributes));
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
					this.nativeImageAttributes = IntPtr.Zero;
				}
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x000225B8 File Offset: 0x000215B8
		~ImageAttributes()
		{
			this.Dispose(false);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x000225E8 File Offset: 0x000215E8
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneImageAttributes(new HandleRef(this, this.nativeImageAttributes), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new ImageAttributes(zero);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002261F File Offset: 0x0002161F
		public void SetColorMatrix(ColorMatrix newColorMatrix)
		{
			this.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0002262A File Offset: 0x0002162A
		public void SetColorMatrix(ColorMatrix newColorMatrix, ColorMatrixFlag flags)
		{
			this.SetColorMatrix(newColorMatrix, flags, ColorAdjustType.Default);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00022638 File Offset: 0x00021638
		public void SetColorMatrix(ColorMatrix newColorMatrix, ColorMatrixFlag mode, ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesColorMatrix(new HandleRef(this, this.nativeImageAttributes), type, true, newColorMatrix, null, mode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00022666 File Offset: 0x00021666
		public void ClearColorMatrix()
		{
			this.ClearColorMatrix(ColorAdjustType.Default);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00022670 File Offset: 0x00021670
		public void ClearColorMatrix(ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesColorMatrix(new HandleRef(this, this.nativeImageAttributes), type, false, null, null, ColorMatrixFlag.Default);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0002269E File Offset: 0x0002169E
		public void SetColorMatrices(ColorMatrix newColorMatrix, ColorMatrix grayMatrix)
		{
			this.SetColorMatrices(newColorMatrix, grayMatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x000226AA File Offset: 0x000216AA
		public void SetColorMatrices(ColorMatrix newColorMatrix, ColorMatrix grayMatrix, ColorMatrixFlag flags)
		{
			this.SetColorMatrices(newColorMatrix, grayMatrix, flags, ColorAdjustType.Default);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x000226B8 File Offset: 0x000216B8
		public void SetColorMatrices(ColorMatrix newColorMatrix, ColorMatrix grayMatrix, ColorMatrixFlag mode, ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesColorMatrix(new HandleRef(this, this.nativeImageAttributes), type, true, newColorMatrix, grayMatrix, mode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x000226E7 File Offset: 0x000216E7
		public void SetThreshold(float threshold)
		{
			this.SetThreshold(threshold, ColorAdjustType.Default);
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x000226F4 File Offset: 0x000216F4
		public void SetThreshold(float threshold, ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesThreshold(new HandleRef(this, this.nativeImageAttributes), type, true, threshold);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00022720 File Offset: 0x00021720
		public void ClearThreshold()
		{
			this.ClearThreshold(ColorAdjustType.Default);
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0002272C File Offset: 0x0002172C
		public void ClearThreshold(ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesThreshold(new HandleRef(this, this.nativeImageAttributes), type, false, 0f);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0002275C File Offset: 0x0002175C
		public void SetGamma(float gamma)
		{
			this.SetGamma(gamma, ColorAdjustType.Default);
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00022768 File Offset: 0x00021768
		public void SetGamma(float gamma, ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesGamma(new HandleRef(this, this.nativeImageAttributes), type, true, gamma);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00022794 File Offset: 0x00021794
		public void ClearGamma()
		{
			this.ClearGamma(ColorAdjustType.Default);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x000227A0 File Offset: 0x000217A0
		public void ClearGamma(ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesGamma(new HandleRef(this, this.nativeImageAttributes), type, false, 0f);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x000227D0 File Offset: 0x000217D0
		public void SetNoOp()
		{
			this.SetNoOp(ColorAdjustType.Default);
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000227DC File Offset: 0x000217DC
		public void SetNoOp(ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesNoOp(new HandleRef(this, this.nativeImageAttributes), type, true);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00022807 File Offset: 0x00021807
		public void ClearNoOp()
		{
			this.ClearNoOp(ColorAdjustType.Default);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00022810 File Offset: 0x00021810
		public void ClearNoOp(ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesNoOp(new HandleRef(this, this.nativeImageAttributes), type, false);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002283B File Offset: 0x0002183B
		public void SetColorKey(Color colorLow, Color colorHigh)
		{
			this.SetColorKey(colorLow, colorHigh, ColorAdjustType.Default);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00022848 File Offset: 0x00021848
		public void SetColorKey(Color colorLow, Color colorHigh, ColorAdjustType type)
		{
			int num = colorLow.ToArgb();
			int num2 = colorHigh.ToArgb();
			int num3 = SafeNativeMethods.Gdip.GdipSetImageAttributesColorKeys(new HandleRef(this, this.nativeImageAttributes), type, true, num, num2);
			if (num3 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num3);
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00022885 File Offset: 0x00021885
		public void ClearColorKey()
		{
			this.ClearColorKey(ColorAdjustType.Default);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00022890 File Offset: 0x00021890
		public void ClearColorKey(ColorAdjustType type)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipSetImageAttributesColorKeys(new HandleRef(this, this.nativeImageAttributes), type, false, num, num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x000228BF File Offset: 0x000218BF
		public void SetOutputChannel(ColorChannelFlag flags)
		{
			this.SetOutputChannel(flags, ColorAdjustType.Default);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x000228CC File Offset: 0x000218CC
		public void SetOutputChannel(ColorChannelFlag flags, ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesOutputChannel(new HandleRef(this, this.nativeImageAttributes), type, true, flags);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x000228F8 File Offset: 0x000218F8
		public void ClearOutputChannel()
		{
			this.ClearOutputChannel(ColorAdjustType.Default);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00022904 File Offset: 0x00021904
		public void ClearOutputChannel(ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesOutputChannel(new HandleRef(this, this.nativeImageAttributes), type, false, ColorChannelFlag.ColorChannelLast);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00022930 File Offset: 0x00021930
		public void SetOutputChannelColorProfile(string colorProfileFilename)
		{
			this.SetOutputChannelColorProfile(colorProfileFilename, ColorAdjustType.Default);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0002293C File Offset: 0x0002193C
		public void SetOutputChannelColorProfile(string colorProfileFilename, ColorAdjustType type)
		{
			IntSecurity.DemandReadFileIO(colorProfileFilename);
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesOutputChannelColorProfile(new HandleRef(this, this.nativeImageAttributes), type, true, colorProfileFilename);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0002296E File Offset: 0x0002196E
		public void ClearOutputChannelColorProfile()
		{
			this.ClearOutputChannel(ColorAdjustType.Default);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00022978 File Offset: 0x00021978
		public void ClearOutputChannelColorProfile(ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesOutputChannel(new HandleRef(this, this.nativeImageAttributes), type, false, ColorChannelFlag.ColorChannelLast);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x000229A4 File Offset: 0x000219A4
		public void SetRemapTable(ColorMap[] map)
		{
			this.SetRemapTable(map, ColorAdjustType.Default);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000229B0 File Offset: 0x000219B0
		public void SetRemapTable(ColorMap[] map, ColorAdjustType type)
		{
			int num = map.Length;
			int num2 = 4;
			IntPtr intPtr = Marshal.AllocHGlobal(checked(num * num2 * 2));
			try
			{
				for (int i = 0; i < num; i++)
				{
					Marshal.StructureToPtr(map[i].OldColor.ToArgb(), (IntPtr)((long)intPtr + (long)(i * num2 * 2)), false);
					Marshal.StructureToPtr(map[i].NewColor.ToArgb(), (IntPtr)((long)intPtr + (long)(i * num2 * 2) + (long)num2), false);
				}
				int num3 = SafeNativeMethods.Gdip.GdipSetImageAttributesRemapTable(new HandleRef(this, this.nativeImageAttributes), type, true, num, new HandleRef(null, intPtr));
				if (num3 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num3);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00022A80 File Offset: 0x00021A80
		public void ClearRemapTable()
		{
			this.ClearRemapTable(ColorAdjustType.Default);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00022A8C File Offset: 0x00021A8C
		public void ClearRemapTable(ColorAdjustType type)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesRemapTable(new HandleRef(this, this.nativeImageAttributes), type, false, 0, NativeMethods.NullHandleRef);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00022ABD File Offset: 0x00021ABD
		public void SetBrushRemapTable(ColorMap[] map)
		{
			this.SetRemapTable(map, ColorAdjustType.Brush);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00022AC7 File Offset: 0x00021AC7
		public void ClearBrushRemapTable()
		{
			this.ClearRemapTable(ColorAdjustType.Brush);
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00022AD0 File Offset: 0x00021AD0
		public void SetWrapMode(WrapMode mode)
		{
			this.SetWrapMode(mode, default(Color), false);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00022AEE File Offset: 0x00021AEE
		public void SetWrapMode(WrapMode mode, Color color)
		{
			this.SetWrapMode(mode, color, false);
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00022AFC File Offset: 0x00021AFC
		public void SetWrapMode(WrapMode mode, Color color, bool clamp)
		{
			int num = SafeNativeMethods.Gdip.GdipSetImageAttributesWrapMode(new HandleRef(this, this.nativeImageAttributes), (int)mode, color.ToArgb(), clamp);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00022B30 File Offset: 0x00021B30
		public void GetAdjustedPalette(ColorPalette palette, ColorAdjustType type)
		{
			IntPtr intPtr = palette.ConvertToMemory();
			try
			{
				int num = SafeNativeMethods.Gdip.GdipGetImageAttributesAdjustedPalette(new HandleRef(this, this.nativeImageAttributes), new HandleRef(null, intPtr), type);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				palette.ConvertFromMemory(intPtr);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		// Token: 0x04000A21 RID: 2593
		internal IntPtr nativeImageAttributes;
	}
}
