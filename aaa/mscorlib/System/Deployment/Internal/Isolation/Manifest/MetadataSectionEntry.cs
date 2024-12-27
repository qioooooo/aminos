using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CC RID: 460
	[StructLayout(LayoutKind.Sequential)]
	internal class MetadataSectionEntry : IDisposable
	{
		// Token: 0x0600149E RID: 5278 RVA: 0x00036608 File Offset: 0x00035608
		~MetadataSectionEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00036638 File Offset: 0x00035638
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00036644 File Offset: 0x00035644
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

		// Token: 0x040007C8 RID: 1992
		public uint SchemaVersion;

		// Token: 0x040007C9 RID: 1993
		public uint ManifestFlags;

		// Token: 0x040007CA RID: 1994
		public uint UsagePatterns;

		// Token: 0x040007CB RID: 1995
		public IDefinitionIdentity CdfIdentity;

		// Token: 0x040007CC RID: 1996
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LocalPath;

		// Token: 0x040007CD RID: 1997
		public uint HashAlgorithm;

		// Token: 0x040007CE RID: 1998
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ManifestHash;

		// Token: 0x040007CF RID: 1999
		public uint ManifestHashSize;

		// Token: 0x040007D0 RID: 2000
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ContentType;

		// Token: 0x040007D1 RID: 2001
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeImageVersion;

		// Token: 0x040007D2 RID: 2002
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr MvidValue;

		// Token: 0x040007D3 RID: 2003
		public uint MvidValueSize;

		// Token: 0x040007D4 RID: 2004
		public DescriptionMetadataEntry DescriptionData;

		// Token: 0x040007D5 RID: 2005
		public DeploymentMetadataEntry DeploymentData;

		// Token: 0x040007D6 RID: 2006
		public DependentOSMetadataEntry DependentOSData;

		// Token: 0x040007D7 RID: 2007
		[MarshalAs(UnmanagedType.LPWStr)]
		public string defaultPermissionSetID;

		// Token: 0x040007D8 RID: 2008
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RequestedExecutionLevel;

		// Token: 0x040007D9 RID: 2009
		public bool RequestedExecutionLevelUIAccess;

		// Token: 0x040007DA RID: 2010
		public IReferenceIdentity ResourceTypeResourcesDependency;

		// Token: 0x040007DB RID: 2011
		public IReferenceIdentity ResourceTypeManifestResourcesDependency;

		// Token: 0x040007DC RID: 2012
		[MarshalAs(UnmanagedType.LPWStr)]
		public string KeyInfoElement;
	}
}
