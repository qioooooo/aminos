using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.DirectoryServices.Interop
{
	// Token: 0x0200005C RID: 92
	[ComVisible(false)]
	[SuppressUnmanagedCodeSecurity]
	internal class SafeNativeMethods
	{
		// Token: 0x06000208 RID: 520
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void VariantClear(IntPtr pObject);

		// Token: 0x06000209 RID: 521
		[DllImport("oleaut32.dll")]
		public static extern void VariantInit(IntPtr pObject);

		// Token: 0x0600020A RID: 522
		[DllImport("activeds.dll")]
		public static extern bool FreeADsMem(IntPtr pVoid);

		// Token: 0x0600020B RID: 523
		[DllImport("activeds.dll", CharSet = CharSet.Unicode)]
		public static extern int ADsGetLastError(out int error, StringBuilder errorBuffer, int errorBufferLength, StringBuilder nameBuffer, int nameBufferLength);

		// Token: 0x0600020C RID: 524
		[DllImport("activeds.dll", CharSet = CharSet.Unicode)]
		public static extern int ADsSetLastError(int error, string errorString, string provider);

		// Token: 0x0600020D RID: 525
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		public static extern int FormatMessageW(int dwFlags, int lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, int arguments);

		// Token: 0x04000287 RID: 647
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x04000288 RID: 648
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04000289 RID: 649
		public const int FORMAT_MESSAGE_FROM_STRING = 1024;

		// Token: 0x0400028A RID: 650
		public const int FORMAT_MESSAGE_FROM_HMODULE = 2048;

		// Token: 0x0400028B RID: 651
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x0400028C RID: 652
		public const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x0400028D RID: 653
		public const int FORMAT_MESSAGE_MAX_WIDTH_MASK = 255;

		// Token: 0x0400028E RID: 654
		public const int ERROR_MORE_DATA = 234;

		// Token: 0x0400028F RID: 655
		public const int ERROR_SUCCESS = 0;

		// Token: 0x0200005D RID: 93
		[ComVisible(false)]
		public class EnumVariant
		{
			// Token: 0x0600020F RID: 527 RVA: 0x0000872F File Offset: 0x0000772F
			public EnumVariant(SafeNativeMethods.IEnumVariant en)
			{
				if (en == null)
				{
					throw new ArgumentNullException("en");
				}
				this.enumerator = en;
			}

			// Token: 0x06000210 RID: 528 RVA: 0x00008757 File Offset: 0x00007757
			public bool GetNext()
			{
				this.Advance();
				return this.currentValue != SafeNativeMethods.EnumVariant.NoMoreValues;
			}

			// Token: 0x06000211 RID: 529 RVA: 0x0000876F File Offset: 0x0000776F
			public object GetValue()
			{
				if (this.currentValue == SafeNativeMethods.EnumVariant.NoMoreValues)
				{
					throw new InvalidOperationException(Res.GetString("DSEnumerator"));
				}
				return this.currentValue;
			}

			// Token: 0x06000212 RID: 530 RVA: 0x00008794 File Offset: 0x00007794
			public void Reset()
			{
				this.enumerator.Reset();
				this.currentValue = SafeNativeMethods.EnumVariant.NoMoreValues;
			}

			// Token: 0x06000213 RID: 531 RVA: 0x000087AC File Offset: 0x000077AC
			private void Advance()
			{
				this.currentValue = SafeNativeMethods.EnumVariant.NoMoreValues;
				IntPtr intPtr = Marshal.AllocCoTaskMem(16);
				try
				{
					int[] array = new int[1];
					int[] array2 = array;
					SafeNativeMethods.VariantInit(intPtr);
					this.enumerator.Next(1, intPtr, array2);
					try
					{
						if (array2[0] > 0)
						{
							this.currentValue = Marshal.GetObjectForNativeVariant(intPtr);
						}
					}
					finally
					{
						SafeNativeMethods.VariantClear(intPtr);
					}
				}
				finally
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}

			// Token: 0x04000290 RID: 656
			private static readonly object NoMoreValues = new object();

			// Token: 0x04000291 RID: 657
			private object currentValue = SafeNativeMethods.EnumVariant.NoMoreValues;

			// Token: 0x04000292 RID: 658
			private SafeNativeMethods.IEnumVariant enumerator;
		}

		// Token: 0x0200005E RID: 94
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00020404-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IEnumVariant
		{
			// Token: 0x06000215 RID: 533
			[SuppressUnmanagedCodeSecurity]
			void Next([MarshalAs(UnmanagedType.U4)] [In] int celt, [In] [Out] IntPtr rgvar, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

			// Token: 0x06000216 RID: 534
			[SuppressUnmanagedCodeSecurity]
			void Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			// Token: 0x06000217 RID: 535
			[SuppressUnmanagedCodeSecurity]
			void Reset();

			// Token: 0x06000218 RID: 536
			[SuppressUnmanagedCodeSecurity]
			void Clone([MarshalAs(UnmanagedType.LPArray)] [Out] SafeNativeMethods.IEnumVariant[] ppenum);
		}
	}
}
