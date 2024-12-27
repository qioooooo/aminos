using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x0200044A RID: 1098
	public class InputLanguageChangingEventArgs : CancelEventArgs
	{
		// Token: 0x0600417D RID: 16765 RVA: 0x000EAB5A File Offset: 0x000E9B5A
		public InputLanguageChangingEventArgs(CultureInfo culture, bool sysCharSet)
		{
			this.inputLanguage = InputLanguage.FromCulture(culture);
			this.culture = culture;
			this.sysCharSet = sysCharSet;
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x000EAB7C File Offset: 0x000E9B7C
		public InputLanguageChangingEventArgs(InputLanguage inputLanguage, bool sysCharSet)
		{
			if (inputLanguage == null)
			{
				throw new ArgumentNullException("inputLanguage");
			}
			this.inputLanguage = inputLanguage;
			this.culture = inputLanguage.Culture;
			this.sysCharSet = sysCharSet;
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x0600417F RID: 16767 RVA: 0x000EABAC File Offset: 0x000E9BAC
		public InputLanguage InputLanguage
		{
			get
			{
				return this.inputLanguage;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06004180 RID: 16768 RVA: 0x000EABB4 File Offset: 0x000E9BB4
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06004181 RID: 16769 RVA: 0x000EABBC File Offset: 0x000E9BBC
		public bool SysCharSet
		{
			get
			{
				return this.sysCharSet;
			}
		}

		// Token: 0x04001F99 RID: 8089
		private readonly InputLanguage inputLanguage;

		// Token: 0x04001F9A RID: 8090
		private readonly CultureInfo culture;

		// Token: 0x04001F9B RID: 8091
		private readonly bool sysCharSet;
	}
}
