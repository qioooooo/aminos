using System;

namespace System.Web.Compilation
{
	// Token: 0x0200014D RID: 333
	internal class BuildResultCompiledGlobalAsaxType : BuildResultCompiledType
	{
		// Token: 0x06000F69 RID: 3945 RVA: 0x00044FFD File Offset: 0x00043FFD
		public BuildResultCompiledGlobalAsaxType()
		{
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00045005 File Offset: 0x00044005
		public BuildResultCompiledGlobalAsaxType(Type t)
			: base(t)
		{
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0004500E File Offset: 0x0004400E
		internal override BuildResultTypeCode GetCode()
		{
			return BuildResultTypeCode.BuildResultCompiledGlobalAsaxType;
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x00045011 File Offset: 0x00044011
		// (set) Token: 0x06000F6D RID: 3949 RVA: 0x00045023 File Offset: 0x00044023
		internal bool HasAppOrSessionObjects
		{
			get
			{
				return this._flags[524288];
			}
			set
			{
				this._flags[524288] = value;
			}
		}
	}
}
