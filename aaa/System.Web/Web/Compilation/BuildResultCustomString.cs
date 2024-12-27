using System;
using System.Reflection;

namespace System.Web.Compilation
{
	// Token: 0x02000146 RID: 326
	internal class BuildResultCustomString : BuildResultCompiledAssembly
	{
		// Token: 0x06000F40 RID: 3904 RVA: 0x00044C05 File Offset: 0x00043C05
		internal BuildResultCustomString()
		{
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00044C0D File Offset: 0x00043C0D
		internal BuildResultCustomString(Assembly a, string customString)
			: base(a)
		{
			this._customString = customString;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x00044C1D File Offset: 0x00043C1D
		internal override BuildResultTypeCode GetCode()
		{
			return BuildResultTypeCode.BuildResultCustomString;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00044C20 File Offset: 0x00043C20
		internal override void GetPreservedAttributes(PreservationFileReader pfr)
		{
			base.GetPreservedAttributes(pfr);
			this._customString = pfr.GetAttribute("customString");
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00044C3A File Offset: 0x00043C3A
		internal override void SetPreservedAttributes(PreservationFileWriter pfw)
		{
			base.SetPreservedAttributes(pfw);
			pfw.SetAttribute("customString", this._customString);
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x00044C54 File Offset: 0x00043C54
		internal string CustomString
		{
			get
			{
				return this._customString;
			}
		}

		// Token: 0x040015E0 RID: 5600
		private string _customString;
	}
}
