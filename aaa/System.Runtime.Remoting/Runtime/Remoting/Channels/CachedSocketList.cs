using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000017 RID: 23
	internal class CachedSocketList
	{
		// Token: 0x0600008C RID: 140 RVA: 0x000042D9 File Offset: 0x000032D9
		internal CachedSocketList(TimeSpan socketLifetime, SocketCachePolicy socketCachePolicy)
		{
			this._socketCount = 0;
			this._socketLifetime = socketLifetime;
			this._socketCachePolicy = socketCachePolicy;
			this._socketList = null;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004300 File Offset: 0x00003300
		internal SocketHandler GetSocket()
		{
			if (this._socketCount == 0)
			{
				return null;
			}
			lock (this)
			{
				if (this._socketList != null)
				{
					SocketHandler handler = this._socketList.Handler;
					this._socketList = this._socketList.Next;
					handler.RaceForControl();
					this._socketCount--;
					return handler;
				}
			}
			return null;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000437C File Offset: 0x0000337C
		internal void ReturnSocket(SocketHandler socket)
		{
			TimeSpan timeSpan = DateTime.UtcNow - socket.CreationTime;
			bool flag = false;
			lock (this)
			{
				if (this._socketCachePolicy != SocketCachePolicy.AbsoluteTimeout || timeSpan < this._socketLifetime)
				{
					for (CachedSocket cachedSocket = this._socketList; cachedSocket != null; cachedSocket = cachedSocket.Next)
					{
						if (socket == cachedSocket.Handler)
						{
							return;
						}
					}
					this._socketList = new CachedSocket(socket, this._socketList);
					this._socketCount++;
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				socket.Close();
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004420 File Offset: 0x00003420
		internal void TimeoutSockets(DateTime currentTime, TimeSpan socketLifetime)
		{
			lock (this)
			{
				CachedSocket cachedSocket = null;
				CachedSocket cachedSocket2 = this._socketList;
				while (cachedSocket2 != null)
				{
					if ((this._socketCachePolicy == SocketCachePolicy.AbsoluteTimeout && currentTime - cachedSocket2.Handler.CreationTime > socketLifetime) || currentTime - cachedSocket2.LastUsed > socketLifetime)
					{
						cachedSocket2.Handler.Close();
						if (cachedSocket == null)
						{
							this._socketList = cachedSocket2.Next;
							cachedSocket2 = this._socketList;
						}
						else
						{
							cachedSocket2 = cachedSocket2.Next;
							cachedSocket.Next = cachedSocket2;
						}
						this._socketCount--;
					}
					else
					{
						cachedSocket = cachedSocket2;
						cachedSocket2 = cachedSocket2.Next;
					}
				}
			}
		}

		// Token: 0x0400008F RID: 143
		private int _socketCount;

		// Token: 0x04000090 RID: 144
		private TimeSpan _socketLifetime;

		// Token: 0x04000091 RID: 145
		private SocketCachePolicy _socketCachePolicy;

		// Token: 0x04000092 RID: 146
		private CachedSocket _socketList;
	}
}
