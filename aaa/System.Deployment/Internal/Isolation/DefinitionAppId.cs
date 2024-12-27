using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000115 RID: 277
	internal sealed class DefinitionAppId
	{
		// Token: 0x06000688 RID: 1672 RVA: 0x0001F668 File Offset: 0x0001E668
		internal DefinitionAppId(IDefinitionAppId id)
		{
			if (id == null)
			{
				throw new ArgumentNullException();
			}
			this._id = id;
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001F680 File Offset: 0x0001E680
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x0001F68D File Offset: 0x0001E68D
		public string SubscriptionId
		{
			get
			{
				return this._id.get_SubscriptionId();
			}
			set
			{
				this._id.put_SubscriptionId(value);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001F69B File Offset: 0x0001E69B
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x0001F6A8 File Offset: 0x0001E6A8
		public string Codebase
		{
			get
			{
				return this._id.get_Codebase();
			}
			set
			{
				this._id.put_Codebase(value);
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0001F6B6 File Offset: 0x0001E6B6
		public EnumDefinitionIdentity AppPath
		{
			get
			{
				return new EnumDefinitionIdentity(this._id.EnumAppPath());
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001F6C8 File Offset: 0x0001E6C8
		private void SetAppPath(IDefinitionIdentity[] Ids)
		{
			this._id.SetAppPath((uint)Ids.Length, Ids);
		}

		// Token: 0x0400050A RID: 1290
		internal IDefinitionAppId _id;
	}
}
