using System;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000B3 RID: 179
	public sealed class ClerkInfo
	{
		// Token: 0x06000447 RID: 1095 RVA: 0x0000D6C8 File Offset: 0x0000C6C8
		internal ClerkInfo(object index, CrmMonitor monitor, _IMonitorClerks clerks)
		{
			this._index = index;
			this._clerks = clerks;
			this._monitor = monitor;
			this._monitor.AddRef();
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0000D6F0 File Offset: 0x0000C6F0
		public Clerk Clerk
		{
			get
			{
				return new Clerk(this._monitor.HoldClerk(this.InstanceId));
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0000D708 File Offset: 0x0000C708
		public string InstanceId
		{
			get
			{
				return (string)this._clerks.Item(this._index);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000D720 File Offset: 0x0000C720
		public string Compensator
		{
			get
			{
				return (string)this._clerks.ProgIdCompensator(this._index);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x0000D738 File Offset: 0x0000C738
		public string Description
		{
			get
			{
				return (string)this._clerks.Description(this._index);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0000D750 File Offset: 0x0000C750
		public string TransactionUOW
		{
			get
			{
				return (string)this._clerks.TransactionUOW(this._index);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x0000D768 File Offset: 0x0000C768
		public string ActivityId
		{
			get
			{
				return (string)this._clerks.ActivityId(this._index);
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000D780 File Offset: 0x0000C780
		~ClerkInfo()
		{
			this._monitor.Release();
		}

		// Token: 0x040001EC RID: 492
		private object _index;

		// Token: 0x040001ED RID: 493
		private CrmMonitor _monitor;

		// Token: 0x040001EE RID: 494
		private _IMonitorClerks _clerks;
	}
}
