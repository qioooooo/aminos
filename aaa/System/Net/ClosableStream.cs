using System;
using System.IO;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000685 RID: 1669
	internal class ClosableStream : DelegatedStream
	{
		// Token: 0x060033A9 RID: 13225 RVA: 0x000DA29F File Offset: 0x000D929F
		internal ClosableStream(Stream stream, EventHandler onClose)
			: base(stream)
		{
			this.onClose = onClose;
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x000DA2AF File Offset: 0x000D92AF
		public override void Close()
		{
			if (Interlocked.Increment(ref this.closed) == 1 && this.onClose != null)
			{
				this.onClose(this, new EventArgs());
			}
		}

		// Token: 0x04002FA8 RID: 12200
		private EventHandler onClose;

		// Token: 0x04002FA9 RID: 12201
		private int closed;
	}
}
