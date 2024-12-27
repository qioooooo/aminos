using System;
using System.Data.SqlClient;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200003B RID: 59
	internal class SmiGettersStream : Stream
	{
		// Token: 0x06000219 RID: 537 RVA: 0x001CB600 File Offset: 0x001CAA00
		internal SmiGettersStream(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
		{
			this._sink = sink;
			this._getters = getters;
			this._ordinal = ordinal;
			this._readPosition = 0L;
			this._metaData = metaData;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600021A RID: 538 RVA: 0x001CB638 File Offset: 0x001CAA38
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600021B RID: 539 RVA: 0x001CB648 File Offset: 0x001CAA48
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600021C RID: 540 RVA: 0x001CB658 File Offset: 0x001CAA58
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600021D RID: 541 RVA: 0x001CB668 File Offset: 0x001CAA68
		public override long Length
		{
			get
			{
				return ValueUtilsSmi.GetBytesInternal(this._sink, this._getters, this._ordinal, this._metaData, 0L, null, 0, 0, false);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600021E RID: 542 RVA: 0x001CB698 File Offset: 0x001CAA98
		// (set) Token: 0x0600021F RID: 543 RVA: 0x001CB6AC File Offset: 0x001CAAAC
		public override long Position
		{
			get
			{
				return this._readPosition;
			}
			set
			{
				throw SQL.StreamSeekNotSupported();
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x001CB6C0 File Offset: 0x001CAAC0
		public override void Flush()
		{
			throw SQL.StreamWriteNotSupported();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x001CB6D4 File Offset: 0x001CAAD4
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw SQL.StreamSeekNotSupported();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x001CB6E8 File Offset: 0x001CAAE8
		public override void SetLength(long value)
		{
			throw SQL.StreamWriteNotSupported();
		}

		// Token: 0x06000223 RID: 547 RVA: 0x001CB6FC File Offset: 0x001CAAFC
		public override int Read(byte[] buffer, int offset, int count)
		{
			long bytesInternal = ValueUtilsSmi.GetBytesInternal(this._sink, this._getters, this._ordinal, this._metaData, this._readPosition, buffer, offset, count, false);
			this._readPosition += bytesInternal;
			return checked((int)bytesInternal);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x001CB744 File Offset: 0x001CAB44
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw SQL.StreamWriteNotSupported();
		}

		// Token: 0x0400058A RID: 1418
		private SmiEventSink_Default _sink;

		// Token: 0x0400058B RID: 1419
		private ITypedGettersV3 _getters;

		// Token: 0x0400058C RID: 1420
		private int _ordinal;

		// Token: 0x0400058D RID: 1421
		private long _readPosition;

		// Token: 0x0400058E RID: 1422
		private SmiMetaData _metaData;
	}
}
