using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000653 RID: 1619
	public sealed class MailSettingsSectionGroup : ConfigurationSectionGroup
	{
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600321C RID: 12828 RVA: 0x000D5AEB File Offset: 0x000D4AEB
		public SmtpSection Smtp
		{
			get
			{
				return (SmtpSection)base.Sections["smtp"];
			}
		}
	}
}
