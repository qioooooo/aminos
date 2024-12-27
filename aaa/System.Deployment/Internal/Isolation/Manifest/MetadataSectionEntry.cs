using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B9 RID: 441
	[StructLayout(LayoutKind.Sequential)]
	internal class MetadataSectionEntry : IDisposable
	{
		// Token: 0x06000804 RID: 2052 RVA: 0x00020F48 File Offset: 0x0001FF48
		~MetadataSectionEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00020F78 File Offset: 0x0001FF78
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00020F84 File Offset: 0x0001FF84
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

		// Token: 0x04000758 RID: 1880
		public uint SchemaVersion;

		// Token: 0x04000759 RID: 1881
		public uint ManifestFlags;

		// Token: 0x0400075A RID: 1882
		public uint UsagePatterns;

		// Token: 0x0400075B RID: 1883
		public IDefinitionIdentity CdfIdentity;

		// Token: 0x0400075C RID: 1884
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LocalPath;

		// Token: 0x0400075D RID: 1885
		public uint HashAlgorithm;

		// Token: 0x0400075E RID: 1886
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ManifestHash;

		// Token: 0x0400075F RID: 1887
		public uint ManifestHashSize;

		// Token: 0x04000760 RID: 1888
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ContentType;

		// Token: 0x04000761 RID: 1889
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeImageVersion;

		// Token: 0x04000762 RID: 1890
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr MvidValue;

		// Token: 0x04000763 RID: 1891
		public uint MvidValueSize;

		// Token: 0x04000764 RID: 1892
		public DescriptionMetadataEntry DescriptionData;

		// Token: 0x04000765 RID: 1893
		public DeploymentMetadataEntry DeploymentData;

		// Token: 0x04000766 RID: 1894
		public DependentOSMetadataEntry DependentOSData;

		// Token: 0x04000767 RID: 1895
		[MarshalAs(UnmanagedType.LPWStr)]
		public string defaultPermissionSetID;

		// Token: 0x04000768 RID: 1896
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RequestedExecutionLevel;

		// Token: 0x04000769 RID: 1897
		public bool RequestedExecutionLevelUIAccess;

		// Token: 0x0400076A RID: 1898
		public IReferenceIdentity ResourceTypeResourcesDependency;

		// Token: 0x0400076B RID: 1899
		public IReferenceIdentity ResourceTypeManifestResourcesDependency;

		// Token: 0x0400076C RID: 1900
		[MarshalAs(UnmanagedType.LPWStr)]
		public string KeyInfoElement;
	}
}
