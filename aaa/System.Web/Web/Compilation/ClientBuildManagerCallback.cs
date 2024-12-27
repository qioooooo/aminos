using System;
using System.CodeDom.Compiler;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x02000161 RID: 353
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ClientBuildManagerCallback : MarshalByRefObject
	{
		// Token: 0x06000FFE RID: 4094 RVA: 0x00046CBD File Offset: 0x00045CBD
		public virtual void ReportCompilerError(CompilerError error)
		{
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00046CBF File Offset: 0x00045CBF
		public virtual void ReportParseError(ParserError error)
		{
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00046CC1 File Offset: 0x00045CC1
		public virtual void ReportProgress(string message)
		{
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00046CC3 File Offset: 0x00045CC3
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
