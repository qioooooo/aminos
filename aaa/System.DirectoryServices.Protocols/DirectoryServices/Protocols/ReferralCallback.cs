using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000089 RID: 137
	public sealed class ReferralCallback
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0000DEC8 File Offset: 0x0000CEC8
		public ReferralCallback()
		{
			Utility.CheckOSVersion();
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000DED5 File Offset: 0x0000CED5
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000DEDD File Offset: 0x0000CEDD
		public QueryForConnectionCallback QueryForConnection
		{
			get
			{
				return this.query;
			}
			set
			{
				this.query = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000DEE6 File Offset: 0x0000CEE6
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x0000DEEE File Offset: 0x0000CEEE
		public NotifyOfNewConnectionCallback NotifyNewConnection
		{
			get
			{
				return this.notify;
			}
			set
			{
				this.notify = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000DEF7 File Offset: 0x0000CEF7
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x0000DEFF File Offset: 0x0000CEFF
		public DereferenceConnectionCallback DereferenceConnection
		{
			get
			{
				return this.dereference;
			}
			set
			{
				this.dereference = value;
			}
		}

		// Token: 0x04000298 RID: 664
		private QueryForConnectionCallback query;

		// Token: 0x04000299 RID: 665
		private NotifyOfNewConnectionCallback notify;

		// Token: 0x0400029A RID: 666
		private DereferenceConnectionCallback dereference;
	}
}
