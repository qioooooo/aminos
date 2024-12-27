using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Internal
{
	// Token: 0x0200002D RID: 45
	[SuppressUnmanagedCodeSecurity]
	internal static class IntSafeNativeMethods
	{
		// Token: 0x060000CA RID: 202
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateSolidBrush", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateSolidBrush(int crColor);

		// Token: 0x060000CB RID: 203 RVA: 0x00004620 File Offset: 0x00003620
		public static IntPtr CreateSolidBrush(int crColor)
		{
			return global::System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreateSolidBrush(crColor), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060000CC RID: 204
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePen", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreatePen(int fnStyle, int nWidth, int crColor);

		// Token: 0x060000CD RID: 205 RVA: 0x00004640 File Offset: 0x00003640
		public static IntPtr CreatePen(int fnStyle, int nWidth, int crColor)
		{
			return global::System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreatePen(fnStyle, nWidth, crColor), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060000CE RID: 206
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "ExtCreatePen", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntExtCreatePen(int fnStyle, int dwWidth, IntNativeMethods.LOGBRUSH lplb, int dwStyleCount, [MarshalAs(UnmanagedType.LPArray)] int[] lpStyle);

		// Token: 0x060000CF RID: 207 RVA: 0x00004664 File Offset: 0x00003664
		public static IntPtr ExtCreatePen(int fnStyle, int dwWidth, IntNativeMethods.LOGBRUSH lplb, int dwStyleCount, int[] lpStyle)
		{
			return global::System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntExtCreatePen(fnStyle, dwWidth, lplb, dwStyleCount, lpStyle), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060000D0 RID: 208
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateRectRgn", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

		// Token: 0x060000D1 RID: 209 RVA: 0x00004688 File Offset: 0x00003688
		public static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
		{
			return global::System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreateRectRgn(x1, y1, x2, y2), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060000D2 RID: 210
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetUserDefaultLCID();

		// Token: 0x060000D3 RID: 211
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool GdiFlush();

		// Token: 0x0200002E RID: 46
		public sealed class CommonHandles
		{
			// Token: 0x040001BB RID: 443
			public static readonly int EMF = global::System.Internal.HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);

			// Token: 0x040001BC RID: 444
			public static readonly int GDI = global::System.Internal.HandleCollector.RegisterType("GDI", 90, 50);

			// Token: 0x040001BD RID: 445
			public static readonly int HDC = global::System.Internal.HandleCollector.RegisterType("HDC", 100, 2);
		}
	}
}
