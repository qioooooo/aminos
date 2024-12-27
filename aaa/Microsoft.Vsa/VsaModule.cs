using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x02000016 RID: 22
	[AttributeUsage(AttributeTargets.All)]
	[Guid("7f64f934-c1cc-338e-b695-f64d71e820fe")]
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	public class VsaModule : Attribute
	{
		// Token: 0x06000093 RID: 147 RVA: 0x0000286D File Offset: 0x0000186D
		public VsaModule(bool bIsVsaModule)
		{
			this.IsVsaModule = bIsVsaModule;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000287C File Offset: 0x0000187C
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00002884 File Offset: 0x00001884
		public bool IsVsaModule
		{
			get
			{
				return this.isVsaModule;
			}
			set
			{
				this.isVsaModule = value;
			}
		}

		// Token: 0x0400006A RID: 106
		private bool isVsaModule;
	}
}
