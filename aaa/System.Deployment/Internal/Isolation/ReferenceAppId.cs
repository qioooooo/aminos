using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000117 RID: 279
	internal sealed class ReferenceAppId
	{
		// Token: 0x06000694 RID: 1684 RVA: 0x0001F6D9 File Offset: 0x0001E6D9
		internal ReferenceAppId(IReferenceAppId id)
		{
			if (id == null)
			{
				throw new ArgumentNullException();
			}
			this._id = id;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0001F6F1 File Offset: 0x0001E6F1
		// (set) Token: 0x06000696 RID: 1686 RVA: 0x0001F6FE File Offset: 0x0001E6FE
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

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0001F70C File Offset: 0x0001E70C
		// (set) Token: 0x06000698 RID: 1688 RVA: 0x0001F719 File Offset: 0x0001E719
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

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001F727 File Offset: 0x0001E727
		public EnumReferenceIdentity AppPath
		{
			get
			{
				return new EnumReferenceIdentity(this._id.EnumAppPath());
			}
		}

		// Token: 0x0400050B RID: 1291
		internal IReferenceAppId _id;
	}
}
