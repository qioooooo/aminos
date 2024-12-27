using System;
using System.Deployment.Internal.Isolation.Manifest;

namespace System.Deployment.Application.Manifest
{
	// Token: 0x02000017 RID: 23
	internal class DependentOS
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00005840 File Offset: 0x00004840
		public DependentOS(DependentOSMetadataEntry dependentOSMetadataEntry)
		{
			this._majorVersion = dependentOSMetadataEntry.MajorVersion;
			this._minorVersion = dependentOSMetadataEntry.MinorVersion;
			this._buildNumber = dependentOSMetadataEntry.BuildNumber;
			this._servicePackMajor = dependentOSMetadataEntry.ServicePackMajor;
			this._servicePackMinor = dependentOSMetadataEntry.ServicePackMinor;
			this._supportUrl = AssemblyManifest.UriFromMetadataEntry(dependentOSMetadataEntry.SupportUrl, "Ex_DependentOSSupportUrlNotValid");
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000058A5 File Offset: 0x000048A5
		public ushort MajorVersion
		{
			get
			{
				return this._majorVersion;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000058AD File Offset: 0x000048AD
		public ushort MinorVersion
		{
			get
			{
				return this._minorVersion;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x000058B5 File Offset: 0x000048B5
		public ushort BuildNumber
		{
			get
			{
				return this._buildNumber;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000058BD File Offset: 0x000048BD
		public byte ServicePackMajor
		{
			get
			{
				return this._servicePackMajor;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x000058C5 File Offset: 0x000048C5
		public byte ServicePackMinor
		{
			get
			{
				return this._servicePackMinor;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000058CD File Offset: 0x000048CD
		public Uri SupportUrl
		{
			get
			{
				return this._supportUrl;
			}
		}

		// Token: 0x0400006F RID: 111
		private readonly ushort _majorVersion;

		// Token: 0x04000070 RID: 112
		private readonly ushort _minorVersion;

		// Token: 0x04000071 RID: 113
		private readonly ushort _buildNumber;

		// Token: 0x04000072 RID: 114
		private readonly byte _servicePackMajor;

		// Token: 0x04000073 RID: 115
		private readonly byte _servicePackMinor;

		// Token: 0x04000074 RID: 116
		private readonly Uri _supportUrl;
	}
}
