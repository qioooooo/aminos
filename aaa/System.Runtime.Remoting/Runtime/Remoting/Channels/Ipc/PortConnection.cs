using System;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x0200005C RID: 92
	internal class PortConnection
	{
		// Token: 0x060002F6 RID: 758 RVA: 0x0000E4D8 File Offset: 0x0000D4D8
		internal PortConnection(IpcPort port)
		{
			this._port = port;
			this._socketLastUsed = DateTime.Now;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000E4F2 File Offset: 0x0000D4F2
		internal IpcPort Port
		{
			get
			{
				return this._port;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000E4FA File Offset: 0x0000D4FA
		internal DateTime LastUsed
		{
			get
			{
				return this._socketLastUsed;
			}
		}

		// Token: 0x0400020F RID: 527
		private IpcPort _port;

		// Token: 0x04000210 RID: 528
		private DateTime _socketLastUsed;
	}
}
