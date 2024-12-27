using System;
using System.IO;

namespace System.Web
{
	// Token: 0x02000077 RID: 119
	internal class HttpInputStream : Stream
	{
		// Token: 0x06000518 RID: 1304 RVA: 0x000150F6 File Offset: 0x000140F6
		internal HttpInputStream(HttpRawUploadedContent data, int offset, int length)
		{
			this.Init(data, offset, length);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00015107 File Offset: 0x00014107
		protected void Init(HttpRawUploadedContent data, int offset, int length)
		{
			this._data = data;
			this._offset = offset;
			this._length = length;
			this._pos = 0;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00015125 File Offset: 0x00014125
		protected void Uninit()
		{
			this._data = null;
			this._offset = 0;
			this._length = 0;
			this._pos = 0;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00015143 File Offset: 0x00014143
		internal byte[] GetAsByteArray()
		{
			if (this._length == 0)
			{
				return null;
			}
			return this._data.GetAsByteArray(this._offset, this._length);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00015166 File Offset: 0x00014166
		internal void WriteTo(Stream s)
		{
			if (this._data != null && this._length > 0)
			{
				this._data.WriteBytes(this._offset, this._length, s);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00015191 File Offset: 0x00014191
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00015194 File Offset: 0x00014194
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00015197 File Offset: 0x00014197
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0001519A File Offset: 0x0001419A
		public override long Length
		{
			get
			{
				return (long)this._length;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x000151A3 File Offset: 0x000141A3
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x000151AC File Offset: 0x000141AC
		public override long Position
		{
			get
			{
				return (long)this._pos;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000151B8 File Offset: 0x000141B8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.Uninit();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000151E8 File Offset: 0x000141E8
		public override void Flush()
		{
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000151EC File Offset: 0x000141EC
		public override long Seek(long offset, SeekOrigin origin)
		{
			int num = this._pos;
			int num2 = (int)offset;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = num2;
				break;
			case SeekOrigin.Current:
				num = this._pos + num2;
				break;
			case SeekOrigin.End:
				num = this._length + num2;
				break;
			default:
				throw new ArgumentOutOfRangeException("origin");
			}
			if (num < 0 || num > this._length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			this._pos = num;
			return (long)this._pos;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00015264 File Offset: 0x00014264
		public override void SetLength(long length)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001526C File Offset: 0x0001426C
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this._length - this._pos;
			if (count < num)
			{
				num = count;
			}
			if (num > 0)
			{
				this._data.CopyBytes(this._offset + this._pos, buffer, offset, num);
			}
			this._pos += num;
			return num;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x000152BB File Offset: 0x000142BB
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04001055 RID: 4181
		private HttpRawUploadedContent _data;

		// Token: 0x04001056 RID: 4182
		private int _offset;

		// Token: 0x04001057 RID: 4183
		private int _length;

		// Token: 0x04001058 RID: 4184
		private int _pos;
	}
}
