using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CE RID: 462
	[Guid("AB1ED79F-943E-407d-A80B-0744E3A95B28")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMetadataSectionEntry
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060014A2 RID: 5282
		MetadataSectionEntry AllData { get; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060014A3 RID: 5283
		uint SchemaVersion { get; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060014A4 RID: 5284
		uint ManifestFlags { get; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060014A5 RID: 5285
		uint UsagePatterns { get; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060014A6 RID: 5286
		IDefinitionIdentity CdfIdentity { get; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060014A7 RID: 5287
		string LocalPath
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060014A8 RID: 5288
		uint HashAlgorithm { get; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060014A9 RID: 5289
		object ManifestHash
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060014AA RID: 5290
		string ContentType
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060014AB RID: 5291
		string RuntimeImageVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060014AC RID: 5292
		object MvidValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060014AD RID: 5293
		IDescriptionMetadataEntry DescriptionData { get; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060014AE RID: 5294
		IDeploymentMetadataEntry DeploymentData { get; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060014AF RID: 5295
		IDependentOSMetadataEntry DependentOSData { get; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060014B0 RID: 5296
		string defaultPermissionSetID
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060014B1 RID: 5297
		string RequestedExecutionLevel
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060014B2 RID: 5298
		bool RequestedExecutionLevelUIAccess { get; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060014B3 RID: 5299
		IReferenceIdentity ResourceTypeResourcesDependency { get; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060014B4 RID: 5300
		IReferenceIdentity ResourceTypeManifestResourcesDependency { get; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060014B5 RID: 5301
		string KeyInfoElement
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
