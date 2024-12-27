using System;

namespace System.Security.Permissions
{
	// Token: 0x020002C3 RID: 707
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class StorePermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06001822 RID: 6178 RVA: 0x00053509 File Offset: 0x00052509
		public StorePermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001823 RID: 6179 RVA: 0x00053512 File Offset: 0x00052512
		// (set) Token: 0x06001824 RID: 6180 RVA: 0x0005351A File Offset: 0x0005251A
		public StorePermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				StorePermission.VerifyFlags(value);
				this.m_flags = value;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001825 RID: 6181 RVA: 0x00053529 File Offset: 0x00052529
		// (set) Token: 0x06001826 RID: 6182 RVA: 0x00053539 File Offset: 0x00052539
		public bool CreateStore
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.CreateStore) != StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.CreateStore) : (this.m_flags & ~StorePermissionFlags.CreateStore));
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001827 RID: 6183 RVA: 0x00053557 File Offset: 0x00052557
		// (set) Token: 0x06001828 RID: 6184 RVA: 0x00053567 File Offset: 0x00052567
		public bool DeleteStore
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.DeleteStore) != StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.DeleteStore) : (this.m_flags & ~StorePermissionFlags.DeleteStore));
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001829 RID: 6185 RVA: 0x00053585 File Offset: 0x00052585
		// (set) Token: 0x0600182A RID: 6186 RVA: 0x00053595 File Offset: 0x00052595
		public bool EnumerateStores
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.EnumerateStores) != StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.EnumerateStores) : (this.m_flags & ~StorePermissionFlags.EnumerateStores));
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x0600182B RID: 6187 RVA: 0x000535B3 File Offset: 0x000525B3
		// (set) Token: 0x0600182C RID: 6188 RVA: 0x000535C4 File Offset: 0x000525C4
		public bool OpenStore
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.OpenStore) != StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.OpenStore) : (this.m_flags & ~StorePermissionFlags.OpenStore));
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x0600182D RID: 6189 RVA: 0x000535E3 File Offset: 0x000525E3
		// (set) Token: 0x0600182E RID: 6190 RVA: 0x000535F4 File Offset: 0x000525F4
		public bool AddToStore
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.AddToStore) != StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.AddToStore) : (this.m_flags & ~StorePermissionFlags.AddToStore));
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x0600182F RID: 6191 RVA: 0x00053613 File Offset: 0x00052613
		// (set) Token: 0x06001830 RID: 6192 RVA: 0x00053624 File Offset: 0x00052624
		public bool RemoveFromStore
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.RemoveFromStore) != StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.RemoveFromStore) : (this.m_flags & ~StorePermissionFlags.RemoveFromStore));
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001831 RID: 6193 RVA: 0x00053643 File Offset: 0x00052643
		// (set) Token: 0x06001832 RID: 6194 RVA: 0x00053657 File Offset: 0x00052657
		public bool EnumerateCertificates
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.EnumerateCertificates) != StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.EnumerateCertificates) : (this.m_flags & ~StorePermissionFlags.EnumerateCertificates));
			}
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0005367C File Offset: 0x0005267C
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new StorePermission(PermissionState.Unrestricted);
			}
			return new StorePermission(this.m_flags);
		}

		// Token: 0x04001622 RID: 5666
		private StorePermissionFlags m_flags;
	}
}
