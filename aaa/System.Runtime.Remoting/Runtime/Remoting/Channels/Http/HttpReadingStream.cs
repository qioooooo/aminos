using System;
using System.IO;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000033 RID: 51
	internal abstract class HttpReadingStream : Stream
	{
		// Token: 0x060001AA RID: 426 RVA: 0x000089B0 File Offset: 0x000079B0
		public virtual bool ReadToEnd()
		{
			byte[] array = new byte[16];
			int num;
			do
			{
				num = this.Read(array, 0, 16);
			}
			while (num > 0);
			return num == 0;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000089DA File Offset: 0x000079DA
		public virtual bool FoundEnd
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001AC RID: 428 RVA: 0x000089DD File Offset: 0x000079DD
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000089E0 File Offset: 0x000079E0
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001AE RID: 430 RVA: 0x000089E3 File Offset: 0x000079E3
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001AF RID: 431 RVA: 0x000089E6 File Offset: 0x000079E6
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000089ED File Offset: 0x000079ED
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x000089F4 File Offset: 0x000079F4
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000089FB File Offset: 0x000079FB
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00008A02 File Offset: 0x00007A02
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008A09 File Offset: 0x00007A09
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00008A10 File Offset: 0x00007A10
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}
