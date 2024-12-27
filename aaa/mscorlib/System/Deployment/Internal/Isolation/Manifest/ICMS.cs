using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000186 RID: 390
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a504e5b0-8ccf-4cb4-9902-c9d1b9abd033")]
	[ComImport]
	internal interface ICMS
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060013E8 RID: 5096
		IDefinitionIdentity Identity { get; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060013E9 RID: 5097
		ISection FileSection { get; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060013EA RID: 5098
		ISection CategoryMembershipSection { get; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060013EB RID: 5099
		ISection COMRedirectionSection { get; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060013EC RID: 5100
		ISection ProgIdRedirectionSection { get; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060013ED RID: 5101
		ISection CLRSurrogateSection { get; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060013EE RID: 5102
		ISection AssemblyReferenceSection { get; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060013EF RID: 5103
		ISection WindowClassSection { get; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060013F0 RID: 5104
		ISection StringSection { get; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060013F1 RID: 5105
		ISection EntryPointSection { get; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060013F2 RID: 5106
		ISection PermissionSetSection { get; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060013F3 RID: 5107
		ISectionEntry MetadataSectionEntry { get; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060013F4 RID: 5108
		ISection AssemblyRequestSection { get; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060013F5 RID: 5109
		ISection RegistryKeySection { get; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060013F6 RID: 5110
		ISection DirectorySection { get; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060013F7 RID: 5111
		ISection FileAssociationSection { get; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060013F8 RID: 5112
		ISection EventSection { get; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060013F9 RID: 5113
		ISection EventMapSection { get; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060013FA RID: 5114
		ISection EventTagSection { get; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060013FB RID: 5115
		ISection CounterSetSection { get; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060013FC RID: 5116
		ISection CounterSection { get; }
	}
}
