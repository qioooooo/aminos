using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Text
{
	// Token: 0x0200008C RID: 140
	public abstract class FontCollection : IDisposable
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x0001E904 File Offset: 0x0001D904
		internal FontCollection()
		{
			this.nativeFontCollection = IntPtr.Zero;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001E917 File Offset: 0x0001D917
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001E926 File Offset: 0x0001D926
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0001E928 File Offset: 0x0001D928
		public FontFamily[] Families
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetFontCollectionFamilyCount(new HandleRef(this, this.nativeFontCollection), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				IntPtr[] array = new IntPtr[num];
				int num3 = 0;
				num2 = SafeNativeMethods.Gdip.GdipGetFontCollectionFamilyList(new HandleRef(this, this.nativeFontCollection), num, array, out num3);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				FontFamily[] array2 = new FontFamily[num3];
				for (int i = 0; i < num3; i++)
				{
					IntPtr intPtr;
					SafeNativeMethods.Gdip.GdipCloneFontFamily(new HandleRef(null, array[i]), out intPtr);
					array2[i] = new FontFamily(intPtr);
				}
				return array2;
			}
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0001E9C0 File Offset: 0x0001D9C0
		~FontCollection()
		{
			this.Dispose(false);
		}

		// Token: 0x04000646 RID: 1606
		internal IntPtr nativeFontCollection;
	}
}
