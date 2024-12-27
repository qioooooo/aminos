using System;
using System.ComponentModel;

namespace System.Net.Security
{
	// Token: 0x02000537 RID: 1335
	internal class ProtocolToken
	{
		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x060028D0 RID: 10448 RVA: 0x000A9838 File Offset: 0x000A8838
		internal bool Failed
		{
			get
			{
				return this.Status != SecurityStatus.OK && this.Status != SecurityStatus.ContinueNeeded;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x000A9854 File Offset: 0x000A8854
		internal bool Done
		{
			get
			{
				return this.Status == SecurityStatus.OK;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x000A985F File Offset: 0x000A885F
		internal bool Renegotiate
		{
			get
			{
				return this.Status == SecurityStatus.Renegotiate;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060028D3 RID: 10451 RVA: 0x000A986E File Offset: 0x000A886E
		internal bool CloseConnection
		{
			get
			{
				return this.Status == SecurityStatus.ContextExpired;
			}
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000A987D File Offset: 0x000A887D
		internal ProtocolToken(byte[] data, SecurityStatus errorCode)
		{
			this.Status = errorCode;
			this.Payload = data;
			this.Size = ((data != null) ? data.Length : 0);
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000A98A2 File Offset: 0x000A88A2
		internal Win32Exception GetException()
		{
			if (!this.Done)
			{
				return new Win32Exception((int)this.Status);
			}
			return null;
		}

		// Token: 0x040027BA RID: 10170
		internal SecurityStatus Status;

		// Token: 0x040027BB RID: 10171
		internal byte[] Payload;

		// Token: 0x040027BC RID: 10172
		internal int Size;
	}
}
