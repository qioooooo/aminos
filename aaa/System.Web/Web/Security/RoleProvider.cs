using System;
using System.Configuration.Provider;
using System.Security.Permissions;

namespace System.Web.Security
{
	// Token: 0x02000328 RID: 808
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class RoleProvider : ProviderBase
	{
		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x060027AA RID: 10154
		// (set) Token: 0x060027AB RID: 10155
		public abstract string ApplicationName { get; set; }

		// Token: 0x060027AC RID: 10156
		public abstract bool IsUserInRole(string username, string roleName);

		// Token: 0x060027AD RID: 10157
		public abstract string[] GetRolesForUser(string username);

		// Token: 0x060027AE RID: 10158
		public abstract void CreateRole(string roleName);

		// Token: 0x060027AF RID: 10159
		public abstract bool DeleteRole(string roleName, bool throwOnPopulatedRole);

		// Token: 0x060027B0 RID: 10160
		public abstract bool RoleExists(string roleName);

		// Token: 0x060027B1 RID: 10161
		public abstract void AddUsersToRoles(string[] usernames, string[] roleNames);

		// Token: 0x060027B2 RID: 10162
		public abstract void RemoveUsersFromRoles(string[] usernames, string[] roleNames);

		// Token: 0x060027B3 RID: 10163
		public abstract string[] GetUsersInRole(string roleName);

		// Token: 0x060027B4 RID: 10164
		public abstract string[] GetAllRoles();

		// Token: 0x060027B5 RID: 10165
		public abstract string[] FindUsersInRole(string roleName, string usernameToMatch);
	}
}
