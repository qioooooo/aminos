using System;
using System.IO;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000030 RID: 48
	internal abstract class HttpServerResponseStream : Stream
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000085D3 File Offset: 0x000075D3
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000193 RID: 403 RVA: 0x000085D6 File Offset: 0x000075D6
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000194 RID: 404 RVA: 0x000085D9 File Offset: 0x000075D9
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000195 RID: 405 RVA: 0x000085DC File Offset: 0x000075DC
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000085E3 File Offset: 0x000075E3
		// (set) Token: 0x06000197 RID: 407 RVA: 0x000085EA File Offset: 0x000075EA
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

		// Token: 0x06000198 RID: 408 RVA: 0x000085F1 File Offset: 0x000075F1
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000085F8 File Offset: 0x000075F8
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000085FF File Offset: 0x000075FF
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}
