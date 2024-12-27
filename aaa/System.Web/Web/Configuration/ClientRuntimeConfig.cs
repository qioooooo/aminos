using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001BB RID: 443
	internal class ClientRuntimeConfig : RuntimeConfig
	{
		// Token: 0x0600197E RID: 6526 RVA: 0x00079108 File Offset: 0x00078108
		internal ClientRuntimeConfig()
			: base(null, false)
		{
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00079112 File Offset: 0x00078112
		[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
		protected override object GetSectionObject(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
