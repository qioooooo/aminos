using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200062F RID: 1583
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class StrongNameIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003998 RID: 14744 RVA: 0x000C2568 File Offset: 0x000C1568
		public StrongNameIdentityPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06003999 RID: 14745 RVA: 0x000C2571 File Offset: 0x000C1571
		// (set) Token: 0x0600399A RID: 14746 RVA: 0x000C2579 File Offset: 0x000C1579
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x0600399B RID: 14747 RVA: 0x000C2582 File Offset: 0x000C1582
		// (set) Token: 0x0600399C RID: 14748 RVA: 0x000C258A File Offset: 0x000C158A
		public string Version
		{
			get
			{
				return this.m_version;
			}
			set
			{
				this.m_version = value;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x0600399D RID: 14749 RVA: 0x000C2593 File Offset: 0x000C1593
		// (set) Token: 0x0600399E RID: 14750 RVA: 0x000C259B File Offset: 0x000C159B
		public string PublicKey
		{
			get
			{
				return this.m_blob;
			}
			set
			{
				this.m_blob = value;
			}
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x000C25A4 File Offset: 0x000C15A4
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new StrongNameIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.m_blob == null && this.m_name == null && this.m_version == null)
			{
				return new StrongNameIdentityPermission(PermissionState.None);
			}
			if (this.m_blob == null)
			{
				throw new ArgumentException(Environment.GetResourceString("ArgumentNull_Key"));
			}
			StrongNamePublicKeyBlob strongNamePublicKeyBlob = new StrongNamePublicKeyBlob(this.m_blob);
			if (this.m_version == null || this.m_version.Equals(string.Empty))
			{
				return new StrongNameIdentityPermission(strongNamePublicKeyBlob, this.m_name, null);
			}
			return new StrongNameIdentityPermission(strongNamePublicKeyBlob, this.m_name, new Version(this.m_version));
		}

		// Token: 0x04001DC6 RID: 7622
		private string m_name;

		// Token: 0x04001DC7 RID: 7623
		private string m_version;

		// Token: 0x04001DC8 RID: 7624
		private string m_blob;
	}
}
