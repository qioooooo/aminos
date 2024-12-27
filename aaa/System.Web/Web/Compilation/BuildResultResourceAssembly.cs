using System;
using System.Reflection;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000148 RID: 328
	internal class BuildResultResourceAssembly : BuildResultCompiledAssembly
	{
		// Token: 0x06000F4E RID: 3918 RVA: 0x00044DDC File Offset: 0x00043DDC
		internal BuildResultResourceAssembly()
		{
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00044DE4 File Offset: 0x00043DE4
		internal BuildResultResourceAssembly(Assembly a)
			: base(a)
		{
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00044DED File Offset: 0x00043DED
		internal override BuildResultTypeCode GetCode()
		{
			return BuildResultTypeCode.BuildResultResourceAssembly;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00044DF4 File Offset: 0x00043DF4
		internal override string ComputeSourceDependenciesHashCode(VirtualPath virtualPath)
		{
			if (virtualPath == null)
			{
				virtualPath = base.VirtualPath;
			}
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			hashCodeCombiner.AddResourcesDirectory(virtualPath.MapPathInternal());
			return hashCodeCombiner.CombinedHashString;
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000F52 RID: 3922 RVA: 0x00044E2A File Offset: 0x00043E2A
		// (set) Token: 0x06000F53 RID: 3923 RVA: 0x00044E38 File Offset: 0x00043E38
		internal string ResourcesDependenciesHash
		{
			get
			{
				this.EnsureResourcesDependenciesHashComputed();
				return this._resourcesDependenciesHash;
			}
			set
			{
				this._resourcesDependenciesHash = value;
			}
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x00044E41 File Offset: 0x00043E41
		private void EnsureResourcesDependenciesHashComputed()
		{
			if (this._resourcesDependenciesHash != null)
			{
				return;
			}
			this._resourcesDependenciesHash = HashCodeCombiner.GetDirectoryHash(base.VirtualPath);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x00044E5D File Offset: 0x00043E5D
		internal override void GetPreservedAttributes(PreservationFileReader pfr)
		{
			base.GetPreservedAttributes(pfr);
			this.ResourcesDependenciesHash = pfr.GetAttribute("resHash");
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00044E77 File Offset: 0x00043E77
		internal override void SetPreservedAttributes(PreservationFileWriter pfw)
		{
			base.SetPreservedAttributes(pfw);
			pfw.SetAttribute("resHash", this.ResourcesDependenciesHash);
		}

		// Token: 0x040015E3 RID: 5603
		private string _resourcesDependenciesHash;
	}
}
