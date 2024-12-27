using System;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x02000060 RID: 96
	internal class CollectionName
	{
		// Token: 0x06000218 RID: 536 RVA: 0x00006424 File Offset: 0x00005424
		private static void Initialize()
		{
			if (!CollectionName._initialized)
			{
				lock (typeof(CollectionName))
				{
					if (!CollectionName._initialized)
					{
						if (Platform.IsLessThan(Platform.W2K))
						{
							CollectionName._apps = "Packages";
							CollectionName._comps = "ComponentsInPackage";
							CollectionName._interfaces = "InterfacesForComponent";
							CollectionName._meths = "MethodsForInterface";
							CollectionName._roles = "RolesInPackage";
							CollectionName._user = "UsersInRole";
						}
						else
						{
							CollectionName._apps = "Applications";
							CollectionName._comps = "Components";
							CollectionName._interfaces = "InterfacesForComponent";
							CollectionName._meths = "MethodsForInterface";
							CollectionName._roles = "Roles";
							CollectionName._user = "UsersInRole";
						}
						CollectionName._initialized = true;
					}
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00006504 File Offset: 0x00005504
		internal static string Applications
		{
			get
			{
				CollectionName.Initialize();
				return CollectionName._apps;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00006510 File Offset: 0x00005510
		internal static string Components
		{
			get
			{
				CollectionName.Initialize();
				return CollectionName._comps;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000651C File Offset: 0x0000551C
		internal static string Interfaces
		{
			get
			{
				CollectionName.Initialize();
				return CollectionName._interfaces;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00006528 File Offset: 0x00005528
		internal static string Methods
		{
			get
			{
				CollectionName.Initialize();
				return CollectionName._meths;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00006534 File Offset: 0x00005534
		internal static string Roles
		{
			get
			{
				CollectionName.Initialize();
				return CollectionName._roles;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00006540 File Offset: 0x00005540
		internal static string UsersInRole
		{
			get
			{
				CollectionName.Initialize();
				return CollectionName._user;
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000654C File Offset: 0x0000554C
		internal static string RolesFor(string target)
		{
			if (!Platform.IsLessThan(Platform.W2K))
			{
				return "RolesFor" + target;
			}
			if (target == "Component")
			{
				return "RolesForPackageComponent";
			}
			if (target == "Interface")
			{
				return "RolesForPackageComponentInterface";
			}
			return null;
		}

		// Token: 0x040000D2 RID: 210
		private static volatile bool _initialized;

		// Token: 0x040000D3 RID: 211
		private static string _apps;

		// Token: 0x040000D4 RID: 212
		private static string _comps;

		// Token: 0x040000D5 RID: 213
		private static string _interfaces;

		// Token: 0x040000D6 RID: 214
		private static string _meths;

		// Token: 0x040000D7 RID: 215
		private static string _roles;

		// Token: 0x040000D8 RID: 216
		private static string _user;
	}
}
