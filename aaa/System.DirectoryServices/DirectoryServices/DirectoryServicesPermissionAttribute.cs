using System;
using System.Security;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x02000025 RID: 37
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public class DirectoryServicesPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00005A13 File Offset: 0x00004A13
		public DirectoryServicesPermissionAttribute(SecurityAction action)
			: base(action)
		{
			this.path = "*";
			this.permissionAccess = DirectoryServicesPermissionAccess.Browse;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00005A2E File Offset: 0x00004A2E
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00005A36 File Offset: 0x00004A36
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.path = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00005A4D File Offset: 0x00004A4D
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00005A55 File Offset: 0x00004A55
		public DirectoryServicesPermissionAccess PermissionAccess
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

		// Token: 0x06000109 RID: 265 RVA: 0x00005A60 File Offset: 0x00004A60
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new DirectoryServicesPermission(PermissionState.Unrestricted);
			}
			DirectoryServicesPermissionAccess directoryServicesPermissionAccess = this.permissionAccess;
			string text = this.Path;
			return new DirectoryServicesPermission(directoryServicesPermissionAccess, text);
		}

		// Token: 0x040001A6 RID: 422
		private string path;

		// Token: 0x040001A7 RID: 423
		private DirectoryServicesPermissionAccess permissionAccess;
	}
}
