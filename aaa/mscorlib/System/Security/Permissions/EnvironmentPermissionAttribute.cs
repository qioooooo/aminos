using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000625 RID: 1573
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class EnvironmentPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003918 RID: 14616 RVA: 0x000C1B50 File Offset: 0x000C0B50
		public EnvironmentPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06003919 RID: 14617 RVA: 0x000C1B59 File Offset: 0x000C0B59
		// (set) Token: 0x0600391A RID: 14618 RVA: 0x000C1B61 File Offset: 0x000C0B61
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

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600391B RID: 14619 RVA: 0x000C1B6A File Offset: 0x000C0B6A
		// (set) Token: 0x0600391C RID: 14620 RVA: 0x000C1B72 File Offset: 0x000C0B72
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

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x000C1B7B File Offset: 0x000C0B7B
		// (set) Token: 0x0600391E RID: 14622 RVA: 0x000C1B8C File Offset: 0x000C0B8C
		public string All
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GetMethod"));
			}
			set
			{
				this.m_write = value;
				this.m_read = value;
			}
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x000C1B9C File Offset: 0x000C0B9C
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new EnvironmentPermission(PermissionState.Unrestricted);
			}
			EnvironmentPermission environmentPermission = new EnvironmentPermission(PermissionState.None);
			if (this.m_read != null)
			{
				environmentPermission.SetPathList(EnvironmentPermissionAccess.Read, this.m_read);
			}
			if (this.m_write != null)
			{
				environmentPermission.SetPathList(EnvironmentPermissionAccess.Write, this.m_write);
			}
			return environmentPermission;
		}

		// Token: 0x04001DA8 RID: 7592
		private string m_read;

		// Token: 0x04001DA9 RID: 7593
		private string m_write;
	}
}
