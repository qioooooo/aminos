using System;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000448 RID: 1096
	public class InputLanguageChangedEventArgs : EventArgs
	{
		// Token: 0x06004174 RID: 16756 RVA: 0x000EAAFE File Offset: 0x000E9AFE
		public InputLanguageChangedEventArgs(CultureInfo culture, byte charSet)
		{
			this.inputLanguage = InputLanguage.FromCulture(culture);
			this.culture = culture;
			this.charSet = charSet;
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x000EAB20 File Offset: 0x000E9B20
		public InputLanguageChangedEventArgs(InputLanguage inputLanguage, byte charSet)
		{
			this.inputLanguage = inputLanguage;
			this.culture = inputLanguage.Culture;
			this.charSet = charSet;
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06004176 RID: 16758 RVA: 0x000EAB42 File Offset: 0x000E9B42
		public InputLanguage InputLanguage
		{
			get
			{
				return this.inputLanguage;
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004177 RID: 16759 RVA: 0x000EAB4A File Offset: 0x000E9B4A
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004178 RID: 16760 RVA: 0x000EAB52 File Offset: 0x000E9B52
		public byte CharSet
		{
			get
			{
				return this.charSet;
			}
		}

		// Token: 0x04001F96 RID: 8086
		private readonly InputLanguage inputLanguage;

		// Token: 0x04001F97 RID: 8087
		private readonly CultureInfo culture;

		// Token: 0x04001F98 RID: 8088
		private readonly byte charSet;
	}
}
