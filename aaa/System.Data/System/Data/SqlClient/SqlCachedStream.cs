using System;
using System.Collections;
using System.Data.Common;
using System.IO;

namespace System.Data.SqlClient
{
	// Token: 0x0200030F RID: 783
	internal sealed class SqlCachedStream : Stream
	{
		// Token: 0x060028F2 RID: 10482 RVA: 0x0029155C File Offset: 0x0029095C
		internal SqlCachedStream(SqlCachedBuffer sqlBuf)
		{
			this._cachedBytes = sqlBuf.CachedBytes;
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060028F3 RID: 10483 RVA: 0x0029157C File Offset: 0x0029097C
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x060028F4 RID: 10484 RVA: 0x0029158C File Offset: 0x0029098C
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x060028F5 RID: 10485 RVA: 0x0029159C File Offset: 0x0029099C
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x060028F6 RID: 10486 RVA: 0x002915AC File Offset: 0x002909AC
		public override long Length
		{
			get
			{
				return this.TotalLength;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x002915C0 File Offset: 0x002909C0
		// (set) Token: 0x060028F8 RID: 10488 RVA: 0x00291610 File Offset: 0x00290A10
		public override long Position
		{
			get
			{
				long num = 0L;
				if (this._currentArrayIndex > 0)
				{
					for (int i = 0; i < this._currentArrayIndex; i++)
					{
						byte[] array = (byte[])this._cachedBytes[i];
						num += (long)array.Length;
					}
				}
				return num + (long)this._currentPosition;
			}
			set
			{
				if (this._cachedBytes == null)
				{
					throw ADP.StreamClosed("set_Position");
				}
				this.SetInternalPosition(value, "set_Position");
			}
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x0029163C File Offset: 0x00290A3C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._cachedBytes != null)
				{
					this._cachedBytes.Clear();
				}
				this._cachedBytes = null;
				this._currentPosition = 0;
				this._currentArrayIndex = 0;
				this._totalLength = 0L;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x002916A4 File Offset: 0x00290AA4
		public override void Flush()
		{
			throw ADP.NotSupported();
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x002916B8 File Offset: 0x00290AB8
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			if (this._cachedBytes == null)
			{
				throw ADP.StreamClosed("Read");
			}
			if (buffer == null)
			{
				throw ADP.ArgumentNull("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw ADP.ArgumentOutOfRange(string.Empty, (offset < 0) ? "offset" : "count");
			}
			if (buffer.Length - offset < count)
			{
				throw ADP.ArgumentOutOfRange("count");
			}
			if (this._cachedBytes.Count > this._currentArrayIndex)
			{
				byte[] array = (byte[])this._cachedBytes[this._currentArrayIndex];
				while (count > 0)
				{
					if (array.Length <= this._currentPosition)
					{
						this._currentArrayIndex++;
						if (this._cachedBytes.Count <= this._currentArrayIndex)
						{
							break;
						}
						array = (byte[])this._cachedBytes[this._currentArrayIndex];
						this._currentPosition = 0;
					}
					int num2 = array.Length - this._currentPosition;
					if (num2 > count)
					{
						num2 = count;
					}
					Array.Copy(array, this._currentPosition, buffer, offset, num2);
					this._currentPosition += num2;
					count -= num2;
					offset += num2;
					num += num2;
				}
				return num;
			}
			return 0;
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x002917E0 File Offset: 0x00290BE0
		public override long Seek(long offset, SeekOrigin origin)
		{
			long num = 0L;
			if (this._cachedBytes == null)
			{
				throw ADP.StreamClosed("Read");
			}
			switch (origin)
			{
			case SeekOrigin.Begin:
				this.SetInternalPosition(offset, "offset");
				break;
			case SeekOrigin.Current:
				num = offset + this.Position;
				this.SetInternalPosition(num, "offset");
				break;
			case SeekOrigin.End:
				num = this.TotalLength + offset;
				this.SetInternalPosition(num, "offset");
				break;
			default:
				throw ADP.InvalidSeekOrigin("offset");
			}
			return num;
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x00291864 File Offset: 0x00290C64
		public override void SetLength(long value)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x00291878 File Offset: 0x00290C78
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x0029188C File Offset: 0x00290C8C
		private void SetInternalPosition(long lPos, string argumentName)
		{
			long num = lPos;
			if (num < 0L)
			{
				throw new ArgumentOutOfRangeException(argumentName);
			}
			for (int i = 0; i < this._cachedBytes.Count; i++)
			{
				byte[] array = (byte[])this._cachedBytes[i];
				if (num <= (long)array.Length)
				{
					this._currentArrayIndex = i;
					this._currentPosition = (int)num;
					return;
				}
				num -= (long)array.Length;
			}
			if (num > 0L)
			{
				throw new ArgumentOutOfRangeException(argumentName);
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002900 RID: 10496 RVA: 0x00291900 File Offset: 0x00290D00
		private long TotalLength
		{
			get
			{
				if (this._totalLength == 0L && this._cachedBytes != null)
				{
					long num = 0L;
					for (int i = 0; i < this._cachedBytes.Count; i++)
					{
						byte[] array = (byte[])this._cachedBytes[i];
						num += (long)array.Length;
					}
					this._totalLength = num;
				}
				return this._totalLength;
			}
		}

		// Token: 0x04001991 RID: 6545
		private int _currentPosition;

		// Token: 0x04001992 RID: 6546
		private int _currentArrayIndex;

		// Token: 0x04001993 RID: 6547
		private ArrayList _cachedBytes;

		// Token: 0x04001994 RID: 6548
		private long _totalLength;
	}
}
