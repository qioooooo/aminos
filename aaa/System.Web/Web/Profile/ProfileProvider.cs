using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x02000311 RID: 785
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class ProfileProvider : SettingsProvider
	{
		// Token: 0x060026A1 RID: 9889
		public abstract int DeleteProfiles(ProfileInfoCollection profiles);

		// Token: 0x060026A2 RID: 9890
		public abstract int DeleteProfiles(string[] usernames);

		// Token: 0x060026A3 RID: 9891
		public abstract int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate);

		// Token: 0x060026A4 RID: 9892
		public abstract int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate);

		// Token: 0x060026A5 RID: 9893
		public abstract ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords);

		// Token: 0x060026A6 RID: 9894
		public abstract ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords);

		// Token: 0x060026A7 RID: 9895
		public abstract ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);

		// Token: 0x060026A8 RID: 9896
		public abstract ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords);
	}
}
