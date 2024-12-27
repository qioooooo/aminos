using System;

namespace System.Net
{
	// Token: 0x020004BF RID: 1215
	internal class ReceiveState
	{
		// Token: 0x060025B4 RID: 9652 RVA: 0x0009627B File Offset: 0x0009527B
		internal ReceiveState(CommandStream connection)
		{
			this.Connection = connection;
			this.Resp = new ResponseDescription();
			this.Buffer = new byte[1024];
			this.ValidThrough = 0;
		}

		// Token: 0x0400254D RID: 9549
		private const int bufferSize = 1024;

		// Token: 0x0400254E RID: 9550
		internal ResponseDescription Resp;

		// Token: 0x0400254F RID: 9551
		internal int ValidThrough;

		// Token: 0x04002550 RID: 9552
		internal byte[] Buffer;

		// Token: 0x04002551 RID: 9553
		internal CommandStream Connection;
	}
}
