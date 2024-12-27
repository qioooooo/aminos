using System;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200002C RID: 44
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class ServerType
	{
		// Token: 0x060000ED RID: 237 RVA: 0x000041C6 File Offset: 0x000031C6
		public ServerType(Type type)
		{
			this.type = type;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000041D5 File Offset: 0x000031D5
		internal Type Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000EF RID: 239 RVA: 0x000041DD File Offset: 0x000031DD
		internal Evidence Evidence
		{
			get
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Assert();
				return this.Type.Assembly.Evidence;
			}
		}

		// Token: 0x04000265 RID: 613
		private Type type;
	}
}
