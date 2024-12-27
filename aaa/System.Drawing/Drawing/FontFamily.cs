using System;
using System.Drawing.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Drawing
{
	// Token: 0x0200008D RID: 141
	public sealed class FontFamily : MarshalByRefObject, IDisposable
	{
		// Token: 0x060007EE RID: 2030 RVA: 0x0001E9F0 File Offset: 0x0001D9F0
		private void SetNativeFamily(IntPtr family)
		{
			this.nativeFamily = family;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0001E9F9 File Offset: 0x0001D9F9
		internal FontFamily(IntPtr family)
		{
			this.SetNativeFamily(family);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001EA08 File Offset: 0x0001DA08
		internal FontFamily(string name, bool createDefaultOnFail)
		{
			this.createDefaultOnFail = createDefaultOnFail;
			this.CreateFontFamily(name, null);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0001EA1F File Offset: 0x0001DA1F
		public FontFamily(string name)
		{
			this.CreateFontFamily(name, null);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001EA2F File Offset: 0x0001DA2F
		public FontFamily(string name, FontCollection fontCollection)
		{
			this.CreateFontFamily(name, fontCollection);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0001EA40 File Offset: 0x0001DA40
		private void CreateFontFamily(string name, FontCollection fontCollection)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = ((fontCollection == null) ? IntPtr.Zero : fontCollection.nativeFontCollection);
			int num = SafeNativeMethods.Gdip.GdipCreateFontFamilyFromName(name, new HandleRef(fontCollection, intPtr2), out intPtr);
			if (num != 0)
			{
				if (this.createDefaultOnFail)
				{
					intPtr = FontFamily.GetGdipGenericSansSerif();
				}
				else
				{
					if (num == 14)
					{
						throw new ArgumentException(SR.GetString("GdiplusFontFamilyNotFound", new object[] { name }));
					}
					if (num == 16)
					{
						throw new ArgumentException(SR.GetString("GdiplusNotTrueTypeFont", new object[] { name }));
					}
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			this.SetNativeFamily(intPtr);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001EADC File Offset: 0x0001DADC
		public FontFamily(GenericFontFamilies genericFamily)
		{
			IntPtr zero = IntPtr.Zero;
			int num;
			switch (genericFamily)
			{
			case GenericFontFamilies.Serif:
				num = SafeNativeMethods.Gdip.GdipGetGenericFontFamilySerif(out zero);
				goto IL_003E;
			case GenericFontFamilies.SansSerif:
				num = SafeNativeMethods.Gdip.GdipGetGenericFontFamilySansSerif(out zero);
				goto IL_003E;
			}
			num = SafeNativeMethods.Gdip.GdipGetGenericFontFamilyMonospace(out zero);
			IL_003E:
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativeFamily(zero);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001EB38 File Offset: 0x0001DB38
		~FontFamily()
		{
			this.Dispose(false);
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001EB68 File Offset: 0x0001DB68
		internal IntPtr NativeFamily
		{
			get
			{
				return this.nativeFamily;
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0001EB70 File Offset: 0x0001DB70
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			FontFamily fontFamily = obj as FontFamily;
			return fontFamily != null && fontFamily.NativeFamily == this.NativeFamily;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001EBA0 File Offset: 0x0001DBA0
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "[{0}: Name={1}]", new object[]
			{
				base.GetType().Name,
				this.Name
			});
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0001EBDB File Offset: 0x0001DBDB
		public override int GetHashCode()
		{
			return this.GetName(0).GetHashCode();
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001EBE9 File Offset: 0x0001DBE9
		private static int CurrentLanguage
		{
			get
			{
				return CultureInfo.CurrentUICulture.LCID;
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0001EBF5 File Offset: 0x0001DBF5
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001EC04 File Offset: 0x0001DC04
		private void Dispose(bool disposing)
		{
			if (this.nativeFamily != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeleteFontFamily(new HandleRef(this, this.nativeFamily));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.nativeFamily = IntPtr.Zero;
				}
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0001EC70 File Offset: 0x0001DC70
		public string Name
		{
			get
			{
				return this.GetName(FontFamily.CurrentLanguage);
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001EC80 File Offset: 0x0001DC80
		public string GetName(int language)
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			int num = SafeNativeMethods.Gdip.GdipGetFamilyName(new HandleRef(this, this.NativeFamily), stringBuilder, language);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0001ECB9 File Offset: 0x0001DCB9
		public static FontFamily[] Families
		{
			get
			{
				return new InstalledFontCollection().Families;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x0001ECC5 File Offset: 0x0001DCC5
		public static FontFamily GenericSansSerif
		{
			get
			{
				return new FontFamily(FontFamily.GetGdipGenericSansSerif());
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001ECD4 File Offset: 0x0001DCD4
		private static IntPtr GetGdipGenericSansSerif()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetGenericFontFamilySansSerif(out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0001ECFA File Offset: 0x0001DCFA
		public static FontFamily GenericSerif
		{
			get
			{
				return new FontFamily(FontFamily.GetNativeGenericSerif());
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001ED08 File Offset: 0x0001DD08
		private static IntPtr GetNativeGenericSerif()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetGenericFontFamilySerif(out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0001ED2E File Offset: 0x0001DD2E
		public static FontFamily GenericMonospace
		{
			get
			{
				return new FontFamily(FontFamily.GetNativeGenericMonospace());
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001ED3C File Offset: 0x0001DD3C
		private static IntPtr GetNativeGenericMonospace()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetGenericFontFamilyMonospace(out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001ED62 File Offset: 0x0001DD62
		[Obsolete("Do not use method GetFamilies, use property Families instead")]
		public static FontFamily[] GetFamilies(Graphics graphics)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			return new InstalledFontCollection().Families;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001ED7C File Offset: 0x0001DD7C
		public bool IsStyleAvailable(FontStyle style)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsStyleAvailable(new HandleRef(this, this.NativeFamily), style, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001EDB0 File Offset: 0x0001DDB0
		public int GetEmHeight(FontStyle style)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetEmHeight(new HandleRef(this, this.NativeFamily), style, out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001EDE0 File Offset: 0x0001DDE0
		public int GetCellAscent(FontStyle style)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetCellAscent(new HandleRef(this, this.NativeFamily), style, out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001EE10 File Offset: 0x0001DE10
		public int GetCellDescent(FontStyle style)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetCellDescent(new HandleRef(this, this.NativeFamily), style, out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001EE40 File Offset: 0x0001DE40
		public int GetLineSpacing(FontStyle style)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetLineSpacing(new HandleRef(this, this.NativeFamily), style, out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return num;
		}

		// Token: 0x04000647 RID: 1607
		private const int LANG_NEUTRAL = 0;

		// Token: 0x04000648 RID: 1608
		private IntPtr nativeFamily;

		// Token: 0x04000649 RID: 1609
		private bool createDefaultOnFail;
	}
}
