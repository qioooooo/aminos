using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BB RID: 443
	[Guid("AB1ED79F-943E-407d-A80B-0744E3A95B28")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMetadataSectionEntry
	{
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000808 RID: 2056
		MetadataSectionEntry AllData { get; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000809 RID: 2057
		uint SchemaVersion { get; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600080A RID: 2058
		uint ManifestFlags { get; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600080B RID: 2059
		uint UsagePatterns { get; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600080C RID: 2060
		IDefinitionIdentity CdfIdentity { get; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600080D RID: 2061
		string LocalPath
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600080E RID: 2062
		uint HashAlgorithm { get; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600080F RID: 2063
		object ManifestHash
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000810 RID: 2064
		string ContentType
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000811 RID: 2065
		string RuntimeImageVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000812 RID: 2066
		object MvidValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000813 RID: 2067
		IDescriptionMetadataEntry DescriptionData { get; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000814 RID: 2068
		IDeploymentMetadataEntry DeploymentData { get; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000815 RID: 2069
		IDependentOSMetadataEntry DependentOSData { get; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000816 RID: 2070
		string defaultPermissionSetID
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000817 RID: 2071
		string RequestedExecutionLevel
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000818 RID: 2072
		bool RequestedExecutionLevelUIAccess { get; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000819 RID: 2073
		IReferenceIdentity ResourceTypeResourcesDependency { get; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600081A RID: 2074
		IReferenceIdentity ResourceTypeManifestResourcesDependency { get; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600081B RID: 2075
		string KeyInfoElement
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
