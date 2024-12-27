using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms.Internal
{
	// Token: 0x02000021 RID: 33
	[SuppressUnmanagedCodeSecurity]
	internal static class IntSafeNativeMethods
	{
		// Token: 0x06000065 RID: 101
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateSolidBrush", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateSolidBrush(int crColor);

		// Token: 0x06000066 RID: 102 RVA: 0x00003310 File Offset: 0x00002310
		public static IntPtr CreateSolidBrush(int crColor)
		{
			return global::System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreateSolidBrush(crColor), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000067 RID: 103
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePen", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreatePen(int fnStyle, int nWidth, int crColor);

		// Token: 0x06000068 RID: 104 RVA: 0x00003330 File Offset: 0x00002330
		public static IntPtr CreatePen(int fnStyle, int nWidth, int crColor)
		{
			return global::System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreatePen(fnStyle, nWidth, crColor), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000069 RID: 105
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "ExtCreatePen", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntExtCreatePen(int fnStyle, int dwWidth, IntNativeMethods.LOGBRUSH lplb, int dwStyleCount, [MarshalAs(UnmanagedType.LPArray)] int[] lpStyle);

		// Token: 0x0600006A RID: 106 RVA: 0x00003354 File Offset: 0x00002354
		public static IntPtr ExtCreatePen(int fnStyle, int dwWidth, IntNativeMethods.LOGBRUSH lplb, int dwStyleCount, int[] lpStyle)
		{
			return global::System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntExtCreatePen(fnStyle, dwWidth, lplb, dwStyleCount, lpStyle), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600006B RID: 107
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateRectRgn", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

		// Token: 0x0600006C RID: 108 RVA: 0x00003378 File Offset: 0x00002378
		public static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
		{
			return global::System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreateRectRgn(x1, y1, x2, y2), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600006D RID: 109
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetUserDefaultLCID();

		// Token: 0x0600006E RID: 110
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool GdiFlush();

		// Token: 0x02000022 RID: 34
		public sealed class CommonHandles
		{
			// Token: 0x04000A98 RID: 2712
			public static readonly int EMF = global::System.Internal.HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);

			// Token: 0x04000A99 RID: 2713
			public static readonly int GDI = global::System.Internal.HandleCollector.RegisterType("GDI", 90, 50);

			// Token: 0x04000A9A RID: 2714
			public static readonly int HDC = global::System.Internal.HandleCollector.RegisterType("HDC", 100, 2);
		}
	}
}
