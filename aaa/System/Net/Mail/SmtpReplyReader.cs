using System;

namespace System.Net.Mail
{
	// Token: 0x020006D5 RID: 1749
	internal class SmtpReplyReader
	{
		// Token: 0x060035F8 RID: 13816 RVA: 0x000E63F5 File Offset: 0x000E53F5
		internal SmtpReplyReader(SmtpReplyReaderFactory reader)
		{
			this.reader = reader;
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x000E6404 File Offset: 0x000E5404
		internal IAsyncResult BeginReadLines(AsyncCallback callback, object state)
		{
			return this.reader.BeginReadLines(this, callback, state);
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x000E6414 File Offset: 0x000E5414
		internal IAsyncResult BeginReadLine(AsyncCallback callback, object state)
		{
			return this.reader.BeginReadLine(this, callback, state);
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x000E6424 File Offset: 0x000E5424
		public void Close()
		{
			this.reader.Close(this);
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x000E6432 File Offset: 0x000E5432
		internal LineInfo[] EndReadLines(IAsyncResult result)
		{
			return this.reader.EndReadLines(result);
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x000E6440 File Offset: 0x000E5440
		internal LineInfo EndReadLine(IAsyncResult result)
		{
			return this.reader.EndReadLine(result);
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x000E644E File Offset: 0x000E544E
		internal LineInfo[] ReadLines()
		{
			return this.reader.ReadLines(this);
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x000E645C File Offset: 0x000E545C
		internal LineInfo ReadLine()
		{
			return this.reader.ReadLine(this);
		}

		// Token: 0x04003117 RID: 12567
		private SmtpReplyReaderFactory reader;
	}
}
