using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000061 RID: 97
	[Guid("a504e5b0-8ccf-4cb4-9902-c9d1b9abd033")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICMS
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000203 RID: 515
		IDefinitionIdentity Identity { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000204 RID: 516
		ISection FileSection { get; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000205 RID: 517
		ISection CategoryMembershipSection { get; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000206 RID: 518
		ISection COMRedirectionSection { get; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000207 RID: 519
		ISection ProgIdRedirectionSection { get; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000208 RID: 520
		ISection CLRSurrogateSection { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000209 RID: 521
		ISection AssemblyReferenceSection { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600020A RID: 522
		ISection WindowClassSection { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600020B RID: 523
		ISection StringSection { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600020C RID: 524
		ISection EntryPointSection { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600020D RID: 525
		ISection PermissionSetSection { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600020E RID: 526
		ISectionEntry MetadataSectionEntry { get; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600020F RID: 527
		ISection AssemblyRequestSection { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000210 RID: 528
		ISection RegistryKeySection { get; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000211 RID: 529
		ISection DirectorySection { get; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000212 RID: 530
		ISection FileAssociationSection { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000213 RID: 531
		ISection EventSection { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000214 RID: 532
		ISection EventMapSection { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000215 RID: 533
		ISection EventTagSection { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000216 RID: 534
		ISection CounterSetSection { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000217 RID: 535
		ISection CounterSection { get; }
	}
}
