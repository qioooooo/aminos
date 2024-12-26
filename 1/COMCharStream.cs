using System;
using System.IO;
using System.Text;

namespace Microsoft.JScript
{
	// Token: 0x02000101 RID: 257
	public class COMCharStream : Stream
	{
		// Token: 0x06000AEF RID: 2799 RVA: 0x00054730 File Offset: 0x00053730
		public COMCharStream(IMessageReceiver messageReceiver)
		{
			this.messageReceiver = messageReceiver;
			this.buffer = new StringBuilder(128);
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x0005474F File Offset: 0x0005374F
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00054752 File Offset: 0x00053752
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00054755 File Offset: 0x00053755
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x00054758 File Offset: 0x00053758
		public override long Length
		{
			get
			{
				return (long)this.buffer.Length;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x00054766 File Offset: 0x00053766
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x00054774 File Offset: 0x00053774
		public override long Position
		{
			get
			{
				return (long)this.buffer.Length;
			}
			set
			{
			}
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00054776 File Offset: 0x00053776
		public override void Close()
		{
			this.Flush();
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0005477E File Offset: 0x0005377E
		public override void Flush()
		{
			this.messageReceiver.Message(this.buffer.ToString());
			this.buffer = new StringBuilder(128);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x000547A6 File Offset: 0x000537A6
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x000547AD File Offset: 0x000537AD
		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0L;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x000547B1 File Offset: 0x000537B1
		public override void SetLength(long value)
		{
			this.buffer.Length = (int)value;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x000547C0 File Offset: 0x000537C0
		public override void Write(byte[] buffer, int offset, int count)
		{
			for (int i = count; i > 0; i--)
			{
				this.buffer.Append((char)buffer[offset++]);
			}
		}

		// Token: 0x040006AD RID: 1709
		private IMessageReceiver messageReceiver;

		// Token: 0x040006AE RID: 1710
		private StringBuilder buffer;
	}
}
