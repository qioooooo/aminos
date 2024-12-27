using System;
using System.ComponentModel;
using System.Globalization;
using System.Security;
using System.Security.Permissions;

namespace System.ServiceProcess
{
	// Token: 0x0200002A RID: 42
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public class ServiceControllerPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x00004D43 File Offset: 0x00003D43
		public ServiceControllerPermissionAttribute(SecurityAction action)
			: base(action)
		{
			this.machineName = ".";
			this.serviceName = "*";
			this.permissionAccess = ServiceControllerPermissionAccess.Browse;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004D69 File Offset: 0x00003D69
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00004D74 File Offset: 0x00003D74
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
					throw new ArgumentException(Res.GetString("BadMachineName", new object[] { value }));
				}
				this.machineName = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004DAC File Offset: 0x00003DAC
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00004DB4 File Offset: 0x00003DB4
		public ServiceControllerPermissionAccess PermissionAccess
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

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004DBD File Offset: 0x00003DBD
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00004DC8 File Offset: 0x00003DC8
		public string ServiceName
		{
			get
			{
				return this.serviceName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!ServiceController.ValidServiceName(value))
				{
					throw new ArgumentException(Res.GetString("ServiceName", new object[]
					{
						value,
						80.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.serviceName = value;
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004E20 File Offset: 0x00003E20
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new ServiceControllerPermission(PermissionState.Unrestricted);
			}
			return new ServiceControllerPermission(this.PermissionAccess, this.MachineName, this.ServiceName);
		}

		// Token: 0x04000207 RID: 519
		private string machineName;

		// Token: 0x04000208 RID: 520
		private string serviceName;

		// Token: 0x04000209 RID: 521
		private ServiceControllerPermissionAccess permissionAccess;
	}
}
