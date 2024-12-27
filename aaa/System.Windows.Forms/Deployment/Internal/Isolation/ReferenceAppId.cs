using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000EC RID: 236
	internal sealed class ReferenceAppId
	{
		// Token: 0x060003AA RID: 938 RVA: 0x00008021 File Offset: 0x00007021
		internal ReferenceAppId(IReferenceAppId id)
		{
			if (id == null)
			{
				throw new ArgumentNullException();
			}
			this._id = id;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00008039 File Offset: 0x00007039
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00008046 File Offset: 0x00007046
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

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00008054 File Offset: 0x00007054
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00008061 File Offset: 0x00007061
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

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000806F File Offset: 0x0000706F
		public EnumReferenceIdentity AppPath
		{
			get
			{
				return new EnumReferenceIdentity(this._id.EnumAppPath());
			}
		}

		// Token: 0x04000D97 RID: 3479
		internal IReferenceAppId _id;
	}
}
