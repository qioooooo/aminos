using System;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000757 RID: 1879
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public class EventLogPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003986 RID: 14726 RVA: 0x000F436B File Offset: 0x000F336B
		public EventLogPermissionAttribute(SecurityAction action)
			: base(action)
		{
			this.machineName = ".";
			this.permissionAccess = EventLogPermissionAccess.Write;
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06003987 RID: 14727 RVA: 0x000F4387 File Offset: 0x000F3387
		// (set) Token: 0x06003988 RID: 14728 RVA: 0x000F4390 File Offset: 0x000F3390
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(SR.GetString("InvalidProperty", new object[] { "MachineName", value }));
				}
				this.machineName = value;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06003989 RID: 14729 RVA: 0x000F43D0 File Offset: 0x000F33D0
		// (set) Token: 0x0600398A RID: 14730 RVA: 0x000F43D8 File Offset: 0x000F33D8
		public EventLogPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
			set
			{
				this.permissionAccess = value;
			}
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x000F43E1 File Offset: 0x000F33E1
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new EventLogPermission(PermissionState.Unrestricted);
			}
			return new EventLogPermission(this.PermissionAccess, this.MachineName);
		}

		// Token: 0x040032BF RID: 12991
		private string machineName;

		// Token: 0x040032C0 RID: 12992
		private EventLogPermissionAccess permissionAccess;
	}
}
