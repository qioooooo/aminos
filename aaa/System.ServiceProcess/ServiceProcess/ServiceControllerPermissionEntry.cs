using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.ServiceProcess
{
	// Token: 0x0200002B RID: 43
	[Serializable]
	public class ServiceControllerPermissionEntry
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00004E48 File Offset: 0x00003E48
		public ServiceControllerPermissionEntry()
		{
			this.machineName = ".";
			this.serviceName = "*";
			this.permissionAccess = ServiceControllerPermissionAccess.Browse;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004E70 File Offset: 0x00003E70
		public ServiceControllerPermissionEntry(ServiceControllerPermissionAccess permissionAccess, string machineName, string serviceName)
		{
			if (serviceName == null)
			{
				throw new ArgumentNullException("serviceName");
			}
			if (!ServiceController.ValidServiceName(serviceName))
			{
				throw new ArgumentException(Res.GetString("ServiceName", new object[]
				{
					serviceName,
					80.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(Res.GetString("BadMachineName", new object[] { machineName }));
			}
			this.permissionAccess = permissionAccess;
			this.machineName = machineName;
			this.serviceName = serviceName;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004F00 File Offset: 0x00003F00
		internal ServiceControllerPermissionEntry(ResourcePermissionBaseEntry baseEntry)
		{
			this.permissionAccess = (ServiceControllerPermissionAccess)baseEntry.PermissionAccess;
			this.machineName = baseEntry.PermissionAccessPath[0];
			this.serviceName = baseEntry.PermissionAccessPath[1];
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004F30 File Offset: 0x00003F30
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00004F38 File Offset: 0x00003F38
		public ServiceControllerPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004F40 File Offset: 0x00003F40
		public string ServiceName
		{
			get
			{
				return this.serviceName;
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004F48 File Offset: 0x00003F48
		internal ResourcePermissionBaseEntry GetBaseEntry()
		{
			return new ResourcePermissionBaseEntry((int)this.PermissionAccess, new string[] { this.MachineName, this.ServiceName });
		}

		// Token: 0x0400020A RID: 522
		private string machineName;

		// Token: 0x0400020B RID: 523
		private string serviceName;

		// Token: 0x0400020C RID: 524
		private ServiceControllerPermissionAccess permissionAccess;
	}
}
