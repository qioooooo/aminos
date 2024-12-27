using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000016 RID: 22
	internal class CachedSocket
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00004297 File Offset: 0x00003297
		internal CachedSocket(SocketHandler socket, CachedSocket next)
		{
			this._socket = socket;
			this._socketLastUsed = DateTime.UtcNow;
			this._next = next;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000042B8 File Offset: 0x000032B8
		internal SocketHandler Handler
		{
			get
			{
				return this._socket;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000042C0 File Offset: 0x000032C0
		internal DateTime LastUsed
		{
			get
			{
				return this._socketLastUsed;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008A RID: 138 RVA: 0x000042C8 File Offset: 0x000032C8
		// (set) Token: 0x0600008B RID: 139 RVA: 0x000042D0 File Offset: 0x000032D0
		internal CachedSocket Next
		{
			get
			{
				return this._next;
			}
			set
			{
				this._next = value;
			}
		}

		// Token: 0x0400008C RID: 140
		private SocketHandler _socket;

		// Token: 0x0400008D RID: 141
		private DateTime _socketLastUsed;

		// Token: 0x0400008E RID: 142
		private CachedSocket _next;
	}
}
