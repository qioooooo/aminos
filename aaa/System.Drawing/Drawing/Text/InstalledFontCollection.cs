using System;

namespace System.Drawing.Text
{
	// Token: 0x020000C3 RID: 195
	public sealed class InstalledFontCollection : FontCollection
	{
		// Token: 0x06000BD2 RID: 3026 RVA: 0x000233B4 File Offset: 0x000223B4
		public InstalledFontCollection()
		{
			this.nativeFontCollection = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipNewInstalledFontCollection(out this.nativeFontCollection);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}
	}
}
