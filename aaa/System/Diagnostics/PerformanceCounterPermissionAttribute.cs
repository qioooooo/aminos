using System;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000770 RID: 1904
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public class PerformanceCounterPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003A8E RID: 14990 RVA: 0x000F934F File Offset: 0x000F834F
		public PerformanceCounterPermissionAttribute(SecurityAction action)
			: base(action)
		{
			this.categoryName = "*";
			this.machineName = ".";
			this.permissionAccess = PerformanceCounterPermissionAccess.Write;
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06003A8F RID: 14991 RVA: 0x000F9375 File Offset: 0x000F8375
		// (set) Token: 0x06003A90 RID: 14992 RVA: 0x000F937D File Offset: 0x000F837D
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.categoryName = value;
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06003A91 RID: 14993 RVA: 0x000F9394 File Offset: 0x000F8394
		// (set) Token: 0x06003A92 RID: 14994 RVA: 0x000F939C File Offset: 0x000F839C
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

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06003A93 RID: 14995 RVA: 0x000F93DC File Offset: 0x000F83DC
		// (set) Token: 0x06003A94 RID: 14996 RVA: 0x000F93E4 File Offset: 0x000F83E4
		public PerformanceCounterPermissionAccess PermissionAccess
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

		// Token: 0x06003A95 RID: 14997 RVA: 0x000F93ED File Offset: 0x000F83ED
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new PerformanceCounterPermission(PermissionState.Unrestricted);
			}
			return new PerformanceCounterPermission(this.PermissionAccess, this.MachineName, this.CategoryName);
		}

		// Token: 0x0400334B RID: 13131
		private string categoryName;

		// Token: 0x0400334C RID: 13132
		private string machineName;

		// Token: 0x0400334D RID: 13133
		private PerformanceCounterPermissionAccess permissionAccess;
	}
}
