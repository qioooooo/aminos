using System;
using System.Data;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200029C RID: 668
	internal sealed class DummyStream : Stream
	{
		// Token: 0x06002285 RID: 8837 RVA: 0x0026DBF8 File Offset: 0x0026CFF8
		private void DontDoIt()
		{
			throw new Exception(Res.GetString("Sql_InternalError"));
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x0026DC14 File Offset: 0x0026D014
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x0026DC24 File Offset: 0x0026D024
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06002288 RID: 8840 RVA: 0x0026DC34 File Offset: 0x0026D034
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06002289 RID: 8841 RVA: 0x0026DC44 File Offset: 0x0026D044
		// (set) Token: 0x0600228A RID: 8842 RVA: 0x0026DC58 File Offset: 0x0026D058
		public override long Position
		{
			get
			{
				return this.m_size;
			}
			set
			{
				this.m_size = value;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x0600228B RID: 8843 RVA: 0x0026DC6C File Offset: 0x0026D06C
		public override long Length
		{
			get
			{
				return this.m_size;
			}
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x0026DC80 File Offset: 0x0026D080
		public override void SetLength(long value)
		{
			this.m_size = value;
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x0026DC94 File Offset: 0x0026D094
		public override long Seek(long value, SeekOrigin loc)
		{
			this.DontDoIt();
			return -1L;
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x0026DCAC File Offset: 0x0026D0AC
		public override void Flush()
		{
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x0026DCBC File Offset: 0x0026D0BC
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.DontDoIt();
			return -1;
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x0026DCD0 File Offset: 0x0026D0D0
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.m_size += (long)count;
		}

		// Token: 0x04001659 RID: 5721
		private long m_size;
	}
}
