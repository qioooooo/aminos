using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000628 RID: 1576
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x0600393C RID: 14652 RVA: 0x000C1E30 File Offset: 0x000C0E30
		public KeyContainerPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x0600393D RID: 14653 RVA: 0x000C1E47 File Offset: 0x000C0E47
		// (set) Token: 0x0600393E RID: 14654 RVA: 0x000C1E4F File Offset: 0x000C0E4F
		public string KeyStore
		{
			get
			{
				return this.m_keyStore;
			}
			set
			{
				this.m_keyStore = value;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x0600393F RID: 14655 RVA: 0x000C1E58 File Offset: 0x000C0E58
		// (set) Token: 0x06003940 RID: 14656 RVA: 0x000C1E60 File Offset: 0x000C0E60
		public string ProviderName
		{
			get
			{
				return this.m_providerName;
			}
			set
			{
				this.m_providerName = value;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06003941 RID: 14657 RVA: 0x000C1E69 File Offset: 0x000C0E69
		// (set) Token: 0x06003942 RID: 14658 RVA: 0x000C1E71 File Offset: 0x000C0E71
		public int ProviderType
		{
			get
			{
				return this.m_providerType;
			}
			set
			{
				this.m_providerType = value;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06003943 RID: 14659 RVA: 0x000C1E7A File Offset: 0x000C0E7A
		// (set) Token: 0x06003944 RID: 14660 RVA: 0x000C1E82 File Offset: 0x000C0E82
		public string KeyContainerName
		{
			get
			{
				return this.m_keyContainerName;
			}
			set
			{
				this.m_keyContainerName = value;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x000C1E8B File Offset: 0x000C0E8B
		// (set) Token: 0x06003946 RID: 14662 RVA: 0x000C1E93 File Offset: 0x000C0E93
		public int KeySpec
		{
			get
			{
				return this.m_keySpec;
			}
			set
			{
				this.m_keySpec = value;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06003947 RID: 14663 RVA: 0x000C1E9C File Offset: 0x000C0E9C
		// (set) Token: 0x06003948 RID: 14664 RVA: 0x000C1EA4 File Offset: 0x000C0EA4
		public KeyContainerPermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				this.m_flags = value;
			}
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x000C1EB0 File Offset: 0x000C0EB0
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new KeyContainerPermission(PermissionState.Unrestricted);
			}
			if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(this.m_keyStore, this.m_providerName, this.m_providerType, this.m_keyContainerName, this.m_keySpec))
			{
				return new KeyContainerPermission(this.m_flags);
			}
			KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
			KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(this.m_keyStore, this.m_providerName, this.m_providerType, this.m_keyContainerName, this.m_keySpec, this.m_flags);
			keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
			return keyContainerPermission;
		}

		// Token: 0x04001DB3 RID: 7603
		private KeyContainerPermissionFlags m_flags;

		// Token: 0x04001DB4 RID: 7604
		private string m_keyStore;

		// Token: 0x04001DB5 RID: 7605
		private string m_providerName;

		// Token: 0x04001DB6 RID: 7606
		private int m_providerType = -1;

		// Token: 0x04001DB7 RID: 7607
		private string m_keyContainerName;

		// Token: 0x04001DB8 RID: 7608
		private int m_keySpec = -1;
	}
}
