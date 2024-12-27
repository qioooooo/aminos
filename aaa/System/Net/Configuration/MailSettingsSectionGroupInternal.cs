using System;

namespace System.Net.Configuration
{
	// Token: 0x02000654 RID: 1620
	internal sealed class MailSettingsSectionGroupInternal
	{
		// Token: 0x0600321D RID: 12829 RVA: 0x000D5B02 File Offset: 0x000D4B02
		internal MailSettingsSectionGroupInternal()
		{
			this.smtp = SmtpSectionInternal.GetSection();
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600321E RID: 12830 RVA: 0x000D5B15 File Offset: 0x000D4B15
		internal SmtpSectionInternal Smtp
		{
			get
			{
				return this.smtp;
			}
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x000D5B1D File Offset: 0x000D4B1D
		internal static MailSettingsSectionGroupInternal GetSection()
		{
			return new MailSettingsSectionGroupInternal();
		}

		// Token: 0x04002F03 RID: 12035
		private SmtpSectionInternal smtp;
	}
}
