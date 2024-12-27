using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Drawing.Imaging
{
	// Token: 0x020000CB RID: 203
	[Editor("System.Drawing.Design.MetafileEditor, System.Drawing.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Serializable]
	public sealed class Metafile : Image
	{
		// Token: 0x06000C26 RID: 3110 RVA: 0x00024976 File Offset: 0x00023976
		public Metafile(IntPtr hmetafile, WmfPlaceableFileHeader wmfHeader)
			: this(hmetafile, wmfHeader, false)
		{
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00024984 File Offset: 0x00023984
		public Metafile(IntPtr hmetafile, WmfPlaceableFileHeader wmfHeader, bool deleteWmf)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMetafileFromWmf(new HandleRef(null, hmetafile), wmfHeader, deleteWmf, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x000249CC File Offset: 0x000239CC
		public Metafile(IntPtr henhmetafile, bool deleteEmf)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMetafileFromEmf(new HandleRef(null, henhmetafile), deleteEmf, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00024A10 File Offset: 0x00023A10
		public Metafile(string filename)
		{
			IntSecurity.DemandReadFileIO(filename);
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMetafileFromFile(filename, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00024A4C File Offset: 0x00023A4C
		public Metafile(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "stream", "null" }));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMetafileFromStream(new GPStream(stream), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00024AAF File Offset: 0x00023AAF
		public Metafile(IntPtr referenceHdc, EmfType emfType)
			: this(referenceHdc, emfType, null)
		{
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00024ABC File Offset: 0x00023ABC
		public Metafile(IntPtr referenceHdc, EmfType emfType, string description)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipRecordMetafile(new HandleRef(null, referenceHdc), (int)emfType, NativeMethods.NullHandleRef, 7, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00024B07 File Offset: 0x00023B07
		public Metafile(IntPtr referenceHdc, RectangleF frameRect)
			: this(referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00024B12 File Offset: 0x00023B12
		public Metafile(IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit)
			: this(referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00024B1E File Offset: 0x00023B1E
		public Metafile(IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type)
			: this(referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00024B2C File Offset: 0x00023B2C
		public Metafile(IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type, string description)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			GPRECTF gprectf = new GPRECTF(frameRect);
			int num = SafeNativeMethods.Gdip.GdipRecordMetafile(new HandleRef(null, referenceHdc), (int)type, ref gprectf, (int)frameUnit, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00024B83 File Offset: 0x00023B83
		public Metafile(IntPtr referenceHdc, Rectangle frameRect)
			: this(referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00024B8E File Offset: 0x00023B8E
		public Metafile(IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit)
			: this(referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00024B9A File Offset: 0x00023B9A
		public Metafile(IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type)
			: this(referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00024BA8 File Offset: 0x00023BA8
		public Metafile(IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type, string desc)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num;
			if (frameRect.IsEmpty)
			{
				num = SafeNativeMethods.Gdip.GdipRecordMetafile(new HandleRef(null, referenceHdc), (int)type, NativeMethods.NullHandleRef, 7, desc, out zero);
			}
			else
			{
				GPRECT gprect = new GPRECT(frameRect);
				num = SafeNativeMethods.Gdip.GdipRecordMetafileI(new HandleRef(null, referenceHdc), (int)type, ref gprect, (int)frameUnit, desc, out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00024C23 File Offset: 0x00023C23
		public Metafile(string fileName, IntPtr referenceHdc)
			: this(fileName, referenceHdc, EmfType.EmfPlusDual, null)
		{
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00024C2F File Offset: 0x00023C2F
		public Metafile(string fileName, IntPtr referenceHdc, EmfType type)
			: this(fileName, referenceHdc, type, null)
		{
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00024C3C File Offset: 0x00023C3C
		public Metafile(string fileName, IntPtr referenceHdc, EmfType type, string description)
		{
			IntSecurity.DemandReadFileIO(fileName);
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipRecordMetafileFileName(fileName, new HandleRef(null, referenceHdc), (int)type, NativeMethods.NullHandleRef, 7, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00024C8F File Offset: 0x00023C8F
		public Metafile(string fileName, IntPtr referenceHdc, RectangleF frameRect)
			: this(fileName, referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00024C9B File Offset: 0x00023C9B
		public Metafile(string fileName, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit)
			: this(fileName, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00024CA9 File Offset: 0x00023CA9
		public Metafile(string fileName, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type)
			: this(fileName, referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00024CB9 File Offset: 0x00023CB9
		public Metafile(string fileName, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, string desc)
			: this(fileName, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual, desc)
		{
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00024CCC File Offset: 0x00023CCC
		public Metafile(string fileName, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type, string description)
		{
			IntSecurity.DemandReadFileIO(fileName);
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			GPRECTF gprectf = new GPRECTF(frameRect);
			int num = SafeNativeMethods.Gdip.GdipRecordMetafileFileName(fileName, new HandleRef(null, referenceHdc), (int)type, ref gprectf, (int)frameUnit, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00024D2B File Offset: 0x00023D2B
		public Metafile(string fileName, IntPtr referenceHdc, Rectangle frameRect)
			: this(fileName, referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00024D37 File Offset: 0x00023D37
		public Metafile(string fileName, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit)
			: this(fileName, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00024D45 File Offset: 0x00023D45
		public Metafile(string fileName, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type)
			: this(fileName, referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00024D55 File Offset: 0x00023D55
		public Metafile(string fileName, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, string description)
			: this(fileName, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual, description)
		{
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00024D68 File Offset: 0x00023D68
		public Metafile(string fileName, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type, string description)
		{
			IntSecurity.DemandReadFileIO(fileName);
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num;
			if (frameRect.IsEmpty)
			{
				num = SafeNativeMethods.Gdip.GdipRecordMetafileFileName(fileName, new HandleRef(null, referenceHdc), (int)type, NativeMethods.NullHandleRef, (int)frameUnit, description, out zero);
			}
			else
			{
				GPRECT gprect = new GPRECT(frameRect);
				num = SafeNativeMethods.Gdip.GdipRecordMetafileFileNameI(fileName, new HandleRef(null, referenceHdc), (int)type, ref gprect, (int)frameUnit, description, out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00024DED File Offset: 0x00023DED
		public Metafile(Stream stream, IntPtr referenceHdc)
			: this(stream, referenceHdc, EmfType.EmfPlusDual, null)
		{
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00024DF9 File Offset: 0x00023DF9
		public Metafile(Stream stream, IntPtr referenceHdc, EmfType type)
			: this(stream, referenceHdc, type, null)
		{
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00024E08 File Offset: 0x00023E08
		public Metafile(Stream stream, IntPtr referenceHdc, EmfType type, string description)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipRecordMetafileStream(new GPStream(stream), new HandleRef(null, referenceHdc), (int)type, NativeMethods.NullHandleRef, 7, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00024E5A File Offset: 0x00023E5A
		public Metafile(Stream stream, IntPtr referenceHdc, RectangleF frameRect)
			: this(stream, referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00024E66 File Offset: 0x00023E66
		public Metafile(Stream stream, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit)
			: this(stream, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00024E74 File Offset: 0x00023E74
		public Metafile(Stream stream, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type)
			: this(stream, referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00024E84 File Offset: 0x00023E84
		public Metafile(Stream stream, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type, string description)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			GPRECTF gprectf = new GPRECTF(frameRect);
			int num = SafeNativeMethods.Gdip.GdipRecordMetafileStream(new GPStream(stream), new HandleRef(null, referenceHdc), (int)type, ref gprectf, (int)frameUnit, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00024EE2 File Offset: 0x00023EE2
		public Metafile(Stream stream, IntPtr referenceHdc, Rectangle frameRect)
			: this(stream, referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00024EEE File Offset: 0x00023EEE
		public Metafile(Stream stream, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit)
			: this(stream, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00024EFC File Offset: 0x00023EFC
		public Metafile(Stream stream, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type)
			: this(stream, referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00024F0C File Offset: 0x00023F0C
		public Metafile(Stream stream, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type, string description)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num;
			if (frameRect.IsEmpty)
			{
				num = SafeNativeMethods.Gdip.GdipRecordMetafileStream(new GPStream(stream), new HandleRef(null, referenceHdc), (int)type, NativeMethods.NullHandleRef, (int)frameUnit, description, out zero);
			}
			else
			{
				GPRECT gprect = new GPRECT(frameRect);
				num = SafeNativeMethods.Gdip.GdipRecordMetafileStreamI(new GPStream(stream), new HandleRef(null, referenceHdc), (int)type, ref gprect, (int)frameUnit, description, out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00024F95 File Offset: 0x00023F95
		private Metafile(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00024FA0 File Offset: 0x00023FA0
		public static MetafileHeader GetMetafileHeader(IntPtr hmetafile, WmfPlaceableFileHeader wmfHeader)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			MetafileHeader metafileHeader = new MetafileHeader();
			metafileHeader.wmf = new MetafileHeaderWmf();
			int num = SafeNativeMethods.Gdip.GdipGetMetafileHeaderFromWmf(new HandleRef(null, hmetafile), wmfHeader, metafileHeader.wmf);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return metafileHeader;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00024FE8 File Offset: 0x00023FE8
		public static MetafileHeader GetMetafileHeader(IntPtr henhmetafile)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			MetafileHeader metafileHeader = new MetafileHeader();
			metafileHeader.emf = new MetafileHeaderEmf();
			int num = SafeNativeMethods.Gdip.GdipGetMetafileHeaderFromEmf(new HandleRef(null, henhmetafile), metafileHeader.emf);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return metafileHeader;
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00025030 File Offset: 0x00024030
		public static MetafileHeader GetMetafileHeader(string fileName)
		{
			IntSecurity.DemandReadFileIO(fileName);
			MetafileHeader metafileHeader = new MetafileHeader();
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MetafileHeaderEmf)));
			try
			{
				int num = SafeNativeMethods.Gdip.GdipGetMetafileHeaderFromFile(fileName, intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				int[] array = new int[1];
				int[] array2 = array;
				Marshal.Copy(intPtr, array2, 0, 1);
				MetafileType metafileType = (MetafileType)array2[0];
				if (metafileType == MetafileType.Wmf || metafileType == MetafileType.WmfPlaceable)
				{
					metafileHeader.wmf = (MetafileHeaderWmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderWmf));
					metafileHeader.emf = null;
				}
				else
				{
					metafileHeader.wmf = null;
					metafileHeader.emf = (MetafileHeaderEmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderEmf));
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return metafileHeader;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x000250F4 File Offset: 0x000240F4
		public static MetafileHeader GetMetafileHeader(Stream stream)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MetafileHeaderEmf)));
			MetafileHeader metafileHeader;
			try
			{
				int num = SafeNativeMethods.Gdip.GdipGetMetafileHeaderFromStream(new GPStream(stream), intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				int[] array = new int[1];
				int[] array2 = array;
				Marshal.Copy(intPtr, array2, 0, 1);
				MetafileType metafileType = (MetafileType)array2[0];
				metafileHeader = new MetafileHeader();
				if (metafileType == MetafileType.Wmf || metafileType == MetafileType.WmfPlaceable)
				{
					metafileHeader.wmf = (MetafileHeaderWmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderWmf));
					metafileHeader.emf = null;
				}
				else
				{
					metafileHeader.wmf = null;
					metafileHeader.emf = (MetafileHeaderEmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderEmf));
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return metafileHeader;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x000251B8 File Offset: 0x000241B8
		public MetafileHeader GetMetafileHeader()
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MetafileHeaderEmf)));
			MetafileHeader metafileHeader;
			try
			{
				int num = SafeNativeMethods.Gdip.GdipGetMetafileHeaderFromMetafile(new HandleRef(this, this.nativeImage), intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				int[] array = new int[1];
				int[] array2 = array;
				Marshal.Copy(intPtr, array2, 0, 1);
				MetafileType metafileType = (MetafileType)array2[0];
				metafileHeader = new MetafileHeader();
				if (metafileType == MetafileType.Wmf || metafileType == MetafileType.WmfPlaceable)
				{
					metafileHeader.wmf = (MetafileHeaderWmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderWmf));
					metafileHeader.emf = null;
				}
				else
				{
					metafileHeader.wmf = null;
					metafileHeader.emf = (MetafileHeaderEmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderEmf));
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return metafileHeader;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00025280 File Offset: 0x00024280
		public IntPtr GetHenhmetafile()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetHemfFromMetafile(new HandleRef(this, this.nativeImage), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x000252B4 File Offset: 0x000242B4
		public void PlayRecord(EmfPlusRecordType recordType, int flags, int dataSize, byte[] data)
		{
			int num = SafeNativeMethods.Gdip.GdipPlayMetafileRecord(new HandleRef(this, this.nativeImage), recordType, flags, dataSize, data);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x000252E4 File Offset: 0x000242E4
		internal static Metafile FromGDIplus(IntPtr nativeImage)
		{
			Metafile metafile = new Metafile();
			metafile.SetNativeImage(nativeImage);
			return metafile;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000252FF File Offset: 0x000242FF
		private Metafile()
		{
		}
	}
}
