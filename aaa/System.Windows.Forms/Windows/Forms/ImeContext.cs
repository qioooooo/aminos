using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020002AF RID: 687
	public static class ImeContext
	{
		// Token: 0x060025BB RID: 9659 RVA: 0x00058570 File Offset: 0x00057570
		public static void Disable(IntPtr handle)
		{
			if (ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable)
			{
				if (ImeContext.IsOpen(handle))
				{
					ImeContext.SetOpenStatus(false, handle);
				}
				IntPtr intPtr = UnsafeNativeMethods.ImmAssociateContext(new HandleRef(null, handle), NativeMethods.NullHandleRef);
				if (intPtr != IntPtr.Zero)
				{
					ImeContext.originalImeContext = intPtr;
				}
			}
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000585C0 File Offset: 0x000575C0
		public static void Enable(IntPtr handle)
		{
			if (ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable)
			{
				IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(null, handle));
				if (intPtr == IntPtr.Zero)
				{
					if (ImeContext.originalImeContext == IntPtr.Zero)
					{
						intPtr = UnsafeNativeMethods.ImmCreateContext();
						if (intPtr != IntPtr.Zero)
						{
							UnsafeNativeMethods.ImmAssociateContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
						}
					}
					else
					{
						UnsafeNativeMethods.ImmAssociateContext(new HandleRef(null, handle), new HandleRef(null, ImeContext.originalImeContext));
					}
				}
				else
				{
					UnsafeNativeMethods.ImmReleaseContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
				}
				if (!ImeContext.IsOpen(handle))
				{
					ImeContext.SetOpenStatus(true, handle);
				}
			}
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x00058670 File Offset: 0x00057670
		public static ImeMode GetImeMode(IntPtr handle)
		{
			IntPtr intPtr = IntPtr.Zero;
			ImeMode[] inputLanguageTable = ImeModeConversion.InputLanguageTable;
			ImeMode imeMode;
			if (inputLanguageTable == ImeModeConversion.UnsupportedTable)
			{
				imeMode = ImeMode.Inherit;
			}
			else
			{
				intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(null, handle));
				if (intPtr == IntPtr.Zero)
				{
					imeMode = ImeMode.Disable;
				}
				else if (!ImeContext.IsOpen(handle))
				{
					imeMode = inputLanguageTable[3];
				}
				else
				{
					int num = 0;
					int num2 = 0;
					UnsafeNativeMethods.ImmGetConversionStatus(new HandleRef(null, intPtr), ref num, ref num2);
					if ((num & 1) != 0)
					{
						if ((num & 2) != 0)
						{
							imeMode = (((num & 8) != 0) ? inputLanguageTable[6] : inputLanguageTable[7]);
						}
						else
						{
							imeMode = (((num & 8) != 0) ? inputLanguageTable[4] : inputLanguageTable[5]);
						}
					}
					else
					{
						imeMode = (((num & 8) != 0) ? inputLanguageTable[8] : inputLanguageTable[9]);
					}
				}
			}
			if (intPtr != IntPtr.Zero)
			{
				UnsafeNativeMethods.ImmReleaseContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
			}
			return imeMode;
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x00058734 File Offset: 0x00057734
		[Conditional("DEBUG")]
		internal static void TraceImeStatus(Control ctl)
		{
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x00058736 File Offset: 0x00057736
		[Conditional("DEBUG")]
		private static void TraceImeStatus(IntPtr handle)
		{
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x00058738 File Offset: 0x00057738
		public static bool IsOpen(IntPtr handle)
		{
			IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(null, handle));
			bool flag = false;
			if (intPtr != IntPtr.Zero)
			{
				flag = UnsafeNativeMethods.ImmGetOpenStatus(new HandleRef(null, intPtr));
				UnsafeNativeMethods.ImmReleaseContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
			}
			return flag;
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x00058784 File Offset: 0x00057784
		public static void SetImeStatus(ImeMode imeMode, IntPtr handle)
		{
			if (imeMode != ImeMode.Inherit)
			{
				if (imeMode == ImeMode.NoControl)
				{
					return;
				}
				ImeMode[] inputLanguageTable = ImeModeConversion.InputLanguageTable;
				if (inputLanguageTable == ImeModeConversion.UnsupportedTable)
				{
					return;
				}
				int num = 0;
				int num2 = 0;
				if (imeMode == ImeMode.Disable)
				{
					ImeContext.Disable(handle);
				}
				else
				{
					ImeContext.Enable(handle);
				}
				ImeMode imeMode2 = imeMode;
				switch (imeMode2)
				{
				case ImeMode.NoControl:
				case ImeMode.Disable:
					return;
				case ImeMode.On:
					imeMode = ImeMode.Hiragana;
					goto IL_0079;
				case ImeMode.Off:
					if (inputLanguageTable != ImeModeConversion.JapaneseTable)
					{
						imeMode = ImeMode.Alpha;
						goto IL_0079;
					}
					break;
				default:
					if (imeMode2 != ImeMode.Close)
					{
						goto IL_0079;
					}
					break;
				}
				if (inputLanguageTable != ImeModeConversion.KoreanTable)
				{
					ImeContext.SetOpenStatus(false, handle);
					return;
				}
				imeMode = ImeMode.Alpha;
				IL_0079:
				if (ImeModeConversion.ImeModeConversionBits.ContainsKey(imeMode))
				{
					ImeModeConversion imeModeConversion = ImeModeConversion.ImeModeConversionBits[imeMode];
					IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(null, handle));
					UnsafeNativeMethods.ImmGetConversionStatus(new HandleRef(null, intPtr), ref num, ref num2);
					num |= imeModeConversion.setBits;
					num &= ~imeModeConversion.clearBits;
					UnsafeNativeMethods.ImmSetConversionStatus(new HandleRef(null, intPtr), num, num2);
					UnsafeNativeMethods.ImmReleaseContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
				}
			}
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x00058880 File Offset: 0x00057880
		public static void SetOpenStatus(bool open, IntPtr handle)
		{
			if (ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable)
			{
				IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(null, handle));
				if (intPtr != IntPtr.Zero)
				{
					bool flag = UnsafeNativeMethods.ImmSetOpenStatus(new HandleRef(null, intPtr), open);
					if (flag)
					{
						flag = UnsafeNativeMethods.ImmReleaseContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
					}
				}
			}
		}

		// Token: 0x040015F4 RID: 5620
		private static IntPtr originalImeContext;
	}
}
