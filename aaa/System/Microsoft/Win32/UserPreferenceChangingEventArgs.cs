using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x020002AB RID: 683
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class UserPreferenceChangingEventArgs : EventArgs
	{
		// Token: 0x06001693 RID: 5779 RVA: 0x0004825C File Offset: 0x0004725C
		public UserPreferenceChangingEventArgs(UserPreferenceCategory category)
		{
			this.category = category;
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001694 RID: 5780 RVA: 0x0004826B File Offset: 0x0004726B
		public UserPreferenceCategory Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x040015FF RID: 5631
		private readonly UserPreferenceCategory category;
	}
}
