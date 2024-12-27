using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A9 RID: 169
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("AB1ED79F-943E-407d-A80B-0744E3A95B28")]
	[ComImport]
	internal interface IMetadataSectionEntry
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002BD RID: 701
		MetadataSectionEntry AllData { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002BE RID: 702
		uint SchemaVersion { get; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002BF RID: 703
		uint ManifestFlags { get; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002C0 RID: 704
		uint UsagePatterns { get; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002C1 RID: 705
		IDefinitionIdentity CdfIdentity { get; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002C2 RID: 706
		string LocalPath
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002C3 RID: 707
		uint HashAlgorithm { get; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002C4 RID: 708
		object ManifestHash
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002C5 RID: 709
		string ContentType
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002C6 RID: 710
		string RuntimeImageVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002C7 RID: 711
		object MvidValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002C8 RID: 712
		IDescriptionMetadataEntry DescriptionData { get; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002C9 RID: 713
		IDeploymentMetadataEntry DeploymentData { get; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002CA RID: 714
		IDependentOSMetadataEntry DependentOSData { get; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002CB RID: 715
		string defaultPermissionSetID
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002CC RID: 716
		string RequestedExecutionLevel
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060002CD RID: 717
		bool RequestedExecutionLevelUIAccess { get; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060002CE RID: 718
		IReferenceIdentity ResourceTypeResourcesDependency { get; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002CF RID: 719
		IReferenceIdentity ResourceTypeManifestResourcesDependency { get; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002D0 RID: 720
		string KeyInfoElement
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
