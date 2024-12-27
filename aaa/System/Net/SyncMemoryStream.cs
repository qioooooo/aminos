using System;
using System.IO;

namespace System.Net
{
	// Token: 0x020004D1 RID: 1233
	internal sealed class SyncMemoryStream : MemoryStream
	{
		// Token: 0x0600265B RID: 9819 RVA: 0x0009C1B4 File Offset: 0x0009B1B4
		internal SyncMemoryStream(byte[] bytes)
			: base(bytes, false)
		{
			this.m_ReadTimeout = (this.m_WriteTimeout = -1);
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x0009C1DC File Offset: 0x0009B1DC
		internal SyncMemoryStream(int initialCapacity)
			: base(initialCapacity)
		{
			this.m_ReadTimeout = (this.m_WriteTimeout = -1);
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x0009C200 File Offset: 0x0009B200
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			int num = this.Read(buffer, offset, count);
			return new LazyAsyncResult(null, state, callback, num);
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x0009C228 File Offset: 0x0009B228
		public override int EndRead(IAsyncResult asyncResult)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult;
			return (int)lazyAsyncResult.InternalWaitForCompletion();
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x0009C247 File Offset: 0x0009B247
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.Write(buffer, offset, count);
			return new LazyAsyncResult(null, state, callback, null);
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x0009C260 File Offset: 0x0009B260
		public override void EndWrite(IAsyncResult asyncResult)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult;
			lazyAsyncResult.InternalWaitForCompletion();
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002661 RID: 9825 RVA: 0x0009C27B File Offset: 0x0009B27B
		public override bool CanTimeout
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002662 RID: 9826 RVA: 0x0009C27E File Offset: 0x0009B27E
		// (set) Token: 0x06002663 RID: 9827 RVA: 0x0009C286 File Offset: 0x0009B286
		public override int ReadTimeout
		{
			get
			{
				return this.m_ReadTimeout;
			}
			set
			{
				this.m_ReadTimeout = value;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002664 RID: 9828 RVA: 0x0009C28F File Offset: 0x0009B28F
		// (set) Token: 0x06002665 RID: 9829 RVA: 0x0009C297 File Offset: 0x0009B297
		public override int WriteTimeout
		{
			get
			{
				return this.m_WriteTimeout;
			}
			set
			{
				this.m_WriteTimeout = value;
			}
		}

		// Token: 0x040025E7 RID: 9703
		private int m_ReadTimeout;

		// Token: 0x040025E8 RID: 9704
		private int m_WriteTimeout;
	}
}
