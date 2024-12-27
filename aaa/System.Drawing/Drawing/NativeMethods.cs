using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x0200004F RID: 79
	internal class NativeMethods
	{
		// Token: 0x0400038A RID: 906
		public const byte PC_NOCOLLAPSE = 4;

		// Token: 0x0400038B RID: 907
		public const int MAX_PATH = 260;

		// Token: 0x0400038C RID: 908
		internal const int SM_REMOTESESSION = 4096;

		// Token: 0x0400038D RID: 909
		internal const int OBJ_DC = 3;

		// Token: 0x0400038E RID: 910
		internal const int OBJ_METADC = 4;

		// Token: 0x0400038F RID: 911
		internal const int OBJ_MEMDC = 10;

		// Token: 0x04000390 RID: 912
		internal const int OBJ_ENHMETADC = 12;

		// Token: 0x04000391 RID: 913
		internal const int DIB_RGB_COLORS = 0;

		// Token: 0x04000392 RID: 914
		internal const int BI_BITFIELDS = 3;

		// Token: 0x04000393 RID: 915
		internal const int BI_RGB = 0;

		// Token: 0x04000394 RID: 916
		internal const int BITMAPINFO_MAX_COLORSIZE = 256;

		// Token: 0x04000395 RID: 917
		internal const int SPI_GETICONTITLELOGFONT = 31;

		// Token: 0x04000396 RID: 918
		internal const int SPI_GETNONCLIENTMETRICS = 41;

		// Token: 0x04000397 RID: 919
		internal const int DEFAULT_GUI_FONT = 17;

		// Token: 0x04000398 RID: 920
		internal static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

		// Token: 0x02000050 RID: 80
		public enum RegionFlags
		{
			// Token: 0x0400039A RID: 922
			ERROR,
			// Token: 0x0400039B RID: 923
			NULLREGION,
			// Token: 0x0400039C RID: 924
			SIMPLEREGION,
			// Token: 0x0400039D RID: 925
			COMPLEXREGION
		}

		// Token: 0x02000051 RID: 81
		internal struct BITMAPINFO_FLAT
		{
			// Token: 0x0400039E RID: 926
			public int bmiHeader_biSize;

			// Token: 0x0400039F RID: 927
			public int bmiHeader_biWidth;

			// Token: 0x040003A0 RID: 928
			public int bmiHeader_biHeight;

			// Token: 0x040003A1 RID: 929
			public short bmiHeader_biPlanes;

			// Token: 0x040003A2 RID: 930
			public short bmiHeader_biBitCount;

			// Token: 0x040003A3 RID: 931
			public int bmiHeader_biCompression;

			// Token: 0x040003A4 RID: 932
			public int bmiHeader_biSizeImage;

			// Token: 0x040003A5 RID: 933
			public int bmiHeader_biXPelsPerMeter;

			// Token: 0x040003A6 RID: 934
			public int bmiHeader_biYPelsPerMeter;

			// Token: 0x040003A7 RID: 935
			public int bmiHeader_biClrUsed;

			// Token: 0x040003A8 RID: 936
			public int bmiHeader_biClrImportant;

			// Token: 0x040003A9 RID: 937
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
			public byte[] bmiColors;
		}

		// Token: 0x02000052 RID: 82
		[StructLayout(LayoutKind.Sequential)]
		internal class BITMAPINFOHEADER
		{
			// Token: 0x040003AA RID: 938
			public int biSize = 40;

			// Token: 0x040003AB RID: 939
			public int biWidth;

			// Token: 0x040003AC RID: 940
			public int biHeight;

			// Token: 0x040003AD RID: 941
			public short biPlanes;

			// Token: 0x040003AE RID: 942
			public short biBitCount;

			// Token: 0x040003AF RID: 943
			public int biCompression;

			// Token: 0x040003B0 RID: 944
			public int biSizeImage;

			// Token: 0x040003B1 RID: 945
			public int biXPelsPerMeter;

			// Token: 0x040003B2 RID: 946
			public int biYPelsPerMeter;

			// Token: 0x040003B3 RID: 947
			public int biClrUsed;

			// Token: 0x040003B4 RID: 948
			public int biClrImportant;
		}

		// Token: 0x02000053 RID: 83
		internal struct PALETTEENTRY
		{
			// Token: 0x040003B5 RID: 949
			public byte peRed;

			// Token: 0x040003B6 RID: 950
			public byte peGreen;

			// Token: 0x040003B7 RID: 951
			public byte peBlue;

			// Token: 0x040003B8 RID: 952
			public byte peFlags;
		}

		// Token: 0x02000054 RID: 84
		internal struct RGBQUAD
		{
			// Token: 0x040003B9 RID: 953
			public byte rgbBlue;

			// Token: 0x040003BA RID: 954
			public byte rgbGreen;

			// Token: 0x040003BB RID: 955
			public byte rgbRed;

			// Token: 0x040003BC RID: 956
			public byte rgbReserved;
		}

		// Token: 0x02000055 RID: 85
		[StructLayout(LayoutKind.Sequential)]
		internal class NONCLIENTMETRICS
		{
			// Token: 0x040003BD RID: 957
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.NONCLIENTMETRICS));

			// Token: 0x040003BE RID: 958
			public int iBorderWidth;

			// Token: 0x040003BF RID: 959
			public int iScrollWidth;

			// Token: 0x040003C0 RID: 960
			public int iScrollHeight;

			// Token: 0x040003C1 RID: 961
			public int iCaptionWidth;

			// Token: 0x040003C2 RID: 962
			public int iCaptionHeight;

			// Token: 0x040003C3 RID: 963
			[MarshalAs(UnmanagedType.Struct)]
			public SafeNativeMethods.LOGFONT lfCaptionFont;

			// Token: 0x040003C4 RID: 964
			public int iSmCaptionWidth;

			// Token: 0x040003C5 RID: 965
			public int iSmCaptionHeight;

			// Token: 0x040003C6 RID: 966
			[MarshalAs(UnmanagedType.Struct)]
			public SafeNativeMethods.LOGFONT lfSmCaptionFont;

			// Token: 0x040003C7 RID: 967
			public int iMenuWidth;

			// Token: 0x040003C8 RID: 968
			public int iMenuHeight;

			// Token: 0x040003C9 RID: 969
			[MarshalAs(UnmanagedType.Struct)]
			public SafeNativeMethods.LOGFONT lfMenuFont;

			// Token: 0x040003CA RID: 970
			[MarshalAs(UnmanagedType.Struct)]
			public SafeNativeMethods.LOGFONT lfStatusFont;

			// Token: 0x040003CB RID: 971
			[MarshalAs(UnmanagedType.Struct)]
			public SafeNativeMethods.LOGFONT lfMessageFont;
		}
	}
}
