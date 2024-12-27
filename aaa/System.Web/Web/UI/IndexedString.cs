using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200040E RID: 1038
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class IndexedString
	{
		// Token: 0x060032AA RID: 12970 RVA: 0x000DD66B File Offset: 0x000DC66B
		public IndexedString(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				throw new ArgumentNullException("s");
			}
			this._value = s;
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x000DD68D File Offset: 0x000DC68D
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x040023D1 RID: 9169
		private string _value;
	}
}
