using System;

namespace System.ServiceProcess
{
	// Token: 0x02000036 RID: 54
	public struct SessionChangeDescription
	{
		// Token: 0x06000105 RID: 261 RVA: 0x000062A0 File Offset: 0x000052A0
		internal SessionChangeDescription(SessionChangeReason reason, int id)
		{
			this._reason = reason;
			this._id = id;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000062B0 File Offset: 0x000052B0
		public SessionChangeReason Reason
		{
			get
			{
				return this._reason;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000062B8 File Offset: 0x000052B8
		public int SessionId
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000062C0 File Offset: 0x000052C0
		public override bool Equals(object obj)
		{
			return obj != null && obj is SessionChangeDescription && this.Equals((SessionChangeDescription)obj);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000062DB File Offset: 0x000052DB
		public override int GetHashCode()
		{
			return (int)(this._reason ^ (SessionChangeReason)this._id);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000062EA File Offset: 0x000052EA
		public bool Equals(SessionChangeDescription changeDescription)
		{
			return this._reason == changeDescription._reason && this._id == changeDescription._id;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000630C File Offset: 0x0000530C
		public static bool operator ==(SessionChangeDescription a, SessionChangeDescription b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006316 File Offset: 0x00005316
		public static bool operator !=(SessionChangeDescription a, SessionChangeDescription b)
		{
			return !a.Equals(b);
		}

		// Token: 0x0400023E RID: 574
		private SessionChangeReason _reason;

		// Token: 0x0400023F RID: 575
		private int _id;
	}
}
