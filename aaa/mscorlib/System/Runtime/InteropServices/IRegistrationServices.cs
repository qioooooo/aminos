using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000510 RID: 1296
	[Guid("CCBD682C-73A5-4568-B8B0-C7007E11ABA2")]
	[ComVisible(true)]
	public interface IRegistrationServices
	{
		// Token: 0x06003298 RID: 12952
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		bool RegisterAssembly(Assembly assembly, AssemblyRegistrationFlags flags);

		// Token: 0x06003299 RID: 12953
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		bool UnregisterAssembly(Assembly assembly);

		// Token: 0x0600329A RID: 12954
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		Type[] GetRegistrableTypesInAssembly(Assembly assembly);

		// Token: 0x0600329B RID: 12955
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		string GetProgIdForType(Type type);

		// Token: 0x0600329C RID: 12956
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void RegisterTypeForComClients(Type type, ref Guid g);

		// Token: 0x0600329D RID: 12957
		Guid GetManagedCategoryGuid();

		// Token: 0x0600329E RID: 12958
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		bool TypeRequiresRegistration(Type type);

		// Token: 0x0600329F RID: 12959
		bool TypeRepresentsComType(Type type);
	}
}
