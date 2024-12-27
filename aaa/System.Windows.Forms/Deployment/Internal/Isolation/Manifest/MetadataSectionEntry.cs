using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A7 RID: 167
	[StructLayout(LayoutKind.Sequential)]
	internal class MetadataSectionEntry : IDisposable
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x00007750 File Offset: 0x00006750
		~MetadataSectionEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00007780 File Offset: 0x00006780
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000778C File Offset: 0x0000678C
		public void Dispose(bool fDisposing)
		{
			if (this.ManifestHash != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.ManifestHash);
				this.ManifestHash = IntPtr.Zero;
			}
			if (this.MvidValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.MvidValue);
				this.MvidValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000CCE RID: 3278
		public uint SchemaVersion;

		// Token: 0x04000CCF RID: 3279
		public uint ManifestFlags;

		// Token: 0x04000CD0 RID: 3280
		public uint UsagePatterns;

		// Token: 0x04000CD1 RID: 3281
		public IDefinitionIdentity CdfIdentity;

		// Token: 0x04000CD2 RID: 3282
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LocalPath;

		// Token: 0x04000CD3 RID: 3283
		public uint HashAlgorithm;

		// Token: 0x04000CD4 RID: 3284
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ManifestHash;

		// Token: 0x04000CD5 RID: 3285
		public uint ManifestHashSize;

		// Token: 0x04000CD6 RID: 3286
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ContentType;

		// Token: 0x04000CD7 RID: 3287
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeImageVersion;

		// Token: 0x04000CD8 RID: 3288
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr MvidValue;

		// Token: 0x04000CD9 RID: 3289
		public uint MvidValueSize;

		// Token: 0x04000CDA RID: 3290
		public DescriptionMetadataEntry DescriptionData;

		// Token: 0x04000CDB RID: 3291
		public DeploymentMetadataEntry DeploymentData;

		// Token: 0x04000CDC RID: 3292
		public DependentOSMetadataEntry DependentOSData;

		// Token: 0x04000CDD RID: 3293
		[MarshalAs(UnmanagedType.LPWStr)]
		public string defaultPermissionSetID;

		// Token: 0x04000CDE RID: 3294
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RequestedExecutionLevel;

		// Token: 0x04000CDF RID: 3295
		public bool RequestedExecutionLevelUIAccess;

		// Token: 0x04000CE0 RID: 3296
		public IReferenceIdentity ResourceTypeResourcesDependency;

		// Token: 0x04000CE1 RID: 3297
		public IReferenceIdentity ResourceTypeManifestResourcesDependency;

		// Token: 0x04000CE2 RID: 3298
		[MarshalAs(UnmanagedType.LPWStr)]
		public string KeyInfoElement;
	}
}
