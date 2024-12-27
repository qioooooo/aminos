using System;
using System.Security.Permissions;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200004F RID: 79
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class PatternMatcher
	{
		// Token: 0x060001BB RID: 443 RVA: 0x000075C8 File Offset: 0x000065C8
		public PatternMatcher(Type type)
		{
			this.matchType = MatchType.Reflect(type);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000075DC File Offset: 0x000065DC
		public object Match(string text)
		{
			return this.matchType.Match(text);
		}

		// Token: 0x040002AF RID: 687
		private MatchType matchType;
	}
}
