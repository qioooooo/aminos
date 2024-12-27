using System;
using System.Reflection;

namespace System.Web.Compilation
{
	// Token: 0x02000145 RID: 325
	internal class BuildResultCompiledAssembly : BuildResultCompiledAssemblyBase
	{
		// Token: 0x06000F3A RID: 3898 RVA: 0x00044BC5 File Offset: 0x00043BC5
		internal BuildResultCompiledAssembly()
		{
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00044BCD File Offset: 0x00043BCD
		internal BuildResultCompiledAssembly(Assembly a)
		{
			this._assembly = a;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00044BDC File Offset: 0x00043BDC
		internal override BuildResultTypeCode GetCode()
		{
			return BuildResultTypeCode.BuildResultCompiledAssembly;
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x00044BDF File Offset: 0x00043BDF
		// (set) Token: 0x06000F3E RID: 3902 RVA: 0x00044BE7 File Offset: 0x00043BE7
		internal override Assembly ResultAssembly
		{
			get
			{
				return this._assembly;
			}
			set
			{
				this._assembly = value;
			}
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00044BF0 File Offset: 0x00043BF0
		internal override void GetPreservedAttributes(PreservationFileReader pfr)
		{
			base.GetPreservedAttributes(pfr);
			this.ResultAssembly = BuildResultCompiledAssemblyBase.GetPreservedAssembly(pfr);
		}

		// Token: 0x040015DF RID: 5599
		private Assembly _assembly;
	}
}
