using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Security.Permissions
{
	// Token: 0x0200062B RID: 1579
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class RegistryPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x0600395E RID: 14686 RVA: 0x000C2095 File Offset: 0x000C1095
		public RegistryPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x0600395F RID: 14687 RVA: 0x000C209E File Offset: 0x000C109E
		// (set) Token: 0x06003960 RID: 14688 RVA: 0x000C20A6 File Offset: 0x000C10A6
		public string Read
		{
			get
			{
				return this.m_read;
			}
			set
			{
				this.m_read = value;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06003961 RID: 14689 RVA: 0x000C20AF File Offset: 0x000C10AF
		// (set) Token: 0x06003962 RID: 14690 RVA: 0x000C20B7 File Offset: 0x000C10B7
		public string Write
		{
			get
			{
				return this.m_write;
			}
			set
			{
				this.m_write = value;
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06003963 RID: 14691 RVA: 0x000C20C0 File Offset: 0x000C10C0
		// (set) Token: 0x06003964 RID: 14692 RVA: 0x000C20C8 File Offset: 0x000C10C8
		public string Create
		{
			get
			{
				return this.m_create;
			}
			set
			{
				this.m_create = value;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06003965 RID: 14693 RVA: 0x000C20D1 File Offset: 0x000C10D1
		// (set) Token: 0x06003966 RID: 14694 RVA: 0x000C20D9 File Offset: 0x000C10D9
		public string ViewAccessControl
		{
			get
			{
				return this.m_viewAcl;
			}
			set
			{
				this.m_viewAcl = value;
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06003967 RID: 14695 RVA: 0x000C20E2 File Offset: 0x000C10E2
		// (set) Token: 0x06003968 RID: 14696 RVA: 0x000C20EA File Offset: 0x000C10EA
		public string ChangeAccessControl
		{
			get
			{
				return this.m_changeAcl;
			}
			set
			{
				this.m_changeAcl = value;
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06003969 RID: 14697 RVA: 0x000C20F3 File Offset: 0x000C10F3
		// (set) Token: 0x0600396A RID: 14698 RVA: 0x000C2104 File Offset: 0x000C1104
		public string ViewAndModify
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GetMethod"));
			}
			set
			{
				this.m_read = value;
				this.m_write = value;
				this.m_create = value;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x0600396B RID: 14699 RVA: 0x000C211B File Offset: 0x000C111B
		// (set) Token: 0x0600396C RID: 14700 RVA: 0x000C212C File Offset: 0x000C112C
		[Obsolete("Please use the ViewAndModify property instead.")]
		public string All
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GetMethod"));
			}
			set
			{
				this.m_read = value;
				this.m_write = value;
				this.m_create = value;
			}
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x000C2144 File Offset: 0x000C1144
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new RegistryPermission(PermissionState.Unrestricted);
			}
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.None);
			if (this.m_read != null)
			{
				registryPermission.SetPathList(RegistryPermissionAccess.Read, this.m_read);
			}
			if (this.m_write != null)
			{
				registryPermission.SetPathList(RegistryPermissionAccess.Write, this.m_write);
			}
			if (this.m_create != null)
			{
				registryPermission.SetPathList(RegistryPermissionAccess.Create, this.m_create);
			}
			if (this.m_viewAcl != null)
			{
				registryPermission.SetPathList(AccessControlActions.View, this.m_viewAcl);
			}
			if (this.m_changeAcl != null)
			{
				registryPermission.SetPathList(AccessControlActions.Change, this.m_changeAcl);
			}
			return registryPermission;
		}

		// Token: 0x04001DBD RID: 7613
		private string m_read;

		// Token: 0x04001DBE RID: 7614
		private string m_write;

		// Token: 0x04001DBF RID: 7615
		private string m_create;

		// Token: 0x04001DC0 RID: 7616
		private string m_viewAcl;

		// Token: 0x04001DC1 RID: 7617
		private string m_changeAcl;
	}
}
