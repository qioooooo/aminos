using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000173 RID: 371
	[Guid("a504e5b0-8ccf-4cb4-9902-c9d1b9abd033")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICMS
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600074E RID: 1870
		IDefinitionIdentity Identity { get; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600074F RID: 1871
		ISection FileSection { get; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000750 RID: 1872
		ISection CategoryMembershipSection { get; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000751 RID: 1873
		ISection COMRedirectionSection { get; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000752 RID: 1874
		ISection ProgIdRedirectionSection { get; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000753 RID: 1875
		ISection CLRSurrogateSection { get; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000754 RID: 1876
		ISection AssemblyReferenceSection { get; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000755 RID: 1877
		ISection WindowClassSection { get; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000756 RID: 1878
		ISection StringSection { get; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000757 RID: 1879
		ISection EntryPointSection { get; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000758 RID: 1880
		ISection PermissionSetSection { get; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000759 RID: 1881
		ISectionEntry MetadataSectionEntry { get; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600075A RID: 1882
		ISection AssemblyRequestSection { get; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600075B RID: 1883
		ISection RegistryKeySection { get; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600075C RID: 1884
		ISection DirectorySection { get; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600075D RID: 1885
		ISection FileAssociationSection { get; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600075E RID: 1886
		ISection EventSection { get; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600075F RID: 1887
		ISection EventMapSection { get; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000760 RID: 1888
		ISection EventTagSection { get; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000761 RID: 1889
		ISection CounterSetSection { get; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000762 RID: 1890
		ISection CounterSection { get; }
	}
}
