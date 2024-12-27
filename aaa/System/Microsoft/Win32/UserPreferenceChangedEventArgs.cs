using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x020002A9 RID: 681
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class UserPreferenceChangedEventArgs : EventArgs
	{
		// Token: 0x0600168D RID: 5773 RVA: 0x00048245 File Offset: 0x00047245
		public UserPreferenceChangedEventArgs(UserPreferenceCategory category)
		{
			this.category = category;
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600168E RID: 5774 RVA: 0x00048254 File Offset: 0x00047254
		public UserPreferenceCategory Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x040015FE RID: 5630
		private readonly UserPreferenceCategory category;
	}
}
