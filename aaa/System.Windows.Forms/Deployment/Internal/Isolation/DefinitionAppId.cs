using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000EA RID: 234
	internal sealed class DefinitionAppId
	{
		// Token: 0x0600039E RID: 926 RVA: 0x00007FB0 File Offset: 0x00006FB0
		internal DefinitionAppId(IDefinitionAppId id)
		{
			if (id == null)
			{
				throw new ArgumentNullException();
			}
			this._id = id;
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00007FC8 File Offset: 0x00006FC8
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00007FD5 File Offset: 0x00006FD5
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

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00007FE3 File Offset: 0x00006FE3
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00007FF0 File Offset: 0x00006FF0
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

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00007FFE File Offset: 0x00006FFE
		public EnumDefinitionIdentity AppPath
		{
			get
			{
				return new EnumDefinitionIdentity(this._id.EnumAppPath());
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00008010 File Offset: 0x00007010
		private void SetAppPath(IDefinitionIdentity[] Ids)
		{
			this._id.SetAppPath((uint)Ids.Length, Ids);
		}

		// Token: 0x04000D96 RID: 3478
		internal IDefinitionAppId _id;
	}
}
